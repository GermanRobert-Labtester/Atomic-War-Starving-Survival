// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Economy
{
    // ── Domain entry (loaded from regional_prices.json) ────────────────────

    /// <summary>
    /// Plan 14B — one regional price coefficient: how a good's base price is
    /// distorted in one region. Item-level entries override category-level
    /// entries for the same item; goods without any entry are neutral (1.0).
    /// This file never duplicates base prices — <c>economy_goods.json</c>
    /// stays the value authority; the atlas only shapes geography.
    /// </summary>
    public sealed class RegionalPriceEntry
    {
        public string Region = string.Empty;
        /// <summary>Goods-catalog item id (empty for category-level entries).</summary>
        public string ItemId = string.Empty;
        /// <summary>GoodCategories id (empty for item-level entries).</summary>
        public string Category = string.Empty;
        /// <summary>Base-price modifier, permille (700 = 0.7x). Bounded [500, 2000].</summary>
        public int BasePriceModifierPermille { get; }
        /// <summary>Closed-vocabulary scarcity label for UI text (never color-only).</summary>
        public string ScarcityProfile { get; }

        public RegionalPriceEntry(string region, string itemId, string category, int basePriceModifierPermille, string scarcityProfile)
        {
            Region = region ?? string.Empty;
            ItemId = itemId ?? string.Empty;
            Category = category ?? string.Empty;
            BasePriceModifierPermille = basePriceModifierPermille;
            ScarcityProfile = scarcityProfile ?? string.Empty;
        }
    }

    /// <summary>Load outcome: entries plus validation errors (domain result, no exceptions).</summary>
    public sealed class RegionalPriceLoadResult
    {
        public List<RegionalPriceEntry> Entries { get; } = new List<RegionalPriceEntry>();
        public List<string> Errors { get; } = new List<string>();
        public bool HasErrors => Errors.Count > 0;
    }

    /// <summary>Immutable in-memory regional price catalog.</summary>
    public sealed class RegionalPriceCatalog
    {
        private readonly Dictionary<string, RegionalPriceEntry> _byKey =
            new Dictionary<string, RegionalPriceEntry>(StringComparer.Ordinal);

        public int Count => _byKey.Count;
        /// <summary>Register one entry (loader seam and synthetic composition in tests). Last-wins per (region, target) key.</summary>
        public void Add(RegionalPriceEntry entry) => _byKey[KeyFor(entry)] = entry;

        public RegionalPriceEntry? FindItem(string itemId, string region)
        {
            if (string.IsNullOrEmpty(itemId) || string.IsNullOrEmpty(region)) return null;
            return _byKey.TryGetValue(EntryKey(region, itemId), out var entry) ? entry : null;
        }

        public RegionalPriceEntry? FindCategory(string category, string region)
        {
            if (string.IsNullOrEmpty(category) || string.IsNullOrEmpty(region)) return null;
            return _byKey.TryGetValue(CategoryKey(region, category), out var entry) ? entry : null;
        }

        public IReadOnlyList<RegionalPriceEntry> All()
        {
            var list = new List<RegionalPriceEntry>(_byKey.Values);
            list.Sort((a, b) =>
            {
                int byRegion = string.CompareOrdinal(a.Region, b.Region);
                return byRegion != 0 ? byRegion : string.CompareOrdinal(a.ItemId + a.Category, b.ItemId + b.Category);
            });
            return list;
        }

        internal static string KeyFor(RegionalPriceEntry entry) =>
            string.IsNullOrEmpty(entry.ItemId)
                ? CategoryKey(entry.Region, entry.Category)
                : EntryKey(entry.Region, entry.ItemId);

        internal static string EntryKey(string region, string itemId) => region + "|" + itemId;
        internal static string CategoryKey(string region, string category) => region + "|cat:" + category;
    }

    // ── Atlas (read-only queries over the catalog) ─────────────────────────

    /// <summary>
    /// Plan 14B — regional price geography. Pure, data-derived, stateless:
    /// there is no runtime state to persist (the market applies the modifier
    /// as ONE regional factor inside the canonical price equation; embargo
    /// shocks from 14A compose as a separate later factor). Every lookup is
    /// deterministic; unknown items/regions resolve neutral, never crash.
    /// </summary>
    public sealed class RegionalPriceAtlas
    {
        /// <summary>Load-time modifier bound (permille). Absorbs the source's 0.7x–1.5x profiles without permitting absurd values.</summary>
        public const int MinModifierPermille = 500;
        public const int MaxModifierPermille = 2000;

        /// <summary>Closed scarcity-profile vocabulary (textual UI labels; accessibility rule — never color-only).</summary>
        public static readonly IReadOnlyList<string> AcceptedScarcityProfiles =
            new[] { "local_surplus", "balanced", "imported_scarce" };

        private readonly RegionalPriceCatalog _catalog;

        public RegionalPriceAtlas(RegionalPriceCatalog catalog)
        {
            _catalog = catalog ?? new RegionalPriceCatalog();
        }

        public RegionalPriceCatalog Catalog => _catalog;

        /// <summary>Regional modifier for one good (item entry wins over category entry; 1000 when none).</summary>
        public int GetModifierPermille(string itemId, string itemCategory, string region)
        {
            var itemEntry = _catalog.FindItem(itemId, region);
            if (itemEntry != null) return itemEntry.BasePriceModifierPermille;
            var categoryEntry = _catalog.FindCategory(itemCategory, region);
            if (categoryEntry != null) return categoryEntry.BasePriceModifierPermille;
            return 1000;
        }

        /// <summary>Resolved regional price for one good given its base price (never negative).</summary>
        public float GetRegionalPrice(float basePrice, string itemId, string itemCategory, string region)
        {
            if (basePrice <= 0f || float.IsNaN(basePrice) || float.IsInfinity(basePrice)) return 0f;
            float price = basePrice * (GetModifierPermille(itemId, itemCategory, region) / 1000f);
            return Math.Max(0f, price);
        }

        /// <summary>Exact-entry lookup (item-level only).</summary>
        public bool TryGetRegionalEntry(string itemId, string region, out RegionalPriceEntry? entry)
        {
            entry = _catalog.FindItem(itemId, region);
            return entry != null;
        }

        /// <summary>
        /// Cheapest region for one good among the candidate regions.
        /// Deterministic: lowest modifier wins; ties resolve by ordinal region
        /// name. Returns null when no candidate resolves an entry (unknown
        /// goods are neutral everywhere and have no "best" region).
        /// </summary>
        public string? GetBestRegion(string itemId, string itemCategory, IEnumerable<string>? candidateRegions)
        {
            if (candidateRegions == null) return null;
            string? best = null;
            int bestModifier = int.MaxValue;
            foreach (var region in candidateRegions)
            {
                if (string.IsNullOrEmpty(region)) continue;
                bool hasEntry = _catalog.FindItem(itemId, region) != null || _catalog.FindCategory(itemCategory, region) != null;
                if (!hasEntry) continue;
                int modifier = GetModifierPermille(itemId, itemCategory, region);
                if (modifier < bestModifier || (modifier == bestModifier && best != null && string.CompareOrdinal(region, best) < 0))
                {
                    best = region;
                    bestModifier = modifier;
                }
            }
            return best;
        }

        /// <summary>All entries for one region, ordinal order (read-model row source).</summary>
        public List<RegionalPriceEntry> GetRegionalGoods(string region)
        {
            var result = new List<RegionalPriceEntry>();
            if (string.IsNullOrEmpty(region)) return result;
            foreach (var entry in _catalog.All())
                if (string.Equals(entry.Region, region, StringComparison.Ordinal))
                    result.Add(entry);
            return result;
        }

        /// <summary>Regions with an explicit entry for one item, ordinal order.</summary>
        public List<string> GetRegionsForItem(string itemId)
        {
            var result = new List<string>();
            if (string.IsNullOrEmpty(itemId)) return result;
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var entry in _catalog.All())
            {
                if (!string.Equals(entry.ItemId, itemId, StringComparison.Ordinal)) continue;
                if (seen.Add(entry.Region)) result.Add(entry.Region);
            }
            result.Sort(StringComparer.Ordinal);
            return result;
        }
    }

    // ── Loader (strict, CommodityBaselineCatalogLoader-pattern) ────────────

    /// <summary>
    /// Engine-agnostic loader for regional_prices.json with load-time
    /// validation: schema envelope, accepted region vocabulary, known goods
    /// categories, exactly-one-target-kind rule, permille bounds, scarcity
    /// vocabulary, duplicate item/region rows. Errors are collected, never
    /// thrown. Item-id existence is cross-file and validated by the catalog
    /// integrity gate, not here.
    /// </summary>
    public static class RegionalPriceCatalogLoader
    {
        public const string FileName = "regional_prices.json";
        public const int CurrentSchemaVersion = 1;

        public static RegionalPriceLoadResult Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var result = new RegionalPriceLoadResult();
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

            RegionalPriceRoot root;
            try
            {
                root = json.Deserialize<RegionalPriceRoot>(raw);
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

            var rawEntries = root.entries;
            if (rawEntries == null)
            {
                result.Errors.Add("catalog entries array is null");
                return result;
            }

            var seen = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < rawEntries.Count; i++)
            {
                var rawEntry = rawEntries[i];
                if (rawEntry == null) { result.Errors.Add($"entry [{i}] is null"); continue; }

                string region = rawEntry.region ?? string.Empty;
                string itemId = rawEntry.item_id ?? string.Empty;
                string category = rawEntry.category ?? string.Empty;

                if (string.IsNullOrWhiteSpace(region) || !RegionalSupplyRouter.IsAcceptedSupplyTag(region))
                {
                    result.Errors.Add($"entry [{i}] region '{region}' is not in the accepted supply-tag vocabulary");
                    continue;
                }
                bool hasItem = !string.IsNullOrWhiteSpace(itemId);
                bool hasCategory = !string.IsNullOrWhiteSpace(category);
                if (hasItem == hasCategory)
                {
                    result.Errors.Add($"entry [{i}] must set exactly one of item_id or category");
                    continue;
                }
                if (hasCategory && !GoodCategories.IsKnown(category))
                {
                    result.Errors.Add($"entry [{i}] category '{category}' is not a known goods category");
                    continue;
                }
                int modifier = rawEntry.base_price_modifier_permille ?? 1000;
                if (modifier < RegionalPriceAtlas.MinModifierPermille || modifier > RegionalPriceAtlas.MaxModifierPermille)
                {
                    result.Errors.Add($"entry [{i}] base_price_modifier_permille {modifier} outside "
                        + $"[{RegionalPriceAtlas.MinModifierPermille},{RegionalPriceAtlas.MaxModifierPermille}]");
                    continue;
                }
                string profile = rawEntry.scarcity_profile ?? string.Empty;
                if (!RegionalPriceAtlas.AcceptedScarcityProfiles.Contains(profile))
                {
                    result.Errors.Add($"entry [{i}] scarcity_profile '{profile}' is not in the accepted vocabulary "
                        + $"({string.Join("/", RegionalPriceAtlas.AcceptedScarcityProfiles)})");
                    continue;
                }

                string key = RegionalPriceCatalog.KeyFor(
                    new RegionalPriceEntry(region, itemId, category, modifier, profile));
                if (!seen.Add(key))
                {
                    result.Errors.Add($"duplicate entry '{key}' at entry [{i}]");
                    continue;
                }

                result.Entries.Add(new RegionalPriceEntry(region, itemId, category, modifier, profile));
            }
            return result;
        }

        public static RegionalPriceCatalog ToCatalog(RegionalPriceLoadResult load)
        {
            var catalog = new RegionalPriceCatalog();
            if (load != null)
            {
                foreach (var entry in load.Entries)
                    catalog.Add(entry);
            }
            return catalog;
        }

        private class RegionalPriceRoot
        {
            public int schema_version = 1;
            public string collection_id = string.Empty;
            public string description = string.Empty;
            public List<RawRegionalPriceEntry> entries = new List<RawRegionalPriceEntry>();
        }

        /// <summary>Strict DTO: null fields mean ABSENT, not defaulted.</summary>
        private class RawRegionalPriceEntry
        {
            public string region;
            public string item_id;
            public string category;
            public int? base_price_modifier_permille;
            public string scarcity_profile;
        }
    }
}
