namespace Sonny.Application.UseCases.AutoColumnDimension.Models ;

/// <summary>
///     Everything the AutoColumnDimension interactor needs from the active view, read in one pass:
///     the view axes, the valid structural columns, and the grid candidates.
/// </summary>
public class ActiveViewGeometry(
    ViewGeometryData view,
    List<ColumnGeometryData> columns,
    List<GridCandidate> grids)
{
    public ViewGeometryData View { get ; } = view ;
    public List<ColumnGeometryData> Columns { get ; } = columns ;
    public List<GridCandidate> Grids { get ; } = grids ;
}
