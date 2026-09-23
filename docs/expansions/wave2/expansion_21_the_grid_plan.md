# ASHFALL — Expansion 21 Design Bible
# THE GRID
### Wave 2 · Power Generation, Storage, Distribution, Blackouts, EMP, and Energy Society

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-20
**Domain owners touched:** `Ashfall.Core.Shelter` (PowerGrid, Subgrids, SOFC, Solar), `Ashfall.Core` (Kinetic storage), `Ashfall.Core.Economy` (adjacent), `Ashfall.Core.Factions` (grid politics)
**Proposed host owner:** `GridHostSession` (extends `PowerGridHostSession` + distribution/SOFC/solar stores)
**Existing save sections:** `power`, `infrastructure`, `power_subgrids`, `sofc`, `solar`, `kinetic_storage`
**Existing CLI verbs:** `--power-grid-selftest`, `--power-subgrid-selftest`, `--sofc-selftest`, `--solar-selftest`
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already owns electricity with a strong single authority. `PowerGridSystem`
declares itself the "single authority for shelter electrical state," owns
deterministic generation/draw/battery/brownout math, emits typed `PowerGridEvent`s,
and keeps a `PowerGridTickSummary`. It explicitly does not persist generation
contributions: owning systems restore first and the host republishes them.
`PowerDistributionSubgridSystem` models breakers, fuses, load, temperature,
transformer oil, and a capacitor buffer, with a 90% overload threshold.
`SofcElectrochemistryEngine` (Plan 122), `SolarConcentratorEngine`,
`SteamTurbinePowerCatalog`, kinetic flywheel storage, and the geothermal ORC add
generation and storage. `room_air_filtration`, `room_clinic`, `room_water_pump`,
`room_greenhouse`, `room_foundry`, and `room_lighting_main` already declare draw,
priority, and failure effects.

But the content is thin and the social dimension is missing: a few catalogs, a few
rooms, and no model of what a blackout does to a community, who gets power first when
it is short, how a grid is shared between settlements, or what an EMP actually means
for a shelter that depends on electricity for air and water.

**The Grid** turns the power system from a utility screen into the campaign's spine:
load politics, cascading faults, fuel logistics, storage discipline, EMP hardening,
microgrids, and the endless negotiation over who gets to be warm, lit, and breathing.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin; **`PROPOSED`** new.

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

The shelter does not run on hope. It runs on watts. Air filtration, water pumping,
the clinic, the greenhouse, the foundry, the lights, the radios, the deep pumps, the
implants, the robots, and the freezers all draw from the same finite pool, and the
pool is managed by one deterministic authority that the whole game depends on.

**The Grid** is the expansion about that dependency. It adds load politics — who is
shed first when generation falls short. It adds cascading faults — a single trip that
takes down a wing. It adds fuel logistics for every generator, storage discipline for
every battery and flywheel, EMP hardening, a regional microgrid that can be shared or
weaponized, and the social reality of energy poverty: cold rooms, dark evenings,
spoiled medicine, and a shelter that has to choose.

The expansion's central insight is simple: **electricity is not a resource, it is a
priority list.** The player can always keep the lights on for someone; the question
is who.

### 1.2 The five loops it adds

```
    Generation ──► Distribution ──► Load priority ──► Consumption ──► Consequence
        │              │                 │                 │              │
        ▼              ▼                 ▼                 ▼              ▼
    Fuel chains,   Subgrids,        Shedding order,   Rooms,          Warmth, air,
    maintenance    breakers         criticality       equipment       water, morale
        │              │                 │                 │              │
        ▼              ▼                 ▼                 ▼              ▼
    Storage ──► Stability ──► Faults/cascades ──► Repair ──► Trust
        │
        ▼
    Microgrid ──► Regional sharing ──► Treaties, embargo, war
```

### 1.3 What the player manages

1. **Generation.** Every source has a fuel chain, a maintenance cycle, an output
   curve, and a failure mode. `PowerGridSystem` already handles the math; the
   expansion gives each source authored content and upkeep.
2. **Distribution.** `PowerDistributionSubgridSystem` owns breakers, fuses, load,
   temperature, transformer oil, and capacitors. The expansion authors the network
   and its faults.
3. **Load priority.** The live grid has room priorities and failure effects. The
   expansion makes priority a political object: the clinic, the greenhouse, the
   foundry, and the lighting all have constituencies.
4. **Storage.** Batteries, flywheels, and capacitors with charge discipline, losses,
   and danger. Storage is how the shelter survives the night and the storm.
5. **Stability.** Frequency-analogue and voltage-analogue behavior, cascading trips,
   and restoration sequencing.
6. **EMP.** A real threat with real hardening choices; `SHELTER_EMP` work already
   exists and this expansion extends it.
7. **Microgrids.** Neighbor settlements can interconnect; sharing is diplomacy,
   embargo is war, and a shared grid is the most literal form of commons in the game.

### 1.4 What it is not

- Not a second power system. `PowerGridSystem` remains the single authority.
- Not a physics simulator; values are authored game abstractions.
- Not a second economy or currency. Energy is accounted through existing resource
  and trade systems.
