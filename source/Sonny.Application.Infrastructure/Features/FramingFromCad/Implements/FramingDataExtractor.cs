using Sonny.Application.Domain.Entities.FramingFromCad.Contexts ;
using Sonny.Application.Domain.Entities.FramingFromCad.Models ;
using Sonny.Application.Domain.Entities.FramingFromCad.Services ;
using Sonny.Application.Domain.Services ;
using Sonny.Application.Infrastructure.Revit.Services ;
using Sonny.RevitExtensions ;
using Sonny.RevitExtensions.Extensions ;
using Sonny.RevitExtensions.Extensions.GeometryObjects.Curves ;

namespace Sonny.Application.Infrastructure.Features.FramingFromCad.Implements ;

/// <summary>
///     Reads the CAD layer and works out which beams its strokes describe. Ported from the section loop of
///     the original implementation, including two behaviours that look like defects and are not:
///     <list type="bullet">
///         <item>
///             The stroke pool is only reduced between sections when <c>IsCreateForSingleLine</c> is on.
///             With it off, two sections of the same width both find the same pairs, so the run creates
///             two beams on each of those strokes
///         </item>
///         <item>
///             A group of three or more parallel strokes yields one beam from the longest stroke and the
///             first of the rest; the remaining strokes in that group are dropped without a word
///         </item>
///     </list>
/// </summary>
public class FramingDataExtractor(
    IRevitDocument revitDocument,
    IPoint3DConverter point3DConverter,
    IResourceHelper resourceHelper) : IFramingDataExtractor
{
    public FramingExtractionResult Extract(FramingCreationContext context)
    {
        var settings = context.Settings ;
        var document = revitDocument.Document ;

        if (document.GetElement(settings.SelectedCadLinkId) is not ImportInstance cadInstance) {
            throw new InvalidOperationException(resourceHelper.GetString("MessageCadLinkNotFound")) ;
        }

        var strokes = cadInstance.GetLinesOnLayer(settings.SelectedLayer ?? string.Empty) ;

        var pairs = new List<FramingPairModel>() ;
        var sectionWidths = new List<double>() ;

        foreach (var section in context.Sections) {
            // Collected for every section, used only by the leftover filter below
            sectionWidths.Add(section.Width) ;

            var groups = strokes.GetParallelCurveGroups(section.Width) ;

            if (settings.IsCreateForSingleLine) {
                // Consume the paired strokes so they cannot become single-line beams as well. Only
                // done in this mode — that is what makes two same-width sections duplicate beams when
                // the mode is off
                foreach (var stroke in groups.SelectMany(group => group)) {
                    strokes.Remove(stroke) ;
                }
            }

            foreach (var group in groups) {
                if (BuildPair(group,
                        section) is not { } pair) {
                    continue ;
                }

                pairs.Add(pair) ;
            }
        }

        var singleLines = new List<SingleLineFramingModel>() ;
        if (settings.IsCreateForSingleLine) {
            singleLines.AddRange(BuildSingleLines(strokes,
                sectionWidths)) ;
        }

        return new FramingExtractionResult {
            Pairs = pairs,
            SingleLines = singleLines,
        } ;
    }

    /// <summary>
    ///     Turns one group of parallel strokes into a beam: the longest stroke is the axis, and the
    ///     direction across to the next stroke in the group decides the justification side later
    /// </summary>
    private FramingPairModel? BuildPair(List<Curve> group,
        BeamSection section)
    {
        var maximumLength = group.Max(curve => curve.Length) ;
        var axis = group.First(curve =>
            Math.Abs(curve.Length - maximumLength) < ToleranceConstants.GeneralTolerance) ;

        if (group.FirstOrDefault(curve => ! ReferenceEquals(curve,
                axis)) is not { } pairedStroke) {
            return null ;
        }

        return new FramingPairModel {
            Start = point3DConverter.FromXyz(axis.GetEndPoint(0)),
            End = point3DConverter.FromXyz(axis.GetEndPoint(1)),
            Normal = axis.GetNormalBetweenParallelCurves(pairedStroke)
                .ToVector3D(),
            Section = section,
        } ;
    }

    /// <summary>
    ///     What is left after every section took its pairs, minus the strokes whose length equals one of
    ///     the beam widths — those are the short strokes capping the end of a beam outline, not beams
    /// </summary>
    private IEnumerable<SingleLineFramingModel> BuildSingleLines(List<Curve> strokes,
        List<double> sectionWidths)
    {
        var remaining = strokes ;
        foreach (var sectionWidth in sectionWidths) {
            remaining = remaining
                .Where(curve => Math.Abs(curve.Length - sectionWidth) > ToleranceConstants.GeneralTolerance)
                .ToList() ;
        }

        return remaining.Select(curve => new SingleLineFramingModel {
            Start = point3DConverter.FromXyz(curve.GetEndPoint(0)),
            End = point3DConverter.FromXyz(curve.GetEndPoint(1)),
        }) ;
    }
}
