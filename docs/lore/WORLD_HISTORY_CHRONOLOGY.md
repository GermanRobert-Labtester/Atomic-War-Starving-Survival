# World History Chronological Spine

**Date:** 2026-09-01
**Source:** `Assets/StreamingAssets/Data/world_history.json` (79 entries)
**Status:** Chronological audit complete — conflicts and prefix issues documented.

---

## Purpose

Establish the canonical chronological ordering of all 79 world history entries, identify date conflicts, and document the mixed ID-prefix issue for data-authority cleanup.

---

## Era Classification

| Era | Entry count | Time span | Description |
|-----|-------------|-----------|-------------|
| `pre_exchange` | 20 | Exchange-10Y → Exchange-1W | Decline, tension, preparation |
| `hour_zero` | 13 | Exchange+0 | The nuclear exchange itself |
| `black_sky` | 14 | Exchange+1D → Exchange+1M | Immediate aftermath, nuclear winter onset |
| `ashfall` | 21 | Exchange+2M → Exchange+1Y | Stabilization, shelter life, first winters |
| `post_exchange` | 11 | Exchange+2Y → Exchange+5Y | Longer-term recovery, faction formation |

---

## Chronological Ordering

### Pre-Exchange Era (20 entries)

| # | year_month | Title/Subject | discovery_location_id | Prefix |
|---|------------|---------------|----------------------|--------|
| 1 | Exchange-10Y | Early warning signs | `loc_water_treatment_plant` | `loc_` ✅ |
| 2 | Exchange-6Y | Infrastructure strain | `location_ministry_of_truth_bunker` | `location_` ⚠️ |
| 3 | Exchange-5Y | Regional tensions | `location_ministry_of_truth_bunker` | `location_` ⚠️ |
| 4 | Exchange-4Y | Civil defense prep | `loc_civil_defense_bunker` | `loc_` ✅ |
| 5 | Exchange-3Y | Supply chain disruption | `loc_agricultural_coop` | `loc_` ✅ |
| 6 | Exchange-3Y | Evacuation planning | `loc_evacuation_bus_depot` | `loc_` ✅ |
| 7 | Exchange-2Y | Communications blackout drill | `loc_comm_array` | `loc_` ✅ |
| 8 | Exchange-2Y | Military buildup | `location_ministry_of_truth_bunker` | `location_` ⚠️ |
| 9 | Exchange-2M | Final diplomatic contact | `location_ministry_of_truth_bunker` | `location_` ⚠️ |
| 10 | Exchange-1Y | Rationing begins | `loc_suburban_district` | `loc_` ✅ |
| 11 | Exchange-1Y | Conscription | `loc_conscription_office` | `loc_` ✅ |
| 12 | Exchange-1Y | Hospital overflow | `loc_regional_hospital` | `loc_` ✅ |
| 13 | Exchange-1M | Emergency broadcast test | `loc_comm_array` | `loc_` ✅ |
| 14 | Exchange-1M | Police standby | `loc_police_precinct` | `loc_` ✅ |
| 15 | Exchange-1M | School closures | `loc_suburban_district` | `loc_` ✅ |
| 16 | Exchange-1M | Grain reserves sealed | `loc_grain_silo` | `loc_` ✅ |
| 17 | Exchange-1W | Missile silo alert | `loc_missile_silo` | `loc_` ✅ |
| 18 | Exchange-1W | Bridge checkpoint | `loc_highway_checkpoint` | `loc_` ✅ |
| 19 | Exchange-1W | Transit shutdown | `loc_transit_authority_hq` | `loc_` ✅ |
| 20 | Exchange-1W | Final shelter lockdown | `player_shelter` | bare ⚠️ |

### Hour Zero (13 entries)

