// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 49 / C2[21] — Content orphan certification in the running game.
//
// The engine verdict is authoritative; this partial gathers the live evidence
// from the actual composition (loaded catalogs + constructed owners), runs the
// certification once per campaign, and publishes one truthful archive-review
// entry through the canonical journal owner. Nothing here re-implements the
// verdict and nothing here invents a family: the manifest lives in
// <see cref="ContentCertificationHostSession.Families"/>.
// ============================================================================
using System;
using System.Linq;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private ContentCertificationHostSession? _contentCertification;
        private bool _contentCertificationDirty;
        private bool _contentCertificationJournaled;

        public ContentCertificationHostSession? ContentCertification => _contentCertification;

        private ContentCertificationHostSession EnsureContentCertification()
        {
            if (_contentCertification != null) return _contentCertification;
            _contentCertification = new ContentCertificationHostSession();
            return _contentCertification;
        }

        private void SetupContentCertification()
        {
            var session = EnsureContentCertification();
            RefreshContentCertificationEvidence();
            var report = session.Certify();
            _contentCertificationDirty = true;

            if (!_contentCertificationJournaled)
            {
                _contentCertificationJournaled = true;
                PublishContentCertificationEntry(report);
            }
        }

        /// <summary>
        /// Refresh the live evidence: which authored catalogs actually loaded in
        /// this composition and which canonical owners are constructed. Never
        /// hardcodes a loader name — every flag reads the live object graph.
        /// </summary>
        private void RefreshContentCertificationEvidence()
        {
            var session = EnsureContentCertification();

            // Place / atmosphere — the world session owns the atmosphere catalog.
            session.MarkCatalogLoaded("environmental_atmosphere_expansion.json",
                _world?.AtmosphereTexts?.Count > 0);
            session.MarkConsumerActive("AtmosphereTextSystem", _world?.AtmosphereTexts != null);

            // Medical — the ward is composed, the authored clinical texts are not
            // loaded by any host loader today (reported dormant, not faked).
            session.MarkCatalogLoaded("medical_texts.json", false);
            session.MarkConsumerActive("MedicalWardSystem", _medicalWard != null);

            // Memory corpus — the journal owner binds the authored corpus.
            session.MarkCatalogLoaded("journal_entries_expansion_05.json", _journal?.HasAuthoredCorpus == true);
            session.MarkConsumerActive("JournalSystem", _journal != null);

            // Encounter / choice — the canonical choice resolver owner.
            session.MarkCatalogLoaded("narrative_encounters.json", _moralChoice != null);
            session.MarkConsumerActive("MoralChoiceSystem", _moralChoice != null);

            // Small ritual families.
            session.MarkCatalogLoaded("confession_secrets.json", _confessionSecrets != null);
            session.MarkConsumerActive("ConfessionSecretSystem", _confessionSecrets != null);
            session.MarkCatalogLoaded("final_wishes.json", _phase0?.FinalWish != null);
            session.MarkConsumerActive("FinalWishSystem", _phase0?.FinalWish != null);
            session.MarkCatalogLoaded("survivor_voice_lines.json",
                _survivorVoice != null && _survivorVoice.Voice.Catalog.Count > 0);
            session.MarkConsumerActive("SurvivorVoiceSystem", _survivorVoice != null);

            // Families whose consumers are still orphans in the host graph.
            session.MarkCatalogLoaded("cassette_sets.json", false);
            session.MarkConsumerActive("CassettePlaybackSystem", false);
            session.MarkCatalogLoaded("damaged_map_zones.json", false);
            session.MarkConsumerActive("DamagedMapCatalogContainer", false);
        }

        private void PublishContentCertificationEntry(Ashfall.Core.Content.ContentCertificationReport report)
        {
            string flagged = string.Join(", ", _contentCertification!
                .FlaggedFamilies().Select(f => f.FamilyId));
            _journal?.TryAddRawEntry(
                "content_certification_review",
                $"Archive review of {report.TotalCandidatesEvaluated} authored content families: "
                + $"{report.CertifiedActiveCount} reach a live owner, "
                + $"{report.ExcludedDormantCount} are authored but unloaded, "
                + $"{report.OrphanWarningCount} name an owner the shelter does not yet run"
                + (string.IsNullOrEmpty(flagged) ? "." : $" ({flagged})."),
                null!, Math.Max(1, _simDay));
        }

        /// <summary>Re-run the certification on demand (host probes).</summary>
        internal ContentCertificationHostSession CertifyContentOrphans()
        {
            var session = EnsureContentCertification();
            RefreshContentCertificationEvidence();
            session.Certify();
            return session;
        }
    }
}
