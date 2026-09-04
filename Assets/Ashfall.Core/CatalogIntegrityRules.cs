// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core
{
    /// <summary>
    /// Immutable rule tables and predicates for catalog integrity validation.
    /// Defines authoritative id prefixes, definition keys, reference keys,
    /// range keys, vocabulary whitelists, and runtime-injected ids.
    /// </summary>
    public static class CatalogIntegrityRules
    {
        /// <summary>Id namespaces recognised as ids. Extend when a catalog introduces a new one.</summary>
        public static readonly string[] IdPrefixes =
        {
            "item_", "loc_", "location_", "quest_", "npc_", "survivor_", "faction_",
            "disease_", "event_", "recipe_", "relic_", "lore_", "room_", "stage_", "choice_",
            "mutation_", "flag_", "trait_", "anchor_", "season_", "kind_", "clinic_",
            "morph_", "drug_", "co_", "enc_", "narrative_", "dialogue_event_",
            "field_fauna_", "field_flora_", "field_guide_", "char_", "creature_", "settlement_", "territory_", "table_loot_", "scavenge_",
            "frequency_", "schedule_event_", "hidden_cache_", "archetype_",
            "frequency_", "schedule_event_", "hidden_cache_", "archetype_",
            "belief_profile_", "profession_", "background_", "phantom_background_",
            "pre_war_profession_", "personal_keepsake_item_", "stance_", "belief_",
            "trauma_", "phantom_", "echo_", "arc_", "offer_", "graft_", "vouch_",
            "radio_", "broadcast_", "crisis_", "zone_", "step_", "fragment_",
            "trust_", "phase_", "milestone_", "wave_", "scenario_", "toll_",
            "trade_", "wish_", "confession_", "guilt_", "current_", "echo_",
            "cassette_", "carving_", "template_", "zone", "part_", "code_",
            "contraband_", "glitch_", "telemetry_", "blackbox_", "directive_", "audit_",
            "hydrophone_", "borehole_", "cryopod_", "salt_mine_",
            "liturgy_", "canon_", "hymnal_", "epitaph_",
            "journal_psych_", "botany_", "folklore_children_", "folklore_", "fraud_ration_",
            "graffiti_", "ritual_", "superstition_", "memorial_rite_", "mourning_", "schism_",
            "cipher_station_", "alarm_seismic_", "emp_sniffer_", "wiretap_",
            "pathology_autopsy_", "pharma_", "surgery_log_", "sensory_loss_",
            "audit_gate_", "silt_report_", "lead_wall_", "filter_clog_",
            "well_contam_", "biota_cave_", "steam_vent_", "stalactite_assay_",
            "roach_hive_", "molerat_study_", "vulture_sighting_", "mosquito_vector_",
            "dragline_ruin_", "substation_fire_", "locomotive_armored_", "pipeline_sabotage_",
            "hoist_jam_", "munitions_leaching_", "sonar_fault_", "vault_breach_",
            "germplasm_audit_", "compressor_fail_", "methane_eruption_", "crop_genome_",
            "topo_sheet_", "scav_route_", "mudflow_report_", "crater_lake_",
            "slag_leach_", "carbide_tool_", "gear_quench_", "bullet_alloy_",
            "gasket_degrade_", "aramid_rot_", "tire_retread_", "celluloid_decay_",
            "prism_delam_", "sight_glass_", "rad_brown_", "scint_crystal_",
            "rag_pulp_", "ink_assay_", "type_wear_", "stencil_smear_",
            "ammonia_chiller_", "pickling_spoil_", "cellar_rot_", "smokehouse_assay_",
            "lime_kiln_", "pozzolan_mortar_", "firebrick_spall_", "mudbrick_assay_",
            "glass_melt_", "condenser_fracture_", "joint_grease_", "annealing_lehr_",
            "oak_bark_tan_", "mineral_tan_", "rawhide_bate_", "leather_harness_",
            "steam_well_", "turbine_blade_", "boiler_deaerator_", "steam_trap_",
            "escapement_wear_", "pendulum_thermal_", "mainspring_fatigue_", "clepsydra_silt_",
            "hemp_fiber_", "wire_rope_", "manila_hawser_", "rope_transmission_",
            "timber_creosote_", "square_set_", "dry_rot_", "mortise_tenon_",
            "crucible_slag_", "cupola_melting_", "pattern_maker_", "green_sand_",
            "slow_sand_", "ozone_", "chlorine_titration_", "carbon_adsorption_",
            "pneumatic_carrier_", "pneumatic_diverter_", "rootes_blower_", "pneumatic_cylinder_",
            "burr_millstone_", "bolting_silk_", "grain_silo_", "mill_tempering_",
            "cryo_seed_", "ragdoll_germination_", "silica_seed_", "heirloom_seed_",
            "sourdough_mother_", "brewers_yeast_", "silage_pit_", "fermentation_crock_",
            "langstroth_", "apiculture_", "honey_extractor_", "beeswax_",
            "charcoal_mound_", "retort_", "biochar_", "forge_charcoal_",
            "wood_ash_lye_", "tallow_saponification_", "cold_process_", "sweet_water_glycerin_",
            "hollander_beater_", "deckle_mould_", "screw_press_", "tub_sizing_",
            "drop_spindle_", "flyer_wheel_", "inkle_loom_", "backstrap_loom_",
            "treadle_loom_", "fulling_trough_",
            "bark_tanning_", "brain_tanning_", "currying_", "awl_stitch_",
            "clay_wedging_", "bisque_firing_", "slip_glaze_", "kiln_draw_",
            "fibre_heckling_", "strand_twisting_", "rope_closing_", "rope_break_",
            "tallow_rendering_", "beeswax_clarif_", "wick_braiding_", "candle_dip_",
            "bone_degreasing_", "antler_horn_", "bone_scraping_", "bone_tool_",
            // The Weight of Choices — faction branching system (Military slice).
            "branch_", "ending_",
            // Plan 20 — Wasteland Inhabitants & Field Guide
            "field_fauna_", "field_flora_", "field_guide_", "char_", "creature_",
            // Plan 21 — Phantom Memory & Heirloom World Layer
            "heirloom_", "secret_", "phantom_trigger_",
            // Plan 22 — Foundry, Greenhouse & Production
            "foundry_prod_", "crop_",
            // Plan 26 — Knowledge, Research & Skills: The Progression World
            "knowledge_", "skill_", "manual_", "procedure_", "finding_",
            // Plan 28 — Living Wasteland Ecology, Migration & Infestations
            "species_", "migration_", "infestation_", "eco_chain_",
            // Plan 36 — Wildlife Trapping Catalog
            "trap_"
        };

        /// <summary>
        /// Keys whose string value is the entity's OWN id (definition position).
        /// </summary>
        public static readonly string[] DefinitionKeys =
        {
            "id", "survivor_id", "item_id", "relic_id", "quest_id", "faction_id",
            "choiceId", "fragment_id", "frequency_id", "narrative_id", "step_id",
            "archetype_id", "background_id", "belief_profile_id", "profession_id",
            "pre_war_profession_id", "personal_keepsake_item_id",
            "phantom_background_id", "knowledge_key", "complete_mutation",
            "fail_mutation", "world_flag", "traits", "baseTraits", "traitIds",
            "manifesto_law_code", "zone_id", "encounterId", "set_flag",
            "setWorldFlag", "trait_granted", "latentExpertTrait", "inspectKey",
            "questlineId", "stageId", "firstStageId",
            // The Weight of Choices — faction branching system (Military slice).
            "ponr_flag", "ending_id",
            "recipe_id", "key", "message_key",
            // Plan 20
            "choice_id", "chain_id",
            // Plan 21
            "heirloom_id", "secret_id", "trigger_id", "gating_flag",
            // Plan 22
            "product_id", "internal_divisions",
            // Plan 26
            "manual_id", "procedure_id", "possible_findings",
            // Plan 28
            "migration_id", "infestation_id", "species_id",
            // Plan 36
            "trap_id"
        };

        /// <summary>
        /// Keys whose value (or array elements) REFERENCE a registered id.
        /// These are never registered themselves — a typo cannot bless itself.
        /// </summary>
        public static readonly string[] ReferenceKeys =
        {
            "resultItemId", "requiredItemId", "required_components", "materialId",
            "objective_items", "hidden_cache_items", "revealed_items",
            "hidden_cache_location", "revealed_location", "target_location_id",
            "target_location", "requires_location", "discovery_location_id",
            "location_reference", "locationId", "parentLocationId",
            "prereq_quest_id", "activeQuestlineId", "targetIds", "factionId",
            "targetFaction", "visitorFaction", "primaryFaction",
            "threateningFactionId", "requiredTrustFactionId", "survivorId",
            "traitId", "branchId", "scheduleEventId", "dialogue_event_id",
            "requiredFlag", "requiredFlagId", "RequiredFlagId", "RequiredEventFlags",
            "ambushFlag", "cleanWaterRewardFlag", "trait_granted",
            "latentExpertTrait", "requiredTrait", "itemId",
            "downstream_quest_trigger", "gating_flag", "nextStageId",
            "countermeasure_item_id",
            "from", "to",
            // Plan 46 — Scavenging Tables
            "scavenging_table_id",
            // Plans 90-93 — cupola foundry, vertical ascent, acoustic detection
            "feedstock_item_id", "fuel_item_id", "flux_item_id", "base_yield_item_id",
            "allowed_mold_ids", "output_item_id", "refractory_item_id", "descale_item_id",
            "install_item_ids", "repair_item_ids", "dampening_item_id"
        };

        /// <summary>Keys that must be ordered min <= max when both are present.</summary>
        public static readonly string[] RangeKeys = { "minDay", "maxDay", "MinDay", "min_day" };

        /// <summary>
        /// Pure vocabulary keys: their values are category/type/phase labels, not
        /// ids, and are never cross-referenced.
        /// </summary>
        public static readonly string[] VocabularyKeys =
        {
            "tags", "category", "type", "phase", "severity", "discovery_trigger", "badge_asset_id",
            "stance", "short_name", "identity", "sink", "notes", "display_name", "legacy_aliases",
            "collection_id", "observation_clue",
            "hazardType", "will_not", "lootCategories", "tech_offerings",
            "depletion_model", "primary_hazard_type", "hazard_type", "codex_unlock_id", "location_type", "rarity_tier",
            "effect_type", "effect_target", "rarity", "ignoreConsequence", "authenticity",
            "outcome_type", "specialEvents", "hidden_stash_location", "risk_profile",
            "callsign", "entry_type", "record_type", "directive_code", "classification",
            "issuing_authority", "vault_id", "audit_type", "sub_level", "auditor_designation",
            "compliance_status", "carrier_id", "timestamp_relative", "telemetry_channel",
            "payload_status", "effective_day_range", "buoy_callsign", "signal_classification",
            "borehole_id", "geological_formation", "pod_id", "subject_designation",
            "system_alert", "mine_gallery", "rock_medium", "inscription_tool", "recorder_identity",
            "cult_faction", "liturgy_type", "ritual_sacrament", "synod_chapter", "canon_number",
            "metallurgical_rule", "monastery_circle", "hymn_number", "liturgical_acoustic_mode",
            "grave_site", "marker_material", "deceased_identity", "cause_of_death",
            "author_designation", "quiet_hour_time", "psychological_marker", "botanical_name",
            "cultivation_tray", "edibility_status", "tradition_type", "origin_sector",
            "folk_theme", "case_id", "infraction_type", "accused_culprit", "verdict_penalty",
            "station_nickname", "modulation_mode", "station_id", "alert_tier", "detector_id",
            "pulse_classification", "intercept_channel", "target_faction", "speaker_identities",
            "case_number", "anatomical_region", "compound_name", "active_agent", "preparation_method",
            "efficacy_rating", "operation_code", "lead_surgeon", "anesthetic_used", "survival_outcome",
            "case_file", "sensory_modality", "pathological_cause",
            "gate_designation", "mechanical_subsystem", "failure_mode", "structural_severity",
            "sump_basin_id", "pump_model", "operational_status", "wall_sector_id",
            "structural_degradation_mode", "filter_bank_id", "filter_stage",
            "well_identifier", "aquifer_stratum", "contaminant_agent", "species_designation",
            "cavern_location", "bioluminescence_type", "ecological_niche", "vent_manifold_id",
            "failure_diagnostic", "sample_specimen_id", "mineral_species",
            "nest_location", "specimen_morph", "threat_rating", "colony_id", "caste_classification",
            "observation_post", "avian_morphology", "radiation_tracking_behavior", "silo_location_id",
            "vector_species", "pathogen_transmitted",
            "machine_designation", "structural_condition", "substation_id", "contaminant_combustion_byproduct",
            "locomotive_id", "locomotive_type", "current_operational_status", "pipeline_sector",
            "sabotage_method", "environmental_hazard_severity",
            "turret_emplacement_id", "caliber_designation", "hoist_mechanism_type", "jam_classification",
            "magazine_vault_id", "chemical_agent", "hazard_tier", "hydrophone_station_id",
            "transducer_element_type", "failure_classification", "vault_sector_id",
            "barrier_material", "breach_technique",
            "accession_number", "crop_species", "cryocooler_unit_id", "working_fluid",
            "vault_geological_sector", "eruption_trigger", "cultivar_id", "phenotypic_defect",
            "quadrangle_name", "grid_scale_ratio", "dominant_terrain_feature", "route_identifier",
            "lead_scout_name", "canyon_location_id", "structural_impact_severity", "crater_lake_name",
            "stratification_type",
            "furnace_unit_id", "metallurgical_defect", "tool_identifier", "carbide_grade",
            "wear_mechanism", "gear_component_id", "steel_alloy_grade", "quenching_medium",
            "alloy_batch_code",
            "mask_model_designation", "elastomer_polymer_type", "degradation_severity",
            "armor_item_id", "aramid_yarn_type", "failure_phenomenon",
            "tire_casing_id", "rubber_compound_formula", "road_wear_rating",
            "film_archive_reel_id", "polymer_base_chemistry", "decomposition_stage",
            "periscope_assembly_id", "optical_glass_type", "optical_cement_type",
            "boiler_system_id", "glass_composition", "optic_system_id",
            "substrate_material", "solarization_spectral_band", "detector_unit_id",
            "crystal_composition", "degradation_mode",
            "beater_station_id", "raw_fiber_source", "ink_formulation_code", "tannin_source",
            "pigment_complex", "font_case_identifier", "type_metal_composition", "wear_phenomenon",
            "stencil_print_id", "matrix_material_type", "ink_pigment_base", "smear_artifact_description",
            "chiller_unit_id", "system_failure_mode", "barrel_batch_code", "food_substrate_type",
            "spoilage_organism", "root_cellar_bay_id", "stored_crop_species", "fungal_pathogen_name",
            "smokehouse_facility_id", "fuel_wood_species",
            "kiln_structure_id", "feedstock_stone_type", "mortar_recipe_code", "pozzolanic_source",
            "curing_environment", "furnace_zone_id", "refractory_brick_grade", "failure_mechanism",
            "adobe_batch_identifier", "reinforcement_fiber_type", "clay_to_sand_ratio",
            "furnace_pot_identifier", "batch_feedstock_formula", "optical_clarity_grade",
            "distillation_rig_id", "glassware_component", "apparatus_station_id",
            "lubricant_compound_used", "failure_outcome", "lehr_furnace_id", "annealed_glass_article",
            "tannery_vat_id", "bark_source_botanical", "mineral_tan_liquor_id", "mineral_tanning_agent",
            "beamhouse_pit_id", "deliming_chemical_agent", "phenolphthalein_test_status",
            "currying_workshop_id", "fatliquor_compound_formula",
            "wellhead_designation", "turbine_unit_identifier", "boiler_plant_id",
            "steam_distribution_bay_id", "trap_mechanism_type",
            "clock_mechanism_id", "escapement_type", "pendulum_assembly_id", "rod_material_alloy",
            "spring_barrel_id", "spring_alloy_type", "clepsydra_station_id", "orifice_material_type",
            "retting_floor_id", "raw_stalk_crop_origin", "cable_spool_identifier", "wire_rope_construction",
            "hawser_coil_id", "fiber_botanical_origin", "drive_line_shaft_id", "rope_drive_system",
            "treatment_retort_id", "wood_species_treated", "stope_location_id", "timber_framing_system",
            "infestation_site_id", "fungal_species_identified", "framing_assembly_id", "joint_geometry_type",
            "peg_material_species",
            "crucible_pot_id", "crucible_lining_formula", "cupola_furnace_id", "coke_to_iron_charge_ratio",
            "pattern_shop_job_id", "timber_pattern_material", "shrinkage_allowance_fraction",
            "sand_muller_batch_id", "clay_binder_type",
            "filter_basin_id", "ozonator_unit_id", "dosing_station_id", "hypochlorite_reagent_grade",
            "carbon_filter_vessel_id", "carbon_base_feedstock",
            "capsule_serial_number", "diverter_junction_id", "diverter_mechanism_type",
            "blower_station_id", "rotor_lobe_configuration", "actuator_cylinder_id", "packing_leather_type",
            "millstone_pair_id", "stone_material_type", "sifter_reel_id", "silk_gauze_grade",
            "grain_silo_bin_id", "conditioning_bin_id",
            "dewar_canister_id", "crop_botanical_species", "germination_tray_id", "crop_cultivar_name",
            "desiccant_compound_type", "landrace_variety_id",
            "starter_crock_id", "microbial_consortium_type", "fermentation_tun_id", "yeast_strain_designation",
            "silage_trench_id", "forage_substrate_crop", "stoneware_crock_id",
            "hive_assembly_id", "comb_foundation_wax_grade", "chamber_zone_id", "extractor_unit_id",
            "rendering_vat_id",
            "coaling_mound_id", "feedstock_timber_species", "retort_vessel_id", "soil_amendment_lot_id",
            "carbon_assay_batch_id",
            "leaching_vat_id", "feedstock_ash_source", "boiling_kettle_id", "molding_rack_id",
            "glycerin_still_id",
            "beater_tub_id", "rag_feedstock_type", "mould_frame_id", "press_station_id",
            "sizing_vat_id",
            "spindle_unit_id", "fibre_stock_type", "draft_ratio_target",
            "loom_frame_id", "warp_fibre_type", "weft_thread_count",
            "treadle_unit_id", "heddle_count", "tie_up_pattern",
            "fulling_trough_id", "cloth_substrate_type", "nap_raising_tool",
            // Nun — Tanning & Leatherwork
            "required_station",
            "tanning_vat_id", "bark_species", "liquor_strength_baume",
            "hide_source_animal", "brain_emulsion_batch_id", "smoke_cycle_count",
            "tanned_hide_lot_id", "fat_liquor_type", "burnishing_tool",
            "leather_panel_id", "thread_material", "stitch_length_mm",
            // Samekh — Ceramics & Kiln Work
            "clay_bed_source", "wedging_cycle_count", "forming_method",
            "kiln_chamber_id", "peak_temp_celsius", "firing_duration_hours",
            "base_clay_type", "flux_material", "colorant_source",
            "draw_trial_piece_type", "surface_result",
            // Ayin — Rope Making & Cordage
            "fibre_source_plant", "retting_days", "heckling_comb_id",
            "fibre_type", "twist_direction", "strand_count_per_yarn",
            "strand_yarn_id", "rope_diameter_mm", "closing_tool",
            "rope_lot_id", "test_load_kg", "failure_mode",
            // Pe — Candle Making & Wax Rendering
            "fat_source_animal", "rendering_vat_id", "yield_grams",
            "wax_lot_source", "clarification_method", "clarity_grade",
            "wick_fibre_type", "braid_ply_count", "priming_wax_type",
            "candle_method", "wax_blend_type", "burn_duration_hours",
            // Tsadi — Bone & Horn Carving
            "bone_source_animal", "degreasing_method", "prep_duration_days",
            "material_type", "saw_tool_id", "blank_shape_cut",
            "blank_material", "abrasive_used", "surface_finish",
            "tool_type", "bone_blank_id", "point_angle_degrees",
            // Wasteland bestiary — narrative flavor vocabulary, not inventory refs.
            // The harvestable_materials list names creature yields for lore; the
            // WastelandBestiaryCatalog stores them as opaque strings and never
            // resolves them against items.json.
            "harvestable_materials"
        };

        /// <summary>
        /// Ids that are legitimately defined OUTSIDE the catalogs: runtime flags
        /// set by code, pseudo-locations, enum members, and sentinels.
        /// </summary>
        public static readonly string[] KnownRuntimeIds =
        {
            "player_shelter",        // pseudo-location: the player's own bunker (quests, world history)
            "scarred_state",         // world-state flag raised by trauma code
            "guilt_refugee_turned",  // guilt-source pattern ids used by events "add_trait" effects
            "guilt_water_trade", "guilt_stranded_rescue", "cruel_trader_death",
            "none",                  // "no faction / no id" sentinel
            "faction_none",          // "no external faction" sentinel for internal quests
            "Paranoid", "Cautious", "Realist", "Reckless", "Denialist", "Fatalist",
                                     // RiskBiasTrait enum members (events requiredTrait gates)
            "a", "b",                // narrative-arc branch designators (branch_a/branch_b siblings)
            "infected_refugee", "garrison_deserter", "dying_trader", "bounty_target",
                                     // survivors created by events' add_survivor effects
            "stores",               // internal bunker room (set_quarantine effect)
            "trade_goods",          // trade-category label used in wants/offers
            "flag_verdict_eden_log_recovered", "flag_verdict_fuse_world_read",
            "flag_verdict_shift_charter_restored", "flag_verdict_clerk_met",
            "flag_verdict_call_resolved", "flag_verdict_relay_read",
            "flag_verdict_fuse_advanced", "flag_verdict_wing_slept",
            // Expansion 12 (Vel/Vigil) orphan-knock gating flag — set at runtime by
            // future exp-12 code; registered in whitelists/orphan_knocks.json as a
            // deliberate, canonically-tracked orphan door event.
            "flag_exp07_vel_vigil_knock",
                        // Plan 25 (Faction Ecology & the Muster) — political flags produced at
            // runtime by the FactionActionBoard / FactionWarChainRunner seams and
            // consumed across muster_witnesses.json / muster_camp_scenes.json.
            // Full producer->consumer map: whitelists/plan25_flags.json.
            "flag_become_warlord", "flag_escalation_bitter_water", "flag_escalation_bitter_water_investigated", "flag_escalation_cistern_blockade",
            "flag_escalation_cistern_published", "flag_escalation_empty_chair", "flag_escalation_marked_ruin", "flag_escalation_marked_ruin_mediated",
            "flag_escalation_prisoner_gate", "flag_escalation_prisoner_truth_told", "flag_escalation_stopped_convoy", "flag_favor_coalition_mediation_served",
            "flag_favor_coalition_rules_first", "flag_favor_coalition_supply_shared", "flag_favor_hydro_intake_audited", "flag_favor_hydro_toll_paid",
            "flag_favor_hydro_water_accord_honored", "flag_favor_raider_parley_honored", "flag_favor_scavenger_apprentice_backed", "flag_favor_scavenger_arbitration_fair",
            "flag_favor_scavenger_claim_recognized", "flag_grievance_coalition_mediation_refused", "flag_grievance_coalition_security_backed", "flag_grievance_coalition_supply_refused",
            "flag_grievance_hydro_appeal_refused", "flag_grievance_hydro_intake_disputed", "flag_grievance_hydro_toll_defaulted", "flag_grievance_raider_code_widened",
            "flag_grievance_raider_parley_broken", "flag_grievance_raider_passage_evaded", "flag_grievance_raider_passage_fought", "flag_grievance_scavenger_arbitration_refused",
            "flag_grievance_scavenger_claim_disputed", "flag_grievance_scavenger_registrar_defied", "flag_messenger_kept", "flag_peace_bread_before_bullets",
            "flag_peace_faction_forms", "flag_peace_refusal_at_dawn", "flag_peace_volunteers_dry", "flag_war_refugees_arrived",
            "flag_war_requisition_demand", "flag_war_requisition_met", "flag_war_requisition_refused", "flag_war_shelter_took_wounded",
            "flag_war_sheltered_retaliation_families",
"paper_scrap", "item_teddy_bear", "crayon", "ammo_9x19", "blood_bag",
            "item_suitcase_locked", "fat_rendered", "industrial_bleach", "bone_saw",
            "ammonia_tank", "cardboard_box", "cigarette_pack_sealed",
            "acoustic_foam_panel", "item_anchor_notes",
            // Plan 20 environmental-text pseudo-location labels (settlement areas used as
            // descriptive location strings in environmental_texts_expansion_05.json — not
            // routable map nodes, so not in locations.json)
            "settlement_wall", "settlement_center"
        };

        public static bool IsVocabularyKey(string key) =>
            Array.IndexOf(VocabularyKeys, key) >= 0;

        public static bool IsKnownRuntimeId(string value) =>
            Array.IndexOf(KnownRuntimeIds, value) >= 0;

        public static bool IsDefinitionKey(string key) =>
            Array.IndexOf(DefinitionKeys, key) >= 0;

        public static bool IsReferenceKey(string key) =>
            Array.IndexOf(ReferenceKeys, key) >= 0;

        public static bool IsRangeKey(string key) =>
            Array.IndexOf(RangeKeys, key) >= 0;

        public static bool StartsWithAnyPrefix(string value) =>
            StartsWithAny(value, IdPrefixes);

        public static bool StartsWithAny(string value, string[] prefixes)
        {
            for (int i = 0; i < prefixes.Length; i++)
                if (value.StartsWith(prefixes[i], StringComparison.Ordinal))
                    return true;
            return false;
        }
    }
}
