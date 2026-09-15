# ASHFALL Placeholder 512 Inventory

All files in `assets/art/placeholders-512/` are **PLACEHOLDERS — NOT FINAL ART**.
Every PNG is 512x512, Pillow-generated, deterministic, and stamped `PLACEHOLDER`
in pixels and in `PLACEHOLDER_512_MANIFEST.json`. Replace with final art; never
ship placeholders as finished visuals.

Generator: `scripts/tools/generate-placeholder-512-pack.py` (seed base 20260913).
Generated files this run: 1079.

## Coverage roots (mirrors runtime resolvers)

- `assets/art/{{id}}.jpg` / `{{id}}.png`
- `assets/sprites/Items|Portraits|Locations|Factions/{{id}}.png`
- `ItemIdAliases` semantic fallbacks + category `prefix-add` normalization
  (`item_` / `survivor_`+`npc_` / `loc_` / `faction_`)
- `FactionIconCatalog` explicit `assets/ui/Icons/faction_icon_*.png` emblems

## Counts

| Category | Missing ids | Placeholder files |
|---|---|---|
| item | 592 | 592 |
| portrait | 157 | 157 |
| location | 312 | 312 |
| faction | 18 | 18 |

## Missing catalog ids

### item (592)

`acetate_blank_disc`, `alcohol`, `aluminum_shavings`, `ammo_12g`, `ammo_12g_buck`, `ammo_22lr`, `ammo_308_incendiary`, `ammo_357`
`ammo_357_jhp`, `ammo_556_subsonic`, `ammo_76mm_beacon_smokey`, `ammo_76mm_he_flak`, `ammo_76mm_proximity_fuse`, `ammo_76mm_shaped_charge`, `ammo_76mm_tungsten_penetrator`, `ammo_chaff_burst`
`ammo_improvised_burn`, `ammo_improvised_rod`, `ammunition_brass`, `bionic_arm_prototype`, `bionic_leg_prototype`, `blood_sample`, `bus_ticket`, `camo_ash_cloak`
`camo_ghillie_shroud`, `camo_night_stalker_suit`, `carbon_black_powder`, `cardboard_wad`, `cassette_dam_keeper_log_1`, `cassette_dam_keeper_log_2`, `cassette_dam_keeper_log_3`, `cassette_dam_keeper_log_4`
`cassette_dam_keeper_log_5`, `cassette_evacuation_train_1`, `cassette_evacuation_train_2`, `cassette_evacuation_train_3`, `cassette_evacuation_train_4`, `cassette_fathers_tapes_1`, `cassette_fathers_tapes_2`, `cassette_fathers_tapes_3`
`cassette_fathers_tapes_4`, `cassette_field_hospital_7_1`, `cassette_field_hospital_7_2`, `cassette_field_hospital_7_3`, `cassette_field_hospital_7_4`, `cassette_field_hospital_7_5`, `cassette_greenhouse_tapes_1`, `cassette_greenhouse_tapes_2`
`cassette_greenhouse_tapes_3`, `cassette_quarantine_tapes_1`, `cassette_quarantine_tapes_2`, `cassette_quarantine_tapes_3`, `cassette_quarantine_tapes_4`, `cassette_station_14_1`, `cassette_station_14_2`, `cassette_station_14_3`
`cassette_station_14_4`, `cassette_station_14_5`, `cassette_station_14_6`, `cassette_teachers_recordings_1`, `cassette_teachers_recordings_2`, `cassette_teachers_recordings_3`, `cheap_comb`, `chemical_solvent`
`childs_mitten`, `childs_red_scarf`, `civil_defense_radio`, `concrete_rubble`, `creased_receipt`, `crop_ash_grain`, `crop_biolum_mushroom`, `crop_cold_legume`
`crop_hardy_tuber`, `crop_leafy_green`, `crop_medicinal_herb`, `crop_nutrient_algae`, `crop_oilseed`, `dog_tags_military`, `dog_tags_personal`, `empty_brass_shell`
`empty_tin_can`, `empty_toner_cartridge`, `enamel_mug`, `engineers_slide_rule`, `engraved_lighter`, `family_apartment_key`, `family_heirloom_seeds`, `farm_ledger`
`field_dressing_kit`, `film_reel`, `filter_pack`, `forceps`, `foreman_whistle`, `forensic_clue_bloodstained`, `fungus_spores_bioluminescent`, `fungus_spores_common`
`fungus_spores_medicinal`, `gene_therapy_retroviral_vial`, `harvested_mushrooms_subterranean`, `iron_pipe`, `iron_shackles`, `item_abrasive_grinding_stone`, `item_acoustic_guitar`, `item_aeroponic_food_leaf`
`item_aeroponic_medicinal_root`, `item_air_filter_hepa_hospital`, `item_aircraft_airframe_spares`, `item_ammo_762`, `item_ammonium_nitrate_sack`, `item_amnesty_petition_dossier`, `item_antibiotics`, `item_aquaponic_fish`
`item_aquifer_isolation_module`, `item_arbitration_token`, `item_archive_index_cylinder`, `item_armored_beadlock_set`, `item_armored_blast_shield`, `item_artillery_fuze_wrench`, `item_assay_reagent_pack`, `item_aviation_fuel_canister`
`item_ballistics_cleaning_kit`, `item_battery_electrolyte_concentrate`, `item_battery_maintenance_fluid`, `item_battery_reconditioned`, `item_bedrock_sensor_rig`, `item_beeswax_block`, `item_biofilter_media`, `item_biofuel_generator_grade`
`item_biofuel_high_grade`, `item_biofuel_low_grade`, `item_black_market_pouch`, `item_blowtorch`, `item_boiled_roots`, `item_brass_stamping_die`, `item_brine_protein_tin`, `item_brined_legume_mash`
`item_calibrated_dosimeter`, `item_canned_food`, `item_canned_grain_stew`, `item_carved_figurine`, `item_cast_borosilicate_glass_blank`, `item_caustic_soda_flakes`, `item_cbrn_cartridge`, `item_cerium_oxide_polishing_rouge`
`item_chain_gang_shackles`, `item_charter_draft`, `item_charter_stamp`, `item_chelation_decorporation_course`, `item_chem_clarity_salts`, `item_chem_dulcimer_tincture`, `item_chem_haze_resin`, `item_chem_hyper_stim`
`item_claim_tag_stamped`, `item_clean_water`, `item_cloud_seeding_canister`, `item_coated_combustor_tile`, `item_coated_diesel_injector`, `item_coated_turbine_blade`, `item_cohort_baseline_card`, `item_cold_count_provenance_seal`
`item_collectible_air_filter_manual`, `item_collectible_casualty_list`, `item_collectible_childs_doll`, `item_collectible_civic_token`, `item_collectible_civil_defense_badge`, `item_collectible_civil_defense_poster`, `item_collectible_concert_poster`, `item_collectible_deployment_order`
`item_collectible_diesel_service_manual`, `item_collectible_dosimeter_guide`, `item_collectible_exchange_day_newspaper`, `item_collectible_family_portrait`, `item_collectible_field_medicine_handbook`, `item_collectible_folk_craft`, `item_collectible_hunting_magazine`, `item_collectible_local_newspaper`
`item_collectible_match_program`, `item_collectible_military_patch`, `item_collectible_mothers_letter`, `item_collectible_music_box`, `item_collectible_prayer_beads`, `item_collectible_prayer_book`, `item_collectible_pre_war_novel`, `item_collectible_propaganda_poster`
`item_collectible_radio_repair_guide`, `item_collectible_rejection_letter`, `item_collectible_road_map`, `item_collectible_science_magazine`, `item_collectible_soldiers_letter`, `item_collectible_survivor_map`, `item_collectible_team_pennant`, `item_collectible_topo_map`
`item_collectible_trade_guild_patch`, `item_collectible_transit_badge`, `item_collectible_unit_log_fragment`, `item_collectible_unit_photograph`, `item_collectible_vinyl_chamber_record`, `item_collectible_vinyl_civil_broadcast`, `item_collectible_vinyl_folk_compilation`, `item_collectible_water_treatment_handbook`
`item_comm_codebook_alpha`, `item_corrosion_inhibitor_drum`, `item_crop_waste`, `item_crossing_bread`, `item_crossing_map`, `item_cutting_fluid_canister`, `item_cyanide_antidote_kit`, `item_decor_carved_memorial`
`item_decor_chalk_drawing`, `item_decor_classroom_chart`, `item_decor_locomotive_nameplate`, `item_decor_medal_civic`, `item_decor_memorial_plaque_carving`, `item_decor_memorial_plaque_drawing`, `item_decor_memorial_plaque_generic`, `item_decor_poster_ration`
`item_decor_poster_warning`, `item_decor_pressed_flower`, `item_decor_signal_log`, `item_decryption_keycard_prewar`, `item_deep_service_ribbon`, `item_descent_line`, `item_deserter_coalition_forged_papers`, `item_detector_sensor_module`
`item_diesel_fuel`, `item_diving_suit_vulcanized`, `item_document_barricade_placement`, `item_document_blood_trail_note`, `item_document_broadcast_transcript`, `item_document_casualty_list`, `item_document_child_drawing`, `item_document_civil_defense_poster`
`item_document_confession`, `item_document_death_certificate`, `item_document_debt_default_notice`, `item_document_evacuation_list`, `item_document_evacuation_route_map`, `item_document_family_photograph`, `item_document_field_report`, `item_document_handwritten_warning`
`item_document_journal_fragment`, `item_document_last_letter`, `item_document_maintenance_record`, `item_document_military_map`, `item_document_patrol_order`, `item_document_quarantine_notice`, `item_document_radio_log`, `item_document_ration_record`
`item_document_ration_theft_ledger`, `item_document_repair_note`, `item_document_sealed_door_warning`, `item_document_shelter_rejection_list`, `item_document_supply_inventory`, `item_document_supply_requisition`, `item_document_triage_record`, `item_document_vandalized_propaganda`
`item_document_water_notice`, `item_document_weather_gate_warning`, `item_document_will`, `item_dog_tags_scavenged`, `item_dose_register_book`, `item_dosimeter`, `item_dosimeter_calibrated`, `item_dried_herb_packets`
`item_dried_mushrooms`, `item_dried_rations`, `item_dual_axis_tracking_gimbal`, `item_ebpvd_ceramic_target_ingot`, `item_ebpvd_vacuum_pump_seal`, `item_ecm_jammer_module`, `item_electron_gun_tungsten_filament`, `item_engine`
`item_escort_challenge_ribbon`, `item_expedition_winch_kit`, `item_fat_confit`, `item_fermentation_cleaning_reagent`, `item_fermentation_culture_starter`, `item_fermentation_filter_module`, `item_fermentation_preservation_concentrate`, `item_fermentation_service_kit`
`item_fermentation_starch_feedstock`, `item_fermentation_sugar_feedstock`, `item_fermentation_waste_pomace`, `item_fermented_organic_acid_carboy`, `item_fermented_sauerkraut`, `item_field_guide_annotated`, `item_filtered_water_crossing`, `item_flatbread`
`item_fleet_log_cylinder`, `item_focal_stirling_engine_generator`, `item_fog_mesh_roll`, `item_forged_clean_bill_chit`, `item_foucault_tester_rig`, `item_foundry_bearing_housing`, `item_foundry_blast_fitting`, `item_foundry_casing_blanks`
`item_foundry_cast_shot`, `item_foundry_crucible_spare`, `item_foundry_drill_blanks`, `item_foundry_furnace_grate`, `item_foundry_pickling_reagent`, `item_foundry_press_fitting`, `item_foundry_reinforcement_shoe`, `item_foundry_replacement_die`
`item_foundry_roof_armor_plate`, `item_foundry_shoring_bracket`, `item_foundry_structural_coupling`, `item_foundry_weather_canister`, `item_fuel`, `item_fur_mittens`, `item_garrison_manifest_forgery_kit`, `item_gas_mask`
`item_gas_mask_improved`, `item_gauge_block_set`, `item_geophone_probe`, `item_geothermal_descaling_kit`, `item_grain_flour`, `item_granary_receipt`, `item_greenhouse_ash_fertilizer`, `item_greenhouse_catchment_kit`
`item_greenhouse_compost`, `item_greenhouse_drip_kit`, `item_greenhouse_fish_emulsion`, `item_greenhouse_glass_pane`, `item_greenhouse_hand_cultivator`, `item_greenhouse_insecticidal_soap`, `item_greenhouse_line_filter`, `item_greenhouse_pest_mesh`
`item_greenhouse_pruning_shears`, `item_greenhouse_shade_cloth`, `item_greenhouse_sticky_traps`, `item_greenhouse_trowel`, `item_greenhouse_uv_sheeting`, `item_greenhouse_watering_can`, `item_groundwater_sensor`, `item_hardened_flail_chain`
`item_hardened_ground_anchor_spikes`, `item_harmonica`, `item_heavy_wool_coat`, `item_hermetic_sample_ampoule`, `item_honey_pot`, `item_honey_preserved_pulp`, `item_hot_dust_drum`, `item_hydraulic_actuator`
`item_hydraulic_drive_motor`, `item_hydraulic_ram_assembly`, `item_hydraulic_wire_cutter`, `item_hydro_baron_queue_chit`, `item_hydroponic_nutrients`, `item_icebreaker_rendezvous_flare_rocket`, `item_iff_beacon`, `item_improvised_burn_barrel`
`item_industrial_acid_carboy`, `item_industrial_cell_anode`, `item_industrial_oxidizer_reagent`, `item_insect_larvae_meal`, `item_insulated_boots`, `item_internal_spline_hub`, `item_iron_pyrite_ore`, `item_keyed_actuator_collar`
`item_laminated_ballistic_viewport_glass`, `item_lamp_oil_crossing`, `item_linear_breach_section`, `item_liquid_bleach_carboy`, `item_logistics_cipher_sheet`, `item_long_walk_route_ledger`, `item_low_noise_sensor_amplifier`, `item_machined_blank_medium`
`item_machined_blank_small`, `item_manual_bolt_shears`, `item_manual_field_medicine`, `item_manual_generator_maintenance`, `item_manual_rough_repairs`, `item_manual_seismology`, `item_marine_sealant_kit`, `item_mcraly_bond_coat_powder`
`item_mead_must_base`, `item_mechanical_breach_ram`, `item_mechanical_parts`, `item_medical_kit`, `item_medical_precursor_base`, `item_medical_saline_salt`, `item_mercury_barometer_station`, `item_meridian_archive_copy`
`item_metallurgy_copper_ingot`, `item_metallurgy_gear_blank`, `item_metallurgy_heavy_i_beam`, `item_metallurgy_iron_ingot`, `item_metallurgy_shaft_stock`, `item_metallurgy_shielding_plate`, `item_metallurgy_shoring_plate`, `item_metallurgy_solder_stock`
`item_metallurgy_spring_steel_billet`, `item_metallurgy_steel_billet`, `item_metallurgy_tool_blank`, `item_microfluidic_cartridge_general`, `item_microfluidic_reader`, `item_micrometer_set`, `item_military_radio_module`, `item_military_stimulants`
`item_mine_flail_module`, `item_neutralizer_lime_bag`, `item_nitrogen_supply`, `item_official_ballot_box`, `item_optical_flat`, `item_optical_pitch_lap`, `item_oxidizer_reagent_flask`, `item_oxygen_supply`
`item_parabolic_aluminum_dish_segment`, `item_paraffin_wax_neutron_shield`, `item_pdms_silicone_kit`, `item_pemmican`, `item_periscope_optics_prism`, `item_pickled_tubers`, `item_playing_cards`, `item_pneumatic_capsule_100mm`
`item_pneumatic_capsule_50mm`, `item_pocket_dosimeter`, `item_portable_kerosene_heater`, `item_portable_pid_detector`, `item_potassium_iodide_pack`, `item_potassium_permanganate_crystals`, `item_powered_mist_assist_module`, `item_precision_rangefinder_achromat`
`item_preservation_salt`, `item_press_tooling_set`, `item_prewar_diagnostic_scanner`, `item_quarantine_bands`, `item_radar_display_tube`, `item_radiation_shielding_panel`, `item_radiation_survey_meter`, `item_radio_cipher_rotor`
`item_radio_vacuum_tube`, `item_radiosonde`, `item_rail_control_component`, `item_rail_grinding_head`, `item_rail_profiling_cylinder`, `item_railroad_hydraulic_spike_puller`, `item_raw_propolis`, `item_reagent_clean`
`item_rebreather_canister`, `item_reinforced_support_cable`, `item_rejection_notice`, `item_rim_bead_kit`, `item_rock_salt_sack`, `item_rotor_balancing_kit`, `item_runflat_balancing_kit`, `item_runflat_insert_reinforced`
`item_runflat_insert_utility`, `item_salted_meat`, `item_salvage_cutting_tool`, `item_scavenger_guild_claim_marker`, `item_sea_ration`, `item_sealed_dive_lamp`, `item_sealed_packaging_foil`, `item_seed_ash_grain`
`item_seed_biolum_mushroom`, `item_seed_cold_legume`, `item_seed_hardy_tuber`, `item_seed_leafy_green`, `item_seed_medicinal_herb`, `item_seed_nutrient_algae`, `item_seed_oilseed`, `item_seed_packet_nonhybrid`
`item_seismic_detector`, `item_sentry_targeting_chip`, `item_separation_media_cartridge`, `item_shielded_badge_case`, `item_shielding_apron`, `item_ships_bell_picket`, `item_signal_lamp_module`, `item_silo_pest_treatment`
`item_slave_collar`, `item_sludge_cake`, `item_smoked_meat`, `item_smuggled_medicine`, `item_smugglers_ledger`, `item_solar_inverter`, `item_soldering_kit`, `item_spark_suppression_manifold`
`item_stabilization_tea`, `item_sterile_solvent_pack`, `item_substitution_dose`, `item_superalloy_turbine_blade_blank`, `item_surface_plate`, `item_surgical_arm_servo`, `item_surgical_kit`, `item_switch_stand_module`
`item_tablet_binder`, `item_tablet_coating_base`, `item_tailings_drum`, `item_taper_kit_opioid`, `item_telegraph_sounder_relay`, `item_thermal_lance`, `item_thermal_parka`, `item_thiamine_dose`
`item_titanium_breaching_shield`, `item_track_maintenance_kit`, `item_trade_salt_sack`, `item_travel_ration`, `item_tungsten_carbide_drill_bit`, `item_unsigned_debt_ledger_page`, `item_vacuum_seal_canner`, `item_vegetable_soup`
`item_vibration_dampening_mount`, `item_vitamin_supplements`, `item_warlord_trophy`, `item_wasteland_sketch`, `item_water_allocation_writ`, `item_water_filter`, `item_water_filter_advanced`, `item_water_purification_tablets_40_of_40`
`item_weighbridge_chit`, `item_well_maintenance_kit`, `item_zinc_bromide_shielding_window`, `keyring_charm`, `leather_strap`, `lighthouse_logbook`, `machinist_caliper`, `matchbook`
`mechanic_gloves`, `microfiche_film`, `midwife_satchel`, `mineral_chunk`, `miners_tag`, `morphine`, `night_optics_goggles`, `nurse_fob_watch`
`organic_residue`, `paper_stock`, `phonograph_needle`, `pocket_notebook`, `projector_bulb`, `prosthetic_wooden_arm`, `prosthetic_wooden_leg`, `railroad_ties`
`recipe_card`, `recipe_tin`, `reloading_primer`, `sandbags`, `scalpel`, `school_primer`, `scrap_chemical`, `scrap_plastic`
`sedative_draught`, `shopping_list`, `silver_scalpel`, `slate_and_chalk`, `smokeless_powder`, `soldering_kit`, `spring_mechanism`, `steel_rail_segment`
`stolen_ration_cache`, `surgical_saw`, `surgical_suture`, `synthetic_fuel_canister`, `tarnished_medal`, `tarnished_pocket_watch`, `teachers_stamp`, `train_coal`
`train_ticket_book`, `tram_punch`, `trap_bird_snare`, `trap_body_grip`, `trap_box`, `trap_cage`, `trap_deadfall`, `trap_fish`
`trap_improvised_wire`, `trap_net`, `trap_pit`, `trap_snare`, `weapon_battle_rifle`, `weapon_coach_shotgun`, `weapon_farm_carbine`, `weapon_marksman_rifle`
`weapon_molotov_thrower`, `weapon_nail_driver`, `weapon_pipe_shotgun`, `weapon_quiet_carbine`, `weapon_rebar_spear`, `weapon_revolver`, `weapon_rust_mosin`, `weapon_service_rifle`
`weapon_sidearm`, `weapon_smg`, `weapon_suppressor_improvised`, `weapon_trail_carbine`, `wedding_ring`, `wooden_plank`, `worn_photograph`, `worn_stethoscope`

