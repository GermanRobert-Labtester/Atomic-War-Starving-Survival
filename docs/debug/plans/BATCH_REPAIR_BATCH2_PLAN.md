# Batch 2 Repair Plan

**Source audit:** `docs/debug/10LOOP_BATCH3_AUDIT.md`
**Prior batch:** `docs/debug/BATCH_REPAIR_5BUGS_RESOLUTION.md` (BUG-06/07/08/09/10 RESOLVED)
**Scope:** Surgical Core-only patches. No host wiring, no JSON data, no schema migration.

## Selection Rationale

Skipping BUG-01 (CRITICAL — 8 orphan Batch 3 systems) — that work needs a separate dedicated plan with its own Phase 2 (JSON catalogs) and Phase 4 (host wiring) — not surgical Core fixes. Defer to a future batch.

Skipping BUG-02 (HIGH — empty catalogs) — coupled to BUG-01; will resolve when BUG-01 is addressed.

Skipping BUG-03/04 (HIGH — thermal integration/physics) — design-ambiguous (audit §13 Cross-System Chains); requires host integration decisions.

Skipping BUG-11 (MEDIUM — Decon net contamination) — design question, not a code defect.

Selecting the 5 surgical Core-only patches below.

| Phase | Bug | Severity | File | Diff |
|---|---|---|---|---|
| 1 | BUG-05 Chronic unreachable | MEDIUM | `MentalHealthCrisisSystem.cs` | ~12 lines |
| 2 | BUG-12 stale boiler temp | LOW | `ShelterThermalSystem.cs` | 1 line |
| 3 | BUG-15 brownout dead branch | LOW | `ShelterScheduleSystem.cs` | ~6 lines |
| 4 | MH caregiver-clear-on-resolve | LOW | `MentalHealthCrisisSystem.cs` | ~2 lines |
| 5 | Library zero-hours completion | LOW | `LibraryStudySystem.cs` | ~4 lines |

## Invariants

1. Core remains engine-agnostic — no `Godot.*`, no `UnityEngine.*`.
2. RNG is `ISeededRng` only.
3. `CaptureState/RestoreState` continue to round-trip.
4. Adjacent xUnit tests stay green.
5. Determinism preserved.
6. Regression tests use production authority path (no shadow test doubles).

## Pre-existing invariants (from Batch 1 verification)

- 2435 tests pass baseline.
- No engine coupling introduced.

## Verification ladder

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj       → 0 errors
2. dotnet test  Ashfall.Core.Tests/<bug_regression> --filter ...   → pass
3. dotnet test  Ashfall.Core.Tests (full)                          → ≥2435+ pass
```

No host re-run, no Unity, no godot for surgical Core edits.

## Execution order

Sequential, one bug at a time, with pre-integration checkpoint before each edit.

## Definition of Done

- 5 bugs closed
- 5 regression tests added (or existing test that now actually covers the case)
- Full suite still ≥2435+ pass (no regressions)
- No new warnings introduced

## Rollback strategy

Each fix is a single-file change. Per-bug `git checkout <file>` restores prior behavior. All fix files brittle enough that an obvious re-fix is possible.
