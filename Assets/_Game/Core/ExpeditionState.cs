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
        Outbound,   // Traveling to target location node
        Looting,    // At target location node (Push-Your-Luck phase)
        Inbound,    // Returning to shelter
        Completed,  // Returned safely with loot unloaded
        Failed      // Survivor died or collapsed
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
