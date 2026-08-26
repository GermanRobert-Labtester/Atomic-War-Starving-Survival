using System;
using Ashfall.Core;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class WeatherSondeSystemTests
    {
        private static WeatherSystem DemoWeather(int seed = 42)
        {
            var weather = new WeatherSystem();
            var profile = new SeasonProfileDef
            {
                id = "test_profile",
                displayName = "Test",
                weatherCheckIntervalHours = 6f,
                seasons = new System.Collections.Generic.List<SeasonWindowDef>
                {
                    new SeasonWindowDef
                    {
                        id = "test_season",
                        displayName = "Test Season",
                        startDay = 0,
                        clearWeight = 1f,
                        rainWeight = 1f,
                        overcastWeight = 1f,
                        ashfallWeight = 0.5f,
                        falloutStormWeight = 0.1f,
                        blizzardWeight = 0.1f,
                        blackRainWeight = 0f
                    }
                }
            };
            weather.BindProfile(profile, seed);
            return weather;
        }

        private static SeededRng Rng(int seed) => new SeededRng(seed);

        // ── Launch ───────────────────────────────────────────────────

        [Fact]
        public void Launch_SetsLaunched()
        {
            var weather = DemoWeather();
            var sys = new WeatherSondeSystem(weather);
            Assert.True(sys.Launch("sonde_1", 1, 12f, 1f, 1f));
            Assert.True(sys.IsLaunched);
        }

        [Fact]
        public void Launch_RejectsWhenAlreadyLaunched()
        {
            var weather = DemoWeather();
            var sys = new WeatherSondeSystem(weather);
            Assert.True(sys.Launch("sonde_1", 1, 12f, 1f, 1f));
            Assert.False(sys.Launch("sonde_2", 2, 12f, 1f, 1f));
        }

        [Fact]
        public void Launch_RejectsInsufficientHydrogen()
        {
            var weather = DemoWeather();
            var sys = new WeatherSondeSystem(weather);
            Assert.False(sys.Launch("sonde_1", 1, 12f, 0.1f, 1f));
        }

        [Fact]
        public void Launch_RejectsInsufficientBattery()
        {
            var weather = DemoWeather();
            var sys = new WeatherSondeSystem(weather);
            Assert.False(sys.Launch("sonde_1", 1, 12f, 1f, 0.1f));
        }

        [Fact]
        public void Launch_RaisesOnLaunchStarted()
        {
            var weather = DemoWeather();
            var sys = new WeatherSondeSystem(weather);
            string? launchedId = null;
            sys.OnLaunchStarted += id => launchedId = id;
            sys.Launch("sonde_1", 1, 12f, 1f, 1f);
            Assert.Equal("sonde_1", launchedId);
        }

        // ── Tick ─────────────────────────────────────────────────────

        [Fact]
        public void Tick_AdvancesFlight()
        {
            var weather = DemoWeather();
            var sys = new WeatherSondeSystem(weather);
            sys.Launch("sonde_1", 1, 12f, 1f, 1f);
            Assert.True(sys.Tick(Rng(1)));
            Assert.Equal(1, sys.State.ticksElapsed);
        }

        [Fact]
        public void Tick_AddsTelemetrySample()
        {
            var weather = DemoWeather();
            var sys = new WeatherSondeSystem(weather);
            sys.Launch("sonde_1", 1, 12f, 1f, 1f);
            sys.Tick(Rng(1));
            Assert.Single(sys.State.samples);
        }

        [Fact]
        public void Tick_DrainsBattery()
        {
            var weather = DemoWeather();
            var sys = new WeatherSondeSystem(weather);
            sys.Launch("sonde_1", 1, 12f, 1f, 1f);
            float batteryBefore = sys.State.batteryLevel;
            sys.Tick(Rng(1));
            Assert.True(sys.State.batteryLevel < batteryBefore);
        }

        [Fact]
        public void Tick_DrainsHydrogen()
        {
            var weather = DemoWeather();
            var sys = new WeatherSondeSystem(weather);
            sys.Launch("sonde_1", 1, 12f, 1f, 1f);
            float hydrogenBefore = sys.State.hydrogenLevel;
            sys.Tick(Rng(1));
            Assert.True(sys.State.hydrogenLevel < hydrogenBefore);
        }

        [Fact]
        public void Tick_IncreasesAltitude()
        {
            var weather = DemoWeather();
            var sys = new WeatherSondeSystem(weather);
            sys.Launch("sonde_1", 1, 12f, 1f, 1f);
            sys.Tick(Rng(1));
            Assert.True(sys.GetCurrentAltitude() > 0f);
        }

        [Fact]
        public void Tick_CompletesAfterFlightDuration()
        {
            var weather = DemoWeather();
            var sys = new WeatherSondeSystem(weather);
            sys.Launch("sonde_1", 1, 12f, 1f, 1f);
            for (int i = 0; i < WeatherSondeSystem.DefaultFlightDurationTicks; i++)
                sys.Tick(Rng(i));
            Assert.True(sys.IsComplete);
            Assert.True(sys.State.isRecovered);
        }

        [Fact]
        public void Tick_GeneratesForecastOnRecovery()
        {
            var weather = DemoWeather();
            var sys = new WeatherSondeSystem(weather);
            sys.Launch("sonde_1", 1, 12f, 1f, 1f);
            for (int i = 0; i < WeatherSondeSystem.DefaultFlightDurationTicks; i++)
                sys.Tick(Rng(i));
            Assert.True(sys.State.forecast.Count > 0);
        }

        [Fact]
        public void Tick_FailsWhenBatteryDepleted()
        {
            var weather = DemoWeather();
            var sys = new WeatherSondeSystem(weather);
            sys.Launch("sonde_1", 1, 12f, 1f, 1f);
            sys.State.batteryLevel = 0.05f; // about to deplete
            sys.Tick(Rng(1));
            Assert.True(sys.State.isFailed);
            Assert.Contains("Battery", sys.State.failureReason);
        }

        [Fact]
        public void Tick_ReturnsFalseWhenNotLaunched()
        {
            var weather = DemoWeather();
            var sys = new WeatherSondeSystem(weather);
            Assert.False(sys.Tick(Rng(1)));
        }

        [Fact]
        public void Tick_ReturnsFalseWhenComplete()
        {
            var weather = DemoWeather();
            var sys = new WeatherSondeSystem(weather);
            sys.Launch("sonde_1", 1, 12f, 1f, 1f);
            for (int i = 0; i < WeatherSondeSystem.DefaultFlightDurationTicks; i++)
                sys.Tick(Rng(i));
            Assert.False(sys.Tick(Rng(1)));
        }

        // ── Forecast ─────────────────────────────────────────────────

        [Fact]
        public void Forecast_HasEntriesForHorizon()
        {
            var weather = DemoWeather();
            var sys = new WeatherSondeSystem(weather);
            sys.Launch("sonde_1", 1, 12f, 1f, 1f);
            for (int i = 0; i < WeatherSondeSystem.DefaultFlightDurationTicks; i++)
                sys.Tick(Rng(i));
            Assert.True(sys.State.forecast.Count >= WeatherSondeSystem.BaseForecastHorizonDays);
        }

        [Fact]
        public void Forecast_ConfidenceDecreasesWithDayOffset()
        {
            var weather = DemoWeather();
            var sys = new WeatherSondeSystem(weather);
            sys.Launch("sonde_1", 1, 12f, 1f, 1f);
            for (int i = 0; i < WeatherSondeSystem.DefaultFlightDurationTicks; i++)
                sys.Tick(Rng(i));
            var forecast = sys.State.forecast;
            if (forecast.Count >= 2)
            {
                Assert.True(forecast[0].confidence >= forecast[1].confidence,
                    "Confidence should decrease with day offset");
            }
        }

        [Fact]
        public void Forecast_DoesNotMutateWeatherSystem()
        {
            var weather = DemoWeather();
            var stateBefore = weather.CaptureState();
            var sys = new WeatherSondeSystem(weather);
            sys.Launch("sonde_1", 1, 12f, 1f, 1f);
            for (int i = 0; i < WeatherSondeSystem.DefaultFlightDurationTicks; i++)
                sys.Tick(Rng(i));
            var stateAfter = weather.CaptureState();
            Assert.Equal(stateBefore.currentKind, stateAfter.currentKind);
            Assert.Equal(stateBefore.rollCount, stateAfter.rollCount);
        }

        // ── Telemetry ────────────────────────────────────────────────

        [Fact]
        public void Telemetry_SamplesHaveAltitude()
        {
            var weather = DemoWeather();
            var sys = new WeatherSondeSystem(weather);
            sys.Launch("sonde_1", 1, 12f, 1f, 1f);
            sys.Tick(Rng(1));
            Assert.True(sys.State.samples[0].altitudeKm > 0f);
        }

        [Fact]
        public void Telemetry_SamplesHaveTemperature()
        {
            var weather = DemoWeather();
            var sys = new WeatherSondeSystem(weather);
            sys.Launch("sonde_1", 1, 12f, 1f, 1f);
            sys.Tick(Rng(1));
            // Temperature should be reasonable (not NaN or extreme)
            Assert.True(float.IsFinite(sys.State.samples[0].temperatureC));
        }

        // ── Determinism ──────────────────────────────────────────────

        [Fact]
        public void SameSeed_SameFlightOutcome()
        {
            var weatherA = DemoWeather(42);
            var sysA = new WeatherSondeSystem(weatherA);
            sysA.Launch("sonde_1", 1, 12f, 1f, 1f);
            for (int i = 0; i < 4; i++) sysA.Tick(Rng(i));

            var weatherB = DemoWeather(42);
            var sysB = new WeatherSondeSystem(weatherB);
            sysB.Launch("sonde_1", 1, 12f, 1f, 1f);
            for (int i = 0; i < 4; i++) sysB.Tick(Rng(i));

            Assert.Equal(sysA.State.samples.Count, sysB.State.samples.Count);
            Assert.Equal(sysA.State.isRecovered, sysB.State.isRecovered);
            Assert.Equal(sysA.State.forecast.Count, sysB.State.forecast.Count);
        }

        // ── Save/Load ────────────────────────────────────────────────

        [Fact]
        public void CaptureRestore_RoundTrips()
        {
            var weather = DemoWeather();
            var sys = new WeatherSondeSystem(weather);
            sys.Launch("sonde_1", 1, 12f, 1f, 1f);
            sys.Tick(Rng(1));

            var state = sys.CaptureState();
            var sys2 = new WeatherSondeSystem(weather);
            sys2.RestoreState(state);

            Assert.Equal(sys.State.ticksElapsed, sys2.State.ticksElapsed);
            Assert.Equal(sys.State.samples.Count, sys2.State.samples.Count);
        }

        [Fact]
        public void CaptureState_StableChecksum()
        {
            var weather = DemoWeather();
            var sys = new WeatherSondeSystem(weather);
            sys.Launch("sonde_1", 1, 12f, 1f, 1f);
            sys.Tick(Rng(1));
            string before = SaveChecksum.Compute(sys.CaptureState());

            var sys2 = new WeatherSondeSystem(weather);
            sys2.RestoreState(sys.CaptureState());
            string after = SaveChecksum.Compute(sys2.CaptureState());

            Assert.Equal(before, after);
        }

        [Fact]
        public void Forecast_SurvivesSaveLoad()
        {
            var weather = DemoWeather();
            var sys = new WeatherSondeSystem(weather);
            sys.Launch("sonde_1", 1, 12f, 1f, 1f);
            for (int i = 0; i < WeatherSondeSystem.DefaultFlightDurationTicks; i++)
                sys.Tick(Rng(i));

            var state = sys.CaptureState();
            var sys2 = new WeatherSondeSystem(weather);
            sys2.RestoreState(state);

            Assert.Equal(sys.State.forecast.Count, sys2.State.forecast.Count);
        }

        // ── Queries ──────────────────────────────────────────────────

        [Fact]
        public void GetSampleCount_ReturnsCorrectCount()
        {
            var weather = DemoWeather();
            var sys = new WeatherSondeSystem(weather);
            sys.Launch("sonde_1", 1, 12f, 1f, 1f);
            sys.Tick(Rng(1));
            sys.Tick(Rng(2));
            Assert.Equal(2, sys.GetSampleCount());
        }

        [Fact]
        public void GetCurrentAltitude_ReturnsZeroWhenNotLaunched()
        {
            var weather = DemoWeather();
            var sys = new WeatherSondeSystem(weather);
            Assert.Equal(0f, sys.GetCurrentAltitude());
        }
    }
}
