// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.IO;

namespace Ashfall.Core.Expeditions
{
    public sealed class ExpeditionLootValidationError
    {
        public string ExpeditionId { get; set; } = string.Empty;
        public string FieldPath { get; set; } = string.Empty;
        public string UnresolvedValue { get; set; } = string.Empty;
        public string ExpectedNamespaces { get; set; } = "item_id or expedition_loot_category_id";

        public string FormattedMessage =>
            $"{FieldPath}: unresolved expedition loot reference '{UnresolvedValue}' (expected: {ExpectedNamespaces})";

        public override string ToString() => FormattedMessage;
    }

    public sealed class ExpeditionLootValidationResult
    {
        public bool IsValid => Errors.Count == 0;
        public List<ExpeditionLootValidationError> Errors { get; } = new List<ExpeditionLootValidationError>();
    }

    /// <summary>
    /// Dedicated validator for expedition loot references.
    /// Ensures every token in lootCategories maps to either a known item or a known semantic category.
    /// Adheres to Invariant 1 (zero engine coupling) and Invariant 6 (JSON data authority).
    /// </summary>
    public static class ExpeditionLootValidator
    {
        public static ExpeditionLootValidationResult Validate(
            IEnumerable<ExpeditionDefinition> expeditions,
            IExpeditionLootReferenceResolver resolver,
            string sourceCatalogName = "expeditions.json")
        {
            var result = new ExpeditionLootValidationResult();
            if (expeditions == null || resolver == null)
                return result;

            foreach (var exp in expeditions)
            {
                if (exp == null || exp.lootCategories == null) continue;

                for (int i = 0; i < exp.lootCategories.Count; i++)
                {
                    string token = exp.lootCategories[i];
                    var refType = resolver.Resolve(token, out _);
                    if (refType == ExpeditionLootReferenceType.Unknown)
                    {
                        result.Errors.Add(new ExpeditionLootValidationError
                        {
                            ExpeditionId = exp.id,
                            FieldPath = $"{sourceCatalogName}:{exp.id}.lootCategories[{i}]",
                            UnresolvedValue = token,
                            ExpectedNamespaces = "item_id or expedition_loot_category_id"
                        });
                    }
                }
            }

            return result;
        }

        public static ExpeditionLootValidationResult ValidateCatalog(
            string dataDir,
            IFileIO fileIO,
            IJsonSerializer serializer,
            IExpeditionLootReferenceResolver? resolver = null)
        {
            if (resolver == null)
            {
                // Auto-populate item IDs from items catalogs
                var itemIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                string itemsPath = fileIO.Combine(dataDir, "items.json");
                if (fileIO.FileExists(itemsPath))
                {
                    try
                    {
                        string raw = fileIO.ReadAllText(itemsPath);
                        var container = serializer.Deserialize<ItemCatalogContainerRaw>(raw);
                        if (container?.items != null)
                        {
                            foreach (var it in container.items)
                                if (!string.IsNullOrEmpty(it.id)) itemIds.Add(it.id);
                        }
                    }
                    catch
                    {
                        // best-effort
                    }
                }
                resolver = new ExpeditionLootReferenceResolver(itemIds);
            }

            var expeditions = ExpeditionCatalogLoader.Load(dataDir, fileIO, serializer);
            return Validate(expeditions, resolver);
        }

        [Serializable]
        private sealed class ItemCatalogContainerRaw
        {
            public List<ItemRecordRaw>? items { get; set; }
        }

        [Serializable]
        private sealed class ItemRecordRaw
        {
            public string id { get; set; } = string.Empty;
        }
    }
}
