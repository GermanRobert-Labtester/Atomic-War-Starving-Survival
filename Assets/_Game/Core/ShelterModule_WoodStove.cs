using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class WoodStoveState
    {
        public string moduleId = "shelter_module_wood_stove";
        public string displayName = "The Cast Iron Stove";
        public bool isBuilt = false;
        public bool isAdjacentToAirVent = false;
        public bool isLit = false;
        public float heatGenerated = 40f;
        public string carbonMonoxideAffliction = "carbon_monoxide_poisoning";
    }

    /// <summary>
    /// Prompt #441: Module: The Cast Iron Stove.
    /// Burns Wood or Books for massive Heat and Cooking capabilities.
    /// Must be placed adjacent to an AirVent to exhaust smoke, or it fills the room with lethal Carbon Monoxide.
    /// </summary>
    public class ShelterModule_WoodStove
    {
        private WoodStoveState _state = new WoodStoveState();

        public event Action<WoodStoveState, float> OnStoveLitHeatGenerated;
        public event Action<WoodStoveState, string> OnCarbonMonoxidePoisoningTriggered;

        public WoodStoveState State => _state;

        public bool LightStove(int woodOrBookCount, out string coPoisoning)
        {
            coPoisoning = null;
            if (!_state.isBuilt || woodOrBookCount <= 0) return false;

            _state.isLit = true;
            OnStoveLitHeatGenerated?.Invoke(_state, _state.heatGenerated);

            if (!_state.isAdjacentToAirVent)
            {
                coPoisoning = _state.carbonMonoxideAffliction;
                OnCarbonMonoxidePoisoningTriggered?.Invoke(_state, coPoisoning);
            }

            return true;
        }
    }
}
