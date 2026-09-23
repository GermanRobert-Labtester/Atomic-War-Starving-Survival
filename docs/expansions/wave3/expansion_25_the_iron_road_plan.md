# ASHFALL — Expansion 25 Design Bible
# THE IRON ROAD
### Wave 3 · Rail, Locomotives, Track, Bridges, Rail Towns, and Interdiction

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-20
**Domain owners touched:** `Ashfall.Core.Expeditions` (Railway, Interlock, RailLogistics, Draisine), `Ashfall.Core.Economy` (trade), `Ashfall.Core.Factions` (route politics)
**Proposed host owner:** `IronRoadHostSession` (extends `RailwaySaveStore` + interlock and logistics surfaces)
**Existing save sections:** `railway`, `rail_interlock`, `vehicles`, `expeditions`
**Existing CLI verbs:** `--railway-selftest`, `--rail-interlock-selftest`, `--rail-logistics-selftest`, `--draisine-selftest`, `--rail-grinding-selftest`
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already owns rail with unusual completeness for a survival game.
`RailwaySystem` declares itself and `rail_network.json` the only owners of nodes,
segments, trains, dispatch, and pathfinding. It models dispatch status
(Idle/Preparing/EnRoute/Derailment/RobberyAmbush/Arrived), rail nodes with types
(Terminal, Junction, Depot, Terminus), track segments with integrity, bridge
requirements, maximum train mass, and hazard tags, and train cars with mass, cargo,
and armor. `RailwayInterlockEngine` (Plan 117) owns switches, route locks, signal
aspects, reservations, obstruction, tamper abstraction, and a fictional route-denial
device, validating every reference against the canonical topology — and never moving
an expedition. `RailLogisticsCatalog` adds edges with gauge tags, track condition,
grade, switchyards, clearance requirements, derailment risk, obstacle profiles, and
repair requirements. `RailGrindingEngine`, `AmphibiousDraisineEngine`,
`DraisineRerailingSystem`, and `MineClearingFlailEngine` fill in maintenance and
recovery. The vehicle garage owns road vehicles.

But the content is thin: `rail_network.json` is 4.2 KB, `rail_logistics_catalog.json`
2.6 KB, `railway_interlock_catalog.json` 2.7 KB. A rail system that could carry a
campaign has almost nothing on its rails.

**The Iron Road** turns the existing rail machinery into a regional spine: a line
worth restoring, track gangs and bridges that must be maintained, locomotives and
cars worth building, rail towns that grow around depots, interdiction by bandits and
armored trains, and the politics of who may run trains on whose rails.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin; **`PROPOSED`** new.

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

Roads in the wasteland are mud, ambushes, and fuel. Rail is none of those things.
A rebuilt line moves ten times the cargo at a tenth of the cost, and it does it on a
schedule. That is why the old world built it, and why the new one will fight over
what remains.

**The Iron Road** is the expansion about restoring a rail corridor between
settlements: surveying, clearing, regauging, rebuilding, and defending a line that
turns scattered camps into a region. It is the logistics expansion with a geography,
and it is the only expansion where a bridge is more important than a rifle.

The expansion's hard rules come from the live owners: `RailwaySystem` owns topology
and dispatch; the interlock owns legality, signals, and reservations; nothing else
moves a train. Every new system reads those authorities and reports, never duplicates
them.

### 1.2 The five loops it adds

```
   Survey ──► Clear ──► Rebuild ──► Run ──► Grow
      │         │          │          │        │
      ▼         ▼          ▼          ▼        ▼
   routes,   obstructions track,     trains,  towns,
   bridges   mines,        bridges,  schedules depots,
   grades    wrecks        signals            trade
      │                                          │
      ▼                                          ▼
   Interdiction ◄── bandits, armored trains, route denial ◄── politics
      │
      ▼
   Commons ──► access, tolls, shared maintenance
```

### 1.3 What the player manages

1. **Topology.** Nodes, segments, junctions, depots, and terminals. The live system
   owns it; the expansion authors a region worth connecting.
2. **Track.** Integrity, grade, condition, grind, and replacement. A rail is a
   consumable worn by every train that passes.
3. **Bridges.** Load ratings, inspection, scour, and collapse. Bridges are the
   line's bottlenecks and its monuments.
4. **Trains.** Locomotives, cars, mass, cargo, armor, crews, fuel, and water.
5. **Interlock and signals.** Switches, route locks, aspects, reservations, and
   obstructions — the difference between a schedule and a wreck.
6. **Towns.** Depots that become settlements, with populations, needs, and loyalties.
7. **Interdiction.** Bandits, armored trains, sabotage, and route denial; defense is
   patrol, escort, blockhouses, and diplomacy.
8. **Commons.** Access, tolls, shared maintenance, and the politics of a line that
   crosses everyone's territory.

### 1.4 What it is not

- Not a second rail system. `RailwaySystem` remains the sole topology and dispatch
  authority.
- Not a second interlock. `RailwayInterlockEngine` remains the legality and signal
  owner.
- Not a train simulator. Movement is abstract, deterministic, and schedule-driven.
- Not a combat expansion. Interdiction is risk, escort, and defense, with combat
  routed through the live combat/encounter authorities.
