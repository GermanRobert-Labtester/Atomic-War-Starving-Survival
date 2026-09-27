// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : RehabilitationProgressionSelfTest
// Core Authority     : Ashfall.Core.Medical.RehabilitationProgressionEngine (F14-E / XP-06)
// Purpose            : deterministic fitting -> adaptation -> mastery progression
// ============================================================================

using System;
using Ashfall.Core.Medical;

namespace AtomicWar.GodotApp
{
    public static class HostCliRehabilitationProgression
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Rehabilitation Arc Progression Self-Test (F14-E) ===");
            int passed = 0;
            const int total = 8;

            try
            {
                var start = RehabilitationProgressionEngine.StartRehabilitation("hand");
                if (start.ProstheticTypeKey == "hand" && start.Phase == "fitting" && start.DaysInPhase == 0
                    && start.QualityRampPermille == 500 && Math.Abs(RehabilitationProgressionEngine.GetQualityFactor(start) - 0.5f) < 0.0001f)
                {
                    Console.WriteLine("[PASS] Check 1: StartRehabilitation initialises fitting at 500 permille (0.5 factor).");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 1: start = {start.Phase}/{start.DaysInPhase}/{start.QualityRampPermille}."); }

                var day3 = RehabilitationProgressionEngine.AdvanceDaily(start, resilienceMultiplier: 1.0f, days: 3);
                if (day3.Phase == "fitting" && day3.DaysInPhase == 3 && day3.QualityRampPermille == 500)
                {
                    Console.WriteLine("[PASS] Check 2: three days remain in fitting with the quality floor held.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 2: day3 = {day3.Phase}/{day3.DaysInPhase}/{day3.QualityRampPermille}."); }

                var day4 = RehabilitationProgressionEngine.AdvanceDaily(day3, resilienceMultiplier: 1.0f, days: 1);
                if (day4.Phase == "adaptation" && day4.DaysInPhase == 0 && day4.QualityRampPermille == 500)
                {
                    Console.WriteLine("[PASS] Check 3: the fourth day transitions fitting -> adaptation.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 3: day4 = {day4.Phase}/{day4.DaysInPhase}/{day4.QualityRampPermille}."); }

                var adaptation = new RehabRecord("hand", "adaptation", 0, 500);
                var ramp7 = RehabilitationProgressionEngine.AdvanceDaily(adaptation, resilienceMultiplier: 1.0f, days: 7, adaptationDurationDays: 14);
                if (ramp7.Phase == "adaptation" && ramp7.DaysInPhase == 7
                    && ramp7.QualityRampPermille > 500 && ramp7.QualityRampPermille < 1000)
                {
                    Console.WriteLine($"[PASS] Check 4: adaptation ramps quality steadily ({ramp7.QualityRampPermille} permille after 7 days).");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 4: ramp = {ramp7.Phase}/{ramp7.DaysInPhase}/{ramp7.QualityRampPermille}."); }

                var mastery = RehabilitationProgressionEngine.AdvanceDaily(ramp7, resilienceMultiplier: 1.0f, days: 7, adaptationDurationDays: 14);
                if (mastery.Phase == "mastery" && mastery.QualityRampPermille == 1000
                    && Math.Abs(RehabilitationProgressionEngine.GetQualityFactor(mastery) - 1.0f) < 0.0001f)
                {
                    Console.WriteLine("[PASS] Check 5: the remaining adaptation days reach permanent mastery at 1000 permille.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 5: mastery = {mastery.Phase}/{mastery.QualityRampPermille}."); }

                var masteryPermanent = RehabilitationProgressionEngine.AdvanceDaily(new RehabRecord("hand", "mastery", 10, 1000), days: 5);
                if (masteryPermanent.Phase == "mastery" && masteryPermanent.DaysInPhase == 15 && masteryPermanent.QualityRampPermille == 1000)
                {
                    Console.WriteLine("[PASS] Check 6: mastery is permanent and keeps counting days.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 6: mastery drift = {masteryPermanent.Phase}/{masteryPermanent.DaysInPhase}/{masteryPermanent.QualityRampPermille}."); }

                if (RehabilitationProgressionEngine.GetQualityFactor(null) == 1.0f
                    && Math.Abs(RehabilitationProgressionEngine.GetQualityFactor(new RehabRecord("leg", "adaptation", 5, 750)) - 0.75f) < 0.0001f)
                {
                    Console.WriteLine("[PASS] Check 7: quality factor is 1.0 for intact and 0.75 at 750 permille.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 7: quality factors wrong."); }

                var noAdvance = RehabilitationProgressionEngine.AdvanceDaily(adaptation, days: 0);
                var nullStart = RehabilitationProgressionEngine.AdvanceDaily(null, days: 3);
                if (ReferenceEquals(noAdvance, adaptation) && nullStart.Phase == "fitting" && nullStart.DaysInPhase == 0)
                {
                    Console.WriteLine("[PASS] Check 8: zero days is a no-op; a null record starts a fresh fitting arc at day 0.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 8: no-op/null = {noAdvance.DaysInPhase}/{nullStart.Phase}/{nullStart.DaysInPhase}."); }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Unhandled: {ex.Message}");
            }

            Console.WriteLine($"Rehabilitation arc progression: {passed}/{total} checks passed");
            return passed == total ? 0 : 1;
        }
    }
}
