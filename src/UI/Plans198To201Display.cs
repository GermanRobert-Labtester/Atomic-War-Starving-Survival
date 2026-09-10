// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plans 198–201: shared player-facing display names for catalog entities.
    /// Raw authored IDs are never rendered in prose — this is the single
    /// prettification authority for the four late-game consoles.
    /// Faction list mirrors the canonical manifest used by FactionMatrixPanel.
    /// </summary>
    internal static class ItemDisplay
    {
        public static string Name(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return "supplies";
            return Prettify(itemId);
        }

        /// <summary>snake_case / kebab-case → Title Case without raw underscores.</summary>
        internal static string Prettify(string id)
        {
            string trimmed = id.Trim();
            if (trimmed.Length == 0) return trimmed;
            string[] parts = trimmed.Split('_', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Length == 0) continue;
                parts[i] = char.ToUpperInvariant(parts[i][0]) + parts[i][1..].ToLowerInvariant();
            }
            return string.Join(' ', parts);
        }
    }

    /// <summary>Canonical faction manifest for invitation surfaces.</summary>
    internal static class FactionDisplay
    {
        internal static readonly IReadOnlyList<(string id, string display)> KnownFactions = new List<(string, string)>
        {
            ("faction_central_garrison", "The Iron Garrison"),
            ("warlords_sector_4", "The Warlords of Sector 4"),
            ("ash_militia", "The Ash Militia"),
            ("cult_of_ash_sign", "The Cult of the Ash Sign"),
            ("faction_rebuilders", "The Rebuilders"),
        };

        public static string Name(string factionId)
        {
            foreach (var f in KnownFactions)
            {
                if (string.Equals(f.id, factionId, StringComparison.Ordinal))
                    return f.display;
            }
            return ItemDisplay.Prettify(factionId ?? string.Empty);
        }
    }

    /// <summary>Player-readable text for authored ceremony disaster IDs.</summary>
    internal static class DisasterDisplay
    {
        public static string Name(string disasterId)
        {
            if (string.IsNullOrEmpty(disasterId)) return "None";
            if (disasterId.StartsWith("disaster_", StringComparison.Ordinal))
                disasterId = disasterId["disaster_".Length..];
            return ItemDisplay.Prettify(disasterId);
        }
    }

    /// <summary>Player-readable text for authored robot directive IDs.</summary>
    internal static class DirectiveDisplay
    {
        public static string Name(string directiveId)
        {
            if (string.IsNullOrEmpty(directiveId)) return "Idle";
            if (directiveId.StartsWith("directive_", StringComparison.Ordinal))
                directiveId = directiveId["directive_".Length..];
            return ItemDisplay.Prettify(directiveId);
        }

        /// <summary>Directive roster offered by the robotics console.</summary>
        internal static readonly IReadOnlyList<(string id, string display)> KnownDirectives = new List<(string, string)>
        {
            ("directive_idle", "Idle (dock and recharge)"),
            ("directive_haul", "Hauling (shelter cargo)"),
            ("directive_guard", "Guard duty (perimeter)"),
            ("directive_repair", "Maintenance labor"),
            ("directive_scout", "Recon sweeps"),
        };
    }
}