- Not a second save authority.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Expeditions/RailwaySystem.cs` | Topology, nodes, segments, cars, dispatch status, pathfinding | `LIVE` |
| `Assets/Ashfall.Core/Expeditions/RailwayInterlockEngine.cs` | Switches, route locks, signals, reservations, obstruction, tamper, route denial | `LIVE` |
| `Assets/Ashfall.Core/Expeditions/RailLogisticsCatalog.cs` | Edges: gauge, condition, grade, clearance, derailment, repair | `LIVE` |
| `Assets/Ashfall.Core/Expeditions/RailGrindingEngine.cs` | Rail grinding | `LIVE` |
| `Assets/Ashfall.Core/Expeditions/AmphibiousDraisineEngine.cs` | Amphibious draisine | `LIVE` |
| `Assets/Ashfall.Core/Expeditions/DraisineRerailingSystem.cs` | Rerailing | `LIVE` |
| `Assets/Ashfall.Core/Expeditions/MineClearingFlailEngine.cs` | Mine clearing | `LIVE` |
| `Assets/Ashfall.Core/Expeditions/VehicleGarageSystem.cs` | Road vehicles (adjacent) | `LIVE` |
| `src/Host/RailwaySaveStore.cs` | Persistence | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Size | Notes |
|---|---|---|
| `rail_network.json` | 4.2 KB | five nodes, a few segments |
| `rail_logistics_catalog.json` | 2.6 KB | edges with condition and risk |
| `railway_interlock_catalog.json` | 2.7 KB | junctions and routes |
| `rail_grinding_catalog.json` | 1.5 KB | grinding profiles |
| `amphibious_draisine_catalog.json` | 4.0 KB | draisine |
| `mine_flail_catalog.json` | 2.0 KB | clearing equipment |
| `runflat_tire_catalog.json` | 2.2 KB | road adjacent |

### 2.3 Confirmed gaps

- **GAP-25-1 — Five nodes is a demo.** The topology supports a regional network; only
  a fragment is authored.
- **GAP-25-2 — No trackside infrastructure.** No water towers, coal stages,
  roundhouses, turntables, or blockhouses.
- **GAP-25-3 — No bridges.** `bridge_required` exists; no bridge catalog, load
  ratings, inspection, or collapse.
- **GAP-25-4 — No locomotives or car catalog.** `TrainCarDef` exists; nothing is
  authored beyond the expected defaults.
- **GAP-25-5 — No track maintenance loop.** Grinding exists; no gang, schedule,
  replacement, or weather damage.
- **GAP-25-6 — No rail towns.** Depots exist as node types; no settlements,
  populations, needs, or loyalties.
- **GAP-25-7 — No schedules.** No timetables, windows, meets, or priority.
- **GAP-25-8 — No interdiction content.** `RobberyAmbush` and the route-denial device
  exist; no bandits, armored trains, escorts, or blockhouses.
- **GAP-25-9 — No rail commons.** No access treaties, tolls, or shared maintenance.

### 2.4 Non-duplication statement

This expansion will **not** add a second rail topology, dispatch, or interlock
system. It extends `RailwaySystem`, `RailwayInterlockEngine`, `RailLogisticsCatalog`,
`RailGrindingEngine`, and the draisine/flail engines with data and additive
subsystems. It routes road vehicles through `VehicleGarageSystem`, combat through the
live combat authorities, and section standing through `FactionStanceEngine`. It does
not duplicate the Long Line expansion's telegraph network or the caravan economy.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Rail is infrastructure with a schedule.** A line is only useful if a
train comes back. Timetables matter more than heroics.

**Pillar 2 — Every kilometre has an owner.** The line crosses territory. Running a
train is a diplomatic act, and the track gang that maintains a section has a claim.

**Pillar 3 — Bridges are the line's truth.** A line is as strong as its weakest
bridge, and a bridge is the most expensive thing to repair. Bridge arcs should carry
the expansion's emotional weight.

**Pillar 4 — Track is a consumable.** Every train wears the rail. Maintenance is not
optional; neglect is a future derailment with a date on it.

**Pillar 5 — Towns grow at the stops.** A depot with water and coal becomes a
settlement, and a settlement becomes a politics. The line makes the region.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| A train | Weight, heat, a whistle carried far | Steam-punk spectacle |
| Track work | Spikes, ties, sweat, a surveyor's level | Heroic montage |
| A bridge | A span, a flood line, a cracked pier | Thrill-ride collapse |
| A depot town | Water, coal, and a reason to stay | Wild-west cliché |
| Interdiction | A stopped train, a demand, a standoff | Train robbery adventure |
| The commons | A timetable nailed to a shed | Empire fantasy |

### 3.3 Content limits

- No real railway company, locomotive class, or military rail unit is named.
- Rail robbery is a social and economic event, not a combat set piece.
- Bridge failures are grave and rare; no destruction footage.
- Rail towns are inhabited by survivors with real needs, not scenery.
- No content that trivializes the road/rail logistics tradeoff.

---

## 4. THE IRON ROAD WORLD

### 4.1 Interior rooms

- **`room_rail_yard`** — workshops, cars, and a turntable.
- **`room_roundhouse`** — locomotive stalls, pits, and cranes.
- **`room_rail_office`** — timetables, tokens, and the dispatcher's board.
- **`room_track_store`** — rail, ties, spikes, and tools.
- **`room_rail_dorm`** — crew and gang bunks.
- **`room_water_tower`** — water for the locomotives.
- **`room_coal_stage`** — fuel staging and ash removal.
- **`room_blockhouse`** — line defense and signal post.

### 4.2 Exterior locations

| ID | Name | Danger | Purpose |
|---|---|---|---|
| `loc_switchyard_ruins` | The Switchyard | 5 | Junctions, switches, and salvage |
| `loc_trestle_bridge` | The High Trestle | 6 | Long bridge; load and scour risk |
| `loc_river_bridge` | The River Span | 5 | Crossing; contested maintenance |
| `loc_rail_tunnel` | The Black Tunnel | 6 | Tunnel clearance and ventilation |
| `loc_track_gang_camp` | The Gang Camp | 3 | Maintenance base |
| `loc_depot_town` | Depot Town | 4 | Rail settlement and trade |
| `loc_water_stop` | The Water Stop | 3 | Trackside water point |
| `loc_wreck_line` | The Wreck Line | 6 | Derailed train salvage |
| `loc_armored_siding` | The Armored Siding | 7 | Hostile train base |
| `loc_rail_junction` | The Crossing | 5 | Two lines meet; politics |

All locations require valid item references and scanner registration.

### 4.3 The rail graph

The rail graph is authored in `rail_network.json` (nodes and segments) and extended
by `rail_logistics_catalog.json` (edges). The expansion adds nodes, segments, depots,
junctions, water stops, and bridges, all validated by the interlock against the
canonical topology. No other system may move a train.

---

## 5. MAIN STORYLINE — "THE LINE THAT HOLDS"

### 5.1 Central conflict

The shelter's road convoys are bleeding fuel and people. A survey finds a mostly
intact rail corridor running from the shelter's yard to a rail town three days east,
with two broken spans, a flooded cutting, and a tunnel that may not clear. Restoring
it would let the shelter move grain, ore, and medicine at a tenth of the road cost —
and would make it the most valuable target in the region.

The railway engineer, **Vera Kast**, wants the line. The caravan master, **Dorn
Hale**, does not trust it: he has watched rail promises die in mud before. The rail
town of **Kilometre Nine** wants the line too — and wants to own its section of it.
And an armored train out of the east has begun stopping convoys at the crossing,
taking a "transit toll" in goods and occasionally in people.

The expansion's question: **when you rebuild a line, who owns the ground it crosses?**

### 5.2 Theme (unspoken)

**A line connects places that have learned not to trust each other. The track is the
easy part.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_engineer_vera_kast` | Vera Kast | Railway engineer | Builds the line; uncompromising on standards |
| `npc_caravan_master_dorn_hale` | Dorn Hale | Caravan master | Defender of the road; needs convincing |
| `npc_section_boss_mara` | Mara Osk | Track section boss | Owns a stretch; resents trespass |
| `npc_dispatcher_lin` | Lin Vey | Dispatcher | Timetables, tokens, and the board |
| `npc_town_mayor_hask` | Hask Orr | Depot town mayor | Wants the line and the leverage |
| `npc_raider_captain_grale` | Grale | Armored train captain | Demands tolls; not irrational |
| `npc_crew_lead_koval` | Koval | Train crew lead | Drives; afraid of one particular bridge |
| `npc_child_rail_pim` | Pim | Child of the depot | Why the town matters |

### 5.4 Story beats (15)

1. **The Survey.** The corridor is found; the cost is estimated.
2. **The Argument.** Road versus rail; the first bridge is chosen.
3. **The Cutting.** A flooded cutting is drained and reballasted.
4. **The First Train.** A locomotive is restored and runs a test.
5. **The Crossing.** The armored train stops the test run and demands a toll.
6. **The Tunnel.** Clearance is measured; a tunnel is opened or bypassed.
7. **The Town.** Kilometre Nine wants a schedule and a share.
8. **The Trestle.** The high trestle is load-tested and found wanting.
9. **The Gang.** A section boss refuses access to her stretch.
10. **The Theft.** Rail and spikes are stolen from the line.
11. **The Toll.** The armored train raises its demand.
12. **The Storm.** Flooding scours a pier; the bridge is closed.
13. **The Meet.** Two trains must pass; the interlock decides who waits.
14. **The Reckoning.** The line is run, shared, or abandoned.
15. **The Line That Holds.** Final disposition of the corridor.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Line vs. road | invest rail / keep road / both | future vs. present |
| Bridge | rebuild strong / patch / bypass | cost vs. safety |
| Town share | full partner / client / exclude | politics |
| Toll | pay / resist / negotiate | submission vs. risk |
| Schedule priority | own cargo / town cargo / fair | interest vs. commons |
| Maintenance | funded / minimum / toll-funded | reliability |
| Interdiction | escort / blockhouse / tribute | defense vs. cost |
| Final | own / share / abandon | legacy |

### 5.6 Endings (5 + fade)

