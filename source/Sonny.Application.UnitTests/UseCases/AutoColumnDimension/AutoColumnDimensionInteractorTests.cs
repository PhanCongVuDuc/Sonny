using NSubstitute ;
using NSubstitute.ExceptionExtensions ;
using Serilog ;
using Sonny.Application.Domain.Entities ;
using Sonny.Application.Domain.Services ;
using Sonny.Application.UseCases.AutoColumnDimension.Implements ;
using Sonny.Application.UseCases.AutoColumnDimension.Models ;
using Sonny.Application.UseCases.AutoColumnDimension.Services ;

namespace Sonny.Application.UnitTests.UseCases.AutoColumnDimension ;

/// <summary>
///     Characterization tests for the UseCases <see cref="AutoColumnDimensionInteractor" /> — pin
///     the behaviour documented in docs/features/AutoColumnDimension.md: MessageNoColumnsFound
///     before any transaction, one transaction around the whole loop, the load-bearing per-column
///     catch, and the "failed = arithmetic" result report (ADR 0001).
/// </summary>
public class AutoColumnDimensionInteractorTests
{
    private IColumnGeometryReader _columnGeometryReader = null! ;
    private IDimensionPlanExecutor _dimensionPlanExecutor = null! ;
    private IMessageService _messageService = null! ;
    private ILogger _logger = null! ;
    private IResourceHelper _resourceHelper = null! ;
    private ITransactionManagerFactory _transactionManagerFactory = null! ;
    private ITransactionManager _transactionManager = null! ;
    private AutoColumnDimensionInteractor _interactor = null! ;

    [SetUp]
    public void Setup()
    {
        _columnGeometryReader = Substitute.For<IColumnGeometryReader>() ;
        _dimensionPlanExecutor = Substitute.For<IDimensionPlanExecutor>() ;
        _messageService = Substitute.For<IMessageService>() ;
        _logger = Substitute.For<ILogger>() ;
        _transactionManagerFactory = Substitute.For<ITransactionManagerFactory>() ;
        _transactionManager = Substitute.For<ITransactionManager>() ;

        _resourceHelper = Substitute.For<IResourceHelper>() ;
        _resourceHelper.GetString(Arg.Any<string>())
            .Returns(call => call.Arg<string>()) ;
        _resourceHelper.GetString(Arg.Any<string>(),
                Arg.Any<object[]>())
            .Returns(call => call.ArgAt<string>(0)) ;

        _transactionManagerFactory.Create(Arg.Any<string>(),
                Arg.Any<IEnumerable<FailurePreprocessorType>?>())
            .Returns(_transactionManager) ;

        _interactor = new AutoColumnDimensionInteractor(_columnGeometryReader,
            _dimensionPlanExecutor,
            _messageService,
            _logger,
            _resourceHelper,
            _transactionManagerFactory) ;
    }

    [TearDown]
    public void TearDown() => _transactionManager.Dispose() ;

    [Test]
    public void Execute_NoValidColumns_ShowsMessageAndOpensNoTransaction()
    {
        _columnGeometryReader.ReadActiveView()
            .Returns(Geometry()) ;

        _interactor.Execute(5.0) ;

        _messageService.Received(1)
            .ShowInfo("MessageNoColumnsFound") ;
        _transactionManagerFactory.DidNotReceive()
            .Create(Arg.Any<string>(),
                Arg.Any<IEnumerable<FailurePreprocessorType>?>()) ;
    }

    [Test]
    public void Execute_TwoDimensionsPerColumn_ReportsZeroFailures()
    {
        _columnGeometryReader.ReadActiveView()
            .Returns(Geometry(Column("c1"),
                Column("c2"),
                Column("c3"))) ;
        _dimensionPlanExecutor.Execute(Arg.Any<string>(),
                Arg.Any<ColumnDimensionPlan>(),
                Arg.Any<double>(),
                Arg.Any<string?>())
            .Returns(2) ;

        _interactor.Execute(5.0) ;

        _resourceHelper.Received(1)
            .GetString("ExecutionResultSuccess",
                Arg.Is<object[]>(args => (int)args[0] == 6)) ;
        _resourceHelper.Received(1)
            .GetString("ExecutionResultFailed",
                Arg.Is<object[]>(args => (int)args[0] == 0)) ;
        _transactionManager.Received(1)
            .Commit() ;
    }

