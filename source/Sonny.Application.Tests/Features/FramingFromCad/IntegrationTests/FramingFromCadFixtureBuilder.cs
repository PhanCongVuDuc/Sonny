using System ;
using System.IO ;
using System.Linq ;
using System.Reflection ;
using System.Runtime.CompilerServices ;
using Autodesk.Revit.DB ;
using NUnit.Framework ;
using Sonny.RevitExtensions.Extensions ;
using Sonny.RevitExtensions.Extensions.GeometryObjects.Curves ;

namespace Sonny.Application.Tests.Features.FramingFromCad.IntegrationTests ;

/// <summary>
///     One-time builder for the FramingFromCad fixture (Test_V2023_FramingFromCad.rvt). Seeds from
///     `PlaceHolder_V2023.rvt` — which already carries `M_Concrete-Rectangular Beam` with "b"/"h" type
///     parameters, so no family authoring is involved — and imports the **real Revit-exported DWG** the
///     project owner supplied under `Resources/RevitFiles/Dwgs`.
///     <para>
///         An earlier version of this builder generated its own DXF from coordinates in source. That was
///         replaced because the hand-made drawing was far too clean: it never produced a group of three
///         strokes, never made two end caps pair with each other, and so never exercised the two quirks
///         that dominate the feature's behaviour on a real drawing.
///     </para>
///     Self-verifies every count in <see cref="FramingFromCadFixtureFacts" /> before saving — a fixture
///     that does not verify is worse than none, because the test would then pass against a drawing that
///     means something else.
///     Must run on Revit 2023 so every later version can still open the file:
///     dotnet test -c "Debug R23" --filter "FullyQualifiedName~FramingFromCadFixtureBuilder"
///     Skips itself when the fixture already exists — delete the .rvt to rebuild.
/// </summary>
[TestFixture]
public class FramingFromCadFixtureBuilder : SonnyDocumentTestBase
{
    private const string FixtureFileName = "Test_V2023_FramingFromCad.rvt" ;
    private const string PlaceholderFileName = "PlaceHolder_V2023.rvt" ;
    private const double FeetPerMillimeter = 1 / 304.8 ;

    private bool _buildAttempted ;
    private bool _succeeded ;

    private string? _documentFilePath ;

    /// <summary>
    ///     Cached because the base OnSetup reads this twice, and the copy below makes a second evaluation
    ///     take the "already exists" branch
    /// </summary>
    protected override string? DocumentFilePath => _documentFilePath ??= ResolveDocumentFilePath() ;

    private string? ResolveDocumentFilePath()
    {
        var sourcePath = GetSourceFixturePath() ;
        if (Application!.VersionNumber != "2023"
            || File.Exists(sourcePath)) {
            return GetTestRevitFilePath(FixtureFileName)
                   ?? GetTestRevitFilePath(PlaceholderFileName) ;
        }

        File.Copy(GetTestRevitFilePath(PlaceholderFileName)!,
            sourcePath,
            true) ;

        return sourcePath ;
    }

    [Test]
    public void BuildFixture()
    {
        if (Application!.VersionNumber != "2023") {
            Assert.Ignore("The fixture must be authored in Revit 2023 so later versions can still open it.") ;
        }

        var targetPath = GetSourceFixturePath() ;
        if (! string.Equals(Document!.PathName,
                targetPath,
                StringComparison.OrdinalIgnoreCase)) {
            Assert.Ignore($"{FixtureFileName} already exists — delete it to rebuild.") ;
        }

        _buildAttempted = true ;

        var importInstance = ImportDrawing(Document!) ;
        VerifyDrawing(importInstance) ;

        Document!.Save() ;
        CopyFixtureToOutput(targetPath) ;
        _succeeded = true ;

        Assert.Pass($"Fixture written to {targetPath}") ;
    }

    protected override void OnTearDown()
    {
        if (! string.Equals(Path.GetFileName(Document!.PathName),
                PlaceholderFileName,
                StringComparison.OrdinalIgnoreCase)) {
            base.OnTearDown() ;
        }

        if (_buildAttempted
            && ! _succeeded) {
            // Never leave a half-built file behind: its existence makes reruns skip themselves
            var targetPath = GetSourceFixturePath() ;
            if (File.Exists(targetPath)) {
                File.Delete(targetPath) ;
            }
        }
    }

