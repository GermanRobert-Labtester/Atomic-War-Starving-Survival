# Batch 5 Repair Plan (warmth + decon + tuning + BUG-15 deferred)

**Falsification of prior batch's "design-blocked" verdicts.** Before writing this plan I re-read `Assets/Ashfall.Core/ShelterScheduleSystem.cs`, `Assets/Ashfall.Core/ShelterThermalSystem.cs`, and the audit-specified `ShelterAssignmentSystem` API. Three of my Batch 4 verdicts were wrong and are corrected below. The work this batch proceeds on is the **actual open Core** following re-validation, not the snapshot from `BATCH_REPAIR_BATCH4_RESOLUTION.md`.

> **Batch 4 self-correction.** BUG-07 was labeled "design gap, `fatigueRecoveryModifier` doesn't exist" in Batch 4's *next-batch* section. **Wrong.** Source: `ShelterScheduleSystem.cs:14, 23, 184-185` already declare / read the modifier across phases; tests `ShelterScheduleSystemTests.cs:118-154` pin both 1.3 (boost) and 0.7 (suppressed) modifiers. Closed in Batch 2 — this batch does **not** re-apply it.

**Prior batches.** Batch 1 + 2 + 3 + 4 — all RESOLVED. (`docs/debug/BATCH_REPAIR_{5BUGS,BATCH2,BATCH3,BATCH4}_RESOLUTION.md`.)

**Source audit.** `docs/debug/10LOOP_BATCH3_AUDIT.md` (BUG-03 §7 HIGH, BUG-11 §8 MEDIUM, BUG-15 §8 MEDIUM, BUG-04 §7 HIGH).

**Branch baseline.** `b2b04212 feat(host): wire ShelterAssignment orphan + checksum sweep tests`.

**Scope.** Core-only surgical + one Core-side tuning constant + one deferred regression test from Batch 2.

**Stitch.** Force-loaded via `STITCH_API_KEY` from `/home/robertsrff/Desktop/design.env` per user instruction. `stitch-mcp doctor` reports `Healthy (200)`. Used **only** read-only tools (`get_screen_code`, `get_screen_image`, `list_tools`, `get_project`) against existing user projects — no `create_project` calls against the `lightgames77@gmail.com` workspace this session.

## UI/visual gap re-check

User-asked: "*if Batch 2 and 1 needed UI or visual assets then we need to make those*". Re-validated:

| Prior report | Remaining risk stated for each phase |
|---|---|
| `docs/debug/BATCH_REPAIR_5BUGS_RESOLUTION.md` | `### Remaining Risk: None` (every phase) |
| `docs/debug/BATCH_REPAIR_BATCH2_RESOLUTION.md` | Only deferred item was BUG-15 brownout testability (Core test, not UI); closed in Batch 4 by `PowerGridSystem.EffectiveTotalDrawWatts` |

**Conclusion: no UI/visual gaps for Batch 1 or Batch 2.** I am explicitly **not** fabricating work to fill non-gaps — that is the silent-regression failure AGENTS.md warns against. This batch coordinates with the user that "Batch 1 + Batch 2 had no UI/visual debt outstanding at close-time."

## Patch set

| Phase | Defect | File(s) | Class |
|---|---|---|---|
| 1 | BUG-03: `ShelterThermalSystem.TickDay` injects `NeedsSystem` and adopts `GetRoomWarmthModifier` but never iterates in-room survivors. `ShelterAssignmentSystem.GetAssignmentsForRoom(roomId)` is the existing canonical read API for room→survivor mapping. Core wires the path now; host upgrades in a future batch if needed. | `Assets/Ashfall.Core/ShelterThermalSystem.cs` | Core wire-up |
| 2 | BUG-11: `DecontaminationSystem.CompleteCycle(bypass)` symmetric transfer (`+0.1f` surface, `+0.1f` shelter) ⇒ "*bypass at minimum not increase net shelter contamination*". User-confirmed interpretation: cancel transfer. New constant `BypassShelterDelta = -BypassSurfaceDelta` (`-0.1f`) so the sum is zero. | `Assets/Ashfall.Core/DecontaminationSystem.cs` | Core constant semantics |
| 3 | BUG-04 host-tuning: corrected physics + 100 fuel × 10 kW/fuel ≈ 1,000 kW saturates any room in seconds. Set `KwPerFuelUnit = 0.01f` so 100 fuel × 0.01 kW/fuel = 1 kW sustained; ≈ 8 °C/day in default 80 m³ room. (Boiler fuel consumption already drops `boilerFuelLevel - 0.5f` per tick, so the unit scales naturally for longer games.) | `Assets/Ashfall.Core/ShelterThermalSystem.cs` | Core tuning constant |
| 4 | BUG-15 deferred (from Batch 2): regression test that pins `ShelterScheduleSystem.TickDay` doubling-down effect under `PowerGridSystem` brownout. Builders: a small `PowerGridSystem` configured under-sourced (`GenerationWatts < TotalDrawWatts`, `BatteryReserveWh = 0`). | `Ashfall.Core.Tests/ShelterScheduleSystemTests.cs` | Test gap close |

