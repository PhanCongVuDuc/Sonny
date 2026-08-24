// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using Autodesk.Revit.DB.Structure ;
using Sonny.Application.Domain.Entities.ColumnFromCad.Contexts ;
using Sonny.Application.Domain.Entities.ColumnFromCad.Models ;
using Sonny.Application.Domain.Entities.ColumnFromCad.Services ;
using Sonny.Application.Infrastructure.Revit.Services ;
using Sonny.RevitExtensions.Extensions ;

namespace Sonny.Application.Infrastructure.Features.ColumnFromCad.Strategies ;

public abstract class ColumnCreationStrategy(
    ColumnModel columnModel,
    ColumnCreationContext columnCreationContext,
    IRevitDocument revitDocument,
    IPoint3DConverter point3DConverter) : IColumnCreationStrategy
{
    protected readonly ColumnCreationContext ColumnCreationContext = columnCreationContext ;
    protected readonly ColumnModel ColumnModel = columnModel ;
    protected readonly IRevitDocument RevitDocument = revitDocument ;
    protected readonly IPoint3DConverter Point3DConverter = point3DConverter ;

    public string? Execute()
    {
        if (GetOrCreateFamilySymbol() is not { } familySymbol) {
            return null ;
        }

        if (! familySymbol.IsActive) {
            familySymbol.Activate() ;
        }

        var document = RevitDocument.Document ;
        var baseLevel = document.GetElementById<Level>(ColumnCreationContext.Settings.BaseLevelId!) ;
        var topLevel = document.GetElementById<Level>(ColumnCreationContext.Settings.TopLevelId!) ;

        if (baseLevel == null
            || topLevel == null) {
            return null ;
        }

        var centerXyz = Point3DConverter.ToXyz(ColumnModel.Center) ;
        var instance = document.Create.NewFamilyInstance(centerXyz,
            familySymbol,
            baseLevel,
            StructuralType.Column) ;

        instance.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_PARAM)
            .Set(baseLevel.Id) ;
        instance.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_PARAM)
            .Set(topLevel.Id) ;
        instance.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM)
            .Set(ColumnCreationContext.BaseOffset) ;
        instance.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM)
            .Set(ColumnCreationContext.TopOffset) ;

        RotateElement(instance) ;

        return instance.UniqueId ;
    }

    protected abstract FamilySymbol? GetOrCreateFamilySymbol() ;

    protected virtual void RotateElement(Element element)
    {
    }

    protected static double GetDoubleValue(Parameter parameter) =>
        parameter.StorageType switch
        {
            StorageType.Double => parameter.AsDouble(),
            StorageType.Integer => parameter.AsInteger(),
            _ => 0
        } ;
}
