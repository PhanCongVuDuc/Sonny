using NSubstitute ;
using NSubstitute.ExceptionExtensions ;
using Sonny.Application.Domain.Entities ;
using Sonny.Application.Domain.Entities.ColumnFromCad.Contexts ;
using Sonny.Application.Domain.Entities.ColumnFromCad.Models ;
using Sonny.Application.Domain.Entities.ColumnFromCad.Services ;
using Sonny.Application.Domain.Services ;
using Sonny.Application.UseCases.ColumnFromCad.Implements ;

namespace Sonny.Application.UnitTests.UseCases.ColumnFromCad ;

/// <summary>
///     Characterization tests for <see cref="ColumnFromCadInteractor" /> — they pin the behaviour
///     documented in docs/features/ColumnFromCad.md (## Contract) as it is TODAY, including the
///     silent per-column skip and the order dependency through _extractedColumns. Written as the
///     safety net for the Clean Architecture refactor (docs/adr/0001). Do not "fix" a behaviour
///     here without changing the contract first.
/// </summary>
public class ColumnFromCadInteractorTests
{
    private IColumnDataExtractor _columnDataExtractor = null! ;
    private IResourceHelper _resourceHelper = null! ;
    private ITransactionManagerFactory _transactionManagerFactory = null! ;
    private ITransactionManager _transactionManager = null! ;
    private ITransactionGroupManager _transactionGroupManager = null! ;
    private IColumnCreationStrategyFactory _strategyFactory = null! ;
    private IProgressReporter _progressReporter = null! ;
    private IMessageService _messageService = null! ;
    private IElementSelector _elementSelector = null! ;
    private ColumnFromCadInteractor _interactor = null! ;

    [SetUp]
    public void Setup()
    {
        _columnDataExtractor = Substitute.For<IColumnDataExtractor>() ;
        _transactionManagerFactory = Substitute.For<ITransactionManagerFactory>() ;
        _transactionManager = Substitute.For<ITransactionManager>() ;
        _transactionGroupManager = Substitute.For<ITransactionGroupManager>() ;
        _strategyFactory = Substitute.For<IColumnCreationStrategyFactory>() ;
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

        _transactionManagerFactory.Create(Arg.Any<string>(),
                Arg.Any<IEnumerable<FailurePreprocessorType>?>())
            .Returns(_transactionManager) ;
        _transactionManagerFactory.CreateGroup(Arg.Any<string>())
            .Returns(_transactionGroupManager) ;

        _interactor = new ColumnFromCadInteractor(_columnDataExtractor,
            _resourceHelper,
            _transactionManagerFactory,
            _strategyFactory,
            _progressReporter,
            _messageService,
            _elementSelector,
            new ImmediateRevitTaskRunner()) ;
    }

    [TearDown]
    public void TearDown()
    {
        _transactionManager.Dispose() ;
        _transactionGroupManager.Dispose() ;
    }

    [Test]
    public void CreateColumns_BeforeExtractColumnData_ThrowsInvalidOperationException()
    {
        var context = new ColumnCreationContext() ;

        var exception = Assert.Throws<InvalidOperationException>(() => _interactor.CreateColumns(context)) ;

        Assert.That(exception.Message,
            Is.EqualTo("MessageNoExtractedColumnsFound")) ;
    }

    [Test]
    public async Task Execute_ExtractionReturnsZero_ShowsNoColumnsFoundAndOpensNoTransaction()
    {
        var context = new ColumnCreationContext() ;
        _columnDataExtractor.Extract(context)
            .Returns([]) ;

        await _interactor.Execute(context) ;

        _messageService.Received(1)
            .ShowInfo("MessageNoColumnsFound") ;
        _transactionManagerFactory.DidNotReceive()
            .CreateGroup(Arg.Any<string>()) ;
        _progressReporter.DidNotReceive()
            .Show(Arg.Any<string>()) ;
    }

