# ASHFALL — Expansion 34 Design Bible
# THE LONG ROAD
### Wave 5 · Highway Corridors, Bridges, Fords, Ferries, Waystations, Convoys, and Route Clearance

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-22
**Domain owners touched:** `Ashfall.Core.World` (RouteInfrastructureSystem), `Ashfall.Core` (ExpeditionVehicleSystem, VehicleGarageSystem), `Ashfall.Core.World` (WeatherGate), `Ashfall.Core` (caravan route data)
**Proposed host owner:** `RoadwaysHostSession` (extends route, garage, and waystation surfaces)
**Existing save sections:** route infrastructure state, vehicle state, weather gate state
**Existing CLI verbs:** `--data-integrity-selftest`, `--content-utilization-selftest`, vehicle/infrastructure selftests
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already records route state. `RouteInfrastructureSystem` (16.7 KB)
defines `RouteSegmentInfrastructureRecord` with route and segment ids, a `Mode`
of road, rail, subway, service tunnel, or offroad, a `ClearanceState` of
unbreached, partial, cleared, or secured, a speed limit, a roughness index, and
a last-modified day. It defines `MinefieldSegmentState` with density, detection,
cleared fraction, last-cleared day, and residual risk. It defines
`RailSegmentCondition` with roughness, profile error, surface defects, rust
scale, safe speed limits, and grounding. It exposes `GetSpeedLimit`,
`GetHazardModifier`, `GetTravelModifier`, and `CanTraverse` with vehicle
capabilities. The data is real: `waystations.json` (10.8 KB) already carries
named stations with keepers, specialties, condition, filter health, defense
rating, services (trade, staging, rest, filter recharge), and stock;
`caravan_trade_routes.json` (6.7 KB), `caravans.json` (4.2 KB), and
`merchant_caravans.json` (3.3 KB) define routes and traffic;
`weather_route_gates.json` (11 KB) gates weather-threatened segments.
`ExpeditionVehicleSystem`, `VehicleGarageSystem`, `vehicles.json` (3 KB),
`vehicle_modifications.json` (4.9 KB), and `vehicle_armor_grades.json` (4.6 KB)
cover vehicles. Expansion 01 owns the frozen ice road; Wave 3's Iron Road owns
rail.

What does not exist: road corridors as authored content, bridges and fords and
ferries as structures with state, convoys as scheduled operations, clearance
work beyond minefields, road maintenance, roadside dangers, and tolls or
agreements between settlements.

**The Long Road** turns the live route system into the shelter's circulatory
system: the corridors, crossings, waystations, convoys, and clearance crews that
move people, goods, and news across a broken landscape.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

A shelter with one working road is a shelter with one artery. When the bridge
goes, the shelter learns which one it was.

**The Long Road** is the expansion about movement: highway corridors, bridges
and fords and ferries, waystations, convoys, cargo, clearance, maintenance,
roadside danger, and the agreements that let strangers pass. It extends the live
route infrastructure system with authored segments, structures, operations, and
crews, and it wires every effect through the owners that already exist: vehicles
keep their garage and condition systems, weather keeps its gates, economy keeps
trade, and combat keeps its rules.

The expansion's hard rules follow the live owners: `RouteInfrastructureSystem`
remains the segment authority, `WeatherGate` keeps weather gating, the ice road
and rail keep their own authorities, vehicles keep their condition and armor
models, and no second road, vehicle, or trade system is created.

### 1.2 The five loops it adds

```
   Scout ──► Survey ──► Clear ──► Open route ──► Keep it open
     │         │          │           │              │
     ▼         ▼          ▼           ▼              ▼
   Threats,  Segments   Mines,      Convoys,     Maintenance,
   crossings in state   debris,     schedules,   drainage,
                        collapse    cargo        grading
                                       │
                                       ▼
                              Waystations ──► Rest, fuel, trade,
                              keepers         repairs, warnings
                                       │
                                       ▼
                              Tolls & agreements ──► Passage rights
```

### 1.3 What the player manages

1. **Corridors.** Authored roads with segments, condition, and seasonal
   behavior.
2. **Crossings.** Bridges, fords, ferries, and culverts as structures with
   state, inspection, and repair.
3. **Convoys.** Routes, schedules, vehicles, cargo, escorts, and losses.
4. **Clearance.** Debris, slides, wrecks, and minefields cleared by crews with
   real risk.
5. **Waystations.** Keepers, services, defense, fuel, rest, and warnings.
6. **Maintenance.** Grading, drainage, surfacing, and winter work.
7. **Danger.** Banditry, breakdowns, weather, and the cost of a stopped convoy.
8. **Agreements.** Tolls, passage rights, and shelter-to-shelter arrangements.

### 1.4 What it is not

- Not a second vehicle system. `ExpeditionVehicleSystem` and the garage keep
  vehicles.
- Not a second rail system. Wave 3's Iron Road owns rail.
- Not a second ice road. Expansion 01 owns frozen convoys.
- Not a second weather system. `WeatherGate` owns weather gating.
- Not a second economy. Prices and trade stay with the economy authority.
- Not a new save section.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/World/RouteInfrastructureSystem.cs` | Segments, modes, clearance, minefields, speed | `LIVE` |
| `Assets/Ashfall.Core/World/WeatherGate.cs` | Weather route gating | `LIVE` |
| `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs` | Vehicle expeditions | `LIVE` |
| `Assets/Ashfall.Core/Shelter/VehicleGarageSystem.cs` | Garage and maintenance | `LIVE` |
| `Assets/Ashfall.Core/World/PatrolTerritoryAuthority.cs` | Territory control | `LIVE` |
| `Assets/Ashfall.Core/IceRoadSystem.cs` | Ice road (Exp 01) | `LIVE` |
| `Assets/Ashfall.Core/RailwaySystem.cs` | Rail (Wave 3) | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Size | Notes |
|---|---|---|
| `waystations.json` | 10.8 KB | stations, keepers, services, stock |
| `weather_route_gates.json` | 11 KB | weather gating |
| `caravan_trade_routes.json` | 6.7 KB | route definitions |
| `vehicle_modifications.json` | 4.9 KB | vehicle depth |
| `vehicle_armor_grades.json` | 4.6 KB | Plan 50 seam |
| `caravans.json` / `merchant_caravans.json` | 4.2 / 3.3 KB | traffic |
| Bridge/ferry/convoy/maintenance data | none | confirmed absent |

### 2.3 Confirmed gaps

- **GAP-34-1 — No authored road corridors.** Segments exist without content.
- **GAP-34-2 — No bridges, fords, or ferries.** Crossings are fiction only.
- **GAP-34-3 — No convoy operations.** Vehicles exist; scheduled convoys do not.
- **GAP-34-4 — No clearance work beyond minefields.** Slides, wrecks, and
  collapses are unmodeled.
- **GAP-34-5 — No road maintenance.** Grading, drainage, and surfacing are
  absent.
- **GAP-34-6 — No roadside dangers or encounters.**
- **GAP-34-7 — No tolls or passage agreements.**
- **GAP-34-8 — No road crew roles or equipment content.**
- **GAP-34-9 — Waystations have data but no operational loop.**

### 2.4 Non-duplication statement

This expansion will **not** add a second route, vehicle, rail, ice-road, weather,
territory, or economy system. It extends `RouteInfrastructureSystem` with
authored corridors and structure-backed segments, extends the waystation data
into an operational loop, uses `ExpeditionVehicleSystem` and the garage for
vehicles, uses `WeatherGate` for gating, defers territory to
`PatrolTerritoryAuthority`, and adds state only as additive sub-objects of the
existing route store. No new save section.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — A road is a promise between places.** Keeping it open is
infrastructure, not adventure.

**Pillar 2 — Every crossing is a decision.** A bridge, a ford, or a ferry; each
has a cost, a season, and a failure mode.

**Pillar 3 — Movement is cargo, people, and news.** A convoy carries all three,
and losing one loses all three.

**Pillar 4 — Clearance is work with a body count.** Mines, slides, and wrecks
are cleared by crews who know the risk.

**Pillar 5 — Passage is negotiated.** Tolls, rights, and agreements are how
strangers become partners.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| A convoy | Prep, departure, return | Road-movie montage |
| A bridge | Inspection, math, repair | Spectacle collapse |
| Mine clearance | Procedure and nerve | Gore |
| Waystation | Rest, food, news | Tavern cliché |
| A toll | Negotiation | Mafia extortion |
| Roadside danger | Tension and choice | Action set piece |

### 3.3 Content limits

- Mine clearance is procedural and dangerous, never gory or celebratory.
- Banditry is social and economic, never a racist or xenophobic caricature.
- Road deaths are authored, rare, and grieved.
- No real-world highway, bridge, or agency is copied.
- Tolls and passage rights are negotiated agreements, not protection rackets.

---

## 4. THE ROAD WORLD

### 4.1 Interior rooms

- **`room_road_office`** — maps, schedules, and the route board.
- **`room_garage_bay`** — vehicles, lifts, and tools.
- **`room_tire_shop`** — rubber, patching, and spares.
- **`room_fuel_depot`** — fuel, cans, and rationing.
- **`room_clearance_store`** — detectors, probes, and markers.
- **`room_convoy_ready`** — cargo staging and load plans.
- **`room_waystation_store`** — waystation supplies.
- **`room_bridge_shop`** — timber, cable, and formwork.

### 4.2 Exterior locations

| ID | Name | Danger | Purpose |
|---|---|---|---|
| `loc_old_highway` | The Old Highway | 4 | Main corridor |
| `loc_broken_bridge` | The Broken Span | 6 | Bridge repair and risk |
| `loc_ford_crossing` | The Ford | 4 | Seasonal crossing |
| `loc_ferry_landing` | The Ferry Landing | 4 | River crossing service |
| `loc_rail_cut` | The Rail Cut | 5 | Road-rail interchange |
| `loc_minefield` | The Minefield | 7 | Clearance work |
| `loc_landslide` | The Slide | 5 | Debris clearance |
| `loc_wreck_field` | The Wreck Field | 5 | Salvage and blockage |
| `loc_waystation_north` | Waystation North | 2 | Rest and resupply |
| `loc_toll_post` | The Toll Post | 3 | Passage agreement |

All locations require valid item references and scanner registration.

### 4.3 The road week

Monday scout, Tuesday load, Wednesday convoy, Thursday clear, Friday maintain,
Saturday trade, Sunday repair. The road's rhythm is the shelter's supply rhythm,
and the expansion makes it visible.

---

## 5. MAIN STORYLINE — "WHAT THE ROAD COST"

### 5.1 Central conflict

The shelter's trade depends on one corridor and one bridge. **Oswin Gray** has
been telling anyone who will listen that the bridge has one winter left.
**Rhoda Vance** runs the convoys and cannot afford to stop them. When the span
cracks, the shelter's supplies double in price overnight and a minefield
discovered on the detour closes the alternative.

The shelter must decide what it is willing to spend to keep moving: rebuild a
bridge, clear a minefield, run ferries through the thaw, or accept a smaller
world. **Moss** at Waystation North offers help at a price, **Beck** the
ferryman offers a crossing with a schedule, and **Yara** leads clearance crews
who know exactly what a mistake costs.

The expansion's question: **what is a shelter willing to spend to keep a road
open, and who pays for the crossing?**

### 5.2 Theme (unspoken)

**Distance is a cost, and somebody always pays it.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_road_captain_rhoda_vance` | Rhoda Vance | Road captain | Convoys, schedules, and cargo |
| `npc_engineer_oswin_gray` | Oswin Gray | Bridge engineer | Inspection, math, and repair |
| `npc_ferryman_beck` | Beck | Ferryman | Crossings, cables, and schedules |
| `npc_keeper_moss` | Moss | Waystation keeper | Rest, fuel, and news |
| `npc_driver_tam_rook` | Tam Rook | Driver | Vehicles and bad roads |
| `npc_scout_ivy` | Ivy | Scout | Route survey and danger |
| `npc_clearance_yara` | Yara | Clearance lead | Minefields and debris |
| `npc_toll_clerk_hask` | Hask | Toll clerk | Passage agreements |

