using Sonny.Application.Domain.Services ;
using Sonny.Application.Presentation.Services ;

namespace Sonny.Application.Presentation.Bases ;

/// <summary>
///     Base class for ViewModels that need settings management
/// </summary>
/// <typeparam name="TSettings">Settings model type</typeparam>
public abstract class BaseViewModelWithSettings<TSettings> : BaseViewModel where TSettings : class, new()
{
    private readonly IViewModelSettingsService<TSettings> _settingsService ;

    private bool _isLoadingSettings ;

    protected BaseViewModelWithSettings(ICommonServices commonServices,
        IDisplayUnitProvider displayUnitProvider,
        IViewModelSettingsService<TSettings> settingsService) : base(commonServices,
        displayUnitProvider) =>
        _settingsService = settingsService ;

    /// <summary>
    ///     Called after all data is initialized, before loading settings.
    ///     Override this method to initialize ViewModel data (load collections, set defaults, etc.)
    /// </summary>
    protected abstract void OnDataInitialized() ;

    /// <summary>
    ///     Called to apply loaded settings to ViewModel properties.
    ///     Override this method to map settings properties to ViewModel properties.
    /// </summary>
    /// <param name="settings">Settings loaded from storage</param>
    protected abstract void ApplySettings(TSettings settings) ;

    /// <summary>
    ///     Called to create settings object from current ViewModel state.
    ///     Override this method to map ViewModel properties to settings object.
    /// </summary>
    /// <returns>Settings object with current ViewModel state</returns>
    protected abstract TSettings CreateSettings() ;

    /// <summary>
    ///     Initialize data and load settings.
    ///     Call this method in constructor after setting up dependencies.
    /// </summary>
    protected void InitializeWithSettings()
    {
        OnDataInitialized() ;
        LoadSettings() ;
    }

    private void LoadSettings()
    {
        var settings = _settingsService.LoadSettings() ;
        if (settings == null) {
            return ;
        }

        _isLoadingSettings = true ;
        try {
            ApplySettings(settings) ;
        }
        finally {
            _isLoadingSettings = false ;
        }
    }

    /// <summary>
    ///     Saves current settings to storage.
    ///     This method checks if settings are currently being loaded to avoid saving during load.
    /// </summary>
    protected void SaveSettings()
    {
        if (_isLoadingSettings) {
            return ; // Don't save while loading
        }

        var settings = CreateSettings() ;
        _settingsService.SaveSettings(settings) ;
    }
}
