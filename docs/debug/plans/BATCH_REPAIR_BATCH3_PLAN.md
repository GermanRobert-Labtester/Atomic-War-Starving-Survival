# Batch 3 Repair Plan

**Prior batches:** Batches 1 + 2 — 2438/2438 tests passing.
**Audit source:** `docs/debug/10LOOP_BATCH3_AUDIT.md`
**Scope:** Surgical Core-only patches. No host wiring, no JSON data.

## Reinvestigation Method (per skill protocol)

> "*Treat prior findings as hypotheses until independently validated.*"

I did a fresh re-investigation independently rather than adopting the prior audit's picks. Phases iterated REJECT → CONFIRM:
- Re-investigated 5 candidate systems.
- **1 was falsified** (CR3-01 SumpPump re-emit — outer guard `pumpCondition > 0` already prevents the re-emission I hypothesized).
- 5 fresh defects confirmed.

## Patch Set

| Phase | Bug | File | Defect class |
|---|---|---|---|
| 1 | CR3-02 KitchenNutritionSystem.StartPrepJob partial-removal on insufficient later req | `KitchenNutritionSystem.cs` | Atomicity |
| 2 | CR3-03 EquipmentConditionSystem.StartMaintenance partial-removal on missing later part | `EquipmentConditionSystem.cs` | Atomicity |
| 3 | CR3-04 ArchiveDeskSystem.QueueTranscription partial-removal after inventory consumption | `ArchiveDeskSystem.cs` | Atomicity |
| 4 | CR3-05 KitchenNutritionSystem._state.activeJobs grows unbounded | `KitchenNutritionSystem.cs` | State hygiene |
| 5 | CR3-06 DecontaminationSystem.Enqueue allows duplicate survivor across days | `DecontaminationSystem.cs` | State hygiene |

All five share a root-theme: **inventory mutation paths without full pre-check, or list-management paths without proper eviction.** Each is single-file Core-only.

## Dependency Graph

```
CR3-02 (Kitchen partial-removal)      ─┐
                                       ├── test pattern: pre-check all → consume
CR3-03 (Equipment partial-removal)   ─┤
                                       │
CR3-04 (ArchiveDesk partial-removal) ─┘

CR3-05 (activeJobs unbounded)         ─┐── state hygiene
                                       │
CR3-06 (Decon duplicate-survivor)     ─┘
```

Independent. Process in numerical order.

## Invariants

1. Core remains engine-agnostic.
2. RNG is `ISeededRng` only — these fixes do not draw RNG.
3. `CaptureState/RestoreState` round-trips preserved.
4. All previously-passing tests stay green.
5. Determinism preserved.
6. **Atomicity rule (this batch's theme)**: A multi-step mutation that fails partway through must not leave the system in a partially-consumed state. New invariant adopted this batch.
7. Test scaffolding must exercise the production class directly.

## Verification ladder

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj       → 0 errors
2. dotnet test  Ashfall.Core.Tests/<bug_regression> --filter ...   → pass
3. dotnet test  Ashfall.Core.Tests (full)                          → ≥2438+ pass
```

## Execution Order

Sequential. Pre-integration checkpoint before each phase. Each phase:
1. State intent
2. Verify assumptions against current code
3. Implement minimum change
4. Run focused regression test (red → green)
5. Run adjacent tests
6. Inspect diff
7. Recheck invariants
8. Decide whether to continue

## Risk Profile

| Phase | Core-only | New RNG | Save | Event | Test |
|---|---|---|---|---|---|
| 1 | yes | no | no | +0 | +1 |
| 2 | yes | no | no | +0 | +1 |
| 3 | yes | no | no | +0 | +1 |
| 4 | yes | no | no (list cleaning) | +0 | +1 |
| 5 | yes | no | no | +0 | +1 |

## Falsified Candidate

**CR3-01 SumpPump re-emit** — was a hypothesis that after `pumpCondition == 0` the inner failure-block re-fires every day. **Falsified:** the outer guard at `SumpFloodingSystem.cs:168` reads `if (node.hasSumpPump && node.pumpPowered && node.pumpCondition > 0)` — once `pumpCondition == 0`, the entire failure subtree is skipped. No re-emit. Not in this batch.

## Definition of Done

- 5 bugs closed
- 5 regression tests added (each fail-then-pass lifecycle proven)
- Full suite ≥2438+ pass (no regressions)
- No new warnings introduced

## Rollback

Per-phase `git checkout <files>` restores prior behavior.