### 5.4 Story beats (15)

1. **The Crack.** A span fails and a convoy turns back.
2. **The Price.** Supplies double and the shelter feels it.
3. **The Survey.** Ivy maps the detours and the risk.
4. **The Minefield.** The detour is blocked; clearance begins.
5. **The Ferry.** Beck offers a thaw crossing at a price.
6. **The Waystation.** Moss negotiates staging and fuel.
7. **The Load.** A convoy is planned and loaded.
8. **The Crossing.** The first convoy over the repaired span or ferry.
9. **The Loss.** A convoy is lost or damaged and the shelter responds.
10. **The Tolls.** Passage rights are argued and written.
11. **The Wrecks.** Clearance crews open a blocked cutting.
12. **The Maintenance.** Drainage and grading stop the next failure.
13. **The Winter.** Ice and snow test the route plan.
14. **The Road Board.** Route operations become an institution.
15. **What the Road Cost.** Final disposition of the roads.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Bridge | repair / detour / abandon | cost vs. reach |
| Clearance | full / partial / avoid | risk vs. route |
| Ferry | buy / build / none | crossing cost |
| Convoy | large / small / escorted | risk model |
| Tolls | pay / negotiate / refuse | access vs. pride |
| Maintenance | scheduled / reactive / none | future vs. now |
| Waystations | invest / trade / ignore | range |
| Final | roads as network / corridor / memory | identity |

### 5.6 Endings (5 + fade)

1. **The Open Road** — the network is maintained, waystations are staffed, and
   the shelter moves what it needs year round.
2. **The Ring Road** — the shelter builds redundancy and survives a closure
   without a shortage.
3. **The Ferry Republic** — crossings become the shelter's trade and its grip.
4. **The Single Lane** — one road, one bridge, and a shelter that holds its
   breath every winter.
5. **The Cut Road** — the corridor closes, the region shrinks, and the shelter
   learns what it lost.
