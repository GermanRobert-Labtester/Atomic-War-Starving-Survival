// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.IO;
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

        private readonly Dictionary<string, BunkerGlitchEntry> _byLogCode =
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
                if (_byId.ContainsKey(g.glitch_id)) continue; // Idempotent: prevent duplicate additions

                _byId[g.glitch_id] = g;
                if (!string.IsNullOrEmpty(g.log_code))
                {
                    _byLogCode[g.log_code] = g;
                }
                _allGlitches.Add(g);
            }
        }

        public void Clear()
        {
            _byId.Clear();
            _byLogCode.Clear();
            _allGlitches.Clear();
        }

        /// <summary>
        /// Canonical directory loader that ingests narrative/bunker_maintenance_glitches.json.
        /// </summary>
        public static BunkerMaintenanceCatalog LoadFromDirectory(string dataDir, IFileIO fileIo, IJsonSerializer serializer)
        {
            var catalog = new BunkerMaintenanceCatalog();
            if (string.IsNullOrWhiteSpace(dataDir) || fileIo == null || serializer == null)
                return catalog;

            string canonicalPath = fileIo.Combine(dataDir, "narrative", "bunker_maintenance_glitches.json");
            if (fileIo.FileExists(canonicalPath))
            {
                try
                {
                    catalog.Load(fileIo.ReadAllText(canonicalPath), serializer);
                }
                catch (Exception ex)
                {
                    CatalogDiagnostics.Warn(canonicalPath, "BunkerMaintenanceCatalog", ex);
                }
            }

            return catalog;
        }

        public BunkerGlitchEntry? GetById(string glitchId)
        {
            if (string.IsNullOrEmpty(glitchId)) return null;
            _byId.TryGetValue(glitchId, out var entry);
            return entry;
        }

        public BunkerGlitchEntry? GetByLogCode(string logCode)
        {
            if (string.IsNullOrEmpty(logCode)) return null;
            _byLogCode.TryGetValue(logCode, out var entry);
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

        public List<BunkerGlitchEntry> GetBySeverity(int severityTier)
        {
            var results = new List<BunkerGlitchEntry>();
            for (int i = 0; i < _allGlitches.Count; i++)
            {
                var g = _allGlitches[i];
                if (g.severity_tier == severityTier)
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

        public List<BunkerGlitchEntry> GetBySearch(string query)
        {
            var results = new List<BunkerGlitchEntry>();
            if (string.IsNullOrWhiteSpace(query)) return results;

            for (int i = 0; i < _allGlitches.Count; i++)
            {
                var g = _allGlitches[i];
                if (MatchesSearch(g, query))
                {
                    results.Add(g);
                }
            }
            return results;
        }

        public List<BunkerGlitchEntry> GetSortedBySeverity(bool descending = true)
        {
            var copy = new List<BunkerGlitchEntry>(_allGlitches);
            copy.Sort((a, b) =>
            {
                int cmp = descending
                    ? b.severity_tier.CompareTo(a.severity_tier)
                    : a.severity_tier.CompareTo(b.severity_tier);
                if (cmp != 0) return cmp;
                return string.Compare(a.glitch_id, b.glitch_id, StringComparison.Ordinal);
            });
            return copy;
        }

        private static bool MatchesSearch(BunkerGlitchEntry g, string query)
        {
            if (ContainsIgnoreCase(g.glitch_id, query)) return true;
            if (ContainsIgnoreCase(g.log_code, query)) return true;
            if (ContainsIgnoreCase(g.affected_subsystem, query)) return true;
            if (ContainsIgnoreCase(g.anomaly_description, query)) return true;
            if (ContainsIgnoreCase(g.diagnostic_telemetry, query)) return true;
            if (ContainsIgnoreCase(g.emergency_protocol, query)) return true;
            if (ContainsIgnoreCase(g.dmitri_shift_note, query)) return true;

            if (g.tags != null)
            {
                for (int i = 0; i < g.tags.Length; i++)
                {
                    if (ContainsIgnoreCase(g.tags[i], query)) return true;
                }
            }

            if (g.required_repair_kit != null)
            {
                for (int i = 0; i < g.required_repair_kit.Length; i++)
                {
                    if (ContainsIgnoreCase(g.required_repair_kit[i], query)) return true;
                }
            }

            return false;
        }

        private static bool ContainsIgnoreCase(string? source, string query)
        {
            if (string.IsNullOrEmpty(source)) return false;
            return source.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
