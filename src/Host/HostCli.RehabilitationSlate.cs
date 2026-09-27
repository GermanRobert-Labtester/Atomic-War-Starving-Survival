// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : RehabilitationSlateSelfTest
// Core Authority     : Ashfall.Core.Medical.RehabilitationSlateProjection (EN-04)
// Purpose            : body state + rehab record -> medical-slate read model
// ============================================================================

using System;
using Ashfall.Core.Medical;

namespace AtomicWar.GodotApp
{
    public static class HostCliRehabilitationSlate
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Rehabilitation Medicine Slate Self-Test (EN-04) ===");
            int passed = 0;
            const int total = 8;

            try
            {
                var nullSlate = RehabilitationSlateProjection.Project("survivor_null", null);
                if (!nullSlate.HasProsthetics && nullSlate.CurrentPhase == "none"
                    && nullSlate.QualityPercent == 100f && nullSlate.NextMilestone == "No prosthetics fitted")
                {
                    Console.WriteLine("[PASS] Check 1: a null body state renders the intact/none slate.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 1: null body slate wrong."); }

                var intactSlate = RehabilitationSlateProjection.Project("survivor_intact", SurvivorBodyState.CreateDefaultIntact());
                if (!intactSlate.HasProsthetics && intactSlate.CurrentPhase == "none" && intactSlate.QualityPercent == 100f)
                {
                    Console.WriteLine("[PASS] Check 2: an intact body renders no prosthetics and full quality.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 2: intact body slate wrong."); }

                var fittingBody = SurvivorBodyState.CreateDefaultIntact();
                fittingBody.SetLimbCondition(SurvivorBodyState.LeftArmKey, "prosthetized", "item_hook_prosthetic");
                fittingBody.Rehab = new RehabRecord("hand", "fitting", 2, 500);
                var fitting = RehabilitationSlateProjection.Project("survivor_fit", fittingBody, hasPhantomPain: true);
                if (fitting.HasProsthetics && fitting.FittedProstheticsCount == 1 && fitting.CurrentPhase == "fitting"
                    && fitting.DaysInPhase == 2 && fitting.QualityPercent == 50f
                    && fitting.NextMilestone.Contains("Adaptation phase in 2 days") && fitting.HasPhantomPain)
                {
                    Console.WriteLine("[PASS] Check 3: fitting phase renders count, 50% ramp, milestone, and phantom pain.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 3: fitting slate wrong ({fitting.CurrentPhase}/{fitting.QualityPercent}/{fitting.NextMilestone})."); }

                var adaptBody = SurvivorBodyState.CreateDefaultIntact();
                adaptBody.SetLimbCondition(SurvivorBodyState.RightLegKey, "prosthetized", "item_peg_leg");
                adaptBody.Rehab = new RehabRecord("leg", "adaptation", 7, 750);
                var adaptation = RehabilitationSlateProjection.Project("survivor_adapt", adaptBody, adaptationDurationDays: 14);
                if (adaptation.CurrentPhase == "adaptation" && adaptation.DaysInPhase == 7
                    && adaptation.QualityPercent == 75f && adaptation.NextMilestone.Contains("Full mastery in 7 days"))
                {
                    Console.WriteLine("[PASS] Check 4: adaptation phase renders 75% ramp and remaining-days milestone.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 4: adaptation slate wrong ({adaptation.CurrentPhase}/{adaptation.QualityPercent}/{adaptation.NextMilestone})."); }

                var masteryBody = SurvivorBodyState.CreateDefaultIntact();
                masteryBody.SetLimbCondition(SurvivorBodyState.LeftArmKey, "prosthetized", "item_articulated_hand");
                masteryBody.Rehab = new RehabRecord("hand", "mastery", 10, 1000);
                var mastery = RehabilitationSlateProjection.Project("survivor_mastery", masteryBody);
                if (mastery.CurrentPhase == "mastery" && mastery.QualityPercent == 100f
                    && mastery.NextMilestone == "Prosthetic mastery achieved")
                {
                    Console.WriteLine("[PASS] Check 5: mastery phase renders permanent full quality.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 5: mastery slate wrong ({mastery.CurrentPhase}/{mastery.NextMilestone})."); }

                var twoBody = SurvivorBodyState.CreateDefaultIntact();
                twoBody.SetLimbCondition(SurvivorBodyState.LeftArmKey, "prosthetized", "item_hook_prosthetic");
                twoBody.SetLimbCondition(SurvivorBodyState.RightLegKey, "prosthetic", "item_peg_leg");
                twoBody.Rehab = new RehabRecord("both", "fitting", 1, 250);
                var two = RehabilitationSlateProjection.Project("survivor_two", twoBody);
                if (two.FittedProstheticsCount == 2 && two.HasProsthetics)
                {
                    Console.WriteLine("[PASS] Check 6: both 'prosthetized' and 'prosthetic' limb conditions count toward the tally.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 6: prosthetic count = {two.FittedProstheticsCount}."); }

                var overQuality = new RehabilitationSlateProjection("survivor_q", true, -3, "mastery", -2, 150f, "x", false);
                if (overQuality.FittedProstheticsCount == 0 && overQuality.DaysInPhase == 0 && overQuality.QualityPercent == 100f
                    && overQuality.SurvivorId == "survivor_q")
                {
                    Console.WriteLine("[PASS] Check 7: the constructor clamps counts/days at 0 and quality at 100.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 7: clamps = {overQuality.FittedProstheticsCount}/{overQuality.DaysInPhase}/{overQuality.QualityPercent}."); }

                var legacyBody = SurvivorBodyState.CreateDefaultIntact();
                legacyBody.Rehab = new RehabRecord("hand", "unknown_phase", 3, 1000);
                var legacy = RehabilitationSlateProjection.Project("survivor_legacy", legacyBody);
                if (legacy.CurrentPhase == "unknown_phase" && legacy.NextMilestone == "Stable" && !legacy.HasProsthetics)
                {
                    Console.WriteLine("[PASS] Check 8: an unrecognised phase renders a Stable milestone without inventing prosthetics.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 8: legacy slate = {legacy.CurrentPhase}/{legacy.NextMilestone}/{legacy.HasProsthetics}."); }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Unhandled: {ex.Message}");
            }

            Console.WriteLine($"Rehabilitation medicine slate: {passed}/{total} checks passed");
            return passed == total ? 0 : 1;
        }
    }
}
