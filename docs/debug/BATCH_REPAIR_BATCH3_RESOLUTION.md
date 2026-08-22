# Batch 3 Resolution Report

**Plan:** `docs/debug/plans/BATCH_REPAIR_BATCH3_PLAN.md`
**Log:** `docs/debug/logs/BATCH_REPAIR_BATCH3_IMPLEMENTATION_LOG.md`
**Source audit:** `docs/debug/10LOOP_BATCH3_AUDIT.md`
**Prior batches:** Batch 1 + Batch 2 — both RESOLVED (their resolution reports are the canonical record of those batches).
**Commit baseline:** `2ce22451` (working tree on Batch 3 start).
**Verified Build:** `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` → **0 errors, 59 warnings (all pre-existing)**.
**Verified Tests:** `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` → **2443 PASS / 0 FAILED**.

---

## Bug List Closed (5 phases)

| Phase | Bug | Severity | File | Status |
|---|---|---|---|---|
| 5 | UnityJsonSerializer engine-coupling (pre-existing invariant break) | HIGH | relocation: `Assets/Ashfall.Core/IO/UnityJsonSerializer.cs` → `docs/contracts/Cross-host/UnityJsonSerializer.template.cs` | RESOLVED |
| 1 | CR3-06 Decon cross-day duplicate-survivor | MEDIUM | `DecontaminationSystem.cs` | RESOLVED |
| 2 | CR3-05 KitchenNutrition activeJobs unbounded | MEDIUM | `KitchenNutritionSystem.cs` | RESOLVED |
| 3 | CR3-02 KitchenNutrition.StartPrepJob atomicity | MEDIUM | `KitchenNutritionSystem.cs` | RESOLVED |
| 4 | CR3-03 EquipmentConditionSystem.StartMaintenance atomicity | MEDIUM | `EquipmentConditionSystem.cs` | RESOLVED |

**Falsified and excluded:**
- CR3-01 SumpPump pump-failure re-emit — falsified in Loop 1; outer guard `pumpCondition > 0` already prevents. Not in batch.

---

## Phase 5 — UnityJsonSerializer engine-coupling

### Original Bug
`Assets/Ashfall.Core/IO/UnityJsonSerializer.cs` (introduced at commit `2ce22451 feat(save): Unity IJsonSerializer adapter for cross-host wire-format contract`) referenced `using UnityEngine;` and `UnityEngine.JsonUtility`. Although the body is `#if ASHFALL_UNITY` guarded and never compiles in the Godot build, the file's presence in `Assets/Ashfall.Core/` violated AGENTS.md Invariant 1 ("*Core must be engine-agnostic — no `UnityEngine`, no `UnityEditor`, no `Godot`, no `GodotSharp`*"), causing `Core_HasZeroEngineCoupling` to fail.

### Reproduction
Pre-fix `CoreInvariantSourceTests::Core_HasZeroEngineCoupling` reports:
```
Ashfall.Core is engine-agnostic (Invariant 1) but references engine types:
Assets/Ashfall.Core/IO/UnityJsonSerializer.cs:29 :: using UnityEngine;
Assets/Ashfall.Core/IO/UnityJsonSerializer.cs:48 :: return JsonUtility.ToJson(value);
Assets/Ashfall.Core/IO/UnityJsonSerializer.cs:59 :: return JsonUtility.FromJson<T>(json);
```

### Root Cause
The file was authored as a reference template that lives in the Godot Core tree for review-and-copy purposes, but its presence confuses textual invariant scans. Its header comment correctly states "*Copy this file into Assets/_Game/Core/ in the Unity project tree*" — implying the file's intended location is the Unity project, not Godot Core.

### Selected Repair
Relocate the file from `Assets/Ashfall.Core/IO/` to `docs/contracts/Cross-host/UnityJsonSerializer.template.cs`. Preserves the reference implementation, makes the role explicit (template lives with the cross-host contract documentation), and clears the invariant.

### Files Changed
- File move (no content change).

### Verification
```
dotnet test ... CoreInvariantSourceTests.Core_HasZeroEngineCoupling → PASS
```

