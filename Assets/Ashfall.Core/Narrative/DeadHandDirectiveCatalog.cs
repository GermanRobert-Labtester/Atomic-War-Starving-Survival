using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Narrative
{
    [Serializable]
    public sealed class DeadHandDirectiveEntry
    {
        public string directive_id;
        public string timestamp_utc;
        public string clearance_level;
        public string issuing_authority;
        public string crypto_checksum;
        public string directive_title;
        public string transcript;
        public string archaeological_notes;
        public string[] tags;
    }

    [Serializable]
    public sealed class DeadHandDirectivesFile
    {
        public int schema_version;
        public string collection_id;
        public List<DeadHandDirectiveEntry> directives = new List<DeadHandDirectiveEntry>();
    }

    /// <summary>
    /// Engine-agnostic loader and query interface for The 20 Dead-Hand Classified Directives & Silo 0 Dossiers.
    /// </summary>
    public sealed class DeadHandDirectiveCatalog
    {
        private readonly Dictionary<string, DeadHandDirectiveEntry> _byId =
            new Dictionary<string, DeadHandDirectiveEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly List<DeadHandDirectiveEntry> _allDirectives = new List<DeadHandDirectiveEntry>();

        public IReadOnlyList<DeadHandDirectiveEntry> AllDirectives => _allDirectives;

        public void Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var file = serializer.Deserialize<DeadHandDirectivesFile>(json);
            if (file?.directives == null) return;

            foreach (var d in file.directives)
            {
                if (d == null || string.IsNullOrEmpty(d.directive_id)) continue;
                _byId[d.directive_id] = d;
                _allDirectives.Add(d);
            }
        }

        public DeadHandDirectiveEntry? GetById(string directiveId)
        {
            if (string.IsNullOrEmpty(directiveId)) return null;
            _byId.TryGetValue(directiveId, out var entry);
            return entry;
        }

        public List<DeadHandDirectiveEntry> GetByClearance(string clearanceSnippet)
        {
            var results = new List<DeadHandDirectiveEntry>();
            if (string.IsNullOrEmpty(clearanceSnippet)) return results;

            for (int i = 0; i < _allDirectives.Count; i++)
            {
                var d = _allDirectives[i];
                if (d.clearance_level != null && d.clearance_level.IndexOf(clearanceSnippet, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    results.Add(d);
                }
            }
            return results;
        }

        public List<DeadHandDirectiveEntry> GetByAuthority(string authoritySnippet)
        {
            var results = new List<DeadHandDirectiveEntry>();
            if (string.IsNullOrEmpty(authoritySnippet)) return results;

            for (int i = 0; i < _allDirectives.Count; i++)
            {
                var d = _allDirectives[i];
                if (d.issuing_authority != null && d.issuing_authority.IndexOf(authoritySnippet, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    results.Add(d);
                }
            }
            return results;
        }

        public List<DeadHandDirectiveEntry> GetByTag(string tag)
        {
            var results = new List<DeadHandDirectiveEntry>();
            if (string.IsNullOrEmpty(tag)) return results;

            for (int i = 0; i < _allDirectives.Count; i++)
            {
                var d = _allDirectives[i];
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
