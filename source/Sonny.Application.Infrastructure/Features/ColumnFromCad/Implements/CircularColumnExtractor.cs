using Sonny.Application.Domain.Entities.ColumnFromCad ;
using Sonny.Application.Domain.Entities.ColumnFromCad.Models ;
using Sonny.Application.Infrastructure.Features.ColumnFromCad.Services ;
using Sonny.Application.Infrastructure.Revit.Services ;
using Sonny.RevitExtensions.Extensions ;
using Sonny.RevitExtensions.Extensions.CurveLoops ;
using Sonny.RevitExtensions.Extensions.Elements ;
using Sonny.RevitExtensions.Extensions.GeometryObjects ;
using Sonny.RevitExtensions.Extensions.GeometryObjects.Curves ;
using Sonny.RevitExtensions.Extensions.GeometryObjects.Solids ;

namespace Sonny.Application.Infrastructure.Features.ColumnFromCad.Implements ;

public class CircularColumnExtractor(
    IColumnModelFactory columnModelFactory,
    IPoint3DConverter point3DConverter) : ICircularColumnExtractor
{
    public List<CircularColumnModel> ExtractFromBoundaryLines(ImportInstance cadInstance,
        string selectedLayer)
    {
        var columns = new List<CircularColumnModel>() ;

        var arcs = cadInstance.GetArcs()
            .Where(x => x.IsOnLayer(selectedLayer,
                cadInstance.Document))
            .ToList() ;

        foreach (var arc in arcs) {
            columns.Add(columnModelFactory.CreateCircular(arc)) ;
        }

        return columns ;
    }

    public List<CircularColumnModel> ExtractFromPlanarFaces(ImportInstance cadInstance,
        string selectedLayer)
    {
        var columns = new List<CircularColumnModel>() ;
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

            // Skip rectangular loops before tessellating anything
            if (ColumnShapeDetector.IsRectangleLoop(curves.Count)) {
                continue ;
            }

            // Mechanism: tessellate the loop to points; the circle decision is Domain's
            var points = curves.GetXYZPoints()
                .Select(point3DConverter.FromXyz)
                .ToList() ;

            if (ColumnShapeDetector.TryDetectCircle(curves.Count,
                    points) is { } circularColumnModel) {
                columns.Add(circularColumnModel) ;
            }
        }

        return columns ;
    }
}
