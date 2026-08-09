#if NETCOREAPP
using System.Text.Json ;
#else
using Newtonsoft.Json ;
#endif
using Sonny.Application.Domain.Entities.Settings ;
using Sonny.Application.Domain.Services ;

namespace Sonny.Application.Infrastructure.Settings.Implements ;

public class SettingsService : ISettingsService
{
    private const string SettingsFileName = "SonnySettings.json" ;
    private readonly string _settingsFilePath ;
    private AppDisplayUnit? _cachedDisplayUnit ;
    private AppLanguageCode? _cachedLanguage ;

    public SettingsService()
    {
        // Store settings in user's AppData folder
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) ;
        var sonnyFolder = Path.Combine(appDataPath,
            "Sonny") ;
        Directory.CreateDirectory(sonnyFolder) ;
        _settingsFilePath = Path.Combine(sonnyFolder,
            SettingsFileName) ;
    }

    public event EventHandler<AppDisplayUnit>? DisplayUnitChanged ;

    public event EventHandler<AppLanguageCode>? LanguageChanged ;

    public AppDisplayUnit? GetDisplayUnit()
    {
        // Return cached value if available
        if (_cachedDisplayUnit != null) {
            return _cachedDisplayUnit ;
        }

        // Try to load from file
        if (File.Exists(_settingsFilePath)) {
            try {
                var settings = LoadSettingsData() ;

                if (settings?.DisplayUnit != null
                    && Enum.TryParse<AppDisplayUnit>(settings.DisplayUnit,
                        out var displayUnit)) {
                    _cachedDisplayUnit = displayUnit ;
                    return displayUnit ;
                }
            }
            catch {
                // If deserialization fails, return null
            }
        }

        return null ;
    }

    public AppDisplayUnit GetDisplayUnitOrDefault(Func<AppDisplayUnit> defaultUnitProvider) =>
        GetDisplayUnit() ?? defaultUnitProvider() ;

    public void SetDisplayUnit(AppDisplayUnit displayUnit)
    {
        _cachedDisplayUnit = displayUnit ;

        try {
            // Load existing settings to preserve other settings
            var settings = LoadSettingsData() ;
            settings.DisplayUnit = displayUnit.ToString() ;

            SaveSettingsData(settings) ;

            // Raise event to notify subscribers
            DisplayUnitChanged?.Invoke(this,
                displayUnit) ;
        }
        catch {
            // Log error but don't throw - settings are not critical
        }
    }

    public AppLanguageCode GetLanguage()
    {
        // Return cached value if available
        if (_cachedLanguage != null) {
            return _cachedLanguage.Value ;
        }

        // Try to load from file
        if (File.Exists(_settingsFilePath)) {
            try {
                var settings = LoadSettingsData() ;

                if (settings.LanguageCode != null
                    && Enum.TryParse<AppLanguageCode>(settings.LanguageCode,
                        out var languageCode)) {
                    _cachedLanguage = languageCode ;
                    return languageCode ;
                }
            }
            catch {
                // If deserialization fails, fall back to default
            }
        }

        // Fall back to default
        const AppLanguageCode defaultLanguage = AppLanguageCode.En ;
        _cachedLanguage = defaultLanguage ;
        return defaultLanguage ;
    }

    public void SetLanguage(AppLanguageCode languageCode)
    {
        _cachedLanguage = languageCode ;

        try {
            // Load existing settings to preserve other settings
            var settings = LoadSettingsData() ;
            settings.LanguageCode = languageCode.ToString() ;

            SaveSettingsData(settings) ;

            // Raise event to notify subscribers
            LanguageChanged?.Invoke(this,
                languageCode) ;
        }
        catch {
            // Log error but don't throw - settings are not critical
        }
    }

    #region Private Methods

    private SettingsData LoadSettingsData()
    {
        if (File.Exists(_settingsFilePath)) {
            try {
                var json = File.ReadAllText(_settingsFilePath) ;
#if NETCOREAPP
                return JsonSerializer.Deserialize<SettingsData>(json) ?? new SettingsData() ;
#else
                return JsonConvert.DeserializeObject<SettingsData>(json) ?? new SettingsData() ;
#endif
            }
            catch {
                return new SettingsData() ;
            }
        }

        return new SettingsData() ;
    }

    private void SaveSettingsData(SettingsData settings)
    {
#if NETCOREAPP
        var json = JsonSerializer.Serialize(settings,
            new JsonSerializerOptions { WriteIndented = true }) ;
#else
        var json = JsonConvert.SerializeObject(settings,
            Formatting.Indented) ;
#endif
        File.WriteAllText(_settingsFilePath,
            json) ;
    }

    private class SettingsData
    {
        public string? DisplayUnit { get ; set ; }
        public string? LanguageCode { get ; set ; }
    }

    #endregion
}
