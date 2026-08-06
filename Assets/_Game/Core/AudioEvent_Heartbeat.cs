using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class HeartbeatState
    {
        public string eventId = "audio_event_heartbeat";
        public float healthThreshold = 0.1f;
        public bool isActive = false;
    }

    /// <summary>
    /// Prompt #758: Heartbeat Haptics.
    /// As Health drops below 10%, music fades out, replaced by heavy heartbeat.
    /// If heartbeat stops, they're dead.
    /// </summary>
    public class AudioEvent_Heartbeat
    {
        private HeartbeatState _state = new HeartbeatState();

        public event Action OnHeartbeatStarted;
        public event Action OnHeartbeatStopped;
        public event Action OnDeathTransition;

        public HeartbeatState State => _state;

        public void CheckHealth(string survivorId, float healthPercent)
        {
            if (healthPercent < _state.healthThreshold && !_state.isActive)
            {
                _state.isActive = true;
                OnHeartbeatStarted?.Invoke();
            }
            else if (healthPercent >= _state.healthThreshold && _state.isActive)
            {
                // Health recovered above threshold
                StopHeartbeat(isDeath: false);
            }
        }

        public void StopHeartbeat(bool isDeath)
        {
            if (!_state.isActive && !isDeath)
                return;

            _state.isActive = false;

            if (isDeath)
            {
                OnDeathTransition?.Invoke();
            }
            else
            {
                OnHeartbeatStopped?.Invoke();
            }
        }

        public bool IsActive() => _state.isActive;
    }
}
