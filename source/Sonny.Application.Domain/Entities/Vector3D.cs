namespace Sonny.Application.Domain.Entities ;

/// <summary>
///     Represents a direction in 3D space. Distinct from <see cref="Point3D" /> on purpose: a direction
///     that gets compared against another direction is not a location, and mixing the two makes the
///     comparison sites unreadable.
///     No equality helper here by design — direction comparisons happen in Infrastructure through the
///     Revit API's own <c>XYZ.IsAlmostEqualTo</c>, so a ported feature keeps the exact comparison the
///     original made instead of a reimplementation that drifts
/// </summary>
public class Vector3D(double x, double y, double z)
{
    /// <summary>
    ///     Gets the X component
    /// </summary>
    public double X { get ; } = x ;

    /// <summary>
    ///     Gets the Y component
    /// </summary>
    public double Y { get ; } = y ;

    /// <summary>
    ///     Gets the Z component
    /// </summary>
    public double Z { get ; } = z ;
}
