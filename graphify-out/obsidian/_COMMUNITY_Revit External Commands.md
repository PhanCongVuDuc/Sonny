---
type: community
cohesion: 0.19
members: 13
---

# Revit External Commands

**Cohesion:** 0.19 - loosely connected
**Members:** 13 nodes

## Members
- [[dot-ExecuteInternal()_4]] - code - source/Sonny.Application/Commands/SettingsCommand.cs
- [[AutoColumnDimensionCommand.cs]] - code - source/Sonny.Application/Commands/AutoColumnDimensionCommand.cs
- [[ColumnFromCadCommand]] - code - source/Sonny.Application/Commands/ColumnFromCadCommand.cs
- [[ColumnFromCadCommand.cs]] - code - source/Sonny.Application/Commands/ColumnFromCadCommand.cs
- [[ElementSet_4]] - code
- [[ExternalCommandData_4]] - code
- [[LoginCommand.cs]] - code - source/Sonny.Application/Commands/LoginCommand.cs
- [[Result_4]] - code
- [[SettingsCommand]] - code - source/Sonny.Application/Commands/SettingsCommand.cs
- [[SettingsCommand.cs]] - code - source/Sonny.Application/Commands/SettingsCommand.cs
- [[Sonny.Application.Bases]] - code - source/Sonny.Application/Bases/BaseExternalCommand.cs
- [[Sonny.Application.Commands]] - code - source/Sonny.Application/Commands/AutoColumnDimensionCommand.cs
- [[Sonny.Application.Presentation.Settings.Views]] - code - source/Sonny.Application.Presentation/Settings/Views/SettingsView.xaml.cs

## Live Query (requires Dataview plugin)

```dataview
TABLE source_file, type FROM #community/Revit_External_Commands
SORT file.name ASC
```

## Connections to other communities
- 4 edges to [[_COMMUNITY_Presentation Services & Progress]]
- 2 edges to [[_COMMUNITY_External Command Base]]
- 2 edges to [[_COMMUNITY_Domain & Infrastructure Namespaces]]
- 1 edge to [[_COMMUNITY_ColumnFromCadCommand]]
- 1 edge to [[_COMMUNITY_AutoColumnDimensionCommand]]
- 1 edge to [[_COMMUNITY_LoginCommand]]
- 1 edge to [[_COMMUNITY_Sonny Ribbon Tab]]

## Top bridge nodes
- [[AutoColumnDimensionCommand.cs]] - degree 4, connects to 2 communities
- [[LoginCommand.cs]] - degree 4, connects to 2 communities
- [[ColumnFromCadCommand]] - degree 3, connects to 2 communities
- [[ColumnFromCadCommand.cs]] - degree 4, connects to 1 community
- [[SettingsCommand]] - degree 3, connects to 1 community