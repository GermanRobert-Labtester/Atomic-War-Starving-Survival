// SPDX-License-Identifier: MIT

using System.Linq;
using Ashfall.Core.Endgame;
using Xunit;

namespace Ashfall.Core.Tests.Endgame
{
    public sealed class CampaignCompletionHistoryTests
    {
        [Fact]
        public void Append_DerivesFrozenRecordFromCanonicalEpilogueContext()
        {
            var history = new CampaignCompletionHistory();

            var result = CampaignCompletionHistoryService.Append(
                history,
                Observation("default/slot_1/seed_17", "ending_dawn_of_thaw", days: 360, living: 9, deaths: 4),
                out CampaignCompletionRecord? record);

            Assert.Equal(CompletionHistoryAppendResult.Appended, result);
            Assert.NotNull(record);
            Assert.Equal("ending_dawn_of_thaw", record!.endingId);
            Assert.Equal(360, record.daysSurvived);
            Assert.Equal(9, record.livingDwellers);
            Assert.Equal(4, record.deathsRecorded);
            Assert.True(record.grandTreatySigned);
            Assert.True(record.debtLedgersBurned);
            Assert.True(CampaignCompletionHistoryService.TryValidate(history, out string error), error);
        }

        [Fact]
        public void Append_SameCompletionObservationIsIdempotent()
        {
            var history = new CampaignCompletionHistory();
            var observation = Observation("default/slot_1/seed_17", "ending_dawn_of_thaw");

            Assert.Equal(CompletionHistoryAppendResult.Appended,
                CampaignCompletionHistoryService.Append(history, observation, out _));
            Assert.Equal(CompletionHistoryAppendResult.AlreadyRecorded,
                CampaignCompletionHistoryService.Append(history, observation, out _));
            Assert.Single(history.records);
        }

        [Fact]
        public void Append_DifferentCampaignsWithSameEndingRemainDistinct()
        {
            var history = new CampaignCompletionHistory();

            Assert.Equal(CompletionHistoryAppendResult.Appended,
                CampaignCompletionHistoryService.Append(history,
                    Observation("default/slot_1/seed_17", "ending_dawn_of_thaw"), out _));
            Assert.Equal(CompletionHistoryAppendResult.Appended,
                CampaignCompletionHistoryService.Append(history,
                    Observation("default/slot_2/seed_17", "ending_dawn_of_thaw"), out _));

            Assert.Equal(2, history.records.Count);
            Assert.Equal(1, history.records[0].completionOrdinal);
            Assert.Equal(2, history.records[1].completionOrdinal);
            Assert.Equal(history.records[0].Checksum, history.records[1].previousChecksum);
        }

