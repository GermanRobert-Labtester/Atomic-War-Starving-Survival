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

        private int ForceMentalBreakBingeEat(Survivor sv, MentalBreakSO br)
        {
            if (sv == null || br == null || Inventory == null || Inventory.Slots == null) return 0;
            if (!sv.IsAlive) return 0;

            InventorySlot best = null;
            float bestValue = float.NegativeInfinity;
            int scanned = 0;
            for (int i = 0; i < Inventory.Slots.Count && scanned < MentalBreakSystem.BingeEaterMaxSlotsScanned; i++)
            {
                var slot = Inventory.Slots[i];
                if (slot == null || slot.Item == null || slot.Amount <= 0) continue;
                if (slot.Item.type != ItemType.Food) continue;
                if (slot.Item.hungerRestore < br.minFoodValueForBinge) continue;
                if (slot.Item.hungerRestore > bestValue)
                {
                    best = slot;
                    bestValue = slot.Item.hungerRestore;
                }
                scanned++;
            }
            if (best == null) return 0;

            int wanted = Mathf.Max(1, Mathf.CeilToInt(br.consumptionMultiplier));
            int consumed = Mathf.Min(wanted, best.Amount);
            if (consumed <= 0) return 0;
            Inventory.Remove(best.Item, consumed);
            float restore = best.Item.hungerRestore * consumed;
            sv.Needs.Hunger = Mathf.Max(0f, sv.Needs.Hunger - restore);
            return consumed;
        }

        private bool ForceMentalBreakComfortCure(Survivor sv, MentalBreakSO br)
        {
            if (sv == null || br == null || Inventory == null || Inventory.Slots == null) return false;

            // Find a Comfort item (e.g. old_book, music_disc). Prefer the
            // one with the highest moraleRestore / sellValue as a stand-in
            // for "high-value".
            InventorySlot best = null;
            float bestValue = float.NegativeInfinity;
            for (int i = 0; i < Inventory.Slots.Count; i++)
            {
                var slot = Inventory.Slots[i];
                if (slot == null || slot.Item == null || slot.Amount <= 0) continue;
                if (slot.Item.type != ItemType.Comfort) continue;
                // Use tradeValue + moraleEffect as a high-value proxy.
                float value = slot.Item.tradeValue + slot.Item.moraleEffect;
                if (value > bestValue)
                {
                    best = slot;
                    bestValue = value;
                }
            }
            if (best == null || best.Item == null) return false;

            // Consume one unit of the comfort item. The system-side
            // TryCureWithComfortItem will then advance mentalBreakCureProgress
            // by br.comfortItemCureAmount and call Cure() if the threshold
            // is met.
            return Inventory.Remove(best.Item, 1);
        }

        private T CreateAction<T>() where T : SurvivorAction
        {
            var action = ScriptableObject.CreateInstance<T>();
            return action;
        }

        private bool TryApplyPedalCost(string id, float fatigueDelta, float hungerDelta)
        {
            if (Survivors == null || string.IsNullOrEmpty(id)) return false;
            Survivor pedaler = null;
            for (int i = 0; i < Survivors.Count; i++)
            {
                if (Survivors[i] != null && Survivors[i].Id == id)
                {
                    pedaler = Survivors[i];
                    break;
                }
            }
            if (pedaler == null || !pedaler.IsAlive || pedaler.Needs == null)
                return false;
            if (pedaler.Needs.Fatigue >= 95f)
                return false;
            pedaler.Needs.Fatigue = Mathf.Clamp(
                pedaler.Needs.Fatigue + fatigueDelta, 0f, 100f);
            pedaler.Needs.Hunger = Mathf.Clamp(
                pedaler.Needs.Hunger + hungerDelta, 0f, 100f);
            return true;
        }

        private bool IsWiretapAntennaOperational()
        {
            var state = RadioTunerSystem?.State;
            return state != null && state.IsOperational;
        }

    }
}
