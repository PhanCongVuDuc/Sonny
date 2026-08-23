# Sonny documentation

Behaviour documentation, written to be read before writing code or tests — by a person or by an agent.

These files carry what the knowledge graph in `graphify-out/` cannot: intent, business rules, edge cases,
and what a test is supposed to prove. The graph carries structure — which type implements which interface,
what calls what. Use both.

## Read in this order

| Document | When |
|---|---|
| [architecture/command-flow.md](architecture/command-flow.md) | **First.** How any command reaches its interactor, and the DI hops the graph cannot see. Also the UIDocument lifetime rule. |
| [../.sonnyflow/test-safety-net.md](../.sonnyflow/test-safety-net.md) | Before moving, renaming or deleting any type — which tests bind to implementation details, and the retarget rule. Lives in `.sonnyflow/` because it is flow machinery (feeds the refactor baseline step) — note neither knowledge graph indexes dotfolders, so open it directly |
| [features/AutoColumnDimension.md](features/AutoColumnDimension.md) | Working on auto-dimensioning columns against grids |
| [features/ColumnFromCad.md](features/ColumnFromCad.md) | Working on creating Revit columns from a CAD link |
| [features/AutoJoin.md](features/AutoJoin.md) | Working on rule-based joining/unjoining of intersecting elements (ported from AlphaBIM — full of deliberate quirks, read before "fixing" anything) |
| [features/FramingFromCad.md](features/FramingFromCad.md) | Working on creating Revit structural framing from a CAD link (two kept bugs and two opposite justification rules, read before "fixing" anything) |
| [adr/](adr/) | Why a hard-to-reverse decision was made the way it was |
| [bugs/](bugs/) | Known-but-unfixed defects — one file per bug, index in its README. A feature doc links its open bugs from `## Related` |

Structural questions — "what implements `IDimensionCreator`", "what does `ServiceRegistration` touch" — go
to the graph instead:

```powershell
graphify query "<question>"          # architecture, docs, .xaml
codegraph explore "<Sym1 Sym2>"      # verbatim source, callers, blast radius
```

or open `graphify-out/wiki/index.md` for module-level browsing. `CLAUDE.md` → "Knowledge graphs" has the
arbitration table for when the two graphs disagree.

## Shape of a feature document

Every feature document uses the same five sections, in this order. `ColumnFromCad.md` and
`AutoColumnDimension.md` are the reference implementations.

| Section | Kind | Contains |
|---|---|---|
| *(intro)* | — | Two or three lines: what it does, and what the user clicks |
| `## Contract` | **reference** | Input, Output, Invariants, **named failure modes**. Changing this section changes the *requirement* |
| `## Flow` | **diagram** | One `sequenceDiagram` — the cross-layer call path, ViewModel → runner → interactor → API |
| `## Behaviour` | **explanation** | Only what the code does not state plainly, plus one `flowchart TD` per phase with real branching |
| `## What a test should prove` | reference | What is pinned today, the exact command to run it, and the gaps |
| `## Related` | — | Sibling docs, ADRs, a one-line link per **open bug** in [bugs/](bugs/), and a bag of symbol names to hand to `codegraph explore` |

Keeping `Contract` (reference) and `Behaviour` (explanation) apart is deliberate — they answer different
questions and mixing them makes both harder to trust. The intended edit surface is `Contract`.

### What makes these documents worth reading

Not the summary — the parts an agent cannot infer from reading the code quickly:

- **Silent behaviour.** Every `catch { }`, every `continue`, every null-return that drops work without
  telling the user. Both features have several; both documents list them explicitly, and both draw them.
- **Order dependencies.** `ColumnFromCadInteractor` requires `ExtractColumnData` before `CreateColumns` and
  nothing enforces it.
- **Unit boundaries.** Which values are display units and which are internal feet, and where the conversion
  happens.
- **Invariants.** What must not be changed, and what breaks if it is — transaction scope, caching rules.
- **Deliberate oddities.** The crossed grid-lookup arguments in `AutoColumnDimension` look like a typo and
  are not. Say so, or someone will "fix" it.
- **What a test should prove**, including the gaps not covered today, and whether each gap needs Revit.

Skip anything the code already states plainly. A document that restates method signatures adds nothing the
graph does not already have.

### Diagrams

Mermaid only — GitHub renders it inline in `.md`, it diffs in a PR, and it is text so a routine feature
task can update it. Two kinds, one abstraction level each:

- **`sequenceDiagram`** for the cross-layer call path. This is the one the graph genuinely cannot produce:
  `graphify path` routes around `Host.GetService<T>()` and returns a path that is not the real call flow.
- **`flowchart TD`** for decision and failure paths inside a phase. Every silent skip gets a node, so
  "45 columns in, 30 created, nothing said about the other 15" is visible at a glance.

Do not use Mermaid's `C4Context` syntax — it is still experimental and GitHub does not render it. Keep C4's
*thinking* instead: one abstraction level per diagram, never mixed. And no SVG or HTML diagrams — their
layout is hand-maintained coordinates, so they stop being updated after the first change.

**A diagram that no longer matches the code is worse than no diagram.** Whoever changes a feature updates
its diagram in the same change, and verifies it against `codegraph` before closing the task.

## Keeping the graph in sync

`graphify hook install` has been run, so `graph.json` rebuilds on every commit (AST only, no API cost) and
merges cleanly when two branches touch it. After adding or editing a document here, run:

```powershell
graphify update .
graphify export wiki
```

so the new prose becomes graph nodes linked to the code it describes.
