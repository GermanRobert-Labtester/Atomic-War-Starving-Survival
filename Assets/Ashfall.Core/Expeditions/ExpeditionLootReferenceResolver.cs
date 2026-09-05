// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Expeditions
{
    /// <summary>
    /// Type of entity referenced by a token in an expedition's lootCategories array.
    /// Case C: Mixed namespace containing direct item IDs and semantic category/vocabulary tokens.
    /// </summary>
    public enum ExpeditionLootReferenceType
    {
        Unknown = 0,
        Item = 1,
        Category = 2
    }

    /// <summary>
    /// Explicit resolver for expedition loot references.
    /// Distinguishes direct item IDs from semantic category/vocabulary tokens.
    /// </summary>
    public interface IExpeditionLootReferenceResolver
    {
        ExpeditionLootReferenceType Resolve(string reference, out string canonicalId);
    }

    /// <summary>
    /// Production implementation of IExpeditionLootReferenceResolver.
    /// Reusable across catalog integrity validation, content utilization scanning, and expedition runtime.
    /// </summary>
    public sealed class ExpeditionLootReferenceResolver : IExpeditionLootReferenceResolver
    {
        private readonly HashSet<string> _itemIds;
        private readonly HashSet<string> _knownCategories;

        /// <summary>
        /// Authoritative list of semantic loot categories and vocabulary tokens used across expeditions and locations.
        /// </summary>
        public static readonly string[] DefaultLootCategories = new[]
        {
            "alloy_plates", "ammo", "antiseptic", "badges", "bandages", "batteries", "capacitors", "coal",
            "copper_wire", "drill_bits", "electronics", "fertilizer", "filters", "firewood", "game_scraps",
            "gasoline", "hand_tools", "heavy_armor", "iodine", "kerosene", "keys", "medical", "medicinal_herbs",
            "military_gear", "miner_helmets", "radio_parts", "rations", "resin", "riot_shields", "rocket_fuel",
            "rodent_scraps", "sacks", "scrap_cloth", "sealed_cans", "sedatives", "seeds", "shotgun_shells",
            "spare_tires", "steel_rails", "surgical_kits", "tools", "trade_goods", "vacuum_tubes", "valves",
            "water_filters", "waterproof_gear", "wheat", "wiring"
        };

        public ExpeditionLootReferenceResolver(
            IEnumerable<string>? itemIds = null,
            IEnumerable<string>? knownCategories = null)
        {
            _itemIds = itemIds != null
                ? new HashSet<string>(itemIds, StringComparer.OrdinalIgnoreCase)
                : new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            _knownCategories = knownCategories != null
                ? new HashSet<string>(knownCategories, StringComparer.OrdinalIgnoreCase)
                : new HashSet<string>(DefaultLootCategories, StringComparer.OrdinalIgnoreCase);
        }

        public void RegisterItem(string itemId)
        {
            if (!string.IsNullOrEmpty(itemId))
                _itemIds.Add(itemId);
        }

        public void RegisterCategory(string categoryId)
        {
            if (!string.IsNullOrEmpty(categoryId))
                _knownCategories.Add(categoryId);
        }

        public ExpeditionLootReferenceType Resolve(string reference, out string canonicalId)
        {
            canonicalId = reference ?? string.Empty;
            if (string.IsNullOrWhiteSpace(reference))
                return ExpeditionLootReferenceType.Unknown;

            // 1. Direct item match
            if (_itemIds.Contains(reference))
                return ExpeditionLootReferenceType.Item;

            // 2. Semantic category match
            if (_knownCategories.Contains(reference))
                return ExpeditionLootReferenceType.Category;

            return ExpeditionLootReferenceType.Unknown;
        }
    }
}
