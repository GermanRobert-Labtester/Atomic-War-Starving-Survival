using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Ashfall.Core;
using Ashfall.Core.IO;

namespace Ashfall.Core.Inventory
{
    /// <summary>
    /// Shared Core loader for item_description_texts.json (authority).
    /// Adheres to Invariant 1 (zero engine coupling) and Invariant 6 (JSON authority).
    /// </summary>
    public static class ItemDescriptionCatalogLoader
    {
        public const string PrimaryFileName = "item_description_texts.json";

        public static ItemDescriptionCatalog LoadCatalog(string dataDir, IFileIO fileIO, IJsonSerializer serializer)
        {
            var result = LoadCatalogWithResult(dataDir, fileIO, serializer);
            result.ThrowIfFatal();
            return result.Entries.Count > 0 ? result.Entries[0] : new ItemDescriptionCatalog();
        }

        public static CatalogLoadResult<ItemDescriptionCatalog> LoadCatalogWithResult(
            string dataDir, IFileIO fileIO, IJsonSerializer serializer)
        {
            var catalog = new ItemDescriptionCatalog();
            if (fileIO == null || serializer == null || string.IsNullOrEmpty(dataDir))
            {
                var failResult = new CatalogLoadResult<ItemDescriptionCatalog>(
                    PrimaryFileName,
                    "ItemDescriptionCatalog",
                    CatalogClassification.Optional);
                failResult.AddFatal("Required dependencies are null or dataDir is empty");
                return failResult;
            }

            string fullPath = fileIO.Combine(dataDir, PrimaryFileName);
            var result = new CatalogLoadResult<ItemDescriptionCatalog>(
                fullPath,
                "ItemDescriptionCatalog",
                CatalogClassification.Optional);

            if (!fileIO.FileExists(fullPath))
            {
                result.AddWarning($"Item description catalog file '{fullPath}' not found.");
                result.AddEntry(catalog);
                return result;
            }

            try
            {
                string json = fileIO.ReadAllText(fullPath);
                if (string.IsNullOrWhiteSpace(json))
                {
                    result.AddWarning($"Item description catalog file '{fullPath}' is empty.");
                    result.AddEntry(catalog);
                    return result;
                }

                var root = serializer.Deserialize<ItemDescriptionTextsJsonRoot>(json);
                if (root == null || root.Descriptions == null)
                {
                    result.AddWarning($"Failed to deserialize item descriptions from '{fullPath}'.");
                    result.AddEntry(catalog);
                    return result;
                }

                int loadedCount = 0;
                int duplicateCount = 0;
                foreach (var entry in root.Descriptions)
                {
                    if (entry == null || string.IsNullOrWhiteSpace(entry.ItemId))
                        continue;

                    if (catalog.Register(entry))
                    {
                        loadedCount++;
                    }
                    else
                    {
                        duplicateCount++;
                    }
                }

                if (duplicateCount > 0)
                {
                    result.AddWarning($"Deduplicated {duplicateCount} item description entries in '{PrimaryFileName}'.");
                }

                result.AddEntry(catalog);
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(fullPath, "ItemDescriptionTextsJsonRoot", ex);
                result.AddFatal($"Exception loading item descriptions from '{fullPath}': {ex.Message}");
            }

            return result;
        }

        [Serializable]
        private sealed class ItemDescriptionTextsJsonRoot
        {
            [JsonPropertyName("schema_version")]
            public int SchemaVersion { get; set; } = 1;

            [JsonPropertyName("collection_id")]
            public string CollectionId { get; set; } = string.Empty;

            [JsonPropertyName("descriptions")]
            public List<ItemDescriptionEntry> Descriptions { get; set; } = new List<ItemDescriptionEntry>();
        }
    }
}
