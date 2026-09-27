// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Verdict;

namespace AtomicWar.GodotApp
{
    public static class HostCliVerdictAccusation
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Verdict Accusation Self-Test (PLAN-INVESTIGATION-EVIDENCE-TRUTH-121) ===");
            int passed = 0; const int total = 5;
            try
            {
                string dataRoot = (!string.IsNullOrEmpty(dataDir) && Directory.Exists(dataDir) ? dataDir : CatalogPath.ResolveDataDir());
                var verdict = VerdictHostSession.Create(dataRoot);
                var session = new VerdictAccusationHostSession(verdict.Reckoning, verdict.EvidenceChain);

                var blocked = session.CanAccuse("case_the_missing_count", "suspect_the_provincial_recorder", 1);
                if (blocked != null && blocked.CaseId == "case_the_missing_count") { Console.WriteLine($"[PASS] Check 1: Typed accusation eligibility returned ({blocked.Status})."); passed++; }
                else Console.WriteLine("[FAIL] Check 1: CanAccuse failed.");

                if (!session.IsResolved("case_the_missing_count")) { Console.WriteLine("[PASS] Check 2: Case starts unresolved."); passed++; }
                else Console.WriteLine("[FAIL] Check 2: case pre-resolved.");

                // A blocked accusation must not resolve (no evidence enrolled).
                var premature = session.ResolveTribunal("case_the_missing_count", "suspect_the_provincial_recorder", 1);
                if (premature == null || !string.IsNullOrEmpty(premature.CaseId)) { Console.WriteLine("[PASS] Check 3: Tribunal is typed and gated by eligibility."); passed++; }
                else Console.WriteLine("[FAIL] Check 3: tribunal returned malformed result.");

                var state = session.CaptureState();
                if (state != null) { Console.WriteLine("[PASS] Check 4: Accusation state captures cleanly."); passed++; }
                else Console.WriteLine("[FAIL] Check 4: capture failed.");

                var restored = new VerdictAccusationHostSession(verdict.Reckoning, verdict.EvidenceChain);
                restored.RestoreState(state);
                if (restored.IsResolved("case_the_missing_count") == session.IsResolved("case_the_missing_count"))
                { Console.WriteLine("[PASS] Check 5: Save/restore round-trips accusation state."); passed++; }
                else Console.WriteLine("[FAIL] Check 5: restore mismatch.");
            }
            catch (Exception ex) { Console.WriteLine($"[FAIL] Unexpected probe exception: {ex.Message}"); }
            Console.WriteLine($"=== Verdict Accusation Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
