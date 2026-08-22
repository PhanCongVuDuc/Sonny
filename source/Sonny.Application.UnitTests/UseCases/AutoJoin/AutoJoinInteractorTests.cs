using NSubstitute ;
using Serilog ;
using Sonny.Application.Domain.Entities.AutoJoin.Models ;
using Sonny.Application.Domain.Services ;
using Sonny.Application.UseCases.AutoJoin.Implements ;
using Sonny.Application.UseCases.AutoJoin.Models ;
using Sonny.Application.UseCases.AutoJoin.Services ;

namespace Sonny.Application.UnitTests.UseCases.AutoJoin ;

/// <summary>
///     Contract tests for the UseCases <see cref="AutoJoinInteractor" /> — each test pins one row
///     of the Named failure modes table in docs/features/AutoJoin.md (F3–F11) or one decision
///     (D2, D3, D7) so the ported AlphaBIM behaviour cannot drift silently.
/// </summary>
public class AutoJoinInteractorTests
{
    private IAutoJoinScopeReader _scopeReader = null! ;
    private IAutoJoinPairExecutor _pairExecutor = null! ;
    private IFailingElementIdsTracker _failingElementIdsTracker = null! ;
    private ITransactionManagerFactory _transactionManagerFactory = null! ;
    private ITransactionManager _transactionManager = null! ;
    private IProgressReporter _progressReporter = null! ;
    private IMessageService _messageService = null! ;
    private IResourceHelper _resourceHelper = null! ;
    private ILogger _logger = null! ;
    private AutoJoinInteractor _interactor = null! ;

    [SetUp]
    public void Setup()
    {
        _scopeReader = Substitute.For<IAutoJoinScopeReader>() ;
        _pairExecutor = Substitute.For<IAutoJoinPairExecutor>() ;
        _failingElementIdsTracker = Substitute.For<IFailingElementIdsTracker>() ;
        _transactionManagerFactory = Substitute.For<ITransactionManagerFactory>() ;
        _transactionManager = Substitute.For<ITransactionManager>() ;
        _progressReporter = Substitute.For<IProgressReporter>() ;
        _messageService = Substitute.For<IMessageService>() ;
        _logger = Substitute.For<ILogger>() ;

        _resourceHelper = Substitute.For<IResourceHelper>() ;
        _resourceHelper.GetString(Arg.Any<string>())
            .Returns(call => call.Arg<string>()) ;
        _resourceHelper.GetString(Arg.Any<string>(),
                Arg.Any<object[]>())
            .Returns(call => call.ArgAt<string>(0)) ;

        _transactionManagerFactory.Create(Arg.Any<string>(),
                Arg.Any<IEnumerable<FailurePreprocessorType>?>())
            .Returns(_transactionManager) ;

        // Safe defaults: empty world, nothing selected, everything intersects, joins succeed
        _scopeReader.GetSelectedSolidElementIds()
            .Returns([]) ;
        _scopeReader.GetActiveViewSolidElementIds()
            .Returns([]) ;
        _scopeReader.GetPriorityElementIds(Arg.Any<IReadOnlyCollection<long>>(),
                Arg.Any<JoinCategory>())
            .Returns([]) ;
        _scopeReader.GetJoinWithElementIds(Arg.Any<JoinCategory>())
            .Returns([]) ;
        _scopeReader.GetBoundingBoxIntersectingInView(Arg.Any<long>())
            .Returns([]) ;
        _scopeReader.GetBoundingBoxIntersecting(Arg.Any<long>(),
                Arg.Any<IReadOnlyCollection<long>>())
            .Returns([]) ;
        _failingElementIdsTracker.GetFailingElementIds()
            .Returns([]) ;
        _pairExecutor.CheckIntersectWithAnchor(Arg.Any<long>())
            .Returns(SolidIntersectCheck.Intersecting) ;
        _pairExecutor.TryExecuteJoin(Arg.Any<long>(),
                Arg.Any<long>(),
                Arg.Any<bool>(),
                Arg.Any<bool>())
            .Returns(true) ;

        _interactor = new AutoJoinInteractor(_scopeReader,
            _pairExecutor,
            _failingElementIdsTracker,
            _transactionManagerFactory,
            _progressReporter,
            _messageService,
            _resourceHelper,
            _logger) ;
    }

