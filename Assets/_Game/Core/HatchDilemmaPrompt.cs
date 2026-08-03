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

        private readonly float _defaultTimeout;

        public bool IsActive { get; private set; }
        public string ExpeditionId { get; private set; } = string.Empty;
        public ExpeditionState ActiveExpedition { get; private set; }
        public float HoursRemaining { get; private set; }

        /// <summary>Alias used by follow-up tests / UI.</summary>
        public float TimeRemainingGameHours => HoursRemaining;

        /// <summary>Fired when <see cref="Begin"/> successfully starts a prompt.</summary>
        public event Action<ExpeditionState> OnPromptReady;

        /// <summary>Fired once when the timeout expires. Argument is the auto resolution.</summary>
        public event Action<HatchDilemmaResolvedSignal.Resolution> OnTimeout;

        /// <summary>Fired when a choice is applied via <see cref="ApplyChoice"/> / <see cref="Resolve"/>.</summary>
        public event Action<HatchDilemmaResolvedSignal.Resolution> OnChoiceApplied;

        public HatchDilemmaPrompt(float timeoutGameHours = DefaultTimeoutHours)
        {
            _defaultTimeout = Mathf.Max(0.1f, timeoutGameHours);
        }

        /// <summary>Begin tracking a hatch dilemma. No-op if already active.</summary>
        public void Begin(ExpeditionState exp, float timeoutHours = -1f)
        {
            if (IsActive) return;
            if (exp == null)
            {
                Cancel();
                return;
            }

            float timeout = timeoutHours > 0f ? timeoutHours : _defaultTimeout;
            IsActive = true;
            ActiveExpedition = exp;
            ExpeditionId = exp.ExpeditionId ?? string.Empty;
            HoursRemaining = Mathf.Max(0.1f, timeout);
            OnPromptReady?.Invoke(exp);
        }

        /// <summary>Advance the timeout. Fires <see cref="OnTimeout"/> once when it expires.</summary>
        public void Tick(float gameHours)
        {
            if (!IsActive || gameHours <= 0f) return;

            HoursRemaining -= gameHours;
            if (HoursRemaining > 0f) return;

            IsActive = false;
            HoursRemaining = 0f;
            ActiveExpedition = null;
            var resolution = HatchDilemmaResolvedSignal.Resolution.ForceDeconOutside;
            OnTimeout?.Invoke(resolution);
        }

        /// <summary>
        /// Player (or AI) selected a resolution. Cancels the timeout and
        /// notifies <see cref="OnChoiceApplied"/>.
        /// </summary>
        public void ApplyChoice(HatchDilemmaResolvedSignal.Resolution resolution)
        {
            Resolve(resolution);
        }

        /// <summary>Player chose a resolution before timeout.</summary>
        public void Resolve(HatchDilemmaResolvedSignal.Resolution resolution)
        {
            if (!IsActive)
            {
                OnChoiceApplied?.Invoke(resolution);
                return;
            }

            IsActive = false;
            HoursRemaining = 0f;
            ActiveExpedition = null;
            OnChoiceApplied?.Invoke(resolution);
        }

        /// <summary>Abort without firing timeout (choice already resolved elsewhere).</summary>
        public void Cancel()
        {
            IsActive = false;
            HoursRemaining = 0f;
            ExpeditionId = string.Empty;
            ActiveExpedition = null;
        }
    }
}
