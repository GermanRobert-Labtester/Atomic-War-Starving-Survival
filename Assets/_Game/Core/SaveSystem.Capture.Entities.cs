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

        private void CaptureMedicalAndBodySystems(SaveData data)
        {
            // medical / blood_transfusion / amputation / scurvy / mutagenesis —
            // dual-path CapIf removed; RegisterSystem + SubsystemSaveIds own capture.
            // RestIf in RestoreMedicalAndBodySystems still reads positional DTOs for
            // pre-migration saves.
            if (_chelationSystem != null) data.Chelation = _chelationSystem.CaptureState();
            if (_antibioticResistSystem != null) data.AntibioticResist = _antibioticResistSystem.CaptureState();
            if (_triageSystem != null) data.Triage = _triageSystem.CaptureState();
            if (_polypharmacySystem != null) data.Polypharmacy = _polypharmacySystem.CaptureState();
            if (_sterilizationSystem != null) data.Sterilization = _sterilizationSystem.CaptureState();
            if (_childSystem != null) data.ChildDependent = _childSystem.CaptureState();
            if (_corpseSystem != null) data.Corpses = _corpseSystem.CaptureState();
        }

        private void CaptureMapWaterAffinity(SaveData data)
        {
            if (_generatedMap != null)
                data.GeneratedMap = _generatedMap.CaptureState();
            if (_waterStorage != null)
                data.Water = _waterStorage.CaptureState();

            if (_mentalBreakSystem != null)
            {
                var affSave = new AffinityMatrixSave();
                affSave.Entries.AddRange(_mentalBreakSystem.Affinity.Snapshot());
                data.Affinity = affSave;
            }

            if (_captureChoreographer != null)
                data.FlashpointChoreographer = _captureChoreographer();
        }

        private void CaptureExpeditions(SaveData data)
        {
            if (_expeditionSystem == null || _expeditionSystem.ActiveExpeditions == null)
                return;

            foreach (var exp in _expeditionSystem.ActiveExpeditions)
            {
                if (exp == null) continue;
                data.Expeditions.Add(CaptureExpeditionState(exp));
            }
        }

        private static ExpeditionSaveState CaptureExpeditionState(ExpeditionState exp)
        {
            var saveExp = new ExpeditionSaveState
            {
                ExpeditionId = exp.ExpeditionId,
                SurvivorId = exp.SurvivorId,
                TargetLocationId = exp.TargetLocationId,
                TargetLocationName = exp.TargetLocationName,
                Stance = exp.Stance,
                Phase = exp.Phase,
                CurrentTick = exp.CurrentTick,
                TotalDistanceTicks = exp.TotalDistanceTicks,
                TravelTicksCompleted = exp.TravelTicksCompleted,
                LootingTicksCompleted = exp.LootingTicksCompleted,
                CarryingCapacity = exp.CarryingCapacity,
                CurrentWeight = exp.CurrentWeight,
                Stamina = exp.Stamina,
                SuitDegradation = exp.SuitDegradation,
                TrueRadPerHour = exp.TrueRadPerHour,
                DangerLevel = exp.DangerLevel,
                IsPushingLuck = exp.IsPushingLuck,
                IsRetreating = exp.IsRetreating,
                LocationEncounterFired = exp.LocationEncounterFired,
                UxoDetonated = exp.UxoDetonated,
                HasBicycle = exp.HasBicycle,
                BicycleDurability = exp.BicycleDurability,
                IsWading = exp.IsWading,
                IsNightScavenge = exp.IsNightScavenge,
                HasFlashlight = exp.HasFlashlight,
                FlashlightBattery = exp.FlashlightBattery
            };
            if (exp.CollectedLootItemIds != null)
                saveExp.CollectedLootItemIds.AddRange(exp.CollectedLootItemIds);
            return saveExp;
        }

        private void CapturePhantomAndRooms(SaveData data)
        {
            if (_phantomIntruderSystem != null && _phantomIntruderSystem.Cooldowns != null)
            {
                foreach (var kv in _phantomIntruderSystem.Cooldowns)
                {
                    if (kv.Value <= 0f) continue;
                    data.PhantomCooldownKeys.Add(kv.Key);
                    data.PhantomCooldownValues.Add(kv.Value);
                }
            }

            if (_shelter == null || _shelter.Rooms == null) return;

            foreach (var room in _shelter.Rooms)
            {
                if (room == null || string.IsNullOrEmpty(room.RoomId)) continue;
                data.ShelterRooms.Add(new ShelterRoomSave
                {
                    RoomId = room.RoomId,
                    UnlockState = (int)room.UnlockState,
                    RubbleClearHoursRemaining = room.RubbleClearHoursRemaining,
                    RubbleClearHoursTotal = room.RubbleClearHoursTotal,
                    DiaryFragmentIds = room.DiaryFragmentIds != null
                        ? new List<string>(room.DiaryFragmentIds)
                        : new List<string>(),
                    RevealedDiaryIndices = room.RevealedDiaryIndices != null
                        ? new List<int>(room.RevealedDiaryIndices)
                        : new List<int>()
                });
            }
        }

    }
}
