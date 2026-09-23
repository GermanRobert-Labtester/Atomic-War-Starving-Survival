using System.Collections.Generic;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    public class PowerLoadSheddingEngineTests
    {
        [Fact]
        public void Evaluate_SufficientPower_ServesAllLoadsAndZeroShed()
        {
            var demands = new[]
            {
                new SubgridLoadDemand("scrubber", LoadPriorityTier.Tier0_LifeSupport, 50),
                new SubgridLoadDemand("clinic", LoadPriorityTier.Tier1_Clinical, 30),
                new SubgridLoadDemand("lights", LoadPriorityTier.Tier4_Comfort, 20)
            };

            var result = PowerLoadSheddingEngine.Evaluate(
                availableGenerationKw: 200,
                demands: demands,
                gridWearPermille: 100
            );

            Assert.Equal(100, result.TotalDemandKw);
            Assert.Equal(100, result.ServedLoadKw);
            Assert.Equal(0, result.ShedLoadKw);
            Assert.Empty(result.ShedConsumers);
            Assert.False(result.IsBlackout);
            Assert.Equal(0, result.EnergyPovertyMoralePenaltyPermille);
        }

        [Fact]
        public void Evaluate_PowerDeficit_ShedsComfortFirst_PreservesLifeSupport()
        {
            var demands = new[]
            {
                new SubgridLoadDemand("lights", LoadPriorityTier.Tier4_Comfort, 20),
                new SubgridLoadDemand("workshop", LoadPriorityTier.Tier3_Industrial, 40),
                new SubgridLoadDemand("clinic", LoadPriorityTier.Tier1_Clinical, 30),
                new SubgridLoadDemand("air_scrubber", LoadPriorityTier.Tier0_LifeSupport, 50)
            };

            // Available 90 kW: Can serve LifeSupport (50) + Clinic (30) = 80 kW.
            // Remaining 10 kW cannot serve Workshop (40) or Lights (20).
            var result = PowerLoadSheddingEngine.Evaluate(
                availableGenerationKw: 90,
                demands: demands,
                gridWearPermille: 200
            );

            Assert.Equal(140, result.TotalDemandKw);
            Assert.Equal(80, result.ServedLoadKw);
            Assert.Equal(60, result.ShedLoadKw);
            Assert.Contains("lights", result.ShedConsumers);
            Assert.Contains("workshop", result.ShedConsumers);
            Assert.DoesNotContain("air_scrubber", result.ShedConsumers);
            Assert.DoesNotContain("clinic", result.ShedConsumers);
            Assert.True(result.EnergyPovertyMoralePenaltyPermille > 0);
        }

        [Fact]
        public void Evaluate_CompleteBlackout_TriggersBlackoutFlagAndHighMoralePenalty()
        {
            var demands = new[]
            {
                new SubgridLoadDemand("air_scrubber", LoadPriorityTier.Tier0_LifeSupport, 50),
                new SubgridLoadDemand("clinic", LoadPriorityTier.Tier1_Clinical, 30)
            };

            var result = PowerLoadSheddingEngine.Evaluate(
                availableGenerationKw: 0,
                demands: demands
            );

            Assert.True(result.IsBlackout);
            Assert.Equal(80, result.ShedLoadKw);
            Assert.Equal(0, result.ServedLoadKw);
            Assert.Equal(550, result.EnergyPovertyMoralePenaltyPermille); // 400 + 150
            Assert.Equal(1000, result.BrownoutRiskPermille);
        }

        [Fact]
        public void Evaluate_NearCapacityLoad_ElevatesBrownoutAndCascadingRisk()
        {
            var demands = new[]
            {
                new SubgridLoadDemand("substation_a", LoadPriorityTier.Tier2_Agricultural, 95),
                new SubgridLoadDemand("substation_b", LoadPriorityTier.Tier4_Comfort, 20)
            };

            var result = PowerLoadSheddingEngine.Evaluate(
                availableGenerationKw: 100, // 95 served out of 100 = 95% load factor (>85%)
                demands: demands,
                gridWearPermille: 500 // High wear
            );

            Assert.Equal(95, result.ServedLoadKw);
            Assert.Equal(20, result.ShedLoadKw);
            Assert.True(result.BrownoutRiskPermille > 500);
            Assert.True(result.CascadingTripRiskPermille > 0);
        }
    }
}
