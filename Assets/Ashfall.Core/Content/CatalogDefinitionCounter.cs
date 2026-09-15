// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Ashfall.Core.Content
{
    /// <summary>
    /// Counts authored records from the catalog root shape instead of looking
    /// only for literal <c>"id"</c> pairs. This keeps bare arrays and
    /// first-level *_id catalogs visible to utilization gates without counting
    /// nested effects, choices, or item references as definitions.
    /// </summary>
    public static class CatalogDefinitionCounter
    {
        public static int Count(string json, IList<string>? sampleIds = null)
        {
            if (string.IsNullOrWhiteSpace(json)) return 0;
            using var document = JsonDocument.Parse(json);
            return Count(document.RootElement, sampleIds);
        }

        public static int Count(JsonElement root, IList<string>? sampleIds = null)
        {
            int total = 0;
            if (root.ValueKind == JsonValueKind.Array)
            {
                foreach (var element in root.EnumerateArray())
                    total += CountDefinitionElement(element, total, sampleIds);
                return total;
            }

            if (root.ValueKind != JsonValueKind.Object) return 0;

            bool foundCollection = false;
            foreach (var property in root.EnumerateObject())
            {
                if (property.Value.ValueKind != JsonValueKind.Array) continue;
                foundCollection = true;
                foreach (var element in property.Value.EnumerateArray())
                    total += CountDefinitionElement(element, total, sampleIds);
            }

            if (!foundCollection && TryGetDefinitionId(root, out string id))
            {
                sampleIds?.Add(id);
                return 1;
            }
            return total;
        }

        private static int CountDefinitionElement(
            JsonElement element,
            int ordinal,
            IList<string>? sampleIds)
        {
            if (element.ValueKind != JsonValueKind.Object)
                return 0;

            if (TryGetDefinitionId(element, out string id))
                sampleIds?.Add(id);
            else
                sampleIds?.Add("definition_" + ordinal);
            return 1;
        }

        private static bool TryGetDefinitionId(JsonElement element, out string id)
        {
            foreach (var property in element.EnumerateObject())
            {
                if (!IsDefinitionIdProperty(property.Name)) continue;
                if (property.Value.ValueKind != JsonValueKind.String) continue;
                string value = property.Value.GetString() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(value)) continue;
                id = value;
                return true;
            }
            id = string.Empty;
            return false;
        }

        private static bool IsDefinitionIdProperty(string name)
        {
            return string.Equals(name, "id", StringComparison.OrdinalIgnoreCase) ||
                   name.EndsWith("_id", StringComparison.OrdinalIgnoreCase);
        }
    }
}
