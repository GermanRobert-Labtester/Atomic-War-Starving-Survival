// SPDX-License-Identifier: MIT
// Expansion 32 — The Wild : WildlifeHarvestQuotaEngine focused tests
using Xunit;
using Ashfall.Core.World;

namespace Ashfall.Core.Tests.World
{
    public sealed class WildlifeHarvestQuotaEngineTests
    {
        // ── 1. Harvest is banned on a critically low population ──
        [Fact]
        public void EvaluateHarvestQuota_BansHarvest_WhenPopulationCriticallyLow()
        {
            var result = WildlifeHarvestQuotaEngine.EvaluateHarvestQuota(
                currentPopulationPermille: 80,  // <100 → CriticallyLow
                seasonalReproductionPermille: 50,
                requestedHarvestUnits: 5);

            Assert.Equal(0, result.MaxSafeHarvestUnits);
            Assert.False(result.IsWithinQuota);
            Assert.True(result.OverhuntCollapseRiskPermille > 800,
                "Critically low species should carry extreme overhunt risk");
        }

        // ── 2. Stable population allows a sustainable quota ──
        [Fact]
        public void EvaluateHarvestQuota_AllowsQuota_WhenPopulationStable()
        {
            var result = WildlifeHarvestQuotaEngine.EvaluateHarvestQuota(
                currentPopulationPermille: 750,  // Stable band
                seasonalReproductionPermille: 120,
                requestedHarvestUnits: 5);

            Assert.True(result.MaxSafeHarvestUnits > 0,
                "Stable population should allow some sustainable harvest");
            Assert.True(result.PostHarvestBand >= SpeciesPopulationBand.Recovering,
                "Small harvest on stable population should keep it recovering or better");
        }

        // ── 3. Predator conflict is Passive when shelter is far away ──
        [Fact]
        public void EvaluatePredatorConflict_Passive_WhenProximityVeryHigh()
        {
            var posture = WildlifeHarvestQuotaEngine.EvaluatePredatorConflict(
                predatorPopulationPermille: 800,
                proximityMetres: 2000,  // well beyond 300*3 = 900m
                shelterNoisePermille: 500);

            Assert.Equal(PredatorConflictPosture.Passive, posture);
        }

        // ── 4. High predator population at close range triggers Aggressive or Rampage ──
        [Fact]
        public void EvaluatePredatorConflict_Aggressive_WhenCloseHighPopulation()
        {
            var posture = WildlifeHarvestQuotaEngine.EvaluatePredatorConflict(
                predatorPopulationPermille: 900,
                proximityMetres: 150,   // within conflict proximity
                shelterNoisePermille: 700);

            Assert.True(posture >= PredatorConflictPosture.Aggressive,
                $"High population + close proximity should yield Aggressive or Rampage, got {posture}");
        }

        // ── 5. Untameable species (tamability=0) always returns not-tameable ──
        [Fact]
        public void EvaluateTamingReadiness_NotTameable_WhenSpeciesTamabilityZero()
        {
            var result = WildlifeHarvestQuotaEngine.EvaluateTamingReadiness(
                animalHungerPermille: 900,
                trustExposurePermille: 1000,
                speciesTamabilityPermille: 0,  // apex predator
                tamingSeed: 42);

            Assert.False(result.IsTameable,
                "Species with zero tamability should never be tameable");
            Assert.Equal(0, result.ReadinessPermille);
        }
    }
}
