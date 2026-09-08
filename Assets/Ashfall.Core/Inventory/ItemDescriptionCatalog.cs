using System;
using System.Collections.Generic;

namespace Ashfall.Core.Inventory
{
    /// <summary>
    /// Engine-agnostic catalog of rich item description overlays from item_description_texts.json.
    /// Provides read-only sensory, visual, hazard, and operational prose to enrich item inspection.
    /// Supports deterministic alias resolution to bridge legacy item ids and canonical variants.
    /// </summary>
    public sealed class ItemDescriptionCatalog
    {
        private readonly Dictionary<string, ItemDescriptionEntry> _entries = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, string> _aliases = new(StringComparer.OrdinalIgnoreCase);

        public int Count => _entries.Count;
        public IReadOnlyCollection<string> Ids => _entries.Keys;
        public IReadOnlyCollection<ItemDescriptionEntry> Entries => _entries.Values;

        public ItemDescriptionCatalog()
        {
            RegisterDefaultAliases();
        }

        public bool Register(ItemDescriptionEntry entry)
        {
            if (entry == null || string.IsNullOrWhiteSpace(entry.ItemId))
                return false;

            entry.Normalize();
            string key = entry.ItemId.Trim();
            if (_entries.ContainsKey(key))
                return false; // deduplicate/reject duplicate

            _entries[key] = entry;
            return true;
        }

        public void RegisterAlias(string aliasId, string targetId)
        {
            if (string.IsNullOrWhiteSpace(aliasId) || string.IsNullOrWhiteSpace(targetId))
                return;
            _aliases[aliasId.Trim()] = targetId.Trim();
        }

        public ItemDescriptionEntry? Get(string? itemId)
        {
            if (string.IsNullOrWhiteSpace(itemId))
                return null;

            string key = itemId.Trim();
            if (_entries.TryGetValue(key, out var direct))
                return direct;

            // Check registered description aliases
            if (_aliases.TryGetValue(key, out var targetKey) && _entries.TryGetValue(targetKey, out var aliased))
                return aliased;

            // Check canonical item alias mapping via ItemAliases
            string canonical = ItemAliases.ToCanonical(key);
            if (!string.Equals(canonical, key, StringComparison.OrdinalIgnoreCase) && _entries.TryGetValue(canonical, out var fromCanonical))
                return fromCanonical;

            // Check prefix variations (e.g. item_dosimeter -> dosimeter)
            if (key.StartsWith("item_", StringComparison.OrdinalIgnoreCase))
            {
                string stripped = key.Substring(5);
                if (_entries.TryGetValue(stripped, out var fromStripped))
                    return fromStripped;
                if (_aliases.TryGetValue(stripped, out var strippedTarget) && _entries.TryGetValue(strippedTarget, out var fromStrippedTarget))
                    return fromStrippedTarget;
            }

            return null;
        }

        public bool Contains(string? itemId) => Get(itemId) != null;

        private void RegisterDefaultAliases()
        {
            // Core Starting Supplies & Dual-Identity Items
            RegisterAlias("item_dosimeter_pen", "dosimeter");
            RegisterAlias("item_dosimeter", "dosimeter");
            RegisterAlias("item_geiger_m3", "geiger_counter");
            RegisterAlias("item_air_filter_hepa", "air_filter");
            RegisterAlias("item_desal_membrane", "water_filter");
            RegisterAlias("rad_away", "anti_rad");
            RegisterAlias("item_rad_away", "anti_rad");
            RegisterAlias("scrap_mechanical", "scrap_metal");
            RegisterAlias("item_scrap_mechanical", "scrap_metal");
            RegisterAlias("scrap_electronic", "electronic_scrap");
            RegisterAlias("item_scrap_electronic", "electronic_scrap");
            RegisterAlias("item_clean_water", "clean_water");
            RegisterAlias("item_canned_food", "canned_food");
            RegisterAlias("item_irradiated_water", "irradiated_water");
            RegisterAlias("item_iodine_pills", "iodine_pills");
            RegisterAlias("item_bandage", "bandage");
            RegisterAlias("item_medical_kit", "medical_kit");
            RegisterAlias("item_gas_mask", "gas_mask");
            RegisterAlias("item_hazmat_suit", "hazmat_suit");
            RegisterAlias("item_battery", "battery");
            RegisterAlias("item_cloth", "cloth");
            RegisterAlias("item_chemicals", "chemicals");
            RegisterAlias("item_mechanical_parts", "mechanical_parts");
            RegisterAlias("item_fuel", "fuel");
            RegisterAlias("item_handheld_radio", "handheld_radio");

            // Canonical Trade, Medical, Consumables & Equipment Aliases
            RegisterAlias("water_purification_tablets", "survival_water_purification_tablets");
            RegisterAlias("jewelry", "luxury_jewelry");
            RegisterAlias("book", "luxury_book");
            RegisterAlias("rope", "material_rope");
            RegisterAlias("battery_pack", "electronics_battery_pack");
            RegisterAlias("stethoscope", "electronics_stethoscope");
            RegisterAlias("ammo_9x19", "ammo_9mm");
            RegisterAlias("ammo_9mm", "ammo_9mm");
            RegisterAlias("pistol_cz75_9x19", "weapon_pistol");
            RegisterAlias("service_pistol", "weapon_pistol");
            RegisterAlias("weapon_sidearm", "weapon_pistol");
            RegisterAlias("weapon_pipe_shotgun", "weapon_shotgun");
            RegisterAlias("weapon_marksman_rifle", "weapon_sniper_rifle");
            RegisterAlias("item_heavy_wool_coat", "clothing_coat");
            RegisterAlias("item_insulated_boots", "clothing_boots");
            RegisterAlias("item_fur_mittens", "clothing_gloves");
        }
    }
}
