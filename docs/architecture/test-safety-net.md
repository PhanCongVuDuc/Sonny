# Test safety net — what the tests bind to, and what a refactor must check first

Two different things break tests during a refactor, and they need opposite reactions:

- **An assertion changes value** → behaviour changed. Stop, a human decides (refactor flow rule).
- **The test no longer compiles** → the test was bound to an *implementation detail* (a concrete type it
  constructs, a method pair it calls directly). Retargeting that wiring is allowed, but only with the
  assertions untouched, and it must be recorded in the ADR of the refactor.

The second kind is invisible until the move is already made — unless it is inventoried **before** moving,
during the baseline step. This file is that inventory. **Whoever moves or deletes a type updates this
file in the same change.**

## Inventory — implementation details the current tests bind to

| Test | Binds to | Breaks when |
|---|---|---|
| `AutoColumnDimensionIntegrationTest` ([source](../../source/Sonny.Application.Tests/Features/AutoColumnDimension/IntegrationTests/AutoColumnDimensionIntegrationTest.cs)) | Constructs the UseCases `AutoColumnDimensionInteractor` **by hand** in `OnSetup` (to inject a `FakeMessageService` from `TestDoubles.cs` — NSubstitute is banned in Revit-hosted tests, see revit-test-environment.md) → bound to the constructor signature and both port interfaces | The interactor's constructor or ports change. Retarget the wiring only; the 80-dimension assertion must not move |
| `ColumnFromCadIntegrationTest` ([source](../../source/Sonny.Application.Tests/Features/ColumnFromCad/IntegrationTests/ColumnFromCadIntegrationTest.cs)) | Calls `ExtractColumnData` and `CreateColumns` **separately** (bound to the method pair and the `_extractedColumns` state between them); hard-coded `UniqueId`s of the fixture `.rvt`; expected rotation angles compared with **exact double equality** | The method pair is merged/renamed; the fixture changes; **any** floating-point drift in the extraction math — `RectangularColumnModel.FromCorners` must stay bit-compatible with Revit's `Normalize`/`AngleTo` (`atan2(|a×b|, a·b)`) |
| Characterization tests in `Sonny.Application.UnitTests` | The resource-key passthrough convention (`GetString` mocked to return the key), and the public API of Domain/UseCases decision code | Message routing switches resource keys; decision classes change signature. These tests pin *current* behaviour — a red one means behaviour changed, not that the test is stale |
| `UnitConverterTests` ([source](../../source/Sonny.Application.Tests/Core/RevitApiTests/UnitConverterTests.cs)) | The Revit API itself — `UnitUtils.Convert` and the `ForgeTypeId` string literals (`autodesk.unit.unit:millimeters-1.0.1` …) that `GetUnitDisplayName` switches on | Autodesk renames a unit TypeId in a new Revit year. Lives in `Core/RevitApiTests`, **not** in the Revit-free project, precisely because of this (ADR 0002) |
| `CultureChangedEventArgsTests`, `LanguageCodeExtensionsTests`, `LanguageOptionTests` (moved to `Sonny.Application.UnitTests` by ADR 0002) | Nothing beyond the public API of `Sonny.ResourceManager`, `AppLanguageCode` and `LanguageOption` | Those public shapes change. They assert plain round-tripping and are cheap to retarget |
| `AutoJoinInteractorTests` ([source](../../source/Sonny.Application.UnitTests/UseCases/AutoJoin/AutoJoinInteractorTests.cs)) | The two port interfaces (`IAutoJoinScopeReader`, `IAutoJoinPairExecutor`), the resource-key passthrough convention, `Received.InOrder` on the `BeginAnchor` → check → join sequence, and the literal transaction name `"Auto Join"` | A port signature changes (wiring-only retarget allowed). A changed *assertion* means a ported AlphaBIM behaviour changed — every test maps to a named failure mode F3–F11 in [AutoJoin.md](../features/AutoJoin.md), read D1–D8 there before "fixing" anything |
| `AutoJoinSeedMapperTests` ([source](../../source/Sonny.Application.UnitTests/UseCases/AutoJoin/AutoJoinSeedMapperTests.cs)) | Hard-coded numeric `BuiltInCategory` values (−2001330, −2000011, …) and the deliberately quirky mapping D4 | The mapping is "corrected" — the quirk (Structural Columns → Architectural Column, …) is the requirement, not a bug |
| `AutoJoinIntegrationTest` ([source](../../source/Sonny.Application.Tests/Features/AutoJoin/IntegrationTests/AutoJoinIntegrationTest.cs)) | Constructs the UseCases `AutoJoinInteractor` **by hand** (`FakeProgressReporter`/`FakeMessageService` from `TestDoubles.cs`); finds fixture elements by **Comments-parameter tags** (`AutoJoinFixtureTags`), not `UniqueId`s; needs the fixture's `AutoJoin 3D` view active (the scope reader is view-scoped); one 15 m station per test keeps tests order-independent despite sharing the open document | The interactor constructor or the tags change; someone renames the `AutoJoin 3D` view; station elements are moved close enough for joins to leak across cases |
| `AutoJoinFixtureBuilder` ([source](../../source/Sonny.Application.Tests/Features/AutoJoin/IntegrationTests/AutoJoinFixtureBuilder.cs)) | The Revit 2023 environment itself: stock family templates under `C:\ProgramData\Autodesk\RVT 2023\Family Templates`, the placeholder project as seed, in-memory `LoadFamily`, plain `Commit()` (no callbacks defined in the test assembly — they fail silently under the ricaun host) | Runs only when `Test_V2023_AutoJoin.rvt` is missing and the process is Revit 2023 — otherwise `Assert.Ignore`. Delete the fixture to rebuild it; the builder self-verifies every case's solids before saving |