## Defects held (out of scope this batch)

- **BUG-15 visible UI/visual surfacing:** ticks the test gap, not the UI. A host-side wiring pass to expose `IsBrownout` to the player via the `ShelterSchedulePanel` is documented as a follow-up.
- **BUG-03 host wiring of `IEnumerable<string>` survivor residency:** the host already has `ShelterAssignmentHostSession`; advancing that to pass an `OccupancyResolver` to `ShelterThermalSystem` is a separate batch — Core API is set in this batch (Phase 1's ctor parameter).

## Invariants

1. Core remains engine-agnostic.
2. **No `Environment.TickCount*`, `DateTime.Now`, `System.Random`.** (Reaffirmed Batch 4 Phase 2.)
3. RNG is `ISeededRng` only (these fixes do not draw RNG).
4. `CaptureState/RestoreState` round-trips preserved.
5. **No regression to Batch 4's three closed items** — re-verified `Bug-04_HeatGain_Physics_Matches_Audit_Formula`, `Bug04_Adding_Room_Does_Not_Reduce_PerRoomHeat`, `Determinism_FirstEventId_Not_Tainted_ByEnvironmentTick`, `SharedRoster_BothSystems_BlockSurvivorOnDuty` still green.
6. **No `AGENTS.md` edits** per user standing direction this session.

## Verification ladder

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj    → 0 errors
2. dotnet build Ashfall.csproj                                  → 0 errors
3. dotnet test  Ashfall.Core.Tests/Ashfall.Core.Tests.csproj    → ≥2494 + new tests, 0 failed
4. godot --headless --path . -- --data-integrity-selftest       → 0 errors
5. godot --headless --path . -- --bridge-selftest               → exits 0
```

(Step 2 + step 3 are run after each phase.)

## Execution order

Sequential. Pre-integration checkpoint before each phase. Each phase ends with steps 1 + 3 at minimum; full ladder after all 4 phases land.

## Risk profile

| Phase | Core engine-coupling | New RNG | DTO shape | Save/load | New test |
|---|---|---|---|---|---|
| 1 | yes (no change) | no | no | no | +1 (`Bug03_Warmth_PropagatesToInRoomSurvivors`) |
| 2 | yes (no change) | no | no | no | +1 (`Bug11_Bypass_NetShelterContamination_IsZero`) |
| 3 | yes (no change) | no | no | no | existing `Bug04_HeatGain_Physics_Matches_Audit_Formula` must still pass after `KwPerFuelUnit` retune |
| 4 | n/a (test) | n/a | n/a | n/a | +1 (`TickDay_Brownout_DoublesLightingDemandHalving`) |

## Definition of done

- 4 phases closed.
- 3 new regression tests added (each fail-then-pass proven in the log).
- Existing 2494 + 3 = 2497 tests passing minimum, all green.
- Full ladder per AGENTS.md green.
- Batch 5 resolution report at `docs/debug/BATCH_REPAIR_BATCH5_RESOLUTION.md`.
- Implementation log at `docs/debug/logs/BATCH_REPAIR_BATCH5_IMPLEMENTATION_LOG.md`.
- AGENTS.md untouched.

## Rollback

Pre-phase `git checkout -- <files>` restores prior behavior. The four changes are independently revertible.