    [Test]
    public async Task Execute_StrategyNullForSomeColumns_SkipsThemSilentlyAndReportsTheRest()
    {
        // 5 columns extracted, the strategy factory recognises only 3 — the contract's named
        // silent-skip: the 2 drops produce no message, no log, and the run still succeeds
        var context = new ColumnCreationContext() ;
        var models = CreateModels(5) ;
        _columnDataExtractor.Extract(context)
            .Returns(models) ;

        SetUpStrategy(models[0],
            "id-0") ;
        SetUpStrategy(models[2],
            "id-2") ;
        SetUpStrategy(models[4],
            "id-4") ;
        _strategyFactory.CreateStrategy(models[1],
                context)
            .Returns((IColumnCreationStrategy?)null) ;
        _strategyFactory.CreateStrategy(models[3],
                context)
            .Returns((IColumnCreationStrategy?)null) ;

        await _interactor.Execute(context) ;

        _messageService.Received(1)
            .ShowInfo("MessageSuccessfullyCreated") ;
        _messageService.DidNotReceive()
            .ShowWarning(Arg.Any<string>()) ;
        _elementSelector.Received(1)
            .SelectElements(Arg.Is<ICollection<string>>(ids => ids.Count == 3)) ;
    }

    [Test]
    public async Task Execute_EveryColumnThrows_WarnsNoColumnsCreatedAndStillAssimilates()
    {
        var context = new ColumnCreationContext() ;
        var models = CreateModels(3) ;
        _columnDataExtractor.Extract(context)
            .Returns(models) ;

        _strategyFactory.CreateStrategy(Arg.Any<ColumnModel>(),
                context)
            .Throws(new Exception("creation failed")) ;

        await _interactor.Execute(context) ;

        _messageService.Received(1)
            .ShowWarning("MessageNoColumnsCreated") ;
        _transactionGroupManager.Received(1)
            .Assimilate() ;
        _progressReporter.Received(1)
            .Close() ;
        _elementSelector.DidNotReceive()
            .SelectElements(Arg.Any<ICollection<string>>()) ;
    }

    [Test]
    public async Task Execute_CalledTwice_DoesNotLeakExtractedColumnsBetweenRuns()
    {
        var context = new ColumnCreationContext() ;
        _columnDataExtractor.Extract(context)
            .Returns(CreateModels(3),
                CreateModels(2)) ;
        _strategyFactory.CreateStrategy(Arg.Any<ColumnModel>(),
                context)
            .Returns((IColumnCreationStrategy?)null) ;

        await _interactor.Execute(context) ;
        _strategyFactory.ClearReceivedCalls() ;

        await _interactor.Execute(context) ;

        // A leak would replay the 3 columns of the first run on top of the 2 new ones
        _strategyFactory.Received(2)
            .CreateStrategy(Arg.Any<ColumnModel>(),
                context) ;
    }

    private static List<ColumnModel> CreateModels(int count) =>
        Enumerable.Range(0,
                count)
            .Select(i => (ColumnModel)new CircularColumnModel(1.0 + i,
                new Point3D(i,
                    0,
                    0)))
            .ToList() ;

    private void SetUpStrategy(ColumnModel model,
        string createdUniqueId)
    {
        var strategy = Substitute.For<IColumnCreationStrategy>() ;
        strategy.Execute()
            .Returns(createdUniqueId) ;
        _strategyFactory.CreateStrategy(model,
                Arg.Any<ColumnCreationContext>())
            .Returns(strategy) ;
    }

    /// <summary>
    ///     Executes the delegate inline — outside Revit there is no API context to marshal into,
    ///     and the interactor's behaviour under test is what it runs, not where
    /// </summary>
    private sealed class ImmediateRevitTaskRunner : IRevitTaskRunner
    {
        public Task<TResult> RunAsync<TResult>(Func<TResult> function) => Task.FromResult(function()) ;

        public Task RunAsync(Action action)
        {
            action() ;
            return Task.CompletedTask ;
        }
    }
}
