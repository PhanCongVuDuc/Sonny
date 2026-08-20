using Sonny.Application.Domain.Entities ;

namespace Sonny.Application.UseCases.AutoColumnDimension.Models ;

/// <summary>
///     Revit-free description of the active view: whether it is a plan view, and its axes —
///     everything the dimension policy needs to pick dimensioning directions.
/// </summary>
public class ViewGeometryData(
    string name,
    bool isPlan,
    Point3D upDirection,
    Point3D rightDirection,
    Point3D viewDirection)
{
    public string Name { get ; } = name ;
    public bool IsPlan { get ; } = isPlan ;
    public Point3D UpDirection { get ; } = upDirection ;
    public Point3D RightDirection { get ; } = rightDirection ;
    public Point3D ViewDirection { get ; } = viewDirection ;
}