- Not a second fault or EMP authority; it extends the live ones.
- Not a save-section fork; all new state is additive in existing envelopes.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs` | Single authority: generation, draw, battery, brownout, typed events | `LIVE` |
| `Assets/Ashfall.Core/Shelter/PowerDistributionSubgridSystem.cs` | Nodes, breakers, fuses, load, temperature, oil, capacitors | `LIVE` |
| `Assets/Ashfall.Core/Shelter/PowerGridSave.cs` | Grid persistence | `LIVE` |
| `Assets/Ashfall.Core/Shelter/ShelterPowerGridCatalog.cs` | Room/draw catalog loading | `LIVE` |
| `Assets/Ashfall.Core/Shelter/PowerSubgridCatalog.cs` | Subgrid catalog | `LIVE` |
| `Assets/Ashfall.Core/Shelter/SofcElectrochemistryEngine.cs` | SOFC generation | `LIVE` |
| `Assets/Ashfall.Core/Shelter/SofcPowerCatalog.cs` | SOFC catalog | `LIVE` |
| `Assets/Ashfall.Core/Shelter/SolarConcentratorEngine.cs` | Solar generation | `LIVE` |
| `Assets/Ashfall.Core/Narrative/SteamTurbinePowerCatalog.cs` | Steam turbine | `LIVE` |
| `Assets/Ashfall.Core/Narrative/` kinetic flywheel catalog | Flywheel storage | `LIVE` |
| `src/Host/PowerGridHostSession.cs` | Host | `LIVE` |
| `src/UI/PowerGridPanel.cs`, `DefenseGridPanel.cs`, `GeothermalSteamTurbinePanel.cs`, `HeavyMarineDieselGeneratorPanel.cs` | UI | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Size | Notes |
|---|---|---|
| `power_grid.json` | 3.7 KB | rooms, draw, priorities, failure effects |
| `power_subgrid_nodes.json` | 4.2 KB | node capacities, fuses, oil |
| `sofc_power_catalog.json` | 5.4 KB | fuel cells, grades, heat |
| `kinetic_flywheel_catalog.json` | 6.0 KB | rotor storage |
| `solar_concentrator_catalog.json` | 1.3 KB | solar concentration |
| `cellulosic_ethanol_catalog.json` | 2.1 KB | fuel production |
| `bio_fermentation_catalog.json` | 3.3 KB | fuel production |
| `geothermal_strata_catalog.json` | 1.5 KB | heat source |
| `geothermal_drilling_depths.json` | 2.1 KB | drilling bands |

### 2.3 Confirmed gaps

- **GAP-21-1 — Few rooms and nodes.** The grid supports far more than the handful of
  authored rooms and subgrid nodes.
- **GAP-21-2 — No load politics.** Priorities exist as data but there is no
  constituency, legitimacy, or social consequence to shedding.
- **GAP-21-3 — No cascade model.** A trip does not propagate; there is no restoration
  sequence, no frequency analogue.
- **GAP-21-4 — Fuel chains are partial.** SOFC and ethanol exist, but there is no
  unified fuel logistics model across diesel, biogas, ethanol, coal, and wood gas.
- **GAP-21-5 — Storage lacks discipline.** Flywheels and batteries exist; no charge
  policy, loss, danger, or reserve doctrine.
- **GAP-21-6 — EMP is an incident, not a strategy.** Hardening, spares, and recovery
  planning are thin.
- **GAP-21-7 — No microgrid.** Settlements cannot interconnect, share, or embargo.
- **GAP-21-8 — No grid locations or NPCs.**
- **GAP-21-9 — No energy accounting or poverty model.** Consumption lacks a social
  face: cold rooms, dark evenings, spoiled medicine, rationed heat.

### 2.4 Non-duplication statement

This expansion will **not** add a second power grid, a second distribution system, a
second storage system, a second EMP authority, or a second save section. It extends
`PowerGridSystem`, `PowerDistributionSubgridSystem`, the SOFC/solar/turbine/flywheel
generation systems, and the existing EMP work. It consumes `NeedsSystem`,
`MoraleContagionSystem`, `FactionStanceEngine`, and `Inventory`. Generation
contributions remain runtime projections republished by the host, exactly as the live
system requires.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Power is a priority list.** Every blackout is a decision about whose
need matters. The expansion makes the decision visible and political.

**Pillar 2 — The grid is fragile by design.** One bad trip can cascade. Restoration
is a sequence, not a switch. Redundancy is a budget item.

**Pillar 3 — Fuel is a chain.** Every generator has an upstream: a refinery, a still,
a mine, a digester, a forest, or a scavenging route. Cut the chain and the generator
is furniture.

**Pillar 4 — Storage is discipline.** Charge too little and the night kills; charge
too much and the flywheel screams. Reserve doctrine is a skill.

**Pillar 5 — A shared grid is a shared fate.** Interconnecting with a neighbor means
sharing shortages and surpluses. It is the most honest diplomacy in the game.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| A blackout | A fan spooling down, a held breath | Neon explosion |
| A shed decision | A clipboard and a closed breaker | Dramatic lever pull |
| A flywheel | Vibration, vacuum, heat, fear | Sci-fi turbine |
| A fuel run | Drums, hoses, a long road | Action convoy |
| A microgrid | Two cables and a treaty | Techno utopia |
| Restoration | Sequence, patience, a checklist | One-button reset |

### 3.3 Content limits

- All generation and storage are authored game abstractions, not physics simulations.
- No real-world utility, reactor, or grid brand is referenced.
- EMP content is abstract; no real weapon design or effect is described.
- Energy poverty is depicted with restraint and dignity, never as set dressing.
- No energy content may trivialize the survival loop.

---

## 4. THE GRID WORLD

### 4.1 Interior rooms

- **`room_generator_hall`** — prime movers, exhaust, heat, and noise.
- **`room_battery_vault`** — cells, racks, ventilation, and fire risk.
- **`room_flywheel_pit`** — rotor storage, vacuum, and containment.
- **`room_switchgear`** — breakers, protection, and the manual panel.
- **`room_transformer_bay`** — step-downs, oil, cooling, and fire suppression.
- **`room_fuel_depot`** — drums, tanks, pumps, and vapor control.
- **`room_load_control`** — the priority board and the shedding console.
- **`room_grid_workshop`** — cable, fuses, meters, and spares.

### 4.2 Exterior locations

| ID | Name | Danger | Purpose |
|---|---|---|---|
| `loc_substation_ruins` | The Substation | 6 | Switchgear and transformer salvage |
| `loc_turbine_hall` | The Turbine Hall | 7 | Large prime mover salvage |
| `loc_solar_field` | The Mirror Field | 5 | Concentrator array; cleaning and theft |
| `loc_wind_ridge` | The Wind Ridge | 6 | Wind generation site |
| `loc_battery_yard` | The Cell Yard | 6 | Battery salvage; fire hazard |
| `loc_fuel_depot` | The Tank Farm | 6 | Fuel logistics and vapor risk |
| `loc_microgrid_hub` | The Intertie | 5 | Regional interconnect point |
| `loc_cable_trench` | The Long Trench | 4 | Buried cable route; fault location |
| `loc_relay_station` | The Relay | 5 | Load signaling and control |
| `loc_old_powerhouse` | The Powerhouse | 7 | Pre-war generation archive and salvage |

All locations require valid item references and scanner registration.

### 4.3 The grid map

The grid is a graph, not a new map screen. Rooms are loads; subgrid nodes are
distribution points; generation sources are contributions; interties are links.
`PowerGridSystem` walks the graph. The expansion authors the nodes, edges, and
priorities in data, and the existing panel displays them.

---

## 5. MAIN STORYLINE — "WHO GETS TO BE WARM"

### 5.1 Central conflict

A winter storm takes out the solar field and drops the battery reserve to fourteen
hours. The shelter has enough power for the clinic and the water pump, or for the
greenhouse and the foundry, or for heat and air filtration — but not all of them.
The chief engineer, **Rosa Idris**, presents the shedding plan. The greenhouse
keeper, **Ottilie Frayne**, points out that losing the crop means spring starvation.
The foundry foreman, **Cael Ormund**, points out that losing the forge means no
replacement parts for the pumps that keep everyone alive.

At the same time, a neighboring settlement offers an intertie: their wind ridge
against the shelter's battery reserve, a shared grid that would end both shortages.
The offer comes with a treaty, a toll, and an intelligence risk. And the old
powerhouse contains a pre-war load-control archive that would let the shelter manage
the winter far better — if it can be reached and read.

The expansion's question: **when there is not enough power for everyone, who decides,
and how does the shelter live with the decision?**

### 5.2 Theme (unspoken)

**A shelter is only as generous as its reserve. When the reserve is gone, the shelter
learns what it really values.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_engineer_rosa_idris` | Rosa Idris | Chief engineer | Owns the grid; must choose and explain |
| `npc_grower_ottilie_frayne` | Ottilie Frayne | Greenhouse keeper | Fights for the grow lights |
| `npc_foreman_cael_ormund` | Cael Ormund | Foundry foreman | Fights for the forge |
| `npc_medic_asa_vell` | Dr. Asa Vell | Physician | Fights for the clinic and the cold chain |
| `npc_gridkeeper_pell_arn` | Pell Arn | Battery keeper | Charge discipline and reserve doctrine |
| `npc_wind_envoy_sable` | Sable | Neighbor envoy | Offers the intertie and the treaty |
| `npc_archivist_volt_marr` | Volt Marr | Archive engineer | The pre-war load-control records |
| `npc_child_lamp_junip` | Junip | Child of the dark | The human face of load shedding |

### 5.4 Story beats (15)

1. **The Storm.** Solar field lost; reserve at fourteen hours.
2. **The Shedding Plan.** Rosa presents the priorities; the room splits.
3. **The Cold Room.** A shed room drops to freezing; consequences.
4. **The Intertie Offer.** A neighbor proposes a shared grid.
5. **The Treaty Terms.** Toll, priority, and intelligence clauses.
6. **The Fuel Run.** Diesel must be found before the generator stops.
7. **The Flywheel.** Storage doctrine; a rotor over-speed scare.
8. **The Archive.** The pre-war load-control records are located.
9. **The Cascade.** A breaker fault trips a wing; restoration sequence.
10. **The Theft.** Someone is stealing power; a private line is found.
11. **The EMP Drill.** A preparedness exercise reveals the shelter's exposure.
12. **The Choice.** The winter's deepest night; the final shedding decision.
13. **The Intertie.** Connect, refuse, or build their own.
14. **The Reckoning.** Trust, warmth, and the cost of the winter.
15. **The Lamp.** Final disposition of the grid and the reserve.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Shedding priority | clinic / food / heat / industry | need vs. future |
| Intertie | connect / refuse / counter-offer | sharing vs. control |
| Fuel run | risk the road / ration / buy | supply vs. safety |
| Private line | punish / legalize / ignore | order vs. freedom |
| EMP posture | harden / spares / accept | cost vs. risk |
| Storage doctrine | deep reserve / cycling | safety vs. efficiency |
| Archive | read / sell / ignore | knowledge vs. leverage |
| Final | reserve / share / ration forever | legacy |

### 5.6 Endings (5 + fade)

