# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Communication language

Reply to the user in Vietnamese, even when the question is asked in English.

Keep English for: code, file/type/member/variable names, comments and XML docs inside code, commit messages, branch
names, and technical terms with no natural Vietnamese equivalent (transaction, singleton, dependency injection,
interactor, ribbon…).

## What this is

An Autodesk Revit add-in (`Sonny.Application`) targeting Revit 2021–2026 from a single source tree, layered as Clean
Architecture and built by Nuke.

## Build & test

Everything is driven by configuration names of the form `Debug R25` / `Release R21` — **note the space**, so always quote
them on the command line.

```powershell
# Full pipeline (default target = Compile; Clean runs first on local builds)
./.nuke/build.cmd

# Named targets
./.nuke/build.cmd CreateBundle          # -> output/Sonny.Application.bundle.zip
./.nuke/build.cmd CreateInstaller       # -> .msi via install/Installer.csproj
./.nuke/build.cmd PublishGitHub --ReleaseVersion 1.0.0   # server builds only

# Build a single Revit version directly (faster inner loop)
dotnet build source/Sonny.Application/Sonny.Application.csproj -c "Debug R25"
```

The Nuke `Compile` target only globs `Release*` configurations and **skips any project whose name contains "Tests"** —
tests are never built or run by CI. To touch Debug output you must build it yourself.

```powershell
# Tests launch a real Revit process (ricaun.RevitTest.TestAdapter); Revit of the matching year must be installed
dotnet test source/Sonny.Application.Tests/Sonny.Application.Tests.csproj -c "Debug R23"
dotnet test source/Sonny.Application.Tests/Sonny.Application.Tests.csproj -c "Debug R23" --filter "FullyQualifiedName~ColumnFromCad_CreateColumns_Test"
```

Integration tests under `Features/**/IntegrationTests` open `.rvt` fixtures from `Resources/RevitFiles` and assert exact
element counts against hard-coded `UniqueId`s — they are pinned to those specific documents (mostly `Test_V2023_*.rvt`).
Unit tests under `Core/UnitTests` and `ResourceManager/UnitTests` use NUnit + NSubstitute and need no Revit document.

Revit-free unit tests live in `source/Sonny.Application.UnitTests` (NUnit 4 + NSubstitute, references
Domain + UseCases + ResourceManager only, full R21–R26 matrix — net48 for R21–R24, net8.0-windows for
R25/R26 — finishes in seconds without launching Revit):

```powershell
dotnet test source/Sonny.Application.UnitTests/Sonny.Application.UnitTests.csproj -c "Debug R25"
```

`Nice3point.Revit.Build.Tasks` deploys the add-in to the local Revit add-ins folder on build (`DeployRevitAddin`), so a
successful build is enough to try the tool in Revit.

## Version matrix & conditional compilation

| Configuration | RevitVersion | TargetFramework   |
|---------------|--------------|-------------------|
| `* R21`–`R24` | 2021–2024    | `net48`           |
| `* R25`,`R26` | 2025, 2026   | `net8.0-windows`  |

Every product `.csproj` repeats this same block; adding a Revit year means editing `<Configurations>` in all of them plus
the solution. `net48` needs explicit `PackageReference`s the shared framework provides for free on `net8.0-windows` —
`Newtonsoft.Json` is used there instead of `System.Text.Json`, and `PresentationCore`/`PresentationFramework`/
`WindowsBase` are referenced manually. Test project defines `REVIT$(RevitVersion)` for `#if` branching.

`IsRepackable` (ILRepack) is **on for Release, off for Debug**. Release merges dependencies because the add-in's
`System.Text.Json` is newer than Revit's; Debug leaves them separate so the test project can reference the same types
without conflicts. Do not flip this per-configuration behavior.

## Submodules

`source/Revit.Async`, `source/Sonny.EasyRibbon`, `source/Sonny.RevitExtensions`, `source/Sonny.Keygen` are git
submodules. Nothing builds without `git submodule update --init --recursive`. Changes to those libraries belong in their
own repositories — never commit edits to them from this repo.

## Architecture

Layers, innermost first (see `.cursor/rules/cleanarchitecture.mdc` for the full rule set — it is `alwaysApply`):

| Project                            | Role                                             | Revit API? |
|------------------------------------|--------------------------------------------------|------------|
| `Sonny.Application.Domain`         | Entities + service interfaces. References *nothing*. | ❌ |
| `Sonny.Application.UseCases`       | Interactors (one per feature), input ports        | ❌ (by rule) |
| `Sonny.Application.Infrastructure` | Revit adapters, feature implementations, license  | ✅ |
| `Sonny.Application.Presentation`   | WPF Views + CommunityToolkit.Mvvm ViewModels      | ❌ (must not reference Infrastructure) |
| `Sonny.Application`                | Entry point: `SonnyApp`, `Host`, commands, ribbon | ✅ |