6. **Fade** — the same road is patched again, and again, and the bridge waits.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_road_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_road_crack`, `quest_road_price`, `quest_road_survey`,
`quest_road_minefield`, `quest_road_ferry`, `quest_road_waystation`,
`quest_road_load`, `quest_road_crossing`, `quest_road_loss`, `quest_road_tolls`,
`quest_road_wrecks`, `quest_road_maintenance`, `quest_road_winter`,
`quest_road_board`, `quest_road_what_it_cost`.

### 6.2 Side quests (30)

**Corridors (5)**
- `quest_road_survey_segment` — survey a route segment
- `quest_road_mark_route` — mark and sign a route
- `quest_road_seasonal_risk` — assess seasonal behavior
- `quest_road_bypass` — find and map a bypass
- `quest_road_priority` — set route priority

**Crossings (5)**
- `quest_road_bridge_inspect` — inspect a bridge
- `quest_road_bridge_repair` — repair a span
- `quest_road_ford` — improve and mark a ford
- `quest_road_ferry_run` — run a ferry crossing
- `quest_road_culvert` — build a culvert

**Convoys (5)**
- `quest_road_convoy_plan` — plan a convoy
- `quest_road_load_plan` — load and balance
- `quest_road_escort` — escort a convoy
- `quest_road_breakdown` — repair a breakdown on route
- `quest_road_lost_convoy` — search for a missing convoy

**Clearance (5)**
- `quest_road_debris` — clear a slide
- `quest_road_wreck_clear` — remove a wreck
- `quest_road_mine_clear` — clear a minefield
- `quest_road_marker` — mark safe passage
- `quest_road_verify` — verify cleared ground

**Waystations (5)**
- `quest_road_station_restore` — restore a station
- `quest_road_station_stock` — stock fuel and food
- `quest_road_station_defense` — improve defense
- `quest_road_station_news` — run the news exchange
- `quest_road_station_repair` — repair vehicles there

**Agreements (5)**
- `quest_road_toll_talk` — negotiate a toll
- `quest_road_passage_right` — write passage rights
- `quest_road_mutual_aid` — agree on mutual aid
- `quest_road_dispute` — resolve a road dispute
- `quest_road_shared_map` — share route maps

### 6.3 Repeatable quests (8)

`quest_road_repeat_patrol`, `quest_road_repeat_maintain`,
`quest_road_repeat_convoy`, `quest_road_repeat_clear`,
`quest_road_repeat_scout`, `quest_road_repeat_repair`,
`quest_road_repeat_station`, `quest_road_repeat_log`.

### 6.4 Dynamic hooks

Live events (route state changes, minefield detection, weather gate closures,
vehicle breakdowns, territory changes, caravan arrivals) attach authored
follow-ups through existing seams. No new event bus.

### 6.5 Constraints

- Route segments remain owned by `RouteInfrastructureSystem`.
- Vehicles stay with `ExpeditionVehicleSystem` and the garage.
- Weather gating stays with `WeatherGate`.
- Territory stays with `PatrolTerritoryAuthority`.
- Trade prices stay with the economy authority.
- Combat, if any, stays with the combat system; the road provides context.
- No road may be opened without survey, clearance, and capacity.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `RoadCorridorSystem` (new, `Ashfall.Core.World`)

**Owns:** authored corridors, segment content, route priority, seasonal
behavior, and bypasses. **Consumes:** `RouteInfrastructureSystem`,
`WeatherGate`, `Inventory`, `DutyRoster`. **Data:** `road_corridors.json`.
**Rules:** every corridor is a chain of live segments; content names, describes,
and prioritizes them; no parallel segment state.

### 7.2 `CrossingSystem` (new, `Ashfall.Core.World`)

**Owns:** bridges, fords, ferries, and culverts as structures with condition,
capacity, inspection, and repair. **Consumes:** `RouteInfrastructureSystem`
(segment capacity and clearance), `Inventory`, `BuildWorksSystem` (Wave 4),
`WeatherGate`. **Data:** `bridges.json`, `ferries.json`.
**Rules:** crossings carry real limits and fail visibly; a ferry runs on a
schedule with a cable and a crew; repairs consume materials and close the route.

### 7.3 `ConvoySystem` (new, `Ashfall.Core.World`)

**Owns:** convoy planning, schedules, load plans, escort assignment, departure,
progress, losses, and return. **Consumes:** `ExpeditionVehicleSystem`,
`VehicleGarageSystem`, `Inventory`, `RouteInfrastructureSystem`,
`PatrolTerritoryAuthority`, `WeatherGate`, `DutyRoster`.
**Data:** `convoys.json`. **Rules:** convoys move on the live route graph at
authored speeds; breakdowns and dangers resolve deterministically; cargo is real
inventory.

### 7.4 `ClearanceSystem` (new, `Ashfall.Core.World`)

**Owns:** debris, slides, wrecks, and mine clearance crews and procedures.
**Consumes:** `RouteInfrastructureSystem` minefield state, `Inventory`,
`DutyRoster`, `MedicalPipelineCoordinator` for injuries.
**Data:** `clearance_jobs.json`, `roadside_dangers.json`.
**Rules:** clearance has authored risk and time; a cleared segment records its
cleared fraction through the live system; a mistake injures a real person.

### 7.5 `RoadMaintenanceSystem` (new, thin, `Ashfall.Core.World`)

**Owns:** grading, drainage, surfacing, winter work, and maintenance schedules.
**Consumes:** `RouteInfrastructureSystem` roughness and speed limits,
`BuildWorksSystem`, `WeatherSystem`, `DutyRoster`.
**Data:** `road_maintenance.json`. **Rules:** maintenance lowers roughness and
raises speed limits through the live state; neglect raises them; winter work has
a season window.

### 7.6 `WaystationOpsSystem` (extend waystation data)

**Owns:** station operation: rest, fuel, food, repairs, warnings, news, and
defense. **Consumes:** existing `waystations.json`, `TradingSystem` (prices stay
economy), `WeatherGate`, `NoticeSystem`. **Data:** extended `waystations.json`.
**Rules:** stations have real stock and keepers; running out is a real event;
news exchanged at stations travels the region.

### 7.7 `PassageAgreementSystem` (new, thin, `Ashfall.Core.World`)

**Owns:** tolls, passage rights, mutual aid, and road disputes. **Consumes:**
`PolicySystem`, `StandingRecord` (Exp 03), `TradingSystem`, `DiplomaticTreaty`
surface where present. **Data:** `toll_agreements.json`.
**Rules:** agreements are written, signed, and enforced by people; refusal is a
real option with a real cost.

### 7.8 Systems explicitly not added

- No second route, vehicle, rail, ice road, weather, or territory system.
- No road-building freeform editor.
- No combat overhaul; the road provides context, combat stays in combat.
- No new currency.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `road_corridors.json` (new)

```json
{
  "schema_version": 1,
  "corridors": [
    {
      "corridor_id": "corridor_old_highway",
      "display_name": "The Old Highway",
      "segments": ["seg_hwy_01", "seg_hwy_02", "seg_hwy_03"],
      "priority": 1,
      "seasonal": "year_round",
      "bypass": "corridor_river_road",
      "tags": ["main", "trade", "flood_prone"]
    }
  ]
}
```

### 8.2 `bridges.json` (new)

Bridges: span type, length, capacity, condition, inspection date, failure mode,
and repair recipe.

### 8.3 `ferries.json` (new)

Ferries: crossing, cable, capacity, schedule, crew, fare, and seasonal limits.

### 8.4 `convoys.json` (new)

Convoys: route, vehicles, cargo, escort, departure, return, and loss model.

### 8.5 `clearance_jobs.json` (new)

Jobs: type (slide, wreck, minefield), length, risk, crew, days, and output.

### 8.6 `road_maintenance.json` (new)

Work: type, roughness reduction, speed gain, materials, crew, and season.

### 8.7 `roadside_dangers.json` (new)

Dangers: kind, trigger, severity, and response.

### 8.8 `toll_agreements.json` (new)

Agreements: party, route, toll, rights, duration, and enforcement.

### 8.9 Extend `waystations.json`

Operational fields: stock levels, fuel, repair capacity, news log, defense
state, and keeper state consistent with the live schema.

### 8.10 Items

New items appended to `items.json`: `item_road_map`, `item_route_marker`,
`item_gravel_load`, `item_culvert_pipe`, `item_bridge_timber`,
`item_ferry_cable`, `item_pontoon`, `item_warning_sign`, `item_convoy_kit`,
`item_road_flare`, `item_spare_wheel`, `item_jack_tool`,
`item_cargo_netting`, `item_tarpaulin`, `item_mine_probe`,
`item_mine_marker`, `item_tow_chain`, `item_winch`, `item_road_log`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

Route infrastructure state, vehicle state, and weather gate state remain the
live save owners. New sub-objects (corridor content, crossing structures,
convoys, clearance jobs, maintenance plans, station stock, agreements) are
additive inside them. No new save section.

### 9.2 State to persist

- Corridor priorities and survey knowledge.
- Bridge, ford, ferry, and culvert condition.
- Convoy schedules, loads, and outcomes.
- Clearance job progress and residual risk.
- Maintenance schedule and history.
- Waystation stock, fuel, and keeper state.
- Passage agreements and toll status.

### 9.3 Determinism

- Route travel uses `GetTravelModifier`, `GetSpeedLimit`, `CanTraverse`, and
  `GetHazardModifier` from the live system.
- Convoy incidents resolve deterministically from authored risk, route state,
  and vehicle condition; any randomness uses the live seeded path.
- Clearance risk is authored and deterministic given crew, tools, and procedure.
- Paired replay hashes must match.

### 9.4 Migration

Legacy saves load with existing route, vehicle, and gate state untouched; no
corridor, crossing, convoy, clearance, maintenance, station, or agreement state
exists until started. Existing minefield and gate state keeps working.

### 9.5 Checksum

Invariant-culture floats; integer counts for cargo, days, and cleared fractions
where the live schema allows.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `RoadBoardPanel` (new) | Corridors, priority, status | `RoadwaysHostSession` |
| `CrossingPanel` (new) | Bridges, fords, ferries | same |
| `ConvoyPanel` (new) | Plan, load, depart, track | same |
| `ClearancePanel` (new) | Jobs, risk, progress | same |
| `MaintenancePanel` (new) | Work schedule | same |
| `WaystationPanel` (new) | Stations, stock, keepers | same |
| `AgreementPanel` (new) | Tolls and rights | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Route closures state the reason and the alternative.
- Convoy risk is stated before departure.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- Road status uses symbols and text together, never color alone.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: an engine, gravel under tires, a cable
winching, a bridge plank, a mine probe tapping, a waystation bell. No cue is
required; text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `RouteInfrastructureSystem` | Segments, modes, clearance, minefields |
| `WeatherGate` | Weather closures |
| `ExpeditionVehicleSystem` | Expeditions and breakdowns |
| `VehicleGarageSystem` | Repair and condition |
| `PatrolTerritoryAuthority` | Territory on routes |
| `IceRoadSystem` | Frozen corridor (untouched) |
| `RailwaySystem` | Rail (untouched) |
| `BuildWorksSystem` (Wave 4) | Bridges and culverts |
| `TradingSystem` | Cargo value |
| `HuntingSystem` (Wave 5) | Route-side supplies |
| `WeatherSystem` (Wave 5) | Conditions and gates |
| `NoticeSystem` (Wave 4) | Route notices |
| `MedicalPipelineCoordinator` | Clearance injuries |
| `EpilogueChronicleBuilder` | Road milestones |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm `RouteInfrastructureSystem`, modes and
clearance fields, `WeatherGate`, vehicles and garage, ice road and rail
boundaries, waystation data, and caravan route data. Record file:line; change
nothing.

**Phase 1 — Data + validators.** Author corridors, bridges, ferries, convoys,
clearance jobs, maintenance, dangers, agreements; extend waystations; append
items. Register validators and scanner.

**Phase 2 — Pure Core.** `RoadCorridorSystem`, `CrossingSystem`, `ConvoySystem`,
`ClearanceSystem`, `RoadMaintenanceSystem`, `WaystationOpsSystem`,
`PassageAgreementSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip,
determinism.

**Phase 4 — Host + CLI.** `RoadwaysHostSession`, selftest coverage, fresh
journey.

**Phase 5 — UI.** New panels with lifecycle and accessibility.

