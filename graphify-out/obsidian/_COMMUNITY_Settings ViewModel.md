---
type: community
cohesion: 0.14
members: 17
---

# Settings ViewModel

**Cohesion:** 0.14 - loosely connected
**Members:** 17 nodes

## Members
- [[dot-Cancel()_1]] - code - source/Sonny.Application.Presentation/Settings/ViewModels/SettingsViewModel.cs
- [[dot-CloseWindow()]] - code - source/Sonny.Application.Presentation/Bases/BaseViewModel.cs
- [[dot-GetDefaultDisplayUnit()]] - code - source/Sonny.Application.Domain/Services/IDisplayUnitProvider.cs
- [[dot-GetDefaultDisplayUnit()_1]] - code - source/Sonny.Application.Infrastructure/Revit/Implements/DisplayUnitProvider.cs
- [[dot-InitializeLanguageOptions()]] - code - source/Sonny.Application.Presentation/Settings/ViewModels/SettingsViewModel.cs
- [[dot-InitializeUnitOptions()]] - code - source/Sonny.Application.Presentation/Settings/ViewModels/SettingsViewModel.cs
- [[dot-LoadCurrentSettings()]] - code - source/Sonny.Application.Presentation/Settings/ViewModels/SettingsViewModel.cs
- [[dot-Save()]] - code - source/Sonny.Application.Presentation/Settings/ViewModels/SettingsViewModel.cs
- [[dot-ToString()_4]] - code - source/Sonny.Application.Domain/Entities/Settings/Models/UnitOption.cs
- [[AppDisplayUnit_1]] - code
- [[DisplayUnitProvider]] - code - source/Sonny.Application.Infrastructure/Revit/Implements/DisplayUnitProvider.cs
- [[IDisplayUnitProvider]] - code - source/Sonny.Application.Domain/Services/IDisplayUnitProvider.cs
- [[ObservableCollection_2]] - code
- [[RelayCommand_2]] - code
- [[SettingsViewModel]] - code - source/Sonny.Application.Presentation/Settings/ViewModels/SettingsViewModel.cs
- [[UnitOption]] - code - source/Sonny.Application.Domain/Entities/Settings/Models/UnitOption.cs
- [[UnitOption.cs]] - code - source/Sonny.Application.Domain/Entities/Settings/Models/UnitOption.cs

## Live Query (requires Dataview plugin)

```dataview
TABLE source_file, type FROM #community/Settings_ViewModel
SORT file.name ASC
```

## Connections to other communities
- 4 edges to [[_COMMUNITY_Domain & Infrastructure Namespaces]]
- 2 edges to [[_COMMUNITY_ViewModel Base & Messaging]]
- 2 edges to [[_COMMUNITY_ColumnFromCad ViewModel & Settings]]
- 2 edges to [[_COMMUNITY_Settings & Display Unit Contracts]]
- 2 edges to [[_COMMUNITY_Settings Service]]
- 2 edges to [[_COMMUNITY_Settings Service Tests]]
- 2 edges to [[_COMMUNITY_Async Task & View Scale]]
- 1 edge to [[_COMMUNITY_Language Option Tests]]

## Top bridge nodes
- [[dot-Save()]] - degree 6, connects to 3 communities
- [[SettingsViewModel]] - degree 11, connects to 2 communities
- [[dot-LoadCurrentSettings()]] - degree 4, connects to 2 communities
- [[dot-GetDefaultDisplayUnit()]] - degree 3, connects to 1 community
- [[UnitOption.cs]] - degree 2, connects to 1 community