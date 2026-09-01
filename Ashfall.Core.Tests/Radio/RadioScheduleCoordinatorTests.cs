// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    public class RadioScheduleCoordinatorTests
    {
        [Fact]
        public void Resolve_ReturnsStaticDeadAir_WhenNoStationInTolerance()
        {
            var catalog = new RadioBroadcastCatalog();
            var stations = new RadioStationCatalog();
            var coordinator = new RadioScheduleCoordinator(catalog, stations);
            var rng = new SeededRng(2026);

            var res = coordinator.Resolve(50.0f, 10, rng, toleranceMhz: 0.1f);
            Assert.False(res.HasTransmission);
            Assert.True(res.IsSilence);
            Assert.Equal("DEAD AIR / STATIC", res.StationName);
        }

        [Fact]
        public void Resolve_ReturnsSilenceText_WhenStationIsSilent()
        {
            var catalog = new RadioBroadcastCatalog();
            catalog.RegisterAuthoredGapBroadcasts();
            var stations = new RadioStationCatalog();
            stations.SetStationState(RadioStationCatalog.StationCivilDefense, RadioStationState.Silent);

            var coordinator = new RadioScheduleCoordinator(catalog, stations);
            var rng = new SeededRng(2026);

            var res = coordinator.Resolve(88.50f, 40, rng);
            Assert.False(res.HasTransmission);
            Assert.True(res.IsSilence);
            Assert.Contains("Civil Defense carrier lost", res.Message);
        }

        [Fact]
        public void Resolve_ReturnsJammedText_WhenStationIsJammed()
        {
            var catalog = new RadioBroadcastCatalog();
            catalog.RegisterAuthoredGapBroadcasts();
            var stations = new RadioStationCatalog();
            stations.SetStationState(RadioStationCatalog.StationGarrisonOverlord, RadioStationState.Jammed);

            var coordinator = new RadioScheduleCoordinator(catalog, stations);
            var rng = new SeededRng(2026);

            var res = coordinator.Resolve(88.40f, 50, rng);
            Assert.True(res.HasTransmission);
            Assert.True(res.IsJammed);
            Assert.Contains("jammed", res.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void DynamicWeatherAlert_OverridesRoutineBroadcastsImmediately()
        {
            var catalog = new RadioBroadcastCatalog();
            catalog.RegisterAuthoredGapBroadcasts();
            var stations = new RadioStationCatalog();
            var coordinator = new RadioScheduleCoordinator(catalog, stations);
            var rng = new SeededRng(2026);

            coordinator.InjectWeatherAlert("FLASH TORNADO AND FALLOUT PLUME APPROACHING SECTOR 4.");

            var res = coordinator.Resolve(88.50f, 40, rng);
            Assert.True(res.HasTransmission);
            Assert.True(res.IsEmergency);
            Assert.Equal(BroadcastPriority.Emergency, res.Priority);
            Assert.Contains("TORNADO", res.Message);
        }

        [Fact]
        public void DynamicOrbitalAlert_InjectsEmergencyWarningOnAutomatedArray()
        {
            var catalog = new RadioBroadcastCatalog();
            var stations = new RadioStationCatalog();
            var coordinator = new RadioScheduleCoordinator(catalog, stations);
            var rng = new SeededRng(2026);

            coordinator.InjectOrbitalAlert("KINETIC DEORBIT DETECTED. IMPACT GRID 44.");

            var res = coordinator.Resolve(142.85f, 200, rng);
            Assert.True(res.HasTransmission);
            Assert.True(res.IsEmergency);
            Assert.Contains("KINETIC DEORBIT", res.Message);
        }

        [Fact]
        public void AppointmentPrograms_AreSixCanonicalPrograms()
        {
            var catalog = new RadioBroadcastCatalog();
            var stations = new RadioStationCatalog();
            var coordinator = new RadioScheduleCoordinator(catalog, stations);

            Assert.Equal(6, coordinator.AppointmentPrograms.Count);
            Assert.Contains(coordinator.AppointmentPrograms, p => p.ProgramId == "prog_morning_weather");
            Assert.Contains(coordinator.AppointmentPrograms, p => p.ProgramId == "prog_lost_and_found");
            Assert.Contains(coordinator.AppointmentPrograms, p => p.ProgramId == "prog_market_caravan");
            Assert.Contains(coordinator.AppointmentPrograms, p => p.ProgramId == "prog_route_conditions");
            Assert.Contains(coordinator.AppointmentPrograms, p => p.ProgramId == "prog_public_health");
            Assert.Contains(coordinator.AppointmentPrograms, p => p.ProgramId == "prog_industrial_foundry");
        }
    }
}
