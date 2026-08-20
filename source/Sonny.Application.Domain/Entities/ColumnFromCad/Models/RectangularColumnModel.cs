namespace Sonny.Application.Domain.Entities.ColumnFromCad.Models ;

public class RectangularColumnModel(double shortSide, double longSide, Point3D center, double rotationAngle)
    : ColumnModel(center)
{
    public double ShortSide { get ; } = shortSide ;
    public double LongSide { get ; } = longSide ;
    public double RotationAngle { get ; } = rotationAngle ;

    /// <summary>
    ///     Creates a rectangular column model from four consecutive corner points of the rectangle.
    ///     Carries the quadrant-dependent rotation rule: the angle between the X axis and the short
    ///     side is remapped per quadrant of the short-side direction so the column family (whose
    ///     width parameter runs along X) ends up aligned with the drawn outline.
    /// </summary>
    /// <param name="corners">At least four consecutive corner points (first side = corners[0]→[1])</param>
    /// <returns>Rectangular column model with side lengths, center and rotation angle</returns>
    /// <exception cref="ArgumentException">Thrown when fewer than four corners are given</exception>
    public static RectangularColumnModel FromCorners(IReadOnlyList<Point3D> corners)
    {
        if (corners.Count < 4) {
            throw new ArgumentException("Corners list must contain at least 4 points to form a rectangle.",
                nameof( corners )) ;
        }

        var firstSideLength = Distance(corners[0],
            corners[1]) ;
        var secondSideLength = Distance(corners[1],
            corners[2]) ;

        double shortSide ;
        double longSide ;
        Point3D shortSideStart ;
        Point3D shortSideEnd ;

        if (firstSideLength > secondSideLength) {
            shortSide = secondSideLength ;
            longSide = firstSideLength ;
            shortSideStart = corners[1] ;
            shortSideEnd = corners[2] ;
        }
        else {
            shortSide = firstSideLength ;
            longSide = secondSideLength ;
            shortSideStart = corners[0] ;
            shortSideEnd = corners[1] ;
        }

        // Center of the rectangle = midpoint of the diagonal corners[0] → corners[2]
        var center = new Point3D(0.5 * (corners[0].X + corners[2].X),
            0.5 * (corners[0].Y + corners[2].Y),
            0.5 * (corners[0].Z + corners[2].Z)) ;

        var direction = Normalize(shortSideEnd.X - shortSideStart.X,
            shortSideEnd.Y - shortSideStart.Y,
            shortSideEnd.Z - shortSideStart.Z) ;

        // Angle between BasisX and the short side direction vector: atan2(|X × d|, X · d)
        var rotationAngle = Math.Atan2(Math.Sqrt(direction.Y * direction.Y + direction.Z * direction.Z),
            direction.X) ;

        // Adjust rotation angle based on the quadrant of the direction vector
        if (direction.X > 0
            && direction.Y < 0) {
            // Quadrant IV
            rotationAngle = Math.PI - rotationAngle ;
        }
        else if (direction.X > 0
                 && direction.Y > 0) {
            // Quadrant I
            rotationAngle = Math.PI / 2 + rotationAngle ;
        }
        else if (direction.X < 0
                 && direction.Y < 0) {
            // Quadrant III
            rotationAngle = Math.PI - rotationAngle ;
        }
        // Quadrant II uses the angle as-is

        return new RectangularColumnModel(shortSide,
            longSide,
            center,
            rotationAngle) ;
    }

    private static double Distance(Point3D first,
        Point3D second)
    {
        var dx = second.X - first.X ;
        var dy = second.Y - first.Y ;
        var dz = second.Z - first.Z ;
        return Math.Sqrt(dx * dx + dy * dy + dz * dz) ;
    }

    private static Point3D Normalize(double x,
        double y,
        double z)
    {
        var length = Math.Sqrt(x * x + y * y + z * z) ;
        if (length == 0) {
            return new Point3D(0,
                0,
                0) ;
        }

        return new Point3D(x / length,
            y / length,
            z / length) ;
    }
}
