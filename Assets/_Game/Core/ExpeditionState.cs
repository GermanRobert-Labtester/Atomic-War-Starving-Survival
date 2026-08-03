using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    public enum ExpeditionStance
    {
        Stealth, // Lower encounter chance, standard travel speed, less noise
        Speed    // Faster travel (1.5x speed), higher encounter chance / noise
    }

    public enum ExpeditionPhase
    {
        Outbound,        // Traveling to target location node
        Looting,         // At target location node (Push-Your-Luck phase)
        Inbound,         // Returning to shelter
        AtHatchDilemma,  // At the hatch; the player must decide whether to let them in
        Completed,       // Returned safely with loot unloaded
        Failed           // Survivor died or collapsed
    }

    /// <summary>
    /// How a survivor reacted when caught in a Day-30 Flashpoint while on
    /// expedition. The intercept system sets this once based on the survivor's
    /// <see cref="RiskBiasTrait"/>; it persists with the expedition so save/load
    /// round-trips preserve the trait-driven state (dropped loot, paused ETA,
    /// speed multiplier, acquired afflictions).
    /// </summary>
    public enum FlashpointBehavior
    {
        None = 0,
        RecklessPushThrough, // Default: keep loot, normal return speed, full rad accumulation
        ParanoidSprint,      // Drop all loot, sprint home empty-handed
        CautiousShelter,     // Pause ETA 12-24 ticks, gain RadiationAnxiety
        FatalistNumbWalk     // Keep loot, halve return speed, gain Numbness
    }

    /// <summary>
    /// Runtime model tracking an active survivor expedition: stance, location node,
    /// carrying capacity, collected loot, stamina drain, suit wear, and phase progress.
    /// Save/load safe.
    /// </summary>
    [Serializable]
    public class ExpeditionState
    {
        public string ExpeditionId;
        public string SurvivorId;
        public string TargetLocationId;
        public string TargetLocationName;

        public ExpeditionStance Stance = ExpeditionStance.Stealth;
        public ExpeditionPhase Phase = ExpeditionPhase.Outbound;

        public int CurrentTick;
        public int TotalDistanceTicks;
        public int TravelTicksCompleted;
        public int LootingTicksCompleted;

        public float CarryingCapacity = 30f;
        public float CurrentWeight;
        public float Stamina = 100f;
        public float SuitDegradation;

        public float TrueRadPerHour;
        public float DangerLevel = 1f;

        public bool IsPushingLuck;
        public bool IsRetreating;

        /// <summary>
        /// True once a location-bound forceOnArrival encounter has fired for
        /// this expedition (Prompt #47 — Safe Haven ambush / empty cache).
        /// </summary>
        public bool LocationEncounterFired;

        // -----------------------------------------------------------------
        // Day-30 Flashpoint intercept state (Prompt #26)
        // -----------------------------------------------------------------

        /// <summary>
        /// True when the EMP severed comms to this expedition. The UI must
        /// replace ETA / health / status with "SIGNAL LOST" and disable the
        /// recall button. The player has no further control over this survivor
        /// until they reach the hatch.
        /// </summary>
        public bool isCommsSevered;

        /// <summary>
        /// Trait-driven resolution the survivor committed to when the EMP hit.
        /// Drives loot behavior, return speed, and acquired statuses.
        /// </summary>
        public FlashpointBehavior flashpointBehavior = FlashpointBehavior.None;

        /// <summary>
        /// ETA at the moment the intercept fired. Persisted so the Cautious
        /// "12-24 hour delay" and the "ETA halved" visual can be restored on
        /// load without recomputing from current state.
        /// </summary>
        public float originalEtaTicks;

        /// <summary>
        /// For <see cref="FlashpointBehavior.CautiousShelter"/>: ticks the
        /// expedition sits in a ruin before resuming the return. Counts down
        /// each inbound tick; while > 0 the survivor doesn't advance.
        /// </summary>
        public int shelterDelayTicksRemaining;

        /// <summary>
        /// For <see cref="FlashpointBehavior.ParanoidSprint"/>: speed
        /// multiplier applied to the return step (e.g. 2.0 = sprint).
        /// </summary>
        public float returnSpeedMultiplier = 1f;

        /// <summary>
        /// For <see cref="FlashpointBehavior.FatalistNumbWalk"/>: speed
        /// multiplier applied to the return step (e.g. 0.5 = slow walk).
        /// </summary>
        public float returnSpeedDivisor = 1f;

        public List<string> CollectedLootItemIds = new List<string>();

        [NonSerialized]
        public List<ItemDefinition> CollectedLoot = new List<ItemDefinition>();

        [NonSerialized]
        public Survivor Survivor;

        /// <summary>Recalculate total loot weight from collected items.</summary>
        public void RecalculateWeight()
        {
            float weight = 0f;
            if (CollectedLoot != null)
            {
                for (int i = 0; i < CollectedLoot.Count; i++)
                {
                    if (CollectedLoot[i] != null)
                    {
                        weight += CollectedLoot[i].weight;
                    }
                }
            }
            CurrentWeight = weight;
        }

        /// <summary>Add item to loot if within carrying capacity (returns false if full).</summary>
        public bool TryAddLoot(ItemDefinition item)
        {
            if (item == null) return false;
            if (CurrentWeight + item.weight > CarryingCapacity && CollectedLoot.Count > 0)
            {
                return false; // Exceeds capacity
            }

            CollectedLoot.Add(item);
            if (!string.IsNullOrEmpty(item.id))
            {
                CollectedLootItemIds.Add(item.id);
            }
            RecalculateWeight();
            return true;
        }

        /// <summary>Drop proportion or all of collected loot (e.g. Paranoid flee response).</summary>
        public List<ItemDefinition> DropLoot(float fraction = 1.0f)
        {
            var dropped = new List<ItemDefinition>();
            if (CollectedLoot == null || CollectedLoot.Count == 0) return dropped;

            int countToDrop = Mathf.Clamp(Mathf.CeilToInt(CollectedLoot.Count * fraction), 1, CollectedLoot.Count);
            for (int i = 0; i < countToDrop; i++)
            {
                int lastIdx = CollectedLoot.Count - 1;
                dropped.Add(CollectedLoot[lastIdx]);
                CollectedLoot.RemoveAt(lastIdx);
                if (CollectedLootItemIds.Count > lastIdx)
                {
                    CollectedLootItemIds.RemoveAt(lastIdx);
                }
            }

            RecalculateWeight();
            return dropped;
        }
    }
}
