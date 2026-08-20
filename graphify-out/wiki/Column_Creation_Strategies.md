# Column Creation Strategies

> 30 nodes · cohesion 0.10

## Key Concepts

- **ColumnCreationStrategy** (13 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Strategies/ColumnCreationStrategy.cs`
- **Point3D** (5 connections) — `source/Sonny.Application.Domain/Entities/Point3D.cs`
- **.GetOrCreateCircularFamilySymbol()** (5 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Strategies/CircularColumnCreationStrategy.cs`
- **RectangularColumnCreationStrategy** (5 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Strategies/RectangularColumnCreationStrategy.cs`
- **.GetOrCreateRectangularFamilySymbol()** (5 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Strategies/RectangularColumnCreationStrategy.cs`
- **.ToXyz()** (5 connections) — `source/Sonny.Application.Infrastructure/Revit/Implements/Point3DConverter.cs`
- **IPoint3DConverter** (5 connections) — `source/Sonny.Application.Infrastructure/Revit/Services/IPoint3DConverter.cs`
- **CircularColumnCreationStrategy** (4 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Strategies/CircularColumnCreationStrategy.cs`
- **.Execute()** (4 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Strategies/ColumnCreationStrategy.cs`
- **.GetDoubleValue()** (4 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Strategies/ColumnCreationStrategy.cs`
- **Point3DConverter** (4 connections) — `source/Sonny.Application.Infrastructure/Revit/Implements/Point3DConverter.cs`
- **.GetOrCreateFamilySymbol()** (3 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Strategies/CircularColumnCreationStrategy.cs`
- **.GetOrCreateFamilySymbol()** (3 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Strategies/ColumnCreationStrategy.cs`
- **.RotateElement()** (3 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Strategies/ColumnCreationStrategy.cs`
- **.GetOrCreateFamilySymbol()** (3 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Strategies/RectangularColumnCreationStrategy.cs`
- **.RotateElement()** (3 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Strategies/RectangularColumnCreationStrategy.cs`
- **.FromXyz()** (3 connections) — `source/Sonny.Application.Infrastructure/Revit/Implements/Point3DConverter.cs`
- **.FromXyz()** (3 connections) — `source/Sonny.Application.Infrastructure/Revit/Services/IPoint3DConverter.cs`
- **.ToXyz()** (3 connections) — `source/Sonny.Application.Infrastructure/Revit/Services/IPoint3DConverter.cs`
- **FamilySymbol** (2 connections)
- **FamilySymbol** (2 connections)
- **XYZ** (2 connections)
- **XYZ** (2 connections)
- **Family** (1 connections)
- **double** (1 connections)
- *... and 5 more nodes in this community*

## Relationships

- [ColumnFromCad Interactor](ColumnFromCad_Interactor.md) (3 shared connections)
- [Revit Document Access](Revit_Document_Access.md) (1 shared connections)

## Source Files

- `source/Sonny.Application.Domain/Entities/Point3D.cs`
- `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Strategies/CircularColumnCreationStrategy.cs`
- `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Strategies/ColumnCreationStrategy.cs`
- `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Strategies/RectangularColumnCreationStrategy.cs`
- `source/Sonny.Application.Infrastructure/Revit/Implements/Point3DConverter.cs`
- `source/Sonny.Application.Infrastructure/Revit/Services/IPoint3DConverter.cs`

## Audit Trail

- EXTRACTED: 46 (96%)
- INFERRED: 2 (4%)
- AMBIGUOUS: 0 (0%)

---

*Part of the graphify knowledge wiki. See [index](index.md) to navigate.*