// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : ProceduralEulogySelfTest
// Subsystem          : Procedural Eulogies
// ============================================================================

using System;
using Ashfall.Core.Journal;

namespace AtomicWar.GodotApp
{
    public static class HostCliProceduralEulogy
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Procedural Eulogy Self-Test ===");
            int passed = 0;
            const int total = 6;

            try
            {
                var session = ProceduralEulogyHostSession.Create();

                if (session.ArchivedCount == 0)
                {
                    Console.WriteLine("[PASS] Check 1: Eulogy archive starts empty.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 1: Archive was not empty.");
                }

                var life = new DwellerLifeRecord
                {
                    dwellerId = "surv_a",
                    dwellerName = "Marta Vell",
                    preWarProfession = "field nurse",
                    daysSurvived = 412,
                    mealsPrepared = 1180,
                    shiftsCompleted = 640,
                    radDoseAbsorbedMsv = 86,
                    causeOfDeath = "acute radiation sickness",
                    favoriteRelicName = "chipped enamel kettle"
                };

                string eulogy = session.ComposeEulogy(life);
                if (!string.IsNullOrWhiteSpace(eulogy) && eulogy.Length > 20)
                {
                    Console.WriteLine("[PASS] Check 2: Eulogy composed from the dweller life record.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 2: Eulogy text was empty or too short.");
                }

                if (session.ArchivedCount == 1 && eulogy == session.GetMostRecentEulogy())
                {
                    Console.WriteLine("[PASS] Check 3: Composed eulogy archived and retrievable.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 3: Archive mismatch ({session.ArchivedCount}).");
                }

                // Determinism: identical life records compose identical text.
                var again = ProceduralEulogyHostSession.Create();
                string second = again.ComposeEulogy(life);
                if (second == eulogy)
                {
                    Console.WriteLine("[PASS] Check 4: Composition is deterministic for identical input.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 4: Composition is non-deterministic.");
                }

                var shortLife = new DwellerLifeRecord
                {
                    dwellerId = "surv_b",
                    dwellerName = "Unnamed Drifter",
                    daysSurvived = 3,
                    causeOfDeath = "exposure"
                };
                string third = session.ComposeEulogy(shortLife);
                if (!string.IsNullOrWhiteSpace(third) && session.ArchivedCount == 2)
                {
                    Console.WriteLine("[PASS] Check 5: Sparse life records still compose and archive.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 5: Sparse record archive mismatch ({session.ArchivedCount}).");
                }

                bool saved = session.TrySave();
                var reloaded = ProceduralEulogyHostSession.Create();
                bool loaded = reloaded.TryLoad();
                if (saved && loaded && reloaded.ArchivedCount == 2
                    && ProceduralEulogySaveStore.SectionName.Equals("procedural_eulogy", StringComparison.Ordinal)
                    && ProceduralEulogySaveStore.FileName.Equals("procedural_eulogy_save.json", StringComparison.Ordinal))
                {
                    Console.WriteLine("[PASS] Check 6: Save/restore and store contract verified.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 6: Save/restore failed (saved={saved}, loaded={loaded}).");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Self-test crashed: {ex.Message}");
            }

            Console.WriteLine($"=== Procedural Eulogy Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
