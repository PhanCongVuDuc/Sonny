using Sonny.Application.UseCases.AutoColumnDimension.Models ;

namespace Sonny.Application.UseCases.AutoColumnDimension.Services ;

/// <summary>
///     Output port: executes one column's dimension plan against the Revit document. Implemented
///     in Infrastructure; must be called inside an open transaction.
/// </summary>
public interface IDimensionPlanExecutor
{
    /// <summary>
    ///     Creates the dimensions described by the plan for one column
    /// </summary>
    /// <param name="columnUniqueId">UniqueId of the column family instance</param>
    /// <param name="plan">The dimension plan decided by the policy</param>
    /// <param name="snapDistance">Snap distance in internal units, already scaled by view scale</param>
    /// <param name="dimensionTypeUniqueId">UniqueId of the dimension type, or null for the view default</param>
    /// <returns>The number of dimensions actually created</returns>
    int Execute(string columnUniqueId,
        ColumnDimensionPlan plan,
        double snapDistance,
        string? dimensionTypeUniqueId) ;
}
