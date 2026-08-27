using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using System.IO;
using System.Text.Json;

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
            "item_", "loc_", "location_", "quest_", "npc_", "survivor_", "faction_",
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
            "journal_psych_", "botany_", "folklore_children_", "fraud_ration_",
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
            "branch_", "ending_"
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
            "recipe_id", "key"
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
            "latentExpertTrait", "requiredTrait", "roomId", "itemId",
            "downstream_quest_trigger", "gating_flag", "nextStageId",
            "countermeasure_item_id",
            "from", "to"
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
            "tags", "category", "type", "phase", "discovery_trigger", "badge_asset_id",
            "stance", "short_name", "identity", "sink", "notes", "display_name",
            "collection_id",
            "hazardType", "will_not", "lootCategories", "tech_offerings",
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
            "flag_verdict_eden_log_recovered", "flag_verdict_fuse_world_read",
            "flag_verdict_shift_charter_restored", "flag_verdict_clerk_met",
            "flag_verdict_call_resolved", "flag_verdict_relay_read",
            "flag_verdict_fuse_advanced", "flag_verdict_wing_slept",
            // Expansion 12 (Vel/Vigil) orphan-knock gating flag — set at runtime by
            // future exp-12 code; registered in whitelists/orphan_knocks.json as a
            // deliberate, canonically-tracked orphan door event.
            "flag_exp07_vel_vigil_knock",
            "paper_scrap", "item_teddy_bear", "crayon", "ammo_9x19", "blood_bag",
            "item_suitcase_locked", "fat_rendered", "industrial_bleach", "bone_saw",
            "ammonia_tank", "cardboard_box", "cigarette_pack_sealed",
            "acoustic_foam_panel", "item_anchor_notes"
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
                if (TryParse(file, files, out JsonDocument doc))
                {
                    using (doc)
                    {
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
                    report.Error("unresolved " + (prefixed ? "id '" : "reference '")
                        + r.Value + "' at " + r.Path);
            }

            report.AuthoredIds = ctx.Authored;
            report.ReuseCount = ctx.Reuse;

            return report;
        }

        private static bool TryParse(string path, IFileIO files, out JsonDocument doc)
        {
            doc = null!;
            try
            {
                string text = files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(text)) return false;
                doc = JsonDocument.Parse(text);
                return true;
            }
            catch (Exception)
            {
                /* cleanup: fallback on malformed json */
                return false;
            }
        }

        private static void Walk(JsonElement element, string path, Ctx ctx)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    foreach (JsonProperty property in element.EnumerateObject())
                    {
                        string childPath = path + "/" + property.Name;
                        JsonElement value = property.Value;

                        if (value.ValueKind == JsonValueKind.String)
                        {
                            string? text = value.GetString();
                            if (string.IsNullOrEmpty(text) || IsVocabularyKey(property.Name)) continue;
                            RegisterOrReference(property.Name, text, childPath, ctx);
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
                                            Strict = true
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

                        Walk(value, childPath, ctx);
                    }
                    break;

                case JsonValueKind.Array:
                    int index = 0;
                    foreach (JsonElement item in element.EnumerateArray())
                    {
                        Walk(item, path + "[" + index + "]", ctx);
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

        private static void RegisterOrReference(string key, string value, string path, Ctx ctx)
        {
            if (IsDefinitionKey(key))
            {
                Register(key, value, path, ctx);
            }
            if (IsReferenceKey(key))
            {
                ctx.PendingRefs.Add(new Ref { Value = value, Path = path, Strict = true });
            }
            else if (!IsDefinitionKey(key) && StartsWithAny(value, IdPrefixes))
            {
                // Any prefixed string in a non-id position is still a reference
                // (Tier 1) — e.g. a narrative field naming an item id.
                ctx.PendingRefs.Add(new Ref { Value = value, Path = path, Strict = false });
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
    }
}
