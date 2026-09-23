# ASHFALL — Expansion 40 Design Bible
# THE WHEEL
### Wave 6 · Mechanical Power, Machine Tools, Drivelines, and Precision Work

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-23
**Domain owners touched:** `Ashfall.Core.Shelter` (KineticStorageSystem, ShelterWorkshopSystem, PrecisionMetrologySystem, PrecisionBroachingCatalog, CupolaFoundryEngine), `MachineIdentity`
**Proposed host owner:** `WorksPowerHostSession` (extends power, workshop, and calibration surfaces)
**Existing save sections:** flywheel state, workshop job state, metrology state, machine identities
**Existing CLI verbs:** `--workshop-selftest` (if present), `--data-integrity-selftest`, `--content-utilization-selftest`
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already has rotating mass and workshops. `KineticStorageSystem` defines
`KineticFlywheelCatalog` (`flywheel_classes`, `surge_events`, `black_start`,
`containment_hazard`) and `FlywheelClassDef` (`flywheel_id`, `display_name`,
`rotor_mass_kg`, `effective_radius_m`, `moment_of_inertia_factor`).
`ShelterWorkshopSystem` defines `WorkshopJobKind`, `WorkshopJobStatus`,
`WorkshopRecipeInput`, `WorkshopRecipeOutput`, and `WorkshopRecipeDefinition`
(`Id`, `DisplayName`, `Category`, and inputs/outputs).
`PrecisionMetrologySystem` defines `PrecisionCalibrationGrade`, `MetrologyGradeDef`
(`grade_id`, `display_name`, `ordinal`, `tooling_calibration`, `drift_per_day`),
and `MetrologyStandardDef` (`standard_id`, `item_id`, `max_grade`,
`required_room_ids`, `calibration_labor_ticks`).
`PrecisionBroachingCatalog` defines `BroachBenchDef` (`bench_id`, `display_name`,
`construction_required_items`, `construction_labor_days`, `max_condition`,
`maintenance_interval_days`, `maintenance_required_items`, `room_id`,
`machine_load_class`). `MachineIdentity` and `shelter_machine_identities.json`
record per-machine identity, quirks, and glitch events.

What does not exist: water wheels, windmills, shafting, belts, gears, machine
tools as content, lubrication, maintenance schedules, mechanical power budgets,
mill work, saw pits, and the trades that keep rotation honest. Machines have
identities and no drivelines.

**The Wheel** turns rotating power into a department: wheels and mills, shafts
and belts, gears and bearings, lathes and drills, calibration and lubrication,
maintenance and repair. It extends the live flywheel, workshop, and metrology
owners and hands power accounting to the grid owner.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

### 0.1 Boundary contracts

| Neighbour | Owns | This plan does |
|---|---|---|
| 21 The Grid | Electrical power, loads, storage | Mechanical power only; interfaces at the shaft |
| 10 The Silent Foundry | Smelting, alloys, casting | Orders castings, never makes metal |
| 29 The Glass | Optics and lab glass | Orders bearings, gauges, and windows |
| 31 The Kiln | Ceramics, refractories, lime | Orders millstones, bearings, and bricks |
| 39 The Reagent | Lubricant chemistry | Orders grease and oil, never makes it |
| 03 Standing Record | Records and archives | Files inspection and calibration records |
| 23 The Alarm | Fire and emergency | Uses its drills for machine fires |
| 02 Duty Roster | Shifts | Staffs mill and workshop turns |

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

The shelter has a flywheel, a workshop, and no way to turn anything. Every
machine is a hand tool, and the saw pit is a person with a saw.

**The Wheel** is the expansion about mechanical power: the water wheel on the
stream, the windmill on the ridge, the shafting and belts that carry rotation to
the lathe and the saw, the gears that trade speed for force, and the boring
ordinary discipline of grease, alignment, and maintenance. It gives the shelter
a machine shop that makes parts for every other department.

### 1.2 The five loops it adds

```
  Water/Wind ──► Shaft ──► Gear ──► Belt ──► Machine
       │           │         │        │          │
       ▼           ▼         ▼        ▼          ▼
   Wheels,      Line      Ratios,  Drives,   Lathe, saw,
   flow, wind   shafting  bearings couplings drill, press
                                                │
                                                ▼
                                          Calibration ──► Parts ──► Every wave
```

### 1.3 What the player manages

1. **Sites.** Streams, weirs, races, ridge towers, and foundations.
2. **Wheels.** Water wheels, windmills, and their condition.
3. **Drivelines.** Shafts, couplings, bearings, and alignment.
4. **Gearing.** Ratios, teeth, wear, and speed-for-force trades.
5. **Belts.** Leather, rope, and canvas drives with tension and slip.
6. **Machines.** Lathes, drills, saws, presses, hammers, and broach benches.
7. **Lubrication.** Oils, greases, and the wear they prevent.
8. **Calibration.** Gauges, standards, grades, and drift.
9. **Maintenance.** Schedules, inspections, spares, and rebuilds.
10. **Work.** Mill batches, sawn stock, bored parts, and the parts every other
    wave depends on.

### 1.4 What it is not

- Not a second power grid. Electrical generation stays with 21; this expansion
  ends at the shaft and the belt.
- Not a second workshop or metrology system. It extends the live owners.
- Not perpetual motion. Every wheel has a head or a wind, every belt slips, and
  every bearing wears out.
- Not a factory fantasy. The shelter gets one or two honest machines and the
  parts they make.
