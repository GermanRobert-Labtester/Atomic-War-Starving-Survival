# Phase 3 — Water Integration Foundation (B5–B8 / Plan 66) — increment 1 landed

> Phase 1's `WaterRequest` seam and the corrected authority map
> (`B5_B8_AUTHORITY_MAP.md`) mean most of Plan 66's brief premises were
> already live: treatment authority, sump→contamination bridge, brine system,
> greywater routing (`TryAddWaterFromSource` + `source_sump_greywater`),
> inventory-item water spendable authority. This increment closes the two
> real gaps the reconciliation identified, plus one silent-failure repair.

## Landed in this increment

### 1. Power-load registration contract (§6.3) — `PowerGridSystem.RegisterLoadRoom`
- Consumer systems expose a stable load id + nominal draw; the grid owns
  allocation/shedding thereafter. Idempotent; catalog rooms take precedence;
  registered loads join draw + priority allocation immediately; typed
  `PowerGridEventKind.LoadRoomRegistered` event.
- This is the shared foundation Phase 7 perimeter sentries will use
  (`perimeter_defenses.json` `power_draw_watts` currently has no grid path
  either — same pattern, later phase).

### 2. Sump pump power coupling (the reconciliation's flagged silent gap)
**Evidence:** `SumpFloodingSystem.TickDay` pump drainage and
`RunCentrifugeBatch` both gate on `_powerGrid.IsRoomPowered(node.nodeId)` —
but sump node ids were never grid rooms in live play (only test helpers
registered them), so `IsRoomPowered` was always false and **installed sump
pumps could never drain**. The panel's ENGAGE PUMP POWER toggled a flag that
could never produce water flow.

- `SumpFloodingSystem.SumpPumpDrawWatts = 90f` (documented, tunable);
  `InstallPump` registers the node as a critical-class dynamic load room
  (InfrastructureEssential; player priority overrides apply);
  `RestoreState` re-registers all pumped nodes (the grid room list is not
  persisted — deterministic, idempotent, no events replayed).
- Pump drainage + centrifuge migrated from `IsRoomPowered` (global-outage
  semantics) to `IsRoomServed` (Phase 2 allocation-aware query): during a
  brownout the critical sump load stays served while generation covers it —
  priority shedding, not a blackout (flagship §8.11: power owns availability,
  sump owns flood consequences).
- Save compatibility: `SumpFloodingState` unchanged (fixture byte-parity
  intact, no re-capture). Behavioral migration note: campaigns with installed
  pumps gain a 90 W load and working drainage — the intended Plan 66 outcome.

### 3. Decon canonical water cost repair (silent failure, §9.13)
**Evidence:** `DecontaminationSystem` consumed `"water_clean"` + `"soap"` —
neither id exists in `items.json`, any alias table, or any recipe. The bill
could never succeed: **every decon wash/chelation was permanently blocked in
live play**, and the system's own effluent recovery grant (the only
`water_clean` producer) was unreachable behind the always-blocked wash — a
dead bootstrap loop.

- Wash/chelation bill → canonical `clean_water` (the spendable potable
  inventory item — part of the spendable authority per the corrected map) +
  `item_liquid_bleach_carboy` (canonical sodium-hypochlorite disinfectant,
  produced by the live Plan 110 chlor-alkali plant, which had **zero
  consumers**). This closes a real producer→consumer chain and makes decon
  compete for real potable water exactly as §9.13 intends.
- Effluent recovery grants canonical `clean_water`.
- Decon test fixtures updated to the canonical ids (the old tests pinned an
  unreachable contract).

## Audit conclusions (no change needed — documented per reconciliation)

| Consumer | Finding | Disposition |
|---|---|---|
| Greenhouse irrigation | Host checks/consumes `clean_water`/`irradiated_water` from inventory before `System.Water(...)` | already authoritative (Phase 0 finding holds) |
| Decon water volume | Consumes inventory water items (canonical after repair); effluent volumes internal | inventory items are the spendable authority — no private counter |
| Kitchen | `KitchenNutritionSystem` has no water mechanic at all | defer — do not invent balance; the `WaterRequest` seam exists when needed |
| Unsafe water → disease/dose | Ration/needs paths route exposure through their own authorities; no direct stat writes from water pools | consistent with §9.12; sanitation's `foul_water_draw` disease bridge (Wave 6) is the pattern |
| Greywater → treatment | `TryAddWaterFromSource(SumpGreywaterSourceId, ...)` already live (Plan 189 intake bridge) | preserve; Plan 189 owns expansion |

## Deliberately deferred (remaining Phase 3+ scope)

| Item | Target phase | Note |
|---|---|---|
| Sump breaker/priority persistence for dynamic loads | documented limitation | grid restore validates against rooms known at that time; sump re-registers fresh each session (closed breaker, critical default) |
| Kitchen water demand | deferred | no existing mechanic; would be new balance scope |
| `WATER_FLOW_BASELINE.md` refresh | this phase's closeout | consumer matrix update to reflect the served-state migration |
| Perimeter sentry load registration | Phase 7 | same `RegisterLoadRoom` pattern |
| 30-day water balance harness | Phase 8/9 | flagship §9.18 scenario set |

## Verification record (2026-09-13)

| Command | Result |
|---|---|
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/Phase3WaterIntegrationTests.cs` | 10/10 PASS (new) |
| `bash scripts/run_test.sh Ashfall.Core.Tests/SumpFloodingSystemTests.cs` | 39/39 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/DeconAirlockSystemTests.cs` | 13/13 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/DeconAirlockSaveTests.cs` | 6/6 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/WaterTreatmentSumpBridgeTests.cs` | 4/4 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/PowerGridPhase2AllocationTests.cs` | 14/14 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/PowerGridPhase2GenerationPortfolioTests.cs` | 11/11 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/PowerGridSystemTests.cs` | 20/20 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/PowerGridDeterminismTests.cs` | 4/4 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Save/B5B8Phase0SaveFixtureTests.cs` | 6/6 PASS (no re-capture needed) |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Phase1SharedContractsTests.cs` | 13/13 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/WaterAndQuarantinePowerTests.cs` | 8/8 PASS |
| `godot --headless --path . -- --data-integrity-selftest` | PASS — 0 findings, 325 catalogs |
| `dotnet build Ashfall.csproj` | 0 warnings, 0 errors |

## Files changed

- `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs` (`RegisterLoadRoom`, event kind)
- `Assets/Ashfall.Core/SumpFloodingSystem.cs` (pump load registration, restore re-registration, `IsRoomServed` migration)
- `Assets/Ashfall.Core/DecontaminationSystem.cs` (canonical water/bleach ids ×4 sites)
- `Ashfall.Core.Tests/Shelter/DeconAirlockSystemTests.cs`, `DeconAirlockSaveTests.cs` (canonical id fixtures)
- `Ashfall.Core.Tests/Shelter/Phase3WaterIntegrationTests.cs` (new, 10 tests)
- `docs/plans/flagship_b5_b8/PHASE3_WATER_INTEGRATION.md` (this file)
