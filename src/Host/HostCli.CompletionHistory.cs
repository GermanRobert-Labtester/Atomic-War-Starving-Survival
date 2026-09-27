// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : CompletionHistorySelfTest
// Core Authority     : Ashfall.Core.Endgame.CampaignCompletionHistoryService (EN-07)
// Purpose            : append-only completion history -> pure chronicle summary
// ============================================================================

using System;
using Ashfall.Core.Endgame;

namespace AtomicWar.GodotApp
{
    public static class HostCliCompletionHistory
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Completion History Chronicle Self-Test (EN-07) ===");
            int passed = 0;
            const int total = 8;

            try
            {
                var empty = CampaignCompletionHistoryService.Summarize(null);
                var emptyHistory = CampaignCompletionHistoryService.Summarize(new CampaignCompletionHistory());
                if (empty.TotalCompletions == 0 && empty.TotalDaysSurvived == 0 && empty.AverageDaysSurvived == 0.0
                    && empty.DistinctEndingIds.Count == 0 && empty.DifficultyCompletions.Count == 0
                    && emptyHistory.TotalCompletions == 0)
                {
                    Console.WriteLine("[PASS] Check 1: null or empty history yields a zeroed summary.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 1: empty summary not zeroed."); }

                var history = new CampaignCompletionHistory();
                var append1 = CampaignCompletionHistoryService.Append(history, new CampaignCompletionObservation(
                    "run_1", "ending_treaty",
                    new EpilogueContextInputs(300, 10, 2, true, false, true, true, false), "difficulty_standard"), out _);
                var append2 = CampaignCompletionHistoryService.Append(history, new CampaignCompletionObservation(
                    "run_2", "ending_tempest",
                    new EpilogueContextInputs(400, 8, 5, false, true, false, true, true), "difficulty_standard"), out _);
                var append3 = CampaignCompletionHistoryService.Append(history, new CampaignCompletionObservation(
                    "run_3", "ending_treaty",
                    new EpilogueContextInputs(500, 12, 0, true, true, false, true, false), "difficulty_hardcore"), out _);

                if (append1 == CompletionHistoryAppendResult.Appended
                    && append2 == CompletionHistoryAppendResult.Appended
                    && append3 == CompletionHistoryAppendResult.Appended
                    && history.records.Count == 3)
                {
                    Console.WriteLine("[PASS] Check 2: three valid observations append in order.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 2: appends = {append1}/{append2}/{append3}, count {history.records.Count}."); }

                var duplicate = CampaignCompletionHistoryService.Append(history, new CampaignCompletionObservation(
                    "run_1", "ending_treaty",
                    new EpilogueContextInputs(300, 10, 2, true, false, true, true, false), "difficulty_standard"), out _);
                if (duplicate == CompletionHistoryAppendResult.AlreadyRecorded)
                {
                    Console.WriteLine("[PASS] Check 3: a re-appended identical completion is rejected as already recorded.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 3: duplicate result = {duplicate}."); }

                var invalid = CampaignCompletionHistoryService.Append(history, new CampaignCompletionObservation(
                    "", "ending_treaty",
                    new EpilogueContextInputs(1, 1, 0, false, false, false, false, false)), out _);
                if (invalid == CompletionHistoryAppendResult.InvalidObservation)
                {
                    Console.WriteLine("[PASS] Check 4: a blank run identity is refused as an invalid observation.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 4: invalid result = {invalid}."); }

                var summary = CampaignCompletionHistoryService.Summarize(history);
                if (summary.TotalCompletions == 3 && summary.TotalDaysSurvived == 1200
                    && Math.Abs(summary.AverageDaysSurvived - 400.0) < 0.001)
                {
                    Console.WriteLine($"[PASS] Check 5: days aggregate to 1200 (avg {summary.AverageDaysSurvived:F1}).");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 5: totals = {summary.TotalCompletions}/{summary.TotalDaysSurvived}/{summary.AverageDaysSurvived}."); }

                if (summary.TotalDeathsRecorded == 7 && summary.TotalLivingDwellers == 30 && summary.DistinctEndingIds.Count == 2)
                {
                    Console.WriteLine("[PASS] Check 6: deaths/living totals and two distinct endings are reported.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 6: deaths {summary.TotalDeathsRecorded}, living {summary.TotalLivingDwellers}, endings {summary.DistinctEndingIds.Count}."); }

                if (summary.GrandTreatiesSigned == 2 && summary.TempestsDecommissioned == 2
                    && summary.DebtLedgersBurned == 1 && summary.ChildrenSurvivedCount == 3 && summary.VelSecretsExposed == 1)
                {
                    Console.WriteLine("[PASS] Check 7: every milestone counter aggregates exactly.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 7: milestones {summary.GrandTreatiesSigned}/{summary.TempestsDecommissioned}/{summary.DebtLedgersBurned}/{summary.ChildrenSurvivedCount}/{summary.VelSecretsExposed}."); }

                bool difficultyOk = summary.DifficultyCompletions.Count == 2
                    && summary.DifficultyCompletions.TryGetValue("difficulty_standard", out int standard) && standard == 2
                    && summary.DifficultyCompletions.TryGetValue("difficulty_hardcore", out int hardcore) && hardcore == 1;
                bool pure = CampaignCompletionHistoryService.TryValidate(history, out _) && history.records.Count == 3;
                if (difficultyOk && pure)
                {
                    Console.WriteLine("[PASS] Check 8: difficulty breakdown is exact and Summarize did not mutate history.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 8: difficulty={difficultyOk} pure={pure}."); }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Unhandled: {ex.Message}");
            }

            Console.WriteLine($"Completion history chronicle: {passed}/{total} checks passed");
            return passed == total ? 0 : 1;
        }
    }
}
