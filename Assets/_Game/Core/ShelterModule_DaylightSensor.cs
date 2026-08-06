using System;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class DaylightSensorState
    {
        public string moduleId = "shelter_module_daylight_sensor";
        public bool isActive = false;
        public float powerDrawGrowLightsWatts = 120f;
        public float powerDrawLightbulbsWatts = 60f;
    }

    /// <summary>
    /// Prompt #802: Daylight Sensors.
    /// Connected to SurfaceDome. Auto-disables GrowLights/Lightbulbs during daytime.
    /// Optimizes PowerNetwork by cutting unnecessary internal lighting.
    /// </summary>
    public class ShelterModule_DaylightSensor
    {
        public event Action OnLightsDisabled;
        public event Action OnLightsEnabled;
        public event Action<float> OnPowerSaved;   // wattsSaved

        private DaylightSensorState _state;
        private bool _lightsCurrentlyOn = true;

        public ShelterModule_DaylightSensor(DaylightSensorState state = null)
        {
            _state = state ?? new DaylightSensorState();
        }

        public string ModuleId => _state.moduleId;
        public bool IsActive() => _state.isActive;

        public void Activate()
        {
            _state.isActive = true;
        }

        public void Deactivate()
        {
            _state.isActive = false;
        }

        /// <summary>
        /// Called once per in-game hour. Toggles internal lights based on daylight.
        /// </summary>
        public void TickHour(bool isDaytime)
        {
            if (!_state.isActive)
                return;

            if (isDaytime && _lightsCurrentlyOn)
            {
                _lightsCurrentlyOn = false;
                OnLightsDisabled?.Invoke();
                float saved = GetPowerSavings(true);
                if (saved > 0f)
                    OnPowerSaved?.Invoke(saved);
            }
            else if (!isDaytime && !_lightsCurrentlyOn)
            {
                _lightsCurrentlyOn = true;
                OnLightsEnabled?.Invoke();
            }
        }

        /// <summary>
        /// Returns watts saved when lights are auto-disabled during daytime.
        /// </summary>
        public float GetPowerSavings(bool isDaytime)
        {
            if (!isDaytime)
                return 0f;

            return _state.powerDrawGrowLightsWatts + _state.powerDrawLightbulbsWatts;
        }

        public bool AreLightsOn() => _lightsCurrentlyOn;

        public DaylightSensorState CaptureState()
        {
            return new DaylightSensorState
            {
                moduleId = _state.moduleId,
                isActive = _state.isActive,
                powerDrawGrowLightsWatts = _state.powerDrawGrowLightsWatts,
                powerDrawLightbulbsWatts = _state.powerDrawLightbulbsWatts
            };
        }

        public void RestoreState(DaylightSensorState state)
        {
            _state = state ?? new DaylightSensorState();
        }
    }
}
