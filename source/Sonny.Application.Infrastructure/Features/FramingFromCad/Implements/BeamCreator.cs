using Autodesk.Revit.DB.Structure ;
using Sonny.Application.Domain.Entities ;
using Sonny.Application.Domain.Entities.FramingFromCad ;
using Sonny.Application.Domain.Entities.FramingFromCad.Contexts ;
using Sonny.Application.Domain.Entities.FramingFromCad.Models ;
using Sonny.Application.Domain.Entities.FramingFromCad.Services ;
using Sonny.Application.Domain.Exceptions ;
using Sonny.Application.Domain.Services ;
using Sonny.Application.Infrastructure.Revit.Services ;
using Sonny.RevitExtensions.Extensions ;
using Sonny.RevitExtensions.Extensions.Families ;
using Sonny.RevitExtensions.Extensions.GeometryObjects.Curves ;

namespace Sonny.Application.Infrastructure.Features.FramingFromCad.Implements ;

/// <summary>
///     Places beam instances, ported from the creation block of the original implementation.
///     Two ported behaviours are load-bearing and deliberately not "improved":
///     <list type="bullet">
///         <item>
///             The family's symbol list is captured once per run, before any duplication. A type this
///             run created is therefore invisible to later matching, which is why a section typed twice
///             tries to duplicate the same type name and fails — see
///             docs/bugs/FFC-002-duplicate-type-name-loses-a-whole-section-silently.md
///         </item>
///         <item>
///             Justification is set here for single-stroke beams but not for paired ones, whose facing
///             orientation is only trustworthy after the creating transaction commits
///         </item>
///     </list>
/// </summary>
public class BeamCreator(
    IRevitDocument revitDocument,
    IPoint3DConverter point3DConverter,
    IResourceHelper resourceHelper) : IBeamCreator
{
    /// <summary>
    ///     Resolved symbol per section. Keyed by section instance, which reproduces the original's
    ///     per-section variable: a section reuses its symbol across its own groups, and a second section
    ///     starts the search again
    /// </summary>
    private readonly Dictionary<BeamSection, FamilySymbol> _symbolsBySection = new() ;

    /// <summary>
    ///     The family's symbols as they were when the run started — before this run duplicated anything
    /// </summary>
    private List<FamilySymbol>? _symbolsAtRunStart ;

    private FamilySymbol? _firstResolvedSymbol ;
    private Level? _referenceLevel ;

    public bool HasResolvedSymbol => _firstResolvedSymbol != null ;

    public void Reset()
    {
        _symbolsBySection.Clear() ;
        _symbolsAtRunStart = null ;
        _firstResolvedSymbol = null ;
        _referenceLevel = null ;
    }

    public string? CreatePairedBeam(FramingPairModel model,
        FramingCreationContext context)
    {
        if (ResolveSymbol(model.Section,
                context) is not { } familySymbol) {
            return null ;
        }

        var instance = PlaceBeam(model.Start,
            model.End,
            familySymbol,
            context) ;

        // Justification deliberately left to the second transaction
        return instance.UniqueId ;
    }

    public string CreateSingleLineBeam(SingleLineFramingModel model,
        FramingCreationContext context)
    {
        if (_firstResolvedSymbol is not { } familySymbol) {
            // The kept crash: the original dereferenced a null symbol here and took the run down with it
            throw new InvalidOperationException(
                resourceHelper.GetString("MessageNoSymbolResolvedForSingleLines")) ;
        }

        var instance = PlaceBeam(model.Start,
            model.End,
            familySymbol,
            context) ;

        // Single-stroke beams get their justification immediately, and the mapping is the opposite way
        // round from the paired pass. Both are as the original had them
        var strokeDirection = point3DConverter.ToXyz(model.End)
            .Subtract(point3DConverter.ToXyz(model.Start))
            .Normalize() ;

        var justification = GetBeamDirection(instance)
            is { } beamDirection
            && beamDirection.IsAlmostEqualTo(strokeDirection)
                ? BeamJustification.Left
                : BeamJustification.Right ;

        instance.get_Parameter(BuiltInParameter.Y_JUSTIFICATION)
            .Set(justification) ;

        return instance.UniqueId ;
    }

    /// <summary>
    ///     Finds an existing symbol whose width and height match the section, or duplicates the family's
    ///     first symbol into a new type named after the section
    /// </summary>
    /// <returns>The symbol to use, or null when the section is too small to name a type after</returns>
    private FamilySymbol? ResolveSymbol(BeamSection section,
        FramingCreationContext context)
    {
        if (_symbolsBySection.TryGetValue(section,
                out var cachedSymbol)) {
            return cachedSymbol ;
        }

        var settings = context.Settings ;
        var symbols = GetSymbolsAtRunStart(settings) ;

        var matched = FindMatchingSymbol(symbols,
            section,
            settings) ;

        if (matched is null) {
            if (FramingSymbolSizingPolicy.TryBuildSymbolName(section.WidthDisplay,
                    section.HeightDisplay,
                    context.MinimumSizeDisplay) is not { } typeName) {
                return null ;
            }

            // Throws when the project already has a type of this name. Left to throw on purpose
            var duplicated = symbols[0]
                .Duplicate(typeName) ;
            duplicated.LookupParameter(settings.WidthParameter)
                ?.Set(section.Width) ;
            duplicated.LookupParameter(settings.HeightParameter)
                ?.Set(section.Height) ;
            matched = duplicated as FamilySymbol ;
        }

        if (matched is null) {
            return null ;
        }

        if (! matched.IsActive) {
            matched.Activate() ;
        }

        _firstResolvedSymbol ??= matched ;
        _symbolsBySection[section] = matched ;

        return matched ;
    }

    /// <summary>
    ///     Walks the symbols captured at run start looking for one whose dimensions match the section.
    ///     A symbol without both dimension parameters aborts the whole run rather than being skipped
    /// </summary>
    private static FamilySymbol? FindMatchingSymbol(List<FamilySymbol> symbols,
        BeamSection section,
        Domain.Entities.FramingFromCad.Models.FramingFromCadSettings settings)
    {
        foreach (var symbol in symbols) {
            var widthParameter = symbol.LookupParameter(settings.WidthParameter) ;
            var heightParameter = symbol.LookupParameter(settings.HeightParameter) ;

            if (widthParameter == null
                || heightParameter == null) {
                throw new FramingParameterMissingException(settings.WidthParameter,
                    settings.HeightParameter) ;
            }

            if (FramingSymbolSizingPolicy.Matches(ReadLength(widthParameter),
                    section.Width)
                && FramingSymbolSizingPolicy.Matches(ReadLength(heightParameter),
                    section.Height)) {
                return symbol ;
            }
        }

        return null ;
    }

    /// <summary>
    ///     Reads a dimension parameter as feet, tolerating an integer-backed parameter
    /// </summary>
    private static double ReadLength(Parameter parameter) =>
        parameter.StorageType switch {
            StorageType.Double => parameter.AsDouble(),
            StorageType.Integer => parameter.AsInteger(),
            _ => 0,
        } ;

    /// <summary>
    ///     Places one beam: the axis is flattened onto the reference level's elevation, then the vertical
    ///     offset is applied and both end elevations are zeroed, so the offset is the only thing lifting
    ///     the beam off its level
    /// </summary>
    private FamilyInstance PlaceBeam(Point3D start,
        Point3D end,
        FamilySymbol familySymbol,
        FramingCreationContext context)
    {
        var level = GetReferenceLevel(context) ;
        var startXyz = point3DConverter.ToXyz(start) ;
        var endXyz = point3DConverter.ToXyz(end) ;

        var axis = Line.CreateBound(new XYZ(startXyz.X,
                startXyz.Y,
                level.Elevation),
            new XYZ(endXyz.X,
                endXyz.Y,
                level.Elevation)) ;

        var instance = revitDocument.Document.Create.NewFamilyInstance(axis,
            familySymbol,
            level,
            StructuralType.Beam) ;

        instance.get_Parameter(BuiltInParameter.Z_OFFSET_VALUE)
            .Set(context.ZOffset) ;
        instance.get_Parameter(BuiltInParameter.STRUCTURAL_BEAM_END0_ELEVATION)
            .Set(0) ;
        instance.get_Parameter(BuiltInParameter.STRUCTURAL_BEAM_END1_ELEVATION)
            .Set(0) ;

        if (context.Settings.IsDisallowJoin) {
            StructuralFramingUtils.DisallowJoinAtEnd(instance,
                0) ;
            StructuralFramingUtils.DisallowJoinAtEnd(instance,
                1) ;
        }

        return instance ;
    }

    /// <summary>
    ///     The direction the created beam actually runs in, read back from its location curve
    /// </summary>
    private static XYZ? GetBeamDirection(Element instance) =>
        instance.Location is not LocationCurve locationCurve
            ? null
            : locationCurve.Curve.Direction()
                ?.Normalize() ;

    private List<FamilySymbol> GetSymbolsAtRunStart(
        Domain.Entities.FramingFromCad.Models.FramingFromCadSettings settings)
    {
        if (_symbolsAtRunStart != null) {
            return _symbolsAtRunStart ;
        }

        if (revitDocument.Document.GetElement(settings.SelectedFamilyId) is not Family family) {
            throw new InvalidOperationException(resourceHelper.GetString("MessageFramingFamilyNotFound")) ;
        }

        _symbolsAtRunStart = family.GetFamilySymbols()
            .ToList() ;

        if (_symbolsAtRunStart.Count == 0) {
            throw new InvalidOperationException(resourceHelper.GetString("MessageFramingFamilyHasNoTypes")) ;
        }

        return _symbolsAtRunStart ;
    }

    private Level GetReferenceLevel(FramingCreationContext context)
    {
        if (_referenceLevel != null) {
            return _referenceLevel ;
        }

        if (revitDocument.Document.GetElement(context.Settings.ReferenceLevelId) is not Level level) {
            throw new InvalidOperationException(resourceHelper.GetString("MessageReferenceLevelNotFound")) ;
        }

        _referenceLevel = level ;

        return level ;
    }
}
