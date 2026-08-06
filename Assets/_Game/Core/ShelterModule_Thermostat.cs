using System;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class ThermostatState
    {
        public string moduleId = "shelter_module_thermostat";
        public float lowThreshold = 10f;   // below this, heater turns on
        public float highThreshold = 20f;  // above this, heater turns off
    }

    /// <summary>
    /// Prompt #804: Smart Thermostats.
    /// Auto-toggles Heater: on if below lowThreshold, off if above highThreshold.
    /// Prevents fuel waste and Heatstroke.
    /// </summary>
    public class ShelterModule_Thermostat
    {
        public event Action<string, bool> OnHeaterToggled;   // roomId, isOn
        public event Action OnHeatstrokePrevented;
        public event Action OnFuelWastePrevented;

        private ThermostatState _state;

        // Track per-room heater state
        private System.Collections.Generic.Dictionary<string, bool> _roomHeaterOn =
            new System.Collections.Generic.Dictionary<string, bool>();

        public ShelterModule_Thermostat(ThermostatState state = null)
        {
            _state = state ?? new ThermostatState();
        }

        public string ModuleId => _state.moduleId;

        /// <summary>
        /// Called once per in-game hour. Auto-toggles heater based on room temperature.
        /// </summary>
        public void TickHour(string roomId, float currentTempCelsius)
        {
            if (string.IsNullOrEmpty(roomId))
            {
                Debug.LogWarning("[ShelterModule_Thermostat] TickHour called with null/empty roomId.");
                return;
            }

            bool currentlyOn;
            _roomHeaterOn.TryGetValue(roomId, out currentlyOn);

            bool shouldBeOn = ShouldHeaterBeOn(currentTempCelsius);

            if (shouldBeOn != currentlyOn)
            {
                _roomHeaterOn[roomId] = shouldBeOn;
                OnHeaterToggled?.Invoke(roomId, shouldBeOn);

                if (!shouldBeOn && currentTempCelsius >= _state.highThreshold)
                {
                    // Heater was running but temp is high — prevent heatstroke
                    OnHeatstrokePrevented?.Invoke();
                }
                else if (!shouldBeOn && currentTempCelsius > _state.lowThreshold)
                {
                    // Heater turned off because temp is adequate — save fuel
                    OnFuelWastePrevented?.Invoke();
                }
            }
        }

        /// <summary>
        /// Determine if the heater should be on given current temperature.
        /// On if below low threshold, off if above high threshold.
        /// In between, maintain current state (hysteresis).
        /// </summary>
        public bool ShouldHeaterBeOn(float tempCelsius)
        {
            if (tempCelsius < _state.lowThreshold)
                return true;

            if (tempCelsius > _state.highThreshold)
                return false;

            // Within hysteresis band — default to off to save fuel
            return false;
        }

        public bool IsHeaterOn(string roomId)
        {
            bool isOn;
            _roomHeaterOn.TryGetValue(roomId, out isOn);
            return isOn;
        }

        public ThermostatState CaptureState()
        {
            return new ThermostatState
            {
                moduleId = _state.moduleId,
                lowThreshold = _state.lowThreshold,
                highThreshold = _state.highThreshold
            };
        }

        public void RestoreState(ThermostatState state)
        {
            _state = state ?? new ThermostatState();
        }
    }
}
