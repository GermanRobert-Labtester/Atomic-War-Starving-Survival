using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Foundry;
using Ashfall.Core.Greenhouse;
using Ashfall.Core.Radio;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.Survivors;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class ApicultureAndTriangulationIntegrationTests
    {
        [Fact]
        public void Apiculture_InstallAndTick_ProducesHoneyAndPollination()
        {
            var system = new ApicultureSystem();
            bool installed = system.InstallHive("hive_main", "bay_orchard", 1);
            Assert.True(installed);
            system.LinkPlots("hive_main", new List<string> { "plot_0", "plot_1" });
            Assert.NotNull(system.GetHive("hive_main"));

            var rng = new SeededRng(1234);

            // Tick for 5 days under optimal conditions
            for (int day = 1; day <= 5; day++)
            {
                system.TickDaily(
                    day: day,
                    greenhouseTemperatureC: 22f,
                    greenhouseContamination: 0f,
                    radiationLevel: 0f,
                    rng: rng);
            }

            var hive = system.GetHive("hive_main");
            Assert.NotNull(hive);
            Assert.False(hive.isDead);
            Assert.True(hive.honeyBuffer > 0f);
            Assert.True(hive.waxBuffer > 0f);

            // Verify pollination bonus
            float plot0Bonus = system.GetPollinationBonus("plot_0");
            Assert.True(plot0Bonus >= 0f);

            // Harvest honey
            var (honeyHarvested, waxHarvested) = system.Harvest("hive_main");
            Assert.True(honeyHarvested > 0f);
            Assert.Equal(0f, hive.honeyBuffer);

            // Save/Restore roundtrip
            var state = system.CaptureState();
            Assert.NotNull(state);
            Assert.Single(state.hives);

            var restored = new ApicultureSystem();
            restored.RestoreState(state);
            Assert.NotNull(restored.GetHive("hive_main"));
            var restoredHive = restored.GetHive("hive_main");
            Assert.NotNull(restoredHive);
            Assert.Equal(hive.queenVitality, restoredHive.queenVitality, 3);
        }

        [Fact]
        public void SignalTriangulation_ThreeObservations_DiscoversLocation()
        {
            var system = new SignalTriangulationSystem();
            string signalId = "sig_emergency_relay";
            var rng = new SeededRng(42);

            var obs1 = new RadioObservation
            {
                signalId = signalId,
                stationId = "station_alpha",
                day = 1,
                hour = 8f,
                bearingDegrees = 45f,
                errorDegrees = 1f,
                signalStrength = 1.0f,
                noiseLevel = 0f,
                frequencyMhz = 94.2f,
                weatherCondition = "Clear",
                operatorSkill = 1.0f
            };

            var obs2 = new RadioObservation
            {
                signalId = signalId,
                stationId = "station_beta",
                day = 1,
                hour = 12f,
                bearingDegrees = 90f,
                errorDegrees = 1f,
                signalStrength = 1.0f,
                noiseLevel = 0f,
                frequencyMhz = 94.2f,
                weatherCondition = "Clear",
                operatorSkill = 1.0f
            };

            var obs3 = new RadioObservation
            {
                signalId = signalId,
                stationId = "station_gamma",
                day = 2,
                hour = 10f,
                bearingDegrees = 135f,
                errorDegrees = 1f,
                signalStrength = 1.0f,
                noiseLevel = 0f,
                frequencyMhz = 94.2f,
                weatherCondition = "Clear",
                operatorSkill = 1.0f
            };

            system.RecordObservation(obs1);
            system.RecordObservation(obs2);
            system.RecordObservation(obs3);

            Assert.Equal(3, system.GetObservationCount(signalId));

            var candidate = system.Triangulate(signalId, rng);
            Assert.NotNull(candidate);
            Assert.True(candidate.confidence >= SignalTriangulationSystem.ConfidenceThreshold);

            // Verify location discovery
            Assert.True(system.IsLocationDiscovered(candidate.locationId));

            // State Capture and Restore
            var state = system.CaptureState();
            Assert.NotNull(state);
            Assert.Equal(3, state.observations.Count);
            Assert.Single(state.discoveredLocationIds);

            var restored = new SignalTriangulationSystem();
            restored.RestoreState(state);
            Assert.Equal(3, restored.GetObservationCount(signalId));
            Assert.True(restored.IsLocationDiscovered(candidate.locationId));
        }

        [Fact]
        public void SimClock_Implements_IClock_And_ISimClock_Seamlessly()
        {
            var clock = new Ashfall.Core.Clock.SimClock(initialTick: 0);
            IClock iclock = clock;

            Assert.Equal(0, iclock.Day);
            Assert.Equal(0, clock.DayIndex);
            Assert.Equal(0, clock.HourOfDay);

            iclock.AdvanceDays(5);
            Assert.Equal(5, iclock.Day);
            Assert.Equal(5, clock.DayIndex);
            Assert.Equal(0, clock.HourOfDay);
            Assert.Equal(5 * 24 * 60, clock.CurrentTick);

            clock.AdvanceHours(12);
            Assert.Equal(5, iclock.Day);
            Assert.Equal(12, clock.HourOfDay);

            iclock.SetDay(10);
            Assert.Equal(10, iclock.Day);
            Assert.Equal(10, clock.DayIndex);
            Assert.Equal(0, clock.HourOfDay);
        }

        [Fact]
        public void ShelterThermal_AddAuxiliaryHeat_WarmsTargetRoom()
        {
            var df = new YearOfAshDeepFreezeSystem(new YearOfAshDeepFreezeState { indoorTemperatureCelsius = 5f });
            var thermal = new ShelterThermalSystem(new SeededRng(42), new NeedsSystem(), new StartingLevelSystem(), df);
            thermal.AddRoom("room_workshop", "Workshop", volumeM3: 50f, insulationFactor: 1.0f, hasRadiator: false);

            var beforeRoom = thermal.State.rooms.Find(r => r.roomId == "room_workshop");
            Assert.NotNull(beforeRoom);
            float initialTemp = beforeRoom.currentTempC;

            // Inject 25 kW waste heat (e.g. from active Silent Foundry heat)
            thermal.AddAuxiliaryHeat("room_workshop", 25.0f);
            Assert.Equal(25.0f, thermal.GetAuxiliaryHeat("room_workshop"));

            thermal.TickDay(1);

            var afterRoom = thermal.State.rooms.Find(r => r.roomId == "room_workshop");
            Assert.NotNull(afterRoom);
            // Temperature should be higher than initial cold baseline due to auxiliary waste heat
            Assert.True(afterRoom.currentTempC > initialTemp);
        }

        [Fact]
        public void SilentFoundry_PowerDemand_And_WasteHeat_Reflects_HeatStages()
        {
            var catalog = new SilentFoundryCatalog();
            var system = new SilentFoundrySystem(rng: new SeededRng(42));
            system.BindCatalog(catalog, 4);
            system.Unlock(1);

            Assert.False(system.IsHeatActive);
            Assert.Equal(0f, system.CurrentPowerDemandKw);
            Assert.Equal(0f, system.CurrentWasteHeatKw);

            // Test stage-dependent power and heat levels
            system.State.heatStage = FoundryHeatStage.Preheat;
            Assert.True(system.IsHeatActive);
            Assert.Equal(15.0f, system.CurrentPowerDemandKw);
            Assert.Equal(12.0f, system.CurrentWasteHeatKw);

            system.State.heatStage = FoundryHeatStage.AtHeat;
            Assert.Equal(22.0f, system.CurrentPowerDemandKw);
            Assert.Equal(25.0f, system.CurrentWasteHeatKw);

            // Test brownout suspension
            system.SuspendHeat("Grid Brownout", 1);
            Assert.False(system.IsHeatActive);
            Assert.Equal(0f, system.CurrentPowerDemandKw);
            Assert.Single(system.FailedCasts);
            Assert.Contains("Grid Brownout", system.FailedCasts[0].reason);
        }

        private sealed class SeededRng : ISeededRng
        {
            private readonly System.Random _rng;
            public int Seed { get; }
            public SeededRng(int seed) { Seed = seed; _rng = new System.Random(seed); }
            public int Next(int min, int max) => _rng.Next(min, max);
            public float NextFloat() => (float)_rng.NextDouble();
            public double NextDouble() => _rng.NextDouble();
        }
    }
}
