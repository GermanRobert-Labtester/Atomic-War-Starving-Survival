using System;
using System.Collections.Generic;

namespace Ashfall.Core.Narrative
{
    [Serializable]
    public sealed class VinylRecordEntry
    {
        public string record_id;
        public string catalog_number;
        public string title;
        public string performer;
        public int recording_year;
        public string format_rpm;
        public string physical_condition;
        public int daily_morale_modifier;
        public float broadcast_frequency_mhz;
        public string needle_audio_texture;
        public string dweller_resonance_notes;
        public string[] tags;
    }

    [Serializable]
    public sealed class VinylRecordsFile
    {
        public int schema_version;
        public string collection_id;
        public List<VinylRecordEntry> records = new List<VinylRecordEntry>();
    }

    /// <summary>
    /// Engine-agnostic loader and query interface for The 30 Pre-War Radio Music Archive & Vinyl Record Master Catalog.
    /// </summary>
    public sealed class VinylRecordCatalog
    {
        private readonly Dictionary<string, VinylRecordEntry> _byId =
            new Dictionary<string, VinylRecordEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly List<VinylRecordEntry> _allRecords = new List<VinylRecordEntry>();

        public IReadOnlyList<VinylRecordEntry> AllRecords => _allRecords;

        public void Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var file = serializer.Deserialize<VinylRecordsFile>(json);
            if (file?.records == null) return;

            foreach (var r in file.records)
            {
                if (r == null || string.IsNullOrEmpty(r.record_id)) continue;
                _byId[r.record_id] = r;
                _allRecords.Add(r);
            }
        }

        public VinylRecordEntry GetById(string recordId)
        {
            if (string.IsNullOrEmpty(recordId)) return null;
            _byId.TryGetValue(recordId, out var entry);
            return entry;
        }

        public List<VinylRecordEntry> GetByFormat(string formatSnippet)
        {
            var results = new List<VinylRecordEntry>();
            if (string.IsNullOrEmpty(formatSnippet)) return results;

            for (int i = 0; i < _allRecords.Count; i++)
            {
                var r = _allRecords[i];
                if (r.format_rpm != null && r.format_rpm.IndexOf(formatSnippet, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    results.Add(r);
                }
            }
            return results;
        }

        public List<VinylRecordEntry> GetByTag(string tag)
        {
            var results = new List<VinylRecordEntry>();
            if (string.IsNullOrEmpty(tag)) return results;

            for (int i = 0; i < _allRecords.Count; i++)
            {
                var r = _allRecords[i];
                if (r.tags == null) continue;
                for (int j = 0; j < r.tags.Length; j++)
                {
                    if (string.Equals(r.tags[j], tag, StringComparison.OrdinalIgnoreCase))
                    {
                        results.Add(r);
                        break;
                    }
                }
            }
            return results;
        }
    }
}
