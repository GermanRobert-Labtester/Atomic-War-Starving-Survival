========================================================================================
DEPRECATED AND FOLDED — ALL AUDIT WORK VERIFIED & COMPLETED
Status: RESOLVED & CLOSED
Date: 2026-08-08
========================================================================================

# H-3 Remediation Plan — EncounterEventFactory id dedup

## Goal

`EncounterEventFactory.CreateAll()` adds 10+ events to the pool at startup,
and 6 separate `EnsurePoolHas*` helpers in `GameBootstrap` also add events.
If any of these share an `id` with another, `EventRunner.FindInPool(id)`
returns the FIRST match (`List<T>.Find` does a linear scan, returns the
first match). The second event is silently shadowed.

This is a real bug class because:
- The factory and the `Ensure*` helpers were written by different prompts
  in different PRs and never cross-checked.
- A duplicated id is invisible in normal play (the player just never
  sees the shadowed event).
- A future maintainer adding a new event to the factory might pick an
  id that already exists in the catalog, also producing a silent shadow.

## Audit Strategy

1. **Static analyzer** — Walk every GameEvent source and assert no
   duplicate ids. Run as a one-shot command in CI and a test in the
   EditMode suite.

2. **EventRunner.FindInPool** — confirm it returns the first match (the
   current behavior, which is the silent-shadow risk).

3. **Test coverage** — at least 3 tests:
   - **No duplicate ids across factory + Ensure* helpers + catalog**
   - **All events have non-empty ids** (catches "id = ''" typos)
   - **All event ids are snake_case** (matches the project's AGENTS.md
     convention)

4. **Document the contract** — add a class-level comment to
   `EventRunner.FindInPool` and a developer note in `EncounterEventFactory`
   warning future maintainers to run the analyzer.

## Files

- `Assets/_Game/Editor/EventIdValidator.cs` (new) — editor tool that walks
  every event source and reports duplicate ids.
- `Assets/Tests/EditMode/EventIdValidatorTests.cs` (new) — automated
  test that runs the validator against the production code and asserts
  no duplicates.
- `Assets/_Game/Events/EventRunner.cs` — add class-level comment to
  `FindInPool` documenting the first-match semantics.
- `Assets/_Game/Data/EncounterEventFactory.cs` — add developer note
  pointing to the validator.
- A menu item under `Tools/ASHFALL/Validate Event Ids` so the
  validator is accessible from the Unity editor.

## Risk

- The validator might surface REAL duplicates that nobody noticed. If
  so, the fix is to rename the duplicate id and update the catalog.
  I'll handle that during implementation.
- The validator must not block CI indefinitely; it should complete in
  < 1s.
