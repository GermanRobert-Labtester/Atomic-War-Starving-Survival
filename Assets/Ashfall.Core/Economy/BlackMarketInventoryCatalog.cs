// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Shelter;
#pragma warning disable CS0649
#pragma warning disable CS8618

namespace Ashfall.Core.Economy
{
    /// <summary>Plan 211 — underworld syndicate profile (authored gameplay values).</summary>
    [Serializable]
    public sealed class SyndicateProfile
    {
        public string syndicate_id = string.Empty;
        public string display_name = string.Empty;
        public string description = string.Empty;
        public int base_premium_bp = 1400;
        public int sell_discount_bp = 2500;
        public int heat_decay_per_day = 3;
        public int credit_limit_units = 400;
        public int loan_interest_bp = 1500;
        public int required_access_tier = 1;
    }

    /// <summary>Plan 211 — one illicit stock roster entry.</summary>
    [Serializable]
    public sealed class BlackMarketEntryDefinition
    {
        public string entry_id = string.Empty;
        public string item_id = string.Empty;
        public List<string> syndicate_ids = new List<string>();
        public int access_tier = 1;
        public int stock_weight = 50;             // 0..100 inclusion weight
        public int min_stock = 1;
        public int max_stock = 1;
        public int scarcity_sensitivity = 50;     // 0..100 category-index response
        public int risk_premium_bp = 1500;        // added over the canonical value
        public int cooldown_days = 1;
        public List<string> tags = new List<string>();
    }

    /// <summary>Load outcome with validation errors (domain result, no exceptions).</summary>
    public sealed class BlackMarketInventoryLoadResult
    {
        public List<SyndicateProfile> Syndicates { get; } = new List<SyndicateProfile>();
        public List<BlackMarketEntryDefinition> Entries { get; } = new List<BlackMarketEntryDefinition>();
        public List<string> Errors { get; } = new List<string>();
        public bool HasErrors => Errors.Count > 0;
    }

    /// <summary>In-memory black-market catalog. Immutable after load.</summary>
    public sealed class BlackMarketInventoryCatalog
    {
        private readonly Dictionary<string, SyndicateProfile> _syndicates =
            new Dictionary<string, SyndicateProfile>(StringComparer.Ordinal);
        private readonly Dictionary<string, BlackMarketEntryDefinition> _entries =
            new Dictionary<string, BlackMarketEntryDefinition>(StringComparer.Ordinal);

        public IReadOnlyDictionary<string, SyndicateProfile> SyndicatesById => _syndicates;
        public IReadOnlyDictionary<string, BlackMarketEntryDefinition> EntriesById => _entries;
        public int SyndicateCount => _syndicates.Count;
        public int EntryCount => _entries.Count;

        public SyndicateProfile? FindSyndicate(string id)
        {
            return !string.IsNullOrEmpty(id) && _syndicates.TryGetValue(id, out var s) ? s : null;
        }

        public BlackMarketEntryDefinition? FindEntry(string entryId)
        {
            return !string.IsNullOrEmpty(entryId) && _entries.TryGetValue(entryId, out var e) ? e : null;
        }

        public IReadOnlyList<BlackMarketEntryDefinition> EntriesFor(string syndicateId)
        {
            var list = new List<BlackMarketEntryDefinition>();
            foreach (var e in _entries.Values)
            {
                if (e == null || e.syndicate_ids == null) continue;
                if (e.syndicate_ids.Contains(syndicateId, StringComparer.Ordinal)) list.Add(e);
            }
            list.Sort((a, b) => string.CompareOrdinal(a.entry_id, b.entry_id));
            return list;
        }

        internal void AddSyndicate(SyndicateProfile s) => _syndicates[s.syndicate_id] = s;
        internal void AddEntry(BlackMarketEntryDefinition e) => _entries[e.entry_id] = e;
    }

    /// <summary>
    /// Engine-agnostic loader for black_market_inventory.json. Shape/range
    /// validation here; item-id cross-references are validated at bind time
    /// against the bound GoodsCatalog (the trade authority), so an unknown
    /// item id can never become purchasable stock.
    /// </summary>
    public static class BlackMarketInventoryCatalogLoader
    {
        public const string FileName = "black_market_inventory.json";
        public const int CurrentSchemaVersion = 1;

