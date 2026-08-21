using System;
using System.Collections.Generic;
#pragma warning disable CS0649

namespace Ashfall.Core.Economy
{
    /// <summary>Known good categories (the catalog validates against these).</summary>
    public static class GoodCategories
    {
        public static readonly string[] Known =
        {
            "food", "water", "medical", "fuel", "weapons", "tools",
            "materials", "ammo", "documents", "luxury", "misc"
        };

        public static bool IsKnown(string category)
        {
            if (string.IsNullOrEmpty(category)) return false;
            for (int i = 0; i < Known.Length; i++)
                if (Known[i] == category) return true;
            return false;
        }
    }

    /// <summary>
    /// Immutable goods definition (the JSON is the authority). Price-affecting
    /// parameters mirror the Unity economy's demand model: demand multipliers
    /// live in [MinDemandMult, MaxDemandMult]; volatility drives the
    /// deterministic daily walk; elasticity scales the demand response.
    /// </summary>
    [Serializable]
    public class GoodDefinition
    {
        public string id = string.Empty;
        public string displayName = string.Empty;
        public string category = "misc";
        public float basePrice = 1f;
        public float volatility = 0.1f;   // 0..1: daily price noise amplitude
        public float elasticity = 1f;     // > 0: how strongly demand moves price
        public int stackSize = 10;
        public float weightKg = 1f;
        public string barterNote = string.Empty; // optional barter-relevant metadata
    }

    /// <summary>Load outcome: goods plus any validation errors (domain result, no exceptions).</summary>
    public class GoodsCatalogLoadResult
    {
        public List<GoodDefinition> Goods { get; } = new List<GoodDefinition>();
        public List<string> Errors { get; } = new List<string>();
        public bool HasErrors => Errors.Count > 0;
    }

    /// <summary>
    /// In-memory goods catalog with validation. Loaded via
    /// <see cref="GoodsCatalogLoader"/>; immutable after load.
    /// </summary>
    public class GoodsCatalog
    {
        private readonly Dictionary<string, GoodDefinition> _byId =
            new Dictionary<string, GoodDefinition>();

        public IReadOnlyDictionary<string, GoodDefinition> ById => _byId;
        public int Count => _byId.Count;

        public GoodDefinition Find(string id)
        {
            return !string.IsNullOrEmpty(id) && _byId.TryGetValue(id, out var def) ? def : null;
        }

        public IReadOnlyList<GoodDefinition> All()
        {
            var list = new List<GoodDefinition>(_byId.Values);
            list.Sort((a, b) => string.CompareOrdinal(a.id, b.id));
            return list;
        }

        internal void Add(GoodDefinition def) => _byId[def.id] = def;
    }

    /// <summary>
    /// Engine-agnostic loader for economy_goods.json with load-time validation:
    /// duplicate ids, missing required fields, invalid ranges, unknown
    /// categories, malformed values. Errors are collected, never thrown.
    /// </summary>
    public static class GoodsCatalogLoader
    {
        public const string FileName = "economy_goods.json";
        public const int CurrentSchemaVersion = 1;

        public static GoodsCatalogLoadResult Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var result = new GoodsCatalogLoadResult();
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
            {
                result.Errors.Add("loader requires dataDir, IFileIO and IJsonSerializer");
                return result;
            }

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
            {
                result.Errors.Add("catalog file missing: " + FileName);
                return result;
            }

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
            {
                result.Errors.Add("catalog file empty: " + FileName);
                return result;
            }

            // Schema-envelope: parse root object with schema_version + goods array.
            GoodsCatalogRoot root;
            try
            {
                root = json.Deserialize<GoodsCatalogRoot>(raw);
            }
            catch (Exception e)
            {
                result.Errors.Add("catalog malformed JSON: " + e.Message);
                return result;
            }
            if (root == null)
            {
                result.Errors.Add("catalog parsed to null");
                return result;
            }
            if (root.schema_version > CurrentSchemaVersion)
            {
                result.Errors.Add($"catalog schema {root.schema_version} is newer than supported {CurrentSchemaVersion}");
                return result;
            }

            // Strict pass: RawGoodDefinition uses nullable fields so an ABSENT
            // required field is distinguishable from a defaulted one (the
            // GoodDefinition initializers would otherwise mask absence).
            var rawEntries = root.goods;
            if (rawEntries == null)
            {
                result.Errors.Add("catalog goods array is null");
                return result;
            }

