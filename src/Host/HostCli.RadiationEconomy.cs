// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : RadiationEconomySelfTest
// Subsystem          : Radiation Economy Bridge
// ============================================================================

using System;
using Ashfall.Core.Radiation;

namespace AtomicWar.GodotApp
{
    public static class HostCliRadiationEconomy
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Radiation Economy Self-Test ===");
            int passed = 0;
            const int total = 7;

            try
            {
                string data = string.IsNullOrWhiteSpace(dataDir) ? CatalogPath.ResolveDataDir() : dataDir;
                var session = RadiationEconomyHostSession.Create();
                bool catalog = session.LoadCatalog(data);

                if (catalog && session.Bridge.Rules.Count > 0)
                {
                    Console.WriteLine($"[PASS] Check 1: {session.Bridge.Rules.Count} contamination trade rules loaded.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 1: Catalog not ready ({catalog}).");
                }

                var low = session.EvaluateTrade("canned_food", "food_water", 10, 100f);
                if (!low.isBlocked && Math.Abs(low.priceMultiplier - 0.70f) < 0.001f && Math.Abs(low.finalPrice - 70f) < 0.01f)
                {
                    Console.WriteLine($"[PASS] Check 2: Tainted food priced x{low.priceMultiplier:0.00} -> {low.finalPrice:0}.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 2: Tainted food mismatch (blocked={low.isBlocked}, x{low.priceMultiplier}).");
                }

                var lethal = session.EvaluateTrade("canned_food", "food_water", 85, 100f);
                if (lethal.isBlocked && !string.IsNullOrEmpty(lethal.blockReason))
                {
                    Console.WriteLine($"[PASS] Check 3: Lethal-dose food blocked ({lethal.blockReason}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 3: Lethal-dose food was not blocked.");
                }

                var gamma = session.EvaluateTrade("filter", "equipment", 90, 100f);
                if (gamma.isBlocked && Math.Abs(gamma.priceMultiplier - 0.20f) < 0.001f)
                {
                    Console.WriteLine("[PASS] Check 4: Gamma-emitting gear blocked at x0.20.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 4: Gamma gear mismatch (blocked={gamma.isBlocked}, x{gamma.priceMultiplier}).");
                }

                var clean = session.EvaluateTrade("scrap", "unknown_category", 50, 100f);
                if (!clean.isBlocked && Math.Abs(clean.priceMultiplier - 1.0f) < 0.001f)
                {
                    Console.WriteLine("[PASS] Check 5: Unknown category defaults to x1.00 and unblocked.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 5: Unknown-category default mismatch (x{clean.priceMultiplier}).");
                }

                if (session.Bridge.TotalEvaluations == 4 && session.Bridge.TotalBlockedTrades == 2)
                {
                    Console.WriteLine("[PASS] Check 6: Evaluation ledger counted 4 evaluations / 2 blocks.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 6: Ledger mismatch ({session.Bridge.TotalEvaluations}/{session.Bridge.TotalBlockedTrades}).");
                }

                bool saved = session.TrySave();
                var reloaded = RadiationEconomyHostSession.Create();
                bool loaded = reloaded.TryLoad();
                if (saved && loaded && reloaded.Bridge.TotalEvaluations == session.Bridge.TotalEvaluations
                    && RadiationEconomySaveStore.SectionName.Equals("radiation_economy", StringComparison.Ordinal)
                    && RadiationEconomySaveStore.FileName.Equals("radiation_economy_save.json", StringComparison.Ordinal))
                {
                    Console.WriteLine("[PASS] Check 7: Save/restore and store contract verified.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 7: Save/restore failed (saved={saved}, loaded={loaded}).");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Self-test crashed: {ex.Message}");
            }

            Console.WriteLine($"=== Radiation Economy Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
