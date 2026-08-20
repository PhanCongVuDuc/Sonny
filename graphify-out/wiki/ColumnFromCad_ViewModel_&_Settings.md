# ColumnFromCad ViewModel & Settings

> 52 nodes · cohesion 0.06

## Key Concepts

- **ColumnFromCadViewModel** (24 connections) — `source/Sonny.Application.Presentation/ColumnFromCad/ViewModels/ColumnFromCadViewModel.cs`
- **BaseViewModelWithSettings** (11 connections) — `source/Sonny.Application.Presentation/Bases/BaseViewModelWithSettings.cs`
- **FamilyModel** (9 connections) — `source/Sonny.Application.Domain/Entities/Settings/Models/FamilyModel.cs`
- **IColumnFromCadContext** (8 connections) — `source/Sonny.Application.UseCases/ColumnFromCad/Services/IColumnFromCadContext.cs`
- **ColumnFromCadContext** (7 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Implements/ColumnFromCadContext.cs`
- **.Execute()** (7 connections) — `source/Sonny.Application.Presentation/ColumnFromCad/ViewModels/ColumnFromCadViewModel.cs`
- **ColumnFromCadSettings** (5 connections) — `source/Sonny.Application.Domain/Entities/ColumnFromCad/Models/ColumnFromCadSettings.cs`
- **LevelModel** (5 connections) — `source/Sonny.Application.Domain/Entities/Settings/Models/LevelModel.cs`
- **.LoadCircularColumnParameters()** (5 connections) — `source/Sonny.Application.Presentation/ColumnFromCad/ViewModels/ColumnFromCadViewModel.cs`
- **.LoadRectangularColumnParameters()** (5 connections) — `source/Sonny.Application.Presentation/ColumnFromCad/ViewModels/ColumnFromCadViewModel.cs`
- **ViewModelSettingsService** (5 connections) — `source/Sonny.Application.Presentation/Implements/ViewModelSettingsService.cs`
- **IViewModelSettingsService** (5 connections) — `source/Sonny.Application.Presentation/Services/IViewModelSettingsService.cs`
- **.LoadSettings()** (4 connections) — `source/Sonny.Application.Presentation/Bases/BaseViewModelWithSettings.cs`
- **.ApplySettings()** (4 connections) — `source/Sonny.Application.Presentation/ColumnFromCad/ViewModels/ColumnFromCadViewModel.cs`
- **.LoadColumnFamilies()** (4 connections) — `source/Sonny.Application.Presentation/ColumnFromCad/ViewModels/ColumnFromCadViewModel.cs`
- **.InitializeWithSettings()** (3 connections) — `source/Sonny.Application.Presentation/Bases/BaseViewModelWithSettings.cs`
- **.SaveSettings()** (3 connections) — `source/Sonny.Application.Presentation/Bases/BaseViewModelWithSettings.cs`
- **.Cancel()** (3 connections) — `source/Sonny.Application.Presentation/ColumnFromCad/ViewModels/ColumnFromCadViewModel.cs`
- **.CreateSettings()** (3 connections) — `source/Sonny.Application.Presentation/ColumnFromCad/ViewModels/ColumnFromCadViewModel.cs`
- **.OnDataInitialized()** (3 connections) — `source/Sonny.Application.Presentation/ColumnFromCad/ViewModels/ColumnFromCadViewModel.cs`
- **.OnSelectedCircularColumnFamilyChanged()** (3 connections) — `source/Sonny.Application.Presentation/ColumnFromCad/ViewModels/ColumnFromCadViewModel.cs`
- **.OnSelectedRectangularColumnFamilyChanged()** (3 connections) — `source/Sonny.Application.Presentation/ColumnFromCad/ViewModels/ColumnFromCadViewModel.cs`
- **IColumnFromCadContext.cs** (3 connections) — `source/Sonny.Application.UseCases/ColumnFromCad/Services/IColumnFromCadContext.cs`
- **ColumnFromCadSettings.cs** (2 connections) — `source/Sonny.Application.Domain/Entities/ColumnFromCad/Models/ColumnFromCadSettings.cs`
- **FamilyModel.cs** (2 connections) — `source/Sonny.Application.Domain/Entities/Settings/Models/FamilyModel.cs`
- *... and 27 more nodes in this community*

## Relationships

- [Domain & Infrastructure Namespaces](Domain_%26_Infrastructure_Namespaces.md) (6 shared connections)
- [ColumnFromCad Interactor](ColumnFromCad_Interactor.md) (2 shared connections)
- [Settings ViewModel](Settings_ViewModel.md) (2 shared connections)
- [ViewModel Base & Messaging](ViewModel_Base_%26_Messaging.md) (1 shared connections)
- [Unit Converter & Tests](Unit_Converter_%26_Tests.md) (1 shared connections)

## Source Files

- `source/Sonny.Application.Domain/Entities/ColumnFromCad/Models/ColumnFromCadSettings.cs`
- `source/Sonny.Application.Domain/Entities/Settings/Models/FamilyModel.cs`
- `source/Sonny.Application.Domain/Entities/Settings/Models/LevelModel.cs`
- `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Implements/ColumnFromCadContext.cs`
- `source/Sonny.Application.Presentation/Bases/BaseViewModelWithSettings.cs`
- `source/Sonny.Application.Presentation/ColumnFromCad/ViewModels/ColumnFromCadViewModel.cs`
- `source/Sonny.Application.Presentation/Implements/ViewModelSettingsService.cs`
- `source/Sonny.Application.Presentation/Services/IViewModelSettingsService.cs`
- `source/Sonny.Application.UseCases/ColumnFromCad/Services/IColumnFromCadContext.cs`

## Audit Trail

- EXTRACTED: 85 (94%)
- INFERRED: 5 (6%)
- AMBIGUOUS: 0 (0%)

---

*Part of the graphify knowledge wiki. See [index](index.md) to navigate.*