# Plan 135 — Narrative Discovery Producer Graph

Every activated record is connected to a reachable producer in the ASHFALL campaign graph. Discovery state is strictly one-time and managed by `JournalSystem.KnowledgeBase`.

| Discovery ID | Channel | Producer ID | Producer Type | Reachable Point | Idempotent Key | Reader Surface |
|---|---|---|---|---|---|---|
| `disc_glitch_radiator_header` | `location_inspection` | `room_bunker_corridor` | shelter_subsystem | Day 1+ | `narrative_disc_disc_glitch_radiator_header` | `JournalBookUI` (Codex/Log) |
| `disc_glitch_artesian_intake` | `location_inspection` | `room_water_pump` | shelter_subsystem | Day 1+ | `narrative_disc_disc_glitch_artesian_intake` | `JournalBookUI` (Codex/Log) |
| `disc_glitch_substation_neutral` | `location_inspection` | `room_workshop` | shelter_subsystem | Day 1+ | `narrative_disc_disc_glitch_substation_neutral` | `JournalBookUI` (Codex/Log) |
| `disc_glitch_air_filter_mold` | `location_inspection` | `room_filtration` | shelter_subsystem | Day 1+ | `narrative_disc_disc_glitch_air_filter_mold` | `JournalBookUI` (Codex/Log) |
| `disc_boiler_deaerator_oxygen_pitting` | `location_inspection` | `location_geo_thermal_plant_ruins` | wasteland_location | Day 2+ | `narrative_disc_disc_boiler_deaerator_oxygen_pitting` | `JournalBookUI` (Codex/Log) |
| `disc_boiler_deaerator_sulfite_depletion` | `location_inspection` | `location_geo_thermal_plant_ruins` | wasteland_location | Day 2+ | `narrative_disc_disc_boiler_deaerator_sulfite_depletion` | `JournalBookUI` (Codex/Log) |
| `disc_boiler_deaerator_mud_drum_sludge` | `location_inspection` | `location_geo_thermal_plant_ruins` | wasteland_location | Day 2+ | `narrative_disc_disc_boiler_deaerator_mud_drum_sludge` | `JournalBookUI` (Codex/Log) |
| `disc_blueprint_surface_airlock` | `shelter_room_archive` | `room_airlock` | shelter_room | Day 1+ | `narrative_disc_disc_blueprint_surface_airlock` | `JournalBookUI` (Codex/Log) |
| `disc_blueprint_ventilation_blower` | `shelter_room_archive` | `room_filtration` | shelter_room | Day 1+ | `narrative_disc_disc_blueprint_ventilation_blower` | `JournalBookUI` (Codex/Log) |
| `disc_blueprint_artesian_well_pump` | `shelter_room_archive` | `room_water_pump` | shelter_room | Day 1+ | `narrative_disc_disc_blueprint_artesian_well_pump` | `JournalBookUI` (Codex/Log) |
| `disc_blueprint_hydroponic_greenhouse` | `shelter_room_archive` | `room_greenhouse` | shelter_room | Day 1+ | `narrative_disc_disc_blueprint_hydroponic_greenhouse` | `JournalBookUI` (Codex/Log) |
| `disc_court_moonshine_still` | `library_terminal` | `government_bunker` | archive_terminal | Day 3+ | `narrative_disc_disc_court_moonshine_still` | `JournalBookUI` (Codex/Log) |
| `disc_court_chore_chit_forgery` | `library_terminal` | `government_bunker` | archive_terminal | Day 3+ | `narrative_disc_disc_court_chore_chit_forgery` | `JournalBookUI` (Codex/Log) |
| `disc_court_accordion_recital` | `library_terminal` | `government_bunker` | archive_terminal | Day 3+ | `narrative_disc_disc_court_accordion_recital` | `JournalBookUI` (Codex/Log) |
| `disc_court_smuggled_calico_cat` | `library_terminal` | `government_bunker` | archive_terminal | Day 3+ | `narrative_disc_disc_court_smuggled_calico_cat` | `JournalBookUI` (Codex/Log) |
| `disc_wire_vel_iodine` | `radio_archive` | `room_radio_tuner` | radio_station | Day 14+ | `narrative_disc_disc_wire_vel_iodine` | `JournalBookUI` (Codex/Log) |
| `disc_wire_morozov_lock4` | `radio_archive` | `room_radio_tuner` | radio_station | Day 40+ | `narrative_disc_disc_wire_morozov_lock4` | `JournalBookUI` (Codex/Log) |
| `disc_wire_sonya_buttons` | `radio_archive` | `room_radio_tuner` | radio_station | Day 10+ | `narrative_disc_disc_wire_sonya_buttons` | `JournalBookUI` (Codex/Log) |
| `disc_trade_salt_gauze` | `scavenging_document` | `room_storage_bay` | shelter_depot | Day 1+ | `narrative_disc_disc_trade_salt_gauze` | `JournalBookUI` (Codex/Log) |
| `disc_trade_kerosene_barter` | `scavenging_document` | `location_sub_level_4_transit` | wasteland_location | Day 2+ | `narrative_disc_disc_trade_kerosene_barter` | `JournalBookUI` (Codex/Log) |
| `disc_trade_weld_grain_audit` | `scavenging_document` | `room_storage_bay` | shelter_depot | Day 1+ | `narrative_disc_disc_trade_weld_grain_audit` | `JournalBookUI` (Codex/Log) |
| `disc_treaty_lock4_sluice` | `library_terminal` | `government_bunker` | archive_terminal | Day 5+ | `narrative_disc_disc_treaty_lock4_sluice` | `JournalBookUI` (Codex/Log) |
| `disc_treaty_north_ridge_timber` | `library_terminal` | `government_bunker` | archive_terminal | Day 5+ | `narrative_disc_disc_treaty_north_ridge_timber` | `JournalBookUI` (Codex/Log) |
| `disc_treaty_upper_gorge_hydro` | `library_terminal` | `government_bunker` | archive_terminal | Day 5+ | `narrative_disc_disc_treaty_upper_gorge_hydro` | `JournalBookUI` (Codex/Log) |
| `disc_scav_route_bridge_toll` | `scavenging_document` | `stranger_cache` | scavenge_node | Day 2+ | `narrative_disc_disc_scav_route_bridge_toll` | `JournalBookUI` (Codex/Log) |
| `disc_scav_route_frozen_swamp` | `scavenging_document` | `stranger_cache` | scavenge_node | Day 2+ | `narrative_disc_disc_scav_route_frozen_swamp` | `JournalBookUI` (Codex/Log) |
| `disc_scav_route_collapsed_tunnel` | `scavenging_document` | `stranger_cache` | scavenge_node | Day 2+ | `narrative_disc_disc_scav_route_collapsed_tunnel` | `JournalBookUI` (Codex/Log) |
| `disc_topo_ground_zero_basin` | `location_inspection` | `location_silent_observatory` | wasteland_location | Day 3+ | `narrative_disc_disc_topo_ground_zero_basin` | `JournalBookUI` (Codex/Log) |
| `disc_topo_ridge_crest_divide` | `location_inspection` | `location_silent_observatory` | wasteland_location | Day 3+ | `narrative_disc_disc_topo_ridge_crest_divide` | `JournalBookUI` (Codex/Log) |
| `disc_topo_river_delta_confluence` | `location_inspection` | `location_silent_observatory` | wasteland_location | Day 3+ | `narrative_disc_disc_topo_river_delta_confluence` | `JournalBookUI` (Codex/Log) |
| `disc_case_radiation_syndrome` | `quest_aftermath` | `abandoned_hospital` | quest_encounter | Day 3+ | `narrative_disc_disc_case_radiation_syndrome` | `JournalBookUI` (Codex/Log) |
| `disc_case_radiation_pneumonitis` | `quest_aftermath` | `abandoned_hospital` | quest_encounter | Day 3+ | `narrative_disc_disc_case_radiation_pneumonitis` | `JournalBookUI` (Codex/Log) |
| `disc_case_compound_fracture` | `quest_aftermath` | `room_clinic` | shelter_subsystem | Day 1+ | `narrative_disc_disc_case_compound_fracture` | `JournalBookUI` (Codex/Log) |
| `disc_case_dysentery_outbreak` | `quest_aftermath` | `room_clinic` | shelter_subsystem | Day 2+ | `narrative_disc_disc_case_dysentery_outbreak` | `JournalBookUI` (Codex/Log) |
| `disc_germination_red_fife_wheat` | `item_examination` | `item_seed_ash_grain` | item_instance | Day 1+ | `narrative_disc_disc_germination_red_fife_wheat` | `JournalBookUI` (Codex/Log) |
| `disc_germination_golden_bantam_corn` | `item_examination` | `family_heirloom_seeds` | item_instance | Day 1+ | `narrative_disc_disc_germination_golden_bantam_corn` | `JournalBookUI` (Codex/Log) |
| `disc_germination_crimson_clover` | `item_examination` | `item_seed_leafy_green` | item_instance | Day 1+ | `narrative_disc_disc_germination_crimson_clover` | `JournalBookUI` (Codex/Log) |
| `disc_well_tritium_spike` | `location_inspection` | `location_abandoned_desalination` | wasteland_location | Day 3+ | `narrative_disc_disc_well_tritium_spike` | `JournalBookUI` (Codex/Log) |
| `disc_well_strontium_leach` | `location_inspection` | `location_abandoned_desalination` | wasteland_location | Day 3+ | `narrative_disc_disc_well_strontium_leach` | `JournalBookUI` (Codex/Log) |
| `disc_well_iron_slime` | `location_inspection` | `room_water_pump` | shelter_subsystem | Day 1+ | `narrative_disc_disc_well_iron_slime` | `JournalBookUI` (Codex/Log) |
| `disc_slow_sand_biofilm_maturation` | `location_inspection` | `location_municipal_sewage` | wasteland_location | Day 2+ | `narrative_disc_disc_slow_sand_biofilm_maturation` | `JournalBookUI` (Codex/Log) |
| `disc_slow_sand_skin_scraping` | `location_inspection` | `location_municipal_sewage` | wasteland_location | Day 2+ | `narrative_disc_disc_slow_sand_skin_scraping` | `JournalBookUI` (Codex/Log) |
| `disc_slow_sand_uniformity_coeff` | `location_inspection` | `room_filtration` | shelter_subsystem | Day 1+ | `narrative_disc_disc_slow_sand_uniformity_coeff` | `JournalBookUI` (Codex/Log) |
| `disc_folklore_clicking_beetle` | `scavenging_document` | `room_bunks` | shelter_living | Day 1+ | `narrative_disc_disc_folklore_clicking_beetle` | `JournalBookUI` (Codex/Log) |
| `disc_folklore_grey_man_vents` | `scavenging_document` | `room_bunks` | shelter_living | Day 1+ | `narrative_disc_disc_folklore_grey_man_vents` | `JournalBookUI` (Codex/Log) |
| `disc_folklore_yellow_lamp_sun` | `scavenging_document` | `room_kitchen` | shelter_living | Day 1+ | `narrative_disc_disc_folklore_yellow_lamp_sun` | `JournalBookUI` (Codex/Log) |
| `disc_folklore_iron_door_whisper` | `scavenging_document` | `room_bunks` | shelter_living | Day 1+ | `narrative_disc_disc_folklore_iron_door_whisper` | `JournalBookUI` (Codex/Log) |
| `disc_directive_seismic_trigger` | `library_terminal` | `government_bunker` | archive_terminal | Day 4+ | `narrative_disc_disc_directive_seismic_trigger` | `JournalBookUI` (Codex/Log) |
| `disc_directive_kinetic_harrow` | `library_terminal` | `government_bunker` | archive_terminal | Day 4+ | `narrative_disc_disc_directive_kinetic_harrow` | `JournalBookUI` (Codex/Log) |
| `disc_directive_deep_strata_lockdown` | `library_terminal` | `government_bunker` | archive_terminal | Day 4+ | `narrative_disc_disc_directive_deep_strata_lockdown` | `JournalBookUI` (Codex/Log) |
| `disc_dispatch_salt_runner` | `scavenging_document` | `location_sub_level_4_transit` | scavenge_node | Day 2+ | `narrative_disc_disc_dispatch_salt_runner` | `JournalBookUI` (Codex/Log) |
| `disc_dispatch_deserters_farewell` | `scavenging_document` | `location_sub_level_4_transit` | scavenge_node | Day 2+ | `narrative_disc_disc_dispatch_deserters_farewell` | `JournalBookUI` (Codex/Log) |
| `disc_dispatch_smugglers_parley` | `scavenging_document` | `location_sub_level_4_transit` | scavenge_node | Day 2+ | `narrative_disc_disc_dispatch_smugglers_parley` | `JournalBookUI` (Codex/Log) |
| `disc_clepsydra_jeweled_orifice` | `location_inspection` | `location_the_sump_cathedral` | wasteland_location | Day 2+ | `narrative_disc_disc_clepsydra_jeweled_orifice` | `JournalBookUI` (Codex/Log) |
| `disc_clepsydra_overflow_weir` | `location_inspection` | `location_the_sump_cathedral` | wasteland_location | Day 2+ | `narrative_disc_disc_clepsydra_overflow_weir` | `JournalBookUI` (Codex/Log) |
| `disc_clepsydra_vessel_sediment` | `location_inspection` | `location_the_sump_cathedral` | wasteland_location | Day 2+ | `narrative_disc_disc_clepsydra_vessel_sediment` | `JournalBookUI` (Codex/Log) |
| `disc_glass_melt_potash_fining` | `location_inspection` | `room_foundry` | shelter_subsystem | Day 1+ | `narrative_disc_disc_glass_melt_potash_fining` | `JournalBookUI` (Codex/Log) |
| `disc_glass_melt_iron_oxide_green` | `location_inspection` | `room_foundry` | shelter_subsystem | Day 1+ | `narrative_disc_disc_glass_melt_iron_oxide_green` | `JournalBookUI` (Codex/Log) |
| `disc_glass_melt_cobalt_blue_smalt` | `location_inspection` | `room_foundry` | shelter_subsystem | Day 1+ | `narrative_disc_disc_glass_melt_cobalt_blue_smalt` | `JournalBookUI` (Codex/Log) |
| `disc_glass_melt_arsenic_decolorizing` | `location_inspection` | `room_foundry` | shelter_subsystem | Day 1+ | `narrative_disc_disc_glass_melt_arsenic_decolorizing` | `JournalBookUI` (Codex/Log) |
