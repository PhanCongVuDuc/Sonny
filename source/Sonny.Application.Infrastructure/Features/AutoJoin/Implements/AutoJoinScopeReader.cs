using Sonny.Application.Domain.Entities.AutoJoin.Models ;
using Sonny.Application.Infrastructure.Revit.Services ;
using Sonny.Application.UseCases.AutoJoin.Services ;
using Sonny.RevitExtensions.Extensions ;
using Sonny.RevitExtensions.Extensions.Elements ;
using Sonny.RevitExtensions.Utilities ;

namespace Sonny.Application.Infrastructure.Features.AutoJoin.Implements ;

/// <summary>
///     Revit adapter behind IAutoJoinScopeReader. The category filters port the original AlphaBIM
///     behaviour verbatim (docs/features/AutoJoin.md, D1): only StructuralFloor/StructuralWall
///     look at the structural parameter — the architectural variants match every floor/wall.
///     Reads through IRevitDocument on every call and never caches the document.
/// </summary>
public class AutoJoinScopeReader(IRevitDocument revitDocument) : IAutoJoinScopeReader
{
    // The original expands the probe box 5 mm in X/Y before collecting candidates
    private const double BoundingBoxExpandMillimeters = 5 ;

    public bool HasSelectedElements() =>
        revitDocument.UIDocument
            .Selection
            .GetElementIds()
            .Count
        > 0 ;

    public long? GetFirstSelectedModelCategoryId()
    {
        var document = revitDocument.Document ;

        return revitDocument.UIDocument
            .Selection
            .GetElementIds()
            .Select(document.GetElement)
            .FirstOrDefault(element => element?.Category?.CategoryType == CategoryType.Model)
            ?.Category
            .Id
            .GetValue() ;
    }

    public IReadOnlyList<long> GetSelectedSolidElementIds()
    {
        var document = revitDocument.Document ;

        return revitDocument.UIDocument
            .Selection
            .GetElementIds()
            .Select(document.GetElement)
            .Where(HasComputedVolume)
            .Select(element => element.Id.GetValue())
            .ToList() ;
    }

    public IReadOnlyList<long> GetActiveViewSolidElementIds() =>
        new FilteredElementCollector(revitDocument.Document,
                revitDocument.ActiveView.Id).WhereElementIsNotElementType()
            .Where(HasComputedVolume)
            .Select(element => element.Id.GetValue())
            .ToList() ;

    public IReadOnlyList<long> GetPriorityElementIds(IReadOnlyCollection<long> scopeElementIds,
        JoinCategory category)
    {
        if (scopeElementIds.Count == 0) {
            return [] ;
        }

        var collector = new FilteredElementCollector(revitDocument.Document,
            ToElementIds(scopeElementIds)) ;

        IEnumerable<Element> elements = category switch
        {
            JoinCategory.StructuralFloor => collector.OfClass(typeof( Floor ))
                .Where(element => IsParameterOne(element,
                    BuiltInParameter.FLOOR_PARAM_IS_STRUCTURAL)),
            JoinCategory.StructuralWall => collector.OfClass(typeof( Wall ))
                .Where(element => IsParameterOne(element,
                    BuiltInParameter.WALL_STRUCTURAL_SIGNIFICANT)),
            JoinCategory.All => [],
            _ => collector.WhereElementIsNotElementType()
                .Where(element => IsOfCategory(element,
                    category))
        } ;

        return elements.Select(element => element.Id.GetValue())
            .ToList() ;
    }

    public IReadOnlyList<long> GetJoinWithElementIds(JoinCategory category)
    {
        var collector = new FilteredElementCollector(revitDocument.Document,
            revitDocument.ActiveView.Id).WhereElementIsNotElementType() ;

        IEnumerable<Element> elements = category switch
        {
            JoinCategory.All => collector.Where(HasComputedVolume),
            JoinCategory.StructuralFloor => collector.OfClass(typeof( Floor ))
                .Where(element => IsParameterOne(element,
                    BuiltInParameter.FLOOR_PARAM_IS_STRUCTURAL)),
            JoinCategory.StructuralWall => collector.OfClass(typeof( Wall ))
                .Where(element => IsParameterOne(element,
                    BuiltInParameter.WALL_STRUCTURAL_SIGNIFICANT)),
            // Ported quirk: the join-with side accepts volume >= 0 (not > 0) for category matches
            _ => collector.Where(element => HasNonNegativeVolume(element)
                                            && IsOfCategory(element,
                                                category))
        } ;

        return elements.Select(element => element.Id.GetValue())
            .ToList() ;
    }

    public IReadOnlyList<long> GetBoundingBoxIntersectingInView(long elementId) =>
        CollectBoundingBoxIntersecting(elementId,
            null) ;

    public IReadOnlyList<long> GetBoundingBoxIntersecting(long elementId,
        IReadOnlyCollection<long> candidateElementIds) =>
        CollectBoundingBoxIntersecting(elementId,
            ToElementIds(candidateElementIds)) ;

    public void SelectElements(IReadOnlyCollection<long> elementIds) =>
        revitDocument.UIDocument
            .Selection
            .SetElementIds(ToElementIds(elementIds)) ;

    private IReadOnlyList<long> CollectBoundingBoxIntersecting(long elementId,
        ICollection<ElementId>? candidateIds)
    {
        var element = revitDocument.Document.GetElement(elementId.ToElementId()) ;

        return element.GetBoundingBoxIntersectingElements(candidateIds,
                ElementCategoryFilters.CreateAllElementCategoriesFilter(),
                BoundingBoxExpandMillimeters)
            .Select(candidate => candidate.Id.GetValue())
            .ToList() ;
    }

    private static List<ElementId> ToElementIds(IReadOnlyCollection<long> elementIds) =>
        elementIds.Select(id => id.ToElementId())
            .ToList() ;

    private static bool HasComputedVolume(Element? element) =>
        element?.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED) is { } parameter
        && parameter.AsDouble() > 0 ;

    private static bool HasNonNegativeVolume(Element element) =>
        element.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED) is { } parameter
        && parameter.AsDouble() >= 0 ;

    private static bool IsParameterOne(Element element,
        BuiltInParameter parameter) =>
        element.get_Parameter(parameter) is { } value
        && value.AsInteger() == 1 ;

    private static bool IsOfCategory(Element element,
        JoinCategory category)
    {
        if (element.Category?.Name == null) {
            return false ;
        }

        return element.Category
                   .Id
                   .GetValue()
               == GetBuiltInCategoryId(category) ;
    }

    private static long GetBuiltInCategoryId(JoinCategory category) =>
        category switch
        {
            JoinCategory.Beam => (long)BuiltInCategory.OST_StructuralFraming,
            JoinCategory.ArchitecturalColumn => (long)BuiltInCategory.OST_Columns,
            JoinCategory.StructuralColumn => (long)BuiltInCategory.OST_StructuralColumns,
            JoinCategory.ArchitecturalFloor => (long)BuiltInCategory.OST_Floors,
            JoinCategory.ArchitecturalWall => (long)BuiltInCategory.OST_Walls,
            JoinCategory.Foundation => (long)BuiltInCategory.OST_StructuralFoundation,
            JoinCategory.Roof => (long)BuiltInCategory.OST_Roofs,
            JoinCategory.Ceiling => (long)BuiltInCategory.OST_Ceilings,
            JoinCategory.GenericModel => (long)BuiltInCategory.OST_GenericModel,
            _ => 0
        } ;
}