1. **The Shared Grid** — the intertie works; the region shares warmth and risk.
2. **The Dark Winter** — the shelter rations and survives, colder and smaller.
3. **The Burned Field** — a cascade destroys the switchgear; years of rebuilding.
4. **The Private Lines** — power is privatized by faction; the grid fragments.
5. **The Deep Reserve** — storage discipline wins; the shelter never runs dark again.
6. **Fade** — the winter passes; the grid is patched and forgotten.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_grid_`. Schema follows `year_of_ash_quests.json`.

### 6.1 Main questline (15)

`quest_grid_the_storm`, `quest_grid_shedding_plan`, `quest_grid_cold_room`,
`quest_grid_intertie_offer`, `quest_grid_treaty_terms`, `quest_grid_fuel_run`,
`quest_grid_the_flywheel`, `quest_grid_the_archive`, `quest_grid_the_cascade`,
`quest_grid_the_theft`, `quest_grid_emp_drill`, `quest_grid_the_choice`,
`quest_grid_the_intertie`, `quest_grid_the_reckoning`, `quest_grid_the_lamp`.

### 6.2 Side quests (30)

**Generation (6)**
- `quest_grid_generator_service` — service a prime mover
- `quest_grid_exhaust_repair` — fix an exhaust leak
- `quest_grid_fuel_quality` — dirty fuel damages a generator
- `quest_grid_solar_cleaning` — clean concentrator mirrors
- `quest_grid_wind_repair` — repair a wind machine
- `quest_grid_geothermal_tap` — bring a heat source online

**Storage (5)**
- `quest_grid_battery_rack` — replace a failed cell
- `quest_grid_flywheel_vacuum` — restore rotor vacuum
- `quest_grid_capacitor_bank` — repair the buffer
- `quest_grid_charge_policy` — write the reserve doctrine
- `quest_grid_battery_fire` — contain a cell fire

**Distribution (5)**
- `quest_grid_breaker_fault` — find an intermittent trip
- `quest_grid_transformer_oil` — change oil before failure
- `quest_grid_cable_trench` — repair a buried cable
- `quest_grid_fuse_shortage` — improvise fuses from salvage
- `quest_grid_load_balance` — rebalance a phase

**Load politics (5)**
- `quest_grid_priority_board` — write the shedding order
- `quest_grid_cold_complaint` — answer a shed room's grievance
- `quest_grid_clinic_guarantee` — guarantee the cold chain
- `quest_grid_greenhouse_guarantee` — guarantee the grow lights
- `quest_grid_foundry_guarantee` — guarantee the forge

**EMP and resilience (4)**
- `quest_grid_emp_drill` — run a preparedness exercise
- `quest_grid_spare_parts` — stock critical spares
- `quest_grid_hardened_panel` — harden the control room
- `quest_grid_recovery_plan` — write the restoration sequence

**Microgrid (5)**
- `quest_grid_intertie_cable` — lay the interconnect
- `quest_grid_shared_frequency` — agree on a shared standard
- `quest_grid_embargo_test` — a neighbor cuts power
- `quest_grid_priority_negotiation` — negotiate shared priorities
- `quest_grid_counter_grid` — build a rival interconnect

### 6.3 Repeatable quests (8)

`quest_grid_repeat_service`, `quest_grid_repeat_fuel`,
`quest_grid_repeat_charge`, `quest_grid_repeat_patrol`,
`quest_grid_repeat_meter`, `quest_grid_repeat_fuse`,
`quest_grid_repeat_drill`, `quest_grid_repeat_treaty_check`.

### 6.4 Dynamic hooks

`PowerGridSystem` emits typed power events and tick summaries; subgrids emit trips;
SOFC/solar/flywheel emit degradation; EMP emits incidents. The generator attaches
authored follow-ups without a new event bus.

### 6.5 Constraints

- No second power model. All generation and load flow through `PowerGridSystem`.
- No new currency. Fuel and energy trade uses existing resources and trade.
- No cascade may damage the grid outside the live subgrid model.
- No blackout may be scripted to kill; consequences route through needs and medical
  systems, never a direct wipe.
- No intertie may bypass `FactionStanceEngine`.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `LoadPrioritySystem` (new, `Ashfall.Core.Shelter`)

**Owns:** the editorial priority ladder, constituency legitimacy, and shedding
consequences. **Consumes:** `PowerGridSystem` room priorities and `PowerGridEvent`s,
`NeedsSystem`, `MoraleContagionSystem`. **Data:** `load_profiles.json`.
**Rules:** priority is authored and can be changed only through quests; every change
has a constituency; shedding routes consequences through live owners.

### 7.2 `GridStabilitySystem` (new, `Ashfall.Core.Shelter`)

**Owns:** frequency-analogue and voltage-analogue behavior, trip propagation,
restoration sequencing, and black-start order. **Consumes:**
`PowerDistributionSubgridSystem`, `PowerGridSystem`. **Data:** `grid_faults.json`.
**Rules:** trips propagate only along authored edges; restoration is a sequence with
authored order and risk; the system never invents a fault outside authored rates.

### 7.3 `FuelChainSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** generator fuel profiles, quality, storage, and upstream chains across
diesel, biogas, ethanol, coal, wood gas, and hydrogen. **Consumes:** `Inventory`,
`PowerGridSystem` contributions, fermentation/ethanol catalogs. **Data:**
`fuel_chains.json`.
**Rules:** fuel quality affects output and wear; a generator without its chain is
inert; fuel storage has spoilage and hazard.

### 7.4 `EnergyStorageSystem` (extend)

**Owns:** battery, flywheel, and capacitor reserve doctrine, losses, and hazard.
**Consumes:** existing storage engines and catalogs. **Data:** `energy_storage.json`.
**Rules:** storage loses energy over time; overcharge and over-discharge are risky;
reserve doctrine is authored and visible.

### 7.5 `EmpResilienceSystem` (new, thin, `Ashfall.Core.Shelter`)

**Owns:** EMP exposure assessment, hardening, spare stock, and recovery planning.
**Consumes:** existing EMP incidents, `PowerGridSystem`, `Inventory`. **Data:**
`emp_hardening.json`.
**Rules:** hardening reduces but never eliminates risk; spares determine recovery
time; the shelter can choose to accept risk.

### 7.6 `MicrogridSystem` (new, `Ashfall.Core.Factions`)

**Owns:** interties, shared standards, shared priority, embargo, and treaty state.
**Consumes:** `PowerGridSystem`, `FactionStanceEngine`, `DiplomaticTreaties`.
**Data:** `microgrid_nodes.json`, `power_treaties.json`.
**Rules:** shared power moves through the live grid; an embargo is a standing and
needs event; no second grid is created.

### 7.7 `EnergyAccountingSystem` (new, thin, `Ashfall.Core.Economy`)

**Owns:** generation/draw accounting, waste heat reuse, and energy cost reporting.
**Consumes:** `PowerGridSystem` summaries. **Data:** `load_profiles.json`.
**Rules:** accounting is reporting and trade support, never a second resource. Energy
is traded as fuel and service, not as a new currency.

### 7.8 Systems explicitly not added

- No second grid, storage, EMP, or economy system.
- No physics simulation beyond authored abstraction.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `power_grid.json` (extend)

Existing schema preserved (`schema_version`, `generation_watts_default`,
`battery_capacity_wh_default`, `fuel_units_default`, `rooms[]` with `id`,
`display_name`, `draw_watts`, `default_priority`, `failure_effect_id`). New rooms:
battery vault, flywheel pit, switchgear, transformer bay, fuel depot, load control,
grid workshop, deep pumps, clean ward, quarantine block, filter works, mask store,
data room, and a dozen others.

### 8.2 `power_subgrid_nodes.json` (extend)

New nodes across the authored grid: main bus, clinic, workshop, greenhouse, foundry,
deep levels, microgrid intertie, exterior sites, and redundant paths. Every node keeps
`node_id`, `display_name`, `target_room_id`, `max_capacity_watts`,
`surge_limit_watts`, `transformer_oil_condition`, `cooling_efficiency`,
`fuse_rating_amps`, `is_critical`.

### 8.3 `generation_sources.json` (new)

```json
{
  "schema_version": 1,
  "sources": [
    {
      "source_id": "gen_diesel_primary",
      "display_name": "Primary Diesel Generator",
      "source_class": "combustion",
      "rated_watts": 2200,
      "fuel_item_id": "fuel_diesel",
      "fuel_per_hour": 4,
      "min_load_fraction": 0.3,
      "efficiency": 0.34,
      "waste_heat_watts": 900,
      "maintenance_interval_days": 14,
      "wear_per_day_bp": 40,
      "acoustic_signature": "high",
      "cold_start_days": 0,
      "tags": ["diesel", "baseline", "noisy"]
    }
  ]
}
```

### 8.4 `load_profiles.json` (new)

Load rows: room, draw, priority, constituency, failure effect, minimum viable power,
and written justification.

### 8.5 `grid_faults.json` (new)

Fault rows: cause, affected nodes, propagation, restoration sequence, repair recipe,
and severity.

### 8.6 `energy_storage.json` (new)

Storage rows: type, capacity, charge/discharge limits, losses, hazard, reserve
doctrine options.

### 8.7 `fuel_chains.json` (new)

Chain rows: fuel type, source chain, quality bands, spoilage, storage hazard, and
generator compatibility.

### 8.8 `emp_hardening.json` (new)

Hardening rows: target, cost, risk reduction, spare list, recovery days.

### 8.9 `microgrid_nodes.json` (new)

Intertie rows: partner settlement, capacity, standard, priority terms, embargo state.

### 8.10 `power_treaties.json` (new)

Treaty rows: terms, toll, shared priority, intelligence clauses, breach consequences.

### 8.11 Items

New items: `item_fuel_diesel`, `item_fuel_biogas`, `item_fuel_ethanol`,
`item_coal_chunk`, `item_fuse_cartridge`, `item_breaker_assembly`,
`item_transformer_oil`, `item_cable_reel`, `item_meter_gauge`,
`item_battery_cell`, `item_capacitor_bank`, `item_flywheel_bearing`,
`item_waste_heat_exchanger`, `item_hardened_panel`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

`PowerGridSystem` state is captured in `PowerGridSave`; generation contributions are
runtime-only and republished by the host after restore. Subgrid state has its own
store; storage engines have theirs. New sub-objects are additive. No new save section.

### 9.2 State to persist