1. **The Open Line** — the corridor runs; the region trades and grows.
2. **The Toll Road** — the line runs under an armored-train toll; trade continues, pride does not.
3. **The Broken Span** — the line fails at the worst moment; the road remains.
4. **The Town's Line** — Kilometre Nine owns its section; the shelter is a partner, not an owner.
5. **The Iron Commons** — the line becomes shared infrastructure with a written timetable.
6. **Fade** — the corridor is left to rust; the road keeps running.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_rail_`. Schema follows `year_of_ash_quests.json`.

### 6.1 Main questline (15)

`quest_rail_survey`, `quest_rail_argument`, `quest_rail_cutting`, `quest_rail_first_train`,
`quest_rail_crossing`, `quest_rail_tunnel`, `quest_rail_town`, `quest_rail_trestle`,
`quest_rail_gang`, `quest_rail_theft`, `quest_rail_toll`, `quest_rail_storm`,
`quest_rail_meet`, `quest_rail_reckoning`, `quest_rail_line_holds`.

### 6.2 Side quests (30)

**Survey and grading (5)**
- `quest_rail_corridor_survey` — walk and map the corridor
- `quest_rail_grade_check` — measure grades and curves
- `quest_rail_clearance` — check tunnel and bridge clearance
- `quest_rail_landmark` — set mileposts and markers
- `quest_rail_right_of_way` — negotiate crossing land

**Track work (5)**
- `quest_rail_ballast` — dig and lay ballast
- `quest_rail_tie_replace` — cut and replace ties
- `quest_rail_rail_gang` — lay rail with a gang
- `quest_rail_gauge_fix` — correct a gauge defect
- `quest_rail_spike_run` — make or salvage spikes

**Bridges and tunnels (5)**
- `quest_rail_bridge_inspect` — inspect and rate a span
- `quest_rail_pier_repair` — repair a scoured pier
- `quest_rail_span_replace` — replace a span
- `quest_rail_tunnel_vent` — clear and ventilate the tunnel
- `quest_rail_underpin` — shore a failing abutment

**Trains and crews (5)**
- `quest_rail_locomotive_restore` — restore a locomotive
- `quest_rail_car_build` — build a car
- `quest_rail_crew_hire` — find and qualify a crew
- `quest_rail_water_stop` — build a water stop
- `quest_rail_coal_stage` — build a coal stage

**Interlock and schedule (5)**
- `quest_rail_switch_repair` — repair a junction switch
- `quest_rail_signal_restore` — restore signal aspects
- `quest_rail_token` — establish a token system
- `quest_rail_timetable` — write the first timetable
- `quest_rail_meet_plan` — plan a passing meet

**Towns and politics (5)**
- `quest_rail_depot_town` — formalize the depot town
- `quest_rail_town_needs` — supply the town
- `quest_rail_section_claim` — settle a section claim
- `quest_rail_toll_talk` — negotiate with the armored train
- `quest_rail_commons_pact` — draft a shared-line pact

**Interdiction (5)**
- `quest_rail_escort` — escort a train
- `quest_rail_blockhouse` — build a blockhouse
- `quest_rail_patrol` — patrol the line
- `quest_rail_wreck_clear` — clear a wreck
- `quest_rail_denial_counter` — counter a route-denial device

### 6.3 Repeatable quests (8)

`quest_rail_repeat_inspect`, `quest_rail_repeat_grind`,
`quest_rail_repeat_crew`, `quest_rail_repeat_patrol`,
`quest_rail_repeat_schedule`, `quest_rail_repeat_town_run`,
`quest_rail_repeat_bridge_check`, `quest_rail_repeat_escort`.

### 6.4 Dynamic hooks

`RailwaySystem` emits dispatch and incident events; the interlock reports legality,
reservations, and obstruction; grinding and flail engines emit maintenance events.
The generator attaches authored follow-ups without a new event bus.

### 6.5 Constraints

- Only `RailwaySystem` may move a train or change topology.
- Only `RailwayInterlockEngine` may authorize a route.
- No bridge may collapse without inspection warnings and load history.
- No interdiction may bypass the live combat/encounter authorities.
- No town may be abstracted into a number; it has needs and people.
- No rail commons may bypass `FactionStanceEngine`.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `TrackMaintenanceSystem` (new, `Ashfall.Core.Expeditions`)

**Owns:** track condition over time, gangs, grinding schedules, tie and rail
replacement, and weather damage. **Consumes:** `RailwaySystem` segments,
`RailLogisticsCatalog` edges, `RailGrindingEngine`, `WeatherSystem`, `Inventory`.
**Data:** `track_materials.json`, `track_gang_roles.json`.
**Rules:** every train pass wears the track; neglect raises derailment risk with an
authored warning; maintenance consumes rail, ties, spikes, and labor.

### 7.2 `BridgeSystem` (new, `Ashfall.Core.Expeditions`)

**Owns:** bridges, spans, piers, abutments, load ratings, inspection, scour, and
collapse risk. **Consumes:** `RailwaySystem` segments, `WeatherSystem`, `Inventory`,
`SilentFoundrySystem` (beams). **Data:** `bridge_catalog.json`.
**Rules:** a train's mass against the load rating decides passage; inspections reveal
condition; flooding raises scour; collapse is preceded by warnings and can close the
line permanently.

### 7.3 `LocomotiveSystem` (new, `Ashfall.Core.Expeditions`)

**Owns:** locomotives, fuel, water, steam, wear, and availability. **Consumes:**
`RailwaySystem` cars and dispatch, `PowerGridSystem` (workshop), `Inventory`.
**Data:** `locomotives.json`, `train_cars.json`.
**Rules:** every locomotive has a fuel and water demand, a maintenance cycle, and a
failure mode; a cold locomotive takes time to raise; no free motion.

### 7.4 `RailScheduleSystem` (new, `Ashfall.Core.Expeditions`)

**Owns:** timetables, departure windows, meets, priorities, and delays. **Consumes:**
`RailwaySystem`, `RailwayInterlockEngine`, `Clock`. **Data:** `rail_schedules.json`.
**Rules:** schedules create obligations; a delayed train cascades; the interlock still
authorizes every movement; priority is editorial and political.

### 7.5 `RailTownSystem` (new, `Ashfall.Core.World`)

**Owns:** depots as settlements with population, needs, loyalty, and trade.
**Consumes:** `Settlements`/`FactionStanceEngine`, `TradingSystem`, `NeedsSystem`.
**Data:** `rail_towns.json`.
**Rules:** a town grows with traffic and starves without it; its loyalty responds to
service and exclusion; it is a real settlement, not a resource node.

### 7.6 `RailInterdictionSystem` (new, `Ashfall.Core.Expeditions`)

**Owns:** bandit pressure, armored trains, tolls, escorts, blockhouses, route denial
counters, and threat escalation. **Consumes:** `RailwaySystem`,
`RailwayInterlockEngine` tamper/denial state, `ExpeditionSystem`, combat/encounter
authorities, `FactionStanceEngine`. **Data:** `rail_interdiction.json`.
**Rules:** interdiction is risk and negotiation, not a set-piece battle; a stopped
train is an encounter; route denial devices are fictional abstractions; escalation is
authored and visible.

### 7.7 `GaugeSystem` (new, `Ashfall.Core.Expeditions`)

**Owns:** gauge standards, interchange points, and conversion costs. **Consumes:**
`RailLogisticsCatalog` gauge tags, `Inventory`. **Data:** `gauge_tables.json`.
**Rules:** mismatched gauge blocks a through-run; conversion is expensive and slow;
interchange points are political.

### 7.8 `RailCommonsSystem` (new, `Ashfall.Core.Factions`)

**Owns:** access agreements, tolls, shared maintenance, and timetable pacts.
**Consumes:** `FactionStanceEngine`, `DiplomaticTreaties`, `RailScheduleSystem`.
**Data:** `rail_pacts.json`.
**Rules:** a shared line moves real trains; a cut is a standing and logistics event;
no second rail network is created.

### 7.9 Systems explicitly not added

- No second topology, dispatch, interlock, or rail movement system.
- No train physics simulation.
- No combat system; interdiction routes through the live authorities.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered
and cross-checked against the canonical rail topology by the interlock.

### 8.1 `rail_network.json` (extend)

Existing schema preserved (`nodes[]` with `node_id`, `display_name`, `zone_id`,
`node_type`; `segments[]` with `segment_id`, `display_name`, `start_node_id`,
`end_node_id`, `distance_km`, `base_integrity`, `bridge_required`, `max_train_mass`,
`hazard_tags`). The expansion authors a full regional corridor with junctions,
depots, terminals, and a second line.

### 8.2 `rail_logistics_catalog.json` (extend)

Existing edge schema preserved (`rail_edge_id`, `from_node`, `to_node`, `gauge_tag`,
`track_condition`, `grade`, `switchyard_id`, `clearance_requirement`,
`derailment_risk_bp`, `obstacle_profile`, `repair_requirement`, `tags`).

### 8.3 `railway_interlock_catalog.json` (extend)

Junctions, signals, routes, and reservations. The interlock validates every
reference; invalid topology fails the integrity gate.

### 8.4 `locomotives.json` (new)

```json
{
  "schema_version": 1,
  "locomotives": [
    {
      "locomotive_id": "loco_yard_restored",
      "display_name": "Yard Restoration Engine",
      "power_class": "steam",
      "mass_kg": 42000,
      "tractive_effort": 0.32,
      "fuel_item_id": "item_coal_chunk",
      "fuel_per_km": 12,
      "water_per_km": 90,
      "max_speed_kmh": 40,
      "warmup_hours": 3,
      "maintenance_interval_days": 20,
      "wear_per_100km_bp": 60,
      "crew_required": 3,
      "tags": ["steam", "freight", "heavy"]
    }
  ]
}
```

### 8.5 `train_cars.json` (new)

Car rows: type, mass, capacity, armor, coupling, special role (flat, box, tank,
reefer, caboose, gun, crane, clinic).

### 8.6 `bridge_catalog.json` (new)

Bridge rows: span type, length, load rating, pier count, scour risk, inspection
interval, repair materials.

### 8.7 `track_materials.json` (new)

Rail, tie, spike, ballast, and switch materials with wear and replacement values.

### 8.8 `track_gang_roles.json` (new)

Gang roles: boss, surveyor, spiker, grinder, welder, flagman, guard, with skills.

### 8.9 `rail_schedules.json` (new)

Schedule rows: service, departure window, priority, cargo class, meet rules.

### 8.10 `rail_towns.json` (new)

Town rows: depot, population, needs, loyalty drivers, trade goods, and growth rules.

### 8.11 `rail_interdiction.json` (new)

Threat rows: bandit pressure, armored train, toll demand, sabotage, denial device,
and escalation steps.

### 8.12 `gauge_tables.json` (new)

Gauge rows: standard, narrow, broad, conversion cost, interchange requirements.

### 8.13 `rail_pacts.json` (new)

Access rows: partner, section, toll, maintenance share, priority, breach effect.

### 8.14 Items

New items appended to `items.json`: `item_rail_length`, `item_crosstie`,
`item_spike_box`, `item_ballast_load`, `item_switch_frog`, `item_coupler`,
`item_locomotive_part`, `item_water_tank_car`, `item_coal_chunk`,
`item_signal_lamp`, `item_token_staff`, `item_bridge_beam`,
`item_blockhouse_plate`, `item_rail_token`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

`src/Host/RailwaySaveStore.cs` captures topology and dispatch. Interlock state is
captured by its own envelope. New sub-objects are additive. No new save section.

### 9.2 State to persist

- Track condition per segment and grinding history.
- Bridge condition, inspections, and load history.
- Locomotives, cars, fuel, and water.
- Schedules, delays, and meets.
- Rail towns and their growth.
- Interdiction pressure and toll agreements.
- Gauge and commons pacts.

### 9.3 Determinism

- Movement and interlock evaluation are deterministic given topology and time.
- Track wear and bridge scour are pure arithmetic on authored rates.
- Derailment and interdiction rolls use the host-forked `ISeededRng`.
- Paired replay hashes must match.

### 9.4 Migration

Legacy saves load with the existing topology, no track wear history, no bridges, no
locomotives, no schedules, no towns, and no pacts. Existing rail state is untouched.

### 9.5 Checksum

Invariant-culture floats; integer-permille for wear, risk, and condition.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `RailwayPanel` (new/extend) | Network, dispatch, trains, cargo | `IronRoadHostSession` |
| `TrackPanel` (new) | Segment condition, grinding, gangs | same |
| `BridgePanel` (new) | Spans, load ratings, inspections, repairs | same |
| `LocomotivePanel` (new) | Engines, fuel, water, wear, crews | same |
| `SchedulePanel` (new) | Timetables, windows, meets, delays | same |
| `RailTownPanel` (new) | Depots, populations, needs, loyalty | same |
| `InterlockPanel` (new) | Switches, signals, locks, reservations | same |
| `InterdictionPanel` (new) | Threats, tolls, escorts, blockhouses | same |
| `RailCommonsPanel` (new) | Access, tolls, maintenance shares | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Track and bridge risk are stated with thresholds and warnings, never hidden.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- Derailment and collapse risks are explicit before committing a train.
- Town needs and loyalty are visible; a town is never a silent number.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: distant whistle, rail joint, spike hammer,
locomotive chuff, bridge creak, switch throw, signal bell. No cue is required; text
carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `RailwaySystem` | Topology, dispatch, cars, pathfinding extended |
| `RailwayInterlockEngine` | Switches, signals, locks, reservations consumed |
| `RailLogisticsCatalog` | Edges extended |
| `RailGrindingEngine` | Grinding consumed by maintenance |
| `AmphibiousDraisineEngine`, `DraisineRerailingSystem` | Recovery and light rail |
| `MineClearingFlailEngine` | Line clearing |
| `VehicleGarageSystem` | Road comparison and interchange |
| `WeatherSystem` | Track and bridge damage |
| `PowerGridSystem` | Workshops, roundhouse, signals |
| `SilentFoundrySystem` | Beams, castings, switch parts |
| `TradingSystem` | Rail cargo and town trade |
| `FactionStanceEngine` | Tolls, access, commons |
| `ExpeditionSystem` | Escorts and interdiction |
| `Combat`/encounter authorities | Any physical confrontation |
| `NeedsSystem` | Town and crew needs |
| `MemorialSystem` | Derailment and defense deaths |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm `RailwaySystem`, `RailwayInterlockEngine`,
`RailLogisticsCatalog`, `RailGrindingEngine`, draisine/flail engines, save store, and
panels. Record file:line; change nothing.

**Phase 1 — Data + validators.** Extend rail network, logistics, and interlock;
author locomotives, cars, bridges, track materials, gang roles, schedules, towns,
interdiction, gauge tables, pacts. Register validators and scanner.

**Phase 2 — Pure Core.** `TrackMaintenanceSystem`, `BridgeSystem`,
`LocomotiveSystem`, `RailScheduleSystem`, `RailTownSystem`,
`RailInterdictionSystem`, `GaugeSystem`, `RailCommonsSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip, determinism.

