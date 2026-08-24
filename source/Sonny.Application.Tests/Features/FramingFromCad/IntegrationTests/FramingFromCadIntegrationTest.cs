using System.Collections.Generic ;
using System.Linq ;
using Autodesk.Revit.DB ;
using Autodesk.Revit.DB.Structure ;
using NUnit.Framework ;
using Sonny.Application.Domain.Entities ;
using Sonny.Application.Domain.Entities.FramingFromCad.Contexts ;
using Sonny.Application.Domain.Entities.FramingFromCad.Models ;
using Sonny.Application.Domain.Entities.FramingFromCad.Services ;
using Sonny.Application.Domain.Services ;
using Sonny.Application.Infrastructure.Revit.Services ;
using Sonny.Application.UseCases.FramingFromCad.Implements ;
using Sonny.RevitExtensions.Extensions ;

namespace Sonny.Application.Tests.Features.FramingFromCad.IntegrationTests ;

/// <summary>
///     Integration tests for FramingFromCad against a fixture built from a **real Revit-exported DWG**.
///     Every expected number in <see cref="FramingFromCadFixtureFacts" /> was measured by running the
///     feature over that drawing, and the builder re-verifies them before saving the fixture — so these
///     tests are characterization tests of the ported behaviour on production-shaped input, not of a
///     hand-tidied drawing.
///     <para>
///         Two of the numbers look wrong and are not: section 300 finds 30 pairs rather than 18 (twelve are
///         pairs of end-cap strokes), and turning single-line mode on loses all three 350 beams to the 300
///         section's consumption. Both are pinned deliberately — read the Contract before "fixing" either.
///     </para>
/// </summary>
[TestFixture]
public class FramingFromCadIntegrationTest : SonnyDocumentTestBase
{
    private const string TestRevitFileName = "Test_V2023_FramingFromCad.rvt" ;
    private const double FeetPerMillimeter = 1 / 304.8 ;
    private const double ZOffsetMm = 100 ;

    /// <summary>
    ///     Matching a created beam back to the stroke pair it came from. The drawing has beams as short as
    ///     300 mm, so the tolerance must stay well below that
    /// </summary>
    private const double MatchToleranceFeet = 1.0 * FeetPerMillimeter ;

    private FakeMessageService _messageService = null! ;

    protected override string? DocumentFilePath => GetTestRevitFilePath(TestRevitFileName) ;

    protected override void OnSetup()
    {
        base.OnSetup() ;

        Host.Start() ;
        Host.GetService<IUIDocumentProvider>()
            .SetUIDocument(UIDocument!) ;
    }

    /// <summary>
    ///     Extraction only, so it mutates nothing and stays valid whatever order the tests run in.
    ///     Pins both single-line modes, including the beam loss that switching it on causes
    /// </summary>
    [Test]
    public void FramingFromCad_ExtractsPairsFromTheRealDrawing()
    {
        var extractor = Host.GetService<IFramingDataExtractor>() ;

        var withoutSingleLines = extractor.Extract(BuildContext(false)) ;
        Log($"single-line OFF: {withoutSingleLines.Pairs.Count} pairs, "
            + $"{withoutSingleLines.SingleLines.Count} single strokes") ;

        Assert.Multiple(() => {
            Assert.AreEqual(FramingFromCadFixtureFacts.TotalPairCount,
                withoutSingleLines.Pairs.Count,
                "Total pairs with single-line mode off") ;
            Assert.AreEqual(0,
                withoutSingleLines.SingleLines.Count,
                "Single strokes are only collected when the mode is on") ;

            AssertPairsForSection(withoutSingleLines,
                FramingFromCadFixtureFacts.NarrowWidthMm,
                FramingFromCadFixtureFacts.NarrowPairCount) ;
            AssertPairsForSection(withoutSingleLines,
                FramingFromCadFixtureFacts.MediumWidthMm,
                FramingFromCadFixtureFacts.MediumPairCount) ;
            AssertPairsForSection(withoutSingleLines,
                FramingFromCadFixtureFacts.WideWidthMm,
                FramingFromCadFixtureFacts.WidePairCount) ;
            AssertPairsForSection(withoutSingleLines,
                FramingFromCadFixtureFacts.WidestWidthMm,
                FramingFromCadFixtureFacts.WidestPairCount) ;
        }) ;

        // Twelve of the 300 pairs are end caps pairing with each other, so their axis is only a few
        // hundred millimetres — nothing filters them out, and beams do get built on them
        var shortMediumAxes = withoutSingleLines.Pairs
            .Where(pair => pair.Section.WidthDisplay == FramingFromCadFixtureFacts.MediumWidthMm)
            .Count(pair => AxisLengthMm(pair) < 1000) ;
        Assert.AreEqual(12,
            shortMediumAxes,
            "End-cap pairs among the 300 mm section: axis shorter than a metre") ;

        var withSingleLines = extractor.Extract(BuildContext(true)) ;
        Log($"single-line ON : {withSingleLines.Pairs.Count} pairs, "
            + $"{withSingleLines.SingleLines.Count} single strokes") ;

        Assert.Multiple(() => {
            Assert.AreEqual(FramingFromCadFixtureFacts.TotalPairCountWithSingleLineMode,
                withSingleLines.Pairs.Count,
                "Total pairs with single-line mode on") ;
            Assert.AreEqual(FramingFromCadFixtureFacts.SingleLineCountWithSingleLineMode,
                withSingleLines.SingleLines.Count,
                "Leftover strokes with single-line mode on") ;

            // The point of this assertion: switching the mode on silently costs three beams, because
            // the 300 section consumed the strokes the 350 section needed
            AssertPairsForSection(withSingleLines,
                FramingFromCadFixtureFacts.WideWidthMm,
                FramingFromCadFixtureFacts.WidePairCountWithSingleLineMode) ;
        }) ;

        Assert.AreEqual(FramingFromCadFixtureFacts.WidePairCount
                        - FramingFromCadFixtureFacts.WidePairCountWithSingleLineMode,
            FramingFromCadFixtureFacts.TotalPairCount
            - FramingFromCadFixtureFacts.TotalPairCountWithSingleLineMode,
            "The whole difference between the two modes must be the 350 section, or the drawing changed") ;
    }

