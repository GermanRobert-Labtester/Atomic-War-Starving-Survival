// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.PlayerCommand
{
    /// <summary>
    /// Execution-context flags that can block player commands independent of
    /// gameplay validation. Distinct from resource/availability failures.
    /// </summary>
    [Serializable]
    public sealed class CommandContext
    {
        public bool IsTutorialActive { get; set; }
        public bool IsModalOpen { get; set; }
        public bool IsTerminalRun { get; set; }
        public bool IsPaused { get; set; }

        /// <summary>Validate context and return a failure code if blocked.</summary>
        public string? GetBlockingFailureCode()
        {
            if (IsPaused) return "paused";
            if (IsModalOpen) return "blocked_by_modal";
            if (IsTerminalRun) return "terminal_run";
            if (IsTutorialActive) return "tutorial_restriction";
            return null;
        }
    }
}
