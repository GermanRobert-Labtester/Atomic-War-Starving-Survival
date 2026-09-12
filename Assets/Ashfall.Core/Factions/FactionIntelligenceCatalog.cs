// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Factions
{
    [Serializable]
    public sealed class FactionOperationDefinition
    {
        public string id = string.Empty;
        public string display_name = string.Empty;
        public string target_faction = string.Empty;
        public string operation_class = string.Empty;
        public string target_subsystem = string.Empty;
        public int suspicion_per_tick;
        public int cooldown_days;
        public int detection_difficulty;
        public List<string> tags = new List<string>();
    }

    [Serializable]
    public sealed class DeadDropTemplateDefinition
    {
        public string id = string.Empty;
        public string display_name = string.Empty;
        public string target_location = string.Empty;
        public int reward_intel_points;
        public string risk_level = "medium";
        public int expiry_days = 5;
    }

    [Serializable]
    public sealed class FactionIntelligenceCatalog
    {
        public int schema_version = 1;
        public List<FactionOperationDefinition> operations = new List<FactionOperationDefinition>();
        public List<DeadDropTemplateDefinition> dead_drop_templates = new List<DeadDropTemplateDefinition>();
    }

    public static class FactionIntelligenceCatalogLoader
    {
        public static FactionIntelligenceCatalog Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentException("Faction intelligence JSON string cannot be null or empty.", nameof(json));
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));

            var catalog = serializer.Deserialize<FactionIntelligenceCatalog>(json);
            if (catalog == null)
                throw new InvalidOperationException("Failed to deserialize faction intelligence catalog.");

            return catalog;
        }
    }
}
