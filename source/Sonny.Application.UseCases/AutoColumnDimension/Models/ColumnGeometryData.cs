using Sonny.Application.Domain.Entities ;

namespace Sonny.Application.UseCases.AutoColumnDimension.Models ;

/// <summary>
///     Revit-free geometry of one structural column in the active view, read by the
///     Infrastructure geometry reader for the dimension policy to decide on. A column whose
///     bounding box could not be resolved in the view carries null bounds and is skipped.
/// </summary>
public class ColumnGeometryData(
    string uniqueId,
    Point3D? boundingBoxMin,
    Point3D? boundingBoxMax,
    Point3D handOrientation,
    Point3D facingOrientation)
{
    public string UniqueId { get ; } = uniqueId ;
    public Point3D? BoundingBoxMin { get ; } = boundingBoxMin ;
    public Point3D? BoundingBoxMax { get ; } = boundingBoxMax ;
    public Point3D HandOrientation { get ; } = handOrientation ;
    public Point3D FacingOrientation { get ; } = facingOrientation ;
}
