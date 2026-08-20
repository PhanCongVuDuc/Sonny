---
type: community
cohesion: 0.22
members: 9
---

# Revit Element Wrappers

**Cohesion:** 0.22 - loosely connected
**Members:** 9 nodes

## Members
- [[Dictionary]] - code
- [[Dimension]] - code
- [[DimensionWrapperBase]] - code - source/Sonny.RevitExtensions/source/Sonny.RevitExtensions/RevitWrapper/DimensionWrapperBase.cs
- [[DimensionWrapperBase.cs]] - code - source/Sonny.RevitExtensions/source/Sonny.RevitExtensions/RevitWrapper/DimensionWrapperBase.cs
- [[Element_4]] - code
- [[ElementWrapperBase]] - code - source/Sonny.RevitExtensions/source/Sonny.RevitExtensions/RevitWrapper/ElementWrapperBase.cs
- [[Wall]] - code
- [[WallWrapperBase]] - code - source/Sonny.RevitExtensions/source/Sonny.RevitExtensions/RevitWrapper/WallWrapperBase.cs
- [[WallWrapperBase.cs]] - code - source/Sonny.RevitExtensions/source/Sonny.RevitExtensions/RevitWrapper/WallWrapperBase.cs

## Live Query (requires Dataview plugin)

```dataview
TABLE source_file, type FROM #community/Revit_Element_Wrappers
SORT file.name ASC
```

## Connections to other communities
- 3 edges to [[_COMMUNITY_View & Element Wrappers]]
- 3 edges to [[_COMMUNITY_AutoColumnDimension Executor]]
- 2 edges to [[_COMMUNITY_Dimension Creator]]
- 2 edges to [[_COMMUNITY_AutoColumnDimension Interactor]]
- 2 edges to [[_COMMUNITY_AutoColumnDimension Core]]
- 1 edge to [[_COMMUNITY_Family Instance Wrapper]]
- 1 edge to [[_COMMUNITY_Grid Finder]]
- 1 edge to [[_COMMUNITY_Column Extractor Adapters]]

## Top bridge nodes
- [[ElementWrapperBase]] - degree 17, connects to 1 community
- [[DimensionWrapperBase.cs]] - degree 2, connects to 1 community
- [[WallWrapperBase.cs]] - degree 2, connects to 1 community