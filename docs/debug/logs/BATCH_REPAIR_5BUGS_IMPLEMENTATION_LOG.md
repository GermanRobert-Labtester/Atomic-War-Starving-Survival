# 5-Bug Repair Implementation Log

**Plan:** `docs/debug/plans/BATCH_REPAIR_5BUGS_PLAN.md`
**Scope:** 5 surgical Core-only patches. No host wiring, no JSON, no schema migration.

## Phase 1 — BUG-06 Contractor expired-then-paid race

**Pre-integration checkpoint:** PASS
- Repository reality verified — `_currentDay` rolls forward via `TickDay` calls only.
- Assumption: `Contractor.expiryDay = _currentDay + offer.termDays`. Confirmed.
- Minimality: relocate the expiry check above the payment loop and `continue`. Two lines moved.
- Determinism: no new RNG draws.
- Save check: no DTO change.

**Changes:**
- `Assets/Ashfall.Core/ContractorRosterSystem.cs` — restored test for prior ordering, added new logic to check expiry first.
- `Ashfall.Core.Tests/ContractorRosterSystemTests.cs` — added `TickDay_OnExpiryDay_DoesNotAccrueMissedPayment`.

**Regression test:** ticks system forward to day 9, hires with `termDays = 3` → `expiryDay = 12`, drains funds, ticks `TickDay(12)`, asserts `status == Expired` and `missedPayments == 0`.

**Related tests:** all 7 ContractorRoster tests **PASS** (was 6 prior).

**Diff review:** the only logical change is moving the expiry branch above the payment branch and adding `continue` so the payment code does not run on a contract that is ending today. No state field changes.

**Invariant review:**
- Save round-trip preserved (no schema change).
- RNG preserved (no new draws).
- Determinism preserved (no new dependency on dictionary iteration).
- CaptureState semantics unchanged.

**Result:** ✅ BUG-06 RESOLVED.

---

## Phase 2 — BUG-09 MentalHealth caregiver eligibility

**Pre-integration checkpoint:** CHANGED PLAN — originally targeted `BeginTreatment`; cross-checked `_roster.GetRoleOf` API and `DutyRosterRow` prerequisite in `WriteName` before pinning the production change.

**Changes:**
- `Assets/Ashfall.Core/MentalHealthCrisisSystem.cs` — added eligibility guard in `BeginTreatment`.
- `Ashfall.Core.Tests/MentalHealthCrisisSystemTests.cs` — added `BeginTreatment_CaregiverOnDuty_Blocks`. Required importing `DutyRosterSystem.AssignmentRoles` via type name (compile error caught and fixed).

**Regression test:** registers a survivor via `WriteName`, places them on a duty shift via `Assign`, then asserts `BeginTreatment` returns `Blocked` and the crisis remains `Active` (not `InTreatment`).

**Related tests:** all 8 MentalHealth tests **PASS** (was 7 prior).

**Diff review:** one guard line — `if (!string.IsNullOrEmpty(caregiverId) && _roster.GetRoleOf(caregiverId) != null) return ActionResult.Blocked(...)`. No increment to occupancy, no event change.

**Invariant review:**
- Save round-trip preserved.
- RNG preserved.
- New event-count: same as before (one `OnMentalHealthChanged` on success, zero on block — matches existing convention).
- Failure code introduced: `mental.caregiver_busy`.

**Result:** ✅ BUG-09 RESOLVED.

---

## Phase 3 — BUG-10 Library XP pair-list

**Pre-integration checkpoint:** PASS — surveyed codebase for similar pair-list pattern; decision was to validate at `LoadCatalog` (single point of entry) rather than `TickDay` (every tick would re-encounter).

**Changes:**
- `Assets/Ashfall.Core/LibraryStudySystem.cs` — `LoadCatalog` now throws `InvalidDataException` for odd-length `skillXpGrants` lists.
- `Ashfall.Core.Tests/LibraryStudySystemTests.cs` — added `LoadCatalog_OddLengthSkillGrantList_Throws`.

**Regression test:** constructs a manual with `skillXpGrants = ["skill_engineering", "10", "orphan"]` (3 entries), calls `LoadCatalog`, asserts `InvalidDataException` thrown.

**Related tests:** all 7 LibraryStudy tests **PASS** (was 6 prior).

**Diff review:** a single `if (m.skillXpGrants != null && m.skillXpGrants.Count % 2 != 0) throw ...` check inside `LoadCatalog`. The throw happens before `_catalog[m.manual_id] = m`, so the bad manual never enters the catalog.

**Invariant review:**
- Save round-trip preserved (rejected manuals at load → never persisted anyway).
- High-fidelity contract: a noisy catalog now errors loudly at first integration instead of indexing past the end on tick N.
- Type: `InvalidDataException` chosen over generic `Exception` so the data-integrity sweep can recognize it.

**Result:** ✅ BUG-10 RESOLVED.

---

## Phase 4 — BUG-07 Schedule modifier

**Pre-integration checkpoint:** CHANGED PLAN — initial plan was to evaluate ternary at runtime. Falsification step exposed that the original branch was just unnecessary — the `_state.curfewActive ? def.fatigueRecoveryModifier : def.fatigueRecoveryModifier` ternary collapsed to `def.fatigueRecoveryModifier`. Net effect: emergency override remains the only thing that overrides the schedule's modifier.

