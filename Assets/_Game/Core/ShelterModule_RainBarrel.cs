using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class RainBarrelState
    {
        public string moduleId = "shelter_module_rain_barrel";
        public string displayName = "Rain Catchment Barrel";
        public bool isBuilt = false;
        public bool isOutside = true;
        public int waterCapacity = 20;
        public int currentWater = 0;
        public bool isDestroyedByFreeze = false;
    }

    /// <summary>
    /// Prompt #442: Module: Rain Catchment Barrel.
    /// Lowest tier outdoor water collector (holds 20 units).
    /// If temperature drops below 0°C, the water freezes and bursts the barrel, destroying the module permanently.
    /// </summary>
    public class ShelterModule_RainBarrel
    {
        private RainBarrelState _state = new RainBarrelState();

        public event Action<RainBarrelState> OnBarrelBurstFromFreeze;
        public event Action<RainBarrelState, int> OnWaterCollected;

        public RainBarrelState State => _state;

        public void TickTemperature(float ambientTemp)
        {
            if (!_state.isBuilt || _state.isDestroyedByFreeze) return;

            if (ambientTemp < 0f && _state.currentWater > 0)
            {
                _state.isDestroyedByFreeze = true;
                _state.currentWater = 0;
                OnBarrelBurstFromFreeze?.Invoke(_state);
            }
        }

        public void CollectRain(int amount)
        {
            if (!_state.isBuilt || _state.isDestroyedByFreeze) return;
            _state.currentWater = Mathf.Min(_state.waterCapacity, _state.currentWater + amount);
            OnWaterCollected?.Invoke(_state, _state.currentWater);
        }
    }
}
