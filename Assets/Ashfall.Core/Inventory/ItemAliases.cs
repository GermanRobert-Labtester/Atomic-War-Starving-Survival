using System;
using System.Collections.Generic;

namespace Ashfall.Core.Inventory
{
    /// <summary>
    /// Canonical resource and item ID resolver. Maps legacy prefixed IDs (item_*)
    /// and aliases to canonical item IDs defined in the JSON data authority.
    /// </summary>
    public static class ItemAliases
    {
        private static readonly Dictionary<string, string> Aliases = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "item_canned_food", "canned_food" },
            { "item_purified_water", "clean_water" },
            { "item_clean_water", "clean_water" },
            { "item_fuel_canister", "fuel_canister" },
            { "item_scrap_metal", "scrap_mechanical" },
            { "item_scrap_mechanical", "scrap_mechanical" },
            { "item_electronics", "scrap_electronic" },
            { "item_scrap_electronic", "scrap_electronic" },
            { "item_first_aid_kit", "first_aid" },
            { "first_aid_kit", "first_aid" },
            { "item_bandage", "bandage" },
            { "item_iodine_pills", "iodine_pills" },
            { "item_rad_away", "rad_away" },
            { "item_battery", "battery" },
            { "item_gas_mask", "gas_mask" },
            { "item_hazmat_suit", "hazmat_suit" },
            { "item_herbal_antibiotic", "herbal_antibiotics" },
            { "item_antibiotic", "antibiotics" },
            { "item_anti_rad_tea", "rad_tea" },
            { "item_salted_meat", "canned_food" },
            { "item_ammo_9mm", "ammo_9mm" },
            { "item_medkit", "first_aid" }
        };

        /// <summary>
        /// Returns the canonical item ID for any given item identifier or alias.
        /// </summary>
        public static string ToCanonical(string? id)
        {
            if (string.IsNullOrWhiteSpace(id)) return string.Empty;
            string clean = id.Trim();
            return Aliases.TryGetValue(clean, out string? canonical) ? canonical : clean;
        }

        /// <summary>
        /// True if the given ID is a known alias that maps to a different canonical ID.
        /// </summary>
        public static bool IsAlias(string? id, out string canonical)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                canonical = string.Empty;
                return false;
            }
            string clean = id.Trim();
            if (Aliases.TryGetValue(clean, out string? resolved) && !string.Equals(clean, resolved, StringComparison.Ordinal))
            {
                canonical = resolved;
                return true;
            }
            canonical = clean;
            return false;
        }
    }
}
