namespace Sonny.Application.Domain.Entities.FramingFromCad.Models ;

/// <summary>
///     A beam recognised from a lone CAD stroke, built only when <c>IsCreateForSingleLine</c> is on.
///     It carries no section: the size comes from whichever symbol the paired pass resolved first, so a
///     run whose paired pass resolved nothing has nothing to build these with
/// </summary>
public class SingleLineFramingModel
{
    /// <summary>
    ///     Start of the beam axis, in feet
    /// </summary>
    public Point3D Start { get ; set ; } = null! ;

    /// <summary>
    ///     End of the beam axis, in feet
    /// </summary>
    public Point3D End { get ; set ; } = null! ;
}
