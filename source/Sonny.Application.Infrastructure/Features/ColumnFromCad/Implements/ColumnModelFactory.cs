using Sonny.Application.Domain.Entities ;
using Sonny.Application.Domain.Entities.ColumnFromCad.Models ;
using Sonny.Application.Infrastructure.Features.ColumnFromCad.Services ;
using Sonny.Application.Infrastructure.Revit.Services ;

namespace Sonny.Application.Infrastructure.Features.ColumnFromCad.Implements ;

public class ColumnModelFactory(IPoint3DConverter point3DConverter) : IColumnModelFactory
{
    public RectangularColumnModel CreateRectangular(List<Curve> curves)
    {
        if (curves.Count < 4) {
            throw new ArgumentException("Curves list must contain at least 4 curves to form a rectangle.",
                nameof( curves )) ;
        }

        // Mechanism only: convert the Revit curves to plain corners; the side/center/rotation
        // decisions live on the Domain model (RectangularColumnModel.FromCorners, ADR 0001)
        var corners = new List<Point3D>
        {
            point3DConverter.FromXyz(curves[0]
                .GetEndPoint(0)),
            point3DConverter.FromXyz(curves[0]
                .GetEndPoint(1)),
            point3DConverter.FromXyz(curves[1]
                .GetEndPoint(1)),
            point3DConverter.FromXyz(curves[2]
                .GetEndPoint(1))
        } ;

        return RectangularColumnModel.FromCorners(corners) ;
    }

    public CircularColumnModel CreateCircular(Arc arc)
    {
        var center = point3DConverter.FromXyz(arc.Center) ;
        var diameter = arc.Radius * 2 ;
        return new CircularColumnModel(diameter,
            center) ;
    }
}
