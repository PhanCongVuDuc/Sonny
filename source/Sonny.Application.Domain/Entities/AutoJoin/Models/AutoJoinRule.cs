namespace Sonny.Application.Domain.Entities.AutoJoin.Models ;

/// <summary>
///     One auto-join rule: every element of the priority category cuts the intersecting elements
///     of the join-with category. Parameterless and mutable so it round-trips through JSON settings.
/// </summary>
public class AutoJoinRule
{
    /// <summary>
    ///     Category whose elements cut the others
    /// </summary>
    public JoinCategory PriorityCategory { get ; set ; } = JoinCategory.Beam ;

    /// <summary>
    ///     Category whose elements get cut
    /// </summary>
    public JoinCategory JoinWithCategory { get ; set ; } = JoinCategory.All ;

    /// <summary>
    ///     Reverses the cut order so the join-with element cuts the priority element instead
    /// </summary>
    public bool IsReverse { get ; set ; }
}
