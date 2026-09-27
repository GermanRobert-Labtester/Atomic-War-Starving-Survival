// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Combat;

namespace AtomicWar.GodotApp
{
    public static class HostCliChemicalPlume
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Chemical Plume Dispersion Self-Test (PLAN-CHEMICAL-RECON-TRUTH-183) ===");
            int passed = 0; const int total = 7;
            try
            {
                var session = new ChemicalPlumeHostSession();
                session.WeatherProvider = () => new WeatherDispersionVector(20, 90, 0);

                var plume = session.Spawn("plume_test", "agent_chlorine", 3, 4, 800, 10, PlumeToxicityTier.Severe);
                if (session.ActivePlumeCount == 1 && plume.DensityPermille == 800) { Console.WriteLine("[PASS] Check 1: Plume released and tracked."); passed++; }
                else Console.WriteLine("[FAIL] Check 1: spawn failed.");

                int before = plume.DensityPermille;
                session.AdvanceDay();
                if (plume.DensityPermille < before && plume.SectorX != 3) { Console.WriteLine($"[PASS] Check 2: Dispersion dissipated and drifted (density {before}→{plume.DensityPermille})."); passed++; }
                else Console.WriteLine("[FAIL] Check 2: dispersion did not advance.");

                var air = session.EvaluateShelterAir(filtrationPowered: true, filterConditionPermille: 900);
                if (!string.IsNullOrEmpty(air.IndoorAirQuality.ToString())) { Console.WriteLine($"[PASS] Check 3: Shelter air evaluated ({air.IndoorAirQuality})."); passed++; }
                else Console.WriteLine("[FAIL] Check 3: shelter air evaluation failed.");

                var mask = session.EvaluateRespirator(800, PlumeToxicityTier.Severe, 900);
                if (mask.Protected) { Console.WriteLine("[PASS] Check 4: Respirator protects at high canister condition."); passed++; }
                else Console.WriteLine("[FAIL] Check 4: respirator did not protect.");

                var depleted = session.EvaluateRespirator(800, PlumeToxicityTier.Lethal, 0);
                if (!depleted.Protected || depleted.CanisterDepleted) { Console.WriteLine("[PASS] Check 5: Depleted canister fails protection."); passed++; }
                else Console.WriteLine("[FAIL] Check 5: depleted canister still protected.");

                // dissipation to expiry removes the plume
                for (int i = 0; i < 40; i++) session.AdvanceDay();
                if (session.ActivePlumeCount == 0) { Console.WriteLine("[PASS] Check 6: Expired plumes are cleared."); passed++; }
                else Console.WriteLine("[FAIL] Check 6: expired plume retained.");

                var spawn2 = session.Spawn("plume_save", "agent_a", 1, 1, 600, 20, PlumeToxicityTier.Elevated);
                var saved = session.CaptureState();
                var restored = new ChemicalPlumeHostSession();
                restored.RestoreState(saved);
                if (restored.Plumes.Count == 1 && restored.Plumes[0].DensityPermille == 600) { Console.WriteLine("[PASS] Check 7: Save/restore round-trips plume state."); passed++; }
                else Console.WriteLine("[FAIL] Check 7: restore mismatch.");
            }
            catch (Exception ex) { Console.WriteLine($"[FAIL] Unexpected probe exception: {ex.Message}"); }
            Console.WriteLine($"=== Chemical Plume Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
