# Plan 28 — machine-readable diagnostics (Phases 3-4, generated 2026-09-01)

Source authorities: world_evolution_seeds.json + ecological_infestations.json.

## 1. Species / pack inventory

- `pack_hinterland_dogs` | species_rad_dog | sector_4_hinterlands | pop 6
- `pack_hill_wolves` | species_wolf | sector_4_hills | pop 4
- `pack_floodplain_boars` | species_ash_boar | sector_4_floodplain | pop 7
- `pack_canyon_rats` | species_blight_rat | sector_4_canyon | pop 12
- `pack_railway_crows` | species_iron_crow | sector_4_railway_cut | pop 9
- `pack_river_herons` | species_gray_heron | sector_4_river | pop 3
- `pack_junction_dogs` | species_rad_dog | sector_4_highway_junction | pop 5
- `pack_bluffs_goats` | species_feral_goat | sector_8_bluffs | pop 6
- `pack_lowlands_hares` | species_cotton_hare | sector_8_lowlands | pop 10
- `pack_estuary_gulls` | species_ash_gull | sector_8_estuary | pop 14
- `pack_quarries_lynx` | species_dust_lynx | sector_8_quarries | pop 2
- `pack_river_carp_run` | species_mirror_carp | sector_4_river | pop 18
- `pack_lowlands_moth_bloom` | species_ghost_moth | sector_8_lowlands | pop 16
- water sectors: sector_4_river, sector_8_estuary

## 2. Infestation eligibility report (10 authored)

- `infestation_subway_molerat_nest` | location | loc_flooded_subway_depot | seasons: any | food_loss/day: 0 | leave: canned_food x2 (max 2)
- `infestation_quarry_hornet_hive` | location | location_quarry_overlook | seasons: window_thaw,window_black_bloom | food_loss/day: 0 | leave: medkit x1 (max 1)
- `infestation_cellar_mold_bloom` | location | loc_cider_press | seasons: window_thaw,window_black_bloom | food_loss/day: 0 | leave: item_mycelium_bricks x1 (max 1)
- `infestation_bunker_roach_colony` | location | loc_ordnance_shoulder | seasons: any | food_loss/day: 0 | leave: canned_food x1 (max 1)
- `infestation_canal_fungal_carpet` | location | loc_bridge_seven | seasons: window_thaw | food_loss/day: 0 | leave: item_bio_plastic x1 (max 1)
- `infestation_mill_rat_king` | location | loc_printworks | seasons: window_deep_freeze,window_high_cold | food_loss/day: 0 | leave: fuel_canister x1 (max 1)
- `infestation_shelter_vent_mold` | shelter | room_filtration | seasons: window_thaw,window_black_bloom | food_loss/day: 0 | leave: none (max 0)
- `infestation_shelter_pantry_weevils` | shelter | room_storage_bay | seasons: window_thaw,window_black_bloom | food_loss/day: 2 | leave: canned_food x1 (max 1)
- `infestation_shelter_wall_nest` | shelter | room_bunks | seasons: window_deep_freeze,window_high_cold | food_loss/day: 1 | leave: none (max 0)
- `infestation_shelter_tray_cutworm` | shelter | room_greenhouse | seasons: window_thaw,window_black_bloom | food_loss/day: 1 | leave: none (max 0)

## 3. Corridor validation report

- 11 sectors / 13 packs / 2 water flags - all nodes resolve (selftest steps 2-3)
- location-seed sector bindings: all 12 map onto the 11-sector graph

## 4. Unreachable-content report

- orphan species: 0; invalid corridors: 0; unreachable events: 0 (8 event_eco_* in the live event runtime)
- infestations without resolution: 0 (all 10 have at least one authored clear option)