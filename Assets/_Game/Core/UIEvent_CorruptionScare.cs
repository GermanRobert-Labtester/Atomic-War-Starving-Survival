using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class CorruptionScareState
    {
        public string eventId = "ui_event_corruption_scare";
        public float displayDurationSeconds = 2f;
        public bool isActive = false;
    }

    /// <summary>
    /// Prompt #755: Save File Corruption Scare.
    /// During EMP/RadioactiveStorm, fake "SAVE FILE CORRUPTED" for 2 seconds.
    /// Purely aesthetic, induces adrenaline.
    /// </summary>
    public class UIEvent_CorruptionScare
    {
        private CorruptionScareState _state = new CorruptionScareState();
        private float _remainingSeconds = 0f;

        public event Action OnScareStarted;
        public event Action OnScareEnded;

        public CorruptionScareState State => _state;

        public void TriggerScare()
        {
            if (_state.isActive)
                return;

            _state.isActive = true;
            _remainingSeconds = _state.displayDurationSeconds;
            OnScareStarted?.Invoke();
        }

        public void TickSecond(float delta)
        {
            if (!_state.isActive)
                return;

            _remainingSeconds -= delta;
            if (_remainingSeconds <= 0f)
            {
                _remainingSeconds = 0f;
                _state.isActive = false;
                OnScareEnded?.Invoke();
            }
        }

        public bool IsActive() => _state.isActive;
    }
}
