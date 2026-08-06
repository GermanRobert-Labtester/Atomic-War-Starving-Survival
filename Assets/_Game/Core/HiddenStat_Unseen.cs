using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class UnseenState
    {
        public string statId = "hidden_stat_unseen";
        public float currentLevel = 0f;
        public float peakThreshold = 1.0f;
        public float risePerDarkRoom = 0.15f;
    }

    /// <summary>
    /// Prompt #754: The "Unseen" — Hidden Tracker.
    /// Leaving rooms in 100% darkness raises stat. At peak, game plays
    /// breathing/footsteps from dark room.
    /// </summary>
    public class HiddenStat_Unseen
    {
        private UnseenState _state = new UnseenState();
        private bool _peakAudioCuedThisCycle = false;

        public event Action OnAudioCuePlayed;
        public event Action<string> OnDarkRoomAudioHeard;

        public UnseenState State => _state;

        public void LeaveDarkRoom(bool isRoom100Dark, string roomId = null)
        {
            if (!isRoom100Dark)
                return;

            _state.currentLevel = Mathf.Clamp01(_state.currentLevel + _state.risePerDarkRoom);

            if (!string.IsNullOrEmpty(roomId))
            {
                OnDarkRoomAudioHeard?.Invoke(roomId);
            }

            // At peak, trigger audio cue once per cycle
            if (IsAtPeak() && !_peakAudioCuedThisCycle)
            {
                _peakAudioCuedThisCycle = true;
                OnAudioCuePlayed?.Invoke();
            }
        }

        public bool IsAtPeak() => _state.currentLevel >= _state.peakThreshold;

        public float GetLevel() => _state.currentLevel;

        /// <summary>
        /// Resets to 0 when lights are turned on.
        /// </summary>
        public void Reset()
        {
            _state.currentLevel = 0f;
            _peakAudioCuedThisCycle = false;
        }
    }
}
