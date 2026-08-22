# Pure Revit API helpers live in Sonny.RevitExtensions, not in Infrastructure

Decided while porting AutoJoin (2026-08-22). The feature needed four framework-level helpers: the largest
solid of an element, join/unjoin with cut-order control, a bounding-box-intersection probe with an X/Y
expansion, and a strict solid-intersection check. All four went into the `Sonny.RevitExtensions`
submodule instead of `Sonny.Application.Infrastructure`, and this is the rule for the next feature too.

## The rule

A method goes to `Sonny.RevitExtensions` when **all** of these hold:

- It touches only the Revit API and BCL types — no Sonny service, no DI, no `IRevitDocument`, no logging.
- Its behaviour is a general fact about Revit ("the biggest solid of an element", "join and make A cut
  B"), not a policy of one feature ("treat an unverifiable pair as intersecting" stays in the AutoJoin
  adapter).
- It would be equally correct in any other add-in.

Everything else — anything that resolves a service, reads through `IRevitDocument`, chooses an error
policy, or encodes a feature's category rules — stays in Infrastructure behind the feature's ports.

## Why this is worth an ADR

- **Khó đảo:** the submodule is a public library surface shared across repos; once callers exist in more
  than one product, moving a method back means a breaking change everywhere at once.
- **Người sau sẽ hỏi:** the natural place for "code the feature needs" is the feature's Infrastructure
  folder — finding it in a submodule needs an explanation.
- **Phương án đã loại:** keeping the helpers in `Infrastructure/Features/AutoJoin` was rejected
  deliberately — the owner is growing `Sonny.RevitExtensions` into a large reusable library, and helpers
  buried in a feature folder get rewritten instead of reused.

## The trap discovered while applying it

`Sonny.RevitExtensions` globally imports **Nice3point.Revit.Extensions**, which already ships many
generic helpers. Adding `ToElementId(long)` to the submodule produced an ambiguous-call compile error
across every consumer, because Nice3point already has that exact extension. **Before adding a helper to
the submodule, grep Nice3point.Revit.Extensions for it first.** Version-safe `ElementId` conversion, unit
conversion (`FromMillimeters`), and much of the query surface already exist there.

## Consequences

- Changes to the submodule are commits in **its own repository** (the Sonny repo only moves the
  submodule pointer) — per the standing submodule rule in `CLAUDE.md`.
- AutoJoin's additions: `ElementSolidExtensions.GetSolidMax`, `ElementJoinExtensions` (new),
  `ElementIntersectExtensions.GetBoundingBoxIntersectingElements`,
  `SolidIntersectExtensions.IntersectsSolidByUnion`, `Utilities/ElementCategoryFilters`.
- These helpers have no tests: the submodule has no test infrastructure that runs outside Revit, and all
  five need the Revit API at runtime. Their decision-making callers are tested Revit-free in
  `Sonny.Application.UnitTests` instead.
