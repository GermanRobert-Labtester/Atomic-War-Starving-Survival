// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Campaign;
using Ashfall.Core.Commitments;
using Ashfall.Core.Radio;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    public sealed class Plan33_38IntelCalendarIntegrationTests
    {
        [Fact]
        public void Plan33_DistressStageAndFollowUp_DeterministicProgression()
        {
            // 1. DistressStageResolver: deterministic absolute campaign day fragment selection
            var signal = new DistressSignalDefinition
            {
                FrequencyId = "freq_distress_echo_bunker",
                FrequencyMhzStr = "102.4",
                SourceName = "Echo Bunker"
            };
            signal.MessageFragments.Add(new DistressMessageFragment
            {
                Day = 1,
                Text = "Weak carrier signal, coordinates obscured.",
                Clarity = 0.3f
            });
            signal.MessageFragments.Add(new DistressMessageFragment
            {
                Day = 5,
                Text = "Automated distress repeating: bunker seal compromised.",
                Clarity = 0.6f
            });
            signal.MessageFragments.Add(new DistressMessageFragment
            {
                Day = 10,
                Text = "Final survivors broadcasting: location 42-North 18-West.",
                Clarity = 0.95f
            });

            // Before Day 5: Stage 0 (Day 1)
            var stageAtDay3 = DistressStageResolver.Resolve(signal, 3);
            Assert.NotNull(stageAtDay3);
            Assert.Equal(0, stageAtDay3.Value.StageIndex);
            Assert.Equal(0.3f, stageAtDay3.Value.Fragment.Clarity);

            // Day 5 onwards: Stage 1
            var stageAtDay5 = DistressStageResolver.Resolve(signal, 5);
            Assert.NotNull(stageAtDay5);
            Assert.Equal(1, stageAtDay5.Value.StageIndex);
            Assert.Equal(0.6f, stageAtDay5.Value.Fragment.Clarity);

            // Day 12: Stage 2 (Day 10)
            var stageAtDay12 = DistressStageResolver.Resolve(signal, 12);
            Assert.NotNull(stageAtDay12);
            Assert.Equal(2, stageAtDay12.Value.StageIndex);
            Assert.Equal(0.95f, stageAtDay12.Value.Fragment.Clarity);

            // 2. DistressFollowUpScheduler: event-driven delayed follow-ups
            var radioSystem = new RadioDistressSystem();
            signal.FollowUpSignals.Add(new SignalFollowUpDefinition
            {
                Id = "followup_relief_acknowledgment",
                TriggerCondition = SignalFollowUpTriggers.RescueSuccess,
                DelayDays = 2,
                Text = "Survivors have reached safe perimeter. Transmission closing.",
                Clarity = 0.9f
            });
            radioSystem.RegisterSignal(signal);

            var missions = new DistressRescueMissionManager(null, radioSystem);
            var scheduler = new DistressFollowUpScheduler(radioSystem, missions);
            scheduler.BindToMissionEvents();

            var firedTransmissions = new List<(string Parent, SignalFollowUpDefinition FollowUp, int Day)>();
            scheduler.OnFollowUpFired += (parent, followUp, day) => firedTransmissions.Add((parent, followUp, day));

            // Set current day to Day 6 and schedule follow up for RescueSuccess (2 day delay -> due Day 8)
            scheduler.SetDay(6);
            int scheduledCount = scheduler.ScheduleForTrigger("freq_distress_echo_bunker", SignalFollowUpTriggers.RescueSuccess);
            Assert.Equal(1, scheduledCount);

            // Day 7: not yet due
            scheduler.TickDaily(7);
            Assert.Empty(firedTransmissions);

            // Day 8: fires exactly once
            scheduler.TickDaily(8);
            Assert.Single(firedTransmissions);
            Assert.Equal("freq_distress_echo_bunker", firedTransmissions[0].Parent);
            Assert.Equal("followup_relief_acknowledgment", firedTransmissions[0].FollowUp.Id);
            Assert.Equal(8, firedTransmissions[0].Day);

            // Tick again: exactly-once deduplication prevents refiring
            scheduler.TickDaily(9);
            Assert.Single(firedTransmissions);
        }

        [Fact]
        public void Plan38_CalendarSeasonsAndAmbientTemperature()
        {
            var calendar = new CampaignCalendar(initialDay: 28);

            string? transitionedFrom = null;
            string? transitionedTo = null;
            calendar.OnSeasonChanged += (oldSeason, newSeason) =>
            {
                transitionedFrom = oldSeason;
                transitionedTo = newSeason;
            };

            // Initial day 28: First Thaw
            var day28Model = calendar.CurrentReadModel;
            Assert.Equal(28, day28Model.Day);
            Assert.Equal("window_first_thaw", day28Model.SeasonId);
            Assert.Equal(1, day28Model.Year);

            // Advance across boundary to day 30
            calendar.SetDay(30);
            Assert.Equal("window_first_thaw", transitionedFrom);
            Assert.Equal("window_ash_settling", transitionedTo);

            var ashModel = calendar.CurrentReadModel;
            Assert.Equal("window_ash_settling", ashModel.SeasonId);

            // Deep freeze parity: at day 210, peak winter reaches around -45C
            var deepFreezeModel = calendar.ResolveDay(210);
            Assert.InRange(deepFreezeModel.AmbientTemperatureC, -46.0f, -44.0f);

            // Multi-year wrap: day 366 enters Year 2
            var year2Model = calendar.ResolveDay(366);
            Assert.Equal(2, year2Model.Year);
            Assert.Equal(2, year2Model.Chapter);
            Assert.Equal("window_first_thaw", year2Model.SeasonId);
        }

        [Fact]
        public void Plan38_CommitmentDeadlineAndConsequenceRouting()
        {
            var def = new CommitmentDefinition
            {
                id = "commitment_water_filter_relief",
                type = "treaty_term",
                title = "Water Purification Filter Supply",
                counterparty = "faction_grain_exchange",
                target_id = "item_water_filter",
                target_quantity = 5,
                start_day = 10,
                due_day = 20,
                warning_lead_days = 4,
                consequence_class = "faction_standing_penalty",
                consequence_target = "faction_grain_exchange",
                consequence_magnitude = -25
            };

            var system = new CommitmentSystem(new[] { def });

            bool warningIssued = false;
            system.OnWarningIssued += model =>
            {
                warningIssued = true;
                Assert.Equal("commitment_water_filter_relief", model.Id);
            };

            bool commitmentMet = false;
            system.OnCommitmentMet += model =>
            {
                commitmentMet = true;
                Assert.Equal(CommitmentStatus.Met, model.Status);
            };

            var events = new List<DayStateChangeEvent>();

            // Day 15: not yet warning window (20 - 4 = 16)
            system.TickDay(15, events);
            Assert.False(warningIssued);

            // Day 16: enters warning window -> warning emitted
            system.TickDay(16, events);
            Assert.True(warningIssued);

            // Record partial progress: 3 of 5 filters
            bool firstStep = system.RecordProgress("commitment_water_filter_relief", 3);
            Assert.False(firstStep); // Not yet complete
            var readModelPartial = system.GetCommitment("commitment_water_filter_relief", 16);
            Assert.NotNull(readModelPartial);
            Assert.Equal(3, readModelPartial.CurrentQuantity);

            // Record remaining 2 filters: meets target
            bool met = system.RecordProgress("commitment_water_filter_relief", 2);
            Assert.True(met);
            Assert.True(commitmentMet);
            Assert.Contains("commitment_water_filter_relief", system.MetIds);

            // Terminal commitment cannot accept further progress or flip state
            bool extraProgress = system.RecordProgress("commitment_water_filter_relief", 1);
            Assert.False(extraProgress);

            // Test failure/consequence routing on a missed commitment
            var defFail = new CommitmentDefinition
            {
                id = "commitment_antibiotics_urgent",
                type = "tribute",
                title = "Urgent Antibiotics Shipment",
                counterparty = "faction_supply_corps",
                target_id = "item_antibiotics",
                target_quantity = 10,
                start_day = 1,
                due_day = 5,
                warning_lead_days = 2,
                consequence_class = "economy_shock",
                consequence_target = "consequence_embargo_imposed",
                consequence_magnitude = -50
            };
            var systemFail = new CommitmentSystem(new[] { defFail });

            bool missedFired = false;
            string? routedConsequence = null;
            int routedMagnitude = 0;

            systemFail.OnCommitmentMissed += model => missedFired = true;
            systemFail.OnConsequenceRouted += (target, consequence, mag) =>
            {
                routedConsequence = consequence;
                routedMagnitude = mag;
            };

            // Advance past due day (day 6)
            var failEvents = new List<DayStateChangeEvent>();
            systemFail.TickDay(6, failEvents);
            Assert.True(missedFired);
            Assert.Contains("commitment_antibiotics_urgent", systemFail.MissedIds);
            Assert.Equal("consequence_embargo_imposed", routedConsequence);
            Assert.Equal(-50, routedMagnitude);

            // Save/Restore roundtrip
            var saveState = systemFail.CaptureState();
            var restored = new CommitmentSystem(new[] { defFail });
            restored.RestoreState(saveState);
            Assert.Contains("commitment_antibiotics_urgent", restored.MissedIds);
        }

        [Fact]
        public void Plan33_Plan38_CombinedEcosystem_WinterDistressDeadlineJourney()
        {
            // Scenario:
            // 1. Calendar is ticking towards harsh winter (Day 100).
            // 2. Radio distress signal is intercepted and staged across days.
            // 3. Shelter commits to a strict winter deadline obligation: deliver 6 medicine units by Day 105.
            // 4. Shelter completes the obligation on Day 104, meeting the commitment.
            // 5. Successful rescue registers a follow-up transmission on the radio network, scheduled for Day 106.
            // 6. At Day 106, the follow-up transmission fires, completing the narrative loop.

            var calendar = new CampaignCalendar(initialDay: 100);

            var distressSystem = new RadioDistressSystem();
            var signal = new DistressSignalDefinition
            {
                FrequencyId = "freq_distress_mount_outpost",
                FrequencyMhzStr = "106.1",
                SourceName = "Mountain Outpost Bravo"
            };
            signal.MessageFragments.Add(new DistressMessageFragment { Day = 98, Text = "Storm isolation, medical supplies exhausted.", Clarity = 0.5f });
            signal.MessageFragments.Add(new DistressMessageFragment { Day = 103, Text = "Freezing temperatures imminent, need urgent courier.", Clarity = 0.85f });
            signal.FollowUpSignals.Add(new SignalFollowUpDefinition
            {
                Id = "followup_courier_arrival_ack",
                TriggerCondition = SignalFollowUpTriggers.RescueSuccess,
                DelayDays = 2,
                Text = "Courier arrived with antibiotics. Outpost survived the frost.",
                Clarity = 1.0f
            });
            distressSystem.RegisterSignal(signal);

            var missions = new DistressRescueMissionManager(null, distressSystem);
            var scheduler = new DistressFollowUpScheduler(distressSystem, missions);

            var commitment = new CommitmentDefinition
            {
                id = "commitment_bravo_medical_relief",
                type = "rescue_aid",
                title = "Mountain Outpost Bravo Medical Relief",
                counterparty = "faction_frontier_freeholders",
                target_id = "item_antibiotics",
                target_quantity = 6,
                start_day = 100,
                due_day = 105,
                warning_lead_days = 2
            };
            var commitmentSystem = new CommitmentSystem(new[] { commitment });

            // Day 101: Calendar advances
            calendar.SetDay(101);
            var stageDay101 = DistressStageResolver.Resolve(signal, 101);
            Assert.NotNull(stageDay101);
            Assert.Equal(0, stageDay101.Value.StageIndex); // First fragment

            // Day 103: Second fragment audible
            calendar.SetDay(103);
            var stageDay103 = DistressStageResolver.Resolve(signal, 103);
            Assert.NotNull(stageDay103);
            Assert.Equal(1, stageDay103.Value.StageIndex); // Second fragment audible

            // Day 104: Courier delivers full quota of 6 antibiotics
            bool met = commitmentSystem.RecordProgress("commitment_bravo_medical_relief", 6);
            Assert.True(met);
            Assert.Contains("commitment_bravo_medical_relief", commitmentSystem.MetIds);

            // Mission completes -> triggers follow up scheduled for Day 106 (104 + 2)
            scheduler.SetDay(104);
            scheduler.ScheduleForTrigger("freq_distress_mount_outpost", SignalFollowUpTriggers.RescueSuccess);

            // Day 105: Deadline day, commitment remains Met
            calendar.SetDay(105);
            var events105 = new List<DayStateChangeEvent>();
            commitmentSystem.TickDay(105, events105);
            Assert.Contains("commitment_bravo_medical_relief", commitmentSystem.MetIds);
            Assert.DoesNotContain("commitment_bravo_medical_relief", commitmentSystem.MissedIds);

            // Day 106: Follow-up transmission fires on the radio network
            calendar.SetDay(106);
            bool followUpReceived = false;
            scheduler.OnFollowUpFired += (parent, followUp, day) =>
            {
                followUpReceived = true;
                Assert.Equal("freq_distress_mount_outpost", parent);
                Assert.Equal("followup_courier_arrival_ack", followUp.Id);
                Assert.Equal(106, day);
            };

            scheduler.TickDaily(106);
            Assert.True(followUpReceived);
        }
    }
}
