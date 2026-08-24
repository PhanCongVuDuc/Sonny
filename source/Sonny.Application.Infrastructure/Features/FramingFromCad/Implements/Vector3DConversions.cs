using Sonny.Application.Domain.Entities ;

namespace Sonny.Application.Infrastructure.Features.FramingFromCad.Implements ;

/// <summary>
///     Converts between the Domain's <see cref="Vector3D" /> and the Revit API's XYZ.
///     Kept local to this feature rather than added to <c>IPoint3DConverter</c>: that interface is shared
///     by three features and widening it is a change to an abstraction others depend on
/// </summary>
internal static class Vector3DConversions
{
    /// <summary>
    ///     Converts a Revit direction to a Domain vector
    /// </summary>
    internal static Vector3D ToVector3D(this XYZ xyz) =>
        new(xyz.X,
            xyz.Y,
            xyz.Z) ;

    /// <summary>
    ///     Converts a Domain vector back to a Revit direction
    /// </summary>
    internal static XYZ ToXyz(this Vector3D vector) =>
        new(vector.X,
            vector.Y,
            vector.Z) ;
}
