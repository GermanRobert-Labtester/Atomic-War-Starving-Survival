using System;
using System.Collections.Generic;

namespace Ashfall.Core.Content
{
    /// <summary>
    /// Certification status of an authored content row or candidate asset.
    /// </summary>
    public enum ContentCertificationStatus
    {
        CertifiedActive = 0,   // Composed canonical consumer verified; active in gameplay
        ExcludedDormant = 1,   // Authored and valid, but deliberately parked pending prerequisites
        OrphanWarning = 2      // Loaded in catalog but lacks a composed consumer or fallback
    }

    /// <summary>
    /// Authored content candidate row metadata.
    /// </summary>
    public readonly struct ContentCandidateRow
    {
        public string ContentId { get; }
        public string CatalogSource { get; }
        public string CanonicalConsumer { get; }
        public bool IsPrerequisiteMet { get; }

        public ContentCandidateRow(string contentId, string catalogSource, string canonicalConsumer, bool isPrerequisiteMet)
        {
            ContentId = contentId ?? string.Empty;
            CatalogSource = catalogSource ?? string.Empty;
            CanonicalConsumer = canonicalConsumer ?? string.Empty;
            IsPrerequisiteMet = isPrerequisiteMet;
        }
    }

    /// <summary>
    /// Immutable certification report produced by <see cref="ContentOrphanCertificationEngine"/>.
    /// </summary>
    public readonly struct ContentCertificationReport
    {
        public bool IsCertificationClean { get; }
        public int TotalCandidatesEvaluated { get; }
        public int CertifiedActiveCount { get; }
        public int ExcludedDormantCount { get; }
        public int OrphanWarningCount { get; }
        public bool CanPromotePlan49 { get; }
        public IReadOnlyList<string> OrphanIds { get; }

        public ContentCertificationReport(
            bool isCertificationClean,
            int totalCandidatesEvaluated,
            int certifiedActiveCount,
            int excludedDormantCount,
            int orphanWarningCount,
            bool canPromotePlan49,
            IReadOnlyList<string> orphanIds)
        {
            IsCertificationClean = isCertificationClean;
            TotalCandidatesEvaluated = totalCandidatesEvaluated;
            CertifiedActiveCount = certifiedActiveCount;
            ExcludedDormantCount = excludedDormantCount;
            OrphanWarningCount = orphanWarningCount;
            CanPromotePlan49 = canPromotePlan49;
            OrphanIds = orphanIds ?? Array.Empty<string>();
        }
    }

    /// <summary>
    /// Pure domain engine for certifying scanner content rows and verifying canonical consumers (Plan 49 / C1[16] / UNBLOCK-03).
    /// Prevents silent orphan leaks and validates prerequisite chains before content promotion.
    /// </summary>
    public static class ContentOrphanCertificationEngine
    {
        /// <summary>
        /// Certifies candidate content rows against known composed consumers.
        /// </summary>
        /// <param name="candidates">Collection of content candidate rows to audit.</param>
        /// <param name="activeConsumerRegistry">Set of currently composed, active consumer system names.</param>
        /// <returns>Immutable <see cref="ContentCertificationReport"/>.</returns>
        public static ContentCertificationReport Certify(
            IEnumerable<ContentCandidateRow> candidates,
            ISet<string> activeConsumerRegistry)
        {
            int total = 0;
            int active = 0;
            int dormant = 0;
            int orphans = 0;
            var orphanList = new List<string>();

            if (candidates != null)
            {
                foreach (var row in candidates)
                {
                    total++;

                    if (string.IsNullOrWhiteSpace(row.CanonicalConsumer))
                    {
                        orphans++;
                        orphanList.Add(row.ContentId);
                        continue;
                    }

                    bool consumerActive = activeConsumerRegistry != null && activeConsumerRegistry.Contains(row.CanonicalConsumer);

                    if (consumerActive && row.IsPrerequisiteMet)
                    {
                        active++;
                    }
                    else if (!row.IsPrerequisiteMet)
                    {
                        dormant++;
                    }
                    else
                    {
                        // Consumer named but not active in composition root
                        orphans++;
                        orphanList.Add(row.ContentId);
                    }
                }
            }

            bool clean = orphans == 0;
            bool canPromote = clean && active > 0;

            return new ContentCertificationReport(
                isCertificationClean: clean,
                totalCandidatesEvaluated: total,
                certifiedActiveCount: active,
                excludedDormantCount: dormant,
                orphanWarningCount: orphans,
                canPromotePlan49: canPromote,
                orphanIds: orphanList
            );
        }
    }
}
