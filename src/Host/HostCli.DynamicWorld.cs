using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        /// <summary>
        /// --dynamic-world-selftest / --plan19-selftest:
        /// Verifies Plan 19 Dynamic World Systems:
        /// Weather forecasting lookahead, weather station tiers (offline, damaged, functional, calibrated),
        /// 6-phase seasonal calendar, seasonal events (18+ events),
        /// Orbital Harrow kinetic strike event templates (5 templates),
        /// Sky armor impact cascades, salvage opportunity generation, site reveals,
        /// and save/load persistence.
        /// </summary>
        public static int RunDynamicWorldSelfTest(string dataDirectory)
        {
            CatalogLocator.UseInvariantCulture();
            int failures = 0;
            int totalAssertions = 0;

            void Check(bool ok, string label)
            {
                totalAssertions++;
                GD.Print($"[{(ok ? "PASS" : "FAIL")}] {label}");
                if (!ok) failures++;
            }

            GD.Print("[DynamicWorldHeadlessDemo] begin Plan 19 verification...");

            var json = new SystemTextJsonSerializer();
            var files = new FileSystemIO();

            // 1. Weather Seasons & 6 Phases (weather_seasons.json)
            var seasonProfile = WeatherProfileLoader.Load(dataDirectory, files, json);
            Check(seasonProfile != null, "Weather seasons profile loaded");
            Check(seasonProfile != null && seasonProfile.seasons.Count >= 6, $"Season phase count (expected >= 6, got {seasonProfile?.seasons.Count ?? 0})");
            Check(seasonProfile != null && seasonProfile.seasons.Any(s => s.id == "window_ashfall"), "Season phase window_ashfall present");
            Check(seasonProfile != null && seasonProfile.seasons.Any(s => s.id == "window_deep_freeze"), "Season phase window_deep_freeze present");
            Check(seasonProfile != null && seasonProfile.seasons.Any(s => s.id == "window_thaw"), "Season phase window_thaw present");
            Check(seasonProfile != null && seasonProfile.seasons.Any(s => s.id == "window_black_bloom"), "Season phase window_black_bloom present");
            Check(seasonProfile != null && seasonProfile.seasons.Any(s => s.id == "window_high_cold"), "Season phase window_high_cold present");
            Check(seasonProfile != null && seasonProfile.seasons.Any(s => s.id == "window_the_turning"), "Season phase window_the_turning present");

            // 2. Weather System & Non-mutating Lookahead
            var weather = new WeatherSystem();
            if (seasonProfile != null)
                weather.BindProfile(seasonProfile, seed: 12345);

            int initialRoll = weather.State.rollCount;
            var peek = weather.PeekForecast(7);
            Check(peek.Count == 7, "WeatherSystem.PeekForecast(7) returns 7 days");
            Check(weather.State.rollCount == initialRoll, "PeekForecast does not mutate simulation rollCount");

            // 3. Weather Station Tiers & Forecasting
            var station = new WeatherStationSystem(weather, new SeededRng(12345));
            Check(station.CurrentTier == WeatherStationTier.Offline, "Uninstalled station is Tier Offline");
            Check(station.EffectiveHorizonDays == 0, "Offline station horizon is 0 days");

            station.Install(1);
            Check(station.CurrentTier == WeatherStationTier.Functional, "Installed station is Tier Functional");
            Check(station.State.isInstalled, "Station marked installed");

            station.Calibrate(2);
            Check(station.CurrentTier == WeatherStationTier.Calibrated, "Calibrated station is Tier Calibrated");
            Check(station.EffectiveHorizonDays == 7, "Calibrated station horizon is 7 days");

            var forecastRes = station.GenerateForecast(3);
            Check(forecastRes.Status == ActionResult.StatusKind.Success, "Calibrated forecast generated successfully");
            Check(station.GetForecast().Count == 7, "Forecast contains 7 entries");
            Check(!string.IsNullOrEmpty(station.GetForecast()[0].preparationPayoff), "Forecast entries contain actionable preparation payoffs");
            Check(!string.IsNullOrEmpty(station.GetForecast()[0].atmosphericFlavor), "Forecast entries contain atmospheric flavor");

            // Station degradation
            station.Degrade(70f);
            Check(station.CurrentTier == WeatherStationTier.Damaged, "Degraded station drops to Tier Damaged");
            station.GenerateForecast(4);
            Check(station.GetForecast().Count == 1, "Damaged station horizon restricted to 1 day");

            // 4. Orbital Harrow Event Templates (orbital_harrow_events.json)
            var orbitalEvents = OrbitalHarrowCatalogLoader.Load(dataDirectory, files, json);
            Check(orbitalEvents.Count >= 5, $"Orbital Harrow event templates count (expected >= 5, got {orbitalEvents.Count})");
            Check(orbitalEvents.Any(e => e.id == "event_orbital_small_debris_shower"), "Template event_orbital_small_debris_shower present");
            Check(orbitalEvents.Any(e => e.id == "event_orbital_heavy_kinetic_impact"), "Template event_orbital_heavy_kinetic_impact present");
            Check(orbitalEvents.Any(e => e.id == "event_orbital_clustered_impact"), "Template event_orbital_clustered_impact present");
            Check(orbitalEvents.Any(e => e.id == "event_orbital_near_miss_shockwave"), "Template event_orbital_near_miss_shockwave present");
            Check(orbitalEvents.Any(e => e.id == "event_orbital_low_warning_strike"), "Template event_orbital_low_warning_strike present");

            // 5. Orbital Harrow Telemetry & Sky Armor Impact Cascades
            var armor = new SkyLayerArmorSystem();
            armor.SetCellArmor(10, CeilingMaterialTier.ReinforcedConcrete, 1.5f, 100f);
            armor.SetCellArmor(11, CeilingMaterialTier.ReinforcedConcrete, 1.5f, 100f);

            var orbital = new OrbitalHarrowTelemetrySystem(armor, new SeededRng(54321));
            orbital.ActivateTelemetry(1);
            Check(orbital.State.telemetryActive, "Orbital telemetry activated");

            var heavyStrike = orbitalEvents.Find(e => e.id == "event_orbital_heavy_kinetic_impact");
            if (heavyStrike != null)
                orbital.ScheduleEventDef(heavyStrike, day: 10, gridX: 10);

            Check(orbital.HasPendingImpact, "Pending impact registered");
            Check(orbital.State.warnings.Count >= 1, "Impact warning generated");

            // Brace impact
            var braceRes = orbital.Brace("concrete", 5);
            Check(braceRes.Status == ActionResult.StatusKind.Success, "Bracing successful");
            Check(orbital.State.isBraced, "Braced status active");

            // Resolve impact on Day 10
            OrbitalImpactReport? impactReport = null;
            orbital.OnImpactDetailed += rep => impactReport = rep;
            orbital.TickDay(10);

            Check(!orbital.HasPendingImpact, "Impact resolved on Day 10");
            Check(impactReport != null, "Detailed impact report generated");
            Check(orbital.ActiveSalvage.Count > 0, "Post-strike salvage opportunity spawned");
            Check(orbital.RevealedSites.Contains("loc_excavation_command_vault"), "Rare site revealed from strike");

            // Claim salvage
            var claimRes = orbital.ClaimSalvage("event_orbital_heavy_kinetic_impact");
            Check(claimRes.Status == ActionResult.StatusKind.Success, "Salvage claimed successfully");

            // 6. Seasonal Events (seasonal_events.json)
            var seasonalEvents = SeasonalEventCatalogLoader.Load(dataDirectory, files, json);
            Check(seasonalEvents.Count >= 18, $"Seasonal events count (expected >= 18, got {seasonalEvents.Count})");
            Check(seasonalEvents.Any(e => e.id == "event_season_ash_filter_clog"), "Event event_season_ash_filter_clog present");
            Check(seasonalEvents.Any(e => e.id == "event_season_freeze_pipe_burst"), "Event event_season_freeze_pipe_burst present");
            Check(seasonalEvents.Any(e => e.id == "event_season_thaw_sump_flood"), "Event event_season_thaw_sump_flood present");
            Check(seasonalEvents.Any(e => e.id == "event_season_bloom_greenhouse_spores"), "Event event_season_bloom_greenhouse_spores present");
            Check(seasonalEvents.Any(e => e.id == "event_season_highcold_generator_stall"), "Event event_season_highcold_generator_stall present");
            Check(seasonalEvents.Any(e => e.id == "event_season_turning_clear_sky_window"), "Event event_season_turning_clear_sky_window present");

            var seasonalSys = new SeasonalEventSystem();
            seasonalSys.BindDefinitions(seasonalEvents);
            seasonalSys.TickDay(1, "window_ashfall", new SeededRng(1111));

            // 7. Weather Intelligence Coordinator & Save Round-Trip
            var coord = new WeatherIntelligenceCoordinator(weather, armor, new SeededRng(777));
            coord.Seasonal.BindDefinitions(seasonalEvents);
            coord.Station.Install(1);
            coord.Station.Calibrate(2);
            coord.Orbital.ActivateTelemetry(1);
            coord.Orbital.ScheduleImpact(15, 3, 25f);
            coord.TickDay(3);

            var saveState = coord.CaptureState();
            Check(saveState.station.isInstalled, "Save captures station state");
            Check(saveState.orbital.telemetryActive, "Save captures orbital telemetry state");

            var saveJson = json.Serialize(saveState);
            var restoredState = json.Deserialize<WeatherIntelligenceSaveState>(saveJson);
            Check(restoredState != null, "WeatherIntelligenceSaveState deserializes cleanly");

            var coordRestored = new WeatherIntelligenceCoordinator(weather, armor, new SeededRng(888));
            coordRestored.RestoreState(restoredState);
            var readModel = coordRestored.BuildReadModel();

            Check(readModel.stationInstalled && readModel.stationCalibrated, "Restored coordinator station is operational");
            Check(readModel.telemetryActive && readModel.hasPendingImpact, "Restored coordinator orbital telemetry is active with pending impact");
            Check(!string.IsNullOrEmpty(readModel.advisory), "Restored coordinator read model advisory is populated");

            GD.Print($"[DynamicWorldHeadlessDemo] {(failures == 0 ? "PASS" : "FAIL")} {totalAssertions - failures}/{totalAssertions}");
            string status = failures == 0 ? "PASS" : "FAIL";
            GD.Print($"[HOST_SELFTEST] dynamic_world_selftest {status}");
            GD.Print($"[HOST_SELFTEST_SUMMARY] test=dynamic_world_selftest status={status} exit_code={(failures == 0 ? 0 : 1)} passed={totalAssertions - failures} failed={failures} total={totalAssertions} details=\"[DynamicWorldHeadlessDemo] {status} {totalAssertions - failures}/{totalAssertions}\"");
            GD.Print($"[HOST_SELFTEST_JSON] {{\"test\":\"dynamic_world_selftest\",\"status\":\"{status}\",\"exit_code\":{(failures == 0 ? 0 : 1)},\"passed\":{totalAssertions - failures},\"failed\":{failures},\"total\":{totalAssertions},\"details\":\"[DynamicWorldHeadlessDemo] {status} {totalAssertions - failures}/{totalAssertions}\"}}");
            GD.Print($"SELFTEST {status}: dynamic_world_selftest");
            GD.Print($"DYNAMIC_WORLD_SELFTEST {status}");

            return failures == 0 ? 0 : 1;
        }
    }
}
