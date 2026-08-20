using Sonny.Application.Infrastructure.Revit.Services ;
using Sonny.Application.UseCases.AutoColumnDimension.Models ;
using Sonny.Application.UseCases.AutoColumnDimension.Services ;
using Sonny.RevitExtensions.Extensions ;
using Sonny.RevitExtensions.Extensions.Elements ;
using Sonny.RevitExtensions.RevitWrapper ;

namespace Sonny.Application.Infrastructure.Features.AutoColumnDimension.Implements ;

public class ColumnGeometryReader(
    IRevitDocument revitDocument,
    IPoint3DConverter point3DConverter) : IColumnGeometryReader
{
    public ActiveViewGeometry ReadActiveView()
    {
        // Read fresh on every call — this class is a singleton and must never cache the view
        var viewWrapper = new ViewWrapperBase(revitDocument.ActiveView) ;

        var familyInstanceWrappers = viewWrapper.FamilyInstanceWrappers.Where(x =>
            x.FamilyInstance.IsBuiltInCategory(BuiltInCategory.OST_StructuralColumns)) ;

        // Same validity filter as before the move: a column whose center point cannot be
        // resolved in this view is dropped silently
        var columnWrappers = familyInstanceWrappers.Select(x => new ColumnWrapperBase(x.FamilyInstance))
            .Where(x => x.GetCenterPoint(viewWrapper) != null)
            .ToList() ;

        var columns = columnWrappers.Select(x => CreateColumnData(x,
                viewWrapper))
            .ToList() ;

        var grids = viewWrapper.GridWrappers
            .Where(x => x.Line != null)
            .Select(x => new GridCandidate(x.Element.UniqueId,
                point3DConverter.FromXyz(x.Line!.Direction),
                point3DConverter.FromXyz(x.Line!.GetEndPoint(0))))
            .ToList() ;

        var view = viewWrapper.View ;
        var viewData = new ViewGeometryData(view.Name,
            viewWrapper.IsViewPlan,
            point3DConverter.FromXyz(view.UpDirection),
            point3DConverter.FromXyz(view.RightDirection),
            point3DConverter.FromXyz(view.ViewDirection)) ;

        return new ActiveViewGeometry(viewData,
            columns,
            grids) ;
    }

    private ColumnGeometryData CreateColumnData(ColumnWrapperBase columnWrapper,
        ViewWrapperBase viewWrapper)
    {
        var boundingBox = columnWrapper.GetBoundingBoxXyz(viewWrapper) ;

        return new ColumnGeometryData(columnWrapper.Element.UniqueId,
            boundingBox == null
                ? null
                : point3DConverter.FromXyz(boundingBox.Min),
            boundingBox == null
                ? null
                : point3DConverter.FromXyz(boundingBox.Max),
            point3DConverter.FromXyz(columnWrapper.FamilyInstance.HandOrientation),
            point3DConverter.FromXyz(columnWrapper.FamilyInstance.FacingOrientation)) ;
    }
}
