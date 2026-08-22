using System.Collections.Generic ;
using System.Linq ;
using Autodesk.Revit.DB ;
using NSubstitute ;
using NUnit.Framework ;
using Serilog ;
using Sonny.Application.Domain.Entities.AutoJoin.Models ;
using Sonny.Application.Domain.Services ;
using Sonny.Application.Infrastructure.Revit.Services ;
using Sonny.Application.UseCases.AutoJoin.Implements ;
using Sonny.Application.UseCases.AutoJoin.Services ;
using Tags = Sonny.Application.Tests.Features.AutoJoin.IntegrationTests.AutoJoinFixtureTags ;

namespace Sonny.Application.Tests.Features.AutoJoin.IntegrationTests ;

/// <summary>
///     Integration tests for AutoJoin against Test_V2023_AutoJoin.rvt (authored by
///     <see cref="AutoJoinFixtureBuilder" /> — one station per case, elements looked up through
///     their Comments tag). Each test drives the real interactor with the real Infrastructure
///     adapters and asserts the join state through JoinGeometryUtils afterwards.
///     Stations are 15 m apart, so joins made by one test cannot reach another test's elements —
///     the tests stay order-independent even though they share the open document.
/// </summary>
[TestFixture]
public class AutoJoinIntegrationTest : SonnyDocumentTestBase
{
    private const string TestRevitFileName = "Test_V2023_AutoJoin.rvt" ;
    private const string FixtureViewName = "AutoJoin 3D" ;
    private IAutoJoinInteractor? _interactor ;

    protected override string? DocumentFilePath => GetTestRevitFilePath(TestRevitFileName) ;

    protected override void OnSetup()
    {
        base.OnSetup() ;

        Host.GetService<IUIDocumentProvider>()
            .SetUIDocument(UIDocument!) ;
        ActivateFixtureView() ;

        // Real scope reader / pair executor / transactions; mocked dialogs and progress so the
        // run never blocks the test process (IsCancelRequested defaults to false)
        var progressReporter = Substitute.For<IProgressReporter>() ;
        var messageService = Substitute.For<IMessageService>() ;
        _interactor = new AutoJoinInteractor(Host.GetService<IAutoJoinScopeReader>(),
            Host.GetService<IAutoJoinPairExecutor>(),
            Host.GetService<IFailingElementIdsTracker>(),
            Host.GetService<ITransactionManagerFactory>(),
            progressReporter,
            messageService,
            Host.GetService<IResourceHelper>(),
            Host.GetService<ILogger>()) ;
    }

    [Test]
    public void RuleBased_BeamRule_JoinsBeamAsCuttingStructuralColumn()
    {
        var beam = GetByTag(Tags.S01Beam) ;
        var column = GetByTag(Tags.S01Column) ;

        RunRuleBased(JoinCategory.Beam,
            JoinCategory.StructuralColumn,
            false,
            false,
            beam) ;

        AssertJoined(beam,
            column,
            "S01: beam and column should be joined") ;
        AssertCutting(beam,
            column,
            "S01: the priority beam should cut the column") ;
    }

    [Test]
    public void RuleBased_ReverseRule_JoinsColumnAsCuttingBeam()
    {
        var beam = GetByTag(Tags.S02Beam) ;
        var column = GetByTag(Tags.S02Column) ;

        RunRuleBased(JoinCategory.Beam,
            JoinCategory.StructuralColumn,
            true,
            false,
            beam) ;

        AssertJoined(beam,
            column,
            "S02: beam and column should be joined") ;
        AssertCutting(column,
            beam,
            "S02: IsReverse should make the column cut the beam") ;
    }

    [Test]
    public void RuleBased_ArchitecturalFloorRule_MatchesNonStructuralFloor()
    {
        var column = GetByTag(Tags.S03Column) ;
        var floor = GetByTag(Tags.S03Floor) ;

        RunRuleBased(JoinCategory.StructuralColumn,
            JoinCategory.ArchitecturalFloor,
            false,
            false,
            column) ;

        AssertJoined(column,
            floor,
            "S03: ArchitecturalFloor should match a plain floor") ;
        AssertCutting(column,
            floor,
            "S03: the priority column should cut the floor") ;
    }

    [Test]
    public void RuleBased_StructuralFloorRule_SkipsNonStructuralFloor()
    {
        var column = GetByTag(Tags.S04Column) ;
        var structuralFloor = GetByTag(Tags.S04StructuralFloor) ;
        var architecturalFloor = GetByTag(Tags.S04ArchitecturalFloor) ;

        RunRuleBased(JoinCategory.StructuralColumn,
            JoinCategory.StructuralFloor,
            false,
            false,
            column) ;

        AssertJoined(column,
            structuralFloor,
            "S04: the structural floor should be joined") ;
        AssertNotJoined(column,
            architecturalFloor,
            "S04: the non-structural floor must not match a StructuralFloor rule") ;
    }

