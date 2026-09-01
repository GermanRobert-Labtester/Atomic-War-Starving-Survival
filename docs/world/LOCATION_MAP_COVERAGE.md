# Location Map Coverage & Audit Report

**Authority:** `Assets/StreamingAssets/Data/locations.json`
**Map Graph:** `Assets/StreamingAssets/Data/wasteland_map_v1.json`

---

## 1. Coverage Metrics

- **Total Locations Defined in Authoritative Catalog:** 129
- **Total Locations Bound to Physical Map Nodes:** 60
- **Coverage Ratio:** 46.5%
- **Deferred Sub-Locations / Interior Scenes:** 69 (Managed via quests, event triggers, encounter references, and tactical combat deployments).

---

## 2. Categorical Distribution of Mapped Locations

| Category | Total Defined | Mapped to Map Nodes | Deferred / Sub-Location |
|---|---|---|---|
| **Shelter & Home Base** | 7 | 3 (`loc_holdfast`, `loc_shelter_gate`, `loc_water_station`) | 4 (Interior rooms: infirmary, meeting, storage, quarters) |
| **Faction Strongholds & Seats** | 12 | 10 (`loc_grange_hall`, `loc_grain_silo`, `loc_garrison_checkpoint_gamma`, `loc_black_flotilla_outpost`, `loc_weighbridge`, `loc_railway_span_44_alpha`, `loc_forward_roster_camp`, `loc_motel_verity`, `loc_shrine_switchback_waystation`, `loc_the_vessels_cell`) | 2 (Interior command sanctums) |
| **Deep Strata Excavations** | 5 | 5 (100% Mapped: `excavation_command_vault`, `excavation_utility_tunnels`, `excavation_metro_interchange`, `excavation_mine_shaft`, `excavation_archive_bunker`) | 0 |
| **Cipher & Signal Depots** | 3 | 3 (100% Mapped: `loc_hidden_relay_bunker`, `loc_logistics_reserve_cache`, `loc_deaddrop_command_shelter`) | 0 |
| **Infrastructure & Industry** | 22 | 14 (`loc_cut_abandoned_depot`, `loc_cut_arsenal_ruin`, `loc_diesel_tank_farm`, `loc_transit_authority_hq`, `loc_recovery_yard`, `location_concrete_batching_plant`, `location_substation_omega`, `loc_terrace_pumphouse`, `loc_lock_gate_four`, `loc_pump_station_nine`, etc.) | 8 |
| **Civilian & Suburbs** | 24 | 10 (`suburban_house`, `loc_school_gymnasium`, `loc_conscription_office`, `loc_the_allotments`, `loc_dentists_row`, `loc_cut_merchant_caravanserai`, `loc_apiary_rows`, `loc_seed_library_annex`, `loc_cider_press`, `loc_ration_queue_plaza`) | 14 |
| **Deep Coast & Maritime** | 16 | 8 (`loc_cold_store_atlantic`, `loc_bathymetric_boat`, `loc_the_shallows_market`, `loc_drowned_cinema`, `location_submerged_arcology`, `location_ash_whale_carcass`, etc.) | 8 |
| **Scarp & Mountain Facilities** | 18 | 7 (`loc_snowline_station`, `loc_pilgrim_switchbacks`, `loc_avalanche_gallery`, `loc_summit_relay`, `loc_low_background_lab`, `loc_ice_core_store`, etc.) | 11 |
| **Hazardous Ground Zero & Fallout** | 22 | 6 (`loc_cut_radiation_zone_alpha`, `location_the_dead_hand_core`, `location_magnetic_anomaly_crater`, `location_drone_hive_silo`, `location_automated_mortar_pit`, `location_deep_core_borehole`) | 16 |

---

## 3. Preservation of Historical and Quest References

All major narrative hubs, quest milestones (Duty Roster, Moral Choice, Faction War, Deep Coast, Year of Ash, Warlords), and trade crossroads have dedicated map nodes. Sub-rooms and minor caches resolve hierarchically through the parent node's exploration or encounter runtime without cluttering the regional canvas.
