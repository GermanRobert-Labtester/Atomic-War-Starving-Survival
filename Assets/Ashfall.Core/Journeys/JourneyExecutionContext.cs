// SPDX-License-Identifier: MIT
// ASHFALL Core End-to-End Player Journey Context & Diagnostics (Task 110).
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Ashfall.Core.Journeys
{
    /// <summary>
    /// Tracks execution context across deterministic end-to-end player journeys.
    /// Provides standardized machine-readable failure diagnostics with seed, day,
    /// route, step index, and last attempted action for CI triage.
    /// </summary>
    public sealed class JourneyExecutionContext
    {
        public string JourneyName { get; }
        public ulong Seed { get; }
        public int Day { get; set; } = 1;
        public string CurrentRoute { get; set; } = "main_menu";
        public string LastAction { get; set; } = "bootstrap";
        public int StepIndex { get; set; }
        public List<string> ActionLog { get; } = new();

        public JourneyExecutionContext(string journeyName, ulong seed)
        {
            JourneyName = journeyName ?? throw new ArgumentNullException(nameof(journeyName));
            Seed = seed;
            ActionLog.Add($"[Day 1] Initialized journey '{journeyName}' with seed {seed}");
        }

        public void Navigate(string route, string actionDescription)
        {
            StepIndex++;
            CurrentRoute = route;
            LastAction = actionDescription;
            ActionLog.Add($"[Day {Day} | Step {StepIndex}] Route: {route} -> Action: {actionDescription}");
        }

        public void AdvanceDay(int newDay)
        {
            Day = newDay;
            StepIndex++;
            LastAction = $"AdvanceDay -> {newDay}";
            ActionLog.Add($"[Day {Day} | Step {StepIndex}] Advanced campaign day to {newDay}");
        }

        /// <summary>
        /// Emits a single-line standardized diagnostic string for CI logs.
        /// </summary>
        public string FormatFailureDiagnostic(string failureMessage)
        {
            return $"[JOURNEY_FAILURE] journey=\"{JourneyName}\" seed={Seed} day={Day} route=\"{CurrentRoute}\" action=\"{LastAction}\" step={StepIndex} details=\"{failureMessage}\"";
        }

        /// <summary>
        /// Emits structured JSON diagnostics for machine-readable CI artifact emission.
        /// </summary>
        public string FormatFailureJson(string failureMessage)
        {
            var data = new
            {
                status = "FAILED",
                journey = JourneyName,
                seed = Seed,
                day = Day,
                route = CurrentRoute,
                lastAction = LastAction,
                stepIndex = StepIndex,
                details = failureMessage,
                recentLog = ActionLog
            };
            return JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = false });
        }
    }
}
