using Sonny.Application.Domain.Entities.FramingFromCad.Contexts ;
using Sonny.Application.Domain.Entities.FramingFromCad.Models ;

namespace Sonny.Application.Domain.Entities.FramingFromCad.Services ;

/// <summary>
///     Reads the CAD layer named in the context and works out which beams it describes.
///     Pure reading: nothing is written to the model, so this runs outside any transaction
/// </summary>
public interface IFramingDataExtractor
{
    /// <summary>
    ///     Finds the beams described by the CAD strokes on the configured layer.
    ///     Each section is processed in the order the user typed it, and each section consumes the strokes
    ///     it pairs, so a stroke never belongs to two beams. Arcs on the layer contribute nothing
    /// </summary>
    /// <param name="context">
    ///     The run's settings and sections, with all measurements already in feet
    /// </param>
    /// <returns>
    ///     The paired beams, plus the leftover single strokes when <c>IsCreateForSingleLine</c> is on.
    ///     An empty result is normal — an empty section list or a layer with no strokes both produce it
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the CAD link named in the settings can no longer be found in the model
    /// </exception>
    FramingExtractionResult Extract(FramingCreationContext context) ;
}
