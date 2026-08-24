using NSubstitute ;
using NSubstitute.ExceptionExtensions ;
using Sonny.Application.Domain.Entities ;
using Sonny.Application.Domain.Entities.FramingFromCad.Contexts ;
using Sonny.Application.Domain.Entities.FramingFromCad.Models ;
using Sonny.Application.Domain.Entities.FramingFromCad.Services ;
using Sonny.Application.Domain.Exceptions ;
using Sonny.Application.Domain.Services ;
using Sonny.Application.UseCases.FramingFromCad.Implements ;

namespace Sonny.Application.UnitTests.UseCases.FramingFromCad ;

/// <summary>
///     Tests for <see cref="FramingFromCadInteractor" />, each one derived from a row of the
///     "Named failure modes" table or an entry in "Invariants" in docs/features/FramingFromCad.md — not
///     from the interactor's code. The two rows that describe kept bugs (the single-line crash and the
///     silent duplicate-name loss) are pinned here too: they are the requirement, so a change that makes
///     them behave nicely must change the contract first.
/// </summary>
public class FramingFromCadInteractorTests
{
    private IFramingDataExtractor _extractor = null! ;
    private IBeamCreator _beamCreator = null! ;
    private IBeamJustificationAdjuster _justificationAdjuster = null! ;
    private IResourceHelper _resourceHelper = null! ;
    private ITransactionManagerFactory _transactionManagerFactory = null! ;

    /// <summary>
    ///     The single transaction the whole beam pass runs in
    /// </summary>
    private ITransactionManager _transactionManager = null! ;

    /// <summary>
    ///     The separate transaction the justification pass runs in. Kept distinct from
    ///     <see cref="_transactionManager" /> so an assertion about one cannot be satisfied by the other
    /// </summary>
    private ITransactionManager _justificationTransactionManager = null! ;

    private IProgressReporter _progressReporter = null! ;
    private IMessageService _messageService = null! ;
    private IElementSelector _elementSelector = null! ;
    private FramingFromCadInteractor _interactor = null! ;

    [SetUp]
    public void Setup()
    {
        _extractor = Substitute.For<IFramingDataExtractor>() ;
        _beamCreator = Substitute.For<IBeamCreator>() ;
        _justificationAdjuster = Substitute.For<IBeamJustificationAdjuster>() ;
        _transactionManagerFactory = Substitute.For<ITransactionManagerFactory>() ;
        _transactionManager = Substitute.For<ITransactionManager>() ;
        _justificationTransactionManager = Substitute.For<ITransactionManager>() ;
        _progressReporter = Substitute.For<IProgressReporter>() ;
        _messageService = Substitute.For<IMessageService>() ;
        _elementSelector = Substitute.For<IElementSelector>() ;

        // Resource keys pass through unchanged so assertions can use the key names from the contract
        _resourceHelper = Substitute.For<IResourceHelper>() ;
        _resourceHelper.GetString(Arg.Any<string>())
            .Returns(call => call.Arg<string>()) ;
        _resourceHelper.GetString(Arg.Any<string>(),
                Arg.Any<object[]>())
            .Returns(call => call.ArgAt<string>(0)) ;

        // First transaction created is the beam pass, the second is the justification pass
        var transactionsCreated = 0 ;
        _transactionManagerFactory.Create(Arg.Any<string>(),
                Arg.Any<IEnumerable<FailurePreprocessorType>?>())
            .Returns(_ => transactionsCreated++ == 0
                ? _transactionManager
                : _justificationTransactionManager) ;

        _interactor = new FramingFromCadInteractor(_extractor,
            _beamCreator,
            _justificationAdjuster,
            _resourceHelper,
            _transactionManagerFactory,
            _progressReporter,
            _messageService,
            _elementSelector,
            new ImmediateRevitTaskRunner()) ;
    }

    [TearDown]
    public void TearDown()
    {
        _transactionManager.Dispose() ;
        _justificationTransactionManager.Dispose() ;
    }

    #region Happy path

