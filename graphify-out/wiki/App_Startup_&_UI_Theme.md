# App Startup & UI Theme

> 20 nodes · cohesion 0.13

## Key Concepts

- **SonnyModule** (5 connections) — `source/Sonny.Application/Modules/SonnyModule.cs`
- **SonnyApp** (5 connections) — `source/Sonny.Application/SonnyApp.cs`
- **.OnStartup()** (5 connections) — `source/Sonny.Application/SonnyApp.cs`
- **UIStyleManager** (4 connections) — `source/Sonny.Application.UIStyle/UIStyleManager.cs`
- **.LoadTheme()** (4 connections) — `source/Sonny.Application.UIStyle/UIStyleManager.cs`
- **.Create()** (4 connections) — `source/Sonny.Keygen/Sonny.Keygen/Services/KeygenConfigBuilder.cs`
- **App** (4 connections) — `source/Sonny.Keygen/Sonny.Keygen.Test/App.xaml.cs`
- **ExternalApplication** (3 connections)
- **.OnShutdown()** (3 connections) — `source/Sonny.Application/Modules/SonnyModule.cs`
- **.OnStartup()** (3 connections) — `source/Sonny.Application/Modules/SonnyModule.cs`
- **.GetOrCreateApplication()** (3 connections) — `source/Sonny.Application.UIStyle/UIStyleManager.cs`
- **.RemoveTheme()** (3 connections) — `source/Sonny.Application.UIStyle/UIStyleManager.cs`
- **.OnStartup()** (3 connections) — `source/Sonny.Keygen/Sonny.Keygen.Test/App.xaml.cs`
- **UIControlledApplication** (2 connections)
- **.OnShutdown()** (2 connections) — `source/Sonny.Application/SonnyApp.cs`
- **Application** (2 connections)
- **Application** (2 connections) — `source/Sonny.Keygen/Sonny.Keygen.Test/App.xaml`
- **Application** (1 connections)
- **Sonny.Keygen.Test/App.xaml** (1 connections) — `source/Sonny.Keygen/Sonny.Keygen.Test/App.xaml`
- **StartupEventArgs** (1 connections)

## Relationships

- [Application Module System](Application_Module_System.md) (1 shared connections)
- [Host Composition Root](Host_Composition_Root.md) (1 shared connections)
- [Keygen Config Builder](Keygen_Config_Builder.md) (1 shared connections)

## Source Files

- `source/Sonny.Application.UIStyle/UIStyleManager.cs`
- `source/Sonny.Application/Modules/SonnyModule.cs`
- `source/Sonny.Application/SonnyApp.cs`
- `source/Sonny.Keygen/Sonny.Keygen.Test/App.xaml`
- `source/Sonny.Keygen/Sonny.Keygen.Test/App.xaml.cs`
- `source/Sonny.Keygen/Sonny.Keygen/Services/KeygenConfigBuilder.cs`

## Audit Trail

- EXTRACTED: 26 (93%)
- INFERRED: 2 (7%)
- AMBIGUOUS: 0 (0%)

---

*Part of the graphify knowledge wiki. See [index](index.md) to navigate.*