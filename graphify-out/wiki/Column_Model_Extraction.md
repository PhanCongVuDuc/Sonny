# Column Model Extraction

> 40 nodes · cohesion 0.07

## Key Concepts

- **CircularColumnModel** (8 connections) — `source/Sonny.Application.Domain/Entities/ColumnFromCad/Models/CircularColumnModel.cs`
- **RectangularColumnModel** (8 connections) — `source/Sonny.Application.Domain/Entities/ColumnFromCad/Models/RectangularColumnModel.cs`
- **CircularColumnExtractor** (5 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Implements/CircularColumnExtractor.cs`
- **.ExtractFromBoundaryLines()** (4 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Implements/CircularColumnExtractor.cs`
- **.ExtractFromPlanarFaces()** (4 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Implements/CircularColumnExtractor.cs`
- **ColumnModelFactory** (4 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Implements/ColumnModelFactory.cs`
- **.CreateRectangular()** (4 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Implements/ColumnModelFactory.cs`
- **RectangularColumnExtractor** (4 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Implements/RectangularColumnExtractor.cs`
- **.ExtractFromBoundaryLines()** (4 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Implements/RectangularColumnExtractor.cs`
- **.ExtractFromPlanarFaces()** (4 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Implements/RectangularColumnExtractor.cs`
- **ICircularColumnExtractor** (4 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Services/ICircularColumnExtractor.cs`
- **.ExtractFromBoundaryLines()** (4 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Services/ICircularColumnExtractor.cs`
- **.ExtractFromPlanarFaces()** (4 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Services/ICircularColumnExtractor.cs`
- **IColumnModelFactory** (4 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Services/IColumnModelFactory.cs`
- **.CreateRectangular()** (4 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Services/IColumnModelFactory.cs`
- **IRectangularColumnExtractor** (4 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Services/IRectangularColumnExtractor.cs`
- **.ExtractFromBoundaryLines()** (4 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Services/IRectangularColumnExtractor.cs`
- **.ExtractFromPlanarFaces()** (4 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Services/IRectangularColumnExtractor.cs`
- **.CreateCircular()** (3 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Implements/ColumnModelFactory.cs`
- **Services/ICircularColumnExtractor.cs** (3 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Services/ICircularColumnExtractor.cs`
- **IColumnModelFactory.cs** (3 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Services/IColumnModelFactory.cs`
- **.CreateCircular()** (3 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Services/IColumnModelFactory.cs`
- **Services/IRectangularColumnExtractor.cs** (3 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Services/IRectangularColumnExtractor.cs`
- **CircularColumnModel.cs** (2 connections) — `source/Sonny.Application.Domain/Entities/ColumnFromCad/Models/CircularColumnModel.cs`
- **RectangularColumnModel.cs** (2 connections) — `source/Sonny.Application.Domain/Entities/ColumnFromCad/Models/RectangularColumnModel.cs`
- *... and 15 more nodes in this community*

## Relationships

- [Domain & Infrastructure Namespaces](Domain_%26_Infrastructure_Namespaces.md) (8 shared connections)
- [ColumnFromCad Interactor](ColumnFromCad_Interactor.md) (2 shared connections)

## Source Files

- `source/Sonny.Application.Domain/Entities/ColumnFromCad/Models/CircularColumnModel.cs`
- `source/Sonny.Application.Domain/Entities/ColumnFromCad/Models/RectangularColumnModel.cs`
- `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Implements/CircularColumnExtractor.cs`
- `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Implements/ColumnModelFactory.cs`
- `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Implements/RectangularColumnExtractor.cs`
- `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Services/ICircularColumnExtractor.cs`
- `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Services/IColumnModelFactory.cs`
- `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Services/IRectangularColumnExtractor.cs`

## Audit Trail

- EXTRACTED: 65 (100%)
- INFERRED: 0 (0%)
- AMBIGUOUS: 0 (0%)

---

*Part of the graphify knowledge wiki. See [index](index.md) to navigate.*