Support projects: `Sonny.ResourceManager` (localization engine), `Sonny.Application.UIStyle` (WPF theme).

Known, deliberate deviation: `Infrastructure` references `UseCases` so it can implement input ports. Both
interactors live in `UseCases` and touch no Revit type (since ADR 0001 — `AutoColumnDimensionInteractor`
reads geometry through `IColumnGeometryReader` and executes through `IDimensionPlanExecutor`, both
implemented in Infrastructure). Keep new interactors in UseCases; when a feature needs the Revit API, put
the mechanism behind ports with a plain-DTO boundary instead of moving the interactor down.


### Composition root

`Host` (`source/Sonny.Application/Host.cs`) is a static, lazily-initialized, double-checked-locked `IServiceProvider`.
Each layer owns its registrations via an extension method (`AddInfrastructureServices`, `AddUseCaseServices`,
`AddPresentationsServices`, `AddSerilogConfiguration`) — add new services to the owning layer's `ServiceRegistration.cs`,
not to `Host`.

**UIDocument lifetime is the sharpest edge here.** `IUIDocumentProvider` is a singleton holding the *currently running*
command's `UIDocument`; `BaseExternalCommand` re-sets it on every invocation. Singleton consumers therefore must read
through `IRevitDocument`/the provider on each call and never cache `UIDocument`, `Document`, or `ActiveView` in a field.
The comments in `Infrastructure/ServiceRegistration.cs` record which registrations are singleton vs transient and why —
respect them when adding services.

### Command flow

Ribbon UI is declarative: `Ribbon/SonnyTab.cs` uses `Sonny.EasyRibbon`'s `[Tab]`/`[Panel]`/`[Button]` attributes on
nested classes, wired up by `SonnyModule` in `SonnyApp.OnStartup`. A new tool means: button attribute in `SonnyTab` →
command class deriving `BaseExternalCommand` → View/ViewModel registered in Presentation → interactor.

`BaseExternalCommand.Execute` is the fixed pipeline: `Host.Start()` → license check (override `ShouldCheckLicense()` to
skip) → `IUIDocumentProvider.SetUIDocument` → `RevitTask.Initialize` → `ExecuteInternal`, with logging + `IMessageService`
error reporting around it. Commands themselves are thin: resolve a View from `Host` and `Show()` it.

Long Revit work is dispatched through `IRevitTaskRunner` (Revit.Async) because ViewModels run outside the API context.
Transactions go through `ITransactionManagerFactory` (`Create` / `CreateGroup`, `using`-scoped, with
`FailurePreprocessorType` suppression), never raw `Transaction`.

### Localization

UI strings live in `source/Sonny.Application/Resources/Languages/<Feature>/<Feature>.{en,vi}.xaml` ResourceDictionaries,
registered in `SonnyResourcesInitializer` and resolved at runtime via `IResourceHelper.GetString(key, args…)`. A new
feature with user-facing text needs an entry in both language files and a `RegisterResource` call. `LanguageChangeHandler`
is resolved eagerly at host startup to subscribe to language-change events — that's why `Host.Start` touches it.

## Code style

`.editorconfig` plus Rider settings produce a distinctive layout — **match the surrounding file exactly**:

- A space before every statement-terminating semicolon: `var x = 1 ;`
- Opening brace on the same line for control flow (`if (x) {`), on a new line for types/methods
- File-scoped namespaces, primary constructors for services, expression-bodied members where they fit
- Private fields `_camelCase`, private static `s_camelCase`
- Multi-argument calls are broken one argument per line

Per the Cursor rules: **document interfaces with full XML docs; implementations only when the logic is non-obvious.**
Comments explain *why*, in English.

## Git workflow

Git Flow: feature branches off `develop`, PRs target `develop` (never `master`); `master` is release-only and tags
trigger `PublishRelease.yml`. Commit messages are prefixed `Add:` / `Fix:` / `Update:` / `Refactor:` / `Docs:` / `Test:`.

## Feature workflow — docs are the deliverable

A non-trivial feature runs through the `sonny-flow` plugin, in two commands with a human gate between them:

```
/sonny-flow:spec <Feature>     orient (docs → graphify → codegraph) → ## Spec + ## Contract → ## Plan
                               ── stops here for approval
/sonny-flow:build <Feature>    implement + tests → dotnet test → ## Behaviour + diagrams
```

Everything lands in one file, `docs/features/<Feature>.md`, which starts as spec-plus-plan and ends as
permanent behaviour documentation. **The file is the progress tracker** — an unchecked `- [ ]` under
`## Plan` means the feature is not done. There is no separate artifact directory and no JSON schema.

Three rules that hold whether or not the plugin is driving:

- **Read before writing.** `docs/README.md` → `docs/architecture/command-flow.md` → the relevant feature
  doc, *then* both graphs. The docs carry silent behaviour, order dependencies and unit boundaries that
  neither graph can see; reading code quickly will not tell you whether a `catch { }` is deliberate.
