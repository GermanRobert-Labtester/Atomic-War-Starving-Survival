# Batch 5 Resolution Report

**Plan.** `docs/debug/plans/BATCH_REPAIR_BATCH5_PLAN.md`
**Log.** `docs/debug/logs/BATCH_REPAIR_BATCH5_IMPLEMENTATION_LOG.md`
**Source audit.** `docs/debug/10LOOP_BATCH3_AUDIT.md`
**Prior batches.** Batch 1 + 2 + 3 + 4 — all RESOLVED.
**AGENTS.md state.** Untouched, per user's standing direction this session.

---

## Final verification

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   → 0 errors, 0 warnings
dotnet test  Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   → 2497 PASS / 0 FAILED  (+3 vs. Batch 4)
dotnet build Ashfall.csproj                                 → 0 errors, 0 warnings
godot --headless --path . -- --data-integrity-selftest      → PASS (3600 ids, 680 reuses, 0 errors)
godot --headless --path . -- --bridge-selftest              → exits 0
```

---

## Bug list closed

| Phase | Defect | Severity | File | Status |
|---|---|---|---|---|
| 1 | BUG-03 warmth propagation from warm rooms into `NeedsSystem.Warmth` for in-room survivors | HIGH (audit §7) | `Assets/Ashfall.Core/ShelterThermalSystem.cs` | RESOLVED |
| 2 | BUG-11 decon bypass symmetric contamination transfer (+0.1/+0.1 after BypassSurfaceDelta) | MEDIUM (audit §8) | `Assets/Ashfall.Core/DecontaminationSystem.cs` | RESOLVED |
| 3 | BUG-04 corrected-physics runtime (KwPerFuelUnit retune + analytic time relaxation, replacing Batch 4's explicit-Euler overshoot) | HIGH (audit §7 + Batch 4 own-resolution follow-up) | `Assets/Ashfall.Core/ShelterThermalSystem.cs` | RESOLVED |
| 4 | BUG-15 deferred from Batch 2: brownout lighting demand regression test | MEDIUM (audit §8, Batch 2 resolution "deferred to future batch") | `Ashfall.Core.Tests/ShelterScheduleSystemTests.cs` | RESOLVED |

**Corrected from Batch 4's next-batch section:**
- BUG-07 flagged "design gap, `fatigueRecoveryModifier` doesn't exist" — **falsified** this batch. Source `ShelterScheduleSystem.cs:14/23/184-185` + tests `ShelterScheduleSystemTests.cs:118-154` show it closed in Batch 2.
- UI/visual enhancements for Batch 1 + 2 — verified not outstanding ("Remaining Risk: None" on all phases in both resolution reports). Did NOT fabricate work; documented.

---

## Phase 1 — BUG-03 warmth propagation

### Original Bug (audit §7)
`ShelterThermalSystem` computed room temperature but never propagated it into survivor `Warmth`. `NeedsSystem` was already injected (unused), and `ShelterAssignmentSystem.GetAssignmentsForRoom(roomId)` provides the room→survivor map. "the host session reads this and applies to survivor warmth" comment was forward-looking fiction.

### Selected Repair

- `ShelterThermalSystem` gains an optional constructor parameter `ShelterAssignmentSystem? assignment = null!` (`Assets/Ashfall.Core/ShelterThermalSystem.cs`).
- `TickDay` gained, after the per-room solve, a `if (_assignments != null && _needs != null)` block iterating every room: for each `inRoom = _assignments.GetAssignmentsForRoom(room.roomId)`, `warmth = GetRoomWarmthModifier(room.roomId)`, apply `_needs.Modify(survivorId, NeedKind.Warmth, warmth * 24f)` when `warmth > 0f`. Positive `Warmth` delta = good (in-need semantics: warmth LOW = worse). 24f = gameHours per day.
- No host wiring needed: the Core seam is additive/optional; existing call sites stay compiling. `ShelterThermalHostSession` wiring to pass `_shelterAssignment` is the only remaining host follow-up.

### Files Changed
- `Assets/Ashfall.Core/ShelterThermalSystem.cs` (+1 optional injectable ref, ~22 lines warmth loop)
- `Ashfall.Core.Tests/ShelterThermalSystemTests.cs` (+1 regression)

### Regression Test Added
`Bug03_Warmth_Propagates_ToInRoomSurvivors`

### Verification:
```
dotnet test --filter "FullyQualifiedName~ShelterThermalSystemTests" → 13/13 PASS
```

### Save Compatibility
No DTO change. Warmth values themselves change at runtime as designed.

### Remaining Risk
Host wiring of `ShelterAssignmentSystem` into `ShelterThermalHostSession` to enable the propagation live; Core seam is in-place but the Godot host has not yet passed the reference (it currently constructs `ShelterThermalSystem` without `assignment`, so the Core block stays safely off). This is documented as follow-up.

### Status
**RESOLVED.**

---

## Phase 2 — BUG-11 decon bypass net-contamination

### Original Bug (audit §8)
`BypassSurfaceDelta = -0.1f`, `BypassShelterDelta = +0.1f` — pure symmetric transfer; the audit's "*bypass should at minimum NOT increase net shelter contamination*".

### Selected Repair
`BypassShelterDelta = 0f`. Bypassed surface dust is no longer poured into shelter air; surface contamination still drops `-0.1f`, shelter air stays put. The class-level docstring keeps the tunable/designer knob intact.

### Files Changed
- `Assets/Ashfall.Core/DecontaminationSystem.cs` (1 constant + comment)
- `Ashfall.Core.Tests/DecontaminationSystemTests.cs` (updated `CompleteCycle_Bypass_IncreasesShelterContamination` to assert the post-fix zero-net semantics, added `Bug11_Bypass_NetShelterContamination_IsNotIncreased`)

### Verification
```
dotnet test --filter "FullyQualifiedName~DecontaminationSystemTests" → 11/11 PASS
```

### Adversarial Post-Fix Review
- Existing test `CompleteCycle_Bypass_IncreasesShelterContamination` pinned the old symmetric transfer value. Updated to assert `shelterContaminationLevel == 0f` post-fix while still checking `shelterContaminated == true` (event semantics unchanged). Root-cause didn't move — constant is the control point.
- `SafeReleaseShelterDelta = -0.05f` unchanged — safe-release continues to **reduce** shelter contamination as intended.

### Status
**RESOLVED.**

---

## Phase 3 — BUG-04 corrected-physics runtime follow-up

### Original Issue (Batch 4 own-resolution follow-up, audit §7)
Batch 4 installed the audit-recommended physics but in explicit-Euler form (per-day ΔT = `gainC - lossC` treating `kW×86400s` with zero-uplift), producing instant clamp saturation. Batch 5 replaced it with the analytic exact solve of:
```
dT/dt = (G - k·(T - T_out))/C
T(t) = T_out + G/k + (T_0 - T_out - G/k)·exp(-k t / C)
```
Time constant `τ = ρ·cp / h` (NewtonCoolingCoefficient = 0.001 kW/(m³·K)) is ~20 minutes at ISA air — rooms relax to steady state `T_out + G/k` instead of jumping to clamp.

`KwPerFuelUnit` retuned: `10f` → `0.05f`. Full fuel (`100` × `0.05` = 5 kW sustained) yields ~50 °C steady-state above ambient in a 100 m³ room at insulation 1 — thawing a cold bunker across a day without instant saturation. Tuners documented; the formula untouched.

### Files Changed
- `Assets/Ashfall.Core/ShelterThermalSystem.cs`
- `Ashfall.Core.Tests/ShelterThermalSystemTests.cs` — two Bug-04 regression tests rewritten to assert analytic relaxation + steady state instead of the stale explicit-Euler closed form.

### Verification
```
dotnet test --filter "FullyQualifiedName~ShelterThermalSystemTests" → 13/13 PASS
Bug04_HeatGain_Physics_Matches_Audit_Formula → PASS (steady-state form)
Bug04_Adding_Room_Does_Not_Reduce_PerRoomHeat → PASS (per-room independence of roomCount)
```

### Save Compatibility
No DTO change. Fresh mathematical timestep; save/load unaffected.

### Remaining Risk
None Core-side. Host `ShelterThermalPanel` shows `KwPerFuelUnit`-dependent boiler numbers; that UI string is host-side and not touched this batch.

### Status
**RESOLVED.**

---

## Phase 4 — BUG-15 deferred from Batch 2 (brownout lighting demand)

### Original Deferral (Batch 2 resolution)
"*None on the production code; the deferred automated test should be authored in a future batch that addresses the `PowerGridSystem` brownout testability issue first.*" Batch 4 provided `PowerGridSystem.EffectiveTotalDrawWatts`. Batch 5 uses it in a direct regression test.

### Selected Repair
Test-only close: `TickDay_Brownout_DoublesLightingDemandHalving` — constructs a true brownout (`GenerationWatts=50`, `BatteryReserve=0`, `BatteryCapacity=0`, draw 150W > generation), ticks `ShelterScheduleSystem.TickDay(1)`, and asserts `lightingDemand ≈ 0.25f` (0.5 × 0.5) versus the 0.5 baseline.

### Files Changed
- `Ashfall.Core.Tests/ShelterScheduleSystemTests.cs` (+1 test)

### Verification
```
dotnet test --filter "FullyQualifiedName~ShelterScheduleSystemTests" → all PASS
```

### Status
**RESOLVED.**

---

## Falsified candidates (logged for honesty)

- **BUG-07 re-open** — falsified; already closed in Batch 2 with regression tests.
- **Batch 1/2 UI-visual gaps** — falsified; both resolution reports carry "Remaining Risk: None" on every phase; no UI work exists to do.

## Architectural Impact

- **No engine coupling.** `using Ashfall.Core.Shelter;` in `ShelterThermalSystem` imports a Core sibling.
- **No new RNG draws.**
- **No DTO schema changes.**
- **No new event channels.**
- **One optional Core API extension**: `ShelterThermalSystem` constructor now accepts an optional `ShelterAssignmentSystem?` for live warmth propagation.
- **One existing-test semantic update** (BUG-11 bypass expected value), both tests above updated to post-fix semantics.

## Files Changed

| File | Change |
|---|---|
| `Assets/Ashfall.Core/ShelterThermalSystem.cs` | Optional `_assignments` + warmth propagation + analytic T solver + `KwPerFuelUnit = 0.05f` |
| `Assets/Ashfall.Core/DecontaminationSystem.cs` | `BypassShelterDelta = 0f` + docstring |
| `Ashfall.Core.Tests/ShelterThermalSystemTests.cs` | Bug-03 + updated Bug-04 regressions |
| `Ashfall.Core.Tests/DecontaminationSystemTests.cs` | Bug-11 regressions + semantic update |
| `Ashfall.Core.Tests/ShelterScheduleSystemTests.cs` | Bug-15 deferred regression |

## Status

**4/4 phases RESOLVED. Batch 5 fully CLOSED.**

Remaining follow-up (host-side): pass `ShelterAssignmentSystem` into `ShelterThermalHostSession` so the BUG-03 warmth propagation runs live in the Godot host; separate pinned plan required.
