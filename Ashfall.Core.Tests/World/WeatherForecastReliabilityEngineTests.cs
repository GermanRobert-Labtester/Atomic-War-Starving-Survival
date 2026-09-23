using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public class WeatherForecastReliabilityEngineTests
    {
        [Fact]
        public void EvaluateReliability_CloseRange1Day_HighConfidence()
        {
            var result = WeatherForecastReliabilityEngine.EvaluateReliability(
                distanceKmFromStation: 15,
                leadTimeDays: 1,
                atmosphericInterferencePermille: 50,
                stationCalibrationPermille: 900
            );

            Assert.Equal(ForecastConfidenceGrade.High, result.Grade);
            Assert.True(result.IsReliableForDispatch);
            Assert.True(result.ReliabilityScorePermille >= 900);
            Assert.Contains("High barometric confidence", result.ReliabilitySummary);
        }

        [Fact]
        public void EvaluateReliability_LongDistanceAndHeavyInterference_Unusable()
        {
            var result = WeatherForecastReliabilityEngine.EvaluateReliability(
                distanceKmFromStation: 250, // Far from station
                leadTimeDays: 7,            // Week ahead
                atmosphericInterferencePermille: 850, // Ash storm
                stationCalibrationPermille: 400
            );

            Assert.Equal(ForecastConfidenceGrade.Unusable, result.Grade);
            Assert.False(result.IsReliableForDispatch);
            Assert.True(result.ReliabilityScorePermille < 350);
            Assert.Contains("Unusable radio telemetry", result.ReliabilitySummary);
        }

        [Fact]
        public void EvaluateReliability_LeadTimeDecay_ReducesAccuracyMonotonically()
        {
            var day1 = WeatherForecastReliabilityEngine.EvaluateReliability(30, 1, 100);
            var day3 = WeatherForecastReliabilityEngine.EvaluateReliability(30, 3, 100);
            var day7 = WeatherForecastReliabilityEngine.EvaluateReliability(30, 7, 100);

            Assert.True(day1.ReliabilityScorePermille > day3.ReliabilityScorePermille);
            Assert.True(day3.ReliabilityScorePermille > day7.ReliabilityScorePermille);
        }

        [Fact]
        public void EvaluateReliability_BoundaryClamping_HandlesNegativeInputs()
        {
            var result = WeatherForecastReliabilityEngine.EvaluateReliability(
                distanceKmFromStation: -50,
                leadTimeDays: 0,
                atmosphericInterferencePermille: -100,
                stationCalibrationPermille: 1500
            );

            Assert.True(result.ReliabilityScorePermille > 0);
            Assert.Equal(1, result.LeadTimeDays); // Clamped to min 1 day
        }
    }
}