- **Test cases come from `## Contract`, not from the code just written.** A test derived by re-reading
  fresh code passes for wrong code too. Every named failure mode in the contract deserves a test.
- **When a doc disagrees with the code, say so — do not silently fix either side.** The doc may be stale,
  or the code may be wrong. Overwriting the doc to match the code turns a bug into a specification.

The shape of a feature doc, and the two Mermaid diagram types it must carry, are specified in
[`docs/README.md`](docs/README.md). Diagrams belong to the feature: whoever changes the feature updates
its diagram in the same change, because a diagram that no longer matches the code is worse than none.

`docs/adr/` holds decisions that are hard to reverse, surprising without context, and the result of a real
trade-off — all three, or it is not an ADR.

`docs/features/**` is in `claudeMdExcludes`: these are look-up documents, not resident context.

## Knowledge graphs — graphify and codegraph

Two indexes cover this repo and they have different blind spots, so **for a question about the code, ask both.**
Measured over 10 real questions against this repo: codegraph alone answered 5, graphify alone 3, both together 9.
Neither is a fallback for the other — each is the only one that can see part of this codebase.

| | graphify (`graphify-out/`) | codegraph (`.codegraph/`) |
|-------------|--------------------------------------------|-------------------------------------|
| Indexes     | code **+ `.md` docs + `.xaml`**            | code only — `.cs`, `.xml`, `.yaml`  |
| Returns     | node names, communities, wiki pages        | verbatim line-numbered source       |
| Freshness   | manual `graphify update .`                 | auto-syncs ~1s after a save         |
| Learns      | yes — `memory/`, `reflections/LESSONS.md`  | no, always deterministic            |
| Cost        | LLM pass for docs; code updates are free   | zero, fully local                   |

Both hooks are deliberate and stay enabled: codegraph's `UserPromptSubmit` **pushes** likely-relevant symbols into
every turn, graphify's `PreToolUse` **nudges** toward `graphify query` before a grep or a read. That is the soft
nudge, not strict mode — it never blocks a `Read`. Ignore either one when it fuzzy-matches something irrelevant.

### Which one wins when they disagree

| Question | Trust |
|-------------------------------------------------|-------------------------------------------------------------------|
| What the code says, exact line numbers          | **codegraph** — it re-reads from disk on every call               |
| Full blast radius of a change                   | **codegraph** — `impact` is transitive; `graphify affected` stops at `--depth 2` |
| `.xaml` bindings, localization resource keys    | **graphify** — codegraph does not index `.xaml` at all            |
| Architectural intent, anything under `docs/`    | **graphify** — codegraph does not index `.md` at all              |
| Both point at the same place                    | Done — do not verify further                                      |

Two known traps. `codegraph affected` reports "no test files affected" for feature code, because the integration
tests bind to `.rvt` fixtures and `UniqueId`s rather than an import chain — never use it to decide which tests to
run. And `graphify path` routes around `Host.GetService<T>()` through shared namespace imports, returning a path
that is not the real call flow; `docs/architecture/command-flow.md` exists to cover that gap — read it instead.

### Give codegraph symbol names, not an English question

`codegraph explore` expects a bag of symbol names. An English question makes it miss: *"where is
IColumnDataExtractor registered in dependency injection?"* returned `KeygenModels.cs` and `RevitTask.cs`, while
`"AddColumnFromCadServices IColumnDataExtractor ServiceRegistration"` landed on `ServiceRegistration.cs:96`.
Get the names from graphify or from the prompt hook first, then hand those names to codegraph.

### Commands

```powershell
# graphify — orientation, docs, .xaml, architecture
graphify query "<question>"           # scoped subgraph for a question
graphify affected "<X>" --depth 2     # what directly touches X
graphify explain "<X>"                # X plus its neighbours, both directions
graphify update .                     # refresh after edits (AST only, no API cost)
```

```powershell
# codegraph — source to edit, callers, blast radius
codegraph node "<Type.Member>"        # one symbol's source + caller trail
codegraph explore "<Name1 Name2>"     # several symbols' source + call paths
codegraph impact "<Symbol>"           # everything a change reaches
```

`graphify-out/wiki/index.md` is the map for broad navigation; `GRAPH_REPORT.md` only for a full architecture
review. When codegraph proves graphify wrong, record it so the graph learns:
`graphify save-result --question "<q>" --outcome corrected --correction "<what was actually true>"`.

Rebuilding the whole graphify graph (`/graphify`, via `.claude/skills/graphify/SKILL.md`) runs an LLM pass over
the docs and costs tokens — `graphify update .` is the free AST-only refresh and is what you want after editing
code. This section is the single source of truth for both graphs; `.claude/CLAUDE.md` only points back here.
