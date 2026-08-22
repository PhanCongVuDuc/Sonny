// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.DependencyInjection ;
using Sonny.Application.Domain.Services ;
using Sonny.Application.Presentation.AutoColumnDimension.ViewModels ;
using Sonny.Application.Presentation.AutoColumnDimension.Views ;
using Sonny.Application.Presentation.AutoJoin.ViewModels ;
using Sonny.Application.Presentation.AutoJoin.Views ;
using Sonny.Application.Presentation.ColumnFromCad.ViewModels ;
using Sonny.Application.Presentation.ColumnFromCad.Views ;
using Sonny.Application.Presentation.Implements ;
using Sonny.Application.Presentation.Services ;
using Sonny.Application.Presentation.Settings.ViewModels ;
using Sonny.Application.Presentation.Settings.Views ;
using Sonny.Application.Presentation.Views ;

namespace Sonny.Application.Presentation ;

/// <summary>
///     Service registration for Sonny.Application.Presentation Layer
/// </summary>
public static class ServiceRegistration
{
    /// <summary>
    ///     Adds Sonny.Application.Presentation Layer services to the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    public static void AddPresentationsServices(this IServiceCollection services)
    {
        // User-facing dialogs and the aggregate the ViewModels take as a single dependency
        services.AddSingleton<IMessageService, MessageService>() ;
        services.AddTransient<ICommonServices, CommonServices>() ;

        // Open generic: any ViewModel settings type is served without touching the composition root
        services.AddTransient(typeof( IViewModelSettingsService<> ),
            typeof( ViewModelSettingsService<> )) ;

        services.AddTransient<ProgressView>() ;
        services.AddTransient<IProgressReporter, ProgressReporter>() ;

        // Register Views
        services.AddTransient<AutoColumnDimensionViewModel>() ;
        services.AddTransient<AutoColumnDimensionView>() ;

        services.AddTransient<ColumnFromCadViewModel>() ;
        services.AddTransient<ColumnFromCadView>() ;

        services.AddTransient<AutoJoinViewModel>() ;
        services.AddTransient<AutoJoinView>() ;

        // Settings services
        services.AddTransient<SettingsViewModel>() ;
        services.AddTransient<SettingsView>() ;
    }
}
