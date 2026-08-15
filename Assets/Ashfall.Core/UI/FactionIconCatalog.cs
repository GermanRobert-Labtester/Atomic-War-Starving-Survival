using System.Collections.Generic;

namespace Ashfall.Core.UI
{
    /// <summary>
    /// ASHFALL — engine-agnostic faction→icon path resolver.
    /// Owns the canonical mapping from systems faction ids
    /// (`currents.json`) to texture paths the hosts can fetch.
    /// Falls back to a known asset for any id that has no coverage,
    /// so callers (Trade, Radio, Dose, Verdict) never present a blank
    /// emblem to the player.
    /// No `UnityEngine` or `Godot` references — platform-agnostic.
    /// </summary>
    public static class FactionIconCatalog
    {
        /// <summary>Default fallback when an id has no emblem on disk.</summary>
        public const string FallbackIconPath = "Assets/UI/Icons/icon_unknown_faction.png";

        /// <summary>
        /// The 16 systems faction ids declared authoritative in
        /// `Assets/StreamingAssets/Data/currents.json`, mapped to
        /// the canonical emblem file each id currently resolves to.
        /// Aliases are explicit and documented; lore-namespace ids
        /// (`scavenger_camp`, `iron_garrison`, `cult_of_the_glow`,
        /// `militia`, `warlord`, etc.) are intentionally NOT in this
        /// map — they are visual cousins, not same fictional entities.
        /// </summary>
        private static readonly Dictionary<string, string> _systemsIdsToIcon =
            new Dictionary<string, string>(System.StringComparer.Ordinal)
            {
                // ── Systems namespace (currents.json) ──────────────────────
                { "faction_hydro_barons",        "Assets/UI/Icons/faction_icon_hydro_barons.png" },
                { "faction_archivists",          "Assets/UI/Icons/faction_icon_archivists.png" },
                { "faction_lamplighters",        "Assets/UI/Icons/faction_icon_lamplighters.png" },
                { "faction_quiet_house",         "Assets/UI/Icons/faction_icon_quiet_house.png" },
                { "faction_grain_exchange",      "Assets/UI/Icons/faction_icon_grain_exchange.png" },
                { "faction_sun_seekers",         "Assets/UI/Icons/faction_icon_sun_seekers.png" },
                { "faction_osteophages",         "Assets/UI/Icons/faction_icon_osteophages.png" },
                { "faction_the_tally",           "Assets/UI/Icons/faction_icon_the_tally.png" },
                { "faction_undertow",            "Assets/UI/Icons/faction_icon_undertow.png" },
                { "faction_cold_count",          "Assets/UI/Icons/faction_icon_cold_count.png" },
                { "faction_deserter_coalition",  "Assets/UI/Icons/faction_icon_deserter_coalition.png" },
                { "faction_the_provisioned",     "Assets/UI/Icons/faction_icon_the_provisioned.png" },
                { "faction_long_walk",           "Assets/UI/Icons/faction_icon_long_walk.png" },
                { "faction_scavenger_guild",     "Assets/UI/Icons/faction_icon_scavenger_guild.png" },
                { "faction_iron_raiders",        "Assets/UI/Icons/faction_icon_iron_raiders.png" },
                { "faction_the_tempest",         "Assets/UI/Icons/faction_icon_the_tempest.png" },

                // ── Lore namespace (Unity voice matrix, Trade, Radio) ─────
                { "scavenger_camp",              "Assets/UI/Icons/faction_icon_scavenger_camp.png" },
                { "cult_of_the_glow",            "Assets/UI/Icons/faction_icon_cult_of_the_glow.png" },
                { "military_remnants",           "Assets/UI/Icons/faction_icon_military_remnants.png" },
                { "upland_militia",              "Assets/UI/Icons/faction_icon_upland_militia.png" },
                { "rot_farmers",                 "Assets/UI/Icons/faction_icon_rot_farmers.png" },
                { "wire_heads",                  "Assets/UI/Icons/faction_icon_wire_heads.png" },
                { "sump_dredgers",               "Assets/UI/Icons/faction_icon_sump_dredgers.png" },
                { "custodians",                  "Assets/UI/Icons/faction_icon_custodians.png" },
                { "doomsday_preppers",           "Assets/UI/Icons/faction_icon_doomsday_preppers.png" },
                { "echo_bats",                   "Assets/UI/Icons/faction_icon_echo_bats.png" },
                { "safe_haven_community",        "Assets/UI/Icons/faction_icon_safe_haven_community.png" },
            };

        /// <summary>
        /// Resolve a faction id to its canonical emblem path.
        /// Returns <see cref="FallbackIconPath"/> for any unintended
        /// miss; returns the same fallback for null / empty input.
        /// </summary>
        public static string Resolve(string factionId)
        {
            if (string.IsNullOrEmpty(factionId)) return FallbackIconPath;
            return _systemsIdsToIcon.TryGetValue(factionId, out var path)
                ? path
                : FallbackIconPath;
        }

        /// <summary>
        /// Test/observability surface: returns true iff the id was explicitly
        /// mapped. Lets callers distinguish "real mapping" from "silently fell
        /// back to the default emblem". Do not use as a hard validation gate
        /// in production code.
        /// </summary>
        public static bool HasExplicitMapping(string factionId)
        {
            if (string.IsNullOrEmpty(factionId)) return false;
            return _systemsIdsToIcon.ContainsKey(factionId);
        }

        /// <summary>
        /// Read-only enumeration of the canonical systems faction ids this
        /// catalog maps. Useful for validator/static-audit tooling.
        /// </summary>
        public static IReadOnlyCollection<string> CoveredFactionIds =>
            _systemsIdsToIcon.Keys;
    }
}
