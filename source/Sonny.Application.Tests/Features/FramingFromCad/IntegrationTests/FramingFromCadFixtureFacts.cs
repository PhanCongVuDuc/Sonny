namespace Sonny.Application.Tests.Features.FramingFromCad.IntegrationTests ;

/// <summary>
///     What the FramingFromCad fixture drawing actually contains, shared by the builder that imports it and
///     the tests that assert against it.
///     <para>
///         The drawing is a **real Revit-generated DWG** supplied by the project owner
///         (`Resources/RevitFiles/Dwgs/Test_V2023_FramingFromCad.dwg`) — beams and columns drawn in Revit
///         and exported, i.e. exactly what the tool meets in production. Nothing here defines geometry;
///         every number was **measured** by running the feature's own extractor over the imported drawing
///         and is re-derivable that way. If the DWG is replaced, re-measure rather than guess.
///     </para>
/// </summary>
public static class FramingFromCadFixtureFacts
{
    /// <summary>
    ///     The supplied drawing, kept under Resources/RevitFiles/Dwgs alongside the .dxf and .pcp the owner
    ///     exported with it
    /// </summary>
    public const string DwgFileName = "Test_V2023_FramingFromCad.dwg" ;

    /// <summary>
    ///     Layer carrying the beam outlines. Upper case on purpose: this is what a real Revit DWG export
    ///     produces, and the original implementation's case-sensitive `Contains("Beam")` default would miss it
    /// </summary>
    public const string BeamLayerName = "S-BEAM" ;

    /// <summary>
    ///     Column outlines — 16 columns of 300x450. Present so a wrong layer choice is visibly wrong rather
    ///     than empty: asking for width 300 on this layer yields 16 groups of axis 450
    /// </summary>
    public const string ColumnLayerName = "S-COLS" ;

    /// <summary>
    ///     Beam hidden-line ticks, 12 short strokes
    /// </summary>
    public const string BeamHiddenLineLayerName = "S-BEAM-HDLN" ;

    /// <summary>
    ///     Grid lines, 10 strokes 36–61 m long
    /// </summary>
    public const string GridLayerName = "S-GRID" ;

    /// <summary>
    ///     Grid bubbles — blocks and text only, so the feature reads zero strokes from it
    /// </summary>
    public const string GridIdentifierLayerName = "S-GRID-IDEN" ;

    /// <summary>
    ///     Family already loaded in the placeholder project, whose types use "b" and "h"
    /// </summary>
    public const string FramingFamilyName = "M_Concrete-Rectangular Beam" ;

    public const string WidthParameterName = "b" ;
    public const string HeightParameterName = "h" ;

    #region Layer stroke counts

    /// <summary>
    ///     Straight strokes the feature reads from each layer, after arcs, solids, contained curves and
    ///     geometric duplicates are removed
    /// </summary>
    public const int BeamLayerStrokeCount = 168 ;

    public const int ColumnLayerStrokeCount = 96 ;
    public const int BeamHiddenLineLayerStrokeCount = 12 ;
    public const int GridLayerStrokeCount = 10 ;

    /// <summary>
    ///     Blocks (INSERT) and text (MTEXT) only — neither is a straight stroke
    /// </summary>
    public const int GridIdentifierLayerStrokeCount = 0 ;

    #endregion

    #region The four sections the drawing was drawn with

    /// <summary>
    ///     Sections in the order they are typed into the dialog. Two of them (300x600, 400x800) match types
    ///     the placeholder already has, so they exercise the matching branch; the other two must be
    ///     duplicated into new types
    /// </summary>
    public const string AllSectionsText = "200x300; 300x600; 350x700; 400x800" ;

    public const double NarrowWidthMm = 200 ;
    public const double NarrowHeightMm = 300 ;
    public const double MediumWidthMm = 300 ;
    public const double MediumHeightMm = 600 ;
    public const double WideWidthMm = 350 ;
    public const double WideHeightMm = 700 ;
    public const double WidestWidthMm = 400 ;
    public const double WidestHeightMm = 800 ;

    /// <summary>
    ///     Types the placeholder project already carries at exactly these sections
    /// </summary>
    public const string ExistingMediumTypeName = "300 x 600mm" ;

    public const string ExistingWidestTypeName = "400 x 800mm" ;

    /// <summary>
    ///     Types the run has to duplicate. Note the naming shape: no spaces, no unit suffix
    /// </summary>
    public const string GeneratedNarrowTypeName = "200x300" ;

    public const string GeneratedWideTypeName = "350x700" ;

    #endregion

    #region Measured pair counts, single-line mode OFF

    /// <summary>
    ///     Beams found per section when the stroke pool is never reduced. Every section searches the full
    ///     168 strokes, so a stroke may belong to a pair in more than one section
    /// </summary>
    public const int NarrowPairCount = 4 ;

    /// <summary>
    ///     30, not 18. Twelve of these are **pairs of end-cap strokes** — the short strokes closing a beam
    ///     outline sit 300 mm apart on some beams, so they pair with each other and produce beams only
    ///     300–450 mm long. Three groups also contain three strokes rather than two, and the extra stroke is
    ///     dropped without a word. This is the ported algorithm behaving as it always has; see the Contract
    /// </summary>
    public const int MediumPairCount = 30 ;

    public const int WidePairCount = 3 ;
    public const int WidestPairCount = 6 ;

    /// <summary>
    ///     Total beams created with single-line mode off
    /// </summary>
    public const int TotalPairCount =
        NarrowPairCount + MediumPairCount + WidePairCount + WidestPairCount ;

    #endregion

    #region Measured counts, single-line mode ON

    /// <summary>
    ///     Turning single-line mode on makes each section consume the strokes it pairs, and the 300 section
    ///     runs **before** the 350 one — so it eats the strokes the 350 section needed and that section
    ///     drops from 3 beams to none. Reordering the section list changes which beams exist. This is the
    ///     single most surprising consequence of the ported design
    /// </summary>
    public const int WidePairCountWithSingleLineMode = 0 ;

    /// <summary>
    ///     Three fewer than <see cref="TotalPairCount" />, entirely because of the 350 section above
    /// </summary>
    public const int TotalPairCountWithSingleLineMode = 40 ;

    /// <summary>
    ///     Strokes left over once every section took its pairs and every stroke whose length equals one of
    ///     the four section widths was filtered out
    /// </summary>
    public const int SingleLineCountWithSingleLineMode = 41 ;

    #endregion
}