    [Test]
    public void RuleBased_StructuralWallRule_MatchesOnlyStructuralWall()
    {
        var structuralWall = GetByTag(Tags.S05StructuralWall) ;
        var architecturalWall = GetByTag(Tags.S05ArchitecturalWall) ;
        var floor = GetByTag(Tags.S05Floor) ;

        RunRuleBased(JoinCategory.StructuralWall,
            JoinCategory.ArchitecturalFloor,
            false,
            false,
            structuralWall,
            architecturalWall) ;

        AssertJoined(structuralWall,
            floor,
            "S05: the structural wall should be joined to the floor") ;
        AssertNotJoined(architecturalWall,
            floor,
            "S05: the non-structural wall must not match a StructuralWall rule") ;
        AssertCutting(structuralWall,
            floor,
            "S05: the priority wall should cut the floor") ;
    }

    [Test]
    public void RuleBased_ArchitecturalWallRule_MatchesStructuralWallToo()
    {
        var structuralWall = GetByTag(Tags.S06StructuralWall) ;
        var floor = GetByTag(Tags.S06Floor) ;

        RunRuleBased(JoinCategory.ArchitecturalWall,
            JoinCategory.ArchitecturalFloor,
            false,
            false,
            structuralWall) ;

        // Ported quirk (D1): ArchitecturalWall matches every wall, the structural one included
        AssertJoined(structuralWall,
            floor,
            "S06: an ArchitecturalWall rule should match a structural wall too") ;
    }

    [Test]
    public void RuleBased_FoundationRule_JoinsBeamAsCuttingFoundation()
    {
        var beam = GetByTag(Tags.S07Beam) ;
        var foundation = GetByTag(Tags.S07Foundation) ;

        RunRuleBased(JoinCategory.Beam,
            JoinCategory.Foundation,
            false,
            false,
            beam) ;

        AssertJoined(beam,
            foundation,
            "S07: beam and footing should be joined") ;
        AssertCutting(beam,
            foundation,
            "S07: the priority beam should cut the footing") ;
    }

    [Test]
    public void RuleBased_RoofRule_JoinsColumnAsCuttingRoof()
    {
        var column = GetByTag(Tags.S08Column) ;
        var roof = GetByTag(Tags.S08Roof) ;

        RunRuleBased(JoinCategory.StructuralColumn,
            JoinCategory.Roof,
            false,
            false,
            column) ;

        AssertJoined(column,
            roof,
            "S08: column and roof should be joined") ;
        AssertCutting(column,
            roof,
            "S08: the priority column should cut the roof") ;
    }

    [Test]
    public void RuleBased_CeilingRule_JoinsColumnAsCuttingCeiling()
    {
        var column = GetByTag(Tags.S09Column) ;
        var ceiling = GetByTag(Tags.S09Ceiling) ;

        RunRuleBased(JoinCategory.StructuralColumn,
            JoinCategory.Ceiling,
            false,
            false,
            column) ;

        AssertJoined(column,
            ceiling,
            "S09: column and ceiling should be joined") ;
        AssertCutting(column,
            ceiling,
            "S09: the priority column should cut the ceiling") ;
    }

    [Test]
    public void RuleBased_GenericModelRule_JoinsColumnAsCuttingGenericModel()
    {
        var column = GetByTag(Tags.S10Column) ;
        var genericModel = GetByTag(Tags.S10GenericModel) ;

        RunRuleBased(JoinCategory.StructuralColumn,
            JoinCategory.GenericModel,
            false,
            false,
            column) ;

        AssertJoined(column,
            genericModel,
            "S10: column and generic model should be joined") ;
        AssertCutting(column,
            genericModel,
            "S10: the priority column should cut the generic model") ;
    }

    [Test]
    public void RuleBased_BeamRule_JoinsArchitecturalColumn()
    {
        var beam = GetByTag(Tags.S11Beam) ;
        var column = GetByTag(Tags.S11Column) ;

        RunRuleBased(JoinCategory.Beam,
            JoinCategory.ArchitecturalColumn,
            false,
            false,
            beam) ;

        AssertJoined(beam,
            column,
            "S11: beam and architectural column should be joined") ;
        AssertCutting(beam,
            column,
            "S11: the priority beam should cut the architectural column") ;
    }

    [Test]
    public void RuleBased_JoinWithAll_JoinsEveryIntersectingCandidate()
    {
        var beam = GetByTag(Tags.S12Beam) ;
        var column = GetByTag(Tags.S12Column) ;
        var floor = GetByTag(Tags.S12Floor) ;

        RunRuleBased(JoinCategory.Beam,
            JoinCategory.All,
            false,
            false,
            beam) ;

        AssertJoined(beam,
            column,
            "S12: the All rule should join the beam to the column") ;
        AssertJoined(beam,
            floor,
            "S12: the All rule should join the beam to the floor") ;
    }