**Phase 6 — Content.** Locations, rooms, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** 720-day soak: closures, detours, maintenance, and convoy
losses.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Corridors | 12 |
| Bridges | 15 |
| Ferries / fords | 8 |
| Convoys | 20 |
| Clearance jobs | 20 |
| Maintenance work | 15 |
| Roadside dangers | 20 |
| Waystations | 10 |
| Agreements | 12 |
| Items | 19 |
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
| Second route/vehicle system | Critical | Extend live owners |
| Second rail/ice road | Critical | Boundaries respected |
| Convoy loses everything | High | Authored partial losses |
| Mine clearance gore | High | Procedure and restraint |
| Economy duplication | High | Prices stay economy |
| Toll as extortion | Medium | Negotiated agreements |
| Determinism break | Low | Live travel modifiers |
| Content overrun | Medium | Budget §13 |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `road_corridors.json` | 12 | 3,500 |
| `bridges.json` | 15 | 4,000 |
| `ferries.json` | 8 | 2,500 |
| `convoys.json` | 20 | 5,000 |
| `clearance_jobs.json` | 20 | 4,500 |
| `road_maintenance.json` | 15 | 3,500 |
| `roadside_dangers.json` | 20 | 4,000 |
| `waystations.json` | +10 | 4,000 |
| `toll_agreements.json` | 12 | 3,000 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 19 | 2,500 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~65,000** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R34-1 | Second route system | Low | Critical | One segment owner |
| R34-2 | Rail/ice road overlap | Low | Critical | Explicit boundaries |
| R34-3 | Total convoy loss | Med | High | Partial loss model |
| R34-4 | Clearance gore | Low | High | Procedural restraint |
| R34-5 | Economy duplication | Med | High | Prices stay economy |
| R34-6 | Toll extortion tone | Med | Med | Negotiated agreements |
| R34-7 | Maintenance grind | Med | Med | Schedules and crews |
| R34-8 | Determinism | Low | High | Live travel modifiers |
| R34-9 | Content overrun | Med | Med | Budget §13 |
| R34-10 | Waystation shallow | Med | Med | Real stock and keepers |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Can a convoy be lost entirely?** Recommended: rarely, with partial cargo
   and crew survival as the default.
2. **Do ferries run on schedules or on demand?** Recommended: schedules with
   seasonal windows.
3. **Can the shelter build new roads?** Recommended: no freeform building; it
   can improve and bypass existing corridors.
4. **Are tolls enforced by force?** Recommended: by agreement and refusal, with
   territory authority as the backstop.
5. **Does road maintenance permanently raise speed limits?** Recommended: yes,
   through the live roughness state, degrading with neglect.

---

## 17. APPENDIX D — CORRIDOR TABLE (12 CORRIDORS)

| # | Corridor | Segments | Priority | Season | Bypass |
|---|---|---|---|---|---|
| 1 | Old Highway | 3 | 1 | year | River Road |
| 2 | River Road | 2 | 2 | spring–autumn | Old Highway |
| 3 | Ridge Track | 2 | 3 | summer | Valley Track |
| 4 | Valley Track | 3 | 3 | all | Ridge Track |
| 5 | Rail Cut Road | 2 | 2 | all | Old Highway |
| 6 | North Spur | 2 | 4 | all | none |
| 7 | Ash Flats Road | 2 | 4 | winter | none |
| 8 | Minefield Cut | 1 | blocked | none | River Road |
| 9 | Ferry Approach | 2 | 2 | thaw only | River Road |
| 10 | Waystation Loop | 3 | 3 | all | none |
| 11 | Wreckfield Road | 2 | blocked | none | Ridge Track |
| 12 | Toll Road | 1 | 2 | all | Valley Track |

Corridors are chains of live segments with names, priorities, seasons, and
bypasses. A corridor without a bypass is a risk the shelter has chosen to
accept.

---

## 18. APPENDIX E — BRIDGE TABLE (15 BRIDGES)

| # | Bridge | Type | Length | Capacity | Condition | Failure |
|---|---|---|---|---|---|---|
| 1 | Old Span | beam | 30 m | 8 t | poor | crack |
| 2 | Mill Bridge | arch | 20 m | 12 t | fair | scour |
| 3 | Rail Cut Bridge | beam | 18 m | 10 t | fair | deck |
| 4 | Ford Ramp | slab | 12 m | 6 t | good | washout |
| 5 | Ash Bridge | beam | 25 m | 8 t | poor | freeze |
| 6 | North Crossing | truss | 35 m | 15 t | fair | collapse |
| 7 | Ferry Jetty | deck | 10 m | 5 t | good | storm |
| 8 | Culvert Line | culvert | 8 m | 4 t | fair | block |
| 9 | Second Span | beam | 30 m | 10 t | new | none |
| 10 | Valley Bridge | arch | 22 m | 9 t | fair | scour |
| 11 | Toll Bridge | truss | 28 m | 12 t | good | none |
| 12 | Wreck Bridge | beam | 16 m | 6 t | poor | impact |
| 13 | Waystation Bridge | slab | 14 m | 8 t | good | frost |
| 14 | Old Culvert | culvert | 6 m | 3 t | poor | collapse |
| 15 | Pontoon Site | pontoon | 24 m | 6 t | seasonal | cable |

Bridges are the expansion's most legible infrastructure: long, expensive,
load-limited, and honest about when they are tired.

---

## 19. APPENDIX F — FERRY AND FORD TABLE (8 CROSSINGS)

| # | Crossing | Type | Capacity | Schedule | Season | Fare |
|---|---|---|---|---|---|---|
| 1 | Beck's Ferry | cable | 4 t | 3 daily | thaw–freeze | goods |
| 2 | River Ford | ford | 2 t | none | summer | free |
| 3 | Marsh Ford | ford | 1 t | none | dry | free |
| 4 | Pontoon Line | pontoon | 3 t | on call | all | fee |
| 5 | North Ferry | cable | 5 t | 2 daily | thaw | share |
| 6 | Bay Crossing | boat | 2 t | tide | all | fee |
| 7 | Rail Ford | ford | 3 t | none | summer | free |
| 8 | Cut Ford | ford | 2 t | none | summer | free |

Ferries and fords are the expansion's seasonal answer to a missing bridge: slow,
weather-bound, and worth every hour when the alternative is a closed road.

---

## 20. APPENDIX G — CONVOY TABLE (20 CONVOYS)

| # | Convoy | Route | Vehicles | Cargo | Escort | Risk |
|---|---|---|---|---|---|---|
| 1 | Supply Run | Old Highway | 2 truck | food | 1 | low |
| 2 | Fuel Haul | River Road | 1 tanker | fuel | 2 | med |
| 3 | Grain Train | Old Highway | 3 truck | grain | 1 | low |
| 4 | Trade Fair | Valley Track | 2 truck | goods | 2 | med |
| 5 | Medical Run | Old Highway | 1 van | medicine | 1 | low |
| 6 | Ore Haul | Rail Cut Road | 2 truck | ore | 2 | med |
| 7 | Timber Run | Ridge Track | 2 truck | timber | 1 | med |
| 8 | Water Run | River Road | 1 tanker | water | 1 | low |
| 9 | Refugee Move | Old Highway | 2 bus | people | 2 | med |
| 10 | Mail Run | Waystation Loop | 1 van | post | 1 | low |
| 11 | Salvage Trip | Wreckfield Road | 2 truck | scrap | 2 | high |
| 12 | Carbon Run | North Spur | 1 truck | charcoal | 1 | med |
| 13 | Seed Run | Valley Track | 1 van | seed | 1 | low |
| 14 | Herd Move | River Road | none | animals | 2 | med |
| 15 | Kiln Load | Old Highway | 2 truck | brick | 1 | low |
| 16 | Glass Run | Toll Road | 1 van | glass | 1 | med |
| 17 | Gazette Run | Waystation Loop | 1 van | print | 1 | low |
| 18 | Night Run | Old Highway | 2 truck | mixed | 2 | high |
| 19 | Evacuation | North Spur | 3 bus | people | 3 | high |
| 20 | Relief Convoy | Ferry Approach | 2 truck | aid | 2 | high |

Every convoy is a real load, a real route, and a real risk. The list is the
shelter's circulation written out as scheduled traffic.

---

## 21. APPENDIX H — CLEARANCE JOB TABLE (20 JOBS)

| # | Job | Type | Length | Risk | Crew | Days |
|---|---|---|---|---|---|---|
| 1 | Slide A | landslide | 40 m | low | 3 | 2 |
| 2 | Slide B | landslide | 80 m | med | 4 | 4 |
| 3 | Rockfall | debris | 20 m | med | 2 | 2 |
| 4 | Wreck One | vehicle | 10 m | low | 2 | 1 |
| 5 | Wreck Field | vehicles | 60 m | med | 4 | 5 |
| 6 | Minefield North | mines | 100 m | high | 6 | 8 |
| 7 | Minefield Cut | mines | 60 m | high | 5 | 6 |
| 8 | Collapsed Culvert | structural | 8 m | med | 3 | 3 |
| 9 | Flood Silt | debris | 50 m | low | 4 | 3 |
| 10 | Snow Drift | snow | 200 m | low | 4 | 2 |
| 11 | Ice Block | ice | 30 m | med | 3 | 2 |
| 12 | Bridge Debris | structural | 20 m | high | 4 | 4 |
| 13 | Fallen Trees | debris | 40 m | low | 2 | 2 |
| 14 | Mud Slide | landslide | 70 m | med | 4 | 4 |
| 15 | Ash Drift | ash | 150 m | low | 3 | 3 |
| 16 | Rail Crossing | mixed | 25 m | med | 3 | 3 |
| 17 | Collapsed Wall | structural | 15 m | high | 3 | 3 |
| 18 | Ordnance Find | mines | 5 m | high | 2 | 1 |
| 19 | Verify Passage | survey | section | med | 2 | 2 |
| 20 | Full Reopen | mixed | corridor | high | 6 | 10 |