**Phase 4 — Host + CLI.** `IronRoadHostSession`, extended selftest verbs, fresh journey.

**Phase 5 — UI.** Extended and new surfaces with lifecycle and accessibility.

**Phase 6 — Content.** Locations, rooms, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** 180/360-day soak including maintenance, weather, towns, tolls.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Rail nodes | 20 |
| Rail segments | 24 |
| Bridges | 12 |
| Locomotives | 10 |
| Train cars | 20 |
| Track materials | 12 |
| Gang roles | 8 |
| Schedules | 15 |
| Rail towns | 6 |
| Interdiction threats | 12 |
| Gauge types | 4 |
| Rail pacts | 8 |
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
| Second rail system | Critical | Extend live owners |
| Trains trivialize road logistics | High | Fuel, water, wear, interdiction |
| Bridge collapse unfair | High | Inspections and warnings |
| Towns become scenery | High | Real needs and loyalty |
| Interdiction becomes combat spam | Medium | Negotiation and escort first |
| Determinism break | Low | Host-forked RNG |
| Content overrun | Medium | Budget §13 |
| Track maintenance tedious | Medium | Gang abstraction and schedules |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `rail_network.json` | +36 | 6,000 |
| `rail_logistics_catalog.json` | +24 | 4,000 |
| `railway_interlock_catalog.json` | +15 | 3,000 |
| `locomotives.json` | 10 | 3,000 |
| `train_cars.json` | 20 | 3,500 |
| `bridge_catalog.json` | 12 | 2,500 |
| `track_materials.json` | 12 | 2,000 |
| `track_gang_roles.json` | 8 | 1,500 |
| `rail_schedules.json` | 15 | 2,500 |
| `rail_towns.json` | 6 | 3,000 |
| `rail_interdiction.json` | 12 | 3,000 |
| `gauge_tables.json` | 4 | 1,000 |
| `rail_pacts.json` | 8 | 2,000 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 14 | 2,000 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~68,500** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R25-1 | Second rail system | Low | Critical | Extend owners |
| R25-2 | Rail trivializes roads | Med | High | Costs and threats |
| R25-3 | Bridge collapse unfair | Med | High | Inspection warnings |
| R25-4 | Towns are scenery | Med | High | Real needs and loyalty |
| R25-5 | Interdiction spam | Med | Med | Negotiation first |
| R25-6 | Determinism | Low | High | Host-forked RNG |
| R25-7 | Content overrun | Med | Med | Budget §13 |
| R25-8 | Maintenance tedious | Med | Med | Gang abstraction |
| R25-9 | Schedule confusion | Med | Med | Explicit timetable UI |
| R25-10 | Gauge rules opaque | Low | Med | Visible interchange rules |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Can a bridge ever fully collapse?** Recommended: yes, after inspection warnings
   and with a bypass path.
2. **Does rail movement use expedition time or its own clock?** Recommended: its own
   dispatch clock, still driven by the campaign day.
3. **Can the player own the rail town?** Recommended: no; partner, client, or
   rival, but not property.
4. **Does the armored train ever become an ally?** Recommended: yes, through a
   negotiated toll and a commons pact.
5. **Is gauge conversion worth authoring in Wave 3?** Recommended: yes, one interchange
   point, to make the standard political.

---

## 17. APPENDIX D — RAIL NODE TABLE (20 NODES)

| # | Node | Type | Zone | Role |
|---|---|---|---|---|
| 1 | Holdfast Central Yard *(LIVE)* | Terminal | loc_holdfast | origin |
| 2 | River Delta Junction *(LIVE)* | Junction | loc_river_delta | split |
| 3 | Silo Rail Spur *(LIVE)* | Depot | loc_ruined_silo | freight |
| 4 | Hydro Dam Terminus *(LIVE)* | Terminus | loc_hydro_dam | power tap |
| 5 | Old Switchyard *(LIVE)* | Junction | loc_switchyard | yards |
| 6 | Kilometre Nine | Depot | loc_depot_town | town |
| 7 | Water Stop West | Depot | loc_water_stop | water |
| 8 | Coal Stage North | Depot | loc_coal_stage | fuel |
| 9 | Trestle Junction | Junction | loc_trestle_bridge | bridge |
| 10 | River Span South | Junction | loc_river_bridge | bridge |
| 11 | Black Tunnel East | Terminus | loc_rail_tunnel | tunnel |
| 12 | Gang Camp | Depot | loc_track_gang_camp | maintenance |
| 13 | Wreck Line Siding | Depot | loc_wreck_line | salvage |
| 14 | Armored Siding | Terminus | loc_armored_siding | hostile |
| 15 | The Crossing | Junction | loc_rail_junction | politics |
| 16 | Foundry Spur | Depot | loc_foundry_enclave | industry |
| 17 | Market Platform | Depot | loc_market_town | trade |
| 18 | Deep Bunker Siding | Depot | loc_deep_bunker | shelter |
| 19 | Flood Cutting | Depot | loc_ash_fields | repair |
| 20 | Boundary Post | Terminus | loc_border_checkpoint | limits |

Each node is validated against the canonical topology. A node that references an
unknown zone or segment fails the integrity gate rather than loading silently.

---

## 18. APPENDIX E — RAIL SEGMENT TABLE (24 SEGMENTS)

