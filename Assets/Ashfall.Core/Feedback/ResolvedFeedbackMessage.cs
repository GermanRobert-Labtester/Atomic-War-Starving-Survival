using System;

namespace Ashfall.Core.Feedback
{
    /// <summary>
    /// Engine-agnostic resolved feedback message ready for presentation rendering.
    /// Contains formatted text, severity styling metadata, and presentation duration.
    /// </summary>
    public sealed class ResolvedFeedbackMessage
    {
        public string Key { get; init; } = string.Empty;
        public string Category { get; init; } = string.Empty;
        public FeedbackSeverity Severity { get; init; } = FeedbackSeverity.Info;
        public string FormattedText { get; init; } = string.Empty;
        public float DisplayDurationSeconds { get; init; } = 3.0f;
        public string DedupeKey { get; init; } = string.Empty;
        public string? SourceSystem { get; init; }
        public string? PresentationContext { get; init; }
        public bool IsDiagnosticOnly { get; init; }
        public long TimestampTick { get; init; }
    }
}
