// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.PlayerCommand
{
    /// <summary>
    /// Side-effect-free preview of a player command.
    /// Answers: CAN I DO THIS? WHAT WILL IT DO?
    /// </summary>
    public readonly struct CommandPreview
    {
        /// <summary>True when the command can be executed in the current state.</summary>
        public bool IsAvailable { get; }

        /// <summary>Stable failure code when unavailable; empty on success.</summary>
        public string FailureCode { get; }

        /// <summary>Localization message key for the preview outcome.</summary>
        public string MessageKey { get; }

        /// <summary>Projected resource/item deltas if the command succeeds.</summary>
        public IReadOnlyDictionary<string, double> ProjectedDeltas { get; }

        /// <summary>Estimated wall-clock or game-clock duration, if applicable.</summary>
        public float? EstimatedDurationHours { get; }

        /// <summary>Stable risk codes known before execution.</summary>
        public IReadOnlyList<string> RiskCodes { get; }

        /// <summary>True when the consequence cannot be undone without a new explicit action.</summary>
        public bool IsIrreversible { get; }

        /// <summary>
        /// Monotonic state version captured at preview time.
        /// Execute must present the same version or be rejected as stale.
        /// </summary>
        public long StateVersion { get; }

        /// <summary>Stable command identifier.</summary>
        public string CommandCode { get; }

        private CommandPreview(
            bool isAvailable,
            string failureCode,
            string messageKey,
            IReadOnlyDictionary<string, double>? projectedDeltas,
            float? estimatedDurationHours,
            IReadOnlyList<string>? riskCodes,
            bool isIrreversible,
            long stateVersion,
            string commandCode)
        {
            IsAvailable = isAvailable;
            FailureCode = failureCode ?? string.Empty;
            MessageKey = messageKey ?? string.Empty;
            ProjectedDeltas = projectedDeltas ?? new Dictionary<string, double>();
            EstimatedDurationHours = estimatedDurationHours;
            RiskCodes = riskCodes ?? Array.Empty<string>();
            IsIrreversible = isIrreversible;
            StateVersion = stateVersion;
            CommandCode = commandCode ?? string.Empty;
        }

        public static CommandPreview Available(
            string commandCode,
            long stateVersion,
            IReadOnlyDictionary<string, double>? projectedDeltas = null,
            float? estimatedDurationHours = null,
            IReadOnlyList<string>? riskCodes = null,
            bool isIrreversible = false,
            string messageKey = "")
        {
            return new CommandPreview(
                true, string.Empty, messageKey, projectedDeltas,
                estimatedDurationHours, riskCodes, isIrreversible,
                stateVersion, commandCode);
        }

        public static CommandPreview Unavailable(
            string commandCode,
            string failureCode,
            string messageKey,
            long stateVersion,
            IReadOnlyDictionary<string, double>? projectedDeltas = null,
            float? estimatedDurationHours = null,
            IReadOnlyList<string>? riskCodes = null)
        {
            return new CommandPreview(
                false, failureCode, messageKey, projectedDeltas,
                estimatedDurationHours, riskCodes, false,
                stateVersion, commandCode);
        }

        public override string ToString()
        {
            return $"[{CommandCode}] available={IsAvailable} version={StateVersion} failure={FailureCode}";
        }
    }
}
