using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class RagPulpBeaterEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("beater_station_id")]
        public string BeaterStationId { get; set; } = string.Empty;

        [JsonPropertyName("raw_fiber_source")]
        public string RawFiberSource { get; set; } = string.Empty;

        [JsonPropertyName("freeness_canadian_ml")]
        public float FreenessCanadianMl { get; set; }

        [JsonPropertyName("pulp_hydration_hours")]
        public float PulpHydrationHours { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class IronGallInkAssayEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("ink_formulation_code")]
        public string InkFormulationCode { get; set; } = string.Empty;

        [JsonPropertyName("tannin_source")]
        public string TanninSource { get; set; } = string.Empty;

        [JsonPropertyName("measured_ph_level")]
        public float MeasuredPhLevel { get; set; }

        [JsonPropertyName("pigment_complex")]
        public string PigmentComplex { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class TypographicLeadWearEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("font_case_identifier")]
        public string FontCaseIdentifier { get; set; } = string.Empty;

        [JsonPropertyName("type_metal_composition")]
        public string TypeMetalComposition { get; set; } = string.Empty;

        [JsonPropertyName("impression_count_cycles")]
        public int ImpressionCountCycles { get; set; }

        [JsonPropertyName("wear_phenomenon")]
        public string WearPhenomenon { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class StencilPropagandaSmearEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("stencil_print_id")]
        public string StencilPrintId { get; set; } = string.Empty;

        [JsonPropertyName("matrix_material_type")]
        public string MatrixMaterialType { get; set; } = string.Empty;

        [JsonPropertyName("ink_pigment_base")]
        public string InkPigmentBase { get; set; } = string.Empty;

        [JsonPropertyName("smear_artifact_description")]
        public string SmearArtifactDescription { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class PaperPrintingCatalog
    {
        private readonly List<RagPulpBeaterEntry> _pulpEntries = new List<RagPulpBeaterEntry>();
        private readonly List<IronGallInkAssayEntry> _inkEntries = new List<IronGallInkAssayEntry>();
        private readonly List<TypographicLeadWearEntry> _typeEntries = new List<TypographicLeadWearEntry>();
        private readonly List<StencilPropagandaSmearEntry> _stencilEntries = new List<StencilPropagandaSmearEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<RagPulpBeaterEntry> PulpEntries => _pulpEntries;
        public IReadOnlyList<IronGallInkAssayEntry> InkEntries => _inkEntries;
        public IReadOnlyList<TypographicLeadWearEntry> TypeEntries => _typeEntries;
        public IReadOnlyList<StencilPropagandaSmearEntry> StencilEntries => _stencilEntries;

        public int TotalCount => _pulpEntries.Count + _inkEntries.Count + _typeEntries.Count + _stencilEntries.Count;

        public static PaperPrintingCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new PaperPrintingCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Rag-Pulp Cotton Linen Beater Records
            string pulpPath = Path.Combine(directoryPath, "rag_pulp_beater_records.json");
            if (File.Exists(pulpPath))
            {
                var list = JsonSerializer.Deserialize<List<RagPulpBeaterEntry>>(File.ReadAllText(pulpPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._pulpEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. Iron-Gall Ink Tannin Acidity Assays
            string inkPath = Path.Combine(directoryPath, "iron_gall_ink_acidity_reports.json");
            if (File.Exists(inkPath))
            {
                var list = JsonSerializer.Deserialize<List<IronGallInkAssayEntry>>(File.ReadAllText(inkPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._inkEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Gutenberg Hand-Press Typographic Lead Wear
            string typePath = Path.Combine(directoryPath, "typographic_lead_wear_logs.json");
            if (File.Exists(typePath))
            {
                var list = JsonSerializer.Deserialize<List<TypographicLeadWearEntry>>(File.ReadAllText(typePath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._typeEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Wood-Block Stencil Propaganda Smear Audits
            string stencilPath = Path.Combine(directoryPath, "stencil_propaganda_smear_logs.json");
            if (File.Exists(stencilPath))
            {
                var list = JsonSerializer.Deserialize<List<StencilPropagandaSmearEntry>>(File.ReadAllText(stencilPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._stencilEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public RagPulpBeaterEntry GetPulp(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is RagPulpBeaterEntry e ? e : null;
        }

        public IronGallInkAssayEntry GetInk(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is IronGallInkAssayEntry e ? e : null;
        }

        public TypographicLeadWearEntry GetType(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is TypographicLeadWearEntry e ? e : null;
        }

        public StencilPropagandaSmearEntry GetStencil(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is StencilPropagandaSmearEntry e ? e : null;
        }
    }
}