- Not a new save section.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Shelter/KineticStorageSystem.cs` | Flywheels, surge, black start | `LIVE` |
| `Assets/Ashfall.Core/Shelter/ShelterWorkshopSystem.cs` | Workshop jobs and recipes | `LIVE` |
| `Assets/Ashfall.Core/Shelter/PrecisionMetrologySystem.cs` | Grades and standards | `LIVE` |
| `Assets/Ashfall.Core/Shelter/PrecisionBroachingCatalog.cs` | Broach benches | `LIVE` |
| `Assets/Ashfall.Core/Shelter/CupolaFoundryEngine.cs` | Foundry castings | `LIVE` |
| `Assets/Ashfall.Core/Shelter/PneumaticDispatchSystem.cs` | Pneumatics | `LIVE` |
| `MachineIdentity` / `ShelfMachineIdentity` | Machine quirks and history | `LIVE` |
| `Assets/Ashfall.Core/Inventory/Inventory.cs` | Parts and stock | `LIVE` |
| `Assets/Ashfall.Core/Needs/NeedsSystem.cs` | Fatigue and injury | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Size | Notes |
|---|---|---|
| `precision_broaching_catalog.json` | **1,739 B** | thin |
| `metrology_standards_catalog.json` | 3,143 B | grades + few standards |
| `kinetic_flywheel_catalog.json` | 5,984 B | flywheel classes |
| `workshop_recipes.json` | 12,281 B | recipe list |
| `shelter_machine_identities.json` | 24,630 B | identities, quirks |
| Water wheels, drivelines, gearing, machine tools, lubrication, maintenance | absent | confirmed none |

### 2.3 Confirmed gaps

- **GAP-40-1 — No water wheel or windmill content.**
- **GAP-40-2 — No driveline or shafting content.**
- **GAP-40-3 — No gearing or ratio content.**
- **GAP-40-4 — No belt or coupling content.**
- **GAP-40-5 — No machine tool content beyond broach benches.**
- **GAP-40-6 — No lubrication or wear content.**
- **GAP-40-7 — No maintenance schedule content.**
- **GAP-40-8 — No mechanical power budget.**
- **GAP-40-9 — No mill work or saw pit content.**
- **GAP-40-10 — Calibration standards are a 3 KB stub.**

### 2.4 Non-duplication statement

This expansion will **not** add a second power, workshop, metrology, foundry, or
pneumatic system. It extends `KineticStorageSystem` with flywheel content,
`ShelterWorkshopSystem` with machine jobs, `PrecisionMetrologySystem` with
standards and drift, `PrecisionBroachingCatalog` with benches, and routes
electrical interfaces to the grid owner. It adds state only as additive
sub-objects of the existing flywheel, workshop, and metrology stores. No new
save section.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Power is a budget with a source.** Every turn comes from water,
wind, or muscle, and the ledger says which.

**Pillar 2 — Alignment is sacred.** Most machine failure is a shaft out of true
and a bearing run dry.

**Pillar 3 — Precision is a service.** The machine shop exists to make parts for
every other department.

**Pillar 4 — Maintenance beats repair.** A schedule is cheaper than a rebuild.

**Pillar 5 — A machine has a character.** Identities, quirks, and histories make
iron feel lived-in, never haunted.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| Water wheel | Weather, flow, patience | Picturesque nostalgia |
| Mill work | Batch discipline | Flour-dust romance |
| Gears | Ratios and wear | Steampunk decoration |
| Bearings | Grease and fit | Mystical precision |
| Calibration | Standards and drift | Magic gauges |
| Breakdown | Cause and rebuild | Explosive drama |
| Machine identity | A named lathe with habits | Haunted machines |
| Parts | Service to all waves | Self-serving industry |

### 3.3 Content limits

- No child operation of machinery; apprentices are 16+ and supervised.
- No gore; injuries are consequences with medical routing.
- No magic or haunted machines; quirks are mechanical and documented.
- No perpetual motion, no over-unity, no free power.
- No steam-era nationalism or industrial triumphalism; machines are tools.
- No unsafe practices presented as clever or admirable.

---

## 4. THE WHEEL WORLD

### 4.1 Interior rooms

- **`room_wheel_house`** — the wheel, the race, and the sluice controls.
- **`room_lineshaft`** — the shaft alley with belts and guards.
- **`room_machine_shop`** — lathe, drill, and benches.
- **`room_saw_shed`** — the saw frame and the timber rack.
- **`room_mill_room`** — stones, sifter, and flour bins.
- **`room_gauge_room`** — standards, gauges, and the calibration log.
- **`room_grease_store`** — oils, greases, and rags.
- **`room_spares_rack`** — bearings, belts, and castings.

### 4.2 Exterior locations

| ID | Name | Tone | Purpose |
|---|---|---|---|
| `loc_weir` | The Weir | 3 | Head and flow control |
| `loc_mill_race` | The Mill Race | 2 | Water delivery |
| `loc_ridge_tower` | The Ridge Tower | 3 | Wind power |
| `loc_saw_pit` | The Saw Pit | 3 | Historic hand work |
| `loc_timber_yard` | The Timber Yard | 2 | Stock and drying |
| `loc_bearing_heap` | The Bearing Heap | 2 | Salvage and sorting |
| `loc_shaft_yard` | The Shaft Yard | 2 | Line shaft assembly |
| `loc_cooling_pool` | The Cooling Pool | 2 | Forge and quench |
| `loc_mill_track` | The Mill Track | 3 | Handcart route |
| `loc_foundry_gate` | The Foundry Gate | 3 | Casting deliveries |

All locations require valid item references and scanner registration.

### 4.3 The turn

Wheels turn with water and wind; shafts carry it; belts spend it; machines use
it. The expansion's clock is the turn, and the shop logs every one.

---

## 5. MAIN STORYLINE — "THE FIRST TRUE SHAFT"

### 5.1 Central conflict

The mill race runs all winter and turns nothing. **Orth Garrow** the miller
wants stones turning before the grain spoils. **Jessa Crane** the mechanic wants
a line shaft, bearings, and a lathe that can make its own replacement parts.
**Thena** the wind keeper wants a ridge tower for calm days. **Odo** the bearing
maker wants fit and clearance respected. **Baro** the shaft smith wants a forge
that can straighten a shaft without cracking it. **Vale** the metrologist wants
gauges that agree with each other, because a machine shop that cannot measure
cannot fix anything.

Then the first shaft whips, a bearing runs hot, and the shelter learns that
rotation is a discipline. The expansion's question: **can a shelter build a
machine that outlives the person who made it?**

### 5.2 Theme (unspoken)

**Precision is how a community says it intends to last.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_miller_orth_garrow` | Orth Garrow | Miller | Mill batches and stones |
| `npc_mechanic_jessa_crane` | Jessa Crane | Mechanic | Drivelines and alignment |
| `npc_sawyer_burl` | Burl | Sawyer | Saw frame and timber |
| `npc_metrologist_vale` | Vale | Metrologist | Gauges, grades, drift |
| `npc_apprentice_nim` | Nim | Apprentice | Learning, greasing, checks |
| `npc_bearing_maker_odo` | Odo | Bearing maker | Fit, clearance, bronze |
| `npc_wind_keeper_thena` | Thena | Wind keeper | Ridge tower and sails |
| `npc_shaft_smith_baro` | Baro | Shaft smith | Straightening and forging |

### 5.4 Story beats (15)

1. **The Idle Race.** Water runs and nothing turns.
2. **The Wheel.** A wheel is built and hung.
3. **The Shaft.** The first line shaft is assembled and aligned.
4. **The Whip.** The shaft runs true, then doesn't, and the shelter learns why.
5. **The Bearings.** Odo fits the first bronze bearings.
6. **The Belt.** A leather belt transmits power and teaches tension.
7. **The Stones.** The mill turns grain into flour for the first time.
8. **The Lathe.** A lathe is built to make its own parts.
9. **The Gauges.** Vale calibrates the shop's measuring tools.
10. **The Grease.** A lubrication round becomes a job.
11. **The Saw.** The saw frame replaces the pit.
12. **The Tower.** The windmill turns on calm days.
13. **The Breakdown.** A bearing seizes and the shop rebuilds it.
14. **The Schedule.** Maintenance becomes a calendar, not a rescue.
15. **The First True Shaft.** The machine shop serves every wave.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Power source | water / wind / both | reliability vs. cost |
| Driveline | single shaft / split / belts only | simplicity vs. reach |
| Gearing | wooden / bronze / mixed | repairable vs. durable |
| Calibration | field / shop / reference | speed vs. precision |
| Maintenance | reactive / scheduled / strict | labor vs. uptime |
| Mill | one stone / two / trade | output vs. grain |
| Saw | pit / frame / mill | labor vs. capacity |
| Final | shop as institution / practice / memory | identity |

### 5.6 Endings (5 + fade)

1. **The Turning Works** — wheel, shaft, belts, and machines run as one system.
2. **The Reference Shop** — calibration culture makes the shop the shelter's
   center of precision.
3. **The Patient Mill** — the mill runs slow and never fails during harvest.
4. **The Bronze Year** — the shelter invests in bearings and gears and the
   machines outlive their makers.
5. **The Dry Season** — the stream drops, the wheel stops, and the shelter
   learns that power is a season, not a possession.
6. **Fade** — a lathe turning in a warm shop while the rain fills the race.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_wheel_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_wheel_idle_race`, `quest_wheel_wheel`, `quest_wheel_shaft`,
`quest_wheel_whip`, `quest_wheel_bearings`, `quest_wheel_belt`,
`quest_wheel_stones`, `quest_wheel_lathe`, `quest_wheel_gauges`,
`quest_wheel_grease`, `quest_wheel_saw`, `quest_wheel_tower`,
`quest_wheel_breakdown`, `quest_wheel_schedule`, `quest_wheel_first_shaft`.

### 6.2 Side quests (30)

**Water (5)**
- `quest_wheel_weir` — build and set the weir
- `quest_wheel_race` — clear the race
- `quest_wheel_sluice` — control flow
- `quest_wheel_flood` — protect the wheel
- `quest_wheel_freeze` — winter operation

**Wind (5)**
- `quest_wheel_tower_build` — build the tower
- `quest_wheel_sails` — canvas the sails
- `quest_wheel_governor` — regulate speed
- `quest_wheel_storm` — furl in a gale
- `quest_wheel_bearing_wind` — wind-specific bearings

**Driveline (5)**
- `quest_wheel_align` — align a shaft
- `quest_wheel_couplings` — fit couplings
- `quest_wheel_belts` — lace and tension belts
- `quest_wheel_guards` — guard the line shaft
- `quest_wheel_flywheel` — connect the flywheel

**Machines (5)**
- `quest_wheel_lathe_build` — build a lathe
- `quest_wheel_drill` — build a drill
- `quest_wheel_press` — build a press
- `quest_wheel_hammer` — build a trip hammer
- `quest_wheel_broach` — set up a broach bench

**Precision (5)**
- `quest_wheel_gauge` — make gauge blocks
- `quest_wheel_calibrate` — calibrate tooling
- `quest_wheel_drift` — track drift
- `quest_wheel_standard` — set a shop standard
- `quest_wheel_log` — keep the calibration log

**Work (5)**
- `quest_wheel_mill_batch` — run a mill batch
- `quest_wheel_planks` — saw timber
- `quest_wheel_parts` — make service parts
- `quest_wheel_lubricate` — the grease round
- `quest_wheel_inspect` — weekly inspection

### 6.3 Repeatable quests (8)

`quest_wheel_repeat_grease`, `quest_wheel_repeat_align`,
`quest_wheel_repeat_calibrate`, `quest_wheel_repeat_mill`,
`quest_wheel_repeat_saw`, `quest_wheel_repeat_parts`,
`quest_wheel_repeat_inspect`, `quest_wheel_repeat_clear`.

### 6.4 Dynamic hooks

Live events (weather, flood, grain supply, foundry deliveries, power brownouts,
machine quirks, injuries) attach authored follow-ups through existing seams. No
new event bus.

### 6.5 Constraints

- Flywheel, surge, and black start stay with `KineticStorageSystem`.
- Electrical interfaces stay with the grid owner.
- Castings stay with the foundry; the shop machines them.
- Metrology stays with `PrecisionMetrologySystem`.
- Machine identity and quirks stay with `MachineIdentity`.
- No new save section.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `MechanicalPowerSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** wheel sites, water and wind sources, shafting, gearing, belts, and the
mechanical power budget. **Consumes:** `KineticStorageSystem`, weather and
season data, `ShelterWorkshopSystem`, `PowerGridSystem` (interface only).
**Data:** `mechanical_power.json`, `wheel_sites.json`, `drivelines.json`,
`gearing.json`. **Rules:** every turn has a source and a budget; head, flow, and
wind produce real output; losses through shafts, gears, and belts are accounted;
the system never creates power from nothing.

