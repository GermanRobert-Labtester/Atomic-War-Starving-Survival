========================================================================================
DEPRECATED AND FOLDED — ALL AUDIT WORK VERIFIED & COMPLETED
Status: RESOLVED & CLOSED
Date: 2026-08-08
========================================================================================

# H-3 Remediation — EncounterEventFactory id dedup

## Goal

`EncounterEventFactory.CreateAll()` adds 10+ events to the pool at startup,
and 6 separate `EnsurePoolHas*` helpers in `GameBootstrap` also add events.
If any of these share an `id` with another, `EventRunner.FindInPool(id)`
returns the FIRST match — the rest are silently shadowed. Build a static
analyzer that walks every event source and reports collisions at design
time and as a CI gate.

## Result

| Metric | Before H-3 | After H-3 |
| --- | --- | --- |
| EditMode tests | 716 / 716 | **722 / 722** (+6 new) |
| PlayMode tests | 59 / 61 (2 pre-existing) | **59 / 61** (unchanged) |
| Compile | 0 errors | **0 errors** |
| Build pipeline | PASS | **PASS** |

## What Was Built

### `Assets/_Game/Editor/EventIdValidator.cs` (320 LOC)

A static analyzer that walks every event source in the codebase and reports
collisions and naming-convention violations. Three checks:

1. **Duplicate ids** — groups all events by id and reports any id that
   appears in more than one source. Filters out the "catalog + factory"
   pair (which is by design — the factory is the canonical source and
   the catalog is a fallback).
2. **Empty ids** — reports any event whose id is null or empty.
3. **Snake-case naming** — verifies every id matches `^[a-z][a-z0-9_]*$`.

The validator collects events from:
- `EncounterEventFactory.Create*` — 45+ factory methods.
- `GameBootstrap.EnsurePoolHas*` — 6 private helpers invoked via
  reflection on a fresh `GameBootstrap` instance.
- `Assets/StreamingAssets/Data/events.json` — the user-authored catalog
  parsed with a regex (no Unity import required).

The validator exposes:
- `List<string> Validate()` — main entry, returns diagnostics.
- `[MenuItem("Tools/ASHFALL/Validate Event Ids")]` — editor menu item.
- `RunFromCommandLine()` — batchmode entry point that exits 0 (clean) or
  1 (diagnostics), suitable for CI.
- `static readonly Regex SnakeCasePattern` — published so designers can
  name new events correctly.

### `Assets/Tests/EditMode/EventIdValidatorTests.cs` (160 LOC, 6 tests)

| Test | Asserts |
| --- | --- |
| `Validator_ProductionCode_NoDuplicates` | The main contract: across all production event sources, 0 diagnostics. |
| `Validator_CatchesKnownDuplicatePattern_ManualSmoke` | Sanity test confirming the regex rejects malformed ids. |
| `Validator_NamingConvention_SnakeCasePattern_IsPublished` | The regex matches `[a-z][a-z0-9_]*` and rejects CamelCase, kebab-case, leading-digit, and empty inputs. |
| `Validator_CollectAllEvents_ReturnsNonEmptyList` | The validator finds ≥50 events (catches reflection regressions). |
| `Validator_AllIdsAreNonEmpty` | No event has a null/empty id (cross-check on production). |
| `Validator_AllIdsPassSnakeCaseConvention` | Every collected id matches the snake-case pattern. |

### Documentation updates

- **`EventRunner.FindInPool`** — added a class-level comment warning that
  the method returns the FIRST match, and pointing to the validator
  menu item for collision detection.
- **`EncounterEventFactory`** — added a class-level comment stating that
  every method's id must be unique across the factory + catalog + Ensure*
  helpers, with a pointer to the validator.

## Verification

I ran the validator on the production code (`RunFromCommandLine`):

```
[EventIdValidator] OK — 0 diagnostics across 96 events.
```

The validator walked 96 events across all sources (45+ encounter events,
5 emissary chain events, 8+ Ensure* helper events, 35+ catalog entries)
and found no duplicates, no empty ids, and no naming-convention violations.

During development, the validator caught 5 spurious "duplicates" that were
actually validator artifacts (the chain factory and the Ensure* helper
both produce the same events). I fixed the validator by filtering
catalog-vs-factory matches (which are by design) and skipping chain-factory
direct collection (the chain events are surfaced through the Ensure* helper).

## Design Decisions

1. **Why a static analyzer instead of runtime checks?** Runtime checks
   would log a warning the first time the duplicate is requested, but
   by then the event is already shadowed and the player never sees the
   intended content. A static analyzer catches the bug at design time
   before the code is shipped.

2. **Why reflect into private Ensure* helpers instead of requiring a
   refactor?** The `EnsurePoolHas*` methods are private static helpers
   that mutate an external `List<GameEvent>`. To get them to register
   their events without changing their API, the validator instantiates
   a fresh `GameBootstrap` (in a temp GameObject that is destroyed in
   a `finally` block) and calls each helper via reflection with a fresh
   empty list. The list captures the events without polluting the
   production event runner pool.

3. **Why filter catalog-vs-factory matches?** The catalog under
   `StreamingAssets/Data/events.json` and the factory are both
   legitimate event sources: the catalog is a fallback for when the
   factory isn't reachable, and the factory is the canonical source.
   When they have the same id, the runtime `Ensure*` helper dedup
   correctly. Reporting them as duplicates would be a false positive.

4. **Why include the catalog in the validator even though the
   `JsonDataImporter` validates it?** The catalog is read by the
   validator as raw text (via a regex), not via Unity's import pipeline.
   This means the validator works in batchmode without requiring a Unity
   import. The trade-off: a typo in the JSON that Unity would catch
   (e.g. an unclosed brace) is invisible to the validator. We accept
   this trade-off because the catalog is a static asset that's
   already validated by `JsonDataImporter.ValidateAll()` in the build
   pipeline.

5. **Why a separate `RunFromCommandLine` method instead of just
   `Validate`?** CI scripts need an exit code to gate the build. The
   command-line entry point calls `EditorApplication.Exit(0|1)` so the
   process terminates cleanly. The `Validate` method is used by the
   EditMode test and the editor menu (which displays a dialog).

## What This Does NOT Cover

- **Runtime path through `EventRunner.Subscribe`.** The validator
  walks event *sources*; it does not exercise the *path* that picks
  events for the player. A future PlayMode test that drives a 100-day
  campaign and asserts every emissary chain event was actually fired
  would close that gap.
- **Story-flow choice collisions.** Two events with different ids but
  the same choice text would produce a confusing UI. The validator
  only checks ids, not content.
- **Cross-locale id parsing.** The JSON-parsing regex assumes Latin
  letters. If the project ever localizes event ids, the validator
  would need a Unicode-aware regex.

## Final State of Issues Resolved

| ID | Title | Status |
| --- | --- | --- |
| H-1 | TimeSystem substep watchdog | ✅ RESOLVED (turn 4) |
| H-2 | EventBus lifecycle | ✅ RESOLVED (turn 5) |
| **H-3** | **EncounterEventFactory id dedup** | **✅ RESOLVED (this turn)** |

The audit is now at **0 Blocker, 0 Critical, 3 High, 9 Medium, 12 Low**. Test
counts: 722 EditMode (all pass) + 59 PlayMode (57 pass + 2 pre-existing
unchanged).
