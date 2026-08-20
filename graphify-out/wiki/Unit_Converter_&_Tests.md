# Unit Converter & Tests

> 21 nodes · cohesion 0.20

## Key Concepts

- **UnitConverterTests** (11 connections) — `source/Sonny.Application.Tests/Core/UnitTests/Services/UnitConverterTests.cs`
- **UnitConverter** (9 connections) — `source/Sonny.Application.Infrastructure/Revit/Implements/UnitConverter.cs`
- **.FromInternalUnit()** (9 connections) — `source/Sonny.Application.Infrastructure/Revit/Implements/UnitConverter.cs`
- **.ToInternalUnit()** (8 connections) — `source/Sonny.Application.Infrastructure/Revit/Implements/UnitConverter.cs`
- **Test** (8 connections)
- **.ConvertToForgeTypeId()** (6 connections) — `source/Sonny.Application.Infrastructure/Revit/Implements/UnitConverter.cs`
- **.OnDisplayUnitChanged()** (6 connections) — `source/Sonny.Application.Presentation/AutoColumnDimension/ViewModels/AutoColumnDimensionViewModel.cs`
- **.GetUnitDisplayName()** (5 connections) — `source/Sonny.Application.Infrastructure/Revit/Implements/UnitConverter.cs`
- **.FormatWithUnit()** (4 connections) — `source/Sonny.Application.Infrastructure/Revit/Implements/UnitConverter.cs`
- **.OnDisplayUnitChanged()** (4 connections) — `source/Sonny.Application.Presentation/ColumnFromCad/ViewModels/ColumnFromCadViewModel.cs`
- **.FormatWithUnit_ShouldFormatValueWithUnitSuffix()** (3 connections) — `source/Sonny.Application.Tests/Core/UnitTests/Services/UnitConverterTests.cs`
- **.FromInternalUnit_ShouldConvertFeetToInches()** (3 connections) — `source/Sonny.Application.Tests/Core/UnitTests/Services/UnitConverterTests.cs`
- **.FromInternalUnit_ShouldConvertFeetToMillimeters()** (3 connections) — `source/Sonny.Application.Tests/Core/UnitTests/Services/UnitConverterTests.cs`
- **.FromInternalUnit_ShouldReturnSameValue_WhenAlreadyInFeet()** (3 connections) — `source/Sonny.Application.Tests/Core/UnitTests/Services/UnitConverterTests.cs`
- **.GetUnitDisplayName_ShouldReturnCorrectUnitNames()** (3 connections) — `source/Sonny.Application.Tests/Core/UnitTests/Services/UnitConverterTests.cs`
- **.ToInternalUnit_ShouldConvertInchesToFeet()** (3 connections) — `source/Sonny.Application.Tests/Core/UnitTests/Services/UnitConverterTests.cs`
- **.ToInternalUnit_ShouldConvertMillimetersToFeet()** (3 connections) — `source/Sonny.Application.Tests/Core/UnitTests/Services/UnitConverterTests.cs`
- **.ToInternalUnit_ShouldReturnSameValue_WhenAlreadyInFeet()** (3 connections) — `source/Sonny.Application.Tests/Core/UnitTests/Services/UnitConverterTests.cs`
- **ForgeTypeId** (2 connections)
- **.Setup()** (2 connections) — `source/Sonny.Application.Tests/Core/UnitTests/Services/UnitConverterTests.cs`
- **SetUp** (1 connections)

## Relationships

- [Settings & Display Unit Contracts](Settings_%26_Display_Unit_Contracts.md) (8 shared connections)
- [ViewModel Base & Messaging](ViewModel_Base_%26_Messaging.md) (1 shared connections)
- [AutoColumnDimension ViewModel](AutoColumnDimension_ViewModel.md) (1 shared connections)

## Source Files

- `source/Sonny.Application.Infrastructure/Revit/Implements/UnitConverter.cs`
- `source/Sonny.Application.Presentation/AutoColumnDimension/ViewModels/AutoColumnDimensionViewModel.cs`
- `source/Sonny.Application.Presentation/ColumnFromCad/ViewModels/ColumnFromCadViewModel.cs`
- `source/Sonny.Application.Tests/Core/UnitTests/Services/UnitConverterTests.cs`

## Audit Trail

- EXTRACTED: 43 (83%)
- INFERRED: 9 (17%)
- AMBIGUOUS: 0 (0%)

---

*Part of the graphify knowledge wiki. See [index](index.md) to navigate.*