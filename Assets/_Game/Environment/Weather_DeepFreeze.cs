using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Environment
{
    [Serializable]
    public class DeepFreezeState
    {
        public string weatherId = "weather_deep_freeze";
        public string displayName = "The Deep Freeze (-50°C)";
        public bool isActive = false;
        public float durationHoursRemaining = 0f; // 72 hours
        public float targetTemperature = -50f;
        public float heaterFuelConsumptionMultiplier = 3.0f; // Triples
        public bool cropsKilled = false;
    }

    /// <summary>
    /// Prompt #378: System: The Deep Freeze (-50°C).
    /// A 3-day temperature plunge to -50°C. Heater fuel consumption triples.
    /// Indoor crops die instantly; shelter temp below 0°C inflicts Frostbite.
    /// </summary>
    public class Weather_DeepFreeze
    {
        private DeepFreezeState _state = new DeepFreezeState();

        public event Action<DeepFreezeState> OnDeepFreezeStarted;
        public event Action<DeepFreezeState> OnCropsKilledByFreeze;
        public event Action<DeepFreezeState, float> OnFrostbiteRiskInflicted;

        public DeepFreezeState State => _state;

        public void TriggerDeepFreeze()
        {
            _state.isActive = true;
            _state.durationHoursRemaining = 72f;
            _state.cropsKilled = true;

            OnDeepFreezeStarted?.Invoke(_state);
            OnCropsKilledByFreeze?.Invoke(_state);
        }

        public void TickHourly(float hoursElapsed, float currentBunkerTemp)
        {
            if (!_state.isActive) return;

            _state.durationHoursRemaining -= hoursElapsed;
            if (currentBunkerTemp < 0f)
            {
                OnFrostbiteRiskInflicted?.Invoke(_state, currentBunkerTemp);
            }

            if (_state.durationHoursRemaining <= 0f)
            {
                _state.durationHoursRemaining = 0f;
                _state.isActive = false;
            }
        }

        public DeepFreezeState CaptureState() => _state;

        public void RestoreState(DeepFreezeState saved)
        {
            _state = saved ?? new DeepFreezeState();
        }
    }
}
