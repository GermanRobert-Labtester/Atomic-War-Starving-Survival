# Faction War Location Resolution Contract

> **Catalog Authority:** `Assets/StreamingAssets/Data/faction_war_location_overrides.json`
> **Target Catalogs:** `locations.json`, `deep_lore_locations.json`, `crossing_locations.json`, `year_of_ash_locations.json`, `damaged_zone_locations.json`
> **Test Gate:** `Ashfall.Core.Tests.FactionWarLocationOverridesExpansionTests.AllOverrides_TargetCanonicalLocations`

---

## 1. Zero Orphan / Zero Invented Location Policy

A core architectural invariant of ASHFALL is that content expansions must never invent unreferenced ID strings. Every `locationId` declared in `faction_war_location_overrides.json` must resolve against an existing canonical location in the data authority.

---

## 2. Complete Resolution Table (20 Overrides)

| Index | Override ID | `locationId` | Source Catalog | Location Title / Entity |
|---|---|---|---|---|
| 0 | `loc_override_almshouse_pre_strike` | `loc_almshouse` | `locations.json` | St. Jude's Almshouse |
| 1 | `loc_override_almshouse_post_strike` | `loc_almshouse` | `locations.json` | St. Jude's Almshouse |
| 2 | `loc_override_plaza_cleared` | `loc_ration_queue_plaza` | `locations.json` | Ration Distribution Plaza |
| 3 | `loc_override_silo_fortified` | `loc_grain_silo` | `locations.json` | Leaning Grain Silo |
| 4 | `loc_override_conscription_burned` | `loc_conscription_office` | `locations.json` | District Conscription Office |
| 5 | `loc_override_cache_looted` | `loc_d9_cache_bunker_delta` | `locations.json` | D/9 Cache Bunker Delta |
| 6 | `loc_override_weighbridge_barricaded` | `loc_weighbridge` | `locations.json` | North Weighbridge Checkpoint |
| 7 | `loc_override_waystation_refugee` | `loc_shrine_switchback_waystation` | `locations.json` | Shrine Switchback Waystation |
| 8 | `loc_override_understory_transmitter_ambient` | `loc_understory_transmitter` | `locations.json` | Understory Radio Mast |
| 9 | `loc_override_checkpoint_occupied` | `loc_garrison_checkpoint_gamma` | `year_of_ash_locations.json` | Garrison Checkpoint Gamma |
| 10 | `loc_override_granary_burned` | `loc_crossing_granary_pledge` | `crossing_locations.json` | Crossing Granary Pledge Post |
| 11 | `loc_override_well_contaminated` | `location_municipal_water_reservoir` | `deep_lore_locations.json` | Municipal Water Reservoir |
| 12 | `loc_override_rail_yard_fortified` | `loc_sector_4_rail_switchyard` | `year_of_ash_locations.json` | Sector 4 Rail Switchyard |
| 13 | `loc_override_village_abandoned` | `loc_settlement_iron_siding` | `locations.json` | Iron Siding Settlement |
| 14 | `loc_override_factory_occupied` | `location_chemical_plant` | `deep_lore_locations.json` | Abandoned Chemical Synthesis Plant |
| 15 | `loc_override_bridge_destroyed` | `loc_bridge_seven` | `locations.json` | Bridge Seven Crossing |
| 16 | `loc_override_roadblock_liberated` | `loc_ash_militia_deadfall_barrier` | `year_of_ash_locations.json` | Ash Militia Deadfall Barrier |
| 17 | `loc_override_camp_overrun` | `loc_crossing_petition_tent` | `crossing_locations.json` | Crossing Petition & Assembly Tent |
| 18 | `loc_override_station_reclaimed` | `location_metro_station` | `deep_lore_locations.json` | Flooded Metro Station |
| 19 | `loc_override_field_scorched` | `location_burned_woodland` | `deep_lore_locations.json` | Burned Woodland Basin |

---

## 3. Catalog Distribution Summary

- `locations.json`: 9 locations (10 overrides)
- `deep_lore_locations.json`: 4 locations (4 overrides)
- `year_of_ash_locations.json`: 3 locations (3 overrides)
- `crossing_locations.json`: 2 locations (2 overrides)

Total: **19 distinct locations covered across 20 overrides**. 100% of location references are verified against authoritative catalogs.
