// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 49 / C2[21] — Content orphan certification host adapter.
//
// Authority boundary (deliberate): ContentOrphanCertificationEngine remains
// the sole verdict authority (clean / certified-active / dormant / orphan and
// the Plan 49 promotion gate). This session only assembles the evidence the
// engine consumes from the LIVE composition:
//   * prerequisite met  -> the authored catalog was actually loaded this run,
//   * consumer active   -> the canonical owner is actually constructed.
//
// The family table below is the plan's cargo manifest (the authored clusters):
//   place/atmosphere, medical, memory corpus, encounter/choice and the small
//   ritual families (confessions, final wishes, voice, cassettes, map zones).
// Every entry names a real catalog file and a real Core consumer, so the
// report is evidence, not an assertion. Families whose consumer is still an
// orphan (e.g. the cassette authority) are reported as orphans instead of
// being quietly dropped — that is the plan's cargo list.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Content;

namespace AtomicWar.GodotApp
{
    /// <summary>One authored content family and its canonical consumer.</summary>
    public sealed class ContentCertificationFamily
    {
        public ContentCertificationFamily(string familyId, string cluster, string catalogFileName, string canonicalConsumer)
        {
            FamilyId = familyId;
            Cluster = cluster;
            CatalogFileName = catalogFileName;
            CanonicalConsumer = canonicalConsumer;
        }

        public string FamilyId { get; }
        public string Cluster { get; }
        public string CatalogFileName { get; }
        public string CanonicalConsumer { get; }
    }

    public sealed class ContentCertificationHostSession : HostSessionBase
    {
        /// <summary>
        /// The Plan 49 cargo manifest. Cluster names come from the plan's source
        /// baseline; each catalog file and consumer is verified against source
        /// and data by the integration tests.
        /// </summary>
        public static readonly IReadOnlyList<ContentCertificationFamily> Families = new[]
        {
            new ContentCertificationFamily("place_atmosphere_texts", "PLACE / ATMOSPHERE", "environmental_atmosphere_expansion.json", "AtmosphereTextSystem"),
            new ContentCertificationFamily("medical_clinical_texts", "MEDICAL / CLINICAL", "medical_texts.json", "MedicalWardSystem"),
            new ContentCertificationFamily("memory_corpus_journal", "MEMORY CORPUS", "journal_entries_expansion_05.json", "JournalSystem"),
            new ContentCertificationFamily("encounter_choice_prose", "ENCOUNTER / CHOICE", "narrative_encounters.json", "MoralChoiceSystem"),
            new ContentCertificationFamily("confession_ritual_records", "SMALL RITUAL / COLLECTION / TRADE", "confession_secrets.json", "ConfessionSecretSystem"),
            new ContentCertificationFamily("final_wish_records", "SMALL RITUAL / COLLECTION / TRADE", "final_wishes.json", "FinalWishSystem"),
            new ContentCertificationFamily("survivor_voice_lines", "PLAN 42 VOICE DELIVERY", "survivor_voice_lines.json", "SurvivorVoiceSystem"),
            new ContentCertificationFamily("cassette_sets", "SMALL RITUAL / COLLECTION / TRADE", "cassette_sets.json", "CassettePlaybackSystem"),
            new ContentCertificationFamily("damaged_map_zones", "SMALL RITUAL / COLLECTION / TRADE", "damaged_map_zones.json", "DamagedMapCatalogContainer")
        };

        private readonly Dictionary<string, bool> _catalogLoaded = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _activeConsumers = new(StringComparer.Ordinal);

        public ContentCertificationReport LastReport { get; private set; }

        public bool HasReport => LastReport.TotalCandidatesEvaluated > 0;

        /// <summary>Record whether an authored catalog actually loaded this run.</summary>
        public void MarkCatalogLoaded(string catalogFileName, bool loaded)
        {
            if (string.IsNullOrWhiteSpace(catalogFileName)) return;
            _catalogLoaded[catalogFileName] = loaded;
            RaiseStateChanged();
        }

        /// <summary>Record whether the canonical consumer is composed right now.</summary>
        public void MarkConsumerActive(string canonicalConsumer, bool active)
        {
            if (string.IsNullOrWhiteSpace(canonicalConsumer)) return;
            if (active) _activeConsumers.Add(canonicalConsumer);
            else _activeConsumers.Remove(canonicalConsumer);
            RaiseStateChanged();
        }

        public bool IsCatalogLoaded(string catalogFileName)
            => !string.IsNullOrWhiteSpace(catalogFileName)
               && _catalogLoaded.TryGetValue(catalogFileName, out bool loaded) && loaded;

        public bool IsConsumerActive(string canonicalConsumer)
            => !string.IsNullOrWhiteSpace(canonicalConsumer) && _activeConsumers.Contains(canonicalConsumer);

        /// <summary>
        /// Run the certification engine over the live evidence. Pure verdict
        /// stays in Core; this only materializes the candidate rows.
        /// </summary>
        public ContentCertificationReport Certify()
        {
            var rows = new List<ContentCandidateRow>(Families.Count);
            foreach (var family in Families)
            {
                rows.Add(new ContentCandidateRow(
                    family.FamilyId,
                    family.CatalogFileName,
                    family.CanonicalConsumer,
                    IsCatalogLoaded(family.CatalogFileName)));
            }

            LastReport = ContentOrphanCertificationEngine.Certify(rows, _activeConsumers);
            RaiseStateChanged();
            return LastReport;
        }

        /// <summary>Families the engine flagged, for host logging and probes.</summary>
        public IEnumerable<ContentCertificationFamily> FlaggedFamilies()
        {
            if (!HasReport) return Array.Empty<ContentCertificationFamily>();
            var flagged = new HashSet<string>(LastReport.OrphanIds, StringComparer.Ordinal);
            return Families.Where(f => flagged.Contains(f.FamilyId));
        }

        /// <summary>Families excluded as dormant (catalog exists, content not loaded).</summary>
        public IEnumerable<ContentCertificationFamily> DormantFamilies()
            => Families.Where(f => !IsCatalogLoaded(f.CatalogFileName));

        public string Describe()
        {
            if (!HasReport) return "content certification: not run";
            return $"content certification: {LastReport.TotalCandidatesEvaluated} families, "
                + $"{LastReport.CertifiedActiveCount} certified active, "
                + $"{LastReport.ExcludedDormantCount} dormant, "
                + $"{LastReport.OrphanWarningCount} orphan "
                + $"(promote: {(LastReport.CanPromotePlan49 ? "yes" : "no")})";
        }
    }
}
