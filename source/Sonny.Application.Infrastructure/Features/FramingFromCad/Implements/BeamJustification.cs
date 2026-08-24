namespace Sonny.Application.Infrastructure.Features.FramingFromCad.Implements ;

/// <summary>
///     The integer values Revit stores in <c>BuiltInParameter.Y_JUSTIFICATION</c> on a structural framing
///     instance. Named because "3" at a call site says nothing, and because the two passes of this
///     feature map to them in opposite directions on purpose — see the invariant in
///     docs/features/FramingFromCad.md
/// </summary>
internal static class BeamJustification
{
    /// <summary>
    ///     Justify to the left of the beam's direction of travel
    /// </summary>
    internal const int Left = 0 ;

    /// <summary>
    ///     Justify to the beam's centre line
    /// </summary>
    internal const int Center = 1 ;

    /// <summary>
    ///     Justify to the right of the beam's direction of travel. Note it is 3, not 2
    /// </summary>
    internal const int Right = 3 ;
}
