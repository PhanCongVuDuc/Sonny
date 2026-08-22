namespace Sonny.Application.UseCases.AutoJoin.Models ;

/// <summary>
///     Outcome of checking whether a candidate's solid intersects the current anchor's solid.
///     The interactor maps each outcome to a decision (see docs/features/AutoJoin.md, F6–F8)
/// </summary>
public enum SolidIntersectCheck
{
    /// <summary>
    ///     The anchor element has no solid — the pair joins without any check (F6)
    /// </summary>
    NoAnchorSolid,

    /// <summary>
    ///     The solids really intersect — the pair joins
    /// </summary>
    Intersecting,

    /// <summary>
    ///     No intersection (or the candidate has no solid) — the pair is skipped silently (F7)
    /// </summary>
    NotIntersecting,

    /// <summary>
    ///     The check itself failed — the pair joins anyway, joining too much beats missing one (F8, D2)
    /// </summary>
    CheckFailed
}
