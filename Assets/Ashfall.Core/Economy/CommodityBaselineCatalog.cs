// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
#pragma warning disable CS0649
#pragma warning disable CS8618

namespace Ashfall.Core.Economy
{
    /// <summary>
    /// Plan 212 — commodity behavior definition. One row per known goods
    /// category. Authored gameplay values only:
    /// <list type="bullet">
    /// <item><c>base_multiplier_permille</c> — the category target multiplier
    /// the market index pulls toward each day (1000 = 1.0x);</item>
    /// <item><c>elasticity_class</c> — how strongly net trade pressure moves
    /// the index (low/medium/high);</item>
    /// <item><c>scarcity_floor/ceiling_permille</c> — hard bounds the index
    /// can never leave, so no runaway inflation or deflation.</item>
    /// </list>
    /// Item base prices are NEVER duplicated here — <c>economy_goods.json</c>
    /// stays the value authority; this catalog only shapes category dynamics.
    /// </summary>
    [Serializable]
    public sealed class CommodityBaselineDefinition
    {
        public string category_id = string.Empty;
        public int base_multiplier_permille = 1000;
        public string elasticity_class = "medium";
        public int scarcity_floor_permille = 700;
        public int scarcity_ceiling_permille = 2000;
    }

    /// <summary>Load outcome: rows plus validation errors (domain result, no exceptions).</summary>
    public sealed class CommodityBaselineLoadResult
    {
        public List<CommodityBaselineDefinition> Categories { get; } = new List<CommodityBaselineDefinition>();
        public List<string> Errors { get; } = new List<string>();
        public bool HasErrors => Errors.Count > 0;
    }

    /// <summary>In-memory commodity baseline catalog with validation. Immutable after load.</summary>
    public sealed class CommodityBaselineCatalog
    {
        private readonly Dictionary<string, CommodityBaselineDefinition> _byCategoryId =
            new Dictionary<string, CommodityBaselineDefinition>(StringComparer.Ordinal);

        public IReadOnlyDictionary<string, CommodityBaselineDefinition> ByCategoryId => _byCategoryId;
        public int Count => _byCategoryId.Count;

        public CommodityBaselineDefinition? Find(string categoryId)
        {
            return !string.IsNullOrEmpty(categoryId) && _byCategoryId.TryGetValue(categoryId, out var def)
                ? def
                : null;
        }

        /// <summary>Category multipliers are permille integers converted to multipliers (1000 → 1.0x).</summary>
        public const int PermilleScale = 1000;

        internal void Add(CommodityBaselineDefinition def) => _byCategoryId[def.category_id] = def;
    }

    /// <summary>
    /// Engine-agnostic loader for commodity_baselines.json with load-time
    /// validation: duplicate category ids, unknown categories (must be one of
    /// <see cref="GoodCategories.Known"/> so every row is live), permille
    /// bounds, floor/ceiling ordering, elasticity class vocabulary,
    /// snake_case ids. Errors are collected, never thrown.
    /// </summary>
    public static class CommodityBaselineCatalogLoader
    {
        public const string FileName = "commodity_baselines.json";
        public const int CurrentSchemaVersion = 1;

        public static readonly IReadOnlyList<string> AcceptedElasticityClasses =
            new[] { "low", "medium", "high" };

        public const int MinMultiplierPermille = 200;
        public const int MaxMultiplierPermille = 5000;

        public static CommodityBaselineLoadResult Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var result = new CommodityBaselineLoadResult();
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

