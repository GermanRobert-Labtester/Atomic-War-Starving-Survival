// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.PlayerCommand
{
    /// <summary>
    /// Result of executing a player command.
    /// Wraps the authoritative <see cref="ActionResult"/> and adds
    /// command correlation, state-version validation, and deterministic
    /// action-log identity.
    /// </summary>
    public readonly struct CommandResult
    {
        /// <summary>Stable command identifier.</summary>
        public string CommandCode { get; }

        /// <summary>Authoritative gameplay result.</summary>
        public ActionResult ActionResult { get; }

        /// <summary>True when the command mutated state successfully.</summary>
        public bool IsSuccess => ActionResult.IsSuccess;

        /// <summary>True when the command was blocked by a known precondition.</summary>
        public bool IsBlocked => ActionResult.Status == ActionResult.StatusKind.Blocked;

        /// <summary>True when the command failed unexpectedly.</summary>
        public bool IsFailed => ActionResult.Status == ActionResult.StatusKind.Failed;

        /// <summary>
        /// State version that was current when the preview was captured.
        /// Mismatch against the current session version indicates a stale preview.
        /// </summary>
        public long ExpectedStateVersion { get; }

        /// <summary>
        /// State version after this command executed, or the same version if it did not execute.
        /// </summary>
        public long ActualStateVersion { get; }

        /// <summary>Deterministic action-log sequence number, or -1 if not logged.</summary>
        public long ActionLogSequence { get; }

        /// <summary>Stable reason code from the inner result.</summary>
        public string FailureCode => ActionResult.FailureCode;

        /// <summary>Localization message key from the inner result.</summary>
        public string MessageKey => ActionResult.MessageKey;

        /// <summary>Resource deltas from the inner result.</summary>
        public IReadOnlyDictionary<string, double> Deltas => ActionResult.Deltas;

        public CommandResult(
            string commandCode,
            ActionResult actionResult,
            long expectedStateVersion,
            long actualStateVersion,
            long actionLogSequence = -1)
        {
            CommandCode = commandCode ?? string.Empty;
            ActionResult = actionResult;
            ExpectedStateVersion = expectedStateVersion;
            ActualStateVersion = actualStateVersion;
            ActionLogSequence = actionLogSequence;
        }

        public static CommandResult FromSuccess(
            string commandCode,
            ActionResult actionResult,
            long expectedStateVersion,
            long actualStateVersion,
            long actionLogSequence = -1)
        {
            return new CommandResult(commandCode, actionResult, expectedStateVersion, actualStateVersion, actionLogSequence);
        }

        public static CommandResult FromPreview(CommandPreview preview, string messageKey = "")
        {
            var ar = ActionResult.Blocked(preview.FailureCode, string.IsNullOrEmpty(messageKey) ? preview.MessageKey : messageKey);
            return new CommandResult(preview.CommandCode, ar, preview.StateVersion, preview.StateVersion);
        }

        public static CommandResult StalePreview(
            string commandCode,
            long expectedVersion,
            long actualVersion,
            string messageKey = "command.stale_preview")
        {
            var ar = ActionResult.Blocked("stale_preview", messageKey);
            return new CommandResult(commandCode, ar, expectedVersion, actualVersion);
        }

        public static CommandResult ContextBlocked(
            string commandCode,
            string failureCode,
            string messageKey,
            long stateVersion)
        {
            var ar = ActionResult.Blocked(failureCode, messageKey);
            return new CommandResult(commandCode, ar, stateVersion, stateVersion);
        }

        public override string ToString()
        {
            return $"[{CommandCode}] {ActionResult.Status} failure={FailureCode} expectedVersion={ExpectedStateVersion} actualVersion={ActualStateVersion}";
        }
    }
}
