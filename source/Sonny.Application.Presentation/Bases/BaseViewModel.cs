using System.Windows ;
using Serilog ;
using Sonny.Application.Domain.Entities.Settings ;
using Sonny.Application.Domain.Services ;
using Sonny.Application.Presentation.Services ;

namespace Sonny.Application.Presentation.Bases ;

/// <summary>
///     Base ViewModel with common dependencies for Sonny Application
/// </summary>
public abstract class BaseViewModel : ObservableObject
{
    #region Constructor

    protected BaseViewModel(ICommonServices commonServices,
        IDisplayUnitProvider displayUnitProvider)
    {
        MessageService = commonServices.MessageService ;
        Logger = commonServices.Logger ;
        UnitConverter = commonServices.UnitConverter ;
        SettingsService = commonServices.SettingsService ;
        ResourceHelper = commonServices.ResourceHelper ;

        // Initialize display unit from user settings (or default)
        DisplayUnit = SettingsService.GetDisplayUnitOrDefault(displayUnitProvider.GetDefaultDisplayUnit) ;

        // Subscribe to display unit changes
        SettingsService.DisplayUnitChanged += OnDisplayUnitChanged ;
    }

    private void OnDisplayUnitChanged(object? sender,
        AppDisplayUnit newUnit)
    {
        var oldUnit = DisplayUnit ;
        DisplayUnit = newUnit ;
        OnPropertyChanged(nameof( DisplayUnit )) ;
        OnPropertyChanged(nameof( DisplayUnitName )) ;

        // Allow derived classes to handle unit conversion
        OnDisplayUnitChanged(oldUnit,
            newUnit) ;
    }

    /// <summary>
    ///     Called when display unit changes, allowing derived classes to convert values
    /// </summary>
    /// <param name="oldUnit">Previous display unit</param>
    /// <param name="newUnit">New display unit</param>
    protected virtual void OnDisplayUnitChanged(AppDisplayUnit oldUnit,
        AppDisplayUnit newUnit)
    {
        // Override in derived classes to convert values when unit changes
    }

    #endregion

    #region Common Services (Dependency Injection)

    protected IMessageService MessageService { get ; }

    protected ILogger Logger { get ; }

    /// <summary>
    ///     Unit converter for converting between display units and internal units
    /// </summary>
    protected IUnitConverter UnitConverter { get ; }

    /// <summary>
    ///     Settings service for managing application preferences
    /// </summary>
    protected ISettingsService SettingsService { get ; }

    protected IResourceHelper ResourceHelper { get ; }

    #endregion

    #region Common Properties

    /// <summary>
    ///     The window (set by View)
    /// </summary>
    public Window? Window { get ; set ; }

    /// <summary>
    ///     Display unit type (default: millimeters for metric, feet for imperial)
    /// </summary>
    protected AppDisplayUnit DisplayUnit { get ; private set ; }

    /// <summary>
    ///     Display unit name for UI (e.g., "mm", "cm", "ft")
    /// </summary>
    public string DisplayUnitName => UnitConverter.GetUnitDisplayName(DisplayUnit) ;

    #endregion

    #region Common Helper Methods

    protected void CloseWindow() => Window?.Close() ;

    protected void LogInfo(string message) => Logger.Information(message) ;

    protected void LogWarning(string message) => Logger.Warning(message) ;

    protected void LogError(string message,
        Exception? ex = null)
    {
        if (ex != null) {
            Logger.Error(ex,
                message) ;
        }
        else {
            Logger.Error(message) ;
        }
    }

    protected void ShowError(string message) => MessageService.ShowError(message) ;

    protected void ShowInfo(string message) => MessageService.ShowInfo(message) ;

    protected void ShowWarning(string message) => MessageService.ShowWarning(message) ;

    #endregion
}
