using Ashfall.Core.Telemetry;
using Xunit;

namespace Ashfall.Core.Tests.Telemetry
{
    public class PlayableMetricsAggregationEngineTests
    {
        [Fact]
        public void Evaluate_ZeroCasualtiesAndHighResolution_AwardsExemplaryGrade()
        {
            var inputs = new SessionMetricInputs(
                daysSurvived: 30,
                peakPopulation: 20,
                casualtiesCount: 0,
                totalScavengeSorties: 25,
                totalResourcesHarvested: 500,
                totalWaterPurifiedLiters: 800,
                crisesResolved: 5,
                crisesFailed: 0,
                difficultyScalarPermille: 1000
            );

            var result = PlayableMetricsAggregationEngine.Evaluate(inputs);

            Assert.Equal(SessionReadinessGrade.A_Exemplary, result.Grade);
            Assert.False(result.HasCriticalFailure);
            Assert.True(result.SurvivalStabilityScorePermille >= 900);
            Assert.True(result.EfficiencyRatingPermille > 0);
            Assert.Contains("Exemplary shelter cohesion", result.SummaryDescription);
        }

        [Fact]
        public void Evaluate_HighCasualties_TriggersCollapsedGradeAndCriticalFailure()
        {
            var inputs = new SessionMetricInputs(
                daysSurvived: 15,
                peakPopulation: 10,
                casualtiesCount: 7, // 70% casualties >= 60% threshold
                totalScavengeSorties: 5,
                totalResourcesHarvested: 50,
                totalWaterPurifiedLiters: 100,
                crisesResolved: 1,
                crisesFailed: 3,
                difficultyScalarPermille: 1200
            );

            var result = PlayableMetricsAggregationEngine.Evaluate(inputs);

            Assert.Equal(SessionReadinessGrade.F_Collapsed, result.Grade);
            Assert.True(result.HasCriticalFailure);
            Assert.True(result.SurvivalStabilityScorePermille < 200);
            Assert.Contains("Catastrophic collapse", result.SummaryDescription);
        }

        [Fact]
        public void Evaluate_HardshipIndex_ScalesWithDifficultyModifierAndCrises()
        {
            var lowDiff = new SessionMetricInputs(
                daysSurvived: 20,
                peakPopulation: 10,
                casualtiesCount: 1,
                totalScavengeSorties: 10,
                totalResourcesHarvested: 200,
                totalWaterPurifiedLiters: 300,
                crisesResolved: 1,
                crisesFailed: 0,
                difficultyScalarPermille: 800
            );

            var highDiff = new SessionMetricInputs(
                daysSurvived: 20,
                peakPopulation: 10,
                casualtiesCount: 1,
                totalScavengeSorties: 10,
                totalResourcesHarvested: 200,
                totalWaterPurifiedLiters: 300,
                crisesResolved: 4,
                crisesFailed: 2,
                difficultyScalarPermille: 1500
            );

            var lowResult = PlayableMetricsAggregationEngine.Evaluate(lowDiff);
            var highResult = PlayableMetricsAggregationEngine.Evaluate(highDiff);

            Assert.True(highResult.HardshipIndexPermille > lowResult.HardshipIndexPermille);
        }

        [Fact]
        public void Evaluate_ZeroDaysSurvived_HandlesGracefullyWithoutDivisionByZero()
        {
            var inputs = new SessionMetricInputs(
                daysSurvived: 0,
                peakPopulation: 10,
                casualtiesCount: 0,
                totalScavengeSorties: 0,
                totalResourcesHarvested: 0,
                totalWaterPurifiedLiters: 0,
                crisesResolved: 0,
                crisesFailed: 0
            );

            var result = PlayableMetricsAggregationEngine.Evaluate(inputs);

            Assert.Equal(0, result.EfficiencyRatingPermille);
            Assert.False(result.HasCriticalFailure);
        }
    }
}
