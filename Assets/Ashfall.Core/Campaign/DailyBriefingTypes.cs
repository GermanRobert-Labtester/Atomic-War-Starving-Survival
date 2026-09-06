using System;
using System.Collections.Generic;

namespace Ashfall.Core.Campaign
{
    /// <summary>
    /// Strict classification taxonomy for dawn intelligence report facts.
    /// </summary>
    public enum BriefingTaxonomyClass
    {
        /// <summary>Immediate existential threats requiring urgent commander intervention.</summary>
        Critical = 0,

        /// <summary>Deteriorating infrastructure, resources, or environmental conditions.</summary>
        Warning = 1,

        /// <summary>Strategic opportunities, reconnaissance, decoded signals, or discoveries.</summary>
        Intel = 2,

        /// <summary>Diegetic settlement atmosphere and survivor morale observations.</summary>
        Flavor = 3
    }

    /// <summary>
    /// Discrete atomic intelligence fact produced by a settlement simulation system.
    /// </summary>
    [Serializable]
    public sealed class BriefingFact
    {
        public string FactId { get; set; } = string.Empty;
        public string ProducerId { get; set; } = string.Empty;
        public BriefingTaxonomyClass Taxonomy { get; set; }
        public float Severity { get; set; }
        public string? RelatedEntityId { get; set; }
        public string? DeepLinkRoute { get; set; }
        public string Message { get; set; } = string.Empty;
        public int Day { get; set; }
        public CampaignProvenanceRecord? Provenance { get; set; }

        public BriefingFact() { }

        public BriefingFact(
            string factId,
            string producerId,
            BriefingTaxonomyClass taxonomy,
            float severity,
            string message,
            int day,
            string? deepLinkRoute = null,
            string? relatedEntityId = null,
            CampaignProvenanceRecord? provenance = null)
        {
            FactId = factId ?? string.Empty;
            ProducerId = producerId ?? string.Empty;
            Taxonomy = taxonomy;
            Severity = Math.Clamp(severity, 0f, 100f);
            Message = message ?? string.Empty;
            Day = Math.Max(1, day);
            DeepLinkRoute = deepLinkRoute;
            RelatedEntityId = relatedEntityId;
            Provenance = provenance;
        }

        public override string ToString() =>
            $"[{Taxonomy} S:{Severity:F0}] {FactId}: {Message} (Link: {DeepLinkRoute ?? "None"})";
    }

    /// <summary>
    /// Common interface for systems that emit dawn briefing intelligence facts.
    /// </summary>
    public interface IBriefingFactCollector
    {
        IReadOnlyList<BriefingFact> CollectBriefingFacts(int day);
    }

    /// <summary>
    /// Serializable cadence tracking record for suppressing unchanged warnings across days.
    /// </summary>
    [Serializable]
    public sealed class DailyBriefingCadenceRecord
    {
        public string FactId = string.Empty;
        public float LastReportedSeverity;
        public int LastReportedDay;

        public DailyBriefingCadenceRecord() { }

        public DailyBriefingCadenceRecord(string factId, float severity, int day)
        {
            FactId = factId ?? string.Empty;
            LastReportedSeverity = severity;
            LastReportedDay = day;
        }

        public DailyBriefingCadenceRecord Clone() => new DailyBriefingCadenceRecord(FactId, LastReportedSeverity, LastReportedDay);
    }
}
