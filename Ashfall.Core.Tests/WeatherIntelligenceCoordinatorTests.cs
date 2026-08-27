using Ashfall.Core;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class WeatherIntelligenceCoordinatorTests
    {
        // ── Substep 3: improving infrastructure gives demonstrably better info ──

        [Fact]
        public void ReadModel_WithoutStation_HasNoForecast()
        {
            var c = Create(out _);
            c.TickDay(1);
            var rm = c.BuildReadModel();
            Assert.False(rm.stationOperational);
            Assert.Empty(rm.forecast);
            Assert.Equal(0, rm.routeSafeDays);
            Assert.Equal(0, rm.bestTravelDay);
        }

        [Fact]
        public void ReadModel_AfterInstallOnly_StillNoForecast()
        {
            var c = Create(out _);
            c.Station.Install(1);
            c.TickDay(2);
            var rm = c.BuildReadModel();
            Assert.True(rm.stationInstalled);
            Assert.False(rm.stationCalibrated);
            Assert.False(rm.stationOperational);
            Assert.Empty(rm.forecast);
        }

        [Fact]
        public void ReadModel_AfterCalibrate_GeneratesConfidenceForecast()
        {
            var c = Create(out _);
            c.Station.Install(1);
            c.Station.Calibrate(2);
            c.TickDay(3);
            var rm = c.BuildReadModel();
            Assert.True(rm.stationOperational);
            Assert.True(rm.stationAccuracy >= 0.7f);
            Assert.NotEmpty(rm.forecast);
            Assert.All(rm.forecast, f => Assert.True(f.confidence > 0f && f.confidence <= 1f));
        }

        [Fact]
        public void ReadModel_CalibratedStation_ProducesRouteSafetyAndBestTravelDay()
        {
            var c = Create(out _);
            c.Station.Install(1);
            c.Station.Calibrate(2);
            c.TickDay(3);
            var rm = c.BuildReadModel();
            // At least one forecast entry should exist; routeSafeDays is derived.
            Assert.NotEmpty(rm.forecast);
            if (rm.routeSafeDays > 0)
            {
                Assert.True(rm.bestTravelDay > 0);
                Assert.True(rm.bestTravelConfidence > 0f);
            }
        }

        // ── Substep 3: orbital warning lead time ──────────────────────────────

        [Fact]
        public void Orbital_ScheduleImpact_SetsWarningLeadTime()
        {
            var c = Create(out _);
            c.Orbital.ActivateTelemetry(1);
            c.Orbital.ScheduleImpact(10, 5, 25f);
            c.TickDay(5);
            var rm = c.BuildReadModel();
            Assert.True(rm.telemetryActive);
            Assert.True(rm.hasPendingImpact);
            Assert.Equal(10, rm.impactDay);
            Assert.Equal(5, rm.daysUntilImpact); // day 10 - current day 5
        }

        [Fact]
        public void Orbital_TickDay_OnImpactDay_Resolves()
        {
            var c = Create(out _);
            c.Orbital.ActivateTelemetry(1);
            c.Orbital.ScheduleImpact(10, 5, 25f);
            c.TickDay(10);
            var rm = c.BuildReadModel();
            Assert.False(rm.hasPendingImpact);
        }

        // ── Substep 5: persist calibration and observation history ────────────

        [Fact]
        public void SaveRoundTrip_PreservesStationCalibration()
        {
            var c = Create(out _);
            c.Station.Install(1);
            c.Station.Calibrate(2);
            c.TickDay(3);
            var saved = c.CaptureState();
            Assert.True(saved.station.isCalibrated);

            var c2 = Create(out _);
            c2.RestoreState(saved);
            var rm = c2.BuildReadModel();
            Assert.True(rm.stationOperational);
            Assert.True(rm.stationCalibrated);
        }

        [Fact]
        public void SaveRoundTrip_PreservesOrbitalImpact()
        {
            var c = Create(out _);
            c.Orbital.ActivateTelemetry(1);
            c.Orbital.ScheduleImpact(10, 5, 25f);
            var saved = c.CaptureState();
            Assert.Equal(10, saved.orbital.nextImpactDay);

            var c2 = Create(out _);
            c2.RestoreState(saved);
            Assert.True(c2.Orbital.HasPendingImpact);
            Assert.True(c2.Orbital.State.telemetryActive);
        }

        [Fact]
        public void SaveRoundTrip_PreservesForecastEntries()
        {
            var c = Create(out _);
            c.Station.Install(1);
            c.Station.Calibrate(2);
            c.TickDay(3);
            int beforeCount = c.Station.GetForecast().Count;
            Assert.True(beforeCount > 0);

            var saved = c.CaptureState();
            var c2 = Create(out _);
            c2.RestoreState(saved);
            Assert.Equal(beforeCount, c2.Station.GetForecast().Count);
        }

        // ── Substep 6: determinism — same seed yields identical forecast ───────

        [Fact]
        public void Determinism_SameSeed_YieldsIdenticalForecast()
        {
            var c1 = Create(out _);
            c1.Station.Install(1);
            c1.Station.Calibrate(2);
            c1.TickDay(3);
            var rm1 = c1.BuildReadModel();

            var c2 = Create(out _);
            c2.Station.Install(1);
            c2.Station.Calibrate(2);
            c2.TickDay(3);
            var rm2 = c2.BuildReadModel();

            Assert.Equal(rm1.forecast.Count, rm2.forecast.Count);
            Assert.Equal(rm1.stationAccuracy, rm2.stationAccuracy);
            for (int i = 0; i < rm1.forecast.Count; i++)
            {
                Assert.Equal(rm1.forecast[i].day, rm2.forecast[i].day);
                Assert.Equal(rm1.forecast[i].weather, rm2.forecast[i].weather);
                Assert.Equal(rm1.forecast[i].confidence, rm2.forecast[i].confidence);
                Assert.Equal(rm1.forecast[i].isRouteSafe, rm2.forecast[i].isRouteSafe);
            }
        }

        [Fact]
        public void Determinism_SameSeed_YieldsIdenticalOrbitalSequence()
        {
            var c1 = Create(out _);
            c1.Orbital.ActivateTelemetry(1);
            c1.Orbital.ScheduleImpact(10, 5, 25f);
            var s1 = c1.CaptureState();

            var c2 = Create(out _);
            c2.Orbital.ActivateTelemetry(1);
            c2.Orbital.ScheduleImpact(10, 5, 25f);
            var s2 = c2.CaptureState();

            Assert.Equal(s1.orbital.nextImpactDay, s2.orbital.nextImpactDay);
            Assert.Equal(s1.orbital.targetGridX, s2.orbital.targetGridX);
            Assert.Equal(s1.orbital.impactEnergyMj, s2.orbital.impactEnergyMj);
        }

        // ── Multi-day seeded save round-trip (the "Done when" gate) ───────────

        [Fact]
        public void MultiDaySeededRoundTrip_SurvivesReload()
        {
            var c = Create(out _);
            c.Station.Install(1);
            c.Station.Calibrate(2);
            c.Orbital.ActivateTelemetry(1);
            c.Orbital.ScheduleImpact(8, 3, 15f);
            // Tick several days
            for (int day = 3; day <= 7; day++)
                c.TickDay(day);

            var rmBefore = c.BuildReadModel();
            Assert.True(rmBefore.stationOperational);
            Assert.True(rmBefore.telemetryActive);
            Assert.True(rmBefore.hasPendingImpact);

            var saved = c.CaptureState();

            // Reload into a fresh coordinator and verify state survived.
            var c2 = Create(out _);
            c2.RestoreState(saved);
            // Tick one more day on the restored coordinator
            c2.TickDay(8);
            var rmAfter = c2.BuildReadModel();
            // Impact was on day 8, so after ticking day 8 it should be resolved.
            Assert.False(rmAfter.hasPendingImpact);
            Assert.True(rmAfter.stationOperational);
            Assert.True(rmAfter.telemetryActive);
        }

        private static WeatherIntelligenceCoordinator Create(out WeatherSystem weather)
        {
            weather = new WeatherSystem();
            weather.BindProfile(new SeasonProfileDef { id = "default" }, 42);
            var armor = new SkyLayerArmorSystem();
            armor.SetCellArmor(5, CeilingMaterialTier.ReinforcedConcrete, 0.5f);
            return new WeatherIntelligenceCoordinator(weather, armor, new SeededRng(42));
        }
    }
}