    /// <summary>
    ///     Creation with single-line mode off: 43 beams across four sections, two of which reuse a type the
    ///     project already had and two of which cause a type to be duplicated
    /// </summary>
    [Test]
    public void FramingFromCad_CreatesBeamsFromPairedStrokes()
    {
        var document = Document! ;
        var context = BuildContext(false) ;

        var extraction = Host.GetService<IFramingDataExtractor>()
            .Extract(context) ;

        var beamsBefore = GetBeamIds(document) ;

        BuildInteractor()
            .Execute(context)
            .GetAwaiter()
            .GetResult() ;

        var createdBeams = GetBeamIds(document)
            .Except(beamsBefore)
            .Select(id => (FamilyInstance)document.GetElement(id))
            .ToList() ;

        Log($"Created {createdBeams.Count} beams. Messages: {string.Join(" | ", _messageService.Messages)}") ;

        Assert.AreEqual(FramingFromCadFixtureFacts.TotalPairCount,
            createdBeams.Count,
            "One beam per extracted pair — a shortfall means the silent per-pair skip fired") ;

        AssertTypesAreAsSpecified(createdBeams) ;
        AssertPlacement(createdBeams,
            document) ;
        AssertJustification(extraction,
            createdBeams) ;
    }

    /// <summary>
    ///     Two sections must reuse the types the placeholder already had; two must have been duplicated,
    ///     named without spaces or a unit suffix
    /// </summary>
    private static void AssertTypesAreAsSpecified(List<FamilyInstance> createdBeams)
    {
        var typeNames = createdBeams.Select(beam => beam.Symbol.Name)
            .ToList() ;

        Assert.Multiple(() => {
            Assert.AreEqual(FramingFromCadFixtureFacts.NarrowPairCount,
                typeNames.Count(name => name == FramingFromCadFixtureFacts.GeneratedNarrowTypeName),
                $"Beams on the duplicated {FramingFromCadFixtureFacts.GeneratedNarrowTypeName}") ;
            Assert.AreEqual(FramingFromCadFixtureFacts.MediumPairCount,
                typeNames.Count(name => name == FramingFromCadFixtureFacts.ExistingMediumTypeName),
                $"Beams on the pre-existing {FramingFromCadFixtureFacts.ExistingMediumTypeName}") ;
            Assert.AreEqual(FramingFromCadFixtureFacts.WidePairCount,
                typeNames.Count(name => name == FramingFromCadFixtureFacts.GeneratedWideTypeName),
                $"Beams on the duplicated {FramingFromCadFixtureFacts.GeneratedWideTypeName}") ;
            Assert.AreEqual(FramingFromCadFixtureFacts.WidestPairCount,
                typeNames.Count(name => name == FramingFromCadFixtureFacts.ExistingWidestTypeName),
                $"Beams on the pre-existing {FramingFromCadFixtureFacts.ExistingWidestTypeName}") ;
        }) ;

        AssertGeneratedTypeDimensions(createdBeams,
            FramingFromCadFixtureFacts.GeneratedNarrowTypeName,
            FramingFromCadFixtureFacts.NarrowWidthMm,
            FramingFromCadFixtureFacts.NarrowHeightMm) ;
        AssertGeneratedTypeDimensions(createdBeams,
            FramingFromCadFixtureFacts.GeneratedWideTypeName,
            FramingFromCadFixtureFacts.WideWidthMm,
            FramingFromCadFixtureFacts.WideHeightMm) ;
    }