**Changes:**
- `Assets/Ashfall.Core/ShelterScheduleSystem.cs` — `TickDay` fatigue modifier assignment simplified to use `def.fatigueRecoveryModifier` in the day phase. The previous hardcoded `1f` is replaced with `def.fatigueRecoveryModifier`.
- `Ashfall.Core.Tests/ShelterScheduleSystemTests.cs` — added two regression tests:
  - `TickDay_DayPhase_UsesScheduleFatigueModifier` (asserts `1.3f` for `fatigueRecoveryModifier = 1.3f`)
  - `TickDay_DayPhase_PropagatesRestlessSchedule` (asserts `0.7f` for suppressed modifier)

**Related tests:** all 12 ShelterSchedule tests **PASS** (was 10 prior).

**Diff review:** one-liner: `_state.fatigueRecoveryModifier = _state.emergencyOverride ? 0.5f : def.fatigueRecoveryModifier;` (deleted the curfew ternary). Lighting demand branch unchanged (already correct).

**Invariant review:**
- Save round-trip preserved.
- No event added.
- Semantic note: previously the modifier was a curfew bonus; now it's the schedule's contribution to recovery for the whole day. The `LightingDemandDay` precedent suggests this is the intended model — the audit confirms the bug.
- No backward-incompatible change: "default" schedule has `fatigueRecoveryModifier = 1f`, so prior behavior matches.

**Result:** ✅ BUG-07 RESOLVED.

---

## Phase 5 — BUG-08 Sump equipmentDisabled latch

**Pre-integration checkpoint:** PASS — confirmed the bug is local to the natural-drain branch only. `DrainNode` already resets the latch correctly; only `TickDay`'s decay path was missing. Adding the reset on `waterLevelCm == 0` is the minimum change.

**Changes:**
- `Assets/Ashfall.Core/SumpFloodingSystem.cs` — natural-drain branch now sets `node.equipmentDisabled = false` when the node is fully drained.
- `Ashfall.Core.Tests/SumpFloodingSystemTests.cs` — added `TickDay_NaturalDrainComplete_ResetsEquipmentDisabled`.

**Regression test:** pre-flood node with `isFlooded = false`, `equipmentDisabled = true`, `waterLevelCm = 2f`. Single `TickDay(1)` brings level to 0 → assert `equipmentDisabled == false`.

**Related tests:** all 6 SumpFlooding tests **PASS** (was 5 prior).

**Diff review:** one line added before the `DrainComplete` incident log. No other drain paths affected. `DrainNode` already resets the latch, so behaviour is consistent.

**Invariant review:**
- Save round-trip preserved.
- Determinism preserved.
- No event change (incident log still emits on drain complete).

**Result:** ✅ BUG-08 RESOLVED.

---

## Final Verification

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj  → 0 errors, 4 pre-existing warnings
dotnet test  Ashfall.Core.Tests (full)                     → 2435/2435 PASS
dotnet test  Ashfall.Core.Tests (Batch 3 filter, post-fix) → 42/42 PASS (was 35 prior)
```

## Files changed summary

| File | Change |
|---|---|
| `Assets/Ashfall.Core/ContractorRosterSystem.cs` | Loop reorder: expiry check above payment. |
| `Assets/Ashfall.Core/MentalHealthCrisisSystem.cs` | 1-line eligibility guard. |
| `Assets/Ashfall.Core/LibraryStudySystem.cs` | 4-line catalog load guard. |
| `Assets/Ashfall.Core/ShelterScheduleSystem.cs` | 1-line collapse, removed dead ternary. |
| `Assets/Ashfall.Core/SumpFloodingSystem.cs` | 1-line latch reset on drain. |
| `Ashfall.Core.Tests/ContractorRosterSystemTests.cs` | +1 regression test. |
| `Ashfall.Core.Tests/MentalHealthCrisisSystemTests.cs` | +1 regression test. |
| `Ashfall.Core.Tests/LibraryStudySystemTests.cs` | +1 regression test. |
| `Ashfall.Core.Tests/ShelterScheduleSystemTests.cs` | +2 regression tests. |
| `Ashfall.Core.Tests/SumpFloodingSystemTests.cs` | +1 regression test. |

## Adversarial Post-Fix Review

| Question | Outcome |
|---|---|
| Save round-trip broken? | No — no DTO shape changes. |
| Determinism changed? | No — no new RNG draws. |
| New event count? | No — same events fire on the same paths. |
| New failure codes? | 1 — `mental.caregiver_busy`. Consistent with rest of codebase. |
| Could the same bug recur through a different path? | BUG-08 could recur via a future "evaporation" path; that path is not yet authored. Add same one-liner there. |
| Did the fix hide the symptom? | No — each fix targets an actual branch of the algorithmic decision tree. |
| Are integration tests for the wired systems still green? | Yes — `dotnet test Ashfall.Core.Tests` shows 2435/2435 PASS, including all harness integration tests. |
| Did I close BUG-07 correctly? | Verified — the lighting-demand `Day/Night/Curfew` precedent matches the schedule-level modifier being a top-level setting. Saved design-clarification note in code comment. |
