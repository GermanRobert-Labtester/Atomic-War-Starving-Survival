// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using System.IO;
using System.Text.Json;
using Ashfall.Core.Narrative;
using Ashfall.Core.Radiation;

namespace Ashfall.Core
{
    /// <summary>
    /// ASHFALL data-layer gate: cross-references every id used across the JSON
    /// catalogs in StreamingAssets/Data against the definitions those catalogs
    /// declare. Implements the AGENTS.md rule "never invent an id that isn't in
    /// the master list" mechanically:
    ///
    /// 1. REGISTRY — every value in a definition position (an entity's own id,
    ///    structural id, mutation/knowledge-key, survivor field tag, ...) is
    ///    collected with its file + JSON path.
    /// 2. TIER-1 — every string anywhere in the corpus that starts with a known
    ///    snake_case id-namespace prefix must resolve in the registry. Catches
    ///    dangling cross-file references and prefixed typos.
    /// 3. TIER-2 — every value (or array element) of a known *reference* key
    ///    (resultItemId, requiredItemId, target_location_id, prereq_quest_id,
    ///    effects.*, required_components, ...) must resolve in the registry.
    ///    Reference keys are deliberately NOT registered, so a typo cannot
    ///    bless itself.
    /// 4. RANGES — minDay/maxDay pairs must be ordered.
    /// 5. UNIQUENESS — the same definition id must not appear twice in one file.
    ///
    /// Engine-agnostic: IFileIO + IJsonSerializer for hosts, and a direct
    /// System.Text.Json walk for structure. Wired as a Godot gate via
    /// --data-integrity-selftest (see HostCli).
    /// </summary>
    public sealed class CatalogIntegrityReport
    {
        public readonly List<string> Errors = new List<string>();
        public readonly List<string> Warnings = new List<string>();

        public int ErrorCount => Errors.Count;
        public bool Clean => Errors.Count == 0;

        /// <summary>Distinct ids whose first occurrence was registered here. Each
        /// id has exactly one author in the registry.</summary>
        public int AuthoredIds;

        /// <summary>Legitimate references to an already-authored id — cross-file
        /// enrichment foreign keys, shared stage/choice templates, per-container
        /// row rewrites. This is normal composition, not a conflict.</summary>
        public int ReuseCount;

        public string Summary
        {
            get
            {
                string head = Errors.Count == 0
                    ? "DATA_INTEGRITY_SELFTEST PASS"
                    : "DATA_INTEGRITY_SELFTEST FAIL (" + Errors.Count + ")";
                return head + " — " + (Errors.Count + Warnings.Count) + " findings"
                    + " (" + AuthoredIds + " ids authored, " + ReuseCount + " reuses reserved)";
            }
        }

        public void Error(string message)
        {
            Errors.Add(message);
        }

        public void Warn(string message)
        {
            Warnings.Add(message);
        }
    }

    public static class CatalogIntegrityValidator
    {
        /// <summary>Id namespaces recognised as ids. Extend when a catalog introduces a new one.</summary>
        public static readonly string[] IdPrefixes =
        {
            "weather_gate_",
            "item_", "loc_", "location_", "quest_", "npc_", "survivor_", "faction_", "settlement_", "territory_", "table_loot_", "scavenge_",
            "chem_agent_", "comms_target_", "ceremony_", "robot_",
            "vessel_", "hobby_", "degrade_profile_", "thermal_gear_", "insul_", "fault_", "mentorship_", "caravan_",
            "fallout_pattern_", "desperation_", "bounty_template_", "lore_archive_",
            "procedure_", "rail_node_", "rail_segment_", "rail_edge_", "car_", "strain_", "substrate_", "law_",
            "development_trait_", "interrogation_", "camo_",
            "disease_", "event_", "recipe_", "relic_", "lore_", "room_", "stage_", "choice_",
            "mutation_", "flag_", "trait_", "anchor_", "season_", "kind_", "clinic_",
            "morph_", "drug_", "co_", "enc_", "narrative_", "dialogue_event_",
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
            "trap_",
            // Plan 40 — Ledger Debt Templates
            "debt_", "conseq_",
            // Plans 182-185 — Aviation, Forced Labor, Narcotics, Settlement Politics
            "aircraft_", "camp_", "chem_", "policy_",
            // Plans 90-93 — acoustic listening arrays (rig_/mold_/charge_/synth_ ids
            // register via definition keys; references validate via ReferenceKeys)
            "array_",
            // Flagship institutions (Tasks 5-8) — culture / diplomacy / sky defense / sanatorium
            "tome_", "treaty_", "therapy_", "condition_",
            // Flagship XI (Plans 154-157) — morale contagion, pathogen strains,
            // subterranean networks, psyops campaigns (ids register via `id`;
            // cross-references validate via ReferenceKeys)
            "contagion_", "pathogen_", "subnode_", "psyops_",
            // Plans 110-113 — Industrial Chemistry, Solar-Thermal, Precision Optics, Ballistic Shields
            "process_chlor_alkali_", "solar_dish_", "optic_", "shield_",
            // Flagship Tasks 5-8 — Weather Hardening, Counter-Intelligence, Geothermal, Recon Telemetry
            "upgrade_", "agent_", "strata_", "probe_",
            // Plans 62-65 — Pre-war Archives, Captives, Food Preservation, Epilogues
            "archive_", "captive_", "topic_", "preservation_", "recipe_cure_", "recipe_smoke_", "epilogue_",
            // Plans 50-53 — Vehicle Garage, Faction Espionage, Survivor Mental Health, Subterranean Acoustics
            "vmod_", "fop_", "fdrop_", "acue_",
            // Plan 135 — Narrative Codex Discovery
            "disc_",
            // Plans 126-129 — Industrial Resilience & Remote Sensing Tranche
            "bio_ferm_"
        };

        /// <summary>
        /// Keys whose string value is the entity's OWN id (definition position).
        /// Structural ids and usage-defined vocabularies (knowledge keys,
        /// mutations, survivor-field tags, trait lists) are registered so
        /// cross-references to them resolve. Array-valued definition keys
        /// (traits, baseTraits) register each element.
        /// </summary>
        public static readonly string[] DefinitionKeys =
        {
            "route_id", "id", "survivor_id", "item_id", "relic_id", "quest_id", "faction_id",
            "choiceId", "fragment_id", "frequency_id", "narrative_id", "step_id",
            "archetype_id", "background_id", "belief_profile_id", "profession_id",
            "pre_war_profession_id", "personal_keepsake_item_id",
            "phantom_background_id", "knowledge_key", "complete_mutation",
            "fail_mutation", "world_flag", "traits", "baseTraits", "traitIds",
            "manifesto_law_code", "zone_id", "encounterId", "set_flag",
            "setWorldFlag", "trait_granted", "latentExpertTrait", "inspectKey",
            "questlineId", "stageId", "firstStageId",
            // The Weight of Choices — faction branching system (Military slice).
            "ponr_flag", "ending_id", "ending_key",
            "recipe_id", "silo_id", "gear_id", "key",
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
            "trap_id",
            // Plans 194-197
            "profile_id", "vessel_id", "hobby_id",
            // Plans 186-189
            "pattern_id", "template_id", "archive_id", "desperation_id",
            // Plans 190-193
            "segment_id", "node_id", "car_type_id", "strain_id", "substrate_id", "law_id",
            // Plans 178-181
            "trait_id", "tactic_id", "mutation_id", "camo_id",
            // Plans 182-185
            "aircraft_id", "camp_id", "chem_id", "policy_id", "stage_id",
            // Flagship institutions (Tasks 5-8)
            "tome_id", "treaty_id", "ordnance_id", "therapy_id", "condition_id",
            // Plans 110-113
            "process_id", "concentrator_id", "optic_recipe_id", "shield_id",
            "upgrade_id", "strata_id", "channel_id", "platform_id", "equipment_id", "reactor_id",
            // Plans 54-57 — Thermodynamics, Seismic, Barter, Apprenticeship
            "insulation_id", "fault_id", "caravan_id", "mentorship_id", "legacy_trait_id",
            // Plan 73 — Rail Logistics
            "rail_edge_id",
            // Plan 135 — Narrative Codex Discovery
            "discovery_id"
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
            "downstream_quest_trigger", "nextStageId",
            "countermeasure_item_id",
            "from", "to",
            // Plan 21
            "base_item_id", "discovery_source_id",
            // Plan 40 — Ledger Debt Templates
            "creditorId", "principalItemId", "consequenceId", "escalationId",
            "targetFactionId", "collateralItemId",
            // Plans 110-113
            "blank_item_id",
            // Plan 46 — Scavenging Tables
            "scavenging_table_id",
            // Plans 178-181
            "parent_mutation_ids", "exclusive_mutation_ids",
            // Plans 90-93 — cupola foundry, vertical ascent, acoustic detection
            "feedstock_item_id", "fuel_item_id", "flux_item_id", "base_yield_item_id",
            "allowed_mold_ids", "output_item_id", "refractory_item_id", "descale_item_id",
            "install_item_ids", "repair_item_ids", "dampening_item_id",
            // Flagship institutions (Tasks 5-8) — sanatorium condition refs
            "eligible_conditions",
            // Flagship XI (Plans 154-157) — pathogen strain lineage, subterranean
            // anchors, psyops faction targets
            "strain_of", "mutation_targets", "surface_anchor_id", "target_faction_id",
            // Plans 62-65
            "cleaning_solvent_id", "reward_research_ids", "preservative_item_id", "reward_item_id", "potential_topics", "input_item_id",
            // Wildlife trapping prey definition: non-empty value = explicit disease-catalog reference; empty value = valid runtime tier fallback.
            "diseaseId",
            // Plans 126-129 — biological fermentation catalog references
            "feedstock_item_ids", "byproduct_item_ids", "starter_item_id",
            "filter_item_id", "build_cost_item_ids", "sanitize_cost_item_ids",
            "service_cost_item_ids"
        };

        /// <summary>Keys that must be ordered min <= max when both are present.</summary>
        public static readonly string[] RangeKeys = { "minDay", "maxDay", "MinDay", "min_day" };

        /// <summary>
        /// Pure vocabulary keys: their values are category/type/phase labels, not
        /// ids, and are never cross-referenced. Not checked at all. (wants/offers/
        /// lootCategories deliberately stay checked for PREFIXED ids — they mix
        /// vocab and real item ids, and dangling item refs there are real bugs.)
        /// </summary>
        public static readonly string[] VocabularyKeys =
        {
            // Flagship institutions (Tasks 5-8) + Plans 90-93 label columns whose
            // slug vocabularies are not catalog references (repaired alongside the
            // flagship institutions data registration).
            "agenda_clauses", "canonical_surface", "sensor_class", "tool_class", "effectType",
            "tags", "intel_tags", "category", "type", "phase", "discovery_trigger", "badge_asset_id", "art_asset_id",
            "stance", "short_name", "identity", "sink", "notes", "display_name", "exclusive_group",
            "collection_id", "affinity_key", "legacy_aliases", "observation_clue",
            "hazardType", "will_not", "lootCategories", "tech_offerings", "narrativeHook", "patrol_archetype",
            "gauge_tag", "clearance_requirement", "obstacle_profile", "repair_requirement", "track_condition",
            "trigger_condition", // Tasks 9–12: closed distress follow-up trigger grammar (validated by ValidateDistressSignalStages), never an id reference
            "outcome_type", "specialEvents", "hidden_stash_location", "risk_profile", "target_type",
            "forged_credentials", "behavior_flags", "platform_type", "sensor_suite",
            "callsign", "entry_type", "record_type", "directive_code", "classification",
            "issuing_authority", "vault_id", "audit_type", "sub_level", "auditor_designation",
            "compliance_status", "carrier_id", "timestamp_relative", "telemetry_channel",
            "payload_status", "effective_day_range", "buoy_callsign", "signal_classification",
            "depletion_model", "primary_hazard_type", "hazard_type", "codex_unlock_id", "location_type", "rarity_tier",
            "effect_type", "effect_target", "rarity", "ignoreConsequence", "authenticity",
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
            "harvestable_materials",
            // Plans 62-65
            "encryption_grade", "intel_category", "allowed_food_types", "food_type_by_item_id",
            // Plans 50-53
            "slot_type", "compatible_vehicle_tags", "operation_class", "target_subsystem", "risk_level", "trigger_tags", "journal_entry_key", "bus_id", "playback_mode", "ducking_group", "attenuation_profile",
            // Plan 135 — Narrative Discovery Manifest vocabulary & foreign keys
            "channel", "source_record_id", "source_catalog", "producer_type"
        };

