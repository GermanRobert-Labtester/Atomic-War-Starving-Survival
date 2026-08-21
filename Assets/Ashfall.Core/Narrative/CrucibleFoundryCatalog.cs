using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class CrucibleClayPotSlagEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("crucible_pot_id")]
        public string CruciblePotId { get; set; } = string.Empty;

        [JsonPropertyName("crucible_lining_formula")]
        public string CrucibleLiningFormula { get; set; } = string.Empty;

        [JsonPropertyName("melt_temperature_celsius")]
        public float MeltTemperatureCelsius { get; set; }

        [JsonPropertyName("slag_erosion_depth_mm")]
        public float SlagErosionDepthMm { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class CupolaMeltingRatioEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("cupola_furnace_id")]
        public string CupolaFurnaceId { get; set; } = string.Empty;

        [JsonPropertyName("coke_to_iron_charge_ratio")]
        public string CokeToIronChargeRatio { get; set; } = string.Empty;

        [JsonPropertyName("iron_tap_temperature_celsius")]
        public float IronTapTemperatureCelsius { get; set; }

        [JsonPropertyName("melting_rate_tons_per_hour")]
        public float MeltingRateTonsPerHour { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class PatternMakerShrinkageEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("pattern_shop_job_id")]
        public string PatternShopJobId { get; set; } = string.Empty;

        [JsonPropertyName("timber_pattern_material")]
        public string TimberPatternMaterial { get; set; } = string.Empty;

        [JsonPropertyName("shrinkage_allowance_fraction")]
        public string ShrinkageAllowanceFraction { get; set; } = string.Empty;

        [JsonPropertyName("draft_angle_degrees")]
        public float DraftAngleDegrees { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class GreenSandBentoniteEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("sand_muller_batch_id")]
        public string SandMullerBatchId { get; set; } = string.Empty;

        [JsonPropertyName("clay_binder_type")]
        public string ClayBinderType { get; set; } = string.Empty;

        [JsonPropertyName("temper_moisture_pct")]
        public float TemperMoisturePct { get; set; }

        [JsonPropertyName("green_compressive_strength_kpa")]
        public float GreenCompressiveStrengthKpa { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class CrucibleFoundryCatalog
    {
        private readonly List<CrucibleClayPotSlagEntry> _crucibleEntries = new List<CrucibleClayPotSlagEntry>();
        private readonly List<CupolaMeltingRatioEntry> _cupolaEntries = new List<CupolaMeltingRatioEntry>();
        private readonly List<PatternMakerShrinkageEntry> _patternEntries = new List<PatternMakerShrinkageEntry>();
        private readonly List<GreenSandBentoniteEntry> _sandEntries = new List<GreenSandBentoniteEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<CrucibleClayPotSlagEntry> CrucibleEntries => _crucibleEntries;
        public IReadOnlyList<CupolaMeltingRatioEntry> CupolaEntries => _cupolaEntries;
        public IReadOnlyList<PatternMakerShrinkageEntry> PatternEntries => _patternEntries;
        public IReadOnlyList<GreenSandBentoniteEntry> SandEntries => _sandEntries;

        public int TotalCount => _crucibleEntries.Count + _cupolaEntries.Count + _patternEntries.Count + _sandEntries.Count;

        public static CrucibleFoundryCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new CrucibleFoundryCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Huntsman Crucible Clay Pot Slag Attack Logs
            string crucPath = Path.Combine(directoryPath, "crucible_clay_pot_slag_logs.json");
            if (File.Exists(crucPath))
            {
                var list = JsonSerializer.Deserialize<List<CrucibleClayPotSlagEntry>>(File.ReadAllText(crucPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._crucibleEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. Cupola Furnace Coke-to-Iron Melting Ratios
            string cupPath = Path.Combine(directoryPath, "cupola_melting_ratio_audits.json");
            if (File.Exists(cupPath))
            {
                var list = JsonSerializer.Deserialize<List<CupolaMeltingRatioEntry>>(File.ReadAllText(cupPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._cupolaEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Pine Pattern Shrinkage & Draft Angle Records
            string patPath = Path.Combine(directoryPath, "pattern_maker_shrinkage_records.json");
            if (File.Exists(patPath))
            {
                var list = JsonSerializer.Deserialize<List<PatternMakerShrinkageEntry>>(File.ReadAllText(patPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._patternEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Green Sand Molding Binder Moisture Assays
            string sandPath = Path.Combine(directoryPath, "green_sand_bentonite_assays.json");
            if (File.Exists(sandPath))
            {
                var list = JsonSerializer.Deserialize<List<GreenSandBentoniteEntry>>(File.ReadAllText(sandPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._sandEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public CrucibleClayPotSlagEntry? GetCrucible(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is CrucibleClayPotSlagEntry e ? e : null;
        }

        public CupolaMeltingRatioEntry? GetCupola(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is CupolaMeltingRatioEntry e ? e : null;
        }

        public PatternMakerShrinkageEntry? GetPattern(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is PatternMakerShrinkageEntry e ? e : null;
        }

        public GreenSandBentoniteEntry? GetSand(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is GreenSandBentoniteEntry e ? e : null;
        }
    }
}
