namespace Sonny.Application.Infrastructure.Features.AutoJoin.Services ;

/// <summary>
///     Outcome of the pre-window environment check (F1/F2 in docs/features/AutoJoin.md)
/// </summary>
public enum AutoJoinEnvironmentCheck
{
    /// <summary>
    ///     The tool can run
    /// </summary>
    Ok,

    /// <summary>
    ///     The active document is a family document (F1)
    /// </summary>
    FamilyDocument,

    /// <summary>
    ///     The active view is a schedule, column schedule or sheet (F2)
    /// </summary>
    UnsupportedViewType
}

/// <summary>
///     Checks whether AutoJoin can run in the current document and view. Must be called BEFORE
///     the window opens — rejecting the user after they filled in rules is a contract violation
///     (see the invariants in docs/features/AutoJoin.md)
/// </summary>
public interface IAutoJoinEnvironmentChecker
{
    /// <summary>
    ///     Checks the active document and view
    /// </summary>
    /// <returns>The first blocking condition found, or <see cref="AutoJoinEnvironmentCheck.Ok" /></returns>
    AutoJoinEnvironmentCheck Check() ;
}
