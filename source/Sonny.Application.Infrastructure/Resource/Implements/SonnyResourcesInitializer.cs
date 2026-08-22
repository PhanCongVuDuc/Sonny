using Sonny.ResourceManager ;

namespace Sonny.Application.Infrastructure.Resource.Implements ;

/// <summary>
///     Initializes and registers all Sonny resources with ResourceDictionaryManager
///     Automatically loads resources when assembly is loaded
/// </summary>
public static class SonnyResourcesInitializer
{
    private static bool s_isInitialized ;

    /// <summary>
    ///     Initialize all Sonny resources
    ///     Should be called once when application starts
    /// </summary>
    /// <param name="languageCode">Language code to load resources</param>
    public static void Initialize(LanguageCode languageCode = LanguageCode.En)
    {
        if (s_isInitialized) {
            return ;
        }

        RegisterAllResources(languageCode) ;
        LoadAllResources(languageCode) ;

        s_isInitialized = true ;
    }

    /// <summary>
    ///     Register all Sonny resources with ResourceDictionaryManager
    /// </summary>
    private static void RegisterAllResources(LanguageCode languageCode)
    {
        var manager = ResourceDictionaryManager.Instance ;

        // Register Common resource (must be loaded first for shared buttons)
        manager.RegisterResource("Sonny.Application",
            "Resources/Languages/Common/Common",
            languageCode) ;

        // Register AutoColumnDimension resource
        manager.RegisterResource("Sonny.Application",
            "Resources/Languages/AutoColumnDimension/AutoColumnDimension",
            languageCode) ;

        // Register ColumnFromCad resource
        manager.RegisterResource("Sonny.Application",
            "Resources/Languages/ColumnFromCad/ColumnFromCad",
            languageCode) ;

        // Register AutoJoin resource
        manager.RegisterResource("Sonny.Application",
            "Resources/Languages/AutoJoin/AutoJoin",
            languageCode) ;
    }

    /// <summary>
    ///     Load all registered resources
    /// </summary>
    /// <param name="languageCode">Language code to load</param>
    public static void LoadAllResources(LanguageCode languageCode = LanguageCode.En)
    {
        var manager = ResourceDictionaryManager.Instance ;
        manager.LoadAllResources(languageCode) ;
    }
}
