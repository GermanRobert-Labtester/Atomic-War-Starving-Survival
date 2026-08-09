using System;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Survivors;
using UnityEngine;
using Random = System.Random;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Prompt #12 — Unexploded Ordnance (UXO): civil-war landmines left on
    /// map nodes. Reckless looting or panicked flee can trigger detonation.
    /// Pure C#; no MonoBehaviour.
    /// </summary>
    public static class UxoHazardSystem
    {
        /// <summary>Fraction of non-shelter nodes seeded with UXO at map gen.</summary>
        public const float MapUxoChance = 0.20f;

        /// <summary>Detonation chance when a Reckless survivor loots a UXO node.</summary>
        public const float RecklessLootDetonateChance = 0.55f;

        /// <summary>
        /// Detonation chance for careful looters (non-Reckless). Near-zero —
        /// they watch their footing.
        /// </summary>
        public const float CarefulLootDetonateChance = 0f;

        /// <summary>Detonation chance when fleeing an encounter on a UXO node.</summary>
        public const float FleeDetonateChance = 0.35f;

        /// <summary>Health lost on detonation (before clamp).</summary>
        public const float DetonationHealthDamage = 35f;

        public const string DetonationLogMessage =
            "The ground gave way. Something left over from a war that does not matter anymore.";

        /// <summary>
        /// Whether looting on a UXO node detonates for this trait.
        /// Caller must already know the node has UXO.
        /// </summary>
        public static bool ShouldDetonateOnLoot(RiskBiasTrait trait, Random rng)
        {
            if (rng == null) return false;
            float chance = trait == RiskBiasTrait.Reckless
                ? RecklessLootDetonateChance
                : CarefulLootDetonateChance;
            if (chance <= 0f) return false;
            return rng.NextDouble() < chance;
        }

        /// <summary>
        /// Whether fleeing across a UXO node detonates. Trait-independent —
        /// panic does not pick a careful path.
        /// </summary>
        public static bool ShouldDetonateOnFlee(Random rng)
        {
            if (rng == null) return false;
            return rng.NextDouble() < FleeDetonateChance;
        }

        /// <summary>
        /// Apply detonation consequences: health damage, broken bone, drop all loot,
        /// force inbound return. Returns false if survivor/exp invalid.
        /// </summary>
        public static bool ApplyDetonation(
            ExpeditionState exp,
            MedicalSystem medical = null)
        {
            if (exp?.Survivor == null || !exp.Survivor.IsAlive) return false;

            var s = exp.Survivor;
            if (s.Needs != null)
            {
                SurvivorNeedWrite.AdjustHealth(s, -DetonationHealthDamage);
            }

            if (medical != null && s.IsAlive)
            {
                medical.Inflict(s, AfflictionSO.Ids.BrokenBone);
            }

            // Everything dropped — no glory in the crater.
            exp.DropLoot(1f);
            exp.IsPushingLuck = false;
            exp.Phase = ExpeditionPhase.Inbound;
            exp.UxoDetonated = true;

            if (s.State == SurvivorState.Dead)
                exp.Phase = ExpeditionPhase.Failed;

            return true;
        }
    }
}