        [Fact]
        public void Validation_DetectsSemanticTamperAndBrokenChain()
        {
            var history = HistoryWithTwoRecords();
            history.records[1].endingId = "ending_tampered";

            Assert.False(CampaignCompletionHistoryService.TryValidate(history, out string error));
            Assert.Contains("checksum", error, System.StringComparison.OrdinalIgnoreCase);

            history = HistoryWithTwoRecords();
            history.records[1].previousChecksum = "not-the-first-record";
            Assert.False(CampaignCompletionHistoryService.TryValidate(history, out error));
            Assert.Contains("link", error, System.StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Serialize_RoundTripsAndRejectsCorruptPayload()
        {
            var history = HistoryWithTwoRecords();
            string json = CampaignCompletionHistoryService.Serialize(history);

            Assert.True(CampaignCompletionHistoryService.TryDeserialize(json, out CampaignCompletionHistory restored, out string error), error);
            Assert.Equal(2, restored.records.Count);
            Assert.Equal(history.records[1].Checksum, restored.records[1].Checksum);

            string corrupt = json.Replace("ending_dawn_of_thaw", "ending_tampered");
            Assert.False(CampaignCompletionHistoryService.TryDeserialize(corrupt, out _, out error));
            Assert.NotEmpty(error);
        }

        [Fact]
        public void Clone_DetachesReadModelsFromStoredHistory()
        {
            var history = HistoryWithTwoRecords();
            CampaignCompletionHistory clone = CampaignCompletionHistoryService.Clone(history);
            clone.records[0].endingId = "presentation_must_not_mutate_history";

            Assert.Equal("ending_dawn_of_thaw", history.records[0].endingId);
            Assert.NotEqual(clone.records[0].endingId, history.records[0].endingId);
        }

        [Fact]
        public void RecordDerivation_IsDeterministicAndDoesNotUseSourceText()
        {
            var first = new CampaignCompletionHistory();
            var second = new CampaignCompletionHistory();
            var firstContext = new EpilogueContextInputs(360, 9, 4, true, false, true, true, false, new[] { "trace_a" });
            var secondContext = new EpilogueContextInputs(360, 9, 4, true, false, true, true, false, new[] { "trace_b" });

            CampaignCompletionHistoryService.Append(first,
                new CampaignCompletionObservation("default/slot_1/seed_17", "ending_dawn_of_thaw", firstContext), out CampaignCompletionRecord? firstRecord);
            CampaignCompletionHistoryService.Append(second,
                new CampaignCompletionObservation("default/slot_1/seed_17", "ending_dawn_of_thaw", secondContext), out CampaignCompletionRecord? secondRecord);

            Assert.Equal(firstRecord!.completionId, secondRecord!.completionId);
            Assert.Equal(firstRecord.Checksum, secondRecord.Checksum);
        }

        [Fact]
        public void Append_IsObservationOnlyAndLeavesCanonicalInputUntouched()
        {
            var history = new CampaignCompletionHistory();
            var context = new EpilogueContextInputs(360, 9, 4, true, false, true, true, false,
                new[] { "treaty", "memorial" });
            var observation = new CampaignCompletionObservation(
                "default/slot_1/seed_17", "ending_dawn_of_thaw", context);

            Assert.Equal(CompletionHistoryAppendResult.Appended,
                CampaignCompletionHistoryService.Append(history, observation, out _));

            Assert.Same(context, observation.Context);
            Assert.Equal(360, context.Days);
            Assert.Equal(9, context.LivingDwellers);
            Assert.Equal(4, context.DeathsRecorded);
            Assert.Equal(new[] { "treaty", "memorial" }, context.SourceIds);
        }

        [Fact]
        public void Append_StampsDifficultyPresetOnV2Records()
        {
            var history = new CampaignCompletionHistory();
            var observation = new CampaignCompletionObservation(
                "default/slot_1/seed_17",
                "ending_dawn_of_thaw",
                new EpilogueContextInputs(360, 9, 4, true, false, true, true, false),
                "difficulty_austere");

            Assert.Equal(CompletionHistoryAppendResult.Appended,
                CampaignCompletionHistoryService.Append(history, observation, out CampaignCompletionRecord? record));
            Assert.Equal(2, record!.schemaVersion);
            Assert.Equal("difficulty_austere", record.difficultyPresetId);
            Assert.True(CampaignCompletionHistoryService.TryValidate(history, out string error), error);
        }

        [Fact]
        public void TryValidate_AcceptsLegacyV1RecordsWithoutDifficultyField()
        {
            var seeded = new CampaignCompletionHistory();
            Assert.Equal(CompletionHistoryAppendResult.Appended,
                CampaignCompletionHistoryService.Append(seeded, Observation("default/slot_1/seed_17", "ending_dawn_of_thaw"), out CampaignCompletionRecord? modern));

            var v1 = new CampaignCompletionRecord
            {
                schemaVersion = 1,
                completionId = modern!.completionId,
                completionOrdinal = 1,
                runIdentity = modern.runIdentity,
                endingId = modern.endingId,
                daysSurvived = modern.daysSurvived,
                livingDwellers = modern.livingDwellers,
                deathsRecorded = modern.deathsRecorded,
                grandTreatySigned = modern.grandTreatySigned,
                tempestDecommissioned = modern.tempestDecommissioned,
                debtLedgersBurned = modern.debtLedgersBurned,
                childrenSurvived = modern.childrenSurvived,
                velSecretExposed = modern.velSecretExposed,
                previousChecksum = string.Empty
            };
            v1.Checksum = Ashfall.Core.SaveChecksum.Compute(v1);

            var history = new CampaignCompletionHistory { schemaVersion = 1 };
            history.records.Add(v1);
            Assert.True(CampaignCompletionHistoryService.TryValidate(history, out string error), error);
        }

        [Fact]
        public void PublicHistoryApi_ExposesNoMutationOrDeletionOperation()
        {
            string[] methodNames = typeof(CampaignCompletionHistoryService)
                .GetMethods()
                .Select(method => method.Name)
                .ToArray();

            Assert.DoesNotContain(methodNames, name => name.Contains("Update", System.StringComparison.OrdinalIgnoreCase));
            Assert.DoesNotContain(methodNames, name => name.Contains("Delete", System.StringComparison.OrdinalIgnoreCase));
            Assert.DoesNotContain(methodNames, name => name.Contains("Remove", System.StringComparison.OrdinalIgnoreCase));
        }

        private static CampaignCompletionHistory HistoryWithTwoRecords()
        {
            var history = new CampaignCompletionHistory();
            CampaignCompletionHistoryService.Append(history,
                Observation("default/slot_1/seed_17", "ending_dawn_of_thaw"), out _);
            CampaignCompletionHistoryService.Append(history,
                Observation("default/slot_2/seed_18", "ending_dawn_of_thaw", days: 361), out _);
            return history;
        }

        private static CampaignCompletionObservation Observation(
            string runIdentity,
            string endingId,
            int days = 360,
            int living = 9,
            int deaths = 4)
        {
            return new CampaignCompletionObservation(
                runIdentity,
                endingId,
                new EpilogueContextInputs(days, living, deaths, true, false, true, true, false));
        }
    }
}
