// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : ProstheticConditionWearSelfTest
// Core Authority     : Ashfall.Core.Medical.ProstheticConditionWearEngine (F14-D)
// Purpose            : deterministic prosthetic condition wear / efficiency / risk
// ============================================================================

using System;
using Ashfall.Core.Medical;

namespace AtomicWar.GodotApp
{
    public static class HostCliProstheticWear
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Prosthetic Condition & Wear Self-Test (F14-D) ===");
            int passed = 0;
            const int total = 8;

            try
            {
                var simple = ProstheticConditionWearEngine.EvaluateDailyWear(
                    currentConditionPermille: 900,
                    complexity: ProstheticComplexityClass.SimpleImprovised,
                    laborIntensityPermille: 300,
                    maintenanceQualityPermille: 800);
                if (simple.WearDeltaPermille <= 10 && simple.BiomechanicalEfficiencyPermille == 600
                    && simple.FailureRiskPermille == 0 && !simple.RequiresImmediateMaintenance
                    && simple.MaintenanceStatus.Contains("Optimal alignment") && simple.NetConditionPermille == 894)
                {
                    Console.WriteLine("[PASS] Check 1: a well-maintained simple prosthetic wears slowly and caps at 600 efficiency.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 1: simple = wear {simple.WearDeltaPermille}, eff {simple.BiomechanicalEfficiencyPermille}."); }

                var heavy = ProstheticConditionWearEngine.EvaluateDailyWear(
                    currentConditionPermille: 800,
                    complexity: ProstheticComplexityClass.AdvancedArticulated,
                    laborIntensityPermille: 900,
                    maintenanceQualityPermille: 100);
                if (heavy.WearDeltaPermille >= 50 && heavy.BiomechanicalEfficiencyPermille > 700 && heavy.FailureRiskPermille == 0)
                {
                    Console.WriteLine($"[PASS] Check 2: heavy labor on a poorly-serviced advanced limb wears {heavy.WearDeltaPermille} permille.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 2: heavy = wear {heavy.WearDeltaPermille}, eff {heavy.BiomechanicalEfficiencyPermille}."); }

                var critical = ProstheticConditionWearEngine.EvaluateDailyWear(
                    currentConditionPermille: 150,
                    complexity: ProstheticComplexityClass.StandardMechanical,
                    laborIntensityPermille: 500,
                    maintenanceQualityPermille: 0);
                if (critical.FailureRiskPermille > 500 && critical.RequiresImmediateMaintenance
                    && critical.BiomechanicalEfficiencyPermille < 200
                    && critical.MaintenanceStatus.Contains("Imminent mechanical breakdown"))
                {
                    Console.WriteLine($"[PASS] Check 3: a critical standard prosthetic flags failure risk {critical.FailureRiskPermille} and immediate service.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 3: critical = risk {critical.FailureRiskPermille}, eff {critical.BiomechanicalEfficiencyPermille}."); }

                var broken = ProstheticConditionWearEngine.EvaluateDailyWear(
                    currentConditionPermille: 0,
                    complexity: ProstheticComplexityClass.AdvancedArticulated,
                    laborIntensityPermille: 500,
                    maintenanceQualityPermille: 500);
                if (broken.NetConditionPermille == 0 && broken.BiomechanicalEfficiencyPermille == 0
                    && broken.FailureRiskPermille == 1000 && broken.RequiresImmediateMaintenance)
                {
                    Console.WriteLine("[PASS] Check 4: zero condition yields zero efficiency and maximum (1000) failure risk.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 4: broken = net {broken.NetConditionPermille}, risk {broken.FailureRiskPermille}."); }

                var clamped = ProstheticConditionWearEngine.EvaluateDailyWear(-500, ProstheticComplexityClass.SimpleImprovised, -100, -100);
                if (clamped.NetConditionPermille == 0 && clamped.WearDeltaPermille >= 1)
                {
                    Console.WriteLine("[PASS] Check 5: out-of-range inputs clamp to the permille scale and wear never goes below 1.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 5: clamped = net {clamped.NetConditionPermille}, wear {clamped.WearDeltaPermille}."); }

                var standardCap = ProstheticConditionWearEngine.EvaluateDailyWear(1000, ProstheticComplexityClass.StandardMechanical, 0, 1000);
                var advancedCap = ProstheticConditionWearEngine.EvaluateDailyWear(1000, ProstheticComplexityClass.AdvancedArticulated, 0, 1000);
                if (standardCap.BiomechanicalEfficiencyPermille == 800 && advancedCap.BiomechanicalEfficiencyPermille == 1000)
                {
                    Console.WriteLine("[PASS] Check 6: optimal condition delivers the tier efficiency cap (800 / 1000).");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 6: caps = {standardCap.BiomechanicalEfficiencyPermille}/{advancedCap.BiomechanicalEfficiencyPermille}."); }

                var wearHigher = ProstheticConditionWearEngine.EvaluateDailyWear(900, ProstheticComplexityClass.StandardMechanical, 800, 200);
                var wearLower = ProstheticConditionWearEngine.EvaluateDailyWear(900, ProstheticComplexityClass.StandardMechanical, 200, 800);
                if (wearHigher.WearDeltaPermille > wearLower.WearDeltaPermille)
                {
                    Console.WriteLine($"[PASS] Check 7: labor raises wear ({wearHigher.WearDeltaPermille}) and maintenance lowers it ({wearLower.WearDeltaPermille}).");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 7: labor/maintenance did not order wear ({wearHigher.WearDeltaPermille} vs {wearLower.WearDeltaPermille})."); }

                var again = ProstheticConditionWearEngine.EvaluateDailyWear(900, ProstheticComplexityClass.SimpleImprovised, 300, 800);
                if (again.NetConditionPermille == simple.NetConditionPermille && again.WearDeltaPermille == simple.WearDeltaPermille
                    && again.BiomechanicalEfficiencyPermille == simple.BiomechanicalEfficiencyPermille)
                {
                    Console.WriteLine("[PASS] Check 8: identical inputs produce identical evaluations (deterministic).");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 8: determinism violated."); }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Unhandled: {ex.Message}");
            }

            Console.WriteLine($"Prosthetic condition wear: {passed}/{total} checks passed");
            return passed == total ? 0 : 1;
        }
    }
}
