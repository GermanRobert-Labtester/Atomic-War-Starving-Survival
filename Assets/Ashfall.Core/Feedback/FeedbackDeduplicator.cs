// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Feedback
{
    /// <summary>
    /// Engine-agnostic deduplication and spam suppression policy engine.
    /// Suppresses repeated transient notifications within a presentation window,
    /// while allowing severity escalations to bypass cooldowns.
    /// Also tracks state transitions so ticked warnings fire only when entering an alert state.
    /// </summary>
    public sealed class FeedbackDeduplicator
    {
        private struct DedupeEntry
        {
            public float Timestamp;
            public FeedbackSeverity Severity;
        }

        private readonly Dictionary<string, DedupeEntry> _lastSeen = new Dictionary<string, DedupeEntry>(StringComparer.Ordinal);
        private readonly HashSet<string> _activeAlertStates = new HashSet<string>(StringComparer.Ordinal);

        public float CooldownSeconds { get; set; } = 5.0f;

        public FeedbackDeduplicator(float cooldownSeconds = 5.0f)
        {
            CooldownSeconds = Math.Max(0.5f, cooldownSeconds);
        }

        /// <summary>
        /// Evaluates whether an event with the given dedupe key and severity should be suppressed.
        /// An escalation to a strictly higher severity level bypasses suppression.
        /// </summary>
        public bool ShouldSuppress(string dedupeKey, FeedbackSeverity severity, float currentTimeSeconds)
        {
            if (string.IsNullOrEmpty(dedupeKey)) return false;

            if (_lastSeen.TryGetValue(dedupeKey, out var entry))
            {
                // Severity escalation bypasses cooldown
                if (severity > entry.Severity)
                {
                    return false;
                }

                // If within cooldown window, suppress
                if (currentTimeSeconds >= entry.Timestamp && (currentTimeSeconds - entry.Timestamp) < CooldownSeconds)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Records an emitted event for subsequent deduplication checks.
        /// </summary>
        public void Record(string dedupeKey, FeedbackSeverity severity, float currentTimeSeconds)
        {
            if (string.IsNullOrEmpty(dedupeKey)) return;

            _lastSeen[dedupeKey] = new DedupeEntry
            {
                Timestamp = currentTimeSeconds,
                Severity = severity
            };
        }

        /// <summary>
        /// State transition check for continuous/ticked warnings.
        /// Returns true ONLY when transitioning from normal (false) to alert (true).
        /// When isAlert becomes false, the state is cleared so subsequent alerts can fire again.
        /// </summary>
        public bool EvaluateTransition(string transitionKey, bool isAlert)
        {
            if (string.IsNullOrEmpty(transitionKey)) return isAlert;

            if (isAlert)
            {
                // Returns true only if it wasn't already in the alert set
                return _activeAlertStates.Add(transitionKey);
            }
            else
            {
                _activeAlertStates.Remove(transitionKey);
                return false;
            }
        }

        /// <summary>
        /// Clears all deduplication history and transition states (e.g. on game load/reset).
        /// </summary>
        public void Reset()
        {
            _lastSeen.Clear();
            _activeAlertStates.Clear();
        }
    }
}
