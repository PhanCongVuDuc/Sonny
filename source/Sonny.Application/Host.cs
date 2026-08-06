using Microsoft.Extensions.DependencyInjection ;
using Serilog ;
using Sonny.Application.Config.Logging ;
using Sonny.Application.Infrastructure ;
using Sonny.Application.Infrastructure.Revit.Implements ;
using Sonny.Application.Presentation ;
using Sonny.Application.UseCases ;

namespace Sonny.Application ;

/// <summary>
///     Provides a host for the application's services and manages their lifetimes
/// </summary>
public static class Host
{
    private static IServiceProvider? s_serviceProvider ;
    private static readonly object s_lock = new() ;

    /// <summary>
    ///     Starts the host and configures the application's services
    /// </summary>
    public static void Start()
    {
        if (s_serviceProvider != null) {
            return ; // Already initialized
        }

        lock (s_lock) {
            if (s_serviceProvider != null) {
                return ; // Double-check locking
            }

            var services = new ServiceCollection() ;

            // Each layer owns the registrations for the types it declares. Registration order
            // carries no meaning here - IServiceCollection resolves by type, not by sequence.
            services.AddSerilogConfiguration() ;
            services.AddInfrastructureServices() ;
            services.AddUseCaseServices() ;
            services.AddPresentationsServices() ;

            s_serviceProvider = services.BuildServiceProvider() ;

            // Initialize LanguageChangeHandler to subscribe to language change events
            _ = s_serviceProvider.GetRequiredService<LanguageChangeHandler>() ;

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException ;
        }
    }

    /// <summary>
    ///     Ensures the host is initialized (lazy initialization for Addin Manager debugging)
    /// </summary>
    private static void EnsureInitialized()
    {
        if (s_serviceProvider == null) {
            Start() ;
        }
    }

    /// <summary>
    ///     Get service of type <typeparamref name="T" />
    /// </summary>
    /// <typeparam name="T">The type of service object to get</typeparam>
    /// <exception cref="System.InvalidOperationException">There is no service of type <typeparamref name="T" /></exception>
    public static T GetService<T>() where T : class
    {
        EnsureInitialized() ;
        return s_serviceProvider!.GetRequiredService<T>() ;
    }

    private static void OnUnhandledException(object sender,
        UnhandledExceptionEventArgs args)
    {
        var exception = (Exception)args.ExceptionObject ;
        var logger = GetService<ILogger>() ;
        logger.Fatal(exception,
            "Domain unhandled exception") ;
    }
}
