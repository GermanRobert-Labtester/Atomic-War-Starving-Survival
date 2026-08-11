using System;
using UnityEngine;

namespace AtomicWar._Game.Environment
{
    /// <summary>
    /// weather_thermal_inversion — Thermal Inversion (Section X).
    /// Cold air trapped under a warm layer. Surface -5 C, bunker +12 C.
    /// Sounds carry for miles. Every hammer strike, every generator hum,
    /// travels. Noise discipline is critical. The inversion also traps
    /// fallout close to the ground. Surface radiation doubles.
    /// </summary>
    [Serializable]
    public class ThermalInversionState
    {
        public string weatherId = "weather_thermal_inversion";
        public string displayName = "Thermal Inversion";
        public bool isActive = false;
        public float surfaceTemperatureC = -5f;
        public float shelterTemperatureC = 12f;
        public float noisePropagationMultiplier = 3f;   // sounds travel 3x further
        public float surfaceRadiationMultiplier = 2f;
        public int durationHours = 24;
    }

    /// <summary>DEMOTE-Weather-batch — dormant ghost. Re-promote with Boot+Save+host.</summary>
    public class Weather_ThermalInversion
    {
        private ThermalInversionState _state = new ThermalInversionState();

        public event Action<ThermalInversionState> OnSoundCarriedFar;
        public event Action<ThermalInversionState, float> OnSurfaceRadiationDoubled;

        public ThermalInversionState State => _state;

        public float GetNoisePropagationMultiplier() => _state.isActive ? _state.noisePropagationMultiplier : 1f;
        public float GetSurfaceRadiationMultiplier() => _state.isActive ? _state.surfaceRadiationMultiplier : 1f;

        public void Tick(float deltaHours, float hoursOfHammering, float hoursOfGeneratorRun)
        {
            if (!_state.isActive) return;
            if (hoursOfHammering + hoursOfGeneratorRun > 0f)
            {
                OnSoundCarriedFar?.Invoke(_state);
            }
            OnSurfaceRadiationDoubled?.Invoke(_state, deltaHours);
        }

        public void SetActive(bool active) { _state.isActive = active; }

        /// <summary>Convenience: fire the event for its full configured duration (matches Weather_BloodRain.Trigger() convention).</summary>
        public void Trigger() => SetActive(true);

        public ThermalInversionState CaptureState() => _state;
        public void RestoreState(ThermalInversionState s) { _state = s ?? new ThermalInversionState(); }
    }
}
