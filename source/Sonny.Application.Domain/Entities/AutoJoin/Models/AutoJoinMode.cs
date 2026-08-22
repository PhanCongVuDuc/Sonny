namespace Sonny.Application.Domain.Entities.AutoJoin.Models ;

/// <summary>
///     How the auto-join run decides which elements join which
/// </summary>
public enum AutoJoinMode
{
    /// <summary>
    ///     Default: iterate the rule table over the selection (or the active view when nothing is selected)
    /// </summary>
    RuleBased,

    /// <summary>
    ///     The selected elements get cut by every intersecting element in the active view
    /// </summary>
    CutSelectedElements,

    /// <summary>
    ///     The selected elements cut every intersecting element in the active view
    /// </summary>
    CutOtherElements
}