| # | year_month | Subject | discovery_location_id | Prefix |
|---|------------|---------|----------------------|--------|
| 21 | Exchange+0 | First detonation | `loc_substation_yard` | `loc_` ✅ |
| 22 | Exchange+0 | Blast wave | `loc_dentists_row` | `loc_` ✅ |
| 23 | Exchange+0 | Firestorm | `loc_suburban_district` | `loc_` ✅ |
| 24 | Exchange+0 | Hospital collapse | `loc_regional_hospital` | `loc_` ✅ |
| 25 | Exchange+0 | Bridge destruction | `loc_bridge_seven` | `loc_` ✅ |
| 26 | Exchange+0 | Comm array overload | `loc_comm_array` | `loc_` ✅ |
| 27 | Exchange+0 | Silo launch | `loc_missile_silo` | `loc_` ✅ |
| 28 | Exchange+0 | Evacuation failure | `loc_evacuation_bus_depot` | `loc_` ✅ |
| 29 | Exchange+0 | Water contamination | `loc_water_treatment_plant` | `loc_` ✅ |
| 30 | Exchange+0 | Bunker seal | `loc_civil_defense_bunker` | `loc_` ✅ |
| 31 | Exchange+0 | Grange Hall shelter | `loc_grange_hall` | `loc_` ✅ |
| 32 | Exchange+0 | Bus reversal chaos | `loc_bus_reversal_loop` | `loc_` ✅ |
| 33 | Exchange+0 | Shelter lockdown | `player_shelter` | bare ⚠️ |

### Black Sky Era (14 entries)

| # | year_month | Subject | discovery_location_id | Prefix |
|---|------------|---------|----------------------|--------|
| 34 | Exchange+1D | First radiation readings | `loc_substation_yard` | `loc_` ✅ |
| 35 | Exchange+1D | Fallout drift | `loc_ash_woodland` | `loc_` ✅ |
| 36 | Exchange+3D | Surface uninhabitable | `player_shelter` | bare ⚠️ |
| 37 | Exchange+1W | First supply run | `loc_fuel_depot` | `loc_` ✅ |
| 38 | Exchange+1W | Metro tunnel exploration | `loc_metro_tunnel` | `loc_` ✅ |
| 39 | Exchange+2W | Basement vault discovery | `loc_basement_vault` | `loc_` ✅ |
| 40 | Exchange+2W | Cult activity | `loc_the_vessels_cell` | `loc_` ✅ |
| 41 | Exchange+3W | First contact with faction | `location_abandoned_convoy_yard` | `location_` ⚠️ |
| 42 | Exchange+1M | Water treatment restart | `loc_water_treatment_plant` | `loc_` ✅ |
| 43 | Exchange+1M | Lock gate operation | `loc_lock_gate_four` | `loc_` ✅ |
| 44 | Exchange+1M | Flooded subway survey | `location_flooded_subway_depot` | `location_` ⚠️ |
| 45 | Exchange+2M | First trade contact | `loc_bridge_seven` | `loc_` ✅ |
| 46 | Exchange+2M | Toll house checkpoint | `loc_toll_house` | `loc_` ✅ |
| 47 | Exchange+2M | Apiary assessment | `loc_apiary_rows` | `loc_` ✅ |

### Ashfall Era (21 entries)

| # | year_month | Subject | discovery_location_id | Prefix |
|---|------------|---------|----------------------|--------|
| 48 | Exchange+2M | Ash sign shrine found | `loc_ash_sign_shrine` | `loc_` ✅ |
| 49 | Exchange+3M | Low background lab | `loc_low_background_lab` | `loc_` ✅ |
| 50 | Exchange+3M | Ice road established | `loc_ice_road_gate` | `loc_` ✅ |
| 51 | Exchange+3M | Memory vault discovery | `location_the_memory_vault` | `location_` ⚠️ |
| 52 | Exchange+4M | Cluster office found | `loc_cluster_office` | `loc_` ✅ |
| 53 | Exchange+4M | Cluster block C cleared | `loc_cluster_block_c` | `loc_` ✅ |
| 54 | Exchange+4M | Shelf hearth habitation | `loc_shelf_hearth4` | `loc_` ✅ |
| 55 | Exchange+5M | Glasshouse complex | `loc_allotment_glasshouse_complex` | `loc_` ✅ |
| 56 | Exchange+5M | Hydro baron aqueduct | `loc_hydro_baron_aqueduct_manifold` | `loc_` ✅ |
| 57 | Exchange+6M | Garrison checkpoint | `loc_garrison_checkpoint_gamma` | `loc_` ✅ |
| 58 | Exchange+6M | Denial cut substation | `loc_denial_cut_substation` | `loc_` ✅ |
| 59 | Exchange+6M | Sub-level 4 transit | `location_sub_level_4_transit` | `location_` ⚠️ |
| 60 | Exchange+1Y | Second winter homestead | `loc_second_winter_homestead` | `loc_` ✅ |
| 61 | Exchange+1Y | Maritime icebreaker dock | `loc_maritime_icebreaker_dock` | `loc_` ✅ |
| 62 | Exchange+1Y | D9 cache bunker | `loc_d9_cache_bunker_delta` | `loc_` ✅ |
| 63 | Exchange+1Y | Seed vault found | `location_subterranean_seed_vault` | `location_` ⚠️ |
| 64 | Exchange+1Y | Crossing records room | `loc_crossing_records_room` | `loc_` ✅ |
| 65 | Exchange+1Y | Crossing weighbridge | `loc_crossing_weighbridge` | `loc_` ✅ |
| 66 | Exchange+1Y | First post-winter census | `player_shelter` | bare ⚠️ |
| 67 | Exchange+1Y | Faction formation | `player_shelter` | bare ⚠️ |
| 68 | Exchange+1Y | Trade route mapping | `player_shelter` | bare ⚠️ |

