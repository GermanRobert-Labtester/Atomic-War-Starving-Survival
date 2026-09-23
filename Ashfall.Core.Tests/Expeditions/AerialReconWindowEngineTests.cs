using Ashfall.Core.Expeditions;
using Xunit;

namespace Ashfall.Core.Tests.Expeditions
{
    public class AerialReconWindowEngineTests
    {
        [Fact]
        public void Evaluate_OptimalConditions_ProducesOptimalWindowAndMinimalRisk()
        {
            var result = AerialReconWindowEngine.Evaluate(
                baseRangeKm: 200,
                airworthinessPermille: 1000,
                windSpeedKmh: 15,
                visibilityPermille: 950,
                temperatureCelsius: 18,
                payloadWeightKg: 20,
                maxPayloadKg: 100
            );

            Assert.Equal(FlightWindowCondition.Optimal, result.Condition);
            Assert.True(result.IsLaunchPermitted);
            Assert.True(result.TotalFlightRiskPermille < 150);
            Assert.True(result.EffectiveRangeKm > 160);
            Assert.True(result.AirdropDriftMeters <= 350);
            Assert.Contains("Optimal flight window", result.Advisory);
        }

        [Fact]
        public void Evaluate_SevereGaleAndZeroVisibility_ForcesGroundedStatus()
        {
            var result = AerialReconWindowEngine.Evaluate(
                baseRangeKm: 150,
                airworthinessPermille: 800,
                windSpeedKmh: 95, // Above 80 km/h threshold
                visibilityPermille: 50,  // Severe ash storm
                temperatureCelsius: 5,
                payloadWeightKg: 50,
                maxPayloadKg: 100
            );

            Assert.Equal(FlightWindowCondition.Grounded, result.Condition);
            Assert.False(result.IsLaunchPermitted);
            Assert.Equal(0, result.AirworthinessWearPermille); // No wear when flight aborted
            Assert.Contains("Flight aborted", result.Advisory);
        }

        [Fact]
        public void Evaluate_IcingZoneAndTurbulence_ClassifiesHazardous()
        {
            var result = AerialReconWindowEngine.Evaluate(
                baseRangeKm: 200,
                airworthinessPermille: 900,
                windSpeedKmh: 55,
                visibilityPermille: 350,
                temperatureCelsius: -3, // Prime icing zone
                payloadWeightKg: 40,
                maxPayloadKg: 100
            );

            Assert.Equal(FlightWindowCondition.Hazardous, result.Condition);
            Assert.True(result.IsLaunchPermitted);
            Assert.True(result.IcingRiskPermille >= 150);
            Assert.True(result.TotalFlightRiskPermille >= 250);
        }

        [Fact]
        public void Evaluate_CriticalAirworthiness_AbortsFlight()
        {
            var result = AerialReconWindowEngine.Evaluate(
                baseRangeKm: 100,
                airworthinessPermille: 150, // Below 200 threshold
                windSpeedKmh: 10,
                visibilityPermille: 900,
                temperatureCelsius: 20,
                payloadWeightKg: 10,
                maxPayloadKg: 100
            );

            Assert.Equal(FlightWindowCondition.Grounded, result.Condition);
            Assert.False(result.IsLaunchPermitted);
            Assert.Contains("critical structural degradation", result.Advisory);
        }

        [Fact]
        public void Evaluate_HeavyPayload_SignificantlyReducesEffectiveRange()
        {
            var lightResult = AerialReconWindowEngine.Evaluate(
                baseRangeKm: 200,
                airworthinessPermille: 1000,
                windSpeedKmh: 10,
                visibilityPermille: 900,
                temperatureCelsius: 20,
                payloadWeightKg: 0,
                maxPayloadKg: 100
            );

            var heavyResult = AerialReconWindowEngine.Evaluate(
                baseRangeKm: 200,
                airworthinessPermille: 1000,
                windSpeedKmh: 10,
                visibilityPermille: 900,
                temperatureCelsius: 20,
                payloadWeightKg: 100, // 100% capacity
                maxPayloadKg: 100
            );

            Assert.True(lightResult.EffectiveRangeKm > heavyResult.EffectiveRangeKm);
            Assert.Equal(192, lightResult.EffectiveRangeKm); // 200 * (1000 - 40) / 1000
            Assert.Equal(112, heavyResult.EffectiveRangeKm); // 200 * (1000 - 440) / 1000
        }
    }
}
