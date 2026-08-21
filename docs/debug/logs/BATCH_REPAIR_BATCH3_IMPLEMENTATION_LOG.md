# Batch 3 Repair Implementation Log

**Plan:** `docs/debug/plans/BATCH_REPAIR_BATCH3_PLAN.md`
**Source audit:** `docs/debug/10LOOP_BATCH3_AUDIT.md`
**Prior batches:** Batch 1 (`docs/debug/BATCH_REPAIR_5BUGS_RESOLUTION.md`) + Batch 2 (`docs/debug/BATCH_REPAIR_BATCH2_RESOLUTION.md`)
**Scope:** 4 surgical Core-only patches (CR3-06, CR3-05, CR3-02, CR3-03) plus Phase 5 pre-existing invariant close (UnityJsonSerializer move).

---

## Phase 5 — Pre-existing engine-coupling invariant closed

**Pre-integration checkpoint:** PASS — `Assets/Ashfall.Core/IO/UnityJsonSerializer.cs` was authored at commit `2ce22451 feat(save): Unity IJsonSerializer adapter for cross-host wire-format contract` with header comment *"Copy this file into Assets/_Game/Core/ in the Unity project tree"*. The body is `#if ASHFALL_UNITY`-guarded (never compiles in Godot). However, the textual invariant scan in `CoreInvariantSourceTests::Core_HasZeroEngineCoupling` does not understand `#if`, so it flagged the file as a Core engine-coupling violation. The minimum safe fix is to move the file to `docs/contracts/Cross-host/UnityJsonSerializer.template.cs` — preserves the cross-host reference implementation, fully disambiguates its role, and resolves AGENTS.md Invariant 1.

**Changes:**
- `Assets/Ashfall.Core/IO/UnityJsonSerializer.cs` → `docs/contracts/Cross-host/UnityJsonSerializer.template.cs` (file move via `git mv`).

**Regression test:** `CoreInvariantSourceTests::Core_HasZeroEngineCoupling` now passes. Behavior at runtime is unchanged — the file was already inaccessible due to the `#if` guard.

**Verification:**
```
dotnet build ... → 0 errors
dotnet test ... → Core_HasZeroEngineCoupling PASS
```

**Diff review:** One file relocated without content changes. The move is reversible via `git mv` reverse.

**Invariant review:**
- Save round-trip preserved (the template is not loaded by any runtime path).
- Determinism preserved.
- Documentation moves from "misplaced in Core" to "correctly placed in contract docs".

**Result:** ✅ RESOLVED. Cleared pre-existing invariant break that was masked by previous session "2438 PASS" cached-binary drift misreading.

---

## Phase 1 — CR3-06 Decon cross-day duplicate-survivor

**Pre-integration checkpoint:** PASS — `caseId = $"decon_{_currentDay}_{survivorId}"` is day-scoped, so the existing caseId predicate lets a survivor re-enqueue every new day forever even with an unresolved case on the queue or as the active case. Lock by `(survivorId + not-yet-resolved)`, matching `MentalHealthCrisisSystem.TriggerCrisis` which checks by `survivorId + status`. Defense-in-depth: keep the caseId predicate.

**Changes:**
- `Assets/Ashfall.Core/DecontaminationSystem.cs` — added two new lock predicates covering both `_state.queue` and `_state.activeCase`.
- `Ashfall.Core.Tests/DecontaminationSystemTests.cs` — added `Enqueue_SurvivorAlreadyOnQueue_Blocks`.

**Regression test:** enqueues survivor on day 1, ticks to day 2, asserts second `Enqueue` is `Blocked` with `FailureCode = "survivor_busy"`.

**Verification:**
```
dotnet test --filter "FullyQualifiedName~DecontaminationSystemTests" → 10/10 PASS
```

**Diff review:** +13 production lines; +18 test lines. No event, no DTO change, no save impact.

**Invariant review:**
- Save round-trip preserved (DTO shape unchanged).
- RNG preserved (none).
- Failure code `survivor_busy` mirrors the existing vocabulary.

**Result:** ✅ RESOLVED.

---

## Phase 2 — CR3-05 KitchenNutrition activeJobs unbounded

