# Plan 16 — Cartography & Infrastructure Baseline Report

**Execution Timestamp:** 2026-09-01T00:34:00Z
**Project:** ASHFALL (Godot 4.7+ / .NET 8 / C# / xUnit .NET 9)
**Target:** Physical & Institutional Cartography Densification (Nodes, Routes, Waystations, Caravans, Treaties, Embargoes)

---

## 1. Executive Summary

ASHFALL’s world simulation contains extensive narrative, faction, and quest systems, but the live traversable map graph (`wasteland_map_v1.json`) historically exposed only 14 nodes and 15 routes, while `locations.json` authoritatively defines 123 distinct wasteland locations.

This baseline report captures the initial state of the world graph, environmental data, waystation and caravan logistics, and diplomatic treaty layers before executing the 60-node densification, 6-waystation infrastructure network, 4-caravan circuit schedule, and 12-treaty accord web.

---

## 2. Graph & Data Topology Baseline

### 2.1 Map Graph (`wasteland_map_v1.json`)
- **Total Nodes:** 14 (1 starting unlocked: `loc_holdfast`, `loc_cut_merchant_caravanserai`; 12 discoverable)
- **Total Routes:** 15 directed travel edges
- **Shelter Component:** 1 fully connected component containing `loc_holdfast`
- **Coordinate Canvas Bounds:** X: [250, 900], Y: [120, 600]
- **Danger Tier Distribution:**
  - `none`: 1 (`loc_holdfast`)
  - `low`: 4 (`loc_cut_abandoned_depot`, `loc_cut_merchant_caravanserai`, `loc_excavation_utility_tunnels`, `loc_logistics_reserve_cache`)
  - `medium`: 6 (`loc_cut_arsenal_ruin`, `loc_excavation_command_vault`, `loc_excavation_metro_interchange`, `loc_excavation_archive_bunker`, `loc_hidden_relay_bunker`, `loc_deaddrop_command_shelter`)
  - `high`: 2 (`loc_cut_radiation_zone_alpha`, `loc_excavation_mine_shaft`)
  - `locked`: 1 (`loc_black_flotilla_outpost`)

### 2.2 Authored Location Inventory (`locations.json`)
- **Total Authored Locations:** 123
- **Coverage on Live Map:** 14 / 123 (11.4%)
- **Unmapped Authored Locations:** 109 / 123 (88.6%)
- **Target Map Coverage:** ~60 nodes (48.8% of authored locations), organized across 6 macro-regions.

### 2.3 Environmental & Damaged Map Data
- **`damaged_map_zones.json`:** 3 initial zones (`industrial_district`, `suburban_heights`, `military_corridor`) with 8 total map fragments.
- **`currents.json`:** 17 cultural currents / faction entries across wasteland regions (`the_drown`, `the_grid`, `the_verge`, `the_scarp`, `the_hinterlands`).

### 2.4 Waystation & Logistics Baseline
- **`WaystationSystem.cs`:** Supports single forward camp (`loc_cut_waystation_a`) with stove, filter degradation, bunk occupancy, and watch assignments.
- **`TravelingCaravanSystem.cs`:** Manages wandering caravan state (`CaravanEntry`, `CaravanInventoryItem`, `TravelingCaravanState`), scheduled node transit, days-at-node stay durations, and regional specialty inventory.

### 2.5 Diplomatic Treaty Baseline
- **`foundry_accords.json`:** 4 initial treaties (`treaty_brine_pipe_and_iodine_exchange`, `treaty_cluster_labour_schedule`, `treaty_road_iron_charter`, `treaty_the_cluster_charter`).
- **`foundry_treaty_consequences.json`:** 6 consequence policies with market demand deltas and standing shifts.
- **`RegionalTreatySystem.cs`:** State machine supporting proposal, ratification, violation, compliance checks, and status event propagation.

---

## 3. Six Macro-Region Schema

The expanded 60-node wasteland graph will be partitioned into 6 distinct geographic and environmental zones:

| Macro-Region | Coordinate Region | Dominant Hazard / Trait | Core Factions / Themes | Representative Anchor Locations |
|---|---|---|---|---|
| **1. Crater Core** | X: [400–700], Y: [300–550] | High Radiation, Magnetic Anomalies, Ground Zero Ruins | Military remnants, Dead Hand, Automated Defenses | `loc_holdfast`, `loc_cut_radiation_zone_alpha`, `location_the_dead_hand_core`, `location_magnetic_anomaly_crater` |
| **2. Dead Suburbs** | X: [200–500], Y: [100–350] | Scavenger Traps, Structural Collapse, Low Rads | Civilian caches, Old Library, Family Bunkers | `loc_cut_merchant_caravanserai`, `loc_grange_hall`, `loc_school_gymnasium`, `loc_logistics_reserve_cache` |
| **3. Industrial Belt** | X: [600–950], Y: [100–350] | Chemical Seepage, Rail Chokepoints, Smelter Hazards | Silent Foundry, Transit Authority, Weighbridge | `loc_cut_abandoned_depot`, `loc_cut_arsenal_ruin`, `loc_weighbridge`, `loc_diesel_tank_farm`, `loc_railway_span_44_alpha` |
| **4. Deep Coast** | X: [450–850], Y: [550–850] | Flooding, Salt Fog, Corrosive Water, Sump Gas | Black Flotilla, Maritime Salvage, Drowned Arcologies | `loc_black_flotilla_outpost`, `loc_cold_store_atlantic`, `loc_bathymetric_boat`, `loc_the_shallows_market` |
| **5. Ash Flats & The Verge** | X: [800–1200], Y: [250–650] | High Wind Exposure, Particulate Ash, Toll Roads | Central Garrison, Rebuilders, Forward Roster | `loc_grain_silo`, `loc_garrison_checkpoint_gamma`, `loc_forward_roster_camp`, `loc_eastern_road` |
| **6. Northern Treeline & High Scarp** | X: [100–450], Y: [500–850] | Extreme Cold, Avalanches, High Altitude, Deep Bunkers | Cult of Ash Sign, Research Stations, Observatories | `loc_shrine_switchback_waystation`, `loc_snowline_station`, `loc_pilgrim_switchbacks`, `loc_low_background_lab` |

---

## 4. Verification Baseline

- **xUnit Test Count:** 5,327 tests passing (0 failures).
- **Data Integrity Selftest:** 140 catalogs passing with 0 errors.
- **Content Utilization Selftest:** 415 catalogs scanned, CI gate PASS.
- **Scene Binding Selftest:** 22/22 scenes verified.
- **Scene Linter:** 26 scenes checked, 0 errors.
