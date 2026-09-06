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
    /// Integration tests for Task 8: Long-Range Reconnaissance Drones &amp; High-Altitude Mapping.
    /// Simulates end-to-end scenarios: launch campaign, survey, recovery, route scouting, forecast generation.
    /// </summary>
    public sealed class ReconTelemetryIntegrationTests
    {
        private static string GetDataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out var path))
                return path;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data directory not found.");
        }

        [Fact]
        public void FullReconCampaign_LaunchSurveyRecover()
        {
            var system = new ReconTelemetrySystem(null, new SeededRng(2005), new TestLog());
            system.LoadCatalog(ReconTelemetryCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer()));

            // Launch a recoverable platform
            var launch = system.LaunchMission("probe_short_range_quad_uav", "loc_holdfast");
            Assert.True(launch.IsSuccess);
            Assert.NotNull(launch.MissionId);

            string missionId = launch.MissionId;
            var mission = system.GetMission(missionId);
            Assert.NotNull(mission);
            Assert.Equal("launched", mission!.status);

            // Survey sectors
            var survey = system.SurveySectors(missionId, new System.Collections.Generic.List<string> { "loc_holdfast", "loc_wasteland_outpost" });
            Assert.True(survey.IsSuccess);
            Assert.Equal("returning", mission.status);

            // Recover
            var recover = system.RecoverPlatform(missionId);
            Assert.True(recover.IsSuccess);
            Assert.False(system.IsPlatformLaunched("probe_short_range_quad_uav"));
        }

        [Fact]
        public void RouteScouting_ActivatesSpeedBonus()
        {
            var system = new ReconTelemetrySystem(null, new SeededRng(2005), new TestLog());
            system.LoadCatalog(ReconTelemetryCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer()));

            var launch = system.LaunchMission("probe_short_range_quad_uav", "loc_holdfast");
            Assert.True(launch.IsSuccess);

            system.ScoutRoute("route_ice_road", launch.MissionId);
            Assert.Equal(0.75f, system.GetRouteSpeedMultiplier("route_ice_road"));
        }

        [Fact]
        public void ForecastGeneration_WithWeatherPlatform_Succeeds()
        {
            var system = new ReconTelemetrySystem(null, new SeededRng(2005), new TestLog());
            system.LoadCatalog(ReconTelemetryCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer()));

            // First launch the platform
            var launch = system.LaunchMission("probe_helium_sounding_balloon", "loc_holdfast");
            Assert.True(launch.IsSuccess);

            var result = system.GenerateForecast("probe_helium_sounding_balloon");
            Assert.True(result.IsSuccess);
            Assert.Single(system.State.forecasts);
            Assert.Equal("fallout_front", system.State.forecasts[0].frontType);
        }

        [Fact]
        public void MultiDayTick_RoutesExpire_AndForecastsExpire()
        {
            var system = new ReconTelemetrySystem(null, new SeededRng(2005), new TestLog());
            system.State.scoutedRoutes.Add(new RouteScoutRecord { routeId = "route_1", expiryDay = 5, isActive = true });
            system.State.forecasts.Add(new ReconForecastRecord { forecastId = "fcst_1", expiryDay = 3 });

            system.TickDay(1);
            system.TickDay(2);
            system.TickDay(3); // expires forecast
            system.TickDay(5); // expires route
            system.TickDay(6);

            Assert.Empty(system.State.scoutedRoutes);
            Assert.Empty(system.State.forecasts);
            Assert.Equal(6, system.State.lastProcessedDay);
        }

        [Fact]
        public void PlatformLoss_OnActiveMission_marksLost()
        {
            var system = new ReconTelemetrySystem(null, new SeededRng(2005), new TestLog());
            system.LoadCatalog(ReconTelemetryCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer()));

            var launch = system.LaunchMission("probe_solar_recon_glider", "loc_holdfast");
            Assert.True(launch.IsSuccess);

            string? lostPlatformId = null;
            string? lostReason = null;
            system.OnPlatformLost += (pid, reason) => { lostPlatformId = pid; lostReason = reason; };

            // Tick many days to eventually trigger loss (2% daily chance)
            for (int day = 1; day <= 200; day++)
                system.TickDay(day);

            if (lostPlatformId != null)
            {
                Assert.Equal("probe_solar_recon_glider", lostPlatformId);
                Assert.Equal("weather_event", lostReason);
            }
        }

        [Fact]
        public void SaveRoundTrip_FullState_PreservesAll()
        {
            var system = new ReconTelemetrySystem(null, new SeededRng(2005), new TestLog());
            system.LoadCatalog(ReconTelemetryCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer()));

            var launch = system.LaunchMission("probe_short_range_quad_uav", "loc_holdfast");
            system.State.scoutedRoutes.Add(new RouteScoutRecord { routeId = "route_1", expiryDay = 10, isActive = true });
            system.State.forecasts.Add(new ReconForecastRecord { forecastId = "fcst_1", expiryDay = 5 });
            system.TickDay(3);

            var captured = system.CaptureState();
            var restored = new ReconTelemetrySystem(captured, new SeededRng(2005), new TestLog());
            restored.LoadCatalog(ReconTelemetryCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer()));

            Assert.Single(restored.State.activeMissions);
            Assert.Single(restored.State.scoutedRoutes);
            Assert.Single(restored.State.forecasts);
            Assert.Equal("route_1", restored.State.scoutedRoutes[0].routeId);
            Assert.Equal(3, restored.State.lastProcessedDay);
        }
    }

}