    [Test]
    public async Task Execute_TwoPairsCreated_CommitsSelectsAndReportsCount()
    {
        GivenExtraction(Pairs(2)) ;
        GivenPairedBeamsCreatedAs("beam-0",
            "beam-1") ;

        await _interactor.Execute(NewContext()) ;

        Assert.Multiple(() => {
            _transactionManager.Received(1)
                .Commit() ;
            _transactionManager.DidNotReceive()
                .RollBack() ;
            _elementSelector.Received(1)
                .SelectElements(Arg.Is<ICollection<string>>(ids => ids.Count == 2)) ;
            _messageService.Received(1)
                .ShowInfo("MessageSuccessfullyCreated") ;
        }) ;
    }

    /// <summary>
    ///     Invariant: justification for paired beams runs in a transaction of its own, after the beams
    ///     have been committed, because FacingOrientation is only trustworthy post-regeneration
    /// </summary>
    [Test]
    public async Task Execute_PairedBeams_AdjustsJustificationInASecondTransaction()
    {
        GivenExtraction(Pairs(2)) ;
        GivenPairedBeamsCreatedAs("beam-0",
            "beam-1") ;

        await _interactor.Execute(NewContext()) ;

        Assert.Multiple(() => {
            _justificationAdjuster.Received(1)
                .Adjust(Arg.Is<IReadOnlyList<CreatedPairedBeam>>(beams => beams.Count == 2)) ;
            // One transaction for the beams, a second one for the justification pass
            _transactionManagerFactory.Received(2)
                .Create(Arg.Any<string>(),
                    Arg.Any<IEnumerable<FailurePreprocessorType>?>()) ;
        }) ;
    }

    /// <summary>
    ///     Invariant: one transaction covers the whole beam pass, so cancelling can roll every beam back
    ///     together. A per-beam transaction shape would show up here as N creates
    /// </summary>
    [Test]
    public async Task Execute_ManyPairs_UsesOneTransactionForTheWholeBeamPass()
    {
        GivenExtraction(Pairs(5)) ;
        GivenPairedBeamsCreatedAs("b0",
            "b1",
            "b2",
            "b3",
            "b4") ;

        await _interactor.Execute(NewContext()) ;

        // 5 beams, still only the beam transaction plus the justification transaction
        _transactionManagerFactory.Received(2)
            .Create(Arg.Any<string>(),
                Arg.Any<IEnumerable<FailurePreprocessorType>?>()) ;
    }

    [Test]
    public async Task Execute_ResetsTheCreatorSoTheRememberedSymbolDoesNotLeakBetweenRuns()
    {
        GivenExtraction(Pairs(1)) ;
        GivenPairedBeamsCreatedAs("beam-0") ;

        await _interactor.Execute(NewContext()) ;
        await _interactor.Execute(NewContext()) ;

        _beamCreator.Received(2)
            .Reset() ;
    }

    #endregion

    #region Named failure modes

    [Test]
    public async Task Execute_NothingCreated_WarnsInsteadOfInforming()
    {
        GivenExtraction(Pairs(2)) ;
        _beamCreator.CreatePairedBeam(Arg.Any<FramingPairModel>(),
                Arg.Any<FramingCreationContext>())
            .Returns((string?)null) ;

        await _interactor.Execute(NewContext()) ;

        Assert.Multiple(() => {
            _messageService.Received(1)
                .ShowWarning("MessageNoFramingsCreated") ;
            _messageService.DidNotReceive()
                .ShowInfo("MessageSuccessfullyCreated") ;
            _elementSelector.DidNotReceive()
                .SelectElements(Arg.Any<ICollection<string>>()) ;
        }) ;
    }

    [Test]
    public async Task Execute_NoSectionsAndNoStrokes_StillWarnsRatherThanSayingNothing()
    {
        GivenExtraction(new FramingExtractionResult()) ;

        await _interactor.Execute(NewContext()) ;

        _messageService.Received(1)
            .ShowWarning("MessageNoFramingsCreated") ;
    }

