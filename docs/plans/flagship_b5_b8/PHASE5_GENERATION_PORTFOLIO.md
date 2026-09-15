# Phase 5 — Generation Portfolio Completion (B5–B8 / Plan 65) — increment 1 landed

> Phases 2–4 landed solar (grid-tie + mirror maintenance), battery banks
> (bounded build chain), and kept the nuclear/geothermal contribution
> contract. The last "free forever power" hole was the base generator: a
> fuel loop with no condition, no wear, and no maintenance. This increment
> closes it with the project's canonical maintenance pattern.

## Landed in this increment

### Generator condition/maintenance (§8.7, §8.8)
- **Condition** — `PowerGridState.GeneratorCondition` (0..100), bounded
  component state; all five §8.8 requirements:
  - *wear producer*: `GeneratorWearPerDay` (0.25) applies **only on
    fuel-burning days** — an idle generator does not degrade (pinned);
  - *bounded effect*: output degrades below the 50 threshold, linearly from
    ×1.0 down to the ×0.5 floor (`GeneratorMinOutputFactor`) — a half-dead
    engine still runs at half rating, never zero while fueled; degradation
    can push the grid into brownout/shedding at the same fuel state (pinned
    end-to-end through the Phase 2 allocation summary);
  - *maintenance action*: `PerformGeneratorMaintenance` restores 100;
    servicing a healthy generator is a blocked action, not a silent success;
  - *real item cost*: canonical `machine_oil` (the same consumable the
    subgrid repair and sky-defense battery service use; produced by the
    Fischer-Tropsch lubricant chain — producer→consumer closed). Host
    consumes it; the grid never touches inventory (coated-part discipline).
  - *UI visibility*: `PowerGridPanel` gen label shows
    `GEN n W (CONDITION n%)`, color-shifts below the threshold, with a
    SERVICE GENERATOR button → Main handler (inventory check → consume →
    commit → sigil; blocked reasons truthful).
- **Typed event**: `PowerGridEventKind.GeneratorMaintained`.
- **Scope discipline**: the condition factor scales **only the base
  generator's rated watts**; external contributions (nuclear, geothermal,
  solar, coated parts) are untouched — each owning system owns its own
  condition (pinned).

### Save migration contract
- Field initializer `100f` is the migration rule: legacy saves lacking the
  field restore a **healthy** generator — no retroactive degradation, no
  energy-state change (flagship §8.14 "old saves must not begin with
  different battery energy" analog). Pinned against a pre-Phase-5 payload.
- `Capture`/`RestoreInto` copy + clamp (0..100) exactly like the battery
  fields; Phase 0 fixture re-captured (additive `"GeneratorCondition": 100`).

## Deliberately deferred (with reasons)

| Item | Target | Reason |
|---|---|---|
| Charge/discharge efficiency < 100% | battery-chain rework | the legacy daily aggregate battery math (pinned by legacy tests + Phase 0 fixture) has no efficiency seam; introducing one changes every restored campaign's energy parity. Current model never *creates* energy (charge = surplus only, discharge = deficit only), which satisfies the conservation intent; real conversion loss needs the hourly model |
| Solar/battery/wind balance harness (§8.15) | Phase 8 | 30-day scenario set with all four systems active |
| Geothermal build chain | follow-on | `GeothermalOrcSystem` owns generation + its research chain; no observed gap in its contribution path |
| Battery maintenance fluid consumer | content stream | `item_battery_maintenance_fluid` is catalog-only; banks deliberately do not degrade (storage, not generation) — a fluid consumer needs a battery wear model first |

## Verification record (2026-09-13)

| Command | Result |
|---|---|
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/PowerGridPhase5MaintenanceTests.cs` | 9/9 PASS (new) |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/PowerGridPhase2AllocationTests.cs` | 14/14 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/PowerGridPhase2GenerationPortfolioTests.cs` | 11/11 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/Phase3WaterIntegrationTests.cs` | 10/10 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/PowerGridSystemTests.cs` | 20/20 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/PowerGridDeterminismTests.cs` | 4/4 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/PowerGridSurgeTests.cs` | 14/14 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/PowerGridSurgeConfigurationTests.cs` | 4/4 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/NuclearCorePowerGridPublishTests.cs` | 4/4 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/BalancePowerEconomyTests.cs` | 7/7 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Phase1SharedContractsTests.cs` | 13/13 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/WaterAndQuarantinePowerTests.cs` | 8/8 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Save/B5B8Phase0SaveFixtureTests.cs` | 6/6 PASS (fixture re-captured) |
| `dotnet build Ashfall.csproj` | 0 warnings, 0 errors |

## Files changed

- `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs` (condition state, wear, output factor, maintenance, event kind, capture/restore)
- `src/Host/PowerGridHostSession.cs` (PerformGeneratorMaintenance)
- `src/Main.World.cs` (service route: check → consume → commit → sigil)
- `src/UI/PowerGridPanel.cs` (condition display + service button)
- `Ashfall.Core.Tests/Fixtures/B5B8_Phase0/power_grid_phase0.json` (re-captured)
- `Ashfall.Core.Tests/Shelter/PowerGridPhase5MaintenanceTests.cs` (new, 9 tests)
- `docs/plans/flagship_b5_b8/PHASE5_GENERATION_PORTFOLIO.md` (this file)
