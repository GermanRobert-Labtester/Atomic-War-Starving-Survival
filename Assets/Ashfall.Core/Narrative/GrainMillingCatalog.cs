using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class BurrMillstoneDressingEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("millstone_pair_id")]
        public string MillstonePairId { get; set; } = string.Empty;

        [JsonPropertyName("stone_material_type")]
        public string StoneMaterialType { get; set; } = string.Empty;

        [JsonPropertyName("cracks_per_inch_count")]
        public float CracksPerInchCount { get; set; }

        [JsonPropertyName("runner_rotational_rpm")]
        public float RunnerRotationalRpm { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class BoltingSilkMeshEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("sifter_reel_id")]
        public string SifterReelId { get; set; } = string.Empty;

        [JsonPropertyName("silk_gauze_grade")]
        public string SilkGauzeGrade { get; set; } = string.Empty;

        [JsonPropertyName("mesh_aperture_microns")]
        public float MeshApertureMicrons { get; set; }

        [JsonPropertyName("flour_extraction_yield_pct")]
        public float FlourExtractionYieldPct { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class GrainSiloWeevilEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("grain_silo_bin_id")]
        public string GrainSiloBinId { get; set; } = string.Empty;

        [JsonPropertyName("grain_crop_species")]
        public string GrainCropSpecies { get; set; } = string.Empty;

        [JsonPropertyName("grain_moisture_content_pct")]
        public float GrainMoistureContentPct { get; set; }

        [JsonPropertyName("grain_temperature_celsius")]
        public float GrainTemperatureCelsius { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class MillDampenerTemperingEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("conditioning_bin_id")]
        public string ConditioningBinId { get; set; } = string.Empty;

        [JsonPropertyName("tempering_water_addition_pct")]
        public float TemperingWaterAdditionPct { get; set; }

        [JsonPropertyName("target_milling_moisture_pct")]
        public float TargetMillingMoisturePct { get; set; }

        [JsonPropertyName("conditioning_dwell_hours")]
        public float ConditioningDwellHours { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class GrainMillingCatalog
    {
        private readonly List<BurrMillstoneDressingEntry> _millstoneEntries = new List<BurrMillstoneDressingEntry>();
        private readonly List<BoltingSilkMeshEntry> _silkEntries = new List<BoltingSilkMeshEntry>();
        private readonly List<GrainSiloWeevilEntry> _siloEntries = new List<GrainSiloWeevilEntry>();
        private readonly List<MillDampenerTemperingEntry> _temperEntries = new List<MillDampenerTemperingEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<BurrMillstoneDressingEntry> MillstoneEntries => _millstoneEntries;
        public IReadOnlyList<BoltingSilkMeshEntry> SilkEntries => _silkEntries;
        public IReadOnlyList<GrainSiloWeevilEntry> SiloEntries => _siloEntries;
        public IReadOnlyList<MillDampenerTemperingEntry> TemperEntries => _temperEntries;

        public int TotalCount => _millstoneEntries.Count + _silkEntries.Count + _siloEntries.Count + _temperEntries.Count;

        public static GrainMillingCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new GrainMillingCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. French Burr Millstone Quartz Dressing & Chisel Furrow Logs
            string millPath = Path.Combine(directoryPath, "burr_millstone_dressing_logs.json");
            if (File.Exists(millPath))
            {
                var list = JsonSerializer.Deserialize<List<BurrMillstoneDressingEntry>>(File.ReadAllText(millPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._millstoneEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. Dutch Bolting Silk Sifter Mesh Wear Reports
            string silkPath = Path.Combine(directoryPath, "bolting_silk_mesh_reports.json");
            if (File.Exists(silkPath))
            {
                var list = JsonSerializer.Deserialize<List<BoltingSilkMeshEntry>>(File.ReadAllText(silkPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._silkEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Subterranean Grain Silo Moisture & Weevil Infestation Audits
            string siloPath = Path.Combine(directoryPath, "grain_silo_weevil_audits.json");
            if (File.Exists(siloPath))
            {
                var list = JsonSerializer.Deserialize<List<GrainSiloWeevilEntry>>(File.ReadAllText(siloPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._siloEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Mill Dampener Tempering Water Addition Assays
            string temperPath = Path.Combine(directoryPath, "mill_dampener_tempering_assays.json");
            if (File.Exists(temperPath))
            {
                var list = JsonSerializer.Deserialize<List<MillDampenerTemperingEntry>>(File.ReadAllText(temperPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._temperEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public BurrMillstoneDressingEntry GetMillstone(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is BurrMillstoneDressingEntry e ? e : null;
        }

        public BoltingSilkMeshEntry GetSilk(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is BoltingSilkMeshEntry e ? e : null;
        }

        public GrainSiloWeevilEntry GetSilo(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is GrainSiloWeevilEntry e ? e : null;
        }

        public MillDampenerTemperingEntry GetTemper(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is MillDampenerTemperingEntry e ? e : null;
        }
    }
}
