# Settings Service Tests

> 17 nodes · cohesion 0.26

## Key Concepts

- **SettingsServiceLanguageTests** (13 connections) — `source/Sonny.Application.Tests/Core/UnitTests/Services/SettingsServiceLanguageTests.cs`
- **.SetLanguage()** (12 connections) — `source/Sonny.Application.Infrastructure/Settings/Implements/SettingsService.cs`
- **.GetLanguage()** (11 connections) — `source/Sonny.Application.Infrastructure/Settings/Implements/SettingsService.cs`
- **Test** (8 connections)
- **.GetLanguage_ShouldLoadFromFile_WhenSettingsFileExists()** (4 connections) — `source/Sonny.Application.Tests/Core/UnitTests/Services/SettingsServiceLanguageTests.cs`
- **.GetLanguage_ShouldReturnCachedValue_AfterSetLanguage()** (4 connections) — `source/Sonny.Application.Tests/Core/UnitTests/Services/SettingsServiceLanguageTests.cs`
- **.LanguageChanged_ShouldNotRaiseEvent_WhenSettingSameLanguage()** (4 connections) — `source/Sonny.Application.Tests/Core/UnitTests/Services/SettingsServiceLanguageTests.cs`
- **.SetLanguage_ShouldSaveLanguageToFile()** (4 connections) — `source/Sonny.Application.Tests/Core/UnitTests/Services/SettingsServiceLanguageTests.cs`
- **.SetLanguage_ShouldUpdateExistingSettingsFile()** (4 connections) — `source/Sonny.Application.Tests/Core/UnitTests/Services/SettingsServiceLanguageTests.cs`
- **.SetLanguage_ShouldWorkWithAllLanguageCodes()** (4 connections) — `source/Sonny.Application.Tests/Core/UnitTests/Services/SettingsServiceLanguageTests.cs`
- **.GetLanguage_ShouldReturnDefaultLanguage_WhenNoSettingsFileExists()** (3 connections) — `source/Sonny.Application.Tests/Core/UnitTests/Services/SettingsServiceLanguageTests.cs`
- **.SetLanguage_ShouldRaiseLanguageChangedEvent()** (3 connections) — `source/Sonny.Application.Tests/Core/UnitTests/Services/SettingsServiceLanguageTests.cs`
- **.Setup()** (2 connections) — `source/Sonny.Application.Tests/Core/UnitTests/Services/SettingsServiceLanguageTests.cs`
- **.TearDown()** (2 connections) — `source/Sonny.Application.Tests/Core/UnitTests/Services/SettingsServiceLanguageTests.cs`
- **SetUp** (1 connections)
- **string** (1 connections)
- **TearDown** (1 connections)

## Relationships

- [Settings Service](Settings_Service.md) (4 shared connections)
- [Language Change Handling](Language_Change_Handling.md) (2 shared connections)

## Source Files

- `source/Sonny.Application.Infrastructure/Settings/Implements/SettingsService.cs`
- `source/Sonny.Application.Tests/Core/UnitTests/Services/SettingsServiceLanguageTests.cs`

## Audit Trail

- EXTRACTED: 27 (66%)
- INFERRED: 14 (34%)
- AMBIGUOUS: 0 (0%)

---

*Part of the graphify knowledge wiki. See [index](index.md) to navigate.*