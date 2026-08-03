using System;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Post-hatch-repel player prompt: offer to open trade / demand parley
    /// without hunting the trade UI. Soft timeout so the modal does not
    /// hang the campaign forever if the player never chooses.
    /// </summary>
    public class ParleyOfferPrompt
    {
        public const float DefaultTimeoutHours = 6f;

        public enum Resolution
        {
            /// <summary>Demand surrender immediately via economy.</summary>
            DemandParley,
            /// <summary>Open the trade screen (player can press [P] later).</summary>
            OpenTrade,
            /// <summary>Dismiss — no action.</summary>
            Dismiss,
            /// <summary>Timeout auto-dismiss.</summary>
            TimedOut
        }

        private readonly float _defaultTimeout;

        public bool IsActive { get; private set; }
        public string FactionId { get; private set; } = string.Empty;
        public string LeaderName { get; private set; } = string.Empty;
        public float HoursRemaining { get; private set; }

        public event Action<string /*factionId*/> OnPromptReady;
        public event Action<Resolution> OnTimeout;
        public event Action<Resolution> OnResolved;

        public ParleyOfferPrompt(float timeoutGameHours = DefaultTimeoutHours)
        {
            _defaultTimeout = Mathf.Max(0.1f, timeoutGameHours);
        }

        /// <summary>Begin a parley offer after a successful hatch repel. No-op if already active.</summary>
        public void Begin(string factionId, string leaderName = null, float timeoutHours = -1f)
        {
            if (IsActive) return;
            if (string.IsNullOrEmpty(factionId))
            {
                Cancel();
                return;
            }

            float timeout = timeoutHours > 0f ? timeoutHours : _defaultTimeout;
            IsActive = true;
            FactionId = factionId;
            LeaderName = string.IsNullOrEmpty(leaderName) ? string.Empty : leaderName;
            HoursRemaining = Mathf.Max(0.1f, timeout);
            OnPromptReady?.Invoke(FactionId);
        }

        public void Tick(float gameHours)
        {
            if (!IsActive || gameHours <= 0f) return;

            HoursRemaining -= gameHours;
            if (HoursRemaining > 0f) return;

            IsActive = false;
            HoursRemaining = 0f;
            FactionId = string.Empty;
            LeaderName = string.Empty;
            OnTimeout?.Invoke(Resolution.TimedOut);
        }

        public void Resolve(Resolution resolution)
        {
            if (!IsActive)
            {
                OnResolved?.Invoke(resolution);
                return;
            }

            IsActive = false;
            HoursRemaining = 0f;
            // Keep FactionId readable for the handler until Cancel clears it.
            OnResolved?.Invoke(resolution);
            FactionId = string.Empty;
            LeaderName = string.Empty;
        }

        public void Cancel()
        {
            IsActive = false;
            HoursRemaining = 0f;
            FactionId = string.Empty;
            LeaderName = string.Empty;
        }
    }
}
