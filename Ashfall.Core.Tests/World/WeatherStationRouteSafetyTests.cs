using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public sealed class WeatherStationRouteSafetyTests
    {
        private readonly WeatherGateCatalog _catalog;

        public WeatherStationRouteSafetyTests()
        {
            string dataDir = WeatherGateAuditSimulator.FindDataDir();
            _catalog = WeatherGateCatalogLoader.LoadFromDirectory(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
        }

        private WeatherStationSystem CreateStation(WeatherGateCatalog? catalog = null)
        {
            var weather = new WeatherSystem();
            weather.BindProfile(new SeasonProfileDef { id = "test_season" }, 42);
            var ws = new WeatherStationSystem(weather, new SeededRng(42), NullLog.Instance, catalog ?? _catalog);
            ws.Install(1);
            ws.Calibrate(1);
            return ws;
        }

        [Fact]
        public void Blizzard_MountainPassRoute_IsUnsafe()
        {
            var ws = CreateStation();
            ws.State.cachedForecast.Add(new ForecastEntry
            {
                day = 10,
                weather = WeatherKind.Blizzard,
                confidence = 0.9f,
                isRouteSafe = false
            });

            bool safeCanonical = ws.IsRouteSafe(10, "route_12_the_cloud_eyrie_meteorological_ascent");
            bool safeAlias = ws.IsRouteSafe(10, "route_12");

            Assert.False(safeCanonical);
            Assert.False(safeAlias);
        }

        [Fact]
        public void Clear_MountainPassRoute_IsSafe()
        {
            var ws = CreateStation();
            ws.State.cachedForecast.Add(new ForecastEntry
            {
                day = 11,
                weather = WeatherKind.Clear,
                confidence = 0.9f,
                isRouteSafe = true
            });

            bool safe = ws.IsRouteSafe(11, "route_12");

            Assert.True(safe);
        }

        [Fact]
        public void Blizzard_FrozenLakePositiveGate_IsSafe()
        {
            var ws = CreateStation();
            ws.State.cachedForecast.Add(new ForecastEntry
            {
                day = 12,
                weather = WeatherKind.Blizzard,
                confidence = 0.9f,
                isRouteSafe = false // Blizzard is globally unsafe
            });

            // Frozen lake crossing requires Blizzard, so Blizzard matches requirement -> positive route safe!
            bool safe = ws.IsRouteSafe(12, "route_06_the_thermal_brine_salt_pass");
            bool safeAlias = ws.IsRouteSafe(12, "route_06");

            Assert.True(safe);
            Assert.True(safeAlias);
        }

        [Fact]
        public void Clear_FrozenLakePositiveGate_IsUnsafe()
        {
            var ws = CreateStation();
            ws.State.cachedForecast.Add(new ForecastEntry
            {
                day = 13,
                weather = WeatherKind.Clear,
                confidence = 0.9f,
                isRouteSafe = true // Clear is globally safe
            });

            // Frozen lake crossing requires Blizzard, so Clear does not match -> route unsafe!
            bool safe = ws.IsRouteSafe(13, "route_06");

            Assert.False(safe);
        }

        [Fact]
        public void BioFog_LowlandMarshRoute_NoOverrideContext_IsUnsafe()
        {
            var ws = CreateStation();
            ws.State.cachedForecast.Add(new ForecastEntry
            {
                day = 14,
                weather = WeatherKind.BioFog,
                confidence = 0.85f,
                isRouteSafe = false
            });

            // Lowland marsh route blocks BioFog. Without inventory context, WeatherStation returns false.
            bool safe = ws.IsRouteSafe(14, "route_07_the_aluminium_whale_salvage_run");
            bool safeAlias = ws.IsRouteSafe(14, "route_07");

            Assert.False(safe);
            Assert.False(safeAlias);
        }

        [Fact]
        public void NonexistentRoute_MatchesGlobalRouteSafety()
        {
            var ws = CreateStation();
            ws.State.cachedForecast.Add(new ForecastEntry
            {
                day = 15,
                weather = WeatherKind.Rain,
                confidence = 0.8f,
                isRouteSafe = true
            });

            bool globalSafe = ws.IsRouteSafe(15);
            bool routeSafe = ws.IsRouteSafe(15, "route_nonexistent_999");

            Assert.True(globalSafe);
            Assert.Equal(globalSafe, routeSafe);
        }

        [Fact]
        public void SameDayAndRouteAndWeather_IsDeterministic()
        {
            var ws = CreateStation();
            ws.State.cachedForecast.Add(new ForecastEntry
            {
                day = 16,
                weather = WeatherKind.Blizzard,
                confidence = 0.9f,
                isRouteSafe = false
            });

            bool first = ws.IsRouteSafe(16, "route_12");
            bool second = ws.IsRouteSafe(16, "route_12");
            bool third = ws.IsRouteSafe(16, "route_12");

            Assert.Equal(first, second);
            Assert.Equal(second, third);
        }

        [Fact]
        public void ExistingRouteUnawareSafety_RemainsUnchanged()
        {
            var ws = CreateStation();
            ws.State.cachedForecast.Add(new ForecastEntry
            {
                day = 17,
                weather = WeatherKind.Blizzard,
                confidence = 0.9f,
                isRouteSafe = false
            });
            ws.State.cachedForecast.Add(new ForecastEntry
            {
                day = 18,
                weather = WeatherKind.Clear,
                confidence = 0.9f,
                isRouteSafe = true
            });

            Assert.False(ws.IsRouteSafe(17));
            Assert.True(ws.IsRouteSafe(18));
            Assert.False(ws.IsRouteSafe(999));
        }

        [Fact]
        public void RouteAwareForecastGeneration_PopulatesRouteSafety()
        {
            var weather = new WeatherSystem();
            weather.BindProfile(new SeasonProfileDef { id = "test" }, 123);
            weather.ForceWeather(WeatherKind.Blizzard);

            var ws = new WeatherStationSystem(weather, new SeededRng(123), NullLog.Instance, _catalog);
            ws.Install(1);
            ws.Calibrate(1);

            var actionResult = ws.GenerateForecast(1, "route_12");

            Assert.Equal(ActionResult.StatusKind.Success, actionResult.Status);
            var forecast = ws.GetForecast();
            Assert.NotEmpty(forecast);
            Assert.NotNull(forecast[0].routeSafety);
            Assert.NotNull(forecast[0].RouteSafety);
            Assert.False(forecast[0].routeSafety!.Value);
        }

        [Fact]
        public void RouteUnawareForecastGeneration_LeavesRouteSafetyNull()
        {
            var weather = new WeatherSystem();
            weather.BindProfile(new SeasonProfileDef { id = "test" }, 123);
            weather.ForceWeather(WeatherKind.Clear);

            var ws = new WeatherStationSystem(weather, new SeededRng(123), NullLog.Instance, _catalog);
            ws.Install(1);
            ws.Calibrate(1);

            var actionResult = ws.GenerateForecast(1);

            Assert.Equal(ActionResult.StatusKind.Success, actionResult.Status);
            var forecast = ws.GetForecast();
            Assert.NotEmpty(forecast);
            Assert.Null(forecast[0].routeSafety);
            Assert.Null(forecast[0].RouteSafety);
        }
    }
}
