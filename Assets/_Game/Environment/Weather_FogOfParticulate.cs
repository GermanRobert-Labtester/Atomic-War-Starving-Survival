using System;
using UnityEngine;

namespace AtomicWar._Game.Environment
{
    /// <summary>
    /// weather_fog_of_particulate — Particulate Fog (Section X).
    /// Visibility drops to 2 metres. Geiger counter clicks faster. The fog
    /// is radioactive particulate suspended in moisture. Breathing it
    /// without a mask adds 5 mSv/h. The filters work double-time.
    /// </summary>
    [Serializable]
    public class ParticulateFogState
    {
        public string weatherId = "weather_fog_of_particulate";
        public string displayName = "Particulate Fog";
        public bool isActive = false;
        public float visibilityMeters = 2f;
        public float unmaskedBreathingDoseMSvPerHour = 5f;
        public float airFilterDrainMultiplier = 2f;
        public int durationHours = 12;
    }

    /// <summary>DEMOTE-Weather-batch — dormant ghost. Re-promote with Boot+Save+host.</summary>
    public class Weather_FogOfParticulate
    {
        private ParticulateFogState _state = new ParticulateFogState();

        public event Action<ParticulateFogState, float> OnFilterLoadDoubled;   // (state, multiplier)
        public event Action<ParticulateFogState, float> OnUnmaskedDoseApplied; // (state, mSv)

        public ParticulateFogState State => _state;

        public float Tick(float deltaHours, bool isOutside, bool hasMask, System.Random rng)
        {
            if (!_state.isActive) return 0f;
            if (isOutside && !hasMask)
            {
                float dose = _state.unmaskedBreathingDoseMSvPerHour * deltaHours;
                OnUnmaskedDoseApplied?.Invoke(_state, dose);
                return dose;
            }
            return 0f;
        }

        public void NotifyFilterLoad(float deltaHours)
        {
            if (!_state.isActive) return;
            OnFilterLoadDoubled?.Invoke(_state, _state.airFilterDrainMultiplier * deltaHours);
        }

        public void SetActive(bool active) { _state.isActive = active; }

        /// <summary>Convenience: fire the event for its full configured duration (matches Weather_BloodRain.Trigger() convention).</summary>
        public void Trigger() => SetActive(true);

        public ParticulateFogState CaptureState() => _state;
        public void RestoreState(ParticulateFogState s) { _state = s ?? new ParticulateFogState(); }
    }
}
