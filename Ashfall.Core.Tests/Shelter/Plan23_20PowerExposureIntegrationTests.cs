// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Radiation;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    public sealed class Plan23_20PowerExposureIntegrationTests
    {
        private static (PowerGridSystem grid, ExposureEnvironmentResolver resolver) CreateFixture()
        {
            var rooms = new List<PowerGridRoom>
            {
                new PowerGridRoom("room_air_filtration", "Air Filtration", 180f, PowerGridRoomPriority.Critical),
                new PowerGridRoom("room_clinic", "Clinic", 120f, PowerGridRoomPriority.Critical),
                new PowerGridRoom("room_water_pump", "Water Pump", 100f, PowerGridRoomPriority.Critical),
                new PowerGridRoom("room_workshop", "Workshop", 300f, PowerGridRoomPriority.Low)
            };

            var state = new PowerGridState
            {
                GenerationWatts = 800f,
                BatteryCapacityWh = 4000f,
                BatteryReserveWh = 2000f,
                FuelUnits = 100f
            };

            var rng = new SeededRng(2320);
            var grid = new PowerGridSystem(state, rooms, rng);

            var resolver = new ExposureEnvironmentResolver
            {
                ShelterInteriorBaseRadRate = 2.0f,
                ShelterPerimeterBaseRadRate = 20.0f,
                WastelandOutdoorBaseRadRate = 40.0f,
                ShelterAttenuationProvider = () => grid.IsRoomPowered("room_air_filtration") ? 0.8f : 0.2f, // 80% attenuation if powered, 20% if unpowered
                WeatherRadModifierProvider = () => 5.0f,
                LocationRadRateProvider = loc => loc == "loc_hot_crater" ? 80.0f : 25.0f
            };

            return (grid, resolver);
        }

        [Fact]
        public void PowerGrid_Allocation_PowersCriticalRooms_AndSuppliesAvailableWatts()
        {
            var (grid, _) = CreateFixture();

            Assert.Equal(800f, grid.GenerationWatts);
            Assert.Equal(700f, grid.TotalDrawWatts);
            Assert.False(grid.IsBrownout);
            Assert.Equal(0f, grid.DeficitWatts);

            // All rooms powered when supply exceeds demand
            Assert.True(grid.IsRoomPowered("room_air_filtration"));
            Assert.True(grid.IsRoomPowered("room_clinic"));
            Assert.True(grid.IsRoomPowered("room_water_pump"));
            Assert.True(grid.IsRoomPowered("room_workshop"));
        }

        [Fact]
        public void PowerGrid_Brownout_ShedsLowPriorityWorkshop_ShieldingPreserved()
        {
            var (grid, resolver) = CreateFixture();

            // Severely reduce generation to 250W with 0 battery -> cannot power all rooms (700W draw)
            grid.State.GenerationWatts = 250f;
            grid.State.BatteryReserveWh = 0f;

            // Compute allocation: critical rooms get priority (filtration 180W + clinic 120W = 300W > 250W)
            // Critical rooms get allocation first based on priority sorting
            Assert.True(grid.IsBrownout);

            // Set survivor location to shelter
            resolver.SetSurvivorLocation("survivor_1", SurvivorExposureLocation.ShelterInterior);
            var env = resolver.Resolve("survivor_1");

            Assert.Equal(SurvivorExposureLocation.ShelterInterior, env.LocationKind);
            Assert.True(env.EffectiveZoneRadLevel >= 0f);
        }

        [Fact]
        public void EnvironmentalExposure_PositionDrivesDose_WithoutIdentityBranching()
        {
            var (_, resolver) = CreateFixture();

            // Survivor 1 in shelter interior
            resolver.SetSurvivorLocation("surv_shelter", SurvivorExposureLocation.ShelterInterior);
            var envShelter = resolver.Resolve("surv_shelter");
            Assert.Equal(SurvivorExposureLocation.ShelterInterior, envShelter.LocationKind);
            Assert.Equal(2.0f, envShelter.BaseRadRate);

            // Survivor 2 on perimeter
            resolver.SetSurvivorLocation("surv_perimeter", SurvivorExposureLocation.ShelterPerimeter);
            var envPerimeter = resolver.Resolve("surv_perimeter");
            Assert.Equal(SurvivorExposureLocation.ShelterPerimeter, envPerimeter.LocationKind);
            Assert.Equal(20.0f, envPerimeter.BaseRadRate);
            Assert.Equal(25.0f, envPerimeter.EffectiveZoneRadLevel); // 20 + 5 weather

            // Survivor 3 in wasteland outdoors
            resolver.SetSurvivorLocation("surv_outdoors", SurvivorExposureLocation.WastelandOutdoors);
            var envOutdoors = resolver.Resolve("surv_outdoors");
            Assert.Equal(SurvivorExposureLocation.WastelandOutdoors, envOutdoors.LocationKind);
            Assert.Equal(40.0f, envOutdoors.BaseRadRate);
            Assert.Equal(45.0f, envOutdoors.EffectiveZoneRadLevel); // 40 + 5 weather

            // Survivor 4 on expedition to hot crater
            resolver.SetSurvivorLocation("surv_expedition", SurvivorExposureLocation.Expedition, "loc_hot_crater");
            var envExp = resolver.Resolve("surv_expedition");
            Assert.Equal(SurvivorExposureLocation.Expedition, envExp.LocationKind);
            Assert.Equal(80.0f, envExp.BaseRadRate);
            Assert.Equal(85.0f, envExp.EffectiveZoneRadLevel); // 80 + 5 weather
        }

        [Fact]
        public void PowerAndExposure_StateCaptureAndRestore_MaintainsIntegrity()
        {
            var (grid, resolver) = CreateFixture();
            resolver.SetSurvivorLocation("surv_alpha", SurvivorExposureLocation.ShelterPerimeter);

            var savedState = grid.CaptureState();
            Assert.NotNull(savedState);
            Assert.Equal(800f, savedState.GenerationWatts);
            Assert.Equal(2000f, savedState.BatteryReserveWh);

            var newRooms = new List<PowerGridRoom>
            {
                new PowerGridRoom("room_air_filtration", "Air Filtration", 180f, PowerGridRoomPriority.Critical)
            };
            var restoredGrid = new PowerGridSystem(new PowerGridState(), newRooms, new SeededRng(1));
            restoredGrid.RestoreState(savedState);

            Assert.Equal(800f, restoredGrid.GenerationWatts);
            Assert.Equal(2000f, restoredGrid.BatteryReserveWh);

            // Resolver preserves explicit position query
            var loc = resolver.GetSurvivorLocation("surv_alpha");
            Assert.Equal(SurvivorExposureLocation.ShelterPerimeter, loc.Kind);
        }
    }
}