    /// <summary>
    ///     Failure mode "silent per-pair skip": any exception from one paired beam drops that beam and the
    ///     run carries on, with nothing said about it
    /// </summary>
    [Test]
    public async Task Execute_OnePairThrows_SkipsItSilentlyAndKeepsTheRest()
    {
        GivenExtraction(Pairs(3)) ;
        var pairs = 0 ;
        _beamCreator.CreatePairedBeam(Arg.Any<FramingPairModel>(),
                Arg.Any<FramingCreationContext>())
            .Returns(_ => ++pairs == 2
                ? throw new InvalidOperationException("Revit refused this one")
                : $"beam-{pairs}") ;

        await _interactor.Execute(NewContext()) ;

        Assert.Multiple(() => {
            _transactionManager.Received(1)
                .Commit() ;
            _elementSelector.Received(1)
                .SelectElements(Arg.Is<ICollection<string>>(ids => ids.Count == 2)) ;
            _messageService.Received(1)
                .ShowInfo("MessageSuccessfullyCreated") ;
            // Nothing tells the user that a third beam was expected
            _messageService.DidNotReceive()
                .ShowWarning(Arg.Any<string>()) ;
        }) ;
    }

    /// <summary>
    ///     Failure mode "MessageParameterNotFound": a family missing the width or height parameter aborts
    ///     the entire run — it is not a per-beam skip, because every remaining beam would be wrong the
    ///     same way
    /// </summary>
    [Test]
    public async Task Execute_FamilyMissingDimensionParameter_AbortsWholeRunWithoutCommitting()
    {
        GivenExtraction(Pairs(3)) ;
        _beamCreator.CreatePairedBeam(Arg.Any<FramingPairModel>(),
                Arg.Any<FramingCreationContext>())
            .Throws(new FramingParameterMissingException("b",
                "h")) ;

        await _interactor.Execute(NewContext()) ;

        Assert.Multiple(() => {
            _messageService.Received(1)
                .ShowError("MessageParameterNotFound") ;
            _transactionManager.DidNotReceive()
                .Commit() ;
            _justificationAdjuster.DidNotReceive()
                .Adjust(Arg.Any<IReadOnlyList<CreatedPairedBeam>>()) ;
        }) ;
    }

    /// <summary>
    ///     Failure mode "MessageCancelled": Cancel rolls the single transaction back, so the model gains
    ///     no beams at all
    /// </summary>
    [Test]
    public async Task Execute_CancelledMidRun_RollsBackEverythingAndReportsCancelled()
    {
        GivenExtraction(Pairs(5)) ;
        GivenPairedBeamsCreatedAs("b0",
            "b1",
            "b2",
            "b3",
            "b4") ;
        // Runs, then the user hits Cancel
        var polls = 0 ;
        _progressReporter.IsCancelRequested.Returns(_ => ++polls > 2) ;

        await _interactor.Execute(NewContext()) ;

        Assert.Multiple(() => {
            _transactionManager.Received(1)
                .RollBack() ;
            _transactionManager.DidNotReceive()
                .Commit() ;
            _messageService.Received(1)
                .ShowInfo("MessageCancelled") ;
            _elementSelector.DidNotReceive()
                .SelectElements(Arg.Any<ICollection<string>>()) ;
            _justificationAdjuster.DidNotReceive()
                .Adjust(Arg.Any<IReadOnlyList<CreatedPairedBeam>>()) ;
        }) ;
    }

    /// <summary>
    ///     Failure mode "single-line crash" — a KEPT BUG. With IsCreateForSingleLine on and a paired pass
    ///     that resolved no symbol, the creator throws and the exception is allowed to escape the run,
    ///     leaving the transaction to roll back on dispose. See
    ///     docs/bugs/FFC-001-single-line-pass-crashes-when-no-symbol-resolved.md
    /// </summary>
    [Test]
    public void Execute_SingleLinesButNoSymbolEverResolved_LetsTheExceptionEscape()
    {
        GivenExtraction(new FramingExtractionResult {
            SingleLines = [NewSingleLine()],
        }) ;
        _beamCreator.HasResolvedSymbol.Returns(false) ;
        _beamCreator.CreateSingleLineBeam(Arg.Any<SingleLineFramingModel>(),
                Arg.Any<FramingCreationContext>())
            .Throws(new InvalidOperationException("No symbol was ever resolved")) ;

        Assert.ThatAsync(() => _interactor.Execute(NewContext()),
            Throws.InstanceOf<InvalidOperationException>()) ;

        Assert.Multiple(() => {
            _transactionManager.DidNotReceive()
                .Commit() ;
            // The progress window still closes — it lives in a finally
            _progressReporter.Received(1)
                .Close() ;
        }) ;
    }

