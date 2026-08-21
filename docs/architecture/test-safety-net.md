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
| `AutoColumnDimensionIntegrationTest` ([source](../../source/Sonny.Application.Tests/Features/AutoColumnDimension/IntegrationTests/AutoColumnDimensionIntegrationTest.cs)) | Constructs the UseCases `AutoColumnDimensionInteractor` **by hand** in `OnSetup` (to inject a mocked `IMessageService`) → bound to the constructor signature and both port interfaces | The interactor's constructor or ports change. Retarget the wiring only; the 80-dimension assertion must not move |
| `ColumnFromCadIntegrationTest` ([source](../../source/Sonny.Application.Tests/Features/ColumnFromCad/IntegrationTests/ColumnFromCadIntegrationTest.cs)) | Calls `ExtractColumnData` and `CreateColumns` **separately** (bound to the method pair and the `_extractedColumns` state between them); hard-coded `UniqueId`s of the fixture `.rvt`; expected rotation angles compared with **exact double equality** | The method pair is merged/renamed; the fixture changes; **any** floating-point drift in the extraction math — `RectangularColumnModel.FromCorners` must stay bit-compatible with Revit's `Normalize`/`AngleTo` (`atan2(|a×b|, a·b)`) |
| Characterization tests in `Sonny.Application.UnitTests` | The resource-key passthrough convention (`GetString` mocked to return the key), and the public API of Domain/UseCases decision code | Message routing switches resource keys; decision classes change signature. These tests pin *current* behaviour — a red one means behaviour changed, not that the test is stale |

## How to rebuild this inventory (baseline step of a refactor)

For every type about to move, rename, or be deleted, search the test projects for direct bindings:

```powershell
# constructed by hand, resolved as a concrete type, or called directly
grep -rn "new <TypeName>\|GetService<<TypeName>>\|<TypeName>\." source/Sonny.Application.Tests source/Sonny.Application.UnitTests
```

Record every hit in the refactor's ADR draft as a known risk **before** the first move: which test, which
binding, and whether the planned fix is wiring-only. A binding discovered at the gate instead of at
baseline means the baseline step was skipped.

## Related

- [ADR 0001](../adr/0001-decision-logic-lives-in-usecases.md) — the refactor where the
  `AutoColumnDimensionIntegrationTest` wiring retarget happened, and the bit-compatibility constraint was
  discovered
- Feature docs carry what each test *asserts*; this file carries what each test *touches*