Clearance is where the expansion is most serious: real ground, real tools, real
risk, and a cleared fraction recorded in the live system only when it is
actually cleared.

---

## 22. APPENDIX I — MAINTENANCE TABLE (15 WORK TYPES)

| # | Work | Roughness | Speed gain | Materials | Crew | Season |
|---|---|---|---|---|---|---|
| 1 | Grade Road | −0.10 | +5 | none | 3 | dry |
| 2 | Fill Potholes | −0.08 | +4 | gravel | 2 | dry |
| 3 | Drain Ditch | −0.05 | +3 | none | 3 | spring |
| 4 | Clear Culvert | −0.06 | +4 | tools | 2 | spring |
| 5 | Surface Patch | −0.12 | +6 | tar | 3 | summer |
| 6 | Gravel Spread | −0.10 | +5 | gravel | 3 | summer |
| 7 | Shoulder Repair | −0.04 | +2 | gravel | 2 | any |
| 8 | Snow Fence | 0 | +5 winter | timber | 2 | autumn |
| 9 | Ice Breaking | −0.05 | +4 | tools | 3 | winter |
| 10 | Bridge Deck | −0.15 | +8 | timber | 4 | dry |
| 11 | Culvert Build | −0.10 | +5 | pipe | 3 | dry |
| 12 | Sign Restore | 0 | +1 | signs | 1 | any |
| 13 | Guard Rail | 0 | +2 | timber | 2 | any |
| 14 | Rut Fill | −0.07 | +3 | gravel | 2 | dry |
| 15 | Full Regrade | −0.20 | +10 | gravel | 5 | dry |

Maintenance is the unglamorous work that keeps the live roughness index low and
the speed limits high. The shelter that maintains its road spends less time
clearing it.

---

## 23. APPENDIX J — ROADSIDE DANGER TABLE (20 DANGERS)

| # | Danger | Trigger | Severity | Response |
|---|---|---|---|---|
| 1 | Bandit Ambush | route traffic | high | escort, negotiate |
| 2 | Toll Demand | passage | med | pay, argue |
| 3 | Breakdown | vehicle | med | repair, tow |
| 4 | Flat Tire | road state | low | patch |
| 5 | Overheat | load | low | cool, wait |
| 6 | Fuel Runout | distance | med | cache |
| 7 | Landslide | rain | high | clear, detour |
| 8 | Washout | flood | high | detour |
| 9 | Drift | snow | med | dig |
| 10 | Fog | weather | med | stop |
| 11 | Ice Patch | winter | high | slow, chain |
| 12 | Fallen Tree | storm | low | cut |
| 13 | Animal Crossing | wildlife | low | wait |
| 14 | Predator Sign | wildlife | med | escort |
| 15 | Contaminated Rain | black rain | high | shelter |
| 16 | Minefield Unknown | debris | extreme | stop, clear |
| 17 | Structural Crack | bridge | high | inspect, close |
| 18 | Sinkhole | drainage | high | mark, fill |
| 19 | Night Stopping | schedule | med | camp |
| 20 | Lost Bearings | map | low | scout |

Dangers are authored events with real responses. The road is dangerous in the
way infrastructure is dangerous: boring until the day it is not, and always
explainable afterwards.

---

## 24. APPENDIX K — WAYSTATION TABLE (10 STATIONS)

| # | Station | Region | Keeper | Services | Defense | Stock |
|---|---|---|---|---|---|---|
| 1 | North | north spur | Moss | rest, fuel, repair | 4 | fuel, food |
| 2 | The Cut | industrial | Kessel | trade, staging | 5 | tools |
| 3 | River Landing | river | Beck | ferry, rest | 3 | water |
| 4 | Ridge Camp | ridge | Durn | rest, watch | 4 | food |
| 5 | Valley Post | valley | Senn | trade, repair | 3 | parts |
| 6 | Ash Halt | ash flats | Pim | filters, rest | 2 | filters |
| 7 | Mine Gate | minefield | Yara | clearance, staging | 6 | tools |
| 8 | Toll Post | toll road | Hask | toll, rest | 4 | goods |
| 9 | Rail Junction | rail cut | Corl | trade, freight | 4 | fuel |
| 10 | South Turn | south road | Vell | rest, news | 3 | mixed |

Waystations are the expansion's social infrastructure: a keeper, a service, a
defense rating, and stock that can run out. They are where the region talks to
itself.

---

## 25. APPENDIX L — AGREEMENT TABLE (12 AGREEMENTS)

| # | Agreement | Party | Route | Term | Toll / Right |
|---|---|---|---|---|---|
| 1 | North Passage | Waystation North | North Spur | 1 year | fuel share |
| 2 | Ferry Right | Beck | Ferry Approach | season | goods fee |
| 3 | Toll Compact | Toll Post | Toll Road | 2 years | fixed fee |
| 4 | Mutual Aid | Market Town | all | standing | aid exchange |
| 5 | Mine Clearance Pact | Mine Gate | Minefield Cut | operation | shared cost |
| 6 | Bridge Share | Valley Town | Valley Bridge | 5 years | upkeep split |
| 7 | News Exchange | South Turn | Waystation Loop | standing | free |
| 8 | Medical Run | Foundry | Old Highway | standing | free |
| 9 | Grain Freight | Farmstead | River Road | season | freight rate |
| 10 | Salvage Permit | Wreckfield | Wreckfield Road | year | cut |
| 11 | Refugee Passage | Flotilla | all | standing | aid |
| 12 | Emergency Access | All parties | all | permanent | free |

Agreements are the shelter's foreign policy written as road access. They are
negotiated, signed, and enforced by people, and refusal is always an option.

---

## 26. APPENDIX M — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_road_crack` | 4 | Inspect; convoy turns back |
| `quest_road_price` | 4 | Measure the shortage |
| `quest_road_survey` | 5 | Map detours and risk |
| `quest_road_minefield` | 5 | Clear the detour |
| `quest_road_ferry` | 4 | Negotiate the crossing |
| `quest_road_waystation` | 4 | Stage and stock |
| `quest_road_load` | 4 | Plan and load a convoy |
| `quest_road_crossing` | 5 | First crossing |
| `quest_road_loss` | 5 | Respond to a convoy loss |
| `quest_road_tolls` | 4 | Argue and write rights |
| `quest_road_wrecks` | 4 | Open the cutting |
| `quest_road_maintenance` | 4 | Drain and grade |
| `quest_road_winter` | 5 | Winter route plan |
| `quest_road_board` | 4 | Route operations as institution |
| `quest_road_what_it_cost` | 3 | Final disposition |

---

## 27. APPENDIX N — NPC DOSSIERS (BRIEF)

**Rhoda Vance** — road captain. Plans convoys like campaigns and counts the
cost of every kilometre. Believes a road is only real if it is open in winter.

**Oswin Gray** — bridge engineer. Has been right about the span for two years
and hates being right. Repairs bridges by arithmetic and patience.

**Beck** — ferryman. Runs a cable crossing with a schedule and a fare and
refuses to run in a flood. The river is Beck's partner, not Beck's property.

**Moss** — waystation keeper at North. Trades news before goods and knows
everyone on the road by their engine noise.

**Tam Rook** — driver. Has driven the same battered truck for six years and can
hear a failing bearing from the cab. Superstitious about the bridge.

**Ivy** — scout. Walks routes ahead of convoys and marks danger with silent
practicality. Keeps the best maps in the region in their head and then draws
them.

**Yara** — clearance lead. Clears mines and slides with a procedure and a
rosary of small habits, and has never lost a crew member by rushing.

**Hask** — toll clerk. Collects fees, records rights, and enforces agreements
with paperwork rather than weapons.

---

## 28. APPENDIX O — LOCATION DETAIL

- **The Old Highway** — the main corridor, cracked, patched, and always busy.
- **The Broken Span** — the shelter's most important and most tired bridge.
- **The Ford** — seasonal, muddy, and a lesson in wheel depth.
- **The Ferry Landing** — cable, schedule, and a queue of waiting trade.
- **The Rail Cut** — where road meets rail and cargo changes hands.
- **The Minefield** — marked, fenced, and cleared metre by metre.
- **The Slide** — rock and mud that closes the road with no warning.
- **The Wreck Field** — salvage and blockage in the same pile.
- **Waystation North** — rest, fuel, and the region's gossip.
- **The Toll Post** — a gate, a ledger, and a negotiation.

