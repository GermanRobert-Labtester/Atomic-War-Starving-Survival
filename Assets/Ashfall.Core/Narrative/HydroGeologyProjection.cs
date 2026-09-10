// SPDX-License-Identifier: MIT
// ASHFALL Core — Plan 154 Hydrogeology Science Projection & Metadata Authority
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Narrative
{
    public enum HydroGeologyRecordFamily
    {
        ArtesianWell,
        CaveBiota,
        GeothermalSteam,
        StalactiteMineral
    }

    public enum HydroGeologyProvenanceClass
    {
        HistoricalWaterAssay,
        HistoricalEcologicalSurvey,
        HistoricalEngineeringDiagnostic,
        HistoricalMineralAssay,
        AmbiguousRequiresReconciliation
    }

    public enum HydroGeologyContaminantClassification
    {
        None,
        Radionuclide,
        ChemicalToxicant,
        BiologicalBiofouling
    }

    public sealed class HydroGeologyRecordMetadata
    {
        public string RecordId { get; set; } = string.Empty;
        public HydroGeologyRecordFamily Family { get; set; }
        public HydroGeologyProvenanceClass Provenance { get; set; }
        public HydroGeologyContaminantClassification ContaminantClass { get; set; } = HydroGeologyContaminantClassification.None;
        public string ProducerId { get; set; } = string.Empty;
        public string NamedSubject { get; set; } = string.Empty;
        public string Channel { get; set; } = "expedition_survey";
        public int MinDay { get; set; } = 1;
        public bool IsActivated { get; set; }
        public string MeasurementProvenance { get; set; } = string.Empty;
    }

    /// <summary>
    /// Plan 154: Projection and metadata authority for the Hydrogeology science catalog.
    /// Provides canonical producer mappings, scientific discipline classifications, and
    /// provenance-anchored measurement formatting without mutating live simulation state.
    /// </summary>
    public static class HydroGeologyProjection
    {
        private static readonly Dictionary<string, HydroGeologyRecordMetadata> Records =
            new Dictionary<string, HydroGeologyRecordMetadata>(StringComparer.OrdinalIgnoreCase)
            {
                // ── Artesian Well Contamination Logs (8 total: 5 activated, 3 deferred) ──
                ["well_contam_tritium_percolation_spike"] = new HydroGeologyRecordMetadata
                {
                    RecordId = "well_contam_tritium_percolation_spike",
                    Family = HydroGeologyRecordFamily.ArtesianWell,
                    Provenance = HydroGeologyProvenanceClass.HistoricalWaterAssay,
                    ContaminantClass = HydroGeologyContaminantClassification.Radionuclide,
                    ProducerId = "room_water_pump",
                    NamedSubject = "DEEP_ARTESIAN_WELL_03",
                    Channel = "room_inspection",
                    MinDay = 1,
                    IsActivated = true,
                    MeasurementProvenance = "4,500.0 Bq/L Tritium (Historical assay at sample time Year 02 Month 09)"
                },
                ["well_contam_strontium_90_limestone_leach"] = new HydroGeologyRecordMetadata
                {
                    RecordId = "well_contam_strontium_90_limestone_leach",
                    Family = HydroGeologyRecordFamily.ArtesianWell,
                    Provenance = HydroGeologyProvenanceClass.HistoricalWaterAssay,
                    ContaminantClass = HydroGeologyContaminantClassification.Radionuclide,
                    ProducerId = "room_filtration",
                    NamedSubject = "COMMISSARY_SUPPLY_WELL_01",
                    Channel = "room_inspection",
                    MinDay = 5,
                    IsActivated = true,
                    MeasurementProvenance = "320.0 Bq/L Strontium-90 (Historical assay at sample time Year 04 Month 11)"
                },
                ["well_contam_ferro_bacterial_iron_slime"] = new HydroGeologyRecordMetadata
                {
                    RecordId = "well_contam_ferro_bacterial_iron_slime",
                    Family = HydroGeologyRecordFamily.ArtesianWell,
                    Provenance = HydroGeologyProvenanceClass.HistoricalWaterAssay,
                    ContaminantClass = HydroGeologyContaminantClassification.BiologicalBiofouling,
                    ProducerId = "location_municipal_water_reservoir",
                    NamedSubject = "HYDRO_POWER_RECIRC_WELL_04",
                    Channel = "expedition_survey",
                    MinDay = 10,
                    IsActivated = true,
                    MeasurementProvenance = "15.0 Bq/L baseline / 600->45 GPM biofouling (Historical assay at sample time Year 06 Month 04)"
                },
                ["well_contam_sub_surface_diesel_plume"] = new HydroGeologyRecordMetadata
                {
                    RecordId = "well_contam_sub_surface_diesel_plume",
                    Family = HydroGeologyRecordFamily.ArtesianWell,
                    Provenance = HydroGeologyProvenanceClass.HistoricalWaterAssay,
                    ContaminantClass = HydroGeologyContaminantClassification.ChemicalToxicant,
                    ProducerId = "location_abandoned_desalination",
                    NamedSubject = "SOUTH_WING_AUXILIARY_WELL_02",
                    Channel = "expedition_survey",
                    MinDay = 15,
                    IsActivated = true,
                    MeasurementProvenance = "0.0 Bq/L (Chemical hydrocarbon plume; historical survey Year 08 Month 07)"
                },
                ["well_contam_radium_226_geothermal_upwelling"] = new HydroGeologyRecordMetadata
                {
                    RecordId = "well_contam_radium_226_geothermal_upwelling",
                    Family = HydroGeologyRecordFamily.ArtesianWell,
                    Provenance = HydroGeologyProvenanceClass.HistoricalWaterAssay,
                    ContaminantClass = HydroGeologyContaminantClassification.Radionuclide,
                    ProducerId = "location_geothermal_borehole_site",
                    NamedSubject = "GEOTHERMAL_REINJECTION_WELL_07",
                    Channel = "borehole_core",
                    MinDay = 20,
                    IsActivated = true,
                    MeasurementProvenance = "2,960.0 Bq/L Radium-226 (Historical assay at sample time Year 11 Month 02)"
                },
                ["well_contam_mine_acid_drainage_inrush"] = new HydroGeologyRecordMetadata
                {
                    RecordId = "well_contam_mine_acid_drainage_inrush",
                    Family = HydroGeologyRecordFamily.ArtesianWell,
                    Provenance = HydroGeologyProvenanceClass.HistoricalWaterAssay,
                    ContaminantClass = HydroGeologyContaminantClassification.ChemicalToxicant,
                    ProducerId = "location_collapsed_salt_mine",
                    NamedSubject = "ABANDONED_DRIFT_MINE_INTAKE",
                    Channel = "expedition_survey",
                    MinDay = 25,
                    IsActivated = false, // Phase 2 deferred: coal drift adit expansion
                    MeasurementProvenance = "85.0 Bq/L, pH 2.9 sulfuric acid (Historical assay Year 13 Month 10)"
                },
                ["well_contam_caustic_lye_decon_runoff"] = new HydroGeologyRecordMetadata
                {
                    RecordId = "well_contam_caustic_lye_decon_runoff",
                    Family = HydroGeologyRecordFamily.ArtesianWell,
                    Provenance = HydroGeologyProvenanceClass.HistoricalWaterAssay,
                    ContaminantClass = HydroGeologyContaminantClassification.ChemicalToxicant,
                    ProducerId = "room_airlock",
                    NamedSubject = "SURFACE_PORTAL_SUMP_WELL_05",
                    Channel = "room_inspection",
                    MinDay = 28,
                    IsActivated = false, // Phase 2 deferred: decon portal drainage expansion
                    MeasurementProvenance = "1,200.0 Bq/L NaOH decon slurry (Historical assay Year 16 Month 05)"
                },
                ["well_contam_cyanide_heap_leach_plume"] = new HydroGeologyRecordMetadata
                {
                    RecordId = "well_contam_cyanide_heap_leach_plume",
                    Family = HydroGeologyRecordFamily.ArtesianWell,
                    Provenance = HydroGeologyProvenanceClass.HistoricalWaterAssay,
                    ContaminantClass = HydroGeologyContaminantClassification.ChemicalToxicant,
                    ProducerId = "location_chemical_plant",
                    NamedSubject = "WEST_RIDGE_DEEP_PRODUCTION_06",
                    Channel = "expedition_survey",
                    MinDay = 30,
                    IsActivated = false, // Phase 2 deferred: west ridge tailings expansion
                    MeasurementProvenance = "0.0 Bq/L (Free potassium cyanide; historical assay Year 19 Month 08)"
                },

                // ── Cave Aquatic Biota Logs (8 total: 5 activated, 3 deferred) ──
                ["biota_cave_eyeless_albino_trout"] = new HydroGeologyRecordMetadata
                {
                    RecordId = "biota_cave_eyeless_albino_trout",
                    Family = HydroGeologyRecordFamily.CaveBiota,
                    Provenance = HydroGeologyProvenanceClass.HistoricalEcologicalSurvey,
                    ProducerId = "location_flooded_subway_depot",
                    NamedSubject = "SALVELINUS_SUBTERRANEUS_ALBA",
                    Channel = "subterranean_recon",
                    MinDay = 12,
                    IsActivated = true,
                    MeasurementProvenance = "Apex troglobite predator observation (Survey Year 03 Bio Survey 01)"
                },
                ["biota_cave_bioluminescent_blue_amphipod"] = new HydroGeologyRecordMetadata
                {
                    RecordId = "biota_cave_bioluminescent_blue_amphipod",
                    Family = HydroGeologyRecordFamily.CaveBiota,
                    Provenance = HydroGeologyProvenanceClass.HistoricalEcologicalSurvey,
                    ProducerId = "location_drainage_network",
                    NamedSubject = "NIPHARGUS_LUCENS_AZUREA",
                    Channel = "subterranean_recon",
                    MinDay = 8,
                    IsActivated = true,
                    MeasurementProvenance = "470nm pulsed luciferin emission (Survey Year 05 Bio Survey 04)"
                },
                ["biota_cave_sulfur_oxidizing_mucilage_veil"] = new HydroGeologyRecordMetadata
                {
                    RecordId = "biota_cave_sulfur_oxidizing_mucilage_veil",
                    Family = HydroGeologyRecordFamily.CaveBiota,
                    Provenance = HydroGeologyProvenanceClass.HistoricalEcologicalSurvey,
                    ProducerId = "location_geo_thermal_plant_ruins",
                    NamedSubject = "THIOBACILLUS_SNOTTITA_ACIDICA",
                    Channel = "expedition_survey",
                    MinDay = 18,
                    IsActivated = true,
                    MeasurementProvenance = "pH 0.8 chemolithotrophic biofilm drips (Survey Year 07 Bio Survey 02)"
                },
                ["biota_cave_blind_troglobitic_crayfish"] = new HydroGeologyRecordMetadata
                {
                    RecordId = "biota_cave_blind_troglobitic_crayfish",
                    Family = HydroGeologyRecordFamily.CaveBiota,
                    Provenance = HydroGeologyProvenanceClass.HistoricalEcologicalSurvey,
                    ProducerId = "location_metro_tunnel",
                    NamedSubject = "ORCONECTES_SUBTERRANEUS_GHOST",
                    Channel = "subterranean_recon",
                    MinDay = 14,
                    IsActivated = true,
                    MeasurementProvenance = "Benthic troglobite scavenger survey (Survey Year 09 Bio Survey 03)"
                },
                ["biota_cave_methanotrophic_algal_mat"] = new HydroGeologyRecordMetadata
                {
                    RecordId = "biota_cave_methanotrophic_algal_mat",
                    Family = HydroGeologyRecordFamily.CaveBiota,
                    Provenance = HydroGeologyProvenanceClass.HistoricalEcologicalSurvey,
                    ProducerId = "location_drainage_network",
                    NamedSubject = "METHYLOCOCCUS_ROSEUS_TROGLO",
                    Channel = "subterranean_recon",
                    MinDay = 24,
                    IsActivated = false, // Phase 2 deferred: deep coal seep expansion
                    MeasurementProvenance = "Methane-oxidizing bacterial mat observation (Survey Year 12 Bio Survey 01)"
                },
                ["biota_cave_phosphorescent_cave_sponge"] = new HydroGeologyRecordMetadata
                {
                    RecordId = "biota_cave_phosphorescent_cave_sponge",
                    Family = HydroGeologyRecordFamily.CaveBiota,
                    Provenance = HydroGeologyProvenanceClass.HistoricalEcologicalSurvey,
                    ProducerId = "location_collapsed_salt_mine",
                    NamedSubject = "SPONGILLA_RADIOACTIVA_AMBER",
                    Channel = "subterranean_recon",
                    MinDay = 22,
                    IsActivated = true,
                    MeasurementProvenance = "Sustained amber mineral phosphorescence (Survey Year 15 Bio Survey 02)"
                },
                ["biota_cave_giant_segmented_leech"] = new HydroGeologyRecordMetadata
                {
                    RecordId = "biota_cave_giant_segmented_leech",
                    Family = HydroGeologyRecordFamily.CaveBiota,
                    Provenance = HydroGeologyProvenanceClass.HistoricalEcologicalSurvey,
                    ProducerId = "location_municipal_water_reservoir",
                    NamedSubject = "HAEMOPIS_GIGAS_CAVERNICOLA",
                    Channel = "expedition_survey",
                    MinDay = 26,
                    IsActivated = false, // Phase 2 deferred: thermal canal sector 6 expansion
                    MeasurementProvenance = "30cm thermal-seeking parasite observation (Survey Year 17 Bio Survey 03)"
                },
                ["biota_cave_sub_glacial_ice_worm_cluster"] = new HydroGeologyRecordMetadata
                {
                    RecordId = "biota_cave_sub_glacial_ice_worm_cluster",
                    Family = HydroGeologyRecordFamily.CaveBiota,
                    Provenance = HydroGeologyProvenanceClass.HistoricalEcologicalSurvey,
                    ProducerId = "location_frozen_wetland",
                    NamedSubject = "MESENCHYTRAEUS_SOLIFUGUS_SUB",
                    Channel = "expedition_survey",
                    MinDay = 28,
                    IsActivated = false, // Phase 2 deferred: glacier dam crevasse expansion
                    MeasurementProvenance = "-3°C psychrophile cryopredator survey (Survey Year 20 Bio Survey 01)"
                },

                // ── Geothermal Steam Vent Diagnostics (7 total: 5 activated, 2 deferred) ──
                ["steam_vent_superheated_nozzle_erosion"] = new HydroGeologyRecordMetadata
                {
                    RecordId = "steam_vent_superheated_nozzle_erosion",
                    Family = HydroGeologyRecordFamily.GeothermalSteam,
                    Provenance = HydroGeologyProvenanceClass.HistoricalEngineeringDiagnostic,
                    ProducerId = "location_geo_thermal_plant_ruins",
                    NamedSubject = "PRIMARY_GEOTHERMAL_HEADER_01",
                    Channel = "facility_log",
                    MinDay = 15,
                    IsActivated = true,
                    MeasurementProvenance = "285.0°C / 42.0 bar (Throttle erosion log Year 03 Steam Diag 01)"
                },
                ["steam_vent_hydrogen_sulfide_stress_cracking"] = new HydroGeologyRecordMetadata
                {
                    RecordId = "steam_vent_hydrogen_sulfide_stress_cracking",
                    Family = HydroGeologyRecordFamily.GeothermalSteam,
                    Provenance = HydroGeologyProvenanceClass.HistoricalEngineeringDiagnostic,
                    ProducerId = "location_geothermal_borehole_site",
                    NamedSubject = "BOREHOLE_CASING_FLANGE_SECTOR_6",
                    Channel = "borehole_core",
                    MinDay = 20,
                    IsActivated = true,
                    MeasurementProvenance = "210.0°C / 35.0 bar, 650 ppm H2S (Flange stress failure Year 06 Steam Diag 03)"
                },
                ["steam_vent_condensate_slug_hammer_shock"] = new HydroGeologyRecordMetadata
                {
                    RecordId = "steam_vent_condensate_slug_hammer_shock",
                    Family = HydroGeologyRecordFamily.GeothermalSteam,
                    Provenance = HydroGeologyProvenanceClass.HistoricalEngineeringDiagnostic,
                    ProducerId = "loc_cluster_steam_substation",
                    NamedSubject = "MAIN_TRANSMISSION_RISER_LEVEL_3",
                    Channel = "facility_log",
                    MinDay = 10,
                    IsActivated = true,
                    MeasurementProvenance = "175.0°C / 28.0 bar, 50L slug water hammer (Diagnostic Year 09 Steam Diag 02)"
                },
                ["steam_vent_silica_scaling_throttle_constriction"] = new HydroGeologyRecordMetadata
                {
                    RecordId = "steam_vent_silica_scaling_throttle_constriction",
                    Family = HydroGeologyRecordFamily.GeothermalSteam,
                    Provenance = HydroGeologyProvenanceClass.HistoricalEngineeringDiagnostic,
                    ProducerId = "room_main",
                    NamedSubject = "FLASH_VESSEL_DISCHARGE_NOZZLE",
                    Channel = "room_inspection",
                    MinDay = 6,
                    IsActivated = true,
                    MeasurementProvenance = "140.0°C / 12.5 bar, 60% silica constriction (Diagnostic Year 12 Steam Diag 04)"
                },
                ["steam_vent_geothermal_wellhead_subsidence"] = new HydroGeologyRecordMetadata
                {
                    RecordId = "steam_vent_geothermal_wellhead_subsidence",
                    Family = HydroGeologyRecordFamily.GeothermalSteam,
                    Provenance = HydroGeologyProvenanceClass.HistoricalEngineeringDiagnostic,
                    ProducerId = "location_geo_thermal_plant_ruins",
                    NamedSubject = "PRODUCTION_WELL_CLUSTER_B",
                    Channel = "facility_log",
                    MinDay = 25,
                    IsActivated = true,
                    MeasurementProvenance = "240.0°C / 38.0 bar, 15cm subsidence / 4° tilt (Diagnostic Year 15 Steam Diag 01)"
                },
                ["steam_vent_acoustic_jet_screamer_attenuation"] = new HydroGeologyRecordMetadata
                {
                    RecordId = "steam_vent_acoustic_jet_screamer_attenuation",
                    Family = HydroGeologyRecordFamily.GeothermalSteam,
                    Provenance = HydroGeologyProvenanceClass.HistoricalEngineeringDiagnostic,
                    ProducerId = "location_geo_thermal_plant_ruins",
                    NamedSubject = "EMERGENCY_ATMOSPHERIC_BLOWDOWN_D",
                    Channel = "facility_log",
                    MinDay = 27,
                    IsActivated = false, // Phase 2 deferred: high-altitude atmospheric relief stack
                    MeasurementProvenance = "260.0°C / 45.0 bar, 138 dB acoustic jet (Diagnostic Year 17 Steam Diag 02)"
                },
                ["steam_vent_non_condensable_gas_vent_fire"] = new HydroGeologyRecordMetadata
                {
                    RecordId = "steam_vent_non_condensable_gas_vent_fire",
                    Family = HydroGeologyRecordFamily.GeothermalSteam,
                    Provenance = HydroGeologyProvenanceClass.HistoricalEngineeringDiagnostic,
                    ProducerId = "loc_cluster_steam_substation",
                    NamedSubject = "VACUUM_EJECTOR_EXHAUST_STACK",
                    Channel = "facility_log",
                    MinDay = 29,
                    IsActivated = false, // Phase 2 deferred: vacuum ejector flare platform
                    MeasurementProvenance = "95.0°C / 1.2 bar, 8% CH4 / 4% H2 flare (Diagnostic Year 20 Steam Diag 01)"
                },

                // ── Stalactite Mineral Assay Reports (7 total: 5 activated, 2 deferred) ──
                ["stalactite_assay_uranophane_canary_crust"] = new HydroGeologyRecordMetadata
                {
                    RecordId = "stalactite_assay_uranophane_canary_crust",
                    Family = HydroGeologyRecordFamily.StalactiteMineral,
                    Provenance = HydroGeologyProvenanceClass.HistoricalMineralAssay,
                    ProducerId = "location_collapsed_salt_mine",
                    NamedSubject = "SPELEO-MINERAL-ASSAY-009",
                    Channel = "subterranean_recon",
                    MinDay = 16,
                    IsActivated = true,
                    MeasurementProvenance = "52.4% metal assay, 8,400.0 µR/hr (Assay Year 04 Mineral Assay)"
                },
                ["stalactite_assay_malachite_copper_dripstone"] = new HydroGeologyRecordMetadata
                {
                    RecordId = "stalactite_assay_malachite_copper_dripstone",
                    Family = HydroGeologyRecordFamily.StalactiteMineral,
                    Provenance = HydroGeologyProvenanceClass.HistoricalMineralAssay,
                    ProducerId = "location_flooded_subway_depot",
                    NamedSubject = "SPELEO-MINERAL-ASSAY-023",
                    Channel = "subterranean_recon",
                    MinDay = 12,
                    IsActivated = true,
                    MeasurementProvenance = "57.5% Cu assay, 12.0 µR/hr (Assay Year 07 Mineral Assay)"
                },
                ["stalactite_assay_galena_lead_soda_straw"] = new HydroGeologyRecordMetadata
                {
                    RecordId = "stalactite_assay_galena_lead_soda_straw",
                    Family = HydroGeologyRecordFamily.StalactiteMineral,
                    Provenance = HydroGeologyProvenanceClass.HistoricalMineralAssay,
                    ProducerId = "location_drainage_network",
                    NamedSubject = "SPELEO-MINERAL-ASSAY-041",
                    Channel = "subterranean_recon",
                    MinDay = 14,
                    IsActivated = true,
                    MeasurementProvenance = "86.6% Pb assay, 45.0 µR/hr (Assay Year 10 Mineral Assay)"
                },
                ["stalactite_assay_cinnabar_mercury_globule_seep"] = new HydroGeologyRecordMetadata
                {
                    RecordId = "stalactite_assay_cinnabar_mercury_globule_seep",
                    Family = HydroGeologyRecordFamily.StalactiteMineral,
                    Provenance = HydroGeologyProvenanceClass.HistoricalMineralAssay,
                    ProducerId = "location_metro_tunnel",
                    NamedSubject = "SPELEO-MINERAL-ASSAY-058",
                    Channel = "subterranean_recon",
                    MinDay = 18,
                    IsActivated = true,
                    MeasurementProvenance = "82.0% Hg assay, 8.0 µR/hr (Assay Year 13 Mineral Assay)"
                },
                ["stalactite_assay_arsenopyrite_garlic_exhalation"] = new HydroGeologyRecordMetadata
                {
                    RecordId = "stalactite_assay_arsenopyrite_garlic_exhalation",
                    Family = HydroGeologyRecordFamily.StalactiteMineral,
                    Provenance = HydroGeologyProvenanceClass.HistoricalMineralAssay,
                    ProducerId = "location_collapsed_salt_mine",
                    NamedSubject = "SPELEO-MINERAL-ASSAY-076",
                    Channel = "subterranean_recon",
                    MinDay = 26,
                    IsActivated = false, // Phase 2 deferred: deep shale joint expedition
                    MeasurementProvenance = "46.0% Fe-As assay, 25.0 µR/hr (Assay Year 16 Mineral Assay)"
                },
                ["stalactite_assay_fluorite_purple_cubic_drusy"] = new HydroGeologyRecordMetadata
                {
                    RecordId = "stalactite_assay_fluorite_purple_cubic_drusy",
                    Family = HydroGeologyRecordFamily.StalactiteMineral,
                    Provenance = HydroGeologyProvenanceClass.HistoricalMineralAssay,
                    ProducerId = "location_geothermal_borehole_site",
                    NamedSubject = "SPELEO-MINERAL-ASSAY-092",
                    Channel = "borehole_core",
                    MinDay = 24,
                    IsActivated = true,
                    MeasurementProvenance = "51.3% Ca assay, 180.0 µR/hr (Assay Year 18 Mineral Assay)"
                },
                ["stalactite_assay_autunite_green_fluorescent_blade"] = new HydroGeologyRecordMetadata
                {
                    RecordId = "stalactite_assay_autunite_green_fluorescent_blade",
                    Family = HydroGeologyRecordFamily.StalactiteMineral,
                    Provenance = HydroGeologyProvenanceClass.HistoricalMineralAssay,
                    ProducerId = "location_collapsed_salt_mine",
                    NamedSubject = "SPELEO-MINERAL-ASSAY-110",
                    Channel = "subterranean_recon",
                    MinDay = 30,
                    IsActivated = false, // Phase 2 deferred: hot zone drainage sump expedition
                    MeasurementProvenance = "48.2% U assay, 14,200.0 µR/hr (Assay Year 20 Mineral Assay)"
                }
            };

        public static HydroGeologyRecordMetadata? ResolveMetadata(string recordId)
        {
            if (string.IsNullOrEmpty(recordId)) return null;
            return Records.TryGetValue(recordId, out var meta) ? meta : null;
        }

        public static HydroGeologyRecordFamily ResolveFamily(string recordId)
        {
            if (string.IsNullOrEmpty(recordId)) return HydroGeologyRecordFamily.ArtesianWell;
            if (Records.TryGetValue(recordId, out var meta)) return meta.Family;
            if (recordId.StartsWith("well_contam_", StringComparison.OrdinalIgnoreCase)) return HydroGeologyRecordFamily.ArtesianWell;
            if (recordId.StartsWith("biota_cave_", StringComparison.OrdinalIgnoreCase)) return HydroGeologyRecordFamily.CaveBiota;
            if (recordId.StartsWith("steam_vent_", StringComparison.OrdinalIgnoreCase)) return HydroGeologyRecordFamily.GeothermalSteam;
            if (recordId.StartsWith("stalactite_assay_", StringComparison.OrdinalIgnoreCase)) return HydroGeologyRecordFamily.StalactiteMineral;
            return HydroGeologyRecordFamily.ArtesianWell;
        }

        public static HydroGeologyProvenanceClass ResolveProvenance(string recordId)
        {
            if (string.IsNullOrEmpty(recordId)) return HydroGeologyProvenanceClass.AmbiguousRequiresReconciliation;
            if (Records.TryGetValue(recordId, out var meta)) return meta.Provenance;
            return HydroGeologyProvenanceClass.AmbiguousRequiresReconciliation;
        }

        public static string ResolveProducer(string recordId)
        {
            if (string.IsNullOrEmpty(recordId)) return "room_water_pump";
            if (Records.TryGetValue(recordId, out var meta)) return meta.ProducerId;
            return "room_water_pump";
        }

        public static int ResolveMinDay(string recordId)
        {
            if (string.IsNullOrEmpty(recordId)) return 1;
            if (Records.TryGetValue(recordId, out var meta)) return meta.MinDay;
            return 1;
        }

        public static bool IsActivated(string recordId)
        {
            if (string.IsNullOrEmpty(recordId)) return false;
            if (Records.TryGetValue(recordId, out var meta)) return meta.IsActivated;
            return false;
        }

        public static IReadOnlyList<HydroGeologyRecordMetadata> GetAllMetadata()
        {
            return new List<HydroGeologyRecordMetadata>(Records.Values);
        }

        public static IReadOnlyList<HydroGeologyRecordMetadata> GetActiveRecordsForProducer(string producerId)
        {
            var list = new List<HydroGeologyRecordMetadata>();
            if (string.IsNullOrEmpty(producerId)) return list;

            foreach (var kvp in Records)
            {
                if (kvp.Value.IsActivated && string.Equals(kvp.Value.ProducerId, producerId, StringComparison.OrdinalIgnoreCase))
                {
                    list.Add(kvp.Value);
                }
            }
            return list;
        }

        public static IReadOnlyList<HydroGeologyRecordMetadata> GetActivatedRecords()
        {
            var list = new List<HydroGeologyRecordMetadata>();
            foreach (var kvp in Records)
            {
                if (kvp.Value.IsActivated) list.Add(kvp.Value);
            }
            return list;
        }

        public static IReadOnlyList<HydroGeologyRecordMetadata> GetDeferredRecords()
        {
            var list = new List<HydroGeologyRecordMetadata>();
            foreach (var kvp in Records)
            {
                if (!kvp.Value.IsActivated) list.Add(kvp.Value);
            }
            return list;
        }
    }
}
