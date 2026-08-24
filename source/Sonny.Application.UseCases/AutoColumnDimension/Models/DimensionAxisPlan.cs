using Sonny.Application.Domain.Entities ;

namespace Sonny.Application.UseCases.AutoColumnDimension.Models ;

/// <summary>
///     One axis of the dimension plan for a column: the measuring direction, the direction the
///     dimension line is offset along, and the grid to reference — or null when no grid applies.
/// </summary>
public class DimensionAxisPlan(
    Point3D direction,
    Point3D offsetDirection,
    string? gridUniqueId)
{
    public Point3D Direction { get ; } = direction ;
    public Point3D OffsetDirection { get ; } = offsetDirection ;
    public string? GridUniqueId { get ; } = gridUniqueId ;
}
