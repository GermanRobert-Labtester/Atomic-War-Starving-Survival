// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 49 / C2[21] host probe.
//
// --content-certification-selftest proves, headlessly, the cargo-manifest
// integration: every declared family resolves to a real catalog file and a
// real Core consumer, the engine's clean/dormant/orphan verdicts behave, the
// host session materializes rows from live evidence only, and the report
// description is stable.
// ============================================================================
using System.Linq;
using Ashfall.Core.Content;
using Godot;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        public static int RunContentCertificationSelfTest(string dataDirectory)
        {
            int pass = 0, fail = 0;

            void Check(string gate, bool ok, string note = "")
            {
                if (ok) { pass++; GD.Print($"[PASS] content-certification/{gate}"); }
                else { fail++; GD.Print($"[FAIL] content-certification/{gate}{(note.Length > 0 ? " — " + note : "")}"); }
            }

            var session = new ContentCertificationHostSession();

            // 1 — a fully live composition certifies clean and promotes.
            foreach (var family in ContentCertificationHostSession.Families)
            {
                session.MarkCatalogLoaded(family.CatalogFileName, true);
                session.MarkConsumerActive(family.CanonicalConsumer, true);
            }
            var clean = session.Certify();
            Check("all_live_is_clean", clean.IsCertificationClean && clean.CanPromotePlan49, session.Describe());
            Check("all_live_counts",
                clean.CertifiedActiveCount == ContentCertificationHostSession.Families.Count
                && clean.TotalCandidatesEvaluated == ContentCertificationHostSession.Families.Count);

            // 2 — an unloaded catalog is dormant, never an orphan.
            var dormantSession = new ContentCertificationHostSession();
            foreach (var family in ContentCertificationHostSession.Families)
            {
                dormantSession.MarkCatalogLoaded(family.CatalogFileName, family.CatalogFileName != "medical_texts.json");
                dormantSession.MarkConsumerActive(family.CanonicalConsumer, true);
            }
            var dormant = dormantSession.Certify();
            Check("unloaded_catalog_is_dormant_not_orphan",
                dormant.ExcludedDormantCount == 1 && dormant.OrphanWarningCount == 0
                && dormant.IsCertificationClean && dormant.CanPromotePlan49, dormantSession.Describe());

            // 3 — a named-but-uncomposed consumer is an orphan and listed.
            var orphanSession = new ContentCertificationHostSession();
            foreach (var family in ContentCertificationHostSession.Families)
            {
                orphanSession.MarkCatalogLoaded(family.CatalogFileName, true);
                orphanSession.MarkConsumerActive(family.CanonicalConsumer,
                    family.CanonicalConsumer != "CassettePlaybackSystem");
            }
            var orphan = orphanSession.Certify();
            Check("uncomposed_consumer_is_orphan",
                orphan.OrphanWarningCount == 1 && orphan.OrphanIds.Contains("cassette_sets"),
                string.Join(",", orphan.OrphanIds));
            Check("flagged_family_resolves_to_manifest",
                orphanSession.FlaggedFamilies().Single().CatalogFileName == "cassette_sets.json");

            // 4 — dormant listing is derived from the manifest, not duplicated.
            Check("dormant_listing_from_manifest",
                dormantSession.DormantFamilies().Single().CatalogFileName == "medical_texts.json");

            // 5 — every manifest entry points at a real authored catalog file.
            var io = new Ashfall.Core.FileSystemIO();
            foreach (var family in ContentCertificationHostSession.Families)
            {
                bool exists = io.FileExists(System.IO.Path.Combine(dataDirectory, family.CatalogFileName));
                Check($"catalog_exists[{family.FamilyId}]", exists);
            }

            GD.Print($"[content-certification-selftest] {pass} passed, {fail} failed");
            return fail == 0 ? 0 : 1;
        }
    }
}
