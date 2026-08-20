# Transaction Managers

> 35 nodes · cohesion 0.07

## Key Concepts

- **TransactionGroupManager** (10 connections) — `source/Sonny.Application.Infrastructure/Revit/Managers/Transactions/TransactionGroupManager.cs`
- **TransactionManager** (10 connections) — `source/Sonny.Application.Infrastructure/Revit/Managers/Transactions/TransactionManager.cs`
- **Sonny.Application.Domain.Entities** (7 connections) — `source/Sonny.Application.Domain/Entities/DomainTransactionStatus.cs`
- **TransactionManager.cs** (6 connections) — `source/Sonny.Application.Infrastructure/Revit/Managers/Transactions/TransactionManager.cs`
- **Sonny.Application.Infrastructure.Revit.Managers.Transactions** (5 connections) — `source/Sonny.Application.Infrastructure/Revit/Managers/Transactions/TransactionGroupManager.cs`
- **TransactionGroupManager.cs** (5 connections) — `source/Sonny.Application.Infrastructure/Revit/Managers/Transactions/TransactionGroupManager.cs`
- **.Convert()** (5 connections) — `source/Sonny.Application.Infrastructure/Revit/Managers/Transactions/TransactionStatusConverter.cs`
- **DomainTransactionStatus** (4 connections) — `source/Sonny.Application.Domain/Entities/DomainTransactionStatus.cs`
- **.GetStatus()** (4 connections) — `source/Sonny.Application.Infrastructure/Revit/Managers/Transactions/TransactionGroupManager.cs`
- **.GetStatus()** (4 connections) — `source/Sonny.Application.Infrastructure/Revit/Managers/Transactions/TransactionManager.cs`
- **Sonny.Application.Domain.Exceptions** (3 connections) — `source/Sonny.Application.Domain/Exceptions/TransactionCommitFailedException.cs`
- **ITransactionManager.cs** (3 connections) — `source/Sonny.Application.Domain/Services/ITransactionManager.cs`
- **TransactionStatusConverter.cs** (3 connections) — `source/Sonny.Application.Infrastructure/Revit/Managers/Transactions/TransactionStatusConverter.cs`
- **IPoint3DConverter.cs** (3 connections) — `source/Sonny.Application.Infrastructure/Revit/Services/IPoint3DConverter.cs`
- **DomainTransactionStatus.cs** (2 connections) — `source/Sonny.Application.Domain/Entities/DomainTransactionStatus.cs`
- **Point3D.cs** (2 connections) — `source/Sonny.Application.Domain/Entities/Point3D.cs`
- **TransactionCommitFailedException.cs** (2 connections) — `source/Sonny.Application.Domain/Exceptions/TransactionCommitFailedException.cs`
- **TransactionCommitFailedException** (2 connections) — `source/Sonny.Application.Domain/Exceptions/TransactionCommitFailedException.cs`
- **.IsRolledBack()** (2 connections) — `source/Sonny.Application.Infrastructure/Revit/Managers/Transactions/TransactionGroupManager.cs`
- **.Commit()** (2 connections) — `source/Sonny.Application.Infrastructure/Revit/Managers/Transactions/TransactionManager.cs`
- **TransactionStatusConverter** (2 connections) — `source/Sonny.Application.Infrastructure/Revit/Managers/Transactions/TransactionStatusConverter.cs`
- **Exception** (1 connections)
- **bool** (1 connections)
- **.Assimilate()** (1 connections) — `source/Sonny.Application.Infrastructure/Revit/Managers/Transactions/TransactionGroupManager.cs`
- **.Dispose()** (1 connections) — `source/Sonny.Application.Infrastructure/Revit/Managers/Transactions/TransactionGroupManager.cs`
- *... and 10 more nodes in this community*

## Relationships

- [Domain & Infrastructure Namespaces](Domain_%26_Infrastructure_Namespaces.md) (6 shared connections)
- [Transaction & Failure Contracts](Transaction_%26_Failure_Contracts.md) (3 shared connections)
- [Column Creation Strategies](Column_Creation_Strategies.md) (2 shared connections)

## Source Files

- `source/Sonny.Application.Domain/Entities/DomainTransactionStatus.cs`
- `source/Sonny.Application.Domain/Entities/Point3D.cs`
- `source/Sonny.Application.Domain/Exceptions/TransactionCommitFailedException.cs`
- `source/Sonny.Application.Domain/Services/ITransactionManager.cs`
- `source/Sonny.Application.Infrastructure/Revit/Managers/Transactions/TransactionGroupManager.cs`
- `source/Sonny.Application.Infrastructure/Revit/Managers/Transactions/TransactionManager.cs`
- `source/Sonny.Application.Infrastructure/Revit/Managers/Transactions/TransactionStatusConverter.cs`
- `source/Sonny.Application.Infrastructure/Revit/Services/IPoint3DConverter.cs`

## Audit Trail

- EXTRACTED: 54 (100%)
- INFERRED: 0 (0%)
- AMBIGUOUS: 0 (0%)

---

*Part of the graphify knowledge wiki. See [index](index.md) to navigate.*