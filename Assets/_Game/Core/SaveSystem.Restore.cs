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
        // -----------------------------------------------------------------
        // Restore from snapshot (split helpers keep cyclomatic complexity low)
        // -----------------------------------------------------------------

        private void RestoreFromSnapshot(SaveData data)
        {
            // Fallback rng streams are process-static, so without this the same
            // slot loaded twice in one session resumes mid-stream and rolls
            // differently the second time -- the exact drift the seeded-rng work
            // (MISC-005) set out to remove.
            AtomicWar._Game.Utilities.SeededRandom.ResetStreams();

            RestoreGameStateCore(data);
            RestoreSurvivorsAndShelter(data);
            RestoreMedicalAndBodySystems(data);
            RestoreWorldAndFactionSystems(data);
            RestoreShelterTacticalSystems(data);
            RestoreSimulationExtras(data);
            RestoreSubsystemStates(data);
            RestoreMapWaterAffinity(data);
            RestorePhantomAndRooms(data);
            RestoreExpeditions(data.Expeditions);
            ApplyPostExchangeFlags();
        }

        private void RestoreGameStateCore(SaveData data)
        {
            _gameState.Day = data.GameState.Day;
            _gameState.Phase = data.GameState.Phase;
            _gameState.IsPaused = data.GameState.IsPaused;

            if (_weatherSystem != null && data.Weather != null)
                _weatherSystem.RestoreState(data.Weather);

            // Keep Temperature + TimeSystem aligned from the same ElapsedHours field.
            if (_temperatureSystem != null)
                _temperatureSystem.SetElapsedHours(data.ElapsedHours);
            if (_timeSystem != null && data.ElapsedHours > 0f)
                _timeSystem.SetElapsedHours(data.ElapsedHours);

            _worldFlags.Clear();
            if (data.WorldFlagKeys != null && data.WorldFlagValues != null)
            {
                int count = Mathf.Min(data.WorldFlagKeys.Count, data.WorldFlagValues.Count);
                for (int i = 0; i < count; i++)
                    _worldFlags[data.WorldFlagKeys[i]] = data.WorldFlagValues[i];
            }

            if (_photoPeriodSystem != null && data.Photoperiod != null)
                _photoPeriodSystem.RestoreState(data.Photoperiod);
            if (_knowledgeMap != null && data.RadiationKnowledge != null)
                _knowledgeMap.RestoreState(data.RadiationKnowledge);
            if (_inventory != null && data.Inventory != null && _itemLookup != null)
                _inventory.RestoreState(data.Inventory, _itemLookup);
            // water_storage dual-path removed; RestIf first so SubsystemSaveIds can
            // overwrite. Must not run after RestoreSubsystemStates (empty positional
            // Water would wipe subsystem-restored cisterns).
            if (_waterStorage != null && data.Water != null)
                _waterStorage.RestoreState(data.Water);
        }

        private void RestoreSurvivorsAndShelter(SaveData data)
        {
            var existing = _getSurvivors?.Invoke();
            if (existing != null && data.Survivors != null)
            {
                // SAVE-007: match by survivor Id, never by roster index. Index restore
                // mis-maps after recruit/death/reorder and drops extras silently.
                for (int i = 0; i < data.Survivors.Count; i++)
                {
                    var saveSv = data.Survivors[i];
                    if (saveSv == null) continue;
                    Survivor sv = FindSurvivorById(existing, saveSv.Id);
                    if (sv == null && i < existing.Count && string.IsNullOrEmpty(saveSv.Id))
                        sv = existing[i]; // legacy saves without ids: fall back to index
                    if (sv == null) continue;
                    RestoreSurvivor(sv, saveSv);
                }
            }

            if (_shelter != null && data.ShelterModules != null)
                RestoreShelterModules(data.ShelterModules);

            if (_radiationSystem != null && data.Survivors != null)
                RestoreDosimeters(data.Survivors);

            if (_shelter != null)
                _shelter.SetBunkerContamination(data.BunkerContamination);
        }

    }
}
