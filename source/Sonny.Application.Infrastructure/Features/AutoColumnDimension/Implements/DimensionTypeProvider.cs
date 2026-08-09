using Sonny.Application.Domain.Entities.Settings.Models ;
using Sonny.Application.Domain.Services ;
using Sonny.Application.Infrastructure.Revit.Services ;
using Sonny.RevitExtensions.Extensions ;

namespace Sonny.Application.Infrastructure.Features.AutoColumnDimension.Implements ;

public class DimensionTypeProvider(IRevitDocument revitDocument) : IDimensionTypeProvider
{
    public List<DimensionTypeModel> GetDimensionTypes()
    {
        var dimensionTypes = revitDocument.Document
            .GetAllElements<DimensionType>()
            .Where(x => x.StyleType == DimensionStyleType.Linear)
            .ToList() ;

        var models = new List<DimensionTypeModel>() ;
        foreach (var dimensionType in dimensionTypes) {
            var snapDistance = GetSnapDistance(dimensionType) ;
            models.Add(new DimensionTypeModel(dimensionType.UniqueId,
                dimensionType.Name,
                snapDistance)) ;
        }

        return models ;
    }

    private static double GetSnapDistance(DimensionType dimensionType)
    {
        var parameter = dimensionType.FindParameter(BuiltInParameter.DIM_STYLE_DIM_LINE_SNAP_DIST) ;
        return parameter is not { StorageType: StorageType.Double } ? 0.0 : parameter.AsDouble() ;
    }
}
