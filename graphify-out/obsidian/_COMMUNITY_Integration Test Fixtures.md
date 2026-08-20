---
type: community
cohesion: 0.25
members: 8
---

# Integration Test Fixtures

**Cohesion:** 0.25 - loosely connected
**Members:** 8 nodes

## Members
- [[dot-ColumnFromCad_CreateColumns_Test()]] - code - source/Sonny.Application.Tests/Features/ColumnFromCad/IntegrationTests/ColumnFromCadIntegrationTest.cs
- [[dot-OnSetup()]] - code - source/Sonny.Application.Tests/Features/AutoColumnDimension/IntegrationTests/AutoColumnDimensionIntegrationTest.cs
- [[AutoColumnDimensionIntegrationTest]] - code - source/Sonny.Application.Tests/Features/AutoColumnDimension/IntegrationTests/AutoColumnDimensionIntegrationTest.cs
- [[ColumnFromCadIntegrationTest]] - code - source/Sonny.Application.Tests/Features/ColumnFromCad/IntegrationTests/ColumnFromCadIntegrationTest.cs
- [[SonnyDocumentTestBase_1]] - code
- [[Test]] - code
- [[string_1]] - code
- [[string_2]] - code

## Live Query (requires Dataview plugin)

```dataview
TABLE source_file, type FROM #community/Integration_Test_Fixtures
SORT file.name ASC
```

## Connections to other communities
- 1 edge to [[_COMMUNITY_Async Task & View Scale]]
- 1 edge to [[_COMMUNITY_Revit Document Access]]
- 1 edge to [[_COMMUNITY_AutoColumnDimension Test Helpers]]
- 1 edge to [[_COMMUNITY_Host Composition Root]]
- 1 edge to [[_COMMUNITY_Revit Test Base Classes]]
- 1 edge to [[_COMMUNITY_Domain & Infrastructure Namespaces]]
- 1 edge to [[_COMMUNITY_AutoColumnDimension Core]]

## Top bridge nodes
- [[AutoColumnDimensionIntegrationTest]] - degree 7, connects to 3 communities
- [[ColumnFromCadIntegrationTest]] - degree 5, connects to 1 community
- [[dot-ColumnFromCad_CreateColumns_Test()]] - degree 3, connects to 1 community