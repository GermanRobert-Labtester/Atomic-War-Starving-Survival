using System;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class BloodRainState
    {
        public string weatherId = "weather_blood_rain";
        public string displayName = "Blood Rain";
        public float durationHours = 24f;
        public float hoursRemaining = 0f;
        public float panicDebuff = -20f;
        public float despairDebuff = -15f;
        public float waterContaminationMorale = -10f;
    }

    /// <summary>
    /// Prompt #652: Weather — Blood Rain.
    /// Vaporized red algae falls as rain. Physically harmless but deeply horrifying.
    /// Causes massive Panic and Despair. Contaminated red water applies an additional
    /// Morale penalty when consumed.
    /// </summary>
    public class Weather_BloodRain
    {
        private BloodRainState _state = new BloodRainState();

        // -- Events --
        public event Action<BloodRainState> OnBloodRainTriggered;
        public event Action<BloodRainState> OnBloodRainEnded;
        public event Action<BloodRainState, float, float> OnMoraleDebuffApplied;

        public BloodRainState State => _state;

        public bool IsActive => _state.hoursRemaining > 0f;

        /// <summary>
        /// Triggers the blood rain for its full configured duration.
        /// </summary>
        public void Trigger()
        {
            _state.hoursRemaining = _state.durationHours;
            OnBloodRainTriggered?.Invoke(_state);
        }

        /// <summary>
        /// Per-hour tick. Decrements remaining time and applies psychological debuffs.
        /// </summary>
        public void TickHour()
        {
            if (!IsActive) return;

            _state.hoursRemaining = Mathf.Max(0f, _state.hoursRemaining - 1f);

            OnMoraleDebuffApplied?.Invoke(_state, _state.panicDebuff, _state.despairDebuff);

            if (!IsActive)
            {
                OnBloodRainEnded?.Invoke(_state);
            }
        }

        /// <summary>
        /// Returns the combined morale penalty (panic + despair) while active.
        /// Returns 0 when the event has ended.
        /// </summary>
        public float GetMoralePenalty()
        {
            if (!IsActive) return 0f;
            return _state.panicDebuff + _state.despairDebuff;
        }

        /// <summary>
        /// Returns the morale penalty applied when drinking water contaminated
        /// by blood rain. Returns 0 when the event is not active.
        /// </summary>
        public float GetWaterContaminationEffect()
        {
            if (!IsActive) return 0f;
            return _state.waterContaminationMorale;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public BloodRainState GetState() => _state;

        public BloodRainState CaptureState() => GetState();

        public void RestoreState(BloodRainState state)
        {
            _state = state ?? new BloodRainState();
        }
    }
}
