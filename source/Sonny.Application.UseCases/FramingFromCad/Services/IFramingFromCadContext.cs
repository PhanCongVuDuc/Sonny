using Sonny.Application.Domain.Entities.Settings.Models ;

namespace Sonny.Application.UseCases.FramingFromCad.Services ;

/// <summary>
///     Everything the FramingFromCad dialog needs to populate itself, gathered once when the command
///     starts. Building this is what picks the CAD link, so a cancelled pick fails construction and the
///     dialog never opens
/// </summary>
public interface IFramingFromCadContext
{
    /// <summary>
    ///     UniqueId of the CAD import instance the user picked
    /// </summary>
    string SelectedCadLinkId { get ; }

    /// <summary>
    ///     Layer names found in that CAD link
    /// </summary>
    HashSet<string> LayerNames { get ; }

    /// <summary>
    ///     Structural framing families in the project that have at least one type, ordered by name
    /// </summary>
    List<FamilyModel> FramingFamilies { get ; }

    /// <summary>
    ///     Levels in the project, ordered by elevation
    /// </summary>
    List<LevelModel> Levels { get ; }

    /// <summary>
    ///     Numeric type parameter names per family, keyed by the family's UniqueId. Bookkeeping
    ///     parameters (Assembly, OmniClass, Material, Category, Type) are filtered out, so what remains is
    ///     what a user might plausibly pick as width or height
    /// </summary>
    Dictionary<string, HashSet<string>> FamilyNumericParameters { get ; }
}
