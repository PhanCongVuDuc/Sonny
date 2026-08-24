using Sonny.Application.Domain.Entities.FramingFromCad.Models ;
using Sonny.Application.Domain.Entities.FramingFromCad.Services ;
using Sonny.Application.Infrastructure.Revit.Services ;

namespace Sonny.Application.Infrastructure.Features.FramingFromCad.Implements ;

/// <summary>
///     The second pass: justify each paired beam to the side its partner stroke sits on.
///     Runs in its own transaction because <c>FacingOrientation</c> reports a stale value until the
///     instance has been regenerated, which only happens once the creating transaction commits. The
///     comparison is left to the Revit API's own <c>IsAlmostEqualTo</c> so it stays bit-identical to the
///     original implementation
/// </summary>
public class BeamJustificationAdjuster(IRevitDocument revitDocument) : IBeamJustificationAdjuster
{
    public void Adjust(IReadOnlyList<CreatedPairedBeam> beams)
    {
        var document = revitDocument.Document ;

        foreach (var beam in beams) {
            if (document.GetElement(beam.UniqueId) is not FamilyInstance instance) {
                continue ;
            }

            // Facing the same way as the normal means the partner stroke is on the beam's right
            var justification = beam.Normal.ToXyz()
                .IsAlmostEqualTo(instance.FacingOrientation)
                ? BeamJustification.Right
                : BeamJustification.Left ;

            instance.get_Parameter(BuiltInParameter.Y_JUSTIFICATION)
                ?.Set(justification) ;
        }
    }
}
