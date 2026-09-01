using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.World
{
    [Serializable]
    public sealed class FieldGuideEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty; // "Fauna" or "Flora"

        [JsonPropertyName("subject_id")]
        public string SubjectId { get; set; } = string.Empty;

        [JsonPropertyName("common_name")]
        public string CommonName { get; set; } = string.Empty;

        [JsonPropertyName("scientific_name")]
        public string ScientificName { get; set; } = string.Empty;

        [JsonPropertyName("habitat")]
        public string Habitat { get; set; } = string.Empty;

        [JsonPropertyName("threat_level")]
        public int ThreatLevel { get; set; } = 1;

        [JsonPropertyName("observation")]
        public string Observation { get; set; } = string.Empty;

        [JsonPropertyName("field_intel")]
        public string FieldIntel { get; set; } = string.Empty;

        [JsonPropertyName("trap_preference")]
        public string TrapPreference { get; set; } = string.Empty;

        [JsonPropertyName("edibility")]
        public string Edibility { get; set; } = string.Empty;

        [JsonPropertyName("unlock_trigger")]
        public string UnlockTrigger { get; set; } = string.Empty;

        [JsonPropertyName("art_id")]
        public string ArtId { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class FieldGuideCatalogData
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("collection_id")]
        public string CollectionId { get; set; } = string.Empty;

        [JsonPropertyName("entries")]
        public List<FieldGuideEntry> Entries { get; set; } = new List<FieldGuideEntry>();
    }

    [Serializable]
    public sealed class FieldGuideState
    {
        [JsonPropertyName("unlocked_entry_ids")]
        public List<string> UnlockedEntryIds { get; set; } = new List<string>();
    }

    public sealed class FieldGuideCatalog
    {
        private readonly Dictionary<string, FieldGuideEntry> _entriesById = new(StringComparer.OrdinalIgnoreCase);
        private readonly List<FieldGuideEntry> _allEntries = new();
        private readonly HashSet<string> _unlockedIds = new(StringComparer.OrdinalIgnoreCase);

        public int Count => _allEntries.Count;
        public IReadOnlyList<FieldGuideEntry> Entries => _allEntries;
        public int UnlockedCount => _unlockedIds.Count;

        public static FieldGuideCatalog LoadFromJson(string json)
        {
            var catalog = new FieldGuideCatalog();
            if (string.IsNullOrWhiteSpace(json)) return catalog;

            var data = JsonSerializer.Deserialize<FieldGuideCatalogData>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (data?.Entries != null)
            {
                foreach (var entry in data.Entries)
                {
                    if (string.IsNullOrEmpty(entry.Id)) continue;
                    catalog._entriesById[entry.Id] = entry;
                    catalog._allEntries.Add(entry);
                }
            }

            return catalog;
        }

        public static FieldGuideCatalog LoadFromDirectory(string directoryPath, IFileIO fileIO)
        {
            if (string.IsNullOrEmpty(directoryPath) || fileIO == null) return new FieldGuideCatalog();
            string path = Path.Combine(directoryPath, "field_guide.json");
            if (!fileIO.FileExists(path)) return new FieldGuideCatalog();
            return LoadFromJson(fileIO.ReadAllText(path));
        }

        public bool TryGetEntry(string id, out FieldGuideEntry entry)
        {
            if (string.IsNullOrEmpty(id))
            {
                entry = null!;
                return false;
            }
            return _entriesById.TryGetValue(id, out entry!);
        }

        public FieldGuideEntry? GetEntry(string id)
        {
            TryGetEntry(id, out var entry);
            return entry;
        }

        public IReadOnlyList<FieldGuideEntry> GetEntriesByCategory(string category)
        {
            var result = new List<FieldGuideEntry>();
            if (string.IsNullOrEmpty(category)) return result;

            foreach (var entry in _allEntries)
            {
                if (string.Equals(entry.Category, category, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(entry);
                }
            }
            return result;
        }

        public IReadOnlyList<FieldGuideEntry> GetEntriesByTag(string tag)
        {
            var result = new List<FieldGuideEntry>();
            if (string.IsNullOrEmpty(tag)) return result;

            foreach (var entry in _allEntries)
            {
                if (entry.Tags.Exists(t => string.Equals(t, tag, StringComparison.OrdinalIgnoreCase)))
                {
                    result.Add(entry);
                }
            }
            return result;
        }

        public bool UnlockEntry(string id)
        {
            if (string.IsNullOrEmpty(id) || !_entriesById.ContainsKey(id)) return false;
            return _unlockedIds.Add(id);
        }

        public bool IsUnlocked(string id)
        {
            if (string.IsNullOrEmpty(id)) return false;
            return _unlockedIds.Contains(id);
        }

        public FieldGuideState CaptureState()
        {
            return new FieldGuideState
            {
                UnlockedEntryIds = new List<string>(_unlockedIds)
            };
        }

        public void RestoreState(FieldGuideState? state)
        {
            _unlockedIds.Clear();
            if (state?.UnlockedEntryIds != null)
            {
                foreach (var id in state.UnlockedEntryIds)
                {
                    if (_entriesById.ContainsKey(id))
                    {
                        _unlockedIds.Add(id);
                    }
                }
            }
        }
    }
}
