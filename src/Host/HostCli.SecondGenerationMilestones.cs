// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class HostCliSecondGenerationMilestones
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Second-Generation Milestones Self-Test (PLAN-GENERATIONAL-MILESTONE-TRUTH-160) ===");
            int passed = 0; const int total = 7;
            try
            {
                string dataRoot = (!string.IsNullOrEmpty(dataDir) && Directory.Exists(dataDir) ? dataDir : CatalogPath.ResolveDataDir());
                var children = ChildDevelopmentHostSession.Create(dataRoot);
                var session = new SecondGenerationMilestoneHostSession(children.System);

                children.RegisterChild("child_one", "Ada", 1, null, "caregiver_a");
                var profile = children.GetChild("child_one")!;

                profile.Stage = DevelopmentStage.Infant;
                profile.EducationScore = 0f;
                var r0 = session.EvaluateAndRecord("child_one", 30);
                if (!r0.Eligible) { Console.WriteLine("[PASS] Check 1: Infant not eligible for first milestone."); passed++; }
                else Console.WriteLine("[FAIL] Check 1: infant wrongly eligible.");

                profile.Stage = DevelopmentStage.Toddler;
                var r1 = session.EvaluateAndRecord("child_one", 40);
                if (r1.Eligible && profile.Milestones.Contains("first_words")) { Console.WriteLine("[PASS] Check 2: Toddler records first_words."); passed++; }
                else Console.WriteLine("[FAIL] Check 2: first_words not recorded.");

                int before = profile.Milestones.Count;
                session.EvaluateAndRecord("child_one", 41);
                if (profile.Milestones.Count == before) { Console.WriteLine("[PASS] Check 3: Milestone fires once (no re-fire)."); passed++; }
                else Console.WriteLine("[FAIL] Check 3: duplicate milestone.");

                profile.Stage = DevelopmentStage.Child;
                profile.EducationScore = 30f;
                var r2 = session.EvaluateAndRecord("child_one", 60);
                if (r2.Eligible && profile.Milestones.Contains("foundational_letters")) { Console.WriteLine("[PASS] Check 4: Education gate records foundational_letters."); passed++; }
                else Console.WriteLine("[FAIL] Check 4: foundational_letters not recorded.");

                var census = children.GetCensus();
                if (children.System.CaptureState().MilestoneHistory.Count >= 2) { Console.WriteLine("[PASS] Check 5: Milestone history grows in canonical child state."); passed++; }
                else Console.WriteLine("[FAIL] Check 5: history not updated.");

                int recorded = session.TickAll(90);
                if (recorded >= 0) { Console.WriteLine($"[PASS] Check 6: TickAll evaluated the roster ({recorded} recorded)."); passed++; }
                else Console.WriteLine("[FAIL] Check 6: TickAll failed.");

                var restored = new SecondGenerationMilestoneHostSession(new ChildDevelopmentSystem(children.System.CaptureState()));
                if (restored.EvaluateAndRecord("child_one", 91).Milestone != Ashfall.Core.Generations.MilestoneKind.None)
                { Console.WriteLine("[PASS] Check 7: Capture/restore preserves milestone state."); passed++; }
                else Console.WriteLine("[FAIL] Check 7: restore lost state.");
            }
            catch (Exception ex) { Console.WriteLine($"[FAIL] Unexpected probe exception: {ex.Message}"); }
            Console.WriteLine($"=== Second-Generation Milestones Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