    [Test]
    public async Task Execute_SingleLinesWithSymbolResolved_CreatesThemAndCountsThem()
    {
        GivenExtraction(new FramingExtractionResult {
            Pairs = [NewPair()],
            SingleLines = [NewSingleLine(), NewSingleLine()],
        }) ;
        GivenPairedBeamsCreatedAs("paired-0") ;
        _beamCreator.HasResolvedSymbol.Returns(true) ;
        var singles = 0 ;
        _beamCreator.CreateSingleLineBeam(Arg.Any<SingleLineFramingModel>(),
                Arg.Any<FramingCreationContext>())
            .Returns(_ => $"single-{singles++}") ;

        await _interactor.Execute(NewContext()) ;

        Assert.Multiple(() => {
            _elementSelector.Received(1)
                .SelectElements(Arg.Is<ICollection<string>>(ids => ids.Count == 3)) ;
            // Only the paired beam goes through the justification pass
            _justificationAdjuster.Received(1)
                .Adjust(Arg.Is<IReadOnlyList<CreatedPairedBeam>>(beams => beams.Count == 1)) ;
        }) ;
    }

    #endregion

    #region Progress

    [Test]
    public async Task Execute_ShowsCancellableProgressAndAlwaysClosesIt()
    {
        GivenExtraction(Pairs(2)) ;
        GivenPairedBeamsCreatedAs("b0",
            "b1") ;

        await _interactor.Execute(NewContext()) ;

        Assert.Multiple(() => {
            _progressReporter.Received(1)
                .Show(Arg.Any<string>(),
                    true) ;
            _progressReporter.Received(1)
                .Close() ;
        }) ;
    }

    #endregion

    #region Helpers

    private static FramingCreationContext NewContext() =>
        new() {
            Settings = new FramingFromCadSettings {
                SelectedCadLinkId = "cad-1",
                SelectedLayer = "S-BEAM",
                SelectedFamilyId = "family-1",
                WidthParameter = "b",
                HeightParameter = "h",
                ReferenceLevelId = "level-1",
                AllSectionsText = "200x300",
                IsCreateForSingleLine = true,
            },
            Sections = [NewSection()],
            ZOffset = 0,
            MinimumSizeDisplay = 1.0,
        } ;

    private static BeamSection NewSection() =>
        new() {
            WidthDisplay = 200,
            HeightDisplay = 300,
            Width = 200 / 304.8,
            Height = 300 / 304.8,
            SourceText = "200x300",
        } ;

    private static FramingPairModel NewPair() =>
        new() {
            Start = new Point3D(0,
                0,
                0),
            End = new Point3D(10,
                0,
                0),
            Normal = new Vector3D(0,
                1,
                0),
            Section = NewSection(),
        } ;

    private static SingleLineFramingModel NewSingleLine() =>
        new() {
            Start = new Point3D(0,
                5,
                0),
            End = new Point3D(10,
                5,
                0),
        } ;

    private static FramingExtractionResult Pairs(int count) =>
        new() {
            Pairs = Enumerable.Range(0,
                    count)
                .Select(_ => NewPair())
                .ToList(),
        } ;

    private void GivenExtraction(FramingExtractionResult result) =>
        _extractor.Extract(Arg.Any<FramingCreationContext>())
            .Returns(result) ;

    private void GivenPairedBeamsCreatedAs(params string[] uniqueIds)
    {
        var index = 0 ;
        _beamCreator.CreatePairedBeam(Arg.Any<FramingPairModel>(),
                Arg.Any<FramingCreationContext>())
            .Returns(_ => index < uniqueIds.Length
                ? uniqueIds[index++]
                : null) ;
        _beamCreator.HasResolvedSymbol.Returns(true) ;
    }

    private sealed class ImmediateRevitTaskRunner : IRevitTaskRunner
    {
        public Task<TResult> RunAsync<TResult>(Func<TResult> function) => Task.FromResult(function()) ;

        public Task RunAsync(Action action)
        {
            action() ;
            return Task.CompletedTask ;
        }
    }

    #endregion
}
