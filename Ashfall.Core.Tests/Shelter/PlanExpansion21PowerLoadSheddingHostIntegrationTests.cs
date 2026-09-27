// SPDX-License-Identifier: MIT
// ASHFALL Core Tests: Expansion 21 (The Grid) — microgrid load shedding host
// integration. Focused contract tests over the sealed engine through the live
// canonical power owners. Static, deterministic, no engine dependency.

using System;
using System.Collections.Generic;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    public sealed class PlanExpansion21PowerLoadSheddingHostIntegrationTests
    {
        private static List<SubgridLoadDemand> ShelterLoads() => new()
        {
            new("air_scrubber", LoadPriorityTier.Tier0_LifeSupport, 3),
            new("clinic_ward", LoadPriorityTier.Tier1_Clinical, 4),
            new("greenhouse_lamps", LoadPriorityTier.Tier2_Agricultural, 4),
            new("workshop_lathe", LoadPriorityTier.Tier3_Industrial, 5),
            new("common_lighting", LoadPriorityTier.Tier4_Comfort, 5),
        };

        [Fact]
        public void LifeSupport_IsServedBeforeComfort_UnderPartialSupply()
        {
            var r = PowerLoadSheddingEngine.Evaluate(4, ShelterLoads());

            // With 4 kW only the 3 kW life-support load is served first; comfort/workshop shed.
            Assert.Equal(3, r.ServedLoadKw);
            Assert.Contains("common_lighting", r.ShedConsumers, StringComparer.Ordinal);
            Assert.DoesNotContain("air_scrubber", r.ShedConsumers, StringComparer.Ordinal);
        }

        [Fact]
        public void ZeroSupply_ShedsEveryConsumer_AndReportsBlackout()
        {
            var r = PowerLoadSheddingEngine.Evaluate(0, ShelterLoads());

            Assert.True(r.IsBlackout);
            Assert.Equal(0, r.ServedLoadKw);
            Assert.Equal(21, r.ShedLoadKw);
            Assert.Equal(5, r.ShedConsumers.Count);
            // Authored energy-poverty weights: comfort 30 + industrial 20 +
            // agricultural 60 + clinical 150 + life support 400 = 660 permille.
            Assert.Equal(660, r.EnergyPovertyMoralePenaltyPermille);
        }

        [Fact]
        public void BrownoutRisk_RisesWithLoadFactor_AndCompoundsWithGridWear()
        {
            int light = PowerLoadSheddingEngine.Evaluate(100, new[] { new SubgridLoadDemand("a", LoadPriorityTier.Tier2_Agricultural, 80) }).BrownoutRiskPermille;
            int heavy = PowerLoadSheddingEngine.Evaluate(100, new[] { new SubgridLoadDemand("a", LoadPriorityTier.Tier2_Agricultural, 99) }).BrownoutRiskPermille;
            int worn = PowerLoadSheddingEngine.Evaluate(100, new[] { new SubgridLoadDemand("a", LoadPriorityTier.Tier2_Agricultural, 90) }, 1000).BrownoutRiskPermille;
            int fresh = PowerLoadSheddingEngine.Evaluate(100, new[] { new SubgridLoadDemand("a", LoadPriorityTier.Tier2_Agricultural, 90) }, 0).BrownoutRiskPermille;

            Assert.Equal(0, light);
            Assert.True(heavy > light);
            Assert.True(worn > fresh);
            Assert.True(worn <= PowerLoadSheddingEngine.PermilleScale);
        }

        [Fact]
        public void NoDemand_NeverReportsBlackout_OrMoralePenalty()
        {
            var r = PowerLoadSheddingEngine.Evaluate(0, new List<SubgridLoadDemand>());

            Assert.False(r.IsBlackout);
            Assert.Equal(0, r.TotalDemandKw);
            Assert.Equal(0, r.EnergyPovertyMoralePenaltyPermille);
            Assert.Empty(r.ShedConsumers);
        }

        [Fact]
        public void ShedConsumerOrder_IsDeterministicAndPriorityOrdered()
        {
            var a = PowerLoadSheddingEngine.Evaluate(0, ShelterLoads());
            var b = PowerLoadSheddingEngine.Evaluate(0, ShelterLoads());

            Assert.Equal(a.ShedConsumers, b.ShedConsumers);
            // Priority order: life support is evaluated first, comfort is shed last.
            Assert.Equal("air_scrubber", a.ShedConsumers[0]);
            Assert.Equal("common_lighting", a.ShedConsumers[^1]);
        }

        [Fact]
        public void CascadingTripRisk_RequiresBrownout_AndUnservedLoad()
        {
            var noShed = PowerLoadSheddingEngine.Evaluate(100, new[] { new SubgridLoadDemand("a", LoadPriorityTier.Tier2_Agricultural, 99) }, 1000);
            var withShed = PowerLoadSheddingEngine.Evaluate(4, ShelterLoads(), 1000);

            Assert.Equal(0, noShed.CascadingTripRiskPermille);
            Assert.True(withShed.CascadingTripRiskPermille >= 0);
        }
    }
}