    private static void AssertGeneratedTypeDimensions(List<FamilyInstance> createdBeams,
        string typeName,
        double widthMm,
        double heightMm)
    {
        var symbol = createdBeams.Select(beam => beam.Symbol)
            .First(candidate => candidate.Name == typeName) ;

        Assert.Multiple(() => {
            Assert.AreEqual(widthMm * FeetPerMillimeter,
                symbol.LookupParameter(FramingFromCadFixtureFacts.WidthParameterName)
                    .AsDouble(),
                0.001,
                $"{typeName} width") ;
            Assert.AreEqual(heightMm * FeetPerMillimeter,
                symbol.LookupParameter(FramingFromCadFixtureFacts.HeightParameterName)
                    .AsDouble(),
                0.001,
                $"{typeName} height") ;
        }) ;
    }

    /// <summary>
    ///     Every beam sits on the reference level with the offset carried by Z_OFFSET_VALUE alone, both end
    ///     elevations zeroed, and both ends set to disallow join
    /// </summary>
    private static void AssertPlacement(List<FamilyInstance> createdBeams,
        Document document)
    {
        var levelId = document.GetAllElements<Level>()
            .OrderBy(level => level.Elevation)
            .First()
            .Id ;

        foreach (var beam in createdBeams) {
            Assert.Multiple(() => {
                Assert.AreEqual(levelId,
                    beam.get_Parameter(BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM)
                        .AsElementId(),
                    $"Beam {beam.Id} reference level") ;
                Assert.AreEqual(ZOffsetMm * FeetPerMillimeter,
                    beam.get_Parameter(BuiltInParameter.Z_OFFSET_VALUE)
                        .AsDouble(),
                    0.0001,
                    $"Beam {beam.Id} z offset") ;
                Assert.AreEqual(0,
                    beam.get_Parameter(BuiltInParameter.STRUCTURAL_BEAM_END0_ELEVATION)
                        .AsDouble(),
                    0.0001,
                    $"Beam {beam.Id} start elevation") ;
                Assert.AreEqual(0,
                    beam.get_Parameter(BuiltInParameter.STRUCTURAL_BEAM_END1_ELEVATION)
                        .AsDouble(),
                    0.0001,
                    $"Beam {beam.Id} end elevation") ;
                Assert.IsFalse(StructuralFramingUtils.IsJoinAllowedAtEnd(beam,
                        0),
                    $"Beam {beam.Id} should disallow join at start") ;
                Assert.IsFalse(StructuralFramingUtils.IsJoinAllowedAtEnd(beam,
                        1),
                    $"Beam {beam.Id} should disallow join at end") ;
            }) ;
        }
    }

    /// <summary>
    ///     The justification rule, recomputed from the model rather than hard-coded: a paired beam faces the
    ///     same way as its stored normal exactly when it is justified right. This is what proves the second
    ///     transaction ran — skip it and every beam keeps the family's default instead
    /// </summary>
    private void AssertJustification(FramingExtractionResult extraction,
        List<FamilyInstance> createdBeams)
    {
        var matched = 0 ;

        foreach (var pair in extraction.Pairs) {
            if (createdBeams.FirstOrDefault(candidate => MatchesAxis(candidate,
                    pair.Start,
                    pair.End)) is not { } beam) {
                continue ;
            }

            matched++ ;

            var expected = new XYZ(pair.Normal.X,
                    pair.Normal.Y,
                    pair.Normal.Z)
                .IsAlmostEqualTo(beam.FacingOrientation)
                ? 3
                : 0 ;

            Assert.AreEqual(expected,
                beam.get_Parameter(BuiltInParameter.Y_JUSTIFICATION)
                    .AsInteger(),
                $"Beam {beam.Id} justification: right(3) when it faces the paired stroke, left(0) otherwise") ;
        }

        // Several sections pair on the same long stroke, so a beam can match more than one pair by axis;
        // requiring every pair to match at least one beam is the meaningful bound
        Assert.AreEqual(FramingFromCadFixtureFacts.TotalPairCount,
            matched,
            "Every extracted pair should have a created beam on its axis") ;
    }

