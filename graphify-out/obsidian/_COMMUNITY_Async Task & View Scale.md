---
type: community
cohesion: 0.12
members: 17
---

# Async Task & View Scale

**Cohesion:** 0.12 - loosely connected
**Members:** 17 nodes

## Members
- [[dot-Execute()_7]] - code - source/Sonny.Application.UseCases/AutoColumnDimension/Services/IAutoColumnDimensionInteractor.cs
- [[dot-GetActiveViewScale()]] - code - source/Sonny.Application.Domain/Services/IViewScaleProvider.cs
- [[dot-GetActiveViewScale()_1]] - code - source/Sonny.Application.Infrastructure/Revit/Implements/ViewScaleProvider.cs
- [[dot-LogError()]] - code - source/Sonny.Application.Presentation/Bases/BaseViewModel.cs
- [[dot-Run()]] - code - source/Sonny.Application.Presentation/AutoColumnDimension/ViewModels/AutoColumnDimensionViewModel.cs
- [[dot-RunAsync()_1]] - code - source/Sonny.Application.Domain/Services/IRevitTaskRunner.cs
- [[Action_1]] - code
- [[Exception_1]] - code
- [[Func_3]] - code
- [[IAutoColumnDimensionInteractor]] - code - source/Sonny.Application.UseCases/AutoColumnDimension/Services/IAutoColumnDimensionInteractor.cs
- [[IAutoColumnDimensionInteractor.cs]] - code - source/Sonny.Application.UseCases/AutoColumnDimension/Services/IAutoColumnDimensionInteractor.cs
- [[IViewScaleProvider]] - code - source/Sonny.Application.Domain/Services/IViewScaleProvider.cs
- [[IViewScaleProvider.cs]] - code - source/Sonny.Application.Domain/Services/IViewScaleProvider.cs
- [[RelayCommand_3]] - code
- [[Task_12]] - code
- [[Task_13]] - code
- [[ViewScaleProvider]] - code - source/Sonny.Application.Infrastructure/Revit/Implements/ViewScaleProvider.cs

## Live Query (requires Dataview plugin)

```dataview
TABLE source_file, type FROM #community/Async_Task__View_Scale
SORT file.name ASC
```

## Connections to other communities
- 4 edges to [[_COMMUNITY_AutoColumnDimension ViewModel]]
- 2 edges to [[_COMMUNITY_Settings ViewModel]]
- 2 edges to [[_COMMUNITY_Domain & Infrastructure Namespaces]]
- 1 edge to [[_COMMUNITY_Integration Test Fixtures]]
- 1 edge to [[_COMMUNITY_Revit Task Runner]]
- 1 edge to [[_COMMUNITY_ViewModel Base & Messaging]]
- 1 edge to [[_COMMUNITY_AutoColumnDimension Interactor]]
- 1 edge to [[_COMMUNITY_AutoColumnDimension Core]]

## Top bridge nodes
- [[dot-Run()]] - degree 8, connects to 1 community
- [[IViewScaleProvider.cs]] - degree 2, connects to 1 community
- [[IAutoColumnDimensionInteractor.cs]] - degree 2, connects to 1 community