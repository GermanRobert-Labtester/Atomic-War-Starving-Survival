// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Campaign
{
    /// <summary>
    /// C2 / Plan 17A-S — semantic-vocabulary classification for day events.
    ///
    /// The re-audit of Plan 17 found that <see cref="DailyBriefingReportBuilder"/>
    /// handled only a subset of the kinds producers actually emit, and its closed
    /// switch silently discarded the rest. This vocabulary is the C2 consumer-side
    /// repair: every emitted kind is either
    ///
    ///   1. explicitly handled by a builder case (player-facing, tailored text),
    ///   2. classified as an internal heartbeat (intentionally non-player-facing —
    ///      steady-state owner ticks that would flood the briefing noise budget),
    ///   or 3. rendered through the documented generic representation
    ///      (<see cref="GenericSectionTitle"/> / <see cref="RenderGeneric"/>) so a
    ///      valid producer event can never silently disappear.
    ///
    /// This is NOT the Plan 31 semantic-kind authority. Plan 31 (Event Layer
    /// Semantic Kinds / No Silent Drops) may replace or formalize this vocabulary;
    /// until then this class is the documented consumer contract C2 depends on.
    /// Classification is deterministic: no RNG, no dictionary-order dependence.
    /// </summary>
    public static class DayEventVocabulary
    {
        /// <summary>Section title for generically rendered non-heartbeat kinds.</summary>
        public const string GenericSectionTitle = "System Activity";

        /// <summary>
        /// Kinds that do not end in the heartbeat suffix but are still
        /// internal aggregation markers, intentionally not shown to players.
        /// Documented and tested as intentionally retained.
        /// </summary>
        private static readonly HashSet<string> InternalKinds = new(StringComparer.Ordinal)
        {
            "events_evaluated", // protagonist/narrative aggregate heartbeat
            "world_ticked",     // world-simulation aggregate heartbeat
        };

        /// <summary>
        /// True when the kind is a steady-state owner heartbeat. Heartbeats are
        /// intentionally classified as non-player-facing: rendering every daily
        /// tick would breach the briefing noise budget (C2 §42.1). The
        /// classification is suffix-based plus a curated internal set so new
        /// owners that follow the repository naming convention are classified
        /// without code changes here.
        /// </summary>
        public static bool IsInternalHeartbeat(string? kind)
        {
            if (string.IsNullOrEmpty(kind)) return true;
            if (InternalKinds.Contains(kind)) return true;
            return kind.EndsWith("_ticked", StringComparison.Ordinal);
        }

        /// <summary>
        /// Deterministic generic rendering for an unhandled, non-heartbeat event.
        /// Shows kind, source owner, entities, and numeric payload so the player
        /// sees that something happened and where it came from — never silently
        /// dropped.
        /// </summary>
        public static string RenderGeneric(DayStateChangeEvent evt)
        {
            if (evt == null) return string.Empty;
            string kind = string.IsNullOrEmpty(evt.Kind) ? "unknown" : evt.Kind;
            string owner = string.IsNullOrEmpty(evt.SourceOwnerId) ? "system" : evt.SourceOwnerId;
            string text = $"{kind.Replace('_', ' ')} ({owner}";
            if (!string.IsNullOrEmpty(evt.PrimaryId)) text += $": {evt.PrimaryId}";
            text += ")";
            if (!string.IsNullOrEmpty(evt.SecondaryId)) text += $" — {evt.SecondaryId}";
            if (evt.Numeric != 0f) text += $" [{evt.Numeric:F0}]";
            return text;
        }
    }
}