        /// <summary>
        /// Ids that are legitimately defined OUTSIDE the catalogs: runtime flags
        /// set by code, pseudo-locations (the player's own bunker), enum members
        /// (RiskBiasTrait), sentinels, and structural branch designators. Keep in
        /// sync with the code constants that create them.
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
            "caravan_space", "caravan_access", // trade services offered in character wants/offers
            "archive_proof",        // Current service token offered by the Tempest; not an inventory/catalog id.
            "flag_verdict_eden_log_recovered", "flag_verdict_fuse_world_read",
            "flag_verdict_shift_charter_restored", "flag_verdict_clerk_met",
            "flag_verdict_call_resolved", "flag_verdict_relay_read",
            "flag_verdict_fuse_advanced", "flag_verdict_wing_slept",
            "flag_verdict_reid_enrolled", "flag_verdict_vane_enrolled", "flag_verdict_holt_enrolled",
            "flag_verdict_cliff_signal_decoded", // Plan 93: materialized in VerdictHostSession (machine-log read depth)
            // Expansion 12 (Vel/Vigil) orphan-knock gating flag — set at runtime by
            // future exp-12 code; registered in whitelists/orphan_knocks.json as a
            // deliberate, canonically-tracked orphan door event.
            "flag_exp07_vel_vigil_knock",
            "flag_grievance_scavenger_claim_disputed", "flag_escalation_marked_ruin",
            "flag_escalation_marked_ruin_mediated", "flag_favor_scavenger_claim_recognized",
            "flag_favor_scavenger_arbitration_fair",
            // Plan 25 (Faction Ecology & the Muster) — political flags produced at
            // runtime by the FactionActionBoard / FactionWarChainRunner seams;
            // producer->consumer map in whitelists/plan25_flags.json.
            "flag_become_warlord", "flag_escalation_bitter_water", "flag_escalation_bitter_water_investigated", "flag_escalation_cistern_blockade",
            "flag_escalation_cistern_published", "flag_escalation_empty_chair", "flag_escalation_prisoner_gate", "flag_escalation_prisoner_truth_told",
            "flag_escalation_stopped_convoy", "flag_favor_coalition_mediation_served", "flag_favor_coalition_rules_first", "flag_favor_coalition_supply_shared",
            "flag_favor_hydro_intake_audited", "flag_favor_hydro_toll_paid", "flag_favor_hydro_water_accord_honored", "flag_favor_raider_parley_honored",
            "flag_favor_scavenger_apprentice_backed", "flag_grievance_coalition_mediation_refused", "flag_grievance_coalition_security_backed", "flag_grievance_coalition_supply_refused",
            "flag_grievance_hydro_appeal_refused", "flag_grievance_hydro_intake_disputed", "flag_grievance_hydro_toll_defaulted", "flag_grievance_raider_code_widened",
            "flag_grievance_raider_parley_broken", "flag_grievance_raider_passage_evaded", "flag_grievance_raider_passage_fought", "flag_grievance_scavenger_arbitration_refused",
            "flag_grievance_scavenger_registrar_defied", "flag_messenger_kept", "flag_peace_bread_before_bullets", "flag_peace_faction_forms",
            "flag_peace_refusal_at_dawn", "flag_peace_volunteers_dry", "flag_war_refugees_arrived", "flag_war_requisition_demand",
            "flag_war_requisition_met", "flag_war_requisition_refused", "flag_war_shelter_took_wounded", "flag_war_sheltered_retaliation_families",
            "paper_scrap", "item_teddy_bear", "crayon", "ammo_9x19", "blood_bag",
            "item_suitcase_locked", "fat_rendered", "industrial_bleach", "bone_saw",
            "ammonia_tank", "cardboard_box", "cigarette_pack_sealed",
            "acoustic_foam_panel", "item_anchor_notes",
            // Godot host presentation IDs. These cues are registered by the
            // host-side AudioCueCatalog rather than a gameplay JSON catalog, but
            // authored radio broadcasts may reference them through audio_cue.
            "radio_vo_ch3_ash_road", "radio_vo_ch7_milband",
            "radio_vo_ch11_stockpile", "radio_vo_kind_hatch",
            "radio_vo_kind_parley", "radio_vo_verdict_meter",
            "radio_vo_verdict_eden", "radio_vo_verdict_count",
            "radio_vo_verdict_geophone", "radio_vo_verdict_reckoning",
            // Plan 29 — shelter runtime room ids. These rooms are authored in CODE
            // (StartingLevelSystem Day-1 roster, ShelterAssignmentHostSession,
            // HoldfastInteriorView spatial map), not in a JSON catalog, so tier-1
            // references to them (shelter_room_identities.json legacy_aliases,
            // future identity/quirk data) resolve here instead. Keep in sync with
            // those rosters. Rooms defined in power_grid.json (room_air_filtration,
            // room_clinic, room_water_pump, room_greenhouse, room_foundry,
            // room_lighting_main) are data-registered and deliberately NOT listed.
            "room_bunker_corridor", "room_filtration_stack", "room_storage_bay",
            "room_bunks_living", "room_radio_tuner",
            "room_memorial_wall", // ShelterDecorHostSession's authored memorial-wall room
            "room_bunks", "room_kitchen", "room_workshop", "room_filtration",
            "room_airlock",
            "room_main",  // "Main Vault" — ShelterScheduleHostSession's power room; the Plan 29B generator home (continuity §7.3)
            // Plan 28/20A — field-guide unlock trigger keywords (runtime only,
            // not data-authority ids; the field-guide unlock system reads these
            // strings from unlock_trigger fields).
            "trap_catch", "combat_encounter", "encounter_sighting",
            "forage_discovery", "greenhouse_cultivation", "harvest_event",
            "scout_observation",
            // Radio rumor / broadcast quest hooks
            "quest_patrol_bounty", "quest_missing_caravan", "quest_orphaned_stock",
            // Companion trust gating flags (whitelists/companion_trust_flags.json)
            "trust_edor_above_zero", "trust_leva_above_zero", "trust_yara_above_zero", "trust_mire_above_zero",
            // Radio station owner / broadcast factions
            "faction_civil_defense", "faction_independent_survivors", "faction_unknown_intelligence", "faction_automated_infrastructure"
        };

        /// <summary>
        /// Plan 144 — prefix-grammar keys. The string value at these keys is an
        /// id PATTERN used to recognize whole families of ids (e.g.
        /// moral_choice_chains.json merge_rules.merge_quest_prefix =
        /// "quest_moral_merge_"), never a concrete id itself. Values are
        /// shape-checked (trailing underscore + rooted in a known id
        /// namespace) and are exempt from Tier-1 foreign-key resolution — the
        /// pattern token must never need a placeholder definition to pass, and
        /// must never be selectable as an entity.
        /// </summary>
        public static readonly string[] PrefixPatternKeys =
        {
            "merge_quest_prefix"
        };

        /// <summary>
        /// Plan 144 — playable-quest grammar markers. A quest row carrying any
        /// of these defines an EXECUTABLE quest. Identity-only registries (e.g.
        /// questline_master.json) acknowledge ids without these markers and do
        /// not count as second definitions.
        /// </summary>
        private static readonly string[] QuestExecutableMarkers =
        {
            "choices", "stages", "objectives", "steps"
        };

        private sealed class Ctx
        {
            public readonly Dictionary<string, List<string>> Registry =
                new Dictionary<string, List<string>>(StringComparer.Ordinal);
            public readonly List<Ref> PendingRefs = new List<Ref>();
            public readonly Dictionary<string, RangeMemoEntry> RangeMemo =
                new Dictionary<string, RangeMemoEntry>(StringComparer.Ordinal);
            public CatalogIntegrityReport Report;
            public string File;
            /// <summary>Ids authored (first-ever occurrence of each value).</summary>
            public int Authored;
            /// <summary>References to an id that is already authored (reuse).</summary>
            public int Reuse;
        }

        private struct Ref
        {
            public string Value;
            public string Path;
            /// <summary>True when the position is a declared reference key (Tier 2:
            /// bare ids must also resolve). False = generic position (Tier 1:
            /// only prefixed ids are checked).</summary>
            public bool Strict;
            public string? EntityContext;
        }

        public static CatalogIntegrityReport Validate(string dataDirectory, IFileIO files)
            => Validate(dataDirectory, files, SearchOption.TopDirectoryOnly);

