using Sonny.Application.Domain.Entities.AutoJoin.Models ;

namespace Sonny.Application.UseCases.AutoJoin.Services ;

/// <summary>
///     Input port: reads the elements an auto-join run can touch, as plain numeric element ids
///     (ADR 0001 — no Revit type crosses this boundary). Implemented in Infrastructure; reads the
///     document through IRevitDocument on every call and never caches it.
/// </summary>
public interface IAutoJoinScopeReader
{
    /// <summary>
    ///     Whether anything at all is selected, volume or not — gates the seed-from-selection path
    /// </summary>
    bool HasSelectedElements() ;

    /// <summary>
    ///     Gets the category id of the first selected element with a model category, or null
    /// </summary>
    long? GetFirstSelectedModelCategoryId() ;

    /// <summary>
    ///     Gets the selected elements that have a computed volume greater than zero
    /// </summary>
    IReadOnlyList<long> GetSelectedSolidElementIds() ;

    /// <summary>
    ///     Gets the active-view elements that have a computed volume greater than zero
    /// </summary>
    IReadOnlyList<long> GetActiveViewSolidElementIds() ;

    /// <summary>
    ///     Filters the scope down to the elements matching the rule's priority category
    /// </summary>
    /// <param name="scopeElementIds">The ids the run considers for the priority side</param>
    /// <param name="category">The priority category of the rule</param>
    IReadOnlyList<long> GetPriorityElementIds(IReadOnlyCollection<long> scopeElementIds,
        JoinCategory category) ;

    /// <summary>
    ///     Gets the active-view elements matching the rule's join-with category
    /// </summary>
    /// <param name="category">The join-with category of the rule</param>
    IReadOnlyList<long> GetJoinWithElementIds(JoinCategory category) ;

    /// <summary>
    ///     Gets the active-view elements whose bounding box intersects the element's box
    ///     (expanded 5 mm in X/Y), excluding the element itself
    /// </summary>
    /// <param name="elementId">The probe element</param>
    IReadOnlyList<long> GetBoundingBoxIntersectingInView(long elementId) ;

    /// <summary>
    ///     Same bounding-box probe, restricted to the given candidates
    /// </summary>
    /// <param name="elementId">The probe element</param>
    /// <param name="candidateElementIds">The only ids allowed in the result</param>
    IReadOnlyList<long> GetBoundingBoxIntersecting(long elementId,
        IReadOnlyCollection<long> candidateElementIds) ;

    /// <summary>
    ///     Selects the elements in the Revit UI so the user can inspect them
    /// </summary>
    /// <param name="elementIds">The ids to select</param>
    void SelectElements(IReadOnlyCollection<long> elementIds) ;
}
