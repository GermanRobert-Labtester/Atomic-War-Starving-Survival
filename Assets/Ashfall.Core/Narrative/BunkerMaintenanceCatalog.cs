using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Narrative
{
    [Serializable]
    public sealed class BunkerGlitchEntry
    {
        public string glitch_id;
        public string log_code;
        public string affected_subsystem;
        public int severity_tier;
        public string anomaly_description;
        public string diagnostic_telemetry;
        public string[] required_repair_kit;
        public string emergency_protocol;
        public string dmitri_shift_note;
        public string[] tags;
    }

    [Serializable]
    public sealed class BunkerMaintenanceFile
    {
        public int schema_version;
        public string collection_id;
        public List<BunkerGlitchEntry> glitches = new List<BunkerGlitchEntry>();
    }

    /// <summary>
    /// Engine-agnostic loader and query interface for The 20 Subterranean Engineering Emergencies & Pipe Glitch Logs.
    /// </summary>
    public sealed class BunkerMaintenanceCatalog
    {
        private readonly Dictionary<string, BunkerGlitchEntry> _byId =
            new Dictionary<string, BunkerGlitchEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly List<BunkerGlitchEntry> _allGlitches = new List<BunkerGlitchEntry>();

        public IReadOnlyList<BunkerGlitchEntry> AllGlitches => _allGlitches;

        public void Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var file = serializer.Deserialize<BunkerMaintenanceFile>(json);
            if (file?.glitches == null) return;

            foreach (var g in file.glitches)
            {
                if (g == null || string.IsNullOrEmpty(g.glitch_id)) continue;
                _byId[g.glitch_id] = g;
                _allGlitches.Add(g);
            }
        }

        public BunkerGlitchEntry? GetById(string glitchId)
        {
            if (string.IsNullOrEmpty(glitchId)) return null;
            _byId.TryGetValue(glitchId, out var entry);
            return entry;
        }

        public List<BunkerGlitchEntry> GetBySubsystem(string subsystemSnippet)
        {
            var results = new List<BunkerGlitchEntry>();
            if (string.IsNullOrEmpty(subsystemSnippet)) return results;

            for (int i = 0; i < _allGlitches.Count; i++)
            {
                var g = _allGlitches[i];
                if (g.affected_subsystem != null &&
                    g.affected_subsystem.IndexOf(subsystemSnippet, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    results.Add(g);
                }
            }
            return results;
        }

        public List<BunkerGlitchEntry> GetCriticalEmergencies(int minSeverity = 4)
        {
            var results = new List<BunkerGlitchEntry>();
            for (int i = 0; i < _allGlitches.Count; i++)
            {
                var g = _allGlitches[i];
                if (g.severity_tier >= minSeverity)
                {
                    results.Add(g);
                }
            }
            return results;
        }

        public List<BunkerGlitchEntry> GetByTag(string tag)
        {
            var results = new List<BunkerGlitchEntry>();
            if (string.IsNullOrEmpty(tag)) return results;

            for (int i = 0; i < _allGlitches.Count; i++)
            {
                var g = _allGlitches[i];
                if (g.tags == null) continue;
                for (int j = 0; j < g.tags.Length; j++)
                {
                    if (string.Equals(g.tags[j], tag, StringComparison.OrdinalIgnoreCase))
                    {
                        results.Add(g);
                        break;
                    }
                }
            }
            return results;
        }
    }
}
