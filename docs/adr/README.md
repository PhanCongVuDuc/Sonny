# Architecture decision records

One decision per file, numbered sequentially: `0001-slug.md`, `0002-slug.md`. Once written, an ADR is not
edited — it gets superseded by a later one.

## When to write one

All three must be true. Two out of three is not enough.

1. **Hard to reverse** — changing your mind later costs real work.
2. **Surprising without context** — a future reader will look at the code and ask "why on earth is it done
   this way?"
3. **A real trade-off** — there were genuine alternatives and one was picked for specific reasons.

If a decision is easy to reverse, skip it — you will just reverse it. If it is not surprising, nobody will
wonder why. If there was no alternative, there is nothing to record beyond "we did the obvious thing".

## What qualifies here

Concrete examples from this codebase that would each earn an ADR:

- `Infrastructure` references `UseCases` so it can implement input ports — a deliberate Clean Architecture
  deviation that a reader would otherwise try to "fix".
- `AutoColumnDimensionInteractor` lives in `Infrastructure` while `ColumnFromCadInteractor` lives in
  `UseCases`. Same role, opposite placement, for a reason.
- `IsRepackable` on for Release, off for Debug — ILRepack merges dependencies because the add-in's
  `System.Text.Json` is newer than Revit's, but Debug must leave them separate so the test project can
  reference the same types.
- Integration tests binding to hard-coded `UniqueId`s in fixture `.rvt` files instead of building models
  in code.

## What does not belong

Current behaviour of a feature — that goes in [`../features/`](../features/), which gets overwritten every
time the feature changes. An ADR is a decision at a point in time; a feature doc is the present tense.
Mixing them means the first refactor deletes the reasoning.

## Format

A single paragraph is a valid ADR. Title states the decision as a claim, not a topic:

```markdown
# {The decision, as a statement}

{1–3 sentences: the context, what was decided, and why.}

## Phương án đã loại        ← only when the rejected options are worth remembering
## Consequences             ← only when a downstream effect is non-obvious
```

Add `status: superseded by <NNNN-slug>` as frontmatter when a later ADR replaces this one.
