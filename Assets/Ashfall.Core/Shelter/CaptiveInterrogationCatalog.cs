// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Ashfall.Core.Shelter
{
    public sealed class CaptiveArchetypeDef
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string faction_origin { get; set; } = string.Empty;
        public float base_resistance { get; set; } = 50f;
        public float base_hostility { get; set; } = 50f;
        public float penal_labor_efficiency { get; set; } = 1.0f;
        public List<string> potential_topics { get; set; } = new List<string>();
        public List<string> tags { get; set; } = new List<string>();
    }

    public sealed class InterrogationTopicDef
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string intel_category { get; set; } = "tactical_intel";
        public int intel_gain { get; set; } = 2;
        public float resistance_cost { get; set; } = 30f;
        public string reward_item_id { get; set; } = string.Empty;
    }

    public sealed class CaptiveInterrogationCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<CaptiveArchetypeDef> captive_archetypes { get; set; } = new List<CaptiveArchetypeDef>();
        public List<InterrogationTopicDef> interrogation_topics { get; set; } = new List<InterrogationTopicDef>();

        private readonly Dictionary<string, CaptiveArchetypeDef> _archetypesById = new Dictionary<string, CaptiveArchetypeDef>(StringComparer.Ordinal);
        private readonly Dictionary<string, InterrogationTopicDef> _topicsById = new Dictionary<string, InterrogationTopicDef>(StringComparer.Ordinal);

        public void Index()
        {
            _archetypesById.Clear();
            _topicsById.Clear();

            foreach (var arch in captive_archetypes)
            {
                if (!string.IsNullOrEmpty(arch.id))
                    _archetypesById[arch.id] = arch;
            }

            foreach (var topic in interrogation_topics)
            {
                if (!string.IsNullOrEmpty(topic.id))
                    _topicsById[topic.id] = topic;
            }
        }

        public CaptiveArchetypeDef? GetArchetype(string archetypeId)
        {
            if (string.IsNullOrEmpty(archetypeId)) return null;
            _archetypesById.TryGetValue(archetypeId, out var arch);
            return arch;
        }

        public InterrogationTopicDef? GetTopic(string topicId)
        {
            if (string.IsNullOrEmpty(topicId)) return null;
            _topicsById.TryGetValue(topicId, out var topic);
            return topic;
        }

        public IReadOnlyCollection<CaptiveArchetypeDef> GetAllArchetypes() => captive_archetypes;
    }

    public static class CaptiveInterrogationCatalogLoader
    {
        public static CaptiveInterrogationCatalog Load(string dataDir, IFileIO fileIo)
        {
            string path = Path.Combine(dataDir, "captive_interrogations.json");
            if (!fileIo.FileExists(path))
            {
                return new CaptiveInterrogationCatalog();
            }

            string json = fileIo.ReadAllText(path);
            var catalog = JsonSerializer.Deserialize<CaptiveInterrogationCatalog>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new CaptiveInterrogationCatalog();

            catalog.Index();
            return catalog;
        }
    }
}
