using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class DraglineRuinEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("machine_designation")]
        public string MachineDesignation { get; set; } = string.Empty;

        [JsonPropertyName("operating_weight_tons")]
        public float OperatingWeightTons { get; set; }

        [JsonPropertyName("boom_length_meters")]
        public float BoomLengthMeters { get; set; }

        [JsonPropertyName("structural_condition")]
        public string StructuralCondition { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class SubstationFireEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("substation_id")]
        public string SubstationId { get; set; } = string.Empty;

        [JsonPropertyName("transformer_mva_rating")]
        public float TransformerMvaRating { get; set; }

        [JsonPropertyName("oil_volume_liters")]
        public int OilVolumeLiters { get; set; }

        [JsonPropertyName("contaminant_combustion_byproduct")]
        public string ContaminantCombustionByproduct { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class ArmoredLocomotiveEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("locomotive_id")]
        public string LocomotiveId { get; set; } = string.Empty;

        [JsonPropertyName("locomotive_type")]
        public string LocomotiveType { get; set; } = string.Empty;

        [JsonPropertyName("armor_thickness_mm")]
        public float ArmorThicknessMm { get; set; }

        [JsonPropertyName("current_operational_status")]
        public string CurrentOperationalStatus { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class PipelineSabotageEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("pipeline_sector")]
        public string PipelineSector { get; set; } = string.Empty;

        [JsonPropertyName("pipe_diameter_inches")]
        public float PipeDiameterInches { get; set; }

        [JsonPropertyName("sabotage_method")]
        public string SabotageMethod { get; set; } = string.Empty;

        [JsonPropertyName("environmental_hazard_severity")]
        public string EnvironmentalHazardSeverity { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class IndustrialRuinsCatalog
    {
        private readonly List<DraglineRuinEntry> _draglineEntries = new List<DraglineRuinEntry>();
        private readonly List<SubstationFireEntry> _substationEntries = new List<SubstationFireEntry>();
        private readonly List<ArmoredLocomotiveEntry> _locomotiveEntries = new List<ArmoredLocomotiveEntry>();
        private readonly List<PipelineSabotageEntry> _pipelineEntries = new List<PipelineSabotageEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<DraglineRuinEntry> DraglineEntries => _draglineEntries;
        public IReadOnlyList<SubstationFireEntry> SubstationEntries => _substationEntries;
        public IReadOnlyList<ArmoredLocomotiveEntry> LocomotiveEntries => _locomotiveEntries;
        public IReadOnlyList<PipelineSabotageEntry> PipelineEntries => _pipelineEntries;

        public int TotalCount => _draglineEntries.Count + _substationEntries.Count + _locomotiveEntries.Count + _pipelineEntries.Count;

        public static IndustrialRuinsCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new IndustrialRuinsCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Surface Dragline Excavator Ruins
            string draglinePath = Path.Combine(directoryPath, "surface_dragline_ruins.json");
            if (File.Exists(draglinePath))
            {
                var list = CatalogLocator.LoadWrappedList<DraglineRuinEntry>(File.ReadAllText(draglinePath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._draglineEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. High-Voltage Grid Substation Fires
            string subPath = Path.Combine(directoryPath, "substation_transformer_fires.json");
            if (File.Exists(subPath))
            {
                var list = CatalogLocator.LoadWrappedList<SubstationFireEntry>(File.ReadAllText(subPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._substationEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Armored Locomotive Manifests
            string locoPath = Path.Combine(directoryPath, "armored_locomotive_manifests.json");
            if (File.Exists(locoPath))
            {
                var list = CatalogLocator.LoadWrappedList<ArmoredLocomotiveEntry>(File.ReadAllText(locoPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._locomotiveEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Underground Pipeline Sabotage Records
            string pipePath = Path.Combine(directoryPath, "pipeline_sabotage_records.json");
            if (File.Exists(pipePath))
            {
                var list = CatalogLocator.LoadWrappedList<PipelineSabotageEntry>(File.ReadAllText(pipePath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._pipelineEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public DraglineRuinEntry? GetDragline(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is DraglineRuinEntry e ? e : null;
        }

        public SubstationFireEntry? GetSubstation(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is SubstationFireEntry e ? e : null;
        }

        public ArmoredLocomotiveEntry? GetLocomotive(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is ArmoredLocomotiveEntry e ? e : null;
        }

        public PipelineSabotageEntry? GetPipeline(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is PipelineSabotageEntry e ? e : null;
        }
    }
}
