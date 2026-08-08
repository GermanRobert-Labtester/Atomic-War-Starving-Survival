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
        // Capture snapshot (split helpers keep cyclomatic complexity low)
        // -----------------------------------------------------------------

        private SaveData CaptureSnapshot()
        {
            var data = new SaveData
            {
                SaveVersion = CurrentSaveVersion,
                GameState = new GameStateSave
                {
                    Phase = _gameState.Phase,
                    Day = _gameState.Day,
                    IsPaused = _gameState.IsPaused
                }
            };

            CaptureCoreSystems(data);
            CaptureMedicalAndBodySystems(data);
            CaptureWorldAndFactionSystems(data);
            // Shelter tactical + simulation extras: dual-path CapIf fully removed
            // (batches 1–4); RegisterSystem + CaptureSubsystemStates own capture.
            CaptureSubsystemStates(data);
            CaptureMapWaterAffinity(data);
            CaptureExpeditions(data);
            CapturePhantomAndRooms(data);
            return data;
        }

        private void CaptureCoreSystems(SaveData data)
        {
            // Host core state (not dual-path RegisterSystem adapters).
            if (_weatherSystem != null)
                data.Weather = _weatherSystem.GetState();

            if (_temperatureSystem != null)
                data.ElapsedHours = _temperatureSystem.TotalElapsedHours;

            if (_getSurvivors != null)
            {
                var survivors = _getSurvivors();
                if (survivors != null)
                {
                    foreach (var sv in survivors)
                        data.Survivors.Add(CaptureSurvivor(sv));
                }
            }

            if (_shelter != null)
            {
                foreach (var mod in _shelter.Modules)
                {
                    data.ShelterModules.Add(new ShelterModuleSave
                    {
                        ModuleId = mod.ModuleId,
                        Level = mod.Level,
                        IsEnabled = mod.IsEnabled,
                        FilterHealth = mod.FilterHealth,
                        Fuel = mod.Fuel,
                        FuelBurnMultiplier = mod.FuelBurnMultiplier,
                        WaterConversionProgress = mod.WaterConversionProgress,
                        RoomId = mod.RoomId,
                        Occupancy = mod.Occupancy,
                        ComfortLevel = mod.ComfortLevel,
                        Capacity = mod.Capacity,
                        SecurityContribution = mod.SecurityContribution
                    });
                }
                data.BunkerContamination = _shelter.BunkerContamination;
            }

            if (_worldFlags.Count > 0)
            {
                foreach (var kv in _worldFlags)
                {
                    data.WorldFlagKeys.Add(kv.Key);
                    data.WorldFlagValues.Add(kv.Value);
                }
            }

            // photoperiod / radiation_knowledge / inventory — dual-path positional
            // capture removed; RegisterSystem + SubsystemSaveIds own capture.
            // RestoreGameStateCore still reads positional DTOs for pre-migration saves.
        }

    }
}