### Save Compatibility
N/A — file not loaded by any runtime path; the `#if` guard kept it inactive.

### Determinism
Preserved.

### Architecture Impact
Documentation move; the cross-host contract is unchanged.

### Plan Divergences
None — Phase 5 was added beyond the original 5-phase plan to clear the pre-existing invariant that surfaced once cached-binary drift was eliminated by the discipline from Batches 1–2.

### Adversarial Post-Fix Review
- File referenced by anything else? `grep -rn "UnityJsonSerializer" Assets/Ashfall.Core/ src/ Ashfall.Core.Tests/` returns zero non-self references.
- File called from anywhere at runtime? Zero `new UnityJsonSerializer()` call sites.
- The contract tests still pass: `SaveWireContractTests` uses `SystemTextJsonSerializer` (Godot side); the Unity adapter is comment-documented and now lives where the contract documentation is.

### Remaining Risk
None.

### Status
**RESOLVED.**

---

## Phase 1 — CR3-06 Decon cross-day duplicate-survivor

### Original Bug
`DecontaminationSystem.Enqueue` computes `caseId = $"decon_{_currentDay}_{survivorId}"`. The duplicate guard checks `caseId` only, so the same survivor can re-enqueue every day forever even with their prior case still on the queue or as the active case. Compare with `MentalHealthCrisisSystem.TriggerCrisis` which correctly checks `survivorId + status`.

### Reproduction
Pre-fix test `Enqueue_SurvivorAlreadyOnQueue_Blocks`:
```
TickDay(1)
Enqueue("survivor_1", "gear_a", 0.5f) → Success
TickDay(2)
Enqueue("survivor_1", "gear_a", 0.5f) → Success  ← BUG
```

### Root Cause
`caseId` is day-scoped, so day-scoped predicates are insufficient to enforce survivor-level uniqueness.

### Selected Repair
Add two new lock predicates covering both `_state.queue` and `_state.activeCase`. Both reject any survivor with an unresolved case (`status != Complete && != Bypassed && != Failed`). Defense-in-depth: keep the caseId predicate.

### Files Changed
- `Assets/Ashfall.Core/DecontaminationSystem.cs`
- `Ashfall.Core.Tests/DecontaminationSystemTests.cs`

### Regression Test Added
`Enqueue_SurvivorAlreadyOnQueue_Blocks`

### Verification
```
dotnet test --filter "FullyQualifiedName~DecontaminationSystemTests" → 10/10 PASS
```

### Save Compatibility
No DTO change.

### Determinism
No RNG.

### Architecture Impact
None.

### Adversarial Post-Fix Review
- Same-day same-survivor (caseId still resolves): blocked at the existing caseId predicate.
- Different-survivor same-day: still allowed.
- Cross-day same-survivor (now blocked): the active case completes/bypasses/fails → status flips to a terminal state → the new lock predicate no longer matches → survivor can re-enqueue. Correct.
- `activeCase` covered: no survivor can be both Queued-as-an-active and Queued-again.

### Remaining Risk
None.

### Status
**RESOLVED.**

---

## Phase 2 — CR3-05 KitchenNutrition activeJobs unbounded

### Original Bug
`KitchenNutritionSystem.TickDay` completed/cancelled jobs by flipping `isComplete`/`isCancelled`, but never removed them from `_state.activeJobs`. `GetActiveJobs` filters on read, so player-facing behavior looked fine, but the underlying list grew without bound across long campaigns AND serialised to every save.

### Reproduction
Pre-fix test `TickDay_JobCompletes_RemovesJobFromActiveList`:
```
StartPrepJob → State.activeJobs.Count == 1
TickDay(1) → state.activeJobs[0].isComplete == true
            State.activeJobs.Count == 1  ← BUG (should be 0 post-eviction)
```

### Root Cause
Missing `RemoveAll` after `TickDay` joined completed/cancelled jobs.

### Selected Repair
Append `_state.activeJobs.RemoveAll(j => j.isComplete || j.isCancelled)` at the end of `TickDay`, after `UpdateSpoilage()`. Pattern matches `ArchiveDeskSystem.cs:164` and `MentalHealthCrisisSystem.cs:175`.

