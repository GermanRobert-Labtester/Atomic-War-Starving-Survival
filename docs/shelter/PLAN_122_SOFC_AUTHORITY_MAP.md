# PLAN 122 — SOFC Authority Map (Phase 1)

**Status:** ACCEPTED (reconnaissance). Premise-verified against current source.

## Verified owners

| Concern | Owner | Evidence |
|---|---|---|
| Power dispatch / producer registration | `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs` | `SetGenerationContribution(sourceId, watts)` :87 — existing producers register contributions (EbPVD precedent `EbPvdInstalledSourceId` :116, wired from `src/Main.AdvancedShelterSystems.cs:480` and `src/Host/Plans74To77HostSessions.cs:86`) |
| Grid fuel | `PowerGridSystem` | `AddFuel(units)` :324, `FuelUnits` :75; host feeds from inventory (`src/Main.World.cs:528`) |
| Battery / brownout | `PowerGridSystem` | `BatteryReserveWh`/`BatteryCapacityWh` :76–77, `IsBrownout` :80 |
| Waste heat / CHP | `Assets/Ashfall.Core/ShelterThermalSystem.cs` | `AddAuxiliaryHeat(roomId, heatKw)` :427; `SetGeneratorWasteHeat(generatorKw, pumpActive)` :961 — **zero current callers, seam live but unfed** |
| Thermal consumers (rooms) | `ShelterThermalSystem` rooms by `roomId` | room list :77; crop temp modifier :410; frostbite risk :193 |
| Load shedding / priority | `PowerGridSystem` | `PowerGridRoomPriority` :269, breakers/trips :280–312 |
| Power storage | `PowerGridSystem` battery fields | no separate battery authority |
| Ceramic precursors | `Narrative/CeramicsKilnCatalog.cs`, `Narrative/CrucibleFoundryCatalog.cs` | kiln/foundry systems own production |
| Save | `Shelter/PowerGridSave.cs` (grid) + new SOFC section via `SaveSectionRegistry` | SOFC persists only: mode, startup/cooldown progress, stack health, seal integrity, degradation counters, fault state, fuel-quality derived state |
| RNG | `CampaignRngStream` fork keys `sofc.stack_degradation`, `sofc.fault_event` | fork-from-parent pattern (Plan 138 precedent) |
| Skills | No `FuelCellChemist`/`Electrochemist` traits exist | use canonical skill keys (engineering/mechanics) via existing skill-modifier pipeline |

## Answers to workstream 122-A questions

1. Producer registration: per-source contribution row (`SetGenerationContribution`) — SOFC
   registers as `sofc_stack_<profile>` with bounded watts.
2. Dispatch priority: room priority + breaker state inside PowerGridSystem; SOFC is a
   baseload contributor, no special-casing.
3. Load shedding: room priority + brownout logic already owned by grid.
4. Fuel model: single `FuelUnits` pool on the grid; consumption modeled by the producer
   contribution math; SOFC engine never mutates fuel inventory.
5. Generator noise: **no acoustic-signature system exists** (only `MachineIdentity/
   MachineTellAudioSync.cs`, presentation tells). SOFC exposes `AcousticSignatureRating`
   as a read-only fact; consumer deferred (master map §4.3).
6. Heat seam: `ShelterThermalSystem` — SOFC never touches kitchen/greenhouse stats
   directly; it calls `AddAuxiliaryHeat` per allocated room through the host tick.
7. Equipment condition: `EquipmentConditionSystem` — stack health is SOFC-owned (it is a
   plant, not a handheld), persisted in the SOFC section; NOT routed through the item
   condition sink.
8. Startup/shutdown: new `SofcOperatingMode` state machine; grid contribution is zero
   until Online and scales during Preheating/Stabilizing — no instant on/off.
9. Biogas output authority: **none exists** — out of scope (master map §4.2).
10. Gas cleaning: represented as SOFC-owned `FuelQualityState` (Dirty/Treated/Clean)
    derived from consumed fuel item IDs; filter items consumed via inventory owner.
11. Refractory crafting: kiln/foundry owners produce precursor items; SOFC consumes item
    IDs for rebuilds.
12. Battery authority: grid-owned; SOFC does not add storage.
13. Duplicate generator model: none created — SOFC is one more contribution row plus its
    own degradation state.

## Non-goals / must-not (per plan §24)

No real electrochemistry; no gas-mixture recipes; no undetectable flag; no global fuel
economy multiplier; no duplicated fuel/grid/thermal truth; diesel and other generators
remain relevant (dominance table is a test deliverable, `SofcElectrochemistryEngineTests`
case 20).

## New files (planned)

- `Assets/Ashfall.Core/Shelter/SofcElectrochemistryEngine.cs`
- `Assets/StreamingAssets/Data/sofc_power_catalog.json`
- `src/Host/SofcPowerHostSession.cs`, `src/Host/SofcPowerSaveStore.cs`
- `src/Main.SofcPower.cs`, `src/UI/SolidOxideFuelCellPanel.cs`
- `Ashfall.Core.Tests/Shelter/SofcElectrochemistryEngineTests.cs`
- Save rows: `sofc_power` (registry + schema version + filename + section-count update)
