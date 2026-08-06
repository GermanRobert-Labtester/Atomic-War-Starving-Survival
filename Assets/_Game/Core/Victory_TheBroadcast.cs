using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class BroadcastState
    {
        public string victoryId = "victory_the_broadcast";
        public bool isActive = false;
        public float uploadProgress = 0f;       // 0-100
        public float uploadSpeedPerMinute = 5f;
        public bool warlordAssaultActive = false;
        public float defensePower = 0f;
        public bool isUploadComplete = false;
    }

    /// <summary>
    /// Prompt #568: Endgame: The Broadcast (A Warning).
    /// Discover nukes were fired by rogue AI. Use HamRadio to broadcast kill-codes to orbital
    /// weapon platforms. Warlords assault bunker to stop you. Hold line until upload finishes.
    /// </summary>
    public class Victory_TheBroadcast
    {
        private BroadcastState _state = new BroadcastState();

        public event Action<BroadcastState> OnUploadStarted;
        public event Action<BroadcastState, float> OnUploadProgress;
        public event Action<BroadcastState, float> OnWarlordAssault;
        public event Action<BroadcastState> OnUploadComplete;
        public event Action<BroadcastState, string> OnUploadFailed;

        private bool _uploadFailed = false;

        public BroadcastState State => _state;

        public bool StartUpload(bool hasHamRadio, bool hasKillCodes)
        {
            if (!hasHamRadio || !hasKillCodes) return false;

            _state.isActive = true;
            _state.uploadProgress = 0f;
            _state.isUploadComplete = false;
            _uploadFailed = false;
            OnUploadStarted?.Invoke(_state);
            return true;
        }

        public void TickMinute(float defensePower, System.Random rng)
        {
            if (!_state.isActive || _uploadFailed || _state.isUploadComplete) return;

            _state.defensePower = defensePower;

            // Warlord assault may occur each minute tick
            if (rng != null && rng.NextDouble() < 0.3)
            {
                _state.warlordAssaultActive = true;
                float assaultStrength = (float)(rng.NextDouble() * 50.0 + 25.0);
                OnWarlordAssault?.Invoke(_state, assaultStrength);
            }

            // Upload progresses; warlord assault can slow it
            float progressGain = _state.uploadSpeedPerMinute;
            if (_state.warlordAssaultActive && _state.defensePower < 50f)
            {
                progressGain *= 0.5f; // Assault slows upload if defense is weak
            }

            _state.uploadProgress = Math.Min(_state.uploadProgress + progressGain, 100f);
            OnUploadProgress?.Invoke(_state, _state.uploadProgress);

            if (_state.uploadProgress >= 100f)
            {
                _state.isUploadComplete = true;
                _state.isActive = false;
                OnUploadComplete?.Invoke(_state);
            }
        }

        public bool ApplyWarlordAssault(float assaultStrength, float defensePower)
        {
            if (!_state.isActive || _uploadFailed) return false;

            _state.warlordAssaultActive = true;
            _state.defensePower = defensePower;
            OnWarlordAssault?.Invoke(_state, assaultStrength);

            if (assaultStrength > defensePower)
            {
                _uploadFailed = true;
                _state.isActive = false;
                OnUploadFailed?.Invoke(_state, "warlord_breach");
                return false;
            }

            return true;
        }

        public bool IsUploadComplete()
        {
            return _state.isUploadComplete;
        }

        public bool IsVictoryAchieved()
        {
            return _state.isUploadComplete;
        }
    }
}
