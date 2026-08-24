using Sonny.Application.Domain.Entities.FramingFromCad.Models ;

namespace Sonny.Application.Domain.Entities.FramingFromCad.Contexts ;

/// <summary>
///     Everything one FramingFromCad run needs, with all measurements already in internal units.
///     The interactor and everything below it never convert — see the unit boundary invariant in
///     docs/features/FramingFromCad.md
/// </summary>
public class FramingCreationContext
{
    /// <summary>
    ///     The dialog state this run was launched with
    /// </summary>
    public FramingFromCadSettings Settings { get ; set ; } = null! ;

    /// <summary>
    ///     Sections to build, in the order the user typed them. Malformed entries are already gone
    /// </summary>
    public List<BeamSection> Sections { get ; set ; } = [] ;

    /// <summary>
    ///     Vertical offset in feet (internal unit), applied as <c>Z_OFFSET_VALUE</c> on every beam
    /// </summary>
    public double ZOffset { get ; set ; }

    /// <summary>
    ///     Minimum side length in display units below which a generated type name is refused,
    ///     i.e. 1 mm expressed in the project's display unit
    /// </summary>
    public double MinimumSizeDisplay { get ; set ; }
}
