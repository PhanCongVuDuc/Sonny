using Sonny.Application.Infrastructure.Features.AutoColumnDimension.Services ;
using Sonny.RevitExtensions.Extensions.Elements ;
using Sonny.RevitExtensions.Extensions.GeometryObjects.Solids ;
using Sonny.RevitExtensions.Extensions.Views ;
using Sonny.RevitExtensions.Extensions.XYZs ;
using Sonny.RevitExtensions.RevitWrapper ;

namespace Sonny.Application.Infrastructure.Features.AutoColumnDimension.Contexts ;

public class ColumnDimensionContext(
    List<PlanarFace> planarFaces,
    XYZ maxPoint,
    XYZ firstDirection,
    XYZ secondDirection,
    GridWrapperBase? firstGridWrapper,
    GridWrapperBase? secondGridWrapper)
{
    public List<PlanarFace> PlanarFaces { get ; } = planarFaces ;
    public XYZ MaxPoint { get ; } = maxPoint ;
    public XYZ FirstDirection { get ; } = firstDirection ;
    public GridWrapperBase? FirstGridWrapper { get ; } = firstGridWrapper ;
    public XYZ SecondDirection { get ; } = secondDirection ;
    public GridWrapperBase? SecondGridWrapper { get ; } = secondGridWrapper ;

    public static ColumnDimensionContext? Create(ColumnWrapperBase columnWrapper,
        ViewWrapperBase viewWrapper,
        IGridFinder gridFinder)
    {
        if (columnWrapper.GetBoundingBoxXyz(viewWrapper) is not { } boundingBox) {
            return null ;
        }

        var options = viewWrapper.View.CreateDimensionOptions() ;
        var solids = columnWrapper.Element.GetSolids(options) ;
        var planarFaces = solids.GetPlanarFaces()
            .ToList() ;

        var midPoint = 0.5 * (boundingBox.Min + boundingBox.Max) ;

        XYZ firstDirection ;
        XYZ secondDirection ;
        GridWrapperBase? firstGridWrapper = null ;
        GridWrapperBase? secondGridWrapper = null ;

        if (viewWrapper.IsViewPlan) {
            firstDirection = columnWrapper.FamilyInstance.HandOrientation ;
            secondDirection = columnWrapper.FamilyInstance.FacingOrientation ;

            firstGridWrapper = gridFinder.GetNearestGrid(secondDirection,
                midPoint,
                firstDirection,
                viewWrapper) ;

            secondGridWrapper = gridFinder.GetNearestGrid(firstDirection,
                midPoint,
                secondDirection,
                viewWrapper) ;
        }
        else {
            firstDirection = viewWrapper.View.UpDirection ;
            secondDirection = viewWrapper.View.RightDirection ;

            if (! firstDirection.IsParallel(XYZ.BasisZ)) {
                firstGridWrapper = gridFinder.GetNearestGrid(viewWrapper.View.ViewDirection,
                    midPoint,
                    firstDirection,
                    viewWrapper) ;
            }

            if (! secondDirection.IsParallel(XYZ.BasisZ)) {
                secondGridWrapper = gridFinder.GetNearestGrid(viewWrapper.View.ViewDirection,
                    midPoint,
                    secondDirection,
                    viewWrapper) ;
            }
        }

        var maxPoint = new XYZ(boundingBox.Max.X,
            boundingBox.Max.Y,
            boundingBox.Max.Z) ;

        return new ColumnDimensionContext(planarFaces,
            maxPoint,
            firstDirection,
            secondDirection,
            firstGridWrapper,
            secondGridWrapper) ;
    }
}
