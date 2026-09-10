# Plan 145 Location Projection Matrix

Maps every authored graffiti posting to an explicit canonical game target (shelter room or wasteland location).

| Posting ID | Day | Authored Location | Canonical Target ID | Canonical Display | Scope | Target UI Surface |
|---|---:|---|---|---|---|---|
| `graf_01_the_first_stoker_rule` | 3 | Boiler Room Corridor (-10m) | `room_filtration` | Filtration & Boiler Stack | Exact shelter room | ShelterPanel (Filtration) |
| `graf_02_ration_biscuit_warning` | 12 | Canteen Entryway Slate Board | `room_kitchen` | Galley Kitchen | Exact shelter room | ShelterPanel (Kitchen) |
| `graf_03_missing_spanner` | 28 | Machine Shop Tool Rack | `room_workshop` | Workshop | Exact shelter room | ShelterPanel (Workshop) |
| `graf_04_the_first_clandestine_slogan` | 45 | Ventilation Shaft 3 Access Wall | `room_filtration` | Ventilation Stack Shaft 3 | Exact shelter room | ShelterPanel (Filtration) |
| `graf_05_the_snoring_manifesto` | 62 | Bunk Room 2 Door Frame | `room_bunks` | Bunk Living | Exact shelter room | ShelterPanel (Bunks) |
| `graf_06_vel_dispensary_notice` | 78 | Clinic Waiting Alcove | `room_clinic` | Medical Ward | Exact shelter room | ShelterPanel (Clinic) |
| `graf_07_secret_romance_cipher` | 95 | Hydroponic Pipe Junction 7 | `room_greenhouse` | Greenhouse / Hydroponics | Exact shelter room | ShelterPanel (Greenhouse) |
| `graf_08_voss_inspection_order` | 110 | Surface Airlock Inner Airway | `room_airlock` | Airlock Hatch | Exact shelter room | ShelterPanel (Airlock) |
| `graf_09_the_shoe_sole_controversy` | 135 | Shoemaker's Bench Blackboard | `room_workshop` | Workshop | Exact shelter room | ShelterPanel (Workshop) |
| `graf_10_the_weepers_psalm` | 160 | Lower Crypt Archway | `room_main` | Lower Crypt / Main Vault | Exact shelter room | ShelterPanel (Main Vault) |
| `graf_11_the_mushroom_soup_boycott` | 190 | Mess Hall Notice Board | `room_kitchen` | Galley Kitchen | Exact shelter room | ShelterPanel (Kitchen) |
| `graf_12_the_ghost_train_rumor` | 225 | Sub-Level 2 Rail Tunnel Entrance | `location_sub_level_4_transit` | Sub-Level 4 Transit / Rail Tunnel | Exact canonical world location | MapDetailPanel |
| `graf_13_the_baby_tally` | 260 | Infirmary Delivery Room Wall | `room_clinic` | Medical Ward Delivery Bay | Exact shelter room | ShelterPanel (Clinic) |
| `graf_14_tobacco_contraband_warning` | 295 | Upper Airlock Guard Post | `room_airlock` | Upper Airlock Guard Post | Exact shelter room | ShelterPanel (Airlock) |
| `graf_15_the_century_seed_pledge` | 330 | Main Assembly Hall Pillar | `room_main` | Main Vault Assembly Pillar | Exact shelter room | ShelterPanel (Main Vault) |
| `graf_16_the_hydroponic_slug_bounty` | 375 | Greenhouse Bed 4 Header | `room_greenhouse` | Greenhouse | Exact shelter room | ShelterPanel (Greenhouse) |
| `graf_17_the_frozen_pipe_blame_game` | 420 | Washhouse Cold Water Manifold | `room_water_pump` | Washhouse Water Manifold | Exact shelter room | ShelterPanel (Water Pump) |
| `graf_18_the_black_wire_warning` | 460 | Radio Alcove Partition | `room_radio_tuner` | Tuner Station Alcove | Exact shelter room | ShelterPanel (Radio Tuner) |
| `graf_19_the_chess_tournament_standings` | 510 | Recreation Corner Corkboard | `room_bunker_corridor` | Central Corridor Recreation Corner | Generic shelter corridor | ShelterPanel (Corridor) |
| `graf_20_the_sapper_elegy` | 580 | Excavation Drift 5 Header Beam | `room_workshop` | Workshop Drift 5 | Exact shelter room | ShelterPanel (Workshop) |
| `graf_21_the_rat_racing_league` | 640 | Sump Pump Drain Channel Wall | `room_filtration` | Sump Pump Drain Channel | Exact shelter room | ShelterPanel (Filtration) |
| `graf_22_the_candle_ration_strike` | 710 | Workshops Central Corridor | `room_workshop` | Workshops Central Corridor | Exact shelter room | ShelterPanel (Workshop) |
| `graf_23_the_radio_song_request_slate` | 790 | Radio Room Window Ledge | `room_radio_tuner` | Tuner Station Window Ledge | Exact shelter room | ShelterPanel (Radio Tuner) |
| `graf_24_the_first_flower_in_the_duct` | 880 | Air Exhaust Louver #2 | `room_filtration` | Air Exhaust Louver #2 | Exact shelter room | ShelterPanel (Filtration) |
| `graf_25_the_generator_noise_complaint` | 970 | Generator Hall Sound Baffle | `room_filtration` | Generator Hall Sound Baffle | Exact shelter room | ShelterPanel (Filtration) |
| `graf_26_the_clandestine_cross_graffiti` | 1080 | Abandoned Blast Tunnel Wall | `room_bunker_corridor` | Abandoned Blast Tunnel | Generic shelter corridor | ShelterPanel (Corridor) |
| `graf_27_the_tea_substitute_review` | 1200 | Canteen Menu Board | `room_kitchen` | Galley Kitchen Menu Board | Exact shelter room | ShelterPanel (Kitchen) |
| `graf_28_the_lost_wedding_band` | 1380 | Shower Sump Grate | `room_water_pump` | Shower Sump Grate | Exact shelter room | ShelterPanel (Water Pump) |
| `graf_29_the_sentry_weather_rhyme` | 1550 | Cupola Watch Bench | `room_airlock` | Cupola Watch Bench | Exact shelter room | ShelterPanel (Airlock) |
| `graf_30_the_foundry_strike_threat` | 1820 | Foundry Casting Floor Wall | `room_foundry` | Silent Foundry Casting Floor | Exact shelter room | ShelterPanel (Foundry) |
| `graf_31_the_secret_library_alcove` | 2100 | Archive Shelf 18 (Behind Geology) | `room_main` | Archive Shelf 18 / Library | Exact shelter room | ShelterPanel (Main Vault) |
| `graf_32_the_great_pancake_day` | 2450 | Kitchen Chimney Breast | `room_kitchen` | Galley Kitchen Chimney | Exact shelter room | ShelterPanel (Kitchen) |
| `graf_33_the_first_surface_wedding_announcement` | 2800 | Upper Airlock Bulletin Board | `room_airlock` | Upper Airlock Bulletin Board | Exact shelter room | ShelterPanel (Airlock) |
| `graf_34_the_last_coal_wagon_graffiti` | 3100 | Boiler Coal Chute Gate | `room_filtration` | Boiler Coal Chute Gate | Exact shelter room | ShelterPanel (Filtration) |
| `graf_35_the_charter_ratification_notice` | 3500 | Assembly Hall Slate Archway | `room_main` | Assembly Hall Slate Archway | Exact shelter room | ShelterPanel (Main Vault) |
| `graf_36_the_final_slate_greeting` | 3650 | Outer Blast Door (Open to the Sun) | `room_airlock` | Outer Blast Door (Open to the Sun) | Exact shelter room | ShelterPanel (Airlock) |
| `graf_dir_01_pump` | 5 | South corridor junction | `room_water_pump` | South Corridor Pump Junction | Exact shelter room | ShelterPanel (Water Pump) |
| `graf_dir_02_well` | 8 | Sub-level 1 stairwell | `room_water_pump` | Sub-Level 1 Well Stairwell | Exact shelter room | ShelterPanel (Water Pump) |
| `graf_dir_03_school` | 16 | Ward C corridor, by the lamp | `room_clinic` | Ward C Corridor | Exact shelter room | ShelterPanel (Clinic) |
| `graf_dir_04_cache` | 22 | Scavenger trail, third marker | `stranger_cache` | Scavenger Trail Cache Marker | Settlement/faction site | MapDetailPanel |
| `graf_dir_05_wrong_way` | 30 | North road, fallen sign | `rural_gas_station` | North Road Fallen Sign | Exact canonical world location | MapDetailPanel |
| `graf_dir_06_mess_hall` | 12 | Main corridor, eye-level | `room_kitchen` | Main Concourse to Mess Hall | Exact shelter room | ShelterPanel (Kitchen) |
| `graf_warn_07_rad` | 3 | Bunker perimeter fence | `government_bunker` | Bunker Perimeter Fence | Exact canonical world location | MapDetailPanel |
| `graf_warn_08_rad_added` | 20 | Same sign, below the paint | `government_bunker` | Bunker Perimeter Sign Sub-layer | Exact canonical world location | MapDetailPanel |
| `graf_warn_09_filter` | 30 | Sub-level 3 entrance | `room_filtration` | Sub-Level 3 Filter Entrance | Exact shelter room | ShelterPanel (Filtration) |
| `graf_warn_10_dont_drink` | 14 | Blue drum, stores | `room_storage_bay` | Blue Drum in Stores | Exact shelter room | ShelterPanel (Storage Bay) |
| `graf_warn_11_floor` | 25 | Collapsed building, hallway | `suburban_house` | Collapsed Hallway Floor | Exact canonical world location | MapDetailPanel |
| `graf_warn_12_live` | 50 | Substation omega, railing | `location_geo_thermal_plant_ruins` | Substation Omega Railing | Exact canonical world location | MapDetailPanel |
| `graf_warn_13_quiet` | 18 | Shelter, near the hatch | `room_airlock` | Shelter Near the Hatch | Exact shelter room | ShelterPanel (Airlock) |
| `graf_warn_14_gas` | 35 | Submerged data center, stairwell | `location_submerged_data_center` | Submerged Data Center Stairwell | Exact canonical world location | MapDetailPanel |
| `graf_acc_15_thief` | 22 | West locker, inside door | `room_storage_bay` | West Locker Door | Exact shelter room | ShelterPanel (Storage Bay) |
| `graf_acc_16_raid` | 40 | Scavenger camp, perimeter | `stranger_cache` | Scavenger Camp Perimeter | Settlement/faction site | MapDetailPanel |
| `graf_acc_17_lied` | 27 | Bunker wall, main corridor | `room_bunker_corridor` | Bunker Wall Main Corridor | Generic shelter corridor | ShelterPanel (Corridor) |
| `graf_acc_18_who` | 33 | Clinic, by the sheeted bay | `room_clinic` | Clinic Sheeted Bay | Exact shelter room | ShelterPanel (Clinic) |
| `graf_acc_19_left` | 50 | House, blue door, inside | `suburban_house` | Suburban House Blue Door | Exact canonical world location | MapDetailPanel |
| `graf_joke_20_biscuit` | 12 | Canteen entryway slate | `room_kitchen` | Canteen Entryway Slate | Exact shelter room | ShelterPanel (Kitchen) |
| `graf_joke_21_stoker` | 3 | Boiler room corridor, steam pipe | `room_filtration` | Boiler Room Steam Pipe | Exact shelter room | ShelterPanel (Filtration) |
| `graf_joke_22_sun` | 27 | Shelter yard, low wall | `room_airlock` | Shelter Yard Low Wall | Exact shelter room | ShelterPanel (Airlock) |
| `graf_joke_23_coffee` | 30 | Mess hall, by the hatch | `room_kitchen` | Mess Hall Hatch | Exact shelter room | ShelterPanel (Kitchen) |
| `graf_joke_24_tuesday` | 38 | Mess hall entrance, over the hatch | `room_kitchen` | Mess Hall Entrance | Exact shelter room | ShelterPanel (Kitchen) |
| `graf_joke_25_boots` | 45 | Generator room, on the logbook | `room_filtration` | Generator Room Logbook | Exact shelter room | ShelterPanel (Filtration) |
| `graf_grief_26_dima` | 23 | School, by his bed | `room_bunks` | School Ward Bunks Bed | Exact shelter room | ShelterPanel (Bunks) |
| `graf_grief_27_names` | 62 | Plaza wall, the tally | `room_main` | Plaza Tally Wall | Exact shelter room | ShelterPanel (Main Vault) |
| `graf_grief_28_stone` | 33 | South plot, to the left of her husband | `location_ash_dune_cemetery` | South Cemetery Plot | Exact canonical world location | MapDetailPanel |
| `graf_grief_29_victor` | 95 | Clinic, by his bed, after | `room_clinic` | Clinic Victor Bedside | Exact shelter room | ShelterPanel (Clinic) |
| `graf_grief_30_river` | 34 | Clinic, sheeted bay, after | `room_clinic` | Clinic River Woman Bay | Exact shelter room | ShelterPanel (Clinic) |
| `graf_grief_31_empty` | 22 | Shelter, a bed, by the candle | `room_bunks` | Bunks Candle Bed | Exact shelter room | ShelterPanel (Bunks) |
| `graf_mark_32_tally` | 9 | Stores, west locker wall | `room_storage_bay` | Stores West Locker Wall | Exact shelter room | ShelterPanel (Storage Bay) |
| `graf_mark_33_key` | 1 | Tin by the bed, under the handkerchiefs | `room_bunks` | Bunk Tin by Bed | Exact shelter room | ShelterPanel (Bunks) |
| `graf_mark_34_locker` | 20 | Public swimming baths, locker row | `location_municipal_sewage` | Public Swimming Baths Locker Row | Exact canonical world location | MapDetailPanel |
| `graf_mark_35_shift` | 7 | Sentry post, north cupola | `room_airlock` | North Cupola Sentry Post | Exact shelter room | ShelterPanel (Airlock) |
| `graf_mark_36_water` | 14 | Pump, south corridor | `room_water_pump` | South Corridor Pump | Exact shelter room | ShelterPanel (Water Pump) |
| `graf_mark_37_fuel` | 60 | Diesel tank farm, tank side | `room_filtration` | Diesel Tank Farm Tank Side | Exact shelter room | ShelterPanel (Filtration) |
| `graf_long_38_stop` | 100 | Crossroads, a cairn | `rural_gas_station` | Crossroads Cairn Stop Sign | Exact canonical world location | MapDetailPanel |
| `graf_long_39_sun_face` | 27 | Shelter yard, low wall | `room_airlock` | Shelter Yard Low Wall | Exact shelter room | ShelterPanel (Airlock) |
| `graf_long_40_wall` | 62 | Plaza, the tally wall | `room_main` | Plaza Tally Wall | Exact shelter room | ShelterPanel (Main Vault) |
