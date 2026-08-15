using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class RadiationAutopsyEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("case_number")]
        public string CaseNumber { get; set; } = string.Empty;

        [JsonPropertyName("anatomical_region")]
        public string AnatomicalRegion { get; set; } = string.Empty;

        [JsonPropertyName("estimated_dose_rads")]
        public int EstimatedDoseRads { get; set; }

        [JsonPropertyName("cause_of_death")]
        public string CauseOfDeath { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class HerbalPharmacologyEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("compound_name")]
        public string CompoundName { get; set; } = string.Empty;

        [JsonPropertyName("active_agent")]
        public string ActiveAgent { get; set; } = string.Empty;

        [JsonPropertyName("preparation_method")]
        public string PreparationMethod { get; set; } = string.Empty;

        [JsonPropertyName("efficacy_rating")]
        public string EfficacyRating { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class SurgicalLogEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("operation_code")]
        public string OperationCode { get; set; } = string.Empty;

        [JsonPropertyName("lead_surgeon")]
        public string LeadSurgeon { get; set; } = string.Empty;

        [JsonPropertyName("anesthetic_used")]
        public string AnestheticUsed { get; set; } = string.Empty;

        [JsonPropertyName("survival_outcome")]
        public string SurvivalOutcome { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class SensoryLossEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("case_file")]
        public string CaseFile { get; set; } = string.Empty;

        [JsonPropertyName("sensory_modality")]
        public string SensoryModality { get; set; } = string.Empty;

        [JsonPropertyName("pathological_cause")]
        public string PathologicalCause { get; set; } = string.Empty;

        [JsonPropertyName("functional_impairment_pct")]
        public int FunctionalImpairmentPct { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class MedicalPathologyCatalog
    {
        private readonly List<RadiationAutopsyEntry> _autopsyEntries = new List<RadiationAutopsyEntry>();
        private readonly List<HerbalPharmacologyEntry> _pharmaEntries = new List<HerbalPharmacologyEntry>();
        private readonly List<SurgicalLogEntry> _surgeryEntries = new List<SurgicalLogEntry>();
        private readonly List<SensoryLossEntry> _sensoryEntries = new List<SensoryLossEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<RadiationAutopsyEntry> AutopsyEntries => _autopsyEntries;
        public IReadOnlyList<HerbalPharmacologyEntry> PharmaEntries => _pharmaEntries;
        public IReadOnlyList<SurgicalLogEntry> SurgeryEntries => _surgeryEntries;
        public IReadOnlyList<SensoryLossEntry> SensoryEntries => _sensoryEntries;

        public int TotalCount => _autopsyEntries.Count + _pharmaEntries.Count + _surgeryEntries.Count + _sensoryEntries.Count;

        public static MedicalPathologyCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new MedicalPathologyCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Radiation Autopsy Records
            string autopsyPath = Path.Combine(directoryPath, "rad_pathology_autopsy_records.json");
            if (File.Exists(autopsyPath))
            {
                var list = JsonSerializer.Deserialize<List<RadiationAutopsyEntry>>(File.ReadAllText(autopsyPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._autopsyEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. Bunker Herbalism & Pharmacology
            string pharmaPath = Path.Combine(directoryPath, "bunker_herbalism_pharmacology.json");
            if (File.Exists(pharmaPath))
            {
                var list = JsonSerializer.Deserialize<List<HerbalPharmacologyEntry>>(File.ReadAllText(pharmaPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._pharmaEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Operating Theater Surgical Logs
            string surgeryPath = Path.Combine(directoryPath, "operating_theater_surgical_logs.json");
            if (File.Exists(surgeryPath))
            {
                var list = JsonSerializer.Deserialize<List<SurgicalLogEntry>>(File.ReadAllText(surgeryPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._surgeryEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Chronic Sensory Loss Records
            string sensoryPath = Path.Combine(directoryPath, "fallout_sensory_loss_records.json");
            if (File.Exists(sensoryPath))
            {
                var list = JsonSerializer.Deserialize<List<SensoryLossEntry>>(File.ReadAllText(sensoryPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._sensoryEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public RadiationAutopsyEntry GetAutopsy(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is RadiationAutopsyEntry e ? e : null;
        }

        public HerbalPharmacologyEntry GetPharma(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is HerbalPharmacologyEntry e ? e : null;
        }

        public SurgicalLogEntry GetSurgery(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is SurgicalLogEntry e ? e : null;
        }

        public SensoryLossEntry GetSensory(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is SensoryLossEntry e ? e : null;
        }
    }
}
