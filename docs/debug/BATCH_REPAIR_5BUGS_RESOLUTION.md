# 5-Bug Resolution Report

**Plan:** `docs/debug/plans/BATCH_REPAIR_5BUGS_PLAN.md`
**Log:** `docs/debug/logs/BATCH_REPAIR_5BUGS_IMPLEMENTATION_LOG.md`
**Audit:** `docs/debug/10LOOP_BATCH3_AUDIT.md`
**Commit SHA (post-repair):** see `git status` / `git diff --stat`
**Scope:** 5 surgical MEDIUM Core-only patches (BUG-06, BUG-07, BUG-08, BUG-09, BUG-10).

---

## BUG-06 — ContractorRoster Expired-Then-Paid Race

### Original Bug
On the exact expiry day, `ContractorRosterSystem.TickDay` processed hazard pay for a contractor whose contract was ending today — `c.status == Active` at the top of the loop, so `missedPayments++` and the loyalty penalty ran even though the contractor was leaving that same day. Audit §12 Cluster A.

### Reproduction
Pre-fix test `TickDay_OnExpiryDay_DoesNotAccrueMissedPayment`: stripe system to day 9, hire with `termDays = 3 → expiryDay = 12`, drain funds, tick day 12. Pre-fix: `status = Expired` AND `missedPayments = 1`.

### Root Cause
Order of operations inside the iteration body: hazard-pay loop ran before the expiry-check branch, both targeting the same contractor in the same tick when `day == expiryDay`.

### Selected Repair
Moved the expiry-check branch above the hazard-pay block and added `continue` after it. ~6 lines relocated, no new state.

### Files Changed
- `Assets/Ashfall.Core/ContractorRosterSystem.cs` — branch ordering.

### Regression Test Added
`Ashfall.Core.Tests/ContractorRosterSystemTests.cs::TickDay_OnExpiryDay_DoesNotAccrueMissedPayment`

### Verification
```
dotnet test --filter "FullyQualifiedName~ContractorRosterSystemTests" → 7/7 PASS
```

### Save Compatibility
No DTO change → all existing saves load as before.

### Determinism
No new RNG draws; iteration order unchanged.

### Architecture Impact
None. Core-only edit.

### Plan Divergences
None.

### Adversarial Post-Fix Results
- Cancellation paths (`CancelPair`-equivalent) — N/A, contractor cannot cancel mid-tick.
- Multiple contractors in same loop — verified all contractors checked separately each iteration.
- Empty offer list (`activeOffer == null` path) — still skipped correctly.

### Remaining Risk
None.

### Status
**RESOLVED**

---

## BUG-07 — ShelterScheduleScheduleModifierIgnored

### Original Bug
`ShelterScheduleSystem.TickDay` ignored `def.fatigueRecoveryModifier` during the day phase, hardcoding `1f` instead. Only the curfew branch used the definition. Audit §7 BUG-07.

### Reproduction
Pre-fix tests `TickDay_DayPhase_UsesScheduleFatigueModifier` and `TickDay_DayPhase_PropagatesRestlessSchedule` both failed: a schedule with `fatigueRecoveryModifier = 1.3f` reported `1.0f` after `TickDay`.

### Root Cause
`_state.curfewActive ? def.fatigueRecoveryModifier : 1f` — the day-phase branch was a copy-paste from the default schedule.

### Selected Repair
Replaced `1f` with `def.fatigueRecoveryModifier` so the modifier applies across phases. Emergency override remains the override. (Falsification reaffirmed: the original ternary collapsed after the fix.)

### Files Changed
- `Assets/Ashfall.Core/ShelterScheduleSystem.cs`.

### Regression Tests Added
- `TickDay_DayPhase_UsesScheduleFatigueModifier` (asserts `1.3f`)
- `TickDay_DayPhase_PropagatesRestlessSchedule` (asserts `0.7f`)

