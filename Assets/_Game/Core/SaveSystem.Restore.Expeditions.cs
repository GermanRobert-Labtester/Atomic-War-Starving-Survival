using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Simulation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Events;

namespace AtomicWar._Game.Core
{
    public partial class SaveSystem
    {

        private void RestoreExpeditions(List<ExpeditionSaveState> expeditions)
        {
            if (_expeditionSystem == null || expeditions == null) return;

            var existingExpeditions = _expeditionSystem.ActiveExpeditions as List<ExpeditionState>;
            var survivors = _getSurvivors?.Invoke();

            foreach (var saveExp in expeditions)
            {
                if (saveExp == null || string.IsNullOrEmpty(saveExp.SurvivorId)) continue;
                var survivor = FindSurvivorById(survivors, saveExp.SurvivorId);
                var state = GetOrCreateExpeditionState(saveExp.SurvivorId, existingExpeditions);
                ApplyExpeditionSaveFields(state, saveExp, survivor);
                RebuildExpeditionLoot(state);
            }
        }

        private static Survivor FindSurvivorById(IReadOnlyList<Survivor> survivors, string id)
        {
            if (survivors == null) return null;
            for (int i = 0; i < survivors.Count; i++)
            {
                if (survivors[i]?.Id == id)
                    return survivors[i];
            }
            return null;
        }

        private ExpeditionState GetOrCreateExpeditionState(
            string survivorId,
            List<ExpeditionState> existingExpeditions)
        {
            var state = _expeditionSystem.GetExpeditionBySurvivor(survivorId);
            if (state != null) return state;

            state = new ExpeditionState();
            existingExpeditions?.Add(state);
            return state;
        }

        private static void ApplyExpeditionSaveFields(
            ExpeditionState state,
            ExpeditionSaveState saveExp,
            Survivor survivor)
        {
            state.ExpeditionId = saveExp.ExpeditionId;
            state.SurvivorId = saveExp.SurvivorId;
            state.TargetLocationId = saveExp.TargetLocationId;
            state.TargetLocationName = saveExp.TargetLocationName;
            state.Stance = saveExp.Stance;
            state.Phase = saveExp.Phase;
            state.CurrentTick = saveExp.CurrentTick;
            state.TotalDistanceTicks = saveExp.TotalDistanceTicks;
            state.TravelTicksCompleted = saveExp.TravelTicksCompleted;
            state.LootingTicksCompleted = saveExp.LootingTicksCompleted;
            state.CarryingCapacity = saveExp.CarryingCapacity;
            state.CurrentWeight = saveExp.CurrentWeight;
            state.Stamina = saveExp.Stamina;
            state.SuitDegradation = saveExp.SuitDegradation;
            state.TrueRadPerHour = saveExp.TrueRadPerHour;
            state.DangerLevel = saveExp.DangerLevel;
            state.IsPushingLuck = saveExp.IsPushingLuck;
            state.IsRetreating = saveExp.IsRetreating;
            state.isCommsSevered = saveExp.IsCommsSevered;
            state.flashpointBehavior = saveExp.FlashpointBehavior;
            state.originalEtaTicks = saveExp.OriginalEtaTicks;
            state.shelterDelayTicksRemaining = saveExp.ShelterDelayTicksRemaining;
            state.returnSpeedMultiplier = saveExp.ReturnSpeedMultiplier;
            state.returnSpeedDivisor = saveExp.ReturnSpeedDivisor;
            state.LocationEncounterFired = saveExp.LocationEncounterFired;
            state.UxoDetonated = saveExp.UxoDetonated;
            state.HasBicycle = saveExp.HasBicycle;
            state.BicycleDurability = saveExp.BicycleDurability;
            state.IsWading = saveExp.IsWading;
            state.IsNightScavenge = saveExp.IsNightScavenge;
            state.HasFlashlight = saveExp.HasFlashlight;
            state.FlashlightBattery = saveExp.FlashlightBattery;
            state.Survivor = survivor;

            state.CollectedLootItemIds.Clear();
            if (saveExp.CollectedLootItemIds != null)
                state.CollectedLootItemIds.AddRange(saveExp.CollectedLootItemIds);
        }

        private void RebuildExpeditionLoot(ExpeditionState state)
        {
            state.CollectedLoot.Clear();
            if (_itemLookup == null || state.CollectedLootItemIds == null) return;

            for (int i = 0; i < state.CollectedLootItemIds.Count; i++)
            {
                var itemDef = _itemLookup(state.CollectedLootItemIds[i]);
                if (itemDef != null)
                    state.CollectedLoot.Add(itemDef);
            }
            state.RecalculateWeight();
        }

    }
}
