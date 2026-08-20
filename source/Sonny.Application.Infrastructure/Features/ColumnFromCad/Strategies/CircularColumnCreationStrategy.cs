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

public class CircularColumnCreationStrategy(
    CircularColumnModel circularColumnModel,
    ColumnCreationContext columnCreationContext,
    IRevitDocument revitDocument,
    IPoint3DConverter point3DConverter,
    IDisplayUnitProvider displayUnitProvider,
    IUnitConverter unitConverter,
    ISettingsService settingsService) : ColumnCreationStrategy(circularColumnModel,
    columnCreationContext,
    revitDocument,
    point3DConverter)
{
    protected override FamilySymbol? GetOrCreateFamilySymbol()
    {
        var family =
            RevitDocument.Document.GetElementById<Family>(ColumnCreationContext.Settings.CircularColumnFamilyId!) ;
        if (family == null) {
            return null ;
        }

        return GetOrCreateCircularFamilySymbol(family,
            circularColumnModel.Diameter,
            ColumnCreationContext.Settings.DiameterParameter!) ;
    }

    /// <summary>
    ///     Gets or creates a family symbol for circular column with specified diameter
    /// </summary>
    private FamilySymbol? GetOrCreateCircularFamilySymbol(Family family,
        double diameter,
        string diameterParameter)
    {
        var allFamilySymbols = family.GetFamilySymbols()
            .ToList() ;

        // Try to find existing symbol with matching diameter
        foreach (var familySymbol in allFamilySymbols) {
            var diameterParam = familySymbol.LookupParameter(diameterParameter) ;
            if (diameterParam == null) {
                continue ;
            }

            var diameterValue = GetDoubleValue(diameterParam) ;

            if (ColumnSymbolSizingPolicy.Matches(diameterValue,
                    diameter)) {
                return familySymbol ;
            }
        }

        // Create new symbol if not found
        if (allFamilySymbols.Count == 0) {
            return null ;
        }

        // Get display unit from settings (or default) and convert diameter
        var displayUnit = settingsService.GetDisplayUnitOrDefault(displayUnitProvider.GetDefaultDisplayUnit) ;
        var diameterInDisplayUnit = unitConverter.FromInternalUnit(diameter,
            displayUnit) ;
        // Minimum size: 1mm converted to the current display unit (mechanism); the
        // round/minimum/naming decision itself is Domain's (ColumnSymbolSizingPolicy)
        const double minSizeInMm = 1.0 ;
        var minSizeInInternalUnit = unitConverter.ToInternalUnit(minSizeInMm,
            AppDisplayUnit.Millimeters) ;
        var minSize = unitConverter.FromInternalUnit(minSizeInInternalUnit,
            displayUnit) ;

        var unitName = unitConverter.GetUnitDisplayName(displayUnit) ;
        if (ColumnSymbolSizingPolicy.TryBuildCircularSymbolName(diameterInDisplayUnit,
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
        newSymbol?.LookupParameter(diameterParameter)
            ?.Set(diameter) ;

        return newSymbol ;
    }
}
