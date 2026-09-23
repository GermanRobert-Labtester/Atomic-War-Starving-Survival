// SPDX-License-Identifier: MIT
// Expansion 31 — The Kiln : KilnFiringEngine focused tests
using Xunit;
using Ashfall.Core.Shelter;

namespace Ashfall.Core.Tests.Shelter
{
    public sealed class KilnFiringEngineTests
    {
        // ── 1. Fast ramp on cold load (stage 0, high temp) causes thermal shock ──
        [Fact]
        public void AdvanceFiringStage_CausesThermalShock_WhenRampedTooFast()
        {
            var batch = new KilnBatchState
            {
                BatchId       = "kiln-001",
                LoadKind      = KilnLoadKind.FiredBrick,
                RawMaterialQualityPermille = 700,
                FiringStage   = 0,
                HeatWorkPermille = 0  // cold load
            };

            // Ramp straight to maximum temperature on a cold load → thermal shock
            KilnFiringEngine.AdvanceFiringStage(batch, kilnTemperaturePermille: 950, fuelAvailablePermille: 1000);

            Assert.True(batch.IsWaster, "Fast ramp on cold load should produce a waster");
            Assert.Equal(DrawGrade.Waster, batch.ResultingGrade);
        }

        // ── 2. Gradual three-stage firing produces at least Standard grade ──
        [Fact]
        public void AdvanceFiringStage_ProducesStandardGrade_WhenGradualFire()
        {
            var batch = new KilnBatchState
            {
                BatchId       = "kiln-002",
                LoadKind      = KilnLoadKind.ClayPottery,
                RawMaterialQualityPermille = 800,
                FiringStage   = 0
            };

            // Stage 0: low temp to avoid thermal shock; Stage 1-2: build heat
            KilnFiringEngine.AdvanceFiringStage(batch, 400, 1000);
            KilnFiringEngine.AdvanceFiringStage(batch, 750, 1000);
            KilnFiringEngine.AdvanceFiringStage(batch, 900, 1000);

            Assert.False(batch.IsWaster, "Gradual firing should not produce a waster");
            Assert.Equal(KilnFiringEngine.MaxFiringStages, batch.FiringStage);
            Assert.True(batch.ResultingGrade >= DrawGrade.Standard,
                $"Expected ≥ Standard grade, got {batch.ResultingGrade}");
        }

        // ── 3. Lime calcination yields quicklime at sufficient temperature ──
        [Fact]
        public void CalcinateLimestone_YieldsQuicklime_WhenTemperatureSufficient()
        {
            var result = KilnFiringEngine.CalcinateLimestone(
                limestoneKg: 100,
                kilnTemperaturePermille: 850,
                soakHours: 24,
                fuelAvailablePermille: 1000);

            Assert.True(result.QuicklimeYieldKg > 0,
                "Should yield quicklime from limestone at 850 permille temperature");
            Assert.True(result.IsFullyCalcined || result.ResidualCarbonatePermille < 500,
                "Calcination should make significant progress at high temperature");
        }

        // ── 4. Insufficient temperature fails to calcinate limestone ──
        [Fact]
        public void CalcinateLimestone_YieldsZero_WhenTemperatureTooLow()
        {
            var result = KilnFiringEngine.CalcinateLimestone(
                limestoneKg: 100,
                kilnTemperaturePermille: 400, // below 700 threshold
                soakHours: 24,
                fuelAvailablePermille: 1000);

            Assert.Equal(0, result.QuicklimeYieldKg);
            Assert.False(result.IsFullyCalcined);
        }

        // ── 5. Refractory lining wear is higher for lime calcination than pottery ──
        [Fact]
        public void CalculateRefractoryLiningWear_LimeHigher_ThanPottery()
        {
            int limeWear    = KilnFiringEngine.CalculateRefractoryLiningWear(KilnLoadKind.LimestoneCalc, 900);
            int potteryWear = KilnFiringEngine.CalculateRefractoryLiningWear(KilnLoadKind.ClayPottery, 900);

            Assert.True(limeWear > potteryWear,
                "Lime calcination should cause more lining wear than pottery firing");
            Assert.True(limeWear > 0 && potteryWear > 0,
                "Both should have positive wear values");
        }
    }
}
