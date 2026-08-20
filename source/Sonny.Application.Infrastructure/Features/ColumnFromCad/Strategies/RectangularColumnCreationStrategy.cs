// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using Sonny.Application.Domain.Entities.ColumnFromCad ;
using Sonny.Application.Domain.Entities.ColumnFromCad.Contexts ;
using Sonny.Application.Domain.Entities.ColumnFromCad.Models ;
using Sonny.Application.Domain.Entities.Settings ;
using Sonny.Application.Domain.Services ;
using Sonny.Application.Infrastructure.Revit.Services ;
using Sonny.RevitExtensions.Extensions ;
using Sonny.RevitExtensions.Extensions.Families ;

namespace Sonny.Application.Infrastructure.Features.ColumnFromCad.Strategies ;

public class RectangularColumnCreationStrategy(
    RectangularColumnModel rectangularColumnModel,
    ColumnCreationContext columnCreationContext,
    IRevitDocument revitDocument,
    IPoint3DConverter point3DConverter,
    IDisplayUnitProvider displayUnitProvider,
    IUnitConverter unitConverter,
    ISettingsService settingsService) : ColumnCreationStrategy(rectangularColumnModel,
    columnCreationContext,
    revitDocument,
    point3DConverter)
{
    protected override FamilySymbol? GetOrCreateFamilySymbol()
    {
        if (ColumnSymbolSizingPolicy.IsNegligibleSize(rectangularColumnModel.ShortSide)
            || ColumnSymbolSizingPolicy.IsNegligibleSize(rectangularColumnModel.LongSide)) {
            return null ;
        }

        var family =
            RevitDocument.Document.GetElementById<Family>(ColumnCreationContext.Settings.RectangularColumnFamilyId!) ;
        if (family == null) {
            return null ;
        }

        var familySymbol = GetOrCreateRectangularFamilySymbol(family,
            rectangularColumnModel.ShortSide,
            rectangularColumnModel.LongSide,
            ColumnCreationContext.Settings.WidthParameter!,
            ColumnCreationContext.Settings.HeightParameter!) ;

        return familySymbol ;
    }

    protected override void RotateElement(Element element)
    {
        // Rotate column if needed
        var centerXyz = Point3DConverter.ToXyz(ColumnModel.Center) ;
        if (rectangularColumnModel.RotationAngle >= 0) {
            ElementTransformUtils.RotateElement(element.Document,
                element.Id,
                Line.CreateBound(centerXyz,
                    centerXyz.Add(XYZ.BasisZ)),
                rectangularColumnModel.RotationAngle) ;
        }
        else {
            ElementTransformUtils.RotateElement(element.Document,
                element.Id,
                Line.CreateBound(centerXyz,
                    centerXyz.Add(XYZ.BasisZ)),
                -rectangularColumnModel.RotationAngle + Math.PI / 2) ;
        }
    }

    private FamilySymbol? GetOrCreateRectangularFamilySymbol(Family family,
        double width,
        double height,
        string widthParameter,
        string heightParameter)
    {
        var allFamilySymbols = family.GetFamilySymbols()
            .ToList() ;

        // Try to find existing symbol with matching dimensions
        foreach (var familySymbol in allFamilySymbols) {
            var widthParam = familySymbol.LookupParameter(widthParameter) ;
            var heightParam = familySymbol.LookupParameter(heightParameter) ;

            if (widthParam == null
                || heightParam == null) {
                continue ;
            }

            var widthValue = GetDoubleValue(widthParam) ;
            var heightValue = GetDoubleValue(heightParam) ;

            if (ColumnSymbolSizingPolicy.Matches(widthValue,
                    width)
                && ColumnSymbolSizingPolicy.Matches(heightValue,
                    height)) {
                return familySymbol ;
            }
        }

        // Create new symbol if not found
        if (allFamilySymbols.Count == 0) {
            return null ;
        }

        // Get display unit from settings (or default) and convert dimensions
        var displayUnit = settingsService.GetDisplayUnitOrDefault(displayUnitProvider.GetDefaultDisplayUnit) ;
        var widthInDisplayUnit = unitConverter.FromInternalUnit(width,
            displayUnit) ;
        var heightInDisplayUnit = unitConverter.FromInternalUnit(height,
            displayUnit) ;
        // Minimum size: 1mm converted to the current display unit (mechanism); the
        // round/minimum/naming decision itself is Domain's (ColumnSymbolSizingPolicy)
        const double minSizeInMm = 1.0 ;
        var minSizeInInternalUnit = unitConverter.ToInternalUnit(minSizeInMm,
            AppDisplayUnit.Millimeters) ;
        var minSize = unitConverter.FromInternalUnit(minSizeInInternalUnit,
            displayUnit) ;

        var unitName = unitConverter.GetUnitDisplayName(displayUnit) ;
        if (ColumnSymbolSizingPolicy.TryBuildRectangularSymbolName(widthInDisplayUnit,
                heightInDisplayUnit,
                minSize,
                unitName) is not { } name) {
            return null ;
        }

        // Check if symbol with this name already exists
        var existingSymbol = allFamilySymbols.FirstOrDefault(f => f.Name.Equals(name)) ;
        if (existingSymbol != null) {
            return existingSymbol ;
        }

        var newSymbol = allFamilySymbols[0]
            .Duplicate(name) as FamilySymbol ;
        newSymbol?.LookupParameter(widthParameter)
            ?.Set(width) ;
        newSymbol?.LookupParameter(heightParameter)
            ?.Set(height) ;

        return newSymbol ;
    }
}