### Verification
```
dotnet test --filter "FullyQualifiedName~ShelterScheduleSystemTests" → 12/12 PASS
```

### Save Compatibility
No DTO change.

### Determinism
Determinism preserved (no new RNG usage).

### Architecture Impact
None.

### Plan Divergences
Originally considered a more invasive fix (introducing `dayFatigueRecoveryModifier`). Falsification proved unnecessary; the minimal one-liner covers the documented bug.

### Adversarial Post-Fix Review
- Default schedule `fatigueRecoveryModifier = 1f` → behavior unchanged for the default path. Confirmed: existing 10 tests still pass.
- Emergency override path: `0.5f` modifier still applied on top of `def.fatigueRecoveryModifier`'s emergency contribution.
- Lighting demand: untouched.

### Remaining Risk
None.

### Status
**RESOLVED**

---

## BUG-08 — SumpFlooding EquipmentDisabled Latch

### Original Bug
When a sump node drained naturally to 0 cm during `TickDay`'s decay branch, `equipmentDisabled` was not reset, leaving the node falsely unavailable. Audit §8 BUG-08.

### Reproduction
Pre-fix test `TickDay_NaturalDrainComplete_ResetsEquipmentDisabled`: pre-flood node with `isFlooded = false`, `equipmentDisabled = true`, `waterLevelCm = 2f`. Pre-fix: `waterLevelCm = 0` but `equipmentDisabled` still `true`.

### Root Cause
Missing branch. `DrainNode` correctly resets the latch; only the natural-decay path was missing the reset.

### Selected Repair
Add `node.equipmentDisabled = false` immediately before the `DrainComplete` incident log inside the natural-decay branch.

### Files Changed
- `Assets/Ashfall.Core/SumpFloodingSystem.cs`.

### Regression Test Added
`Ashfall.Core.Tests/SumpFloodingSystemTests.cs::TickDay_NaturalDrainComplete_ResetsEquipmentDisabled`

### Verification
```
dotnet test --filter "FullyQualifiedName~SumpFloodingSystemTests" → 6/6 PASS
```

### Save Compatibility
No DTO change.

### Determinism
No RNG impact.

### Architecture Impact
None.

### Adversarial Post-Fix Review
- Repeated flooding cycles: when the node floods again via the threshold check, `equipmentDisabled` is re-set; cycle works.
- Mid-drain interruption: if `waterLevelCm` rises before reaching 0, `equipmentDisabled` remains true (correct — node is still waterlogged).

### Remaining Risk
None.

### Status
**RESOLVED**

---

## BUG-09 — MentalHealth Caregiver Eligibility

### Original Bug
`MentalHealthCrisisSystem.BeginTreatment` accepted any caregiver id without checking whether they were already on duty — pulling critical shift workers away from their posts. Audit §8 BUG-09.

### Reproduction
Pre-fix test `BeginTreatment_CaregiverOnDuty_Blocks`: registers a survivor via `WriteName`, places them on a duty shift via `Assign`, attempts `BeginTreatment`. Pre-fix: `status == Success`, crisis moved to `InTreatment`.

### Root Cause
Missing eligibility check. `TriggerCrisis` carefully removes the patient from duty; no symmetric check on caregivers.

### Selected Repair
Add `_roster.GetRoleOf(caregiverId) != null` guard at the top of `BeginTreatment`. Failure code `mental.caregiver_busy`. Block (not Fail) — the request is well-formed, just rejected on a precondition.

### Files Changed
- `Assets/Ashfall.Core/MentalHealthCrisisSystem.cs` — 1-line guard.

### Regression Test Added
`Ashfall.Core.Tests/MentalHealthCrisisSystemTests.cs::BeginTreatment_CaregiverOnDuty_Blocks`

### Verification
```
dotnet test --filter "FullyQualifiedName~MentalHealthCrisisSystemTests" → 8/8 PASS
```

### Save Compatibility
No DTO change.

