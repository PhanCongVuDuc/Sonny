# Login Orchestration

> 17 nodes · cohesion 0.19

## Key Concepts

- **LoginOrchestrator** (8 connections) — `source/Sonny.Keygen/Sonny.Keygen/Services/LoginOrchestrator.cs`
- **Task** (6 connections)
- **.LoginAsync()** (6 connections) — `source/Sonny.Keygen/Sonny.Keygen/Services/LoginOrchestrator.cs`
- **.CreateAndActivateTrialLicenseAsync()** (5 connections) — `source/Sonny.Keygen/Sonny.Keygen/Services/LoginOrchestrator.cs`
- **.HandleNewUserRegistrationAsync()** (5 connections) — `source/Sonny.Keygen/Sonny.Keygen/Services/LoginOrchestrator.cs`
- **.HandleMachineActivationFailureAsync()** (4 connections) — `source/Sonny.Keygen/Sonny.Keygen/Services/LoginOrchestrator.cs`
- **.HandleRegistrationErrorAsync()** (4 connections) — `source/Sonny.Keygen/Sonny.Keygen/Services/LoginOrchestrator.cs`
- **ActivateMachineResult** (3 connections) — `source/Sonny.Keygen/Sonny.Keygen/Models/ActivateMachineResult.cs`
- **.PerformAuth0LoginAsync()** (3 connections) — `source/Sonny.Keygen/Sonny.Keygen/Services/LoginOrchestrator.cs`
- **MachineService.cs** (3 connections) — `source/Sonny.Keygen/Sonny.Keygen/Services/MachineService.cs`
- **MachineService** (3 connections) — `source/Sonny.Keygen/Sonny.Keygen/Services/MachineService.cs`
- **.ActivateMachineAsync()** (3 connections) — `source/Sonny.Keygen/Sonny.Keygen/Services/MachineService.cs`
- **ActivateMachineResult.cs** (2 connections) — `source/Sonny.Keygen/Sonny.Keygen/Models/ActivateMachineResult.cs`
- **LoginOrchestrationResult.cs** (2 connections) — `source/Sonny.Keygen/Sonny.Keygen/Models/LoginOrchestrationResult.cs`
- **LoginOrchestrationResult** (2 connections) — `source/Sonny.Keygen/Sonny.Keygen/Models/LoginOrchestrationResult.cs`
- **RestClient** (1 connections)
- **Task** (1 connections)

## Relationships

- [Keygen Auth & Auto-Login](Keygen_Auth_%26_Auto-Login.md) (4 shared connections)
- [Machine Fingerprint](Machine_Fingerprint.md) (1 shared connections)
- [Keygen Services & UI](Keygen_Services_%26_UI.md) (1 shared connections)

## Source Files

- `source/Sonny.Keygen/Sonny.Keygen/Models/ActivateMachineResult.cs`
- `source/Sonny.Keygen/Sonny.Keygen/Models/LoginOrchestrationResult.cs`
- `source/Sonny.Keygen/Sonny.Keygen/Services/LoginOrchestrator.cs`
- `source/Sonny.Keygen/Sonny.Keygen/Services/MachineService.cs`

## Audit Trail

- EXTRACTED: 32 (100%)
- INFERRED: 0 (0%)
- AMBIGUOUS: 0 (0%)

---

*Part of the graphify knowledge wiki. See [index](index.md) to navigate.*