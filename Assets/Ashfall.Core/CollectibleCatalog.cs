using System;
using System.Collections.Generic;
using Ashfall.Core.IO;

namespace Ashfall.Core
{
    /// <summary>
    /// Collectible definition from collectibles.json.
    /// Keyed by item_id — one-to-one with a physical item in items.json.
    /// </summary>
    [Serializable]
    public sealed class CollectibleDefinition
    {
        public string item_id = string.Empty;
        public string category = string.Empty;
        public string rarity = "common";
        public string effect_type = "none";
        public string effect_target = string.Empty;
        public float effect_value;
        public string location_type = string.Empty;
        public bool unique;
    }

    [Serializable]
    internal sealed class CollectibleCatalogFileRaw
    {
        public int schema_version = 1;
        public List<CollectibleDefinition> collectibles = new List<CollectibleDefinition>();
    }

    /// <summary>
    /// Loader for collectibles.json.
    /// Engine-agnostic: IFileIO + IJsonSerializer ports.
    /// Missing file returns null (silent-empty).
    /// </summary>
    public static class CollectibleCatalogLoader
    {
        public const string FileName = "collectibles.json";

        public static CollectibleCatalog? Load(
            string dataDir, IFileIO fileIO, IJsonSerializer json, ILog? log = null)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir)) return null;
            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path)) return null;
            try
            {
                string raw = fileIO.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;
                var file = json.Deserialize<CollectibleCatalogFileRaw>(raw);
                if (file == null) return null;
                return new CollectibleCatalog(file.collectibles);
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(path, "CollectibleCatalogFileRaw", ex);
                return null;
            }
        }
    }

    /// <summary>
    /// Loaded collectible catalog. Keyed by item_id.
    /// </summary>
    public sealed class CollectibleCatalog
    {
        private readonly Dictionary<string, CollectibleDefinition> _byItemId =
            new Dictionary<string, CollectibleDefinition>(StringComparer.Ordinal);

        public CollectibleCatalog(List<CollectibleDefinition>? definitions)
        {
            if (definitions != null)
            {
                foreach (var d in definitions)
                {
                    if (d != null && !string.IsNullOrEmpty(d.item_id))
                        _byItemId[d.item_id] = d;
                }
            }
        }

        public IReadOnlyDictionary<string, CollectibleDefinition> ByItemId => _byItemId;
        public int Count => _byItemId.Count;

        public CollectibleDefinition? GetByItemId(string itemId)
        {
            return _byItemId.TryGetValue(itemId, out var d) ? d : null;
        }

        public bool IsCollectible(string itemId) => _byItemId.ContainsKey(itemId);
    }
}
