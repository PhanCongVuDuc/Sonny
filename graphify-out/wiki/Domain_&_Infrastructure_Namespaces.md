# Domain & Infrastructure Namespaces

> 66 nodes · cohesion 0.08

## Key Concepts

- **Sonny.Application.Domain.Services** (56 connections) — `source/Sonny.Application.Domain/Services/FailurePreprocessorType.cs`
- **Sonny.Application.Infrastructure.Revit.Services** (31 connections) — `source/Sonny.Application.Infrastructure/Revit/Services/ICadLinkSelector.cs`
- **Sonny.RevitExtensions.Extensions** (27 connections) — `source/Sonny.RevitExtensions/source/Sonny.RevitExtensions/Extensions/CategoryExtensions.cs`
- **Sonny.Application.Domain.Entities.ColumnFromCad.Models** (22 connections) — `source/Sonny.Application.Domain/Entities/ColumnFromCad/Models/CircularColumnModel.cs`
- **Sonny.Application.Domain.Entities.Settings** (20 connections) — `source/Sonny.Application.Domain/Entities/Settings/DisplayUnit.cs`
- **Sonny.Application.Infrastructure/ServiceRegistration.cs** (18 connections) — `source/Sonny.Application.Infrastructure/ServiceRegistration.cs`
- **Sonny.Application.Infrastructure.Revit.Implements** (17 connections) — `source/Sonny.Application.Infrastructure/Revit/Implements/CadLinkSelector.cs`
- **Sonny.Application.Domain.Entities.ColumnFromCad.Contexts** (12 connections) — `source/Sonny.Application.Domain/Entities/ColumnFromCad/Contexts/ColumnCreationContext.cs`
- **Sonny.Application.Domain.Entities.Settings.Models** (11 connections) — `source/Sonny.Application.Domain/Entities/Settings/Models/DimensionTypeModel.cs`
- **Sonny.Application.Presentation.Services** (10 connections) — `source/Sonny.Application.Presentation/Services/ICommonServices.cs`
- **ColumnFromCadViewModel.cs** (10 connections) — `source/Sonny.Application.Presentation/ColumnFromCad/ViewModels/ColumnFromCadViewModel.cs`
- **ColumnDataExtractor.cs** (9 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Implements/ColumnDataExtractor.cs`
- **CircularColumnCreationStrategy.cs** (9 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Strategies/CircularColumnCreationStrategy.cs`
- **RectangularColumnCreationStrategy.cs** (9 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Strategies/RectangularColumnCreationStrategy.cs`
- **ColumnFromCadIntegrationTest.cs** (9 connections) — `source/Sonny.Application.Tests/Features/ColumnFromCad/IntegrationTests/ColumnFromCadIntegrationTest.cs`
- **Sonny.Application.Domain.Entities.ColumnFromCad.Services** (8 connections) — `source/Sonny.Application.Domain/Entities/ColumnFromCad/Services/IColumnCreationStrategy.cs`
- **Sonny.Application.Infrastructure.Features.ColumnFromCad.Services** (8 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Services/ICircularColumnExtractor.cs`
- **Sonny.Application.UseCases.ColumnFromCad.Services** (8 connections) — `source/Sonny.Application.UseCases/ColumnFromCad/Services/IColumnFromCadContext.cs`
- **ColumnFromCadContext.cs** (8 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Implements/ColumnFromCadContext.cs`
- **AutoColumnDimensionViewModel.cs** (8 connections) — `source/Sonny.Application.Presentation/AutoColumnDimension/ViewModels/AutoColumnDimensionViewModel.cs`
- **SettingsViewModel.cs** (8 connections) — `source/Sonny.Application.Presentation/Settings/ViewModels/SettingsViewModel.cs`
- **ColumnModelFactory.cs** (7 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Implements/ColumnModelFactory.cs`
- **ColumnCreationStrategy.cs** (7 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Strategies/ColumnCreationStrategy.cs`
- **ColumnCreationStrategyFactory.cs** (7 connections) — `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Strategies/ColumnCreationStrategyFactory.cs`
- **ColumnFromCadInteractor.cs** (7 connections) — `source/Sonny.Application.UseCases/ColumnFromCad/Implements/ColumnFromCadInteractor.cs`
- *... and 41 more nodes in this community*

## Relationships

- [ColumnFromCad Interactor](ColumnFromCad_Interactor.md) (7 shared connections)
- [AutoColumnDimension Core](AutoColumnDimension_Core.md) (5 shared connections)
- [Host & Resource Bootstrap](Host_%26_Resource_Bootstrap.md) (4 shared connections)
- [Settings & Display Unit Contracts](Settings_%26_Display_Unit_Contracts.md) (4 shared connections)
- [ColumnFromCad ViewModel & Settings](ColumnFromCad_ViewModel_%26_Settings.md) (4 shared connections)
- [Column Creation Strategies](Column_Creation_Strategies.md) (4 shared connections)
- [AutoColumnDimension ViewModel](AutoColumnDimension_ViewModel.md) (3 shared connections)
- [Settings ViewModel](Settings_ViewModel.md) (3 shared connections)
- [Presentation Services & Progress](Presentation_Services_%26_Progress.md) (3 shared connections)
- [Failure Preprocessors](Failure_Preprocessors.md) (2 shared connections)
- [ResourceManager Unit Tests](ResourceManager_Unit_Tests.md) (2 shared connections)
- [Language Change Handling](Language_Change_Handling.md) (2 shared connections)

## Source Files

- `source/Revit.Async/Revit.Async/Revit.Async.csproj`
- `source/Sonny.Application.Domain/Entities/ColumnFromCad/Contexts/ColumnCreationContext.cs`
- `source/Sonny.Application.Domain/Entities/ColumnFromCad/Models/CircularColumnModel.cs`
- `source/Sonny.Application.Domain/Entities/ColumnFromCad/Services/IColumnCreationStrategy.cs`
- `source/Sonny.Application.Domain/Entities/ColumnFromCad/Services/IColumnCreationStrategyFactory.cs`
- `source/Sonny.Application.Domain/Entities/ColumnFromCad/Services/IColumnDataExtractor.cs`
- `source/Sonny.Application.Domain/Entities/Settings/DisplayUnit.cs`
- `source/Sonny.Application.Domain/Entities/Settings/Models/DimensionTypeModel.cs`
- `source/Sonny.Application.Domain/Services/FailurePreprocessorType.cs`
- `source/Sonny.Application.Domain/Services/IDimensionTypeProvider.cs`
- `source/Sonny.Application.Domain/Services/IDisplayUnitProvider.cs`
- `source/Sonny.Application.Domain/Services/ISettingsService.cs`
- `source/Sonny.Application.Domain/Services/IUnitConverter.cs`
- `source/Sonny.Application.Infrastructure/Features/AutoColumnDimension/Implements/DimensionTypeProvider.cs`
- `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Implements/CircularColumnExtractor.cs`
- `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Implements/ColumnDataExtractor.cs`
- `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Implements/ColumnFromCadContext.cs`
- `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Implements/ColumnModelFactory.cs`
- `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Services/ICircularColumnExtractor.cs`
- `source/Sonny.Application.Infrastructure/Features/ColumnFromCad/Strategies/CircularColumnCreationStrategy.cs`

## Audit Trail

- EXTRACTED: 245 (100%)
- INFERRED: 0 (0%)
- AMBIGUOUS: 0 (0%)

---

*Part of the graphify knowledge wiki. See [index](index.md) to navigate.*