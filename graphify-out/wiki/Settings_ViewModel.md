# Settings ViewModel

> 17 nodes · cohesion 0.14

## Key Concepts

- **SettingsViewModel** (11 connections) — `source/Sonny.Application.Presentation/Settings/ViewModels/SettingsViewModel.cs`
- **.CloseWindow()** (6 connections) — `source/Sonny.Application.Presentation/Bases/BaseViewModel.cs`
- **.Save()** (6 connections) — `source/Sonny.Application.Presentation/Settings/ViewModels/SettingsViewModel.cs`
- **UnitOption** (4 connections) — `source/Sonny.Application.Domain/Entities/Settings/Models/UnitOption.cs`
- **IDisplayUnitProvider** (4 connections) — `source/Sonny.Application.Domain/Services/IDisplayUnitProvider.cs`
- **.LoadCurrentSettings()** (4 connections) — `source/Sonny.Application.Presentation/Settings/ViewModels/SettingsViewModel.cs`
- **.GetDefaultDisplayUnit()** (3 connections) — `source/Sonny.Application.Domain/Services/IDisplayUnitProvider.cs`
- **DisplayUnitProvider** (3 connections) — `source/Sonny.Application.Infrastructure/Revit/Implements/DisplayUnitProvider.cs`
- **.Cancel()** (3 connections) — `source/Sonny.Application.Presentation/Settings/ViewModels/SettingsViewModel.cs`
- **UnitOption.cs** (2 connections) — `source/Sonny.Application.Domain/Entities/Settings/Models/UnitOption.cs`
- **.GetDefaultDisplayUnit()** (2 connections) — `source/Sonny.Application.Infrastructure/Revit/Implements/DisplayUnitProvider.cs`
- **RelayCommand** (2 connections)
- **AppDisplayUnit** (1 connections)
- **.ToString()** (1 connections) — `source/Sonny.Application.Domain/Entities/Settings/Models/UnitOption.cs`
- **ObservableCollection** (1 connections)
- **.InitializeLanguageOptions()** (1 connections) — `source/Sonny.Application.Presentation/Settings/ViewModels/SettingsViewModel.cs`
- **.InitializeUnitOptions()** (1 connections) — `source/Sonny.Application.Presentation/Settings/ViewModels/SettingsViewModel.cs`

## Relationships

- [Settings & Display Unit Contracts](Settings_%26_Display_Unit_Contracts.md) (2 shared connections)
- [Settings Service](Settings_Service.md) (2 shared connections)
- [Settings Service Tests](Settings_Service_Tests.md) (2 shared connections)
- [Domain & Infrastructure Namespaces](Domain_%26_Infrastructure_Namespaces.md) (1 shared connections)
- [Language Option Tests](Language_Option_Tests.md) (1 shared connections)
- [ViewModel Base & Messaging](ViewModel_Base_%26_Messaging.md) (1 shared connections)
- [Async Task & View Scale](Async_Task_%26_View_Scale.md) (1 shared connections)

## Source Files

- `source/Sonny.Application.Domain/Entities/Settings/Models/UnitOption.cs`
- `source/Sonny.Application.Domain/Services/IDisplayUnitProvider.cs`
- `source/Sonny.Application.Infrastructure/Revit/Implements/DisplayUnitProvider.cs`
- `source/Sonny.Application.Presentation/Bases/BaseViewModel.cs`
- `source/Sonny.Application.Presentation/Settings/ViewModels/SettingsViewModel.cs`

## Audit Trail

- EXTRACTED: 25 (86%)
- INFERRED: 4 (14%)
- AMBIGUOUS: 0 (0%)

---

*Part of the graphify knowledge wiki. See [index](index.md) to navigate.*