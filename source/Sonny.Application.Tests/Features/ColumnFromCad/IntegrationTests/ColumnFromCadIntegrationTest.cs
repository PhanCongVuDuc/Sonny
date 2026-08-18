using System.Linq ;
using Autodesk.Revit.DB ;
using NSubstitute ;
using NUnit.Framework ;
using Serilog ;
using Sonny.Application.Domain.Entities.ColumnFromCad.Contexts ;
using Sonny.Application.Domain.Entities.ColumnFromCad.Models ;
using Sonny.Application.Domain.Services ;
using Sonny.Application.Infrastructure.Revit.Services ;
using Sonny.Application.Presentation.Implements ;
using Sonny.Application.UseCases.ColumnFromCad.Services ;
using Sonny.RevitExtensions.Extensions ;

namespace Sonny.Application.Tests.Features.ColumnFromCad.IntegrationTests ;

/// <summary>
///     Integration test for ColumnFromCad feature
///     Test flow: OpenRevit => OpenDialog => MockValues => ExecuteCommand => VerifyColumns
/// </summary>
[TestFixture]
public class ColumnFromCadIntegrationTest : SonnyDocumentTestBase
{
    private const string TestRevitFileName = "Test_V2023_ModelColumnFromAutoCad.rvt" ;

    /// <summary>
    ///     Get relative path to test Revit file
    ///     File is copied to output directory during build
    /// </summary>
    protected override string? DocumentFilePath => GetTestRevitFilePath(TestRevitFileName) ;

    protected override void OnSetup()
    {
        base.OnSetup() ;

        // Initialize Host if not already initialized
        Host.Start() ;

        // Set UIDocument in provider for DI container
        var uiDocumentProvider = Host.GetService<IUIDocumentProvider>() ;
        uiDocumentProvider.SetUIDocument(UIDocument!) ;
    }