| # | Segment | From | To | km | Integrity | Bridge | Max mass | Hazard |
|---|---|---|---|---|---|---|---|---|
| 1 | Holdfast–Delta *(LIVE)* | 1 | 2 | 18 | 0.80 | no | 200 t | river |
| 2 | Delta–Silo *(LIVE)* | 2 | 3 | 22 | 0.75 | no | 200 t | wrecks |
| 3 | Delta–Dam *(LIVE)* | 2 | 4 | 25 | 0.70 | yes | 180 t | flood |
| 4 | Delta–Yard *(LIVE)* | 2 | 5 | 12 | 0.85 | no | 220 t | yards |
| 5 | Yard–Nine | 5 | 6 | 14 | 0.65 | no | 200 t | town |
| 6 | Nine–Water West | 6 | 7 | 10 | 0.60 | no | 190 t | curve |
| 7 | Water–Coal | 7 | 8 | 12 | 0.55 | no | 190 t | ash |
| 8 | Coal–Trestle | 8 | 9 | 16 | 0.50 | yes | 150 t | bridge |
| 9 | Trestle–River | 9 | 10 | 8 | 0.55 | yes | 160 t | bridge |
| 10 | River–Tunnel | 10 | 11 | 20 | 0.45 | yes | 140 t | tunnel |
| 11 | Tunnel–Camp | 11 | 12 | 9 | 0.70 | no | 200 t | curve |
| 12 | Camp–Wreck | 12 | 13 | 11 | 0.40 | no | 180 t | wreck |
| 13 | Wreck–Armored | 13 | 14 | 13 | 0.35 | no | 160 t | hostile |
| 14 | Yard–Crossing | 5 | 15 | 15 | 0.60 | no | 200 t | politics |
| 15 | Crossing–Foundry | 15 | 16 | 18 | 0.70 | no | 210 t | industry |
| 16 | Crossing–Market | 15 | 17 | 12 | 0.75 | no | 200 t | trade |
| 17 | Market–Bunker | 17 | 18 | 16 | 0.55 | yes | 170 t | bridge |
| 18 | Bunker–Cutting | 18 | 19 | 10 | 0.50 | no | 180 t | flood |
| 19 | Cutting–Boundary | 19 | 20 | 24 | 0.45 | no | 170 t | limits |
| 20 | Shortcut West | 6 | 12 | 26 | 0.30 | yes | 120 t | broken |
| 21 | Old Mine Spur | 8 | 13 | 15 | 0.25 | no | 140 t | mine |
| 22 | Market Loop | 17 | 5 | 20 | 0.60 | no | 190 t | traffic |
| 23 | Dam–Cutting | 4 | 19 | 30 | 0.35 | yes | 130 t | flood |
| 24 | Boundary Turn | 20 | 6 | 40 | 0.25 | yes | 110 t | remote |

Track condition is the line's real currency. The corridor is runnable from the start
and genuinely unreliable, which is the expansion's intended tension.

---

## 19. APPENDIX F — BRIDGE TABLE (12 BRIDGES)

| # | Bridge | Segment | Span type | Length | Load rating | Piers | Scour risk |
|---|---|---|---|---|---|---|---|
| 1 | Delta Truss | 3 | truss | 60 m | 180 t | 2 | med |
| 2 | High Trestle | 8 | trestle | 220 m | 150 t | 9 | low |
| 3 | River Span | 9 | girder | 140 m | 160 t | 3 | high |
| 4 | Eden Bridge | 10 | truss | 100 m | 140 t | 2 | high |
| 5 | Bunker Span | 17 | girder | 80 m | 170 t | 2 | med |
| 6 | Shortcut Span | 20 | beam | 45 m | 120 t | 1 | low |
| 7 | Dam Span | 23 | arch | 90 m | 130 t | 2 | high |
| 8 | Turn Span | 24 | beam | 55 m | 110 t | 1 | med |
| 9 | Yard Flyover | 4 | girder | 40 m | 220 t | 1 | low |
| 10 | Market Overpass | 16 | slab | 35 m | 200 t | 1 | low |
| 11 | Cutting Culvert | 18 | culvert | 18 m | 180 t | 0 | med |
| 12 | Wreck Wash | 12 | beam | 30 m | 160 t | 1 | high |

Bridges are the line's bottlenecks and its monuments. The expansion's most important
number is not speed; it is the load rating on the River Span.

---

## 20. APPENDIX G — LOCOMOTIVE TABLE (10 ENGINES)

| # | Locomotive | Class | Mass | Tractive | Fuel/km | Water/km | Speed | Crew |
|---|---|---|---|---|---|---|---|---|
| 1 | Yard Restoration | steam | 42 t | 0.32 | 12 coal | 90 L | 40 | 3 |
| 2 | Delta Shunter | steam | 30 t | 0.28 | 8 coal | 70 L | 30 | 2 |
| 3 | Long Hauler | steam | 58 t | 0.36 | 18 coal | 130 L | 45 | 4 |
| 4 | Light Draisine | petrol | 6 t | 0.20 | 4 petrol | 0 | 50 | 1 |
| 5 | Armored Engine | steam | 70 t | 0.30 | 22 coal | 150 L | 38 | 6 |
| 6 | Salvage Mule | diesel | 36 t | 0.34 | 10 diesel | 0 | 42 | 3 |
| 7 | Fireless Shunter | steam | 28 t | 0.22 | 0 stored | 0 | 20 | 2 |
| 8 | Mountain Engine | steam | 66 t | 0.40 | 20 coal | 140 L | 32 | 5 |
| 9 | Clinic Engine | steam | 44 t | 0.26 | 12 coal | 90 L | 40 | 4 |
| 10 | Wreck Rebuild | steam | 38 t | 0.25 | 11 coal | 85 L | 28 | 3 |

Every locomotive has an appetite. A train that runs is a train that consumes coal,
water, and maintenance, and the expansion never lets rail become free transport.

---

## 21. APPENDIX H — TRAIN CAR TABLE (20 CARS)

| # | Car | Mass | Capacity | Armor | Role |
|---|---|---|---|---|---|
| 1 | Box Car | 18 t | 40 t | low | freight |
| 2 | Flat Car | 14 t | 45 t | none | heavy |
| 3 | Gondola | 16 t | 50 t | none | bulk |
| 4 | Tank Car | 20 t | 40 kL | low | liquid |
| 5 | Reefer Car | 22 t | 35 t | low | cold |
| 6 | Covered Hopper | 18 t | 48 t | low | grain |
| 7 | Ore Car | 20 t | 55 t | low | ore |
| 8 | Flat with Crane | 24 t | 30 t | low | recovery |
| 9 | Caboose | 12 t | living | low | crew |
| 10 | Clinic Car | 22 t | 12 beds | med | medical |
| 11 | Workshop Car | 24 t | tools | low | repair |
| 12 | Kitchen Car | 18 t | meals | low | food |
| 13 | Water Car | 20 t | 30 kL | none | water |
| 14 | Gun Car | 26 t | turret | high | defense |
| 15 | Armored Box | 28 t | 30 t | high | escort |
| 16 | Prison Car | 24 t | 8 cells | high | custody |
| 17 | Signal Car | 16 t | radio | low | comms |
| 18 | Refuge Car | 20 t | 20 people | med | evacuation |
| 19 | Cattle Car | 18 t | 30 head | none | livestock |
| 20 | Derby Car | 15 t | 25 t | none | utility |

Cars are authored with real roles. A clinic car matters more than a gun car on most
runs, and the expansion lets the player learn that.

---

## 22. APPENDIX I — TRACK MATERIAL TABLE (12 MATERIALS)

| # | Material | Use | Wear unit | Replacement | Source |
|---|---|---|---|---|---|
| 1 | Rail Length | rail | per 100 km | 1 per 100 km | foundry |
| 2 | Crosstie | ties | per 50 km | 1 per 50 km | timber |
| 3 | Spike Box | fastening | per 200 km | 1 per 200 km | forge |
| 4 | Ballast Load | drainage | per 150 km | 1 per 150 km | quarry |
| 5 | Fishplate | joint | per 300 km | 1 per 300 km | foundry |
| 6 | Switch Frog | junction | per 500 km | 1 per 500 km | foundry |
| 7 | Switch Stand | junction | per 400 km | 1 per 400 km | workshop |
| 8 | Signal Lamp | signal | per 200 days | 1 per 200 days | glassworks |
| 9 | Signal Wire | signal | per 300 km | 1 per 300 km | workshop |
| 10 | Grinding Stone | maintenance | per 80 km | 1 per 80 km | quarry |
| 11 | Bridge Beam | bridge | per repair | 1 per repair | foundry |
| 12 | Blockhouse Plate | defense | per repair | 1 per repair | foundry |

Track is a consumable. This table is why a rail line is an ongoing expense rather
than a one-time build, and why the maintenance quest chain exists.

---

## 23. APPENDIX J — TRACK GANG ROLE TABLE (8 ROLES)

| # | Role | Skill | Output | Fatigue | Risk |
|---|---|---|---|---|---|
| 1 | Section Boss | leadership | access, plan | low | low |
| 2 | Surveyor | knowledge | grade, clearance | med | low |
| 3 | Spiker | labor | track laid | high | med |
| 4 | Grinder | machining | rail dressed | med | med |
| 5 | Welder | machining | joints, repairs | med | high |
| 6 | Flagman | vigilance | safety | med | high |
| 7 | Guard | combat skill | protection | med | high |
| 8 | Cook | cooking | morale | low | low |

