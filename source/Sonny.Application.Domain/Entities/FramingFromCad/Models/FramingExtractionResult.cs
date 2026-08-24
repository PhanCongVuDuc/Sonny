namespace Sonny.Application.Domain.Entities.FramingFromCad.Models ;

/// <summary>
///     What one pass over the CAD layer found: the paired beams, section by section in the order the
///     user typed them, plus the strokes nothing paired with
/// </summary>
public class FramingExtractionResult
{
    /// <summary>
    ///     Beams recognised from pairs of parallel strokes, grouped by section in typed order
    /// </summary>
    public List<FramingPairModel> Pairs { get ; set ; } = [] ;

    /// <summary>
    ///     Strokes left after every section consumed its pairs, with the stroke lengths that equal one of
    ///     the beam widths already removed — those are the short strokes capping the ends of a beam
    ///     outline, not beams of their own. Empty unless <c>IsCreateForSingleLine</c> is on
    /// </summary>
    public List<SingleLineFramingModel> SingleLines { get ; set ; } = [] ;
}
