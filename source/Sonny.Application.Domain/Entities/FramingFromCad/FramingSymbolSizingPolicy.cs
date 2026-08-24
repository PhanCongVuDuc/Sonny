using System.Globalization ;

namespace Sonny.Application.Domain.Entities.FramingFromCad ;

/// <summary>
///     Sizing and naming rules for framing family symbols: when an existing symbol counts as a match for
///     a requested section, and what a newly duplicated symbol is called.
///     Kept out of Infrastructure (ADR 0001) so the decisions are unit-testable without Revit
/// </summary>
public static class FramingSymbolSizingPolicy
{
    /// <summary>
    ///     Tolerance (feet) for matching an existing symbol's parameter value against a target size
    /// </summary>
    public const double MatchTolerance = 0.001 ;

    /// <summary>
    ///     Decides whether an existing symbol dimension matches the requested one
    /// </summary>
    /// <param name="candidateValue">Parameter value read from the existing symbol, in feet</param>
    /// <param name="targetValue">Requested dimension, in feet</param>
    /// <returns>True when the values match within <see cref="MatchTolerance" /></returns>
    public static bool Matches(double candidateValue,
        double targetValue) =>
        Math.Abs(candidateValue - targetValue) < MatchTolerance ;

    /// <summary>
    ///     Builds the type name for a symbol duplicated to carry a new section, e.g. "200x300" — rounding
    ///     each side to whole display units. Note the shape: no spaces, no unit suffix, lowercase "x".
    ///     That is the naming the users' projects already contain, and it differs from the
    ///     column feature's "200 x 300mm"
    /// </summary>
    /// <param name="widthDisplay">Beam width in display units</param>
    /// <param name="heightDisplay">Beam height in display units</param>
    /// <param name="minimumSizeDisplay">
    ///     Minimum allowed side in display units, i.e. 1 mm expressed in the project's unit
    /// </param>
    /// <returns>The symbol name, or null when either rounded side falls below the minimum</returns>
    public static string? TryBuildSymbolName(double widthDisplay,
        double heightDisplay,
        double minimumSizeDisplay)
    {
        var widthRounded = Math.Round(widthDisplay,
            0,
            MidpointRounding.AwayFromZero) ;
        var heightRounded = Math.Round(heightDisplay,
            0,
            MidpointRounding.AwayFromZero) ;

        if (Math.Abs(widthRounded) < minimumSizeDisplay
            || Math.Abs(heightRounded) < minimumSizeDisplay) {
            return null ;
        }

        // Invariant culture so a type name never picks up a machine-specific decimal separator
        return string.Concat(widthRounded.ToString(CultureInfo.InvariantCulture),
            "x",
            heightRounded.ToString(CultureInfo.InvariantCulture)) ;
    }
}
