# Expedition Destination Candidates & Eligibility Audit

## 1. Candidate Population Overview

All 142 definitions in `locations.json` were audited for surface dispatchability. 8 internal bunker/shelter rooms were excluded from expedition routing, leaving 134 candidate surface destinations.

### Excluded Non-Dispatchable Locations (Internal Rooms)
- `loc_holdfast` (The Holdfast bunker command center)
- `loc_shelter_gate` (Shelter Air Gate)
- `loc_shelter_meeting` (Shelter Meeting Room)
- `loc_shelter_infirmary` (Shelter Infirmary)
- `loc_shelter_storage` (Shelter Storage Depot)
- `loc_shelter_quarters` (Shelter Bunk Quarters)
- `loc_shelter_fire` (Shelter Fire Break)
- `loc_shelter_perimeter` (Shelter Perimeter Trench)

## 2. Selection & Distribution Matrix (50 Destinations)

| Tier | Danger Band | Count in Catalog | Selected Candidates |
| :--- | :--- | :--- | :--- |
| **Scavenge** | Danger 1–3 | 12 total | `loc_the_allotments` (existing), `suburban_house`, `rural_gas_station`, `concert_hall_ruins`, `family_bunker_backyard_shed`, `old_library_cache`, `ruined_garage`, `collapsed_building`, `loc_grange_hall`, `loc_apiary_rows`, `loc_school_gymnasium`, `loc_water_station` |
| **Standard** | Danger 4–5 | 19 total | `loc_denial_cut_substation` (existing), `prewar_medical_cache`, `electrical_substation`, `checkpoint_kilo_armory`, `convoy_echo7_cache`, `loc_seed_library_annex`, `loc_veterinary_surgery`, `loc_cider_press`, `loc_ration_queue_plaza`, `loc_conscription_office`, `loc_municipal_archive`, `loc_dentists_row`, `loc_printworks`, `loc_weighbridge`, `loc_motel_verity`, `hospital_pharmacy`, `loc_terrace_pumphouse`, `loc_garrison_checkpoint_gamma`, `loc_grain_silo` |
| **Hazardous** | Danger 6–7 | 13 total | `abandoned_hospital`, `loc_transit_authority_hq`, `loc_department_store`, `loc_public_swimming_baths`, `loc_recovery_yard`, `loc_diesel_tank_farm`, `loc_radio_relay_mast`, `loc_st_brigids_almshouse`, `loc_ordnance_shoulder`, `loc_lock_gate_four`, `loc_pump_station_nine`, `loc_the_shallows_market`, `location_flooded_subway_depot` |
| **Deep** | Danger 8–10 | 6 total | `government_bunker`, `location_geo_thermal_plant_ruins`, `location_silent_observatory`, `location_arcology_sector_4`, `location_ministry_of_truth_bunker`, `location_the_dead_hand_core` |
| **Total** | — | **50** | **2 existing + 48 newly wired** |

## 3. Prioritization Rubric

1. **Player Loop Value (30%):** Core early-to-midgame resource hubs (fuel, medical, food, parts).
2. **Thematic Diversity (25%):** Inclusion of civic, industrial, agricultural, medical, cultural, and military sites.
3. **Geographical & Distance Breadth (20%):** Balanced travel times from 1 hour ($2 \text{ ticks}$) to 9 hours ($18 \text{ ticks}$).
4. **Risk Progression (15%):** Smooth difficulty gradient from Danger 2 (suburban cache) to Danger 10 (Dead Hand Core).
5. **Future Plan Interlocking (10%):** Alignment with Plan 28 ecology corridors and Plan 24 radio distress anchors.
