using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public class Plan19DynamicWorldTests
    {
        private static readonly string DataDir = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data");

        private static WeatherSystem CreateWeather(int seed = 42)
        {
            var ws = new WeatherSystem();
            var profile = WeatherProfileLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer())
                          ?? new SeasonProfileDef();
            ws.BindProfile(profile, seed);
            return ws;
        }

        // ────────────────────────────────────────────────────────────────────────
        // 1. Weather Forecasting Core & Station Tiers (Tasks 19A, 19B, 19D, 19V)
        // ────────────────────────────────────────────────────────────────────────

        [Fact]
        public void WeatherForecast_PeekDoesNotMutateWeatherState()
        {
            var weather = CreateWeather(12345);
            var initialRoll = weather.State.rollCount;
            var initialHours = weather.State.totalElapsedHours;
            var initialKind = weather.Current;

            var forecast = weather.PeekForecast(7);

            Assert.Equal(7, forecast.Count);
            Assert.Equal(initialRoll, weather.State.rollCount);
            Assert.Equal(initialHours, weather.State.totalElapsedHours);
            Assert.Equal(initialKind, weather.Current);
        }

        [Fact]
        public void WeatherForecast_SameSeedProducesDeterministicForecast()
        {
            var w1 = CreateWeather(9999);
            var w2 = CreateWeather(9999);

            var f1 = w1.PeekForecast(7);
            var f2 = w2.PeekForecast(7);

            Assert.Equal(f1.Count, f2.Count);
            for (int i = 0; i < f1.Count; i++)
            {
                Assert.Equal(f1[i].Day, f2[i].Day);
                Assert.Equal(f1[i].Kind, f2[i].Kind);
                Assert.Equal(f1[i].OutdoorRad, f2[i].OutdoorRad);
                Assert.Equal(f1[i].Visibility, f2[i].Visibility);
            }
        }

        [Fact]
        public void WeatherStation_TierProgression_AffectsHorizonAndConfidence()
        {
            var weather = CreateWeather(5555);
            var station = new WeatherStationSystem(weather, new SeededRng(5555));

            // 1. Offline tier
            Assert.Equal(WeatherStationTier.Offline, station.CurrentTier);
            Assert.Equal(0, station.EffectiveHorizonDays);
            var resOffline = station.GenerateForecast(1);
            Assert.Equal(ActionResult.StatusKind.Blocked, resOffline.Status);

            // 2. Uncalibrated tier (Installed, uncalibrated - offline from forecasting)
            station.Install(1);
            Assert.Equal(WeatherStationTier.Functional, station.CurrentTier);
            var resFunc = station.GenerateForecast(1);
            Assert.Equal(ActionResult.StatusKind.Blocked, resFunc.Status);

            // 3. Calibrated tier
            station.Calibrate(2);
            Assert.Equal(WeatherStationTier.Calibrated, station.CurrentTier);
            Assert.Equal(7, station.EffectiveHorizonDays);
            var resCal = station.GenerateForecast(2);
            Assert.Equal(ActionResult.StatusKind.Success, resCal.Status);
            Assert.Equal(7, station.GetForecast().Count);
            Assert.True(station.GetForecast()[0].confidence > 0.70f);

            // 4. Damaged tier (degraded durability)
            station.Degrade(70f); // durability becomes 30f < 40f
            Assert.Equal(WeatherStationTier.Damaged, station.CurrentTier);
            Assert.Equal(1, station.EffectiveHorizonDays);
            station.GenerateForecast(3);
            Assert.Single(station.GetForecast());
            Assert.True(station.GetForecast()[0].confidence <= 0.40f);
        }

        [Fact]
        public void WeatherStation_PreparationPayoffs_ProvideActionableAdvice()
        {
            var stormAdvice = WeatherStationSystem.GetPreparationPayoff(WeatherKind.FalloutStorm);
            var blackRainAdvice = WeatherStationSystem.GetPreparationPayoff(WeatherKind.BlackRain);
            var blizzardAdvice = WeatherStationSystem.GetPreparationPayoff(WeatherKind.Blizzard);
            var clearAdvice = WeatherStationSystem.GetPreparationPayoff(WeatherKind.Clear);

            Assert.Contains("filters", stormAdvice, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("cisterns", blackRainAdvice, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("heating", blizzardAdvice, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("overland", clearAdvice, StringComparison.OrdinalIgnoreCase);
        }

        // ────────────────────────────────────────────────────────────────────────
        // 2. Orbital Harrow Telemetry & Kinetic Impacts (Tasks 19F–19M)
        // ────────────────────────────────────────────────────────────────────────

        [Fact]
        public void OrbitalCatalog_LoadsAllFiveTemplates()
        {
            var list = OrbitalHarrowCatalogLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.True(list.Count >= 5);
            Assert.Contains(list, e => e.id == "event_orbital_kinetic_early_track");
            Assert.Contains(list, e => e.id == "event_orbital_kinetic_thermal_descent");
            Assert.Contains(list, e => e.id == "event_orbital_kinetic_seismic_precursor");
            Assert.Contains(list, e => e.id == "event_orbital_kinetic_fragmented_track");
            Assert.Contains(list, e => e.id == "event_orbital_cluster_multiple_returns");
        }

        [Fact]
        public void OrbitalImpact_EvaluatesSkyArmor_GeneratesSalvageAndRevealsSite()
        {
            var armor = new SkyLayerArmorSystem();
            armor.SetCellArmor(5, CeilingMaterialTier.ReinforcedConcrete, 1.5f, 100f);
            armor.SetCellArmor(6, CeilingMaterialTier.ReinforcedConcrete, 1.5f, 100f);

            var orbital = new OrbitalHarrowTelemetrySystem(armor, new SeededRng(777));
            orbital.ActivateTelemetry(1);

            var heavyEvent = new OrbitalEventDef
            {
                id = "event_orbital_heavy_kinetic_impact",
                name = "Tungsten Penetrator Plunge",
                severity = "Severe",
                impact_energy_mj = 50f,
                affected_cell_spread = 2,
                salvage_yield_item_id = "scrap_electronic",
                salvage_yield_quantity = 6,
                revealed_site_id = "loc_excavation_command_vault"
            };

            orbital.ScheduleEventDef(heavyEvent, day: 5, gridX: 5);
            Assert.True(orbital.HasPendingImpact);

            OrbitalImpactReport? report = null;
            orbital.OnImpactDetailed += r => report = r;

            orbital.TickDay(5);

            Assert.False(orbital.HasPendingImpact);
            Assert.NotNull(report);
            Assert.Equal(5, report!.Day);
            Assert.Equal("event_orbital_heavy_kinetic_impact", report.EventId);
            Assert.Equal(2, report.CellsAffected);
            Assert.NotEmpty(orbital.ActiveSalvage);
            Assert.Contains("loc_excavation_command_vault", orbital.RevealedSites);

            // Claim salvage
            var claimRes = orbital.ClaimSalvage("event_orbital_heavy_kinetic_impact");
            Assert.Equal(ActionResult.StatusKind.Success, claimRes.Status);
        }

        // ────────────────────────────────────────────────────────────────────────
        // 3. Seasonal Phase Model & Event Cadence (Tasks 19N–19T)
        // ────────────────────────────────────────────────────────────────────────

        [Fact]
        public void SeasonModel_DefinesSixPhasesAcrossYear()
        {
            var weather = CreateWeather(100);
            var p0 = weather.GetSeasonForDay(0);
            var p1 = weather.GetSeasonForDay(75);
            var p2 = weather.GetSeasonForDay(140);
            var p3 = weather.GetSeasonForDay(200);
            var p4 = weather.GetSeasonForDay(260);
            var p5 = weather.GetSeasonForDay(320);

            Assert.Equal("window_ashfall", p0.id);
            Assert.Equal("window_deep_freeze", p1.id);
            Assert.Equal("window_thaw", p2.id);
            Assert.Equal("window_black_bloom", p3.id);
            Assert.Equal("window_high_cold", p4.id);
            Assert.Equal("window_the_turning", p5.id);
        }

        [Fact]
        public void SeasonalEvents_LoadCatalog_AndTriggerDeterministically()
        {
            var events = SeasonalEventCatalogLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.True(events.Count >= 18);

            var system = new SeasonalEventSystem();
            system.BindDefinitions(events);

            var rng = new SeededRng(4242);
            // Simulate 60 days in Ashfall season
            for (int day = 1; day <= 60; day++)
            {
                system.TickDay(day, "window_ashfall", rng);
            }

            Assert.NotEmpty(system.ActiveEvents);
            var active = system.ActiveEvents[0];
            Assert.False(active.isMitigated);

            // Mitigate event
            var mitRes = system.Mitigate(active.eventId);
            Assert.Equal(ActionResult.StatusKind.Success, mitRes.Status);
            Assert.True(active.isMitigated);
        }

        // ────────────────────────────────────────────────────────────────────────
        // 4. Coordinator & Save Round-Trip (Tasks 19AC, 19AD, 19AJ)
        // ────────────────────────────────────────────────────────────────────────

        [Fact]
        public void WeatherIntelligenceCoordinator_SaveRestoreRoundTrip_PreservesAllStates()
        {
            var weather = CreateWeather(8888);
            var armor = new SkyLayerArmorSystem();
            armor.SetCellArmor(1, CeilingMaterialTier.LeadSheeting, 2.0f, 100f);

            var coord = new WeatherIntelligenceCoordinator(weather, armor, new SeededRng(8888));
            var events = SeasonalEventCatalogLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            coord.Seasonal.BindDefinitions(events);

            coord.Station.Install(1);
            coord.Station.Calibrate(2);
            coord.Station.GenerateForecast(3);

            coord.Orbital.ActivateTelemetry(1);
            coord.Orbital.ScheduleImpact(10, 1, 20f);

            coord.TickDay(3);

            var save = coord.CaptureState();
            Assert.True(save.station.isInstalled);
            Assert.True(save.station.isCalibrated);
            Assert.True(save.orbital.telemetryActive);
            Assert.Equal(10, save.orbital.nextImpactDay);

            var json = new SystemTextJsonSerializer().Serialize(save);
            var restoredSave = new SystemTextJsonSerializer().Deserialize<WeatherIntelligenceSaveState>(json);
            Assert.NotNull(restoredSave);

            var coord2 = new WeatherIntelligenceCoordinator(weather, armor, new SeededRng(9999));
            coord2.RestoreState(restoredSave);

            Assert.True(coord2.Station.State.isInstalled);
            Assert.True(coord2.Station.State.isCalibrated);
            Assert.True(coord2.Orbital.State.telemetryActive);
            Assert.Equal(10, coord2.Orbital.State.nextImpactDay);

            var readModel = coord2.BuildReadModel();
            Assert.Equal(WeatherStationTier.Calibrated, readModel.stationTier);
            Assert.True(readModel.telemetryActive);
            Assert.True(readModel.hasPendingImpact);
            Assert.NotEmpty(readModel.forecast);
            Assert.NotEmpty(readModel.advisory);
        }
    }
}
