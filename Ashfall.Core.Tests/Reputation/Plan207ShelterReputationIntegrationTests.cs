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

        [Fact]
        public void AuthoredData_ReputationDimensionsJson_LoadsSuccessfully()
        {
            var dataDir = System.IO.Path.GetFullPath(
                System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,
                    "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));
            if (!System.IO.File.Exists(System.IO.Path.Combine(dataDir, "reputation_dimensions.json")))
                dataDir = System.IO.Path.GetFullPath("Assets/StreamingAssets/Data");

            string filePath = System.IO.Path.Combine(dataDir, "reputation_dimensions.json");
            Assert.True(System.IO.File.Exists(filePath), $"File not found: {filePath}");

            string json = System.IO.File.ReadAllText(filePath);
            var catalog = JsonSerializer.Deserialize<ReputationCatalogData>(json);
            Assert.NotNull(catalog);
            Assert.True(catalog!.dimensions.Count >= 5);
            Assert.True(catalog.tags.Count >= 8);

            foreach (var dim in catalog.dimensions)
            {
                Assert.False(string.IsNullOrWhiteSpace(dim.id));
                Assert.False(string.IsNullOrWhiteSpace(dim.display_name));
                Assert.False(string.IsNullOrWhiteSpace(dim.positive_title));
                Assert.False(string.IsNullOrWhiteSpace(dim.negative_title));
            }

            foreach (var tag in catalog.tags)
            {
                Assert.False(string.IsNullOrWhiteSpace(tag.id));
                Assert.False(string.IsNullOrWhiteSpace(tag.display_name));
            }

            var system = new ShelterReputationSystem();
            system.LoadCatalog(catalog);

            // Every authored dimension must resolve through the runtime authority —
            // this is the content-binding check the host catalog load depends on.
            foreach (var dim in catalog.dimensions)
            {
                Assert.True(Enum.TryParse<ReputationDimension>(dim.id, ignoreCase: true, out var parsed),
                    $"authored dimension id '{dim.id}' has no runtime enum counterpart");
                Assert.NotNull(system.GetDimensionDefinition(parsed));
            }

            Assert.Equal(catalog.dimensions.Count, system.DimensionDefinitions.Select(d => d.id).Distinct().Count());
        }

        [Fact]
        public void DimensionDailyDecay_UsesAuthoredCatalogValue()
        {
            // Plan 207 authority: the JSON catalog owns the decay magnitude.
            // (Shipped values are all 0.25, matching the previous hardcoded constant,
            // so this pins the seam rather than drifting behavior.)
            var catalog = new ReputationCatalogData
            {
                dimensions = new List<ReputationDimensionDef>
                {
                    new ReputationDimensionDef { id = "reliability", display_name = "Reliability", daily_decay = 1.0f },
                },
            };

            var system = new ShelterReputationSystem();
            system.LoadCatalog(catalog);
            system.RecordEvidence("evt_decay_probe", ReputationDimension.Reliability, 10f, InformationMedium.Witness, 1);

            float before = system.GetScore(ReputationDimension.Reliability);
            Assert.True(before > 1.0f, $"probe must start above the decay magnitude (was {before})");

            system.TickDay(2);
            Assert.Equal(before - 1.0f, system.GetScore(ReputationDimension.Reliability), precision: 3);

            // Unknown dimensions fall back to the documented default rather than 0.
            var bare = new ShelterReputationSystem();
            bare.RecordEvidence("evt_default_probe", ReputationDimension.Wealth, 10f, InformationMedium.Witness, 1);
            float wealthBefore = bare.GetScore(ReputationDimension.Wealth);
            bare.TickDay(2);
            Assert.Equal(wealthBefore - 0.25f, bare.GetScore(ReputationDimension.Wealth), precision: 3);
        }

        [Fact]
        public void TradePriceMultiplier_And_DominantTitle_ReflectActiveTags()
        {
            var dataDir = System.IO.Path.GetFullPath(
                System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,
                    "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));
            if (!System.IO.File.Exists(System.IO.Path.Combine(dataDir, "reputation_dimensions.json")))
                dataDir = System.IO.Path.GetFullPath("Assets/StreamingAssets/Data");

            string filePath = System.IO.Path.Combine(dataDir, "reputation_dimensions.json");
            string json = System.IO.File.ReadAllText(filePath);
            var catalog = JsonSerializer.Deserialize<ReputationCatalogData>(json);

            var system = new ShelterReputationSystem();
            system.LoadCatalog(catalog!);

            // Initial baseline: no tags, unknown title, 1.0x price
            Assert.Equal(1.0f, system.GetTradePriceMultiplier());
            Assert.Equal("Unknown Holdfast", system.GetDominantPerceptionTitle());

            // Build wealth and reliability to unlock TradingPost
            system.RecordEvidence("market_event_1", ReputationDimension.Wealth, 35f, InformationMedium.TraderWord, 1);
            system.RecordEvidence("market_event_2", ReputationDimension.Reliability, 25f, InformationMedium.TraderWord, 1);

            Assert.True(system.HasTag(ReputationTag.TradingPost));
            Assert.Equal("Wasteland Trading Post", system.GetDominantPerceptionTitle());
            // Trading post grants 100 permille (10%) discount -> 0.90x
            Assert.Equal(0.90f, system.GetTradePriceMultiplier(), precision: 2);

            // Record severe betrayal: Reliability drops drastically to <= -40 -> Treacherous
            system.RecordEvidence("betrayal_event", ReputationDimension.Reliability, -70f, InformationMedium.Witness, 2);

            Assert.True(system.HasTag(ReputationTag.Treacherous));
            // Dominant title should be Treacherous Holdfast
            Assert.Equal("Treacherous Holdfast", system.GetDominantPerceptionTitle());
            // Treacherous adds -250 permille (penalty) while TradingPost is revoked (Reliability < 20)
            Assert.False(system.HasTag(ReputationTag.TradingPost));
            // Multiplier = 1.0 - (-250/1000) = 1.25x
            Assert.Equal(1.25f, system.GetTradePriceMultiplier(), precision: 2);
        }
    }
}
