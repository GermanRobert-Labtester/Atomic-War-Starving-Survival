// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    public class BlackMarketHeatAttentionEngineTests
    {
        [Fact]
        public void AddHeat_TransitionsThroughAttentionBands()
        {
            var engine = new BlackMarketHeatAttentionEngine();
            string syndicate = "syndicate_rust";

            var rec = engine.GetOrCreateRecord(syndicate, heatThreshold: 100);
            Assert.Equal(BlackMarketAttentionBand.Calm, rec.Band);

            // Add 60 heat -> Raised (>= 50)
            engine.AddHeat(syndicate, 60, "contraband_fence", currentDay: 1);
            Assert.Equal(BlackMarketAttentionBand.Raised, rec.Band);
            Assert.Equal(60, rec.CurrentHeat);

            // Add 50 more -> 110 heat -> Hot (>= 100)
            engine.AddHeat(syndicate, 50, "patrol_skirmish", currentDay: 2);
            Assert.Equal(BlackMarketAttentionBand.Hot, rec.Band);
            Assert.Equal(110, rec.CurrentHeat);

            // Add 45 more -> 155 heat -> CriticalLockout (>= 150)
            engine.AddHeat(syndicate, 45, "enforcer_ambush", currentDay: 3);
            Assert.Equal(BlackMarketAttentionBand.CriticalLockout, rec.Band);
            Assert.Equal(155, rec.CurrentHeat);
        }

        [Fact]
        public void DailyCooling_ReducesHeatDeterministically()
        {
            var engine = new BlackMarketHeatAttentionEngine();
            string syndicate = "syndicate_whisper";

            engine.AddHeat(syndicate, 80, "fence_gold", currentDay: 1);
            var rec = engine.GetOrCreateRecord(syndicate);
            Assert.Equal(80, rec.CurrentHeat);

            // Advance 4 days (cooling 5 per day -> 20 cooled -> 60 heat)
            engine.ProcessDailyTick(currentDay: 5, campaignSeed: 12345, dailyCooling: 5);
            Assert.Equal(60, rec.CurrentHeat);
            Assert.Equal(BlackMarketAttentionBand.Raised, rec.Band);

            // Advance 12 more days (60 cooled -> floors at 0 heat -> Calm)
            engine.ProcessDailyTick(currentDay: 17, campaignSeed: 12345, dailyCooling: 5);
            Assert.Equal(0, rec.CurrentHeat);
            Assert.Equal(BlackMarketAttentionBand.Calm, rec.Band);
        }

        [Fact]
        public void RaidRisk_CalculatesCorrectPermilleByBand()
        {
            var engine = new BlackMarketHeatAttentionEngine();
            string syndicate = "syndicate_black_cove";

            // Calm: 0 risk
            Assert.Equal(0, engine.GetRaidRiskPermille(syndicate));

            // Raised: 50 permille (5%)
            engine.AddHeat(syndicate, 60, "trade", 1);
            Assert.Equal(50, engine.GetRaidRiskPermille(syndicate));
            Assert.True(engine.CheckRaidTrigger(syndicate, 30)); // 30 < 50
            Assert.False(engine.CheckRaidTrigger(syndicate, 60)); // 60 >= 50

            // Hot: 250 permille (25%)
            engine.AddHeat(syndicate, 50, "trade", 1);
            Assert.Equal(250, engine.GetRaidRiskPermille(syndicate));

            // Critical: 600 permille (60%)
            engine.AddHeat(syndicate, 50, "trade", 1);
            Assert.Equal(600, engine.GetRaidRiskPermille(syndicate));
        }

        [Fact]
        public void Relocation_DeterministicLocationAndAntiReroll()
        {
            var engine = new BlackMarketHeatAttentionEngine();
            string syndicate = "syndicate_vault";

            // Seed 42 gives a deterministic location
            engine.TriggerRelocation(syndicate, currentDay: 10, campaignSeed: 42);
            var rec = engine.GetOrCreateRecord(syndicate);

            Assert.True(rec.IsRelocating);
            Assert.Equal(7, rec.RelocationDaysRemaining);
            Assert.Equal(1, rec.RelocationCount);
            string chosenLocation = rec.CurrentLocationKey;
            Assert.False(string.IsNullOrEmpty(chosenLocation));

            // Advancing 7 days completes relocation
            engine.ProcessDailyTick(currentDay: 17, campaignSeed: 42);
            Assert.False(rec.IsRelocating);
            Assert.Equal(0, rec.RelocationDaysRemaining);
            Assert.Equal(chosenLocation, rec.CurrentLocationKey); // Location is persistent

            // Second relocation under same seed increments count and chooses deterministically
            engine.TriggerRelocation(syndicate, currentDay: 20, campaignSeed: 42);
            Assert.Equal(2, rec.RelocationCount);
        }

        [Fact]
        public void SaveRestoreRoundTrip_PreservesAllSyndicateHeatState()
        {
            var engine1 = new BlackMarketHeatAttentionEngine();
            engine1.AddHeat("syndicate_a", 75, "fence", 5);
            engine1.TriggerRelocation("syndicate_b", currentDay: 6, campaignSeed: 999);

            var save = engine1.CaptureState();
            Assert.NotNull(save);
            Assert.Equal(2, save.Syndicates.Count);

            var engine2 = new BlackMarketHeatAttentionEngine();
            engine2.RestoreState(save);

            Assert.Equal(2, engine2.Records.Count);
            var recA = engine2.GetOrCreateRecord("syndicate_a");
            Assert.Equal(75, recA.CurrentHeat);
            Assert.Equal(BlackMarketAttentionBand.Raised, recA.Band);

            var recB = engine2.GetOrCreateRecord("syndicate_b");
            Assert.True(recB.IsRelocating);
            Assert.Equal(1, recB.RelocationCount);
        }
    }
}
