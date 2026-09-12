// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Narrative
{
    public enum AbyssalRecordFamily
    {
        HydrophoneAcoustic,
        GeothermalBorehole,
        CryopodFailure,
        SaltMineInscription
    }

    public enum AbyssalProvenanceClass
    {
        HistoricalInstrumentRecord,
        HistoricalEngineeringRecord,
        HistoricalIncidentRecord,
        PhysicalInscriptionTestimony,
        AmbiguousRequiresReconciliation
    }

    public sealed class AbyssalRecordMetadata
    {
        public string RecordId { get; set; } = string.Empty;
        public AbyssalRecordFamily Family { get; set; }
        public AbyssalProvenanceClass Provenance { get; set; }
        public string ProducerId { get; set; } = string.Empty;
        public string Channel { get; set; } = "location_inspection";
        public int MinDay { get; set; } = 1;
        public bool IsActivated { get; set; }
    }

    /// <summary>
    /// Plan 151: Projection and discovery mapping for the Abyssal Anomalies science archive.
    /// Connects historical telemetry, incident reports, and mine inscriptions to canonical
    /// producers without allowing historical values to mutate live simulation states.
    /// </summary>
    public static class AbyssalAnomaliesProjection
    {
        private static readonly Dictionary<string, AbyssalRecordMetadata> Records =
            new Dictionary<string, AbyssalRecordMetadata>(StringComparer.OrdinalIgnoreCase)
            {
                // ── Hydrophone Acoustic Logs (8) ──
                ["hydrophone_shelf_ice_calving_echo"] = new AbyssalRecordMetadata
                {
                    RecordId = "hydrophone_shelf_ice_calving_echo",
                    Family = AbyssalRecordFamily.HydrophoneAcoustic,
                    Provenance = AbyssalProvenanceClass.HistoricalInstrumentRecord,
                    ProducerId = "loc_ice_core_store",
                    Channel = "location_inspection",
                    MinDay = 20,
                    IsActivated = true
                },
                ["hydrophone_submarine_cavitation_ghost"] = new AbyssalRecordMetadata
                {
                    RecordId = "hydrophone_submarine_cavitation_ghost",
                    Family = AbyssalRecordFamily.HydrophoneAcoustic,
                    Provenance = AbyssalProvenanceClass.HistoricalInstrumentRecord,
                    ProducerId = "loc_bathymetric_boat",
                    Channel = "location_inspection",
                    MinDay = 30,
                    IsActivated = true
                },
                ["hydrophone_deep_trench_thermal_vent"] = new AbyssalRecordMetadata
                {
                    RecordId = "hydrophone_deep_trench_thermal_vent",
                    Family = AbyssalRecordFamily.HydrophoneAcoustic,
                    Provenance = AbyssalProvenanceClass.HistoricalInstrumentRecord,
                    ProducerId = "location_geo_thermal_plant_ruins",
                    Channel = "location_inspection",
                    MinDay = 25,
                    IsActivated = true
                },
                ["hydrophone_sunken_freighter_bulkhead_collapse"] = new AbyssalRecordMetadata
                {
                    RecordId = "hydrophone_sunken_freighter_bulkhead_collapse",
                    Family = AbyssalRecordFamily.HydrophoneAcoustic,
                    Provenance = AbyssalProvenanceClass.HistoricalInstrumentRecord,
                    ProducerId = "loc_cold_store_atlantic",
                    Channel = "location_inspection",
                    MinDay = 15,
                    IsActivated = true
                },
                ["hydrophone_biological_benthos_clicks"] = new AbyssalRecordMetadata
                {
                    RecordId = "hydrophone_biological_benthos_clicks",
                    Family = AbyssalRecordFamily.HydrophoneAcoustic,
                    Provenance = AbyssalProvenanceClass.HistoricalInstrumentRecord,
                    ProducerId = "loc_bathymetric_boat",
                    Channel = "location_inspection",
                    MinDay = 45,
                    IsActivated = true
                },
                ["hydrophone_coastal_minefield_chain_drag"] = new AbyssalRecordMetadata
                {
                    RecordId = "hydrophone_coastal_minefield_chain_drag",
                    Family = AbyssalRecordFamily.HydrophoneAcoustic,
                    Provenance = AbyssalProvenanceClass.HistoricalInstrumentRecord,
                    ProducerId = "loc_lock_gate_four",
                    Channel = "location_inspection",
                    MinDay = 35,
                    IsActivated = false // Phase 2 deferred
                },
                ["hydrophone_active_sonar_orphan_ping"] = new AbyssalRecordMetadata
                {
                    RecordId = "hydrophone_active_sonar_orphan_ping",
                    Family = AbyssalRecordFamily.HydrophoneAcoustic,
                    Provenance = AbyssalProvenanceClass.HistoricalInstrumentRecord,
                    ProducerId = "room_radio_tuner",
                    Channel = "radio_archive",
                    MinDay = 40,
                    IsActivated = false // Phase 2 deferred
                },
                ["hydrophone_glacial_earthquake_harmonic"] = new AbyssalRecordMetadata
                {
                    RecordId = "hydrophone_glacial_earthquake_harmonic",
                    Family = AbyssalRecordFamily.HydrophoneAcoustic,
                    Provenance = AbyssalProvenanceClass.HistoricalInstrumentRecord,
                    ProducerId = "loc_snowline_station",
                    Channel = "location_inspection",
                    MinDay = 50,
                    IsActivated = false // Phase 2 deferred
                },

                // ── Geothermal Borehole Logs (7) ──
                ["borehole_magma_boundary_temperature_spike"] = new AbyssalRecordMetadata
                {
                    RecordId = "borehole_magma_boundary_temperature_spike",
                    Family = AbyssalRecordFamily.GeothermalBorehole,
                    Provenance = AbyssalProvenanceClass.HistoricalEngineeringRecord,
                    ProducerId = "location_geo_thermal_plant_ruins",
                    Channel = "location_inspection",
                    MinDay = 10,
                    IsActivated = true
                },
                ["borehole_sulfur_steam_vent_corrosion"] = new AbyssalRecordMetadata
                {
                    RecordId = "borehole_sulfur_steam_vent_corrosion",
                    Family = AbyssalRecordFamily.GeothermalBorehole,
                    Provenance = AbyssalProvenanceClass.HistoricalEngineeringRecord,
                    ProducerId = "room_filtration",
                    Channel = "location_inspection",
                    MinDay = 18,
                    IsActivated = true
                },
                ["borehole_seismic_fault_hydraulic_pulse"] = new AbyssalRecordMetadata
                {
                    RecordId = "borehole_seismic_fault_hydraulic_pulse",
                    Family = AbyssalRecordFamily.GeothermalBorehole,
                    Provenance = AbyssalProvenanceClass.HistoricalEngineeringRecord,
                    ProducerId = "room_water_pump",
                    Channel = "location_inspection",
                    MinDay = 22,
                    IsActivated = true
                },
                ["borehole_heavy_metal_brine_precipitate"] = new AbyssalRecordMetadata
                {
                    RecordId = "borehole_heavy_metal_brine_precipitate",
                    Family = AbyssalRecordFamily.GeothermalBorehole,
                    Provenance = AbyssalProvenanceClass.HistoricalEngineeringRecord,
                    ProducerId = "location_the_sump_cathedral",
                    Channel = "location_inspection",
                    MinDay = 28,
                    IsActivated = true
                },
                ["borehole_downhole_drillstring_seizure"] = new AbyssalRecordMetadata
                {
                    RecordId = "borehole_downhole_drillstring_seizure",
                    Family = AbyssalRecordFamily.GeothermalBorehole,
                    Provenance = AbyssalProvenanceClass.HistoricalEngineeringRecord,
                    ProducerId = "room_workshop",
                    Channel = "location_inspection",
                    MinDay = 32,
                    IsActivated = false // Phase 2 deferred
                },
                ["borehole_radon_gas_outgassing_surge"] = new AbyssalRecordMetadata
                {
                    RecordId = "borehole_radon_gas_outgassing_surge",
                    Family = AbyssalRecordFamily.GeothermalBorehole,
                    Provenance = AbyssalProvenanceClass.HistoricalEngineeringRecord,
                    ProducerId = "room_filtration_stack",
                    Channel = "location_inspection",
                    MinDay = 25,
                    IsActivated = false // Phase 2 deferred
                },
                ["borehole_acoustic_resonator_whisper"] = new AbyssalRecordMetadata
                {
                    RecordId = "borehole_acoustic_resonator_whisper",
                    Family = AbyssalRecordFamily.GeothermalBorehole,
                    Provenance = AbyssalProvenanceClass.HistoricalEngineeringRecord,
                    ProducerId = "location_the_sump_cathedral",
                    Channel = "location_inspection",
                    MinDay = 50,
                    IsActivated = false // Phase 2 deferred
                },

                // ── Cryopod Failure Logs (8) ──
                ["cryopod_coolant_circuit_boiloff"] = new AbyssalRecordMetadata
                {
                    RecordId = "cryopod_coolant_circuit_boiloff",
                    Family = AbyssalRecordFamily.CryopodFailure,
                    Provenance = AbyssalProvenanceClass.HistoricalIncidentRecord,
                    ProducerId = "government_bunker",
                    Channel = "library_terminal",
                    MinDay = 15,
                    IsActivated = true
                },
                ["cryopod_vitrification_crystallization_error"] = new AbyssalRecordMetadata
                {
                    RecordId = "cryopod_vitrification_crystallization_error",
                    Family = AbyssalRecordFamily.CryopodFailure,
                    Provenance = AbyssalProvenanceClass.HistoricalIncidentRecord,
                    ProducerId = "government_bunker",
                    Channel = "library_terminal",
                    MinDay = 25,
                    IsActivated = true
                },
                ["cryopod_neural_eeg_spike_nightmare"] = new AbyssalRecordMetadata
                {
                    RecordId = "cryopod_neural_eeg_spike_nightmare",
                    Family = AbyssalRecordFamily.CryopodFailure,
                    Provenance = AbyssalProvenanceClass.HistoricalIncidentRecord,
                    ProducerId = "loc_low_background_lab",
                    Channel = "location_inspection",
                    MinDay = 35,
                    IsActivated = true
                },
                ["cryopod_perfusion_pump_rotor_jam"] = new AbyssalRecordMetadata
                {
                    RecordId = "cryopod_perfusion_pump_rotor_jam",
                    Family = AbyssalRecordFamily.CryopodFailure,
                    Provenance = AbyssalProvenanceClass.HistoricalIncidentRecord,
                    ProducerId = "abandoned_hospital",
                    Channel = "location_inspection",
                    MinDay = 20,
                    IsActivated = true
                },
                ["cryopod_biometric_subject_identity_corruption"] = new AbyssalRecordMetadata
                {
                    RecordId = "cryopod_biometric_subject_identity_corruption",
                    Family = AbyssalRecordFamily.CryopodFailure,
                    Provenance = AbyssalProvenanceClass.HistoricalIncidentRecord,
                    ProducerId = "loc_records_annex",
                    Channel = "location_inspection",
                    MinDay = 30,
                    IsActivated = false // Phase 2 deferred
                },
                ["cryopod_desperate_manual_thaw_breach"] = new AbyssalRecordMetadata
                {
                    RecordId = "cryopod_desperate_manual_thaw_breach",
                    Family = AbyssalRecordFamily.CryopodFailure,
                    Provenance = AbyssalProvenanceClass.HistoricalIncidentRecord,
                    ProducerId = "government_bunker",
                    Channel = "library_terminal",
                    MinDay = 40,
                    IsActivated = false // Phase 2 deferred
                },
                ["cryopod_power_shedding_priority_cascade"] = new AbyssalRecordMetadata
                {
                    RecordId = "cryopod_power_shedding_priority_cascade",
                    Family = AbyssalRecordFamily.CryopodFailure,
                    Provenance = AbyssalProvenanceClass.HistoricalIncidentRecord,
                    ProducerId = "government_bunker",
                    Channel = "library_terminal",
                    MinDay = 45,
                    IsActivated = false // Phase 2 deferred
                },
                ["cryopod_terminal_euthanasia_protocol"] = new AbyssalRecordMetadata
                {
                    RecordId = "cryopod_terminal_euthanasia_protocol",
                    Family = AbyssalRecordFamily.CryopodFailure,
                    Provenance = AbyssalProvenanceClass.HistoricalIncidentRecord,
                    ProducerId = "government_bunker",
                    Channel = "library_terminal",
                    MinDay = 55,
                    IsActivated = false // Phase 2 deferred
                },

                // ── Salt Mine Inscriptions (7) ──
                ["salt_mine_shaft_tally_340_days"] = new AbyssalRecordMetadata
                {
                    RecordId = "salt_mine_shaft_tally_340_days",
                    Family = AbyssalRecordFamily.SaltMineInscription,
                    Provenance = AbyssalProvenanceClass.PhysicalInscriptionTestimony,
                    ProducerId = "location_the_sump_cathedral",
                    Channel = "location_inspection",
                    MinDay = 8,
                    IsActivated = true
                },
                ["salt_mine_brine_spring_warning"] = new AbyssalRecordMetadata
                {
                    RecordId = "salt_mine_brine_spring_warning",
                    Family = AbyssalRecordFamily.SaltMineInscription,
                    Provenance = AbyssalProvenanceClass.PhysicalInscriptionTestimony,
                    ProducerId = "location_the_sump_cathedral",
                    Channel = "location_inspection",
                    MinDay = 16,
                    IsActivated = true
                },
                ["salt_mine_blind_mule_memorial"] = new AbyssalRecordMetadata
                {
                    RecordId = "salt_mine_blind_mule_memorial",
                    Family = AbyssalRecordFamily.SaltMineInscription,
                    Provenance = AbyssalProvenanceClass.PhysicalInscriptionTestimony,
                    ProducerId = "location_the_sump_cathedral",
                    Channel = "location_inspection",
                    MinDay = 12,
                    IsActivated = true
                },
                ["salt_mine_airlock_collapse_last_words"] = new AbyssalRecordMetadata
                {
                    RecordId = "salt_mine_airlock_collapse_last_words",
                    Family = AbyssalRecordFamily.SaltMineInscription,
                    Provenance = AbyssalProvenanceClass.PhysicalInscriptionTestimony,
                    ProducerId = "room_airlock",
                    Channel = "location_inspection",
                    MinDay = 24,
                    IsActivated = true
                },
                ["salt_mine_crystalline_shrine_vow"] = new AbyssalRecordMetadata
                {
                    RecordId = "salt_mine_crystalline_shrine_vow",
                    Family = AbyssalRecordFamily.SaltMineInscription,
                    Provenance = AbyssalProvenanceClass.PhysicalInscriptionTestimony,
                    ProducerId = "loc_the_vessels_cell",
                    Channel = "location_inspection",
                    MinDay = 35,
                    IsActivated = false // Phase 2 deferred
                },
                ["salt_mine_methane_pocket_marker"] = new AbyssalRecordMetadata
                {
                    RecordId = "salt_mine_methane_pocket_marker",
                    Family = AbyssalRecordFamily.SaltMineInscription,
                    Provenance = AbyssalProvenanceClass.PhysicalInscriptionTestimony,
                    ProducerId = "loc_pump_station_nine",
                    Channel = "location_inspection",
                    MinDay = 22,
                    IsActivated = false // Phase 2 deferred
                },
                ["salt_mine_scrip_forgery_workshop_graffiti"] = new AbyssalRecordMetadata
                {
                    RecordId = "salt_mine_scrip_forgery_workshop_graffiti",
                    Family = AbyssalRecordFamily.SaltMineInscription,
                    Provenance = AbyssalProvenanceClass.PhysicalInscriptionTestimony,
                    ProducerId = "stranger_cache",
                    Channel = "location_inspection",
                    MinDay = 20,
                    IsActivated = false // Phase 2 deferred
                }
            };

        public static AbyssalRecordFamily ResolveFamily(string recordId)
        {
            if (string.IsNullOrEmpty(recordId)) return AbyssalRecordFamily.HydrophoneAcoustic;
            if (Records.TryGetValue(recordId, out var meta)) return meta.Family;
            if (recordId.StartsWith("hydrophone_", StringComparison.OrdinalIgnoreCase)) return AbyssalRecordFamily.HydrophoneAcoustic;
            if (recordId.StartsWith("borehole_", StringComparison.OrdinalIgnoreCase)) return AbyssalRecordFamily.GeothermalBorehole;
            if (recordId.StartsWith("cryopod_", StringComparison.OrdinalIgnoreCase)) return AbyssalRecordFamily.CryopodFailure;
            if (recordId.StartsWith("salt_mine_", StringComparison.OrdinalIgnoreCase)) return AbyssalRecordFamily.SaltMineInscription;
            return AbyssalRecordFamily.HydrophoneAcoustic;
        }

        public static AbyssalProvenanceClass ResolveProvenance(string recordId)
        {
            if (string.IsNullOrEmpty(recordId)) return AbyssalProvenanceClass.AmbiguousRequiresReconciliation;
            if (Records.TryGetValue(recordId, out var meta)) return meta.Provenance;
            return AbyssalProvenanceClass.AmbiguousRequiresReconciliation;
        }

        public static string ResolveProducer(string recordId)
        {
            if (string.IsNullOrEmpty(recordId)) return "location_geo_thermal_plant_ruins";
            if (Records.TryGetValue(recordId, out var meta)) return meta.ProducerId;
            return "location_geo_thermal_plant_ruins";
        }

        public static string ResolveChannel(string recordId)
        {
            if (string.IsNullOrEmpty(recordId)) return "location_inspection";
            if (Records.TryGetValue(recordId, out var meta)) return meta.Channel;
            return "location_inspection";
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

        public static List<AbyssalRecordMetadata> GetAllMetadata()
        {
            return new List<AbyssalRecordMetadata>(Records.Values);
        }

        public static List<AbyssalRecordMetadata> GetActiveRecordsForProducer(string producerId)
        {
            var list = new List<AbyssalRecordMetadata>();
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
    }
}
