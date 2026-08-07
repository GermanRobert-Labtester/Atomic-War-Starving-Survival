using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class DeafeningState
    {
        public string eventId = "audio_event_deafening";
        public float durationMinutes = 3f;
        public bool isActive = false;
    }

    /// <summary>
    /// Prompt #751: Deafening Silence.
    /// After massive explosion, low-pass filter + tinnitus ring for 3 real-time minutes.
    /// Can't hear Raid Warnings or Geiger Clicks.
    /// </summary>
    public class AudioEvent_Deafening
    {
        private DeafeningState _state = new DeafeningState();
        private float _remainingSeconds = 0f;

        public event Action OnDeafeningStarted;
        public event Action OnDeafeningEnded;
        public event Action<string> OnWarningMissed;

        public DeafeningState State => _state;

        public void TriggerDeafening()
        {
            _state.isActive = true;
            _remainingSeconds = _state.durationMinutes * 60f;
            OnDeafeningStarted?.Invoke();
        }

        public void TickSecond(float deltaSeconds)
        {
            if (!_state.isActive)
                return;

            _remainingSeconds -= deltaSeconds;
            if (_remainingSeconds <= 0f)
            {
                _remainingSeconds = 0f;
                _state.isActive = false;
                OnDeafeningEnded?.Invoke();
            }
        }

        public bool CanHearWarnings() => !_state.isActive;

        public bool IsDeaf() => _state.isActive;

        /// <summary>
        /// Call when a warning should fire but deafening blocks it.
        /// </summary>
        public void ReportMissedWarning(string warningType)
        {
            if (_state.isActive)
            {
                OnWarningMissed?.Invoke(warningType);
            }
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public DeafeningState CaptureState() => _state;

        public void RestoreState(DeafeningState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
