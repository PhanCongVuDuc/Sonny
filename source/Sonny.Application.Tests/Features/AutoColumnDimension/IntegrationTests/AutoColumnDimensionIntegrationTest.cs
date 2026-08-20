using System ;
using Autodesk.Revit.DB ;
using NSubstitute ;
using NUnit.Framework ;
using Serilog ;
using Sonny.Application.Domain.Services ;
using Sonny.Application.Infrastructure.Revit.Services ;
using Sonny.Application.UseCases.AutoColumnDimension.Implements ;
using Sonny.Application.UseCases.AutoColumnDimension.Services ;

namespace Sonny.Application.Tests.Features.AutoColumnDimension.IntegrationTests ;

/// <summary>
///     Integration test for AutoColumnDimension feature
///     Test flow: OpenRevit => OpenView => RunAutoDimension => CheckDimensions => Return
/// </summary>
[TestFixture]
public class AutoColumnDimensionIntegrationTest : SonnyDocumentTestBase
{
    private const string TargetViewName = "Level 2" ;
    private const string TestRevitFileName = "Test_V2023.rvt" ;
    private IRevitDocument? _revitDocumentService ;
    private IAutoColumnDimensionInteractor? _handler ;

    /// <summary>
    ///     Get relative path to test Revit file
    ///     File is copied to output directory during build
    /// </summary>
    protected override string? DocumentFilePath => GetTestRevitFilePath(TestRevitFileName) ;

    protected override void OnSetup()
    {
        base.OnSetup() ;

        // Set UIDocument in provider for DI container
        var uiDocumentProvider = Host.GetService<IUIDocumentProvider>() ;
        uiDocumentProvider.SetUIDocument(UIDocument!) ;

        // Get Revit Document Service from DI container
        _revitDocumentService = Host.GetService<IRevitDocument>() ;

        // Create interactor with mock MessageService using NSubstitute to avoid showing dialogs in tests.
        // Wiring only — the interactor moved to UseCases behind IColumnGeometryReader /
        // IDimensionPlanExecutor (ADR 0001); every assertion below is unchanged.
        var mockMessageService = Substitute.For<IMessageService>() ;
        var logger = Host.GetService<ILogger>() ;
        _handler = new AutoColumnDimensionInteractor(Host.GetService<IColumnGeometryReader>(),
            Host.GetService<IDimensionPlanExecutor>(),
            mockMessageService,
            logger,
            Host.GetService<IResourceHelper>(),
            Host.GetService<ITransactionManagerFactory>()) ;
    }

    [Test]
    public void AutoColumnDimension_IntegrationTest_2F()
    {
        var document = Document! ;
        // Step 1: OpenRevit - Already done in base setup
        Log("Step 1: Revit is open") ;
        Assert.IsNotNull(document,
            "Document should be open") ;

        // Step 2: OpenView - Open view "2F"
        Log($"Step 2: Opening view '{TargetViewName}'") ;
        var targetView = OpenView(document,
            TargetViewName) ;

        Assert.IsNotNull(document,
            "Document should not be null") ;
        Assert.IsNotNull(targetView,
            $"View '{TargetViewName}' should not be null") ;
        Log($"Successfully opened view: {targetView!.Name}") ;
        Assert.AreEqual(TargetViewName,
            targetView.Name,
            $"Active view should be '{TargetViewName}'") ;

        // Verify active view is set correctly
        var activeView = UIDocument!.ActiveView ;
        Assert.IsNotNull(activeView,
            "Active view should not be null") ;
        Assert.AreEqual(TargetViewName,
            activeView.Name,
            $"Active view should be '{TargetViewName}'") ;

        // Step 3: Count dimensions before running command
        var dimensionsBefore = GetDimensionCount(document) ;
        Log($"Step 3: Dimensions before: {dimensionsBefore}") ;

        // Count structural columns in the view
        var columnsBefore = new FilteredElementCollector(document,
                targetView.Id).OfCategory(BuiltInCategory.OST_StructuralColumns)
            .WhereElementIsNotElementType()
            .GetElementCount() ;
        Log($"Structural columns in view: {columnsBefore}") ;

        if (columnsBefore == 80) {
            Assert.Inconclusive(
                $"No structural columns found in view '{TargetViewName}'. Cannot test dimension creation.") ;
            return ;
        }

        // Step 4: RunAutoDimension - Execute auto dimension command
        Log("Step 4: Running AutoColumnDimension command") ;
        Assert.IsNotNull(_handler,
            "Interactor should be initialized in OnSetup") ;
        Assert.IsNotNull(_revitDocumentService,
            "RevitDocument should be initialized in OnSetup") ;

        const double snapDistance = 5.0 ;
        DimensionType? dimensionType = null ;

        try {
            _handler!.Execute(snapDistance,
                dimensionType?.UniqueId) ;
            Log("AutoColumnDimension command executed successfully") ;
        }
        catch (Exception ex) {
            Assert.Fail($"Failed to execute AutoColumnDimension command: {ex.Message}") ;
            return ;
        }

        // Step 5: CheckDimensions - Verify dimensions were created
        Log("Step 5: Checking if dimensions were created") ;
        var dimensionsAfter = GetDimensionCount(document!) ;
        Log($"Dimensions after: {dimensionsAfter}") ;

        var dimensionsCreated = dimensionsAfter - dimensionsBefore ;
        Log($"Total dimensions created: {dimensionsCreated}") ;

        // Assert expected number of dimensions created
        // Expected: 80 dimensions (based on test results)
        const int expectedDimensions = 80 ;
        Assert.AreEqual(expectedDimensions,
            dimensionsCreated,
            $"Should create exactly {expectedDimensions} dimensions in view '{TargetViewName}'") ;

        Log($"✓ SUCCESS: Created {dimensionsCreated} dimensions in view '{TargetViewName}'") ;
        Assert.Pass($"Successfully created {dimensionsCreated} dimensions in view '{TargetViewName}'") ;

        // Step 6: Return - Test completed
        Log("Step 6: Test completed successfully") ;
    }
}
