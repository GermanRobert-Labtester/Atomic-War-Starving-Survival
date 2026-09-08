# ASHFALL Distress Destination Matrix

> **Document Status:** Authoritative Mapping & Audit of Distress Signal Destinations
> **Subsystem:** Distress Radio / Expedition System Integration
> **Source Files:** `Assets/StreamingAssets/Data/radio_distress_signals.json`, `Assets/StreamingAssets/Data/radio_distress_signals_expansion.json`, `Assets/StreamingAssets/Data/expeditions.json`
> **Resolver Authority:** `Ashfall.Core.Radio.DistressDestinationResolver` (Invariant 1, pure C#)

---

## 1. Architectural Contract

1. **Expedition Authority:** All expeditions dispatched in ASHFALL must target one of the **55 canonical destinations** defined in `Assets/StreamingAssets/Data/expeditions.json`. Modifying the destination count violates gated regression tests (`Plan32ExpeditionDestinationWiringTests`).
2. **Typed Resolver:** `DistressDestinationResolver` enforces this invariant by resolving every raw location string into a valid canonical destination via a three-tier pipeline:
   - **Tier 1 (Direct):** Matches a canonical destination ID directly in `expeditions.json`.
   - **Tier 2 (Aliased):** Translates legacy world-map coordinates, built-in fallback fixtures, or historic table names into canonical destinations via the versioned alias dictionary.
   - **Tier 3 (Signal Thematic / Fallback):** For signals that contain no explicit geographic coordinates, resolves to a thematic canonical destination (or `collapsed_building` default fallback).

---

## 2. Master Distress Signal Destination Matrix

### Part A: Primary Signals (`radio_distress_signals.json` — 25 Signals)

| Frequency ID | Frequency (MHz) | Signal Name | Authored Location | Canonical Destination ID | Destination Name | Distance (Ticks) | Danger | Scavenging Table ID | Mode |
|---|---|---|---|---|---|---|---|---|---|
| `freq_distress_217_4` | 217.4 | Checkpoint Kilo Automated Beacon | `checkpoint_kilo_armory` | `checkpoint_kilo_armory` | Checkpoint Kilo Armory | 6 | 4 | `table_loot_checkpoint` | Direct |
| `freq_distress_148_2` | 148.2 | Civilian Bunker 4-East | `raider_ambush_site` | `collapsed_building` | Collapsed Building | 4 | 3 | `table_loot_collapsed_structure` | Aliased |
| `freq_distress_392_7` | 392.7 | Automated Weather Station Gamma | *(none)* | `collapsed_building` | Collapsed Building | 4 | 3 | `table_loot_collapsed_structure` | Fallback |
| `freq_distress_55_1` | 55.1 | The Pianist's Last Broadcast | `concert_hall_ruins` | `concert_hall_ruins` | Concert Hall Ruins | 3 | 2 | `table_loot_concert_hall` | Direct |
| `freq_distress_401_9` | 401.9 | Military Convoy Echo-7 | `convoy_echo7_cache` | `convoy_echo7_cache` | Convoy Echo-7 Cache | 7 | 4 | `table_loot_convoy_cache` | Direct |
| `freq_distress_88_3` | 88.3 | Trapped Mechanic at Rail Depot | `loc_recovery_yard` | `loc_recovery_yard` | Recovery Yard | 6 | 6 | `table_loot_recovery_yard` | Direct |
| `freq_distress_156_8` | 156.8 | Injured Trader on Route 6 | `rural_gas_station` | `rural_gas_station` | Rural Gas Station | 3 | 3 | `table_loot_industrial_district` | Direct |
| `freq_distress_203_1` | 203.1 | Isolated Water Treatment Worker | `loc_water_station` | `loc_water_station` | Water Station | 2 | 3 | `table_loot_chemical_plant` | Direct |
| `freq_distress_311_5` | 311.5 | Stranded Expedition Group | `loc_forestry_compound` | `loc_forestry_compound` | Forestry Compound | 6 | 4 | `table_loot_forestry_compound` | Direct |
| `freq_distress_445_2` | 445.2 | Family Shelter Distress Call | `family_bunker_backyard_shed` | `family_bunker_backyard_shed` | Family Bunker: Backyard Shed | 2 | 2 | `table_loot_apartment_block` | Direct |
| `freq_distress_129_6` | 129.6 | Repeating Emergency Beacon | `suburban_house` | `suburban_house` | Suburban House | 2 | 2 | `table_loot_apartment_block` | Direct |
| `freq_distress_278_3` | 278.3 | Old Woman's Garden Broadcast | `loc_school_gymnasium` | `loc_school_gymnasium` | School Gymnasium | 3 | 3 | `table_loot_school` | Direct |
| `freq_distress_367_9` | 367.9 | Dead Man's Loop | `abandoned_hospital` | `abandoned_hospital` | Abandoned Hospital | 4 | 6 | `table_loot_hospital` | Direct |
| `freq_distress_192_4` | 192.4 | Raider Lure: Fuel Cache | `loc_denial_cut_substation` | `loc_denial_cut_substation` | The Denial Cut Substation | 8 | 4 | `table_loot_power_substation` | Direct |
| `freq_distress_410_7` | 410.7 | Scavenger Kidnap Setup | `loc_warehouse_district` | `loc_warehouse_district` | Warehouse District | 5 | 5 | `table_loot_warehouse` | Direct |
| `freq_distress_288_1` | 288.1 | Faction Tactical Bait | `checkpoint_kilo_armory` | `checkpoint_kilo_armory` | Checkpoint Kilo Armory | 6 | 4 | `table_loot_checkpoint` | Direct |
| `freq_distress_333_6` | 333.6 | Impersonated Settlement Call | `loc_grange_hall` | `loc_grange_hall` | The Grange Hall | 2 | 3 | `table_loot_farm` | Direct |
| `freq_distress_478_2` | 478.2 | Impersonated Medical Evacuation | `hospital_pharmacy` | `hospital_pharmacy` | Hospital Pharmacy | 8 | 5 | `table_loot_hospital` | Direct |
| `freq_distress_512_4` | 512.4 | Civil-Defense Emergency Transmitter | `loc_conscription_office` | `loc_conscription_office` | District Conscription Office | 3 | 5 | `table_loot_conscription_office` | Direct |
| `freq_distress_623_8` | 623.8 | Scientific Emergency Beacon | `location_silent_observatory` | `location_silent_observatory` | The Silent Observatory | 14 | 8 | `table_loot_observatory` | Direct |
| `freq_distress_701_3` | 701.3 | Encrypted Military Burst | `checkpoint_kilo_armory` | `checkpoint_kilo_armory` | Checkpoint Kilo Armory | 6 | 4 | `table_loot_checkpoint` | Direct |
| `freq_distress_756_1` | 756.1 | Encrypted Civilian Cipher | `loc_bridge_seven` | `loc_weighbridge` | The Weighbridge | 5 | 5 | `table_loot_weighbridge` | Aliased |
| `freq_distress_812_5` | 812.5 | Child's Call for Help | `loc_school_gymnasium` | `loc_school_gymnasium` | School Gymnasium | 3 | 3 | `table_loot_school` | Direct |
| `freq_distress_867_9` | 867.9 | Siblings Hiding from Threat | `prewar_medical_cache` | `prewar_medical_cache` | Pre-War Medical Cache | 6 | 4 | `table_loot_hospital` | Direct |
| `freq_distress_901_2` | 901.2 | Stranded Military Patrol | `checkpoint_kilo_armory` | `checkpoint_kilo_armory` | Checkpoint Kilo Armory | 6 | 4 | `table_loot_checkpoint` | Direct |

---

### Part B: Expansion Signals (`radio_distress_signals_expansion.json` — 16 Signals)

| Frequency ID | Frequency (MHz) | Signal Name | Authored Location | Canonical Destination ID | Destination Name | Distance (Ticks) | Danger | Scavenging Table ID | Mode |
|---|---|---|---|---|---|---|---|---|---|
| `freq_distress_77_3` | 77.3 | Meridian Cold Store — Sub-Level 2 | *(thematic)* | `loc_the_allotments` | The Works Allotment Commune | 5 | 2 | `table_loot_farm` | Thematic |
| `freq_distress_162_8` | 162.8 | Barge 'Olenka' — VHF Channel 16 | *(thematic)* | `loc_lock_gate_four` | Lock Gate Four | 10 | 7 | `table_loot_waterworks` | Thematic |
| `freq_distress_55_1` | 55.1 | Primary School 14 — Roof Hatch | *(thematic)* | `loc_school_gymnasium` | School Gymnasium | 3 | 3 | `table_loot_school` | Thematic |
| `freq_distress_311_0` | 311.0 | Salt Mine Survey Team | *(thematic)* | `loc_settlement_brine_pans` | Brine-Pan Hollow Salt Camp | 4 | 3 | `table_loot_brine_pans` | Thematic |
| `freq_distress_401_9` | 401.9 | Relay Station Kestrel-9 | *(thematic)* | `loc_radio_relay_mast` | Relay Mast 12 | 8 | 6 | `table_loot_relay_mast` | Thematic |
| `freq_distress_88_5` | 88.5 | Caravan 'Greybell' — Outriders | *(thematic)* | `rural_gas_station` | Rural Gas Station | 3 | 3 | `table_loot_industrial_district` | Thematic |
| `freq_distress_217_4` | 217.4 | District Hospital 3 — Generator Room | *(thematic)* | `abandoned_hospital` | Abandoned Hospital | 4 | 6 | `table_loot_hospital` | Thematic |
| `freq_distress_148_2` | 148.2 | Cape Verity Lighthouse | *(thematic)* | `loc_motel_verity` | The Verity Motel | 6 | 5 | `table_loot_apartment_block` | Thematic |
| `freq_distress_392_7` | 392.7 | North Dam Control — Spillway | *(thematic)* | `loc_pump_station_nine` | Pump Station Nine | 11 | 7 | `table_loot_waterworks` | Thematic |
| `freq_distress_101_3` | 101.3 | Unknown Origin — Band 6 Loop | *(thematic)* | `location_silent_observatory` | The Silent Observatory | 14 | 8 | `table_loot_observatory` | Thematic |
| `freq_distress_55_6` | 55.6 | Patrol Echo-4 — Field Radio | *(thematic)* | `checkpoint_kilo_armory` | Checkpoint Kilo Armory | 6 | 4 | `table_loot_checkpoint` | Thematic |
| `freq_distress_88_9` | 88.9 | Allotment 7 — Greenhouse Bench | *(thematic)* | `loc_the_allotments` | The Works Allotment Commune | 5 | 2 | `table_loot_farm` | Thematic |
| `freq_distress_142_8` | 142.8 | Almshouse Emergency Handset | `loc_st_brigids_almshouse` | `loc_st_brigids_almshouse` | St Brigid's Almshouse | 6 | 7 | `table_loot_hospice_ward` | Direct |
| `freq_distress_166_2` | 166.2 | Substation Omega Maintenance Band | `location_substation_omega` | `electrical_substation` | Electrical Substation | 6 | 4 | `table_loot_power_substation` | Aliased |
| `freq_distress_128_5` | 128.5 | Field Rig, Unsigned (Garrison) | `loc_grange_hall` | `loc_grange_hall` | The Grange Hall | 2 | 3 | `table_loot_farm` | Direct |
| `freq_distress_104_7` | 104.7 | Relay Mast Automated Watch | `loc_radio_relay_mast` | `loc_radio_relay_mast` | Relay Mast 12 | 8 | 6 | `table_loot_relay_mast` | Direct |

---

### Part C: Builtin Fallback Signals (`RadioDistressSystem.RegisterBuiltinCanonicalSignals()` — 8 Signals)

| Frequency ID | Frequency (MHz) | Signal Name | Authored Fallback Location | Canonical Destination ID | Destination Name | Mode |
|---|---|---|---|---|---|---|
| `freq_distress_217_4` | 217.4 | Checkpoint Kilo Automated Beacon | `loc_checkpoint_kilo` | `checkpoint_kilo_armory` | Checkpoint Kilo Armory | Aliased |
| `freq_distress_148_2` | 148.2 | Civilian Bunker 4-East (Raider Bait) | `loc_bunker_4_east_trap` | `collapsed_building` | Collapsed Building | Aliased |
| `freq_distress_108_9` | 108.9 | Sector 9 Electrical Substation | `loc_sector_9_substation` | `electrical_substation` | Electrical Substation | Aliased |
| `freq_distress_134_5` | 134.5 | Relay 44 Bunker SOS | `loc_relay_44_bunker` | `loc_weighbridge` | The Weighbridge | Aliased |
| `freq_distress_162_1` | 162.1 | Marsh Water Caravan Distress | `loc_marsh_caravan_wreck` | `loc_water_station` | Water Station | Aliased |
| `freq_distress_77_3` | 77.3 | Meridian Cold Store — Sub-Level 2 | `loc_meridian_cold_store` | `loc_the_allotments` | The Works Allotment Commune | Aliased |
| `freq_distress_162_8` | 162.8 | Barge 'Olenka' — VHF Channel 16 | `loc_river_barge_olenka` | `loc_lock_gate_four` | Lock Gate Four | Aliased |
| `freq_distress_124_7` | 124.7 | Field Medic Post Omicron | `loc_field_medic_post` | `prewar_medical_cache` | Pre-War Medical Cache | Aliased |

---

## 3. Scavenging Table Aliases (`table_loot_*`)

When systems pass a raw scavenging table identifier, `DistressDestinationResolver` resolves it to its primary expedition destination:

- `table_loot_farm` → `loc_the_allotments`
- `table_loot_power_substation` → `loc_denial_cut_substation`
- `table_loot_apartment_block` → `suburban_house`
- `table_loot_industrial_district` → `rural_gas_station`
- `table_loot_concert_hall` → `concert_hall_ruins`
- `table_loot_school` → `loc_school_gymnasium`
- `table_loot_warehouse` → `loc_warehouse_district`
- `table_loot_collapsed_structure` → `collapsed_building`
- `table_loot_chemical_plant` → `loc_water_station`
- `table_loot_hospital` → `hospital_pharmacy`
- `table_loot_checkpoint` → `checkpoint_kilo_armory`
- `table_loot_convoy_cache` → `convoy_echo7_cache`
- `table_loot_weighbridge` → `loc_weighbridge`
- `table_loot_waterworks` → `loc_terrace_pumphouse`
- `table_loot_recovery_yard` → `loc_recovery_yard`
- `table_loot_tank_farm` → `loc_diesel_tank_farm`
- `table_loot_relay_mast` → `loc_radio_relay_mast`
- `table_loot_hospice_ward` → `loc_st_brigids_almshouse`
- `table_loot_ordnance_shoulder` → `loc_ordnance_shoulder`
- `table_loot_shallows_market` → `loc_the_shallows_market`
- `table_loot_metro_station` → `location_flooded_subway_depot`
- `table_loot_government_bunker` → `government_bunker`
- `table_loot_geo_thermal_plant` → `location_geo_thermal_plant_ruins`
- `table_loot_observatory` → `location_silent_observatory`
- `table_loot_arcology_sector_4` → `location_arcology_sector_4`
- `table_loot_ministry_bunker` → `location_ministry_of_truth_bunker`
- `table_loot_dead_hand_core` → `location_the_dead_hand_core`
- `table_loot_tinkers_notch` → `loc_settlement_tinkers_notch`
- `table_loot_pilgrim_hearth` → `loc_settlement_pilgrim_hearth`
- `table_loot_brine_pans` → `loc_settlement_brine_pans`
- `table_loot_forestry_compound` → `loc_forestry_compound`

---

## 4. Verification

Every row in this matrix is verified by `Ashfall.Core.Tests.Radio.DistressDestinationResolverTests`, guaranteeing:
1. 100% of signals resolve to a canonical destination ID present in `expeditions.json`.
2. All 55 canonical destinations are reachable.
3. Every resolution provides non-zero distanceTicks, valid dangerLevel (1..10), and a valid scavenging table.
