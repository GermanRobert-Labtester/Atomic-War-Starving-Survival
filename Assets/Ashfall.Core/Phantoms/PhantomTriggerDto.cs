// SPDX-License-Identifier: MIT
// ASHFALL Core: shared JSON DTO for the Phase-0/Phantom-Memory trigger catalog.
//
// Two host sessions used to keep their own private copies of these records:
//   * src/Host/Phase0HostSession.cs (lines 820-832)
//   * src/Host/PhantomMemoryHostSession.cs (lines 174-186)
// The audit (item #1) flagged that this duplication drifts. Both record
// shapes are byte-identical; consolidate here so any future field rename
// stays in one place. Allowed names: snake_case JSON keys, plain public
// fields (the same convention as the other catalog save DTOs in Core).

using System.Collections.Generic;

namespace Ashfall.Core.Phantoms
{
    /// <summary>
    /// Single authoritative per-rule JSON DTO for the phantom-memory trigger
    /// catalog (consumed by Phase-0 effects and by the independent
    /// PhantomMemory host session).
    /// </summary>
    [System.Serializable]
    public sealed class PhantomTriggerRuleJson
    {
        public string item_category;
        public float motivation_chance;
        public string description;
        public string motivation_text;
        public string breakdown_text;
    }

    /// <summary>
    /// Single authoritative per-background JSON DTO. One entry per
    /// background_id, with a list of rules that govern when the entry fires.
    /// </summary>
    [System.Serializable]
    public sealed class PhantomTriggerJsonEntry
    {
        public string background_id;
        public List<PhantomTriggerRuleJson> triggers;
    }
}
