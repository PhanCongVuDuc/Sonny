namespace Sonny.Application.Domain.Entities.AutoJoin.Models ;

/// <summary>
///     Everything one auto-join run needs, captured from the UI before execution
/// </summary>
public class AutoJoinInput
{
    /// <summary>
    ///     How elements are matched for joining
    /// </summary>
    public AutoJoinMode Mode { get ; set ; } = AutoJoinMode.RuleBased ;

    /// <summary>
    ///     The rule table. Only used by <see cref="AutoJoinMode.RuleBased" />, but its emptiness
    ///     also gates the success dialog in the other modes (ported behaviour)
    /// </summary>
    public IReadOnlyList<AutoJoinRule> Rules { get ; set ; } = [] ;

    /// <summary>
    ///     Unjoin the matched pairs instead of joining them. Rule-based mode only
    /// </summary>
    public bool IsUnjoin { get ; set ; }

    /// <summary>
    ///     Keep Revit warnings as warnings (deleted from the dialog) instead of resolving everything
    /// </summary>
    public bool IsAcceptWarnings { get ; set ; }
}
