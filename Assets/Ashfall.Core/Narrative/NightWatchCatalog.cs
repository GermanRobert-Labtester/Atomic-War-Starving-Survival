using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Narrative
{
    [Serializable]
    public sealed class NightWatchEntry
    {
        public string log_id;
        public int recorded_day;
        public string watch_hours;
        public string sentry_id;
        public string sentry_name;
        public string post_location;
        public string weather_conditions;
        public string threat_level;
        public string title;
        public string log_entry;
        public string tactical_action;
        public string[] tags;
    }

    [Serializable]
    public sealed class NightWatchLogbookFile
    {
        public int schema_version;
        public string collection_id;
        public List<NightWatchEntry> incidents = new List<NightWatchEntry>();
    }

    /// <summary>
    /// Engine-agnostic loader and incident query service for The Night Watch Logbook (15 sentry reports).
    /// </summary>
    public sealed class NightWatchCatalog
    {
        private readonly Dictionary<string, NightWatchEntry> _byId =
            new Dictionary<string, NightWatchEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly List<NightWatchEntry> _allIncidents = new List<NightWatchEntry>();

        public IReadOnlyList<NightWatchEntry> AllIncidents => _allIncidents;

        public void Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var file = serializer.Deserialize<NightWatchLogbookFile>(json);
            if (file?.incidents == null) return;

            foreach (var inc in file.incidents)
            {
                if (inc == null || string.IsNullOrEmpty(inc.log_id)) continue;
                _byId[inc.log_id] = inc;
                _allIncidents.Add(inc);
            }
        }

        public NightWatchEntry? GetById(string logId)
        {
            if (string.IsNullOrEmpty(logId)) return null;
            _byId.TryGetValue(logId, out var entry);
            return entry;
        }

        public List<NightWatchEntry> GetBySentry(string sentryId)
        {
            var results = new List<NightWatchEntry>();
            if (string.IsNullOrEmpty(sentryId)) return results;

            for (int i = 0; i < _allIncidents.Count; i++)
            {
                var entry = _allIncidents[i];
                if (string.Equals(entry.sentry_id, sentryId, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(entry);
                }
            }
            return results;
        }

        public List<NightWatchEntry> GetByThreatLevel(string threatSnippet)
        {
            var results = new List<NightWatchEntry>();
            if (string.IsNullOrEmpty(threatSnippet)) return results;

            for (int i = 0; i < _allIncidents.Count; i++)
            {
                var entry = _allIncidents[i];
                if (entry.threat_level != null && entry.threat_level.IndexOf(threatSnippet, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    results.Add(entry);
                }
            }
            return results;
        }
    }
}