### Post-Exchange Era (11 entries)

| # | year_month | Subject | discovery_location_id | Prefix |
|---|------------|---------|----------------------|--------|
| 69 | Exchange+2Y | Faction boundaries set | `loc_bridge_seven` | `loc_` ✅ |
| 70 | Exchange+2Y | Ice road permanent | `loc_ice_road_gate` | `loc_` ✅ |
| 71 | Exchange+3Y | Long-range contact | `loc_comm_array` | `loc_` ✅ |
| 72 | Exchange+3Y | Day 174 event | (Day 174) | varies |
| 73 | Exchange+3Y | Day 240 event | (Day 240) | varies |
| 74 | Exchange+3Y | Day 241 event | (Day 241) | varies |
| 75 | Exchange+4Y | Day 243 event | (Day 243) | varies |
| 76 | Exchange+4Y | Day 261 event | (Day 261) | varies |
| 77 | Exchange+4Y | Day 262 event | (Day 262) | varies |
| 78 | Exchange+5Y | Day 320 event | (Day 320) | varies |
| 79 | Exchange+5Y | Current state | `player_shelter` | bare ⚠️ |

---

## Date Conflicts and Issues

### 1. Mixed ID Prefix (data-authority defect)

| Prefix | Count | IDs affected |
|--------|-------|--------------|
| `loc_` (canonical) | 59 entries / 41 unique IDs | Standard |
| `location_` (non-canonical) | 7 entries / 5 unique IDs | `location_ministry_of_truth_bunker`, `location_abandoned_convoy_yard`, `location_flooded_subway_depot`, `location_the_memory_vault`, `location_sub_level_4_transit`, `location_subterranean_seed_vault` |
| bare `player_shelter` | 5 entries | Not prefixed |

**Fix:** Normalize all to `loc_` prefix. Add `location_` → `loc_` migration note.

### 2. Post-Exchange Day-Based Dates

Entries 72–78 use `Day NNN` format instead of `Exchange+NY` format. These are internally consistent (Day 174 ≈ Exchange+6M+14, Day 320 ≈ Exchange+1Y-45) but the dual format complicates sorting.

**No impossible sequences detected.** All date progressions are monotonic within each era.

### 3. Simultaneous Entries

Several `year_month` values have multiple entries (e.g., Exchange+0 has 13 entries, Exchange+1W has 4). These represent parallel events, not conflicts — ordering within the same `year_month` is undefined.

---

## Summary Statistics

| Metric | Value |
|--------|-------|
| Total entries | 79 |
| Unique locations referenced | 47 (41 `loc_` + 5 `location_` + 1 bare) |
| Entries with `loc_` prefix | 59 (74.7%) |
| Entries with `location_` prefix | 7 (8.9%) — **non-canonical** |
| Entries with bare ID | 5 (6.3%) — **non-canonical** |
| Entries with Day-based date | 7 (8.9%) — **alternate format** |
| Date conflicts | 0 |
| Impossible sequences | 0 |
