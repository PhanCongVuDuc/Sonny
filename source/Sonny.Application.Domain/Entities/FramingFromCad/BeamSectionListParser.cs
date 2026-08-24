using Sonny.Application.Domain.Entities.FramingFromCad.Models ;

namespace Sonny.Application.Domain.Entities.FramingFromCad ;

/// <summary>
///     Turns the free-text section list the user types ("200x300; 400x600") into beam sections.
///     Ported from the original implementation, silent skips included: an entry that cannot be read as two
///     positive numbers is dropped without a message, a warning or a log line — so a single mistyped
///     character costs a whole family of beams with no explanation. That is deliberate; see the
///     <c>silent section skip</c> failure mode in docs/features/FramingFromCad.md
/// </summary>
public static class BeamSectionListParser
{
    /// <summary>
    ///     Separator between entries
    /// </summary>
    private const char EntrySeparator = ';' ;

    /// <summary>
    ///     Accepted separators between the two dimensions of one entry
    /// </summary>
    private static readonly char[] s_dimensionSeparators = ['x', 'X'] ;

    /// <summary>
    ///     Parses the section list, dropping every entry that is not two positive numbers
    /// </summary>
    /// <param name="allSectionsText">The text as typed, in display units. Null or blank yields nothing</param>
    /// <param name="toInternalUnit">
    ///     Converts a display-unit length to feet. Supplied by the caller so this class stays free of
    ///     any Revit dependency
    /// </param>
    /// <returns>
    ///     The usable sections, in the order they were typed. Never null; empty when nothing parsed
    /// </returns>
    public static List<BeamSection> Parse(string? allSectionsText,
        Func<double, double> toInternalUnit)
    {
        var sections = new List<BeamSection>() ;

        if (string.IsNullOrWhiteSpace(allSectionsText)) {
            return sections ;
        }

        foreach (var rawEntry in allSectionsText!.Split(EntrySeparator)) {
            var entry = rawEntry.Trim() ;
            if (string.IsNullOrEmpty(entry)) {
                continue ;
            }

            var dimensions = entry.Split(s_dimensionSeparators) ;

            double widthDisplay ;
            double heightDisplay ;
            try {
                // Current culture on purpose: the original used Convert.ToDouble
                widthDisplay = Convert.ToDouble(dimensions[0]) ;
                heightDisplay = Convert.ToDouble(dimensions[1]) ;
            }
            catch (Exception) {
                // Not two numbers — dropped without a word
                continue ;
            }

            if (widthDisplay <= 0
                || heightDisplay <= 0) {
                continue ;
            }

            sections.Add(new BeamSection {
                WidthDisplay = widthDisplay,
                HeightDisplay = heightDisplay,
                Width = toInternalUnit(widthDisplay),
                Height = toInternalUnit(heightDisplay),
                SourceText = entry,
            }) ;
        }

        return sections ;
    }
}
