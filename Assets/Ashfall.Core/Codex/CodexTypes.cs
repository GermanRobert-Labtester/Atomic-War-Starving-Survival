// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Campaign;

namespace Ashfall.Core.Codex
{
    /// <summary>
    /// Major domain disciplines within the settlement Codex.
    /// </summary>
    public enum CodexCategory
    {
        Ecology = 0,
        Technology = 1,
        WastelandLore = 2,
        SurvivalOperations = 3,
        Factions = 4
    }

    /// <summary>
    /// Discovery state of an authored or emergent fact in the Codex.
    /// </summary>
    public enum CodexEntryState
    {
        Locked = 0,
        Studying = 1,
        Known = 2
    }

    /// <summary>
    /// Read-only projected knowledge entry in the settlement Codex.
    /// Pure functional projection: zero persistent save state.
    /// </summary>
    public sealed class CodexEntryProjection
    {
        public string EntryId { get; set; } = string.Empty;
        public CodexCategory Category { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public CodexEntryState State { get; set; }
        public int DayLearned { get; set; }
        public InformationConfidence Confidence { get; set; }
        public IReadOnlyList<CampaignProvenanceRecord> Provenance { get; set; } = Array.Empty<CampaignProvenanceRecord>();
        public IReadOnlyList<string> RelatedLocationIds { get; set; } = Array.Empty<string>();
        public IReadOnlyList<string> Tags { get; set; } = Array.Empty<string>();

        public override string ToString() =>
            $"[{Category}] {Title} ({State}, Day {DayLearned}, {Confidence})";
    }
}
