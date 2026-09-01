// SPDX-License-Identifier: MIT
// ASHFALL Core: Heirloom definitions, historical stages, and catalog loader (Plan 21).

using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Phantoms
{
    /// <summary>
    /// One authored historical epoch for an heirloom (e.g. Pre-War Origin, Exchange Survival, Current).
    /// </summary>
    [Serializable]
    public sealed class HeirloomHistoricalStage
    {
        public int stage_index;
        public string period_label;
        public string original_holder;
        public string historical_fragment;
    }

    /// <summary>
    /// Holder-specific memory fragment unlocked when held by a survivor matching an affinity.
    /// </summary>
    [Serializable]
    public sealed class HeirloomHolderMemory
    {
        public string affinity_key; // "kin", "profession_medical", "trait_caregiver", "generic", etc.
        public string memory_text;
        public float morale_effect;
        public float guilt_effect;
    }

    /// <summary>
    /// Static definition of a named heirloom in the world catalog.
    /// </summary>
    [Serializable]
    public sealed class HeirloomDefinition
    {
        public string heirloom_id;
        public string base_item_id;
        public string title;
        public string origin;
        public bool is_legacy_candidate;
        public bool memorial_eligible;
        public List<HeirloomHistoricalStage> stages = new List<HeirloomHistoricalStage>();
        public List<HeirloomHolderMemory> holder_memories = new List<HeirloomHolderMemory>();
    }

    [Serializable]
    public sealed class HeirloomCatalogJson
    {
        public int schema_version;
        public List<HeirloomDefinition> items = new List<HeirloomDefinition>();
    }

    /// <summary>
    /// Read-only catalog query service for authored heirlooms.
    /// </summary>
    public sealed class HeirloomCatalog
    {
        private readonly Dictionary<string, HeirloomDefinition> _byHeirloomId =
            new Dictionary<string, HeirloomDefinition>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, HeirloomDefinition> _byBaseItemId =
            new Dictionary<string, HeirloomDefinition>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyCollection<HeirloomDefinition> AllHeirlooms => _byHeirloomId.Values;

        public void Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var catalog = serializer.Deserialize<HeirloomCatalogJson>(json);
            if (catalog?.items == null) return;

            foreach (var h in catalog.items)
            {
                if (h == null || string.IsNullOrEmpty(h.heirloom_id)) continue;
                _byHeirloomId[h.heirloom_id] = h;
                if (!string.IsNullOrEmpty(h.base_item_id))
                {
                    _byBaseItemId[h.base_item_id] = h;
                }
            }
        }

        public HeirloomDefinition? GetById(string heirloomId)
        {
            if (string.IsNullOrEmpty(heirloomId)) return null;
            _byHeirloomId.TryGetValue(heirloomId, out var def);
            return def;
        }

        public HeirloomDefinition? GetByBaseItemId(string baseItemId)
        {
            if (string.IsNullOrEmpty(baseItemId)) return null;
            _byBaseItemId.TryGetValue(baseItemId, out var def);
            return def;
        }

        public bool Contains(string heirloomId)
        {
            if (string.IsNullOrEmpty(heirloomId)) return false;
            return _byHeirloomId.ContainsKey(heirloomId);
        }
    }
}
