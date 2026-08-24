namespace Sonny.Application.Domain.Entities.FramingFromCad.Models ;

/// <summary>
///     Persisted dialog state for the FramingFromCad feature. Every field round-trips through the
///     view model settings service, including <see cref="IsDisallowJoin" /> — the original implementation
///     left that one out of its settings class, so the checkbox reset on every run
/// </summary>
public class FramingFromCadSettings
{
    /// <summary>
    ///     UniqueId of the picked CAD <c>ImportInstance</c>
    /// </summary>
    public string? SelectedCadLinkId { get ; set ; }

    /// <summary>
    ///     Name of the CAD layer holding the beam strokes, e.g. "S-BEAM"
    /// </summary>
    public string? SelectedLayer { get ; set ; }

    /// <summary>
    ///     UniqueId of the single structural framing family to instantiate
    /// </summary>
    public string? SelectedFamilyId { get ; set ; }

    /// <summary>
    ///     Name of the family type parameter carrying beam width, e.g. "b"
    /// </summary>
    public string? WidthParameter { get ; set ; }

    /// <summary>
    ///     Name of the family type parameter carrying beam height, e.g. "h"
    /// </summary>
    public string? HeightParameter { get ; set ; }

    /// <summary>
    ///     UniqueId of the level the beams are hosted on
    /// </summary>
    public string? ReferenceLevelId { get ; set ; }

    /// <summary>
    ///     Vertical offset in <b>display units</b>. Converted to feet outside the interactor
    /// </summary>
    public double ZOffsetDisplay { get ; set ; }

    /// <summary>
    ///     Free-text section list exactly as typed, e.g. "200x300; 400x600", in <b>display units</b>.
    ///     Parsed by <see cref="BeamSectionListParser" />, which drops malformed entries silently
    /// </summary>
    public string? AllSectionsText { get ; set ; }

    /// <summary>
    ///     When true, every CAD stroke left over after pairing also becomes a beam, using the first
    ///     symbol the paired pass resolved
    /// </summary>
    public bool IsCreateForSingleLine { get ; set ; }

    /// <summary>
    ///     When true, both ends of every created beam are set to disallow join
    /// </summary>
    public bool IsDisallowJoin { get ; set ; }
}