### Files Changed
- `Assets/Ashfall.Core/KitchenNutritionSystem.cs`
- `Ashfall.Core.Tests/KitchenNutritionSystemTests.cs`

### Regression Tests Added
- `TickDay_JobCompletes_RemovesJobFromActiveList`
- `TickDay_CancelledJob_IsRemovedFromActiveList`

### Verification
```
dotnet test --filter "FullyQualifiedName~KitchenNutritionSystemTests" → 10/10 PASS (after pre-existing test update)
```

### Save Compatibility
No DTO shape change. Save size may SHRINK for long campaigns.

### Determinism
Preserved.

### Architecture Impact
None.

### Plan Divergences
None.

### Adversarial Post-Fix Review
- Pre-existing test `TickDay_CompletesJob` was inadvertently codifying the bug state — its `activeJobs[0].isComplete` and `activeJobs[0].portionsProduced` only existed because `activeJobs` was retaining completed jobs. **Updated the test**, not the production code, to assert via the post-eviction observable surface (`activeJobs` empty, `pantry[0]`, `totalMealsPrepared`).

### Remaining Risk
None.

### Status
**RESOLVED.**

---

## Phase 3 — CR3-02 KitchenNutrition.StartPrepJob atomicity

### Original Bug
`KitchenNutritionSystem.StartPrepJob` was a single-pass loop calling `_inventory.RemoveById(req.Key, req.Value)` BEFORE checking the next iteration's `CountById`. If a later requirement was insufficient, prior decrements were not rolled back.

### Reproduction
Pre-fix test `StartPrepJob_LaterIngredientInsufficient_DoesNotConsumeEarlierIngredient`:
```
CountById("meat") = 12, CountById("veg") = 0
StartPrepJob("stew", "cook_1", {meat:2, veg:1})
  → loop: meat counts OK → RemoveById("meat", 2)
  → loop: veg counts 0 < 1 → Blocked("insufficient_ingredients")
  → AFTER: CountById("meat") = 10  ← BUG (should be 12)
          State.activeJobs empty (correct, no job created)
```

### Root Cause
Single-pass loop violating the new Core atomicity rule (see §"Adopted invariant" below).

### Selected Repair
Two-pass restructure: validate every required count first, then consume if all satisfied. Preserves driver contract and behavior under all-success and the existing `missing-all` early-exit path.

### Files Changed
- `Assets/Ashfall.Core/KitchenNutritionSystem.cs`
- `Ashfall.Core.Tests/KitchenNutritionSystemTests.cs`

### Regression Test Added
`StartPrepJob_LaterIngredientInsufficient_DoesNotConsumeEarlierIngredient`

### Verification
```
dotnet test --filter "FullyQualifiedName~KitchenNutritionSystemTests" → 11/11 PASS
```

### Save Compatibility
No DTO change.

### Determinism
Preserved.

### Architecture Impact
None.

### Plan Divergences
None.

### Adversarial Post-Fix Review
- Empty `inputRequirements` case: now both loops are no-ops; preserves implicit behavior (no consumption, no early-exit).
- Single requirement: validate-loop passes; consume-loop removes one; job created as before.
- Multiple requirements, all satisfiable: validate-loop passes; consume-loop removes all; order of consumption is dictionary-iteration-order → unchanged from before for all-success paths.

### Remaining Risk
None.

### Status
**RESOLVED.**

---

## Phase 4 — CR3-03 EquipmentConditionSystem.StartMaintenance atomicity

### Original Bug
Identical shape to CR3-02 in `EquipmentConditionSystem.StartMaintenance`. Single-pass loop calling `_inventory.RemoveById(part, 1)` before checking the next iteration. Missing later part triggers a blocked return without refunding the earlier consumed part.

### Reproduction
Pre-fix test `StartMaintenance_LaterPartMissing_DoesNotConsumeEarlierPart`:
```
CountById("part_cleaner") = 6, CountById("part_grease") = 0
StartMaintenance("tool_1", "station_1", MaintenanceType.Clean, ["part_cleaner", "part_grease"])
  → loop: part_cleaner counts OK → RemoveById("part_cleaner", 1)
  → loop: part_grease counts 0 → Blocked("missing_part")
  → AFTER: CountById("part_cleaner") = 5  ← BUG (should be 6)
```

