using System;
using System.Collections.Generic;

namespace Ashfall.Core.Narrative
{
    [Serializable]
    public sealed class CourierDispatchEntry
    {
        public string dispatch_id;
        public int recorded_day;
        public string sender;
        public string recipient;
        public string route;
        public string delivery_status;
        public string goods_manifest;
        public string transcript;
        public string[] tags;
    }

    [Serializable]
    public sealed class CourierDispatchFile
    {
        public int schema_version;
        public string collection_id;
        public List<CourierDispatchEntry> dispatches = new List<CourierDispatchEntry>();
    }

    /// <summary>
    /// Engine-agnostic loader and query interface for The 30 Courier Dispatches & Trade Parleys.
    /// </summary>
    public sealed class CourierDispatchCatalog
    {
        private readonly Dictionary<string, CourierDispatchEntry> _byId =
            new Dictionary<string, CourierDispatchEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly List<CourierDispatchEntry> _allDispatches = new List<CourierDispatchEntry>();

        public IReadOnlyList<CourierDispatchEntry> AllDispatches => _allDispatches;

        public void Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var file = serializer.Deserialize<CourierDispatchFile>(json);
            if (file?.dispatches == null) return;

            foreach (var d in file.dispatches)
            {
                if (d == null || string.IsNullOrEmpty(d.dispatch_id)) continue;
                _byId[d.dispatch_id] = d;
                _allDispatches.Add(d);
            }
        }

        public CourierDispatchEntry GetById(string dispatchId)
        {
            if (string.IsNullOrEmpty(dispatchId)) return null;
            _byId.TryGetValue(dispatchId, out var entry);
            return entry;
        }

        public List<CourierDispatchEntry> GetUnlockedByDay(int currentDay)
        {
            var results = new List<CourierDispatchEntry>();
            for (int i = 0; i < _allDispatches.Count; i++)
            {
                var d = _allDispatches[i];
                if (d.recorded_day <= currentDay)
                {
                    results.Add(d);
                }
            }
            return results;
        }

        public List<CourierDispatchEntry> GetByParticipant(string nameSnippet)
        {
            var results = new List<CourierDispatchEntry>();
            if (string.IsNullOrEmpty(nameSnippet)) return results;

            for (int i = 0; i < _allDispatches.Count; i++)
            {
                var d = _allDispatches[i];
                bool matchSender = d.sender != null && d.sender.IndexOf(nameSnippet, StringComparison.OrdinalIgnoreCase) >= 0;
                bool matchRecipient = d.recipient != null && d.recipient.IndexOf(nameSnippet, StringComparison.OrdinalIgnoreCase) >= 0;
                if (matchSender || matchRecipient)
                {
                    results.Add(d);
                }
            }
            return results;
        }

        public List<CourierDispatchEntry> GetByTag(string tag)
        {
            var results = new List<CourierDispatchEntry>();
            if (string.IsNullOrEmpty(tag)) return results;

            for (int i = 0; i < _allDispatches.Count; i++)
            {
                var d = _allDispatches[i];
                if (d.tags == null) continue;
                for (int j = 0; j < d.tags.Length; j++)
                {
                    if (string.Equals(d.tags[j], tag, StringComparison.OrdinalIgnoreCase))
                    {
                        results.Add(d);
                        break;
                    }
                }
            }
            return results;
        }
    }
}
