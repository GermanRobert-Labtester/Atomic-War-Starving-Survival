# Phase 2 — Power Normalization (B5–B8 / Plan 65) — increment 1 landed

> Parity-safe additive increment over the live `PowerGridSystem`.
> No save-shape change, no consumer migration, no balance change to the
> legacy battery/fuel/brownout-hours math.

## Landed in this increment

### 1. Deterministic priority allocation projection
- `PowerGridSystem.ComputeAllocation(...)` (private) + `IsRoomServed(roomId)`
  (public typed query).
- Eligible rooms (closed breaker, not tripped, not Disabled) are ordered by
  effective priority tier **descending**, then RoomId **ordinal ascending**.
  No RNG participates in load order.
- Available power = generation (post fuel-starvation adjustment) + battery
  discharge the tick can sustain (`reserve/24`, only while demand exceeds
  generation — the same condition under which the legacy aggregate math
  drains the battery).
- **Strict priority-prefix serving**: the first room whose load does not fit
  is shed together with every room after it, even if a later smaller room
  would fit (feeder-shedding order). This keeps the flagship invariant exact:
  a lower-priority load is never served while a higher-priority load is
  unserved. Spare capacity below the next whole room stays unused.
- `ServedWatts + UnservedWatts == RequestedDrawWatts` bookkeeping identity.

### 2. Typed tick-summary extension (`PowerGridTickSummary`, additive fields)
`GenerationWatts`, `RequestedDrawWatts`, `ServedWatts`, `UnservedWatts`,
`HasCriticalDeficit`, `BrownoutBegan`, `BrownoutEnded`, `ServedRoomIds`,
`ShedRoomIds`. Not persisted; consumed via `OnTickSummary` only.

### 3. Critical-deficit semantics (flagship §8.6)
`HasCriticalDeficit` = at least one Critical-tier room unserved — the explicit
life-support emergency flag, distinct from an ordinary brownout where only
lower-priority loads shed. Power never damages survivors; the affected
subsystem owns its consequence (consumer migration = later increments).

### 4. Brownout edge transitions (flagship §14.3)
`BrownoutBegan` / `BrownoutEnded` fire exactly once per transition, derived
from runtime-only bookkeeping (`_prevTickBrownout`, never saved).
`RestoreState` re-seeds the flag from the restored state, so a reload never
replays a transition.

### 5. `EffectivePriority` catalog-default fix
Previously an override-less room always read `Standard`, silently discarding
the catalog's `critical`/`low` classification (`power_grid.json`). Now:
player override wins; catalog `DefaultPriority` otherwise. Consumer:
`src/UI/PowerGridPanel.cs` (display only — panel now shows the truthful
catalog priority). Allocation uses the same override-or-default resolution.

## Landed in increment 2 (generation portfolio)

### 6. Solar grid-tie chain (research → recipe → item → install → grid feed)
- `SolarConcentratorEngine.PowerSourceId = "solar_concentrator"` — stable
  contribution id, same contract as the nuclear core.
- Additive `gridTieConnected` state field (legacy saves restore false — no
  free grid feed after migration).
- `ConnectGridTie()`: consumes canonical `item_solar_inverter` exactly once
  (recipe `assemble_pure_sine_solar_inverter`, gated by
  `knowledge_solar_advanced`), requires the mounted Stirling generator;
  blocked reasons name the exact missing piece; zero mutation on failure.
- `GridFeedWatts` = connected ? electrical kW × 1000 : 0 (weather-driven,
  deterministic; × 1000 matches the geothermal ORC unit precedent).
- `SolarConcentratorHostSession.ConnectGridTie()` + Main publish path:
  `PublishSolarConcentratorGeneration()` republishes idempotently on setup,
  restore, output change, and tick — same discipline as the nuclear
  contribution. Contribution lands after the phase-1 power tick, effective
  the next day (nuclear-consistent ordering).
- Maintenance already live upstream (mirror fouling/`CleanMirrors`,
  calibration) — the feed degrades with the dish, no free-forever power.

### 7. Battery bank build chain
- `PowerGridSystem.TryInstallBatteryBank()`: canonical
  `item_battery_reconditioned` consumed by the caller (coated-part pattern);
  bounded `MaxInstalledBatteryBanks = 4`, `+1000 Wh` each
  (`BatteryBankCapacityWh`); never touches the stored reserve; typed
  `PowerGridEventKind.BatteryBankInstalled` event.
- Additive `PowerGridState.InstalledBatteryBankCount` (clamped 0..4 on
  normalize/restore). Old saves: count 0, stored capacity unchanged — no
  free energy.
- Real player route: `PowerGridPanel` "INSTALL BATTERY BANK" button →
  `Main.OpenPowerGrid` handler → inventory check → consume once → commit →
  sigil feedback; blocked installs mutate nothing and say why. Panel battery
  label shows `BANKS n/4`.
- Phase 0 fixture re-captured (deliberate additive migration,
  `InstalledBatteryBankCount: 0`); legacy-shape JSON restore pinned by test
  (`BatteryBank_LegacySaveWithoutField_RestoresDefaults`,
  `GridTie_LegacySaveWithoutField_RestoresDisconnected`).

## Deliberately deferred (remaining Phase 2+ scope)