            var seen = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < rawEntries.Count; i++)
            {
                var rawDef = rawEntries[i];
                if (rawDef == null)
                {
                    result.Errors.Add($"entry [{i}] is null");
                    continue;
                }

                string id = rawDef.id ?? string.Empty;
                if (string.IsNullOrWhiteSpace(id))
                {
                    result.Errors.Add($"entry [{i}] missing id");
                    continue;
                }
                if (!IsSnakeCase(id))
                {
                    result.Errors.Add($"entry [{i}] id '{id}' is not snake_case");
                    continue;
                }
                if (!seen.Add(id))
                {
                    result.Errors.Add($"duplicate id '{id}' at entry [{i}]");
                    continue;
                }
                if (string.IsNullOrWhiteSpace(rawDef.displayName))
                {
                    result.Errors.Add($"'{id}' missing displayName");
                    continue;
                }
                if (string.IsNullOrWhiteSpace(rawDef.category))
                {
                    result.Errors.Add($"'{id}' missing category");
                    continue;
                }
                if (!GoodCategories.IsKnown(rawDef.category))
                {
                    result.Errors.Add($"'{id}' unknown category '{rawDef.category}'");
                    continue;
                }
                if (rawDef.basePrice == null)
                {
                    result.Errors.Add($"'{id}' missing basePrice");
                    continue;
                }
                float basePrice = rawDef.basePrice.Value;
                if (basePrice <= 0f || float.IsNaN(basePrice) || float.IsInfinity(basePrice))
                {
                    result.Errors.Add($"'{id}' basePrice must be > 0 (got {basePrice})");
                    continue;
                }

                float volatility = rawDef.volatility ?? 0.1f;
                if (volatility < 0f || volatility > 1f)
                {
                    result.Errors.Add($"'{id}' volatility must be in [0,1] (got {volatility})");
                    continue;
                }
                float elasticity = rawDef.elasticity ?? 1f;
                if (elasticity <= 0f || float.IsNaN(elasticity))
                {
                    result.Errors.Add($"'{id}' elasticity must be > 0 (got {elasticity})");
                    continue;
                }
                int stackSize = rawDef.stackSize ?? 10;
                if (stackSize < 1)
                {
                    result.Errors.Add($"'{id}' stackSize must be >= 1 (got {stackSize})");
                    continue;
                }
                float weightKg = rawDef.weightKg ?? 1f;
                if (weightKg < 0f || float.IsNaN(weightKg))
                {
                    result.Errors.Add($"'{id}' weightKg must be >= 0 (got {weightKg})");
                    continue;
                }

                result.Goods.Add(new GoodDefinition
                {
                    id = id,
                    displayName = rawDef.displayName,
                    category = rawDef.category,
                    basePrice = basePrice,
                    volatility = volatility,
                    elasticity = elasticity,
                    stackSize = stackSize,
                    weightKg = weightKg,
                    barterNote = rawDef.barterNote ?? string.Empty
                });
            }
            return result;
        }

        /// <summary>Schema-envelope root for economy_goods.json.</summary>
        private class GoodsCatalogRoot
        {
            public int schema_version = 1;
            public List<RawGoodDefinition> goods = new List<RawGoodDefinition>();
        }

        /// <summary>Strict DTO: null fields mean ABSENT, not defaulted.</summary>
        private class RawGoodDefinition
        {
            public string id;
            public string displayName;
            public string category;
            public float? basePrice;
            public float? volatility;
            public float? elasticity;
            public int? stackSize;
            public float? weightKg;
            public string barterNote;
        }

        public static GoodsCatalog ToCatalog(GoodsCatalogLoadResult load)
        {
            var catalog = new GoodsCatalog();
            if (load != null)
            {
                for (int i = 0; i < load.Goods.Count; i++)
                    catalog.Add(load.Goods[i]);
            }
            return catalog;
        }

        internal static bool IsSnakeCase(string id)
        {
            if (string.IsNullOrEmpty(id)) return false;
            for (int i = 0; i < id.Length; i++)
            {
                char c = id[i];
                bool ok = (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') || c == '_';
                if (!ok) return false;
            }
            return id[0] != '_' && id[id.Length - 1] != '_';
        }
    }
}
