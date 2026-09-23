// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core.Endgame;
using Xunit;

namespace Ashfall.Core.Tests.Endgame
{
    public sealed class CrossRunProfileStoreTests
    {
        [Fact]
        public void Record_NullOrInvalidHistory_ReturnsInvalidHistory()
        {
            var store = new CrossRunProfileStore();
            Assert.Equal(ProfileRecordResult.InvalidHistory, store.Record(null));

            var invalidHistory = new CampaignCompletionHistory { schemaVersion = 999 };
            Assert.Equal(ProfileRecordResult.InvalidHistory, store.Record(invalidHistory));
        }

        [Fact]
        public void Record_EmptyHistory_ReturnsNoNewRecords()
        {
            var store = new CrossRunProfileStore();
            var history = new CampaignCompletionHistory();

            Assert.Equal(ProfileRecordResult.NoNewRecords, store.Record(history));
            Assert.Empty(store.Runs);
            Assert.Equal(0, store.TotalRunsCompleted);
        }

        [Fact]
        public void Record_ValidRuns_AppendsAndMaintainsIdempotency()
        {
            var store = new CrossRunProfileStore();
            var history = new CampaignCompletionHistory();

            var ctx1 = new EpilogueContextInputs(
                Days: 360,
                LivingDwellers: 10,
                DeathsRecorded: 3,
                GrandTreatySigned: true,
                TempestDecommissioned: false,
                DebtLedgersBurned: true,
                ChildrenSurvived: true,
                VelSecretExposed: false);
            CampaignCompletionHistoryService.Append(history,
                new CampaignCompletionObservation("run_alpha", "ending_treaty", ctx1, "difficulty_hardcore"), out _);

            var ctx2 = new EpilogueContextInputs(
                Days: 420,
                LivingDwellers: 8,
                DeathsRecorded: 6,
                GrandTreatySigned: false,
                TempestDecommissioned: true,
                DebtLedgersBurned: false,
                ChildrenSurvived: true,
                VelSecretExposed: true);
            CampaignCompletionHistoryService.Append(history,
                new CampaignCompletionObservation("run_beta", "ending_tempest", ctx2, "difficulty_standard"), out _);

            // First record pass
            var result1 = store.Record(history);
            Assert.Equal(ProfileRecordResult.Success, result1);
            Assert.Equal(2, store.TotalRunsCompleted);

            var row1 = store.Runs[0];
            Assert.Equal(1, row1.RunOrdinal);
            Assert.Equal("run_alpha", row1.RunIdentity);
            Assert.Equal("ending_treaty", row1.EndingId);
            Assert.Equal("difficulty_hardcore", row1.DifficultyPresetId);
            Assert.Equal(360, row1.DaysSurvived);
            Assert.Equal(10, row1.LivingDwellers);
            Assert.Equal(3, row1.DeathsRecorded);
            Assert.True(row1.GrandTreatySigned);
            Assert.True(row1.DebtLedgersBurned);

            var row2 = store.Runs[1];
            Assert.Equal(2, row2.RunOrdinal);
            Assert.Equal("run_beta", row2.RunIdentity);
            Assert.Equal("ending_tempest", row2.EndingId);
            Assert.Equal("difficulty_standard", row2.DifficultyPresetId);
            Assert.Equal(420, row2.DaysSurvived);
            Assert.True(row2.TempestDecommissioned);
            Assert.True(row2.VelSecretExposed);

            // Second record pass (idempotency check)
            var result2 = store.Record(history);
            Assert.Equal(ProfileRecordResult.NoNewRecords, result2);
            Assert.Equal(2, store.TotalRunsCompleted);
        }

        [Fact]
        public void CaptureAndRestore_PreservesAllRunsAndIntegrity()
        {
            var store = new CrossRunProfileStore();
            var history = new CampaignCompletionHistory();

            var ctx = new EpilogueContextInputs(200, 5, 1, true, false, false, true, false);
            CampaignCompletionHistoryService.Append(history,
                new CampaignCompletionObservation("run_test", "ending_test", ctx, "difficulty_test"), out _);

            store.Record(history);
            var state = store.CaptureState();
            Assert.NotEmpty(state.Checksum);

            var restored = CrossRunProfileStore.RestoreState(state);
            Assert.Equal(store.TotalRunsCompleted, restored.TotalRunsCompleted);
            Assert.Single(restored.Runs);
            Assert.Equal(store.Runs[0].RunIdentity, restored.Runs[0].RunIdentity);
            Assert.Equal(store.Runs[0].EndingId, restored.Runs[0].EndingId);
            Assert.Equal(store.Runs[0].CompletionId, restored.Runs[0].CompletionId);
        }
    }
}
