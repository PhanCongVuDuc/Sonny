using Sonny.Application.Infrastructure.Features.AutoColumnDimension.Services ;
using Sonny.Application.Infrastructure.Revit.Services ;
using Sonny.Application.UseCases.AutoColumnDimension.Models ;
using Sonny.Application.UseCases.AutoColumnDimension.Services ;
using Sonny.RevitExtensions.Extensions ;
using Sonny.RevitExtensions.Extensions.Elements ;
using Sonny.RevitExtensions.Extensions.GeometryObjects.Solids ;
using Sonny.RevitExtensions.Extensions.Views ;
using Sonny.RevitExtensions.RevitWrapper ;

namespace Sonny.Application.Infrastructure.Features.AutoColumnDimension.Implements ;

public class DimensionPlanExecutor(
    IRevitDocument revitDocument,
    IDimensionCreator dimensionCreator,
    IPoint3DConverter point3DConverter) : IDimensionPlanExecutor
{
    public int Execute(string columnUniqueId,
        ColumnDimensionPlan plan,
        double snapDistance,
        string? dimensionTypeUniqueId)
    {
        var document = revitDocument.Document ;
        var viewWrapper = new ViewWrapperBase(revitDocument.ActiveView) ;

        // The column was read by ColumnGeometryReader moments ago in the same command; a missing
        // element here would throw and be caught by the interactor's per-column catch
        var familyInstance = document.GetElementById<FamilyInstance>(columnUniqueId)! ;
        var columnWrapper = new ColumnWrapperBase(familyInstance) ;

        var options = viewWrapper.View.CreateDimensionOptions() ;
        var solids = columnWrapper.Element.GetSolids(options) ;
        var planarFaces = solids.GetPlanarFaces()
            .ToList() ;

        DimensionType? dimensionType = null ;
        if (dimensionTypeUniqueId != null) {
            dimensionType = document.GetElementById<DimensionType>(dimensionTypeUniqueId) ;
        }

        var maxPoint = point3DConverter.ToXyz(plan.MaxPoint) ;

        var createdCount = CreateForAxis(plan.FirstAxis,
            planarFaces,
            maxPoint,
            snapDistance,
            viewWrapper,
            dimensionType) ;

        createdCount += CreateForAxis(plan.SecondAxis,
            planarFaces,
            maxPoint,
            snapDistance,
            viewWrapper,
            dimensionType) ;

        return createdCount ;
    }

    private int CreateForAxis(DimensionAxisPlan axisPlan,
        List<PlanarFace> planarFaces,
        XYZ maxPoint,
        double snapDistance,
        ViewWrapperBase viewWrapper,
        DimensionType? dimensionType)
    {
        GridWrapperBase? gridWrapper = null ;
        if (axisPlan.GridUniqueId != null
            && revitDocument.Document.GetElementById<Grid>(axisPlan.GridUniqueId) is { } grid) {
            gridWrapper = new GridWrapperBase(grid) ;
        }

        var dimensions = dimensionCreator.DimensionByDirection(planarFaces,
            point3DConverter.ToXyz(axisPlan.Direction),
            point3DConverter.ToXyz(axisPlan.OffsetDirection),
            gridWrapper,
            maxPoint,
            snapDistance,
            viewWrapper,
            dimensionType) ;

        return dimensions.Count ;
    }
}
