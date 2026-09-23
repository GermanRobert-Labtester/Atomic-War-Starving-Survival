// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core.Endgame;
using Xunit;

namespace Ashfall.Core.Tests.Endgame
{
    public sealed class CompletionHistorySummaryTests
    {
        [Fact]
        public void Summarize_NullOrEmptyHistory_ReturnsZeroedSummary()
        {
            var summaryNull = CampaignCompletionHistoryService.Summarize(null);
            Assert.Equal(0, summaryNull.TotalCompletions);
            Assert.Equal(0, summaryNull.TotalDaysSurvived);
            Assert.Equal(0.0, summaryNull.AverageDaysSurvived);
            Assert.Equal(0, summaryNull.TotalDeathsRecorded);
            Assert.Equal(0, summaryNull.TotalLivingDwellers);
            Assert.Empty(summaryNull.DistinctEndingIds);
            Assert.Equal(0, summaryNull.GrandTreatiesSigned);
            Assert.Equal(0, summaryNull.TempestsDecommissioned);
            Assert.Equal(0, summaryNull.DebtLedgersBurned);
            Assert.Equal(0, summaryNull.ChildrenSurvivedCount);
            Assert.Equal(0, summaryNull.VelSecretsExposed);
            Assert.Empty(summaryNull.DifficultyCompletions);

            var emptyHistory = new CampaignCompletionHistory();
            var summaryEmpty = CampaignCompletionHistoryService.Summarize(emptyHistory);
            Assert.Equal(0, summaryEmpty.TotalCompletions);
            Assert.Equal(0, summaryEmpty.TotalDaysSurvived);
            Assert.Equal(0.0, summaryEmpty.AverageDaysSurvived);
        }

        [Fact]
        public void Summarize_MultipleRuns_AccuratelyAggregatesStatisticsAndMilestones()
        {
            var history = new CampaignCompletionHistory();

            // Run 1: 300 days, 10 living, 2 deaths, treaty + debt burned, difficulty_standard
            var ctx1 = new EpilogueContextInputs(
                Days: 300,
                LivingDwellers: 10,
                DeathsRecorded: 2,
                GrandTreatySigned: true,
                TempestDecommissioned: false,
                DebtLedgersBurned: true,
                ChildrenSurvived: true,
                VelSecretExposed: false);
            CampaignCompletionHistoryService.Append(history,
                new CampaignCompletionObservation("run_1", "ending_treaty", ctx1, "difficulty_standard"), out _);

            // Run 2: 400 days, 8 living, 5 deaths, tempest + vel secret, difficulty_standard
            var ctx2 = new EpilogueContextInputs(
                Days: 400,
                LivingDwellers: 8,
                DeathsRecorded: 5,
                GrandTreatySigned: false,
                TempestDecommissioned: true,
                DebtLedgersBurned: false,
                ChildrenSurvived: true,
                VelSecretExposed: true);
            CampaignCompletionHistoryService.Append(history,
                new CampaignCompletionObservation("run_2", "ending_tempest", ctx2, "difficulty_standard"), out _);

            // Run 3: 500 days, 12 living, 0 deaths, treaty + tempest + children, difficulty_hardcore
            var ctx3 = new EpilogueContextInputs(
                Days: 500,
                LivingDwellers: 12,
                DeathsRecorded: 0,
                GrandTreatySigned: true,
                TempestDecommissioned: true,
                DebtLedgersBurned: false,
                ChildrenSurvived: true,
                VelSecretExposed: false);
            CampaignCompletionHistoryService.Append(history,
                new CampaignCompletionObservation("run_3", "ending_treaty", ctx3, "difficulty_hardcore"), out _);

            var summary = CampaignCompletionHistoryService.Summarize(history);

            Assert.Equal(3, summary.TotalCompletions);
            Assert.Equal(1200, summary.TotalDaysSurvived);
            Assert.Equal(400.0, summary.AverageDaysSurvived);
            Assert.Equal(7, summary.TotalDeathsRecorded);
            Assert.Equal(30, summary.TotalLivingDwellers);

            // Distinct endings: "ending_treaty" and "ending_tempest"
            Assert.Equal(2, summary.DistinctEndingIds.Count);
            Assert.Contains("ending_treaty", summary.DistinctEndingIds);
            Assert.Contains("ending_tempest", summary.DistinctEndingIds);

            // Milestones
            Assert.Equal(2, summary.GrandTreatiesSigned);
            Assert.Equal(2, summary.TempestsDecommissioned);
            Assert.Equal(1, summary.DebtLedgersBurned);
            Assert.Equal(3, summary.ChildrenSurvivedCount);
            Assert.Equal(1, summary.VelSecretsExposed);

            // Difficulties
            Assert.Equal(2, summary.DifficultyCompletions.Count);
            Assert.Equal(2, summary.DifficultyCompletions["difficulty_standard"]);
            Assert.Equal(1, summary.DifficultyCompletions["difficulty_hardcore"]);
        }

        [Fact]
        public void Summarize_IsPureReadModel_DoesNotMutateHistory()
        {
            var history = new CampaignCompletionHistory();
            var ctx = new EpilogueContextInputs(100, 5, 1, true, true, true, true, true);
            CampaignCompletionHistoryService.Append(history,
                new CampaignCompletionObservation("run_1", "ending_all", ctx, "difficulty_normal"), out _);

            string initialChecksum = history.records[0].Checksum;
            var summary = CampaignCompletionHistoryService.Summarize(history);

            Assert.Equal(1, summary.TotalCompletions);
            Assert.Equal(initialChecksum, history.records[0].Checksum);
            Assert.True(CampaignCompletionHistoryService.TryValidate(history, out string error), error);
        }
    }
}
