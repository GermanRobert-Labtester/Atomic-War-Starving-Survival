// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : WaterQualityProfileSelfTest
// Subsystem          : Water source purity profiling
// ============================================================================

using System;
using Ashfall.Core;
using Ashfall.Core.Water;

namespace AtomicWar.GodotApp
{
    public static class HostCliWaterQualityProfile
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Water Quality Profile Self-Test ===");
            int passed = 0;
            const int total = 7;

            try
            {
                var session = WaterQualityProfileHostSession.Create();

                if (session.SourceCount == 0)
                {
                    Console.WriteLine("[PASS] Check 1: Water quality ledger starts empty.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 1: Ledger was not empty.");
                }

                // Raw runoff carries the heaviest contaminant load and highest health risk.
                var raw = session.AssaySource("sump_collector", WaterSourcePurityTier.RawSurfaceRunoff, 0, 1);
                int rawRisk = WaterQualityProfileEngine.CalculateHealthRiskPermille(
                    WaterQualityProfileEngine.EvaluateSourceContamination(WaterSourcePurityTier.RawSurfaceRunoff, 0));
                if (raw != null && raw.resultTier == WaterSourcePurityTier.RawSurfaceRunoff
                    && rawRisk > 500 && session.UnsafeSourceCount == 1)
                {
                    Console.WriteLine($"[PASS] Check 2: Raw runoff is unsafe (risk {rawRisk} permille).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 2: Raw runoff verdict wrong (tier {raw?.resultTier.ToString() ?? "none"}, risk {rawRisk}).");
                }

                // Deep aquifer is potable straight from the source.
                var deep = session.AssaySource("deep_well_a", WaterSourcePurityTier.DeepAquifer, 0, 1);
                if (deep.resultTier == WaterSourcePurityTier.DeepAquifer
                    && session.UnsafeSourceCount == 1)
                {
                    Console.WriteLine("[PASS] Check 3: Deep aquifer is potable without treatment.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 3: Deep aquifer verdict {deep.resultTier}.");
                }

                // Charcoal filtration upgrades the runoff tier.
                var yield = session.TreatSource("sump_collector", TreatmentMode.CharcoalFiltration, 1000, 2);
                if (yield.ResultingPurityTier == WaterSourcePurityTier.CharcoalFiltered
                    && session.UnsafeSourceCount == 0)
                {
                    Console.WriteLine("[PASS] Check 4: Charcoal filtration upgrades raw runoff.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 4: Treatment tier {yield.ResultingPurityTier}, unsafe {session.UnsafeSourceCount}.");
                }

                // Filter integrity collapses the treatment yield and raises pathogen risk.
                var worn = session.TreatSource("sump_collector", TreatmentMode.CharcoalFiltration, 100, 3);
                if (worn.PathogenRiskPermille > yield.PathogenRiskPermille
                    && worn.PathogenRiskPermille == 350)
                {
                    Console.WriteLine($"[PASS] Check 5: Worn filters raise pathogen risk ({yield.PathogenRiskPermille} -> {worn.PathogenRiskPermille}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 5: Worn filter behaviour unchanged.");
                }

                // Flood contamination must not lower the derived risk.
                int baseRisk = WaterQualityProfileEngine.CalculateHealthRiskPermille(
                    WaterQualityProfileEngine.EvaluateSourceContamination(WaterSourcePurityTier.DeepAquifer, 0));
                int floodRisk = WaterQualityProfileEngine.CalculateHealthRiskPermille(
                    WaterQualityProfileEngine.EvaluateSourceContamination(WaterSourcePurityTier.DeepAquifer, 1000));
                if (floodRisk >= baseRisk)
                {
                    Console.WriteLine($"[PASS] Check 6: Flood contamination never reduces health risk ({baseRisk} -> {floodRisk}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 6: Flood lowered risk ({baseRisk} -> {floodRisk}).");
                }

                bool saved = session.TrySave();
                var reloaded = WaterQualityProfileHostSession.Create();
                bool loaded = reloaded.TryLoad();
                if (saved && loaded && reloaded.SourceCount == 2
                    && reloaded.Sources[0].filterWearPermille > 0
                    && WaterQualityProfileSaveStore.SectionName.Equals("water_quality_profile", StringComparison.Ordinal)
                    && WaterQualityProfileSaveStore.FileName.Equals("water_quality_profile_save.json", StringComparison.Ordinal))
                {
                    Console.WriteLine("[PASS] Check 7: Save/restore and store contract verified.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 7: Save/restore failed (saved={saved}, loaded={loaded}, n={reloaded.SourceCount}).");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Self-test crashed: {ex.Message}");
            }

            Console.WriteLine($"=== Water Quality Profile Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