    [Test]
    public void RuleBased_BoundingBoxOverlapWithoutSolidIntersection_IsSkipped()
    {
        var column = GetByTag(Tags.S13Column) ;
        var arcWall = GetByTag(Tags.S13ArcWall) ;

        RunRuleBased(JoinCategory.StructuralColumn,
            JoinCategory.ArchitecturalWall,
            false,
            false,
            column) ;

        // F7: the pair passes the bounding-box probe but fails the solid check — silent skip
        AssertNotJoined(column,
            arcWall,
            "S13: a bounding-box-only overlap must not be joined") ;
    }

    [Test]
    public void RuleBased_UnjoinRule_UnjoinsPrejoinedPair()
    {
        var beam = GetByTag(Tags.S14Beam) ;
        var column = GetByTag(Tags.S14Column) ;

        AssertJoined(beam,
            column,
            "S14 precondition: the fixture pair should start joined") ;

        RunRuleBased(JoinCategory.Beam,
            JoinCategory.StructuralColumn,
            false,
            true,
            beam) ;

        AssertNotJoined(beam,
            column,
            "S14: the unjoin rule should unjoin the pair") ;
    }

    [Test]
    public void CutSelectedMode_IntersectingCandidatesCutTheSelectedElement()
    {
        var column = GetByTag(Tags.S15Column) ;
        var beam = GetByTag(Tags.S15Beam) ;
        var genericModel = GetByTag(Tags.S15GenericModel) ;

        RunMode(AutoJoinMode.CutSelectedElements,
            column) ;

        AssertJoined(beam,
            column,
            "S15: the beam should be joined to the selected column") ;
        AssertCutting(beam,
            column,
            "S15: the beam candidate should cut the selected column") ;
        AssertJoined(genericModel,
            column,
            "S15: the generic model should be joined to the selected column") ;
        AssertCutting(genericModel,
            column,
            "S15: the generic model candidate should cut the selected column") ;
    }

    [Test]
    public void CutOtherMode_SelectedElementCutsIntersectingCandidates()
    {
        var column = GetByTag(Tags.S16Column) ;
        var beam = GetByTag(Tags.S16Beam) ;

        RunMode(AutoJoinMode.CutOtherElements,
            column) ;

        AssertJoined(column,
            beam,
            "S16: the selected column should be joined to the beam") ;
        AssertCutting(column,
            beam,
            "S16: the selected column should cut the beam") ;
    }

    #region Helpers

    private void ActivateFixtureView()
    {
        var view = new FilteredElementCollector(Document!).OfClass(typeof( View3D ))
            .Cast<View3D>()
            .First(v => ! v.IsTemplate
                        && v.Name == FixtureViewName) ;
        UIDocument!.ActiveView = view ;
    }

    private Element GetByTag(string tag) =>
        new FilteredElementCollector(Document!).WhereElementIsNotElementType()
            .FirstOrDefault(element => element.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS)
                                           ?.AsString()
                                       == tag)
        ?? throw new AssertionException($"No element tagged '{tag}' in the fixture") ;

    private void RunRuleBased(JoinCategory priorityCategory,
        JoinCategory joinWithCategory,
        bool isReverse,
        bool isUnjoin,
        params Element[] selection)
    {
        Select(selection) ;
        _interactor!.Execute(new AutoJoinInput
        {
            Mode = AutoJoinMode.RuleBased,
            Rules =
            [
                new AutoJoinRule
                {
                    PriorityCategory = priorityCategory,
                    JoinWithCategory = joinWithCategory,
                    IsReverse = isReverse
                }
            ],
            IsUnjoin = isUnjoin
        }) ;
    }

    private void RunMode(AutoJoinMode mode,
        params Element[] selection)
    {
        Select(selection) ;
        _interactor!.Execute(new AutoJoinInput { Mode = mode }) ;
    }

    private void Select(IEnumerable<Element> elements) =>
        UIDocument!.Selection
            .SetElementIds(elements.Select(element => element.Id)
                .ToList()) ;

    private void AssertJoined(Element a,
        Element b,
        string message) =>
        Assert.IsTrue(JoinGeometryUtils.AreElementsJoined(Document!,
                a,
                b),
            message) ;

    private void AssertNotJoined(Element a,
        Element b,
        string message) =>
        Assert.IsFalse(JoinGeometryUtils.AreElementsJoined(Document!,
                a,
                b),
            message) ;

    private void AssertCutting(Element cutter,
        Element cut,
        string message) =>
        Assert.IsTrue(JoinGeometryUtils.IsCuttingElementInJoin(Document!,
                cutter,
                cut),
            message) ;

    #endregion
}
