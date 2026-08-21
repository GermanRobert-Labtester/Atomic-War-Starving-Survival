using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class LangstrothHiveFoundationEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("hive_assembly_id")]
        public string HiveAssemblyId { get; set; } = string.Empty;

        [JsonPropertyName("cell_base_diameter_mm")]
        public float CellBaseDiameterMm { get; set; }

        [JsonPropertyName("comb_foundation_wax_grade")]
        public string CombFoundationWaxGrade { get; set; } = string.Empty;

        [JsonPropertyName("frame_count")]
        public float FrameCount { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class ApicultureRedLightEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("chamber_zone_id")]
        public string ChamberZoneId { get; set; } = string.Empty;

        [JsonPropertyName("illumination_wavelength_nm")]
        public float IlluminationWavelengthNm { get; set; }

        [JsonPropertyName("colony_population_count")]
        public float ColonyPopulationCount { get; set; }

        [JsonPropertyName("brood_chamber_temperature_celsius")]
        public float BroodChamberTemperatureCelsius { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class HoneyExtractorBalanceEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("extractor_unit_id")]
        public string ExtractorUnitId { get; set; } = string.Empty;

        [JsonPropertyName("rotor_speed_rpm")]
        public float RotorSpeedRpm { get; set; }

        [JsonPropertyName("honey_moisture_pct")]
        public float HoneyMoisturePct { get; set; }

        [JsonPropertyName("frame_capacity")]
        public float FrameCapacity { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class BeeswaxRenderingDippingEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("rendering_vat_id")]
        public string RenderingVatId { get; set; } = string.Empty;

        [JsonPropertyName("beeswax_melting_point_celsius")]
        public float BeeswaxMeltingPointCelsius { get; set; }

        [JsonPropertyName("unadulterated_purity_pct")]
        public float UnadulteratedPurityPct { get; set; }

        [JsonPropertyName("candle_burn_rate_grams_per_hour")]
        public float CandleBurnRateGramsPerHour { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class ApicultureBeeCatalog
    {
        private readonly List<LangstrothHiveFoundationEntry> _foundationEntries = new List<LangstrothHiveFoundationEntry>();
        private readonly List<ApicultureRedLightEntry> _redLightEntries = new List<ApicultureRedLightEntry>();
        private readonly List<HoneyExtractorBalanceEntry> _extractorEntries = new List<HoneyExtractorBalanceEntry>();
        private readonly List<BeeswaxRenderingDippingEntry> _waxEntries = new List<BeeswaxRenderingDippingEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<LangstrothHiveFoundationEntry> FoundationEntries => _foundationEntries;
        public IReadOnlyList<ApicultureRedLightEntry> RedLightEntries => _redLightEntries;
        public IReadOnlyList<HoneyExtractorBalanceEntry> ExtractorEntries => _extractorEntries;
        public IReadOnlyList<BeeswaxRenderingDippingEntry> WaxEntries => _waxEntries;

        public int TotalCount => _foundationEntries.Count + _redLightEntries.Count + _extractorEntries.Count + _waxEntries.Count;

        public static ApicultureBeeCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new ApicultureBeeCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Langstroth Beehive Wax Foundation Milling Logs
            string foundationPath = Path.Combine(directoryPath, "langstroth_hive_foundation_logs.json");
            if (File.Exists(foundationPath))
            {
                var list = JsonSerializer.Deserialize<List<LangstrothHiveFoundationEntry>>(File.ReadAllText(foundationPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._foundationEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. Subterranean Bunker Apiculture Red-Light Chamber Audits
            string redLightPath = Path.Combine(directoryPath, "apiculture_red_light_audits.json");
            if (File.Exists(redLightPath))
            {
                var list = JsonSerializer.Deserialize<List<ApicultureRedLightEntry>>(File.ReadAllText(redLightPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._redLightEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Radial Honey Extractor Centrifugal Balance Reports
            string extractorPath = Path.Combine(directoryPath, "honey_extractor_balance_reports.json");
            if (File.Exists(extractorPath))
            {
                var list = JsonSerializer.Deserialize<List<HoneyExtractorBalanceEntry>>(File.ReadAllText(extractorPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._extractorEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Beeswax Candle Rendering & Dipping Vat Assays
            string waxPath = Path.Combine(directoryPath, "beeswax_rendering_dipping_assays.json");
            if (File.Exists(waxPath))
            {
                var list = JsonSerializer.Deserialize<List<BeeswaxRenderingDippingEntry>>(File.ReadAllText(waxPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._waxEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public LangstrothHiveFoundationEntry? GetFoundation(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is LangstrothHiveFoundationEntry e ? e : null;
        }

        public ApicultureRedLightEntry? GetRedLight(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is ApicultureRedLightEntry e ? e : null;
        }

        public HoneyExtractorBalanceEntry? GetExtractor(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is HoneyExtractorBalanceEntry e ? e : null;
        }

        public BeeswaxRenderingDippingEntry? GetWax(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is BeeswaxRenderingDippingEntry e ? e : null;
        }
    }
}