### 7.2 `MachineToolSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** lathes, drills, saws, presses, hammers, and broach benches as
machines. **Consumes:** `ShelterWorkshopSystem` (jobs and recipes),
`PrecisionBroachingCatalog`, `MachineIdentity` (quirks), `MechanicalPowerSystem`
(capacity). **Data:** `machine_tools.json`. **Rules:** a machine has capacity,
condition, quirks, and consumables; jobs schedule against capacity; a machine
without power idles honestly.

### 7.3 `CalibrationSystem` (extend `PrecisionMetrologySystem`)

**Owns:** gauge sets, drift tracking, standards, and calibration records.
**Consumes:** the live grades and standards, `MachineToolSystem`, records
owners. **Data:** `tooling_grades.json`, extends
`metrology_standards_catalog.json`. **Rules:** a tool at a higher grade cannot
be produced by a lower-grade shop without a reference standard; drift is
recorded daily; calibration is labor, not a menu click.

### 7.4 `LubricationSystem` (new, thin, `Ashfall.Core.Shelter`)

**Owns:** grease points, lubricant types, rounds, and wear rates.
**Consumes:** `ChemicalWorksSystem` (Wave 6, grease supply),
`MachineToolSystem`, `MechanicalPowerSystem`. **Data:** `lubrication.json`.
**Rules:** every bearing has a lubricant and an interval; missed rounds show up
as wear; the grease round is the cheapest maintenance in the shelter.

### 7.5 `MachineMaintenanceSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** schedules, inspections, spares, and rebuilds. **Consumes:**
`MachineToolSystem`, `MechanicalPowerSystem`, `ShelterWorkshopSystem`,
`CupolaFoundryEngine` (castings), `Inventory`. **Data:**
`maintenance_schedules.json`. **Rules:** every machine has a schedule; a missed
service raises wear and risk visibly; rebuilds consume real parts made by the
shop itself.

### 7.6 `MillWorkSystem` (new, thin, `Ashfall.Core.Shelter`)

**Owns:** mill batches, saw runs, and stock preparation. **Consumes:**
`MechanicalPowerSystem`, `MachineToolSystem`, `GrainProcessingSystem`
(existing), `Inventory`, `FoodPreservationSystem` (Wave 3).
**Data:** `mill_batches.json`. **Rules:** grain and timber have real conversion
rates; the mill and saw log their runs; output routes to the live food and
build owners.

### 7.7 Systems explicitly not added

- No second power grid, workshop, metrology, foundry, or pneumatics system.
- No perpetual motion or over-unity.
- No child machine operators.
- No new currency.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `mechanical_power.json` (new)

```json
{
  "schema_version": 1,
  "power_budget": {
    "id": "power_west_race",
    "source": "water",
    "head_m": 2.5,
    "flow_lps": 180,
    "wheel_efficiency": 0.35,
    "shaft_loss": 0.06,
    "gear_loss": 0.08,
    "belt_loss": 0.10,
    "output_units": "turns_per_minute",
    "tags": ["wheel", "race"]
  }
}
```

### 8.2 `wheel_sites.json` (new)

Sites: stream, head, foundation, wheel type, condition, seasonal risk.

### 8.3 `drivelines.json` (new)

Lines: shafts, lengths, supports, couplings, alignment, guards.

### 8.4 `gearing.json` (new)

Gears: pairs, ratios, material, teeth, wear, replacement.

### 8.5 `machine_tools.json` (new)

Machines: type, build, capacity, condition, quirks, consumables, power draw.

### 8.6 `tooling_grades.json` (new)

Grades: field, shop, reference; checks, drift, allowed work.

### 8.7 `lubrication.json` (new)

Lubricants: type, points, interval, effect, shortage consequence.

### 8.8 `maintenance_schedules.json` (new)

Schedules: machine, interval, tasks, spares, sign-off.

### 8.9 `mill_batches.json` (new)

Batches: input, rate, output, waste, quality, runtime.

### 8.10 Extensions

Extend `kinetic_flywheel_catalog.json`, `workshop_recipes.json`,
`precision_broaching_catalog.json`, and `metrology_standards_catalog.json` with
flywheels, machine jobs, benches, standards, and grades.

### 8.11 Items

New items appended to `items.json`: `item_wheel_hub`, `item_shaft_section`,
`item_gear_set`, `item_belt_leather`, `item_bearing_bronze`, `item_grease_tin`,
`item_oil_can`, `item_lathe_tool`, `item_drill_bit`, `item_saw_blade`,
`item_hammer_head`, `item_mill_stone`, `item_governor`, `item_gauge_block`,
`item_calipers`, `item_spanner_set`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

Flywheel state, workshop job state, metrology state, and machine identities
remain the live save owners. New sub-objects (sites, drivelines, gearing,
machines, calibration, lubrication, schedules, batches) are additive inside
them. No new save section.

### 9.2 State to persist

- Wheel sites, wheel condition, and seasonal state.
- Driveline layout, alignment, and guards.
- Gearing pairs and wear.
- Machine condition, quirks, and consumables.
- Gauge calibration and drift.
- Lubrication rounds and shortages.
- Maintenance schedules and histories.
- Mill and saw batch records.

### 9.3 Determinism

- Power output derives from authored head, flow, wind, and losses.
- Machine jobs resolve through the live workshop scheduler.
- Wear and drift advance deterministically from use and intervals.
- Breakdowns derive from neglected maintenance and condition, using the live
  seeded paths only where already used.
- Paired replay hashes must match; no wall-clock or `System.Random`.

### 9.4 Migration

Legacy saves load with flywheel, workshop, metrology, and machine identity state
untouched; no sites, drivelines, or schedules exist until started. Existing
recipes and benches keep working; authored content adds to them.

### 9.5 Checksum

Invariant-culture floats; integer revolution, hour, and part counts.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `PowerPanel` (new) | Sites, budgets, losses | `WorksPowerHostSession` |
| `DrivelinePanel` (new) | Shafts, gears, belts | same |
| `MachinePanel` (new) | Machines, condition, jobs | same |
| `CalibrationPanel` (new) | Grades, gauges, drift | same |
| `LubricationPanel` (new) | Rounds and points | same |
| `MaintenancePanel` (new) | Schedules and spares | same |
| `MillPanel` (new) | Batches and output | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Power budgets are shown as a flow from source to machine, never a hidden
  number.
- Wear and drift are readable, with plain-language causes.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- Diagrams have text equivalents; nothing critical is spatial only.
- High-contrast machine boards for workshop use.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: water on the wheel, a belt slap, the
lathe's hum, a saw biting timber, stone on grain, the grease lid opening. No cue
is required; text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `KineticStorageSystem` | Flywheel content and surge |
| `ShelterWorkshopSystem` | Jobs and machines |
| `PrecisionMetrologySystem` | Grades, standards, drift |
| `PrecisionBroachingCatalog` | Broach benches |
| `MachineIdentity` | Quirks and machine histories |
| `CupolaFoundryEngine` | Castings |
| `PowerGridSystem` (Wave 2) | Electrical interface |
| `GrainProcessingSystem` | Mill conversion |
| `FoodPreservationSystem` (Wave 3) | Flour storage |
| `ChemicalWorksSystem` (Wave 6) | Grease and oil |
| `GlassworksHostSession` (Wave 4) | Gauges and windows |
| `KilnworksHostSession` (Wave 4) | Stones and bearings |
| `NeedsSystem` | Fatigue and injuries |
| `MedicalPipelineCoordinator` | Injury routing |
| `DutyRoster` (Exp 02) | Mill and shop shifts |
| `ArchiveDeskSystem` (Wave 4) | Calibration and maintenance records |
| `EpilogueChronicleBuilder` | Machine milestones |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm flywheel, workshop, metrology,
broaching, machine identity, and foundry owners. Record file:line; change
nothing.

**Phase 1 — Data + validators.** Author the nine catalogs; extend the four thin
catalogs; append items; register validators and scanner.

