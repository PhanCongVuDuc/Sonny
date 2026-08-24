namespace Sonny.Application.Tests.Features.AutoJoin.IntegrationTests ;

/// <summary>
///     Comment-parameter tags shared between <see cref="AutoJoinFixtureBuilder" /> (which writes
///     them) and <see cref="AutoJoinIntegrationTest" /> (which looks elements up by them).
///     One station per test case, spaced far apart so joins in one case cannot touch another.
/// </summary>
internal static class AutoJoinFixtureTags
{
    // S01 — rule Beam -> StructuralColumn: beam cuts column
    public const string S01Beam = "S01-Beam" ;
    public const string S01Column = "S01-Column" ;

    // S02 — same pair, rule with IsReverse: column cuts beam
    public const string S02Beam = "S02-Beam" ;
    public const string S02Column = "S02-Column" ;

    // S03 — ArchitecturalFloor rule matches a plain (non-structural) floor
    public const string S03Column = "S03-Column" ;
    public const string S03Floor = "S03-Floor" ;

    // S04 — StructuralFloor rule joins only the structural floor of the two
    public const string S04Column = "S04-Column" ;
    public const string S04StructuralFloor = "S04-StructuralFloor" ;
    public const string S04ArchitecturalFloor = "S04-ArchitecturalFloor" ;

    // S05 — StructuralWall rule joins only the structural wall of the two
    public const string S05StructuralWall = "S05-StructuralWall" ;
    public const string S05ArchitecturalWall = "S05-ArchitecturalWall" ;
    public const string S05Floor = "S05-Floor" ;

    // S06 — ArchitecturalWall rule matches a structural wall too (ported quirk, D1)
    public const string S06StructuralWall = "S06-StructuralWall" ;
    public const string S06Floor = "S06-Floor" ;

    // S07 — Foundation rule: beam cuts isolated footing (a column would auto-attach the footing,
    // and the resulting mandatory-move warning rolls the whole authoring transaction back)
    public const string S07Beam = "S07-Beam" ;
    public const string S07Foundation = "S07-Foundation" ;

    // S08 — Roof rule
    public const string S08Column = "S08-Column" ;
    public const string S08Roof = "S08-Roof" ;

    // S09 — Ceiling rule
    public const string S09Column = "S09-Column" ;
    public const string S09Ceiling = "S09-Ceiling" ;

    // S10 — GenericModel rule
    public const string S10Column = "S10-Column" ;
    public const string S10GenericModel = "S10-GenericModel" ;

    // S11 — Beam -> ArchitecturalColumn
    public const string S11Column = "S11-Column" ;
    public const string S11Beam = "S11-Beam" ;

    // S12 — Beam -> All joins every intersecting candidate
    public const string S12Beam = "S12-Beam" ;
    public const string S12Column = "S12-Column" ;
    public const string S12Floor = "S12-Floor" ;

    // S13 — bounding boxes overlap but solids do not: pair must be skipped (F7)
    public const string S13Column = "S13-Column" ;
    public const string S13ArcWall = "S13-ArcWall" ;

    // S14 — pre-joined pair for the unjoin rule
    public const string S14Beam = "S14-Beam" ;
    public const string S14Column = "S14-Column" ;

    // S15 — CutSelectedElements mode: candidates cut the selected column
    public const string S15Column = "S15-Column" ;
    public const string S15Beam = "S15-Beam" ;
    public const string S15GenericModel = "S15-GenericModel" ;

    // S16 — CutOtherElements mode: the selected column cuts the candidates
    public const string S16Column = "S16-Column" ;
    public const string S16Beam = "S16-Beam" ;
}
