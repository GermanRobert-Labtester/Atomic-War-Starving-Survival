// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : ShelterMuseumSelfTest
// Subsystem          : Plan 218 — Shelter Museum & Historical Archive
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Culture;

namespace AtomicWar.GodotApp
{
    public static class HostCliShelterMuseum
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Shelter Museum & Historical Archive Self-Test (Plan 218) ===");
            int passed = 0;
            const int total = 12;

            try
            {
                // Check 1: Catalog loading from museum_collection_templates.json
                string dataRoot = (!string.IsNullOrEmpty(dataDir) && Directory.Exists(dataDir) ? dataDir : CatalogPath.ResolveDataDir());
                string catPath = Path.Combine(dataRoot, "museum_collection_templates.json");

                var session = ShelterMuseumHostSession.Create();
                if (File.Exists(catPath))
                {
                    session.LoadCatalog(File.ReadAllText(catPath));
                }

                if (session.AuthoredTemplateCount == 6)
                {
                    Console.WriteLine($"[PASS] Check 1: Catalog loaded {session.AuthoredTemplateCount} artifact templates.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 1: Museum catalog failed to load (count={session.AuthoredTemplateCount}).");
                }

                // Check 2: Nonphysical template accession creates a museum record
                var artifact = session.AccessionTemplateRecord("artifact_founding_charter", "surv_curator", currentDay: 1);
                if (artifact != null && artifact.ArtifactId.StartsWith("art_", StringComparison.Ordinal)
                    && artifact.HistoricalSignificance >= 80f)
                {
                    Console.WriteLine("[PASS] Check 2: Template accession created a museum record (no inventory consumed).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 2: Template accession failed.");
                }

                // Check 3: Unknown template accession refused
                var refused = session.AccessionTemplateRecord("artifact_nonexistent", "surv_curator", currentDay: 1);
                if (refused == null)
                {
                    Console.WriteLine("[PASS] Check 3: Unknown template accession refused.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 3: Unknown template accession was not refused.");
                }

                // Check 4: Curator appointment recorded
                session.AppointCurator("surv_curator", currentDay: 2);
                if (session.System.CuratorId == "surv_curator")
                {
                    Console.WriteLine("[PASS] Check 4: Curator appointment recorded.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 4: Curator appointment failed.");
                }

                // Check 5: Exhibition curated and active
                var exhibition = session.CurateExhibition(
                    "Founding Ashfall", ExhibitionTheme.Founding,
                    new[] { artifact!.ArtifactId }, startDay: 3, durationDays: 3,
                    description: "The first days behind sealed doors.");
                if (exhibition != null && exhibition.Status == ExhibitionStatus.Active && exhibition.EndDay == 6)
                {
                    Console.WriteLine("[PASS] Check 5: Exhibition curated and active with correct end day.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 5: Exhibition curation failed.");
                }

                // Check 6: Explicit visit recorded with morale delta
                float? morale = session.Visit("surv_visitor", currentDay: 4);
                if (morale.HasValue && morale.Value >= 1f && session.System.TotalVisitors == 1)
                {
                    Console.WriteLine($"[PASS] Check 6: Visit recorded with morale delta (+{morale.Value:0.#}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 6: Visit failed (morale={morale}, visitors={session.System.TotalVisitors}).");
                }

                // Check 7: Same-day second visit refused (exactly-once per day)
                float? repeat = session.Visit("surv_visitor", currentDay: 4);
                if (repeat == null && session.System.TotalVisitors == 1)
                {
                    Console.WriteLine("[PASS] Check 7: Same-day second visit refused without mutation.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 7: Same-day repeat visit was not refused.");
                }

                // Check 8: Next-day visit allowed again
                float? nextDay = session.Visit("surv_visitor", currentDay: 5);
                if (nextDay.HasValue && session.System.TotalVisitors == 2)
                {
                    Console.WriteLine("[PASS] Check 8: Next-day visit allowed (once-per-day policy).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 8: Next-day visit failed.");
                }

                // Check 9: Daily expiry closes the exhibition exactly once
                session.TickDay(6);
                session.TickDay(7);
                var active = session.System.GetActiveExhibitions();
                if (exhibition != null && exhibition.Status == ExhibitionStatus.Completed && active.Count == 0)
                {
                    Console.WriteLine("[PASS] Check 9: Expired exhibition closed exactly once by the daily tick.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 9: Exhibition expiry failed.");
                }

                // Check 10: Save/restore round-trip preserves full museum state
                bool saved = session.TrySave();
                var reloaded = ShelterMuseumHostSession.Create();
                bool loaded = reloaded.TryLoad();
                var snap = reloaded.GetSnapshot();
                if (saved && loaded && snap.ArtifactCount == 1 && snap.TotalVisitors == 2
                    && snap.CuratorId == "surv_curator" && snap.RecentEvents.Count >= 3)
                {
                    Console.WriteLine("[PASS] Check 10: Save/restore round-trip preserved collection, curator, visits, and events.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 10: Save/restore failed (saved={saved}, loaded={loaded}, artifacts={snap.ArtifactCount}).");
                }

                // Check 11: Visit ledger survives restore (still once-per-day after reload)
                float? afterRestore = reloaded.Visit("surv_visitor", currentDay: 5);
                if (afterRestore == null)
                {
                    Console.WriteLine("[PASS] Check 11: Visit ledger survived restore (no same-day double visit).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 11: Visit ledger lost across restore.");
                }

                // Check 12: Save store contract names
                bool shelterMuseumContractOk = ShelterMuseumSaveStore.SectionName == "shelter_museum"
                    && ShelterMuseumSaveStore.FileName == "shelter_museum_save.json";
                if (shelterMuseumContractOk)
                {
                    Console.WriteLine("[PASS] Check 12: Save store contract names verified.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 12: Save store contract names mismatch.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Self-test crashed: {ex.Message}");
            }

            Console.WriteLine($"=== Shelter Museum Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