    [Test]
    public void Execute_FewerDimensionsThanExpected_ReportsFailuresByArithmeticNotByError()
    {
        // A column producing one dimension is counted as one "failure" purely by subtraction —
        // pinned so nobody mistakes the report for a real error count
        _columnGeometryReader.ReadActiveView()
            .Returns(Geometry(Column("c1"),
                Column("c2"),
                Column("c3"))) ;
        _dimensionPlanExecutor.Execute(Arg.Any<string>(),
                Arg.Any<ColumnDimensionPlan>(),
                Arg.Any<double>(),
                Arg.Any<string?>())
            .Returns(2,
                1,
                2) ;

        _interactor.Execute(5.0) ;

        _resourceHelper.Received(1)
            .GetString("ExecutionResultFailed",
                Arg.Is<object[]>(args => (int)args[0] == 1)) ;
    }

    [Test]
    public void Execute_OneColumnThrows_RunContinuesAndCommits()
    {
        _columnGeometryReader.ReadActiveView()
            .Returns(Geometry(Column("c1"),
                Column("c2"),
                Column("c3"))) ;
        _dimensionPlanExecutor.Execute("c2",
                Arg.Any<ColumnDimensionPlan>(),
                Arg.Any<double>(),
                Arg.Any<string?>())
            .Throws(new Exception("one bad column")) ;
        _dimensionPlanExecutor.Execute(Arg.Is<string>(id => id != "c2"),
                Arg.Any<ColumnDimensionPlan>(),
                Arg.Any<double>(),
                Arg.Any<string?>())
            .Returns(2) ;

        _interactor.Execute(5.0) ;

        _dimensionPlanExecutor.Received(3)
            .Execute(Arg.Any<string>(),
                Arg.Any<ColumnDimensionPlan>(),
                Arg.Any<double>(),
                Arg.Any<string?>()) ;
        _transactionManager.Received(1)
            .Commit() ;
        _resourceHelper.Received(1)
            .GetString("ExecutionResultSuccess",
                Arg.Is<object[]>(args => (int)args[0] == 4)) ;
    }

    [Test]
    public void Execute_ColumnWithoutBoundingBox_IsSkippedSilently()
    {
        var columnWithoutBox = new ColumnGeometryData("no-box",
            null,
            null,
            new Point3D(1,
                0,
                0),
            new Point3D(0,
                1,
                0)) ;
        _columnGeometryReader.ReadActiveView()
            .Returns(Geometry(Column("c1"),
                columnWithoutBox)) ;
        _dimensionPlanExecutor.Execute(Arg.Any<string>(),
                Arg.Any<ColumnDimensionPlan>(),
                Arg.Any<double>(),
                Arg.Any<string?>())
            .Returns(2) ;

        _interactor.Execute(5.0) ;

        _dimensionPlanExecutor.Received(1)
            .Execute("c1",
                Arg.Any<ColumnDimensionPlan>(),
                Arg.Any<double>(),
                Arg.Any<string?>()) ;
        _dimensionPlanExecutor.DidNotReceive()
            .Execute("no-box",
                Arg.Any<ColumnDimensionPlan>(),
                Arg.Any<double>(),
                Arg.Any<string?>()) ;

        // The skipped column still counts toward "expected", so it shows up as 2 failures
        _resourceHelper.Received(1)
            .GetString("ExecutionResultFailed",
                Arg.Is<object[]>(args => (int)args[0] == 2)) ;
    }

    private static ActiveViewGeometry Geometry(params ColumnGeometryData[] columns) =>
        new(new ViewGeometryData("Level 1",
                true,
                new Point3D(0,
                    1,
                    0),
                new Point3D(1,
                    0,
                    0),
                new Point3D(0,
                    0,
                    -1)),
            columns.ToList(),
            []) ;

    private static ColumnGeometryData Column(string uniqueId) =>
        new(uniqueId,
            new Point3D(-1,
                -1,
                0),
            new Point3D(1,
                1,
                0),
            new Point3D(1,
                0,
                0),
            new Point3D(0,
                1,
                0)) ;
}