**Pre-integration checkpoint:** PASS — `KitchenNutritionSystem.TickDay` invoked `_state.activeJobs.Add(...)` but never evicted completed/cancelled jobs. `GetActiveJobs` filters on read (functional reachability OK), but the underlying list serialised to every save and grew without bound across long campaigns. Pattern is established in `ArchiveDeskSystem.cs:164` and `MentalHealthCrisisSystem.cs:175` (both call `RemoveAll` after the tick loop).

**Changes:**
- `Assets/Ashfall.Core/KitchenNutritionSystem.cs` — added `_state.activeJobs.RemoveAll(j => j.isComplete || j.isCancelled)` at end of `TickDay`, after the `UpdateSpoilage()` call.
- `Ashfall.Core.Tests/KitchenNutritionSystemTests.cs` — added two regression tests AND updated `TickDay_CompletesJob` (a pre-existing test whose assertions inadvertently codified the bug state — its `activeJobs[0].isComplete` and `activeJobs[0].portionsProduced` only existed because `activeJobs` was retaining completed jobs).

**Regression tests:**
- `TickDay_JobCompletes_RemovesJobFromActiveList` — job completes in one tick; `activeJobs` empty post.
- `TickDay_CancelledJob_IsRemovedFromActiveList` — cancellation also evicts.

**Verification:**
```
dotnet test --filter "FullyQualifiedName~KitchenNutritionSystemTests" → 10/10 PASS
```

**Diff review:** +6 production lines including comment block. Updated the pre-existing test's assertions to point at the post-eviction observable surface (`pantry`, `totalMealsPrepared`) — preserving the test's intent (verify job completion) without depending on the bug's structural leftover.

**Invariant review:**
- Save round-trip preserved (shape unchanged; saved file may SHRINK for long campaigns — player-visible improvement).
- Determinism preserved.
- No new events.

**Result:** ✅ RESOLVED.

---

## Phase 3 — CR3-02 KitchenNutrition.StartPrepJob atomicity

**Pre-integration checkpoint:** PASS — `StartPrepJob` was a single-pass loop calling `_inventory.RemoveById(req.Key, req.Value)` BEFORE checking the next iteration's `CountById`. If a later ingredient was insufficient, prior decrements were not rolled back. The fix splits into two passes: (1) pre-check all counts; (2) consume only when all satisfied.

**Changes:**
- `Assets/Ashfall.Core/KitchenNutritionSystem.cs` — restructured `StartPrepJob` into validate-loop + consume-loop + job-creation.
- `Ashfall.Core.Tests/KitchenNutritionSystemTests.cs` — added `StartPrepJob_LaterIngredientInsufficient_DoesNotConsumeEarlierIngredient`.

**Regression test:** enqueues `meat` only (5 units), attempts `{meat:2, veg:1}` — `veg` absent. Asserts: result `Blocked`, code `insufficient_ingredients`, `meat` inventory unchanged (5), no job created.

**Verification:**
```
dotnet test --filter "FullyQualifiedName~KitchenNutritionSystemTests" → 11/11 PASS
```

**Diff review:** +11/-10 production lines (loop restructure, no public API change). The empty `inputRequirements` case is now a no-op rather than entering the loop with no iterations; preserved implicit behavior.

**Invariant review:**
- Save round-trip preserved.
- Determinism preserved.
- No new events.
- Validate-before-mutate invariant adopted as Core-wide atomicity rule.

**Result:** ✅ RESOLVED.

---

## Phase 4 — CR3-03 EquipmentConditionSystem.StartMaintenance atomicity

**Pre-integration checkpoint:** PASS — identical shape to CR3-02 in `EquipmentConditionSystem.StartMaintenance`. Single-pass loop, missing later part triggers blocked return without refunding the earlier consumed part. Apply the same two-pass pattern.

**Changes:**
- `Assets/Ashfall.Core/EquipmentConditionSystem.cs` — restructured `StartMaintenance` into validate-loop + consume-loop + job-creation.
- `Ashfall.Core.Tests/EquipmentConditionSystemTests.cs` — added `StartMaintenance_LaterPartMissing_DoesNotConsumeEarlierPart`.

**Regression test:** adds `part_cleaner` (5 units), attempts `{part_cleaner, part_grease}` — `part_grease` absent. Asserts: result `Blocked`, code `missing_part`, `part_cleaner` unchanged (5), no `pendingJobs` entry.

