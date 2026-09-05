using System;
using System.Collections.Generic;
using Ashfall.Core.Radiation;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Journeys
{
    /// <summary>
    /// Journey J42: Environmental Radiation Exposure Simulation.
    /// Proves end-to-end multi-day physics where survivor position, storm resuspension,
    /// expedition destination, and shelter shielding dictate radiation dose rather than survivor identity.
    /// </summary>
    public class EnvironmentalExposureJourneyTests
    {
        [Fact]
        public void JourneyJ42_EnvironmentalExposure_MultiDaySimulation_ProvesPhysicsOverIdentity()
        {
            // --- Setup ---
            var resolver = new ExposureEnvironmentResolver();
            resolver.ShelterAttenuationProvider = () => 0.5f; // 50% shelter attenuation

            var catalogRads = new Dictionary<string, float>(StringComparer.Ordinal)
            {
                ["abandoned_hospital"] = 35f,
                ["rural_gas_station"] = 15f
            };
            resolver.LocationRadRateProvider = id => catalogRads.TryGetValue(id, out float r) ? r : 40f;

            var weather = new WeatherSystem();
            weather.ForceWeather(WeatherKind.Clear);
            resolver.WeatherRadModifierProvider = () => weather.OutdoorRadModifier;

            var radSystem = new RadiationSystem(
                exposureContext: s => resolver.Resolve(s.Id).ToExposureContext());

            var chen = new SurvivorRadState { Id = "survivor_dr_sarah_chen" };
            var mikhail = new SurvivorRadState { Id = "survivor_gunner_mikhail" };
            var elena = new SurvivorRadState { Id = "elena_vasquez" };

            radSystem.Register(chen);
            radSystem.Register(mikhail);
            radSystem.Register(elena);

            // === DAY 1: Clear skies, all survivors sheltered ===
            // All 3 survivors are in ShelterInterior.
            resolver.SetSurvivorLocation(chen.Id, SurvivorExposureLocation.ShelterInterior);
            resolver.SetSurvivorLocation(mikhail.Id, SurvivorExposureLocation.ShelterInterior);
            resolver.SetSurvivorLocation(elena.Id, SurvivorExposureLocation.ShelterInterior);

            // Tick 24 hours
            radSystem.Tick(24f);

            // In shelter: Zone=2, Shielding=1, Effective=1 mSv/h. Over 24h = 24 mSv.
            // All three survivors must have accumulated the EXACT same dose.
            Assert.Equal(chen.RadiationDose, mikhail.RadiationDose, precision: 2);
            Assert.Equal(chen.RadiationDose, elena.RadiationDose, precision: 2);
            Assert.Equal(24f, chen.RadiationDose, precision: 2);
            Assert.Equal("Shelter Interior", chen.LastExposureReason);
            Assert.Equal("Shelter Interior", mikhail.LastExposureReason);

            // === DAY 2: Fallout storm strikes (+150 mSv/h outdoor resuspension) ===
            weather.ForceWeather(WeatherKind.FalloutStorm);
            Assert.Equal(150f, weather.OutdoorRadModifier);

            // Chen stays inside shelter.
            // Mikhail is deployed to the shelter perimeter.
            // Elena departs on an expedition to abandoned_hospital (base 35 mSv/h).
            resolver.SetSurvivorLocation(chen.Id, SurvivorExposureLocation.ShelterInterior);
            resolver.SetSurvivorLocation(mikhail.Id, SurvivorExposureLocation.ShelterPerimeter);
            resolver.SetSurvivorLocation(elena.Id, SurvivorExposureLocation.Expedition, "abandoned_hospital");

            float chenDoseBefore = chen.RadiationDose;
            float mikhailDoseBefore = mikhail.RadiationDose;
            float elenaDoseBefore = elena.RadiationDose;

            // Tick 2 hours during the storm
            radSystem.Tick(2f);

            float chenDelta = chen.RadiationDose - chenDoseBefore;
            float mikhailDelta = mikhail.RadiationDose - mikhailDoseBefore;
            float elenaDelta = elena.RadiationDose - elenaDoseBefore;

            // Chen inside shelter: (2.0 zone - 1.0 shielding) * 2h = 2.0 mSv
            Assert.Equal(2.0f, chenDelta, precision: 2);
            Assert.Equal("Shelter Interior", chen.LastExposureReason);

            // Mikhail on perimeter: (20 base + 150 storm - 0 shielding) * 2h = 340.0 mSv (clamped to 100 max acute dose)
            Assert.True(mikhailDelta > chenDelta);
            Assert.Contains("Shelter Perimeter", mikhail.LastExposureReason);
            Assert.Contains("150", mikhail.LastExposureReason);

            // Elena on expedition: (35 base + 150 storm - 0 shielding) * 2h = 370.0 mSv
            Assert.True(elenaDelta > chenDelta);
            Assert.Contains("abandoned_hospital", elena.LastExposureReason);
            Assert.Contains("150", elena.LastExposureReason);

            // === DAY 3: Storm passes, Mikhail and Elena return to shelter ===
            weather.ForceWeather(WeatherKind.Clear);
            resolver.SetSurvivorLocation(mikhail.Id, SurvivorExposureLocation.ShelterInterior);
            resolver.SetSurvivorLocation(elena.Id, SurvivorExposureLocation.ShelterInterior);

            // Reset doses to non-clamped values to verify identical rate of change
            mikhail.RadiationDose = 10f;
            chen.RadiationDose = 10f;
            elena.RadiationDose = 10f;

            radSystem.Tick(5f);

            // Now that all are back inside shelter under clear weather:
            // (2.0 zone - 1.0 shielding) * 5h = 5.0 mSv
            Assert.Equal(15f, chen.RadiationDose, precision: 2);
            Assert.Equal(15f, mikhail.RadiationDose, precision: 2);
            Assert.Equal(15f, elena.RadiationDose, precision: 2);
            Assert.Equal("Shelter Interior", mikhail.LastExposureReason);
            Assert.Equal("Shelter Interior", elena.LastExposureReason);
        }
    }
}
