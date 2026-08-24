using Sonny.Application.UseCases.AutoColumnDimension.Models ;

namespace Sonny.Application.UseCases.AutoColumnDimension.Services ;

/// <summary>
///     Input port: reads the active view's geometry as Revit-free data for the dimension policy.
///     Implemented in Infrastructure on top of the Revit wrappers.
/// </summary>
public interface IColumnGeometryReader
{
    /// <summary>
    ///     Reads the active view fresh (never cached — see the UIDocument lifetime rule): its
    ///     axes, every valid structural column (those whose center point resolves in the view),
    ///     and every grid with a line
    /// </summary>
    /// <returns>The view geometry, columns and grid candidates</returns>
    ActiveViewGeometry ReadActiveView() ;
}
