# Plan 71 — Power Grid Rooms Expansion — Completion Report

**Status: COMPLETE.** Final catalog: **18 powered rooms** (9 existing + 9 new). Data-first with 5
minimal host wiring gates and zero Core gameplay changes.

## Summary

`power_grid.json` expanded from 9 to 18 powered rooms. The plan's stated baseline of 6 was stale —
repository truth at execution time was 9 (the cryo vault and quarantine ward had landed via the
SHELTER_HARDENING wave). Delta = 9, per §1.4. All 9 original entries preserved byte-identical,
in order, with stable IDs.

## Baseline (verified)

- 9 rooms; total nominal draw **1530 W** vs `generation_watts_default` **800 W** — the shipped
  design already runs deliberate early-game brownout pressure, escaped by building generation
  (nuclear cores 100 W–5 kW, geothermal ORC up to 450+ kW).
- Priority enum: `Disabled=0, Low=1, Standard=2, Critical=3`; JSON vocabulary
  `critical`/`standard`/`low`/`disabled` (case-insensitive via `MapPriority`).
- Identity model: **Model D (hybrid)** — entries are room definitions for canonical Plan 41 rooms
  (`room_clinic`, `room_workshop`, `room_ward_quarantine`) *and* aggregate services
  (`room_water_pump`, `room_lighting_main`, `room_air_filtration` ↔ Plan 41 `room_filtration`).
- Generator boundary: **Outcome B** — generation is a source (`generation_watts_default` +
  external contributions via `SetGenerationContribution`); no `room_generator_room` consumer was
  added. The Plan 41 `room_generator` identity stays a room, not a grid consumer.
- Save: `PowerGridState` (breakers/tripped/priorities/battery/fuel/lastSurgeDay) with unknown-ID
  tolerance in `NormalizeAndValidate` — new catalog entries are additive-safe for old saves.

## Final catalog (18)

| # | id | draw W | priority | failure effect | consumer |
|---|---|---:|---|---|---|
| 1 | room_air_filtration | 180 | critical | fx_filtration_off | StartingLevelSystem powerAvailability01 |
| 2 | room_clinic | 120 | critical | fx_clinic_off | MedicalPipelineCoordinator + disease day-owner |
| 3 | room_water_pump | 100 | critical | fx_water_pressure_drop | Plan168 fluid day-owner |
| 4 | room_greenhouse | 160 | standard | fx_grow_lights_off | agriculture lighting availability |
| 5 | room_foundry | 220 | low | fx_foundry_standstill | SilentFoundry host power gate |
| 6 | room_lighting_main | 80 | low | fx_lighting_dim | shelter schedule lighting demand |
| 7 | room_workshop | 300 | low | fx_workshop_offline | crafting station sync (Main.World) |
| 8 | room_cryo_vault | 280 | critical | fx_cryo_vault_unpowered | CryoVaultSystem power provider |
| 9 | room_ward_quarantine | 90 | critical | fx_quarantine_ventilation_off | DiseaseQuarantineCoordinator |
| 10 | **room_heating** | 240 | standard | fx_heating_off | ResourceMassBalanceSimulator isNearHeatSource *(dead query made live)* |
| 11 | **room_kitchen** | 150 | standard | fx_kitchen_off | ResourceMassBalanceSimulator kitchenPower *(dead query made live)* |
| 12 | **room_water_filtration** | 140 | critical | fx_water_filtration_off | ResourceMassBalanceSimulator waterPower *(dead query made live)* |
| 13 | **room_airlock** | 130 | standard | fx_airlock_decon_off | raider-assault airlock power (perimeter defense path) *(dead query made live)* |
| 14 | **room_radio_tuner** | 90 | standard | fx_radio_tuner_off | ShelterRadioStationSystem monitoring pause (new host gate) |
| 15 | **room_laboratory_research** | 260 | standard | fx_laboratory_offline | research start-boundary gate (new host gate) |
| 16 | **room_workshop_precision** | 240 | standard | fx_precision_metrology_off | metrology drift pause + zero calibration projection (new host gate) |
| 17 | **room_common_mess_hall** | 70 | low | fx_common_mess_cold | daily decor-morale pause (new host gate) |
| 18 | **room_armory_munitions** | 60 | standard | fx_armory_service_off | powered perimeter emplacements require armory circuit (new host gate) |

## Load budget

Total nominal **2910 W** — critical **910**, standard **1330**, low **670**; base generation 800 W.