    /// <summary>
    ///     Imports the supplied DWG origin-to-origin in millimetres
    /// </summary>
    private ImportInstance ImportDrawing(Document document)
    {
        var dwgPath = GetSourceDwgPath() ;
        Assert.IsTrue(File.Exists(dwgPath),
            $"The supplied drawing is missing: {dwgPath}") ;
        Log($"Importing {dwgPath}") ;

        var view = document.GetAllElements<ViewPlan>()
            .FirstOrDefault(candidate => ! candidate.IsTemplate) ;
        Assert.IsNotNull(view,
            "The placeholder project has no plan view to import into") ;

        ElementId importedId ;
        using (var transaction = new Transaction(document,
                   "Import framing DWG")) {
            transaction.Start() ;

            // A structural template hides some categories in a fresh view. The feature reads model
            // geometry rather than a view-scoped collector, but make the import visible anyway so the
            // fixture can be opened and eyeballed
            if (Category.GetCategory(document,
                    BuiltInCategory.OST_ImportObjectStyles) is { } importCategory
                && view!.CanCategoryBeHidden(importCategory.Id)) {
                view.SetCategoryHidden(importCategory.Id,
                    false) ;
            }

            var options = new DWGImportOptions {
                Unit = ImportUnit.Millimeter,
                Placement = ImportPlacement.Origin,
                ThisViewOnly = false,
                ColorMode = ImportColorMode.Preserved,
            } ;

            // Commit() with no arguments on purpose: a failure preprocessor defined in this assembly
            // makes every commit in this host silently roll back
            var imported = document.Import(dwgPath,
                options,
                view,
                out importedId) ;
            Assert.IsTrue(imported,
                $"Revit refused to import {dwgPath}") ;
            transaction.Commit() ;
        }

        Assert.AreNotEqual(ElementId.InvalidElementId,
            importedId,
            "Revit did not import the DWG") ;

        var importInstance = document.GetElement(importedId) as ImportInstance ;
        Assert.IsNotNull(importInstance,
            $"Imported element {importedId} is not an ImportInstance") ;

        Log($"Imported CAD link UniqueId: {importInstance!.UniqueId}") ;

        return importInstance ;
    }

    /// <summary>
    ///     Checks the drawing means what the tests will assume, using the same two helpers the feature uses
    /// </summary>
    private void VerifyDrawing(ImportInstance importInstance)
    {
        var layerNames = importInstance.GetAllLayerNames(true)
            .ToList() ;
        Log($"Layers: {string.Join(", ", layerNames.OrderBy(name => name))}") ;

        AssertLayerStrokeCount(importInstance,
            layerNames,
            FramingFromCadFixtureFacts.BeamLayerName,
            FramingFromCadFixtureFacts.BeamLayerStrokeCount) ;
        AssertLayerStrokeCount(importInstance,
            layerNames,
            FramingFromCadFixtureFacts.ColumnLayerName,
            FramingFromCadFixtureFacts.ColumnLayerStrokeCount) ;
        AssertLayerStrokeCount(importInstance,
            layerNames,
            FramingFromCadFixtureFacts.BeamHiddenLineLayerName,
            FramingFromCadFixtureFacts.BeamHiddenLineLayerStrokeCount) ;
        AssertLayerStrokeCount(importInstance,
            layerNames,
            FramingFromCadFixtureFacts.GridLayerName,
            FramingFromCadFixtureFacts.GridLayerStrokeCount) ;

        // Pairing, per section width, against the untouched pool — this is the single-line-mode-off case
        var strokes = importInstance.GetLinesOnLayer(FramingFromCadFixtureFacts.BeamLayerName) ;
        AssertPairCount(strokes,
            FramingFromCadFixtureFacts.NarrowWidthMm,
            FramingFromCadFixtureFacts.NarrowPairCount) ;
        AssertPairCount(strokes,
            FramingFromCadFixtureFacts.MediumWidthMm,
            FramingFromCadFixtureFacts.MediumPairCount) ;
        AssertPairCount(strokes,
            FramingFromCadFixtureFacts.WideWidthMm,
            FramingFromCadFixtureFacts.WidePairCount) ;
        AssertPairCount(strokes,
            FramingFromCadFixtureFacts.WidestWidthMm,
            FramingFromCadFixtureFacts.WidestPairCount) ;

        VerifyFramingFamilyPresent() ;
    }