    [TearDown]
    public void TearDown() => _transactionManager.Dispose() ;

    private static AutoJoinInput RuleBasedInput(params AutoJoinRule[] rules) =>
        new()
        {
            Mode = AutoJoinMode.RuleBased, Rules = rules.Length > 0
                ? rules
                : [new AutoJoinRule()]
        } ;

    [Test]
    public void Execute_CutModeWithoutSelection_ShowsDialogThenRollsBack()
    {
        // F3
        _interactor.Execute(new AutoJoinInput { Mode = AutoJoinMode.CutSelectedElements }) ;

        _messageService.Received(1)
            .ShowInfo("MessageNoElementSelected") ;
        _transactionManager.Received(1)
            .RollBack() ;
        _transactionManager.DidNotReceive()
            .Commit() ;
    }

    [Test]
    public void Execute_RuleBasedWithEmptyScope_RollsBackWithoutAnyMessage()
    {
        // F4 — the original says nothing at all here; a dialog would change the contract
        _interactor.Execute(RuleBasedInput()) ;

        _transactionManager.Received(1)
            .RollBack() ;
        _transactionManager.DidNotReceive()
            .Commit() ;
        _messageService.DidNotReceive()
            .ShowInfo(Arg.Any<string>()) ;
        _messageService.DidNotReceive()
            .ShowWarning(Arg.Any<string>()) ;
    }

    [Test]
    public void Execute_RuleWithEmptySide_IsSkippedSilentlyAndRunStillCommits()
    {
        // F5 — the rule contributes nothing, but the run itself completes
        _scopeReader.GetSelectedSolidElementIds()
            .Returns([1L]) ;
        _scopeReader.GetPriorityElementIds(Arg.Any<IReadOnlyCollection<long>>(),
                Arg.Any<JoinCategory>())
            .Returns([]) ;

        _interactor.Execute(RuleBasedInput()) ;

        _pairExecutor.DidNotReceive()
            .TryExecuteJoin(Arg.Any<long>(),
                Arg.Any<long>(),
                Arg.Any<bool>(),
                Arg.Any<bool>()) ;
        _transactionManager.Received(1)
            .Commit() ;
    }

    [Test]
    public void Execute_NotIntersectingPair_IsSkippedWithoutJoinOrLog()
    {
        // F7
        SetupSingleRulePair(10L,
            20L) ;
        _pairExecutor.CheckIntersectWithAnchor(20L)
            .Returns(SolidIntersectCheck.NotIntersecting) ;

        _interactor.Execute(RuleBasedInput()) ;

        _pairExecutor.DidNotReceive()
            .TryExecuteJoin(Arg.Any<long>(),
                Arg.Any<long>(),
                Arg.Any<bool>(),
                Arg.Any<bool>()) ;
        _transactionManager.Received(1)
            .Commit() ;
    }

    [TestCase(SolidIntersectCheck.NoAnchorSolid)]
    [TestCase(SolidIntersectCheck.Intersecting)]
    [TestCase(SolidIntersectCheck.CheckFailed)]
    public void Execute_JoinableCheckOutcomes_AllJoin(SolidIntersectCheck check)
    {
        // F6 (no anchor solid) and F8/D2 (check failed) join exactly like a real intersection
        SetupSingleRulePair(10L,
            20L) ;
        _pairExecutor.CheckIntersectWithAnchor(20L)
            .Returns(check) ;

        _interactor.Execute(RuleBasedInput()) ;

        _pairExecutor.Received(1)
            .TryExecuteJoin(10L,
                20L,
                true,
                false) ;
    }

