using Sonny.Application.Domain.Entities.AutoJoin.Models ;

namespace Sonny.Application.UseCases.AutoJoin.Implements ;

/// <summary>
///     Maps the Revit category of the current selection to the rule seeded when the AutoJoin
///     window opens. The mapping is deliberately identical to the original AlphaBIM tool,
///     quirks included (D4 in docs/features/AutoJoin.md): Structural Columns seed
///     "Architectural Column", Walls seed "Structural Wall", Floors seed "Architectural Floor".
/// </summary>
public static class AutoJoinSeedMapper
{
    // BuiltInCategory values are stable across Revit versions; hard-coding them keeps this
    // mapper (and its tests) free of any Revit reference
    private const long StructuralFramingCategoryId = -2001320 ;
    private const long StructuralColumnsCategoryId = -2001330 ;
    private const long FloorsCategoryId = -2000032 ;
    private const long WallsCategoryId = -2000011 ;
    private const long StructuralFoundationCategoryId = -2001300 ;
    private const long GenericModelCategoryId = -2001640 ;
    private const long CeilingsCategoryId = -2000038 ;
    private const long RoofsCategoryId = -2000035 ;

    /// <summary>
    ///     Creates the seed rule for a selection of the given category, or <c>null</c> when the
    ///     category has no mapping (the rule table then starts empty)
    /// </summary>
    /// <param name="selectedCategoryId">Numeric BuiltInCategory value of the selection</param>
    /// <returns>A rule with the mapped priority category and join-with <see cref="JoinCategory.All" /></returns>
    public static AutoJoinRule? CreateSeedRule(long selectedCategoryId)
    {
        JoinCategory? priorityCategory = selectedCategoryId switch
        {
            StructuralFramingCategoryId => JoinCategory.Beam,
            StructuralColumnsCategoryId => JoinCategory.ArchitecturalColumn,
            FloorsCategoryId => JoinCategory.ArchitecturalFloor,
            WallsCategoryId => JoinCategory.StructuralWall,
            StructuralFoundationCategoryId => JoinCategory.Foundation,
            GenericModelCategoryId => JoinCategory.GenericModel,
            CeilingsCategoryId => JoinCategory.Ceiling,
            RoofsCategoryId => JoinCategory.Roof,
            _ => null
        } ;

        if (priorityCategory is not { } category) {
            return null ;
        }

        return new AutoJoinRule
        {
            PriorityCategory = category, JoinWithCategory = JoinCategory.All
        } ;
    }
}
