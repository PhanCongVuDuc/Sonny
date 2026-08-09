using Sonny.Application.Domain.Services ;
using Sonny.Application.Infrastructure.Revit.Implements ;

namespace Sonny.Application.Infrastructure.Resource.Implements ;

public class ResourcesInitializer(ISettingsService settingsService)
{
    /// <summary>
    ///     Initialize application resources with current language from settings
    /// </summary>
    public void Initialize()
    {
        var currentLanguage = settingsService.GetLanguage() ;
        var resourceManagerLanguageCode = LanguageCodeConverter.ToResourceManagerLanguageCode(currentLanguage) ;
        SonnyResourcesInitializer.Initialize(resourceManagerLanguageCode) ;
    }
}
