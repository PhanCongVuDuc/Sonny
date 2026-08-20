using Sonny.Application.Domain.Entities.ColumnFromCad.Models ;

namespace Sonny.Application.Domain.Entities.ColumnFromCad ;

/// <summary>
///     Shape-detection rules for column outlines extracted from a CAD link: what counts as a
///     rectangle, and what counts as a circle. Moved verbatim from the Infrastructure extractors
///     (ADR 0001) so the decisions are unit-testable without Revit — including their oddities,
///     which are preserved on purpose.
/// </summary>
public static class ColumnShapeDetector
{
    /// <summary>
    ///     Distance tolerance (feet) for the equidistance test that recognises a circle
    /// </summary>
    public const double CircleTolerance = 1e-4 ;

    /// <summary>
    ///     A closed curve loop with exactly this many curves is treated as a rectangle
    /// </summary>
    public const int RectangleCurveCount = 4 ;

    /// <summary>
    ///     A polyline with exactly this many coordinates (closed: last equals first) is a rectangle
    /// </summary>
    public const int RectanglePolylineCoordinateCount = 5 ;

    /// <summary>
    ///     Decides whether a curve loop outlines a rectangular column
    /// </summary>
    /// <param name="curveCount">Number of curves in the loop</param>
    /// <returns>True when the loop is rectangular</returns>
    public static bool IsRectangleLoop(int curveCount) => curveCount == RectangleCurveCount ;

    /// <summary>
    ///     Decides whether a polyline outlines a rectangular column
    /// </summary>
    /// <param name="coordinateCount">Number of coordinates of the polyline</param>
    /// <returns>True when the polyline is rectangular</returns>
    public static bool IsRectanglePolyline(int coordinateCount) =>
        coordinateCount == RectanglePolylineCoordinateCount ;

    /// <summary>
    ///     Tries to recognise a circular column from the tessellated boundary points of a curve
    ///     loop: a candidate circle is fitted through three of the points and accepted when every
    ///     point is equidistant from its center within <see cref="CircleTolerance" />.
    /// </summary>
    /// <param name="curveCount">Number of curves in the loop (rectangles are skipped)</param>
    /// <param name="points">Distinct tessellated boundary points of the loop</param>
    /// <returns>Circular column model, or null when the points do not form a circle</returns>
    public static CircularColumnModel? TryDetectCircle(int curveCount,
        IReadOnlyList<Point3D> points)
    {
        // Rectangles (4 curves) are handled by the rectangular path
        if (IsRectangleLoop(curveCount)) {
            return null ;
        }

        if (points.Count < 3) {
            return null ;
        }

        // Preserved as-is from the original extractor: the candidate circle is fitted through
        // points[1], [3] and [2], so a loop tessellating to exactly 3 points throws — exactly as
        // Arc.Create(points[1], points[3], points[2]) did before the move
        var center = Circumcenter(points[1],
            points[3],
            points[2]) ;

        var referenceDistance = Distance(points[0],
            center) ;

        var allPointsSameDistance = points.All(x =>
        {
            var abs = Math.Abs(Distance(x,
                center) - referenceDistance) ;
            return abs < CircleTolerance ;
        }) ;

        if (! allPointsSameDistance) {
            return null ;
        }

        var diameter = 2 * Distance(points[1],
            center) ;
        return new CircularColumnModel(diameter,
            center) ;
    }

    /// <summary>
    ///     Circumcenter of the triangle (start, end, pointOnArc) — the center Arc.Create would give
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the points are collinear</exception>
    private static Point3D Circumcenter(Point3D start,
        Point3D end,
        Point3D pointOnArc)
    {
        var ax = start.X - pointOnArc.X ;
        var ay = start.Y - pointOnArc.Y ;
        var az = start.Z - pointOnArc.Z ;
        var bx = end.X - pointOnArc.X ;
        var by = end.Y - pointOnArc.Y ;
        var bz = end.Z - pointOnArc.Z ;

        var crossX = ay * bz - az * by ;
        var crossY = az * bx - ax * bz ;
        var crossZ = ax * by - ay * bx ;
        var crossLengthSquared = crossX * crossX + crossY * crossY + crossZ * crossZ ;

        if (crossLengthSquared < 1e-24) {
            // Arc.Create threw Revit's ArgumentsInconsistentException here; the run still ends
            // with the extraction failing loudly rather than silently dropping the outline
            throw new InvalidOperationException("Cannot fit a circle through collinear points.") ;
        }

        var aLengthSquared = ax * ax + ay * ay + az * az ;
        var bLengthSquared = bx * bx + by * by + bz * bz ;

        // offset = ((|a|²·b − |b|²·a) × (a×b)) / (2·|a×b|²)
        var dx = aLengthSquared * bx - bLengthSquared * ax ;
        var dy = aLengthSquared * by - bLengthSquared * ay ;
        var dz = aLengthSquared * bz - bLengthSquared * az ;

        var offsetX = (dy * crossZ - dz * crossY) / (2 * crossLengthSquared) ;
        var offsetY = (dz * crossX - dx * crossZ) / (2 * crossLengthSquared) ;
        var offsetZ = (dx * crossY - dy * crossX) / (2 * crossLengthSquared) ;

        return new Point3D(pointOnArc.X + offsetX,
            pointOnArc.Y + offsetY,
            pointOnArc.Z + offsetZ) ;
    }

    private static double Distance(Point3D first,
        Point3D second)
    {
        var dx = second.X - first.X ;
        var dy = second.Y - first.Y ;
        var dz = second.Z - first.Z ;
        return Math.Sqrt(dx * dx + dy * dy + dz * dz) ;
    }
}
