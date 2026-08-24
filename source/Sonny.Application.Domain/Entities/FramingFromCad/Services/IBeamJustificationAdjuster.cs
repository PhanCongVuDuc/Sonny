using Sonny.Application.Domain.Entities.FramingFromCad.Models ;

namespace Sonny.Application.Domain.Entities.FramingFromCad.Services ;

/// <summary>
///     Sets <c>Y_JUSTIFICATION</c> on paired beams, in a transaction of its own after the beams have
///     been committed. The split is not cosmetic: <c>FacingOrientation</c> only reports the truth once the
///     instance has been regenerated, so deciding justification inside the creating transaction silently
///     justifies beams to the wrong side
/// </summary>
public interface IBeamJustificationAdjuster
{
    /// <summary>
    ///     Justifies each beam to the side its paired stroke sits on: right when the beam faces the same
    ///     way as the stored normal, left otherwise
    /// </summary>
    /// <param name="beams">The created paired beams with their normals. An empty list is a no-op</param>
    void Adjust(IReadOnlyList<CreatedPairedBeam> beams) ;
}
