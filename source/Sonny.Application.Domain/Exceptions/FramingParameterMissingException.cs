namespace Sonny.Application.Domain.Exceptions ;

/// <summary>
///     Raised when the chosen framing family has no type parameter under the width or height name the
///     user selected. Aborts the whole FramingFromCad run instead of skipping one beam: without those two
///     parameters no section can ever be applied, so every remaining beam would be wrong the same way
/// </summary>
/// <param name="widthParameter">The width parameter name that was looked for</param>
/// <param name="heightParameter">The height parameter name that was looked for</param>
public class FramingParameterMissingException(string? widthParameter, string? heightParameter)
    : Exception($"Framing family is missing dimension parameter '{widthParameter}' or '{heightParameter}'")
{
    /// <summary>
    ///     The width parameter name that could not be found
    /// </summary>
    public string? WidthParameter { get ; } = widthParameter ;

    /// <summary>
    ///     The height parameter name that could not be found
    /// </summary>
    public string? HeightParameter { get ; } = heightParameter ;
}
