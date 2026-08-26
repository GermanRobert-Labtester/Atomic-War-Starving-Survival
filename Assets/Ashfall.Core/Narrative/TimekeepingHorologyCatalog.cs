using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class DeadbeatEscapementWearEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("clock_mechanism_id")]
        public string ClockMechanismId { get; set; } = string.Empty;

        [JsonPropertyName("escapement_type")]
        public string EscapementType { get; set; } = string.Empty;

        [JsonPropertyName("daily_rate_drift_seconds")]
        public float DailyRateDriftSeconds { get; set; }

        [JsonPropertyName("wear_depth_microns")]
        public float WearDepthMicrons { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class InvarPendulumThermalEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("pendulum_assembly_id")]
        public string PendulumAssemblyId { get; set; } = string.Empty;

        [JsonPropertyName("rod_material_alloy")]
        public string RodMaterialAlloy { get; set; } = string.Empty;

        [JsonPropertyName("thermal_expansion_coefficient_ppm_k")]
        public float ThermalExpansionCoefficientPpmK { get; set; }

        [JsonPropertyName("bob_mass_kg")]
        public float BobMassKg { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class MainspringFatigueRuptureEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("spring_barrel_id")]
        public string SpringBarrelId { get; set; } = string.Empty;

        [JsonPropertyName("spring_alloy_type")]
        public string SpringAlloyType { get; set; } = string.Empty;

        [JsonPropertyName("full_windup_torque_nm")]
        public float FullWindupTorqueNm { get; set; }

        [JsonPropertyName("failure_cycle_count")]
        public int FailureCycleCount { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class ClepsydraWaterClockEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("clepsydra_station_id")]
        public string ClepsydraStationId { get; set; } = string.Empty;

        [JsonPropertyName("orifice_material_type")]
        public string OrificeMaterialType { get; set; } = string.Empty;

        [JsonPropertyName("nominal_flow_ml_min")]
        public float NominalFlowMlMin { get; set; }

        [JsonPropertyName("viscosity_error_pct")]
        public float ViscosityErrorPct { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class TimekeepingHorologyCatalog
    {
        private readonly List<DeadbeatEscapementWearEntry> _escapementEntries = new List<DeadbeatEscapementWearEntry>();
        private readonly List<InvarPendulumThermalEntry> _pendulumEntries = new List<InvarPendulumThermalEntry>();
        private readonly List<MainspringFatigueRuptureEntry> _springEntries = new List<MainspringFatigueRuptureEntry>();
        private readonly List<ClepsydraWaterClockEntry> _waterEntries = new List<ClepsydraWaterClockEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<DeadbeatEscapementWearEntry> EscapementEntries => _escapementEntries;
        public IReadOnlyList<InvarPendulumThermalEntry> PendulumEntries => _pendulumEntries;
        public IReadOnlyList<MainspringFatigueRuptureEntry> SpringEntries => _springEntries;
        public IReadOnlyList<ClepsydraWaterClockEntry> WaterEntries => _waterEntries;

        public int TotalCount => _escapementEntries.Count + _pendulumEntries.Count + _springEntries.Count + _waterEntries.Count;

        public static TimekeepingHorologyCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new TimekeepingHorologyCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Deadbeat Escapement Pallet Wear Logs
            string escPath = Path.Combine(directoryPath, "deadbeat_escapement_wear_logs.json");
            if (File.Exists(escPath))
            {
                var list = CatalogLocator.LoadWrappedList<DeadbeatEscapementWearEntry>(File.ReadAllText(escPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._escapementEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. Invar Pendulum Thermal Expansion Reports
            string pendPath = Path.Combine(directoryPath, "invar_pendulum_thermal_expansion.json");
            if (File.Exists(pendPath))
            {
                var list = CatalogLocator.LoadWrappedList<InvarPendulumThermalEntry>(File.ReadAllText(pendPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._pendulumEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Mainspring Fatigue Rupture Audits
            string springPath = Path.Combine(directoryPath, "mainspring_fatigue_rupture_audits.json");
            if (File.Exists(springPath))
            {
                var list = CatalogLocator.LoadWrappedList<MainspringFatigueRuptureEntry>(File.ReadAllText(springPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._springEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Clepsydra Water Clock Silt & Orifice Records
            string waterPath = Path.Combine(directoryPath, "water_clock_orifice_silt_records.json");
            if (File.Exists(waterPath))
            {
                var list = CatalogLocator.LoadWrappedList<ClepsydraWaterClockEntry>(File.ReadAllText(waterPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._waterEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public DeadbeatEscapementWearEntry? GetEscapement(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is DeadbeatEscapementWearEntry e ? e : null;
        }

        public InvarPendulumThermalEntry? GetPendulum(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is InvarPendulumThermalEntry e ? e : null;
        }

        public MainspringFatigueRuptureEntry? GetSpring(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is MainspringFatigueRuptureEntry e ? e : null;
        }

        public ClepsydraWaterClockEntry? GetWater(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is ClepsydraWaterClockEntry e ? e : null;
        }
    }
}
