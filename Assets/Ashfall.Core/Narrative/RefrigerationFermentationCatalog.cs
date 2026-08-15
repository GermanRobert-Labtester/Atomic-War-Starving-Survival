using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class AmmoniaChillerLeakEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("chiller_unit_id")]
        public string ChillerUnitId { get; set; } = string.Empty;

        [JsonPropertyName("refrigerant_charge_kg")]
        public float RefrigerantChargeKg { get; set; }

        [JsonPropertyName("leak_rate_ppm_ambient")]
        public float LeakRatePpmAmbient { get; set; }

        [JsonPropertyName("system_failure_mode")]
        public string SystemFailureMode { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class BrinePicklingBarrelEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("barrel_batch_code")]
        public string BarrelBatchCode { get; set; } = string.Empty;

        [JsonPropertyName("food_substrate_type")]
        public string FoodSubstrateType { get; set; } = string.Empty;

        [JsonPropertyName("salinity_percentage")]
        public float SalinityPercentage { get; set; }

        [JsonPropertyName("spoilage_organism")]
        public string SpoilageOrganism { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class RootCellarHumidityRotEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("root_cellar_bay_id")]
        public string RootCellarBayId { get; set; } = string.Empty;

        [JsonPropertyName("stored_crop_species")]
        public string StoredCropSpecies { get; set; } = string.Empty;

        [JsonPropertyName("ambient_humidity_pct")]
        public float AmbientHumidityPct { get; set; }

        [JsonPropertyName("fungal_pathogen_name")]
        public string FungalPathogenName { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class SmokedMeatCreosoteEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("smokehouse_facility_id")]
        public string SmokehouseFacilityId { get; set; } = string.Empty;

        [JsonPropertyName("fuel_wood_species")]
        public string FuelWoodSpecies { get; set; } = string.Empty;

        [JsonPropertyName("creosote_deposit_mg_kg")]
        public float CreosoteDepositMgKg { get; set; }

        [JsonPropertyName("curing_temperature_celsius")]
        public float CuringTemperatureCelsius { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class RefrigerationFermentationCatalog
    {
        private readonly List<AmmoniaChillerLeakEntry> _chillerEntries = new List<AmmoniaChillerLeakEntry>();
        private readonly List<BrinePicklingBarrelEntry> _picklingEntries = new List<BrinePicklingBarrelEntry>();
        private readonly List<RootCellarHumidityRotEntry> _cellarEntries = new List<RootCellarHumidityRotEntry>();
        private readonly List<SmokedMeatCreosoteEntry> _smokeEntries = new List<SmokedMeatCreosoteEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<AmmoniaChillerLeakEntry> ChillerEntries => _chillerEntries;
        public IReadOnlyList<BrinePicklingBarrelEntry> PicklingEntries => _picklingEntries;
        public IReadOnlyList<RootCellarHumidityRotEntry> CellarEntries => _cellarEntries;
        public IReadOnlyList<SmokedMeatCreosoteEntry> SmokeEntries => _smokeEntries;

        public int TotalCount => _chillerEntries.Count + _picklingEntries.Count + _cellarEntries.Count + _smokeEntries.Count;

        public static RefrigerationFermentationCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new RefrigerationFermentationCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Ammonia Absorption Chiller Leaks
            string chillerPath = Path.Combine(directoryPath, "ammonia_chiller_leak_logs.json");
            if (File.Exists(chillerPath))
            {
                var list = JsonSerializer.Deserialize<List<AmmoniaChillerLeakEntry>>(File.ReadAllText(chillerPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._chillerEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. Salt-Brine Pickling Barrel Spoilage
            string picklingPath = Path.Combine(directoryPath, "brine_pickling_barrel_spoilage.json");
            if (File.Exists(picklingPath))
            {
                var list = JsonSerializer.Deserialize<List<BrinePicklingBarrelEntry>>(File.ReadAllText(picklingPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._picklingEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Sub-Basement Root Cellar Humidity Rot
            string cellarPath = Path.Combine(directoryPath, "root_cellar_humidity_rot_reports.json");
            if (File.Exists(cellarPath))
            {
                var list = JsonSerializer.Deserialize<List<RootCellarHumidityRotEntry>>(File.ReadAllText(cellarPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._cellarEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Smoked Meat Wood-Tar Creosote Assays
            string smokePath = Path.Combine(directoryPath, "smoked_meat_creosote_assays.json");
            if (File.Exists(smokePath))
            {
                var list = JsonSerializer.Deserialize<List<SmokedMeatCreosoteEntry>>(File.ReadAllText(smokePath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._smokeEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public AmmoniaChillerLeakEntry GetChiller(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is AmmoniaChillerLeakEntry e ? e : null;
        }

        public BrinePicklingBarrelEntry GetPickling(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is BrinePicklingBarrelEntry e ? e : null;
        }

        public RootCellarHumidityRotEntry GetCellar(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is RootCellarHumidityRotEntry e ? e : null;
        }

        public SmokedMeatCreosoteEntry GetSmoke(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is SmokedMeatCreosoteEntry e ? e : null;
        }
    }
}
