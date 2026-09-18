// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Campaign;
using Ashfall.Core.Commitments;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    public class CommitmentSystemTests
    {
        [Fact]
        public void Commitment_EvaluatesInactive_BeforeStartDay()
        {
            var system = new CommitmentSystem();
            system.RegisterCommitment(new CommitmentDefinition
            {
                id = "commitment_test_tribute",
                type = "tribute",
                title = "Sample Tribute",
                start_day = 10,
                due_day = 20,
                warning_lead_days = 3,
                target_quantity = 50,
                consequence_class = "faction_standing_penalty",
                consequence_target = "iron_coalition",
                consequence_magnitude = -15
            });

            var readModel = system.GetCommitment("commitment_test_tribute", currentDay: 5);
            Assert.NotNull(readModel);
            Assert.Equal(CommitmentStatus.Pending, readModel.Status);
            Assert.Equal(15, readModel.DaysRemaining);
        }

        [Fact]
        public void Warning_FiresAtConfiguredLeadThreshold_AndDoesNotRepeat()
        {
            var system = new CommitmentSystem();
            system.RegisterCommitment(new CommitmentDefinition
            {
                id = "commitment_test_warning",
                type = "treaty_term",
                title = "Border Accord",
                start_day = 5,
                due_day = 15,
                warning_lead_days = 4, // Warning on Day 11 (15 - 4 = 11)
                target_quantity = 10,
                consequence_class = "economy_shock",
                consequence_target = "embargo_border"
            });

            int warningCount = 0;
            system.OnWarningIssued += model =>
            {
                warningCount++;
                Assert.Equal("commitment_test_warning", model.Id);
            };

            var events = new List<DayStateChangeEvent>();

            // Day 10: active, no warning yet
            system.TickDay(10, events);
            Assert.Equal(0, warningCount);
            Assert.Empty(events);

            // Day 11: warning day!
            system.TickDay(11, events);
            Assert.Equal(1, warningCount);
            Assert.Single(events);
            Assert.Equal("obligation_warning", events[0].Kind);

            // Day 12: still in warning status, but warning event does not duplicate
            events.Clear();
            system.TickDay(12, events);
            Assert.Equal(1, warningCount);
            Assert.Empty(events);

            var readModel = system.GetCommitment("commitment_test_warning", currentDay: 12);
            Assert.NotNull(readModel);
            Assert.Equal(CommitmentStatus.Warning, readModel.Status);
            Assert.Equal(3, readModel.DaysRemaining);
        }

        [Fact]
        public void RecordProgress_MarksMetExactlyOnce_WhenThresholdReached()
        {
            var system = new CommitmentSystem();
            system.RegisterCommitment(new CommitmentDefinition
            {
                id = "commitment_test_progress",
                type = "delivery",
                title = "Grain Delivery",
                start_day = 1,
                due_day = 10,
                target_quantity = 100
            });

            int metCount = 0;
            system.OnCommitmentMet += model =>
            {
                metCount++;
                Assert.Equal("commitment_test_progress", model.Id);
            };

            // Add partial progress
            bool completed1 = system.RecordProgress("commitment_test_progress", 40);
            Assert.False(completed1);
            Assert.Equal(0, metCount);

            var readModelPartial = system.GetCommitment("commitment_test_progress", currentDay: 3);
            Assert.NotNull(readModelPartial);
            Assert.Equal(CommitmentStatus.Active, readModelPartial.Status);
            Assert.Equal(40, readModelPartial.CurrentQuantity);

            // Add remaining to satisfy
            bool completed2 = system.RecordProgress("commitment_test_progress", 60);
            Assert.True(completed2);
            Assert.Equal(1, metCount);

            var readModelMet = system.GetCommitment("commitment_test_progress", currentDay: 3);
            Assert.NotNull(readModelMet);
            Assert.Equal(CommitmentStatus.Met, readModelMet.Status);

            // Further progress does not re-complete
            bool completed3 = system.RecordProgress("commitment_test_progress", 50);
            Assert.False(completed3);
            Assert.Equal(1, metCount);
        }

        [Fact]
        public void Settle_MarksMetImmediately()
        {
            var system = new CommitmentSystem();
            system.RegisterCommitment(new CommitmentDefinition
            {
                id = "commitment_test_settle",
                type = "tribute",
                title = "Immediate Settle",
                start_day = 1,
                due_day = 20,
                target_quantity = 200
            });

            bool settled = system.Settle("commitment_test_settle", currentDay: 5);
            Assert.True(settled);

            var readModel = system.GetCommitment("commitment_test_settle", currentDay: 5);
            Assert.NotNull(readModel);
            Assert.Equal(CommitmentStatus.Met, readModel.Status);
            Assert.Equal(200, readModel.CurrentQuantity);
        }

        [Fact]
        public void MissedDeadline_RoutesConsequence_AndNeverRefires()
        {
            var system = new CommitmentSystem();
            system.RegisterCommitment(new CommitmentDefinition
            {
                id = "commitment_test_missed",
                type = "treaty_term",
                title = "Water Quota",
                start_day = 1,
                due_day = 10,
                target_quantity = 50,
                consequence_class = "faction_standing_penalty",
                consequence_target = "highland_union",
                consequence_magnitude = -25
            });

            int missedCount = 0;
            string routedClass = string.Empty;
            string routedTarget = string.Empty;
            int routedMag = 0;

            system.OnCommitmentMissed += model =>
            {
                missedCount++;
            };

            system.OnConsequenceRouted += (cls, target, mag) =>
            {
                routedClass = cls;
                routedTarget = target;
                routedMag = mag;
            };

            var events = new List<DayStateChangeEvent>();

            // Day 10 is due day - not yet missed
            system.TickDay(10, events);
            Assert.Equal(0, missedCount);

            // Day 11 is past due day - Missed!
            events.Clear();
            system.TickDay(11, events);
            Assert.Equal(1, missedCount);
            Assert.Equal("faction_standing_penalty", routedClass);
            Assert.Equal("highland_union", routedTarget);
            Assert.Equal(-25, routedMag);
            Assert.Single(events);
            Assert.Equal("obligation_missed", events[0].Kind);

            // Day 12 does not refire
            events.Clear();
            system.TickDay(12, events);
            Assert.Equal(1, missedCount);
            Assert.Empty(events);

            var readModel = system.GetCommitment("commitment_test_missed", currentDay: 12);
            Assert.NotNull(readModel);
            Assert.Equal(CommitmentStatus.Missed, readModel.Status);
        }

        [Fact]
        public void SaveRestore_PreservesLedger_AndDoesNotRefireTerminalCommitments()
        {
            var system = new CommitmentSystem();
            var defMet = new CommitmentDefinition
            {
                id = "commitment_saved_met",
                type = "tribute",
                start_day = 1,
                due_day = 10,
                target_quantity = 10
            };
            var defMissed = new CommitmentDefinition
            {
                id = "commitment_saved_missed",
                type = "tribute",
                start_day = 1,
                due_day = 10,
                target_quantity = 10
            };
            system.RegisterCommitment(defMet);
            system.RegisterCommitment(defMissed);

            // Mark one met, let other expire
            system.RecordProgress("commitment_saved_met", 10);
            var events = new List<DayStateChangeEvent>();
            system.TickDay(11, events);

            var saveState = system.CaptureState();
            Assert.Contains("commitment_saved_met", saveState.met_ids);
            Assert.Contains("commitment_saved_missed", saveState.missed_ids);

            // Recreate new system and restore
            var restoredSystem = new CommitmentSystem();
            restoredSystem.RegisterCommitment(defMet);
            restoredSystem.RegisterCommitment(defMissed);

            int newMetCount = 0;
            int newMissedCount = 0;
            restoredSystem.OnCommitmentMet += _ => newMetCount++;
            restoredSystem.OnCommitmentMissed += _ => newMissedCount++;

            restoredSystem.RestoreState(saveState);

            var restoredEvents = new List<DayStateChangeEvent>();
            restoredSystem.TickDay(12, restoredEvents);

            // Zero refires on subsequent ticks!
            Assert.Equal(0, newMetCount);
            Assert.Equal(0, newMissedCount);
            Assert.Empty(restoredEvents);

            Assert.Equal(CommitmentStatus.Met, restoredSystem.GetCommitment("commitment_saved_met", 12)!.Status);
            Assert.Equal(CommitmentStatus.Missed, restoredSystem.GetCommitment("commitment_saved_missed", 12)!.Status);
        }

        [Fact]
        public void CommitmentsJson_LoadsCleanly_ThroughCatalogLoader()
        {
            string baseDir = AppContext.BaseDirectory;
            string dataDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data");
            if (!Directory.Exists(dataDir))
            {
                dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data");
            }

            if (Directory.Exists(dataDir))
            {
                var files = new FileSystemIO();
                var serializer = new SystemTextJsonSerializer();
                var result = CommitmentCatalogLoader.Load(dataDir, files, serializer);

                Assert.False(result.HasErrors, string.Join("; ", result.Errors));
                Assert.True(result.Commitments.Count >= 3);
            }
        }
    }
}