        public const int MaxPremiumBp = 5000;         // ≤ the broker 1.25× precedent + headroom
        public const int MaxAccessTier = 3;

        public static BlackMarketInventoryLoadResult Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var result = new BlackMarketInventoryLoadResult();
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

            BlackMarketRoot root;
            try
            {
                root = json.Deserialize<BlackMarketRoot>(raw);
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

            // ── Syndicates ──
            var syndicateIds = new HashSet<string>(StringComparer.Ordinal);
            if (root.syndicates == null || root.syndicates.Count == 0)
            {
                result.Errors.Add("catalog syndicates array is null or empty");
            }
            else
            {
                foreach (var row in root.syndicates)
                {
                    if (row == null) { result.Errors.Add("syndicate entry is null"); continue; }
                    string id = row.syndicate_id ?? string.Empty;
                    if (string.IsNullOrWhiteSpace(id) || !SanitationFacilityCatalogLoader.IsSnakeCase(id))
                    {
                        result.Errors.Add($"syndicate id '{id}' is missing or not snake_case");
                        continue;
                    }
                    if (!syndicateIds.Add(id))
                    {
                        result.Errors.Add($"duplicate syndicate id '{id}'");
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(row.display_name))
                    {
                        result.Errors.Add($"'{id}' missing display_name");
                        continue;
                    }
                    if (row.base_premium_bp < 1000 || row.base_premium_bp > MaxPremiumBp)
                    {
                        result.Errors.Add($"'{id}' base_premium_bp must be in [1000,{MaxPremiumBp}] (got {row.base_premium_bp})");
                        continue;
                    }
                    if (row.sell_discount_bp < 0 || row.sell_discount_bp > 5000)
                    {
                        result.Errors.Add($"'{id}' sell_discount_bp must be in [0,5000] (got {row.sell_discount_bp})");
                        continue;
                    }
                    if (row.heat_decay_per_day < 0 || row.heat_decay_per_day > 50)
                    {
                        result.Errors.Add($"'{id}' heat_decay_per_day must be in [0,50] (got {row.heat_decay_per_day})");
                        continue;
                    }
                    if (row.credit_limit_units <= 0)
                    {
                        result.Errors.Add($"'{id}' credit_limit_units must be > 0 (got {row.credit_limit_units})");
                        continue;
                    }
                    if (row.loan_interest_bp < 0 || row.loan_interest_bp > 5000)
                    {
                        result.Errors.Add($"'{id}' loan_interest_bp must be in [0,5000] (got {row.loan_interest_bp})");
                        continue;
                    }
                    if (row.required_access_tier < 1 || row.required_access_tier > MaxAccessTier)
                    {
                        result.Errors.Add($"'{id}' required_access_tier must be in [1,{MaxAccessTier}]");
                        continue;
                    }
                    result.Syndicates.Add(new SyndicateProfile
                    {
                        syndicate_id = id,
                        display_name = row.display_name!.Trim(),
                        description = row.description ?? string.Empty,
                        base_premium_bp = row.base_premium_bp ?? 0,
                        sell_discount_bp = row.sell_discount_bp ?? 0,
                        heat_decay_per_day = row.heat_decay_per_day ?? 0,
                        credit_limit_units = row.credit_limit_units ?? 0,
                        loan_interest_bp = row.loan_interest_bp ?? 0,
                        required_access_tier = row.required_access_tier ?? 1
                    });
                }
            }

            // ── Entries ──
            if (root.entries == null)
            {
                result.Errors.Add("catalog entries array is null");
            }
            else
            {
                var seen = new HashSet<string>(StringComparer.Ordinal);
                for (int i = 0; i < root.entries.Count; i++)
                {
                    var row = root.entries[i];
                    if (row == null) { result.Errors.Add($"entry [{i}] is null"); continue; }
                    string id = row.entry_id ?? string.Empty;
                    if (string.IsNullOrWhiteSpace(id) || !SanitationFacilityCatalogLoader.IsSnakeCase(id))
                    {
                        result.Errors.Add($"entry [{i}] entry_id '{id}' missing or not snake_case");
                        continue;
                    }
                    if (!seen.Add(id))
                    {
                        result.Errors.Add($"duplicate entry_id '{id}' at entry [{i}]");
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(row.item_id))
                    {
                        result.Errors.Add($"'{id}' missing item_id");
                        continue;
                    }
                    if (row.syndicate_ids == null || row.syndicate_ids.Count == 0)
                    {
                        result.Errors.Add($"'{id}' must name at least one syndicate");
                        continue;
                    }
                    bool anyKnown = false;
                    foreach (var sid in row.syndicate_ids)
                    {
                        if (!syndicateIds.Contains(sid ?? string.Empty))
                            result.Errors.Add($"'{id}' references unknown syndicate '{sid}'");
                        else
                            anyKnown = true;
                    }
                    if (!anyKnown) continue;   // entry would be unreachable stock

                    if (row.access_tier < 1 || row.access_tier > MaxAccessTier)
                    {
                        result.Errors.Add($"'{id}' access_tier must be in [1,{MaxAccessTier}] (got {row.access_tier})");
                        continue;
                    }
                    if (row.stock_weight < 0 || row.stock_weight > 100)
                    {
                        result.Errors.Add($"'{id}' stock_weight must be in [0,100] (got {row.stock_weight})");
                        continue;
                    }
                    if (row.min_stock < 0 || row.max_stock < row.min_stock || row.max_stock <= 0)
                    {
                        result.Errors.Add($"'{id}' stock range invalid (min {row.min_stock}, max {row.max_stock})");
                        continue;
                    }
                    if (row.scarcity_sensitivity < 0 || row.scarcity_sensitivity > 100)
                    {
                        result.Errors.Add($"'{id}' scarcity_sensitivity must be in [0,100] (got {row.scarcity_sensitivity})");
                        continue;
                    }
                    if (row.risk_premium_bp < 1000 || row.risk_premium_bp > MaxPremiumBp)
                    {
                        result.Errors.Add($"'{id}' risk_premium_bp must be in [1000,{MaxPremiumBp}] (got {row.risk_premium_bp})");
                        continue;
                    }
                    if (row.cooldown_days < 0 || row.cooldown_days > 14)
                    {
                        result.Errors.Add($"'{id}' cooldown_days must be in [0,14] (got {row.cooldown_days})");
                        continue;
                    }

                    result.Entries.Add(new BlackMarketEntryDefinition
                    {
                        entry_id = id,
                        item_id = row.item_id!.Trim(),
                        syndicate_ids = row.syndicate_ids.Where(s => syndicateIds.Contains(s ?? string.Empty)).ToList(),
                        access_tier = row.access_tier ?? 1,
                        stock_weight = row.stock_weight ?? 0,
                        min_stock = row.min_stock ?? 0,
                        max_stock = row.max_stock ?? 0,
                        scarcity_sensitivity = row.scarcity_sensitivity ?? 0,
                        risk_premium_bp = row.risk_premium_bp ?? 0,
                        cooldown_days = row.cooldown_days ?? 0,
                        tags = row.tags ?? new List<string>()
                    });
                }
            }
            return result;
        }

