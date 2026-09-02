using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Xunit;

namespace Ashfall.Core.Tests.Expeditions
{
    public class AviationSystemTests
    {
        private static string LoadAviationCatalogJson()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Assets", "StreamingAssets", "Data", "aircraft_parts.json");
            if (!File.Exists(path))
            {
                var dir = new DirectoryInfo(AppContext.BaseDirectory);
                while (dir != null)
                {
                    string candidate = Path.Combine(dir.FullName, "Assets", "StreamingAssets", "Data", "aircraft_parts.json");
                    if (File.Exists(candidate)) return File.ReadAllText(candidate);
                    dir = dir.Parent;
                }
                throw new FileNotFoundException("Could not find aircraft_parts.json");
            }
            return File.ReadAllText(path);
        }

        [Fact]
        public void LoadCatalog_ParsesDefinitionsCorrectly()
        {
            var system = new AviationSystem();
            system.LoadCatalog(LoadAviationCatalogJson(), new SystemTextJsonSerializer());

            var balloon = system.GetDefinition("aircraft_observation_balloon");
            Assert.NotNull(balloon);
            Assert.Equal("Balloon", balloon.category);
            Assert.Equal(1, balloon.crew_requirement);

            var ultralight = system.GetDefinition("aircraft_jury_rigged_ultralight");
            Assert.NotNull(ultralight);
            Assert.Equal("Ultralight", ultralight.category);
            Assert.Equal(2, ultralight.crew_requirement);
            Assert.True(ultralight.base_fuel_burn > 0f);
        }

        [Fact]
        public void CalculateFlightRange_AccountsForPayloadAndWind()
        {
            var system = new AviationSystem();
            system.LoadCatalog(LoadAviationCatalogJson(), new SystemTextJsonSerializer());
            var balloon = system.GetDefinition("aircraft_observation_balloon")!;

            float rangeNormal = system.CalculateFlightRange(balloon, 40f, 0f, 1.0f);
            float rangeHeavy = system.CalculateFlightRange(balloon, 90f, 0f, 1.0f);
            float rangeHeadwind = system.CalculateFlightRange(balloon, 40f, 0f, 0.5f);

            Assert.True(rangeHeavy < rangeNormal, "Heavier payload must reduce flight range");
            Assert.True(rangeHeadwind < rangeNormal, "Headwind must reduce effective flight range");
        }

        [Fact]
        public void CalculateFlightRisk_CalculatesTransparentBreakdown()
        {
            var system = new AviationSystem();
            system.LoadCatalog(LoadAviationCatalogJson(), new SystemTextJsonSerializer());
            var glider = system.GetDefinition("aircraft_scavenged_glider")!;
            var plane = system.RegisterAircraft("glider_01", "aircraft_scavenged_glider");

            var calmRisk = system.CalculateFlightRisk(glider, plane, 10f, 1.0f, 15f, 0f);
            Assert.True(calmRisk.totalRisk < 0.25f);
            Assert.Equal(0f, calmRisk.windShearRisk);

            var severeRisk = system.CalculateFlightRisk(glider, plane, 45f, 0.1f, -25f, 0.8f);
            Assert.True(severeRisk.windShearRisk > 0f);
            Assert.True(severeRisk.visibilityRisk > 0f);
            Assert.True(severeRisk.icingRisk > 0f);
            Assert.True(severeRisk.antiAirRisk > 0f);
            Assert.True(severeRisk.totalRisk > calmRisk.totalRisk);
        }

        [Fact]
        public void ValidateFlightPlan_CatchesBlockers()
        {
            var system = new AviationSystem();
            system.LoadCatalog(LoadAviationCatalogJson(), new SystemTextJsonSerializer());
            system.RegisterAircraft("ultra_01", "aircraft_jury_rigged_ultralight");

            // Missing crew
            bool validNoCrew = system.ValidateFlightPlan("ultra_01", new List<string> { "pilot_1" }, 50f, 10f, out string reason1);
            Assert.False(validNoCrew);
            Assert.Contains("crew", reason1);

            // Insufficient fuel
            bool validNoFuel = system.ValidateFlightPlan("ultra_01", new List<string> { "pilot_1", "pilot_2" }, 50f, 0.5f, out string reason2);
            Assert.False(validNoFuel);
            Assert.Contains("fuel", reason2);

            // Valid
            bool valid = system.ValidateFlightPlan("ultra_01", new List<string> { "pilot_1", "pilot_2" }, 50f, 10f, out string reason3);
            Assert.True(valid, reason3);
        }

