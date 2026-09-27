// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : ApprenticeshipCurriculumSelfTest
// Subsystem          : Learner literacy, comprehension, and certification
// ============================================================================

using System;
using Ashfall.Core.Education;

namespace AtomicWar.GodotApp
{
    public static class HostCliApprenticeshipCurriculum
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Apprenticeship Curriculum Self-Test ===");
            int passed = 0;
            const int total = 6;

            try
            {
                var session = ApprenticeshipCurriculumHostSession.Create();

                if (session.LearnerCount == 0)
                {
                    Console.WriteLine("[PASS] Check 1: Curriculum roster starts empty.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 1: Roster was not empty.");
                }

                session.EnrollLearner("surv_a", LiteracyLevel.Illiterate, 0);
                var result = session.RunSession("surv_a", 500, 200, 1, 1);
                if (session.LearnerCount == 1 && result.ComprehensionGained > 0)
                {
                    Console.WriteLine($"[PASS] Check 2: First session yields comprehension gain ({result.ComprehensionGained}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 2: No comprehension gain from the first session.");
                }

                // Repeated sessions advance the literacy tier over time.
                int guard = 0;
                LiteracyLevel reached = LiteracyLevel.Illiterate;
                while (guard++ < 60)
                {
                    var r = session.RunSession("surv_a", 900, 900, 1, 1);
                    reached = session.GetLiteracyLevel("surv_a");
                    if (reached > LiteracyLevel.Basic || r.AdvancedLiteracyTier) { reached = r.NewLiteracyLevel; break; }
                }
                if (reached >= LiteracyLevel.Basic)
                {
                    Console.WriteLine($"[PASS] Check 3: Repeated sessions advance literacy to {reached}.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 3: Literacy stuck at {reached}.");
                }

                // Sustained fatigue: after the fatigue-onset threshold every session is penalised.
                int penalised = 0;
                for (int i = 0; i < 10; i++)
                {
                    var r = session.RunSession("surv_a", 900, 900, 1, 1);
                    if (r.FatiguePenaltyPermille > 0) penalised++;
                }
                if (penalised >= ApprenticeshipCurriculumEngine.FatigueOnsetSessions)
                {
                    Console.WriteLine($"[PASS] Check 4: Sustained teaching triggers fatigue on {penalised}/10 sessions.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 4: Only {penalised}/10 sessions were fatigue-penalised.");
                }

                // Certification readiness requires both the Core verdict and the trade skill.
                session.EnrollLearner("surv_b", LiteracyLevel.Scholarly, 1000);
                var learnerB = session.FindLearner("surv_b")!;
                learnerB.TradeSkillPermille["medicine"] = 900;
                if (session.IsCertificationReady("surv_b", "medicine")
                    && !session.IsCertificationReady("surv_b", "craft"))
                {
                    Console.WriteLine("[PASS] Check 5: Certification readiness honours the Core threshold.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 5: Certification readiness verdict is wrong.");
                }

                bool saved = session.TrySave();
                var reloaded = ApprenticeshipCurriculumHostSession.Create();
                bool loaded = reloaded.TryLoad();
                if (saved && loaded && reloaded.LearnerCount == 2
                    && reloaded.CertifiedTradeCount("surv_b") == 1
                    && ApprenticeshipCurriculumSaveStore.SectionName.Equals("apprenticeship_curriculum", StringComparison.Ordinal)
                    && ApprenticeshipCurriculumSaveStore.FileName.Equals("apprenticeship_curriculum_save.json", StringComparison.Ordinal))
                {
                    Console.WriteLine($"[PASS] Check 6: Save/restore and store contract verified.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 6: saved={saved}, loaded={loaded}, learners={reloaded.LearnerCount}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Self-test crashed: {ex.Message}");
            }

            Console.WriteLine($"=== Apprenticeship Curriculum Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