| Item | Target phase | Note |
|---|---|---|
| Consumer migration to `IsRoomServed`/tick summary (greenhouse, sump pump, sentries, vinyl/radio) | Phases 3–4, 7 | consumers keep legacy `IsRoomPowered` (global outage) until their phase lands its consequence owner |
| Charge/discharge efficiency < 100% for the battery model | Phase 5 | legacy aggregate math stays exact-parity at 100% (never energy-creating); real efficiency lands with the battery-chain rework |
| Base-generator condition/wear loop | Phase 5 | bounded component state + save migration; fuel loop already live |
| Solar recipe oddity flagged: `assemble_pure_sine_solar_inverter` consumes `item_solar_inverter` → produces `battery_pack`×3 (likely authoring error); data tuning belongs to the content stream, not this package | content stream | untouched, flagged in handoff |
| Per-source maintenance/condition | Phase 5 | reuse per-system patterns (sump `pumpCondition`, water `filterIntegrity`) |
| Strict hourly energy-conservation identity | Phase 5 | legacy daily abstraction drains the battery by *aggregate deficit*, not served load; the identity becomes enforceable once the battery chain adds real charge/discharge efficiency |
| `PowerGridPanel` served/shed/critical-deficit display | Phase 9 UI honesty pass | summary fields already carry the data |
| Perimeter-defense power coupling | Phase 7 | `perimeter_defenses.json` `power_draw_watts` audit |

## Parity evidence

- Legacy aggregate battery/fuel/`brownoutHours` math untouched
  (`TickDay_BrownoutHours_MatchLegacyAggregateMath` pins the legacy formula).
- `PowerGridState` DTO unchanged → Phase 0 fixture
  (`Fixtures/B5B8_Phase0/power_grid_phase0.json`) byte-parity intact,
  `B5B8Phase0SaveFixtureTests` 6/6 green without re-capture.
- Existing subscribers (`PowerGridHostSession`, `ShelterAudioController`)
  unaffected (additive fields only).

## Verification record (2026-09-13)

| Command | Result |
|---|---|
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/PowerGridPhase2AllocationTests.cs` | 14/14 PASS (new) |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/PowerGridSystemTests.cs` | 20/20 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/PowerGridDeterminismTests.cs` | 4/4 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/PowerGridSurgeTests.cs` | 14/14 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/PowerGridSurgeConfigurationTests.cs` | 4/4 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Save/B5B8Phase0SaveFixtureTests.cs` | 6/6 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Phase1SharedContractsTests.cs` | 13/13 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/NuclearCorePowerGridPublishTests.cs` | 4/4 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/BalancePowerEconomyTests.cs` | 7/7 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/WaterAndQuarantinePowerTests.cs` | 8/8 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/ShelterScheduleIntegrationTests.cs` | 3/3 PASS |
| `dotnet build Ashfall.csproj` | 0 warnings, 0 errors |

### Increment 2 verification (2026-09-13)

| Command | Result |
|---|---|
| `B5B8_CAPTURE_FIXTURES=1 bash scripts/run_test.sh Ashfall.Core.Tests/Save/B5B8Phase0SaveFixtureTests.cs` | 6/6 PASS, fixture re-captured (additive `InstalledBatteryBankCount`) |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/PowerGridPhase2GenerationPortfolioTests.cs` | 11/11 PASS (new) |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/PowerGridPhase2AllocationTests.cs` | 14/14 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/PowerGridSystemTests.cs` | 20/20 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/PowerGridDeterminismTests.cs` | 4/4 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/PowerGridSurgeTests.cs` | 14/14 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/PowerGridSurgeConfigurationTests.cs` | 4/4 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/NuclearCorePowerGridPublishTests.cs` | 4/4 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Phase1SharedContractsTests.cs` | 13/13 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/BalancePowerEconomyTests.cs` | 7/7 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/IndustrialCapabilityExpansionTests.cs` | 16/16 PASS |
| `dotnet build Ashfall.csproj` | 0 warnings, 0 errors |

### Increment 2 files changed

- `Assets/Ashfall.Core/Shelter/SolarConcentratorEngine.cs` (PowerSourceId, grid-tie state + install action, capture/restore)
- `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs` (battery-bank consts/state/install/event kind)
- `src/Host/SolarConcentratorHostSession.cs` (ConnectGridTie)
- `src/Host/PowerGridHostSession.cs` (TryInstallBatteryBank)
- `src/Main.Plans110_113.cs` (solar publish path)
- `src/Main.World.cs` (battery-bank install route)
- `src/UI/PowerGridPanel.cs` (install button, bank count display)
- `Ashfall.Core.Tests/Fixtures/B5B8_Phase0/power_grid_phase0.json` (re-captured)
- `Ashfall.Core.Tests/Shelter/PowerGridPhase2GenerationPortfolioTests.cs` (new, 11 tests)

## Files changed

- `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs` (allocation, summary
  fields, edges, `IsRoomServed`, `EffectivePriority` fix, restore re-seed)
- `Ashfall.Core.Tests/Shelter/PowerGridPhase2AllocationTests.cs` (new, 14 tests)
- `docs/plans/flagship_b5_b8/PHASE2_POWER_NORMALIZATION.md` (this file)
