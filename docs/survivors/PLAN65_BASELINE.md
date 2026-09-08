# Plan 65 — Final Wishes Expansion Baseline Reconnaissance

**Document:** `docs/survivors/PLAN65_BASELINE.md`
**Date:** 2026-09-08
**Subsystem:** Survivors / Impending Death / Final Requests (`FinalWishSystem`)

---

## 1. System Inspection Summary

`Assets/Ashfall.Core/Survivors/FinalWishSystem.cs` was inspected end-to-end:
- **Eligibility:** `DeclareTerminalPrognosis(survivorId, archetypeId, isAlive)` activates a final wish when terminal radiation poisoning / fatal prognosis is resolved.
- **Clock & Ticking:** `Tick(survivorId, gameHours, isAlive)` decrements `daysRemaining` (drawn deterministically between `DefaultPrognosisDaysMin = 3f` and `DefaultPrognosisDaysMax = 7f` via `ISeededRng`).
- **Steps:** A sequential counter (`stepsCompleted`). `AdvanceWishStep(survivorId, stepId)` increments the counter. Required steps per wish type are evaluated via switch:
  - `see_the_sky` -> 1 step
  - `build_memorial` -> 3 steps
  - `retrieve_heirloom`, `deliver_letter`, `teach_lesson`, `reconcile` -> 2 steps
  - default (`_ => 2`) -> 2 steps
- **Consequences:** Fixed upon completion (`WishCompletedMoraleBuff = 15f`, `buff_id = "their_memory_lives_on"`). Failure on prognosis expiry applies `WishFailedMoralePenalty = -10f`.
- **Save State:** `FinalWishSaveState` holding a list of `FinalWishSurvivorState` and `Dictionary<string, string> archetypeWishes`. Deep-copied on capture/restore.
- **Catalog Source:** `Assets/StreamingAssets/Data/final_wishes.json` (`schema_version: 1`, top-level `items` array).

---

## 2. Baseline Status

- **Baseline Wishes:** 8 canonical initial entries (`the_surgeon`, `the_soldier`, `the_nurse`, `the_mother`, `the_mechanic`, `the_teacher`, `the_refugee`, `the_electrician`).
- **Expansion Target:** 22 new wishes across 10 requested wish types, reaching **30 total wishes**.
- **Survivors Roster:** 129 total survivors in `survivors.json`, 69 of which have `the_*` archetype IDs.
- **Data Integrity Gate:** Pass (`DATA_INTEGRITY_SELFTEST PASS — 0 findings across 298 catalogs`).
- **Test Suite Baseline:** Pass (`dotnet test Ashfall.Core.Tests` 9357 passed).
