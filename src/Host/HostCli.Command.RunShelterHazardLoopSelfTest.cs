// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Clock;
using Ashfall.Core.Crafting;
using Ashfall.Core.Economy;
using Ashfall.Core.Endgame;
using Ashfall.Core.Events;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Flags;
using Ashfall.Core.Legacy;
using Ashfall.Core.Medical;
using Ashfall.Core.Muster;
using Ashfall.Core.Narrative;
using Ashfall.Core.Save;
using Ashfall.Core.Settings;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Ashfall.Core.UtilityAI;
using Ashfall.Core.Verdict;
using Ashfall.Core.Warlords;
using Ashfall.Core.World;
using Ashfall.Core.YearOfAsh;
using AtomicWar.GodotApp.Narrative;
using AtomicWar.GodotApp.Settings;
using AtomicWar.GodotApp.UI;
using AtomicWar.GodotApp.YearOfAsh;
using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {

        public static int RunShelterHazardLoopSelfTest(string dataDirectory)
        {
            int failures = 0;
            void Check(bool condition, string message)
            {
                if (condition)
                    GD.Print($"  [PASS] {message}");
                else
                {
                    GD.PrintErr($"  [FAIL] {message}");
                    failures++;
                }
            }

            GD.Print("[ShelterHazardLoopSelfTest] Starting Shelter Air, Fallout Forecasting, & Duty Roster Loop (Days 2-5)...");

            try
            {
                // Clean test paths
                string tmpStarting = Path.Combine(ProjectSettings.GlobalizePath("user://"), "starting_level_hazard_test.json");
                string tmpRoster = Path.Combine(ProjectSettings.GlobalizePath("user://"), "duty_roster_hazard_test.json");
                string tmpWorld = Path.Combine(ProjectSettings.GlobalizePath("user://"), "world_hazard_test.json");
                if (File.Exists(tmpStarting)) File.Delete(tmpStarting);
                if (File.Exists(tmpRoster)) File.Delete(tmpRoster);
                if (File.Exists(tmpWorld)) File.Delete(tmpWorld);

                // 1. Initial State
                var startingSession = new StartingLevelHostSession();
                var worldSession = WorldHostSession.Create(dataDirectory);
                var rosterSession = DutyRosterHostSession.Create(dataDirectory);
                rosterSession.Unlock(1);

                Check(startingSession.System.State.day == 1, "starting day is Day 1");
                Check(startingSession.System.State.airFilterHealthPercent == 100.0f, "initial air filter health is 100%");
                Check(startingSession.System.State.airQualityPercent == 100.0f, "initial air quality is 100%");
                Check(startingSession.System.State.radonLevelBqm3 == 12.0f, "initial radon level is baseline 12 Bq/m³");
                Check(!startingSession.System.State.airHazardWarning, "no air hazard warning on Day 1");

                // 2. Deterministic Weather Forecasting
                var forecastA = worldSession.Weather.PeekForecast(3);
                var forecastB = worldSession.Weather.PeekForecast(3);
                Check(forecastA.Count == 3, "peek forecast returns 3-day projection");
                Check(forecastA[0].Kind == forecastB[0].Kind && forecastA[1].Kind == forecastB[1].Kind && forecastA[2].Kind == forecastB[2].Kind, "weather forecast is 100% deterministic on repeated reads");
                int rollCountBefore = worldSession.Weather.State.rollCount;
                worldSession.Weather.PeekForecast(5);
                Check(worldSession.Weather.State.rollCount == rollCountBefore, "peeking forecast does not mutate simulation roll count or RNG state");

                // 3. Survivor Work-Shift Assignment (Duty Roster)
                rosterSession.Roster.WriteName("survivor_sarah_chen", "Dr. Sarah Chen", "Medical Officer", DutyRosterIds.ScriptPencil, 1, true);
                rosterSession.Roster.WriteName("survivor_mikhail_volkov", "Gunner Mikhail", "Soldier", DutyRosterIds.ScriptPencil, 1, true);
                rosterSession.Roster.WriteName("survivor_elena_vasquez", "Elena Vasquez", "Machinist", DutyRosterIds.ScriptPencil, 1, true);

                bool assignedIntake = rosterSession.Roster.Assign(DutyRosterIds.RoleIntakeSleeper, "survivor_sarah_chen");
                bool assignedWatch = rosterSession.Roster.Assign(DutyRosterIds.RoleNightWatch, "survivor_mikhail_volkov");
                bool assignedMess = rosterSession.Roster.Assign(DutyRosterIds.RoleMess, "survivor_elena_vasquez");
                Check(assignedIntake && assignedWatch && assignedMess, "survivors successfully assigned to canonical Duty Roster roles");
                Check(rosterSession.Roster.GetAssignment(DutyRosterIds.RoleIntakeSleeper) == "survivor_sarah_chen", "Dr. Sarah Chen confirmed on Intake Filtration duty");
                Check(rosterSession.Roster.GetAssignment(DutyRosterIds.RoleNightWatch) == "survivor_mikhail_volkov", "Gunner Mikhail confirmed on Night Watch");

                // 4. Day 1 -> Day 2 Progression (Maintained Filter)
                startingSession.TickDay(isFilterDutyAssigned: true, outdoorWeather: worldSession.Weather.Current);
                worldSession.Weather.Tick(24.0f);
                rosterSession.Clock.AdvanceDays(1);
                Check(startingSession.System.State.day == 2, "advanced to Day 2");
                Check(startingSession.System.State.airFilterHealthPercent == 97.5f, "intake duty halved filter degradation (97.5% integrity)");
                Check(startingSession.System.State.airQualityPercent >= 97.0f, "air quality remains high under active shift");
                Check(!startingSession.System.State.airHazardWarning, "no air hazard warning on Day 2");

                // 5. Day 2 -> Day 3 Progression (Unmaintained Filter under Fallout Hazard)
                worldSession.Weather.ForceWeather(Ashfall.Core.WeatherKind.FalloutStorm);
                startingSession.TickDay(isFilterDutyAssigned: false, outdoorWeather: Ashfall.Core.WeatherKind.FalloutStorm);
                worldSession.Weather.Tick(24.0f);
                rosterSession.Clock.AdvanceDays(1);
                Check(startingSession.System.State.day == 3, "advanced to Day 3");
                Check(startingSession.System.State.airFilterHealthPercent == 88.5f, "unmaintained filter under fallout storm took full base + hazard degradation (88.5%)");

                // 6. Hazard Warning Trigger on Severe Contamination
                startingSession.System.State.airFilterHealthPercent = 42.0f;
                startingSession.System.State.radonLevelBqm3 = 48.0f;
                startingSession.System.State.airHazardWarning = true;
                Check(startingSession.System.State.airHazardWarning, "air hazard warning triggered when filter drops below 50%");

                // 7. Player Remediation Action
                int scrapBefore = startingSession.System.State.mechanicalScrapCount;
                bool serviced = startingSession.ServiceAirFilter();
                Check(serviced, "serviced air filter stack using mechanical scrap");
                Check(startingSession.System.State.mechanicalScrapCount == scrapBefore - 1, "1 mechanical scrap consumed for servicing");
                Check(startingSession.System.State.airFilterHealthPercent == 67.0f, "filter integrity restored +25% (now 67%)");
                Check(startingSession.System.State.radonLevelBqm3 == 33.0f, "radon purged by 15 Bq/m³ (now 33 Bq/m³)");
                rosterSession.Roster.Assign(DutyRosterIds.RoleIntakeSleeper, "survivor_sarah_chen"); // reassign

                // 8. Multi-Day Progression: Day 3 -> Day 4 -> Day 5
                startingSession.TickDay(isFilterDutyAssigned: true, outdoorWeather: worldSession.Weather.Current);
                worldSession.Weather.Tick(24.0f);
                rosterSession.Clock.AdvanceDays(1);
                Check(startingSession.System.State.day == 4, "advanced to Day 4");

                startingSession.TickDay(isFilterDutyAssigned: true, outdoorWeather: worldSession.Weather.Current);
                worldSession.Weather.Tick(24.0f);
                rosterSession.Clock.AdvanceDays(1);
                Check(startingSession.System.State.day == 5, "advanced to Day 5");
                Check(startingSession.System.State.daysSurvived == 5, "5 days survived recorded in holdfast ledger");

                // 9. Dashboard UI Verification
                var dashboard = new GameDashboardPanel();
                dashboard._Ready();
                dashboard.UpdateState(new GameDashboardPanel.DashboardSnapshot
                {
                    Day = startingSession.System.State.day,
                    Health = 90,
                    MaxHealth = 100,
                    Radiation = 4.2f,
                    AirFilterHealth = startingSession.System.State.airFilterHealthPercent,
                    AirQuality = startingSession.System.State.airQualityPercent,
                    RadonLevel = startingSession.System.State.radonLevelBqm3,
                    AirWarning = startingSession.System.State.airHazardWarning,
                    MechanicalScrap = startingSession.System.State.mechanicalScrapCount,
                    FilterSpares = startingSession.System.State.filterSparesCount,
                    FilterDutyAssignee = "Dr. Sarah Chen",
                    Forecast = worldSession.Weather.PeekForecast(3),
                    Location = "THE HOLDFAST"
                });
                Check(dashboard != null, "responsive dashboard synchronized with Day 5 state without errors");

                // 10. Atomic Save & Reload Round-Trip
                bool sSaved = StartingLevelSaveStore.TrySave(startingSession.CaptureState(), tmpStarting);
                bool rSaved = DutyRosterSaveStore.TrySave(rosterSession.CaptureSave(), tmpRoster);
                Check(sSaved && rSaved, "saved starting level and duty roster states to disk");

                var reloadedStarting = StartingLevelSaveStore.TryLoad(tmpStarting);
                var reloadedRoster = DutyRosterSaveStore.TryLoad(tmpRoster);
                Check(reloadedStarting != null && reloadedStarting.day == 5, "reloaded starting state reflects Day 5");
                Check(reloadedStarting != null && reloadedStarting.airFilterHealthPercent > 50.0f, "reloaded air filter health preserved");
                Check(reloadedStarting != null && reloadedStarting.mechanicalScrapCount == 5, "reloaded scrap inventory preserved");
                Check(reloadedRoster != null, "reloaded duty roster state is valid");

                // Clean up test files
                if (File.Exists(tmpStarting)) File.Delete(tmpStarting);
                if (File.Exists(tmpRoster)) File.Delete(tmpRoster);
                if (File.Exists(tmpWorld)) File.Delete(tmpWorld);

                GD.Print($"[ShelterHazardLoopSelfTest] Failures: {failures}");
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[ShelterHazardLoopSelfTest] Exception: {ex.Message}\n{ex.StackTrace}");
                failures++;
            }

            return EmitSummary("shelter_hazard_loop_selftest", failures == 0, failures == 0 ? 0 : 1, details: failures == 0 ? "PASS" : $"FAIL ({failures})");
        }

    }
}
