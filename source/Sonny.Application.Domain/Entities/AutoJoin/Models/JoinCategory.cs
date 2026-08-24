namespace Sonny.Application.Domain.Entities.AutoJoin.Models ;

/// <summary>
///     Category a join rule can target. The architectural/structural split follows the behaviour
///     of the original tool this feature was ported from (see docs/features/AutoJoin.md, D1):
///     <see cref="StructuralFloor" /> and <see cref="StructuralWall" /> filter by the structural
///     parameter, while <see cref="ArchitecturalFloor" /> and <see cref="ArchitecturalWall" />
///     match every floor/wall regardless of that parameter.
/// </summary>
public enum JoinCategory
{
    /// <summary>
    ///     Structural framing elements
    /// </summary>
    Beam,

    /// <summary>
    ///     Architectural columns (OST_Columns)
    /// </summary>
    ArchitecturalColumn,

    /// <summary>
    ///     Structural columns (OST_StructuralColumns)
    /// </summary>
    StructuralColumn,

    /// <summary>
    ///     Every floor, regardless of the structural parameter (D1)
    /// </summary>
    ArchitecturalFloor,

    /// <summary>
    ///     Floors whose structural parameter is set
    /// </summary>
    StructuralFloor,

    /// <summary>
    ///     Every wall, regardless of the structural parameter (D1)
    /// </summary>
    ArchitecturalWall,

    /// <summary>
    ///     Walls whose structural parameter is set
    /// </summary>
    StructuralWall,

    /// <summary>
    ///     Structural foundations
    /// </summary>
    Foundation,

    /// <summary>
    ///     Roofs
    /// </summary>
    Roof,

    /// <summary>
    ///     Ceilings
    /// </summary>
    Ceiling,

    /// <summary>
    ///     Generic models
    /// </summary>
    GenericModel,

    /// <summary>
    ///     Every element with a computed volume. Only valid as a join-with category
    /// </summary>
    All
}