- Grid state (live) and room priorities.
- Subgrid nodes, breakers, fuses, oil, temperature.
- Generation source wear, fuel quality, and maintenance state.
- Storage reserve doctrine and charge policy.
- EMP hardening and spare stock.
- Microgrid interties, treaties, and embargo state.
- Load-priority editorial state.
- Fault/restoration state.

### 9.3 Determinism

- Brownout and trip rolls use the host-forked `ISeededRng`.
- Dispatch and load math are pure arithmetic in the live system.
- Generation contributions are runtime-only and must not be persisted.
- Paired replay hashes must match.

### 9.4 Migration

Legacy saves load with default priorities, no authored nodes, no hardening, no
interties, and neutral reserve doctrine. Existing power, subgrid, SOFC, solar, and
flywheel state is untouched.

### 9.5 Checksum

Invariant-culture floats; integer-permille for wear, quality, and priority weights.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `PowerGridPanel` (extend) | Generation, load, reserve, rooms, priorities | `GridHostSession` |
| `LoadPriorityPanel` (new) | Shedding ladder and constituencies | same |
| `SubgridPanel` (new) | Nodes, breakers, oil, temperature | same |
| `GenerationPanel` (new) | Sources, fuel, wear, maintenance | same |
| `StoragePanel` (new) | Batteries, flywheels, capacitors, doctrine | same |
| `FuelChainPanel` (new) | Fuel types, chains, quality, hazard | same |
| `EmpPanel` (new) | Hardening, spares, recovery plan | same |
| `MicrogridPanel` (new) | Interties, treaties, embargo | same |
| `RestorationPanel` (new) | Fault state and black-start sequence | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Priority changes show exactly which rooms lose power and for how long.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- Reserve and brownout warnings are explicit; no silent failure.
- Shedding decisions are logged and visible; a room knows why it is dark.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: generator cough, breaker snap, flywheel
spool, fan dying, fuel pump, cable hum, the silence of a blackout. No cue is
required; text carries meaning. The expansion deliberately keeps lighting low during
blackouts rather than staging drama.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `PowerGridSystem` | Single authority; extended with rooms and priority data |
| `PowerDistributionSubgridSystem` | Nodes and faults extended |
| `SofcElectrochemistryEngine` | Fuel quality and wear |
| `SolarConcentratorEngine` | Cleaning, weather, output |
| `SteamTurbinePowerCatalog` | Large prime movers |
| Kinetic flywheel storage | Reserve doctrine and hazard |
| EMP incidents (`SHELTER_EMP` work) | Hardening and recovery |
| `NeedsSystem` | Warmth, air, water, morale |
| `MoraleContagionSystem` | Cold, dark, grievance |
| `FactionStanceEngine` | Interties and embargo |
| `Inventory` | Fuel, spares, meters |
| `SilentFoundrySystem` | Cast parts, T-beams, switchgear |
| `Excavation` | Cable trench and deep rooms |
| `DiseaseSystem` / `MedicalPipelineCoordinator` | Cold chain and clinic outage |
| `Farming` / `GreenhouseSystem` | Grow-light outage |
| `RadioTuner` | Intertie signaling |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm `PowerGridSystem`, `PowerDistributionSubgridSystem`,
SOFC, solar, turbine, flywheel, EMP work, save stores, and panels. Record file:line;
change nothing.

**Phase 1 — Data + validators.** Extend grid rooms and nodes; author generation
sources, load profiles, faults, storage, fuel chains, hardening, microgrid, treaties.
Register validators and scanner.

