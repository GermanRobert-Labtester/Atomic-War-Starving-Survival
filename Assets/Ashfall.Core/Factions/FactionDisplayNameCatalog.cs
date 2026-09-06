// SPDX-License-Identifier: MIT
// Faction display name resolver — loads from faction_lore.json and provides
// human-readable names for UI presentation. Falls back to ID humanization
// when lore data is unavailable.
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Ashfall.Core.Factions
{
    /// <summary>
    /// Resolves faction IDs to human-readable display names using faction_lore.json
    /// as the authority. Falls back to snake_case → Title Case humanization when
    /// no lore entry exists.
    /// </summary>
    public static class FactionDisplayNameCatalog
    {
        private static readonly Dictionary<string, string> _names = new(StringComparer.OrdinalIgnoreCase);
        private static bool _loaded;

        /// <summary>
        /// Loads display names from faction_lore.json content.
        /// Safe to call multiple times; subsequent calls are no-ops.
        /// </summary>
        public static void LoadFromJson(string json)
        {
            if (_loaded || string.IsNullOrWhiteSpace(json)) return;

            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                // faction_lore.json is shaped as { "items": [ { "faction_id": "...", "display_name": "..." } ] }
                if (root.TryGetProperty("items", out var items) && items.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in items.EnumerateArray())
                    {
                        string id = item.TryGetProperty("faction_id", out var idProp) ? idProp.GetString() ?? "" : "";
                        string name = item.TryGetProperty("display_name", out var nameProp) ? nameProp.GetString() ?? "" : "";
                        if (!string.IsNullOrWhiteSpace(id) && !string.IsNullOrWhiteSpace(name))
                        {
                            _names[id.Trim()] = name.Trim();
                        }
                    }
                }

                _loaded = true;
            }
            catch
            {
                // Silent fallback — humanization will be used instead
            }
        }

        /// <summary>
        /// Returns the human-readable display name for a faction ID.
        /// Uses lore data if loaded; otherwise humanizes the ID.
        /// </summary>
        public static string Resolve(string? factionId)
        {
            if (string.IsNullOrWhiteSpace(factionId)) return "Unknown Faction";

            string clean = factionId.Trim();

            // Try direct lore lookup
            if (_names.TryGetValue(clean, out string? name))
                return name;

            // Try without faction_ prefix
            string stripped = clean.StartsWith("faction_", StringComparison.OrdinalIgnoreCase)
                ? clean["faction_".Length..]
                : clean;
            if (_names.TryGetValue(stripped, out name))
                return name;

            // Fallback: humanize the ID
            return Humanize(stripped);
        }

        /// <summary>
        /// Returns true if the faction has a lore-derived display name.
        /// </summary>
        public static bool HasLoreName(string? factionId)
        {
            if (string.IsNullOrWhiteSpace(factionId)) return false;
            string clean = factionId.Trim();
            if (_names.ContainsKey(clean)) return true;
            string stripped = clean.StartsWith("faction_", StringComparison.OrdinalIgnoreCase)
                ? clean["faction_".Length..]
                : clean;
            return _names.ContainsKey(stripped);
        }

        /// <summary>
        /// Resets the catalog for testing. Not for production use.
        /// </summary>
        internal static void ResetForTesting()
        {
            _names.Clear();
            _loaded = false;
        }

        private static string Humanize(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return string.Empty;
            string[] parts = id.Split('_', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Length == 0) continue;
                parts[i] = char.ToUpperInvariant(parts[i][0]) +
                           (parts[i].Length > 1 ? parts[i][1..].ToLowerInvariant() : string.Empty);
            }
            return string.Join(" ", parts);
        }
    }
}
