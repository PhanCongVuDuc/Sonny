---
type: community
cohesion: 0.08
members: 32
---

# Transaction & Failure Contracts

**Cohesion:** 0.08 - loosely connected
**Members:** 32 nodes

## Members
- [[dot-Assimilate()_1]] - code - source/Sonny.Application.Domain/Services/ITransactionGroupManager.cs
- [[dot-Commit()_1]] - code - source/Sonny.Application.Domain/Services/ITransactionManager.cs
- [[dot-Create()]] - code - source/Sonny.Application.Domain/Services/ITransactionManagerFactory.cs
- [[dot-Create()_1]] - code - source/Sonny.Application.Infrastructure/Revit/Implements/FailurePreprocessorFactory.cs
- [[dot-Create()_2]] - code - source/Sonny.Application.Infrastructure/Revit/Managers/Transactions/TransactionManagerFactory.cs
- [[dot-CreateComposite()]] - code - source/Sonny.Application.Infrastructure/Revit/Implements/FailurePreprocessorFactory.cs
- [[dot-CreateComposite()_1]] - code - source/Sonny.Application.Infrastructure/Revit/Services/IFailurePreprocessorFactory.cs
- [[dot-CreateCompositeFailurePreprocessor()]] - code - source/Sonny.Application.Infrastructure/Revit/Implements/FailurePreprocessorFactory.cs
- [[dot-CreateGroup()]] - code - source/Sonny.Application.Domain/Services/ITransactionManagerFactory.cs
- [[dot-CreateGroup()_1]] - code - source/Sonny.Application.Infrastructure/Revit/Managers/Transactions/TransactionManagerFactory.cs
- [[dot-CreateSuppressWarningsPreprocessor()]] - code - source/Sonny.Application.Infrastructure/Revit/Implements/FailurePreprocessorFactory.cs
- [[dot-Start()_2]] - code - source/Sonny.Application.Domain/Services/ITransactionGroupManager.cs
- [[dot-Start()_3]] - code - source/Sonny.Application.Domain/Services/ITransactionManager.cs
- [[FailurePreprocessorFactory]] - code - source/Sonny.Application.Infrastructure/Revit/Implements/FailurePreprocessorFactory.cs
- [[FailurePreprocessorType]] - code - source/Sonny.Application.Domain/Services/FailurePreprocessorType.cs
- [[FailurePreprocessorType.cs]] - code - source/Sonny.Application.Domain/Services/FailurePreprocessorType.cs
- [[IDisposable]] - code
- [[IEnumerable_8]] - code
- [[IEnumerable_9]] - code
- [[IEnumerable_10]] - code
- [[IEnumerable_11]] - code
- [[IFailurePreprocessorFactory]] - code - source/Sonny.Application.Infrastructure/Revit/Services/IFailurePreprocessorFactory.cs
- [[IFailurePreprocessorFactory.cs]] - code - source/Sonny.Application.Infrastructure/Revit/Services/IFailurePreprocessorFactory.cs
- [[IFailuresPreprocessor_1]] - code
- [[IFailuresPreprocessor_2]] - code
- [[ITransactionGroupManager]] - code - source/Sonny.Application.Domain/Services/ITransactionGroupManager.cs
- [[ITransactionGroupManager.cs]] - code - source/Sonny.Application.Domain/Services/ITransactionGroupManager.cs
- [[ITransactionManager]] - code - source/Sonny.Application.Domain/Services/ITransactionManager.cs
- [[ITransactionManagerFactory]] - code - source/Sonny.Application.Domain/Services/ITransactionManagerFactory.cs
- [[ITransactionManagerFactory.cs]] - code - source/Sonny.Application.Domain/Services/ITransactionManagerFactory.cs
- [[TransactionManagerFactory]] - code - source/Sonny.Application.Infrastructure/Revit/Managers/Transactions/TransactionManagerFactory.cs
- [[TransactionManagerFactory.cs]] - code - source/Sonny.Application.Infrastructure/Revit/Managers/Transactions/TransactionManagerFactory.cs

## Live Query (requires Dataview plugin)

```dataview
TABLE source_file, type FROM #community/Transaction__Failure_Contracts
SORT file.name ASC
```

## Connections to other communities
- 8 edges to [[_COMMUNITY_Domain & Infrastructure Namespaces]]
- 4 edges to [[_COMMUNITY_Transaction Managers]]
- 1 edge to [[_COMMUNITY_Failure Preprocessors]]

## Top bridge nodes
- [[TransactionManagerFactory.cs]] - degree 4, connects to 2 communities
- [[dot-CreateCompositeFailurePreprocessor()]] - degree 3, connects to 1 community
- [[IFailurePreprocessorFactory.cs]] - degree 3, connects to 1 community
- [[FailurePreprocessorType.cs]] - degree 2, connects to 1 community
- [[ITransactionGroupManager.cs]] - degree 2, connects to 1 community