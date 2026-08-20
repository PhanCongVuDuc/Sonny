---
type: community
cohesion: 0.18
members: 11
---

# Revit Document Access

**Cohesion:** 0.18 - loosely connected
**Members:** 11 nodes

## Members
- [[Document_11]] - code
- [[Document_12]] - code
- [[IRevitDocument]] - code - source/Sonny.Application.Infrastructure/Revit/Services/IRevitDocument.cs
- [[IRevitDocument.cs]] - code - source/Sonny.Application.Infrastructure/Revit/Services/IRevitDocument.cs
- [[RevitDocument]] - code - source/Sonny.Application.Infrastructure/Revit/Implements/RevitDocument.cs
- [[UIApplication_1]] - code
- [[UIApplication_2]] - code
- [[UIDocument_2]] - code
- [[UIDocument_3]] - code
- [[View_10]] - code
- [[View_11]] - code

## Live Query (requires Dataview plugin)

```dataview
TABLE source_file, type FROM #community/Revit_Document_Access
SORT file.name ASC
```

## Connections to other communities
- 2 edges to [[_COMMUNITY_Domain & Infrastructure Namespaces]]
- 1 edge to [[_COMMUNITY_Integration Test Fixtures]]
- 1 edge to [[_COMMUNITY_Column Creation Strategies]]

## Top bridge nodes
- [[IRevitDocument.cs]] - degree 2, connects to 1 community