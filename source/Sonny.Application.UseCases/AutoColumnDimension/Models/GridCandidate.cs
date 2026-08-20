using Sonny.Application.Domain.Entities ;

namespace Sonny.Application.UseCases.AutoColumnDimension.Models ;

/// <summary>
///     Revit-free description of one grid line in the active view, identified by UniqueId so the
///     plan executor can map the policy's choice back to the Revit element.
/// </summary>
public class GridCandidate(
    string uniqueId,
    Point3D direction,
    Point3D startPoint)
{
    public string UniqueId { get ; } = uniqueId ;
    public Point3D Direction { get ; } = direction ;
    public Point3D StartPoint { get ; } = startPoint ;
}
