using System;
using UnityEngine;

namespace AtomicWar._Game.Environment
{
    /// <summary>
    /// weather_ice_storm — Ice Storm (Section X).
    /// Freezing rain coats every surface in ice. The hatch mechanism
    /// freezes shut. Surface access impossible. Solar panels blocked.
    /// The power grid drops. The heater works harder. Fuel
    /// consumption increases 50 %.
    /// </summary>
    [Serializable]
    public class IceStormState
    {
        public string weatherId = "weather_ice_storm";
        public string displayName = "Ice Storm";
        public bool isActive = false;
        public bool hatchFrozenShut = false;
        public float solarPowerMultiplier = 0f;          // 0 = solar blocked
        public float fuelConsumptionMultiplier = 1.5f;   // +50%
        public int durationHours = 18;
    }

    /// <summary>DEMOTE-Weather-batch — dormant ghost. Re-promote with Boot+Save+host.</summary>
    public class Weather_IceStorm
    {
        private IceStormState _state = new IceStormState();

        public event Action<IceStormState> OnHatchFrozen;
        public event Action<IceStormState, float> OnFuelBurnIncreased;

        public IceStormState State => _state;

        public void Tick(float deltaHours, float baseFuelPerHour)
        {
            if (!_state.isActive) return;
            if (!_state.hatchFrozenShut)
            {
                _state.hatchFrozenShut = true;
                OnHatchFrozen?.Invoke(_state);
            }
            float extra = baseFuelPerHour * (_state.fuelConsumptionMultiplier - 1f) * deltaHours;
            if (extra > 0f) OnFuelBurnIncreased?.Invoke(_state, extra);
        }

        public void SetActive(bool active)
        {
            _state.isActive = active;
            if (!active) _state.hatchFrozenShut = false;
        }

        /// <summary>Convenience: fire the event for its full configured duration (matches Weather_BloodRain.Trigger() convention).</summary>
        public void Trigger() => SetActive(true);

        public bool BlocksSurfaceAccess => _state.isActive;
        public bool BlocksSolar => _state.isActive;

        public IceStormState CaptureState() => _state;
        public void RestoreState(IceStormState s) { _state = s ?? new IceStormState(); }
    }
}