## How to rebuild this inventory (baseline step of a refactor)

For every type about to move, rename, or be deleted, search the test projects for direct bindings:

```powershell
# constructed by hand, resolved as a concrete type, or called directly
grep -rn "new <TypeName>\|GetService<<TypeName>>\|<TypeName>\." source/Sonny.Application.Tests source/Sonny.Application.UnitTests
```

Record every hit in the refactor's ADR draft as a known risk **before** the first move: which test, which
binding, and whether the planned fix is wiring-only. A binding discovered at the gate instead of at
baseline means the baseline step was skipped.

## Known gaps — behaviour nothing pins today

| Gap | Why there is no test | What it would take |
|---|---|---|
| `SettingsService` — language and display-unit persistence, the cache, and the change events | Its only fixture (`SettingsServiceLanguageTests`) was **deleted** by [ADR 0002](../adr/0002-revit-free-tests-live-in-unittests.md). It drove the real `%APPDATA%\Sonny\SonnySettings.json`, so running the suite destroyed whatever language the developer had chosen, and the nine tests shared that one file in an order-dependent way | `SettingsService` hard-codes its path in the constructor. Give it an injectable directory (or an `ISettingsFileLocation` port) and the fixture becomes an ordinary temp-folder unit test in `Sonny.Application.UnitTests` |
| Everything in `Sonny.Application.Tests`, on any machine where Revit will not launch | `ricaun.RevitTest.TestAdapter` waits for a Revit process and fails every test with `RevitTest: Timeout 10 minutes` if none appears — discovery (`--list-tests`) needs it too. This is an environment limit, not a code failure, but it means **a green local run is not available to everyone** | Nothing in this repo. Treat "not verifiable" as a distinct outcome from "pass" when reporting a gate |

## Related

- [ADR 0001](../adr/0001-decision-logic-lives-in-usecases.md) — the refactor where the
  `AutoColumnDimensionIntegrationTest` wiring retarget happened, and the bit-compatibility constraint was
  discovered
- [ADR 0002](../adr/0002-revit-free-tests-live-in-unittests.md) — why the Revit-free fixtures moved out of
  `Sonny.Application.Tests`, why `UnitConverterTests` stayed, and why `SettingsServiceLanguageTests` was deleted
- Feature docs carry what each test *asserts*; this file carries what each test *touches*
