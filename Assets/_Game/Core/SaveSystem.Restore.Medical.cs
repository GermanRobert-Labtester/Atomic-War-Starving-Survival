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

        private void RestoreMedicalAndBodySystems(SaveData data)
        {
            // Positional RestIf kept for pre-migration saves. New saves leave these
            // null and restore via SubsystemSaveIds (RegisterSystem adapters).
            if (_medicalSystem != null && data.Medical != null)
                _medicalSystem.RestoreState(data.Medical);
            if (_bloodTransfusion != null && data.BloodTransfusion != null)
                _bloodTransfusion.RestoreState(data.BloodTransfusion);
            if (_amputationSystem != null && data.Amputation != null)
                _amputationSystem.RestoreState(data.Amputation);
            if (_scurvySystem != null && data.Scurvy != null)
                _scurvySystem.RestoreState(data.Scurvy);
            if (_mutagenesisSystem != null && data.Mutagenesis != null)
                _mutagenesisSystem.RestoreState(data.Mutagenesis);
            // Positional only when present (pre-migration). New saves restore via SubsystemSaveIds.
            if (_chelationSystem != null && data.Chelation != null)
                _chelationSystem.RestoreState(data.Chelation);
            if (_antibioticResistSystem != null && data.AntibioticResist != null)
                _antibioticResistSystem.RestoreState(data.AntibioticResist);
            if (_triageSystem != null && data.Triage != null)
                _triageSystem.RestoreState(data.Triage);
            if (_polypharmacySystem != null && data.Polypharmacy != null)
                _polypharmacySystem.RestoreState(data.Polypharmacy);
            if (_sterilizationSystem != null && data.Sterilization != null)
                _sterilizationSystem.RestoreState(data.Sterilization);
            // ChildDependent is a struct on SaveData — only apply when legacy payload is non-empty.
            if (_childSystem != null
                && (data.ChildDependent.wasChildFound || !string.IsNullOrEmpty(data.ChildDependent.childId)))
            {
                var survivors = _getSurvivors?.Invoke();
                _childSystem.RestoreState(data.ChildDependent, survivors);
            }
            if (_corpseSystem != null && data.Corpses != null)
                _corpseSystem.RestoreState(data.Corpses);
        }

        private void RestoreMapWaterAffinity(SaveData data)
        {
            // water_storage RestIf moved to RestoreGameStateCore (before
            // RestoreSubsystemStates). This method keeps field-only specials:
            // affinity matrix + flashpoint choreographer.
            if (_mentalBreakSystem != null && data.Affinity != null)
                _mentalBreakSystem.Affinity.Restore(data.Affinity.Entries);

            if (_restoreChoreographer != null)
                _restoreChoreographer(data.FlashpointChoreographer);
        }

        private void RestorePhantomAndRooms(SaveData data)
        {
            if (_phantomIntruderSystem != null && data.PhantomCooldownKeys != null)
            {
                _phantomIntruderSystem.Cooldowns.Clear();
                for (int i = 0; i < data.PhantomCooldownKeys.Count && i < data.PhantomCooldownValues.Count; i++)
                    _phantomIntruderSystem.Cooldowns[data.PhantomCooldownKeys[i]] = data.PhantomCooldownValues[i];
            }

            if (_shelter == null || data.ShelterRooms == null) return;

            for (int i = 0; i < data.ShelterRooms.Count; i++)
            {
                var roomSave = data.ShelterRooms[i];
                if (roomSave == null || string.IsNullOrEmpty(roomSave.RoomId)) continue;
                var room = _shelter.GetRoom(roomSave.RoomId);
                if (room == null) continue;
                room.UnlockState = (RoomUnlockState)roomSave.UnlockState;
                room.RubbleClearHoursRemaining = roomSave.RubbleClearHoursRemaining;
                room.RubbleClearHoursTotal = roomSave.RubbleClearHoursTotal;
                if (roomSave.DiaryFragmentIds != null)
                    room.DiaryFragmentIds = new List<string>(roomSave.DiaryFragmentIds);
                if (roomSave.RevealedDiaryIndices != null)
                    room.RevealedDiaryIndices = new List<int>(roomSave.RevealedDiaryIndices);
            }
        }

        private void ApplyPostExchangeFlags()
        {
            if (_worldPhaseSystem == null || !_worldPhaseSystem.HasTriggeredExchange)
                return;
            if (_radiationSystem != null) _radiationSystem.IsPaused = false;
            if (_weatherSystem != null) _weatherSystem.RestrictToNonHazardWeather = false;
        }

    }
}
