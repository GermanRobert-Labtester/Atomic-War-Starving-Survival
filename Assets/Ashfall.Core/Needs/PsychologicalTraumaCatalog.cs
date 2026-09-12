// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Needs
{
    [Serializable]
    public sealed class TraumaTypeDefinition
    {
        public string id = string.Empty;
        public string display_name = string.Empty;
        public string description = string.Empty;
        public int stress_floor_permille;
        public int insomnia_chance_permille;
        public List<string> trigger_tags = new List<string>();
        public string crisis_affinity = string.Empty;
    }

    [Serializable]
    public sealed class RecoveryActionDefinition
    {
        public string id = string.Empty;
        public string display_name = string.Empty;
        public string description = string.Empty;
        public string required_room = string.Empty;
        public int stress_reduction_permille;
        public int daily_resolution_chance_permille;
        public int counselor_bonus_permille;
    }

    [Serializable]
    public sealed class CrisisEventDefinition
    {
        public string id = string.Empty;
        public string display_name = string.Empty;
        public string description = string.Empty;
        public int threshold_stress_permille;
        public int duration_days;
        public int productivity_penalty_permille;
        public string journal_entry_key = string.Empty;
    }

    [Serializable]
    public sealed class PsychologicalTraumaCatalog
    {
        public int schema_version = 1;
        public List<TraumaTypeDefinition> trauma_types = new List<TraumaTypeDefinition>();
        public List<RecoveryActionDefinition> recovery_actions = new List<RecoveryActionDefinition>();
        public List<CrisisEventDefinition> crisis_events = new List<CrisisEventDefinition>();
    }

    public static class PsychologicalTraumaCatalogLoader
    {
        public static PsychologicalTraumaCatalog Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentException("Psychological trauma JSON string cannot be null or empty.", nameof(json));
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));

            var catalog = serializer.Deserialize<PsychologicalTraumaCatalog>(json);
            if (catalog == null)
                throw new InvalidOperationException("Failed to deserialize psychological trauma catalog.");

            return catalog;
        }
    }
}