### portrait (157)

`alex_raymond`, `ariana_cruz`, `asher_cole`, `blake_sullivan`, `cam_nguyen`, `casey_garcia`, `drew_paterson`, `dylan_park`
`elliot_bennett`, `emerson_wu`, `finley_clark`, `hayden_reyes`, `jamie_chen`, `jordan_kim`, `jude_ramirez`, `logan_white`
`morgan_lee`, `npc_almoner_hanna`, `npc_anete_sarn`, `npc_anton_renn`, `npc_arvo_tamm`, `npc_beacon_keeper_maren`, `npc_cass_polder`, `npc_coastal_chandler_orlov`
`npc_dalia_marun`, `npc_dara_mewn`, `npc_dessa_vane`, `npc_doctor_ianov`, `npc_dr_irina_vel`, `npc_driller_jarek`, `npc_eden_vale`, `npc_edor_vale`
`npc_elder_sava`, `npc_elena_vane`, `npc_emil_soren`, `npc_ferris_voss`, `npc_garrick_daal`, `npc_grease_monkey_tess`, `npc_hadi_morrow`, `npc_halden_mire`
`npc_halloran_vesk`, `npc_ilya_venn`, `npc_ilze_kaar`, `npc_ira_vell`, `npc_iran_bell`, `npc_ivo_fenn`, `npc_ivor_lasko`, `npc_janek_orel`
`npc_joren_malk`, `npc_jorin_hael`, `npc_junk_broker_solomon`, `npc_karel_norn`, `npc_kaspar_drej`, `npc_kasper_holt`, `npc_kess_adler`, `npc_kestrel`
`npc_len_quill`, `npc_lena_rost`, `npc_leva_quist`, `npc_lina`, `npc_liva_kern`, `npc_lotte_verrill`, `npc_mara_elsen`, `npc_mara_veln`
`npc_marek_voln`, `npc_maren_holt`, `npc_market_warden_grimm`, `npc_maro_veen`, `npc_mattis_cray`, `npc_mira_vos`, `npc_mirael_tesk`, `npc_nadia_lem`
`npc_net_mender_kira`, `npc_niko`, `npc_nila_brant`, `npc_nomi_fisk`, `npc_odile_vanter`, `npc_oren_varek`, `npc_oskar_ruut`, `npc_osran_kell`
`npc_osric_tann`, `npc_pavel_eren`, `npc_perrin_ashby`, `npc_piet_abar`, `npc_prior_silas`, `npc_quarry_steward_darek`, `npc_quil_esser`, `npc_rail_chandler_bess`
`npc_rika_dorn`, `npc_rivet_smith_milos`, `npc_salt_boiler_petyr`, `npc_salt_marshal_varn`, `npc_salt_trader_elena`, `npc_saria_voss`, `npc_selya_saltmarsh`, `npc_sena_aris`
`npc_sena_korr`, `npc_sergeant_pell`, `npc_stone_cutter_valya`, `npc_switch_master_korov`, `npc_tamsin_rook`, `npc_tessa_mirn`, `npc_the_cartwright_sisters`, `npc_tomas_geret`
`npc_tomas_reid`, `npc_torin_rask`, `npc_uma_tarran`, `npc_veda_ro`, `npc_wayfarer_tobias`, `npc_whisper_cipher`, `npc_wren`, `npc_wyn_omah`
`npc_wyn_sabler`, `npc_yara_holm`, `quinn_taylor`, `reese_flores`, `ren_murphy`, `riley_cooper`, `rowan_king`, `sage_green`
`skylar_walker`, `survivor_anneke_ruhl`, `survivor_ansel_duth`, `survivor_anton_salt_trader`, `survivor_boris_penal_medic`, `survivor_captain_alder`, `survivor_clara_sloan`, `survivor_colonel_brand`
`survivor_corporal_felix_vane`, `survivor_dr_erik_dahl`, `survivor_elena_vasquez_rail`, `survivor_family_child`, `survivor_first_officer_lindqvist`, `survivor_gregor_salt_miner`, `survivor_hadi_morrow`, `survivor_hierophant_malachi`
`survivor_igor_morozov`, `survivor_kess_adler`, `survivor_len_quill`, `survivor_lydia_hart`, `survivor_marcus_vane`, `survivor_marina_supply_driver`, `survivor_markov_arsenal_assayer`, `survivor_nadia_militia_scout`
`survivor_naomi_strand`, `survivor_ottilie_frayne`, `survivor_pavel_volkov`, `survivor_provost_kroll`, `survivor_sapper_vance`, `survivor_sister_martha`, `survivor_talia_upland_commander`, `survivor_tomas_lind`
`survivor_valeria_koss`, `survivor_vera_sokolov`, `survivor_yuri_foundry_caster`, `survivor_zoya_reid`, `taylor_morgan`

