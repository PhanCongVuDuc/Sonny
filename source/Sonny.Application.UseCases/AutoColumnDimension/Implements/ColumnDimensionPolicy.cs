using Sonny.Application.Domain.Entities ;
using Sonny.Application.UseCases.AutoColumnDimension.Models ;

namespace Sonny.Application.UseCases.AutoColumnDimension.Implements ;

/// <summary>
///     The geometric decision core of AutoColumnDimension, moved verbatim from the Infrastructure
///     ColumnDimensionContext (ADR 0001): which directions a column is dimensioned along, when a
///     grid lookup is skipped, and which grid is "nearest". Pure — no Revit type, no mock needed.
/// </summary>
public static class ColumnDimensionPolicy
{
    /// <summary>
    ///     Mirrors ToleranceConstants.GeneralTolerance of the wrapper extensions (component-wise,
    ///     strict less-than), so parallelism decisions stay identical after the move
    /// </summary>
    private const double ParallelTolerance = 0.001 ;

    private static readonly Point3D s_basisZ = new(0,
        0,
        1) ;

    /// <summary>
    ///     Decides the dimension plan for one column, or null when the column has no bounding box
    ///     in this view (the documented silent skip).
    ///     The grid lookup is deliberately crossed: the first axis looks up its grid using the
    ///     second direction and vice versa — a dimension measuring along one axis references the
    ///     grid running along the other. Do not "fix" the argument order.
    ///     In a non-plan view an axis parallel to Z gets no grid: a vertical axis has no meaningful
    ///     grid to dimension against.
    /// </summary>
    /// <param name="column">Column geometry read from the view</param>
    /// <param name="view">Active view axes</param>
    /// <param name="grids">Grid candidates visible in the view</param>
    /// <returns>The plan for up to two dimensions, or null to skip the column</returns>
    public static ColumnDimensionPlan? CreatePlan(ColumnGeometryData column,
        ViewGeometryData view,
        IReadOnlyList<GridCandidate> grids)
    {
        if (column.BoundingBoxMin is not { } min
            || column.BoundingBoxMax is not { } max) {
            return null ;
        }

        var midPoint = new Point3D(0.5 * (min.X + max.X),
            0.5 * (min.Y + max.Y),
            0.5 * (min.Z + max.Z)) ;

        Point3D firstDirection ;
        Point3D secondDirection ;
        string? firstGridUniqueId = null ;
        string? secondGridUniqueId = null ;

        if (view.IsPlan) {
            firstDirection = column.HandOrientation ;
            secondDirection = column.FacingOrientation ;

            firstGridUniqueId = FindNearestGrid(grids,
                secondDirection,
                midPoint,
                firstDirection) ;

            secondGridUniqueId = FindNearestGrid(grids,
                firstDirection,
                midPoint,
                secondDirection) ;
        }
        else {
            firstDirection = view.UpDirection ;
            secondDirection = view.RightDirection ;

            if (! IsParallel(firstDirection,
                    s_basisZ)) {
                firstGridUniqueId = FindNearestGrid(grids,
                    view.ViewDirection,
                    midPoint,
                    firstDirection) ;
            }

            if (! IsParallel(secondDirection,
                    s_basisZ)) {
                secondGridUniqueId = FindNearestGrid(grids,
                    view.ViewDirection,
                    midPoint,
                    secondDirection) ;
            }
        }

        return new ColumnDimensionPlan(max,
            new DimensionAxisPlan(firstDirection,
                secondDirection,
                firstGridUniqueId),
            new DimensionAxisPlan(secondDirection,
                firstDirection,
                secondGridUniqueId)) ;
    }

    /// <summary>
    ///     Nearest grid parallel to gridDirection, ordered by the distance from midPoint to the
    ///     grid's start point measured along productDirection — the same plane-distance the
    ///     original GridFinder used
    /// </summary>
    private static string? FindNearestGrid(IReadOnlyList<GridCandidate> grids,
        Point3D gridDirection,
        Point3D midPoint,
        Point3D productDirection)
    {
        return grids.Where(x => IsParallel(x.Direction,
                gridDirection))
            .OrderBy(x => DistanceAlongDirection(midPoint,
                x.StartPoint,
                productDirection))
            .FirstOrDefault()
            ?.UniqueId ;
    }

    private static double DistanceAlongDirection(Point3D point,
        Point3D origin,
        Point3D direction)
    {
        var normal = Normalize(direction) ;
        var dot = normal.X * (point.X - origin.X)
                  + normal.Y * (point.Y - origin.Y)
                  + normal.Z * (point.Z - origin.Z) ;
        return Math.Abs(dot) ;
    }

    private static bool IsParallel(Point3D firstVector,
        Point3D secondVector)
    {
        var first = Normalize(firstVector) ;
        var second = Normalize(secondVector) ;

        return AreAlmostEqual(first,
                   second)
               || AreAlmostEqual(first,
                   Negate(second)) ;
    }

    private static bool AreAlmostEqual(Point3D first,
        Point3D second) =>
        Math.Abs(first.X - second.X) < ParallelTolerance
        && Math.Abs(first.Y - second.Y) < ParallelTolerance
        && Math.Abs(first.Z - second.Z) < ParallelTolerance ;

    private static Point3D Negate(Point3D vector) =>
        new(-vector.X,
            -vector.Y,
            -vector.Z) ;

    private static Point3D Normalize(Point3D vector)
    {
        var length = Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z) ;
        if (length == 0) {
            return new Point3D(0,
                0,
                0) ;
        }

        return new Point3D(vector.X / length,
            vector.Y / length,
            vector.Z / length) ;
    }
}