    /// <summary>
    ///     True when the beam's location curve runs between these two points in plan. Z is ignored: the axis
    ///     is flattened onto the level, so the CAD elevation is deliberately not preserved
    /// </summary>
    private static bool MatchesAxis(Element beam,
        Point3D start,
        Point3D end)
    {
        if (beam.Location is not LocationCurve locationCurve) {
            return false ;
        }

        var beamStart = locationCurve.Curve.GetEndPoint(0) ;
        var beamEnd = locationCurve.Curve.GetEndPoint(1) ;

        return (MatchesInPlan(beamStart,
                    start)
                && MatchesInPlan(beamEnd,
                    end))
               || (MatchesInPlan(beamStart,
                       end)
                   && MatchesInPlan(beamEnd,
                       start)) ;
    }

    private static bool MatchesInPlan(XYZ actual,
        Point3D expected) =>
        System.Math.Abs(actual.X - expected.X) < MatchToleranceFeet
        && System.Math.Abs(actual.Y - expected.Y) < MatchToleranceFeet ;

    private static void AssertPairsForSection(FramingExtractionResult extraction,
        double widthMm,
        int expected) =>
        Assert.AreEqual(expected,
            extraction.Pairs.Count(pair => pair.Section.WidthDisplay == widthMm),
            $"Pairs for the {widthMm:F0} mm section") ;

    private static double AxisLengthMm(FramingPairModel pair) =>
        System.Math.Sqrt(((pair.Start.X - pair.End.X) * (pair.Start.X - pair.End.X))
                         + ((pair.Start.Y - pair.End.Y) * (pair.Start.Y - pair.End.Y)))
        / FeetPerMillimeter ;

    private static List<ElementId> GetBeamIds(Document document) =>
        new FilteredElementCollector(document).OfCategory(BuiltInCategory.OST_StructuralFraming)
            .WhereElementIsNotElementType()
            .ToElementIds()
            .ToList() ;

    private FramingCreationContext BuildContext(bool isCreateForSingleLine)
    {
        var document = Document! ;
        var cadLink = document.GetAllElements<ImportInstance>()
            .Single() ;
        var family = document.GetAllElements<Family>()
            .Single(candidate => candidate.Name == FramingFromCadFixtureFacts.FramingFamilyName) ;
        var level = document.GetAllElements<Level>()
            .OrderBy(candidate => candidate.Elevation)
            .First() ;

        return new FramingCreationContext {
            Settings = new FramingFromCadSettings {
                SelectedCadLinkId = cadLink.UniqueId,
                SelectedLayer = FramingFromCadFixtureFacts.BeamLayerName,
                SelectedFamilyId = family.UniqueId,
                WidthParameter = FramingFromCadFixtureFacts.WidthParameterName,
                HeightParameter = FramingFromCadFixtureFacts.HeightParameterName,
                ReferenceLevelId = level.UniqueId,
                ZOffsetDisplay = ZOffsetMm,
                AllSectionsText = FramingFromCadFixtureFacts.AllSectionsText,
                IsCreateForSingleLine = isCreateForSingleLine,
                IsDisallowJoin = true,
            },
            Sections = [
                NewSection(FramingFromCadFixtureFacts.NarrowWidthMm,
                    FramingFromCadFixtureFacts.NarrowHeightMm),
                NewSection(FramingFromCadFixtureFacts.MediumWidthMm,
                    FramingFromCadFixtureFacts.MediumHeightMm),
                NewSection(FramingFromCadFixtureFacts.WideWidthMm,
                    FramingFromCadFixtureFacts.WideHeightMm),
                NewSection(FramingFromCadFixtureFacts.WidestWidthMm,
                    FramingFromCadFixtureFacts.WidestHeightMm),
            ],
            ZOffset = ZOffsetMm * FeetPerMillimeter,
            MinimumSizeDisplay = 1.0,
        } ;
    }

    private static BeamSection NewSection(double widthMm,
        double heightMm) =>
        new() {
            WidthDisplay = widthMm,
            HeightDisplay = heightMm,
            Width = widthMm * FeetPerMillimeter,
            Height = heightMm * FeetPerMillimeter,
            SourceText = $"{widthMm:F0}x{heightMm:F0}",
        } ;

    /// <summary>
    ///     The real interactor over the real Revit ports, with the async runner and the two UI services
    ///     replaced so the run stays headless
    /// </summary>
    private FramingFromCadInteractor BuildInteractor()
    {
        _messageService = new FakeMessageService() ;

        return new FramingFromCadInteractor(Host.GetService<IFramingDataExtractor>(),
            Host.GetService<IBeamCreator>(),
            Host.GetService<IBeamJustificationAdjuster>(),
            Host.GetService<IResourceHelper>(),
            Host.GetService<ITransactionManagerFactory>(),
            new FakeProgressReporter(),
            _messageService,
            Host.GetService<IElementSelector>(),
            new ImmediateRevitTaskRunner()) ;
    }
}