### Root Cause
Same as Phase 3.

### Selected Repair
Same two-pass pattern as Phase 3, applied to `StartMaintenance`.

### Files Changed
- `Assets/Ashfall.Core/EquipmentConditionSystem.cs`
- `Ashfall.Core.Tests/EquipmentConditionSystemTests.cs`

### Regression Test Added
`StartMaintenance_LaterPartMissing_DoesNotConsumeEarlierPart`

### Verification
```
dotnet test --filter "FullyQualifiedName~EquipmentConditionSystemTests" → 8/8 PASS
```

### Save Compatibility
No DTO change.

### Determinism
Preserved.

### Architecture Impact
None.

### Plan Divergences
None.

### Adversarial Post-Fix Review
- Empty `requiredParts` case: empty list, both loops are no-ops.
- Single part: validate-loop passes; consume-loop removes; job created.
- Multiple parts all available: both loops succeed; consumption order is list-iteration-order → unchanged from before for all-success paths.

### Remaining Risk
None.

### Status
**RESOLVED.**

---

## Adopted Invariant (new this batch)

> **Core atomicity rule:** A multi-step mutation that fails partway through must not leave the system in a partially-consumed state. Multi-resource reservations must use the two-pass "validate-everything-first, then consume-all-or-nothing" pattern.

Two systems (Kitchen + Equipment) now obey this rule; the pattern in `DecontaminationSystem.ProcessQueue` (single CheckBoth → loop atomically) already follows it; this invariant is now Core-wide.

---

## Final Verification

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj  → 0 errors, 59 warnings (all pre-existing)
dotnet test  Ashfall.Core.Tests/Ashfall.Core.Tests.csproj  → 2443 PASS / 0 FAILED
```

## Architectural Impact

- **No Core file references engine namespaces** post-Phase 5 move.
- **No new RNG draws.**
- **No DTO schema changes** → no save migration needed.
- **No new event channels.**
- **New invariant:** atomicity rule (see above).

## Files Changed

| File | Change |
|---|---|
| `Assets/Ashfall.Core/DecontaminationSystem.cs` | +13 lines |
| `Assets/Ashfall.Core/KitchenNutritionSystem.cs` | +29 / -13 lines |
| `Assets/Ashfall.Core/EquipmentConditionSystem.cs` | +13 / -13 lines |
| `Ashfall.Core.Tests/DecontaminationSystemTests.cs` | +18 lines |
| `Ashfall.Core.Tests/KitchenNutritionSystemTests.cs` | +32 / -2 lines (one pre-existing test update) |
| `Ashfall.Core.Tests/EquipmentConditionSystemTests.cs` | +21 lines |
| `Assets/Ashfall.Core/IO/UnityJsonSerializer.cs` → `docs/contracts/Cross-host/UnityJsonSerializer.template.cs` | file relocation |

## Status

**5/5 phases RESOLVED. Batch 3 fully CLOSED.**

Next-batch candidates (beyond Core-only surgical scope per the original audit):

1. **BUG-01** (CRITICAL): 8 orphan Batch 3 systems need `ashfall-plan`, not `ashfall-repair` — requires host Session + SaveStore + UI panel + Main wiring + JSON catalog content for each orphan.
2. **BUG-02** (HIGH): 4 wired systems' empty catalogs.
3. **BUG-03 / BUG-04** (HIGH): thermal design decisions needed before any fix.
4. **BUG-11** (MEDIUM): Decon net-contamination — design-intent confirmation needed.
5. **PowerGridSystem.ComputerTotalDraw**) brownout testability (would unblock BUG-15's deferred test from Batch 2).
6. **BUG C6** (CRITICAL): 28 catalog loaders using `JsonUtility` — Unity adapter architectural work, distinct from the Phase 5 reference-template move.

These are all architectural / host-side / design decisions, not Core-only surgical fixes. Each warrants its own dedicated plan document and its own session.