**Verification:**
```
dotnet test --filter "FullyQualifiedName~EquipmentConditionSystemTests" → 8/8 PASS
```

**Diff review:** +13/-13 production lines (parallels Phase 3). No public API change.

**Invariant review:** Same as Phase 3.

**Result:** ✅ RESOLVED.

---

## Cross-cutting invariant

Adopted as a Core-wide invariant this batch (also applies retroactively Batch 2):

> **Atomicity rule:** A multi-step mutation that fails partway through must not leave the system in a partially-consumed state. Implement two passes — pre-check every required resource first, then consume all-or-nothing.

Two systems now obey this rule (Kitchen + Equipment). The pattern in `DecontaminationSystem.ProcessQueue` (single CheckBoth → loop atomically) also obeyed it pre-batch; `_airlock.VisitorArrives` invocation is single call and the family is consistent.

## Final verification

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   → 0 errors, 59 warnings (all pre-existing)
dotnet test  Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   → 2443 PASS / 0 FAILED
```

## Files changed

| File | Change |
|---|---|
| `Assets/Ashfall.Core/DecontaminationSystem.cs` | +13 lines — survivor-level queue lock |
| `Assets/Ashfall.Core/KitchenNutritionSystem.cs` | +29/-13 lines — eviction + atomicity |
| `Assets/Ashfall.Core/EquipmentConditionSystem.cs` | +13/-13 lines — atomicity |
| `Ashfall.Core.Tests/DecontaminationSystemTests.cs` | +18 lines — Phase 1 regression test |
| `Ashfall.Core.Tests/KitchenNutritionSystemTests.cs` | +32/-2 lines — Phase 2 + Phase 3 regression tests and 1 pre-existing test update |
| `Ashfall.Core.Tests/EquipmentConditionSystemTests.cs` | +21 lines — Phase 4 regression test |
| `Assets/Ashfall.Core/IO/UnityJsonSerializer.cs` → `docs/contracts/Cross-host/UnityJsonSerializer.template.cs` | file relocation (Phase 5) |

## Adversarial post-fix review

| Question | Outcome |
|---|---|
| Save round-trip broken? | No — DTO shapes unchanged. Phase 2 may *shrink* saved file size for long campaigns. |
| Determinism changed? | No — no new RNG draws. |
| New event count? | No — same events fire on the same paths. |
| New failure codes? | 1 — `survivor_busy`. Consistent with the existing vocabulary. |
| Could the same bug recur through a different path? | The atomicity invariant is structural; the only recurrence path is a future system author bypassing it. The plan documents the rule. |
| Did I hide a symptom? | No — every fix targets the algorithmic decision point. |
| Are pre-existing tests still green? | Yes — Phase 2 required updating one pre-existing test (`TickDay_CompletesJob`) because its assertions inadvertently verified the bug state. New assertions point at the post-eviction observable surface (`pantry`, `totalMealsPrepared`). Test's intent preserved. |

## Falsified candidate (logged for honesty)

**CR3-01 SumpPump pump-failure incident re-emit** — hypothesis that after `pumpCondition == 0`, the inner failure-subtree would re-fire each day. **Falsified** during Loop 1 of the audit: the outer guard at `SumpFloodingSystem.cs:168` reads `if (node.hasSumpPump && node.pumpPowered && node.pumpCondition > 0)` — once `pumpCondition == 0`, the entire failure subtree is skipped. Not in this batch.

## Plan divergences

| Phase | Divergence | Why |
|---|---|---|
| 2 | Updated pre-existing `TickDay_CompletesJob` test | Assertions inadvertently codified the bug state. New assertions pin observable outcomes (`pantry`, `totalMealsPrepared`) which were already in the test's intent. |
| 5 | Moved `UnityJsonSerializer.cs` out of Core | Pre-existing AGENTS.md Invariant 1 violation surfacing as a real test failure; resolves the audit-flagged 28-catalog-JsonUtility decay by documentation relocation (the runtime adapter path is a separate concern tracked under original-audit BUG C6). |

## Status

5 phases planned. 5 phases resolved. **Batch 3 fully CLOSED.**
