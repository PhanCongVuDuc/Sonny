namespace Sonny.Application.Domain.Entities.ColumnFromCad ;

/// <summary>
///     Sizing and naming rules for column family symbols: when an existing symbol counts as a
///     match, when a requested size is too small to create, and what a new symbol is called.
///     Moved verbatim from the Infrastructure creation strategies (ADR 0001) so the decisions are
///     unit-testable without Revit.
/// </summary>
public static class ColumnSymbolSizingPolicy
{
    /// <summary>
    ///     Tolerance (feet) for matching an existing symbol's parameter value against a target size
    /// </summary>
    public const double MatchTolerance = 0.001 ;

    /// <summary>
    ///     Decides whether an existing symbol dimension matches the requested one
    /// </summary>
    /// <param name="candidateValue">Parameter value read from the existing symbol</param>
    /// <param name="targetValue">Requested dimension</param>
    /// <returns>True when the values match within <see cref="MatchTolerance" /></returns>
    public static bool Matches(double candidateValue,
        double targetValue) =>
        Math.Abs(candidateValue - targetValue) < MatchTolerance ;

    /// <summary>
    ///     Decides whether a dimension is too small to be a real column side
    /// </summary>
    /// <param name="sizeInternal">Dimension in internal units (feet)</param>
    /// <returns>True when the dimension is negligible</returns>
    public static bool IsNegligibleSize(double sizeInternal) => Math.Abs(sizeInternal) < MatchTolerance ;

    /// <summary>
    ///     Builds the type name for a new rectangular symbol, e.g. "220 x 500mm", rounding each
    ///     side to whole display units — or refuses when a rounded side falls below the minimum
    /// </summary>
    /// <param name="widthDisplay">Width in display units</param>
    /// <param name="heightDisplay">Height in display units</param>
    /// <param name="minimumSizeDisplay">Minimum allowed size in display units (1mm converted)</param>
    /// <param name="unitName">Display unit suffix, e.g. "mm"</param>
    /// <returns>The symbol name, or null when the size is below the minimum</returns>
    public static string? TryBuildRectangularSymbolName(double widthDisplay,
        double heightDisplay,
        double minimumSizeDisplay,
        string unitName)
    {
        var widthRounded = Math.Round(widthDisplay,
            0) ;
        var heightRounded = Math.Round(heightDisplay,
            0) ;

        if (Math.Abs(widthRounded) < minimumSizeDisplay
            || Math.Abs(heightRounded) < minimumSizeDisplay) {
            return null ;
        }

        return $"{widthRounded} x {heightRounded}{unitName}" ;
    }

    /// <summary>
    ///     Builds the type name for a new circular symbol, e.g. "350mm" — or refuses when the
    ///     rounded diameter falls below the minimum
    /// </summary>
    /// <param name="diameterDisplay">Diameter in display units</param>
    /// <param name="minimumSizeDisplay">Minimum allowed size in display units (1mm converted)</param>
    /// <param name="unitName">Display unit suffix, e.g. "mm"</param>
    /// <returns>The symbol name, or null when the size is below the minimum</returns>
    public static string? TryBuildCircularSymbolName(double diameterDisplay,
        double minimumSizeDisplay,
        string unitName)
    {
        var diameterRounded = Math.Round(diameterDisplay,
            0) ;

        if (Math.Abs(diameterRounded) < minimumSizeDisplay) {
            return null ;
        }

        return $"{diameterRounded}{unitName}" ;
    }
}
