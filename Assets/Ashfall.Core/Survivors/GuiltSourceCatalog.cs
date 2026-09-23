// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Survivors
{
    public sealed class GuiltSourceDefinition
    {
        [JsonPropertyName("choice_pattern")]
        public string ChoicePattern { get; set; } = string.Empty;

        [JsonPropertyName("severity")]
        public float Severity { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        public string FormatDescription(string survivorName)
        {
            if (string.IsNullOrEmpty(Description))
                return string.Empty;
            return Description.Replace("{name}", survivorName ?? "Someone");
        }
    }

    public sealed class GuiltSourceCatalog
    {
        private sealed class CatalogRoot
        {
            [JsonPropertyName("schema_version")]
            public int SchemaVersion { get; set; }

            [JsonPropertyName("items")]
            public List<GuiltSourceDefinition> Items { get; set; } = new List<GuiltSourceDefinition>();
        }

        private readonly List<GuiltSourceDefinition> _items = new List<GuiltSourceDefinition>();
        private readonly Dictionary<string, GuiltSourceDefinition> _byPattern =
            new Dictionary<string, GuiltSourceDefinition>(StringComparer.Ordinal);

        public IReadOnlyList<GuiltSourceDefinition> Items => _items;
        public int Count => _items.Count;

        public GuiltSourceCatalog() { }

        public GuiltSourceCatalog(IEnumerable<GuiltSourceDefinition> items)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    _items.Add(item);
                    if (!string.IsNullOrEmpty(item.ChoicePattern))
                        _byPattern[item.ChoicePattern] = item;
                }
            }
        }

        public static GuiltSourceCatalog FromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new GuiltSourceCatalog();

            var root = JsonSerializer.Deserialize<CatalogRoot>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return new GuiltSourceCatalog(root?.Items ?? (IEnumerable<GuiltSourceDefinition>)Array.Empty<GuiltSourceDefinition>());
        }

        public static GuiltSourceCatalog LoadFromDirectory(string dataDirectory)
        {
            var filePath = Path.Combine(dataDirectory, "guilt_sources.json");
            if (!File.Exists(filePath))
                return new GuiltSourceCatalog();

            var json = File.ReadAllText(filePath);
            return FromJson(json);
        }

        public GuiltSourceDefinition? GetByPattern(string choicePattern)
        {
            if (string.IsNullOrEmpty(choicePattern)) return null;
            return _byPattern.TryGetValue(choicePattern, out var def) ? def : null;
        }

        public bool TryGetSeverity(string choicePattern, out float severity)
        {
            severity = 0f;
            if (string.IsNullOrEmpty(choicePattern)) return false;
            if (_byPattern.TryGetValue(choicePattern, out var def))
            {
                severity = def.Severity;
                return true;
            }
            return false;
        }
    }
}
