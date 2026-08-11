using System;
using UnityEngine;

namespace AtomicWar._Game.Environment
{
    /// <summary>
    /// weather_silence — The Silence (Section X).
    /// No wind. No ash fall. No radiation spike. The sky is clear for
    /// the first time in weeks. The sun is visible. It's the most
    /// dangerous weather event in the game, because the survivors
    /// want to go outside. They want to stand in the light. They want
    /// to remember what the sky looked like. The radiation is still
    /// there. It's just quiet.
    /// </summary>
    [Serializable]
    public class SilenceState
    {
        public string weatherId = "weather_silence";
        public string displayName = "The Silence";
        public bool isActive = false;
        public float temptationMoraleBonus = 5f;        // the lure
        public float temptationPressurePerHour = 1f;   // accumulates urge to surface
        public int durationHours = 48;
    }

    /// <summary>DEMOTE-Weather-batch — dormant ghost. Re-promote with Boot+Save+host.</summary>
    public class Weather_Silence
    {
        private SilenceState _state = new SilenceState();

        public event Action<SilenceState> OnClearSkyObserved;
        public event Action<SilenceState, float> OnTemptationToSurface;     // (state, accumulatedUrgency)
        public event Action<SilenceState, string> OnSurfaceVentured;       // (state, survivorId) — fatal if rad still there

        public SilenceState State => _state;

        public void Tick(float deltaHours)
        {
            if (!_state.isActive) return;
            OnClearSkyObserved?.Invoke(_state);
            OnTemptationToSurface?.Invoke(_state, _state.temptationPressurePerHour * deltaHours);
        }

        /// <summary>Host calls this when a survivor surfaces during The Silence.</summary>
        public void RecordSurfaceVentured(string survivorId)
        {
            if (!_state.isActive) return;
            OnSurfaceVentured?.Invoke(_state, survivorId);
        }

        public void SetActive(bool active) { _state.isActive = active; }

        /// <summary>Convenience: fire the event for its full configured duration (matches Weather_BloodRain.Trigger() convention).</summary>
        public void Trigger() => SetActive(true);

        public SilenceState CaptureState() => _state;
        public void RestoreState(SilenceState s) { _state = s ?? new SilenceState(); }
    }
}