    [Test]
    public void ColumnFromCad_CreateColumns_Test()
    {
        var document = Document! ;

        // Step 1: Verify document is open
        Log("Step 1: Revit document is open") ;
        Assert.IsNotNull(document,
            "Document should be open") ;
        Log($"Document opened: {document.Title}") ;

        // Step 2: Find CAD link by UniqueId
        Log("Step 2: Looking for CAD link") ;
        var importInstance = document.GetAllElements<ImportInstance>()
            .FirstOrDefault(i => i.UniqueId == "71872eb5-c0e3-4496-bc50-6c92ffd18d4b-00067349") ;

        Assert.IsNotNull(importInstance,
            "CAD link with specified UniqueId should be found") ;
        Log($"✓ Found CAD link: Id={importInstance!.Id}, UniqueId={importInstance.UniqueId}") ;

        var columnFromCadOrchestrator = Host.GetService<IColumnFromCadInteractor>() ;

        // Test data from JSON
        const string testSelectedLayer = "S-COLS" ;
        const bool testIsModelByHatch = false ;
        const string testRectangularColumnFamilyId = "cba1aa2d-dafb-4e78-bf41-6c833d14a64d-00020e97" ;
        const string testCircularColumnFamilyId = "cba1aa2d-dafb-4e78-bf41-6c833d14a64d-00020e97" ;
        const string testWidthParameter = "b" ;
        const string testHeightParameter = "h" ;
        const string testDiameterParameter = "b" ;
        const string testBaseLevelId = "74e46f31-7d48-47ee-9207-931708e57a52-00000153" ;
        const string testTopLevelId = "74e46f31-7d48-47ee-9207-931708e57a52-00000ae3" ;
        const double testBaseOffsetDisplay = 100.0 ;
        const double testTopOffsetDisplay = 100.0 ;

        // Step 3: Extract column data
        Log("Step 3: Extracting column data") ;
        var extractContext = new ColumnCreationContext
        {
            Settings = new ColumnFromCadSettings
            {
                SelectedCadLinkId = importInstance.UniqueId,
                SelectedLayer = testSelectedLayer,
                IsModelByHatch = testIsModelByHatch
            }
        } ;
        var extractColumnData = columnFromCadOrchestrator.ExtractColumnData(extractContext) ;

        Assert.Greater(extractColumnData.Count,
            0,
            "Should extract at least one column from CAD link") ;
        Log($"Extracted {extractColumnData.Count} columns") ;

        // Step 4: Verify extracted column data
        Log("Step 4: Verifying extracted column data") ;
        var rectangularColumns = extractColumnData.OfType<RectangularColumnModel>()
            .ToList() ;
        var circularColumns = extractColumnData.OfType<CircularColumnModel>()
            .ToList() ;

        const int expectedRectangularCount = 37 ;
        const int expectedCircularCount = 8 ;

        Assert.AreEqual(expectedRectangularCount,
            rectangularColumns.Count,
            $"Should extract {expectedRectangularCount} rectangular columns") ;
        Assert.AreEqual(expectedCircularCount,
            circularColumns.Count,
            $"Should extract {expectedCircularCount} circular columns") ;

        // Step 5: Verify distinct rotation angles for rectangular columns
        Log("Step 5: Verifying rotation angles") ;
        var expectedRotationAngles = new[]
            {
                0.0, 0.95993108859687171, 1.5707963267948966, 2.2514747350726836, 2.2514747350726911,
                3.1415926535897931
            }.OrderBy(x => x)
            .ToList() ;

        // Get distinct angles
        var distinctRotationAngles = rectangularColumns.Select(x => x.RotationAngle)
            .ToHashSet() ;

        // Verify all expected angles are present
        var missingAngles = expectedRotationAngles.Where(x => ! distinctRotationAngles.Contains(x))
            .ToList() ;
        Assert.IsEmpty(missingAngles,
            $"All expected rotation angles should be present. Missing: {string.Join(", ", missingAngles)}. Expected: {string.Join(", ", expectedRotationAngles)}, Found: {string.Join(", ", distinctRotationAngles.OrderBy(x => x))}") ;

        Log($"✓ Verified: {rectangularColumns.Count} rectangular columns, {circularColumns.Count} circular columns") ;
        Log($"✓ Verified rotation angles: {string.Join(", ", distinctRotationAngles.OrderBy(x => x))}") ;

        // Step 6: Count columns before creation
        Log("Step 6: Counting existing columns") ;
        var columnsBefore = new FilteredElementCollector(document).OfCategory(BuiltInCategory.OST_StructuralColumns)
            .WhereElementIsNotElementType()
            .GetElementCount() ;
        Log($"Columns before: {columnsBefore}") ;

        // Step 7: Find rectangular column family by UniqueId
        Log("Step 7: Finding column families and levels") ;
        var rectangularFamily = document.GetAllElements<Family>()
            .FirstOrDefault(f => f.UniqueId == testRectangularColumnFamilyId) ;

        Assert.IsNotNull(rectangularFamily,
            $"Rectangular column family with UniqueId '{testRectangularColumnFamilyId}' should be found") ;

        // Find circular column family by UniqueId
        var circularFamily = document.GetAllElements<Family>()
            .FirstOrDefault(f => f.UniqueId == testCircularColumnFamilyId) ;

        Assert.IsNotNull(circularFamily,
            $"Circular column family with UniqueId '{testCircularColumnFamilyId}' should be found") ;

        // Find base level by UniqueId
        var baseLevel = document.GetAllElements<Level>()
            .FirstOrDefault(l => l.UniqueId == testBaseLevelId) ;

        Assert.IsNotNull(baseLevel,
            $"Base level with UniqueId '{testBaseLevelId}' should be found") ;

        // Find top level by UniqueId
        var topLevel = document.GetAllElements<Level>()
            .FirstOrDefault(l => l.UniqueId == testTopLevelId) ;

        Assert.IsNotNull(topLevel,
            $"Top level with UniqueId '{testTopLevelId}' should be found") ;

        Log($"Using families: Rectangular={rectangularFamily!.Name}, Circular={circularFamily!.Name}") ;
        Log($"Using levels: Base={baseLevel!.Name}, Top={topLevel!.Name}") ;

        // Step 8: Convert display offsets to internal units (feet)
        Log("Step 8: Converting offsets") ;
        var mockMessageService = Substitute.For<IMessageService>() ;

        var commonServices = new CommonServices(mockMessageService,
            Host.GetService<ILogger>(),
            Host.GetService<IUnitConverter>(),
            Host.GetService<ISettingsService>(),
            Host.GetService<IResourceHelper>()) ;

        var displayUnitProvider = Host.GetService<IDisplayUnitProvider>() ;
        var displayUnit =
            commonServices.SettingsService.GetDisplayUnitOrDefault(() => displayUnitProvider.GetDefaultDisplayUnit()) ;
        var unitConverter = Host.GetService<IUnitConverter>() ;
        var baseOffsetInternal = unitConverter.ToInternalUnit(testBaseOffsetDisplay,
            displayUnit) ;
        var topOffsetInternal = unitConverter.ToInternalUnit(testTopOffsetDisplay,
            displayUnit) ;

        // Step 9: Create column creation context
        Log("Step 9: Creating columns") ;
        var settings = new ColumnFromCadSettings
        {
            RectangularColumnFamilyId = rectangularFamily.UniqueId,
            CircularColumnFamilyId = circularFamily.UniqueId,
            WidthParameter = testWidthParameter,
            HeightParameter = testHeightParameter,
            DiameterParameter = testDiameterParameter,
            BaseLevelId = baseLevel.UniqueId,
            TopLevelId = topLevel.UniqueId,
            BaseOffsetDisplay = testBaseOffsetDisplay,
            TopOffsetDisplay = testTopOffsetDisplay
        } ;

        var context = new ColumnCreationContext
        {
            Settings = settings, BaseOffset = baseOffsetInternal, TopOffset = topOffsetInternal
        } ;

        // Create columns
        var elementIds = columnFromCadOrchestrator.CreateColumns(context) ;

        // Verify exactly 45 columns were created (37 rectangular + 8 circular)
        const int expectedTotalColumns = 45 ;
        Assert.AreEqual(expectedTotalColumns,
            elementIds.Count,
            $"Should create exactly {expectedTotalColumns} columns (37 rectangular + 8 circular)") ;

        // Step 10: Verify columns were created
        Log("Step 10: Verifying columns were created") ;
        var columnsAfter = new FilteredElementCollector(document).OfCategory(BuiltInCategory.OST_StructuralColumns)
            .WhereElementIsNotElementType()
            .GetElementCount() ;

        Log($"Columns after: {columnsAfter}") ;

        var columnsCreated = columnsAfter - columnsBefore ;
        Log($"Total columns created: {columnsCreated}") ;

        Assert.Greater(elementIds.Count,
            0,
            "At least one column should be created") ;
        Assert.AreEqual(elementIds.Count,
            columnsCreated,
            "Number of created column IDs should match the increase in column count") ;

        Log($"✓ SUCCESS: Created {elementIds.Count} columns successfully") ;
        Assert.Pass($"Successfully created {elementIds.Count} columns from CAD link") ;
    }
}
