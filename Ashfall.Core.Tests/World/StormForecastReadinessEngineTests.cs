// SPDX-License-Identifier: MIT
// Expansion 33 — The Weather : StormForecastReadinessEngine focused tests
using Xunit;
using Ashfall.Core.World;

namespace Ashfall.Core.Tests.World
{
    public sealed class StormForecastReadinessEngineTests
    {
        // ── 1. No forecast when observation post skill is below minimum ──
        [Fact]
        public void EvaluateForecastReliability_Unknown_WhenObservationSkillNearZero()
        {
            var result = StormForecastReadinessEngine.EvaluateForecastReliability(
                observationSkillPermille: 50,   // below 100 threshold
                leadTimeHours: 6,
                stormSeverity: StormSeverityClass.AshStorm,
                forecastSeed: 42);

            Assert.Equal(ForecastConfidenceTier.Unknown, result.ConfidenceTier);
            Assert.False(result.IssueWarning, "No warning should be issued with Unknown confidence");
        }

        // ── 2. Expert observer at short lead time reaches Reliable or HighConfidence ──
        [Fact]
        public void EvaluateForecastReliability_Reliable_WhenExpertObserverShortLead()
        {
            var result = StormForecastReadinessEngine.EvaluateForecastReliability(
                observationSkillPermille: 950,
                leadTimeHours: 4,   // short lead, minimal decay
                stormSeverity: StormSeverityClass.RainSquall,
                forecastSeed: 77);

            Assert.True(result.ConfidenceTier >= ForecastConfidenceTier.Reliable,
                $"Expert observer with short lead should reach Reliable+, got {result.ConfidenceTier}");
            Assert.True(result.IssueWarning,
                "Reliable forecast for a RainSquall should issue a warning");
        }

        // ── 3. Long lead time degrades confidence to Unreliable ──
        [Fact]
        public void EvaluateForecastReliability_Unreliable_WhenLeadTimeTooLong()
        {
            var result = StormForecastReadinessEngine.EvaluateForecastReliability(
                observationSkillPermille: 700,
                leadTimeHours: 72,  // 3 days ahead; massive decay
                stormSeverity: StormSeverityClass.AshStorm,
                forecastSeed: 13);

            Assert.True(result.ConfidenceTier <= ForecastConfidenceTier.Indicative,
                $"72h lead should yield Indicative or below, got {result.ConfidenceTier}");
        }

        // ── 4. Well-prepared shelter can absorb an AshStorm without gaps ──
        [Fact]
        public void AssessSeasonalReadiness_WellPrepared_WhenFullyEquipped()
        {
            var result = StormForecastReadinessEngine.AssessSeasonalReadiness(
                sealedAirlockPermille: 900,
                filterStockPermille: 900,
                medicalReadinessPermille: 850,
                drillRecencyPermille: 800,
                targetSeverity: StormSeverityClass.AshStorm);

            Assert.True(result.ReadinessBand >= SeasonalReadinessBand.WellPrepared,
                $"Fully equipped shelter should be WellPrepared+ against AshStorm, got {result.ReadinessBand}");
            Assert.True(string.IsNullOrEmpty(result.PrimaryGap),
                "WellPrepared shelter should have no primary gap");
        }

        // ── 5. Observation post decay is higher for basic instruments ──
        [Fact]
        public void CalculateObservationPostDecay_HigherForBasic_ThanPrecision()
        {
            int basicDecay    = StormForecastReadinessEngine.CalculateObservationPostDecay(100);
            int precisionDecay = StormForecastReadinessEngine.CalculateObservationPostDecay(900);

            Assert.True(basicDecay > precisionDecay,
                "Basic instruments should decay faster than precision instruments");
            Assert.True(precisionDecay > 0, "Even precision instruments have some daily decay");
        }
    }
}
