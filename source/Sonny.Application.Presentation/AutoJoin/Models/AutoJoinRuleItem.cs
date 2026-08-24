using Sonny.Application.Domain.Entities.AutoJoin.Models ;

namespace Sonny.Application.Presentation.AutoJoin.Models ;

/// <summary>
///     One editable row of the rule grid. Holds display names so the grid combo boxes bind
///     directly; converts to the Domain rule at run/save time
/// </summary>
public partial class AutoJoinRuleItem : ObservableObject
{
    [ObservableProperty]
    private string priorityCategory = JoinCategoryDisplay.ToName(JoinCategory.Beam) ;

    [ObservableProperty]
    private string joinWithCategory = JoinCategoryDisplay.ToName(JoinCategory.All) ;

    [ObservableProperty]
    private bool isReverse ;

    /// <summary>
    ///     Converts the row to a Domain rule
    /// </summary>
    public AutoJoinRule ToRule() =>
        new()
        {
            PriorityCategory = JoinCategoryDisplay.FromName(PriorityCategory),
            JoinWithCategory = JoinCategoryDisplay.FromName(JoinWithCategory),
            IsReverse = IsReverse
        } ;

    /// <summary>
    ///     Creates a row from a Domain rule
    /// </summary>
    public static AutoJoinRuleItem FromRule(AutoJoinRule rule) =>
        new()
        {
            PriorityCategory = JoinCategoryDisplay.ToName(rule.PriorityCategory),
            JoinWithCategory = JoinCategoryDisplay.ToName(rule.JoinWithCategory),
            IsReverse = rule.IsReverse
        } ;
}
