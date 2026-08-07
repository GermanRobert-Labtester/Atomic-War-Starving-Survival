using System;

namespace AtomicWar._Game.Inventory
{
    /// <summary>
    /// Single source of truth for turning an <c>equipSlot</c> string into an
    /// <see cref="EquipSlot"/>.
    ///
    /// Why this exists: the JSON import gate and the runtime world-catalog loader
    /// each had their own parser, and they disagreed. The importer used a strict
    /// <c>Enum.TryParse</c>; the loader used a hand-written switch that also
    /// accepted "torso"/"chest". Data written as <c>"Torso"</c> therefore loaded
    /// fine at runtime but hard-failed the build's data-validation gate — and any
    /// spelling neither parser knew fell through to <see cref="EquipSlot.None"/>,
    /// silently making an item unequippable instead of reporting the typo.
    ///
    /// Both callers now go through <see cref="TryParse"/>. Legacy spellings are
    /// still accepted at runtime so old saves and mod data keep working, but
    /// <see cref="IsCanonicalName"/> lets the import gate insist that shipped data
    /// uses the canonical enum name.
    /// </summary>
    public static class EquipSlots
    {
        /// <summary>
        /// Historical spellings that map onto a canonical slot. Kept permanently:
        /// dropping one silently downgrades an item to <see cref="EquipSlot.None"/>.
        /// </summary>
        private static readonly (string Alias, EquipSlot Slot)[] LegacyAliases =
        {
            ("torso", EquipSlot.Body),
            ("chest", EquipSlot.Body),
        };

        /// <summary>Canonical slot names, for error messages and tooling.</summary>
        public static string[] CanonicalNames => Enum.GetNames(typeof(EquipSlot));

        /// <summary>
        /// Parse a slot string. Empty/null yields <see cref="EquipSlot.None"/> and
        /// returns true — "no slot" is a valid, expected state for most items.
        /// Returns false only for a genuinely unrecognised spelling, so callers can
        /// report it instead of silently dropping the slot.
        /// </summary>
        public static bool TryParse(string raw, out EquipSlot slot)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                slot = EquipSlot.None;
                return true;
            }

            string key = raw.Trim();
            if (Enum.TryParse(key, true, out slot))
                return true;

            for (int i = 0; i < LegacyAliases.Length; i++)
            {
                if (string.Equals(key, LegacyAliases[i].Alias, StringComparison.OrdinalIgnoreCase))
                {
                    slot = LegacyAliases[i].Slot;
                    return true;
                }
            }

            slot = EquipSlot.None;
            return false;
        }

        /// <summary>
        /// Parse, falling back to <paramref name="fallback"/> when unrecognised.
        /// Use only where there is no channel to report the failure on.
        /// </summary>
        public static EquipSlot Parse(string raw, EquipSlot fallback = EquipSlot.None) =>
            TryParse(raw, out var slot) ? slot : fallback;

        /// <summary>
        /// True when <paramref name="raw"/> is an exact canonical enum name
        /// (case-insensitive), or empty. Legacy aliases return false so the data
        /// validation gate can require canonical spellings in shipped JSON.
        /// </summary>
        public static bool IsCanonicalName(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return true;
            return Enum.TryParse<EquipSlot>(raw.Trim(), true, out _);
        }

        /// <summary>
        /// The canonical name a legacy alias resolves to, or null when
        /// <paramref name="raw"/> is not a known alias. Used to make the data
        /// validation error message actionable ("use 'Body' instead of 'Torso'").
        /// </summary>
        public static string CanonicalNameForAlias(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return null;
            string key = raw.Trim();
            for (int i = 0; i < LegacyAliases.Length; i++)
            {
                if (string.Equals(key, LegacyAliases[i].Alias, StringComparison.OrdinalIgnoreCase))
                    return LegacyAliases[i].Slot.ToString();
            }
            return null;
        }
    }
}
