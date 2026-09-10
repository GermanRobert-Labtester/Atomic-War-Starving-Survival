// SPDX-License-Identifier: MIT
// ASHFALL Core: Narrative continuity allowlist (Plan 50 / Phase 16G).
//
// Every entry is justified: ID + reason + owner/context. This list must stay
// small; prefer fixing real wiring over allowlisting. Classification of
// flag-audit findings (telemetry / future-reserved / save-legacy / external).

using System;
using System.Collections.Generic;

namespace Ashfall.Core.Narrative.Continuity
{
    public static class NarrativeContinuityAllowlist
    {
        /// <summary>
        /// Flags whose primary writer is a Core system (not authored narrative).
        /// Reads of these flags without an authored setter are classified as
        /// system/external-provided and demoted to allowlisted warnings.
        /// Keys are exact flag ids; keep the list reviewed and small.
        /// </summary>
        public static readonly IReadOnlySet<string> SystemProvidedFlags = new HashSet<string>(StringComparer.Ordinal)
        {
            // Owner: MicroLocationIntegration (F17–F20) — hazard routing flags are
            // written by the MicroLocationHazardRegistry consumer path, not authored JSON.
            "micro_contamination_exposure",
        };

        /// <summary>
        /// Prefixes owned by Core systems that write world flags at runtime.
        /// Authored reads of these prefixes without an authored setter are
        /// classified as system-provided (documented, non-error).
        /// </summary>
        public static readonly IReadOnlyList<string> SystemProvidedPrefixes = new[]
        {
            "micro_",          // Owner: MicroLocationIntegration (F17–F20)
        };

        /// <summary>
        /// Explicit structural exceptions (rule + node/file). Each must carry a
        /// reason and owner. Prefer inline authoring fixes instead.
        /// </summary>
        public static readonly IReadOnlyList<StructuralExemption> StructuralExemptions = Array.Empty<StructuralExemption>();

        public sealed class StructuralExemption
        {
            public string Rule { get; set; } = string.Empty;
            public string SourceFile { get; set; } = string.Empty;
            public string NodeId { get; set; } = string.Empty;
            public string Reason { get; set; } = string.Empty;
            public string Owner { get; set; } = string.Empty;
        }

        public static bool IsSystemProvided(string flagId)
        {
            if (string.IsNullOrEmpty(flagId)) return false;
            if (SystemProvidedFlags.Contains(flagId)) return true;
            foreach (var p in SystemProvidedPrefixes)
                if (flagId.StartsWith(p, StringComparison.Ordinal)) return true;
            return false;
        }
    }
}
