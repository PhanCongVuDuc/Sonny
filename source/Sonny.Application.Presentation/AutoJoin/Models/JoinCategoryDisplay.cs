using Sonny.Application.Domain.Entities.AutoJoin.Models ;

namespace Sonny.Application.Presentation.AutoJoin.Models ;

/// <summary>
///     Display names for <see cref="JoinCategory" /> — verbatim the strings the original AlphaBIM
///     grid showed (English in every language, so they are not localization keys). The order of
///     the lists is the order of the original combo boxes.
/// </summary>
public static class JoinCategoryDisplay
{
    private static readonly (JoinCategory Category, string Name)[] s_names =
    [
        (JoinCategory.Beam, "Structural Framing"),
        (JoinCategory.ArchitecturalColumn, "Architectural Column"),
        (JoinCategory.StructuralColumn, "Structural Column"),
        (JoinCategory.ArchitecturalFloor, "Architectural Floor"),
        (JoinCategory.StructuralFloor, "Structural Floor"),
        (JoinCategory.ArchitecturalWall, "Architectural Wall"),
        (JoinCategory.StructuralWall, "Structural Wall"),
        (JoinCategory.Foundation, "Foundation"),
        (JoinCategory.Roof, "Roof"),
        (JoinCategory.Ceiling, "Ceiling"),
        (JoinCategory.GenericModel, "Generic Model"),
        (JoinCategory.All, "<All>")
    ] ;

    /// <summary>
    ///     Choices for the priority column — every category except <see cref="JoinCategory.All" />
    /// </summary>
    public static IReadOnlyList<string> PriorityNames { get ; } =
        s_names.Where(pair => pair.Category != JoinCategory.All)
            .Select(pair => pair.Name)
            .ToList() ;

    /// <summary>
    ///     Choices for the join-with column — every category including <see cref="JoinCategory.All" />
    /// </summary>
    public static IReadOnlyList<string> CutNames { get ; } =
        s_names.Select(pair => pair.Name)
            .ToList() ;

    /// <summary>
    ///     Gets the display name of a category
    /// </summary>
    public static string ToName(JoinCategory category) =>
        s_names.First(pair => pair.Category == category)
            .Name ;

    /// <summary>
    ///     Gets the category behind a display name, falling back to <see cref="JoinCategory.Beam" />
    /// </summary>
    public static JoinCategory FromName(string? name)
    {
        foreach (var pair in s_names) {
            if (pair.Name == name) {
                return pair.Category ;
            }
        }

        return JoinCategory.Beam ;
    }
}
