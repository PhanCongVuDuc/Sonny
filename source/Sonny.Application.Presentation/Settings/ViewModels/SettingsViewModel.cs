using System.Collections.ObjectModel ;
using Sonny.Application.Domain.Entities.Settings ;
using Sonny.Application.Domain.Entities.Settings.Models ;
using Sonny.Application.Domain.Services ;
using Sonny.Application.Presentation.Bases ;
using Sonny.Application.Presentation.Services ;
using Sonny.Application.UseCases.Settings.Models ;

namespace Sonny.Application.Presentation.Settings.ViewModels ;

public partial class SettingsViewModel : BaseViewModel
{
    #region Constructor

    private readonly IDisplayUnitProvider _displayUnitProvider ;

    public SettingsViewModel(ICommonServices commonServices,
        IDisplayUnitProvider displayUnitProvider) : base(commonServices,
        displayUnitProvider)
    {
        _displayUnitProvider = displayUnitProvider ;
        InitializeUnitOptions() ;
        InitializeLanguageOptions() ;
        LoadCurrentSettings() ;
    }

    #endregion

    #region Properties for UI Binding

    public ObservableCollection<UnitOption> UnitOptions { get ; private set ; } = [] ;

    [ObservableProperty]
    private UnitOption? selectedUnitOption ;

    public ObservableCollection<LanguageOption> LanguageOptions { get ; private set ; } = [] ;

    [ObservableProperty]
    private LanguageOption? selectedLanguageOption ;

    #endregion

    #region Commands

    [RelayCommand]
    private void Save()
    {
        try {
            if (SelectedUnitOption != null) {
                SettingsService.SetDisplayUnit(SelectedUnitOption.DisplayUnit) ;
            }

            if (SelectedLanguageOption != null) {
                SettingsService.SetLanguage(SelectedLanguageOption.LanguageCode) ;
            }

            ShowInfo("Settings saved successfully") ;
            CloseWindow() ;
        }
        catch (Exception ex) {
            LogError("Failed to save settings",
                ex) ;
            ShowError($"Failed to save settings: {ex.Message}") ;
        }
    }

    [RelayCommand]
    private void Cancel() => CloseWindow() ;

    #endregion

    #region Private Methods

    private void InitializeUnitOptions() =>
        UnitOptions = new ObservableCollection<UnitOption>
        {
            new("Millimeters (mm)",
                AppDisplayUnit.Millimeters),
            new("Centimeters (cm)",
                AppDisplayUnit.Centimeters),
            new("Meters (m)",
                AppDisplayUnit.Meters),
            new("Feet (ft)",
                AppDisplayUnit.Feet),
            new("Inches (in)",
                AppDisplayUnit.Inches)
        } ;

    private void InitializeLanguageOptions() =>
        LanguageOptions =
        [
            new LanguageOption("English",
                AppLanguageCode.En),
            new LanguageOption("Vietnamese",
                AppLanguageCode.Vi)
        ] ;

    private void LoadCurrentSettings()
    {
        var currentUnit = SettingsService.GetDisplayUnitOrDefault(() => _displayUnitProvider.GetDefaultDisplayUnit()) ;
        SelectedUnitOption = UnitOptions.FirstOrDefault(u => u.DisplayUnit == currentUnit) ;

        var currentLanguage = SettingsService.GetLanguage() ;
        SelectedLanguageOption = LanguageOptions.FirstOrDefault(l => l.LanguageCode == currentLanguage) ;
    }

    #endregion
}
