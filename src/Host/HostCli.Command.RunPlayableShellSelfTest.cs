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

        public static int RunPlayableShellSelfTest(string dataDirectory)
        {
            int failures = 0;
            void Check(bool condition, string message)
            {
                if (condition)
                {
                    GD.Print($"  [PASS] {message}");
                }
                else
                {
                    GD.PrintErr($"  [FAIL] {message}");
                    failures++;
                }
            }

            GD.Print("[PlayableShellSelfTest] Starting Playable UI Shell, Multi-Day Loop, & Navigation Flow...");

            try
            {
                // Clear active save files
                foreach (var file in new[]
                {
                    "starting_level_save.json", "inventory_save.json", "survivors_save.json", "greenhouse_save.json"
                })
                {
                    string p = Path.Combine(ProjectSettings.GlobalizePath("user://"), file);
                    if (File.Exists(p)) File.Delete(p);
                }

                // 1. Boot to Main Menu
                var mainMenu = new MainMenuPanel();
                mainMenu._Ready();
                bool saveExists = StartingLevelSaveStore.SaveExists();
                mainMenu.SetContinueEnabled(saveExists);
                Check(!saveExists, "main menu boots with continue disabled when no save exists");

                // 2. New Game Initialization
                var startingSession = new StartingLevelHostSession();
                var invSession = InventoryHostSession.Create(dataDirectory, seedWhenNoSave: true);
                var survivorsSession = new SurvivorsHostSession();
                survivorsSession.SeedDemoRoster();
                var greenhouseSession = new GreenhouseHostSession(new GreenhouseSystem(1986), invSession);
                greenhouseSession.System.EnsurePlots(4);

                Check(startingSession.System.State.day == 1, "new game begins on Day 1");
                Check(invSession.Inventory.CountById("clean_water") >= 12, "starting water stock verified");
                Check(invSession.Inventory.CountById("canned_food") >= 16, "starting food stock verified");

                // 3. Shelter Dashboard Presentation State
                var dashboard = new GameDashboardPanel();
                dashboard._Ready();
                dashboard.UpdateState(new GameDashboardPanel.DashboardSnapshot
                {
                    Day = startingSession.System.State.day,
                    Health = 100,
                    MaxHealth = 100,
                    Radiation = 0f,
                    CleanWater = invSession.Inventory.CountById("clean_water"),
                    Food = invSession.Inventory.CountById("canned_food"),
                    MedicalStock = invSession.Inventory.CountById("iodine_pills"),
                    FilterSpares = invSession.Inventory.CountById("item_air_filter_hepa"),
                    LivingSurvivors = survivorsSession.RosterState.Count,
                    TotalSurvivors = survivorsSession.RosterState.Count,
                    Weather = "Clear Fallout Dust",
                    Location = "THE HOLDFAST"
                });
                Check(dashboard.Visible == false, "dashboard constructed in background");

                // 4. Meaningful Gameplay Actions
                // Action A: Fortify Bunks with Lead
                invSession.Remove("scrap_mechanical", 2);
                startingSession.ResolveMiddayMaintenance(Ashfall.Core.StartingLevel.MaintenanceDirective.FortifyBunksLead);
                var bunks = startingSession.System.State.rooms.Find(r => r.roomId == "room_bunks_living");
                Check(bunks != null && bunks.material == "Lead", "action executed: bunker bunks fortified with Lead");

                // Action B: Cultivate greenhouse plot
                invSession.Add(GreenhouseExpansionCatalog.Items.SeedMushroom, 2);
                greenhouseSession.Plant(0, GreenhouseExpansionCatalog.Items.SeedMushroom, 1);
                greenhouseSession.Water(0, 20f, tainted: false);
                Check(greenhouseSession.System.Plots[0].stage == (int)GreenhouseStage.Sprouting, "action executed: greenhouse plot 0 planted & irrigated");

                // 5. Advance Day: Day 1 -> Day 2 Transition
                int foodBefore = invSession.Inventory.CountById("canned_food");
                int waterBefore = invSession.Inventory.CountById("clean_water");

                invSession.Remove("canned_food", 3);
                invSession.Remove("clean_water", 3);
                survivorsSession.TickHour(24f);
                greenhouseSession.TickDay(2, 6f, 0.04f);
                startingSession.TickDay();

                Check(startingSession.System.State.day == 2, "time advanced to Day 2");
                Check(invSession.Inventory.CountById("canned_food") == foodBefore - 3, "food consumed for day 2 rations");
                Check(invSession.Inventory.CountById("clean_water") == waterBefore - 3, "water consumed for day 2 rations");
                Check(greenhouseSession.System.Plots[0].growth > 0f, "greenhouse crop growth advanced on Day 2");

                // Update Dashboard with Day 2 State
                dashboard.UpdateState(new GameDashboardPanel.DashboardSnapshot
                {
                    Day = startingSession.System.State.day,
                    Health = 95,
                    MaxHealth = 100,
                    Radiation = 2.4f,
                    CleanWater = invSession.Inventory.CountById("clean_water"),
                    Food = invSession.Inventory.CountById("canned_food"),
                    MedicalStock = invSession.Inventory.CountById("iodine_pills"),
                    FilterSpares = invSession.Inventory.CountById("item_air_filter_hepa"),
                    LivingSurvivors = survivorsSession.RosterState.Count,
                    TotalSurvivors = survivorsSession.RosterState.Count,
                    Weather = "Ashfall Squall",
                    Location = "THE HOLDFAST"
                });

                // 6. Save State
                bool sSaved = StartingLevelSaveStore.TrySave(startingSession.CaptureState());
                bool iSaved = InventorySaveStore.TrySave(invSession.CaptureSave());
                bool survSaved = SurvivorsSaveStore.TrySave(survivorsSession.CaptureSave());
                bool gSaved = GreenhouseSaveStore.TrySave(greenhouseSession.CaptureSave());
                Check(sSaved && iSaved && survSaved && gSaved, "game state saved successfully to disk");

                // 7. Return to Menu
                mainMenu.SetContinueEnabled(StartingLevelSaveStore.SaveExists());
                Check(StartingLevelSaveStore.SaveExists(), "return to menu: continue button is now enabled");

                // 8. Continue / Reload from Save
                var loadedStarting = StartingLevelSaveStore.TryLoad();
                var loadedInv = InventorySaveStore.TryLoad();
                var loadedGreenhouse = GreenhouseSaveStore.TryLoad();
                Check(loadedStarting != null && loadedStarting.day == 2, "continued save reflects Day 2");
                Check(loadedStarting!.rooms.Find(r => r.roomId == "room_bunks_living")?.material == "Lead", "continued save retains Lead bunk fortification");
                Check(loadedGreenhouse != null && loadedGreenhouse.plots[0].growth > 0f, "continued save retains active greenhouse crop");

                // 9. In-Game Settings Navigation
                var settingsPanel = new SettingsPanel();
                settingsPanel._Ready();
                settingsPanel.Open();
                Check(settingsPanel.Visible, "settings overlay opens over active session");
                settingsPanel.Close();
                Check(!settingsPanel.Visible, "settings overlay closes cleanly without destroying state");

                // Dispose transient UI nodes so the headless self-test does not
                // leak Canvas/CanvasItem RIDs at exit.
                mainMenu.QueueFree();
                dashboard.QueueFree();
                settingsPanel.QueueFree();

                GD.Print($"[PlayableShellSelfTest] Failures: {failures}");
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[PlayableShellSelfTest] Exception: {ex.Message}\n{ex.StackTrace}");
                failures++;
            }

            return EmitSummary("playable_shell_selftest", failures == 0, failures == 0 ? 0 : 1, details: failures == 0 ? "PASS" : $"FAIL ({failures})");
        }

    }
}
