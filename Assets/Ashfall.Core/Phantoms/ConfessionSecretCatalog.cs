// SPDX-License-Identifier: MIT
// ASHFALL Core: Confession & secret definitions, catalog loader, and query service (Plan 21).

using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Phantoms
{
    [Serializable]
    public sealed class ConfessionSecretEntryJson
    {
        public string secret_id = string.Empty;
        public string archetype_id = string.Empty;
        public string category = "npc_personal"; // "npc_personal", "faction_institutional", "bunker_internal", "historical_confession"
        public string subject_id = string.Empty;
        public string secret_title = string.Empty;
        public string secret_text = string.Empty;
        public string discovery_path = "direct_confession"; // "direct_confession", "document", "radio", "deathbed", "phantom_memory", "expedition"
        public string discovery_source_id = string.Empty;
        public string gating_flag = string.Empty;

        // Interpersonal outcomes
        public string forgiveness_outcome = string.Empty;
        public float forgiveness_affinity = 15f;
        public float forgiveness_morale = 10f;
        public string grudge_outcome = string.Empty;
        public float grudge_affinity = -25f;
        public float grudge_morale = -15f;

        // Moral Leverage choices
        public string expose_outcome = string.Empty;
        public string expose_standing_faction = string.Empty;
        public float expose_standing_delta;
        public float expose_guilt_delta;

        public string blackmail_outcome = string.Empty;
        public string blackmail_resource_gain = string.Empty;
        public float blackmail_hardening_delta;

        public string keep_outcome = string.Empty;
        public float keep_trust_delta = 20f;
    }

    [Serializable]
    public sealed class ConfessionSecretCatalogJson
    {
        public int schema_version;
        public List<ConfessionSecretEntryJson> items = new List<ConfessionSecretEntryJson>();
    }

    /// <summary>
    /// Read-only catalog query service for confession and secret records.
    /// </summary>
    public sealed class ConfessionSecretCatalog
    {
        private readonly Dictionary<string, ConfessionSecretEntryJson> _bySecretId =
            new Dictionary<string, ConfessionSecretEntryJson>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, List<ConfessionSecretEntryJson>> _byArchetypeId =
            new Dictionary<string, List<ConfessionSecretEntryJson>>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, List<ConfessionSecretEntryJson>> _byCategory =
            new Dictionary<string, List<ConfessionSecretEntryJson>>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyCollection<ConfessionSecretEntryJson> AllSecrets => _bySecretId.Values;

        public void Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var catalog = serializer.Deserialize<ConfessionSecretCatalogJson>(json);
            if (catalog?.items == null) return;

            foreach (var s in catalog.items)
            {
                if (s == null) continue;
                string id = !string.IsNullOrEmpty(s.secret_id) ? s.secret_id : $"secret_{s.archetype_id}";
                s.secret_id = id;
                _bySecretId[id] = s;

                if (!string.IsNullOrEmpty(s.archetype_id))
                {
                    if (!_byArchetypeId.TryGetValue(s.archetype_id, out var list))
                    {
                        list = new List<ConfessionSecretEntryJson>();
                        _byArchetypeId[s.archetype_id] = list;
                    }
                    list.Add(s);
                }

                string cat = !string.IsNullOrEmpty(s.category) ? s.category : "npc_personal";
                if (!_byCategory.TryGetValue(cat, out var catList))
                {
                    catList = new List<ConfessionSecretEntryJson>();
                    _byCategory[cat] = catList;
                }
                catList.Add(s);
            }
        }

        public ConfessionSecretEntryJson? GetById(string secretId)
        {
            if (string.IsNullOrEmpty(secretId)) return null;
            _bySecretId.TryGetValue(secretId, out var sec);
            return sec;
        }

        public IReadOnlyList<ConfessionSecretEntryJson> GetByArchetype(string archetypeId)
        {
            if (string.IsNullOrEmpty(archetypeId)) return Array.Empty<ConfessionSecretEntryJson>();
            if (_byArchetypeId.TryGetValue(archetypeId, out var list)) return list;
            return Array.Empty<ConfessionSecretEntryJson>();
        }

        public IReadOnlyList<ConfessionSecretEntryJson> GetByCategory(string category)
        {
            if (string.IsNullOrEmpty(category)) return Array.Empty<ConfessionSecretEntryJson>();
            if (_byCategory.TryGetValue(category, out var list)) return list;
            return Array.Empty<ConfessionSecretEntryJson>();
        }

        public bool Contains(string secretId)
        {
            if (string.IsNullOrEmpty(secretId)) return false;
            return _bySecretId.ContainsKey(secretId);
        }
    }
}
