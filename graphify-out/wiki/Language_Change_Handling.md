# Language Change Handling

> 11 nodes · cohesion 0.22

## Key Concepts

- **AppLanguageCode** (10 connections) — `source/Sonny.Application.Domain/Entities/Settings/LanguageCode.cs`
- **.ToResourceManagerLanguageCode()** (5 connections) — `source/Sonny.Application.Infrastructure/Revit/Implements/LanguageCodeConverter.cs`
- **.Initialize()** (3 connections) — `source/Sonny.Application.Infrastructure/Resource/Implements/ResourcesInitializer.cs`
- **.OnLanguageChanged()** (3 connections) — `source/Sonny.Application.Infrastructure/Revit/Implements/LanguageChangeHandler.cs`
- **LanguageCodeConverter** (3 connections) — `source/Sonny.Application.Infrastructure/Revit/Implements/LanguageCodeConverter.cs`
- **.FromResourceManagerLanguageCode()** (3 connections) — `source/Sonny.Application.Infrastructure/Revit/Implements/LanguageCodeConverter.cs`
- **Settings/LanguageCode.cs** (2 connections) — `source/Sonny.Application.Domain/Entities/Settings/LanguageCode.cs`
- **.GetLanguage()** (2 connections) — `source/Sonny.Application.Domain/Services/ISettingsService.cs`
- **.SetLanguage()** (2 connections) — `source/Sonny.Application.Domain/Services/ISettingsService.cs`
- **ResourcesInitializer** (2 connections) — `source/Sonny.Application.Infrastructure/Resource/Implements/ResourcesInitializer.cs`
- **LanguageChangeHandler** (2 connections) — `source/Sonny.Application.Infrastructure/Revit/Implements/LanguageChangeHandler.cs`

## Relationships

- [Localization Initialization](Localization_Initialization.md) (3 shared connections)
- [Domain & Infrastructure Namespaces](Domain_%26_Infrastructure_Namespaces.md) (1 shared connections)

## Source Files

- `source/Sonny.Application.Domain/Entities/Settings/LanguageCode.cs`
- `source/Sonny.Application.Domain/Services/ISettingsService.cs`
- `source/Sonny.Application.Infrastructure/Resource/Implements/ResourcesInitializer.cs`
- `source/Sonny.Application.Infrastructure/Revit/Implements/LanguageChangeHandler.cs`
- `source/Sonny.Application.Infrastructure/Revit/Implements/LanguageCodeConverter.cs`

## Audit Trail

- EXTRACTED: 16 (100%)
- INFERRED: 0 (0%)
- AMBIGUOUS: 0 (0%)

---

*Part of the graphify knowledge wiki. See [index](index.md) to navigate.*