        public static BlackMarketInventoryCatalog ToCatalog(BlackMarketInventoryLoadResult load)
        {
            var catalog = new BlackMarketInventoryCatalog();
            if (load == null) return catalog;
            foreach (var s in load.Syndicates) catalog.AddSyndicate(s);
            foreach (var e in load.Entries) catalog.AddEntry(e);
            return catalog;
        }

        private class BlackMarketRoot
        {
            public int schema_version = 1;
            public List<RawSyndicateProfile> syndicates = new List<RawSyndicateProfile>();
            public List<RawBlackMarketEntry> entries = new List<RawBlackMarketEntry>();
        }

        private class RawSyndicateProfile
        {
            public string syndicate_id;
            public string display_name;
            public string description;
            public int? base_premium_bp;
            public int? sell_discount_bp;
            public int? heat_decay_per_day;
            public int? credit_limit_units;
            public int? loan_interest_bp;
            public int? required_access_tier;
        }

        private class RawBlackMarketEntry
        {
            public string entry_id;
            public string item_id;
            public List<string> syndicate_ids;
            public int? access_tier;
            public int? stock_weight;
            public int? min_stock;
            public int? max_stock;
            public int? scarcity_sensitivity;
            public int? risk_premium_bp;
            public int? cooldown_days;
            public List<string> tags;
        }
    }
}
