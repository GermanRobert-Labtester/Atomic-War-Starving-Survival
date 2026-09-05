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

        // ── Plan 45 phase 2 — raid / wildlife / site-defense bindings ────

        // Raid strata (matrix §46): organized raider, heavy/enforcer,
        // scavenger attacker, desperate tag-along. Human-only pool —
        // wildlife never rides in a raid crew.
        private static readonly string[] RaidPool =
        {
            "combatant_warlord_veteran",
            "combatant_hydro_pump_warden",
            "combatant_salvage_veteran",
            "combatant_desperate_scavenger",
        };

        /// <summary>
        /// Raid crew composition (matrix §46): the Iron-Raiders / warlord
        /// raid event path. Danger-weighted anchor — an organized raider
        /// (warlord veteran) leads high-danger raids, a scavenger attacker
        /// the low-pressure ones — with the crew rotating through the four
        /// raid roles. Human-only: no fauna in a raid crew.
        /// </summary>
        public static IReadOnlyList<string> SelectRaidComposition(
            int dangerLevel, int enemyCount, ISeededRng? rng = null)
        {
            int count = enemyCount < 1 ? 1 : (enemyCount > MaxAmbushCount ? MaxAmbushCount : enemyCount);
            var result = new string[count];
            for (int i = 0; i < count; i++)
            {
                int idx = i == 0
                    ? (dangerLevel >= 5 ? 0 : 2)   // anchor: warlord enforcer vs scavenger attacker
                    : (dangerLevel + i) % RaidPool.Length;
                if (rng != null && i > 0)
                    idx = rng.Next(0, RaidPool.Length);
                result[i] = RaidPool[idx];
            }
            return result;
        }

        /// <summary>
        /// Wildlife pack composition (matrix §34/§47): a species tag from the
        /// data authority (travel encounter <c>combatant_tag</c>) maps to ONE
        /// fauna/mutant combatant id; the pack is that id repeated — a wolf
        /// pack is four wolves, not a wolf and two bears. Unknown or empty
        /// tags return an EMPTY list: an unbound creature encounter must not
        /// silently become humans in hides (Plan 54 §34).
        /// </summary>
        public static IReadOnlyList<string> SelectWildlifeComposition(string speciesTag, int packCount)
        {
            if (string.IsNullOrEmpty(speciesTag)) return Array.Empty<string>();
            string? id = speciesTag switch
            {
                "pack_canine" => "combatant_feral_mutt",      // wolf pack / hyena den
                "swarm" => "combatant_burrower_mite",          // slag beetles / timber ticks
                "lurker" => "combatant_pale_crawler",          // marsh adder slough
                "spore_predator" => "combatant_spore_hound",   // specter owl perch
                "charger" => "combatant_armored_boar",         // bristleback charge
                "apex" => "combatant_armored_boar",            // cave bear gallery (large-beast row)
                _ => null,                                     // unbound tag — honest empty
            };
            if (id == null) return Array.Empty<string>();
            int count = packCount < 1 ? 1 : (packCount > MaxAmbushCount ? MaxAmbushCount : packCount);
            var result = new string[count];
            for (int i = 0; i < count; i++) result[i] = id;
            return result;
        }

        /// <summary>
        /// Site-defense composition (matrix §38/§48, excavation seam): the
        /// hydro-baron pump warden — pinned position, never resolves, best
        /// cover in the catalog. The prepared API for Plan 37 site-defense
        /// encounters; no excavation runtime is required to exist yet.
        /// </summary>
        public static IReadOnlyList<string> SelectSiteDefense(int defenderCount)
        {
            int count = defenderCount < 1 ? 1 : (defenderCount > MaxAmbushCount ? MaxAmbushCount : defenderCount);
            var result = new string[count];
            for (int i = 0; i < count; i++) result[i] = "combatant_hydro_pump_warden";
            return result;
        }

        /// <summary>
        /// Router for hostile encounter choices (Plan 45 phase 2): a resolved
        /// travel-encounter choice that was neither nonviolent nor avoidance
        /// escalates to tactical combat. Creature encounters field the
        /// wildlife pack for their species tag; Human encounters field raid
        /// crews at high danger and ambush patrols below; every other
        /// category (Environmental, Social, Discovery, Trade, Chained) is
        /// non-combat and yields an empty list — the caller skips combat
        /// spawn entirely.
        /// </summary>
        public static IReadOnlyList<string> SelectForHostileEncounter(
            string category, string speciesTag, int dangerLevel, int enemyCount, ISeededRng? rng = null)
        {
            if (string.Equals(category, "Creature", StringComparison.Ordinal))
                return SelectWildlifeComposition(speciesTag, enemyCount);
            if (string.Equals(category, "Human", StringComparison.Ordinal))
                return dangerLevel >= 5
                    ? SelectRaidComposition(dangerLevel, enemyCount, rng)
                    : SelectAmbushComposition(dangerLevel, enemyCount, rng);
            return Array.Empty<string>();
        }

        private static string[] PoolFor(int dangerLevel)
        {
            if (dangerLevel <= 2) return LowBand;
            if (dangerLevel <= 5) return MediumBand;
            return HighBand;
        }
    }
}