| Scenario | Supply | Outcome |
|---|---|---|
| Early, all rooms, base gen | 800 W | net −2110 W → ~16 h brownout/day; standard/low breakers trip first (seeded); critical core carried by battery until reserve exhausts — **below-critical-envelope is live by design** (910 > 800), forcing generation build-up |
| Mid, +2 kW pebble-bed core | 2800 W | near balance; priority decisions matter (shed low/standard to protect critical) |
| Mid, +5 kW TRIGA | 5800 W | full surplus; battery charges |
| Late, geothermal ORC | 450+ kW | trivial surplus (catalog ceiling by design) |

Early-game viability is proven by the full-campaign economy tests (food/water/fuel ≥ 0, health ≥ 85
across seeded multi-day runs) — passing with the 18-room grid.

## Failure effects / edge semantics

All 9 new effects are **level gates** (true while unpowered, false on restore; no one-shot
irreversible consequences, no per-tick accumulation — §8.1/§8.3). The G6 consumer map in
`PowerGridCatalogTests` pins every fx ID to its named consumer. Four entries fixed the dead-query
bug class: `IsRoomPowered("room_heating"/"room_kitchen"/"room_water_filtration"/"room_airlock")`
previously always returned false because no catalog entry existed.

## Host wiring (5 gates, all at existing call sites)

1. `Main.Plans46_49` — radio monitoring pauses while `room_radio_tuner` is unpowered (absolute
   expiry timestamps batch-resume; no state loss).
2. `Main.CampaignOwners` (SurvivorsNeedsDayOwner) — daily decor morale pauses while
   `room_common_mess_hall` is unpowered (once-per-day, no accumulation).
3. `Main.PlansB86_B89` (TickPrecisionMetrology) — drift tick pauses and the projected
   workshop calibration grade reads zero while `room_workshop_precision` is unpowered.
4. `Main.Plans162_165` — powered perimeter emplacements draw through the armory circuit: the raid
   power delegate now requires `room_armory_munitions` in addition to grid health.
5. `ResearchHostSession.StartResearchGate` (new optional host property, set in
   `Main.PlayerSurfaces` at both creation sites) — new research cannot start while
   `room_laboratory_research` is unpowered; in-flight progress is never touched.

`NEW CORE CHANGE JUSTIFICATION: NOT REQUIRED` — zero `Assets/Ashfall.Core/` gameplay changes.
One embedded-fallback sync (`ShelterPowerGridCatalog.FallbackDefault`) keeps the missing-file boot
path consistent with the catalog, per the loader's own documented contract.

## Incidents / schedules

Incident hooks (Plan 57) already flow through generation contributions and the surge path
(`ApplySurgeDay` trips lowest-tier circuits deterministically, critical-exempt below 0.9 severity);
no incident state was added to room data. Plan 70 schedule coupling: **deferred** — no
dynamic-demand hook exists (Case B, §71D.6); static draw semantics preserved.

## Save compatibility

Old saves: unknown room IDs are pruned by `NormalizeAndValidate`; new rooms default to closed
breakers and standard priority on first appearance; user-set priorities (stored overrides) win over
catalog defaults everywhere (`ApplySurgeDay` reads overrides first). Battery/fuel round-trip is
pinned by existing tests; no new persisted fields.

## Determinism

Shedding/surge ordering is tier-ascending then RoomId ordinal — independent of JSON order and
dictionary iteration. Load-budget math is closed-form from static draws. The seeded load-spike and
trip rolls are unchanged. `PowerGridDeterminismTests` green.

## Verification

| Command | Result |
|---|---|
| `dotnet test --filter PowerGrid + BalancePowerEconomy + SteamTurbine` | **109/109 PASS** |
| `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | **9461/9461 PASS** |
| `godot --headless -- --data-integrity-selftest` | **PASS** — 0 findings, 298 catalogs |
| `godot --headless -- --content-utilization-selftest` | **PASS** |
| `godot --headless -- --power-grid-catalog-selftest` | **PASS** — rooms=18, fluidPower=1, surgeTrips=6 (deterministic), battery 3700 Wh after surge |
| `godot --headless -- --real-campaign-journey-selftest` | **PASS** |
| `dotnet build Ashfall.csproj` | **PASS** — 0 warnings, 0 errors |

## Deferred

- Plan 70 schedule-driven dynamic demand (no live hook).
- Per-machine power gating inside `ShelterWorkshopSystem` (would need a Core API for per-room
  labor gating; the precision/armory rooms are gated at their consumer boundaries instead).
- Generator maintenance/efficiency depth, emergency presets, machine-personality outage reactions.
