using System;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Tracks an active "knock at the hatch" player decision. Starts a
    /// timeout so the expedition does not sit in <c>AtHatchDilemma</c>
    /// forever; on expiry auto-resolves with <see cref="HatchDilemmaResolvedSignal.Resolution.ForceDeconOutside"/>.
    /// </summary>
    public class HatchDilemmaPrompt
    {
        /// <summary>Game-hours before auto-resolve if the player does not choose.</summary>
        public const float DefaultTimeoutHours = 4f;

        public bool IsActive { get; private set; }
        public string ExpeditionId { get; private set; } = string.Empty;
        public float HoursRemaining { get; private set; }

        /// <summary>Fired once when the timeout expires. Argument is the auto resolution.</summary>
        public event Action<HatchDilemmaResolvedSignal.Resolution> OnTimeout;

        /// <summary>Fired when a choice is applied via <see cref="ApplyChoice"/>.</summary>
        public event Action<HatchDilemmaResolvedSignal.Resolution> OnChoiceApplied;

        /// <summary>Begin tracking a hatch dilemma for the given expedition.</summary>
        public void Begin(ExpeditionState exp, float timeoutHours = DefaultTimeoutHours)
        {
            if (exp == null)
            {
                Cancel();
                return;
            }

            IsActive = true;
            ExpeditionId = exp.ExpeditionId ?? string.Empty;
            HoursRemaining = Mathf.Max(0.1f, timeoutHours);
        }

        /// <summary>Advance the timeout. Fires <see cref="OnTimeout"/> once when it expires.</summary>
        public void Tick(float gameHours)
        {
            if (!IsActive || gameHours <= 0f) return;

            HoursRemaining -= gameHours;
            if (HoursRemaining > 0f) return;

            IsActive = false;
            HoursRemaining = 0f;
            var resolution = HatchDilemmaResolvedSignal.Resolution.ForceDeconOutside;
            OnTimeout?.Invoke(resolution);
        }

        /// <summary>
        /// Player (or AI) selected a resolution. Cancels the timeout and
        /// notifies <see cref="OnChoiceApplied"/>.
        /// </summary>
        public void ApplyChoice(HatchDilemmaResolvedSignal.Resolution resolution)
        {
            if (!IsActive)
            {
                OnChoiceApplied?.Invoke(resolution);
                return;
            }

            IsActive = false;
            HoursRemaining = 0f;
            OnChoiceApplied?.Invoke(resolution);
        }

        /// <summary>Abort without firing timeout (choice already resolved elsewhere).</summary>
        public void Cancel()
        {
            IsActive = false;
            HoursRemaining = 0f;
            ExpeditionId = string.Empty;
        }
    }
}
