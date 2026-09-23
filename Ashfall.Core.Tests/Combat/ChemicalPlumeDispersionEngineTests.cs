// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Combat;
using Xunit;

namespace Ashfall.Core.Tests.Combat
{
    public sealed class ChemicalPlumeDispersionEngineTests
    {
        [Fact]
        public void AdvancePlumeDispersion_WindDriftsPlumeAcrossSectors()
        {
            var plume = new ChemicalPlumeState
            {
                PlumeId = "plume_chlorine_01",
                AgentId = "agent_chlorine_sludge",
                SectorX = 5,
                SectorY = 5,
                DensityPermille = 800,
                RemainingLifespanTicks = 20,
                ToxicityTier = PlumeToxicityTier.Severe
            };

            // North wind (0 degrees) at 30 km/h with no rain
            var weather = new WeatherDispersionVector(30, 0, 0);

            ChemicalPlumeDispersionEngine.AdvancePlumeDispersion(plume, weather);

            Assert.Equal(5, plume.SectorX);
            Assert.Equal(6, plume.SectorY);
            Assert.True(plume.DensityPermille < 800);
            Assert.Equal(19, plume.RemainingLifespanTicks);
        }

        [Fact]
        public void AdvancePlumeDispersion_PrecipitationAcceleratesWashout()
        {
            var dryPlume = new ChemicalPlumeState
            {
                DensityPermille = 600,
                RemainingLifespanTicks = 10
            };
            var wetPlume = new ChemicalPlumeState
            {
                DensityPermille = 600,
                RemainingLifespanTicks = 10
            };

            var dryWeather = new WeatherDispersionVector(0, 0, 0);
            var wetWeather = new WeatherDispersionVector(0, 0, 800); // Heavy precipitation

            ChemicalPlumeDispersionEngine.AdvancePlumeDispersion(dryPlume, dryWeather);
            ChemicalPlumeDispersionEngine.AdvancePlumeDispersion(wetPlume, wetWeather);

            Assert.True(wetPlume.DensityPermille < dryPlume.DensityPermille,
                $"Wet plume ({wetPlume.DensityPermille}) should dissipate faster than dry plume ({dryPlume.DensityPermille})");
        }

        [Fact]
        public void EvaluateShelterAirInfiltration_PoweredFiltrationScrubsToxicity()
        {
            const int outdoorDensity = 800;
            const PlumeToxicityTier toxicity = PlumeToxicityTier.Severe;

            var poweredResult = ChemicalPlumeDispersionEngine.EvaluateShelterAirInfiltration(
                outdoorDensity,
                toxicity,
                isFiltrationPowered: true,
                shelterFilterConditionPermille: 900);

            var unpoweredResult = ChemicalPlumeDispersionEngine.EvaluateShelterAirInfiltration(
                outdoorDensity,
                toxicity,
                isFiltrationPowered: false,
                shelterFilterConditionPermille: 900);

            Assert.True(poweredResult.IndoorContaminantDensityPermille < unpoweredResult.IndoorContaminantDensityPermille);
            Assert.Equal(AirQualityBand.Tainted, poweredResult.IndoorAirQuality);
            Assert.True(poweredResult.FilterWearDeltaPermille > 0);
            Assert.False(poweredResult.AirlockBreachWarning);

            Assert.True(unpoweredResult.AirlockBreachWarning);
            Assert.Equal(AirQualityBand.Hazardous, unpoweredResult.IndoorAirQuality);
        }

        [Fact]
        public void EvaluateRespiratorProtection_CanisterFiltersToxicity_UntilDegraded()
        {
            const int ambientDensity = 700;
            const PlumeToxicityTier toxicity = PlumeToxicityTier.Elevated;

            // Fresh canister
            var freshResult = ChemicalPlumeDispersionEngine.EvaluateRespiratorProtection(
                ambientDensity,
                toxicity,
                canisterConditionPermille: 800);

            Assert.True(freshResult.Protected);
            Assert.Equal(0, freshResult.EffectiveExposureDose);
            Assert.False(freshResult.CanisterDepleted);
            Assert.True(freshResult.CanisterWearDeltaPermille > 0);

            // Depleted canister
            var depletedResult = ChemicalPlumeDispersionEngine.EvaluateRespiratorProtection(
                ambientDensity,
                toxicity,
                canisterConditionPermille: 0);

            Assert.False(depletedResult.Protected);
            Assert.True(depletedResult.EffectiveExposureDose > 50);
            Assert.True(depletedResult.CanisterDepleted);
        }

        [Fact]
        public void EvaluateRespiratorProtection_ZeroDensity_ReturnsProtectedWithZeroWear()
        {
            var result = ChemicalPlumeDispersionEngine.EvaluateRespiratorProtection(
                ambientDensityPermille: 0,
                toxicity: PlumeToxicityTier.Lethal,
                canisterConditionPermille: 500);

            Assert.True(result.Protected);
            Assert.Equal(0, result.CanisterWearDeltaPermille);
            Assert.Equal(0, result.EffectiveExposureDose);
            Assert.False(result.CanisterDepleted);
        }
    }
}
