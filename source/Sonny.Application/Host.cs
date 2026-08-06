using Microsoft.Extensions.DependencyInjection ;
using Serilog ;
using Sonny.Application.Domain.Config.Logging ;
using Sonny.Application.Domain.Entities.ColumnFromCad.Models ;
using Sonny.Application.Domain.Entities.ColumnFromCad.Services ;
using Sonny.Application.Domain.Implements ;
using Sonny.Application.Domain.Services ;
using Sonny.Application.Infrastructure.Features.AutoColumnDimension.Implements ;
using Sonny.Application.Infrastructure.Features.AutoColumnDimension.Services ;
using Sonny.Application.Infrastructure.Features.ColumnFromCad.Implements ;
using Sonny.Application.Infrastructure.Features.ColumnFromCad.Services ;
using Sonny.Application.Infrastructure.Features.ColumnFromCad.Strategies ;
using Sonny.Application.Infrastructure.License ;
using Sonny.Application.Infrastructure.Resource.Implements ;
using Sonny.Application.Infrastructure.Revit.Implements ;
using Sonny.Application.Infrastructure.Revit.Managers.Transactions ;
using Sonny.Application.Infrastructure.Revit.Services ;
using Sonny.Application.Presentation ;
using Sonny.Application.Presentation.Implements ;
using Sonny.Application.Presentation.Services ;
using Sonny.Application.UseCases.AutoColumnDimension.Services ;
using Sonny.Application.UseCases.ColumnFromCad.Implements ;
using Sonny.Application.UseCases.ColumnFromCad.Services ;
using Sonny.Application.UseCases.Services ;
using Sonny.Keygen.Services ;

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

            // Logging
            services.AddSerilogConfiguration() ;
            services.AddSingleton<IResourceHelper, ResourceHelper>() ;
            services.AddSingleton<ITransactionManagerFactory, TransactionManagerFactory>() ;
            services.AddSingleton<IFailurePreprocessorFactory, FailurePreprocessorFactory>() ;
            services.AddSingleton<IPoint3DConverter, Point3DConverter>() ;

            // Common services
            services.AddSingleton<IMessageService, MessageService>() ;
            services.AddSingleton<IUnitConverter, UnitConverter>() ;
            services.AddSingleton<ISettingsService, SettingsService>() ;

            // License validation - Keygen services
            services.AddSingleton<AuthService>() ;
            services.AddSingleton<KeygenAuthService>() ;
            services.AddSingleton<OfflineLicenseManager>() ;
            services.AddSingleton<AutoLoginService>() ;
            services.AddSingleton<UserInfoService>() ;
            services.AddSingleton<ILicenseValidator, KeygenLicenseValidator>() ;
            services.AddSingleton<ILicenseCheckService, LicenseCheckService>() ;

            // Resources initializer (must be registered after ISettingsService)
            services.AddSingleton<ResourcesInitializer>() ;

            // Language change handler (must be registered after ISettingsService)
            services.AddSingleton<LanguageChangeHandler>() ;

            // UIDocument Provider (Singleton - stores current UIDocument)
            services.AddSingleton<IUIDocumentProvider, UIDocumentProvider>() ;

            // RevitDocument (Transient - stateless passthrough to IUIDocumentProvider above).
            // Deliberately not a singleton: it owns no data worth sharing, and a fresh instance per
            // resolve means any per-run state added here later is cleared instead of leaking between runs.
            // Caveat: that only holds for consumers resolved per run. Singleton consumers
            // (ColumnDataExtractor, ElementSelector, ColumnCreationStrategyFactory, AutoColumnDimensionInteractor)
            // resolve this once and keep that instance for the whole Revit session, so RevitDocument itself
            // must read through the provider on every call and never cache the UIDocument.
            services.AddTransient<IRevitDocument, RevitDocument>() ;

            // DisplayUnitProvider (depends on IRevitDocument)
            services.AddTransient<IDisplayUnitProvider, DisplayUnitProvider>() ;

            services.AddTransient<IDimensionTypeProvider, DimensionTypeProvider>() ;

            // ViewScaleProvider (depends on IRevitDocument)
            services.AddTransient<IViewScaleProvider, ViewScaleProvider>() ;

            // CommonServices (Transient - plain aggregate of the common services above)
            services.AddTransient<ICommonServices, CommonServices>() ;

            // RevitTaskRunner (for async Revit API execution)
            services.AddSingleton<IRevitTaskRunner, RevitTaskRunner>() ;

            // ColumnFromCadFromCad
            services.AddSingleton<ICadLinkSelector, CadLinkSelector>() ;
            services.AddSingleton<IColumnModelFactory, ColumnModelFactory>() ;

            services.AddSingleton<IRectangularColumnExtractor, RectangularColumnExtractor>() ;
            services.AddSingleton<ICircularColumnExtractor, CircularColumnExtractor>() ;
            services.AddSingleton<IColumnDataExtractor, ColumnDataExtractor>() ;
            services.AddSingleton<IElementSelector, ElementSelector>() ;
            services.AddSingleton<IColumnCreationStrategyFactory, ColumnCreationStrategyFactory>() ;
            services.AddTransient<IColumnFromCadContext, ColumnFromCadContext>() ;
            services.AddTransient<IColumnFromCadInteractor, ColumnFromCadInteractor>() ;

            // AutoColumnDimension
            services.AddSingleton<IGridFinder, GridFinder>() ;
            services.AddSingleton<IDimensionCreator, DimensionCreator>() ;
            services.AddSingleton<IAutoColumnDimension, AutoColumnDimension>() ;
            services.AddSingleton<IAutoColumnDimensionInteractor, AutoColumnDimensionInteractor>() ;
            services.AddTransient<IViewModelSettingsService<ColumnFromCadSettings>>(_ =>
                new ViewModelSettingsService<ColumnFromCadSettings>()) ;

            services.AddPresentationsServices() ; // Presentation services (Views)

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
