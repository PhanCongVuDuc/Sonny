---
type: community
cohesion: 0.20
members: 21
---

# Unit Converter & Tests

**Cohesion:** 0.20 - loosely connected
**Members:** 21 nodes

## Members
- [[dot-ConvertToForgeTypeId()]] - code - source/Sonny.Application.Infrastructure/Revit/Implements/UnitConverter.cs
- [[dot-FormatWithUnit()]] - code - source/Sonny.Application.Infrastructure/Revit/Implements/UnitConverter.cs
- [[dot-FormatWithUnit_ShouldFormatValueWithUnitSuffix()]] - code - source/Sonny.Application.Tests/Core/UnitTests/Services/UnitConverterTests.cs
- [[dot-FromInternalUnit()]] - code - source/Sonny.Application.Infrastructure/Revit/Implements/UnitConverter.cs
- [[dot-FromInternalUnit_ShouldConvertFeetToInches()]] - code - source/Sonny.Application.Tests/Core/UnitTests/Services/UnitConverterTests.cs
- [[dot-FromInternalUnit_ShouldConvertFeetToMillimeters()]] - code - source/Sonny.Application.Tests/Core/UnitTests/Services/UnitConverterTests.cs
- [[dot-FromInternalUnit_ShouldReturnSameValue_WhenAlreadyInFeet()]] - code - source/Sonny.Application.Tests/Core/UnitTests/Services/UnitConverterTests.cs
- [[dot-GetUnitDisplayName()]] - code - source/Sonny.Application.Infrastructure/Revit/Implements/UnitConverter.cs
- [[dot-GetUnitDisplayName_ShouldReturnCorrectUnitNames()]] - code - source/Sonny.Application.Tests/Core/UnitTests/Services/UnitConverterTests.cs
- [[dot-OnDisplayUnitChanged()]] - code - source/Sonny.Application.Presentation/AutoColumnDimension/ViewModels/AutoColumnDimensionViewModel.cs
- [[dot-OnDisplayUnitChanged()_1]] - code - source/Sonny.Application.Presentation/ColumnFromCad/ViewModels/ColumnFromCadViewModel.cs
- [[dot-Setup()_1]] - code - source/Sonny.Application.Tests/Core/UnitTests/Services/UnitConverterTests.cs
- [[dot-ToInternalUnit()]] - code - source/Sonny.Application.Infrastructure/Revit/Implements/UnitConverter.cs
- [[dot-ToInternalUnit_ShouldConvertInchesToFeet()]] - code - source/Sonny.Application.Tests/Core/UnitTests/Services/UnitConverterTests.cs
- [[dot-ToInternalUnit_ShouldConvertMillimetersToFeet()]] - code - source/Sonny.Application.Tests/Core/UnitTests/Services/UnitConverterTests.cs
- [[dot-ToInternalUnit_ShouldReturnSameValue_WhenAlreadyInFeet()]] - code - source/Sonny.Application.Tests/Core/UnitTests/Services/UnitConverterTests.cs
- [[ForgeTypeId]] - code
- [[SetUp]] - code
- [[Test_3]] - code
- [[UnitConverter]] - code - source/Sonny.Application.Infrastructure/Revit/Implements/UnitConverter.cs
- [[UnitConverterTests]] - code - source/Sonny.Application.Tests/Core/UnitTests/Services/UnitConverterTests.cs

## Live Query (requires Dataview plugin)

```dataview
TABLE source_file, type FROM #community/Unit_Converter__Tests
SORT file.name ASC
```

## Connections to other communities
- 8 edges to [[_COMMUNITY_Settings & Display Unit Contracts]]
- 3 edges to [[_COMMUNITY_AutoColumnDimension ViewModel]]
- 2 edges to [[_COMMUNITY_Domain & Infrastructure Namespaces]]
- 1 edge to [[_COMMUNITY_ColumnFromCad ViewModel & Settings]]
- 1 edge to [[_COMMUNITY_ViewModel Base & Messaging]]

## Top bridge nodes
- [[dot-OnDisplayUnitChanged()]] - degree 6, connects to 3 communities
- [[UnitConverter]] - degree 9, connects to 1 community
- [[dot-FromInternalUnit()]] - degree 9, connects to 1 community
- [[dot-ToInternalUnit()]] - degree 8, connects to 1 community
- [[dot-ConvertToForgeTypeId()]] - degree 6, connects to 1 community