    private void AssertLayerStrokeCount(ImportInstance importInstance,
        System.Collections.Generic.List<string> layerNames,
        string layerName,
        int expected)
    {
        Assert.Contains(layerName,
            layerNames,
            $"Layer {layerName} missing from the drawing") ;

        var actual = importInstance.GetLinesOnLayer(layerName)
            .Count ;
        Log($"  {layerName}: {actual} strokes") ;
        Assert.AreEqual(expected,
            actual,
            $"Straight strokes on {layerName}") ;
    }

    private void AssertPairCount(System.Collections.Generic.List<Curve> strokes,
        double widthMm,
        int expected)
    {
        var groups = strokes.GetParallelCurveGroups(widthMm * FeetPerMillimeter) ;
        Log($"  width {widthMm:F0} mm: {groups.Count} groups") ;
        Assert.AreEqual(expected,
            groups.Count,
            $"Pairs {widthMm:F0} mm apart on {FramingFromCadFixtureFacts.BeamLayerName}") ;
    }

    private void VerifyFramingFamilyPresent()
    {
        var family = Document!.GetAllElements<Family>()
            .FirstOrDefault(candidate => candidate.Name == FramingFromCadFixtureFacts.FramingFamilyName) ;
        Assert.IsNotNull(family,
            $"{FramingFromCadFixtureFacts.FramingFamilyName} is not loaded in the placeholder project") ;

        var typeNames = family!.GetFamilySymbolIds()
            .Select(id => Document!.GetElement(id))
            .OfType<FamilySymbol>()
            .Select(symbol => symbol.Name)
            .ToList() ;
        Log($"  {FramingFromCadFixtureFacts.FramingFamilyName} types: {string.Join(", ", typeNames)}") ;

        Assert.Contains(FramingFromCadFixtureFacts.ExistingMediumTypeName,
            typeNames,
            "The fixture needs this type so one section matches an existing symbol") ;
        Assert.Contains(FramingFromCadFixtureFacts.ExistingWidestTypeName,
            typeNames,
            "The fixture needs this type so a second section matches an existing symbol") ;

        Assert.IsFalse(typeNames.Contains(FramingFromCadFixtureFacts.GeneratedNarrowTypeName),
            $"{FramingFromCadFixtureFacts.GeneratedNarrowTypeName} already exists, so the duplicate "
            + "branch would not be exercised") ;
        Assert.IsFalse(typeNames.Contains(FramingFromCadFixtureFacts.GeneratedWideTypeName),
            $"{FramingFromCadFixtureFacts.GeneratedWideTypeName} already exists, so the duplicate "
            + "branch would not be exercised") ;
    }

    /// <summary>
    ///     The test adapter shadow-copies the assembly, so the project folder cannot be found from the
    ///     assembly location — the compile-time source path of this file can, always
    /// </summary>
    private static string GetSourceFixturePath([CallerFilePath] string sourceFilePath = "") =>
        Path.Combine(FindTestProjectDirectory(sourceFilePath),
            "Resources",
            "RevitFiles",
            FixtureFileName) ;

    private static string GetSourceDwgPath([CallerFilePath] string sourceFilePath = "") =>
        Path.Combine(FindTestProjectDirectory(sourceFilePath),
            "Resources",
            "RevitFiles",
            "Dwgs",
            FramingFromCadFixtureFacts.DwgFileName) ;

    private static string FindTestProjectDirectory(string sourceFilePath)
    {
        var directory = new DirectoryInfo(Path.GetDirectoryName(sourceFilePath)!) ;
        while (directory != null
               && ! File.Exists(Path.Combine(directory.FullName,
                   "Sonny.Application.Tests.csproj"))) {
            directory = directory.Parent ;
        }

        Assert.IsNotNull(directory,
            $"Could not locate the test project directory above {sourceFilePath}") ;

        return directory!.FullName ;
    }

    private void CopyFixtureToOutput(string targetPath)
    {
        var outputPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly()
                .Location)!,
            "Resources",
            "RevitFiles",
            FixtureFileName) ;
        Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!) ;
        File.Copy(targetPath,
            outputPath,
            true) ;
        Log($"Fixture saved: {targetPath}") ;
    }
}