        [Fact]
        public void LaunchAndAdvanceFlight_RevealsMapAndReturns()
        {
            var system = new AviationSystem();
            system.LoadCatalog(LoadAviationCatalogJson(), new SystemTextJsonSerializer());
            system.RegisterAircraft("balloon_01", "aircraft_observation_balloon");

            var plan = system.LaunchFlight("flight_01", "balloon_01", new List<string> { "pilot_1" }, "node_base", "node_ridge", 30f, 40f, 0f);
            Assert.Equal(FlightPhase.AirborneOutbound, plan.phase);
            Assert.Equal(1, system.TotalLaunched);

            var rng = new SeededRng(42);
            // Calm weather, no incidents
            for (int i = 0; i < 4; i++)
            {
                system.AdvanceFlightTick("flight_01", 0.5f, 5f, 1.0f, 15f, 0f, rng);
            }

            Assert.True(plan.mapCellsRevealed > 0, "Aerial flight must reveal fog of war cells");
            Assert.Equal(FlightPhase.Landed, plan.phase);
            Assert.Equal(1, system.TotalLanded);
        }

        [Fact]
        public void ForcedLanding_And_RescueResolution()
        {
            var system = new AviationSystem();
            system.LoadCatalog(LoadAviationCatalogJson(), new SystemTextJsonSerializer());
            system.RegisterAircraft("balloon_bad", "aircraft_observation_balloon");

            var plan = system.LaunchFlight("flight_storm", "balloon_bad", new List<string> { "pilot_1" }, "node_base", "node_peak", 80f, 40f, 0f);

            // Force high wind shear and bad weather with a seed that triggers incident
            var rng = new SeededRng(999);
            for (int i = 0; i < 10; i++)
            {
                if (plan.phase != FlightPhase.AirborneOutbound && plan.phase != FlightPhase.AirborneReturn) break;
                system.AdvanceFlightTick("flight_storm", 0.5f, 60f, 0.05f, -30f, 0.9f, rng);
            }

            Assert.True(plan.rescueRequired);
            Assert.True(plan.phase == FlightPhase.ForcedLanding || plan.phase == FlightPhase.Crashed);

            // Rescue successfully
            bool rescued = system.ResolveCrashRescue("flight_storm", true);
            Assert.True(rescued);
            Assert.False(plan.rescueRequired);
            Assert.Equal(FlightPhase.Rescued, plan.phase);
        }

        [Fact]
        public void AviationState_RoundTripPreservation()
        {
            var system = new AviationSystem();
            system.LoadCatalog(LoadAviationCatalogJson(), new SystemTextJsonSerializer());
            system.RegisterAircraft("plane_a", "aircraft_scavenged_glider");
            system.LaunchFlight("flight_rt", "plane_a", new List<string> { "p1" }, "origin", "dest", 40f, 20f, 0f);

            var state = system.CaptureState();
            Assert.Single(state.aircraft);
            Assert.Single(state.activeFlights);
            Assert.Equal(1, state.totalMissionsLaunched);

            var restoredSystem = new AviationSystem();
            restoredSystem.RestoreState(state);

            Assert.Single(restoredSystem.Aircraft);
            Assert.Single(restoredSystem.ActiveFlights);
            Assert.Equal(1, restoredSystem.TotalLaunched);
        }

        [Fact]
        public void DeterministicReplay_ProducesIdenticalOutcomes()
        {
            var system1 = new AviationSystem();
            system1.LoadCatalog(LoadAviationCatalogJson(), new SystemTextJsonSerializer());
            system1.RegisterAircraft("plane_seed1", "aircraft_jury_rigged_ultralight");
            var plan1 = system1.LaunchFlight("f1", "plane_seed1", new List<string> { "p1", "p2" }, "a", "b", 50f, 60f, 5f);

            var system2 = new AviationSystem();
            system2.LoadCatalog(LoadAviationCatalogJson(), new SystemTextJsonSerializer());
            system2.RegisterAircraft("plane_seed2", "aircraft_jury_rigged_ultralight");
            var plan2 = system2.LaunchFlight("f2", "plane_seed2", new List<string> { "p1", "p2" }, "a", "b", 50f, 60f, 5f);

            var rng1 = new SeededRng(12345);
            var rng2 = new SeededRng(12345);

            for (int i = 0; i < 5; i++)
            {
                system1.AdvanceFlightTick("f1", 0.5f, 25f, 0.4f, -5f, 0.3f, rng1);
                system2.AdvanceFlightTick("f2", 0.5f, 25f, 0.4f, -5f, 0.3f, rng2);
            }

            Assert.Equal(plan1.phase, plan2.phase);
            Assert.Equal(plan1.progressKm, plan2.progressKm);
            Assert.Equal(plan1.mapCellsRevealed, plan2.mapCellsRevealed);
        }
    }
}
