using Sonny.Application.Domain.Entities ;

namespace Sonny.Application.UseCases.AutoColumnDimension.Models ;

/// <summary>
///     The dimension policy's decision for one column: up to two dimensions, one per axis, with
///     the point the dimension lines pass near (the bounding box maximum).
/// </summary>
public class ColumnDimensionPlan(
    Point3D maxPoint,
    DimensionAxisPlan firstAxis,
    DimensionAxisPlan secondAxis)
{
    public Point3D MaxPoint { get ; } = maxPoint ;
    public DimensionAxisPlan FirstAxis { get ; } = firstAxis ;
    public DimensionAxisPlan SecondAxis { get ; } = secondAxis ;
}
