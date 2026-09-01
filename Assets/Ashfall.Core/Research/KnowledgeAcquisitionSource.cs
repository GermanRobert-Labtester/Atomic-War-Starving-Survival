// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core
{
    public enum KnowledgeAcquisitionSourceType
    {
        DirectResearch,
        LibraryManualStudy,
        AutopsyPathologyFinding,
        WorkshopReverseEngineering,
        NarrativeEncounterReward,
        FieldScavengeDiscovery
    }

    /// <summary>
    /// Metadata record tracking how a specific research knowledge node was unlocked in the holdfast campaign.
    /// </summary>
    [Serializable]
    public sealed class KnowledgeAcquisitionEvent
    {
        public string KnowledgeId { get; set; } = string.Empty;
        public KnowledgeAcquisitionSourceType SourceType { get; set; }
        public string SourceIdentifier { get; set; } = string.Empty;
        public int DayAcquired { get; set; } = 1;
        public string ContextDetail { get; set; } = string.Empty;

        public KnowledgeAcquisitionEvent() { }

        public KnowledgeAcquisitionEvent(
            string knowledgeId,
            KnowledgeAcquisitionSourceType sourceType,
            string sourceIdentifier,
            int dayAcquired,
            string contextDetail = "")
        {
            KnowledgeId = knowledgeId;
            SourceType = sourceType;
            SourceIdentifier = sourceIdentifier;
            DayAcquired = dayAcquired;
            ContextDetail = contextDetail;
        }
    }
}
