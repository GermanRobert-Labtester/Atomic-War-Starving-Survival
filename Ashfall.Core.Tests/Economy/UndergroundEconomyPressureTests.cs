// SPDX-License-Identifier: MIT
using System;
using Xunit;
using Ashfall.Core.Economy;

namespace Ashfall.Core.Tests.Economy
{
    public sealed class UndergroundEconomyPressureTests
    {
        [Fact]
        public void Evaluate_Calm_WhenHeatLow()
        {
            var pressure = UndergroundEconomyPressure.Evaluate(currentHeat: 20, heatThreshold: 100);

            Assert.Equal(MarketTemperatureBand.Calm, pressure.Band);
            Assert.Equal(1.0f, pressure.PricePressureMultiplier);
            Assert.Equal(1.0f, pressure.AttentionRiskMultiplier);
            Assert.Contains("Discreet", pressure.StatusSummary);
            Assert.False(pressure.IsRelocated);
        }

        [Fact]
        public void Evaluate_Raised_WhenHeatAboveHalfThreshold()
        {
            var pressure = UndergroundEconomyPressure.Evaluate(currentHeat: 55, heatThreshold: 100);

            Assert.Equal(MarketTemperatureBand.Raised, pressure.Band);
            Assert.Equal(1.15f, pressure.PricePressureMultiplier);
            Assert.Equal(1.5f, pressure.AttentionRiskMultiplier);
            Assert.Contains("Noticed", pressure.StatusSummary);
        }

        [Fact]
        public void Evaluate_Hot_WhenHeatReachesThreshold()
        {
            var pressure = UndergroundEconomyPressure.Evaluate(currentHeat: 100, heatThreshold: 100);

            Assert.Equal(MarketTemperatureBand.Hot, pressure.Band);
            Assert.Equal(1.35f, pressure.PricePressureMultiplier);
            Assert.Equal(2.5f, pressure.AttentionRiskMultiplier);
            Assert.Contains("Heavy authority scrutiny", pressure.StatusSummary);
        }

        [Fact]
        public void Evaluate_Relocated_OverridesHeatState()
        {
            var pressure = UndergroundEconomyPressure.Evaluate(currentHeat: 10, heatThreshold: 100, isRelocated: true);

            Assert.Equal(MarketTemperatureBand.Relocated, pressure.Band);
            Assert.Equal(1.50f, pressure.PricePressureMultiplier);
            Assert.Equal(3.0f, pressure.AttentionRiskMultiplier);
            Assert.Contains("dispersed", pressure.StatusSummary);
            Assert.True(pressure.IsRelocated);
        }

        [Fact]
        public void Evaluate_NegativeHeatOrZeroThreshold_ClampedSafely()
        {
            var pressure = UndergroundEconomyPressure.Evaluate(currentHeat: -20, heatThreshold: 0);

            Assert.Equal(0, pressure.CurrentHeat);
            Assert.Equal(1, pressure.HeatThreshold);
            Assert.Equal(MarketTemperatureBand.Calm, pressure.Band);
        }
    }
}
