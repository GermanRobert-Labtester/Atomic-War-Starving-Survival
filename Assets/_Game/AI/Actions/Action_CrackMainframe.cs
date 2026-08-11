using System;
using UnityEngine;

namespace AtomicWar._Game.AI.Actions
{
    [Serializable]
    public class CrackMainframeState
    {
        public string actionId = "action_crack_mainframe";
        public float powerRequired = 500f;
        public float hoursRequired = 24f;
        public float hoursElapsed = 0f;
        public bool isRunning = false;
        public bool isCompleted = false;
        public bool serverFried = false;
    }

    /// <summary>
    /// Mainframe Decryption — a high-power action that runs for 24 hours
    /// consuming 500W continuously. On completion, it yields all WarlordBase
    /// and NuclearSilo coordinates. If power drops below 500W at any point
    /// during the process, the server fries permanently and the action fails.
    /// Prompt #798: Action_CrackMainframe
    /// </summary>
    /// <summary>DEMOTE-Action-remaining — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class Action_CrackMainframe
    {
        // -- Constants --
        public const float PowerRequired = 500f;
        public const float HoursRequired = 24f;

        // -- Events --
        public event Action OnDecryptionStarted;
        public event Action OnDecryptionCompleted;
        public event Action OnServerFried;

        // -- State --
        private float _hoursElapsed = 0f;
        private bool _isRunning = false;
        private bool _isCompleted = false;
        private bool _serverFried = false;

        // -- Public API --

        /// <summary>
        /// Starts the mainframe decryption process. Requires at least 500W
        /// available power. Returns true if the crack was successfully started.
        /// </summary>
        public bool StartCrack(float availablePower)
        {
            if (_isCompleted || _serverFried)
            {
                Debug.LogWarning("[CrackMainframe] Cannot start — action already resolved.");
                return false;
            }
            if (_isRunning)
            {
                Debug.LogWarning("[CrackMainframe] Decryption is already in progress.");
                return false;
            }
            if (availablePower < PowerRequired)
            {
                Debug.LogWarning("[CrackMainframe] Insufficient power to start decryption.");
                return false;
            }

            _hoursElapsed = 0f;
            _isRunning = true;
            OnDecryptionStarted?.Invoke();
            return true;
        }

        /// <summary>
        /// Advances the decryption by the given number of hours.
        /// If available power drops below 500W during processing, the server
        /// fries permanently and the action fails.
        /// Returns true if the decryption completed successfully this tick.
        /// </summary>
        public bool TickHour(float hours, float availablePower)
        {
            if (!_isRunning) return false;
            if (_isCompleted || _serverFried) return false;

            // Check power — if it drops below threshold, server fries
            if (availablePower < PowerRequired)
            {
                _serverFried = true;
                _isRunning = false;
                OnServerFried?.Invoke();
                Debug.LogWarning("[CrackMainframe] Power dropped below 500W — server fried permanently.");
                return false;
            }

            _hoursElapsed += hours;

            // Check completion
            if (_hoursElapsed >= HoursRequired)
            {
                _hoursElapsed = HoursRequired;
                _isRunning = false;
                _isCompleted = true;
                OnDecryptionCompleted?.Invoke();
                return true;
            }

            return false;
        }

        /// <summary>Returns the number of hours remaining until decryption completes.</summary>
        public float GetHoursRemaining()
        {
            return Mathf.Max(0f, HoursRequired - _hoursElapsed);
        }

        /// <summary>Returns true if decryption is currently running.</summary>
        public bool IsRunning() => _isRunning;

        /// <summary>Returns true if decryption completed successfully.</summary>
        public bool IsCompleted() => _isCompleted;

        /// <summary>Returns true if the server was fried by a power failure.</summary>
        public bool IsServerFried() => _serverFried;

        // -- Save / Load --

        public CrackMainframeState CaptureState()
        {
            return new CrackMainframeState
            {
                actionId = "action_crack_mainframe",
                powerRequired = PowerRequired,
                hoursRequired = HoursRequired,
                hoursElapsed = _hoursElapsed,
                isRunning = _isRunning,
                isCompleted = _isCompleted,
                serverFried = _serverFried
            };
        }

        public void RestoreState(CrackMainframeState saved)
        {
            if (saved == null) return;
            _hoursElapsed = saved.hoursElapsed;
            _isRunning = saved.isRunning;
            _isCompleted = saved.isCompleted;
            _serverFried = saved.serverFried;
        }
    }
}
