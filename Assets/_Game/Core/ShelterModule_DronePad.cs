using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class DronePadState
    {
        public string moduleId = "shelter_module_drone_pad";
        public string displayName = "Drone Recharging Station";
        public bool isBuilt = false;
        public float powerDrawWatts = 200f; // Massive power drain
        public bool isDroneDeployed = false;
        public bool isDroneDestroyed = false;
        public int nodesMappedPerDay = 5;
    }

    /// <summary>
    /// Prompt #404: Module: Drone Recharging Station.
    /// Drains massive Power to dock a MilitaryDrone. Maps 5 nodes per day automatically.
    /// If a FalloutStorm hits while the drone is out mapping, the drone is permanently destroyed.
    /// </summary>
    public class ShelterModule_DronePad
    {
        private DronePadState _state = new DronePadState();

        public event Action<DronePadState, int> OnAutomatedMappingCompleted;
        public event Action<DronePadState> OnDroneDestroyedInStorm;

        public DronePadState State => _state;

        public int DeployDroneMapping(bool isFalloutStormActive)
        {
            if (!_state.isBuilt || _state.isDroneDestroyed) return 0;

            _state.isDroneDeployed = true;
            if (isFalloutStormActive)
            {
                _state.isDroneDestroyed = true;
                _state.isDroneDeployed = false;
                OnDroneDestroyedInStorm?.Invoke(_state);
                return 0;
            }

            int mapped = _state.nodesMappedPerDay;
            _state.isDroneDeployed = false;
            OnAutomatedMappingCompleted?.Invoke(_state, mapped);
            return mapped;
        }
    
        public DronePadState CaptureState()
        {
            return _state;
        }

        public void RestoreState(DronePadState saved)
        {
            _state = saved ?? new DronePadState();
        }
    }
}

