---
type: community
cohesion: 0.22
members: 11
---

# Language Change Handling

**Cohesion:** 0.22 - loosely connected
**Members:** 11 nodes

## Members
- [[dot-FromResourceManagerLanguageCode()]] - code - source/Sonny.Application.Infrastructure/Revit/Implements/LanguageCodeConverter.cs
- [[dot-GetLanguage()_1]] - code - source/Sonny.Application.Domain/Services/ISettingsService.cs
- [[dot-Initialize()_1]] - code - source/Sonny.Application.Infrastructure/Resource/Implements/ResourcesInitializer.cs
- [[dot-OnLanguageChanged()]] - code - source/Sonny.Application.Infrastructure/Revit/Implements/LanguageChangeHandler.cs
- [[dot-SetLanguage()_1]] - code - source/Sonny.Application.Domain/Services/ISettingsService.cs
- [[dot-ToResourceManagerLanguageCode()]] - code - source/Sonny.Application.Infrastructure/Revit/Implements/LanguageCodeConverter.cs
- [[AppLanguageCode]] - code - source/Sonny.Application.Domain/Entities/Settings/LanguageCode.cs
- [[LanguageChangeHandler]] - code - source/Sonny.Application.Infrastructure/Revit/Implements/LanguageChangeHandler.cs
- [[LanguageCodeConverter]] - code - source/Sonny.Application.Infrastructure/Revit/Implements/LanguageCodeConverter.cs
- [[ResourcesInitializer]] - code - source/Sonny.Application.Infrastructure/Resource/Implements/ResourcesInitializer.cs
- [[SettingsLanguageCode.cs]] - code - source/Sonny.Application.Domain/Entities/Settings/LanguageCode.cs

## Live Query (requires Dataview plugin)

```dataview
TABLE source_file, type FROM #community/Language_Change_Handling
SORT file.name ASC
```

## Connections to other communities
- 3 edges to [[_COMMUNITY_Localization Initialization]]
- 3 edges to [[_COMMUNITY_Domain & Infrastructure Namespaces]]
- 2 edges to [[_COMMUNITY_Settings & Display Unit Contracts]]
- 2 edges to [[_COMMUNITY_Settings Service Tests]]
- 1 edge to [[_COMMUNITY_Language Option Tests]]
- 1 edge to [[_COMMUNITY_Settings Service]]
- 1 edge to [[_COMMUNITY_Host & Resource Bootstrap]]

## Top bridge nodes
- [[dot-ToResourceManagerLanguageCode()]] - degree 5, connects to 1 community
- [[dot-Initialize()_1]] - degree 3, connects to 1 community
- [[dot-FromResourceManagerLanguageCode()]] - degree 3, connects to 1 community
- [[SettingsLanguageCode.cs]] - degree 2, connects to 1 community