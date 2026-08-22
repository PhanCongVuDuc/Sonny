using Microsoft.Extensions.DependencyInjection ;
using Sonny.Application.Domain.Entities.ColumnFromCad.Services ;
using Sonny.Application.Domain.Services ;
using Sonny.Application.Infrastructure.Features.AutoColumnDimension.Implements ;
using Sonny.Application.Infrastructure.Features.AutoColumnDimension.Services ;
using Sonny.Application.Infrastructure.Features.AutoJoin.Implements ;
using Sonny.Application.Infrastructure.Features.AutoJoin.Services ;
using Sonny.Application.Infrastructure.Features.ColumnFromCad.Implements ;
using Sonny.Application.Infrastructure.Features.ColumnFromCad.Services ;
using Sonny.Application.Infrastructure.Features.ColumnFromCad.Strategies ;
using Sonny.Application.Infrastructure.License ;
using Sonny.Application.Infrastructure.Resource.Implements ;
using Sonny.Application.Infrastructure.Revit.Implements ;
using Sonny.Application.Infrastructure.Revit.Managers.Transactions ;
using Sonny.Application.Infrastructure.Revit.Services ;
using Sonny.Application.Infrastructure.Settings.Implements ;
using Sonny.Application.UseCases.AutoColumnDimension.Services ;
using Sonny.Application.UseCases.AutoJoin.Services ;
using Sonny.Application.UseCases.ColumnFromCad.Services ;
using Sonny.Keygen.Services ;

namespace Sonny.Application.Infrastructure ;

/// <summary>
///     Service registration for the Infrastructure layer
/// </summary>
public static class ServiceRegistration
{
    /// <summary>
    ///     Adds the framework implementations owned by the Infrastructure layer to the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddRevitServices() ;
        services.AddResourceServices() ;
        services.AddLicenseServices() ;
        services.AddColumnFromCadServices() ;
        services.AddAutoColumnDimensionServices() ;
        services.AddAutoJoinServices() ;
    }

    /// <summary>
    ///     Adds the Revit API adapters: document access, transactions, units and failure handling
    /// </summary>
    private static void AddRevitServices(this IServiceCollection services)
    {
        // Holds the UIDocument of the command currently running
        services.AddSingleton<IUIDocumentProvider, UIDocumentProvider>() ;

        // RevitDocument (Transient - stateless passthrough to IUIDocumentProvider above).
        // Deliberately not a singleton: it owns no data worth sharing, and a fresh instance per
        // resolve means any per-run state added here later is cleared instead of leaking between runs.
        // Caveat: that only holds for consumers resolved per run. Singleton consumers
        // (ColumnDataExtractor, ElementSelector, ColumnCreationStrategyFactory, ColumnGeometryReader,
        // DimensionPlanExecutor) resolve this once and keep that instance for the whole Revit session,
        // so RevitDocument itself must read through the provider on every call and never cache the
        // UIDocument.
        services.AddTransient<IRevitDocument, RevitDocument>() ;

        services.AddSingleton<ITransactionManagerFactory, TransactionManagerFactory>() ;
        services.AddSingleton<IFailurePreprocessorFactory, FailurePreprocessorFactory>() ;

        // Singleton on purpose: the failure preprocessors (created by the factory above) and the
        // interactor that reads the ids after commit must share the same instance. Holds only
        // numeric ids and is Clear()-ed at the start of every run.
        services.AddSingleton<IFailingElementIdsTracker, FailingElementIdsTracker>() ;
        services.AddSingleton<IPoint3DConverter, Point3DConverter>() ;
        services.AddSingleton<IUnitConverter, UnitConverter>() ;
        services.AddSingleton<IElementSelector, ElementSelector>() ;
        services.AddSingleton<IRevitTaskRunner, RevitTaskRunner>() ;

        // Read through IRevitDocument, so they follow its per-resolve lifetime
        services.AddTransient<IDisplayUnitProvider, DisplayUnitProvider>() ;
        services.AddTransient<IDimensionTypeProvider, DimensionTypeProvider>() ;
        services.AddTransient<IViewScaleProvider, ViewScaleProvider>() ;
    }

    /// <summary>
    ///     Adds settings storage, localized resource lookup and language switching
    /// </summary>
    private static void AddResourceServices(this IServiceCollection services)
    {
        services.AddSingleton<ISettingsService, SettingsService>() ;
        services.AddSingleton<IResourceHelper, ResourceHelper>() ;
        services.AddSingleton<ResourcesInitializer>() ;
        services.AddSingleton<LanguageChangeHandler>() ;
    }

    /// <summary>
    ///     Adds the Keygen-backed license validation services
    /// </summary>
    private static void AddLicenseServices(this IServiceCollection services)
    {
        services.AddSingleton<AuthService>() ;
        services.AddSingleton<KeygenAuthService>() ;
        services.AddSingleton<OfflineLicenseManager>() ;
        services.AddSingleton<AutoLoginService>() ;
        services.AddSingleton<UserInfoService>() ;
        services.AddSingleton<ILicenseValidator, KeygenLicenseValidator>() ;
    }

    /// <summary>
    ///     Adds the CAD extraction and column creation services for the ColumnFromCad feature
    /// </summary>
    private static void AddColumnFromCadServices(this IServiceCollection services)
    {
        services.AddSingleton<ICadLinkSelector, CadLinkSelector>() ;
        services.AddSingleton<IColumnModelFactory, ColumnModelFactory>() ;
        services.AddSingleton<IRectangularColumnExtractor, RectangularColumnExtractor>() ;
        services.AddSingleton<ICircularColumnExtractor, CircularColumnExtractor>() ;
        services.AddSingleton<IColumnDataExtractor, ColumnDataExtractor>() ;
        services.AddSingleton<IColumnCreationStrategyFactory, ColumnCreationStrategyFactory>() ;

        // Transient: carries the selections of a single ColumnFromCad run
        services.AddTransient<IColumnFromCadContext, ColumnFromCadContext>() ;
    }

    /// <summary>
    ///     Adds the Revit adapters for the AutoColumnDimension feature: the geometry reader and
    ///     plan executor behind the UseCases ports, and the dimension creator they drive
    /// </summary>
    private static void AddAutoColumnDimensionServices(this IServiceCollection services)
    {
        services.AddSingleton<IDimensionCreator, DimensionCreator>() ;

        // Stateless adapters: they read through IRevitDocument on every call and never cache
        // the view, so they are safe as singletons (see the UIDocument lifetime rule)
        services.AddSingleton<IColumnGeometryReader, ColumnGeometryReader>() ;
        services.AddSingleton<IDimensionPlanExecutor, DimensionPlanExecutor>() ;
    }

    /// <summary>
    ///     Adds the Revit adapters for the AutoJoin feature: the scope reader and pair executor
    ///     behind the UseCases ports, and the pre-window environment check
    /// </summary>
    private static void AddAutoJoinServices(this IServiceCollection services)
    {
        // Stateless, reads through IRevitDocument on each call — safe as a singleton
        services.AddSingleton<IAutoJoinEnvironmentChecker, AutoJoinEnvironmentChecker>() ;

        // Transient: the pair executor snapshots the current anchor's solid during a run, and
        // the reader follows the same per-run lifetime as the interactor that owns them
        services.AddTransient<IAutoJoinScopeReader, AutoJoinScopeReader>() ;
        services.AddTransient<IAutoJoinPairExecutor, AutoJoinPairExecutor>() ;
    }
}
