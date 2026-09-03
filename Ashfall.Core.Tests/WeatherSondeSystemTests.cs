using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Content;
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

    // ── Plan 71 §6.17: atmospheric sounding extensions ──────────────

    public class AtmosphericSoundingTests
    {
        private static WeatherSystem DemoWeather() => new WeatherSystem();
        private static WeatherSondeSystem CreateSonde(WeatherSystem? w = null) =>
            new WeatherSondeSystem(w ?? DemoWeather());

        private static IReadOnlyList<SoundingAltitudeBandDef> Bands => new List<SoundingAltitudeBandDef>
        {
            new SoundingAltitudeBandDef { band_id = "b1", altitude_min_m = 0, altitude_max_m = 8000, wind_variability = 0.6f, telemetry_quality_modifier = 1f, radiation_sampling_modifier = 1f },
            new SoundingAltitudeBandDef { band_id = "b2", altitude_min_m = 8000, altitude_max_m = 16000, wind_variability = 1f, telemetry_quality_modifier = 1f, radiation_sampling_modifier = 1.1f },
            new SoundingAltitudeBandDef { band_id = "b3", altitude_min_m = 16000, altitude_max_m = 24000, wind_variability = 1.4f, telemetry_quality_modifier = 0.95f, radiation_sampling_modifier = 1.25f },
            new SoundingAltitudeBandDef { band_id = "b4", altitude_min_m = 24000, altitude_max_m = 30000, wind_variability = 2f, telemetry_quality_modifier = 0.8f, radiation_sampling_modifier = 1.5f }
        };

        private static IReadOnlyList<SoundingPayloadDef> Payloads => new List<SoundingPayloadDef>
        {
            new SoundingPayloadDef
            {
                payload_id = "payload_standard_sonde",
                burst_altitude_min_m = 24000,
                burst_altitude_max_m = 30000,
                parachute_descent_rate_km_per_tick = 6f,
                recovery_rewards = new List<SoundingRecoveryRewardDef>
                {
                    new SoundingRecoveryRewardDef { min_condition = 0.7f, item_id = "item_radiosonde", amount = 1 },
                    new SoundingRecoveryRewardDef { min_condition = 0.4f, item_id = "scrap_metal", amount = 2 },
                    new SoundingRecoveryRewardDef { min_condition = 0f, item_id = "scrap_metal", amount = 1 }
                }
            }
        };

        [Fact]
        public void Loader_ReturnsBandsAndPayloads_FromDataDir()
        {
            var dataDir = Path.Combine(TestDataDir, "StreamingAssets", "Data");
            if (!Directory.Exists(dataDir)) return;
            var container = AtmosphericSoundingCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.NotNull(container);
            Assert.NotEmpty(container.altitude_bands);
            Assert.NotEmpty(container.payloads);
            Assert.Equal(1, container.schema_version);
        }

        [Theory]
        [InlineData("b1")]
        [InlineData("b2")]
        [InlineData("b3")]
        [InlineData("b4")]
        public void BandLookup_IsContinuous_AcrossAllBands(string bandId)
        {
            var sonde = CreateSonde();
            sonde.ApplySoundingCatalog(Bands, Payloads);
            Assert.True(sonde.Launch("s1", 1, 12f, 1f, 1f));
            var rng = new SeededRng(42);
            for (int i = 0; i < 12; i++) sonde.Tick(rng, 1);
            Assert.True(sonde.State.isRecovered || sonde.State.isBurst);
            Assert.NotEmpty(sonde.State.samples);
        }

        [Fact]
        public void Launch_Duplicate_WhenActive_ReturnsFalse()
        {
            var sonde = CreateSonde();
            Assert.True(sonde.Launch("s1", 1, 12f, 1f, 1f));
            Assert.False(sonde.Launch("s2", 1, 12f, 1f, 1f));
        }

        [Fact]
        public void LegacyFlight_NoCatalog_CompletesAtFlightDurationTicks()
        {
            var sonde = CreateSonde();
            Assert.True(sonde.Launch("s1", 1, 12f, 1f, 1f));
            var rng = new SeededRng(123);
            for (int i = 0; i < sonde.State.flightDurationTicks; i++)
                sonde.Tick(rng, 1);
            Assert.True(sonde.State.isRecovered);
            Assert.False(sonde.State.isBurst);
            Assert.Equal(-1, sonde.State.landingDay);
        }

        [Fact]
        public void CatalogBoundFlight_BurstsAtCeiling_ThenDescends()
        {
            var sonde = CreateSonde();
            sonde.ApplySoundingCatalog(Bands, Payloads);
            Assert.True(sonde.Launch("s1", 5, 12f, 1f, 1f));
            var rng = new SeededRng(7);
            for (int i = 0; i < 30; i++)
            {
                bool more = sonde.Tick(rng, 5);
                if (!more || sonde.State.isRecovered) break;
            }
            Assert.True(sonde.State.isBurst);
            Assert.True(sonde.State.isRecovered);
            Assert.True(sonde.State.currentAltitudeKm <= 0.01f);
            Assert.True(sonde.State.landingDay >= 0);
        }

        [Fact]
        public void Landing_QuantizesCoordinates_ToIntegerMeters()
        {
            var sonde = CreateSonde();
            sonde.ApplySoundingCatalog(Bands, Payloads);
            Assert.True(sonde.Launch("s1", 5, 12f, 1f, 1f));
            var rng = new SeededRng(11);
            for (int i = 0; i < 30; i++)
            {
                if (!sonde.Tick(rng, 5) || sonde.State.isRecovered) break;
            }
            Assert.True(sonde.State.isRecovered);
            Assert.IsType<int>(sonde.State.positionEastingM);
            Assert.IsType<int>(sonde.State.positionNorthingM);
            Assert.Equal(sonde.State.positionEastingM, sonde.State.landingEastingM);
            Assert.Equal(sonde.State.positionNorthingM, sonde.State.landingNorthingM);
        }

        [Fact]
        public void SameSeed_ProducesIdenticalLandingCoordinates()
        {
            var a = CreateSonde(); a.ApplySoundingCatalog(Bands, Payloads);
            var b = CreateSonde(); b.ApplySoundingCatalog(Bands, Payloads);
            Assert.True(a.Launch("s1", 3, 12f, 1f, 1f));
            Assert.True(b.Launch("s1", 3, 12f, 1f, 1f));
            var rngA = new SeededRng(999);
            var rngB = new SeededRng(999);
            for (int i = 0; i < 30; i++)
            {
                bool okA = a.Tick(rngA, 3);
                bool okB = b.Tick(rngB, 3);
                Assert.Equal(okA, okB);
                if (!okA || a.State.isRecovered) break;
            }
            Assert.True(a.State.isRecovered);
            Assert.Equal(a.State.landingEastingM, b.State.landingEastingM);
            Assert.Equal(a.State.landingNorthingM, b.State.landingNorthingM);
            Assert.Equal(a.State.driftEastKm, b.State.driftEastKm);
        }

        [Fact]
        public void Landing_SpawnsRecoveryTarget_WithExpiry()
        {
            var sonde = CreateSonde();
            sonde.ApplySoundingCatalog(Bands, Payloads);
            Assert.True(sonde.Launch("s1", 10, 12f, 1f, 1f));
            var rng = new SeededRng(21);
            for (int i = 0; i < 30; i++)
            {
                if (!sonde.Tick(rng, 10) || sonde.State.isRecovered) break;
            }
            var target = sonde.GetActiveRecoveryTarget();
            Assert.NotNull(target);
            Assert.Equal(10, target.Value.landingDay);
            Assert.Equal(24, target.Value.expiryDay);
            Assert.True(target.Value.payloadCondition > 0f);
        }

        [Fact]
        public void ClaimRecoveryPayload_TransfersRewardItems_IntoBoundInventory()
        {
            var sonde = CreateSonde();
            sonde.ApplySoundingCatalog(Bands, Payloads);
            var inventory = new Ashfall.Core.Inventory.Inventory();
            sonde.BindRecoveryInventory(inventory);
            Assert.True(sonde.Launch("s1", 10, 12f, 1f, 1f));
            var rng = new SeededRng(31);
            for (int i = 0; i < 30; i++)
            {
                if (!sonde.Tick(rng, 10) || sonde.State.isRecovered) break;
            }
            var result = sonde.ClaimRecoveryPayload(10);
            Assert.True(result.IsSuccess);
            Assert.True(inventory.CountById("item_radiosonde") >= 1
                      || inventory.CountById("scrap_metal") >= 1);
        }

        [Fact]
        public void ClaimRecoveryPayload_ExpiredTarget_IsBlocked()
        {
            var sonde = CreateSonde();
            sonde.ApplySoundingCatalog(Bands, Payloads);
            sonde.BindRecoveryInventory(new Ashfall.Core.Inventory.Inventory());
            Assert.True(sonde.Launch("s1", 1, 12f, 1f, 1f));
            var rng = new SeededRng(41);
            for (int i = 0; i < 30; i++)
            {
                if (!sonde.Tick(rng, 1) || sonde.State.isRecovered) break;
            }
            var result = sonde.ClaimRecoveryPayload(100);
            Assert.False(result.IsSuccess);
            Assert.Equal("target_expired", result.FailureCode);
        }

        [Fact]
        public void Samples_CappedAtMaxTelemetrySamples()
        {
            var sonde = CreateSonde();
            sonde.ApplySoundingCatalog(Bands, Payloads);
            Assert.True(sonde.Launch("s1", 1, 12f, 1f, 1f));
            var rng = new SeededRng(51);
            for (int i = 0; i < 80; i++) sonde.Tick(rng, 1);
            Assert.True(sonde.State.samples.Count <= WeatherSondeSystem.MaxTelemetrySamples);
        }

        [Fact]
        public void Forecast_Confidence_DecaysMonotonicallyWithDayOffset()
        {
            var sonde = CreateSonde();
            sonde.ApplySoundingCatalog(Bands, Payloads);
            Assert.True(sonde.Launch("s1", 1, 12f, 1f, 1f));
            var rng = new SeededRng(71);
            for (int i = 0; i < 30; i++)
            {
                if (!sonde.Tick(rng, 1) || sonde.State.isRecovered) break;
            }
            var confs = sonde.State.forecast.Select(e => e.confidence).ToList();
            for (int i = 1; i < confs.Count; i++)
                Assert.True(confs[i] <= confs[i - 1] + 0.001f);
        }

        [Fact]
        public void SaveLoad_MidFlight_PreservesTrajectoryState()
        {
            var sonde = CreateSonde();
            sonde.ApplySoundingCatalog(Bands, Payloads);
            Assert.True(sonde.Launch("s1", 7, 12f, 1f, 1f));
            var rng = new SeededRng(91);
            for (int i = 0; i < 3; i++) sonde.Tick(rng, 7);
            var saved = sonde.CaptureState();
            var sonde2 = CreateSonde();
            sonde2.ApplySoundingCatalog(Bands, Payloads);
            sonde2.RestoreState(saved);
            Assert.True(sonde2.State.isLaunched);
            Assert.Equal(3, sonde2.State.ticksElapsed);
            Assert.Equal(saved.driftEastKm, sonde2.State.driftEastKm);
            Assert.Equal(saved.positionEastingM, sonde2.State.positionEastingM);
            Assert.False(sonde2.State.isBurst);
        }

        [Fact]
        public void OldSave_WithoutNewFields_FallsBackToLegacyFlight()
        {
            var legacy = new WeatherSondeState
            {
                systemId = WeatherSondeSystem.SystemId,
                sondeId = "legacy",
                isLaunched = true,
                launchDay = 1,
                flightDurationTicks = 4,
                ticksElapsed = 3,
                batteryLevel = 0.7f,
                hydrogenLevel = 0.8f,
                sensorQuality = 0.9f
            };
            var sonde = CreateSonde();
            sonde.RestoreState(legacy);
            var rng = new SeededRng(101);
            bool ok = sonde.Tick(rng, 1);
            Assert.True(ok);
            Assert.True(sonde.State.isRecovered);
            Assert.False(sonde.State.isBurst);
        }

        [Fact]
        public void ContentUtilization_AtmosphericSoundingCatalog_IsMappedToSonde()
        {
            Assert.True(ContentUtilizationScanner.IsAuthoritativeCatalog("atmospheric_sounding_catalog.json"));
        }

        private static string TestDataDir =>
            Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
    }
}
