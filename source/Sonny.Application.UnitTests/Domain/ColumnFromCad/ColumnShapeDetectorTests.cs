using Sonny.Application.Domain.Entities ;
using Sonny.Application.Domain.Entities.ColumnFromCad ;

namespace Sonny.Application.UnitTests.Domain.ColumnFromCad ;

/// <summary>
///     Characterization tests for <see cref="ColumnShapeDetector" /> — pin the shape rules exactly
///     as they behaved in the Infrastructure extractors before the move (ADR 0001), including the
///     points[3] indexing quirk on 3-point input.
/// </summary>
public class ColumnShapeDetectorTests
{
    [TestCase(4,
        ExpectedResult = true)]
    [TestCase(3,
        ExpectedResult = false)]
    [TestCase(5,
        ExpectedResult = false)]
    public bool IsRectangleLoop_ByCurveCount(int curveCount) =>
        ColumnShapeDetector.IsRectangleLoop(curveCount) ;

    [TestCase(5,
        ExpectedResult = true)]
    [TestCase(4,
        ExpectedResult = false)]
    [TestCase(6,
        ExpectedResult = false)]
    public bool IsRectanglePolyline_ByCoordinateCount(int coordinateCount) =>
        ColumnShapeDetector.IsRectanglePolyline(coordinateCount) ;

    [Test]
    public void TryDetectCircle_RectangleLoop_ReturnsNullWithoutLookingAtPoints()
    {
        // 4 curves = rectangle: skipped before any point is touched, even an empty list
        var result = ColumnShapeDetector.TryDetectCircle(4,
            []) ;

        Assert.That(result,
            Is.Null) ;
    }

    [Test]
    public void TryDetectCircle_FewerThanThreePoints_ReturnsNull()
    {
        var result = ColumnShapeDetector.TryDetectCircle(8,
            [
                new Point3D(0,
                    0,
                    0),
                new Point3D(1,
                    0,
                    0)
            ]) ;

        Assert.That(result,
            Is.Null) ;
    }

    [Test]
    public void TryDetectCircle_ExactlyThreePoints_ThrowsLikeTheOriginalIndexing()
    {
        // The original extractor guarded points.Count >= 3 but then read points[3] — a 3-point
        // loop crashed. Preserved: do not "fix" without changing the contract first.
        var points = OnCircle(2.0,
            0,
            Math.PI / 2,
            Math.PI) ;

        Assert.Throws<ArgumentOutOfRangeException>(() => ColumnShapeDetector.TryDetectCircle(8,
            points)) ;
    }

    [Test]
    public void TryDetectCircle_PointsOnCircle_ReturnsModelWithCenterAndDiameter()
    {
        // 8 points on a circle of radius 1.5 centered at (10, 5, 0)
        var points = OnCircle(1.5,
            0,
            Math.PI / 4,
            Math.PI / 2,
            3 * Math.PI / 4,
            Math.PI,
            5 * Math.PI / 4,
            3 * Math.PI / 2,
            7 * Math.PI / 4) ;

        var result = ColumnShapeDetector.TryDetectCircle(8,
            points) ;

        Assert.That(result,
            Is.Not.Null) ;
        Assert.Multiple(() =>
        {
            Assert.That(result!.Diameter,
                Is.EqualTo(3.0).Within(1e-9)) ;
            Assert.That(result.Center.X,
                Is.EqualTo(10).Within(1e-9)) ;
            Assert.That(result.Center.Y,
                Is.EqualTo(5).Within(1e-9)) ;
        }) ;
    }

    [Test]
    public void TryDetectCircle_PointOffCircleBeyondTolerance_ReturnsNull()
    {
        var points = OnCircle(1.5,
            0,
            Math.PI / 4,
            Math.PI / 2,
            3 * Math.PI / 4,
            Math.PI) ;

        // Push one point off the circle by more than the 1e-4 tolerance
        points[4] = new Point3D(points[4].X + 0.001,
            points[4].Y,
            points[4].Z) ;

        var result = ColumnShapeDetector.TryDetectCircle(8,
            points) ;

        Assert.That(result,
            Is.Null) ;
    }

    [Test]
    public void TryDetectCircle_CollinearFitPoints_Throws()
    {
        // points[1], [3], [2] all on one line — Arc.Create used to throw here too
        var points = new List<Point3D>
        {
            new(0,
                1,
                0),
            new(0,
                0,
                0),
            new(1,
                0,
                0),
            new(2,
                0,
                0)
        } ;

        Assert.Throws<InvalidOperationException>(() => ColumnShapeDetector.TryDetectCircle(8,
            points)) ;
    }

    private static List<Point3D> OnCircle(double radius,
        params double[] angles) =>
        angles.Select(a => new Point3D(10 + radius * Math.Cos(a),
                5 + radius * Math.Sin(a),
                0))
            .ToList() ;
}
