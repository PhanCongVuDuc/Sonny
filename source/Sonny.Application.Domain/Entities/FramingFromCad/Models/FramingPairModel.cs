namespace Sonny.Application.Domain.Entities.FramingFromCad.Models ;

/// <summary>
///     One beam recognised from a pair of parallel CAD strokes: the longer stroke becomes the beam's
///     centre line, and the direction across to the shorter one decides which side the beam is justified
///     to once Revit has computed its facing orientation
/// </summary>
public class FramingPairModel
{
    /// <summary>
    ///     Start of the beam axis, in feet
    /// </summary>
    public Point3D Start { get ; set ; } = null! ;

    /// <summary>
    ///     End of the beam axis, in feet
    /// </summary>
    public Point3D End { get ; set ; } = null! ;

    /// <summary>
    ///     Unit direction from the axis stroke across to the paired stroke. Compared against the created
    ///     instance's <c>FacingOrientation</c> in a second transaction, because that property is only
    ///     trustworthy after the beam has been regenerated
    /// </summary>
    public Vector3D Normal { get ; set ; } = null! ;

    /// <summary>
    ///     The section this beam was matched for
    /// </summary>
    public BeamSection Section { get ; set ; } = null! ;
}