    [Test]
    public void Execute_JoinFails_LogsWarningAndRunContinuesToCommit()
    {
        // F9 / D3 — silent for the user, but never silent in the log
        SetupSingleRulePair(10L,
            20L) ;
        _pairExecutor.TryExecuteJoin(Arg.Any<long>(),
                Arg.Any<long>(),
                Arg.Any<bool>(),
                Arg.Any<bool>())
            .Returns(false) ;

        _interactor.Execute(RuleBasedInput()) ;

        _logger.Received(1)
            .Warning(Arg.Any<string>(),
                10L,
                20L) ;
        _transactionManager.Received(1)
            .Commit() ;
        _messageService.Received(1)
            .ShowInfo("MessageAutoJoinSuccess") ;
    }

    [Test]
    public void Execute_FailingElementsAfterCommit_SelectsThemAndAsksUserToCheck()
    {
        // F10 — and the success dialog must NOT show alongside it
        SetupSingleRulePair(10L,
            20L) ;
        _failingElementIdsTracker.GetFailingElementIds()
            .Returns([20L]) ;

        _interactor.Execute(RuleBasedInput()) ;

        Received.InOrder(() =>
        {
            _transactionManager.Commit() ;
            _scopeReader.SelectElements(Arg.Is<IReadOnlyCollection<long>>(ids => ids.Contains(20L))) ;
            _messageService.ShowInfo("MessageCheckFailingElements") ;
        }) ;
        _messageService.DidNotReceive()
            .ShowInfo("MessageAutoJoinSuccess") ;
    }

    [Test]
    public void Execute_CancelRequested_RollsBackEverythingAndReportsCancel()
    {
        // F11 / D7 — all-or-nothing
        SetupSingleRulePair(10L,
            20L) ;
        _progressReporter.IsCancelRequested.Returns(true) ;

        _interactor.Execute(RuleBasedInput()) ;

        _transactionManager.Received(1)
            .RollBack() ;
        _transactionManager.DidNotReceive()
            .Commit() ;
        _messageService.Received(1)
            .ShowInfo("MessageAutoJoinCancelled") ;
        _pairExecutor.DidNotReceive()
            .TryExecuteJoin(Arg.Any<long>(),
                Arg.Any<long>(),
                Arg.Any<bool>(),
                Arg.Any<bool>()) ;
    }

    [Test]
    public void Execute_Unjoin_PassesIsJoinFalseAndPicksWarningKeepingPreprocessor()
    {
        SetupSingleRulePair(10L,
            20L) ;
        var input = RuleBasedInput() ;
        input.IsUnjoin = true ;

        _interactor.Execute(input) ;

        _pairExecutor.Received(1)
            .TryExecuteJoin(10L,
                20L,
                false,
                false) ;
        _transactionManagerFactory.Received(1)
            .Create("Auto Join",
                Arg.Is<IEnumerable<FailurePreprocessorType>?>(types =>
                    types!.Contains(FailurePreprocessorType.DeleteWarningsResolveErrors))) ;
    }

    [Test]
    public void Execute_DefaultOptions_PickResolveAllFailuresPreprocessor()
    {
        SetupSingleRulePair(10L,
            20L) ;

        _interactor.Execute(RuleBasedInput()) ;

        _transactionManagerFactory.Received(1)
            .Create("Auto Join",
                Arg.Is<IEnumerable<FailurePreprocessorType>?>(types =>
                    types!.Contains(FailurePreprocessorType.ResolveAllFailures))) ;
        _failingElementIdsTracker.Received(1)
            .Clear() ;
    }

    [Test]
    public void Execute_ReverseRule_PassesIsReverseThrough()
    {
        SetupSingleRulePair(10L,
            20L) ;

        _interactor.Execute(RuleBasedInput(new AutoJoinRule { IsReverse = true })) ;

        _pairExecutor.Received(1)
            .TryExecuteJoin(10L,
                20L,
                true,
                true) ;
    }

