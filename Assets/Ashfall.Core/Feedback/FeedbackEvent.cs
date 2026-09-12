// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.Feedback
{
    /// <summary>
    /// Engine-agnostic data transfer object representing a player-facing feedback event
    /// produced by an authoritative domain outcome or state transition.
    /// </summary>
    public sealed class FeedbackEvent
    {
        public string Key { get; }
        public object[] Arguments { get; }
        public string? Category { get; }
        public string? SourceSystem { get; }
        public string? DedupeKey { get; }
        public FeedbackSeverity? SeverityOverride { get; }
        public string? PresentationContext { get; }
        public bool IsDiagnosticOnly { get; }

        public FeedbackEvent(
            string key,
            object[]? arguments = null,
            string? category = null,
            string? sourceSystem = null,
            string? dedupeKey = null,
            FeedbackSeverity? severityOverride = null,
            string? presentationContext = null,
            bool isDiagnosticOnly = false)
        {
            Key = key ?? string.Empty;
            Arguments = arguments ?? Array.Empty<object>();
            Category = category;
            SourceSystem = sourceSystem;
            DedupeKey = dedupeKey ?? key ?? string.Empty;
            SeverityOverride = severityOverride;
            PresentationContext = presentationContext;
            IsDiagnosticOnly = isDiagnosticOnly;
        }
    }
}