        public static CatalogIntegrityReport Validate(string dataDirectory, IFileIO files, SearchOption searchOption)
        {
            var report = new CatalogIntegrityReport();
            if (string.IsNullOrEmpty(dataDirectory) || !files.DirectoryExists(dataDirectory))
            {
                report.Error("data directory missing: " + dataDirectory);
                return report;
            }

            string[] jsonFiles;
            try
            {
                jsonFiles = CatalogFileSystem.EnumerateJsonFiles(files, dataDirectory, searchOption);
            }
            catch (Exception e)
            {
                report.Error("cannot enumerate data directory: " + e.Message);
                return report;
            }
            Array.Sort(jsonFiles, StringComparer.Ordinal);

            var ctx = new Ctx { Report = report };
            foreach (string file in jsonFiles)
            {
                // Path.GetFileName handles both filesystem and res:// paths; trim res:// prefix first for consistent leaf.
                string leaf = file.StartsWith("res://", StringComparison.Ordinal)
                    ? file.Substring(file.LastIndexOf('/') + 1)
                    : Path.GetFileName(file);
                ctx.File = leaf;
                if (TryParse(file, files, out JsonDocument doc, report))
                {
                    using (doc)
                    {
                        RequireSchemaVersion(doc.RootElement, leaf, report);
                        Walk(doc.RootElement, ctx.File, ctx);
                    }
                }
            }

            // Tier 1: prefixed references must resolve.
            // Tier 1 + Tier 2 with dedupe: identical (value, path) pairs can be
            // collected twice (reference-key array elements are also walked).
            var reported = new HashSet<string>(StringComparer.Ordinal);
            foreach (Ref r in ctx.PendingRefs)
            {
                if (!reported.Add(r.Value + "@" + r.Path)) continue;
                if (IsKnownRuntimeId(r.Value)) continue;
                bool prefixed = StartsWithAny(r.Value, IdPrefixes);
                if (ctx.Registry.ContainsKey(r.Value)) continue;
                if (prefixed || r.Strict)
                {
                    string contextSuffix = !string.IsNullOrEmpty(r.EntityContext) ? " (" + r.EntityContext + ")" : string.Empty;
                    report.Error("unresolved " + (prefixed ? "id '" : "reference '")
                        + r.Value + "' at " + r.Path + contextSuffix);
                }
            }

            // Plan 144 (Workstream 144D): one executable quest definition per id
            // across the whole corpus. Registry-only acknowledgement stays legal.
            CheckDuplicateExecutableQuestDefinitions(jsonFiles, files, ctx, report);

            // Plan 45 / F15: Patrol encounter specific integrity validation
            string travelPath = Path.Combine(dataDirectory, "travel_encounters.json");
            if (files.FileExists(travelPath))
            {
                try
                {
                    string travelJson = files.ReadAllText(travelPath);
                    var factionIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    var itemIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    foreach (var key in ctx.Registry.Keys)
                    {
                        var paths = ctx.Registry[key];
                        bool isItem = key.StartsWith("item_", StringComparison.OrdinalIgnoreCase);
                        bool isFaction = key.StartsWith("faction_", StringComparison.OrdinalIgnoreCase);
                        for (int i = 0; i < paths.Count && (!isItem || !isFaction); i++)
                        {
                            string p = paths[i];
                            if (!isItem && p.StartsWith("items.json", StringComparison.OrdinalIgnoreCase)) isItem = true;
                            if (!isFaction && (p.StartsWith("faction_lore.json", StringComparison.OrdinalIgnoreCase) || p.StartsWith("factions.json", StringComparison.OrdinalIgnoreCase))) isFaction = true;
                        }
                        if (isItem) itemIds.Add(key);
                        if (isFaction) factionIds.Add(key);
                    }
                    factionIds.Add("iron_garrison");
                    factionIds.Add("ash_militia");
                    factionIds.Add("cult_of_ash_sign");
                    factionIds.Add("warlords_sector_4");
                    factionIds.Add("military_remnants");
                    factionIds.Add("upland_militia");

                    var patrolErrors = Ashfall.Core.Narrative.PatrolEncounterValidator.ValidateJson(travelJson, factionIds, itemIds);
                    foreach (var err in patrolErrors)
                    {
                        report.Error(err);
                    }
                }
                catch (Exception ex)
                {
                    report.Error("patrol encounter validator error: " + ex.Message);
                }
            }

            // Plan 48 / F7: Weather route gate integrity validation
            string weatherGatePath = Path.Combine(dataDirectory, "weather_route_gates.json");
            if (files.FileExists(weatherGatePath))
            {
                try
                {
                    string gateJson = files.ReadAllText(weatherGatePath);
                    var gateCatalog = Ashfall.Core.World.WeatherGateCatalogLoader.LoadFromJson(gateJson);
                    foreach (var err in gateCatalog.Errors)
                    {
                        report.Error("weather_route_gates.json: " + err);
                    }

                    var routeResolver = new Ashfall.Core.World.RouteGateContextResolver();
                    foreach (var gate in gateCatalog.GetAll())
                    {
                        // Target resolution
                        if (!string.IsNullOrWhiteSpace(gate.TargetId))
                        {
                            bool resolved = ctx.Registry.ContainsKey(gate.TargetId);
                            if (!resolved && string.Equals(gate.GateType, "route", StringComparison.OrdinalIgnoreCase))
                            {
                                resolved = routeResolver.IsRegisteredRoute(gate.TargetId);
                            }

                            if (!resolved)
                            {
                                report.Error($"weather_route_gates.json: gate '{gate.Id}' has unresolved target '{gate.TargetId}' (gate_type '{gate.GateType}')");
                            }
                        }

                        // Override item resolution
                        if (!string.IsNullOrWhiteSpace(gate.OverrideItem))
                        {
                            if (!ctx.Registry.ContainsKey(gate.OverrideItem))
                            {
                                report.Error($"weather_route_gates.json: gate '{gate.Id}' has unresolved override_item '{gate.OverrideItem}'");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    report.Error("weather route gate validator error: " + ex.Message);
                }
            }

            // C2 / Plan 20B: shelter shielding coefficients — ranges and
            // finiteness validated; every contributor coefficient must be
            // authored (no silent scaling defaults).
            string shelterShieldingPath = Path.Combine(dataDirectory, "shelter_shielding.json");
            if (files.FileExists(shelterShieldingPath))
            {
                try
                {
                    var shieldCatalog = Ashfall.Core.Shelter.ShelterShieldingCatalog
                        .LoadFromDirectory(dataDirectory, files);
                    foreach (var err in shieldCatalog.Errors)
                        report.Error(err);
                }
                catch (Exception ex)
                {
                    report.Error("shelter shielding validator error: " + ex.Message);
                }
            }

            // C2 / Plan 20A (G1): weather-effects authority validation — every
            // WeatherKind needs an explicit row; unknown kinds, duplicates,
            // and negative/non-finite modifiers are load errors.
            string weatherEffectsPath = Path.Combine(dataDirectory, "weather_effects.json");
            if (files.FileExists(weatherEffectsPath))
            {
                try
                {
                    var effectsCatalog = Ashfall.Core.World.WeatherEffectsCatalog.LoadFromDirectory(dataDirectory, files);
                    foreach (var err in effectsCatalog.Errors)
                        report.Error("weather_effects.json: " + err);
                    foreach (var missing in effectsCatalog.MissingKinds())
                        report.Error($"weather_effects.json: no explicit effects row for weather kind '{missing}' (no silent defaults)");
                }
                catch (Exception ex)
                {
                    report.Error("weather effects validator error: " + ex.Message);
                }
            }

            // Plan 135: Narrative discovery manifest integrity validation
            string narrativeManifestPath = Path.Combine(dataDirectory, "narrative_discovery_manifest.json");
            if (files.FileExists(narrativeManifestPath))
            {
                try
                {
                    string manifestJson = files.ReadAllText(narrativeManifestPath);
                    using var manifestDoc = JsonDocument.Parse(manifestJson);
                    if (manifestDoc.RootElement.TryGetProperty("entries", out var entriesProp) && entriesProp.ValueKind == JsonValueKind.Array)
                    {
                        if (manifestDoc.RootElement.TryGetProperty("schema_version", out var schemaProp)
                            && schemaProp.ValueKind == JsonValueKind.Number
                            && schemaProp.TryGetInt32(out int schemaVersion)
                            && schemaVersion != 1)
                        {
                            report.Error($"narrative_discovery_manifest.json: unsupported schema_version {schemaVersion}");
                        }

                        var sourceCatalogCache = new Dictionary<string, HashSet<string>>(StringComparer.Ordinal);
                        var manifestIds = new HashSet<string>(StringComparer.Ordinal);
                        foreach (var manifestEntry in entriesProp.EnumerateArray())
                        {
                            if (manifestEntry.TryGetProperty("discovery_id", out var idProp)
                                && idProp.ValueKind == JsonValueKind.String
                                && !string.IsNullOrEmpty(idProp.GetString()))
                            {
                                manifestIds.Add(idProp.GetString()!);
                            }
                        }
                        var seenManifestIds = new HashSet<string>(StringComparer.Ordinal);
                        int entryIndex = 0;
                        foreach (var entryElem in entriesProp.EnumerateArray())
                        {
                            string entryPath = $"narrative_discovery_manifest.json/entries[{entryIndex}]";
                            string discId = entryElem.TryGetProperty("discovery_id", out var dIdProp) ? dIdProp.GetString() ?? "" : "";
                            string sourceCatalog = entryElem.TryGetProperty("source_catalog", out var scProp) ? scProp.GetString() ?? "" : "";
                            string sourceRecordId = entryElem.TryGetProperty("source_record_id", out var sriProp) ? sriProp.GetString() ?? "" : "";
                            string producerId = entryElem.TryGetProperty("producer_id", out var piProp) ? piProp.GetString() ?? "" : "";
                            string truthClass = entryElem.TryGetProperty("truth_class", out var tcProp) ? tcProp.GetString() ?? "" : "";

                            if (string.IsNullOrEmpty(discId))
                            {
                                report.Error($"{entryPath}: missing or empty discovery_id");
                            }
                            else if (!seenManifestIds.Add(discId))
                            {
                                report.Error($"{entryPath}: duplicate discovery_id '{discId}'");
                            }

                            if (entryElem.TryGetProperty("related_discovery_ids", out var relatedProp)
                                && relatedProp.ValueKind != JsonValueKind.Array)
                            {
                                report.Error($"{entryPath}: related_discovery_ids must be an array");
                            }
                            else if (relatedProp.ValueKind == JsonValueKind.Array)
                            {
                                foreach (var related in relatedProp.EnumerateArray())
                                {
                                    string relatedId = related.ValueKind == JsonValueKind.String
                                        ? related.GetString() ?? ""
                                        : "";
                                    if (string.IsNullOrEmpty(relatedId) || !manifestIds.Contains(relatedId))
                                    {
                                        report.Error($"{entryPath}: related_discovery_id '{relatedId}' is not present in the manifest");
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(truthClass)
                                && !FringeCultRuntimeContract.IsValidTruthClass(truthClass))
                            {
                                report.Error($"{entryPath}: unsupported truth_class '{truthClass}'");
                            }

                            if (string.IsNullOrEmpty(sourceCatalog))
                            {
                                report.Error($"{entryPath}: missing or empty source_catalog");
                            }
                            else
                            {
                                string fullCatalogPath = Path.Combine(dataDirectory, sourceCatalog);
                                if (!files.FileExists(fullCatalogPath))
                                {
                                    report.Error($"{entryPath}: source_catalog '{sourceCatalog}' does not exist on disk");
                                }
                                else if (!string.IsNullOrEmpty(sourceRecordId))
                                {
                                    if (!sourceCatalogCache.TryGetValue(sourceCatalog, out var catalogIds))
                                    {
                                        catalogIds = new HashSet<string>(StringComparer.Ordinal);
                                        try
                                        {
                                            string catJson = files.ReadAllText(fullCatalogPath);
                                            using var catDoc = JsonDocument.Parse(catJson);
                                            CollectCatalogIds(catDoc.RootElement, catalogIds);
                                        }
                                        catch (Exception ex)
                                        {
                                            report.Error($"{entryPath}: failed to parse source_catalog '{sourceCatalog}': {ex.Message}");
                                        }
                                        sourceCatalogCache[sourceCatalog] = catalogIds;
                                    }

                                    if (!catalogIds.Contains(sourceRecordId))
                                    {
                                        report.Error($"{entryPath}: source_record_id '{sourceRecordId}' not found in '{sourceCatalog}'");
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(producerId))
                            {
                                if (!ctx.Registry.ContainsKey(producerId) && !IsKnownRuntimeId(producerId))
                                {
                                    report.Error($"{entryPath}: producer_id '{producerId}' is not registered");
                                }
                            }

                            entryIndex++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    report.Error("narrative discovery manifest validator error: " + ex.Message);
                }
            }

            // Wildlife trapping catalog integrity validation:
            // Trap identity, bait reachability, compatibility matrix completeness, domain specializations.
            ValidateWildlifeTrappingCatalog(dataDirectory, files, ctx, report);

            // Tasks 9–12 Wave 1: distress-signal stage contract validation
            // (message_fragments ordering, clarity monotonicity, text/hint presence).
            ValidateDistressSignalStages(dataDirectory, files, report);

            // Plan 14A (C1): trade embargo rule integrity — loader contract,
            // real WeatherKind/region/category vocabularies, goods-id resolution.
            ValidateTradeEmbargoRules(dataDirectory, files, report);

            // Plan 14B (C1): regional price atlas integrity — loader contract,
            // goods-id resolution, region/category vocabularies, duplicate rows.
            ValidateRegionalPriceAtlas(dataDirectory, files, report);

            report.AuthoredIds = ctx.Authored;
            report.ReuseCount = ctx.Reuse;

            return report;
        }

        private static bool TryParse(string path, IFileIO files, out JsonDocument doc, CatalogIntegrityReport report)
        {
            doc = null!;
            try
            {
                string text = files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(text))
                {
                    report.Error("catalog '" + path + "': empty JSON file (no catalog records can be trusted)");
                    return false;
                }
                doc = JsonDocument.Parse(text);
                return true;
            }
            catch (JsonException ex)
            {
                report.Error("catalog '" + path + "': malformed JSON (" + ex.GetType().Name + "): " + ex.Message);
                return false;
            }
            catch (IOException ex)
            {
                report.Error("catalog '" + path + "': I/O failure (" + ex.GetType().Name + "): " + ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                report.Error("catalog '" + path + "': catalog read failure (" + ex.GetType().Name + "): " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Authority rule: every catalog whose root is a JSON object must declare a
        /// top-level <c>schema_version</c>. A catalog added without it silently
        /// bypasses the migration ladder (a V1 loader would never see a V2 field),
        /// so the integrity selftest must fail the build rather than load
        /// unversioned data. Bare-array roots (legacy list catalogs) are exempt:
        /// they carry no per-file metadata slot.
        /// </summary>
        private static void RequireSchemaVersion(JsonElement root, string file, CatalogIntegrityReport report)
        {
            if (root.ValueKind != JsonValueKind.Object) return;
            if (!root.TryGetProperty("schema_version", out _))
                report.Error("catalog '" + file + "': missing required top-level 'schema_version'");
        }

        private static void Walk(JsonElement element, string path, Ctx ctx, string? parentEntityDesc = null)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    string? entityDesc = null;
                    if (path.Contains("bycatchSpecies"))
                    {
                        entityDesc = parentEntityDesc;
                    }
                    else if (element.TryGetProperty("speciesId", out var spProp) && spProp.ValueKind == JsonValueKind.String && !string.IsNullOrEmpty(spProp.GetString()))
                    {
                        entityDesc = "prey '" + spProp.GetString() + "'";
                    }
                    else if (element.TryGetProperty("trap_id", out var trProp) && trProp.ValueKind == JsonValueKind.String && !string.IsNullOrEmpty(trProp.GetString()))
                    {
                        entityDesc = "trap '" + trProp.GetString() + "'";
                    }
                    else if (element.TryGetProperty("id", out var idProp) && idProp.ValueKind == JsonValueKind.String && !string.IsNullOrEmpty(idProp.GetString()))
                    {
                        entityDesc = "entity '" + idProp.GetString() + "'";
                    }
                    else
                    {
                        entityDesc = parentEntityDesc;
                    }

                    foreach (JsonProperty property in element.EnumerateObject())
                    {
                        string childPath = path + "/" + property.Name;
                        JsonElement value = property.Value;

                        if (property.Name == "contaminationDose")
                        {
                            ValidateContaminationDose(value, childPath, entityDesc, ctx);
                        }

                        if (property.Name == "bycatchChance")
                        {
                            ValidateBycatchChance(value, childPath, entityDesc, ctx);
                        }

                        if (property.Name == "speciesId" && value.ValueKind == JsonValueKind.String)
                        {
                            string? spId = value.GetString();
                            if (!string.IsNullOrEmpty(spId))
                            {
                                if (childPath.Contains("bycatchSpecies"))
                                {
                                    ctx.PendingRefs.Add(new Ref
                                    {
                                        Value = spId,
                                        Path = childPath,
                                        Strict = true,
                                        EntityContext = entityDesc ?? parentEntityDesc
                                    });
                                }
                                else
                                {
                                    Register(property.Name, spId, childPath, ctx);
                                }
                            }
                            continue;
                        }

                        if (value.ValueKind == JsonValueKind.String)
                        {
                            string? text = value.GetString();
                            if (string.IsNullOrEmpty(text) || IsVocabularyKey(property.Name)) continue;
                            RegisterOrReference(property.Name, text, childPath, entityDesc, ctx);
                            continue;
                        }

                        if (value.ValueKind == JsonValueKind.Array)
                        {
                            // Vocabulary arrays (tags, category, ...) are never
                            // cross-referenced: skip them wholesale.
                            if (IsVocabularyKey(property.Name)) continue;

                            if (IsDefinitionKey(property.Name))
                            {
                                // Array-valued definition keys (traits, baseTraits):
                                // each element is a definition, not a reference.
                                foreach (JsonElement item in value.EnumerateArray())
                                {
                                    if (item.ValueKind == JsonValueKind.String && !string.IsNullOrEmpty(item.GetString()))
                                        Register(property.Name, item.GetString()!, childPath + "[]", ctx);
                                }
                            }

                            if (IsReferenceKey(property.Name))
                            {
                                foreach (JsonElement item in value.EnumerateArray())
                                {
                                    if (item.ValueKind == JsonValueKind.String && !string.IsNullOrEmpty(item.GetString()))
                                        ctx.PendingRefs.Add(new Ref
                                        {
                                            Value = item.GetString()!,
                                            Path = childPath + "[]",
                                            Strict = true,
                                            EntityContext = entityDesc
                                        });
                                }
                            }
                        }

                        if (value.ValueKind == JsonValueKind.Array && property.Name == "routes")
                        {
                            var seenFileRoutes = new HashSet<string>(StringComparer.Ordinal);
                            int routeIdx = 0;
                            foreach (JsonElement routeElem in value.EnumerateArray())
                            {
                                string rPath = childPath + "[" + routeIdx + "]";
                                if (routeElem.ValueKind == JsonValueKind.Object)
                                {
                                    string? rFrom = null;
                                    string? rTo = null;
                                    float? dist = null;

                                    if (routeElem.TryGetProperty("from", out var fProp) && fProp.ValueKind == JsonValueKind.String)
                                        rFrom = fProp.GetString();
                                    if (routeElem.TryGetProperty("to", out var tProp) && tProp.ValueKind == JsonValueKind.String)
                                        rTo = tProp.GetString();
                                    if (routeElem.TryGetProperty("distanceKm", out var dProp) && dProp.TryGetSingle(out float dVal))
                                        dist = dVal;

                                    if (!string.IsNullOrEmpty(rFrom) && !string.IsNullOrEmpty(rTo))
                                    {
                                        if (string.Equals(rFrom, rTo, StringComparison.Ordinal))
                                        {
                                            ctx.Report.Error("self-route detected at " + rPath + ": '" + rFrom + "' -> '" + rTo + "'");
                                        }
                                        string rKey = rFrom + "->" + rTo;
                                        if (!seenFileRoutes.Add(rKey))
                                        {
                                            ctx.Report.Error("duplicate route '" + rKey + "' at " + rPath);
                                        }
                                    }

                                    if (dist.HasValue && dist.Value <= 0f)
                                    {
                                        ctx.Report.Error("negative or zero distance (" + dist.Value + ") at " + rPath);
                                    }
                                }
                                routeIdx++;
                            }
                        }

                        if (value.ValueKind == JsonValueKind.Number
                            && IsRangeKey(property.Name)
                            && TryGetInt(value, out int rangeVal))
                        {
                            CheckRange(property.Name, rangeVal, childPath, ctx);
                        }

                        Walk(value, childPath, ctx, entityDesc ?? parentEntityDesc);
                    }
                    break;

                case JsonValueKind.Array:
                    int index = 0;
                    foreach (JsonElement item in element.EnumerateArray())
                    {
                        Walk(item, path + "[" + index + "]", ctx, parentEntityDesc);
                        index++;
                    }
                    break;

                case JsonValueKind.String:
                    string? s = element.GetString();
                    if (!string.IsNullOrEmpty(s))
                        ctx.PendingRefs.Add(new Ref { Value = s, Path = path, Strict = false });
                    break;
            }
        }

        private static void RegisterOrReference(string key, string value, string path, string? entityContext, Ctx ctx)
        {
            // Plan 144 (Workstream 144B): prefix-grammar tokens are patterns,
            // not ids and not references. They must never be registered (a
            // pattern must not bless itself) and never enter the Tier-1
            // foreign-key set. Shape-check instead.
            if (IsPrefixPatternKey(key))
            {
                ValidatePrefixPattern(key, value, path, ctx);
                return;
            }

            if (IsDefinitionKey(key))
            {
                Register(key, value, path, ctx);
            }
            if (IsReferenceKey(key))
            {
                ctx.PendingRefs.Add(new Ref { Value = value, Path = path, Strict = true, EntityContext = entityContext });
            }
            else if (!IsDefinitionKey(key) && StartsWithAny(value, IdPrefixes))
            {
                // Any prefixed string in a non-id position is still a reference
                // (Tier 1) — e.g. a narrative field naming an item id.
                ctx.PendingRefs.Add(new Ref { Value = value, Path = path, Strict = false, EntityContext = entityContext });
            }
        }

        private static bool IsPrefixPatternKey(string key)
        {
            for (int i = 0; i < PrefixPatternKeys.Length; i++)
                if (string.Equals(PrefixPatternKeys[i], key, StringComparison.Ordinal)) return true;
            return false;
        }

        /// <summary>Plan 144: a prefix-pattern value must be a non-empty
        /// trailing-underscore token rooted in a known id namespace — a grammar
        /// element, never a concrete id.</summary>
        private static void ValidatePrefixPattern(string key, string value, string path, Ctx ctx)
        {
            if (!string.IsNullOrEmpty(value)
                && value.EndsWith("_", StringComparison.Ordinal)
                && StartsWithAny(value, IdPrefixes))
                return;
            ctx.Report.Error("prefix-pattern key '" + key + "' value '" + value + "' at " + path
                + " must be a trailing-underscore id-namespace prefix (not a concrete id)");
        }

        /// <summary>
        /// Plan 144 (Workstream 144D): a quest id carrying playable grammar
        /// (choices/stages/objectives/steps) in TWO distinct files is a
        /// duplicate executable definition — a blocking error regardless of
        /// whether the two bodies agree. Identity-only acknowledgement
        /// (questline_master.json-style rows with no playable grammar) remains
        /// legal next to the single executable definition. Deterministic:
        /// ordinal-sorted file list + ordinal-sorted per-id file sets.
        /// </summary>
        private static void CheckDuplicateExecutableQuestDefinitions(
            string[] jsonFiles, IFileIO files, Ctx ctx, CatalogIntegrityReport report)
        {
            // leaf → full path; ambiguous leaves (same name in two directories)
            // are skipped conservatively.
            var fileByLeaf = new Dictionary<string, string>(StringComparer.Ordinal);
            var ambiguousLeaves = new HashSet<string>(StringComparer.Ordinal);
            foreach (string file in jsonFiles)
            {
                string leaf = file.StartsWith("res://", StringComparison.Ordinal)
                    ? file.Substring(file.LastIndexOf('/') + 1)
                    : Path.GetFileName(file);
                if (fileByLeaf.TryGetValue(leaf, out string? seen) && !string.Equals(seen, file, StringComparison.Ordinal))
                    ambiguousLeaves.Add(leaf);
                else
                    fileByLeaf[leaf] = file;
            }

            // quest id → leaf → first entity-row definition site (container, index)
            var sites = new SortedDictionary<string, SortedDictionary<string, (string container, int index)>>(
                StringComparer.Ordinal);
            foreach (var kv in ctx.Registry)
            {
                if (!kv.Key.StartsWith("quest_", StringComparison.Ordinal)) continue;
                foreach (string p in kv.Value)
                {
                    if (!p.EndsWith("/id", StringComparison.Ordinal)) continue;
                    string leaf = FileLeaf(p);
                    if (ambiguousLeaves.Contains(leaf)) continue;
                    string rest = p.Substring(leaf.Length).TrimStart('/');
                    if (!rest.EndsWith("/id", StringComparison.Ordinal)) continue;
                    rest = rest.Substring(0, rest.Length - 3); // "quests[3]" or "[3]"
                    int open = rest.LastIndexOf('[');
                    if (open < 0) continue;
                    string container = rest.Substring(0, open);
                    string indexText = rest.Substring(open + 1).TrimEnd(']');
                    if (!int.TryParse(indexText, out int index)) continue;
                    if (!sites.TryGetValue(kv.Key, out var perFile))
                        sites[kv.Key] = perFile = new SortedDictionary<string, (string, int)>(StringComparer.Ordinal);
                    if (!perFile.ContainsKey(leaf))
                        perFile[leaf] = (container, index);
                }
            }

            foreach (var kv in sites)
            {
                if (kv.Value.Count < 2) continue;
                var executable = new List<string>();
                foreach (var site in kv.Value)
                {
                    if (!fileByLeaf.TryGetValue(site.Key, out string? fullPath)) continue;
                    if (QuestRowIsExecutable(fullPath, site.Value.container, site.Value.index, files, report))
                        executable.Add(site.Key);
                }
                if (executable.Count >= 2)
                {
                    report.Error("duplicate executable quest definition '" + kv.Key
                        + "' defined in " + executable[0] + " and " + executable[1]
                        + " — one quest id must have exactly one executable definition (Plan 144)");
                }
            }
        }

        /// <summary>True when the quest row at (container[index]) of the file
        /// carries playable-quest grammar. Conservative on any read/shape
        /// failure (no classification), with the failure reported.</summary>
        private static bool QuestRowIsExecutable(string fullPath, string container, int index, IFileIO files, CatalogIntegrityReport report)
        {
            try
            {
                string raw = files.ReadAllText(fullPath);
                if (string.IsNullOrWhiteSpace(raw)) return false;
                using var doc = JsonDocument.Parse(raw);
                JsonElement row;
                if (string.IsNullOrEmpty(container))
                {
                    if (doc.RootElement.ValueKind != JsonValueKind.Array || doc.RootElement.GetArrayLength() <= index)
                        return false;
                    row = doc.RootElement[index];
                }
                else
                {
                    if (doc.RootElement.ValueKind != JsonValueKind.Object
                        || !doc.RootElement.TryGetProperty(container, out var arr)
                        || arr.ValueKind != JsonValueKind.Array
                        || arr.GetArrayLength() <= index)
                        return false;
                    row = arr[index];
                }
                if (row.ValueKind != JsonValueKind.Object) return false;
                foreach (string marker in QuestExecutableMarkers)
                    if (row.TryGetProperty(marker, out _)) return true;
                return false;
            }
            catch (Exception ex)
            {
                report.Error("Plan 144 duplicate-quest gate could not read '" + fullPath
                    + "' (classified as non-executable): " + ex.Message);
                return false;
            }
        }

        private static void Register(string key, string value, string path, Ctx ctx)
        {
            if (ctx.Registry.TryGetValue(value, out List<string>? existing))
            {
                // The id already has an author. Distinguish a GENUINE within-file
                // entity-id conflict from legitimate id reuse:
                //   • A conflict is a literal `id` registered at entity-root depth
                //     (file.json[N]/id) twice in the SAME file — i.e. two rows of
                //     one catalog claim the same identity (Invariant 6).
                //   • Everything else is reuse: shared stage/choice templates
                //     (stages[N]/id), enrichment *—fields/*—tags foreign keys, and
                //     per-container row rewrites (npcs[i]/id). These are normal
                //     composition across an id's single authority — not an error.
                string firstPath = existing[0];
                if (key == "id"
                    && IsEntityRootId(path)
                    && IsEntityRootId(firstPath)
                    && FileLeaf(path) == FileLeaf(firstPath))
                    ctx.Report.Error("duplicate id '" + value + "' defined at " + path
                        + " (first: " + firstPath + ")");
                else
                    ctx.Reuse++;
            }
            else
            {
                existing = new List<string>();
                ctx.Registry[value] = existing;
                ctx.Authored++;
            }
            existing.Add(path);
        }

        /// <summary>An id is at entity-root depth when its path has exactly one
        /// slash: file.json[N]/id. Deeper paths (stages[N]/id, choices[N]/id,
        /// npcs[i]/id, entries[N]/…/id) are nested template/container ids that
        /// participate in reuse, not entity-root authorship conflicts.</summary>
        private static bool IsEntityRootId(string path)
        {
            int slashes = 0;
            for (int i = 0; i < path.Length; i++)
                if (path[i] == '/') slashes++;
            return slashes == 1;
        }

        /// <summary>Extract the JSON catalog leaf name (strip array indices and
        /// any nested path) so same-file detection compares the actual file.
        /// "a.json[0]/nested[1]/id" → "a.json".</summary>
        private static string FileLeaf(string path)
        {
            int slash = path.IndexOf('/');
            int bracket = path.IndexOf('[');
            int end = int.MaxValue;
            if (slash >= 0 && slash < end) end = slash;
            if (bracket >= 0 && bracket < end) end = bracket;
            return end == int.MaxValue ? path : path.Substring(0, end);
        }

        private static void CheckRange(string key, int value, string parentPath, Ctx ctx)
        {
            // Memoised per parent object: the min/max pair is checked when both
            // siblings have been seen.
            if (!ctx.RangeMemo.TryGetValue(parentPath, out RangeMemoEntry? memo))
            {
                memo = new RangeMemoEntry();
                ctx.RangeMemo[parentPath] = memo;
            }

            if (key.ToLowerInvariant().Contains("min"))
                memo.Min = value;
            else
                memo.Max = value;

            if (memo.Min.HasValue && memo.Max.HasValue && memo.Min.Value > memo.Max.Value)
                ctx.Report.Error("range inverted at " + parentPath + ": min " + memo.Min.Value
                    + " > max " + memo.Max.Value);
        }

        /// <summary>
        /// Workstream B: Validates contaminationDose authored bounds.
        /// Contract:
        /// - Finite and non-negative required (NaN, +Inf, -Inf, < 0 are content errors).
        /// - 0 is valid (runtime tier fallback).
        /// - > 0 and <= AcuteThreshold is valid.
        /// - > AcuteThreshold emits a warning (high exposure content is intentional/legal).
        /// Does not mutate or clamp data.
        /// </summary>
        private static void ValidateContaminationDose(JsonElement value, string childPath, string? entityContext, Ctx ctx)
        {
            string contextSuffix = !string.IsNullOrEmpty(entityContext) ? " (" + entityContext + ")" : string.Empty;
            if (value.ValueKind == JsonValueKind.Number)
            {
                if (value.TryGetDouble(out double dVal))
                {
                    if (double.IsNaN(dVal) || double.IsInfinity(dVal))
                    {
                        ctx.Report.Error($"catalog '{ctx.File}': non-finite contaminationDose ({dVal}) at {childPath}{contextSuffix}");
                    }
                    else if (dVal < 0.0)
                    {
                        ctx.Report.Error($"catalog '{ctx.File}': negative contaminationDose ({dVal}) at {childPath}{contextSuffix}");
                    }
                    else if (dVal > RadiationSystem.AcuteThreshold)
                    {
                        ctx.Report.Warn($"catalog '{ctx.File}': single exposure contaminationDose ({dVal} rads) exceeds acute threshold ({RadiationSystem.AcuteThreshold} rads) at {childPath}{contextSuffix}");
                    }
                }
                else
                {
                    ctx.Report.Error($"catalog '{ctx.File}': unparseable numeric contaminationDose at {childPath}{contextSuffix}");
                }
            }
            else if (value.ValueKind == JsonValueKind.String)
            {
                string? str = value.GetString();
                if (string.Equals(str, "PositiveInfinity", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(str, "+Infinity", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(str, "NegativeInfinity", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(str, "-Infinity", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(str, "Infinity", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(str, "NaN", StringComparison.OrdinalIgnoreCase))
                {
                    ctx.Report.Error($"catalog '{ctx.File}': non-finite contaminationDose ({str}) at {childPath}{contextSuffix}");
                }
                else if (double.TryParse(str, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double parsed))
                {
                    if (double.IsNaN(parsed) || double.IsInfinity(parsed))
                    {
                        ctx.Report.Error($"catalog '{ctx.File}': non-finite contaminationDose ({str}) at {childPath}{contextSuffix}");
                    }
                    else if (parsed < 0.0)
                    {
                        ctx.Report.Error($"catalog '{ctx.File}': negative contaminationDose ({parsed}) at {childPath}{contextSuffix}");
                    }
                    else if (parsed > RadiationSystem.AcuteThreshold)
                    {
                        ctx.Report.Warn($"catalog '{ctx.File}': single exposure contaminationDose ({parsed} rads) exceeds acute threshold ({RadiationSystem.AcuteThreshold} rads) at {childPath}{contextSuffix}");
                    }
                }
                else
                {
                    ctx.Report.Error($"catalog '{ctx.File}': non-numeric contaminationDose '{str}' at {childPath}{contextSuffix}");
                }
            }
            else
            {
                ctx.Report.Error($"catalog '{ctx.File}': invalid contaminationDose format ({value.ValueKind}) at {childPath}{contextSuffix}");
            }
        }

        private static void ValidateBycatchChance(JsonElement value, string childPath, string? entityContext, Ctx ctx)
        {
            string contextSuffix = !string.IsNullOrEmpty(entityContext) ? " (" + entityContext + ")" : string.Empty;
            if (value.ValueKind == JsonValueKind.Number)
            {
                if (value.TryGetDouble(out double bc))
                {
                    if (!double.IsFinite(bc) || bc < 0.0 || bc > 1.0)
                        ctx.Report.Error($"catalog '{ctx.File}': bycatchChance out of range ({bc}) at {childPath}{contextSuffix}");
                }
            }
            else if (value.ValueKind == JsonValueKind.String)
            {
                string? sVal = value.GetString();
                if (double.TryParse(sVal, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double parsedBc))
                {
                    if (!double.IsFinite(parsedBc) || parsedBc < 0.0 || parsedBc > 1.0)
                        ctx.Report.Error($"catalog '{ctx.File}': bycatchChance out of range ({parsedBc}) at {childPath}{contextSuffix}");
                }
                else
                {
                    ctx.Report.Error($"catalog '{ctx.File}': non-numeric bycatchChance '{sVal}' at {childPath}{contextSuffix}");
                }
            }
        }

        private static void ValidateProbability(
            JsonElement value, string path, string propertyName, CatalogIntegrityReport report)
        {
            if (value.ValueKind != JsonValueKind.Number
                || !value.TryGetDouble(out double probability)
                || !double.IsFinite(probability)
                || probability < 0d
                || probability > 1d)
            {
                report.Error($"wildlife_trapping_catalog.json: {propertyName} must be a finite number in [0,1] at {path}");
            }
        }

        private sealed class RangeMemoEntry
        {
            public int? Min;
            public int? Max;
        }

        private static bool TryGetInt(JsonElement element, out int value)
        {
            value = 0;
            if (element.TryGetInt32(out value)) return true;
            if (element.TryGetInt64(out long l) && l >= int.MinValue && l <= int.MaxValue)
            {
                value = (int)l;
                return true;
            }
            return false;
        }

        private static bool IsVocabularyKey(string key) =>
            Array.IndexOf(VocabularyKeys, key) >= 0;

        private static bool IsKnownRuntimeId(string value) =>
            Array.IndexOf(KnownRuntimeIds, value) >= 0;

        private static bool IsDefinitionKey(string key) =>
            Array.IndexOf(DefinitionKeys, key) >= 0;

        private static bool IsReferenceKey(string key) =>
            Array.IndexOf(ReferenceKeys, key) >= 0;

        private static bool IsRangeKey(string key) =>
            Array.IndexOf(RangeKeys, key) >= 0;

        private static bool StartsWithAny(string value, string[] prefixes)
        {
            for (int i = 0; i < prefixes.Length; i++)
                if (value.StartsWith(prefixes[i], StringComparison.Ordinal))
                    return true;
            return false;
        }

        private static void CollectCatalogIds(JsonElement element, HashSet<string> ids)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    foreach (JsonProperty prop in element.EnumerateObject())
                    {
                        if (prop.Value.ValueKind == JsonValueKind.String)
                        {
                            string? str = prop.Value.GetString();
                            if (!string.IsNullOrEmpty(str))
                            {
                                if (prop.Name == "id" || prop.Name == "glitch_id" || prop.Name == "room_id" ||
                                    prop.Name == "case_id" || prop.Name == "confession_id" || prop.Name == "tx_id" ||
                                    prop.Name == "treaty_id" || prop.Name == "directive_id" || prop.Name == "dispatch_id" ||
                                    prop.Name == "report_id" || prop.Name == "audit_id" || prop.Name == "letter_id")
                                {
                                    ids.Add(str);
                                }
                            }
                        }
                        else if (prop.Value.ValueKind == JsonValueKind.Object || prop.Value.ValueKind == JsonValueKind.Array)
                        {
                            CollectCatalogIds(prop.Value, ids);
                        }
                    }
                    break;
                case JsonValueKind.Array:
                    foreach (JsonElement item in element.EnumerateArray())
                    {
                        CollectCatalogIds(item, ids);
                    }
                    break;
            }
        }

        // ── Tasks 9–12 Wave 1: distress-signal stage contract ──────────────────

        /// <summary>
        /// Stage-contract validation for the distress-signal catalogs
        /// (Tasks 9–12 Wave 1). Enforces the documented <c>message_fragments</c>
        /// stage contract: strictly ascending stage days, non-decreasing clarity
        /// in [0,1], non-empty stage text, present-or-empty outcome hints, and
        /// signal-identity rules. Cross-file duplicate IDs are the documented
        /// primary-wins override pattern (expansion loads first, primary Plan 50
        /// authority loads last — see RadioHostSession) and are reported as
        /// warnings, never gate failures.
        /// </summary>
        public static void ValidateDistressSignalStages(string dataDirectory, IFileIO files, CatalogIntegrityReport report)
        {
            var seenIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var primaryIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var seenFollowUpIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // Pass 1: primary authority first so cross-file duplicates in the
            // expansion layer are attributed as overridden secondary rows.
            ValidateDistressSignalStagesFile(dataDirectory, files, "radio_distress_signals.json", seenIds, primaryIds, seenFollowUpIds, report);
            ValidateDistressSignalStagesFile(dataDirectory, files, "radio_distress_signals_expansion.json", seenIds, primaryIds, seenFollowUpIds, report);
        }

        private static void ValidateDistressSignalStagesFile(
            string dataDirectory,
            IFileIO files,
            string fileName,
            HashSet<string> seenIds,
            HashSet<string> primaryIds,
            HashSet<string> seenFollowUpIds,
            CatalogIntegrityReport report)
        {
            string path = Path.Combine(dataDirectory, fileName);
            if (!files.FileExists(path)) return;

            try
            {
                string json = files.ReadAllText(path);
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;
                if (!root.TryGetProperty("radio_broadcasts", out var broadcasts)
                    || broadcasts.ValueKind != JsonValueKind.Array)
                {
                    return;
                }

                bool isPrimary = string.Equals(fileName, "radio_distress_signals.json", StringComparison.Ordinal);
                int broadcastIndex = 0;
                foreach (var broadcast in broadcasts.EnumerateArray())
                {
                    string signalId = broadcast.TryGetProperty("frequency_id", out var idProp)
                                      && idProp.ValueKind == JsonValueKind.String
                        ? idProp.GetString() ?? string.Empty
                        : string.Empty;
                    string label = $"{fileName}:radio_broadcasts[{broadcastIndex}]";
                    broadcastIndex++;

                    if (string.IsNullOrWhiteSpace(signalId))
                    {
                        report.Error($"{label}: missing or empty 'frequency_id'");
                        continue;
                    }

                    if (!seenIds.Add(signalId))
                    {
                        if (isPrimary)
                        {
                            report.Error($"{fileName}:{signalId}: duplicate frequency_id within the primary authority");
                        }
                        else if (!primaryIds.Contains(signalId))
                        {
                            report.Error($"{fileName}:{signalId}: duplicate frequency_id within the expansion layer");
                        }
                        else
                        {
                            // Documented primary-wins override: the expansion row is
                            // dead content shadowed by the primary Plan 50 definition.
                            report.Warn($"{fileName}:{signalId}: expansion row overridden by primary authority (documented primary-wins load order)");
                        }
                    }
                    if (isPrimary) primaryIds.Add(signalId);

                    if (!broadcast.TryGetProperty("message_fragments", out var fragments)
                        || fragments.ValueKind != JsonValueKind.Array
                        || !HasAnyElement(fragments))
                    {
                        report.Error($"{fileName}:{signalId}: 'message_fragments' missing or empty — every authored distress signal requires at least one message stage");
                        continue;
                    }

                    int previousDay = int.MinValue;
                    float previousClarity = -1f;
                    int stageIndex = 0;
                    foreach (var fragment in fragments.EnumerateArray())
                    {
                        string stagePath = $"{fileName}:{signalId}.message_fragments[{stageIndex}]";

                        if (!fragment.TryGetProperty("day", out var dayProp)
                            || dayProp.ValueKind != JsonValueKind.Number
                            || !dayProp.TryGetInt32(out int day))
                        {
                            report.Error($"{stagePath}.day: missing or non-integer stage day");
                        }
                        else if (day == previousDay)
                        {
                            report.Error($"{stagePath}.day={day}: duplicate stage day — stage days must be strictly ascending");
                        }
                        else if (day < previousDay)
                        {
                            report.Error($"{stagePath}.day={day}: must be greater than previous stage day={previousDay}");
                        }
                        else
                        {
                            previousDay = day;
                        }

                        if (!fragment.TryGetProperty("clarity", out var clarityProp)
                            || clarityProp.ValueKind != JsonValueKind.Number)
                        {
                            report.Error($"{stagePath}.clarity: missing or non-numeric stage clarity");
                        }
                        else
                        {
                            float clarity = (float)clarityProp.GetDouble();
                            if (clarity < 0f || clarity > 1f)
                            {
                                report.Error($"{stagePath}.clarity={clarityFormat(clarity)}: outside the valid range [0,1]");
                            }
                            else if (clarity < previousClarity)
                            {
                                report.Error($"{stagePath}.clarity={clarityFormat(clarity)}: must be greater than or equal to previous stage clarity={clarityFormat(previousClarity)}");
                            }
                            else
                            {
                                previousClarity = clarity;
                            }
                        }

                        if (!fragment.TryGetProperty("text", out var textProp)
                            || textProp.ValueKind != JsonValueKind.String
                            || string.IsNullOrWhiteSpace(textProp.GetString()))
                        {
                            report.Error($"{stagePath}.text: missing or empty stage text");
                        }

                        if (fragment.TryGetProperty("outcome_hint", out var hintProp)
                            && hintProp.ValueKind == JsonValueKind.String
                            && string.IsNullOrWhiteSpace(hintProp.GetString()))
                        {
                            report.Error($"{stagePath}.outcome_hint: present but empty — omit the field when no hint is authored at this stage");
                        }

                        // Tasks 9–12 Wave 4: structural audio-cue validation.
                        // Empty = inherit signal default / text-only (legal).
                        // Non-empty must be a clean snake_case cue id.
                        if (fragment.TryGetProperty("audio_cue", out var fragCueProp)
                            && fragCueProp.ValueKind == JsonValueKind.String)
                        {
                            string fragCue = fragCueProp.GetString() ?? string.Empty;
                            if (!IsValidAudioCueId(fragCue))
                            {
                                report.Error($"{stagePath}.audio_cue '{fragCue}': must be empty (inherit/text-only) or a lowercase snake_case cue id without whitespace");
                            }
                        }

                        stageIndex++;
                    }

                    // Tasks 9–12 Wave 3: follow-up transmission validation.
                    if (broadcast.TryGetProperty("follow_up_signals", out var followUps)
                        && followUps.ValueKind == JsonValueKind.Array)
                    {
                        int followUpIndex = 0;
                        foreach (var followUp in followUps.EnumerateArray())
                        {
                            string followUpPath = $"{fileName}:{signalId}.follow_up_signals[{followUpIndex}]";
                            followUpIndex++;

                            string followUpId = followUp.TryGetProperty("id", out var fuIdProp)
                                                && fuIdProp.ValueKind == JsonValueKind.String
                                ? fuIdProp.GetString() ?? string.Empty
                                : string.Empty;
                            if (string.IsNullOrWhiteSpace(followUpId))
                            {
                                report.Error($"{followUpPath}: missing or empty 'id' — every follow-up requires a stable dedupe identity");
                            }
                            else if (!seenFollowUpIds.Add(followUpId))
                            {
                                report.Error($"{followUpPath}: duplicate follow-up id '{followUpId}' — dedupe identities must be unique across the corpus");
                            }

                            string trigger = followUp.TryGetProperty("trigger_condition", out var trigProp)
                                             && trigProp.ValueKind == JsonValueKind.String
                                ? trigProp.GetString() ?? string.Empty
                                : string.Empty;
                            if (!Ashfall.Core.Radio.SignalFollowUpTriggers.IsValid(trigger))
                            {
                                report.Error($"{followUpPath} '{followUpId}': unsupported trigger_condition '{trigger}' — accepted: {string.Join(", ", Ashfall.Core.Radio.SignalFollowUpTriggers.All)}");
                            }

                            if (!followUp.TryGetProperty("delay_days", out var delayProp)
                                || delayProp.ValueKind != JsonValueKind.Number
                                || !delayProp.TryGetInt32(out int delayDays))
                            {
                                report.Error($"{followUpPath} '{followUpId}'.delay_days: missing or non-integer delay");
                            }
                            else if (delayDays < 0)
                            {
                                report.Error($"{followUpPath} '{followUpId}'.delay_days={delayDays}: must be greater than or equal to 0");
                            }

                            if (!followUp.TryGetProperty("text", out var fuTextProp)
                                || fuTextProp.ValueKind != JsonValueKind.String
                                || string.IsNullOrWhiteSpace(fuTextProp.GetString()))
                            {
                                report.Error($"{followUpPath} '{followUpId}'.text: missing or empty follow-up text");
                            }

                            if (followUp.TryGetProperty("clarity", out var fuClarityProp)
                                && fuClarityProp.ValueKind == JsonValueKind.Number)
                            {
                                float fuClarity = (float)fuClarityProp.GetDouble();
                                if (fuClarity < 0f || fuClarity > 1f)
                                {
                                    report.Error($"{followUpPath} '{followUpId}'.clarity={fuClarity.ToString(System.Globalization.CultureInfo.InvariantCulture)}: outside the valid range [0,1]");
                                }
                            }

                            if (followUp.TryGetProperty("outcome_hint", out var fuHintProp)
                                && fuHintProp.ValueKind == JsonValueKind.String
                                && string.IsNullOrWhiteSpace(fuHintProp.GetString()))
                            {
                                report.Error($"{followUpPath} '{followUpId}'.outcome_hint: present but empty — omit the field when no hint is authored");
                            }

                            // Tasks 9–12 Wave 4: structural audio-cue validation
                            // (same rule as stage cues).
                            if (followUp.TryGetProperty("audio_cue", out var fuCueProp)
                                && fuCueProp.ValueKind == JsonValueKind.String)
                            {
                                string fuCue = fuCueProp.GetString() ?? string.Empty;
                                if (!IsValidAudioCueId(fuCue))
                                {
                                    report.Error($"{followUpPath} '{followUpId}'.audio_cue '{fuCue}': must be empty (text-only) or a lowercase snake_case cue id without whitespace");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                report.Error($"{fileName}: distress stage validator error: {ex.Message}");
            }
        }

        // ── Plan 14A/14B (C1): economy geography integrity ────────────────

        /// <summary>
        /// Plan 14A — trade_embargoes.json must load clean through its strict
        /// loader (schema, WeatherKind/region/category vocabularies, bounds,
        /// duplicates) and every affected item id must resolve in
        /// economy_goods.json. A broken embargo rule must fail CI, never
        /// silently no-op in the live economy.
        /// </summary>
        public static void ValidateTradeEmbargoRules(string dataDirectory, IFileIO files, CatalogIntegrityReport report)
        {
            var load = Ashfall.Core.Economy.TradeEmbargoCatalogLoader.Load(
                dataDirectory, files, new SystemTextJsonSerializer());
            foreach (var err in load.Errors)
                report.Error("trade_embargoes.json: " + err);

            var goodIds = CollectGoodsIds(dataDirectory, files, report);
            foreach (var rule in load.Rules)
                foreach (var item in rule.AffectedItemIds)
                    if (!goodIds.Contains(item))
                        report.Error($"trade_embargoes.json: rule '{rule.RuleId}' affected item '{item}' does not resolve in economy_goods.json");
        }

        /// <summary>
        /// Plan 14B — regional_prices.json must load clean through its strict
        /// loader (region/category vocabularies, permille bounds, scarcity
        /// vocabulary, duplicate rows) and every item-level entry must resolve
        /// in economy_goods.json.
        /// </summary>
        public static void ValidateRegionalPriceAtlas(string dataDirectory, IFileIO files, CatalogIntegrityReport report)
        {
            var load = Ashfall.Core.Economy.RegionalPriceCatalogLoader.Load(
                dataDirectory, files, new SystemTextJsonSerializer());
            foreach (var err in load.Errors)
                report.Error("regional_prices.json: " + err);

            var goodIds = CollectGoodsIds(dataDirectory, files, report);
            foreach (var entry in load.Entries)
                if (!string.IsNullOrEmpty(entry.ItemId) && !goodIds.Contains(entry.ItemId))
                    report.Error($"regional_prices.json: entry item '{entry.ItemId}' (region '{entry.Region}') does not resolve in economy_goods.json");
        }

        /// <summary>Goods-catalog id set (economy_goods.json), for cross-file resolution in the economy geography validators.</summary>
        private static HashSet<string> CollectGoodsIds(string dataDirectory, IFileIO files, CatalogIntegrityReport report)
        {
            var ids = new HashSet<string>(StringComparer.Ordinal);
            try
            {
                string path = Path.Combine(dataDirectory, "economy_goods.json");
                if (!files.FileExists(path)) return ids;
                string raw = files.ReadAllText(path);
                using var doc = JsonDocument.Parse(raw);
                if (doc.RootElement.TryGetProperty("goods", out var goods) && goods.ValueKind == JsonValueKind.Array)
                    CollectCatalogIds(goods, ids);
            }
            catch (Exception ex)
            {
                report.Error("economy geography validator could not enumerate economy_goods.json ids: " + ex.Message);
            }
            return ids;
        }

        private static bool HasAnyElement(JsonElement array)
        {
            foreach (var _ in array.EnumerateArray()) return true;
            return false;
        }

        /// <summary>Tasks 9–12 Wave 4 — structural audio-cue id rule: empty
        /// (inherit/text-only) or lowercase snake_case without whitespace.
        /// Semantic resolution against the host cue registry is verified by
        /// the audio selftest once authored content lands (documented
        /// fallback policy: a missing cue logs once and the text path
        /// continues — playback failure never blocks the signal).</summary>
        private static bool IsValidAudioCueId(string cue)
        {
            if (string.IsNullOrEmpty(cue)) return true; // inherit / text-only
            if (cue.IndexOf(' ') >= 0) return false;
            if (cue != cue.ToLowerInvariant()) return false;
            foreach (var ch in cue)
            {
                bool ok = (ch >= 'a' && ch <= 'z') || (ch >= '0' && ch <= '9') || ch == '_';
                if (!ok) return false;
            }
            return true;
        }

        private static string clarityFormat(float value)
        {
            return value.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);
        }

        private static void ValidateWildlifeText(
            JsonElement element,
            string propertyName,
            string context,
            HashSet<string>? uniqueness,
            CatalogIntegrityReport report)
        {
            if (!element.TryGetProperty(propertyName, out var property)
                || property.ValueKind != JsonValueKind.String
                || string.IsNullOrWhiteSpace(property.GetString()))
            {
                report.Error($"{context}: missing or empty '{propertyName}'");
                return;
            }

            string value = property.GetString()!.Trim();
            string folded = value.ToUpperInvariant();
            if (folded.Contains("TODO", StringComparison.Ordinal)
                || folded.Contains("TBD", StringComparison.Ordinal)
                || folded.Contains("LOREM IPSUM", StringComparison.Ordinal))
            {
                report.Error($"{context}: '{propertyName}' contains placeholder text");
            }

            bool looksLikeRawId = value.IndexOf('_') >= 0
                && value.Equals(value.ToLowerInvariant(), StringComparison.Ordinal)
                && value.IndexOf(' ') < 0;
            if (looksLikeRawId)
                report.Error($"{context}: '{propertyName}' must not be a raw content ID");

            if (uniqueness != null && !uniqueness.Add(value))
                report.Error($"{context}: duplicate player-facing '{propertyName}' '{value}'");
        }

        public static void ValidateWildlifeTrappingCatalog(string dataDirectory, IFileIO files, CatalogIntegrityReport report)
        {
            var ctx = new Ctx { Report = report, File = "wildlife_trapping_catalog.json" };
            string itemsPath = Path.Combine(dataDirectory, "items.json");
            if (files.FileExists(itemsPath))
            {
                if (TryParse(itemsPath, files, out JsonDocument doc, report))
                {
                    using (doc)
                    {
                        Walk(doc.RootElement, "items.json", ctx);
                    }
                }
            }
            ValidateWildlifeTrappingCatalog(dataDirectory, files, ctx, report);
        }

        private static void ValidateWildlifeTrappingCatalog(string dataDirectory, IFileIO files, Ctx ctx, CatalogIntegrityReport report)
        {
            string trappingPath = Path.Combine(dataDirectory, "wildlife_trapping_catalog.json");
            if (!files.FileExists(trappingPath)) return;

            try
            {
                string json = files.ReadAllText(trappingPath);
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                // 1. Traps validation
                var trapIds = new HashSet<string>(StringComparer.Ordinal);
                var trapDisplayNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                var trapTypes = new Dictionary<string, string>(StringComparer.Ordinal);
                var trapPreyMap = new Dictionary<string, List<string>>(StringComparer.Ordinal);
                var trapWaterMap = new Dictionary<string, bool>(StringComparer.Ordinal);

                // Miss incidents are resolved by the canonical events catalog.
                // This lookup is validation-only; trapping runtime stores IDs
                // and never loads narrative content on its simulation path.
                var authoredEventIds = new HashSet<string>(StringComparer.Ordinal);
                string eventsPath = Path.Combine(dataDirectory, "events.json");
                if (files.FileExists(eventsPath)
                    && TryParse(eventsPath, files, out JsonDocument eventsDoc, report))
                {
                    using (eventsDoc)
                    {
                        if (eventsDoc.RootElement.TryGetProperty("events", out var eventsProp)
                            && eventsProp.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var eventEl in eventsProp.EnumerateArray())
                            {
                                if (eventEl.TryGetProperty("id", out var eventIdProp)
                                    && eventIdProp.ValueKind == JsonValueKind.String
                                    && !string.IsNullOrWhiteSpace(eventIdProp.GetString()))
                                {
                                    authoredEventIds.Add(eventIdProp.GetString()!);
                                }
                            }
                        }
                    }
                }

                string itemsPath = Path.Combine(dataDirectory, "items.json");
                bool hasItemsCatalog = files.FileExists(itemsPath);

                if (root.TryGetProperty("traps", out var trapsProp) && trapsProp.ValueKind == JsonValueKind.Array)
                {
                    int trapIndex = 0;
                    foreach (var trapEl in trapsProp.EnumerateArray())
                    {
                        string trapPath = $"wildlife_trapping_catalog.json:traps[{trapIndex}]";
                        if (!trapEl.TryGetProperty("trap_id", out var idProp) || string.IsNullOrWhiteSpace(idProp.GetString()))
                        {
                            report.Error($"{trapPath}: missing or empty 'trap_id'");
                            trapIndex++;
                            continue;
                        }
                        string trapId = idProp.GetString()!;
                        if (!trapIds.Add(trapId))
                        {
                            report.Error($"{trapPath}: duplicate trap_id '{trapId}'");
                        }
                        ValidateWildlifeText(trapEl, "displayName", $"{trapPath} '{trapId}'", trapDisplayNames, report);
                        ValidateWildlifeText(trapEl, "description", $"{trapPath} '{trapId}'", null, report);

                        if (trapEl.TryGetProperty("narrativeIncidentChance", out var incidentChanceProp))
                        {
                            ValidateProbability(incidentChanceProp,
                                $"{trapPath} '{trapId}'/narrativeIncidentChance",
                                "narrativeIncidentChance", report);
                        }

                        if (trapEl.TryGetProperty("narrativeIncidentIds", out var incidentIdsProp))
                        {
                            if (incidentIdsProp.ValueKind != JsonValueKind.Array)
                            {
                                report.Error($"{trapPath} '{trapId}': narrativeIncidentIds must be an array");
                            }
                            else
                            {
                                int incidentIndex = 0;
                                foreach (var incidentIdProp in incidentIdsProp.EnumerateArray())
                                {
                                    string incidentPath = $"{trapPath} '{trapId}'/narrativeIncidentIds[{incidentIndex}]";
                                    if (incidentIdProp.ValueKind != JsonValueKind.String
                                        || string.IsNullOrWhiteSpace(incidentIdProp.GetString()))
                                    {
                                        report.Error($"{incidentPath}: event ID cannot be empty");
                                    }
                                    else if (authoredEventIds.Count > 0
                                        && !authoredEventIds.Contains(incidentIdProp.GetString()!))
                                    {
                                        report.Error($"{incidentPath}: undefined event ID '{incidentIdProp.GetString()}'");
                                    }
                                    incidentIndex++;
                                }
                            }
                        }

                        if (hasItemsCatalog && !ctx.Registry.ContainsKey(trapId))
                        {
                            report.Error($"{trapPath}: trap '{trapId}' not found in items catalog");
                        }

                        string trapType = trapEl.TryGetProperty("trapType", out var typeProp) ? typeProp.GetString() ?? "" : "";
                        trapTypes[trapId] = trapType;

                        bool requiresWater = trapEl.TryGetProperty("requiresWater", out var waterProp) && waterProp.GetBoolean();
                        trapWaterMap[trapId] = requiresWater;

                        var preyList = new List<string>();
                        if (trapEl.TryGetProperty("compatiblePrey", out var preyProp) && preyProp.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var p in preyProp.EnumerateArray())
                            {
                                string? preyName = p.GetString();
                                if (!string.IsNullOrEmpty(preyName))
                                {
                                    preyList.Add(preyName);
                                }
                            }
                        }

                        if (preyList.Count == 0)
                        {
                            report.Error($"{trapPath}: trap '{trapId}' has no compatiblePrey");
                        }

                        trapPreyMap[trapId] = preyList;
                        trapIndex++;
                    }
                }
                else
                {
                    report.Error("wildlife_trapping_catalog.json: missing or invalid 'traps' array");
                }

                // 2. Baits validation
                var baitIds = new HashSet<string>(StringComparer.Ordinal);
                var baitDisplayNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                if (root.TryGetProperty("baits", out var baitsProp) && baitsProp.ValueKind == JsonValueKind.Array)
                {
                    int baitIndex = 0;
                    foreach (var baitEl in baitsProp.EnumerateArray())
                    {
                        string baitPath = $"wildlife_trapping_catalog.json:baits[{baitIndex}]";
                        if (!baitEl.TryGetProperty("baitId", out var idProp) || string.IsNullOrWhiteSpace(idProp.GetString()))
                        {
                            report.Error($"{baitPath}: missing or empty 'baitId'");
                            baitIndex++;
                            continue;
                        }
                        string baitId = idProp.GetString()!;
                        if (!baitIds.Add(baitId))
                        {
                            report.Error($"{baitPath}: duplicate baitId '{baitId}'");
                        }
                        ValidateWildlifeText(baitEl, "displayName", $"{baitPath} '{baitId}'", baitDisplayNames, report);

                        if (baitEl.TryGetProperty("catchBonusMultiplier", out var bonusProp))
                        {
                            if (!bonusProp.TryGetDouble(out double mult) || mult <= 0.0)
                            {
                                report.Error($"{baitPath}: bait '{baitId}' catchBonusMultiplier must be positive, got {mult}");
                            }
                        }

                        if (baitEl.TryGetProperty("toxicReduction", out var toxProp))
                        {
                            if (!toxProp.TryGetDouble(out double tox) || tox < 0.0)
                            {
                                report.Error($"{baitPath}: bait '{baitId}' toxicReduction must be non-negative, got {tox}");
                            }
                        }

                        baitIndex++;
                    }
                }
                else
                {
                    report.Error("wildlife_trapping_catalog.json: missing or invalid 'baits' array");
                }

                // 3. Prey validation & Compatibility & Bait reachability
                var preyIds = new HashSet<string>(StringComparer.Ordinal);
                var preyDisplayNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                var baitReferenced = new HashSet<string>(StringComparer.Ordinal);
                var preyPreferredTrapType = new Dictionary<string, string>(StringComparer.Ordinal);

                if (root.TryGetProperty("prey", out var preyArrayProp) && preyArrayProp.ValueKind == JsonValueKind.Array)
                {
                    int pIndex = 0;
                    foreach (var preyEl in preyArrayProp.EnumerateArray())
                    {
                        string preyPath = $"wildlife_trapping_catalog.json:prey[{pIndex}]";
                        if (!preyEl.TryGetProperty("speciesId", out var idProp) || string.IsNullOrWhiteSpace(idProp.GetString()))
                        {
                            report.Error($"{preyPath}: missing or empty 'speciesId'");
                            pIndex++;
                            continue;
                        }
                        string speciesId = idProp.GetString()!;
                        if (!preyIds.Add(speciesId))
                        {
                            report.Error($"{preyPath}: duplicate speciesId '{speciesId}'");
                        }
                        ValidateWildlifeText(preyEl, "displayName", $"{preyPath} '{speciesId}'", preyDisplayNames, report);
                        ValidateWildlifeText(preyEl, "description", $"{preyPath} '{speciesId}'", null, report);
                        if (preyEl.TryGetProperty("moraleEffect", out var moraleProp)
                            && moraleProp.ValueKind == JsonValueKind.Number
                            && moraleProp.TryGetDouble(out double morale)
                            && (double.IsNaN(morale) || double.IsInfinity(morale) || morale < -100d || morale > 100d))
                        {
                            report.Error($"{preyPath} '{speciesId}': moraleEffect must be finite and between -100 and 100");
                        }

                        string preferredTrap = preyEl.TryGetProperty("preferredTrapType", out var prefProp) ? prefProp.GetString() ?? "" : "";
                        preyPreferredTrapType[speciesId] = preferredTrap;

                        if (preyEl.TryGetProperty("attractedByBaitIds", out var attractedProp) && attractedProp.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var b in attractedProp.EnumerateArray())
                            {
                                string? bId = b.GetString();
                                if (!string.IsNullOrEmpty(bId))
                                {
                                    if (!baitIds.Contains(bId))
                                    {
                                        report.Error($"{preyPath}: prey '{speciesId}' references undefined baitId '{bId}'");
                                    }
                                    else
                                    {
                                        baitReferenced.Add(bId);
                                    }
                                }
                            }
                        }

                        pIndex++;
                    }
                }
                else
                {
                    report.Error("wildlife_trapping_catalog.json: missing or invalid 'prey' array");
                }

                // Migration presence is a live sector gate, so every authored
                // migrationSpeciesId must resolve to the ecology species
                // authority rather than becoming an inert string at runtime.
                string ecosystemPath = Path.Combine(dataDirectory, "wildlife_ecosystem.json");
                if (files.FileExists(ecosystemPath))
                {
                    var migrationSpeciesIds = new HashSet<string>(StringComparer.Ordinal);
                    using var ecosystemDoc = JsonDocument.Parse(files.ReadAllText(ecosystemPath));
                    if (ecosystemDoc.RootElement.TryGetProperty("species", out var speciesArray)
                        && speciesArray.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var speciesEl in speciesArray.EnumerateArray())
                        {
                            if (speciesEl.TryGetProperty("id", out var speciesIdProp)
                                && speciesIdProp.ValueKind == JsonValueKind.String
                                && !string.IsNullOrWhiteSpace(speciesIdProp.GetString()))
                                migrationSpeciesIds.Add(speciesIdProp.GetString()!);
                        }
                    }

                    if (root.TryGetProperty("prey", out var migrationPreyArray)
                        && migrationPreyArray.ValueKind == JsonValueKind.Array)
                    {
                        int migrationPreyIndex = 0;
                        foreach (var preyEl in migrationPreyArray.EnumerateArray())
                        {
                            string preyId = preyEl.TryGetProperty("speciesId", out var preyIdProp)
                                ? preyIdProp.GetString() ?? $"index {migrationPreyIndex}"
                                : $"index {migrationPreyIndex}";
                            if (preyEl.TryGetProperty("migrationSpeciesId", out var migrationProp)
                                && migrationProp.ValueKind == JsonValueKind.String
                                && !string.IsNullOrWhiteSpace(migrationProp.GetString())
                                && !migrationSpeciesIds.Contains(migrationProp.GetString()!))
                            {
                                report.Error($"wildlife_trapping_catalog.json: prey '{preyId}' references undefined migration species '{migrationProp.GetString()}'");
                            }
                            migrationPreyIndex++;
                        }
                    }
                }

                // Verify Bait Reachability: Every bait in baits must be reached by >= 1 prey
                foreach (var baitId in baitIds)
                {
                    if (!baitReferenced.Contains(baitId))
                    {
                        report.Error($"wildlife_trapping_catalog.json: bait '{baitId}' is unreachable (not referenced in any prey's attractedByBaitIds)");
                    }
                }

                // Verify Compatibility Matrix Completeness
                var preyCompatibleTraps = new Dictionary<string, List<string>>(StringComparer.Ordinal);
                foreach (var pId in preyIds)
                {
                    preyCompatibleTraps[pId] = new List<string>();
                }

                foreach (var kvp in trapPreyMap)
                {
                    string trapId = kvp.Key;
                    var preyList = kvp.Value;
                    if (preyIds.Count > 1 && preyList.Count >= preyIds.Count)
                    {
                        report.Error($"wildlife_trapping_catalog.json: trap '{trapId}' violates specialization; compatible with all prey species");
                    }

                    foreach (var pId in preyList)
                    {
                        if (!preyIds.Contains(pId))
                        {
                            report.Error($"wildlife_trapping_catalog.json: trap '{trapId}' references undefined prey '{pId}'");
                        }
                        else
                        {
                            preyCompatibleTraps[pId].Add(trapId);
                        }
                    }
                }

                foreach (var kvp in preyCompatibleTraps)
                {
                    string pId = kvp.Key;
                    var traps = kvp.Value;
                    if (traps.Count == 0)
                    {
                        report.Error($"wildlife_trapping_catalog.json: prey '{pId}' has no compatible traps");
                    }

                    if (preyPreferredTrapType.TryGetValue(pId, out var prefType) && !string.IsNullOrEmpty(prefType))
                    {
                        bool satisfied = false;
                        foreach (var tId in traps)
                        {
                            if (trapTypes.TryGetValue(tId, out var tType) && string.Equals(tType, prefType, StringComparison.Ordinal))
                            {
                                satisfied = true;
                                break;
                            }
                        }
                        if (!satisfied)
                        {
                            report.Error($"wildlife_trapping_catalog.json: prey '{pId}' preferredTrapType '{prefType}' is not satisfied by any compatible trap");
                        }
                    }
                }

                // Specialized domain constraints
                string[] waterOnlyPrey = { "mirror_carp", "ash_pike" };
                foreach (var wp in waterOnlyPrey)
                {
                    if (preyCompatibleTraps.TryGetValue(wp, out var traps))
                    {
                        foreach (var tId in traps)
                        {
                            if (!trapWaterMap.TryGetValue(tId, out bool reqWater) || !reqWater)
                            {
                                report.Error($"wildlife_trapping_catalog.json: aquatic prey '{wp}' is assigned to non-water trap '{tId}'");
                            }
                        }
                    }
                }

                string[] heavyPrey = { "deer", "boar" };
                foreach (var hp in heavyPrey)
                {
                    if (preyCompatibleTraps.TryGetValue(hp, out var traps))
                    {
                        foreach (var tId in traps)
                        {
                            if (!trapTypes.TryGetValue(tId, out var tType) || !string.Equals(tType, "pit", StringComparison.Ordinal))
                            {
                                report.Error($"wildlife_trapping_catalog.json: heavy prey '{hp}' is assigned to non-pit trap '{tId}'");
                            }
                        }
                    }
                }

                string[] avianPrey = { "ash_crow", "contaminated_fowl", "pheasant" };
                foreach (var ap in avianPrey)
                {
                    if (preyCompatibleTraps.TryGetValue(ap, out var traps))
                    {
                        foreach (var tId in traps)
                        {
                            if (!trapTypes.TryGetValue(tId, out var tType) || (!string.Equals(tType, "net", StringComparison.Ordinal) && !string.Equals(tType, "bird_snare", StringComparison.Ordinal)))
                            {
                                report.Error($"wildlife_trapping_catalog.json: avian prey '{ap}' is assigned to non-avian trap '{tId}' (type '{tType}')");
                            }
                        }
                    }
                }

                // 4. Recipes validation for traps
                string recipesPath = Path.Combine(dataDirectory, "recipes.json");
                if (files.FileExists(recipesPath))
                {
                    string recipesJson = files.ReadAllText(recipesPath);
                    using var recipesDoc = JsonDocument.Parse(recipesJson);
                    if (recipesDoc.RootElement.TryGetProperty("recipes", out var recipesArr) && recipesArr.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var rEl in recipesArr.EnumerateArray())
                        {
                            if (rEl.TryGetProperty("id", out var rIdProp) && rIdProp.GetString() is string rId && rId.StartsWith("craft_trap_", StringComparison.Ordinal))
                            {
                                if (!rEl.TryGetProperty("resultItemId", out var resProp) || string.IsNullOrWhiteSpace(resProp.GetString()))
                                {
                                    report.Error($"recipes.json: trap recipe '{rId}' missing 'resultItemId'");
                                }
                                else
                                {
                                    string resId = resProp.GetString()!;
                                    if (!ctx.Registry.ContainsKey(resId))
                                    {
                                        report.Error($"recipes.json: trap recipe '{rId}' result item '{resId}' not found in items catalog");
                                    }
                                    if (!trapIds.Contains(resId))
                                    {
                                        report.Error($"recipes.json: trap recipe '{rId}' outputs '{resId}' which is not a valid trap in wildlife_trapping_catalog.json");
                                    }
                                }

                                if (rEl.TryGetProperty("ingredients", out var ingredientsProp)
                                    && ingredientsProp.ValueKind == JsonValueKind.Array)
                                {
                                    int ingredientIndex = 0;
                                    foreach (var ingredientEl in ingredientsProp.EnumerateArray())
                                    {
                                        if (!ingredientEl.TryGetProperty("itemId", out var ingredientIdProp)
                                            || ingredientIdProp.ValueKind != JsonValueKind.String
                                            || string.IsNullOrWhiteSpace(ingredientIdProp.GetString()))
                                        {
                                            report.Error($"recipes.json: trap recipe '{rId}' ingredient[{ingredientIndex}] missing 'itemId'");
                                        }
                                        else if (!ctx.Registry.ContainsKey(ingredientIdProp.GetString()!))
                                        {
                                            report.Error($"recipes.json: trap recipe '{rId}' ingredient '{ingredientIdProp.GetString()}' not found in items catalog");
                                        }

                                        if (ingredientEl.TryGetProperty("amount", out var amountProp)
                                            && (!amountProp.TryGetInt32(out int amount) || amount <= 0))
                                        {
                                            report.Error($"recipes.json: trap recipe '{rId}' ingredient[{ingredientIndex}] amount must be positive");
                                        }
                                        ingredientIndex++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                report.Error("wildlife trapping catalog validator error: " + ex.Message);
            }
        }
    }
}
