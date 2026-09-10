// SPDX-License-Identifier: MIT
// ASHFALL Core — Plan 157 Grain Milling Projection & Metadata Authority
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Narrative
{
    public enum GrainMillingRecordFamily
    {
        MillstoneDressing,
        BoltingSilk,
        SiloWeevil,
        DampenerTempering
    }

    public enum GrainMillingProvenanceClass
    {
        TechnicalMaintenanceLog,
        TechnicalSiftingAssay,
        BiologicalStorageAudit,
        ConditioningAssay,
        EngineeringFabricationLog
    }

    public enum GrainMillingFacilityType
    {
        BurrMillstone,
        BoltingSifter,
        GrainSilo,
        ConditioningBin
    }

    public sealed class GrainMillingRecordMetadata
    {
        public string RecordId { get; set; } = string.Empty;
        public GrainMillingRecordFamily Family { get; set; }
        public GrainMillingProvenanceClass Provenance { get; set; }
        public GrainMillingFacilityType FacilityType { get; set; }
        public string FacilityId { get; set; } = string.Empty;
        public string NamedSubject { get; set; } = string.Empty;
        public string CropOrMaterial { get; set; } = string.Empty;
        public string ProducerId { get; set; } = string.Empty;
        public string Channel { get; set; } = "mill_inspection";
        public int MinDay { get; set; } = 1;
        public bool IsActivated { get; set; }
        public string MeasurementSummary { get; set; } = string.Empty;
        public List<string> RelatedRecordIds { get; set; } = new List<string>();
    }

    /// <summary>
    /// Plan 157: Projection and metadata authority for the Grain Milling catalog.
    /// Provides canonical producer mappings, industrial classifications, and
    /// measurement formatting without mutating live simulation state.
    /// </summary>
    public static class GrainMillingProjection
    {
        private static readonly Dictionary<string, GrainMillingRecordMetadata> Records =
            new Dictionary<string, GrainMillingRecordMetadata>(StringComparer.OrdinalIgnoreCase)
            {
                // ── Burr Millstone Dressing Logs (8 records: 6 activated, 2 deferred) ──
                ["burr_millstone_french_chert_chisel_cracking"] = new GrainMillingRecordMetadata
                {
                    RecordId = "burr_millstone_french_chert_chisel_cracking",
                    Family = GrainMillingRecordFamily.MillstoneDressing,
                    Provenance = GrainMillingProvenanceClass.TechnicalMaintenanceLog,
                    FacilityType = GrainMillingFacilityType.BurrMillstone,
                    FacilityId = "PRIMARY_BURR_RUNNER_PAIR_01",
                    NamedSubject = "Primary Burr Runner Pair 01",
                    CropOrMaterial = "FRENCH_CELLULAR_QUARTZ_CHERT",
                    ProducerId = "room_workshop",
                    Channel = "room_inspection",
                    MinDay = 1,
                    IsActivated = true,
                    MeasurementSummary = "16.0 cracks/in lands, 115.0 runner RPM",
                    RelatedRecordIds = new List<string> { "bolting_silk_gauze_number_mesh_selection", "mill_tempering_hard_wheat_bran_toughening" }
                },
                ["burr_millstone_runner_stone_rynd_balance_lead"] = new GrainMillingRecordMetadata
                {
                    RecordId = "burr_millstone_runner_stone_rynd_balance_lead",
                    Family = GrainMillingRecordFamily.MillstoneDressing,
                    Provenance = GrainMillingProvenanceClass.TechnicalMaintenanceLog,
                    FacilityType = GrainMillingFacilityType.BurrMillstone,
                    FacilityId = "RESERVE_GRIST_MILL_RUNNER_02",
                    NamedSubject = "Reserve Grist Mill Runner 02",
                    CropOrMaterial = "SECTIONAL_QUARTZ_SEGMENTS",
                    ProducerId = "loc_quarry",
                    Channel = "expedition_survey",
                    MinDay = 5,
                    IsActivated = true,
                    MeasurementSummary = "12.0 cracks/in, 125.0 RPM, 1.8 kg balance lead",
                    RelatedRecordIds = new List<string> { "burr_millstone_french_chert_chisel_cracking", "burr_millstone_spindle_footstep_bearing_lignum_vitae" }
                },
                ["burr_millstone_furrow_lands_feather_edge_wear"] = new GrainMillingRecordMetadata
                {
                    RecordId = "burr_millstone_furrow_lands_feather_edge_wear",
                    Family = GrainMillingRecordFamily.MillstoneDressing,
                    Provenance = GrainMillingProvenanceClass.TechnicalMaintenanceLog,
                    FacilityType = GrainMillingFacilityType.BurrMillstone,
                    FacilityId = "COMMISSARY_FLOUR_MILL_01",
                    NamedSubject = "Commissary Flour Mill 01",
                    CropOrMaterial = "DERBYSHIRE_PEAK_GRITSTONE",
                    ProducerId = "loc_grain_silo",
                    Channel = "trade_inspection",
                    MinDay = 8,
                    IsActivated = true,
                    MeasurementSummary = "8.0 cracks/in, 110.0 RPM, 65°C friction spike",
                    RelatedRecordIds = new List<string> { "bolting_silk_centrifugal_reel_beater_tear", "grain_silo_granary_weevil_larva_hollow_berry" }
                },
                ["burr_millstone_spindle_footstep_bearing_lignum_vitae"] = new GrainMillingRecordMetadata
                {
                    RecordId = "burr_millstone_spindle_footstep_bearing_lignum_vitae",
                    Family = GrainMillingRecordFamily.MillstoneDressing,
                    Provenance = GrainMillingProvenanceClass.TechnicalMaintenanceLog,
                    FacilityType = GrainMillingFacilityType.BurrMillstone,
                    FacilityId = "WATER_DRIVEN_GRIST_MILL_03",
                    NamedSubject = "Water Driven Grist Mill 03",
                    CropOrMaterial = "FRENCH_BURR_CHERT_BLOCKS",
                    ProducerId = "loc_quarry",
                    Channel = "expedition_survey",
                    MinDay = 12,
                    IsActivated = true,
                    MeasurementSummary = "14.0 cracks/in, 105.0 RPM, <0.2mm thrust wear",
                    RelatedRecordIds = new List<string> { "burr_millstone_runner_stone_rynd_balance_lead" }
                },
                ["burr_millstone_bridge_tree_tentering_screw_chatter"] = new GrainMillingRecordMetadata
                {
                    RecordId = "burr_millstone_bridge_tree_tentering_screw_chatter",
                    Family = GrainMillingRecordFamily.MillstoneDressing,
                    Provenance = GrainMillingProvenanceClass.TechnicalMaintenanceLog,
                    FacilityType = GrainMillingFacilityType.BurrMillstone,
                    FacilityId = "PRECISION_SEMOLINA_STONE_04",
                    NamedSubject = "Precision Semolina Stone 04",
                    CropOrMaterial = "FINE_GRAIN_QUARTZ_BURR",
                    ProducerId = "room_workshop",
                    Channel = "room_inspection",
                    MinDay = 15,
                    IsActivated = true,
                    MeasurementSummary = "18.0 cracks/in, 130.0 RPM, 0.15mm gap flutter",
                    RelatedRecordIds = new List<string> { "bolting_silk_plansifter_gyratory_counterweight_wobble" }
                },
                ["burr_millstone_swallow_eye_centrifugal_feed_shoe"] = new GrainMillingRecordMetadata
                {
                    RecordId = "burr_millstone_swallow_eye_centrifugal_feed_shoe",
                    Family = GrainMillingRecordFamily.MillstoneDressing,
                    Provenance = GrainMillingProvenanceClass.TechnicalMaintenanceLog,
                    FacilityType = GrainMillingFacilityType.BurrMillstone,
                    FacilityId = "AUTOMATED_HOPPER_FEED_STONE",
                    NamedSubject = "Automated Hopper Feed Stone",
                    CropOrMaterial = "COMPOSITE_EMERY_CEMENT_STONE",
                    ProducerId = "loc_settlement_silo_burrow",
                    Channel = "expedition_survey",
                    MinDay = 18,
                    IsActivated = true,
                    MeasurementSummary = "10.0 cracks/in, 120.0 RPM, 180 kg/hr feed rate",
                    RelatedRecordIds = new List<string> { "bolting_silk_middlings_purifier_air_current_aspiration", "grain_silo_diatomaceous_earth_desiccant_dusting" }
                },
                ["burr_millstone_flour_hoop_leather_sweeper_brush"] = new GrainMillingRecordMetadata
                {
                    RecordId = "burr_millstone_flour_hoop_leather_sweeper_brush",
                    Family = GrainMillingRecordFamily.MillstoneDressing,
                    Provenance = GrainMillingProvenanceClass.TechnicalMaintenanceLog,
                    FacilityType = GrainMillingFacilityType.BurrMillstone,
                    FacilityId = "DUST_FREE_ENCLOSED_MILL_02",
                    NamedSubject = "Dust Free Enclosed Mill 02",
                    CropOrMaterial = "FRENCH_BURR_HEAVY_BLOCKS",
                    ProducerId = "loc_grain_silo",
                    Channel = "trade_inspection",
                    MinDay = 999,
                    IsActivated = false,
                    MeasurementSummary = "14.0 cracks/in, 112.0 RPM, ox-hide perimeter sweep",
                    RelatedRecordIds = new List<string>()
                },
                ["burr_millstone_iron_banding_thermal_shrink_fit"] = new GrainMillingRecordMetadata
                {
                    RecordId = "burr_millstone_iron_banding_thermal_shrink_fit",
                    Family = GrainMillingRecordFamily.MillstoneDressing,
                    Provenance = GrainMillingProvenanceClass.EngineeringFabricationLog,
                    FacilityType = GrainMillingFacilityType.BurrMillstone,
                    FacilityId = "HEAVY_FOUNDRY_MILL_RUNNER_01",
                    NamedSubject = "Heavy Foundry Mill Runner 01",
                    CropOrMaterial = "SEGMENTED_BURR_QUARTZ_RING",
                    ProducerId = "room_foundry",
                    Channel = "room_inspection",
                    MinDay = 999,
                    IsActivated = false,
                    MeasurementSummary = "16.0 cracks/in, 118.0 RPM, dual wrought iron hoops",
                    RelatedRecordIds = new List<string>()
                },

                // ── Bolting Silk Mesh Reports (8 records: 6 activated, 2 deferred) ──
                ["bolting_silk_gauze_number_mesh_selection"] = new GrainMillingRecordMetadata
                {
                    RecordId = "bolting_silk_gauze_number_mesh_selection",
                    Family = GrainMillingRecordFamily.BoltingSilk,
                    Provenance = GrainMillingProvenanceClass.TechnicalSiftingAssay,
                    FacilityType = GrainMillingFacilityType.BoltingSifter,
                    FacilityId = "PRIMARY_PATENT_FLOUR_BOLTER_01",
                    NamedSubject = "Primary Patent Flour Bolter 01",
                    CropOrMaterial = "NO_10_XX_SWISS_BOLTING_SILK",
                    ProducerId = "room_common_mess_hall",
                    Channel = "room_inspection",
                    MinDay = 2,
                    IsActivated = true,
                    MeasurementSummary = "129.0 µm aperture, 72.5% flour extraction yield",
                    RelatedRecordIds = new List<string> { "burr_millstone_french_chert_chisel_cracking", "mill_tempering_hard_wheat_bran_toughening" }
                },
                ["bolting_silk_centrifugal_reel_beater_tear"] = new GrainMillingRecordMetadata
                {
                    RecordId = "bolting_silk_centrifugal_reel_beater_tear",
                    Family = GrainMillingRecordFamily.BoltingSilk,
                    Provenance = GrainMillingProvenanceClass.TechnicalSiftingAssay,
                    FacilityType = GrainMillingFacilityType.BoltingSifter,
                    FacilityId = "CENTRIFUGAL_FORCE_REEL_02",
                    NamedSubject = "Centrifugal Force Reel 02",
                    CropOrMaterial = "HEAVY_DUTY_DUTCH_SILK_12XX",
                    ProducerId = "loc_grain_silo",
                    Channel = "trade_inspection",
                    MinDay = 6,
                    IsActivated = true,
                    MeasurementSummary = "105.0 µm aperture, 68.0% yield, 220.0 RPM beater gash",
                    RelatedRecordIds = new List<string> { "burr_millstone_furrow_lands_feather_edge_wear" }
                },
                ["bolting_silk_flour_mite_infestation_pore_clog"] = new GrainMillingRecordMetadata
                {
                    RecordId = "bolting_silk_flour_mite_infestation_pore_clog",
                    Family = GrainMillingRecordFamily.BoltingSilk,
                    Provenance = GrainMillingProvenanceClass.BiologicalStorageAudit,
                    FacilityType = GrainMillingFacilityType.BoltingSifter,
                    FacilityId = "STORED_FLOUR_SIFTER_UNIT_03",
                    NamedSubject = "Stored Flour Sifter Unit 03",
                    CropOrMaterial = "STANDARD_BOLTING_CLOTH_8X",
                    ProducerId = "loc_grain_silo",
                    Channel = "silo_survey",
                    MinDay = 10,
                    IsActivated = true,
                    MeasurementSummary = "180.0 µm aperture, 55.0% yield, 75% RH Acarus siro clog",
                    RelatedRecordIds = new List<string> { "grain_silo_thermal_convection_moisture_migration", "grain_silo_granary_weevil_larva_hollow_berry" }
                },
                ["bolting_silk_static_electricity_brush_grounding"] = new GrainMillingRecordMetadata
                {
                    RecordId = "bolting_silk_static_electricity_brush_grounding",
                    Family = GrainMillingRecordFamily.BoltingSilk,
                    Provenance = GrainMillingProvenanceClass.TechnicalSiftingAssay,
                    FacilityType = GrainMillingFacilityType.BoltingSifter,
                    FacilityId = "DRY_CLIMATE_PLANSIFTER_01",
                    NamedSubject = "Dry Climate Plansifter 01",
                    CropOrMaterial = "TRIPLE_EXTRA_SILK_11XXX",
                    ProducerId = "room_workshop",
                    Channel = "room_inspection",
                    MinDay = 14,
                    IsActivated = true,
                    MeasurementSummary = "118.0 µm aperture, 74.0% yield, copper tinsel grounding",
                    RelatedRecordIds = new List<string> { "burr_millstone_bridge_tree_tentering_screw_chatter" }
                },
                ["bolting_silk_plansifter_gyratory_counterweight_wobble"] = new GrainMillingRecordMetadata
                {
                    RecordId = "bolting_silk_plansifter_gyratory_counterweight_wobble",
                    Family = GrainMillingRecordFamily.BoltingSilk,
                    Provenance = GrainMillingProvenanceClass.TechnicalSiftingAssay,
                    FacilityType = GrainMillingFacilityType.BoltingSifter,
                    FacilityId = "FREE_SWINGING_PLANSIFTER_BAY",
                    NamedSubject = "Free Swinging Plansifter Bay",
                    CropOrMaterial = "HIGH_TENSION_SILK_NO_9",
                    ProducerId = "room_workshop",
                    Channel = "room_inspection",
                    MinDay = 20,
                    IsActivated = true,
                    MeasurementSummary = "150.0 µm aperture, 78.0% yield, elliptical counterweight harmonic",
                    RelatedRecordIds = new List<string> { "burr_millstone_bridge_tree_tentering_screw_chatter" }
                },
                ["bolting_silk_middlings_purifier_air_current_aspiration"] = new GrainMillingRecordMetadata
                {
                    RecordId = "bolting_silk_middlings_purifier_air_current_aspiration",
                    Family = GrainMillingRecordFamily.BoltingSilk,
                    Provenance = GrainMillingProvenanceClass.TechnicalSiftingAssay,
                    FacilityType = GrainMillingFacilityType.BoltingSifter,
                    FacilityId = "MIDDLINGS_PURIFIER_DECK_04",
                    NamedSubject = "Middlings Purifier Deck 04",
                    CropOrMaterial = "GRADUATED_APERTURE_PURIFIER_SILK",
                    ProducerId = "loc_settlement_silo_burrow",
                    Channel = "expedition_survey",
                    MinDay = 22,
                    IsActivated = true,
                    MeasurementSummary = "220.0 µm aperture, 82.0% yield, bee-wing aspiration lift",
                    RelatedRecordIds = new List<string> { "burr_millstone_swallow_eye_centrifugal_feed_shoe", "mill_tempering_conditioning_rest_bin_dwell_time" }
                },
                ["bolting_silk_nylon_synthetic_monofilament_upgrade"] = new GrainMillingRecordMetadata
                {
                    RecordId = "bolting_silk_nylon_synthetic_monofilament_upgrade",
                    Family = GrainMillingRecordFamily.BoltingSilk,
                    Provenance = GrainMillingProvenanceClass.TechnicalSiftingAssay,
                    FacilityType = GrainMillingFacilityType.BoltingSifter,
                    FacilityId = "MODERNIZED_SIFTING_CHEST_01",
                    NamedSubject = "Modernized Sifting Chest 01",
                    CropOrMaterial = "POLYAMIDE_MONOFILAMENT_120T",
                    ProducerId = "room_workshop",
                    Channel = "room_inspection",
                    MinDay = 999,
                    IsActivated = false,
                    MeasurementSummary = "120.0 µm aperture, 76.5% yield, 3x polyamide lifespan",
                    RelatedRecordIds = new List<string>()
                },
                ["bolting_silk_bran_duster_wire_gauze_scour"] = new GrainMillingRecordMetadata
                {
                    RecordId = "bolting_silk_bran_duster_wire_gauze_scour",
                    Family = GrainMillingRecordFamily.BoltingSilk,
                    Provenance = GrainMillingProvenanceClass.TechnicalSiftingAssay,
                    FacilityType = GrainMillingFacilityType.BoltingSifter,
                    FacilityId = "TERMINAL_BRAN_DUSTER_UNIT",
                    NamedSubject = "Terminal Bran Duster Unit",
                    CropOrMaterial = "PHOSPHOR_BRONZE_WIRE_GAUZE_40M",
                    ProducerId = "loc_grain_silo",
                    Channel = "trade_inspection",
                    MinDay = 999,
                    IsActivated = false,
                    MeasurementSummary = "380.0 µm aperture, 86.0% yield, +3.5% flour recovery",
                    RelatedRecordIds = new List<string>()
                },

                // ── Grain Silo Weevil Audits (7 records: 5 activated, 2 deferred) ──
                ["grain_silo_granary_weevil_larva_hollow_berry"] = new GrainMillingRecordMetadata
                {
                    RecordId = "grain_silo_granary_weevil_larva_hollow_berry",
                    Family = GrainMillingRecordFamily.SiloWeevil,
                    Provenance = GrainMillingProvenanceClass.BiologicalStorageAudit,
                    FacilityType = GrainMillingFacilityType.GrainSilo,
                    FacilityId = "DEEP_STORAGE_SILO_ALPHA_01",
                    NamedSubject = "Deep Storage Silo Alpha 01",
                    CropOrMaterial = "HARD_RED_WINTER_WHEAT",
                    ProducerId = "loc_grain_silo",
                    Channel = "silo_survey",
                    MinDay = 3,
                    IsActivated = true,
                    MeasurementSummary = "13.8% moisture, 24.5°C, -14 kg/hL test weight loss",
                    RelatedRecordIds = new List<string> { "grain_silo_diatomaceous_earth_desiccant_dusting", "mill_tempering_hard_wheat_bran_toughening" }
                },
                ["grain_silo_thermal_convection_moisture_migration"] = new GrainMillingRecordMetadata
                {
                    RecordId = "grain_silo_thermal_convection_moisture_migration",
                    Family = GrainMillingRecordFamily.SiloWeevil,
                    Provenance = GrainMillingProvenanceClass.BiologicalStorageAudit,
                    FacilityType = GrainMillingFacilityType.GrainSilo,
                    FacilityId = "REINFORCED_CONCRETE_SILO_03",
                    NamedSubject = "Reinforced Concrete Silo 03",
                    CropOrMaterial = "SPRING_DURUM_WHEAT",
                    ProducerId = "loc_settlement_silo_burrow",
                    Channel = "silo_survey",
                    MinDay = 7,
                    IsActivated = true,
                    MeasurementSummary = "17.2% moisture, 12.0°C, 20cm moldy crust",
                    RelatedRecordIds = new List<string> { "grain_silo_granary_weevil_larva_hollow_berry", "bolting_silk_flour_mite_infestation_pore_clog" }
                },
                ["grain_silo_carbon_dioxide_inert_gas_asphyxiation"] = new GrainMillingRecordMetadata
                {
                    RecordId = "grain_silo_carbon_dioxide_inert_gas_asphyxiation",
                    Family = GrainMillingRecordFamily.SiloWeevil,
                    Provenance = GrainMillingProvenanceClass.TechnicalMaintenanceLog,
                    FacilityType = GrainMillingFacilityType.GrainSilo,
                    FacilityId = "HERMETIC_STEEL_SILO_CELL_04",
                    NamedSubject = "Hermetic Steel Silo Cell 04",
                    CropOrMaterial = "HULLED_MALTING_BARLEY",
                    ProducerId = "loc_agricultural_outpost",
                    Channel = "expedition_survey",
                    MinDay = 11,
                    IsActivated = true,
                    MeasurementSummary = "11.5% moisture, 16.0°C, 65% CO2 hermetic purge",
                    RelatedRecordIds = new List<string> { "grain_silo_diatomaceous_earth_desiccant_dusting", "mill_tempering_endosperm_mellowing_starch_damage" }
                },
                ["grain_silo_mycotoxin_aflatoxin_hotspot_probe"] = new GrainMillingRecordMetadata
                {
                    RecordId = "grain_silo_mycotoxin_aflatoxin_hotspot_probe",
                    Family = GrainMillingRecordFamily.SiloWeevil,
                    Provenance = GrainMillingProvenanceClass.BiologicalStorageAudit,
                    FacilityType = GrainMillingFacilityType.GrainSilo,
                    FacilityId = "EMERGENCY_RESERVE_SILO_02",
                    NamedSubject = "Emergency Reserve Silo 02",
                    CropOrMaterial = "YELLOW_DENT_MAIZE",
                    ProducerId = "loc_grain_silo",
                    Channel = "silo_survey",
                    MinDay = 16,
                    IsActivated = true,
                    MeasurementSummary = "19.5% moisture, 48.0°C hotspot, Aspergillus risk",
                    RelatedRecordIds = new List<string> { "grain_silo_thermal_convection_moisture_migration" }
                },
                ["grain_silo_diatomaceous_earth_desiccant_dusting"] = new GrainMillingRecordMetadata
                {
                    RecordId = "grain_silo_diatomaceous_earth_desiccant_dusting",
                    Family = GrainMillingRecordFamily.SiloWeevil,
                    Provenance = GrainMillingProvenanceClass.TechnicalMaintenanceLog,
                    FacilityType = GrainMillingFacilityType.GrainSilo,
                    FacilityId = "CENTRAL_COMMISSARY_GRAIN_ELEVATOR",
                    NamedSubject = "Central Commissary Grain Elevator",
                    CropOrMaterial = "SOFT_WHITE_WHEAT",
                    ProducerId = "loc_settlement_silo_burrow",
                    Channel = "silo_survey",
                    MinDay = 999,
                    IsActivated = false,
                    MeasurementSummary = "12.2% moisture, 18.0°C, 0.1% amorphous silica dusting",
                    RelatedRecordIds = new List<string>()
                },
                ["grain_silo_concrete_hopper_funnel_flow_rat_hole"] = new GrainMillingRecordMetadata
                {
                    RecordId = "grain_silo_concrete_hopper_funnel_flow_rat_hole",
                    Family = GrainMillingRecordFamily.SiloWeevil,
                    Provenance = GrainMillingProvenanceClass.TechnicalMaintenanceLog,
                    FacilityType = GrainMillingFacilityType.GrainSilo,
                    FacilityId = "CONICAL_DISCHARGE_BIN_05",
                    NamedSubject = "Conical Discharge Bin 05",
                    CropOrMaterial = "RAW_FIELD_RYE",
                    ProducerId = "loc_grain_silo",
                    Channel = "trade_inspection",
                    MinDay = 999,
                    IsActivated = false,
                    MeasurementSummary = "16.0% moisture, 15.0°C, 120t cohesive arch bridging",
                    RelatedRecordIds = new List<string>()
                },
                ["grain_silo_pneumatic_grain_turnover_aeration"] = new GrainMillingRecordMetadata
                {
                    RecordId = "grain_silo_pneumatic_grain_turnover_aeration",
                    Family = GrainMillingRecordFamily.SiloWeevil,
                    Provenance = GrainMillingProvenanceClass.TechnicalMaintenanceLog,
                    FacilityType = GrainMillingFacilityType.GrainSilo,
                    FacilityId = "MAIN_STORAGE_BATTERY_SILO_06",
                    NamedSubject = "Main Storage Battery Silo 06",
                    CropOrMaterial = "HARD_RED_SPRING_WHEAT",
                    ProducerId = "loc_settlement_silo_burrow",
                    Channel = "silo_survey",
                    MinDay = 999,
                    IsActivated = false,
                    MeasurementSummary = "13.0% moisture, 14.0°C, -8.0°C pneumatic aeration",
                    RelatedRecordIds = new List<string>()
                },

                // ── Mill Dampener Tempering Assays (7 records: 5 activated, 2 deferred) ──
                ["mill_tempering_hard_wheat_bran_toughening"] = new GrainMillingRecordMetadata
                {
                    RecordId = "mill_tempering_hard_wheat_bran_toughening",
                    Family = GrainMillingRecordFamily.DampenerTempering,
                    Provenance = GrainMillingProvenanceClass.ConditioningAssay,
                    FacilityType = GrainMillingFacilityType.ConditioningBin,
                    FacilityId = "PRIMARY_TEMPERING_SILO_01",
                    NamedSubject = "Primary Tempering Silo 01",
                    CropOrMaterial = "Hard Winter Wheat",
                    ProducerId = "room_greenhouse",
                    Channel = "agrarian_record",
                    MinDay = 2,
                    IsActivated = true,
                    MeasurementSummary = "+3.5% water addition, 15.5% target, 24.0h dwell",
                    RelatedRecordIds = new List<string> { "burr_millstone_french_chert_chisel_cracking", "bolting_silk_gauze_number_mesh_selection" }
                },
                ["mill_tempering_conditioning_rest_bin_dwell_time"] = new GrainMillingRecordMetadata
                {
                    RecordId = "mill_tempering_conditioning_rest_bin_dwell_time",
                    Family = GrainMillingRecordFamily.DampenerTempering,
                    Provenance = GrainMillingProvenanceClass.ConditioningAssay,
                    FacilityType = GrainMillingFacilityType.ConditioningBin,
                    FacilityId = "SECONDARY_REST_BIN_BAY_02",
                    NamedSubject = "Secondary Rest Bin Bay 02",
                    CropOrMaterial = "Hard Cereal Grain",
                    ProducerId = "loc_settlement_silo_burrow",
                    Channel = "agrarian_record",
                    MinDay = 6,
                    IsActivated = true,
                    MeasurementSummary = "+2.8% water addition, 14.8% target, 18.0h dwell",
                    RelatedRecordIds = new List<string> { "mill_tempering_hard_wheat_bran_toughening", "bolting_silk_middlings_purifier_air_current_aspiration" }
                },
                ["mill_tempering_hydrothermal_hot_water_scour"] = new GrainMillingRecordMetadata
                {
                    RecordId = "mill_tempering_hydrothermal_hot_water_scour",
                    Family = GrainMillingRecordFamily.DampenerTempering,
                    Provenance = GrainMillingProvenanceClass.ConditioningAssay,
                    FacilityType = GrainMillingFacilityType.ConditioningBin,
                    FacilityId = "HYDROTHERMAL_SCOURER_SKID",
                    NamedSubject = "Hydrothermal Scourer Skid",
                    CropOrMaterial = "Seed Grain",
                    ProducerId = "room_workshop",
                    Channel = "room_inspection",
                    MinDay = 9,
                    IsActivated = true,
                    MeasurementSummary = "+4.0% water, 16.0% target, 52.0°C scour, 8.0h dwell",
                    RelatedRecordIds = new List<string> { "mill_tempering_hard_wheat_bran_toughening" }
                },
                ["mill_tempering_endosperm_mellowing_starch_damage"] = new GrainMillingRecordMetadata
                {
                    RecordId = "mill_tempering_endosperm_mellowing_starch_damage",
                    Family = GrainMillingRecordFamily.DampenerTempering,
                    Provenance = GrainMillingProvenanceClass.ConditioningAssay,
                    FacilityType = GrainMillingFacilityType.ConditioningBin,
                    FacilityId = "PRECISION_MILLING_BIN_03",
                    NamedSubject = "Precision Milling Bin 03",
                    CropOrMaterial = "Durum Wheat",
                    ProducerId = "loc_agricultural_outpost",
                    Channel = "expedition_survey",
                    MinDay = 13,
                    IsActivated = true,
                    MeasurementSummary = "+3.0% water, 15.2% target, 20.0h dwell, -18% kWh",
                    RelatedRecordIds = new List<string> { "grain_silo_carbon_dioxide_inert_gas_asphyxiation" }
                },
                ["mill_tempering_over_wetting_paste_choke_fluting"] = new GrainMillingRecordMetadata
                {
                    RecordId = "mill_tempering_over_wetting_paste_choke_fluting",
                    Family = GrainMillingRecordFamily.DampenerTempering,
                    Provenance = GrainMillingProvenanceClass.ConditioningAssay,
                    FacilityType = GrainMillingFacilityType.ConditioningBin,
                    FacilityId = "EXPERIMENTAL_WET_MILL_CELL",
                    NamedSubject = "Experimental Wet Mill Cell",
                    CropOrMaterial = "Over-Hydrated Wheat",
                    ProducerId = "room_workshop",
                    Channel = "room_inspection",
                    MinDay = 999,
                    IsActivated = false,
                    MeasurementSummary = "+6.5% water, 18.5% soggy target, spiral fluting choke",
                    RelatedRecordIds = new List<string>()
                },
                ["mill_tempering_pin_mill_entoleter_insect_egg_shatter"] = new GrainMillingRecordMetadata
                {
                    RecordId = "mill_tempering_pin_mill_entoleter_insect_egg_shatter",
                    Family = GrainMillingRecordFamily.DampenerTempering,
                    Provenance = GrainMillingProvenanceClass.TechnicalMaintenanceLog,
                    FacilityType = GrainMillingFacilityType.ConditioningBin,
                    FacilityId = "ENTOLETER_IMPACT_STATION",
                    NamedSubject = "Entoleter Impact Station",
                    CropOrMaterial = "Tempered Wheat Kernels",
                    ProducerId = "loc_settlement_silo_burrow",
                    Channel = "silo_survey",
                    MinDay = 999,
                    IsActivated = false,
                    MeasurementSummary = "90.0 m/s pin impact velocity, mechanical egg shatter",
                    RelatedRecordIds = new List<string>()
                },
                ["mill_tempering_automatic_rotor_dampener_spray_jet"] = new GrainMillingRecordMetadata
                {
                    RecordId = "mill_tempering_automatic_rotor_dampener_spray_jet",
                    Family = GrainMillingRecordFamily.DampenerTempering,
                    Provenance = GrainMillingProvenanceClass.ConditioningAssay,
                    FacilityType = GrainMillingFacilityType.ConditioningBin,
                    FacilityId = "CONTINUOUS_INTENSIVE_DAMPENER",
                    NamedSubject = "Continuous Intensive Dampener",
                    CropOrMaterial = "Bulk Grain Stream",
                    ProducerId = "loc_grain_silo",
                    Channel = "trade_inspection",
                    MinDay = 999,
                    IsActivated = false,
                    MeasurementSummary = "+3.2% water, 15.0% target, 4.0 bar atomization, 8 t/hr",
                    RelatedRecordIds = new List<string>()
                }
            };

        public static GrainMillingRecordMetadata? GetMetadata(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) return null;
            return Records.TryGetValue(recordId, out var meta) ? meta : null;
        }

        public static IEnumerable<GrainMillingRecordMetadata> GetAllMetadata() => Records.Values;

        public static GrainMillingRecordFamily? GetFamily(string recordId)
        {
            var meta = GetMetadata(recordId);
            return meta?.Family;
        }

        public static List<GrainMillingRecordMetadata> GetByProducer(string producerId)
        {
            var result = new List<GrainMillingRecordMetadata>();
            if (string.IsNullOrWhiteSpace(producerId)) return result;

            foreach (var meta in Records.Values)
            {
                if (string.Equals(meta.ProducerId, producerId, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(meta);
                }
            }
            return result;
        }

        public static List<string> GetRelated(string recordId)
        {
            var meta = GetMetadata(recordId);
            if (meta == null) return new List<string>();
            return new List<string>(meta.RelatedRecordIds);
        }
    }
}
