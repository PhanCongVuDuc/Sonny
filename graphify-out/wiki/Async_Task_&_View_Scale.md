# Async Task & View Scale

> 17 nodes · cohesion 0.12

## Key Concepts

- **.Run()** (8 connections) — `source/Sonny.Application.Presentation/AutoColumnDimension/ViewModels/AutoColumnDimensionViewModel.cs`
- **.RunAsync()** (5 connections) — `source/Sonny.Application.Domain/Services/IRevitTaskRunner.cs`
- **.LogError()** (5 connections) — `source/Sonny.Application.Presentation/Bases/BaseViewModel.cs`
- **IAutoColumnDimensionInteractor** (5 connections) — `source/Sonny.Application.UseCases/AutoColumnDimension/Services/IAutoColumnDimensionInteractor.cs`
- **IViewScaleProvider** (4 connections) — `source/Sonny.Application.Domain/Services/IViewScaleProvider.cs`
- **ViewScaleProvider** (3 connections) — `source/Sonny.Application.Infrastructure/Revit/Implements/ViewScaleProvider.cs`
- **IViewScaleProvider.cs** (2 connections) — `source/Sonny.Application.Domain/Services/IViewScaleProvider.cs`
- **.GetActiveViewScale()** (2 connections) — `source/Sonny.Application.Domain/Services/IViewScaleProvider.cs`
- **IAutoColumnDimensionInteractor.cs** (2 connections) — `source/Sonny.Application.UseCases/AutoColumnDimension/Services/IAutoColumnDimensionInteractor.cs`
- **.Execute()** (2 connections) — `source/Sonny.Application.UseCases/AutoColumnDimension/Services/IAutoColumnDimensionInteractor.cs`
- **Action** (1 connections)
- **Func** (1 connections)
- **Task** (1 connections)
- **.GetActiveViewScale()** (1 connections) — `source/Sonny.Application.Infrastructure/Revit/Implements/ViewScaleProvider.cs`
- **RelayCommand** (1 connections)
- **Task** (1 connections)
- **Exception** (1 connections)

## Relationships

- [Domain & Infrastructure Namespaces](Domain_%26_Infrastructure_Namespaces.md) (1 shared connections)
- [Settings ViewModel](Settings_ViewModel.md) (1 shared connections)
- [AutoColumnDimension Core](AutoColumnDimension_Core.md) (1 shared connections)

## Source Files

- `source/Sonny.Application.Domain/Services/IRevitTaskRunner.cs`
- `source/Sonny.Application.Domain/Services/IViewScaleProvider.cs`
- `source/Sonny.Application.Infrastructure/Revit/Implements/ViewScaleProvider.cs`
- `source/Sonny.Application.Presentation/AutoColumnDimension/ViewModels/AutoColumnDimensionViewModel.cs`
- `source/Sonny.Application.Presentation/Bases/BaseViewModel.cs`
- `source/Sonny.Application.UseCases/AutoColumnDimension/Services/IAutoColumnDimensionInteractor.cs`

## Audit Trail

- EXTRACTED: 14 (74%)
- INFERRED: 5 (26%)
- AMBIGUOUS: 0 (0%)

---

*Part of the graphify knowledge wiki. See [index](index.md) to navigate.*