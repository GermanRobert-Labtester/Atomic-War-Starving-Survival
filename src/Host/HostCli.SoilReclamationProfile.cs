// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : SoilReclamationProfileSelfTest
// Subsystem          : Soil Reclamation (Expansion 15 — The Deep Root)
// ============================================================================

using System;
using Ashfall.Core.Farming;

namespace AtomicWar.GodotApp
{
    public static class HostCliSoilReclamationProfile
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Soil Reclamation Profile Self-Test ===");
            int passed = 0;
            const int total = 7;

            try
            {
                var session = SoilReclamationProfileHostSession.Create();

                if (session.PlotCount == 0)
                {
                    Console.WriteLine("[PASS] Check 1: Soil ledger starts empty.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 1: Ledger was not empty.");
                }

                // Barren ash: heavy radionuclides, zero humus, wide pH deviation.
                var plot = session.RegisterPlot("plot_a", 700, 900, 20, 40);
                if (plot.qualityTier == SoilQualityTier.BarrenAsh
                    && !plot.isCultivable
                    && session.BarrenPlotCount == 1)
                {
                    Console.WriteLine($"[PASS] Check 2: Contaminated plot is barren and not cultivable.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 2: tier {plot.qualityTier}, cultivable {plot.isCultivable}.");
                }

                // Compost mulch must add humus and improve germination viability.
                var barrenBefore = session.FindPlot("plot_a")!;
                int germinationBefore = barrenBefore.germinationViabilityPermille;
                var compost = session.ApplyAmendment("plot_a", SoilAmendment.CompostMulch, 10, cycleCount: 3);
                if (plot.organicMatterPermille > 20
                    && compost.GerminationViabilityPermille > germinationBefore
                    && compost.NetOrganicMatterPermille == 1000)
                {
                    Console.WriteLine($"[PASS] Check 3: Compost mulch saturates humus ({plot.organicMatterPermille} permille) and lifts viability ({germinationBefore} -> {compost.GerminationViabilityPermille}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 3: humus {plot.organicMatterPermille}, viability {germinationBefore} -> {compost.GerminationViabilityPermille}.");
                }

                // Leaching flush must remove soluble salts without touching radionuclides.
                session.RegisterPlot("plot_b", 800, 400, 500, 50);
                var saltBefore = session.FindPlot("plot_b")!.salinityPermille;
                var radBefore = session.FindPlot("plot_b")!.radionuclideLoadPermille;
                var flushed = session.ApplyAmendment("plot_b", SoilAmendment.LeachingFlush, 5, cycleCount: 1);
                int saltAfter = session.FindPlot("plot_b")!.salinityPermille;
                if (saltAfter < saltBefore
                    && session.FindPlot("plot_b")!.radionuclideLoadPermille == radBefore
                    && flushed.NetSalinityPermille == 200)
                {
                    Console.WriteLine($"[PASS] Check 4: Leaching flush cuts salt only ({saltBefore} -> {flushed.NetSalinityPermille}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 4: salt {saltBefore} -> {saltAfter}.");
                }

                // Biochar must bind radionuclides and lower mutation risk.
                session.RegisterPlot("plot_c", 100, 800, 400, 65);
                var mutationBefore = session.FindPlot("plot_c")!.cropMutationRiskPermille;
                var biochar = session.ApplyAmendment("plot_c", SoilAmendment.BiocharAdsorbent, 10);
                int mutationAfter = session.FindPlot("plot_c")!.cropMutationRiskPermille;
                if (biochar.NetRadionuclideLoadPermille == 0
                    && mutationAfter < mutationBefore)
                {
                    Console.WriteLine($"[PASS] Check 5: Biochar adsorbs isotopes and lowers mutation risk.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 5: rad {biochar.NetRadionuclideLoadPermille}, risk {mutationBefore} -> {mutationAfter}.");
                }

                // Repeated amendment cycles must eventually reach cultivable topsoil.
                session.RegisterPlot("plot_d", 100, 50, 400, 65);
                for (int i = 0; i < 20; i++)
                    session.ApplyAmendment("plot_d", SoilAmendment.CompostMulch, 10, cycleCount: 1);
                var mature = session.FindPlot("plot_d")!;
                if (mature.isCultivable && mature.qualityTier != SoilQualityTier.BarrenAsh
                    && session.CultivablePlotCount >= 1)
                {
                    Console.WriteLine($"[PASS] Check 6: Sustained amendment reaches {mature.qualityTier} (yield {mature.yieldMultiplierPermille} permille).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 6: tier {mature.qualityTier}, cultivable {mature.isCultivable}.");
                }

                session.AdvanceDay(30);
                bool saved = session.TrySave();
                var reloaded = SoilReclamationProfileHostSession.Create();
                bool loaded = reloaded.TryLoad();
                if (saved && loaded
                    && reloaded.PlotCount == session.PlotCount
                    && reloaded.FindPlot("plot_d")!.cycleCount >= 20
                    && SoilReclamationProfileSaveStore.SectionName.Equals("soil_reclamation_profile", StringComparison.Ordinal)
                    && SoilReclamationProfileSaveStore.FileName.Equals("soil_reclamation_profile_save.json", StringComparison.Ordinal))
                {
                    Console.WriteLine("[PASS] Check 7: Save/restore and store contract verified.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 7: saved={saved}, loaded={loaded}, plots={reloaded.PlotCount}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Self-test crashed: {ex.Message}");
            }

            Console.WriteLine($"=== Soil Reclamation Profile Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
