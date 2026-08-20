# Transaction & Failure Contracts

> 32 nodes · cohesion 0.08

## Key Concepts

- **ITransactionGroupManager** (7 connections) — `source/Sonny.Application.Domain/Services/ITransactionGroupManager.cs`
- **ITransactionManager** (7 connections) — `source/Sonny.Application.Domain/Services/ITransactionManager.cs`
- **FailurePreprocessorType** (6 connections) — `source/Sonny.Application.Domain/Services/FailurePreprocessorType.cs`
- **FailurePreprocessorFactory** (6 connections) — `source/Sonny.Application.Infrastructure/Revit/Implements/FailurePreprocessorFactory.cs`
- **.CreateComposite()** (6 connections) — `source/Sonny.Application.Infrastructure/Revit/Implements/FailurePreprocessorFactory.cs`
- **.Create()** (5 connections) — `source/Sonny.Application.Infrastructure/Revit/Implements/FailurePreprocessorFactory.cs`
- **ITransactionManagerFactory** (4 connections) — `source/Sonny.Application.Domain/Services/ITransactionManagerFactory.cs`
- **.Create()** (4 connections) — `source/Sonny.Application.Domain/Services/ITransactionManagerFactory.cs`
- **TransactionManagerFactory.cs** (4 connections) — `source/Sonny.Application.Infrastructure/Revit/Managers/Transactions/TransactionManagerFactory.cs`
- **TransactionManagerFactory** (4 connections) — `source/Sonny.Application.Infrastructure/Revit/Managers/Transactions/TransactionManagerFactory.cs`
- **.Create()** (4 connections) — `source/Sonny.Application.Infrastructure/Revit/Managers/Transactions/TransactionManagerFactory.cs`
- **.CreateComposite()** (4 connections) — `source/Sonny.Application.Infrastructure/Revit/Services/IFailurePreprocessorFactory.cs`
- **IFailuresPreprocessor** (3 connections)
- **.CreateCompositeFailurePreprocessor()** (3 connections) — `source/Sonny.Application.Infrastructure/Revit/Implements/FailurePreprocessorFactory.cs`
- **.CreateSuppressWarningsPreprocessor()** (3 connections) — `source/Sonny.Application.Infrastructure/Revit/Implements/FailurePreprocessorFactory.cs`
- **IFailurePreprocessorFactory.cs** (3 connections) — `source/Sonny.Application.Infrastructure/Revit/Services/IFailurePreprocessorFactory.cs`
- **IFailurePreprocessorFactory** (3 connections) — `source/Sonny.Application.Infrastructure/Revit/Services/IFailurePreprocessorFactory.cs`
- **IDisposable** (2 connections)
- **FailurePreprocessorType.cs** (2 connections) — `source/Sonny.Application.Domain/Services/FailurePreprocessorType.cs`
- **ITransactionGroupManager.cs** (2 connections) — `source/Sonny.Application.Domain/Services/ITransactionGroupManager.cs`
- **ITransactionManagerFactory.cs** (2 connections) — `source/Sonny.Application.Domain/Services/ITransactionManagerFactory.cs`
- **.CreateGroup()** (2 connections) — `source/Sonny.Application.Domain/Services/ITransactionManagerFactory.cs`
- **.CreateGroup()** (2 connections) — `source/Sonny.Application.Infrastructure/Revit/Managers/Transactions/TransactionManagerFactory.cs`
- **.Assimilate()** (1 connections) — `source/Sonny.Application.Domain/Services/ITransactionGroupManager.cs`
- **.Start()** (1 connections) — `source/Sonny.Application.Domain/Services/ITransactionGroupManager.cs`
- *... and 7 more nodes in this community*

## Relationships

- [Domain & Infrastructure Namespaces](Domain_%26_Infrastructure_Namespaces.md) (7 shared connections)
- [Failure Preprocessors](Failure_Preprocessors.md) (1 shared connections)
- [Transaction Managers](Transaction_Managers.md) (1 shared connections)

## Source Files

- `source/Sonny.Application.Domain/Services/FailurePreprocessorType.cs`
- `source/Sonny.Application.Domain/Services/ITransactionGroupManager.cs`
- `source/Sonny.Application.Domain/Services/ITransactionManager.cs`
- `source/Sonny.Application.Domain/Services/ITransactionManagerFactory.cs`
- `source/Sonny.Application.Infrastructure/Revit/Implements/FailurePreprocessorFactory.cs`
- `source/Sonny.Application.Infrastructure/Revit/Managers/Transactions/TransactionManagerFactory.cs`
- `source/Sonny.Application.Infrastructure/Revit/Services/IFailurePreprocessorFactory.cs`

## Audit Trail

- EXTRACTED: 51 (100%)
- INFERRED: 0 (0%)
- AMBIGUOUS: 0 (0%)

---

*Part of the graphify knowledge wiki. See [index](index.md) to navigate.*