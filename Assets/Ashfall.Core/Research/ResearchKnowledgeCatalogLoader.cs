// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ashfall.Core.IO;

namespace Ashfall.Core
{
    [Serializable]
    public sealed class ResearchKnowledgeNodeWireDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("days_to_complete")]
        public int DaysToComplete { get; set; } = 5;

        [JsonPropertyName("prerequisites")]
        public List<string> Prerequisites { get; set; } = new List<string>();

        [JsonPropertyName("breakthrough_item")]
        public string? BreakthroughItem { get; set; }

        public ResearchKnowledgeDef ToDomain()
        {
            return new ResearchKnowledgeDef(
                Id,
                DisplayName,
                Category,
                Description,
                DaysToComplete,
                Prerequisites?.ToArray() ?? Array.Empty<string>(),
                BreakthroughItem);
        }
    }

    [Serializable]
    public sealed class ResearchKnowledgeCatalogContainer
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("collection_id")]
        public string CollectionId { get; set; } = "research_knowledge";

        [JsonPropertyName("knowledge_nodes")]
        public List<ResearchKnowledgeNodeWireDto> KnowledgeNodes { get; set; } = new List<ResearchKnowledgeNodeWireDto>();
    }

    /// <summary>
    /// Loads research knowledge definitions from JSON (Assets/StreamingAssets/Data/research_knowledge.json).
    /// Pure C#, engine-agnostic: uses IFileIO and IJsonSerializer ports.
    /// </summary>
    public static class ResearchKnowledgeCatalogLoader
    {
        public const string DefaultFileName = "research_knowledge.json";

        public static List<ResearchKnowledgeDef> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return new List<ResearchKnowledgeDef>();

            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                return new List<ResearchKnowledgeDef>();

            string rawText = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(rawText))
                return new List<ResearchKnowledgeDef>();

            try
            {
                var container = JsonSerializer.Deserialize<ResearchKnowledgeCatalogContainer>(rawText, SystemTextJsonSerializer.Options);
                if (container?.KnowledgeNodes == null) return new List<ResearchKnowledgeDef>();

                var list = new List<ResearchKnowledgeDef>(container.KnowledgeNodes.Count);
                foreach (var dto in container.KnowledgeNodes)
                {
                    if (dto != null && !string.IsNullOrEmpty(dto.Id))
                    {
                        list.Add(dto.ToDomain());
                    }
                }
                return list;
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn(DefaultFileName, "ResearchKnowledgeCatalogLoader", ex_CATDIAG);
                return new List<ResearchKnowledgeDef>();
            }
        }

        public static int LoadAndRegister(
            ResearchSystem system,
            string dataDir,
            IFileIO fileIO,
            IJsonSerializer json)
        {
            if (system == null) return 0;
            var defs = Load(dataDir, fileIO, json);
            if (defs.Count > 0)
            {
                foreach (var def in defs)
                {
                    system.Register(def);
                }
                return defs.Count;
            }

            // Fallback to built-in defaults if data file is missing (e.g. unit tests without directory setup)
            system.RegisterDefaults();
            return system.CatalogCount;
        }

        /// <summary>
        /// Validates that the research knowledge prerequisites form a strictly acyclic directed graph (DAG).
        /// Returns true if valid; fills errorMessage if a cycle or missing prerequisite is detected.
        /// </summary>
        public static bool ValidateDag(IEnumerable<ResearchKnowledgeDef> defs, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (defs == null) return true;

            var nodeMap = new Dictionary<string, ResearchKnowledgeDef>(StringComparer.OrdinalIgnoreCase);
            foreach (var d in defs)
            {
                if (string.IsNullOrEmpty(d.id))
                {
                    errorMessage = "Encountered research node with null or empty ID.";
                    return false;
                }
                if (nodeMap.ContainsKey(d.id))
                {
                    errorMessage = $"Duplicate research node ID: {d.id}";
                    return false;
                }
                nodeMap[d.id] = d;
            }

            // Check for missing prerequisites
            foreach (var kvp in nodeMap)
            {
                var node = kvp.Value;
                if (node.prerequisites != null)
                {
                    foreach (var prereq in node.prerequisites)
                    {
                        if (!nodeMap.ContainsKey(prereq))
                        {
                            errorMessage = $"Node '{node.id}' has unresolved prerequisite '{prereq}'.";
                            return false;
                        }
                    }
                }
            }

            // Cycle detection via DFS with 3-color marking (0=unvisited, 1=visiting, 2=visited)
            var visited = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (var key in nodeMap.Keys) visited[key] = 0;
            string? cycleError = null;

            bool HasCycleDfs(string currentId)
            {
                visited[currentId] = 1; // visiting
                var current = nodeMap[currentId];
                if (current.prerequisites != null)
                {
                    foreach (var prereq in current.prerequisites)
                    {
                        if (visited.TryGetValue(prereq, out int state))
                        {
                            if (state == 1) // cycle found!
                            {
                                cycleError = $"Cycle detected in research graph involving '{currentId}' -> '{prereq}'.";
                                return true;
                            }
                            if (state == 0 && HasCycleDfs(prereq))
                            {
                                return true;
                            }
                        }
                    }
                }
                visited[currentId] = 2; // visited
                return false;
            }

            foreach (var key in nodeMap.Keys)
            {
                if (visited[key] == 0)
                {
                    if (HasCycleDfs(key))
                    {
                        errorMessage = cycleError ?? "Cycle detected in research graph.";
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
