using Microsoft.Extensions.DependencyInjection ;
using Sonny.Application.UseCases.AutoColumnDimension.Implements ;
using Sonny.Application.UseCases.AutoColumnDimension.Services ;
using Sonny.Application.UseCases.ColumnFromCad.Implements ;
using Sonny.Application.UseCases.ColumnFromCad.Services ;
using Sonny.Application.UseCases.Services ;

namespace Sonny.Application.UseCases ;

/// <summary>
///     Service registration for the Use Cases layer
/// </summary>
public static class ServiceRegistration
{
    /// <summary>
    ///     Adds the interactors owned by the Use Cases layer to the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    public static void AddUseCaseServices(this IServiceCollection services)
    {
        services.AddSingleton<ILicenseCheckService, LicenseCheckService>() ;

        // Transient: the interactor accumulates extracted columns between its Extract and Create
        // phases, so each run needs its own instance.
        services.AddTransient<IColumnFromCadInteractor, ColumnFromCadInteractor>() ;

        // Singleton like its old Infrastructure registration: stateless between runs, reads
        // document state only through its ports on each call and never caches it.
        services.AddSingleton<IAutoColumnDimensionInteractor, AutoColumnDimensionInteractor>() ;
    }
}
