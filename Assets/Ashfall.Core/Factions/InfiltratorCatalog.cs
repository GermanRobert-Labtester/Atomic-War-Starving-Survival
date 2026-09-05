using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Factions
{
    [Serializable]
    public sealed class InfiltratorProfileDef
    {
        [JsonPropertyName("profile_id")]
        public string ProfileId { get; set; } = string.Empty;

        [JsonPropertyName("source_faction_id")]
        public string SourceFactionId { get; set; } = string.Empty;

        [JsonPropertyName("deception_skill")]
        public float DeceptionSkill { get; set; }

        [JsonPropertyName("loyalty_mask")]
        public float LoyaltyMask { get; set; }

        [JsonPropertyName("behavior_flags")]
        public List<string> BehaviorFlags { get; set; } = new List<string>();

        [JsonPropertyName("forged_credentials")]
        public List<string> ForgedCredentials { get; set; } = new List<string>();

        [JsonPropertyName("sabotage_targets")]
        public List<string> SabotageTargets { get; set; } = new List<string>();

        [JsonPropertyName("confession_threshold")]
        public float ConfessionThreshold { get; set; }

        [JsonPropertyName("confession_triggers")]
        public List<string> ConfessionTriggers { get; set; } = new List<string>();

        [JsonPropertyName("defector_probability")]
        public float DefectorProbability { get; set; }
    }

    [Serializable]
    public sealed class InfiltratorCatalog
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("profiles")]
        public List<InfiltratorProfileDef> Profiles { get; set; } = new List<InfiltratorProfileDef>();
    }
}
