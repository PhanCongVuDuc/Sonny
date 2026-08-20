# ColumnFromCad Interactor

> 31 nodes · cohesion 0.10

## Key Concepts

- **ColumnCreationContext** (13 connections) — `source/Sonny.Application.Domain/Entities/ColumnFromCad/Contexts/ColumnCreationContext.cs`
- **ColumnModel** (11 connections) — `source/Sonny.Application.Domain/Entities/ColumnFromCad/Models/ColumnModel.cs`
- **ColumnFromCadInteractor** (6 connections) — `source/Sonny.Application.UseCases/ColumnFromCad/Implements/ColumnFromCadInteractor.cs`
- **IColumnFromCadInteractor** (6 connections) — `source/Sonny.Application.UseCases/ColumnFromCad/Services/IColumnFromCadInteractor.cs`
- **IColumnCreationStrategy** (5 connections) — `source/Sonny.Application.Domain/Entities/ColumnFromCad/Services/IColumnCreationStrategy.cs`
- **.Execute()** (5 connections) — `source/Sonny.Application.UseCases/ColumnFromCad/Implements/ColumnFromCadInteractor.cs`
- **.ExtractColumnData()** (5 connections) — `source/Sonny.Application.UseCases/ColumnFromCad/Implements/ColumnFromCadInteractor.cs`
- **.CreateStrategy()** (4 connections) — `source/Sonny.Application.Domain/Entities/ColumnFromCad/Services/IColumnCreationStrategyFactory.cs`
- **.Extract()** (4 connections) — `source/Sonny.Application.Domain/Entities/ColumnFromCad/Services/IColumnDataExtractor.cs`
- **.Extract()** (4 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Implements/ColumnDataExtractor.cs`
- **.CreateStrategy()** (4 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Strategies/ColumnCreationStrategyFactory.cs`
- **.CreateColumns()** (4 connections) — `source/Sonny.Application.UseCases/ColumnFromCad/Implements/ColumnFromCadInteractor.cs`
- **.Execute()** (4 connections) — `source/Sonny.Application.UseCases/ColumnFromCad/Services/IColumnFromCadInteractor.cs`
- **.ExtractColumnData()** (4 connections) — `source/Sonny.Application.UseCases/ColumnFromCad/Services/IColumnFromCadInteractor.cs`
- **IColumnCreationStrategyFactory** (3 connections) — `source/Sonny.Application.Domain/Entities/ColumnFromCad/Services/IColumnCreationStrategyFactory.cs`
- **IColumnDataExtractor** (3 connections) — `source/Sonny.Application.Domain/Entities/ColumnFromCad/Services/IColumnDataExtractor.cs`
- **ColumnDataExtractor** (3 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Implements/ColumnDataExtractor.cs`
- **ColumnCreationStrategyFactory** (3 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Strategies/ColumnCreationStrategyFactory.cs`
- **.CreateColumns()** (3 connections) — `source/Sonny.Application.UseCases/ColumnFromCad/Services/IColumnFromCadInteractor.cs`
- **ColumnModel.cs** (2 connections) — `source/Sonny.Application.Domain/Entities/ColumnFromCad/Models/ColumnModel.cs`
- **IColumnCreationStrategy.cs** (2 connections) — `source/Sonny.Application.Domain/Entities/ColumnFromCad/Services/IColumnCreationStrategy.cs`
- **List** (2 connections)
- **Point3D** (1 connections)
- **.Execute()** (1 connections) — `source/Sonny.Application.Domain/Entities/ColumnFromCad/Services/IColumnCreationStrategy.cs`
- **List** (1 connections)
- *... and 6 more nodes in this community*

## Relationships

- [Domain & Infrastructure Namespaces](Domain_%26_Infrastructure_Namespaces.md) (2 shared connections)
- [ColumnFromCad ViewModel & Settings](ColumnFromCad_ViewModel_%26_Settings.md) (1 shared connections)

## Source Files

- `source/Sonny.Application.Domain/Entities/ColumnFromCad/Contexts/ColumnCreationContext.cs`
- `source/Sonny.Application.Domain/Entities/ColumnFromCad/Models/ColumnModel.cs`
- `source/Sonny.Application.Domain/Entities/ColumnFromCad/Services/IColumnCreationStrategy.cs`
- `source/Sonny.Application.Domain/Entities/ColumnFromCad/Services/IColumnCreationStrategyFactory.cs`
- `source/Sonny.Application.Domain/Entities/ColumnFromCad/Services/IColumnDataExtractor.cs`
- `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Implements/ColumnDataExtractor.cs`
- `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Strategies/ColumnCreationStrategyFactory.cs`
- `source/Sonny.Application.UseCases/ColumnFromCad/Implements/ColumnFromCadInteractor.cs`
- `source/Sonny.Application.UseCases/ColumnFromCad/Services/IColumnFromCadInteractor.cs`

## Audit Trail

- EXTRACTED: 49 (100%)
- INFERRED: 0 (0%)
- AMBIGUOUS: 0 (0%)

---

*Part of the graphify knowledge wiki. See [index](index.md) to navigate.*