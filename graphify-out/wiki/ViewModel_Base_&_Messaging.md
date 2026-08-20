# ViewModel Base & Messaging

> 20 nodes · cohesion 0.12

## Key Concepts

- **BaseViewModel** (20 connections) — `source/Sonny.Application.Presentation/Bases/BaseViewModel.cs`
- **IMessageService** (9 connections) — `source/Sonny.Application.Domain/Services/IMessageService.cs`
- **MessageService** (7 connections) — `source/Sonny.Application.Presentation/Implements/MessageService.cs`
- **.OnDisplayUnitChanged()** (3 connections) — `source/Sonny.Application.Presentation/Bases/BaseViewModel.cs`
- **IMessageService.cs** (2 connections) — `source/Sonny.Application.Domain/Services/IMessageService.cs`
- **.ShowError()** (2 connections) — `source/Sonny.Application.Presentation/Bases/BaseViewModel.cs`
- **.ShowInfo()** (2 connections) — `source/Sonny.Application.Presentation/Bases/BaseViewModel.cs`
- **.ShowWarning()** (2 connections) — `source/Sonny.Application.Presentation/Bases/BaseViewModel.cs`
- **.ShowError()** (2 connections) — `source/Sonny.Application.Presentation/Implements/MessageService.cs`
- **.ShowInfo()** (2 connections) — `source/Sonny.Application.Presentation/Implements/MessageService.cs`
- **.ShowWarning()** (2 connections) — `source/Sonny.Application.Presentation/Implements/MessageService.cs`
- **.ShowError()** (1 connections) — `source/Sonny.Application.Domain/Services/IMessageService.cs`
- **.ShowInfo()** (1 connections) — `source/Sonny.Application.Domain/Services/IMessageService.cs`
- **.ShowQuestion()** (1 connections) — `source/Sonny.Application.Domain/Services/IMessageService.cs`
- **.ShowWarning()** (1 connections) — `source/Sonny.Application.Domain/Services/IMessageService.cs`
- **ILogger** (1 connections)
- **Window** (1 connections)
- **.LogInfo()** (1 connections) — `source/Sonny.Application.Presentation/Bases/BaseViewModel.cs`
- **string** (1 connections)
- **.ShowQuestion()** (1 connections) — `source/Sonny.Application.Presentation/Implements/MessageService.cs`

## Relationships

- [Settings & Display Unit Contracts](Settings_%26_Display_Unit_Contracts.md) (5 shared connections)
- [Domain & Infrastructure Namespaces](Domain_%26_Infrastructure_Namespaces.md) (1 shared connections)
- [Keygen Login ViewModel](Keygen_Login_ViewModel.md) (1 shared connections)
- [Settings ViewModel](Settings_ViewModel.md) (1 shared connections)
- [AutoColumnDimension ViewModel](AutoColumnDimension_ViewModel.md) (1 shared connections)
- [Async Task & View Scale](Async_Task_%26_View_Scale.md) (1 shared connections)

## Source Files

- `source/Sonny.Application.Domain/Services/IMessageService.cs`
- `source/Sonny.Application.Presentation/Bases/BaseViewModel.cs`
- `source/Sonny.Application.Presentation/Implements/MessageService.cs`

## Audit Trail

- EXTRACTED: 32 (100%)
- INFERRED: 0 (0%)
- AMBIGUOUS: 0 (0%)

---

*Part of the graphify knowledge wiki. See [index](index.md) to navigate.*