**Phase 2 — Pure Core.** `MechanicalPowerSystem`, `MachineToolSystem`,
`CalibrationSystem`, `LubricationSystem`, `MachineMaintenanceSystem`,
`MillWorkSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip,
determinism.

**Phase 4 — Host + CLI.** `WorksPowerHostSession`, selftest coverage, fresh
journey from idle race to first true shaft.

**Phase 5 — UI.** New panels with lifecycle and accessibility.

**Phase 6 — Content.** Rooms, locations, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** 360-day soak: harvest mill, winter freeze, bearing
breakdown, and calibration drift.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Power sites | 8 |
| Drivelines | 10 |
| Gear pairs | 12 |
| Machines | 16 |
| Tooling grades | 6 |
| Lubricants | 8 |
| Maintenance schedules | 16 |
| Mill batches | 12 |
| Calibration standards | 12 |
| Items | 16 |
| Locations | 10 |
| Rooms | 8 |
| NPCs | 8 |
| Main quests | 15 |
| Side quests | 30 |
| Repeatable | 8 |
| Endings | 5 + fade |
| Prose estimate | 55,000–70,000 words |

### 12.3 Risks

| Risk | Severity | Mitigation |
|---|---|---|
| Perpetual motion | Critical | Authored sources only |
| Second power system | Critical | Grid interface only |
| Steam-punk tone | High | Workmanlike presentation |
| Hidden power | High | Visible budgets |
| Maintenance skipped | Medium | Visible wear |
| Child operators | Critical | 16+ supervised |
| Determinism break | Low | Live seeded paths |
| Content overrun | Medium | Budget §13 |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `mechanical_power.json` | 8 | 2,500 |
| `wheel_sites.json` | 8 | 2,500 |
| `drivelines.json` | 10 | 3,000 |
| `gearing.json` | 12 | 2,500 |
| `machine_tools.json` | 16 | 4,500 |
| `tooling_grades.json` | 6 | 1,500 |
| `lubrication.json` | 8 | 2,000 |
| `maintenance_schedules.json` | 16 | 3,500 |
| `mill_batches.json` | 12 | 2,500 |
| Catalog extensions | 40 | 8,000 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 16 | 2,500 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~63,500** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R40-1 | Perpetual motion | Low | Critical | Authored sources |
| R40-2 | Second power system | Low | Critical | Grid interface |
| R40-3 | Tone drift to steampunk | Med | High | Workmanlike art |
| R40-4 | Hidden power numbers | Med | High | Flow panel |
| R40-5 | Maintenance ignored | Med | High | Visible wear |
| R40-6 | Machine quirks as magic | Low | High | Documented mechanics |
| R40-7 | Child labor | Low | Critical | 16+ supervised |
| R40-8 | Determinism | Low | High | Live paths |
| R40-9 | Content overrun | Med | Med | Budget §13 |
| R40-10 | Shop vs. grid overlap | Med | Med | Boundary §0.1 |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Can the stream dry permanently?** Recommended: seasonal droughts yes,
   permanent failure no.
2. **Do wooden gears break before bronze?** Recommended: yes, often and
   repairably; the shelter learns from both.
3. **Is calibration a bottleneck or a courtesy?** Recommended: a real
   bottleneck for reference work and a courtesy for rough work.
4. **Can machines be salvaged from outside?** Recommended: yes, through the
   live salvage and machine identity systems, with quirks attached.
5. **Does the shop sell parts outside?** Recommended: yes, through live trade,
   with honest grades.

---

## 17. APPENDIX D — POWER SITE TABLE (8 SITES)

| # | Site | Source | Head | Season | Risk |
|---|---|---|---|---|---|
| 1 | West race | water | 2.5 m | steady | flood |
| 2 | North race | water | 1.8 m | steady | freeze |
| 3 | Ridge tower | wind | 6 m | variable | storm |
| 4 | Mill pond | water | 3.0 m | summer low | drought |
| 5 | South stream | water | 1.2 m | flashy | washout |
| 6 | Low tower | wind | 4 m | weak | little use |
| 7 | Tail race | water | 0 m | recovered | silting |
| 8 | Combined site | both | mixed | best | complexity |

Each site is a real place with a head, a season, and a failure mode. The table
is why the shelter's power conversation starts with a map of water instead of a
list of machines.

---

## 18. APPENDIX E — DRIVELINE TABLE (10 LINES)

| # | Line | Length | Supports | Coupling | Guards |
|---|---|---|---|---|---|
| 1 | Wheel to jack | 4 m | 2 | keyed | yes |
| 2 | Main alley | 12 m | 5 | flexible | yes |
| 3 | Mill drop | 3 m | 1 | crowned | yes |
| 4 | Lathe takeoff | 2 m | 1 | clutch | yes |
| 5 | Saw takeoff | 2 m | 1 | clutch | yes |
| 6 | Press takeoff | 2 m | 1 | cam | yes |
| 7 | Hammer line | 4 m | 2 | trip | yes |
| 8 | Broach line | 2 m | 1 | direct | yes |
| 9 | Flywheel tie | 3 m | 2 | geared | yes |
| 10 | Pump takeoff | 3 m | 1 | slip | yes |

Drivelines are the expansion's circulatory system. Every line has supports, a
coupling, and a guard, and the guard column is non-negotiable because a shaft
alley without guards is a place where people lose fingers.

---

## 19. APPENDIX F — GEARING TABLE (12 PAIRS)

| # | Pair | Ratio | Material | Teeth | Wear |
|---|---|---|---|---|---|
| 1 | Wheel to jack | 4:1 | wood | 48/12 | medium |
| 2 | Jack to line | 2:1 | wood | 32/16 | medium |
| 3 | Line to mill | 3:1 | bronze | 45/15 | low |
| 4 | Line to lathe | 2.5:1 | bronze | 40/16 | low |
| 5 | Line to saw | 1.5:1 | wood | 30/20 | high |
| 6 | Line to press | 5:1 | bronze | 50/10 | low |
| 7 | Line to hammer | 6:1 | wood | 48/8 | high |
| 8 | Line to broach | 8:1 | bronze | 48/6 | low |
| 9 | Flywheel tie | 1:1 | bronze | 36/36 | low |
| 10 | Pump drive | 3:1 | wood | 36/12 | medium |
| 11 | Governor take | 2:1 | bronze | 24/12 | low |
| 12 | Spare set | any | mixed | any | stock |

Gears trade speed for force, and the material column is the shelter's real
decision: wood is repairable and bronze is durable, and the shop needs both.
The two high-wear pairs are where the maintenance schedule earns its keep.

---

## 20. APPENDIX G — MACHINE TOOL TABLE (16 MACHINES)

| # | Machine | Build | Capacity | Power | Quirks |
|---|---|---|---|---|---|
| 1 | Lathe | bed, head, saddle | 400 mm | med | tailstock wander |
| 2 | Drill press | frame, quill | 20 mm | low | bearing noise |
| 3 | Saw frame | frame, crank | 600 mm | med | blade tension |
| 4 | Trip hammer | cam, helve | 40 kg | high | anvil ring |
| 5 | Press | screw, frame | 5 t | med | thread wear |
| 6 | Broach bench | frame, broach | keyways | low | alignment |
| 7 | Grinder | stone, frame | 300 mm | med | stone balance |
| 8 | Shears | blade, lever | 3 mm | low | blade gap |
| 9 | Planer | bed, gantry | 800 mm | high | feed slip |
| 10 | Mill | stones, hopper | 40 kg/h | high | stone dressing |
| 11 | Sifter | screens, crank | 20 kg/h | low | screen tear |
| 12 | Pump | cylinder, piston | 100 L/h | low | seal wear |
| 13 | Blower | fan, housing | air | med | balance |
| 14 | Winch | drum, brake | 500 kg | low | brake wear |
| 15 | Boring mill | frame, bar | 200 mm | high | bar sag |
| 16 | Slotter | ram, crank | keyways | med | ram play |

Sixteen machines make the shop a service department. Each one has a capacity, a
power draw, and a named quirk, and the quirks are what the operators talk about
at lunch.

---

## 21. APPENDIX H — TOOLING GRADE TABLE

| Grade | Calibration | Drift per day | Allowed work | Check |
|---|---|---|---|---|
| Uncalibrated | 0.00 | 0.00 | rough only | none |
| Field | 0.35 | 0.02 | farm and timber | weekly |
| Shop | 0.55 | 0.01 | general machine work | daily |
| Reference | 0.85 | 0.004 | standards and gauges | per use |
| Master | 0.98 | 0.001 | reference only | sealed |
| Room | environmental | n/a | lab work | logged |

Grades are a ladder of trust, not a purchase tier. The reference grade is a
bottleneck on purpose: a shelter can make one really accurate thing and must
then use it carefully, which is exactly how real machine shops work.

---

## 22. APPENDIX I — LUBRICATION TABLE

| Lubricant | Points | Interval | Effect | Shortage |
|---|---|---|---|---|
| Grease | bearings | 3 days | wear slow | heat |
| Light oil | spindles | daily | smooth | drag |
| Heavy oil | gears | weekly | quiet | wear |
| Tallow | wood gears | weekly | slick | friction |
| Graphite | dry slides | monthly | slip | stick |
| Soap dope | water pumps | weekly | seal | leak |
| Storm oil | wind bearings | weekly | protect | corrosion |
| Spare tin | stores | restock | readiness | none |

Lubrication is the cheapest and most neglected maintenance in any workshop. The
table exists so the player can see the difference between a machine with a
grease round and a machine with a story about the day it seized.

---

## 23. APPENDIX J — MAINTENANCE SCHEDULE TABLE

| Machine | Daily | Weekly | Monthly | Season | Rebuild |
|---|---|---|---|---|---|
| Lathe | oil, wipe | align | bearing check | re-scrape | 2 years |
| Drill | oil | belt | quill | bearing | 3 years |
| Saw | tension | teeth | guide | blade | yearly |
| Hammer | check | pins | anvil | cam | 2 years |
| Press | oil | thread | frame | screw | 2 years |
| Broach | clean | align | broach | bed | 2 years |
| Grinder | balance | stone | mount | dress | yearly |
| Shears | gap | blade | lever | sharpen | yearly |
| Planer | oil | feed | gantry | bed | 3 years |
| Mill | dress | stones | shoe | re-pick | yearly |
| Sifter | screens | crank | frame | cloth | yearly |
| Pump | seal | valve | rod | re-pack | 2 years |
| Blower | balance | bearing | housing | clean | 2 years |
| Winch | brake | rope | drum | re-rope | yearly |
| Boring | oil | bar | bearing | re-scrape | 2 years |
| Slotter | oil | ram | crank | adjust | 2 years |

The schedule is the shop's promise that machines are maintained before they
fail. Every row has a rebuild horizon, which means the shop plans its own spare
parts years in advance.

---

## 24. APPENDIX K — MILL BATCH TABLE

| Batch | Input | Rate | Output | Waste | Quality |
|---|---|---|---|---|---|
| Wheat fine | wheat | 40 kg/h | flour 34 | bran 6 | high |
| Wheat course | wheat | 60 kg/h | meal 50 | bran 10 | med |
| Barley | barley | 40 kg/h | flour 30 | bran 10 | med |
| Oats | oats | 35 kg/h | meal 28 | husk 7 | med |
| Rye | rye | 38 kg/h | flour 31 | bran 7 | med |
| Mixed | blend | 50 kg/h | meal 40 | bran 10 | low |
| Seed clean | seed | 20 kg/h | cleaned 18 | chaff 2 | high |
| Feed crush | feed | 80 kg/h | crush 74 | dust 6 | low |
| Saw soft | softwood | 8 planks/h | planks | sawdust | med |
| Saw hard | hardwood | 4 planks/h | planks | sawdust | high |
| Saw beams | timber | 2 beams/h | beams | slabs | med |
| Saw rounds | logs | 10 rounds/h | rounds | bark | low |

Mill and saw batches give the wheel something to do that the whole shelter
feels. The waste column is deliberate: even the best process produces something
the player has to route.

---

## 25. APPENDIX L — CALIBRATION STANDARD TABLE

| Standard | Used for | Grade | Room | Labor |
|---|---|---|---|---|
| Gauge block set | measuring | reference | gauge room | 30 |
| Master rule | lengths | reference | gauge room | 20 |
| Square | right angles | shop | machine shop | 15 |
| Level | alignment | shop | lineshaft | 15 |
| Surface plate | flatness | reference | gauge room | 40 |
| Thread gauge | screws | shop | machine shop | 20 |
| Bore gauge | holes | reference | machine shop | 25 |
| Temperature rule | shop temp | field | shop | 5 |
| Tension gauge | belts | field | lineshaft | 5 |
| Wear pin | bearings | shop | lineshaft | 10 |
| Stone dress | millstones | field | mill room | 10 |
| Balance stand | rotating | shop | machine shop | 20 |

Standards are how the shop agrees with itself. The labor column is the honest
cost of precision: calibration is time, and the shelter either spends it or
accepts drift.

---

## 26. APPENDIX M — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_wheel_idle_race` | 4 | Water runs unused |
| `quest_wheel_wheel` | 5 | Wheel built and hung |
| `quest_wheel_shaft` | 5 | First line shaft aligned |
| `quest_wheel_whip` | 4 | Whip diagnosed |
| `quest_wheel_bearings` | 4 | Bronze bearings fitted |
| `quest_wheel_belt` | 4 | Belt transmits and slips |
| `quest_wheel_stones` | 5 | First flour milled |
| `quest_wheel_lathe` | 5 | Lathe built |
| `quest_wheel_gauges` | 4 | Shop gauges calibrated |
| `quest_wheel_grease` | 3 | Grease round established |
| `quest_wheel_saw` | 4 | Saw frame working |
| `quest_wheel_tower` | 5 | Wind tower turns |
| `quest_wheel_breakdown` | 5 | Seizure and rebuild |
| `quest_wheel_schedule` | 4 | Maintenance calendar |
| `quest_wheel_first_shaft` | 3 | Final disposition |

