namespace Sonny.Application.Domain.Entities.FramingFromCad.Models ;

/// <summary>
///     One beam cross-section parsed out of the free-text section list. Carries both unit systems on
///     purpose: pairing and symbol matching work in feet, while the generated type name is built from
///     the display values the user typed, so "200x300" in a millimetre project yields a type called
///     "200x300" and not its foot equivalent
/// </summary>
public class BeamSection
{
    /// <summary>
    ///     Beam width in display units, as typed
    /// </summary>
    public double WidthDisplay { get ; set ; }

    /// <summary>
    ///     Beam height in display units, as typed
    /// </summary>
    public double HeightDisplay { get ; set ; }

    /// <summary>
    ///     Beam width in feet (internal units). This is the spacing looked for between parallel strokes
    /// </summary>
    public double Width { get ; set ; }

    /// <summary>
    ///     Beam height in feet (internal units)
    /// </summary>
    public double Height { get ; set ; }

    /// <summary>
    ///     The entry as the user typed it, used only for progress text
    /// </summary>
    public string SourceText { get ; set ; } = string.Empty ;
}
