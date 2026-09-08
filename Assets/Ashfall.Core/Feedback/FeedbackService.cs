using System;
using System.Collections.Generic;

namespace Ashfall.Core.Feedback
{
    /// <summary>
    /// Engine-agnostic canonical feedback service implementing presentation resolution,
    /// safe parameter substitution, deduplication/cooldown gating, and developer-error separation.
    /// </summary>
    public sealed class FeedbackService : IFeedbackService
    {
        private static readonly HashSet<string> InternalDiagnosticKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "missing_id",
            "file_not_found",
            "corrupt_data",
            "system_overload",
            "permission_denied",
            "network_error",
            "out_of_bounds",
            "invalid_input"
        };

        public FeedbackMessageCatalog Catalog { get; }
        public FeedbackDeduplicator Deduplicator { get; }

        public event Action<ResolvedFeedbackMessage>? OnFeedbackEmitted;
        public event Action<ResolvedFeedbackMessage>? OnDiagnosticEmitted;

        public FeedbackService(FeedbackMessageCatalog? catalog = null, FeedbackDeduplicator? deduplicator = null)
        {
            Catalog = catalog ?? new FeedbackMessageCatalog();
            Deduplicator = deduplicator ?? new FeedbackDeduplicator();
        }

        /// <summary>
        /// Pure resolution function converting a FeedbackEvent into a ResolvedFeedbackMessage.
        /// Does not alter deduplication state or trigger emission events.
        /// </summary>
        public ResolvedFeedbackMessage Resolve(FeedbackEvent evt)
        {
            if (evt == null) throw new ArgumentNullException(nameof(evt));

            string key = evt.Key;
            string category = evt.Category ?? string.Empty;

            FeedbackMessageTemplate? template = null;
            if (!string.IsNullOrEmpty(category))
            {
                Catalog.TryGetTemplate(category, key, out template);
            }
            if (template == null)
            {
                Catalog.TryGetTemplate(key, out template);
            }

            string resolvedCategory = template?.category ?? category;
            if (string.IsNullOrEmpty(resolvedCategory))
            {
                resolvedCategory = "status";
            }

            FeedbackSeverity severity = evt.SeverityOverride ?? template?.GetSeverity() ?? FeedbackSeverity.Info;
            float duration = template != null && template.display_duration_seconds > 0
                ? Math.Clamp(template.display_duration_seconds, 1.0f, 15.0f)
                : 3.0f;

            string formattedText;
            if (template != null)
            {
                formattedText = FeedbackMessageCatalog.SafeFormat(template.template, evt.Arguments);
            }
            else if (!string.IsNullOrEmpty(category))
            {
                formattedText = Catalog.FormatCategory(category, key, evt.Arguments);
            }
            else
            {
                formattedText = FeedbackMessageCatalog.SafeFormat("Status: {0}", evt.Arguments.Length > 0 ? evt.Arguments : new object[] { key });
            }

            bool isDiag = evt.IsDiagnosticOnly || InternalDiagnosticKeys.Contains(key);

            return new ResolvedFeedbackMessage
            {
                Key = key,
                Category = resolvedCategory,
                Severity = severity,
                FormattedText = formattedText,
                DisplayDurationSeconds = duration,
                DedupeKey = evt.DedupeKey,
                SourceSystem = evt.SourceSystem,
                PresentationContext = evt.PresentationContext,
                IsDiagnosticOnly = isDiag,
                TimestampTick = Environment.TickCount64
            };
        }

        /// <summary>
        /// Emits a feedback event, evaluating deduplication and routing diagnostics to the appropriate handler.
        /// Returns true if the message was accepted and emitted; false if suppressed by deduplication.
        /// </summary>
        public bool Emit(FeedbackEvent evt, float currentTimeSeconds = 0f)
        {
            if (evt == null) return false;

            var resolved = Resolve(evt);

            // Developer / internal diagnostics are routed to diagnostic stream and bypass gameplay toast suppression
            if (resolved.IsDiagnosticOnly)
            {
                OnDiagnosticEmitted?.Invoke(resolved);
                return true;
            }

            // Check deduplication policy
            if (Deduplicator.ShouldSuppress(resolved.DedupeKey, resolved.Severity, currentTimeSeconds))
            {
                return false;
            }

            Deduplicator.Record(resolved.DedupeKey, resolved.Severity, currentTimeSeconds);
            OnFeedbackEmitted?.Invoke(resolved);
            return true;
        }
    }
}
