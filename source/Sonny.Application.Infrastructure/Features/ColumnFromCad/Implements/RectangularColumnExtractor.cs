using Sonny.Application.Domain.Entities.ColumnFromCad ;
using Sonny.Application.Domain.Entities.ColumnFromCad.Models ;
using Sonny.Application.Infrastructure.Features.ColumnFromCad.Services ;
using Sonny.RevitExtensions.Extensions ;
using Sonny.RevitExtensions.Extensions.CurveLoops ;
using Sonny.RevitExtensions.Extensions.Elements ;
using Sonny.RevitExtensions.Extensions.GeometryObjects ;
using Sonny.RevitExtensions.Extensions.GeometryObjects.Solids ;

namespace Sonny.Application.Infrastructure.Features.ColumnFromCad.Implements ;

public class RectangularColumnExtractor(IColumnModelFactory columnModelFactory) : IRectangularColumnExtractor
{
    public List<RectangularColumnModel> ExtractFromBoundaryLines(ImportInstance cadInstance,
        string selectedLayer)
    {
        var columns = new List<RectangularColumnModel>() ;

        var polyLines = cadInstance.GetPolyLines()
            .Where(x => x.IsOnLayer(selectedLayer,
                cadInstance.Document))
            .ToList() ;

        foreach (var polyLine in polyLines) {
            if (ColumnShapeDetector.IsRectanglePolyline(polyLine.NumberOfCoordinates)) {
                var coordinates = polyLine.GetCoordinates() ;
                var line1 = Line.CreateBound(coordinates[0],
                    coordinates[1]) ;
                var line2 = Line.CreateBound(coordinates[1],
                    coordinates[2]) ;
                var line3 = Line.CreateBound(coordinates[2],
                    coordinates[3]) ;
                var line4 = Line.CreateBound(coordinates[3],
                    coordinates[4]) ;

                columns.Add(columnModelFactory.CreateRectangular([line1, line2, line3, line4])) ;
            }
        }

        return columns ;
    }

    public List<RectangularColumnModel> ExtractFromPlanarFaces(ImportInstance cadInstance,
        string selectedLayer)
    {
        var columns = new List<RectangularColumnModel>() ;
        var planarFaces = cadInstance.GetSolids()
            .GetPlanarFaces()
            .Where(x => x.IsOnLayer(selectedLayer,
                cadInstance.Document)) ;

        foreach (var planarFace in planarFaces) {
            var curveLoops = planarFace.GetEdgesAsCurveLoops() ;
            if (curveLoops.Count == 0) {
                continue ;
            }

            var curveLoop = curveLoops.First() ;
            var curves = curveLoop.GetCurves()
                .ToList() ;

            if (ColumnShapeDetector.IsRectangleLoop(curves.Count)) {
                columns.Add(columnModelFactory.CreateRectangular(curves)) ;
            }
        }

        return columns ;
    }
}
