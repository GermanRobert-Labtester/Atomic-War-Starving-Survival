// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Farming;

namespace AtomicWar.GodotApp
{
    public static class HostCliOilseedPressing
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Oilseed Pressing Self-Test (PLAN-PRESERVATION-TRUTH-118) ===");
            int passed = 0; const int total = 6;
            try
            {
                var session = new OilseedPressingHostSession();
                if (!session.PressInstalled) { Console.WriteLine("[PASS] Check 1: Press starts uninstalled."); passed++; }
                else Console.WriteLine("[FAIL] Check 1: press pre-installed.");

                session.InstallPress(PressToolGrade.HydraulicWorkshopPress);
                if (session.PressInstalled && session.ToolGrade == PressToolGrade.HydraulicWorkshopPress) { Console.WriteLine("[PASS] Check 2: Press installed."); passed++; }
                else Console.WriteLine("[FAIL] Check 2: install failed.");

                var inv = new Dictionary<string, int>(StringComparer.Ordinal) { [OilseedPressingEngine.CropOilseedId] = 100 };
                int Count(string id) => inv.TryGetValue(id, out var q) ? q : 0;
                bool Consume(string id, int q) { inv[id] = Math.Max(0, Count(id) - q); return true; }
                void Add(string id, int q) { inv[id] = Count(id) + q; }

                var result = session.Press(20, OilseedPressingMode.CulinaryCookingOil, Count, Consume, Add);
                if (result.PrimaryOutputAmount > 0 && Count(OilseedPressingEngine.DefaultCookingOilId) == result.PrimaryOutputAmount)
                { Console.WriteLine($"[PASS] Check 3: Pressed 20 seed → {result.PrimaryOutputAmount}x cooking oil."); passed++; }
                else Console.WriteLine("[FAIL] Check 3: pressing produced no oil.");

                if (Count(OilseedPressingEngine.CropOilseedId) == 80) { Console.WriteLine("[PASS] Check 4: Seeds consumed from canonical inventory."); passed++; }
                else Console.WriteLine("[FAIL] Check 4: seed consumption wrong.");

                var tooLittle = session.Press(1, OilseedPressingMode.CulinaryCookingOil, Count, Consume, Add);
                if (tooLittle.PrimaryOutputAmount == 0) { Console.WriteLine("[PASS] Check 5: Below-batch pressing yields nothing."); passed++; }
                else Console.WriteLine("[FAIL] Check 5: sub-batch press yielded output.");

                var saved = session.CaptureState();
                var restored = new OilseedPressingHostSession();
                restored.RestoreState(saved);
                if (restored.PressInstalled && restored.TotalPressed == session.TotalPressed) { Console.WriteLine("[PASS] Check 6: Save/restore round-trips press state."); passed++; }
                else Console.WriteLine("[FAIL] Check 6: restore mismatch.");
            }
            catch (Exception ex) { Console.WriteLine($"[FAIL] Unexpected probe exception: {ex.Message}"); }
            Console.WriteLine($"=== Oilseed Pressing Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