---

## 29. APPENDIX P — TRAVEL AND SPEED MODEL

| Route state | Speed limit | Modifier | Note |
|---|---|---|---|
| Secured | design | 1.0 | full speed |
| Cleared | design | 0.9 | watchful |
| Partial | reduced | 0.6 | slow |
| Unbreached | low | 0.4 | dangerous |
| Minefield | crawl | 0.2 | clearance only |
| Closed | none | 0 | gate |

Travel modifiers come from the live system: speed limit, roughness, hazard, and
vehicle capability. The expansion's corridors only name and organize what the
live engine already measures.

---

## 30. APPENDIX Q — BRIDGE AND CROSSING MODEL

| Step | Action | Cost | Effect |
|---|---|---|---|
| Inspect | survey | hours | condition known |
| Rate | capacity math | hours | load limit |
| Patch | timber, bolts | days | temporary reopen |
| Repair | full materials | weeks | restored capacity |
| Replace | build works | season | new structure |
| Close | barrier | minutes | route closed |
| Ford | gravel, marking | days | seasonal crossing |
| Ferry | cable, boat, crew | weeks | scheduled crossing |

Every crossing decision is a trade between reach, cost, and risk. The expansion
never hides that math; it puts it on the bridge inspector's board.

---

## 31. APPENDIX R — CONVOY LOSS MODEL

| Incident | Cargo loss | Vehicle | Crew | Recovery |
|---|---|---|---|---|
| Flat tire | none | delayed | fine | patch |
| Breakdown | none | disabled | fine | tow |
| Ambush | partial | damaged | injured | escort, aid |
| Slide | partial | stuck | fine | clear |
| Washout | total | lost | rescued | reroute |
| Mine | total | lost | injured | clear, aid |
| Fire | partial/total | lost | rescued | aid |
| Lost | delayed | intact | fine | search |

The default is partial loss and survival, because the expansion's goal is not to
punish the player but to make the road's cost real and survivable.

---

## 32. APPENDIX S — WORKED 720-DAY ROAD SCENARIO

**Days 1–30.** Span cracks; convoy turns back; prices jump.

**Days 31–90.** Detour surveyed; minefield discovered and clearance begins.

**Days 91–150.** Ferry negotiated and running; convoy schedules rewritten;
waystation stocked.

**Days 151–220.** Bridge patched; first convoy crosses; tolls argued and
written.

**Days 221–300.** Wreck field cleared; maintenance begins; drainage stops the
next washout.

**Days 301–420.** Winter plan: snow fences, ice breaking, and ferry closure
dates published. Two convoys delayed, none lost.

**Days 421–540.** Second span built as redundancy. Mutual aid agreement signed;
refugee passage run.

**Days 541–720.** Maintenance becomes scheduled, the route board runs the
network, and the shelter reaches further with less anxiety than a year ago.

---

## 33. APPENDIX T — VIGNETTE (TONE SAMPLE)

> Oswin kneels on the deck and taps the plank with a hammer, and the sound is
> wrong in a place that matters, and he marks the spot with chalk and writes the
> load number in his book, and walks back to tell Rhoda the convoy is going the
> long way.

> Yara probes the ground with a long rod and steps forward on the line she
> marked and does not deviate, and the crew behind her follows because they have
> watched her not deviate for years.

> At Waystation North, Moss pours tea and asks about the bridge before the
> fuel, because the news is the real currency and Moss has been right about that
> for a long time.

---

## 34. APPENDIX U — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| Bridge closed | supply shortage | ferry, detour, repair |
| Mine accident | injury | medical care, review |
| Convoy lost | cargo loss | mutual aid, rebuild |
| Road neglect | slower travel | maintenance schedule |
| Toll dispute | route closed | negotiation |
| Waystation dry | no resupply | stock run |
| Ferry broken | crossing lost | repair cable |
| Winter closure | isolation | stockpile, plan |
| Breakdown | delay | repair skills |
| Map lost | wrong route | survey, re-draw |

No failure is a game over. The deepest failure is a shelter that stops
maintaining a road because nothing has broken yet.

---

## 35. APPENDIX V — CONTENT REVIEW CHECKLIST

- [ ] `RouteInfrastructureSystem` remains the segment authority.
- [ ] Vehicles stay with `ExpeditionVehicleSystem` and the garage.
- [ ] Weather gating stays with `WeatherGate`.
- [ ] Rail and ice-road authorities remain untouched.
- [ ] Trade prices stay with the economy authority.
- [ ] Mine clearance is procedural, never gory.
- [ ] Convoy losses default to partial and survivable.
- [ ] Tolls are negotiated agreements, not extortion.
- [ ] No freeform road construction exists.
- [ ] Save additions are additive and legacy-neutral.
- [ ] Determinism uses live travel modifiers only.

---

## 36. APPENDIX W — GLOSSARY

- **Corridor** — a named chain of route segments.
- **Segment** — a live `RouteSegmentInfrastructureRecord`.
- **Clearance state** — unbreached, partial, cleared, or secured.
- **Crossing** — bridge, ford, ferry, pontoon, or culvert.
- **Convoy** — scheduled movement of vehicles and cargo.
- **Clearance** — removing mines, debris, or wrecks.
- **Roughness** — live surface index that caps speed.
- **Waystation** — a staffed stop with services and stock.
- **Agreement** — a written passage right or toll.
- **Recovery** — reopening a route after failure.

---

## 37. APPENDIX X — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `RouteInfrastructureSystem` | segments | roughness, clearance | corridors |
| `RoadCorridorSystem` | segments | priorities, content | segments |
| `CrossingSystem` | crossings | condition, capacity | segments |
| `ConvoySystem` | routes | schedules, outcomes | vehicles |
| `ClearanceSystem` | minefields | jobs, cleared fraction | segments |
| `RoadMaintenanceSystem` | roughness | reductions | segments |
| `WaystationOpsSystem` | stations | stock, news | trade |
| `PassageAgreementSystem` | policy | agreements | trade |
| `WeatherGate` | weather | closures | segments |
| `ExpeditionVehicleSystem` | route | travel | route |
| `VehicleGarageSystem` | damage | repair | route |
| `PatrolTerritoryAuthority` | territory | control | routes |
| `TradingSystem` | cargo | value | routes |
| `MedicalPipelineCoordinator` | injuries | care | routes |
| `EpilogueChronicleBuilder` | milestones | chronicle | — |

---

## 38. APPENDIX Y — DATA SCHEMA DETAIL (NEW CATALOGS)

**`road_corridors.json`** — `corridor_id`, `display_name`, `segments[]`,
`priority`, `seasonal`, `bypass`, `tags`.

**`bridges.json`** — `bridge_id`, `display_name`, `type`, `length_m`,
`capacity_t`, `condition`, `last_inspection`, `failure_mode`, `repair_recipe`,
`tags`.

**`ferries.json`** — `ferry_id`, `display_name`, `crossing`, `cable`,
`capacity_t`, `schedule`, `crew`, `fare`, `season`, `tags`.

**`convoys.json`** — `convoy_id`, `display_name`, `route`, `vehicles[]`,
`cargo[]`, `escort`, `departure`, `return`, `loss_model`, `tags`.

**`clearance_jobs.json`** — `job_id`, `display_name`, `type`, `length_m`,
`risk`, `crew`, `days`, `output`, `tags`.

**`road_maintenance.json`** — `work_id`, `display_name`, `roughness_delta`,
`speed_gain`, `materials[]`, `crew`, `season`, `tags`.

**`roadside_dangers.json`** — `danger_id`, `display_name`, `trigger`,
`severity`, `response[]`, `tags`.

**`toll_agreements.json`** — `agreement_id`, `display_name`, `party`, `route`,
`term`, `terms`, `enforcement`, `tags`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on missing
or duplicate IDs, invalid references, or out-of-range numbers.

---

## 39. APPENDIX Z — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Open corridors | reach | RoadCorridor |
| Average roughness | road health | RouteInfra |
| Crossing condition | risk | Crossing |
| Convoys run | logistics | Convoy |
| Convoy loss rate | road safety | Convoy |
| Cleared fraction | progress | Clearance |
| Maintenance completed | upkeep | Maintenance |
| Station stock | range | Waystation |
| Agreements active | diplomacy | Agreement |
| Travel time | cost of distance | RouteInfra |

Telemetry is diagnostic only; it never gates content and never becomes a hidden
score.

---

## 40. APPENDIX AA — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive, legacy-neutral, and round-trip tested.
- [ ] Travel resolves through live speed, hazard, and modifier calls.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling checks.
- [ ] Phase 6 content passes the review checklist in §35.
- [ ] Phase 7 soak shows closure, detour, maintenance, and recovery.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No parallel route, vehicle, rail, or gate system exists.

---