Gangs are small, specialized, and expensive to replace. A shelter that trains a
welder and loses them loses a rail corridor's capacity with them.

---

## 24. APPENDIX K — SCHEDULE TABLE (15 SERVICES)

| # | Service | Window | Priority | Cargo | Meet rule |
|---|---|---|---|---|---|
| 1 | Shelter Freight | dawn | high | mixed | hold for none |
| 2 | Town Freight | mid | med | supplies | hold for 1 |
| 3 | Grain Run | dawn | med | grain | hold for 2 |
| 4 | Ore Run | dusk | high | ore | hold for 1 |
| 5 | Medical Run | urgent | critical | medicine | hold for none |
| 6 | Water Run | dawn | high | water | hold for none |
| 7 | Coal Run | dusk | high | coal | hold for 1 |
| 8 | Market Run | weekend | med | trade | hold for 2 |
| 9 | Mail Run | mid | low | mail | hold for 3 |
| 10 | Patrol Run | night | high | guards | hold for none |
| 11 | Maintenance Run | day | low | materials | hold for all |
| 12 | Evacuation Run | emergency | critical | people | hold for none |
| 13 | Aid Run | emergency | critical | aid | hold for none |
| 14 | Salvage Run | flexible | low | scrap | hold for all |
| 15 | Inspection Run | monthly | med | engineers | hold for 1 |

Schedules create obligations: a Met hold means a delayed train, and a delayed train
means a later shortage. Priority is editorial and political, which is why the town
argues about it.

---

## 25. APPENDIX L — RAIL TOWN TABLE (6 TOWNS)

| # | Town | Population | Founded | Needs | Loyalty drivers | Trade |
|---|---|---|---|---|---|---|
| 1 | Kilometre Nine | 40 | pre-war depot | food, fuel | schedule, safety | grain |
| 2 | Water Stop West | 12 | post-war | water goods | maintenance | water |
| 3 | Coal Stage North | 18 | post-war | food, tools | fuel price | coal |
| 4 | The Crossing | 25 | junction | everything | tolls, law | services |
| 5 | Market Platform | 60 | market | security | trade access | goods |
| 6 | Bunker Siding | 8 | shelter outpost | shelter goods | protection | logistics |

Towns grow with traffic and starve without it. Their loyalty is earned by schedules,
protection, and fair prices, and lost by exclusion and extraction.

---

## 26. APPENDIX M — INTERDICTION TABLE (12 THREATS)

| # | Threat | Method | Escalation | Counter |
|---|---|---|---|---|
| 1 | Track gang bandits | steal rail | sabotage | patrol, pay |
| 2 | Toll collectors | stop trains | raise toll | negotiate, escort |
| 3 | Armored train | blockade | demand people | commons pact, defense |
| 4 | Bridge saboteurs | cut spans | collapse | inspection, guard |
| 5 | Signal tamperers | false aspects | collision risk | token system |
| 6 | Route denial device | block route | repeated denial | clear, counter-device |
| 7 | Fuel thieves | drain tanks | arson | guards, tanks |
| 8 | Cargo hijack | stop and take | hostages | escort, speed |
| 9 | Switches seized | jam junction | isolation | repair, defend |
| 10 | Town riot | block track | closure | negotiate, supply |
| 11 | Rival line | build around | traffic loss | pact, speed |
| 12 | Weather closure | floods, scour | seasonal | inspection, bypass |

Interdiction is authored as pressure and negotiation first and confrontation second.
Any physical confrontation routes through the live combat and encounter authorities.

---

## 27. APPENDIX N — GAUGE TABLE

| Gauge | Width class | Used by | Interchange | Conversion |
|---|---|---|---|---|
| Standard | baseline | shelter, towns | full | n/a |
| Narrow | small | mine spur | partial | 3 days |
| Broad | heavy | pre-war mainline | rare | 5 days |
| Wreck | unknown | abandoned line | none | rebuild |

Gauge mismatch is the expansion's quiet political lever: a town on a different gauge
can receive cargo, but cannot run through. Standardizing the line is a real
achievement and a real negotiation.

---

## 28. APPENDIX O — RAIL PACT TABLE (8 PACTS)

| # | Partner | Section | Toll | Maintenance | Priority | Breach |
|---|---|---|---|---|---|---|
| 1 | Kilometre Nine | 5–7 | 5% | shared | fair | standing |
| 2 | Market Platform | 16–17 | 8% | town | town | standing |
| 3 | Coal Stage | 7–8 | coal price | town | fair | warning |
| 4 | Foundry Enclave | 15–16 | trade | enclave | industry | standing |
| 5 | Deep Bunker | 17–18 | reciprocal | bunker | shelter | review |
| 6 | The Crossing | 14 | 10% | shared | toll | crisis |
| 7 | Armored Siding | 12–13 | tribute | none | raider | crisis |
| 8 | Boundary Post | 19–20 | none | shelter | shelter | review |

A rail commons is a written timetable and a maintenance share. It is the expansion's
best ending and its hardest to reach, because every partner must give up the ability
to stop the other's train.

---

## 29. APPENDIX P — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_rail_survey` | 4 | Walk the corridor; map spans and cuttings |
| `quest_rail_argument` | 4 | Road vs. rail; agree a first objective |
| `quest_rail_cutting` | 5 | Drain, ballast, and reopen a flooded cutting |
| `quest_rail_first_train` | 5 | Restore a locomotive; run a test |
| `quest_rail_crossing` | 4 | The armored train stops the test |
| `quest_rail_tunnel` | 4 | Clear and ventilate the tunnel |
| `quest_rail_town` | 5 | Nine wants a schedule and a share |
| `quest_rail_trestle` | 4 | Load-test the high trestle |
| `quest_rail_gang` | 4 | Settle the section boss's claim |
| `quest_rail_theft` | 4 | Recover stolen rail and spikes |
| `quest_rail_toll` | 5 | Answer the raised toll |
| `quest_rail_storm` | 4 | Scour closes the River Span |
| `quest_rail_meet` | 4 | Two trains, one passing loop |
| `quest_rail_reckoning` | 5 | Run, share, or abandon the line |
| `quest_rail_line_holds` | 3 | Final disposition; epilogue |

---

## 30. APPENDIX Q — NPC DOSSIERS (BRIEF)

**Vera Kast** — railway engineer. Builds to standard and refuses shortcuts, because
she has seen what a shortcut does at speed. Not a romantic; an engineer with a
bridge she does not trust.

**Dorn Hale** — caravan master. Defends the road because the road has kept the
shelter alive. His conversion to rail is earned, if it happens, by a train that
actually returns.

**Mara Osk** — section boss. Owns a stretch she maintains with her own gang and
resents anyone who assumes access. The expansion's lesson that infrastructure has
people.

**Lin Vey** — dispatcher. Keeps the board, the tokens, and the timetable. Believes a
schedule is a promise and gets quietly furious when it is broken.

**Hask Orr** — depot town mayor. Wants the line and the leverage it brings. Not a
villain; a small-town politician in a survival economy.

**Grale** — armored train captain. Demands tolls because his train eats coal and his
people eat, and he is entirely willing to negotiate. Represents organized extortion
as a rational system.

**Koval** — crew lead. Knows the line's sounds by ear and fears one bridge in
particular. His fear is correct and the expansion rewards listening.

**Pim** — child of the depot. Grows up with the trains; the reason the town is not
scenery.

---

## 31. APPENDIX R — LOCATION DETAIL

- **The Switchyard** — rows of rusted switches; one still moves.
- **The High Trestle** — a long, high span; the wind crosses it before the train does.
- **The River Span** — piers in moving water; the load rating is painted on a plate.
- **The Black Tunnel** — dark, wet, and too low at one point.
- **The Gang Camp** — bunks, a cooker, and a section claim nailed to a post.
- **Depot Town** — water, coal, a platform, and people who expect trains.
- **The Water Stop** — a tank and a spout; the line's most repeated ritual.
- **The Wreck Line** — a derailed train and everything worth taking.
- **The Armored Siding** — a hostile train with a toll board and a coal pile.
- **The Crossing** — two lines meet; the politics are painted on a shed.

---

## 32. APPENDIX S — TRACK WEAR AND MAINTENANCE MODEL

| Input | Wear effect | Warning | Fix |
|---|---|---|---|
| Train pass | base wear | condition drop | grind |
| Loaded train | heavier wear | risk rise | inspect |
| Wet season | drainage wear | soft spots | ballast |
| Frost | tie cracks | visible | replace |
| Sand | rail polish | low | clean |
| Wreck debris | gouges | visible | repair |
| Flood | washout | severe | rebuild |
| Neglect | cumulative | derailment chance | full relay |

Track condition is shown as a number and as a warning state. A line that is losing
condition tells the player, and a line that is ignored tells them with a derailment.

---

## 33. APPENDIX T — BRIDGE LOAD MODEL

A train crosses a bridge if its total mass is below the current load rating. The
rating changes with condition:

