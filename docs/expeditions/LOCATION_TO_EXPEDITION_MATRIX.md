# Location-to-Expedition Projection Matrix

This matrix documents the projection from authoritative `locations.json` records to the 50 gameplay destinations in `expeditions.json`.

| Location ID | Display Name | Danger | Travel (hr) | Distance (ticks) | Enc / Tick | Drain / hr | Loot Categories |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |
| `loc_the_allotments` | The Works Allotment Commune | 2 | 2.5 | 5 | 0.12 | 2.0 | scrap_metal, clean_water, bandages, food_rations |
| `loc_denial_cut_substation` | The Denial Cut Substation | 4 | 4.0 | 8 | 0.18 | 3.0 | dosimeter, copper_wire, fuel, item_hydro_baron_queue_chit |
| `suburban_house` | Suburban House | 2 | 1.0 | 2 | 0.14 | 2.0 | canned_food, cloth, battery, book |
| `rural_gas_station` | Rural Gas Station | 3 | 1.5 | 3 | 0.16 | 2.2 | fuel, scrap_metal, mechanical_parts, canned_food |
| `concert_hall_ruins` | Concert Hall Ruins | 2 | 1.5 | 3 | 0.14 | 2.0 | cloth, scrap_wood, book, battery |
| `family_bunker_backyard_shed` | Family Bunker: Backyard Shed | 2 | 1.0 | 2 | 0.14 | 2.0 | canned_soup, box_of_nails_10, duct_tape, seed_packets |
| `old_library_cache` | Old Library Cache | 3 | 2.5 | 5 | 0.16 | 2.2 | book, childrens_books, blueprint_roll, cloth |
| `ruined_garage` | Ruined Garage | 3 | 2.0 | 4 | 0.16 | 2.2 | mechanical_parts, metal_pipe, lubricant_oil, scrap_metal |
| `collapsed_building` | Collapsed Building | 3 | 2.0 | 4 | 0.16 | 2.2 | scrap_metal, wooden_plank, box_of_nails_10, concrete_mix |
| `loc_grange_hall` | The Grange Hall | 3 | 1.0 | 2 | 0.16 | 2.2 | seed_packets, scrap_wood, canned_food, rope |
| `loc_apiary_rows` | The Apiary Rows | 3 | 1.5 | 3 | 0.16 | 2.2 | item_honey_pot, roots, berries, clean_water |
| `loc_school_gymnasium` | School Gymnasium | 3 | 1.5 | 3 | 0.16 | 2.2 | bandage, canned_soup, cloth, childrens_books |
| `loc_water_station` | Water Station | 3 | 1.0 | 2 | 0.16 | 2.2 | clean_water, water_filter, water_purification_tablets, metal_pipe |
| `prewar_medical_cache` | Pre-War Medical Cache | 4 | 3.0 | 6 | 0.18 | 2.5 | antibiotics, medical_kit, bandage, splint |
| `electrical_substation` | Electrical Substation | 4 | 3.0 | 6 | 0.18 | 2.5 | electronic_scrap, battery, scrap_metal, mechanical_parts |
| `checkpoint_kilo_armory` | Checkpoint Kilo Armory | 4 | 3.0 | 6 | 0.18 | 2.5 | ammo_9x19, military_mre, bandage, gas_mask |
| `convoy_echo7_cache` | Convoy Echo-7 Cache | 4 | 3.5 | 7 | 0.18 | 2.5 | fuel_canister, mechanical_parts, canned_food, battery |
| `loc_seed_library_annex` | Seed Library Annex | 4 | 1.5 | 3 | 0.18 | 2.5 | seed_packets, growing_manual, chemicals, clean_water |
| `loc_veterinary_surgery` | Large-Animal Surgery | 4 | 2.0 | 4 | 0.18 | 2.5 | bandage, antibiotics, field_surgical_kit, alcohol |
| `loc_cider_press` | The Cider Press | 4 | 2.0 | 4 | 0.18 | 2.5 | canned_food, sugar, clean_water, scrap_wood |
| `loc_ration_queue_plaza` | Ration Plaza | 4 | 1.5 | 3 | 0.18 | 2.5 | canned_food, dried_rations, cloth, scrap_metal |
| `loc_conscription_office` | District Conscription Office | 5 | 1.5 | 3 | 0.20 | 2.8 | book, ammo_9x19, military_mre, bandage |
| `loc_municipal_archive` | Municipal Archive | 5 | 2.0 | 4 | 0.20 | 2.8 | book, blueprint_roll, pocket_notebook, alcohol |
| `loc_dentists_row` | Dentists' Row | 5 | 2.0 | 4 | 0.20 | 2.8 | antibiotics, alcohol, tweezers, bandage |
| `loc_printworks` | The Printworks | 5 | 2.0 | 4 | 0.20 | 2.8 | chemicals, chemical_solvent, book, scrap_metal |
| `loc_weighbridge` | The Weighbridge | 5 | 2.5 | 5 | 0.20 | 2.8 | mechanical_parts, scrap_metal, fuel, diesel_fuel |
| `loc_motel_verity` | The Verity Motel | 5 | 3.0 | 6 | 0.20 | 2.8 | canned_food, cloth, bandage, clean_water |
| `hospital_pharmacy` | Hospital Pharmacy | 5 | 4.0 | 8 | 0.20 | 2.8 | antibiotics, medical_kit, anti_rad, iodine_pills |
| `loc_terrace_pumphouse` | Terrace Pumphouse | 5 | 2.5 | 5 | 0.20 | 2.8 | water_filter, metal_pipe, clean_water, mechanical_parts |
| `loc_garrison_checkpoint_gamma` | Checkpoint Gamma | 4 | 2.5 | 5 | 0.18 | 2.5 | ammo_9x19, military_mre, gas_mask, bandage |
| `loc_grain_silo` | The Grain Exchange | 3 | 2.0 | 4 | 0.16 | 2.2 | canned_food, seed_packets, cloth, scrap_metal |
| `abandoned_hospital` | Abandoned Hospital | 6 | 2.0 | 4 | 0.22 | 3.0 | medical_kit, antibiotics, anti_rad, field_surgical_kit |
| `loc_transit_authority_hq` | Transit Authority | 6 | 2.5 | 5 | 0.22 | 3.0 | mechanical_parts, electronic_scrap, battery, fuel |
| `loc_department_store` | Vansen's Department Store | 6 | 2.5 | 5 | 0.22 | 3.0 | cloth, canned_food, battery, medkit |
| `loc_public_swimming_baths` | Municipal Baths | 6 | 2.0 | 4 | 0.22 | 3.0 | clean_water, chemicals, water_purification_tablets, metal_pipe |
| `loc_recovery_yard` | Recovery Yard | 6 | 3.0 | 6 | 0.22 | 3.0 | scrap_metal, steel_rebar, mechanical_parts, engine |
| `loc_diesel_tank_farm` | Tank Farm 4-East | 6 | 3.5 | 7 | 0.22 | 3.0 | fuel, diesel_fuel, fuel_canister, metal_pipe |
| `loc_radio_relay_mast` | Relay Mast 12 | 6 | 4.0 | 8 | 0.22 | 3.0 | electronic_scrap, vacuum_tube, handheld_radio, aa_batteries |
| `loc_st_brigids_almshouse` | St Brigid's Almshouse | 7 | 3.0 | 6 | 0.24 | 3.2 | bandage, medical_kit, canned_food, cloth |
| `loc_ordnance_shoulder` | The Ordnance Shoulder | 7 | 4.0 | 8 | 0.24 | 3.2 | ammo_762, ammo_12g, ammunition_brass, smokeless_powder |
| `loc_lock_gate_four` | Lock Gate Four | 7 | 5.0 | 10 | 0.24 | 3.2 | mechanical_parts, clean_water, metal_pipe, scrap_metal |
| `loc_pump_station_nine` | Pump Station Nine | 7 | 5.5 | 11 | 0.24 | 3.2 | water_filter, mechanical_parts, fuel, air_filter |
| `loc_the_shallows_market` | The Shallows | 7 | 6.5 | 13 | 0.24 | 3.2 | currency, dried_rations, fuel, military_mre |
| `location_flooded_subway_depot` | Flooded Subway Depot | 7 | 3.5 | 7 | 0.24 | 3.2 | mechanical_parts, electronic_scrap, metal_pipe, clean_water |
| `government_bunker` | Government Bunker | 8 | 4.0 | 8 | 0.26 | 3.5 | military_mre, military_radio, rad_away, anti_rad |
| `location_geo_thermal_plant_ruins` | Geo-Thermal Plant Ruins | 8 | 6.0 | 12 | 0.26 | 3.5 | mechanical_parts, electronic_scrap, battery, solar_cell |
| `location_silent_observatory` | The Silent Observatory | 8 | 7.0 | 14 | 0.26 | 3.5 | vacuum_tube, electronic_scrap, handheld_radio, battery |
| `location_arcology_sector_4` | Arcology Sector 4 | 9 | 8.0 | 16 | 0.28 | 3.8 | electronic_scrap, solar_cell, medical_kit, military_mre |
| `location_ministry_of_truth_bunker` | Ministry of Truth Bunker | 9 | 6.0 | 12 | 0.28 | 3.8 | book, blueprint_roll, military_radio, anti_rad |
| `location_the_dead_hand_core` | The Dead Hand Core | 10 | 9.0 | 18 | 0.30 | 4.0 | rad_away, anti_rad, geiger_counter, electronic_scrap |