    [Test]
    public void Execute_CutSelectedElements_CandidateCutsTheSelectedElement()
    {
        // Orientation is the whole point of this mode: the selection is the TARGET
        _scopeReader.GetSelectedSolidElementIds()
            .Returns([5L]) ;
        _scopeReader.GetBoundingBoxIntersectingInView(5L)
            .Returns([7L]) ;

        _interactor.Execute(new AutoJoinInput { Mode = AutoJoinMode.CutSelectedElements }) ;

        _pairExecutor.Received(1)
            .BeginAnchor(5L) ;
        _pairExecutor.Received(1)
            .TryExecuteJoin(7L,
                5L,
                true,
                false) ;
    }

    [Test]
    public void Execute_CutOtherElements_SelectedElementCutsTheCandidate()
    {
        _scopeReader.GetSelectedSolidElementIds()
            .Returns([5L]) ;
        _scopeReader.GetBoundingBoxIntersectingInView(5L)
            .Returns([7L]) ;

        _interactor.Execute(new AutoJoinInput { Mode = AutoJoinMode.CutOtherElements }) ;

        _pairExecutor.Received(1)
            .BeginAnchor(5L) ;
        _pairExecutor.Received(1)
            .TryExecuteJoin(5L,
                7L,
                true,
                false) ;
    }

    [Test]
    public void Execute_RuleBasedWithoutSelection_FallsBackToActiveViewScope()
    {
        _scopeReader.GetActiveViewSolidElementIds()
            .Returns([1L]) ;
        _scopeReader.GetPriorityElementIds(Arg.Is<IReadOnlyCollection<long>>(ids => ids.Contains(1L)),
                Arg.Any<JoinCategory>())
            .Returns([1L]) ;
        _scopeReader.GetJoinWithElementIds(Arg.Any<JoinCategory>())
            .Returns([2L]) ;
        _scopeReader.GetBoundingBoxIntersecting(1L,
                Arg.Any<IReadOnlyCollection<long>>())
            .Returns([2L]) ;

        _interactor.Execute(RuleBasedInput()) ;

        _pairExecutor.Received(1)
            .TryExecuteJoin(1L,
                2L,
                true,
                false) ;
    }

    [Test]
    public void Execute_EmptyRuleTable_CommitsButShowsNoSuccessDialog()
    {
        // Ported quirk: the success dialog is gated on the rule table being non-empty
        _scopeReader.GetSelectedSolidElementIds()
            .Returns([1L]) ;

        _interactor.Execute(new AutoJoinInput { Mode = AutoJoinMode.RuleBased, Rules = [] }) ;

        _transactionManager.Received(1)
            .Commit() ;
        _messageService.DidNotReceive()
            .ShowInfo(Arg.Any<string>()) ;
    }

    [Test]
    public void Execute_AnchorSnapshotTakenBeforeItsPairsRun()
    {
        SetupSingleRulePair(10L,
            20L) ;

        _interactor.Execute(RuleBasedInput()) ;

        Received.InOrder(() =>
        {
            _pairExecutor.BeginAnchor(10L) ;
            _pairExecutor.CheckIntersectWithAnchor(20L) ;
            _pairExecutor.TryExecuteJoin(10L,
                20L,
                true,
                false) ;
        }) ;
    }

    private void SetupSingleRulePair(long priorityId,
        long targetId)
    {
        _scopeReader.GetSelectedSolidElementIds()
            .Returns([priorityId]) ;
        _scopeReader.GetPriorityElementIds(Arg.Any<IReadOnlyCollection<long>>(),
                Arg.Any<JoinCategory>())
            .Returns([priorityId]) ;
        _scopeReader.GetJoinWithElementIds(Arg.Any<JoinCategory>())
            .Returns([targetId]) ;
        _scopeReader.GetBoundingBoxIntersecting(priorityId,
                Arg.Any<IReadOnlyCollection<long>>())
            .Returns([targetId]) ;
    }
}