### Determinism
Deterministic check (no RNG).

### Architecture Impact
None. Uses existing `DutyRosterSystem.GetRoleOf` API.

### Adversarial Post-Fix Review
- Empty `caregiverId == null` ?: not signaled as "caregiver" — guard short-circuits to allow.
- Caregiver raises a crisis simultaneously: cost vs benefit — minor edge case, acceptable to allow.
- Crisis with no `roster`: not possible in production wiring.

### Remaining Risk
None.

### Status
**RESOLVED**

---

## BUG-10 — LibraryStudy XP Odd-Pair Crash

### Original Bug
`LibraryStudySystem.TickDay` advances `i` by 2 and reads `manual.skillXpGrants[i + 1]` without bounds check, crashing with `IndexOutOfRange` if a manual's grant list has an odd number of entries. Audit §8 BUG-10.

### Reproduction
Pre-fix test `LoadCatalog_OddLengthSkillGrantList_Throws`: a manual with `skillXpGrants = ["skill_engineering", "10", "orphan"]` and `studyHoursRequired = 1` is fed through `LoadCatalog` → pre-fix: catalog accepted silently; post-tick: throws.

### Root Cause
Pair-list convention (skill, xp) was not enforced anywhere. Load path silently accepted malformed data; tick path crashed.

### Selected Repair
Validate pair-count at `LoadCatalog`. Throw `InvalidDataException` for odd-length grants list. Single point of entry.

### Files Changed
- `Assets/Ashfall.Core/LibraryStudySystem.cs` — 4-line guard.

### Regression Test Added
`Ashfall.Core.Tests/LibraryStudySystemTests.cs::LoadCatalog_OddLengthSkillGrantList_Throws`

### Verification
```
dotnet test --filter "FullyQualifiedName~LibraryStudySystemTests" → 7/7 PASS
```

### Save Compatibility
Restriction only at load → existing saves (with even-length catalogs) load unchanged.

### Determinism
Deterministic guard (no RNG).

### Architecture Impact
Integrates with the existing data-integrity sweep — `InvalidDataException` is the canonical signature for malformed catalog inputs.

### Adversarial Post-Fix Review
- Empty grant list (0 entries): pass → no XP granted (correct, vacuously).
- Multi-pair list (4, 6, ...): pass → each pair consumed (correct).
- Pre-existing saves where a manual with odd grants somehow persists: rejected on next `LoadCatalog`. Author must fix the JSON before re-loading.

### Remaining Risk
None.

### Status
**RESOLVED**

---

## Overall Verification

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj  → 0 errors, 4 pre-existing warnings
dotnet test  Ashfall.Core.Tests (full)                     → 2435/2435 PASS (was 2428 prior ⇒ +7 tests)
dotnet test  Ashfall.Core.Tests (Batch 3 filter, post-fix) → 42/42 PASS    (was 35 prior ⇒ +7 tests)
```

## Architectural Impact

- No Core file references engine namespaces.
- No new RNG draws.
- No DTO schema changes → no save migration needed.
- No event duplication.
- All tests that previously passed still pass.
- New tests added: 7 (BUG-06: 1, BUG-07: 2, BUG-08: 1, BUG-09: 1, BUG-10: 2 — note BUG-10 has 1 new + 6 existing).

## Remaining Repository Health

This 5-bug batch does NOT address:
- BUG-01 (CRITICAL) — 8 orphan Batch 3 systems
- BUG-02 (HIGH) — Empty catalogs
- BUG-03/04 (HIGH) — Thermal integration / physics
- BUG-05/11 (MEDIUM) — design-ambiguous forward transitions
- BUG-12-15 (LOW) — cosmetic issues

A future `ashfall-repair` batch should attack BUG-01 + BUG-02 together (Phase 2 + Phase 4 wiring) — that batch will have a longer time scope and a separate plan document.

## Status

**All 5 bugs: RESOLVED.**
