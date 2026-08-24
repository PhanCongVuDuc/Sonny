using Sonny.Application.Domain.Entities.FramingFromCad.Contexts ;

namespace Sonny.Application.UseCases.FramingFromCad.Services ;

/// <summary>
///     Drives one FramingFromCad run: read the CAD layer, build the beams, justify them, report.
/// </summary>
public interface IFramingFromCadInteractor
{
    /// <summary>
    ///     Runs the feature end to end.
    ///     Every beam is created inside a single transaction, so cancelling part-way leaves no beams at
    ///     all; justification then runs in a second transaction once the beams have been regenerated
    /// </summary>
    /// <param name="input">The run's settings and sections, all measurements already in feet</param>
    /// <exception cref="InvalidOperationException">
    ///     Propagates out when single-stroke beams were requested but the paired pass resolved no symbol
    ///     to build them at — a kept quirk of the ported original, which aborts the run and rolls it back.
    ///     See docs/bugs/FFC-001-single-line-pass-crashes-when-no-symbol-resolved.md
    /// </exception>
    Task Execute(FramingCreationContext input) ;
}
