using Sonny.Application.Domain.Entities ;
using Sonny.Application.Domain.Entities.ColumnFromCad.Models ;

namespace Sonny.Application.UnitTests.Domain.ColumnFromCad ;

/// <summary>
///     Characterization tests for <see cref="RectangularColumnModel.FromCorners" /> — they pin the
///     quadrant-remapping rotation rule exactly as it behaved in the original
///     ColumnModelFactory.CreateRectangular (ADR 0001). Expected values are derived analytically
///     from that algorithm, not from running the code under test.
/// </summary>
public class RectangularColumnModelFromCornersTests
{
    private const double Tolerance = 1e-9 ;

    [Test]
    public void FromCorners_FewerThanFourCorners_ThrowsArgumentException()
    {
        var corners = new List<Point3D>
        {
            new(0,
                0,
                0),
            new(1,
                0,
                0),
            new(1,
                1,
                0)
        } ;

        Assert.Throws<ArgumentException>(() => RectangularColumnModel.FromCorners(corners)) ;
    }

    [Test]
    public void FromCorners_ShortSideAlongY_RotationIsHalfPi()
    {
        // First side (length 4) along X, second side (length 2) along Y — the short side
        // direction is +Y, whose X component is 0, so no quadrant branch fires
        var model = RectangularColumnModel.FromCorners(Rectangle(new Point3D(0,
                0,
                0),
            4,
            0,
            0,
            2)) ;

        Assert.Multiple(() =>
        {
            Assert.That(model.ShortSide,
                Is.EqualTo(2).Within(Tolerance)) ;
            Assert.That(model.LongSide,
                Is.EqualTo(4).Within(Tolerance)) ;
            Assert.That(model.RotationAngle,
                Is.EqualTo(Math.PI / 2).Within(Tolerance)) ;
            Assert.That(model.Center.X,
                Is.EqualTo(2).Within(Tolerance)) ;
            Assert.That(model.Center.Y,
                Is.EqualTo(1).Within(Tolerance)) ;
        }) ;
    }

    [Test]
    public void FromCorners_ShortSideAlongPositiveX_RotationIsZero()
    {
        // Short side (length 2) along +X: X > 0, Y == 0 — no quadrant branch fires
        var model = RectangularColumnModel.FromCorners(Rectangle(new Point3D(0,
                0,
                0),
            2,
            0,
            0,
            4)) ;

        Assert.Multiple(() =>
        {
            Assert.That(model.ShortSide,
                Is.EqualTo(2).Within(Tolerance)) ;
            Assert.That(model.LongSide,
                Is.EqualTo(4).Within(Tolerance)) ;
            Assert.That(model.RotationAngle,
                Is.EqualTo(0).Within(Tolerance)) ;
        }) ;
    }

    [Test]
    public void FromCorners_ShortSideInQuadrantOne_RotationIsHalfPiPlusAngle()
    {
        // Short side direction 30° above +X (quadrant I): rotation = π/2 + angle
        var model = RectangularColumnModel.FromCorners(RotatedRectangle(Math.PI / 6)) ;

        Assert.That(model.RotationAngle,
            Is.EqualTo(Math.PI / 2 + Math.PI / 6).Within(Tolerance)) ;
    }

    [Test]
    public void FromCorners_ShortSideInQuadrantTwo_RotationIsAngleAsIs()
    {
        // Short side direction 150° (quadrant II): rotation = angle unchanged
        var model = RectangularColumnModel.FromCorners(RotatedRectangle(5 * Math.PI / 6)) ;

        Assert.That(model.RotationAngle,
            Is.EqualTo(5 * Math.PI / 6).Within(Tolerance)) ;
    }

    [Test]
    public void FromCorners_ShortSideInQuadrantThree_RotationIsPiMinusAngle()
    {
        // Short side direction 210° (quadrant III): AngleTo gives 150°, rotation = π − angle
        var model = RectangularColumnModel.FromCorners(RotatedRectangle(-5 * Math.PI / 6)) ;

        Assert.That(model.RotationAngle,
            Is.EqualTo(Math.PI - 5 * Math.PI / 6).Within(Tolerance)) ;
    }

    [Test]
    public void FromCorners_ShortSideInQuadrantFour_RotationIsPiMinusAngle()
    {
        // Short side direction −30° (quadrant IV): AngleTo gives 30°, rotation = π − angle
        var model = RectangularColumnModel.FromCorners(RotatedRectangle(-Math.PI / 6)) ;

        Assert.That(model.RotationAngle,
            Is.EqualTo(Math.PI - Math.PI / 6).Within(Tolerance)) ;
    }

    /// <summary>
    ///     Builds four consecutive corners from an origin, a first side vector and a second side vector
    /// </summary>
    private static List<Point3D> Rectangle(Point3D origin,
        double firstDx,
        double firstDy,
        double secondDx,
        double secondDy)
    {
        var corner1 = new Point3D(origin.X + firstDx,
            origin.Y + firstDy,
            origin.Z) ;
        var corner2 = new Point3D(corner1.X + secondDx,
            corner1.Y + secondDy,
            corner1.Z) ;
        var corner3 = new Point3D(origin.X + secondDx,
            origin.Y + secondDy,
            origin.Z) ;

        return [origin, corner1, corner2, corner3] ;
    }

    /// <summary>
    ///     Rectangle whose SHORT side (length 2) points at the given angle from +X,
    ///     with the long side (length 4) perpendicular to it
    /// </summary>
    private static List<Point3D> RotatedRectangle(double shortSideAngle)
    {
        var dx = Math.Cos(shortSideAngle) ;
        var dy = Math.Sin(shortSideAngle) ;

        return Rectangle(new Point3D(0,
                0,
                0),
            2 * dx,
            2 * dy,
            -4 * dy,
            4 * dx) ;
    }
}
