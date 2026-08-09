using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Flashpoint;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Simulation;
using AtomicWar._Game.UI;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {

        /// <summary>
        /// Average RadiationDose across living survivors (0..100). Used by
        /// DynamicEconomySystem for trust-inversion factions (Cult of the Glow).
        /// </summary>

        /// <summary>
        /// True when any living survivor has Acute Radiation Syndrome (flag or status).
        /// Cult of the Glow ARS reverence (#16 polish).
        /// </summary>

        /// <summary>
        /// True when any living survivor wears an intact full hazmat suit.
        /// Cult of the Glow sealed-blood contempt (#16 polish).
        /// </summary>

        /// <summary>True when the survivor is currently on an outdoor expedition (Black Rain exposure).</summary>

        /// <summary>
        /// Black Rain hatch listeners: anyone in the entry room, or anyone
        /// underground while the hatch is sealed/open and rain is audible.
        /// Simplified: entry-room assignment OR hatch not Clear during BlackRain.
        /// </summary>

        private bool ForceAddictionPanicDestroy(Survivor sv, System.Random rng)
        {
            if (sv == null || Inventory?.Slots == null || rng == null) return false;
            if (Inventory.Slots.Count == 0) return false;

            // Destroy 1-3 random inventory items, each from a different slot
            int count = rng.Next(1, 4);
            bool destroyed = false;
            var targetedIndices = new System.Collections.Generic.HashSet<int>();
            for (int i = 0; i < count; i++)
            {
                if (!TryPickPanicDestroySlot(rng, targetedIndices, out int idx, out InventorySlot slot))
                    break;
                targetedIndices.Add(idx);
                int toRemove = rng.Next(1, Mathf.Min(slot.Amount, 3));
                if (!Inventory.Remove(slot.Item, toRemove)) continue;
                destroyed = true;
                GameLog.Log($"[Addiction] {sv.DisplayName} destroyed {toRemove}x {slot.Item.id} in a withdrawal panic.");
            }
            return destroyed;
        }

        private bool TryPickPanicDestroySlot(
            System.Random rng,
            System.Collections.Generic.HashSet<int> targetedIndices,
            out int idx,
            out InventorySlot slot)
        {
            idx = -1;
            slot = null;
            for (int attempts = 0; attempts < 20; attempts++)
            {
                idx = rng.Next(0, Inventory.Slots.Count);
                slot = Inventory.Slots[idx];
                bool usable = slot?.Item != null && slot.Amount > 0 && !targetedIndices.Contains(idx);
                if (usable) return true;
            }
            slot = null;
            return false;
        }

        private float ComputeAiRaidThreat(int day)
        {
            if (HatchDefenseSystem == null || day < HatchDefenseSystem.RaidUnlockDay)
                return 0f;

            float raidThreat = 0.25f;
            if (HatchDefenseSystem.GeneratorRunningOutside
                || HatchDefenseSystem.ExternalNoise >= HatchDefenseSystem.ExternalGeneratorNoiseThreshold)
                raidThreat = 0.7f;

            if (EconomySystem == null) return raidThreat;
            foreach (var fac in EconomySystem.Factions.Values)
            {
                if (fac == null) continue;
                if (EconomySystem.GetStance(fac.id) != TradeStance.HostileRaid) continue;
                raidThreat = Mathf.Max(raidThreat, 0.85f);
            }
            return raidThreat;
        }

    }
}