---

## 27. APPENDIX N — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_wheel_weir` | 4 | Weir built and set |
| `quest_wheel_race` | 3 | Race cleared |
| `quest_wheel_sluice` | 3 | Flow controlled |
| `quest_wheel_flood` | 4 | Flood plan |
| `quest_wheel_freeze` | 3 | Winter running |
| `quest_wheel_tower_build` | 5 | Tower built |
| `quest_wheel_sails` | 3 | Sails canvassed |
| `quest_wheel_governor` | 4 | Speed regulated |
| `quest_wheel_storm` | 3 | Gale furl |
| `quest_wheel_bearing_wind` | 3 | Wind bearings |
| `quest_wheel_align` | 4 | Alignment corrected |
| `quest_wheel_couplings` | 3 | Couplings fitted |
| `quest_wheel_belts` | 4 | Belts laced |
| `quest_wheel_guards` | 3 | Guards installed |
| `quest_wheel_flywheel` | 4 | Flywheel tied in |
| `quest_wheel_lathe_build` | 5 | Lathe built |
| `quest_wheel_drill` | 4 | Drill built |
| `quest_wheel_press` | 4 | Press built |
| `quest_wheel_hammer` | 5 | Hammer built |
| `quest_wheel_broach` | 3 | Broach bench set |
| `quest_wheel_gauge` | 4 | Gauge blocks made |
| `quest_wheel_calibrate` | 3 | Tooling calibrated |
| `quest_wheel_drift` | 3 | Drift tracked |
| `quest_wheel_standard` | 3 | Standard posted |
| `quest_wheel_log` | 3 | Log kept |
| `quest_wheel_mill_batch` | 3 | Batch milled |
| `quest_wheel_planks` | 3 | Timber sawn |
| `quest_wheel_parts` | 4 | Parts delivered |
| `quest_wheel_lubricate` | 3 | Round completed |
| `quest_wheel_inspect` | 4 | Inspection signed |

---

## 28. APPENDIX O — NPC DOSSIERS (BRIEF)

**Orth Garrow** — miller. Talks about grain the way other people talk about
weather. Wants stones turning by harvest and a schedule that respects the
season.

**Jessa Crane** — mechanic. Believes alignment is a moral quality. Carries a
level everywhere and checks a shaft with her hand before she trusts a gauge.

**Burl** — sawyer. Came from the pit and respects the frame but misses the
rhythm of two people in a hole. Keeps his blades sharp and his arguments short.

**Vale** — metrologist. Steward of the gauge room, enemy of drift, and the only
person who can explain why a warm day changes a measurement.

**Nim** — apprentice. Sixteen, supervised, and assigned to the grease round
because it teaches every bearing in the shop by name.

**Odo** — bearing maker. Fits bronze the way a tailor fits a coat and argues
that clearance is not a number but a relationship.

**Thena** — wind keeper. Reads the ridge and furls early. Mocks no one and takes
no chances with a gale.

**Baro** — shaft smith. Straightens forged shafts without cracking them and
believes a good heat is better than a heavy hammer.

---

## 29. APPENDIX P — LOCATION DETAIL

- **The Weir** — a wall of stone and wood that decides the whole shop's day.
- **The Mill Race** — clear water, and the reason the shelter eats bread.
- **The Ridge Tower** — wind, and the patience to wait for it.
- **The Saw Pit** — the old way, kept as a benchmark and a backup.
- **The Timber Yard** — drying stacks and the smell of fresh-cut pine.
- **The Bearing Heap** — salvage sorted by size, the shop's true treasury.
- **The Shaft Yard** — alignment frames and chalk marks.
- **The Cooling Pool** — heat, steam, and the sound of a quench.
- **The Mill Track** — handcarts of grain in and flour out.
- **The Foundry Gate** — where castings arrive and are counted twice.

---

## 30. APPENDIX Q — POWER LOSS TABLE (WORKED EXAMPLE)

| Stage | Available | Loss | Remaining |
|---|---|---|---|
| Head and flow | 100 | 0 | 100 |
| Wheel | 100 | 65 | 35 |
| Wheel to jack | 35 | 2 | 33 |
| Jack gears | 33 | 3 | 30 |
| Main shaft | 30 | 4 | 26 |
| Mill gears | 26 | 3 | 23 |
| Belt | 23 | 2 | 21 |
| Mill stones | 21 | 5 | 16 useful |
| Sifter | 16 | 2 | 14 useful |
| Net | 14 | — | 14 of 100 |

The table is deliberately unromantic. A water wheel that turns one hundred
units of water into fourteen units of flour is a good wheel, and the shelter
that understands this builds its next wheel better instead of expecting more.

---

## 31. APPENDIX R — WORKED 360-DAY WHEEL SCENARIO