## 41. APPENDIX AB — OPEN QUESTIONS FOR REVIEW

1. Can the shelter build a new bridge without the Kiln expansion present?
2. Does a convoy loss always return survivors?
3. Are minefields authored or generated from route data?
4. Do waystations take a cut of trade, and who sets it?
5. Can tolls be refused without territory consequences?
6. Does road maintenance persist through storms or require seasonal rework?
7. Should ferry schedules be player-set or keeper-set?
8. Can the shelter operate a ferry as a permanent service?

None of these may be decided unilaterally; each changes balance and tone.

---

## 42. APPENDIX AC — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 12 The Second Generation | Families moving between settlements |
| 1 | 13 The Faithful | Roadside shrines and pilgrims |
| 1 | 14 Above the Ash | Airfields feeding the roads |
| 1 | 15 The Deep Root | Farm freight and seed runs |
| 1 | 16 The Rebuilt Body | Adapted vehicles and controls |
| 2 | 17 The Long Evening | Road songs and waystation culture |
| 2 | 18 The Underneath | Service tunnels as bypasses |
| 2 | 19 The Bitter Air | Decon at checkpoints |
| 2 | 20 The Quiet Hand | Convoys as cover and smuggling |
| 2 | 21 The Grid | Fuel and charging depots |
| 3 | 22 The Clean Flow | Road drainage and culverts |
| 3 | 23 The Alarm | Convoy rescue and road fires |
| 3 | 24 The Long Goodbye | Last journeys home |
| 3 | 25 The Iron Road | Road-rail interchange |
| 3 | 26 The Common Table | Freight for the table |
| 4 | 27 The Thread | Roadside clothing and salvage |
| 4 | 28 The Lesson | Drivers and scouts trained |
| 4 | 29 The Glass | Signals, gauges, and mirrors |
| 4 | 30 The Press | Route notices and timetables |
| 4 | 31 The Kiln | Bridge masonry and culvert pipes |
| 5 | 32 The Wild | Animal crossings and game routes |
| 5 | 33 The Weather | Route weather and closures |
| 5 | 35 The Habit | Drivers and dependency |
| 5 | 36 The Watch | Escorts and road patrols |

Each hook is additive. The Long Road can ship alone, and every other expansion
can ship without it.

---

## 43. APPENDIX AD — ENDING PROSE SKETCHES

**The Open Road.** The network is maintained, the stations are staffed, and the
shelter moves what it needs in every season without a gamble.

**The Ring Road.** A second corridor and a second span make closure an
inconvenience instead of a crisis.

**The Ferry Republic.** Crossings become the shelter's business and its
leverage, and the river becomes a place with a schedule.

**The Single Lane.** One road, one bridge, and a shelter that holds its breath
from first frost to last thaw, and survives again.

**The Cut Road.** The corridor closes for good, the region shrinks, and the
shelter learns what distance costs when there is no way to pay it.

**Fade.** The same span is patched again, and the same convoy takes the long
way, and the road outlives everyone's patience.

---

## 44. APPENDIX AE — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Roads as free travel | removes cost | speed, roughness, closures |
| Convoy as menu | no loading | cargo and load plans |
| Bridge as scenery | no stakes | condition and capacity |
| Clearance as action | wrong genre | procedure and time |
| Tolls as shakedown | tone break | negotiated rights |
| Route editor | scope break | improvements and bypasses |
| Maintenance ignored | decay invisible | schedules and effects |
| Stations as vending | no keepers | stock, news, defense |
| Loss as full wipe | punishing | partial and survivable |
| Rail/ice duplication | authority break | explicit boundaries |

The list exists because roads are easy to treat as menus. The live route system
already measures the cost; the expansion's job is to make that cost part of the
shelter's weekly life.

---

## 45. APPENDIX AF — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Corridors | 12 | 3,500 |
| Bridges | 15 | 4,000 |
| Ferries and fords | 8 | 2,500 |
| Convoys | 20 | 5,000 |
| Clearance jobs | 20 | 4,500 |
| Maintenance work | 15 | 3,500 |
| Roadside dangers | 20 | 4,000 |
| Waystations | 10 | 4,000 |
| Agreements | 12 | 3,000 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 19 | 2,500 |
| Endings | 6 | 3,000 |
| **Total** | | **~65,000** |

---

## 46. APPENDIX AG — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_road_survey_segment` | 3 | Walk, measure, record |
| `quest_road_mark_route` | 3 | Choose, place, verify |
| `quest_road_seasonal_risk` | 4 | Observe, log, predict |
| `quest_road_bypass` | 4 | Scout, walk, map |
| `quest_road_priority` | 3 | Debate, set, post |
| `quest_road_bridge_inspect` | 4 | Inspect, rate, report |
| `quest_road_bridge_repair` | 5 | Plan, stage, repair, test |
| `quest_road_ford` | 3 | Sound, gravel, mark |
| `quest_road_ferry_run` | 4 | Negotiate, schedule, run |
| `quest_road_culvert` | 4 | Site, lay, backfill |
| `quest_road_convoy_plan` | 4 | Choose, schedule, brief |
| `quest_road_load_plan` | 3 | Weigh, balance, secure |
| `quest_road_escort` | 4 | Assign, travel, return |
| `quest_road_breakdown` | 4 | Diagnose, repair, resume |
| `quest_road_lost_convoy` | 5 | Search, locate, aid |
| `quest_road_debris` | 4 | Clear, haul, verify |
| `quest_road_wreck_clear` | 4 | Survey, cut, haul |
| `quest_road_mine_clear` | 5 | Mark, probe, clear, verify |
| `quest_road_marker` | 3 | Place, range, record |
| `quest_road_verify` | 3 | Walk, confirm, certify |
| `quest_road_station_restore` | 4 | Repair, staff, stock |
| `quest_road_station_stock` | 4 | Source, haul, store |
| `quest_road_station_defense` | 3 | Plan, build, drill |
| `quest_road_station_news` | 3 | Collect, exchange, write |
| `quest_road_station_repair` | 3 | Establish, test, price |
| `quest_road_toll_talk` | 4 | Propose, argue, agree |
| `quest_road_passage_right` | 4 | Draft, sign, enforce |
| `quest_road_mutual_aid` | 4 | Offer, verify, invoke |
| `quest_road_dispute` | 4 | Hear, judge, write |
| `quest_road_shared_map` | 3 | Copy, exchange, merge |

---

## 47. APPENDIX AH — CARGO AND LOAD MODEL

| Cargo | Weight | Space | Fragile | Priority |
|---|---|---|---|---|
| Grain | high | med | no | high |
| Fuel | high | low | no | high |
| Medicine | low | low | yes | highest |
| Glass | med | med | yes | med |
| Brick | high | high | no | low |
| Tools | med | low | no | med |
| Print | low | med | yes | med |
| Seed | med | med | yes | high |
| People | med | high | n/a | highest |
| Water | high | high | no | high |
| Ore | high | low | no | med |
| Timber | high | high | no | low |

Load plans are real: weight, space, fragility, and priority decide what a
convoy can carry and what waits. The expansion makes the shelter's reach
visible in what it leaves behind.

---

## 48. APPENDIX AI — VEHICLE READINESS TABLE

| State | Fuel | Tires | Engine | Load | Ready |
|---|---|---|---|---|---|
| Ready | full | good | serviced | 0 | yes |
| Loaded | full | good | serviced | full | yes |
| Due service | 3/4 | fair | due | any | soon |
| Tire risk | 1/2 | poor | ok | light | no |
| Fuel risk | 1/4 | fair | ok | light | no |
| Disabled | any | any | broken | any | no |
| Destroyed | n/a | n/a | n/a | n/a | never |

Vehicles are the garage's responsibility and the road's constraint. A convoy is
only as ready as its least ready vehicle, which is why the load plan starts in
the garage bay.

---

## 49. APPENDIX AJ — REGIONAL ROAD MAP

| Settlement | Access | Strength | Need | Trade |
|---|---|---|---|---|
| The shelter | highway | convoy ops | fuel | sells transport |
| Market Town | river road | trade | road repair | buys freight |
| Fog Ridge Camp | ridge track | shelter | parts | sells shelter |
| Spring Village | north spur | farms | transport | sells grain |
| Foundry Enclave | valley track | metal | ore | trades parts |
| Deep Bunker | offroad | stores | news | trades supplies |
| River Flotilla | river | boats | roads | moves cargo |
| Coal Stage | ash flats | fuel | food | sells fuel |

Every settlement is a node on the road network, and the shelter's reach is the
set of nodes it can supply in winter. The expansion makes that map a plan rather
than a backdrop.

---

## 50. APPENDIX AK — LORE: THE ROAD TRADITION

The fiction:

- **The Old Highway** was the valley's spine; its milled surface is still the
  best road in the region and the most patched.
- **The Broken Span** was built twice before the Exchange and has been repaired
  three times since.
- **The Ferry Landing** was a private crossing that became a public one when the
  bridge first failed, and Beck's family has run it since.