            CommodityBaselineRoot root;
            try
            {
                root = json.Deserialize<CommodityBaselineRoot>(raw);
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

            var rows = root.categories;
            if (rows == null)
            {
                result.Errors.Add("catalog categories array is null");
                return result;
            }

            var seen = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                if (row == null)
                {
                    result.Errors.Add($"entry [{i}] is null");
                    continue;
                }

                string id = row.category_id ?? string.Empty;
                if (string.IsNullOrWhiteSpace(id))
                {
                    result.Errors.Add($"entry [{i}] missing category_id");
                    continue;
                }
                if (!GoodsCatalogLoader.IsSnakeCase(id))
                {
                    result.Errors.Add($"entry [{i}] category_id '{id}' is not snake_case");
                    continue;
                }
                if (!seen.Add(id))
                {
                    result.Errors.Add($"duplicate category_id '{id}' at entry [{i}]");
                    continue;
                }
                // Every authored row must name a live goods category — an
                // unknown category would be unreachable orphaned content.
                if (!GoodCategories.IsKnown(id))
                {
                    result.Errors.Add($"'{id}' is not a known goods category");
                    continue;
                }
                if (row.base_multiplier_permille == null)
                {
                    result.Errors.Add($"'{id}' missing base_multiplier_permille");
                    continue;
                }
                int baseMult = row.base_multiplier_permille.Value;
                if (baseMult < MinMultiplierPermille || baseMult > MaxMultiplierPermille)
                {
                    result.Errors.Add($"'{id}' base_multiplier_permille must be in [{MinMultiplierPermille},{MaxMultiplierPermille}] (got {baseMult})");
                    continue;
                }
                string elasticity = string.IsNullOrWhiteSpace(row.elasticity_class)
                    ? "medium"
                    : row.elasticity_class.Trim();
                if (!IsAcceptedElasticity(elasticity))
                {
                    result.Errors.Add($"'{id}' unknown elasticity_class '{elasticity}'");
                    continue;
                }
                int floor = row.scarcity_floor_permille ?? 700;
                int ceiling = row.scarcity_ceiling_permille ?? 2000;
                if (floor < MinMultiplierPermille || ceiling > MaxMultiplierPermille)
                {
                    result.Errors.Add($"'{id}' scarcity bounds must be within [{MinMultiplierPermille},{MaxMultiplierPermille}] (got floor {floor}, ceiling {ceiling})");
                    continue;
                }
                if (floor >= ceiling)
                {
                    result.Errors.Add($"'{id}' scarcity_floor_permille ({floor}) must be below scarcity_ceiling_permille ({ceiling})");
                    continue;
                }
                if (baseMult < floor || baseMult > ceiling)
                {
                    result.Errors.Add($"'{id}' base_multiplier_permille ({baseMult}) must lie within its own scarcity bounds [{floor},{ceiling}]");
                    continue;
                }

                result.Categories.Add(new CommodityBaselineDefinition
                {
                    category_id = id,
                    base_multiplier_permille = baseMult,
                    elasticity_class = elasticity,
                    scarcity_floor_permille = floor,
                    scarcity_ceiling_permille = ceiling
                });
            }
            return result;
        }

        public static bool IsAcceptedElasticity(string elasticity)
        {
            if (string.IsNullOrEmpty(elasticity)) return false;
            for (int i = 0; i < AcceptedElasticityClasses.Count; i++)
                if (string.Equals(AcceptedElasticityClasses[i], elasticity, StringComparison.Ordinal))
                    return true;
            return false;
        }

        public static CommodityBaselineCatalog ToCatalog(CommodityBaselineLoadResult load)
        {
            var catalog = new CommodityBaselineCatalog();
            if (load != null)
            {
                for (int i = 0; i < load.Categories.Count; i++)
                    catalog.Add(load.Categories[i]);
            }
            return catalog;
        }

        /// <summary>Schema-envelope root for commodity_baselines.json.</summary>
        private class CommodityBaselineRoot
        {
            public int schema_version = 1;
            public List<RawCommodityBaseline> categories = new List<RawCommodityBaseline>();
        }

        /// <summary>Strict DTO: null fields mean ABSENT, not defaulted.</summary>
        private class RawCommodityBaseline
        {
            public string category_id;
            public int? base_multiplier_permille;
            public string elasticity_class;
            public int? scarcity_floor_permille;
            public int? scarcity_ceiling_permille;
        }
    }
}