### location (312)

`checkpoint_kilo_armory`, `collapsed_building`, `concert_hall_ruins`, `convoy_echo7_cache`, `electrical_substation`, `family_bunker_backyard_shed`, `hospital_pharmacy`, `loc_abandoned_agricultural_station`
`loc_abandoned_half_track_convoy_wreck`, `loc_abandoned_tide_gauge`, `loc_agricultural_coop`, `loc_alloc_12b`, `loc_allotment_glasshouse_complex`, `loc_ammonium_nitrate_fertilizer_shed`, `loc_amnesty_petition_hall`, `loc_approach_apron`
`loc_approach_decon`, `loc_approach_hatch`, `loc_approach_stool`, `loc_archive_tape_silo`, `loc_arctic_ice_channel_buoy_12`, `loc_ash_militia_deadfall_barrier`, `loc_ash_needle`, `loc_ash_sign_cathedral_crater`
`loc_ash_sign_pyre_cliff`, `loc_ash_sign_shrine`, `loc_ash_woodland`, `loc_aurora_borealis_grounding_shoal`, `loc_automated_abattoir`, `loc_avalanche_gallery`, `loc_basement_vault`, `loc_bathymetric_boat`
`loc_black_flotilla_outpost`, `loc_black_thaw_drainage_basin`, `loc_border_checkpoint_ruins`, `loc_botanical_nursery`, `loc_breached_civil_defense_cache_9`, `loc_bridge_seven`, `loc_brine_pumping_sluice`, `loc_burned_woodland_ridge`
`loc_bus_reversal_loop`, `loc_cider_press`, `loc_civil_defense_bunker`, `loc_clifftop_observation_bunker`, `loc_cluster_block_c`, `loc_cluster_clinic`, `loc_cluster_gatehouse`, `loc_cluster_office`
`loc_cluster_quad`, `loc_cluster_school`, `loc_cluster_steam_substation`, `loc_coal_mine`, `loc_coastal_fog_signal_station`, `loc_coastal_meteorological_station`, `loc_cold_store_atlantic`, `loc_collapsed_peat_kiln_bunker`
`loc_collapsed_valley_viaduct`, `loc_comm_array`, `loc_conscription_office`, `loc_contaminated_water_access`, `loc_continental_convoy_staging_area`, `loc_continental_radio_beacon`, `loc_crossing_founders_marker`, `loc_crossing_granary_pledge`
`loc_crossing_nightfire`, `loc_crossing_petition_tent`, `loc_crossing_records_room`, `loc_crossing_scalehouse`, `loc_crossing_stallrow`, `loc_crossing_the_annex`, `loc_crossing_the_lockup`, `loc_crossing_underwrite_hall`
`loc_crossing_viaduct_gate`, `loc_crossing_watchtower`, `loc_crossing_weighbridge`, `loc_cut_abandoned_depot`, `loc_cut_accident_12`, `loc_cut_arsenal_ruin`, `loc_cut_brine_pool`, `loc_cut_dredger_hulk`
`loc_cut_kilometre_19`, `loc_cut_merchant_caravanserai`, `loc_cut_radiation_zone_alpha`, `loc_cut_south_beacon`, `loc_cut_waystation_a`, `loc_cut_weigh_hut`, `loc_d9_cache_bunker_delta`, `loc_d9_culvert_junction_bravo`
`loc_d9_underground_telecom_vault`, `loc_dead_zone`, `loc_deaddrop_command_shelter`, `loc_decommissioned_signal_relay`, `loc_deep_salt_hospital_sanctuary`, `loc_denial_cut_substation`, `loc_dentists_row`, `loc_department_store`
`loc_diesel_tank_farm`, `loc_drowned_cinema`, `loc_eastern_road`, `loc_evacuation_bus_depot`, `loc_excavation_archive_bunker`, `loc_excavation_civilian_shelter`, `loc_excavation_command_vault`, `loc_excavation_drainage_network`
`loc_excavation_metro_interchange`, `loc_excavation_mine_shaft`, `loc_excavation_storage_chamber`, `loc_excavation_utility_tunnels`, `loc_flooded_hydro_pump_cavern`, `loc_flooded_quarry_cistern`, `loc_flooded_subway_depot`, `loc_forestry_compound`
`loc_forestry_survey_post`, `loc_forward_roster_camp`, `loc_foundry_west_stacks`, `loc_frozen_river_ferry_crossing`, `loc_frozen_well_station`, `loc_frozen_wetland_crossing`, `loc_fuel_depot`, `loc_garrison_artillery_emplacement_bravo`
`loc_garrison_checkpoint_gamma`, `loc_garrison_checkpoint_gamma_exterior`, `loc_garrison_court_martial_cellar`, `loc_garrison_motor_pool`, `loc_garrison_signal_bunker_echo`, `loc_geological_core_vault`, `loc_geophone_pit_1`, `loc_geothermal_well_alpha`
`loc_grain_exchange`, `loc_grain_silo`, `loc_granite_arsenal_foundry`, `loc_granite_pass_weather_observatory`, `loc_gravel_backbone`, `loc_hidden_relay_bunker`, `loc_high_granite_mortar_pit_charlie`, `loc_highway_checkpoint`
`loc_holdfast`, `loc_hydro_baron_aqueduct_manifold`, `loc_hydro_baron_desal_plant_4`, `loc_hydro_baron_ledger_office`, `loc_ice_core_store`, `loc_ice_road_gate`, `loc_iron_crest`, `loc_iron_garrison`
`loc_iron_raiders_den`, `loc_irradiated_forest_edge`, `loc_junction_box_rail`, `loc_lock_gate_four`, `loc_logistics_reserve_cache`, `loc_low_background_lab`, `loc_maritime_icebreaker_dock`, `loc_metro_tunnel`
`loc_military_depot_perimeter`, `loc_minefield_observation_tower`, `loc_missile_silo`, `loc_motel_verity`, `loc_mountain_tunnel_refuge`, `loc_municipal_archive`, `loc_muster_treeline_camp`, `loc_network_fuse_bunker`
`loc_neutral_ground`, `loc_north_gate_water_tower`, `loc_old_crematory_stacks`, `loc_ordnance_shoulder`, `loc_overflow_alloc_11`, `loc_overflow_alloc_13`, `loc_overflow_blank_cellar`, `loc_overflow_pump_hatch`
`loc_penal_pioneer_trench_sector`, `loc_penal_quarry_crusher_plant`, `loc_pilgrim_switchbacks`, `loc_poison_gas_culvert_marsh`, `loc_police_precinct`, `loc_printworks`, `loc_public_swimming_baths`, `loc_pump_station_nine`
`loc_radio_relay_mast`, `loc_radioisotope_power_station`, `loc_rail_trestle_gorge`, `loc_railway_guild_roundhouse`, `loc_railway_span_44_alpha`, `loc_railway_telegraph_repeater_hut`, `loc_ration_queue_plaza`, `loc_rebuilder_brickworks_kiln`
`loc_records_annex`, `loc_recovery_yard`, `loc_regional_hospital`, `loc_reservoir_water_tower`, `loc_rhizome_research_vault`, `loc_river_bend_outpost`, `loc_river_gauging_station`, `loc_ruined_hospital_grounds`
`loc_rusted_span_bridge`, `loc_salt_cavern_explosives_magazine`, `loc_salt_cavern_medical_depot`, `loc_salt_cooling_canal`, `loc_salt_grade_hut`, `loc_salt_intake_caisson`, `loc_salt_iodine_store`, `loc_salt_membrane_hall`
`loc_salt_miners_barter_hall`, `loc_salt_outfall`, `loc_salt_scrap_membranes`, `loc_scavenger_camp`, `loc_scavenger_guildhall`, `loc_school_gymnasium`, `loc_sealed_marine_laboratory`, `loc_second_winter_homestead`
`loc_sector_4_rail_switchyard`, `loc_settlement_brine_pans`, `loc_settlement_cape_beacon`, `loc_settlement_ferry_crossing`, `loc_settlement_fort_karkov`, `loc_settlement_iron_siding`, `loc_settlement_lock_seven`, `loc_settlement_nine_rails`
`loc_settlement_pilgrim_hearth`, `loc_settlement_silo_burrow`, `loc_settlement_slate_hollow`, `loc_settlement_st_nicholas`, `loc_settlement_tinkers_notch`, `loc_shelf_deep_berth`, `loc_shelf_foghorn`, `loc_shelf_hearth4`
`loc_shelf_perimeter_breakwater`, `loc_shelf_pressure_ridge`, `loc_shelf_roadstead_crane`, `loc_shelf_service_channel`, `loc_shelled_church_belltower_lookout`, `loc_shelled_grain_elevator_ruin`, `loc_shelter_exterior_approach`, `loc_shelter_fire`
`loc_shelter_gate`, `loc_shelter_infirmary`, `loc_shelter_meeting`, `loc_shelter_perimeter`, `loc_shelter_quarters`, `loc_shelter_storage`, `loc_shrine_switchback_waystation`, `loc_signal_hill_tower`
`loc_snowline_station`, `loc_south_beacon_tower`, `loc_st_brigids_almshouse`, `loc_stack_airlock`, `loc_stack_clinic_alcove`, `loc_stack_filtration`, `loc_stack_mess`, `loc_stack_roster_wall`
`loc_stack_sleeping`, `loc_sub_level_maintenance_shaft_9`, `loc_sub_level_sewer_interceptor_6`, `loc_substation_yard`, `loc_suburban_district`, `loc_sulfur_knob`, `loc_summit_relay`, `loc_supply_corps_highway_redoubt`
`loc_surface_observation_post`, `loc_terrace_pumphouse`, `loc_the_allotments`, `loc_the_calibration_bench`, `loc_the_childrens_baseline_board`, `loc_the_dose_room`, `loc_the_final_dawn_outlook`, `loc_the_register_hall`
`loc_the_screening_station`, `loc_the_shallows_market`, `loc_the_tally_hall`, `loc_the_vessels_cell`, `loc_toll_house`, `loc_train_yard`, `loc_transit_authority_hq`, `loc_twelve_gauge_array`
`loc_understory_transmitter`, `loc_urban_pharmacy`, `loc_veterinary_surgery`, `loc_vitrified_crater_spring_pool`, `loc_vitrified_train_derailment_cut`, `loc_warehouse_district`, `loc_water_station`, `loc_water_treatment_plant`
`loc_weighbridge`, `loc_wind_gap_ridge`, `location_agricultural_research`, `location_ammunition_depot`, `location_apartment_block`, `location_automated_abattoir`, `location_burned_woodland`, `location_central_postal_hub`
`location_chemical_plant`, `location_drainage_network`, `location_frozen_wetland`, `location_grand_cinema`, `location_irradiated_forest`, `location_metro_station`, `location_metro_tunnel`, `location_municipal_library`
`location_municipal_water_reservoir`, `location_police_station`, `location_power_substation`, `location_quarry_overlook`, `location_radar_site`, `location_regional_blood_bank`, `location_stadium_evacuation_center`, `location_steelworks`
`location_sunshine_daycare`, `location_television_studio`, `location_upland_logging_camp`, `location_weather_station`, `old_library_cache`, `raider_ambush_site`, `raider_trap_location`, `ruined_garage`

### faction (18)

`cult_of_ash_sign`, `faction_doctrine_archetype_ashprophet`, `faction_doctrine_archetype_besiege`, `faction_doctrine_archetype_procedure`, `faction_doctrine_archetype_traffic`, `faction_forward_roster`, `faction_scavengers`, `faction_the_garrison`
`faction_the_granary_wardens`, `faction_the_lamplighters`, `faction_the_quarantine_post`, `faction_the_rebuilders`, `faction_the_smugglers_court`, `faction_the_water_committee`, `faction_unaligned`, `iron_garrison`
`raiders`, `warlords_sector_4`

## Naming

- Every generated file is named `placeholder_<category>_<catalog_id>.png`.
- No generated file impersonates a final-art stem (`{id}.jpg/png`).
- Manifest maps each `catalog_id` to its placeholder `runtime_path`.

## Regenerate

```bash
python3 scripts/tools/generate-placeholder-512-pack.py --write-inventory
python3 scripts/tools/generate-placeholder-512-pack.py --check
```