- **The Rail Cut** was a construction road for a rail line and remains a
  transfer point between road and rail.
- **The Toll Post** was a customs office before the war and its ledger custom
  survived when the government did not.

No real highway, bridge, or agency is copied. The tradition is generic and
local.

---

## 51. APPENDIX AL — SENSITIVE TOPICS TABLE

| Topic | Risk | Handling |
|---|---|---|
| Mine accidents | Gore | Procedure, restraint |
| Convoy deaths | Spectacle | Rare, grieved |
| Banditry | Stereotype | Social cause, no caricature |
| Tolls | Coercion | Negotiated, refusable |
| Refugees | Exploitation | Dignified passage |
| Child drivers | Child labor | 16+ only, supervised |
| Vehicle wrecks | Disaster porn | Salvage and repair |
| Roadside graves | Grief | Marked, respected |
| Fuel politics | Inequality | Authored policy debate |
| Winter isolation | Despair | Stockpile and plan |

The road is where strangers meet, and the expansion's contract is that
everyone on it is a person: traders, refugees, keepers, and crews.

---

## 52. APPENDIX AM — MEASUREMENT AND REVIEW CADENCE

| Gate | Question | Evidence |
|---|---|---|
| Premise | Do live owners still match? | file:line audit |
| Data | Are all rows valid and reachable? | integrity + scanner |
| Core | Are systems pure and deterministic? | unit tests |
| Persistence | Does legacy load neutral? | round-trip tests |
| Host | Is every command real? | selftest + journey |
| UI | Are route costs honest? | lifecycle + a11y tests |
| Tone | Is the road humane? | content review |
| Balance | Is distance meaningful? | 720-day soak |

---

## 53. APPENDIX AN — WINTER OPERATIONS TABLE

| Work | Window | Risk | Requirement |
|---|---|---|---|
| Snow fence | autumn | low | timber, crew |
| Ice breaking | winter | med | tools, truck |
| Sanding | winter | low | sand, crew |
| Plowing | winter | med | vehicle, blade |
| Ferry close | freeze | n/a | schedule |
| Route marking | winter | low | poles, paint |
| Shelter stays open | winter | low | stock |
| Convoy spacing | winter | high | protocol |
| Ice road link | deep winter | high | Exp 01 system |
| Emergency call | storm | high | radio, rescue |

Winter is the road's examination. The expansion's plan is not to defeat winter
but to publish a schedule of work, closures, and reserves, so the shelter is
never surprised by the same month twice.

---

## 54. APPENDIX AO — ROAD EMERGENCY TABLE

| Emergency | Trigger | Response | Escalation |
|---|---|---|---|
| Convoy overdue | time | search | escalate after night |
| Breakdown | vehicle | repair | tow if stuck |
| Injury | accident | first aid | return or clinic |
| Bridge crack | inspection | close | engineer review |
| Mine find | clearance | stop and mark | full job |
| Washout | flood | detour | repair |
| Fire | vehicle | suppress | abandon cargo |
| Storm | weather | shelter | wait |
| Bandit | ambush | withdraw | escort, negotiate |
| Contamination | black rain | decon | clinic, discard |

Emergency responses are authored procedures with clear triggers and
escalations. The road is dangerous in a way the shelter can plan for, which is
the whole argument of the expansion.

---

## 55. APPENDIX AP — OPEN IMPLEMENTATION NOTES

- Corridors must reference live segment ids and never duplicate segment state.
- Crossings should register as route structures whose state affects the live
  segment records through the owning system.
- Convoys should resolve travel with live speed, hazard, and travel modifiers,
  including vehicle capabilities.
- Clearance must write cleared fractions through the live minefield state.
- Maintenance must adjust the live roughness index through the owning system.
- Waystations must extend the live JSON schema rather than a parallel roster.
- Agreements should register with the policy surface so they can be enforced.
- Injuries must route through the medical pipeline; no local health changes.

---

## 56. APPENDIX AQ — FIRST YEAR OF THE ROADWAYS

| Month | Focus | Milestone |
|---|---|---|
| 1 | Bridge crack | convoy turns back |
| 2 | Survey | detours mapped |
| 3 | Minefield | clearance begins |
| 4 | Ferry | crossing running |
| 5 | Waystation | staging stocked |
| 6 | Bridge patch | first crossing |
| 7 | Tolls | rights written |
| 8 | Wrecks | cutting opened |
| 9 | Maintenance | drainage fixed |
| 10 | Winter plan | fences and schedules |
| 11 | Second span | redundancy begins |
| 12 | Road board | operations institution |

A year of roads is a year of small infrastructure victories, and by the end the
shelter has a network instead of a gamble.

---

## 57. APPENDIX AR — ROUTE SURVEY TABLE

| Survey item | Method | Frequency | Output |
|---|---|---|---|
| Surface | walk, measure | monthly | roughness |
| Crossings | inspect | monthly | condition |
| Drainage | walk | season | blockage list |
| Sightlines | walk | season | hazard list |
| Signs | count | month | restore list |
| Traffic | count | week | priority data |
| Incident spots | review | season | warning marks |
| Seasonal change | compare | season | route plan |
| Fuel points | verify | month | range map |
| Shelter points | verify | month | range map |

Surveys are how the road board stays ahead of the road. The work is walking and
writing, and it is the cheapest maintenance the shelter can buy.

---

## 58. APPENDIX AS — BRIDGE LOAD RATING TABLE

| Rating | Load | Traffic | Action |
|---|---|---|---|
| A | full design | all | normal |
| B | 75% design | most | monitor |
| C | 50% design | light | restrict |
| D | 25% design | one vehicle | escort |
| E | pedestrian | foot only | repair now |
| F | closed | none | rebuild |

Load ratings are the expansion's cleanest piece of infrastructure honesty: a
number chalked on a board that tells a driver whether today is a good day to
cross.

---

## 59. APPENDIX AT — CONVOY DEPARTURE CHECKLIST TABLE

| Check | Owner | Fail consequence | Sign-off |
|---|---|---|---|
| Fuel full | driver | stranded | log |
| Tires sound | driver | breakdown | log |
| Load secured | captain | loss | sign |
| Cargo manifest | steward | dispute | sign |
| Route state | scout | delay | sign |
| Weather outlook | forecaster | exposure | sign |
| Escort assigned | captain | risk | sign |
| Fuel cache | quartermaster | range | sign |
| Rations | quartermaster | hunger | sign |
| First aid kit | medic | injury | sign |

Every departure is signed off item by item. The checklist is the road captain's
quiet answer to the question of why convoys fail: because nobody checked.

---

## 60. APPENDIX AU — ROAD SIGN AND NOTICE TABLE

| Sign | Purpose | Location | Upkeep |
|---|---|---|---|
| Distance marker | range | corridor | paint |
| Direction sign | navigation | junction | paint |
| Hazard sign | warning | danger | replace |
| Load limit | bridge | bridge | paint |
| Route closed | closure | gate | remove |
| Fuel ahead | service | approach | update |
| Shelter ahead | safety | route | repair |
| Toll notice | agreement | post | update |
| Ice warning | winter | cold cut | seasonal |
| Contamination | hazard | black basin | replace |

Road signs are the cheapest infrastructure in the expansion and the easiest to
forget. A painted board at a junction saves a convoy a day, and the shelter that
maintains its signs is a shelter that respects other people's time.

---

## 61. CLOSING STATEMENT

ASHFALL already tracks route segments, modes, clearance states, minefields,
speed limits, travel modifiers, vehicles with modifications and armor grades,
weather gates, territory, and waystations with keepers and stock. What it lacks
is the road as a living system: authored corridors, bridges and fords and
ferries, scheduled convoys, clearance crews, maintenance, dangers, and passage
agreements. The Long Road adds that world without adding a second route or
vehicle system. It adds a span that holds one more winter, a convoy that comes
home short, a waystation that keeps two settlements in contact, and the
arithmetic of how far a shelter can reach.

> Wave 5 note: this plan is one of five Wave 5 expansion bibles (32–36). Each is
> self-contained; none requires another to ship. The shared Wave 5 index lives at
> `docs/expansions/wave5/WAVE5_INDEX.md`. The safe pre-signature step is Phase 1
> (data schemas and validators), which is additive and reversible. Evidence
> anchors: `RouteInfrastructureSystem` (`RouteSegmentInfrastructureRecord` with
> road/rail/subway/service_tunnel/offroad modes and
> unbreached/partial/cleared/secured clearance, `MinefieldSegmentState`,
> `RailSegmentCondition`, `GetSpeedLimit`, `GetHazardModifier`,
> `GetTravelModifier`, `CanTraverse`), `waystations.json` (10.8 KB),
> `weather_route_gates.json` (11 KB), `caravan_trade_routes.json` (6.7 KB),
> `caravans.json`/`merchant_caravans.json`, the vehicle and garage systems, and
> the Expansion 01 ice road and Wave 3 rail boundaries.