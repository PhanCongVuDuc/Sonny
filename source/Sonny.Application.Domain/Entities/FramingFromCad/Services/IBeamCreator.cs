using Sonny.Application.Domain.Entities.FramingFromCad.Contexts ;
using Sonny.Application.Domain.Entities.FramingFromCad.Models ;
using Sonny.Application.Domain.Exceptions ;

namespace Sonny.Application.Domain.Entities.FramingFromCad.Services ;

/// <summary>
///     Creates beam instances in the model. Every method here must be called inside an open
///     transaction — the creator opens none of its own, because the whole run shares one transaction so
///     that cancelling rolls every beam back together.
///     <para>
///         The creator is <b>stateful across one run</b>: it remembers the first family symbol it
///         resolved, which is the size single-stroke beams are built at. Call <see cref="Reset" /> at the
///         start of every run or the previous run's symbol leaks into this one
///     </para>
/// </summary>
public interface IBeamCreator
{
    /// <summary>
    ///     Clears the remembered first symbol. Must be called once before each run
    /// </summary>
    void Reset() ;

    /// <summary>
    ///     True once at least one beam has been created and a symbol has therefore been remembered.
    ///     <see cref="CreateSingleLineBeam" /> cannot work until this is true
    /// </summary>
    bool HasResolvedSymbol { get ; }

    /// <summary>
    ///     Creates one beam from a pair of parallel strokes: resolves the family symbol for the section
    ///     (duplicating a new type named after the section when no existing type matches), places the
    ///     instance on the reference level with the axis flattened to that level's elevation, then applies
    ///     the vertical offset, zeroes both end elevations and optionally disallows join at both ends.
    ///     <para>
    ///         Justification is <b>not</b> set here — it needs the regenerated facing orientation, so it
    ///         happens in a later transaction via <see cref="IBeamJustificationAdjuster" />
    ///     </para>
    /// </summary>
    /// <param name="model">The beam to create, in feet</param>
    /// <param name="context">The run's settings, offsets already in feet</param>
    /// <returns>
    ///     The created instance's UniqueId, or null when the section's size is too small for a type name
    ///     to be generated
    /// </returns>
    /// <exception cref="FramingParameterMissingException">
    ///     Thrown when a symbol of the chosen family has no parameter under the configured width or
    ///     height name. This aborts the whole run rather than skipping one beam, matching the original
    /// </exception>
    string? CreatePairedBeam(FramingPairModel model,
        FramingCreationContext context) ;

    /// <summary>
    ///     Creates one beam along a lone stroke, using the symbol remembered from the paired pass, and
    ///     sets its justification immediately — unlike paired beams, whose justification waits for a
    ///     second transaction.
    /// </summary>
    /// <param name="model">The stroke to build along, in feet</param>
    /// <param name="context">The run's settings, offsets already in feet</param>
    /// <returns>The created instance's UniqueId</returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when no symbol was ever resolved, i.e. the paired pass created nothing. The exception
    ///     escapes the run and rolls it back — the ported original crashed here for the same reason, and
    ///     that behaviour was kept deliberately. See docs/bugs/FFC-001-single-line-pass-crashes-when-no-symbol-resolved.md
    /// </exception>
    string CreateSingleLineBeam(SingleLineFramingModel model,
        FramingCreationContext context) ;
}
