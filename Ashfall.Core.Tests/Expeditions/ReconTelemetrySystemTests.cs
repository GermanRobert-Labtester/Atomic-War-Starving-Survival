using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Random;
using Xunit;

namespace Ashfall.Core.Tests.Expeditions
{
    /// <summary>
    /// Deterministic replay and boundary tests for Task 8:
    /// Long-Range Reconnaissance Drones &amp; High-Altitude Mapping.
    /// </summary>
    public sealed class ReconTelemetrySystemTests
    {
        private static string GetDataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out var path))
                return path;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data directory not found.");
        }

        private static ReconTelemetrySystem CreateSystem(ReconTelemetryState? state = null)
        {
            return new ReconTelemetrySystem(state, new SeededRng(2005), new TestLog());
        }

        // ── Catalog ──────────────────────────────────────────────────

        [Fact]
        public void Catalog_LoadsSuccessfullyFromDataDir()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var catalog = ReconTelemetryCatalogLoader.Load(GetDataDir(), files, json);

            Assert.NotNull(catalog);
            Assert.NotEmpty(catalog!.Platforms);
            Assert.Contains(catalog.Platforms, p => p.PlatformId == "probe_helium_sounding_balloon");
        }

        [Fact]
        public void LoadCatalog_RegistersAllPlatforms()
        {
            var system = CreateSystem();
            var catalog = new ReconTelemetryCatalog
            {
                Platforms = new System.Collections.Generic.List<ReconProbeDef>
                {
                    new ReconProbeDef { PlatformId = "test_probe", EnduranceHours = 24, WindLimitKph = 50 }
                }
            };

            system.LoadCatalog(catalog);
            Assert.Single(system.Platforms);
            Assert.True(system.Platforms.ContainsKey("test_probe"));
        }

        // ── LaunchMission ────────────────────────────────────────────

        [Fact]
        public void LaunchMission_InvalidPlatform_ReturnsFailed()
        {
            var system = CreateSystem();
            var result = system.LaunchMission("unknown_platform", "loc_holdfast");
            Assert.False(result.IsSuccess);
            Assert.Equal("unknown_platform", result.FailureCode);
        }

        [Fact]
        public void LaunchMission_AlreadyLaunchedNonRecoverable_ReturnsFailed()
        {
            var system = CreateSystem();
            var catalog = new ReconTelemetryCatalog
            {
                Platforms = new System.Collections.Generic.List<ReconProbeDef>
                {
                    new ReconProbeDef { PlatformId = "probe_disposable", Recoverable = false, EnduranceHours = 24, WindLimitKph = 100, LaunchCosts = new System.Collections.Generic.List<MaterialCost>() }
                }
            };
            system.LoadCatalog(catalog);
            system.LaunchMission("probe_disposable", "loc_holdfast");

            var result = system.LaunchMission("probe_disposable", "loc_holdfast");
            Assert.False(result.IsSuccess);
            Assert.Equal("already_launched", result.FailureCode);
        }

        [Fact]
        public void LaunchMission_Success_CreatesMission()
        {
            var system = CreateSystem();
            var catalog = new ReconTelemetryCatalog
            {
                Platforms = new System.Collections.Generic.List<ReconProbeDef>
                {
                    new ReconProbeDef { PlatformId = "probe_reusable", Recoverable = true, EnduranceHours = 24, WindLimitKph = 100, LaunchCosts = new System.Collections.Generic.List<MaterialCost>() }
                }
            };
            system.LoadCatalog(catalog);

            var result = system.LaunchMission("probe_reusable", "loc_holdfast");
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.MissionId);
            Assert.Equal("probe_reusable", system.State.launchedPlatformIds[0]);
            Assert.Single(system.State.activeMissions);
        }

        // ── SurveySectors ────────────────────────────────────────────

        [Fact]
        public void SurveySectors_UnknownMission_ReturnsFailed()
        {
            var system = CreateSystem();
            var result = system.SurveySectors("unknown_mission", new System.Collections.Generic.List<string> { "loc_holdfast" });
            Assert.False(result.IsSuccess);
            Assert.Equal("unknown_mission", result.FailureCode);
        }

        // ── RecoverPlatform ──────────────────────────────────────────

        [Fact]
        public void RecoverPlatform_UnknownMission_ReturnsFailed()
        {
            var system = CreateSystem();
            var result = system.RecoverPlatform("unknown_mission");
            Assert.False(result.IsSuccess);
            Assert.Equal("unknown_mission", result.FailureCode);
        }

        // ── GenerateForecast ─────────────────────────────────────────

        [Fact]
        public void GenerateForecast_NoForecastSensors_ReturnsBlocked()
        {
            var system = CreateSystem();
            var catalog = new ReconTelemetryCatalog
            {
                Platforms = new System.Collections.Generic.List<ReconProbeDef>
                {
                    new ReconProbeDef { PlatformId = "probe_visual", SensorSuite = new System.Collections.Generic.List<string> { "visual" } }
                }
            };
            system.LoadCatalog(catalog);

            var result = system.GenerateForecast("probe_visual");
            Assert.False(result.IsSuccess);
            Assert.Equal("no_forecast_sensors", result.FailureCode);
        }

        [Fact]
        public void GenerateForecast_WithWeatherSensor_Succeeds()
        {
            var system = CreateSystem();
            var catalog = new ReconTelemetryCatalog
            {
                Platforms = new System.Collections.Generic.List<ReconProbeDef>
                {
                    new ReconTelemetryCatalog().Platforms.FirstOrDefault() ?? new ReconProbeDef { PlatformId = "probe_weather", SensorSuite = new System.Collections.Generic.List<string> { "weather" } }
                }
            };
            // Use the real catalog if available
            system.LoadCatalog(ReconTelemetryCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer()));

            var result = system.GenerateForecast("probe_helium_sounding_balloon");
            Assert.True(result.IsSuccess);
            Assert.Single(system.State.forecasts);
        }

        // ── ScoutRoute ───────────────────────────────────────────────

        [Fact]
        public void ScoutRoute_UnknownMission_ReturnsFailed()
        {
            var system = CreateSystem();
            var result = system.ScoutRoute("route_ice_road", "unknown_mission");
            Assert.False(result.IsSuccess);
            Assert.Equal("unknown_mission", result.FailureCode);
        }

        // ── TickDay ──────────────────────────────────────────────────

        [Fact]
        public void TickDay_Idempotent_NoDoubleTick()
        {
            var system = CreateSystem();
            system.TickDay(1);
            system.TickDay(1);
            Assert.Equal(1, system.State.lastProcessedDay);
        }

        [Fact]
        public void TickDay_ExpiresOldRouteScouts()
        {
            var system = CreateSystem();
            system.State.scoutedRoutes.Add(new RouteScoutRecord { routeId = "route_old", expiryDay = 5, isActive = true });
            system.TickDay(10);
            Assert.Empty(system.State.scoutedRoutes);
        }

        // ── Save Round-Trip ──────────────────────────────────────────

        [Fact]
        public void SaveRoundTrip_PreservesMissionsAndState()
        {
            var system = CreateSystem();
            var catalog = ReconTelemetryCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            system.LoadCatalog(catalog);

            system.LaunchMission("probe_helium_sounding_balloon", "loc_holdfast");
            system.State.scoutedRoutes.Add(new RouteScoutRecord { routeId = "route_1", expiryDay = 10, isActive = true });
            system.TickDay(3);

            var captured = system.CaptureState();
            var restored = CreateSystem(captured);
            restored.LoadCatalog(catalog);

            Assert.Single(restored.State.activeMissions);
            Assert.Single(restored.State.scoutedRoutes);
            Assert.Equal("route_1", restored.State.scoutedRoutes[0].routeId);
            Assert.Equal(3, restored.State.lastProcessedDay);
        }

        // ── Determinism ──────────────────────────────────────────────

        [Fact]
        public void Deterministic_SameSeed_SameLaunchAndSurvey()
        {
            // Run 1
            var sys1 = CreateSystem();
            var catalog1 = ReconTelemetryCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            sys1.LoadCatalog(catalog1);
            var launch1 = sys1.LaunchMission("probe_helium_sounding_balloon", "loc_holdfast");
            sys1.SurveySectors(launch1.MissionId, new System.Collections.Generic.List<string> { "loc_holdfast" });

            // Run 2 with same seed
            var sys2 = CreateSystem();
            var catalog2 = ReconTelemetryCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            sys2.LoadCatalog(catalog2);
            var launch2 = sys2.LaunchMission("probe_helium_sounding_balloon", "loc_holdfast");
            sys2.SurveySectors(launch2.MissionId, new System.Collections.Generic.List<string> { "loc_holdfast" });

            Assert.Equal(launch1.MissionId, launch2.MissionId);
            Assert.Equal(sys1.State.activeMissions.Count, sys2.State.activeMissions.Count);
            Assert.Equal(sys1.State.surveyedSectorIds.Count, sys2.State.surveyedSectorIds.Count);
        }
    }

    // ── Minimal test log ────────────────────────────────────────────

    internal sealed class TestLog : ILog
    {
        public void Info(string message) { }
        public void Warn(string message) { }
        public void Error(string message) { }
        public void Debug(string message) { }
    }
}
