// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Campaign
{
    /// <summary>
    /// Authoritative origin category for any piece of campaign knowledge or intelligence.
    /// Invariant: Localized strings must never be used as source authority.
    /// </summary>
    public enum KnowledgeSourceKind
    {
        ExpeditionVisit = 0,
        ExpeditionSurvey = 1,
        RadioIntercept = 2,
        TraderRumor = 3,
        FieldGuide = 4,
        Research = 5,
        JournalEvidence = 6,
        Manual = 7,
        Autopsy = 8,
        FactionMuster = 9,
        TreatyEvent = 10,
        WorldEvent = 11,
        ShelterSystem = 12,
        NarrativeArticle = 13
    }

    /// <summary>
    /// Reliability / certainty classification of strategic information.
    /// </summary>
    public enum InformationConfidence
    {
        /// <summary>Unconfirmed hearsay, distant signal, or unverified gossip.</summary>
        Low = 0,

        /// <summary>Corroborated rumor or partially triangulated radio intercept.</summary>
        Medium = 1,

        /// <summary>Technical survey, sensor telemetry, or documented field notes.</summary>
        High = 2,

        /// <summary>Physical on-site observation, completed scientific research, or direct contact.</summary>
        Confirmed = 3
    }

    /// <summary>
    /// Persistent, serializable record of how and when a strategic fact was learned.
    /// </summary>
    [Serializable]
    public sealed class CampaignProvenanceRecord
    {
        public KnowledgeSourceKind SourceKind { get; set; }
        public string SourceId { get; set; } = string.Empty;
        public string ProducerSystemId { get; set; } = string.Empty;
        public int DayObserved { get; set; }
        public InformationConfidence Confidence { get; set; }
        public string? RelatedEntityId { get; set; }

        public CampaignProvenanceRecord() { }

        public CampaignProvenanceRecord(
            KnowledgeSourceKind sourceKind,
            string sourceId,
            string producerSystemId,
            int dayObserved,
            InformationConfidence confidence,
            string? relatedEntityId = null)
        {
            SourceKind = sourceKind;
            SourceId = sourceId ?? string.Empty;
            ProducerSystemId = producerSystemId ?? string.Empty;
            DayObserved = Math.Max(0, dayObserved);
            Confidence = confidence;
            RelatedEntityId = relatedEntityId;
        }

        public CampaignProvenanceRecord Clone() => new CampaignProvenanceRecord
        {
            SourceKind = SourceKind,
            SourceId = SourceId,
            ProducerSystemId = ProducerSystemId,
            DayObserved = DayObserved,
            Confidence = Confidence,
            RelatedEntityId = RelatedEntityId
        };

        public override string ToString() =>
            $"[{SourceKind}:{SourceId}] by {ProducerSystemId} on Day {DayObserved} (Confidence: {Confidence})";
    }

    /// <summary>
    /// Pure functional utilities for merging and evaluating provenance chains deterministically.
    /// </summary>
    public static class CampaignProvenanceEvaluator
    {
        /// <summary>
        /// Merges two or more provenance collections into an ordered, deduplicated list.
        /// Retains distinct (SourceKind, SourceId, ProducerSystemId) records.
        /// </summary>
        public static List<CampaignProvenanceRecord> Merge(
            IEnumerable<CampaignProvenanceRecord>? primary,
            IEnumerable<CampaignProvenanceRecord>? secondary)
        {
            var combined = new List<CampaignProvenanceRecord>();
            if (primary != null) combined.AddRange(primary);
            if (secondary != null) combined.AddRange(secondary);

            var deduped = new Dictionary<string, CampaignProvenanceRecord>(StringComparer.Ordinal);
            foreach (var rec in combined)
            {
                if (rec == null) continue;
                string key = $"{rec.SourceKind}|{rec.SourceId}|{rec.ProducerSystemId}|{rec.RelatedEntityId ?? string.Empty}";
                if (deduped.TryGetValue(key, out var existing))
                {
                    // Update to earlier discovery day and highest confidence
                    if (rec.DayObserved < existing.DayObserved) existing.DayObserved = rec.DayObserved;
                    if (rec.Confidence > existing.Confidence) existing.Confidence = rec.Confidence;
                }
                else
                {
                    deduped[key] = rec.Clone();
                }
            }

            var result = deduped.Values.ToList();
            result.Sort(StableProvenanceSort);
            return result;
        }

        public static int EarliestDay(IEnumerable<CampaignProvenanceRecord>? records, int defaultDay = 0)
        {
            if (records == null) return defaultDay;
            int earliest = int.MaxValue;
            foreach (var r in records)
            {
                if (r != null && r.DayObserved < earliest)
                    earliest = r.DayObserved;
            }
            return earliest == int.MaxValue ? defaultDay : earliest;
        }

        public static InformationConfidence HighestConfidence(
            IEnumerable<CampaignProvenanceRecord>? records,
            InformationConfidence defaultConfidence = InformationConfidence.Low)
        {
            if (records == null) return defaultConfidence;
            InformationConfidence highest = defaultConfidence;
            foreach (var r in records)
            {
                if (r != null && r.Confidence > highest)
                    highest = r.Confidence;
            }
            return highest;
        }

        private static int StableProvenanceSort(CampaignProvenanceRecord a, CampaignProvenanceRecord b)
        {
            int c = a.SourceKind.CompareTo(b.SourceKind);
            if (c != 0) return c;
            c = a.DayObserved.CompareTo(b.DayObserved);
            if (c != 0) return c;
            c = string.CompareOrdinal(a.SourceId, b.SourceId);
            if (c != 0) return c;
            return string.CompareOrdinal(a.ProducerSystemId, b.ProducerSystemId);
        }
    }
}
