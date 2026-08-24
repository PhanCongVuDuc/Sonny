using System ;
using System.Collections.Generic ;
using System.IO ;
using System.Linq ;
using System.Reflection ;
using Autodesk.Revit.DB ;
using Autodesk.Revit.DB.Structure ;
using NUnit.Framework ;
using Sonny.RevitExtensions.Extensions.Elements ;
using Sonny.RevitExtensions.Extensions.GeometryObjects.Solids ;

namespace Sonny.Application.Tests.Features.AutoJoin.IntegrationTests ;

/// <summary>
///     One-time builder for the AutoJoin fixture (Test_V2023_AutoJoin.rvt). Draws every AutoJoin
///     test case as an isolated station along the X axis (15 m apart), tags each element through
///     its Comments parameter (<see cref="AutoJoinFixtureTags" />), self-verifies that every pair
///     really intersects (or really does not, for the F7 case) and only then saves the model into
///     the source Resources/RevitFiles folder so the build copies it next to the tests.
///     The machine has no standard family library installed, so the loadable families (beam,
///     columns, footing, generic model) are authored in memory from the stock family templates as
///     plain boxes — oversized on purpose so placement conventions cannot break the overlaps.
///     Must run on Revit 2023 so every later version can still open the file:
///     dotnet test -c "Debug R23" --filter "FullyQualifiedName~AutoJoinFixtureBuilder"
///     Skips itself when the fixture already exists — delete the .rvt to rebuild.
/// </summary>
[TestFixture]
public class AutoJoinFixtureBuilder : SonnyDocumentTestBase
{
    private const string FixtureFileName = "Test_V2023_AutoJoin.rvt" ;
    private const string FixtureViewName = "AutoJoin 3D" ;
    private const string PlaceholderFileName = "PlaceHolder_V2023.rvt" ;
    private const double FeetPerMillimeter = 1 / 304.8 ;
    private const double StationSpacingMm = 15000 ;

    private bool _buildAttempted ;
    private bool _succeeded ;

    private string? _documentFilePath ;

    /// <summary>
    ///     The document must be opened by the base OnSetup (a separate API event) — a document
    ///     opened and committed within the same event rolls every commit back in this host.
    ///     The fixture is seeded by copying the known-good placeholder project: documents created
    ///     from an .rte template also rolled back every commit (delayed propagation regen state).
    ///     Cached because the base OnSetup reads the property twice, and the copy below makes a
    ///     second evaluation take the "already exists" branch
    /// </summary>
    protected override string? DocumentFilePath => _documentFilePath ??= ResolveDocumentFilePath() ;

