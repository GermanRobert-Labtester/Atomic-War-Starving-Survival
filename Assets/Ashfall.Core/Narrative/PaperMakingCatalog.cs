using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class HollanderBeaterPulpingEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("beater_tub_id")]
        public string BeaterTubId { get; set; } = string.Empty;

        [JsonPropertyName("rag_feedstock_type")]
        public string RagFeedstockType { get; set; } = string.Empty;

        [JsonPropertyName("beating_duration_hours")]
        public float BeatingDurationHours { get; set; }

        [JsonPropertyName("schopper_riegler_freeness_sr")]
        public float SchopperRieglerFreenessSr { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class DeckleMouldWatermarkEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("mould_frame_id")]
        public string MouldFrameId { get; set; } = string.Empty;

        [JsonPropertyName("wire_mesh_count_per_inch")]
        public float WireMeshCountPerInch { get; set; }

        [JsonPropertyName("sheet_width_mm")]
        public float SheetWidthMm { get; set; }

        [JsonPropertyName("sheet_length_mm")]
        public float SheetLengthMm { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class ScrewPressFeltEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("press_station_id")]
        public string PressStationId { get; set; } = string.Empty;

        [JsonPropertyName("post_sheet_count")]
        public float PostSheetCount { get; set; }

        [JsonPropertyName("pressing_force_kilonewtons")]
        public float PressingForceKilonewtons { get; set; }

        [JsonPropertyName("moisture_removed_pct")]
        public float MoistureRemovedPct { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class TubSizingGelatinEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("sizing_vat_id")]
        public string SizingVatId { get; set; } = string.Empty;

        [JsonPropertyName("gelatin_solution_temp_celsius")]
        public float GelatinSolutionTempCelsius { get; set; }

        [JsonPropertyName("alum_additive_pct")]
        public float AlumAdditivePct { get; set; }

        [JsonPropertyName("cobb_water_absorption_g_per_m2")]
        public float CobbWaterAbsorptionGPerM2 { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class PaperMakingCatalog
    {
        private readonly List<HollanderBeaterPulpingEntry> _beaterEntries = new List<HollanderBeaterPulpingEntry>();
        private readonly List<DeckleMouldWatermarkEntry> _mouldEntries = new List<DeckleMouldWatermarkEntry>();
        private readonly List<ScrewPressFeltEntry> _pressEntries = new List<ScrewPressFeltEntry>();
        private readonly List<TubSizingGelatinEntry> _sizingEntries = new List<TubSizingGelatinEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<HollanderBeaterPulpingEntry> BeaterEntries => _beaterEntries;
        public IReadOnlyList<DeckleMouldWatermarkEntry> MouldEntries => _mouldEntries;
        public IReadOnlyList<ScrewPressFeltEntry> PressEntries => _pressEntries;
        public IReadOnlyList<TubSizingGelatinEntry> SizingEntries => _sizingEntries;

        public int TotalCount => _beaterEntries.Count + _mouldEntries.Count + _pressEntries.Count + _sizingEntries.Count;

        public static PaperMakingCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new PaperMakingCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Hollander Beater Linen & Hemp Rag Pulping Logs
            string beaterPath = Path.Combine(directoryPath, "hollander_beater_pulping_logs.json");
            if (File.Exists(beaterPath))
            {
                var list = JsonSerializer.Deserialize<List<HollanderBeaterPulpingEntry>>(File.ReadAllText(beaterPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._beaterEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. Deckle Mould Wire-Mesh Watermark Weaving Audits
            string mouldPath = Path.Combine(directoryPath, "deckle_mould_watermark_audits.json");
            if (File.Exists(mouldPath))
            {
                var list = JsonSerializer.Deserialize<List<DeckleMouldWatermarkEntry>>(File.ReadAllText(mouldPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._mouldEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Screw Press Felt Interleaving Water Extraction Reports
            string pressPath = Path.Combine(directoryPath, "screw_press_felt_reports.json");
            if (File.Exists(pressPath))
            {
                var list = JsonSerializer.Deserialize<List<ScrewPressFeltEntry>>(File.ReadAllText(pressPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._pressEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Gelatin & Alum Tub Sizing Ink-Bleed Assays
            string sizingPath = Path.Combine(directoryPath, "tub_sizing_gelatin_assays.json");
            if (File.Exists(sizingPath))
            {
                var list = JsonSerializer.Deserialize<List<TubSizingGelatinEntry>>(File.ReadAllText(sizingPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._sizingEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public HollanderBeaterPulpingEntry GetBeater(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is HollanderBeaterPulpingEntry e ? e : null;
        }

        public DeckleMouldWatermarkEntry GetMould(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is DeckleMouldWatermarkEntry e ? e : null;
        }

        public ScrewPressFeltEntry GetPress(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is ScrewPressFeltEntry e ? e : null;
        }

        public TubSizingGelatinEntry GetSizing(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is TubSizingGelatinEntry e ? e : null;
        }
    }
}