**Phase 2 — Pure Core.** `LoadPrioritySystem`, `GridStabilitySystem`,
`FuelChainSystem`, `EnergyStorageSystem`, `EmpResilienceSystem`,
`MicrogridSystem`, `EnergyAccountingSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip, determinism.

**Phase 4 — Host + CLI.** `GridHostSession`, extended selftest verbs, fresh journey.

**Phase 5 — UI.** Extended and new surfaces with lifecycle and accessibility.

**Phase 6 — Content.** Locations, rooms, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** 90/180-day soak including winter load, fuel scarcity, cascades.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Grid rooms | 24 |
| Subgrid nodes | 20 |
| Generation sources | 20 |
| Load profiles | 24 |
| Grid faults | 18 |
| Storage types | 12 |
| Fuel chains | 12 |
| Hardening options | 10 |
| Microgrid nodes | 8 |
| Power treaties | 8 |
| Locations | 10 |
| Rooms | 8 |
| NPCs | 8 |
| Main quests | 15 |
| Side quests | 30 |
| Repeatable | 8 |
| Items | 14 |
| Endings | 5 + fade |
| Prose estimate | 60,000–75,000 words |

### 12.3 Risks

| Risk | Severity | Mitigation |
|---|---|---|
| Second power model | Critical | Extend live authority only |
| Persisting contributions | High | Runtime-only, host republishes |
| Blackout unfun or unfair | High | Explicit warnings and recovery |
| Microgrid trivializes scarcity | High | Shared shortage, toll, risk |
| EMP becomes spam | Medium | Authored cadence and hardening |
| Fuel chains break pacing | Medium | Authored bands and salvage |
| Determinism break | Low | Host-forked RNG |
| Content overrun | Medium | Budget §13 |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `power_grid.json` | 24 rooms | 4,000 |
| `power_subgrid_nodes.json` | 20 nodes | 3,000 |
| `generation_sources.json` | 20 | 5,000 |
| `load_profiles.json` | 24 | 4,000 |
| `grid_faults.json` | 18 | 3,500 |
| `energy_storage.json` | 12 | 2,500 |
| `fuel_chains.json` | 12 | 3,000 |
| `emp_hardening.json` | 10 | 2,000 |
| `microgrid_nodes.json` | 8 | 2,000 |
| `power_treaties.json` | 8 | 2,500 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 14 | 2,000 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~62,000** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R21-1 | Second power model | Low | Critical | Extend live authority |
| R21-2 | Contributions persisted | Low | High | Runtime-only |
| R21-3 | Blackout unfair | Med | High | Warnings, recovery |
| R21-4 | Microgrid trivial | Med | High | Shared shortage |
| R21-5 | EMP spam | Med | Med | Authored cadence |
| R21-6 | Fuel pacing | Med | Med | Authored bands |
| R21-7 | Determinism | Low | High | Host-forked RNG |
| R21-8 | Content overrun | Med | Med | Budget §13 |
| R21-9 | Load politics opaque | Med | Med | Explicit priority UI |
| R21-10 | Cascade too punishing | Med | Med | Restoration sequence |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Can the player re-prioritize rooms freely, or only via quests?** Recommended:
   freely, but with constituency consequences.
2. **Should a blackout be able to cause death?** Recommended: only through needs,
   cold chain, and medical systems, never directly.
3. **Should the microgrid be symmetric, or can one partner hold the switch?**
   Recommended: symmetric by default, with authored asymmetric treaties.
4. **Should EMP be a recurring hazard or a rare scripted event?** Recommended:
   recurring at a low authored rate, with a drill quest.
5. **Should energy accounting ever gate content?** Recommended: no; reporting only.

---

## 17. APPENDIX D — GRID ROOM TABLE (24 ROOMS)

| # | Room | Draw W | Priority | Failure effect |
|---|---|---|---|---|
| 1 | Air Filtration *(LIVE)* | 180 | critical | `fx_filtration_off` |
| 2 | Clinic *(LIVE)* | 120 | critical | `fx_clinic_off` |
| 3 | Water Pump *(LIVE)* | 100 | critical | `fx_water_pressure_drop` |
| 4 | Greenhouse *(LIVE)* | 160 | standard | `fx_grow_lights_off` |
| 5 | Silent Foundry *(LIVE)* | 220 | low | `fx_foundry_standstill` |
| 6 | Main Lighting *(LIVE)* | 80 | low | `fx_lights_off` |
| 7 | Battery Vault | 40 | standard | charge halt |
| 8 | Flywheel Pit | 60 | standard | rotor coast |
| 9 | Switchgear | 20 | critical | protection loss |
| 10 | Transformer Bay | 30 | critical | distribution loss |
| 11 | Fuel Depot | 50 | standard | pump stop |
| 12 | Load Control | 25 | critical | blind grid |
| 13 | Grid Workshop | 90 | low | repair stop |
| 14 | Deep Pumps | 200 | standard | deep flood |
| 15 | Ventilation Plant | 150 | critical | deep air loss |
| 16 | Clean Ward | 110 | critical | ward failure |
| 17 | Quarantine Block | 90 | standard | cross-flow |
| 18 | Filter Works | 130 | standard | canister stop |
| 19 | Mask Store | 15 | low | humidity damage |
| 20 | Data Room | 70 | standard | records loss |
| 21 | Radio Room | 60 | standard | silence |
| 22 | Interrogation Room | 10 | low | darkness |
| 23 | Holding Cell | 20 | standard | cell safety |
| 24 | Cryo Bank | 140 | critical | seed/medicine loss |

The table is the campaign's real priority list. Every room has a constituency, and
every blackout is a fight about a line in this table.

---

## 18. APPENDIX E — SUBGRID NODE TABLE (20 NODES)

| # | Node | Target | Capacity W | Surge W | Fuse A | Critical |
|---|---|---|---|---|---|---|
| 1 | Main Vault Bus *(LIVE)* | room_main | 8000 | 10000 | 50 | yes |
| 2 | Clinic Step-Down *(LIVE)* | clinic | 3500 | 4500 | 25 | yes |
| 3 | Workshop Feed *(LIVE)* | workshop | 4500 | 6000 | 32 | no |
| 4 | Greenhouse Feed | greenhouse | 3000 | 4000 | 20 | no |
| 5 | Foundry Heavy | foundry | 6000 | 8000 | 40 | no |
| 6 | Lighting Ring | lighting | 1500 | 2000 | 12 | no |
| 7 | Deep Level Feed | deep | 4000 | 5500 | 30 | yes |
| 8 | Water Pump Feeder | pump | 2500 | 3500 | 18 | yes |
| 9 | Clean Ward Isolated | clean ward | 1800 | 2400 | 15 | yes |
| 10 | Quarantine Isolated | quarantine | 1600 | 2200 | 14 | no |
| 11 | Cryo Feeder | cryo | 2000 | 2600 | 16 | yes |
| 12 | Intertie Breaker | microgrid | 5000 | 7000 | 45 | no |
| 13 | Fuel Depot Feed | depot | 1200 | 1800 | 10 | no |
| 14 | Radio Feed | radio | 1000 | 1500 | 9 | no |
| 15 | Data Feeder | data | 1400 | 2000 | 11 | no |
| 16 | Transformer Bay A | distribution | 9000 | 12000 | 60 | yes |
| 17 | Transformer Bay B | distribution | 9000 | 12000 | 60 | yes |
| 18 | Ring Tie A | redundancy | 4000 | 6000 | 30 | no |
| 19 | Ring Tie B | redundancy | 4000 | 6000 | 30 | no |
| 20 | Emergency Bus | black-start | 2000 | 3000 | 20 | yes |

The redundant ties and the emergency bus determine whether a fault cascades or is
contained. Their condition is the difference between a bad night and a dark winter.

---

## 19. APPENDIX F — GENERATION SOURCE TABLE (20 SOURCES)

| # | Source | Class | Rated W | Fuel | Fuel/h | Efficiency | Wear/day |
|---|---|---|---|---|---|---|---|
| 1 | Diesel Primary | combustion | 2200 | diesel | 4 | 0.34 | 40 |
| 2 | Diesel Backup | combustion | 900 | diesel | 2 | 0.30 | 30 |
| 3 | Biogas Digester | combustion | 1100 | biogas | 6 | 0.28 | 25 |
| 4 | Ethanol Gen | combustion | 800 | ethanol | 5 | 0.26 | 30 |
| 5 | Wood Gasifier | gasification | 600 | wood | 12 | 0.22 | 45 |
| 6 | Coal Boiler Gen | steam | 1400 | coal | 8 | 0.29 | 35 |
| 7 | SOFC Mk I | fuel cell | 1200 | biomethane | 3 | 0.55 | 12 |
| 8 | SOFC Mk II | fuel cell | 1800 | biomethane | 4 | 0.60 | 10 |
| 9 | Solar Concentrator A | solar | 700 | none | 0 | — | 8 |
| 10 | Solar Concentrator B | solar | 700 | none | 0 | — | 8 |
| 11 | Wind Machine A | wind | 500 | none | 0 | — | 10 |
| 12 | Wind Machine B | wind | 900 | none | 0 | — | 12 |
| 13 | Geothermal ORC | geothermal | 1500 | none | 0 | 0.11 | 6 |
| 14 | Steam Turbine | steam | 2600 | coal | 14 | 0.31 | 30 |
| 15 | Micro Hydro | hydro | 400 | none | 0 | — | 5 |
| 16 | Kinetic Flywheel | storage | discharge 1500 | none | 0 | 0.88 | 4 |
| 17 | Battery Bank | storage | discharge 800 | none | 0 | 0.92 | 2 |
| 18 | Capacitor Buffer | storage | burst 2000 | none | 0 | 0.95 | 2 |
| 19 | Intertie Import | external | 3000 | agreement | 0 | — | 0 |
| 20 | Emergency Hand Gen | manual | 120 | labor | 0 | — | 1 |

Every source has a chain, a maintenance cycle, a signature, and a failure mode. The
shelter's job is to keep enough of them alive through winter.

---

## 20. APPENDIX G — LOAD PROFILE TABLE (24 LOADS)

| # | Load | Criticality | Min viable W | Constituency | Shed cost |
|---|---|---|---|---|---|
| 1 | Air Filtration | critical | 120 | everyone | death risk |
| 2 | Clinic | critical | 80 | patients | care loss |
| 3 | Water Pump | critical | 70 | everyone | thirst |
| 4 | Cryo Bank | critical | 90 | future | seed loss |
| 5 | Switchgear | critical | 15 | everyone | protection loss |
| 6 | Transformer Bay | critical | 20 | everyone | distribution loss |
| 7 | Load Control | critical | 20 | operators | blind grid |
| 8 | Deep Pumps | standard | 120 | deep crews | flood |
| 9 | Ventilation Plant | critical | 100 | deep crews | air loss |
| 10 | Clean Ward | critical | 80 | sick | ward failure |
| 11 | Greenhouse | standard | 100 | growers | crop loss |
| 12 | Silent Foundry | low | 150 | smiths | parts stop |
| 13 | Main Lighting | low | 40 | everyone | dark evening |
| 14 | Battery Vault | standard | 25 | reserve | charge halt |
| 15 | Flywheel Pit | standard | 40 | reserve | rotor coast |
| 16 | Fuel Depot | standard | 30 | logistics | pump stop |
| 17 | Grid Workshop | low | 60 | repair | repair stop |
| 18 | Quarantine Block | standard | 60 | quarantined | cross-flow |
| 19 | Filter Works | standard | 90 | safety | canister stop |
| 20 | Data Room | standard | 40 | records | record loss |
| 21 | Radio Room | standard | 35 | signals | silence |
| 22 | Mask Store | low | 10 | safety | media damage |
| 23 | Holding Cell | standard | 15 | prisoners | safety |
| 24 | Interrogation Room | low | 8 | intelligence | darkness |

Shed cost is stated plainly so the player cannot pretend a choice is free. The
priority ladder is editorial, changeable, and always visible.

---

## 21. APPENDIX H — GRID FAULT TABLE (18 FAULTS)

| # | Fault | Cause | Affected | Propagation | Restoration |
|---|---|---|---|---|---|
| 1 | Breaker Trip | overload | one node | none | reset |
| 2 | Fuse Blow | surge | one room | none | replace |
| 3 | Transformer Overheat | sustained load | bay | adjacent | cool, oil |
| 4 | Oil Failure | age | bay | ring | replace oil |
| 5 | Cable Fault | moisture | trench | section | dig, splice |
| 6 | Insulation Failure | wear | node | local | replace |
| 7 | Phase Imbalance | bad load | bus | feeders | rebalance |
| 8 | Capacitor Failure | age | buffer | transient | replace |
| 9 | Battery Cell Failure | heat | bank | bank | replace cell |
| 10 | Flywheel Bearing | vibration | pit | local | service |
| 11 | Generator Overheat | coolant | source | local | cool, service |
| 12 | Exhaust Leak | corrosion | hall | local | repair |
| 13 | Fuel Contamination | dirty fuel | source | local | flush |
| 14 | Control Failure | EMP, age | control | grid | manual mode |
| 15 | Protection Mis-coord | settings | grid | cascade | re-coordinate |
| 16 | Intertie Trip | partner fault | link | both | isolate |
| 17 | Black Start Fail | sequence | grid | all | manual order |
| 18 | Substation Fire | fault, fuel | bay | cascade | suppress |

Faults propagate only along authored edges. Cascades are rare, teachable, and
recoverable; the point is not punishment but consequence.

---

## 22. APPENDIX I — STORAGE TABLE (12 TYPES)

| # | Storage | Capacity | Charge | Discharge | Loss/day | Hazard |
|---|---|---|---|---|---|---|
| 1 | Lead Bank | 4000 Wh | 600 W | 800 W | 1.5% | acid |
| 2 | Lithium Bank | 6000 Wh | 1200 W | 1500 W | 0.5% | fire |
| 3 | Flywheel Small | 2000 Wh | 500 W | 900 W | 4% | rupture |
| 4 | Flywheel Large | 8000 Wh | 1500 W | 2500 W | 3% | rupture |
| 5 | Capacitor Buffer | 400 Wh | 2000 W | 2000 W | 8% | arc |
| 6 | Gravity Stack | 3000 Wh | 300 W | 400 W | 0.2% | mechanical |
| 7 | Compressed Air | 2500 Wh | 400 W | 600 W | 2% | burst |
| 8 | Thermal Store | 5000 Wh | 700 W | 700 W | 1% | scald |
| 9 | Hydrogen Buffer | 3500 Wh | 500 W | 800 W | 2.5% | explosion |
| 10 | Redox Cell | 4500 Wh | 600 W | 700 W | 1% | chemical |
| 11 | Salinity Cell | 2200 Wh | 300 W | 350 W | 0.8% | corrosion |
| 12 | Emergency Bank | 1000 Wh | 300 W | 500 W | 1% | none |

Storage doctrine is the difference between riding a night and rationing one. Each
type trades capacity, rate, loss, and danger differently, and none dominates.

---

## 23. APPENDIX J — FUEL CHAIN TABLE (12 CHAINS)

| # | Fuel | Source chain | Quality bands | Spoilage | Hazard |
|---|---|---|---|---|---|
| 1 | Diesel | salvage, refinery | dirty/clean | low | fire |
| 2 | Biogas | digester, waste | raw/scrubbed | med | methane |
| 3 | Ethanol | still, crops | crude/rectified | low | fire |
| 4 | Methanol | wood, chemistry | crude/refined | low | toxic |
| 5 | Wood Gas | gasifier, timber | raw/filtered | none | CO |
| 6 | Coal | mine, salvage | lump/slack | none | dust |
| 7 | Charcoal | kiln, timber | soft/hard | none | CO |
| 8 | Biomethane | digester, upgrade | raw/clean | low | methane |
| 9 | Hydrogen | electrolysis, reform | wet/dry | high | explosion |
| 10 | Waste Oil | salvage, refining | dirty/treated | low | fire |
| 11 | Peat | bog, drying | wet/dry | med | smoke |
| 12 | Animal Fat | rendering | crude/clean | med | fire |

Each chain gives the region a reason to exist: a digester needs waste, a still needs
crops, a gasifier needs timber, a refinery needs salvage. Cut the chain and the
generator is furniture.

---

## 24. APPENDIX K — EMP HARDENING TABLE

| # | Target | Cost | Risk reduction | Spares | Recovery days |
|---|---|---|---|---|---|
| 1 | Control room | high | 0.60 | panels | 2 |
| 2 | Grid switchgear | high | 0.55 | breakers | 3 |
| 3 | Clinic equipment | med | 0.70 | spares | 1 |
| 4 | Cryo bank | med | 0.65 | cells | 2 |
| 5 | Radio room | low | 0.50 | tubes | 1 |
| 6 | Data room | low | 0.45 | drives | 3 |
| 7 | Generator controls | high | 0.60 | modules | 2 |
| 8 | Intertie | high | 0.50 | relays | 4 |
| 9 | Deep systems | high | 0.55 | fans, pumps | 5 |
| 10 | General wiring | very high | 0.30 | cable | 10 |

Hardening reduces but never eliminates risk, and the shelter can always choose to
accept risk and spend the materials elsewhere. That choice is the expansion's EMP
theme: preparedness is a budget line, not a virtue.

---

## 25. APPENDIX L — MICROGRID AND TREATY TABLE

| # | Partner | Capacity | Standard | Priority terms | Embargo risk |
|---|---|---|---|---|---|
| 1 | Wind Ridge Camp | 900 W | shared Hz | proportional | med |
| 2 | River Flotilla | 600 W | shared Hz | first-come | high |
| 3 | Foundry Enclave | 1500 W | industrial | contract | low |
| 4 | Market Town | 500 W | shared Hz | paid | med |
| 5 | Deep Bunker | 1200 W | exact match | reciprocal | low |
| 6 | Garrison | 2000 W | imposed | garrison-first | very high |
| 7 | Hunter Camp | 300 W | loose | informal | high |
| 8 | The Registry | 200 W | shared Hz | archival | none |

An intertie is the most literal diplomacy in the game: two settlements literally
share the current. A shared standard is trust; an embargo is a weapon that hurts
both sides.

---

## 26. APPENDIX M — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_grid_the_storm` | 3 | Solar field lost; reserve counted |
| `quest_grid_shedding_plan` | 5 | Write, argue, and publish the priority ladder |
| `quest_grid_cold_room` | 4 | A shed room freezes; respond and repair |
| `quest_grid_intertie_offer` | 3 | Receive the neighbor's proposal |
| `quest_grid_treaty_terms` | 5 | Negotiate toll, priority, and intelligence |
| `quest_grid_fuel_run` | 5 | Find diesel before the generator stops |
| `quest_grid_the_flywheel` | 4 | Storage doctrine; an over-speed scare |
| `quest_grid_the_archive` | 5 | Reach the powerhouse; read the load records |
| `quest_grid_the_cascade` | 5 | Contain a cascading fault; restore in order |
| `quest_grid_the_theft` | 4 | Find a private line; punish, legalize, ignore |
| `quest_grid_emp_drill` | 4 | Run the drill; face the results |
| `quest_grid_the_choice` | 6 | The deepest night; final shedding decision |
| `quest_grid_the_intertie` | 5 | Connect, refuse, or counter-offer |
| `quest_grid_the_reckoning` | 5 | Trust, warmth, and the winter's cost |
| `quest_grid_the_lamp` | 3 | Final disposition; epilogue |