**Days 1–20.** The race runs idle; Orth measures the head; Jessa surveys the
foundation; they agree on a wheel.

**Days 21–50.** Wheel built and hung; first turn under water; the jack shaft is
set and coupled; the shelter gathers to watch it move.

**Days 51–80.** Line shaft assembled; alignment corrected twice; the whip is
diagnosed as bearing fit and shaft sag; Odo fits bronze bearings.

**Days 81–110.** Belt laced and tensioned; first load reaches the mill; the
stones turn; the first flour is ground and baked the same day.

**Days 111–140.** Lathe built from foundry castings; the shop makes its first
replacement part; Vale starts the gauge room log.

**Days 141–170.** Grease round established; bearing temperatures fall; the
shelter stops treating maintenance as a heroic event.

**Days 171–200.** Saw frame built; the pit becomes a backup; timber output
triples; builders from every other wave queue up for planks.

**Days 201–230.** Ridge tower raised; sails sewn; the governor is adjusted
three times; the tower runs on calm days and the race runs on still ones.

**Days 231–260.** Winter freeze: the race ices; the tower carries the load;
freeze protocols and wheel covers are tested for the first time.

**Days 261–290.** Bearing seizes during harvest week; the failure is traced to
a missed round; the shelter rebuilds the bearing, adds two grease points, and
changes the schedule.

**Days 291–320.** Calibration drift found in the shop squares; the reference set
is checked and re-set; two jobs are re-machined honestly.

**Days 321–360.** Year review: one wheel, one line, sixteen machines, three
breakdowns, and a shop that every other department depends on.

---

## 32. APPENDIX S — VIGNETTES (TONE SAMPLE)

> Jessa puts her palm on the pillow block and holds it there for a count of
> ten, and then says the bearing is warm, and the words are the whole diagnosis,
> and the work starts before anyone has opened a toolbox.

> Orth stands at the hopper and watches the flour come out and puts his hand in
> it and rubs it between his fingers, and the stone is dressed right, and he says
> nothing because there is nothing to say and the mill is working.

> Vale writes the temperature in the margin of the calibration log, because a
> gauge measured in a cold room and a gauge measured in a hot room are not the
> same gauge, and the difference is the kind of thing that ruins a part quietly.

> Nim walks the grease round every third day and knows the machines by their
> sounds before he knows them by their names, and one afternoon he reports a
> whine he cannot place, and Odo finds it in twenty minutes, and Nim gets the
> credit in the log.

---

## 33. APPENDIX T — BREAKDOWN AND REPAIR PROTOCOL

| Step | Action | Owner |
|---|---|---|
| First | Stop and lock out | nearest worker |
| Second | Mark the machine | mechanic |
| Third | Find the cause | mechanic |
| Fourth | Record in the log | appprentice |
| Fifth | Make or fetch parts | machine shop |
| Sixth | Repair and align | mechanic |
| Seventh | Test under load | mechanic |
| Eighth | Check the schedule | maintenance |
| Ninth | Change one practice | shop |
| Tenth | Restart with a witness | shop |

The protocol assumes that every breakdown is a schedule that was wrong. The
third step is where the shop's culture lives: find the cause first, and never
blame the operator for trusting the calendar.

---

## 34. APPENDIX U — MECHANICAL WEAR TABLE

| Component | Wear cause | Early sign | Check | Replacement |
|---|---|---|---|---|
| Wood gear | friction | dust | weekly | yearly |
| Bronze gear | overload | noise | monthly | years |
| Belt | slip | polish | weekly | yearly |
| Bearing | dryness | heat | daily | variable |
| Shaft | whip | vibration | weekly | decade |
| Coupling | shock | rattle | weekly | years |
| Stone | grain | slow cut | daily | yearly |
| Blade | grit | heat | daily | sharpen |
| Seal | pressure | drip | weekly | months |
| Guard | contact | notch | weekly | replace |
| Foundation | vibration | crack | season | rebuild |
| Governor | friction | hunt | monthly | years |

Wear is the expansion's honest clock. Nothing in the shop is permanent, and the
table turns that fact into a routine instead of a surprise.

---

## 35. APPENDIX V — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| Whip | vibration | realign, re-fit |
| Seized bearing | stopped line | rebuild, grease |
| Broken gear | no drive | swap wood, cast bronze |
| Slipped belt | low output | tension, re-lace |
| Cracked foundation | unsafe | re-bed, re-pour |
| Flooded race | idle | clear, protect |
| Frozen wheel | idle | cover, free, run tower |
| Drifted gauge | bad parts | calibrate, re-make |
| Missed grease | heat | round, schedule |
| Stone blunt | slow mill | dress |

No failure is a game over and no failure is a disgrace. The shop's whole
identity is that machines break and that a shelter measures itself by how
calmly it puts them back together.

---

## 36. APPENDIX W — CONTENT REVIEW CHECKLIST

- [ ] Every power source is water, wind, or muscle.
- [ ] No perpetual motion or over-unity exists.
- [ ] `KineticStorageSystem` remains the flywheel authority.
- [ ] `ShelterWorkshopSystem` remains the job authority.
- [ ] `PrecisionMetrologySystem` remains the calibration authority.
- [ ] `MachineIdentity` remains the quirk authority.
- [ ] Electrical power stays with the grid owner.
- [ ] Guards exist on every driveline in content.
- [ ] Apprentices are 16+ and supervised.
- [ ] Save additions are additive and legacy-neutral.
- [ ] Determinism uses live seeded paths only.

---

## 37. APPENDIX X — GLOSSARY

- **Head** — the height of water that gives a wheel its power.
- **Race** — the channel that delivers water to a wheel.
- **Line shaft** — the rotating spine that carries power through a shop.
- **Pillow block** — the bearing that holds a shaft.
- **Whip** — the vibration of a shaft that is out of true.
- **Crowned pulley** — a pulley shaped to keep a flat belt centered.
- **Backlash** — the play between gear teeth.
- **Drift** — the slow wandering of a tool's accuracy.
- **Dressing** — truing a millstone's cutting surface.
- **Lockout** — proving a machine cannot move before touching it.

---

## 38. APPENDIX Y — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `MechanicalPowerSystem` | sites, weather | budgets | grid state |
| `KineticStorageSystem` | flywheels | storage state | power source |
| `MachineToolSystem` | machines | jobs, condition | recipes |
| `ShelterWorkshopSystem` | jobs | outputs | machine state |
| `CalibrationSystem` | standards | grades, logs | machine jobs |
| `PrecisionMetrologySystem` | grades | calibration | outputs |
| `LubricationSystem` | grease stock | rounds, wear | machine state |
| `MachineMaintenanceSystem` | schedules | history | jobs |
| `MillWorkSystem` | grain, logs | batches | food state |
| `MachineIdentity` | history | quirks | condition |
| `PowerGridSystem` | interface | load | mechanical data |
| `CupolaFoundryEngine` | castings | output | machining |
| `GrainProcessingSystem` | grain | conversion | mill state |
| `NeedsSystem` | fatigue | needs | schedules |
| `EpilogueChronicleBuilder` | milestones | chronicle | shop state |

---

## 39. APPENDIX Z — DATA SCHEMA DETAIL (NEW CATALOGS)

**`mechanical_power.json`** — `power_budget` with `id`, `source`, `head_m`,
`flow_lps`, `wheel_efficiency`, `shaft_loss`, `gear_loss`, `belt_loss`,
`output_units`, `tags`.

**`wheel_sites.json`** — `site_id`, `display_name`, `source`, `head_m`,
`foundation`, `wheel_type`, `condition`, `seasonal_risk`, `tags`.

**`drivelines.json`** — `line_id`, `display_name`, `length_m`, `supports`,
`coupling`, `guards`, `tags`.

**`gearing.json`** — `pair_id`, `display_name`, `ratio`, `material`, `teeth`,
`wear`, `tags`.

**`machine_tools.json`** — `machine_id`, `display_name`, `type`, `build`,
`capacity`, `power_draw`, `quirks[]`, `tags`.

**`tooling_grades.json`** — `grade_id`, `display_name`, `calibration`,
`drift_per_day`, `allowed_work`, `check`, `tags`.

**`lubrication.json`** — `lubricant_id`, `display_name`, `points[]`, `interval`,
`effect`, `shortage`, `tags`.

**`maintenance_schedules.json`** — `machine_id`, `daily[]`, `weekly[]`,
`monthly[]`, `season[]`, `rebuild`, `tags`.

**`mill_batches.json`** — `batch_id`, `display_name`, `input`, `rate`,
`output`, `waste`, `quality`, `tags`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on missing
or duplicate IDs, invalid references, or out-of-range numbers.

---

## 40. APPENDIX AA — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Power delivered | budget honesty | Power |
| Loss per stage | design quality | Power |
| Machine uptime | maintenance | Maintenance |
| Calibration drift | precision | Calibration |
| Grease rounds kept | discipline | Lubrication |
| Breakdowns per year | reliability | Shop |
| Parts delivered | service | Shop |
| Mill batches | food service | Mill |
| Guards inspected | safety | Lineshaft |
| Apprentice hours | training | Roster |

