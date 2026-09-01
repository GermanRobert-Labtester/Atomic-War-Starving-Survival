// SPDX-License-Identifier: MIT
// ASHFALL Core: shared JSON DTO for the Phantom Memory & Heirloom trigger catalog (Plan 21).

using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Phantoms
{
    /// <summary>
    /// Single authoritative per-rule JSON DTO for the phantom-memory trigger
    /// catalog (consumed by Phase-0 effects, PhantomMemoryEngine, and Host sessions).
    /// Extended in Plan 21 with additive metadata for rich narrative anchoring.
    /// </summary>
    [Serializable]
    public sealed class PhantomTriggerRuleJson
    {
        public string trigger_id;
        public string item_category;
        public string item_id;
        public float motivation_chance;
        public string description;
        public string motivation_text;
        public string breakdown_text;
        public string affinity_background;
        public string affinity_trait;
        public bool lore_only;
        public float morale_payload;
        public float guilt_payload;
        public string gating_flag;
        public bool repeatable;
    }

    /// <summary>
    /// Root catalog container DTO matching the JSON shape:
    /// { "schema_version": 1, "items": [ ... ] }
    /// </summary>
    [Serializable]
    public sealed class PhantomTriggerCatalogJson
    {
        public int schema_version;
        public List<PhantomTriggerJsonEntry> items = new List<PhantomTriggerJsonEntry>();
    }

    /// <summary>
    /// Single authoritative per-background JSON DTO. One entry per
    /// background_id, with a list of rules that govern when the entry fires.
    /// </summary>
    [Serializable]
    public sealed class PhantomTriggerJsonEntry
    {
        public string background_id;
        public List<PhantomTriggerRuleJson> triggers = new List<PhantomTriggerRuleJson>();
    }
}