```
current_rating = base_rating * condition_factor * scour_factor
cross_ok = train_mass <= current_rating
```

| Factor | Range | Cause |
|---|---|---|
| condition_factor | 0.4–1.0 | deck, beams, piers |
| scour_factor | 0.5–1.0 | flooding, piers |
| overload_margin | 0–10% | authored tolerance |

An over-limit train can be cut into sections, rerouted, or risked. The expansion
makes the player choose, and Koval's fear is the authored voice telling them which
choice is wise.

---

## 34. APPENDIX U — WORKED 360-DAY RAIL SCENARIO

**Days 1–40.** Survey finds the corridor. The road advocates and rail advocates
settle on a first objective: the flooded cutting. A gang is hired and rails laid on
18 kilometres.

**Days 41–90.** A locomotive is restored from the wreck line. The first test run is
stopped at the crossing by the armored train, which demands a toll in grain. The
shelter pays once and begins planning.

**Days 91–150.** The tunnel is cleared. Nine asks for a schedule and a share; the
first timetable is written. The high trestle is load-tested at 150 tonnes and found
marginal.

**Days 151–220.** A section boss refuses access; negotiation or force. Rail is
stolen; a patrol recovers it. The River Span takes storm scour and closes for two
weeks; the bypass adds a day to every run.

**Days 221–300.** The toll is raised. The shelter escorts a run, builds a blockhouse,
or signs a commons pact. The first town freight runs on schedule; Nine's loyalty
rises.

**Days 301–360.** The line is run, shared, or abandoned. The season's ledger shows
what rail saved in fuel and what it cost in maintenance. The epilogue records who
owns the ground the line crosses.

---

## 35. APPENDIX V — VIGNETTE (TONE SAMPLE)

> The whistle is two notes, and the second one is always late, and everyone in the
depot looks up anyway. Pim runs to the platform rail and stands on the tie beside it,
because the tie is exactly the right height, and Mara does not tell her to get back
because she was that age once.

> The locomotive comes around the cutting at walking pace with its lamps on, and
> behind it the grain cars are still moving the way loaded cars move, heavy and
> patient, and Koval leans out and looks at the bridge as they cross it, the way he
> always does, and then the span is behind them and he lets his breath out.

---

## 36. APPENDIX W — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| Derailment | wreck, delay | flail, rerail, rebuild |
| Bridge scour | closure | repair, bypass |
| Track washout | closure | ballast, relay |
| Locomotive failure | no service | repair, spare |
| Fuel shortage | reduced runs | ration, stage |
| Water shortage | reduced runs | tank car, stop |
| Signal failure | safety risk | token system |
| Interdiction | theft, toll | escort, negotiate |
| Town unrest | closure | supply, talks |
| Schedule collapse | trade loss | re-plan, prioritize |

No failure is a game over. Every failure has a recovery path, and every recovery
costs material, time, or standing. The deepest failure is a line that is rebuilt and
then allowed to wear out.

---

## 37. APPENDIX X — CONTENT REVIEW CHECKLIST

- [ ] `RailwaySystem` remains the topology and dispatch authority.
- [ ] `RailwayInterlockEngine` remains the route-legality authority.
- [ ] No other system moves a train.
- [ ] Every topology reference validates against the canonical catalog.
- [ ] Bridges warn before collapse.
- [ ] Track wear is visible and repairable.
- [ ] Interdiction prefers negotiation and escort.
- [ ] Rail towns have needs and loyalty.
- [ ] Rail commons goes through `FactionStanceEngine`.
- [ ] Save additions are additive and legacy-neutral.
- [ ] Determinism uses the host-forked RNG only.

---

## 38. APPENDIX Y — GLOSSARY

- **Topology** — the canonical rail graph owned by `RailwaySystem`.
- **Edge** — a logistics record for a segment with gauge and condition.
- **Interlock** — the route legality, signal, and reservation authority.
- **Token** — a physical authority to occupy a section.
- **Load rating** — the mass a bridge can currently carry.
- **Scour** — water-driven pier damage.
- **Wear** — cumulative rail condition loss per train pass.
- **Gauge** — track width class; mismatch blocks through-running.
- **Meet** — two trains passing on a schedule.
- **Commons** — a written shared-line agreement.

---

## 39. APPENDIX Z — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `RailwaySystem` | topology | topology, dispatch | interlock |
| `RailwayInterlockEngine` | topology | locks, signals | movement |
| `RailLogisticsCatalog` | edges | — | — |
| `RailGrindingEngine` | wear | grinding | topology |
| `AmphibiousDraisineEngine` | routes | draisine state | — |
| `DraisineRerailingSystem` | wreck | recovery | — |
| `VehicleGarageSystem` | road | vehicles | rail |
| `WeatherSystem` | weather | — | — |
| `PowerGridSystem` | draw | — | — |
| `SilentFoundrySystem` | parts | products | — |
| `TradingSystem` | cargo | trade | — |
| `FactionStanceEngine` | pacts | standing | — |
| `ExpeditionSystem` | escorts | expeditions | — |
| `NeedsSystem` | towns | — | — |
| `MemorialSystem` | deaths | memorials | — |

---

## 40. APPENDIX AA — DATA SCHEMA DETAIL (NEW CATALOGS)

**`locomotives.json`** — `locomotive_id`, `display_name`, `power_class`, `mass_kg`,
`tractive_effort`, `fuel_item_id`, `fuel_per_km`, `water_per_km`, `max_speed_kmh`,
`warmup_hours`, `maintenance_interval_days`, `wear_per_100km_bp`, `crew_required`,
`tags`.

**`train_cars.json`** — `car_type_id`, `display_name`, `mass`, `cargo_capacity`,
`armor_rating`, `coupler_class`, `car_role`, `special_tags[]`, `tags`.

**`bridge_catalog.json`** — `bridge_id`, `display_name`, `segment_id`,
`span_type`, `length_m`, `load_rating`, `pier_count`, `scour_risk`,
`inspection_interval_days`, `repair_materials[]`, `tags`.

**`track_materials.json`** — `material_id`, `display_name`, `use`, `wear_unit`,
`replacement_rate`, `source`, `tags`.

**`track_gang_roles.json`** — `role_id`, `display_name`, `required_skill`,
`output`, `fatigue`, `risk`, `tags`.

**`rail_schedules.json`** — `schedule_id`, `display_name`, `departure_window`,
`priority`, `cargo_class`, `meet_rule`, `obligation`, `tags`.

**`rail_towns.json`** — `town_id`, `display_name`, `depot_node_id`, `population`,
`needs[]`, `loyalty_drivers[]`, `trade_goods[]`, `growth_rules`, `tags`.

**`rail_interdiction.json`** — `threat_id`, `display_name`, `method`,
`escalation[]`, `counter_options[]`, `standing_effect`, `tags`.

**`gauge_tables.json`** — `gauge_id`, `display_name`, `width_class`, `used_by[]`,
`interchange`, `conversion_days`, `tags`.

**`rail_pacts.json`** — `pact_id`, `partner_faction_id`, `section_ids[]`, `toll`,
`maintenance_share`, `priority`, `breach_effect`, `tags`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on missing or
duplicate IDs, invalid topology/zone/faction references, or out-of-range numbers.

---

## 41. APPENDIX AB — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Tonne-km per day | line usefulness | RailwaySystem |
| Fuel per tonne-km | rail advantage | LocomotiveSystem |
| Track condition average | maintenance load | TrackMaintenanceSystem |
| Bridge closures | fragility | BridgeSystem |
| Derailments | safety | RailwaySystem |
| Meets and delays | schedule quality | RailScheduleSystem |
| Town loyalty | politics | RailTownSystem |
| Toll paid | dependence | RailInterdictionSystem |
| Gauge conversions | standard | GaugeSystem |
| Pacts signed | commons | RailCommonsSystem |

Telemetry is diagnostic only; it never gates content and never becomes a hidden
score. It exists so the team can tell whether the line feels worth building.

---

## 42. APPENDIX AC — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored, validated against topology, and scanner-registered.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive, legacy-neutral, and round-trip tested.
- [ ] No non-rail system moves a train.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling checks.
- [ ] Phase 6 content passes the review checklist in §37.
- [ ] Phase 7 soak shows maintenance, weather, towns, and tolls over a year.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No parallel rail, interlock, or movement system exists.

---

## 43. APPENDIX AD — OPEN QUESTIONS FOR REVIEW

1. Should a train crew ever be lost in a derailment?
2. Should bridges be repairable in place, or only replaceable?
3. Should towns ever secede from the common schedule?
4. Should the armored train be defeatable without combat?
5. Should track wear apply to the whole line or only segments used?
6. Should gauge conversion be possible at multiple points?
7. Should rail commons require unanimous consent?
8. Should schedules be player-authored or survivor-authored?

None of these may be decided unilaterally; each changes the expansion's balance and
politics.

---

## 44. APPENDIX AE — CLOSING VIGNETTE

