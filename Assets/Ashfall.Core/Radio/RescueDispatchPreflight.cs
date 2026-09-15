// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.Radio
{
    /// <summary>
    /// Recommended player-agency disposition for a rescue signal (plan §10).
    /// Analysis informs the player; it never automatically disables dispatch —
    /// RecordExpeditionDispatched remains ungated by authenticity.
    /// </summary>
    public enum RescueDispatchRecommendation
    {
        /// <summary>No threat known; normal rescue dispatch.</summary>
        DispatchRescue = 0,
        /// <summary>Deception detected: dispatch is the player's choice, with warning.</summary>
        DispatchWithWarning = 1,
        /// <summary>Sender dead or staleness confirmed: the expedition is a
        /// recovery/investigation, not a live rescue.</summary>
        RecoveryInvestigation = 2,
        /// <summary>Signal is terminal or not actionable; nothing to dispatch.</summary>
        NotApplicable = 3
    }

    /// <summary>
    /// Structured preflight result (plan §10) — a truthful read of the
    /// persisted signal runtime state. Pure projection; no side effects.
    /// </summary>
    [Serializable]
    public sealed class RescueDispatchPreflight
    {
        public string SignalId { get; set; } = string.Empty;
        public string QuestId { get; set; } = string.Empty;
        public bool Analyzed { get; set; }
        public SignalAuthenticityCategory Assessment { get; set; }
        public bool ThreatDetected { get; set; }
        public bool SenderAlive { get; set; }
        public bool Expired { get; set; }
        public RescueDispatchRecommendation Recommendation { get; set; }
        public string Note { get; set; } = string.Empty;
    }
}
