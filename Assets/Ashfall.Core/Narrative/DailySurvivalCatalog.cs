using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class PsychologicalJournalEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("author_designation")]
        public string AuthorDesignation { get; set; } = string.Empty;

        [JsonPropertyName("quiet_hour_time")]
        public string QuietHourTime { get; set; } = string.Empty;

        [JsonPropertyName("psychological_marker")]
        public string PsychologicalMarker { get; set; } = string.Empty;

        [JsonPropertyName("morale_impact")]
        public int MoraleImpact { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class MutatedBotanicalEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("botanical_name")]
        public string BotanicalName { get; set; } = string.Empty;

        [JsonPropertyName("cultivation_tray")]
        public string CultivationTray { get; set; } = string.Empty;

        [JsonPropertyName("edibility_status")]
        public string EdibilityStatus { get; set; } = string.Empty;

        [JsonPropertyName("radiation_uptake_bq_kg")]
        public float RadiationUptakeBqKg { get; set; }

        [JsonPropertyName("caloric_density_kcal_kg")]
        public int CaloricDensityKcalKg { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class ChildrenFolkloreEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("tradition_type")]
        public string TraditionType { get; set; } = string.Empty;

        [JsonPropertyName("origin_sector")]
        public string OriginSector { get; set; } = string.Empty;

        [JsonPropertyName("folk_theme")]
        public string FolkTheme { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class RationFraudEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("case_id")]
        public string CaseId { get; set; } = string.Empty;

        [JsonPropertyName("infraction_type")]
        public string InfractionType { get; set; } = string.Empty;

        [JsonPropertyName("accused_culprit")]
        public string AccusedCulprit { get; set; } = string.Empty;

        [JsonPropertyName("contraband_weight_kg")]
        public float ContrabandWeightKg { get; set; }

        [JsonPropertyName("verdict_penalty")]
        public string VerdictPenalty { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class DailySurvivalCatalog
    {
        private readonly List<PsychologicalJournalEntry> _journalEntries = new List<PsychologicalJournalEntry>();
        private readonly List<MutatedBotanicalEntry> _botanicalEntries = new List<MutatedBotanicalEntry>();
        private readonly List<ChildrenFolkloreEntry> _folkloreEntries = new List<ChildrenFolkloreEntry>();
        private readonly List<RationFraudEntry> _fraudEntries = new List<RationFraudEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<PsychologicalJournalEntry> JournalEntries => _journalEntries;
        public IReadOnlyList<MutatedBotanicalEntry> BotanicalEntries => _botanicalEntries;
        public IReadOnlyList<ChildrenFolkloreEntry> FolkloreEntries => _folkloreEntries;
        public IReadOnlyList<RationFraudEntry> FraudEntries => _fraudEntries;

        public int TotalCount => _journalEntries.Count + _botanicalEntries.Count + _folkloreEntries.Count + _fraudEntries.Count;

        public static DailySurvivalCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new DailySurvivalCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Psychological Journals
            string journalPath = Path.Combine(directoryPath, "dweller_psychological_journals.json");
            if (File.Exists(journalPath))
            {
                var list = JsonSerializer.Deserialize<List<PsychologicalJournalEntry>>(File.ReadAllText(journalPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._journalEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. Mutated Botanical Logs
            string botanyPath = Path.Combine(directoryPath, "mutated_botanical_logs.json");
            if (File.Exists(botanyPath))
            {
                var list = JsonSerializer.Deserialize<List<MutatedBotanicalEntry>>(File.ReadAllText(botanyPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._botanicalEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Children Folklore
            string folklorePath = Path.Combine(directoryPath, "bunker_children_folklore.json");
            if (File.Exists(folklorePath))
            {
                var list = JsonSerializer.Deserialize<List<ChildrenFolkloreEntry>>(File.ReadAllText(folklorePath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._folkloreEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Ration Fraud Records
            string fraudPath = Path.Combine(directoryPath, "ration_fraud_records.json");
            if (File.Exists(fraudPath))
            {
                var list = JsonSerializer.Deserialize<List<RationFraudEntry>>(File.ReadAllText(fraudPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._fraudEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public PsychologicalJournalEntry GetJournal(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is PsychologicalJournalEntry e ? e : null;
        }

        public MutatedBotanicalEntry GetBotanical(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is MutatedBotanicalEntry e ? e : null;
        }

        public ChildrenFolkloreEntry GetFolklore(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is ChildrenFolkloreEntry e ? e : null;
        }

        public RationFraudEntry GetFraud(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is RationFraudEntry e ? e : null;
        }
    }
}
