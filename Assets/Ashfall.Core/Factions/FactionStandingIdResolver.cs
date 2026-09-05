// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Factions
{
    /// <summary>
    /// Canonical resolver mapping narrative, patrol, and lore faction identifiers
    /// to their authoritative systems IDs in <see cref="Ashfall.Core.YearOfAsh.FactionWarSystem"/>
    /// and territorial catalogs. Prevents duplicate faction standing records and ensures
    /// cross-system consistency.
    /// </summary>
    public static class FactionStandingIdResolver
    {
        private static readonly Dictionary<string, string> LoreToSystemsMap =
            new(StringComparer.OrdinalIgnoreCase)
            {
                // Iron Garrison / Central Garrison
                { "iron_garrison", "faction_central_garrison" },
                { "central_garrison", "faction_central_garrison" },
                { "garrison", "faction_central_garrison" },
                { "faction_central_garrison", "faction_central_garrison" },

                // Ash Militia / Upland Militia
                { "ash_militia", "faction_upland_militia" },
                { "upland_militia", "faction_upland_militia" },
                { "militia", "faction_upland_militia" },
                { "faction_upland_militia", "faction_upland_militia" },

                // Cult of the Ash Sign / Cult of the Glow
                { "cult_of_ash_sign", "faction_cult_of_the_glow" },
                { "cult", "faction_cult_of_the_glow" },
                { "cult_of_the_glow", "faction_cult_of_the_glow" },
                { "faction_cult_of_the_glow", "faction_cult_of_the_glow" },

                // Warlords of Sector 4 / Scavenger Warlords
                { "warlords_sector_4", "faction_scavenger_warlords" },
                { "warlords", "faction_scavenger_warlords" },
                { "warlord", "faction_scavenger_warlords" },
                { "sector_4_warlords", "faction_scavenger_warlords" },
                { "faction_scavenger_warlords", "faction_scavenger_warlords" },

                // Black Ops
                { "black_ops", "faction_black_ops" },
                { "faction_black_ops", "faction_black_ops" },

                // Railway Guild
                { "railway_guild", "faction_railway_guild" },
                { "faction_railway_guild", "faction_railway_guild" },

                // Hydro Barons
                { "hydro_barons", "faction_hydro_barons" },
                { "faction_hydro_barons", "faction_hydro_barons" },

                // Ordnance Foundry
                { "ordnance_foundry", "faction_ordnance_foundry" },
                { "foundry", "faction_ordnance_foundry" },
                { "faction_ordnance_foundry", "faction_ordnance_foundry" },

                // Supply Corps
                { "supply_corps", "faction_supply_corps" },
                { "corps", "faction_supply_corps" },
                { "faction_supply_corps", "faction_supply_corps" },

                // Ash Sign
                { "ash_sign", "faction_ash_sign" },
                { "faction_ash_sign", "faction_ash_sign" },

                // Scavengers
                { "scavengers", "faction_scavengers" },
                { "scavenger", "faction_scavengers" },
                { "faction_scavengers", "faction_scavengers" },

                // Penal Battalion
                { "penal_battalion", "faction_penal_battalion" },
                { "penal", "faction_penal_battalion" },
                { "faction_penal_battalion", "faction_penal_battalion" },

                // Rebuilders
                { "rebuilders", "faction_rebuilders" },
                { "faction_rebuilders", "faction_rebuilders" },

                // Forward Roster
                { "forward_roster", "faction_forward_roster" },
                { "faction_forward_roster", "faction_forward_roster" }
            };

        /// <summary>
        /// Converts any lore or alias faction identifier to its canonical systems ID.
        /// If the ID is already canonical or unknown, returns the trimmed ID.
        /// </summary>
        public static string ToSystemsId(string? factionLoreOrSystemsId)
        {
            if (string.IsNullOrWhiteSpace(factionLoreOrSystemsId))
                return string.Empty;

            string clean = factionLoreOrSystemsId.Trim();
            if (LoreToSystemsMap.TryGetValue(clean, out string? systemsId))
            {
                return systemsId;
            }

            return clean;
        }

        /// <summary>
        /// Attempts to map a lore or alias faction identifier to a systems ID.
        /// Returns true if a known mapping exists.
        /// </summary>
        public static bool TryToSystemsId(string? loreId, out string systemsId)
        {
            systemsId = string.Empty;
            if (string.IsNullOrWhiteSpace(loreId))
                return false;

            return LoreToSystemsMap.TryGetValue(loreId.Trim(), out systemsId!);
        }

        /// <summary>
        /// Returns true if the identifier is recognized as a known lore or systems faction.
        /// </summary>
        public static bool IsKnownFaction(string? factionId)
        {
            if (string.IsNullOrWhiteSpace(factionId))
                return false;

            return LoreToSystemsMap.ContainsKey(factionId.Trim());
        }
    }
}
