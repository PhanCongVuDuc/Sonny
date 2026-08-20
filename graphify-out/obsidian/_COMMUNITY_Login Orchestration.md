---
type: community
cohesion: 0.19
members: 17
---

# Login Orchestration

**Cohesion:** 0.19 - loosely connected
**Members:** 17 nodes

## Members
- [[dot-ActivateMachineAsync()]] - code - source/Sonny.Keygen/Sonny.Keygen/Services/MachineService.cs
- [[dot-CreateAndActivateTrialLicenseAsync()]] - code - source/Sonny.Keygen/Sonny.Keygen/Services/LoginOrchestrator.cs
- [[dot-HandleMachineActivationFailureAsync()]] - code - source/Sonny.Keygen/Sonny.Keygen/Services/LoginOrchestrator.cs
- [[dot-HandleNewUserRegistrationAsync()]] - code - source/Sonny.Keygen/Sonny.Keygen/Services/LoginOrchestrator.cs
- [[dot-HandleRegistrationErrorAsync()]] - code - source/Sonny.Keygen/Sonny.Keygen/Services/LoginOrchestrator.cs
- [[dot-LoginAsync()_1]] - code - source/Sonny.Keygen/Sonny.Keygen/Services/LoginOrchestrator.cs
- [[dot-PerformAuth0LoginAsync()]] - code - source/Sonny.Keygen/Sonny.Keygen/Services/LoginOrchestrator.cs
- [[ActivateMachineResult]] - code - source/Sonny.Keygen/Sonny.Keygen/Models/ActivateMachineResult.cs
- [[ActivateMachineResult.cs]] - code - source/Sonny.Keygen/Sonny.Keygen/Models/ActivateMachineResult.cs
- [[LoginOrchestrationResult]] - code - source/Sonny.Keygen/Sonny.Keygen/Models/LoginOrchestrationResult.cs
- [[LoginOrchestrationResult.cs]] - code - source/Sonny.Keygen/Sonny.Keygen/Models/LoginOrchestrationResult.cs
- [[LoginOrchestrator]] - code - source/Sonny.Keygen/Sonny.Keygen/Services/LoginOrchestrator.cs
- [[MachineService]] - code - source/Sonny.Keygen/Sonny.Keygen/Services/MachineService.cs
- [[MachineService.cs]] - code - source/Sonny.Keygen/Sonny.Keygen/Services/MachineService.cs
- [[RestClient_2]] - code
- [[Task_14]] - code
- [[Task_15]] - code

## Live Query (requires Dataview plugin)

```dataview
TABLE source_file, type FROM #community/Login_Orchestration
SORT file.name ASC
```

## Connections to other communities
- 4 edges to [[_COMMUNITY_Keygen Auth & Auto-Login]]
- 2 edges to [[_COMMUNITY_Keygen Login ViewModel]]
- 2 edges to [[_COMMUNITY_Keygen Services & UI]]
- 1 edge to [[_COMMUNITY_Machine Fingerprint]]

## Top bridge nodes
- [[MachineService.cs]] - degree 3, connects to 2 communities
- [[dot-CreateAndActivateTrialLicenseAsync()]] - degree 5, connects to 1 community
- [[dot-HandleRegistrationErrorAsync()]] - degree 4, connects to 1 community
- [[ActivateMachineResult.cs]] - degree 2, connects to 1 community
- [[LoginOrchestrationResult.cs]] - degree 2, connects to 1 community