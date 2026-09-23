// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    public class MigrationConsequenceEngineTests
    {
        private SeasonalHumanMigrationEngine CreateEngineWithWeights(Dictionary<string, int> weights)
        {
            var engine = new SeasonalHumanMigrationEngine(knownRegions: weights.Keys);
            var saveState = engine.CaptureState();
            foreach (var kvp in weights)
            {
                saveState.RegionWeights[kvp.Key] = kvp.Value;
            }
            engine.RestoreState(saveState);
            return engine;
        }

        [Fact]
        public void MarketDemandMultiplier_ScalesWithPopulationWeight()
        {
            var weights = new Dictionary<string, int>
            {
                { "region_high_pop", 150 },
                { "region_norm_pop", 100 },
                { "region_low_pop", 60 }
            };

            var migration = CreateEngineWithWeights(weights);
            var consequence = new MigrationConsequenceEngine(migration);

            // Food demand: 150 weight -> 1500 permille; 100 weight -> 1000 permille; 60 weight -> 600 permille
            Assert.Equal(1500, consequence.GetMarketDemandMultiplierPermille("region_high_pop", "food"));
            Assert.Equal(1000, consequence.GetMarketDemandMultiplierPermille("region_norm_pop", "food"));
            Assert.Equal(600, consequence.GetMarketDemandMultiplierPermille("region_low_pop", "food"));

            // Labor demand: high pop depresses labor cost (excess labor supply)
            // 150 weight (delta +50) -> 1000 - 250 = 750 permille
            Assert.Equal(750, consequence.GetMarketDemandMultiplierPermille("region_high_pop", "labor"));
            // 60 weight (delta -40) -> 1000 - (-200) = 1200 permille (labor is scarce and expensive)
            Assert.Equal(1200, consequence.GetMarketDemandMultiplierPermille("region_low_pop", "labor"));
        }

        [Fact]
        public void LaborPoolAndTerritorialFriction_ScaleWithPopulationWeight()
        {
            var weights = new Dictionary<string, int>
            {
                { "region_dense", 160 },
                { "region_sparse", 70 }
            };

            var migration = CreateEngineWithWeights(weights);
            var consequence = new MigrationConsequenceEngine(migration);

            // Labor pool size: 160% of baseline in dense region
            Assert.Equal(1600, consequence.GetLaborPoolSizeMultiplierPermille("region_dense"));
            Assert.Equal(700, consequence.GetLaborPoolSizeMultiplierPermille("region_sparse"));

            // Friction: 1000 + (60 * 8) = 1480 permille in dense region
            Assert.Equal(1480, consequence.GetTerritorialFrictionMultiplierPermille("region_dense"));
            // 1000 + (-30 * 8) = 760 permille in sparse region
            Assert.Equal(760, consequence.GetTerritorialFrictionMultiplierPermille("region_sparse"));
        }

        [Fact]
        public void CaravanDemandPriority_DeterminesCorrectCategoryBias()
        {
            var weights = new Dictionary<string, int>
            {
                { "region_surge", 130 },
                { "region_stable", 100 },
                { "region_abandoned", 70 }
            };

            var migration = CreateEngineWithWeights(weights);
            var consequence = new MigrationConsequenceEngine(migration);

            Assert.Equal("food_and_fuel", consequence.GetCaravanDemandPriority("region_surge"));
            Assert.Equal("balanced_trade", consequence.GetCaravanDemandPriority("region_stable"));
            Assert.Equal("defense_and_labor", consequence.GetCaravanDemandPriority("region_abandoned"));
        }

        [Fact]
        public void TryApplyPhaseConsequence_EnforcesExactlyOnceKey()
        {
            var migration = new SeasonalHumanMigrationEngine(knownRegions: new[] { "valley" });
            var consequence = new MigrationConsequenceEngine(migration);

            int eventCount = 0;
            consequence.OnMigrationConsequenceApplied += (reg, phase, w, dem) => eventCount++;

            bool first = consequence.TryApplyPhaseConsequence(currentDay: 20, seasonPhase: "deep_winter", regionId: "valley");
            Assert.True(first);
            Assert.Equal(1, eventCount);

            // Second call on same day and phase must be rejected
            bool second = consequence.TryApplyPhaseConsequence(currentDay: 20, seasonPhase: "deep_winter", regionId: "valley");
            Assert.False(second);
            Assert.Equal(1, eventCount);

            // Different phase succeeds
            bool third = consequence.TryApplyPhaseConsequence(currentDay: 35, seasonPhase: "thaw", regionId: "valley");
            Assert.True(third);
            Assert.Equal(2, eventCount);
        }

        [Fact]
        public void SaveRestoreRoundTrip_PreservesAppliedConsequenceKeys()
        {
            var migration = new SeasonalHumanMigrationEngine(knownRegions: new[] { "region_1" });
            var consequence1 = new MigrationConsequenceEngine(migration);

            consequence1.TryApplyPhaseConsequence(10, "winter", "region_1");
            consequence1.TryApplyPhaseConsequence(25, "spring", "region_1");

            var saveState = consequence1.CaptureState();
            Assert.NotNull(saveState);
            Assert.Equal(2, saveState.AppliedConsequenceKeys.Count);

            var consequence2 = new MigrationConsequenceEngine(migration);
            consequence2.RestoreState(saveState);

            Assert.Equal(2, consequence2.AppliedConsequenceKeys.Count);
            // Re-applying preserved key must return false
            Assert.False(consequence2.TryApplyPhaseConsequence(10, "winter", "region_1"));
        }
    }
}
