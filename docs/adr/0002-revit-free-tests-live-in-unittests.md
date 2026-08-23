# A test project is chosen by what the test needs, not by what it is named

`Sonny.Application.Tests` launches a real Revit process for every run. It also held five fixtures filed
under `Core/UnitTests` and `ResourceManager/UnitTests`, named and foldered as if they were plain unit
tests. Only three of them actually were, so those three moved to `Sonny.Application.UnitTests`, one was
deleted, and the last stayed where it was under a folder name that admits what it needs.

## What each of the five turned out to be

| Fixture | What it really depends on | Outcome |
|---|---|---|
| `CultureChangedEventArgsTests` | `Sonny.ResourceManager` | Moved to `Sonny.Application.UnitTests/ResourceManager/` |
| `LanguageCodeExtensionsTests` | `Sonny.ResourceManager` | Moved to `Sonny.Application.UnitTests/ResourceManager/` |
| `LanguageOptionTests` | `Domain.Entities.Settings`, `UseCases.Settings.Models` | Moved to `Sonny.Application.UnitTests/UseCases/Settings/` |
| `SettingsServiceLanguageTests` | `Infrastructure` — and the real `%APPDATA%\Sonny\SonnySettings.json` | **Deleted** |
| `UnitConverterTests` | The Revit API: `ForgeTypeId`, `UnitTypeId`, `UnitUtils.Convert` | Stayed; folder renamed `Core/UnitTests/Services` → `Core/RevitApiTests` |

`UnitConverterTests` is the reason this ADR exists. It tests one small class with no Revit type in its own
signature, so it looks Revit-free; it passes only because it runs inside a Revit process. Anyone reading
the folder name would move it and watch it fail.

`SettingsServiceLanguageTests` was deleted rather than moved. `SettingsService` builds its file path in its
constructor with no seam, so the nine tests drove the developer's real settings file — order-coupled with
each other and destructive to whatever language the developer had picked — while asserting little beyond
getter/setter round-tripping. The coverage it never really provided is recorded as a gap in
[`test-safety-net.md`](../../.sonnyflow/test-safety-net.md).

## Phương án đã loại

**Add a `ProjectReference` to `Infrastructure` and move all five.** `Infrastructure` references
`Nice3point.Revit.Api.RevitAPI`, so this would pull the Revit API into the project whose entire premise is
that it has no Revit dependency and finishes in seconds. The comment at the top of
`Sonny.Application.UnitTests.csproj` states that premise; this option would have made it false.

**Move `SettingsService` itself out of `Infrastructure` first.** It is genuinely misplaced — pure .NET
sitting in the Revit layer. But that move touches the composition root and `ServiceRegistration`, which is
a different change with a different blast radius, and it should not ride along inside a test reorganisation.

**Extract the non-Revit part of `UnitConverter`.** `GetUnitDisplayName` is a `switch` over strings, but the
strings come from `ForgeTypeId.TypeId`. Splitting it means changing product code to suit a test's filing
location, which is the wrong direction of causation.

## Consequences

**The two projects speak different assertion dialects.** `Sonny.Application.Tests` is on NUnit 3, where
`Assert.AreEqual` is fine. `Sonny.Application.UnitTests` is on NUnit 4, where the classic asserts moved to
`NUnit.Framework.Legacy.ClassicAssert`. Every fixture that crosses this boundary must be rewritten to the
constraint model — `Assert.That(actual, Is.EqualTo(expected))`. That is a mechanical retarget with the
expected values untouched, and it is the kind of implementation-detail rebinding that
[`test-safety-net.md`](../../.sonnyflow/test-safety-net.md) allows on condition it is recorded here.

**NUnit 4's analyzers reject assertions NUnit 3 tolerated.** `LanguageOptionTests.Properties_ShouldBeReadOnly`
asserted that an `AppLanguageCode` is not null. An enum never is, so the assert could not fail; NUnit 4
raises `NUnit2023` as an error and refuses to compile. It was dropped rather than replaced — removing an
always-true assertion cannot change a test's outcome — with a comment in place so it is not re-added.
Expect more of these when other fixtures cross over.

**`UnitConverterTests` could not be verified after its move.** On the machine where this refactor was done,
`ricaun.RevitTest.TestAdapter` never got a Revit process up: every test failed with
`RevitTest: Timeout 10 minutes`, and even `--list-tests` returned nothing. That was equally true *before*
the move, so it is an environment limit rather than a regression, and the change to that file is a
namespace rename the compiler checks. It still means the gate for this refactor reads **pass for the
Revit-free project, not-verifiable for the Revit one** — the two are not the same result and should not be
reported as one.

**The move made the Revit-free three actually runnable.** In their old home they were hostage to a Revit
launch that does not happen on every machine. In `Sonny.Application.UnitTests` they run in 255 ms with the
other 46. That, more than tidiness, is what the move bought.