Telemetry is diagnostic only; it never gates content and never becomes a score
against a worker or a machine.

---

## 41. APPENDIX AB — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] The four thin catalogs are extended with real content.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive, legacy-neutral, and round-trip tested.
- [ ] Flywheel, workshop, and metrology authorities remain untouched.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling checks.
- [ ] Phase 6 content passes the review checklist in §36.
- [ ] Phase 7 soak shows a harvest mill, a freeze, and a bearing rebuild.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No perpetual motion or unguarded driveline exists.

---

## 42. APPENDIX AC — OPEN QUESTIONS FOR REVIEW

1. Should the shelter be able to run two wheels at once, or one at a time?
2. Does bronze come from the foundry or from salvage first?
3. Should calibration failures be visible to the customer wave, or silent?
4. Is the grease round assigned to an apprentice or rotated?
5. How long is a rebuild allowed to stop a critical machine?
6. Do wind and water feed the same line, or separate shops?
7. Should the mill be allowed to sell flour outside, and at what grade?
8. Does a machine quirk ever fix itself, or only worsen until repaired?

None of these may be decided unilaterally; each changes tone and balance.

---

## 43. APPENDIX AD — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 12 The Second Generation | Apprenticeship in the shop |
| 1 | 15 The Deep Root | Mill and feed crushing |
| 1 | 16 The Rebuilt Body | Machined prosthetic parts |
| 2 | 17 The Long Evening | Workshop songs and rhythms |
| 2 | 18 The Underneath | Shafts for pumps and vents |
| 2 | 21 The Grid | Belt-driven generator takeoff |
| 3 | 22 The Clean Flow | Pump drives for water |
| 3 | 23 The Alarm | Machine fire drills |
| 3 | 25 The Iron Road | Machined rail parts |
| 3 | 26 The Common Table | Flour, meal, and crushing |
| 4 | 27 The Thread | Belt leather and cloth drives |
| 4 | 28 The Lesson | Shop training and standards |
| 4 | 29 The Glass | Gauges, windows, and bearings |
| 4 | 30 The Press | Calibration and job records |
| 4 | 31 The Kiln | Millstones and refractory linings |
| 5 | 32 The Wild | Feed crushing and hides for belts |
| 5 | 33 The Weather | Flood, freeze, and gale rules |
| 5 | 34 The Long Road | Haulage of castings and timber |
| 5 | 35 The Habit | Shop injuries and care |
| 5 | 36 The Watch | Machine guards and night shutdown |
| 6 | 39 The Reagent | Grease, oil, and tallow |

Each hook is additive. The Wheel can ship alone, and every other expansion can
ship without it.

---

## 44. APPENDIX AE — ENDING PROSE SKETCHES

**The Turning Works.** The wheel, the shaft, the belts, and the machines run as
one system, and the shelter stops thinking of power as a thing it lacks.

**The Reference Shop.** Calibration culture makes the gauge room the quiet center
of the settlement, and every part made elsewhere is checked here first.

**The Patient Mill.** The mill runs slowly, never breaks during harvest, and
turns grain into bread for a settlement that has stopped worrying about winter.

**The Bronze Year.** The shelter invests in bronze and good fit, and the
machines outlive the people who built them, which is exactly what was intended.

**The Dry Season.** The stream drops, the wheel stops, and the shelter learns
that power is a season rather than a possession, and starts planning accordingly.

**Fade.** A lathe turning in a warm shop while the rain fills the race, and a
miller counting sacks, and the wheel doing what wheels do.

---

## 45. APPENDIX AF — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Free power | dishonest | authored sources |
| Steam-punk trim | tone | workmanlike design |
| Hidden losses | fantasy | visible budget |
| Maintenance ignored | unfair | visible wear |
| Magic quirks | breaks world | mechanical quirks |
| Unguarded shafts | unsafe model | guards everywhere |
| Child operators | exploitation | 16+ supervised |
| Shop selff-serving | missed point | service to all waves |
| Instant rebuilds | trivializes | real time and parts |
| Mill as scenery | wasted system | real food output |

The list exists because machines are the easiest place to let fantasy in. The
expansion's rule is that every turn comes from water, wind, or muscle, and
every part comes from a shop that made it on purpose.

---

## 46. APPENDIX AG — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Power sites | 8 | 2,500 |
| Drivelines | 10 | 3,000 |
| Gear pairs | 12 | 2,500 |
| Machine tools | 16 | 4,500 |
| Tooling grades | 6 | 1,500 |
| Lubricants | 8 | 2,000 |
| Maintenance schedules | 16 | 3,500 |
| Mill batches | 12 | 2,500 |
| Calibration standards | 12 | 2,000 |
| Catalog extensions | 40 | 8,000 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 16 | 2,500 |
| Endings | 6 | 3,000 |
| **Total** | | **~63,500** |

---

## 47. APPENDIX AH — FIRST YEAR OF THE WHEEL

| Month | Focus | Milestone |
|---|---|---|
| 1 | Idle race | site chosen |
| 2 | Wheel | hung and turning |
| 3 | Shaft | line assembled |
| 4 | Bearings | bronze fitted |
| 5 | Belt | power transmitted |
| 6 | Mill | first flour |
| 7 | Lathe | first part made |
| 8 | Gauges | shop calibrated |
| 9 | Saw | frame running |
| 10 | Tower | wind power |
| 11 | Schedule | maintenance calendar |
| 12 | Review | year audited |

A year of the wheel is a year of learning that power is not a machine but a
habit, and the shelter ends it with a mill that runs, a shop that serves, and a
schedule that keeps both alive.

---

## 48. APPENDIX AI — PRECISION COVENANT

| Clause | Promise |
|---|---|
| Sources | Every turn comes from water, wind, or muscle |
| Losses | Every stage is accounted honestly |
| Guards | Every driveline is guarded |
| Maintenance | Every machine has a schedule |
| Calibration | Every gauge agrees with a standard |
| Records | Every job and rebuild is written down |
| Safety | Every injury changes a practice |
| Training | Every operator is taught and supervised |
| Service | The shop serves every other department |
| Repair | Machines break and are calmly rebuilt |

The covenant is the shop's identity. A community that can make one accurate
thing can make another, and a community that can repair its machines can plan
beyond its own lifetime, which is the quiet meaning of the whole expansion.

---

## 49. APPENDIX AJ — WORKSHOP JOB TABLE (16 JOBS)

| # | Job | Machine | Material | Output | Grade |
|---|---|---|---|---|---|
| 1 | Turn a shaft | lathe | steel | shaft | shop |
| 2 | Face a flange | lathe | steel | flange | shop |
| 3 | Bore a pulley | lathe | cast | pulley | shop |
| 4 | Cut a keyway | slotter | steel | keyed bore | shop |
| 5 | Drill a plate | drill | steel | plate | field |
| 6 | Tap a thread | drill | steel | threaded part | shop |
| 7 | Saw a plank | saw | timber | plank | field |
| 8 | Saw a beam | saw | timber | beam | field |
| 9 | Forge a pin | hammer | iron | pin | field |
| 10 | Draw a rod | hammer | iron | rod | field |
| 11 | Press a bearing | press | bronze | shell | shop |
| 12 | Broach a gear | broach | steel | gear blank | shop |
| 13 | Grind a tool | grinder | steel | edge | shop |
| 14 | Shear a sheet | shears | steel | sheet | field |
| 15 | Plane a bed | planer | iron | bed | reference |
| 16 | Balance a wheel | stand | mixed | balanced | shop |

Jobs are what the shop actually does all day. Each row is a machine, a material,
an output, and a grade, which means the player can follow a part from the timber
yard or the foundry gate all the way to the machine it repairs.

---

## 50. APPENDIX AK — COUPLING TABLE

| Coupling | Use | Slip | Service | Failure |
|---|---|---|---|---|
| Keyed | permanent | none | rare | shear key |
| Flexible | misalign | slight | yearly | wear |
| Crowned | belt side | none | monthly | edge |
| Clutch | machine takeoff | planned | monthly | slip |
| Cam | hammer | cycle | weekly | wear |
| Trip | hammer | impact | weekly | lock |
| Geared | heavy | none | yearly | teeth |
| Direct | short runs | none | rare | misalign |
| Slip | pump | at load | weekly | heat |
| Spare | stock | — | — | — |

Couplings are where the driveline either forgives or punishes misalignment. The
slip and service columns tell the player which couplings are meant to give and
which are meant to hold, and the spare row is the shop's insurance.

---

## 51. APPENDIX AL — FLOOD, FREEZE, AND GALE PROTOCOL TABLE

| Event | Warning | Immediate | Damage control | Recovery |
|---|---|---|---|---|
| Flood | rain gauge | open sluice | raise bearings | clear race |
| Spate | upstream report | shut wheel | anchor jack | inspect |
| Freeze | cold snap | cover wheel | drain pipes | thaw slow |
| Ice jam | flow drop | stop wheel | break jam safely | clear |
| Gale | wind watch | furl sails | lock brake | inspect tower |
| Drought | low flow | ration turns | run tower | plan |
| Silt | low output | stop wheel | dredge | rehang |
| Debris | visual | rake | protect wheel | repair |