---

## 27. APPENDIX N — NPC DOSSIERS (BRIEF)

**Rosa Idris** — chief engineer. Owns the grid and therefore owns a hundred small
deaths by triage. Competent, unsentimental, and privately devastated by the priority
ladder she has to write. Her arc is learning to share the decision instead of
carrying it.

**Ottilie Frayne** — greenhouse keeper. Fights for the grow lights because she knows
what spring will look like without them. Not an idealist; a person doing arithmetic
about food six months out.

**Cael Ormund** — foundry foreman. Fights for the forge because the pumps need parts
that only the forge can make. The expansion's argument that industry is life support.

**Dr. Asa Vell** — physician. Fights for the clinic and the cold chain. Has already
triaged more than she can hold and does not want to add a power outage to the ledger.

**Pell Arn** — battery keeper. Charge discipline, reserve doctrine, and a healthy
fear of deep discharge. The expansion's voice for storage as a discipline rather
than a tank.

**Sable** — neighbor envoy. Offers the intertie with honest terms and a hidden clause.
Represents the possibility of a shared grid and the risk of dependence.

**Volt Marr** — archive engineer. The pre-war load-control records are his obsession.
Believes the old world knew how to run a grid and that the knowledge can be recovered.

**Junip** — child of the dark. Grew up during blackout winters and is not afraid of
it in the way adults are, which is both comforting and a little alarming.

---

## 28. APPENDIX O — LOCATION DETAIL

- **The Substation** — switchgear in rows, copper stripped from some bays, one
  breaker still warm.
- **The Turbine Hall** — a cathedral of rust; a rotor the size of a room.
- **The Mirror Field** — concentrator mirrors pointed at nothing; cleaning every
  panel is a day's work.
- **The Wind Ridge** — three towers on a bare ridge; only two turn.
- **The Cell Yard** — battery racks outdoors, fire scars, a warning nobody reads.
- **The Tank Farm** — drums, bunds, and the smell of old fuel.
- **The Intertie** — two cables, a breaker house, and a treaty nailed to the door.
- **The Long Trench** — a buried cable route marked with stakes; faults are found
  by walking.
- **The Relay** — a load-signaling hut with one working lamp.
- **The Powerhouse** — the pre-war archive, drawers of load charts still legible.

---

## 29. APPENDIX P — STABILITY AND RESTORATION MODEL

The expansion adds an authored stability analogue on top of the live system:

| Signal | Meaning | Response |
|---|---|---|
| Nominal | within band | normal dispatch |
| Under-frequency | generation below load | shed non-critical |
| Over-frequency | load below generation | drop small sources |
| Low voltage | distribution stress | reduce heavy loads |
| High temperature | node stress | trip that node |
| Protection blind | control loss | manual mode |
| Black start | full loss | emergency bus first |

Restoration follows an authored sequence: emergency bus, switchgear, control, water,
air, clinic, then standard loads. Restoring out of order can re-trip the grid. The
sequence is a checklist the player can follow or ignore, and the grid remembers.

---

## 30. APPENDIX Q — WORKED WINTER SCENARIO

**Day 1.** The storm takes the solar field. Reserve: 14 hours. Rosa publishes a
shedding plan that drops the foundry and lighting first.

**Day 2.** The cold room drops below freezing; two survivors suffer exposure. The
clinic demands priority. The greenhouse threatens crop loss.

**Day 3.** The fuel run returns with contaminated diesel; the primary generator
overheats and trips. The backup carries the night.

**Day 4.** A cascade trips the deep level; the ventilation plant stops for six hours.
The restoration sequence is run correctly and the deep is saved.

