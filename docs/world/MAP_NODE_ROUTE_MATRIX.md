# Map Node & Route Matrix

**Catalog File:** `Assets/StreamingAssets/Data/wasteland_map_v1.json`
**Node Count:** 60 nodes
**Route Count:** 202 directed routes (101 bidirectional connections)
**Connected Components:** 1 (Shelter-reachable: 60/60)

---

## 1. Node Catalog Summary

| Region | Node ID | Display Name | Danger | Faction | Loot Table | (X, Y) | Unlocked |
|---|---|---|---|---|---|---|---|
| R1 | `loc_holdfast` | The Holdfast | none | player | - | (500, 300) | YES |
| R1 | `loc_shelter_gate` | Shelter Gate | none | player | - | (480, 270) | YES |
| R1 | `loc_water_station` | Water Station | low | - | salvage_common | (530, 340) | YES |
| R1 | `loc_cut_radiation_zone_alpha` | Fallout Zone Alpha | high | - | salvage_rare | (420, 380) | NO |
| R1 | `loc_excavation_command_vault` | Collapsed Command Vault | medium | - | salvage_rare | (580, 400) | NO |
| R1 | `location_the_dead_hand_core` | The Dead Hand Core | high | - | salvage_military | (460, 470) | NO |
| R1 | `location_magnetic_anomaly_crater` | Magnetic Crater | high | - | salvage_rare | (390, 440) | NO |
| R1 | `location_drone_hive_silo` | Drone Hive Silo | high | - | salvage_military | (540, 490) | NO |
| R1 | `location_automated_mortar_pit` | Automated Mortar Pit | high | - | salvage_weapons | (610, 460) | NO |
| R1 | `location_deep_core_borehole` | Deep Core Borehole | high | - | salvage_rare | (490, 530) | NO |
| R2 | `loc_cut_merchant_caravanserai` | Merchant Caravanserai | low | faction_the_scale | trade_goods | (380, 220) | YES |
| R2 | `loc_grange_hall` | The Grange Hall | low | faction_rebuilders | salvage_common | (320, 180) | NO |
| R2 | `loc_school_gymnasium` | School Gymnasium | low | - | salvage_common | (260, 140) | NO |
| R2 | `loc_conscription_office` | District Conscription Office | medium | faction_central_garrison | salvage_military | (210, 200) | NO |
| R2 | `loc_the_allotments` | The Allotments | low | faction_rebuilders | salvage_common | (280, 240) | NO |
| R2 | `loc_dentists_row` | Dentists' Row | medium | - | salvage_medical | (340, 270) | NO |
| R2 | `loc_motel_verity` | The Verity Motel | low | faction_the_scale | trade_goods | (230, 290) | NO |
| R2 | `loc_logistics_reserve_cache` | Sub-Basement Logistics Reserve | low | - | trade_goods | (420, 170) | NO |
| R2 | `loc_excavation_utility_tunnels` | Utility Tunnel Network | low | - | salvage_common | (460, 210) | NO |
| R2 | `suburban_house` | Suburban House | low | - | salvage_common | (180, 150) | NO |
| R3 | `loc_cut_abandoned_depot` | Abandoned Rail Depot | low | - | salvage_common | (630, 220) | NO |
| R3 | `loc_cut_arsenal_ruin` | Arsenal Ruin | medium | - | salvage_weapons | (710, 260) | NO |
| R3 | `loc_excavation_metro_interchange` | Buried Metro Interchange | medium | - | salvage_rare | (580, 170) | NO |
| R3 | `loc_weighbridge` | The Weighbridge | medium | faction_the_scale | trade_goods | (680, 180) | NO |
| R3 | `loc_diesel_tank_farm` | Tank Farm 4-East | medium | faction_silent_foundry | salvage_industrial | (760, 190) | NO |
| R3 | `loc_railway_span_44_alpha` | Railway Span 44-Alpha | medium | faction_the_cutters | salvage_industrial | (720, 120) | NO |
| R3 | `loc_transit_authority_hq` | Transit Authority HQ | medium | - | salvage_rare | (640, 130) | NO |
| R3 | `loc_recovery_yard` | Recovery Yard | medium | faction_silent_foundry | salvage_industrial | (790, 140) | NO |
| R3 | `location_concrete_batching_plant` | Concrete Batching Plant | medium | - | salvage_industrial | (830, 210) | NO |
| R3 | `location_substation_omega` | Substation Omega | medium | - | salvage_industrial | (780, 270) | NO |
| R4 | `loc_black_flotilla_outpost` | Black Flotilla Outpost | locked | faction_the_fleet | salvage_maritime | (550, 620) | NO |
| R4 | `loc_deaddrop_command_shelter` | Dead-Drop Command Shelter | medium | - | salvage_rare | (620, 660) | NO |
| R4 | `loc_cold_store_atlantic` | Atlantic Cold Store | high | faction_the_fleet | salvage_maritime | (490, 680) | NO |
| R4 | `loc_bathymetric_boat` | Survey Launch Kittiwake | high | - | salvage_maritime | (560, 740) | NO |
| R4 | `loc_the_shallows_market` | The Shallows Market | medium | faction_the_fleet | trade_goods | (640, 720) | NO |
| R4 | `loc_drowned_cinema` | The Odeon Cinema | medium | - | salvage_rare | (480, 750) | NO |
| R4 | `loc_lock_gate_four` | Lock Gate Four | medium | faction_the_cutters | salvage_industrial | (700, 680) | NO |
| R4 | `loc_pump_station_nine` | Pump Station Nine | medium | - | salvage_industrial | (750, 740) | NO |
| R4 | `location_submerged_arcology` | Submerged Luxury Arcology | high | - | salvage_rare | (520, 810) | NO |
| R4 | `location_ash_whale_carcass` | Ash-Whale Carcass | medium | - | salvage_organic | (600, 820) | NO |
| R5 | `loc_grain_silo` | The Grain Exchange | low | faction_rebuilders | trade_goods | (880, 320) | NO |
| R5 | `loc_garrison_checkpoint_gamma` | Checkpoint Gamma | medium | faction_central_garrison | salvage_military | (950, 280) | NO |
| R5 | `loc_forward_roster_camp` | The Forward Roster Camp | medium | faction_forward_roster | salvage_military | (920, 390) | NO |
| R5 | `loc_apiary_rows` | The Apiary Rows | low | faction_rebuilders | salvage_organic | (860, 440) | NO |
| R5 | `loc_seed_library_annex` | Seed Library Annex | low | faction_rebuilders | salvage_rare | (990, 350) | NO |
| R5 | `loc_cider_press` | The Cider Press | low | faction_rebuilders | salvage_organic | (930, 470) | NO |
| R5 | `loc_terrace_pumphouse` | Terrace Pumphouse | medium | - | salvage_industrial | (880, 520) | NO |
| R5 | `loc_ration_queue_plaza` | Ration Plaza | medium | faction_central_garrison | trade_goods | (1020, 420) | NO |
| R5 | `loc_eastern_road` | Eastern Arterial Road | medium | faction_central_garrison | salvage_common | (1050, 300) | NO |
| R5 | `loc_neutral_ground` | Neutral Ground | low | faction_the_scale | trade_goods | (960, 500) | NO |
| R6 | `loc_shrine_switchback_waystation` | The Switchback Waystation | low | faction_ash_sign | trade_goods | (320, 600) | NO |
| R6 | `loc_snowline_station` | Snowline Patrol Station | medium | faction_forward_roster | salvage_military | (260, 640) | NO |
| R6 | `loc_pilgrim_switchbacks` | The Switchbacks | medium | faction_ash_sign | salvage_common | (210, 690) | NO |
| R6 | `loc_avalanche_gallery` | Avalanche Gallery | high | - | salvage_industrial | (160, 730) | NO |
| R6 | `loc_summit_relay` | Summit Relay Spire | high | - | salvage_rare | (230, 770) | NO |
| R6 | `loc_low_background_lab` | Low-Background Laboratory | medium | - | salvage_rare | (360, 680) | NO |
| R6 | `loc_ice_core_store` | Ice Core Store | medium | - | salvage_rare | (300, 740) | NO |
| R6 | `loc_the_vessels_cell` | The Vessel's Cell | high | faction_ash_sign | salvage_rare | (180, 810) | NO |
| R6 | `loc_excavation_mine_shaft` | Industrial Mine Shaft Adit 4 | high | - | salvage_rare | (390, 750) | NO |
| R6 | `loc_excavation_archive_bunker` | Pre-War Archive Bunker | medium | - | salvage_rare | (310, 830) | NO |

---

## 2. Key Arteries & Trade Corridors

1. **The Great Suburban Bypass:** `loc_holdfast` ↔ `loc_cut_merchant_caravanserai` ↔ `loc_grange_hall` ↔ `loc_the_allotments` ↔ `loc_motel_verity`
2. **The Industrial Rail Artery:** `loc_holdfast` ↔ `loc_cut_abandoned_depot` ↔ `loc_weighbridge` ↔ `loc_railway_span_44_alpha` ↔ `loc_recovery_yard`
3. **The Maritime Coast Line:** `loc_cut_radiation_zone_alpha` ↔ `loc_black_flotilla_outpost` ↔ `loc_the_shallows_market` ↔ `loc_lock_gate_four`
4. **The Verge Agricultural Road:** `loc_cut_arsenal_ruin` ↔ `loc_grain_silo` ↔ `loc_forward_roster_camp` ↔ `loc_apiary_rows` ↔ `loc_neutral_ground`
5. **The High Scarp Pilgrim Path:** `loc_water_station` ↔ `loc_shrine_switchback_waystation` ↔ `loc_snowline_station` ↔ `loc_pilgrim_switchbacks` ↔ `loc_summit_relay`
