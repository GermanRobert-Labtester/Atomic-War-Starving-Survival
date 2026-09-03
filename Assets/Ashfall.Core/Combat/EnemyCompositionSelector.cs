using System;
using System.Collections.Generic;

namespace Ashfall.Core.Combat
{
    /// <summary>
    /// Plan 45 — engine-agnostic enemy composition selector. Maps expedition
    /// ambush context (location danger band, enemy count) onto registered
    /// <c>combatant_*</c> ids from the combat catalog, per the Plan 45 binding
    /// matrix (docs/combat/PATROL_RAID_BINDINGS.md).
    ///
    /// Pure function: no engine types, no state, no hidden randomness. The
    /// optional <see cref="ISeededRng"/> is the host's shared stream (same
    /// ordering decision as <see cref="Ashfall.Core.Expeditions.ExpeditionEncounterBridge"/>:
    /// one deterministic stream per expedition tick); when omitted the pick is
    /// a stable rotation of the band pool so pure calls are fully
    /// deterministic too.
    ///
    /// Threat bands are grounded in the authored location danger distribution
    /// (locations.json: dangerLevel 0..10, bell centered 5–7):
    ///   low    danger ≤ 2 — desperate stragglers and feral dogs;
    ///   medium 3..5       — veteran scavengers, conscripts, flotilla patrols, spore hounds;
    ///   high   ≥ 6        — warlord veterans, site wardens, the deep-wood mutants.
    ///
    /// Faction association travels on the combatant rows themselves
    /// (faction_id → faction_lore.json); the selector is the extension point
    /// where future data-driven faction weapon tables (Plan 54 §79.7) plug in.
    /// Every id returned is a compile-time constant also pinned by
    /// Plan45EnemyCompositionTests against the loaded catalog; if a catalog
    /// row is ever missing, BeginEncounter's honest enemy_catalog_missing
    /// fallback keeps the encounter playable.
    /// </summary>
    public static class EnemyCompositionSelector
    {
        /// <summary>Upper bound on a single ambush group (pack pressure is encounter-side).</summary>
        public const int MaxAmbushCount = 6;

        // Plan 45 patrol binding matrix — low band (danger ≤ 2).
        private static readonly string[] LowBand =
        {
            "combatant_desperate_scavenger",
            "combatant_feral_mutt",
        };

        // Plan 45 patrol binding matrix — medium band (danger 3..5).
        private static readonly string[] MediumBand =
        {
            "combatant_salvage_veteran",
            "combatant_conscript_levy",
            "combatant_flotilla_marine",
            "combatant_spore_hound",
        };

        // Plan 45 patrol binding matrix — high band (danger ≥ 6).
        private static readonly string[] HighBand =
        {
            "combatant_warlord_veteran",
            "combatant_hydro_pump_warden",
            "combatant_pale_crawler",
            "combatant_chrome_loper",
            "combatant_armored_boar",
        };

        /// <summary>
        /// Select the ambush composition for an expedition encounter.
        /// Returns exactly <paramref name="enemyCount"/> ids (clamped to
        /// 1..<see cref="MaxAmbushCount"/>). The first slot is always the
        /// band's anchor archetype (the matrix's primary threat); remaining
        /// slots rotate (or roll, when a stream is supplied) through the band
        /// pool so packs read as coherent groups rather than a grab bag.
        /// </summary>
        public static IReadOnlyList<string> SelectAmbushComposition(
            int dangerLevel, int enemyCount, ISeededRng? rng = null)
        {
            int count = enemyCount < 1 ? 1 : (enemyCount > MaxAmbushCount ? MaxAmbushCount : enemyCount);
            string[] pool = PoolFor(dangerLevel);

            var result = new string[count];
            for (int i = 0; i < count; i++)
            {
                // Anchor first (band's primary threat), then stable rotation.
                // The rotation is offset by the danger level so different
                // locations on the same band do not always field identical
                // groups even without a shared RNG stream.
                int idx = i == 0
                    ? 0
                    : (dangerLevel + i) % pool.Length;
                if (rng != null && i > 0)
                    idx = rng.Next(0, pool.Length);
                result[i] = pool[idx];
            }
            return result;
        }

        /// <summary>The threat band pool for a location danger level.</summary>
        public static IReadOnlyList<string> BandPool(int dangerLevel)
        {
            return PoolFor(dangerLevel);
        }

        private static string[] PoolFor(int dangerLevel)
        {
            if (dangerLevel <= 2) return LowBand;
            if (dangerLevel <= 5) return MediumBand;
            return HighBand;
        }
    }
}