**Day 5.** The intertie offer is accepted with a toll. Shared power restores lighting
two days early. The neighbor's envoy asks for a priority guarantee in return.

**Day 6.** Someone runs a private line to a living quarter. The theft is found. The
player chooses punishment, legalization, or silence.

**Day 7.** The EMP drill reveals the control room is unhardened. Spending the
materials would cost the foundry's winter repair stock.

**Day 14.** The deep winter. The player makes the final shedding decision, and the
shelter learns what it values. The epilogue records it.

---

## 31. APPENDIX R — VIGNETTE (TONE SAMPLE)

> The fan is the loudest thing in the shelter, and when it stops, everyone hears it.
> Rosa stands at the load board with a grease pencil and crosses out the foundry,
> then the lighting, then the workshop, and the room behind her gets quieter in
> stages as each line goes dark.
>
> Pell is in the battery vault with a clipboard and a meter, counting charge like a
> person counting rations for a siege. He writes the number twice.
>
> In the greenhouse, Ottilie stands in the last lit bay with her hands in the soil
> and does not look up, because the lamps above her are the only thing standing
> between the shelter and a hungry spring.

---

## 32. APPENDIX S — CAMPAIGN ARC TIMELINE

| Phase | Days | Theme | Decision |
|---|---|---|---|
| Storm | 1–20 | shock | shedding plan |
| Cold | 21–50 | survival | priority ladder |
| Fuel | 51–90 | logistics | fuel chain |
| Cascade | 91–120 | fragility | restoration order |
| Intertie | 121–150 | diplomacy | connect or refuse |
| Drill | 151–180 | preparedness | harden or accept |
| Winter deep | 181–240 | values | final choice |

Each phase changes the shelter's relationship to warmth, light, and each other.

---

## 33. APPENDIX T — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| Full blackout | dark, cold, no water | black start sequence |
| Frozen room | exposure cases | heat, medical care |
| Crop outage | future food loss | ration, replant |
| Cold chain loss | medicine spoiled | replace, reorder |
| Fuel shortage | generators stop | ration, salvage, buy |
| Contaminated fuel | engine damage | flush, repair |
| Cascade | multiple rooms down | isolate, restore in order |
| Private theft | load imbalance | legalize, meter, punish |
| Intertie embargo | sudden loss | reserve, negotiate |
| EMP | control loss | manual mode, spares |

No failure is a game over. Every failure has a recovery path, and every recovery
costs materials, time, or trust. The deepest failure is a shelter that stops trusting
its own priority board.

---

## 34. APPENDIX U — CONTENT REVIEW CHECKLIST

- [ ] `PowerGridSystem` remains the single power authority.
- [ ] Generation contributions remain runtime-only and host-republished.
- [ ] No second storage, EMP, or distribution system.
- [ ] No new currency; energy is traded as fuel and service.
- [ ] Blackouts route consequences through needs and medical systems, never a wipe.
- [ ] Priority ladders are explicit and visible.
- [ ] Fuel chains have real upstream dependencies.
- [ ] Storage doctrine has real losses and hazards.
- [ ] Interties go through `FactionStanceEngine`.
- [ ] Save additions are additive and legacy-neutral.
- [ ] Determinism uses the host-forked RNG only.

---

## 35. APPENDIX V — GLOSSARY

- **Load priority** — the editorial order in which rooms are shed.
- **Reserve** — stored energy available to ride a shortage.
- **Cascade** — a fault that propagates across nodes.
- **Black start** — restoring a fully dark grid in authored order.
- **Fuel chain** — the upstream supply that feeds a generator.
- **Contribution** — a runtime generation value republished by the host.
- **Intertie** — a physical link between two settlement grids.
- **Embargo** — a partner cutting shared power.
- **Hardening** — reducing EMP exposure at a materials cost.
- **Constituency** — the people who lose when a room is shed.

---

## 36. APPENDIX W — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `PowerGridSystem` | load, reserve | power state | contributions |
| `PowerDistributionSubgridSystem` | nodes | node state | generation |
| `SofcElectrochemistryEngine` | fuel | output | grid state |
| `SolarConcentratorEngine` | weather | output | grid state |
| Flywheel storage | doctrine | charge | grid state |
| EMP incidents | exposure | incidents | — |
| `NeedsSystem` | outage | morale, warmth | — |
| `MoraleContagionSystem` | cold | contagion | — |
| `FactionStanceEngine` | intertie | standing | — |
| `Inventory` | fuel, spares | transfers | — |
| `SilentFoundrySystem` | cast parts | parts | — |
| `Excavation` | trench | ground | — |
| `MedicalPipelineCoordinator` | cold chain | treatment | — |
| `Farming` | grow light | crop state | — |
| `RadioTuner` | intertie signal | — | — |

---

## 37. APPENDIX X — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Reserve hours per day | suspense | PowerGridSystem |
| Shed room-hours | cost of scarcity | LoadPrioritySystem |
| Cascade events | fragility | GridStabilitySystem |
| Fuel days remaining | logistics pressure | FuelChainSystem |
| Storage loss | doctrine quality | EnergyStorageSystem |
| Hardening spend | preparedness | EmpResilienceSystem |
| Intertie uptime | diplomacy | MicrogridSystem |
| Black-start drills | readiness | RestorationPanel |
| Priority changes | politics | LoadPrioritySystem |

Telemetry is diagnostic only; it never gates content and never becomes a hidden
score. It exists so the team can tell whether the grid is tense or merely annoying.

---

## 38. APPENDIX Y — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive, legacy-neutral, and round-trip tested.
- [ ] Generation contributions are never persisted.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling checks.
- [ ] Phase 6 content passes the review checklist in §34.
- [ ] Phase 7 soak shows winter loads, fuel scarcity, and recovery paths.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No parallel power, storage, EMP, or economy system exists.

---

## 39. APPENDIX Z — OPEN QUESTIONS FOR REVIEW

1. Should the player be able to change priorities mid-blackout?
2. Should a fully dark grid ever kill directly?
3. Should private lines be legalizable as a faction policy?
4. Should the intertie be symmetric by default or treaty-specific?
5. Should EMP be a recurring hazard or a rare authored event?
6. Should energy accounting appear in trade screens or remain internal?
7. Should the emergency bus require a hand-crank source?
8. Should the pre-war load archive grant a permanent efficiency bonus?

None of these may be decided unilaterally; each changes the expansion's tone.

---

## 40. APPENDIX AA — CLOSING VIGNETTE

> On the worst night of the winter, Rosa sits at the load board with the grease pencil
> and does not write anything for a long time. The board says what the board says:
> clinic, water, air, cryo. Everything else is dark. She draws a circle around the
> greenhouse and then rubs it out again, twice, and the final mark she makes is the
> one the shelter will remember.
>
> Down the corridor, the fans are still turning. In the greenhouse the lamps are
> still on. In the clinic the cold chain is still cold. Somewhere above the shaft,
> the wind is doing what the wind does, and the shelter is warm enough, and that is
> the whole of it.

---

## 42. APPENDIX AB — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_grid_generator_service` | 4 | Isolate, service, test, restore |
| `quest_grid_exhaust_repair` | 3 | Find the leak, patch, verify CO |
| `quest_grid_fuel_quality` | 4 | Test fuel, flush system, re-run |
| `quest_grid_solar_cleaning` | 3 | Clean mirrors, assess damage, recalibrate |
| `quest_grid_wind_repair` | 4 | Climb, repair, balance, test |
| `quest_grid_geothermal_tap` | 5 | Drill, connect, commission, regulate |
| `quest_grid_battery_rack` | 3 | Identify cell, replace, balance bank |
| `quest_grid_flywheel_vacuum` | 4 | Pump down, test, spin, certify |
| `quest_grid_capacitor_bank` | 3 | Replace, charge, test burst |
| `quest_grid_charge_policy` | 3 | Write doctrine, publish, enforce |
| `quest_grid_battery_fire` | 4 | Contain, vent, evacuate, rebuild |
| `quest_grid_breaker_fault` | 4 | Trace, isolate, replace, re-close |
| `quest_grid_transformer_oil` | 3 | Drain, refill, test, log |
| `quest_grid_cable_trench` | 4 | Locate, dig, splice, backfill |
| `quest_grid_fuse_shortage` | 3 | Salvage, improvise, rate, install |
| `quest_grid_load_balance` | 4 | Measure, move loads, verify |
| `quest_grid_priority_board` | 4 | Draft, argue, publish, defend |
| `quest_grid_cold_complaint` | 3 | Hear grievance, explain, compensate |
| `quest_grid_clinic_guarantee` | 3 | Guarantee cold chain, allocate reserve |
| `quest_grid_greenhouse_guarantee` | 3 | Guarantee lamps, allocate reserve |
| `quest_grid_foundry_guarantee` | 3 | Guarantee forge, allocate reserve |
| `quest_grid_emp_drill` | 4 | Drill, measure, report, fix |
| `quest_grid_spare_parts` | 4 | List, source, stock, verify |
| `quest_grid_hardened_panel` | 3 | Build, install, test |
| `quest_grid_recovery_plan` | 3 | Write, rehearse, publish |
| `quest_grid_intertie_cable` | 5 | Survey, lay, terminate, energize |
| `quest_grid_shared_frequency` | 3 | Agree standard, synchronize, verify |
| `quest_grid_embargo_test` | 4 | Detect cut, respond, negotiate |
| `quest_grid_priority_negotiation` | 4 | Share priorities, agree terms, sign |
| `quest_grid_counter_grid` | 5 | Build rival link, prove independence |