> The timetable is nailed to the shed door, and it is wrong, and Lin knows it is
> wrong, and she takes it down and writes a new one with the window changed by forty
> minutes because the river has been slow all week.

> At the water stop, the spout runs and the tank car fills, and Pim watches the
> water fall and asks whether the trains will still come in winter, and Mara says
> they will come if the line is kept, and the line is kept if people keep it, which
> is the entire answer and not a satisfying one.

> Down the track, past the cutting, the trestle stands over the gap and waits for
> the next train to test it again.

---

## 46. APPENDIX AF — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_rail_corridor_survey` | 4 | Walk, record, map, publish |
| `quest_rail_grade_check` | 3 | Measure, flag, plan |
| `quest_rail_clearance` | 4 | Measure tunnel, platforms, loads |
| `quest_rail_landmark` | 3 | Set posts, paint, log |
| `quest_rail_right_of_way` | 5 | Ask, negotiate, agree, mark |
| `quest_rail_ballast` | 4 | Dig, haul, spread, tamp |
| `quest_rail_tie_replace` | 4 | Cut, fit, spike, check |
| `quest_rail_rail_gang` | 5 | Lay, join, gauge, test |
| `quest_rail_gauge_fix` | 3 | Measure, adjust, verify |
| `quest_rail_spike_run` | 3 | Forge, box, deliver |
| `quest_rail_bridge_inspect` | 4 | Deck, beams, piers, report |
| `quest_rail_pier_repair` | 5 | Dewater, patch, reinforce, test |
| `quest_rail_span_replace` | 6 | Lift, set, join, load test |
| `quest_rail_tunnel_vent` | 4 | Clear, vent, light, certify |
| `quest_rail_underpin` | 4 | Excavate, pour, cure, verify |
| `quest_rail_locomotive_restore` | 6 | Strip, repair, reassemble, fire |
| `quest_rail_car_build` | 4 | Frame, deck, brake, couple |
| `quest_rail_crew_hire` | 4 | Recruit, test, qualify, roster |
| `quest_rail_water_stop` | 4 | Build, fill, shelter, connect |
| `quest_rail_coal_stage` | 4 | Build, stock, convey, ash |
| `quest_rail_switch_repair` | 4 | Inspect, replace, align, test |
| `quest_rail_signal_restore` | 4 | Wire, lamp, aspect, verify |
| `quest_rail_token` | 3 | Make, issue, log, return |
| `quest_rail_timetable` | 5 | Draft, argue, publish, run |
| `quest_rail_meet_plan` | 4 | Choose loop, timing, priority, test |
| `quest_rail_depot_town` | 5 | Platform, water, law, charter |
| `quest_rail_town_needs` | 4 | Audit, supply, schedule, review |
| `quest_rail_section_claim` | 5 | Meet, negotiate, share, sign |
| `quest_rail_toll_talk` | 4 | Meet, offer, counter, agree |
| `quest_rail_commons_pact` | 5 | Draft, round, sign, run |
| `quest_rail_escort` | 4 | Load, ride, watch, deliver |
| `quest_rail_blockhouse` | 4 | Site, build, man, signal |
| `quest_rail_patrol` | 3 | Walk, observe, report |
| `quest_rail_wreck_clear` | 5 | Flail, lift, rerail, reopen |
| `quest_rail_denial_counter` | 4 | Find, disarm, clear, log |

---

## 47. APPENDIX AG — REGIONAL RAIL MAP

| Settlement | Access | Distance | Gauge | Interest |
|---|---|---|---|---|
| The shelter | origin | 0 km | standard | control |
| Kilometre Nine | depot | 52 km | standard | share |
| Market Platform | depot | 40 km | standard | trade |
| Coal Stage | depot | 62 km | standard | fuel price |
| The Crossing | junction | 30 km | standard | tolls |
| Foundry Enclave | depot | 48 km | standard | freight |
| Deep Bunker | depot | 64 km | narrow | isolation |
| Armored Siding | terminus | 78 km | standard | tribute |
| Boundary Post | terminus | 98 km | wreck | none |

Every stop has a different interest, which is why a shared timetable is politics and
not engineering. The map also shows why the crossing matters: it is where two lines
and two interests meet.

---

## 48. APPENDIX AH — DERAILMENT AND RECOVERY MODEL

| Cause | Probability driver | Effect | Recovery |
|---|---|---|---|
| Track wear | condition below threshold | cars off | flail, rerail |
| Broken rail | fatigue | derail | replace |
| Switch defect | misalignment | wrong route | repair |
| Overload | mass over rating | bridge or track failure | cut train |
| Obstruction | debris, denial | stop | clear |
| Weather | flood, ice | track loss | rebuild |
| Signal error | tamper, fault | collision risk | token system |
| Human error | fatigue, speed | derail | discipline |

Derailments are rare, warned, and recoverable. The expansion's purpose is not to
punish the player but to make maintenance feel like the thing that prevents them.

---

## 49. APPENDIX AI — WORKS AND MAINTENANCE COSTS

| Work | Materials | Labor days | Time | Effect |
|---|---|---|---|---|
| Grind 10 km | stone | 2 | 1 day | condition + |
| Replace 20 ties | 20 ties | 3 | 2 days | condition + |
| Relay 1 km | rail, ties, spikes | 8 | 4 days | full restore |
| Patch bridge | beams | 6 | 3 days | rating + |
| Pier repair | concrete | 10 | 7 days | scour − |
| Tunnel vent | fans | 5 | 4 days | clearance |
| Signaling | wire, lamps | 4 | 3 days | legality |
| Switch rebuild | frog, stand | 6 | 2 days | junction |

Every work is authored with real materials from the foundry and quarry. Rail is a
sink for the shelter's industry, which is exactly the point: the line is worth
keeping only if the shelter can afford to keep it.

---

## 50. APPENDIX AJ — LORE: THE MAINLINE

The pre-war world ran a regional mainline and the shelter inherited its bones. The
fiction:

- **The Mainline** connected the cities and the mines; its embankment survived the
  Exchange better than its bridges.
- **The Crossing** was a junction where two companies met; their rivalry is the
  reason the shed still has two approaches.
- **The High Trestle** was a timber and steel span built cheaply and maintained
  expensively, which is the shelter's problem now.
- **Kilometre Nine** was a maintenance depot before it was a town; its water tank is
  why people stayed.
- **The Armored Siding** was a military railhead; the armored train that uses it is
  a post-war improvisation built on a pre-war chassis.

No real railway, company, or locomotive class is named. The Mainline is fictional and
exists to explain why the wasteland's rail knowledge is partially recoverable.

---

## 51. APPENDIX AK — WORKED RAIL ECONOMY

| Corridor | Road cost/100 t | Rail cost/100 t | Time | Risk |
|---|---|---|---|---|
| 50 km | 180 fuel | 40 coal | 1 day | road ambush |
| 100 km | 360 fuel | 80 coal | 2 days | road ambush |
| 200 km | 800 fuel | 160 coal | 4 days | road, weather |
| 300 km | 1400 fuel | 240 coal | 6 days | road, loss |

Rail's advantage is real and conditional: it is only cheaper if the line is
maintained, the bridge holds, and the toll is reasonable. The expansion's intended
lesson is that the line rewards investment and punishes neglect, which is the same
lesson every other survival system teaches.

---

## 52. APPENDIX AL — MEASUREMENT AND REVIEW CADENCE

| Gate | Question | Evidence |
|---|---|---|
| Premise | Do the live owners still match? | file:line audit |
| Data | Are all rows valid against topology? | integrity + scanner |
| Core | Are the systems pure and deterministic? | unit tests |
| Persistence | Does legacy load neutral? | round-trip tests |
| Host | Is every command real? | selftest + journey |
| UI | Is track risk honest? | lifecycle + a11y tests |
| Balance | Is rail worth building? | long soak |
| Politics | Are towns and pacts real? | review + soak |

The balance gate is the hard one. Rail must be decisively better than road over time
and decisively more expensive to build, or the expansion has failed.

---

## 53. CLOSING STATEMENT

ASHFALL already owns rail the right way: one topology authority, one interlock that
validates every reference, dispatch with real statuses, track integrity and load
limits, grinding and rerailing, and a clear rule that no other system moves a train.
What it lacks is a region for the rails to run through: bridges worth maintaining,
track gangs with claims, locomotives with appetites, towns that grow at depots,
scheduled meets that matter, and an armored train that wants a toll. The Iron Road
adds that region without adding a second rail system. It adds a timetable, a span, a
section boss, and the oldest question of infrastructure: who owns the ground the line
crosses?

> Wave 3 note: this plan is one of five Wave 3 expansion bibles (22–26). Each is
> self-contained; none requires another to ship. The shared Wave 3 index lives at
> `docs/expansions/wave3/WAVE3_INDEX.md`. The safe pre-signature step is Phase 1
> (data schemas and validators), which is additive and reversible. Evidence anchors
> used throughout: `RailwaySystem`, `RailwayInterlockEngine`, `RailLogisticsCatalog`,
> `RailGrindingEngine`, the draisine and flail engines, and the live `railway` save
> section.