    private string? ResolveDocumentFilePath()
    {
        var sourcePath = GetSourceFixturePath() ;
        if (Application!.VersionNumber != "2023"
            || File.Exists(sourcePath)) {
            // Nothing to build this run — open the existing fixture so teardown stays sane
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
        var cases = BuildModel(Document!) ;
        VerifyCases(Document!,
            cases) ;
        Document!.Save() ;
        CopyFixtureToOutput(targetPath) ;
        _succeeded = true ;

        Assert.Pass($"Fixture written to {targetPath}") ;
    }

    protected override void OnTearDown()
    {
        // When nothing was built the opened document may BE the placeholder — the base teardown
        // would then try to close the active document, which Revit refuses. Compare by file name:
        // GetTestRevitFilePath mixes path separators, so full-path comparison misses
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

    private List<JoinCase> BuildModel(Document document)
    {
        // If even an empty commit rolls back here, the document or the host is broken and no
        // amount of content fixing will help — fail with that name immediately
        using (var probeTransaction = new Transaction(document,
                   "AutoJoin probe")) {
            probeTransaction.Start() ;
            CommitStage(probeTransaction,
                "Probe-BeforeFamilyLoads",
                false) ;
        }

        var level = new FilteredElementCollector(document).OfClass(typeof( Level ))
            .Cast<Level>()
            .OrderBy(l => l.Elevation)
            .First() ;

        // Loadable families must be created before the project transaction opens
        var beamSymbol = LoadBoxFamilySymbol(document,
            "Metric Structural Framing - Beams and Braces.rft",
            "AutoJoin Beam",
            -3000,
            3000,
            -300,
            300,
            -2000,
            500) ;
        var structuralColumnSymbol = LoadBoxFamilySymbol(document,
            "Metric Structural Column.rft",
            "AutoJoin Structural Column",
            -200,
            200,
            -200,
            200,
            0,
            3000) ;
        var architecturalColumnSymbol = LoadBoxFamilySymbol(document,
            "Metric Column.rft",
            "AutoJoin Architectural Column",
            -200,
            200,
            -200,
            200,
            0,
            3000) ;
        var foundationSymbol = LoadBoxFamilySymbol(document,
            "Metric Structural Foundation.rft",
            "AutoJoin Footing",
            -750,
            750,
            -750,
            750,
            -400,
            400) ;
        var genericModelSymbol = LoadBoxFamilySymbol(document,
            "Metric Generic Model.rft",
            "AutoJoin Generic Model",
            -500,
            500,
            -500,
            500,
            0,
            1000) ;

        var cases = new List<JoinCase>() ;

        using var transaction = new Transaction(document,
            "Build AutoJoin fixture") ;
        transaction.Start() ;

        // Empty probe: distinguishes "family loading corrupted the document" from content issues
        CommitStage(transaction,
            "Probe-AfterFamilyLoads") ;

        // One stage per symbol so a family whose activation breaks regeneration names itself
        ActivateSymbol(transaction,
            beamSymbol,
            "Activate-Beam") ;
        ActivateSymbol(transaction,
            structuralColumnSymbol,
            "Activate-StructuralColumn") ;
        ActivateSymbol(transaction,
            architecturalColumnSymbol,
            "Activate-ArchitecturalColumn") ;
        ActivateSymbol(transaction,
            foundationSymbol,
            "Activate-Footing") ;
        ActivateSymbol(transaction,
            genericModelSymbol,
            "Activate-GenericModel") ;

        var wallTypeId = new FilteredElementCollector(document).OfClass(typeof( WallType ))
            .Cast<WallType>()
            .First(type => type.Kind == WallKind.Basic)
            .Id ;
        var floorTypeId = GetDefaultTypeId(document,
            ElementTypeGroup.FloorType,
            typeof( FloorType )) ;
        // Must be a compound ceiling: the basic ceiling type has no thickness, so no solid to join
        var ceilingTypes = new FilteredElementCollector(document).OfClass(typeof( CeilingType ))
            .Cast<CeilingType>()
            .ToList() ;
        var ceilingTypeId = (ceilingTypes.FirstOrDefault(type => type.FamilyName == "Compound Ceiling")
                             ?? ceilingTypes.First()).Id ;
        // GetDefaultElementTypeId(RoofType) can return an id that resolves to nothing — pick a
        // basic roof type from the collector instead
        var roofType = new FilteredElementCollector(document).OfClass(typeof( RoofType ))
            .Cast<RoofType>()
            .First(type => type.FamilyName == "Basic Roof") ;

        CommitStage(transaction,
            "Setup") ;

        // S01 — Beam -> StructuralColumn
        var x = 0d ;
        var beam01 = CreateBeam(document,
            beamSymbol,
            level,
            x,
            AutoJoinFixtureTags.S01Beam) ;
        var column01 = CreatePointInstance(document,
            structuralColumnSymbol,
            level,
            x,
            0,
            StructuralType.Column,
            AutoJoinFixtureTags.S01Column) ;
        cases.Add(new JoinCase("S01",
            beam01,
            column01)) ;
        CommitStage(transaction,
            "S01") ;

        // S02 — same pair for the IsReverse rule
        x += StationSpacingMm ;
        var beam02 = CreateBeam(document,
            beamSymbol,
            level,
            x,
            AutoJoinFixtureTags.S02Beam) ;
        var column02 = CreatePointInstance(document,
            structuralColumnSymbol,
            level,
            x,
            0,
            StructuralType.Column,
            AutoJoinFixtureTags.S02Column) ;
        cases.Add(new JoinCase("S02",
            beam02,
            column02)) ;
        CommitStage(transaction,
            "S02") ;

        // S03 — StructuralColumn -> ArchitecturalFloor (plain floor)
        x += StationSpacingMm ;
        var column03 = CreatePointInstance(document,
            structuralColumnSymbol,
            level,
            x,
            0,
            StructuralType.Column,
            AutoJoinFixtureTags.S03Column) ;
        var floor03 = CreateFloor(document,
            floorTypeId,
            level,
            x,
            0,
            1500,
            500,
            false,
            AutoJoinFixtureTags.S03Floor) ;
        cases.Add(new JoinCase("S03",
            column03,
            floor03)) ;
        CommitStage(transaction,
            "S03") ;

        // S04 — StructuralFloor rule must join only the structural floor
        x += StationSpacingMm ;
        var column04 = CreatePointInstance(document,
            structuralColumnSymbol,
            level,
            x,
            0,
            StructuralType.Column,
            AutoJoinFixtureTags.S04Column) ;
        var structuralFloor04 = CreateFloor(document,
            floorTypeId,
            level,
            x,
            0,
            1500,
            500,
            true,
            AutoJoinFixtureTags.S04StructuralFloor) ;
        var architecturalFloor04 = CreateFloor(document,
            floorTypeId,
            level,
            x,
            0,
            1500,
            1500,
            false,
            AutoJoinFixtureTags.S04ArchitecturalFloor) ;
        cases.Add(new JoinCase("S04-structural",
            column04,
            structuralFloor04)) ;
        cases.Add(new JoinCase("S04-architectural",
            column04,
            architecturalFloor04)) ;
        CommitStage(transaction,
            "S04") ;

        // S05 — StructuralWall rule must join only the structural wall
        x += StationSpacingMm ;
        var structuralWall05 = CreateWall(document,
            wallTypeId,
            level,
            x,
            -2000,
            true,
            AutoJoinFixtureTags.S05StructuralWall) ;
        var architecturalWall05 = CreateWall(document,
            wallTypeId,
            level,
            x,
            2000,
            false,
            AutoJoinFixtureTags.S05ArchitecturalWall) ;
        var floor05 = CreateFloor(document,
            floorTypeId,
            level,
            x,
            0,
            3000,
            500,
            false,
            AutoJoinFixtureTags.S05Floor) ;
        cases.Add(new JoinCase("S05-structural",
            structuralWall05,
            floor05)) ;
        cases.Add(new JoinCase("S05-architectural",
            architecturalWall05,
            floor05)) ;
        CommitStage(transaction,
            "S05") ;

        // S06 — ArchitecturalWall rule matches a structural wall too (D1 quirk)
        x += StationSpacingMm ;
        var structuralWall06 = CreateWall(document,
            wallTypeId,
            level,
            x,
            0,
            true,
            AutoJoinFixtureTags.S06StructuralWall) ;
        var floor06 = CreateFloor(document,
            floorTypeId,
            level,
            x,
            0,
            3000,
            500,
            false,
            AutoJoinFixtureTags.S06Floor) ;
        cases.Add(new JoinCase("S06",
            structuralWall06,
            floor06)) ;
        CommitStage(transaction,
            "S06") ;

        // S07 — Beam -> Foundation. A column here would auto-attach the footing and the
        // mandatory-move warning rolls the whole transaction back; a beam has no such behaviour
        x += StationSpacingMm ;
        var beam07 = CreateBeam(document,
            beamSymbol,
            level,
            x,
            AutoJoinFixtureTags.S07Beam,
            0) ;
        var foundation07 = CreatePointInstance(document,
            foundationSymbol,
            level,
            x,
            0,
            StructuralType.Footing,
            AutoJoinFixtureTags.S07Foundation) ;
        cases.Add(new JoinCase("S07",
            beam07,
            foundation07)) ;
        CommitStage(transaction,
            "S07") ;

        // S08 — StructuralColumn -> Roof
        x += StationSpacingMm ;
        var column08 = CreatePointInstance(document,
            structuralColumnSymbol,
            level,
            x,
            0,
            StructuralType.Column,
            AutoJoinFixtureTags.S08Column) ;
        var roof08 = CreateRoof(document,
            roofType,
            level,
            x,
            0,
            1500,
            2700,
            AutoJoinFixtureTags.S08Roof) ;
        cases.Add(new JoinCase("S08",
            column08,
            roof08)) ;
        CommitStage(transaction,
            "S08") ;

        // S09 — StructuralColumn -> Ceiling
        x += StationSpacingMm ;
        var column09 = CreatePointInstance(document,
            structuralColumnSymbol,
            level,
            x,
            0,
            StructuralType.Column,
            AutoJoinFixtureTags.S09Column) ;
        var ceiling09 = CreateCeiling(document,
            ceilingTypeId,
            level,
            x,
            0,
            1500,
            2500,
            AutoJoinFixtureTags.S09Ceiling) ;
        cases.Add(new JoinCase("S09",
            column09,
            ceiling09)) ;
        CommitStage(transaction,
            "S09") ;

        // S10 — StructuralColumn -> GenericModel
        x += StationSpacingMm ;
        var column10 = CreatePointInstance(document,
            structuralColumnSymbol,
            level,
            x,
            0,
            StructuralType.Column,
            AutoJoinFixtureTags.S10Column) ;
        var genericModel10 = CreatePointInstance(document,
            genericModelSymbol,
            level,
            x,
            0,
            StructuralType.NonStructural,
            AutoJoinFixtureTags.S10GenericModel) ;
        cases.Add(new JoinCase("S10",
            column10,
            genericModel10)) ;
        CommitStage(transaction,
            "S10") ;

        // S11 — Beam -> ArchitecturalColumn
        x += StationSpacingMm ;
        var column11 = CreatePointInstance(document,
            architecturalColumnSymbol,
            level,
            x,
            0,
            StructuralType.NonStructural,
            AutoJoinFixtureTags.S11Column) ;
        var beam11 = CreateBeam(document,
            beamSymbol,
            level,
            x,
            AutoJoinFixtureTags.S11Beam) ;
        cases.Add(new JoinCase("S11",
            beam11,
            column11)) ;
        CommitStage(transaction,
            "S11") ;

        // S12 — Beam -> All joins the column and the floor
        x += StationSpacingMm ;
        var beam12 = CreateBeam(document,
            beamSymbol,
            level,
            x,
            AutoJoinFixtureTags.S12Beam) ;
        var column12 = CreatePointInstance(document,
            structuralColumnSymbol,
            level,
            x,
            0,
            StructuralType.Column,
            AutoJoinFixtureTags.S12Column) ;
        var floor12 = CreateFloor(document,
            floorTypeId,
            level,
            x,
            0,
            1500,
            1000,
            false,
            AutoJoinFixtureTags.S12Floor) ;
        cases.Add(new JoinCase("S12-column",
            beam12,
            column12)) ;
        cases.Add(new JoinCase("S12-floor",
            beam12,
            floor12)) ;
        CommitStage(transaction,
            "S12") ;

        // S13 — bounding boxes overlap, solids do not (quarter-arc wall, column inside the arc)
        x += StationSpacingMm ;
        var arcWall13 = CreateArcWall(document,
            wallTypeId,
            level,
            x,
            AutoJoinFixtureTags.S13ArcWall) ;
        var column13 = CreatePointInstance(document,
            structuralColumnSymbol,
            level,
            x + 300,
            300,
            StructuralType.Column,
            AutoJoinFixtureTags.S13Column) ;
        cases.Add(new JoinCase("S13",
            column13,
            arcWall13,
            false)) ;
        CommitStage(transaction,
            "S13") ;

        // S14 — pre-joined pair for the unjoin rule
        x += StationSpacingMm ;
        var beam14 = CreateBeam(document,
            beamSymbol,
            level,
            x,
            AutoJoinFixtureTags.S14Beam) ;
        var column14 = CreatePointInstance(document,
            structuralColumnSymbol,
            level,
            x,
            0,
            StructuralType.Column,
            AutoJoinFixtureTags.S14Column) ;
        cases.Add(new JoinCase("S14",
            beam14,
            column14,
            true,
            true)) ;
        CommitStage(transaction,
            "S14") ;

        // S15 — CutSelectedElements mode. The two candidates must cut DISJOINT parts of the
        // column: when their cut regions overlap, Revit decides the later pair no longer
        // intersects and silently unjoins it while committing
        x += StationSpacingMm ;
        var column15 = CreatePointInstance(document,
            structuralColumnSymbol,
            level,
            x,
            0,
            StructuralType.Column,
            AutoJoinFixtureTags.S15Column) ;
        var beam15 = CreateBeam(document,
            beamSymbol,
            level,
            x,
            AutoJoinFixtureTags.S15Beam,
            3500) ;
        // Offset to clip only a corner of the column: centred, the generic model (z 0..1000) plus
        // the beam (z 500..3000) would consume the column's entire body and Revit undoes the
        // second join at commit ("joined elements no longer intersect")
        var genericModel15 = CreatePointInstance(document,
            genericModelSymbol,
            level,
            x + 550,
            550,
            StructuralType.NonStructural,
            AutoJoinFixtureTags.S15GenericModel) ;
        cases.Add(new JoinCase("S15-beam",
            beam15,
            column15)) ;
        cases.Add(new JoinCase("S15-generic",
            genericModel15,
            column15)) ;
        CommitStage(transaction,
            "S15") ;

        // S16 — CutOtherElements mode
        x += StationSpacingMm ;
        var column16 = CreatePointInstance(document,
            structuralColumnSymbol,
            level,
            x,
            0,
            StructuralType.Column,
            AutoJoinFixtureTags.S16Column) ;
        var beam16 = CreateBeam(document,
            beamSymbol,
            level,
            x,
            AutoJoinFixtureTags.S16Beam) ;
        cases.Add(new JoinCase("S16",
            beam16,
            column16)) ;
        CommitStage(transaction,
            "S16") ;

        // Normalize join state: nothing joined except the S14 pair, whatever Revit did on creation
        document.Regenerate() ;
        foreach (var joinCase in cases) {
            var joined = JoinGeometryUtils.AreElementsJoined(document,
                joinCase.ElementA,
                joinCase.ElementB) ;
            if (joinCase.PreJoined && ! joined) {
                JoinGeometryUtils.JoinGeometry(document,
                    joinCase.ElementA,
                    joinCase.ElementB) ;
            }
            else if (! joinCase.PreJoined && joined) {
                JoinGeometryUtils.UnjoinGeometry(document,
                    joinCase.ElementA,
                    joinCase.ElementB) ;
            }
        }

        CommitStage(transaction,
            "NormalizeJoins") ;

        CreateFixtureView(document) ;

        CommitStage(transaction,
            "CreateView",
            false) ;

        return cases ;
    }

    private void VerifyCases(Document document,
        List<JoinCase> cases)
    {
        var problems = new List<string>() ;
        foreach (var joinCase in cases) {
            if (! joinCase.ElementA.IsValidObject
                || ! joinCase.ElementB.IsValidObject) {
                problems.Add($"{joinCase.Name}: an element was deleted during commit "
                             + $"(A valid={joinCase.ElementA.IsValidObject}, B valid={joinCase.ElementB.IsValidObject})") ;
                continue ;
            }

            var solidA = joinCase.ElementA.GetSolidMax() ;
            var solidB = joinCase.ElementB.GetSolidMax() ;
            if (solidA == null
                || solidB == null) {
                problems.Add($"{joinCase.Name}: an element has no solid") ;
                continue ;
            }

            var intersects = solidA.IntersectsSolid(solidB) ;
            if (intersects != joinCase.ExpectIntersect) {
                problems.Add($"{joinCase.Name}: solids intersect = {intersects}, expected {joinCase.ExpectIntersect}") ;
            }

            var joined = JoinGeometryUtils.AreElementsJoined(document,
                joinCase.ElementA,
                joinCase.ElementB) ;
            if (joined != joinCase.PreJoined) {
                problems.Add($"{joinCase.Name}: joined = {joined}, expected {joinCase.PreJoined}") ;
            }
        }

        foreach (var problem in problems) {
            Log($"✗ {problem}") ;
        }

        Assert.IsTrue(problems.Count == 0,
            $"Fixture self-check failed:\n{string.Join("\n", problems)}") ;
        Log($"✓ All {cases.Count} fixture cases verified") ;
    }

    #region Element creation

    private FamilyInstance CreateBeam(Document document,
        FamilySymbol symbol,
        Level level,
        double xMm,
        string tag,
        double zMm = 2500)
    {
        var line = Line.CreateBound(MmPoint(xMm - 1000,
                0,
                zMm),
            MmPoint(xMm + 1000,
                0,
                zMm)) ;
        var beam = document.Create.NewFamilyInstance(line,
            symbol,
            level,
            StructuralType.Beam) ;
        Tag(beam,
            tag) ;

        return beam ;
    }

    private FamilyInstance CreatePointInstance(Document document,
        FamilySymbol symbol,
        Level level,
        double xMm,
        double yMm,
        StructuralType structuralType,
        string tag)
    {
        var instance = document.Create.NewFamilyInstance(MmPoint(xMm,
                yMm,
                0),
            symbol,
            level,
            structuralType) ;
        Tag(instance,
            tag) ;

        return instance ;
    }

    private Floor CreateFloor(Document document,
        ElementId floorTypeId,
        Level level,
        double xMm,
        double yMm,
        double halfSizeMm,
        double offsetMm,
        bool structural,
        string tag)
    {
        var loop = CreateRectangleLoop(xMm - halfSizeMm,
            xMm + halfSizeMm,
            yMm - halfSizeMm,
            yMm + halfSizeMm) ;
        var floor = Floor.Create(document,
            new List<CurveLoop> { loop },
            floorTypeId,
            level.Id) ;
        floor.get_Parameter(BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM)!.Set(MmToFeet(offsetMm)) ;
        floor.get_Parameter(BuiltInParameter.FLOOR_PARAM_IS_STRUCTURAL)!.Set(structural ? 1 : 0) ;
        Tag(floor,
            tag) ;

        return floor ;
    }

    private Wall CreateWall(Document document,
        ElementId wallTypeId,
        Level level,
        double xMm,
        double yMm,
        bool structural,
        string tag)
    {
        var line = Line.CreateBound(MmPoint(xMm - 2000,
                yMm,
                0),
            MmPoint(xMm + 2000,
                yMm,
                0)) ;
        var wall = Wall.Create(document,
            line,
            wallTypeId,
            level.Id,
            MmToFeet(3000),
            0,
            false,
            structural) ;
        Tag(wall,
            tag) ;

        return wall ;
    }

    private Wall CreateArcWall(Document document,
        ElementId wallTypeId,
        Level level,
        double xMm,
        string tag)
    {
        // Quarter arc: bounding box covers the square, the solid only the curved band
        var arc = Arc.Create(MmPoint(xMm,
                0,
                0),
            MmToFeet(2000),
            0,
            Math.PI / 2,
            XYZ.BasisX,
            XYZ.BasisY) ;
        var wall = Wall.Create(document,
            arc,
            wallTypeId,
            level.Id,
            MmToFeet(3000),
            0,
            false,
            false) ;
        Tag(wall,
            tag) ;

        return wall ;
    }

    private RoofBase CreateRoof(Document document,
        RoofType roofType,
        Level level,
        double xMm,
        double yMm,
        double halfSizeMm,
        double offsetMm,
        string tag)
    {
        var footprint = CreateRectangleArray(xMm - halfSizeMm,
            xMm + halfSizeMm,
            yMm - halfSizeMm,
            yMm + halfSizeMm) ;
        // The API marshals the mapping as ref — it must be pre-created or Revit throws
        var footPrintToModelCurveMapping = new ModelCurveArray() ;
        var roof = document.Create.NewFootPrintRoof(footprint,
            level,
            roofType,
            out footPrintToModelCurveMapping) ;
        roof.get_Parameter(BuiltInParameter.ROOF_LEVEL_OFFSET_PARAM)!.Set(MmToFeet(offsetMm)) ;
        Tag(roof,
            tag) ;

        return roof ;
    }

    private Ceiling CreateCeiling(Document document,
        ElementId ceilingTypeId,
        Level level,
        double xMm,
        double yMm,
        double halfSizeMm,
        double offsetMm,
        string tag)
    {
        var loop = CreateRectangleLoop(xMm - halfSizeMm,
            xMm + halfSizeMm,
            yMm - halfSizeMm,
            yMm + halfSizeMm) ;
        var ceiling = Ceiling.Create(document,
            new List<CurveLoop> { loop },
            ceilingTypeId,
            level.Id) ;
        ceiling.get_Parameter(BuiltInParameter.CEILING_HEIGHTABOVELEVEL_PARAM)!.Set(MmToFeet(offsetMm)) ;
        Tag(ceiling,
            tag) ;

        return ceiling ;
    }

    private static void CreateFixtureView(Document document)
    {
        var viewFamilyType = new FilteredElementCollector(document).OfClass(typeof( ViewFamilyType ))
            .Cast<ViewFamilyType>()
            .First(type => type.ViewFamily == ViewFamily.ThreeDimensional) ;
        var view = View3D.CreateIsometric(document,
            viewFamilyType.Id) ;
        view.Name = FixtureViewName ;
        // The placeholder comes from a structural template, so a new view hides architectural
        // categories (roofs, ceilings, architectural columns) — and the view-scoped join-with
        // collectors then silently return nothing. Unhide every category the cases use.
        view.Discipline = ViewDiscipline.Coordination ;
        view.DetailLevel = ViewDetailLevel.Fine ;
        foreach (var category in new[]
                 {
                     BuiltInCategory.OST_Roofs,
                     BuiltInCategory.OST_Ceilings,
                     BuiltInCategory.OST_Columns,
                     BuiltInCategory.OST_StructuralColumns,
                     BuiltInCategory.OST_StructuralFraming,
                     BuiltInCategory.OST_StructuralFoundation,
                     BuiltInCategory.OST_Floors,
                     BuiltInCategory.OST_Walls,
                     BuiltInCategory.OST_GenericModel
                 }) {
            var categoryId = new ElementId(category) ;
            if (view.CanCategoryBeHidden(categoryId)) {
                view.SetCategoryHidden(categoryId,
                    false) ;
            }
        }
    }

    #endregion

    #region Family authoring

    /// <summary>
    ///     Authors a family that is nothing but a box (in family coordinates, mm), saves it to a
    ///     temp .rfa so the family gets a stable name, and loads it into the project
    /// </summary>
    private FamilySymbol LoadBoxFamilySymbol(Document document,
        string templateFileName,
        string familyName,
        double xMinMm,
        double xMaxMm,
        double yMinMm,
        double yMaxMm,
        double zMinMm,
        double zMaxMm)
    {
        var templatePath = FindFile($@"C:\ProgramData\Autodesk\RVT {Application!.VersionNumber}\Family Templates",
            templateFileName) ;
        var familyDocument = Application.NewFamilyDocument(templatePath) ;
        Family? family ;
        try {
            using (var transaction = new Transaction(familyDocument,
                       "Add box")) {
                transaction.Start() ;
                var plane = Plane.CreateByNormalAndOrigin(XYZ.BasisZ,
                    new XYZ(0,
                        0,
                        MmToFeet(zMinMm))) ;
                var sketchPlane = SketchPlane.Create(familyDocument,
                    plane) ;
                var profile = new CurveArrArray() ;
                profile.Append(CreateRectangleArray(xMinMm,
                    xMaxMm,
                    yMinMm,
                    yMaxMm,
                    zMinMm)) ;
                familyDocument.FamilyCreate.NewExtrusion(true,
                    profile,
                    sketchPlane,
                    MmToFeet(zMaxMm - zMinMm)) ;
                transaction.Commit() ;
            }

            // SaveAs first so the loaded family carries a stable name instead of "Family1"
            var tempFolder = Path.Combine(Path.GetTempPath(),
                "SonnyAutoJoinFamilies") ;
            Directory.CreateDirectory(tempFolder) ;
            var familyPath = Path.Combine(tempFolder,
                $"{familyName}.rfa") ;
            familyDocument.SaveAs(familyPath,
                new SaveAsOptions { OverwriteExistingFile = true }) ;

            // In-memory load is the only route that works in this host — document.LoadFamily
            // from the saved file returns false (with or without load options)
            family = familyDocument.LoadFamily(document,
                new OverwriteFamilyLoadOptions()) ;
        }
        finally {
            familyDocument.Close(false) ;
        }

        Assert.IsNotNull(family,
            $"Failed to load family {familyName}") ;

        return (FamilySymbol)document.GetElement(family!.GetFamilySymbolIds()
            .First()) ;
    }

    private void ActivateSymbol(Transaction transaction,
        FamilySymbol symbol,
        string stageName)
    {
        if (! symbol.IsActive) {
            symbol.Activate() ;
        }

        CommitStage(transaction,
            stageName) ;
    }

    private class OverwriteFamilyLoadOptions : IFamilyLoadOptions
    {
        public bool OnFamilyFound(bool familyInUse,
            out bool overwriteParameterValues)
        {
            overwriteParameterValues = true ;

            return true ;
        }

        public bool OnSharedFamilyFound(Family sharedFamily,
            bool familyInUse,
            out FamilySource source,
            out bool overwriteParameterValues)
        {
            source = FamilySource.Family ;
            overwriteParameterValues = true ;

            return true ;
        }
    }

    #endregion

    #region Helpers

    private sealed class JoinCase(string name,
        Element elementA,
        Element elementB,
        bool expectIntersect = true,
        bool preJoined = false)
    {
        public string Name { get ; } = name ;
        public Element ElementA { get ; } = elementA ;
        public Element ElementB { get ; } = elementB ;
        public bool ExpectIntersect { get ; } = expectIntersect ;
        public bool PreJoined { get ; } = preJoined ;
    }

    /// <summary>
    ///     Commits the stage under the warning-swallowing preprocessor and fails loudly with the
    ///     stage name — a silent RolledBack in one big transaction is undebuggable
    /// </summary>
    private void CommitStage(Transaction transaction,
        string stageName,
        bool restart = true)
    {
        // No failures preprocessor here on purpose: setting one whose class lives in this
        // (shadow-copied) test assembly made every commit roll back silently in this host
        var status = transaction.Commit() ;

        Assert.AreEqual(TransactionStatus.Committed,
            status,
            $"Stage {stageName} did not commit (status {status})") ;
        Log($"[{stageName}] committed") ;

        if (restart) {
            transaction.Start() ;
        }
    }

    private static void Tag(Element element,
        string tag) =>
        element.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS)!.Set(tag) ;

    private static XYZ MmPoint(double xMm,
        double yMm,
        double zMm) =>
        new(xMm * FeetPerMillimeter,
            yMm * FeetPerMillimeter,
            zMm * FeetPerMillimeter) ;

    private static double MmToFeet(double mm) => mm * FeetPerMillimeter ;

    private static CurveLoop CreateRectangleLoop(double xMinMm,
        double xMaxMm,
        double yMinMm,
        double yMaxMm)
    {
        var loop = new CurveLoop() ;
        foreach (Curve curve in CreateRectangleArray(xMinMm,
                     xMaxMm,
                     yMinMm,
                     yMaxMm)) {
            loop.Append(curve) ;
        }

        return loop ;
    }

    private static CurveArray CreateRectangleArray(double xMinMm,
        double xMaxMm,
        double yMinMm,
        double yMaxMm,
        double zMm = 0)
    {
        var p1 = MmPoint(xMinMm,
            yMinMm,
            zMm) ;
        var p2 = MmPoint(xMaxMm,
            yMinMm,
            zMm) ;
        var p3 = MmPoint(xMaxMm,
            yMaxMm,
            zMm) ;
        var p4 = MmPoint(xMinMm,
            yMaxMm,
            zMm) ;

        var array = new CurveArray() ;
        array.Append(Line.CreateBound(p1,
            p2)) ;
        array.Append(Line.CreateBound(p2,
            p3)) ;
        array.Append(Line.CreateBound(p3,
            p4)) ;
        array.Append(Line.CreateBound(p4,
            p1)) ;

        return array ;
    }

    private static ElementId GetDefaultTypeId(Document document,
        ElementTypeGroup group,
        Type fallbackType)
    {
        var id = document.GetDefaultElementTypeId(group) ;
        if (id != ElementId.InvalidElementId) {
            return id ;
        }

        return new FilteredElementCollector(document).OfClass(fallbackType)
            .First()
            .Id ;
    }

    /// <summary>
    ///     The test adapter shadow-copies the assembly, so the project folder cannot be found from
    ///     the assembly location — the compile-time source path of this file can, always
    /// </summary>
    private static string GetSourceFixturePath([System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "")
    {
        var directory = new DirectoryInfo(Path.GetDirectoryName(sourceFilePath)!) ;
        while (directory != null
               && ! File.Exists(Path.Combine(directory.FullName,
                   "Sonny.Application.Tests.csproj"))) {
            directory = directory.Parent ;
        }

        Assert.IsNotNull(directory,
            $"Could not locate the test project directory above {sourceFilePath}") ;

        return Path.Combine(directory!.FullName,
            "Resources",
            "RevitFiles",
            FixtureFileName) ;
    }

    private static string FindFile(string rootFolder,
        string fileName)
    {
        var match = Directory.EnumerateFiles(rootFolder,
                fileName,
                SearchOption.AllDirectories)
            .FirstOrDefault() ;
        Assert.IsNotNull(match,
            $"{fileName} not found under {rootFolder}") ;

        return match! ;
    }

    private void CopyFixtureToOutput(string targetPath)
    {
        // Also drop it next to the running assembly so a test run in this same output can see it
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

    #endregion
}