Water and wind are seasons, and the protocol table is how the shop stops
treating a flood as bad luck. Each event has a warning that the shelter can
actually read and an immediate action that someone can actually perform.

---

## 52. APPENDIX AM — MILL SEASON TABLE

| Season | Grain | Demand | Mill hours | Notes |
|---|---|---|---|---|
| Spring | stored | moderate | 4/day | dry grain check |
| Summer | stored | low | 3/day | heat, stones |
| Harvest | fresh | high | 8/day | long dusty days |
| Autumn | mixed | high | 6/day | drying blend |
| Winter | stored | moderate | 4/day | frozen grain |
| Ash | screened | high | 6/day | covers, masks |

The mill has a season the way the fields do. The table lets the harvest be a
planned campaign instead of a crisis, and it makes the miller's ordinary
knowledge visible to the player.

---

## 53. APPENDIX AN — BEARING FIT TABLE

| Fit | Clearance | Use | Feel | Rebuild |
|---|---|---|---|---|
| Press | tight | permanent | hard | cut out |
| Close | small | spindles | snug | ream |
| Running | normal | shafts | smooth | re-fit |
| Loose | wide | slow | free | shim |
| Worn | excessive | none | rattle | replace |
| New | target | any | — | break in |

Odo's whole profession is in this table. Clearance is not a number so much as a
relationship between a shaft, a shell, and a season, and the fit column is how
the game represents that relationship without pretending it is mysterious.

---

## 54. APPENDIX AO — LATHE OPERATION TABLE

| Operation | Setup | Tool | Tolerance | Time |
|---|---|---|---|---|
| Face | chuck | knife | field | 20m |
| Turn | centers | knife | shop | 40m |
| Bore | steady | bar | shop | 50m |
| Thread | geared | chaser | shop | 60m |
| Part | parting | narrow | field | 15m |
| Knurl | press | knurl | field | 20m |
| Taper | offset | knife | shop | 45m |
| Polish | centers | file | field | 20m |

The lathe is the shop's first real machine and the table is its grammar. Each
row is a job the player can order for another wave, which is how the machine
shop becomes the place where every department's parts are made.

---

## 55. APPENDIX AP — SAW PATTERN TABLE

| Pattern | Log | Output | Rate | Blade |
|---|---|---|---|---|
| Plain planks | soft | planks | high | rip |
| Quarter cut | hard | quarters | low | rip |
| Beams | large | beams | med | rip |
| Boards | clean | boards | high | rip |
| Rounds | any | rounds | high | cross |
| Firewood | scrap | lengths | high | cross |
| Lath stock | straight | strips | med | fine |
| Pattern stock | best | matched | low | fine |

The saw is where the shelter's timber becomes buildings, furniture, and parts.
The rate column keeps the miller honest, and the blade column reminds the player
that different cuts consume different steel from the same tiny stock.

---

## 56. APPENDIX AQ — MILLSTONE DRESSING TABLE

| Stage | Tool | Time | Effect | Caution |
|---|---|---|---|---|
| Pick | chisel | 30m | restore bite | dust |
| Dress | hammer | 20m | level face | eye shield |
| Balance | stand | 15m | smooth run | guards |
| Gap set | feeler | 10m | grind quality | hot grain |
| Test run | grain | 20m | confirm | watch |
| Re-check | feel | 5m | pass | record |

Dressing a millstone is the oldest maintenance in the shelter and the one the
miller does most often. The table exists because a dull stone quietly wastes a
town's grain, and nothing about that failure is visible until the flour is wrong.

---

## 57. APPENDIX AR — SHOP APPRENTICE CURRICULUM TABLE

| Month | Skill | Machine | Check |
|---|---|---|---|
| 1 | grease round | all | log signed |
| 2 | measurement | gauges | 3 checks |
| 3 | files and saw | bench | test part |
| 4 | drill press | drill | test hole |
| 5 | lathe basics | lathe | test turn |
| 6 | belts | lineshaft | tension test |
| 7 | bearings | press | fit test |
| 8 | gears | broach | matching test |
| 9 | mill work | mill | batch test |
| 10 | saw patterns | saw | pattern test |
| 11 | maintenance | all | full round |
| 12 | independent job | chosen | verified |

The apprentice curriculum is twelve months long, supervised throughout, and
ends with one verified independent job. The table is the expansion's answer to
how a shelter turns a willing teenager into a mechanic without ever pretending
that a month of training is a career.

---

## 58. APPENDIX AS — LINE SHAFT INSPECTION TABLE

| Point | Check | Interval | Sign | Fail action |
|---|---|---|---|---|
| Pillow block | heat, oil | daily | grease round | oil, re-fit |
| Coupling | play | weekly | mechanic | re-key |
| Pulley | crown, wear | weekly | mechanic | re-turn |
| Belt | tension | weekly | mechanic | re-lace |
| Guard | fixings | weekly | apprentice | re-bolt |
| Support | settle | monthly | mechanic | re-shim |
| Shaft | whip | monthly | mechanic | realign |
| Foundation | crack | season | mason | re-bed |
| Clutch | slip | weekly | mechanic | adjust |
| Paint | rust | season | apprentice | re-coat |

Ten points, walked in order, signed at each. The inspection round is the
expansion's most repeated scene and the one that most shapes the shop's culture,
because it teaches the player that a driveline is looked at, not merely used.

---

## 59. APPENDIX AT — SPARE PARTS STOCK TABLE

| Part | Minimum | Made by | Used by | Rebuild |
|---|---|---|---|---|
| Bearing shell | 4 | press | all | re-bore |
| Key | 10 | saw, file | all | replace |
| Belt section | 3 | leather | lineshaft | re-lace |
| Gear pair | 2 | broach | drive | re-cut |
| Pulley | 2 | lathe | drive | re-turn |
| Clutch plate | 2 | press | takeoff | re-line |
| Seal ring | 6 | turn | pump | replace |
| Blade | 3 | forge | saw | sharpen |
| Stone | 1 | quarry | mill | dress |
| Governor weight | 2 | cast | tower | re-balance |

Spare parts are how the shop survives its own failures. The minimum column is
the stock the shelter keeps against the day a critical machine stops, and the
made-by column is the shop's promise that it can replace almost anything it
builds.

---

## 60. APPENDIX AU — WHEEL HOUSEKEEPING TABLE

| Task | Interval | Who | Sign |
|---|---|---|---|
| Clear race | weekly | laborer | board |
| Grease wheel | weekly | apprentice | round |
| Check sluice | daily | miller | board |
| Cover wheel | season | laborer | board |
| Inspect jack | monthly | mechanic | log |
| Sweep house | daily | apprentice | board |
| Inspect guards | weekly | mechanic | log |
| Freeze drain | season | mechanic | log |

Housekeeping is the unglamorous half of a working wheel, and the table is
included because the expansion would rather teach the player to sweep a wheel
house than to admire one. A dry house, a clear race, and a greased bearing keep
the mill running long after the romance wears off.

---

## 61. CLOSING STATEMENT

ASHFALL already has flywheels, workshop jobs, metrology grades, broach benches,
and machine identities with quirks. What it lacks is the world around them:
wheels, shafts, gears, belts, lathes, grease, schedules, and the parts that every
other wave needs. The Wheel adds that world without adding a second power grid
or a factory fantasy. It adds an honest budget of turns and losses, a shop that
makes its own replacement parts, a calibration culture, and the quiet certainty
of a machine that will still run when the person who built it is gone.

> Wave 6 note: this plan is one of five Wave 6 expansion bibles (37–41). Each is
> self-contained; none requires another to ship. The shared Wave 6 index lives at
> `docs/expansions/wave6/WAVE6_INDEX.md`. The safe pre-signature step is Phase 1
> (data schemas and validators), which is additive and reversible. Evidence
> anchors: `KineticStorageSystem` (`KineticFlywheelCatalog`, `FlywheelClassDef`
> with `rotor_mass_kg`, `effective_radius_m`, `moment_of_inertia_factor`,
> `SurgeEventDef`, `BlackStartDef`, `ContainmentHazardDef`),
> `ShelterWorkshopSystem` (`WorkshopJobKind`, `WorkshopRecipeDefinition`),
> `PrecisionMetrologySystem` (`PrecisionCalibrationGrade`, `MetrologyGradeDef`
> with `tooling_calibration` and `drift_per_day`, `MetrologyStandardDef` with
> `max_grade`, `required_room_ids`, `calibration_labor_ticks`),
> `PrecisionBroachingCatalog` (`BroachBenchDef`), `MachineIdentity`, and the
> thin catalogs `precision_broaching_catalog.json` (1,739 B),
> `metrology_standards_catalog.json` (3,143 B), and
> `kinetic_flywheel_catalog.json` (5,984 B).