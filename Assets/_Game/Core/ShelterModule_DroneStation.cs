using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class DroneStationState
    {
        public string moduleId = "shelter_module_drone_station";
        public float powerRequired = 50f;
        public int droneCount = 3;
        public bool isActive = false;
        // Track which rooms were last cleaned / modules repaired
        public List<string> cleanedRoomIds = new List<string>();
        public List<string> repairedModuleIds = new List<string>();
    }

    /// <summary>
    /// Maintenance Drone Station — deploys 3 ground drones that automatically
    /// clean waste from rooms and repair damaged shelter modules.
    /// Requires 50W continuous power to remain active.
    /// Prompt #790: ShelterModule_DroneStation
    /// </summary>
    public class ShelterModule_DroneStation
    {
        // -- Constants --
        public const float PowerRequired = 50f;
        public const int DefaultDroneCount = 3;

        // -- Events --
        public event Action OnDronesDeployed;
        public event Action<string> OnWasteCleaned;          // roomId
        public event Action<string, string> OnModuleRepaired; // roomId, moduleId

        // -- State --
        private bool _isActive = false;
        private readonly List<string> _cleanedRoomIds = new List<string>();
        private readonly List<string> _repairedModuleIds = new List<string>();

        // -- Public API --

        /// <summary>
        /// Attempts to deploy all 3 drones. Requires at least 50W available power.
        /// Returns true if deployment succeeded.
        /// </summary>
        public bool Deploy(float availablePower)
        {
            if (availablePower < PowerRequired)
            {
                Debug.LogWarning("[DroneStation] Insufficient power to deploy drones.");
                _isActive = false;
                return false;
            }
            _isActive = true;
            OnDronesDeployed?.Invoke();
            return true;
        }

        /// <summary>
        /// Hourly tick — each drone auto-cleans waste from one room and
        /// auto-repairs one damaged module. Call with current room/module lists.
        /// </summary>
        public void TickHour(List<string> dirtyRoomIds = null, List<string> damagedModuleIds = null, List<string> roomIdsForModules = null)
        {
            if (!_isActive) return;

            // Drones clean waste (up to drone count rooms per tick)
            if (dirtyRoomIds != null)
            {
                int cleanCount = Mathf.Min(DefaultDroneCount, dirtyRoomIds.Count);
                for (int i = 0; i < cleanCount; i++)
                {
                    string roomId = dirtyRoomIds[i];
                    if (!_cleanedRoomIds.Contains(roomId))
                        _cleanedRoomIds.Add(roomId);
                    OnWasteCleaned?.Invoke(roomId);
                }
            }

            // Drones repair modules (remaining drones handle repairs)
            if (damagedModuleIds != null && roomIdsForModules != null)
            {
                int repairCount = Mathf.Min(DefaultDroneCount, damagedModuleIds.Count);
                for (int i = 0; i < repairCount; i++)
                {
                    string moduleId = damagedModuleIds[i];
                    string roomId = (i < roomIdsForModules.Count) ? roomIdsForModules[i] : "";
                    if (!_repairedModuleIds.Contains(moduleId))
                        _repairedModuleIds.Add(moduleId);
                    OnModuleRepaired?.Invoke(roomId, moduleId);
                }
            }
        }

        /// <summary>Returns the number of drones in the station.</summary>
        public int GetDroneCount() => DefaultDroneCount;

        /// <summary>Returns true if drones are currently deployed and active.</summary>
        public bool IsActive() => _isActive;

        // -- Save / Load --

        public DroneStationState CaptureState()
        {
            return new DroneStationState
            {
                moduleId = "shelter_module_drone_station",
                powerRequired = PowerRequired,
                droneCount = DefaultDroneCount,
                isActive = _isActive,
                cleanedRoomIds = new List<string>(_cleanedRoomIds),
                repairedModuleIds = new List<string>(_repairedModuleIds)
            };
        }

        public void RestoreState(DroneStationState saved)
        {
            _cleanedRoomIds.Clear();
            _repairedModuleIds.Clear();
            if (saved == null) return;
            _isActive = saved.isActive;
            if (saved.cleanedRoomIds != null)
                _cleanedRoomIds.AddRange(saved.cleanedRoomIds);
            if (saved.repairedModuleIds != null)
                _repairedModuleIds.AddRange(saved.repairedModuleIds);
        }
    }
}