---

## 43. APPENDIX AC — GRID ARCHITECTURE DIAGRAM

```
   Generation sources
   ├─ Diesel primary ──┐
   ├─ Biogas digester ─┤
   ├─ SOFC stack ──────┤
   ├─ Solar field ─────┼──► Main bus ──► Transformer A ──► Switchgear ──┬──► Clinic
   ├─ Wind machines ───┤                 └───────────────► Transformer B ───► Water
   ├─ Geothermal ORC ──┤                                                  ├──► Air
   └─ Intertie ────────┘                                                  ├──► Deep
                                                                          ├──► Grow
   Storage: battery / flywheel / capacitor / thermal                       ├──► Forge
        │                                                                  └──► Lighting
        └──► reserve bus ──► emergency loads
```

The emergency bus is the last line: air, water, clinic, control. Everything else is
a choice.

---

## 44. APPENDIX AD — WASTE HEAT REUSE

Generation produces heat, and the expansion makes it a real resource:

| Source | Waste heat | Use | Benefit |
|---|---|---|---|
| Diesel | 900 W | space heat, water | warmth in winter |
| SOFC | 400 W | greenhouse | crop warmth |
| Turbine | 1600 W | district heat | several rooms |
| Flywheel | 100 W | workshop | dry air |
| Compost | 150 W | greenhouse | seedling heat |
| Foundry | 1200 W | district heat | winter surplus |

The heat map ties the grid to warmth, which ties it to needs, which ties it to
everything. A shelter that wastes its heat is a shelter that freezes while its
generators run.

---

## 45. APPENDIX AE — MAINTENANCE SCHEDULES

| Asset | Interval | Task | Cost | Downtime |
|---|---|---|---|---|
| Diesel gen | 14 days | oil, filter, test | parts | 4 h |
| Backup gen | 30 days | test run | fuel | 1 h |
| SOFC stack | 20 days | inspect, calibrate | parts | 2 h |
| Solar array | 7 days | clean | labor | 6 h |
| Wind machine | 30 days | grease, inspect | parts | 4 h |
| Battery bank | 10 days | water, balance | water | 2 h |
| Flywheel | 7 days | vacuum, bearings | parts | 3 h |
| Transformer | 60 days | oil test | oil | 4 h |
| Switchgear | 90 days | exercise breakers | labor | 2 h |
| Cable route | 180 days | inspect | labor | 8 h |
| Emergency bus | 30 days | test start | fuel | 1 h |
| Fuel depot | 15 days | water check | labor | 1 h |

Maintenance is the expansion's slow drumbeat. A shelter that skips it does not fail
suddenly; it fails on the worst night, which is the point.

---

## 46. APPENDIX AF — LOAD SHEDDING WORKED EXAMPLES

**Example A — 14-hour reserve.** Load total is 1,600 W; generation is 900 W. Shed
first the forge (220), then lighting (80), then workshop (90), then grow lights
(160). Remaining load 1,050 W; reserve drains at 150 W/h. The shelter can ride 14
hours and save the greenhouse. Alternative: shed the greenhouse and keep the forge,
which saves parts and loses spring.

**Example B — cascade night.** A transformer fault trips the deep feed. Restoration
order: emergency bus, switchgear, control, water, air, clinic, deep pumps. Restoring
deep before control re-trips the grid and costs another two hours. The correct order
is authored and learnable.

**Example C — intertie night.** Wind camp sends 900 W. The shelter keeps lighting on
and charges the battery at 200 W. The intertie toll is 10% of throughput. The
neighbor asks for a priority guarantee next winter, which is the real price.

---

## 47. APPENDIX AG — REGIONAL ENERGY MAP

| Settlement | Generation | Storage | Weakness |
|---|---|---|---|
| The shelter | mixed | deep | fuel chains |
| Wind Ridge Camp | wind | small | calm days |
| River Flotilla | diesel | none | fuel cost |
| Foundry Enclave | coal | thermal | emissions |
| Market Town | solar | battery | salvage parts |
| Deep Bunker | geothermal | flywheel | one shaft |
| Garrison | diesel, large | large | fuel dependence |
| Hunter Camp | salvage | none | mobility |

Every settlement's energy profile is authored and creates a reason to trade,
interconnect, or compete. No profile is self-sufficient in every season, which is
what makes the intertie worth considering.

---

## 48. APPENDIX AH — LORE: THE LIGHT BEFORE

Before the Exchange, the region ran on a single interconnected grid managed from a
control room that no longer exists. The fiction:

- **The Powerhouse** was a municipal generation and control site. Its load charts
  are the only surviving record of how the old grid balanced supply and demand.
- **The Intertie** was one of several regional links; two cables survive, and the
  neighbors who own the other end have their own priorities.
- **The Mirror Field** was a municipal solar thermal plant, stripped for parts and
  partially repaired with whatever was salvageable.
- **The Long Trench** carries the main cable between the shelter and the field; it
  was buried because of weather, and now it fails because of water.
- **The Cell Yard** was a battery recycling site; its racks are a salvage bonanza
  and a fire hazard, in that order.

No real utility, grid, or program is referenced. The Light Before is fictional and
exists to explain why the wasteland's grids are partially repairable rather than
gone.

---

## 49. APPENDIX AI — WORKED BALANCE NUMBERS

| Quantity | Value | Rationale |
|---|---|---|
| Base generation default | 800 W | live system default |
| Base battery default | 4000 Wh | live system default |
| Critical load floor | 400 W | keeps air, water, clinic |
| Winter load peak | 1,800 W | forces shedding |
| Summer load typical | 900 W | comfortable |
| Reserve target | 24 h | authored doctrine |
| Deep reserve target | 48 h | hard doctrine |
| Brownout threshold | 0.9 | live subgrid threshold |
| Cascade chance per trip | authored 0.05 | rare |
| Black start duration | 4 h | authored |
| Fuel days per drum | 3–5 | authored by source |
| Intertie toll | 10% | authored |
| EMP hardening budget cap | 20% of materials | authored |

These numbers are the expansion's skeleton. They are deliberately tight enough that
the player must choose and loose enough that the shelter can survive a mistake.

---

## 50. APPENDIX AJ — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Blackout kills directly | removes agency | route through needs/medical |
| One source dominates | removes choice | diversify profiles |
| Storage trivializes night | removes tension | losses and hazards |
| Fuel is infinite | removes logistics | finite chains |
| Intertie is free | removes diplomacy | toll and terms |
| Priority is invisible | removes politics | explicit board |
| Restoration is instant | removes consequence | authored sequence |
| EMP is spammable | removes preparedness | low cadence, hardening |
| Cold has no cost | removes stakes | exposure and crop loss |
| Heat is wasted | removes optimization | waste-heat reuse |

---

## 51. APPENDIX AK — MEASUREMENT AND REVIEW CADENCE

| Gate | Question | Evidence |
|---|---|---|
| Premise | Do the live owners still match? | file:line audit |
| Data | Are all rows valid and reachable? | integrity + scanner |
| Core | Are the systems pure and deterministic? | unit tests |
| Persistence | Does legacy load neutral? | round-trip tests |
| Host | Is every command real? | selftest + journey |
| UI | Is the priority board honest? | lifecycle + a11y tests |
| Balance | Is sensation of scarcity real? | soak results |
| Audit | Is any content gratuitous? | review checklist |

The balance gate is the hardest: the grid must feel scarce without feeling unfair,
and the only way to know is a long soak.

---

## 52. CLOSING STATEMENT

ASHFALL already gives the shelter a real electrical spine: one deterministic grid
authority, breakers and transformers that fail, fuel cells and flywheels and solar
concentrators, and typed events that everything else can hear. What it lacks is the
society around that spine: who gets power when there is not enough, how a fault
cascades, how fuel reaches a generator, how storage is disciplined, how an EMP is
prepared for, and how two settlements can share a grid without sharing a war. The
Grid adds that society without adding a second power system. It adds a priority
board, a reserve doctrine, an intertie treaty, and the oldest question in a cold
shelter: who gets to be warm?

> Wave 2 note: this plan is one of five Wave 2 expansion bibles (17–21). Each is
> self-contained; none requires another to ship. The shared Wave 2 index lives at
> `docs/expansions/wave2/WAVE2_INDEX.md`. The safe pre-signature step is Phase 1
> (data schemas and validators), which is additive and reversible.