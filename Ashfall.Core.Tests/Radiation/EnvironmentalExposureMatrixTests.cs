using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;
using Ashfall.Core.Radiation;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Acceptance tests for Task 42 — Environmental Radiation Exposure Matrix.
    /// Verifies that radiation dose is physics-derived from location, position, weather,
    /// and shelter shielding rather than hardcoded by survivor identity.
    /// </summary>
    public class EnvironmentalExposureMatrixTests
    {
        [Fact]
        public void SameSurvivor_ReceivesDifferentDose_InDifferentEnvironments()
        {
            var resolver = new ExposureEnvironmentResolver();
            resolver.ShelterAttenuationProvider = () => 0.5f; // 50% attenuation
            resolver.WeatherRadModifierProvider = () => 10f;  // light rain / ash

            string survivorId = "survivor_dr_sarah_chen";

            // 1. Shelter Interior
            resolver.SetSurvivorLocation(survivorId, SurvivorExposureLocation.ShelterInterior);
            var envShelter = resolver.Resolve(survivorId);
            Assert.Equal(SurvivorExposureLocation.ShelterInterior, envShelter.LocationKind);
            Assert.Equal(2.0f, envShelter.BaseRadRate);
            Assert.Equal(1.0f, envShelter.ShelterShielding); // 2.0 * 0.5
            Assert.Equal("Shelter Interior", envShelter.ExposureReason);

            // 2. Shelter Perimeter
            resolver.SetSurvivorLocation(survivorId, SurvivorExposureLocation.ShelterPerimeter);
            var envPerimeter = resolver.Resolve(survivorId);
            Assert.Equal(SurvivorExposureLocation.ShelterPerimeter, envPerimeter.LocationKind);
            Assert.Equal(20.0f, envPerimeter.BaseRadRate);
            Assert.Equal(10.0f, envPerimeter.WeatherRadModifier);
            Assert.Equal(30.0f, envPerimeter.EffectiveZoneRadLevel); // 20 + 10
            Assert.Equal(0f, envPerimeter.ShelterShielding);

            // 3. Wasteland Outdoors
            resolver.SetSurvivorLocation(survivorId, SurvivorExposureLocation.WastelandOutdoors);
            var envOutdoors = resolver.Resolve(survivorId);
            Assert.Equal(SurvivorExposureLocation.WastelandOutdoors, envOutdoors.LocationKind);
            Assert.Equal(40.0f, envOutdoors.BaseRadRate);
            Assert.Equal(50.0f, envOutdoors.EffectiveZoneRadLevel); // 40 + 10
            Assert.Equal(0f, envOutdoors.ShelterShielding);

            // 4. Expedition to high-rad site
            resolver.LocationRadRateProvider = loc => loc == "government_bunker" ? 60.0f : 25.0f;
            resolver.SetSurvivorLocation(survivorId, SurvivorExposureLocation.Expedition, "government_bunker");
            var envExpedition = resolver.Resolve(survivorId);
            Assert.Equal(SurvivorExposureLocation.Expedition, envExpedition.LocationKind);
            Assert.Equal(60.0f, envExpedition.BaseRadRate);
            Assert.Equal(70.0f, envExpedition.EffectiveZoneRadLevel); // 60 + 10
            Assert.Equal(0f, envExpedition.ShelterShielding);

            // Verify all four effective zone rates are distinctly different
            var distinctRates = new HashSet<float>
            {
                envShelter.EffectiveZoneRadLevel,
                envPerimeter.EffectiveZoneRadLevel,
                envOutdoors.EffectiveZoneRadLevel,
                envExpedition.EffectiveZoneRadLevel
            };
            Assert.Equal(4, distinctRates.Count);
        }

        [Fact]
        public void DifferentSurvivors_InSameEnvironment_ReceiveIdenticalBaseRate()
        {
            var resolver = new ExposureEnvironmentResolver();
            resolver.ShelterAttenuationProvider = () => 0.8f;
            resolver.WeatherRadModifierProvider = () => 25f;

            string[] testSurvivors =
            {
                "survivor_gunner_mikhail",
                "survivor_dr_sarah_chen",
                "elena_vasquez",
                "arbitrary_new_survivor_99"
            };

            // When all are in ShelterInterior
            foreach (var id in testSurvivors)
            {
                resolver.SetSurvivorLocation(id, SurvivorExposureLocation.ShelterInterior);
                var env = resolver.Resolve(id);
                Assert.Equal(2.0f, env.BaseRadRate);
                Assert.Equal(2.0f * 0.8f, env.ShelterShielding, precision: 3);
                Assert.Equal(2.0f, env.EffectiveZoneRadLevel);
                Assert.Equal("Shelter Interior", env.ExposureReason);
            }

            // When all are in WastelandOutdoors
            foreach (var id in testSurvivors)
            {
                resolver.SetSurvivorLocation(id, SurvivorExposureLocation.WastelandOutdoors);
                var env = resolver.Resolve(id);
                Assert.Equal(40.0f, env.BaseRadRate);
                Assert.Equal(25.0f, env.WeatherRadModifier);
                Assert.Equal(65.0f, env.EffectiveZoneRadLevel);
                Assert.Equal(0f, env.ShelterShielding);
            }
        }

        [Fact]
        public void NoSurvivorIdLiteral_ControlsRadiationDose()
        {
            var resolver = new ExposureEnvironmentResolver();
            resolver.ShelterAttenuationProvider = () => 0.6f;

            // Mikhail vs Sarah Chen in shelter: both must get exactly the same zone and shielding
            resolver.SetSurvivorLocation("survivor_gunner_mikhail", SurvivorExposureLocation.ShelterInterior);
            resolver.SetSurvivorLocation("survivor_dr_sarah_chen", SurvivorExposureLocation.ShelterInterior);

            var mikhailEnv = resolver.Resolve("survivor_gunner_mikhail");
            var sarahEnv = resolver.Resolve("survivor_dr_sarah_chen");

            Assert.Equal(mikhailEnv.EffectiveZoneRadLevel, sarahEnv.EffectiveZoneRadLevel);
            Assert.Equal(mikhailEnv.ShelterShielding, sarahEnv.ShelterShielding);
            Assert.Equal(mikhailEnv.BaseRadRate, sarahEnv.BaseRadRate);

            // Neither is 40f inside the shelter (which was the old Mikhail hardcode)
            Assert.NotEqual(40f, mikhailEnv.EffectiveZoneRadLevel);
            Assert.Equal(2f, mikhailEnv.EffectiveZoneRadLevel);
        }

        [Fact]
        public void ExposureReason_RecordedOnContextAndDosimeter()
        {
            var resolver = new ExposureEnvironmentResolver();
            resolver.WeatherRadModifierProvider = () => 150f; // Fallout storm
            resolver.SetSurvivorLocation("survivor_scout", SurvivorExposureLocation.WastelandOutdoors);

            var radState = new SurvivorRadState { Id = "survivor_scout" };
            var radSystem = new RadiationSystem(
                exposureContext: s => resolver.Resolve(s.Id).ToExposureContext());

            radSystem.Register(radState);
            radSystem.Tick(1.0f);

            // LastExposureReason must be recorded on both SurvivorRadState and Dosimeter
            Assert.False(string.IsNullOrEmpty(radState.LastExposureReason));
            Assert.Contains("Wasteland Surface", radState.LastExposureReason);
            Assert.Contains("150", radState.LastExposureReason);

            var dosimeter = radSystem.GetDosimeter("survivor_scout");
            Assert.Equal(radState.LastExposureReason, dosimeter.LastExposureReason);
            Assert.True(dosimeter.CurrentReading > 0f);
            Assert.True(dosimeter.LifetimeDose > 0f);
        }

        [Theory]
        [InlineData(WeatherKind.Clear, 0f)]
        [InlineData(WeatherKind.FalloutStorm, 150f)]
        [InlineData(WeatherKind.BlackRain, 250f)]
        [InlineData(WeatherKind.Ashfall, 0f)]
        [InlineData(WeatherKind.Blizzard, 0f)]
        public void SeededLocationWeatherExposureMatrix_ScalesOutdoors_WithoutBreachingIntactShelter(
            WeatherKind weatherKind, float expectedWeatherRadMod)
        {
            var weather = new WeatherSystem();
            weather.ForceWeather(weatherKind);

            var resolver = new ExposureEnvironmentResolver();
            resolver.ShelterAttenuationProvider = () => 0.75f;
            resolver.WeatherRadModifierProvider = () => weather.OutdoorRadModifier;

            // Assert weather outdoor rad modifier matches expectation
            Assert.Equal(expectedWeatherRadMod, weather.OutdoorRadModifier);

            // Inside shelter: weather does not breach intact shielding
            var indoorEnv = resolver.ResolveForEnvironment(SurvivorExposureLocation.ShelterInterior);
            Assert.Equal(2.0f, indoorEnv.BaseRadRate);
            Assert.Equal(0f, indoorEnv.WeatherRadModifier);
            Assert.Equal(1.5f, indoorEnv.ShelterShielding); // 2.0 * 0.75

            // Outside surface: base + weather modifier
            var outdoorEnv = resolver.ResolveForEnvironment(SurvivorExposureLocation.WastelandOutdoors);
            Assert.Equal(40.0f, outdoorEnv.BaseRadRate);
            Assert.Equal(expectedWeatherRadMod, outdoorEnv.WeatherRadModifier);
            Assert.Equal(40.0f + expectedWeatherRadMod, outdoorEnv.EffectiveZoneRadLevel);
        }

        [Fact]
        public void ExpeditionDestinations_ReflectCatalogRads()
        {
            var resolver = new ExposureEnvironmentResolver();
            var catalogRads = new Dictionary<string, float>
            {
                ["abandoned_hospital"] = 35f,
                ["government_bunker"] = 60f,
                ["rural_gas_station"] = 15f
            };
            resolver.LocationRadRateProvider = id => catalogRads.TryGetValue(id, out float r) ? r : 40f;

            var hospitalEnv = resolver.ResolveForEnvironment(SurvivorExposureLocation.Expedition, "abandoned_hospital");
            Assert.Equal(35f, hospitalEnv.BaseRadRate);
            Assert.Contains("abandoned_hospital", hospitalEnv.ExposureReason);

            var bunkerEnv = resolver.ResolveForEnvironment(SurvivorExposureLocation.Expedition, "government_bunker");
            Assert.Equal(60f, bunkerEnv.BaseRadRate);
            Assert.Contains("government_bunker", bunkerEnv.ExposureReason);

            var gasStationEnv = resolver.ResolveForEnvironment(SurvivorExposureLocation.Expedition, "rural_gas_station");
            Assert.Equal(15f, gasStationEnv.BaseRadRate);
            Assert.Contains("rural_gas_station", gasStationEnv.ExposureReason);
        }
    }
}
