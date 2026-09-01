// SPDX-License-Identifier: MIT
// ASHFALL: centralised allowed AI moves for the combat catalog. Single
// source of truth so the loader (CombatCatalogLoader), the AI step in
// TacticalCombatSystem.Damage.cs, and any future host code can all ask
// the same set without re-rolling names in two places.
//
// The static class is named CombatAiMoves (plural) to avoid colliding with
// the CombatAiMove enum when extending it later.

using System.Collections.Generic;

namespace Ashfall.Core.Combat
{
    /// <summary>
    /// Closed enumeration of AI special moves a combatant can declare in
    /// combat_catalog.json. The literal-string set is narrow and the
    /// loader rejects any value outside this list. <see cref="None"/> is
    /// the default sentinel for a combatant with no special move.
    /// </summary>
    public enum CombatAiMove
    {
        None = 0,
        Burrow = 1,
        Flank = 2,
        Spore = 3,
        Charge = 4,
        SuppressiveFire = 5,
        TacticalRetreat = 6
    }

    /// <summary>
    /// Closed string-set authority for AI move names. Consulted by the
    /// catalog loader (during validation) and by any future host that
    /// reads a combatant's ai_special_move field. Adding a new move is
    /// a one-place change here.
    /// </summary>
    public static class CombatAiMoves
    {
        /// <summary>Cached, ordinal-ordered array of allowed move names (excluding None).</summary>
        public static readonly string[] AllowedNames = new[]
        {
            "Burrow",
            "Flank",
            "Spore",
            "Charge",
            "SuppressiveFire",
            "TacticalRetreat"
        };

        /// <summary>
        /// True when the supplied name is null/empty (treated as the
        /// default sentinel), "None", or one of the entries in
        /// <see cref="AllowedNames"/>.
        /// </summary>
        public static bool IsAllowed(string name)
        {
            if (string.IsNullOrEmpty(name)) return true;
            if (name == "None") return true;
            for (int i = 0; i < AllowedNames.Length; i++)
                if (AllowedNames[i] == name) return true;
            return false;
        }
    }
}
