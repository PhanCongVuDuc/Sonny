---
type: community
cohesion: 0.26
members: 17
---

# Settings Service Tests

**Cohesion:** 0.26 - loosely connected
**Members:** 17 nodes

## Members
- [[dot-GetLanguage()]] - code - source/Sonny.Application.Infrastructure/Settings/Implements/SettingsService.cs
- [[dot-GetLanguage_ShouldLoadFromFile_WhenSettingsFileExists()]] - code - source/Sonny.Application.Tests/Core/UnitTests/Services/SettingsServiceLanguageTests.cs
- [[dot-GetLanguage_ShouldReturnCachedValue_AfterSetLanguage()]] - code - source/Sonny.Application.Tests/Core/UnitTests/Services/SettingsServiceLanguageTests.cs
- [[dot-GetLanguage_ShouldReturnDefaultLanguage_WhenNoSettingsFileExists()]] - code - source/Sonny.Application.Tests/Core/UnitTests/Services/SettingsServiceLanguageTests.cs
- [[dot-LanguageChanged_ShouldNotRaiseEvent_WhenSettingSameLanguage()]] - code - source/Sonny.Application.Tests/Core/UnitTests/Services/SettingsServiceLanguageTests.cs
- [[dot-SetLanguage()]] - code - source/Sonny.Application.Infrastructure/Settings/Implements/SettingsService.cs
- [[dot-SetLanguage_ShouldRaiseLanguageChangedEvent()]] - code - source/Sonny.Application.Tests/Core/UnitTests/Services/SettingsServiceLanguageTests.cs
- [[dot-SetLanguage_ShouldSaveLanguageToFile()]] - code - source/Sonny.Application.Tests/Core/UnitTests/Services/SettingsServiceLanguageTests.cs
- [[dot-SetLanguage_ShouldUpdateExistingSettingsFile()]] - code - source/Sonny.Application.Tests/Core/UnitTests/Services/SettingsServiceLanguageTests.cs
- [[dot-SetLanguage_ShouldWorkWithAllLanguageCodes()]] - code - source/Sonny.Application.Tests/Core/UnitTests/Services/SettingsServiceLanguageTests.cs
- [[dot-Setup()_2]] - code - source/Sonny.Application.Tests/Core/UnitTests/Services/SettingsServiceLanguageTests.cs
- [[dot-TearDown()_1]] - code - source/Sonny.Application.Tests/Core/UnitTests/Services/SettingsServiceLanguageTests.cs
- [[SetUp_1]] - code
- [[SettingsServiceLanguageTests]] - code - source/Sonny.Application.Tests/Core/UnitTests/Services/SettingsServiceLanguageTests.cs
- [[TearDown]] - code
- [[Test_5]] - code
- [[string_11]] - code

## Live Query (requires Dataview plugin)

```dataview
TABLE source_file, type FROM #community/Settings_Service_Tests
SORT file.name ASC
```

## Connections to other communities
- 6 edges to [[_COMMUNITY_Settings Service]]
- 2 edges to [[_COMMUNITY_Settings ViewModel]]
- 2 edges to [[_COMMUNITY_Language Change Handling]]
- 1 edge to [[_COMMUNITY_Domain & Infrastructure Namespaces]]

## Top bridge nodes
- [[dot-SetLanguage()]] - degree 12, connects to 2 communities
- [[dot-GetLanguage()]] - degree 11, connects to 2 communities
- [[SettingsServiceLanguageTests]] - degree 13, connects to 1 community