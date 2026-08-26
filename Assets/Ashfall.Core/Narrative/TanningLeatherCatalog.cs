using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class OakBarkTanningPitEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("tannery_vat_id")]
        public string TanneryVatId { get; set; } = string.Empty;

        [JsonPropertyName("bark_source_botanical")]
        public string BarkSourceBotanical { get; set; } = string.Empty;

        [JsonPropertyName("barkometer_density_degrees")]
        public float BarkometerDensityDegrees { get; set; }

        [JsonPropertyName("hide_steep_duration_months")]
        public float HideSteepDurationMonths { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class MineralTanLiquorEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("mineral_tan_liquor_id")]
        public string MineralTanLiquorId { get; set; } = string.Empty;

        [JsonPropertyName("mineral_tanning_agent")]
        public string MineralTanningAgent { get; set; } = string.Empty;

        [JsonPropertyName("liquor_ph_level")]
        public float LiquorPhLevel { get; set; }

        [JsonPropertyName("hydrothermal_shrink_temp_celsius")]
        public float HydrothermalShrinkTempCelsius { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class RawhideBatingFailureEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("beamhouse_pit_id")]
        public string BeamhousePitId { get; set; } = string.Empty;

        [JsonPropertyName("deliming_chemical_agent")]
        public string DelimingChemicalAgent { get; set; } = string.Empty;

        [JsonPropertyName("phenolphthalein_test_status")]
        public string PhenolphthaleinTestStatus { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class LeatherHarnessCurryingEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("currying_workshop_id")]
        public string CurryingWorkshopId { get; set; } = string.Empty;

        [JsonPropertyName("fatliquor_compound_formula")]
        public string FatliquorCompoundFormula { get; set; } = string.Empty;

        [JsonPropertyName("oil_content_percentage")]
        public float OilContentPercentage { get; set; }

        [JsonPropertyName("tensile_strength_psi")]
        public float TensileStrengthPsi { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class TanningLeatherCatalog
    {
        private readonly List<OakBarkTanningPitEntry> _barkEntries = new List<OakBarkTanningPitEntry>();
        private readonly List<MineralTanLiquorEntry> _mineralEntries = new List<MineralTanLiquorEntry>();
        private readonly List<RawhideBatingFailureEntry> _batingEntries = new List<RawhideBatingFailureEntry>();
        private readonly List<LeatherHarnessCurryingEntry> _curryingEntries = new List<LeatherHarnessCurryingEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<OakBarkTanningPitEntry> BarkEntries => _barkEntries;
        public IReadOnlyList<MineralTanLiquorEntry> MineralEntries => _mineralEntries;
        public IReadOnlyList<RawhideBatingFailureEntry> BatingEntries => _batingEntries;
        public IReadOnlyList<LeatherHarnessCurryingEntry> CurryingEntries => _curryingEntries;

        public int TotalCount => _barkEntries.Count + _mineralEntries.Count + _batingEntries.Count + _curryingEntries.Count;

        public static TanningLeatherCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new TanningLeatherCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Oak-Bark Vegetable Tanning Pit Logs
            string barkPath = Path.Combine(directoryPath, "oak_bark_tanning_pit_logs.json");
            if (File.Exists(barkPath))
            {
                var list = CatalogLocator.LoadWrappedList<OakBarkTanningPitEntry>(File.ReadAllText(barkPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._barkEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. Chrome-Alum Tanning Bath Liquor Assays
            string mineralPath = Path.Combine(directoryPath, "chrome_alum_tanning_assays.json");
            if (File.Exists(mineralPath))
            {
                var list = CatalogLocator.LoadWrappedList<MineralTanLiquorEntry>(File.ReadAllText(mineralPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._mineralEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Rawhide Deliming & Bating Failure Reports
            string batePath = Path.Combine(directoryPath, "rawhide_bating_failure_reports.json");
            if (File.Exists(batePath))
            {
                var list = CatalogLocator.LoadWrappedList<RawhideBatingFailureEntry>(File.ReadAllText(batePath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._batingEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Harness Neatsfoot Oil Conditioning Audits
            string curryPath = Path.Combine(directoryPath, "leather_harness_conditioning_audits.json");
            if (File.Exists(curryPath))
            {
                var list = CatalogLocator.LoadWrappedList<LeatherHarnessCurryingEntry>(File.ReadAllText(curryPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._curryingEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public OakBarkTanningPitEntry? GetBark(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is OakBarkTanningPitEntry e ? e : null;
        }

        public MineralTanLiquorEntry? GetMineral(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is MineralTanLiquorEntry e ? e : null;
        }

        public RawhideBatingFailureEntry? GetBating(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is RawhideBatingFailureEntry e ? e : null;
        }

        public LeatherHarnessCurryingEntry? GetCurrying(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is LeatherHarnessCurryingEntry e ? e : null;
        }
    }
}
