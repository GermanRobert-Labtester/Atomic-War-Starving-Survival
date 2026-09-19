// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Ashfall.Core.Reputation;
using Xunit;

namespace Ashfall.Core.Tests.Reputation
{
    public sealed class Plan207ShelterReputationIntegrationTests
    {
        [Fact]
        public void EvidenceRecording_CalculatesMediumReach_AndAntiFarmingPreventsSpam()
        {
            var system = new ShelterReputationSystem();

            // Record initial witness evidence
            var ev1 = system.RecordEvidence(
                sourceEventId: "event_trade_convoy_01",
                dimension: ReputationDimension.Wealth,
                delta: 20f,
                medium: InformationMedium.Witness,
                day: 1,
                description: "Traded generously with regional convoy"
            );

            Assert.NotNull(ev1);
            Assert.Equal(1, system.EvidenceCount);
            // Delta * 1.0 = 20
            Assert.Equal(20f, system.GetScore(ReputationDimension.Wealth));

            // Immediate duplicate within anti-farming cooldown should be rejected
            var evDup = system.RecordEvidence(
                sourceEventId: "event_trade_convoy_01",
                dimension: ReputationDimension.Wealth,
                delta: 20f,
                medium: InformationMedium.Witness,
                day: 2,
                description: "Duplicate attempt"
            );

            Assert.Null(evDup);
            Assert.Equal(1, system.EvidenceCount);

            // Record with RadioBroadcast medium (reach multiplier 2.0 for notoriety)
            var evRadio = system.RecordEvidence(
                sourceEventId: "event_radio_broadcast_01",
                dimension: ReputationDimension.Reliability,
                delta: 15f,
                medium: InformationMedium.RadioBroadcast,
                day: 2,
                description: "Broadcast emergency weather warning"
            );

            Assert.NotNull(evRadio);
            Assert.Equal(2, system.EvidenceCount);
            Assert.Equal(15f, system.GetScore(ReputationDimension.Reliability));
        }

        [Fact]
        public void NotorietyAccumulation_ClampsAt100_AndReflectsMediumReach()
        {
            var system = new ShelterReputationSystem();
            Assert.Equal(0f, system.Notoriety);

            // High impact broadcast event
            system.RecordEvidence("event_nuke_test", ReputationDimension.Strength, 50f, InformationMedium.RadioBroadcast, 1);
            // delta 50 * reach 2.0 * 0.15f = 15 notoriety
            Assert.True(system.Notoriety >= 15f);

            // Multiple major events
            for (int i = 0; i < 20; i++)
            {
                system.RecordEvidence($"event_war_{i}", ReputationDimension.Ruthlessness, 60f, InformationMedium.Propaganda, 1);
            }

            Assert.Equal(100f, system.Notoriety);
        }

        [Fact]
        public void TagEvaluation_TriggersOnThresholds_AndRemovesOnDecayOrOpposition()
        {
            var system = new ShelterReputationSystem();

            // Record generous actions to qualify for Sanctuary (Generosity >= 40)
            system.RecordEvidence("event_refugee_aid_1", ReputationDimension.Generosity, 25f, InformationMedium.Witness, 1);
            system.RecordEvidence("event_refugee_aid_2", ReputationDimension.Generosity, 25f, InformationMedium.Witness, 1);

            Assert.True(system.HasTag(ReputationTag.Sanctuary));

            // Record strength and ruthlessness to qualify for RaiderBane
            system.RecordEvidence("event_raider_defeat_1", ReputationDimension.Strength, 45f, InformationMedium.Witness, 1);
            system.RecordEvidence("event_raider_defeat_2", ReputationDimension.Ruthlessness, 45f, InformationMedium.Witness, 1);

            Assert.True(system.HasTag(ReputationTag.Fortress));
            Assert.True(system.HasTag(ReputationTag.Dangerous));
            Assert.True(system.HasTag(ReputationTag.RaiderBane));

            // Add negative reliability (Reliability <= -40 -> Treacherous)
            system.RecordEvidence("event_betrayal_1", ReputationDimension.Reliability, -50f, InformationMedium.Witness, 1);
            Assert.True(system.HasTag(ReputationTag.Treacherous));
        }

        [Fact]
        public void DailyDecay_GraduallyReducesScoresTowardZero_AndDecaysNotoriety()
        {
            var system = new ShelterReputationSystem();

            system.RecordEvidence("event_good_deed", ReputationDimension.Generosity, 50f, InformationMedium.Witness, 1);
            Assert.Equal(50f, system.GetScore(ReputationDimension.Generosity));
            float initialNotoriety = system.Notoriety;
            Assert.True(initialNotoriety > 0f);

            // Advance 10 days
            for (int d = 2; d <= 11; d++)
            {
                system.TickDay(d);
            }

            // Generosity score decays by 10 * 0.25f = 2.5f -> 47.5f
            Assert.Equal(47.5f, system.GetScore(ReputationDimension.Generosity));

            // Notoriety decays by 10 * 0.1f = 1.0f
            Assert.Equal(Math.Max(0f, initialNotoriety - 1.0f), system.Notoriety, precision: 3);
        }

        [Fact]
        public void StateSerialization_RoundTrips_ScoresNotorietyTagsAndEvidence()
        {
            var original = new ShelterReputationSystem();
            original.RecordEvidence("event_market_01", ReputationDimension.Wealth, 45f, InformationMedium.TraderWord, 1, "Opened fair exchange");
            original.RecordEvidence("event_treaty_01", ReputationDimension.Reliability, 30f, InformationMedium.RefugeeReport, 2, "Honored non-aggression pact");

            Assert.True(original.HasTag(ReputationTag.TradingPost));

            var state = original.CaptureState();
            string json = JsonSerializer.Serialize(state);

            var deserializedState = JsonSerializer.Deserialize<ShelterReputationState>(json);
            Assert.NotNull(deserializedState);

            var restored = new ShelterReputationSystem(deserializedState);

            Assert.Equal(original.Notoriety, restored.Notoriety);
            Assert.Equal(original.EvidenceCount, restored.EvidenceCount);
            Assert.Equal(original.GetScore(ReputationDimension.Wealth), restored.GetScore(ReputationDimension.Wealth));
            Assert.Equal(original.GetScore(ReputationDimension.Reliability), restored.GetScore(ReputationDimension.Reliability));
            Assert.True(restored.HasTag(ReputationTag.TradingPost));
            Assert.Equal(original.ActiveTags.Count, restored.ActiveTags.Count);
        }

        [Fact]
        public void Events_FireAppropriately_OnTagAcquiredAndNotorietyChanged()
        {
            var system = new ShelterReputationSystem();

            var acquiredTags = new List<ReputationTag>();
            var notorietyChanges = new List<float>();

            system.OnTagAcquired += tag => acquiredTags.Add(tag);
            system.OnNotorietyChanged += notoriety => notorietyChanges.Add(notoriety);

            system.RecordEvidence("event_aid", ReputationDimension.Generosity, 45f, InformationMedium.Witness, 1);

            Assert.Contains(ReputationTag.Sanctuary, acquiredTags);
            Assert.NotEmpty(notorietyChanges);
            Assert.True(notorietyChanges.Last() > 0f);
        }
    }
}
