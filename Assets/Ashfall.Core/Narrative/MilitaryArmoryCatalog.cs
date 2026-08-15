using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class AmmoHoistJamEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("turret_emplacement_id")]
        public string TurretEmplacementId { get; set; } = string.Empty;

        [JsonPropertyName("caliber_designation")]
        public string CaliberDesignation { get; set; } = string.Empty;

        [JsonPropertyName("hoist_mechanism_type")]
        public string HoistMechanismType { get; set; } = string.Empty;

        [JsonPropertyName("jam_classification")]
        public string JamClassification { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class MunitionsLeachingEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("magazine_vault_id")]
        public string MagazineVaultId { get; set; } = string.Empty;

        [JsonPropertyName("chemical_agent")]
        public string ChemicalAgent { get; set; } = string.Empty;

        [JsonPropertyName("storage_temperature_celsius")]
        public float StorageTemperatureCelsius { get; set; }

        [JsonPropertyName("hazard_tier")]
        public string HazardTier { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class SonarArrayFaultEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("hydrophone_station_id")]
        public string HydrophoneStationId { get; set; } = string.Empty;

        [JsonPropertyName("transducer_element_type")]
        public string TransducerElementType { get; set; } = string.Empty;

        [JsonPropertyName("attenuation_loss_db")]
        public float AttenuationLossDb { get; set; }

        [JsonPropertyName("failure_classification")]
        public string FailureClassification { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class VaultSealBreachEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("vault_sector_id")]
        public string VaultSectorId { get; set; } = string.Empty;

        [JsonPropertyName("barrier_material")]
        public string BarrierMaterial { get; set; } = string.Empty;

        [JsonPropertyName("breach_technique")]
        public string BreachTechnique { get; set; } = string.Empty;

        [JsonPropertyName("breach_time_minutes")]
        public float BreachTimeMinutes { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class MilitaryArmoryCatalog
    {
        private readonly List<AmmoHoistJamEntry> _hoistEntries = new List<AmmoHoistJamEntry>();
        private readonly List<MunitionsLeachingEntry> _munitionsEntries = new List<MunitionsLeachingEntry>();
        private readonly List<SonarArrayFaultEntry> _sonarEntries = new List<SonarArrayFaultEntry>();
        private readonly List<VaultSealBreachEntry> _breachEntries = new List<VaultSealBreachEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<AmmoHoistJamEntry> HoistEntries => _hoistEntries;
        public IReadOnlyList<MunitionsLeachingEntry> MunitionsEntries => _munitionsEntries;
        public IReadOnlyList<SonarArrayFaultEntry> SonarEntries => _sonarEntries;
        public IReadOnlyList<VaultSealBreachEntry> BreachEntries => _breachEntries;

        public int TotalCount => _hoistEntries.Count + _munitionsEntries.Count + _sonarEntries.Count + _breachEntries.Count;

        public static MilitaryArmoryCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new MilitaryArmoryCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Automated Turret Ammo Hoist Jams
            string hoistPath = Path.Combine(directoryPath, "ammo_hoist_jam_reports.json");
            if (File.Exists(hoistPath))
            {
                var list = JsonSerializer.Deserialize<List<AmmoHoistJamEntry>>(File.ReadAllText(hoistPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._hoistEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. Decommissioned Munitions Storage Seepage
            string munitionsPath = Path.Combine(directoryPath, "munitions_leaching_records.json");
            if (File.Exists(munitionsPath))
            {
                var list = JsonSerializer.Deserialize<List<MunitionsLeachingEntry>>(File.ReadAllText(munitionsPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._munitionsEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Perimeter Sonar Hydrophone Array Faults
            string sonarPath = Path.Combine(directoryPath, "sonar_array_fault_logs.json");
            if (File.Exists(sonarPath))
            {
                var list = JsonSerializer.Deserialize<List<SonarArrayFaultEntry>>(File.ReadAllText(sonarPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._sonarEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. High-Security Vault Seal Breaches
            string breachPath = Path.Combine(directoryPath, "vault_seal_breach_logs.json");
            if (File.Exists(breachPath))
            {
                var list = JsonSerializer.Deserialize<List<VaultSealBreachEntry>>(File.ReadAllText(breachPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._breachEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public AmmoHoistJamEntry GetHoistJam(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is AmmoHoistJamEntry e ? e : null;
        }

        public MunitionsLeachingEntry GetMunitionsLeaching(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is MunitionsLeachingEntry e ? e : null;
        }

        public SonarArrayFaultEntry GetSonarFault(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is SonarArrayFaultEntry e ? e : null;
        }

        public VaultSealBreachEntry GetVaultBreach(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is VaultSealBreachEntry e ? e : null;
        }
    }
}
