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

        public static int RunDay1PlayableSelfTest(string dataDirectory)
        {
            int failures = 0;
            void Check(bool cond, string name)
            {
                if (cond)
                    GD.Print($"  [PASS] {name}");
                else
                {
                    GD.PrintErr($"  [FAIL] {name}");
                    failures++;
                }
            }

            GD.Print("[Day1PlayableSelfTest] Starting Phase 0 - Phase 2 Day 1 Playable Verification...");

            try
            {
                // 1. Initial State & Clean Reset
                TryDeleteTempFile(InventorySaveStore.SavePath);
                var startingSession = new StartingLevelHostSession();
                var startingState = startingSession.System.State;
                Check(startingState != null, "starting level state initialized");
                Check(startingState!.day == 1, "starts on Day 1");
                Check(startingState.rooms.Count == 5, "starting bunker has 5 functional rooms");

                // Check rooms
                var bunks = startingState.rooms.Find(r => r.roomId == "room_bunks_living");
                var airlock = startingState.rooms.Find(r => r.roomId == "room_filtration_stack");
                var corridor = startingState.rooms.Find(r => r.roomId == "room_bunker_corridor");
                Check(bunks != null && bunks.material == "Wood", "bunks ceiling starts as wood");
                Check(airlock != null && airlock.attenuation >= 0.90f, "airlock has active lead filtration shielding");
                Check(corridor != null && corridor.isInspected, "central access corridor inspected");

                // 2. Survivor Roster
                var survivorsSession = new SurvivorsHostSession();
                survivorsSession.SeedDemoRoster();
                var roster = survivorsSession.RosterState;
                Check(roster.Count >= 3, "starting survivor roster has at least 3 survivors");
                var drChen = roster.Find(s => s.Id == "survivor_dr_sarah_chen");
                var mikhail = roster.Find(s => s.Id == "survivor_gunner_mikhail");
                var elena = roster.Find(s => s.Id == "elena_vasquez" || s.Id == "survivor_elena_vasquez");
                Check(drChen != null && drChen.Health > 80f, "Dr. Sarah Chen present with good health");
                Check(mikhail != null && mikhail.Health > 70f, "Gunner Mikhail present with combat traits");
                Check(elena != null && elena.Health > 80f, "Elena Vasquez present with machinist expertise");

                // 3. Inventory & Supplies
                var invSession = InventoryHostSession.Create(dataDirectory, seedWhenNoSave: true);
                var inv = invSession.Inventory;
                Check(inv.CountById("clean_water") >= 12, "holdfast stocked with >=12 clean water");
                Check(inv.CountById("canned_food") >= 16, "holdfast stocked with >=16 canned food");
                Check(inv.CountById("iodine_pills") >= 4, "holdfast stocked with >=4 iodine pills");
                Check(inv.CountById("scrap_mechanical") >= 6, "holdfast stocked with >=6 mechanical scrap");
                Check(inv.CountById("item_geiger_m3") >= 1, "holdfast equipped with geiger counter");
                Check(inv.CountById("item_dosimeter_pen") >= 1, "holdfast equipped with dosimeter pen");

                // 4. Opening Protocol Directives:
                // Directive 1: Morning Triage (Standard Rations)
                startingSession.ResolveMorningRationTriage(Ashfall.Core.StartingLevel.RationPolicy.Standard);
                Check(startingState.rationPolicy == Ashfall.Core.StartingLevel.RationPolicy.Standard, "morning triage chosen: Standard rations");
                Check(startingState.morningTriageResolved, "morning triage marked resolved");

                // Directive 2: Midday Maintenance (Fortify Bunks with Lead)
                int scrapBefore = inv.CountById("scrap_mechanical");
                invSession.Remove("scrap_mechanical", 2);
                startingSession.ResolveMiddayMaintenance(Ashfall.Core.StartingLevel.MaintenanceDirective.FortifyBunksLead);
                Check(startingState.maintenanceDirective == Ashfall.Core.StartingLevel.MaintenanceDirective.FortifyBunksLead, "midday maintenance chosen: Fortify Bunks Lead");
                Check(inv.CountById("scrap_mechanical") == scrapBefore - 2, "2 mechanical scrap consumed for bunker lead shielding");
                Check(bunks!.material == "Lead", "bunks ceiling upgraded to Lead shielding");
                Check(bunks.attenuation >= 0.98f, "bunks ceiling provides 99% radiation attenuation");

                // Directive 3: Evening Radio (Acknowledge Hydro Barons)
                var radioSession = RadioHostSession.Create(dataDirectory);
                startingSession.ResolveEveningRadio(Ashfall.Core.StartingLevel.RadioProtocol.AcknowledgeHydroBarons);
                Check(startingState.radioProtocol == Ashfall.Core.StartingLevel.RadioProtocol.AcknowledgeHydroBarons, "evening radio protocol chosen: Acknowledge Hydro Barons");
                Check(startingState.eveningRadioResolved, "evening radio protocol marked resolved");

                // 5. Core-Backed Action: Medical Treatment
                int iodineBefore = inv.CountById("iodine_pills");
                invSession.Remove("iodine_pills", 1);
                survivorsSession.AdministerIodine("survivor_gunner_mikhail");
                Check(inv.CountById("iodine_pills") == iodineBefore - 1, "1 iodine pill administered from medical stores");

                // 6. Core-Backed Action: Sub-Surface Greenhouse Cultivation
                var greenhouseSession = new GreenhouseHostSession(new GreenhouseSystem(1986), invSession);
                greenhouseSession.System.EnsurePlots(4);
                invSession.Add(GreenhouseExpansionCatalog.Items.SeedMushroom, 2);
                bool planted = greenhouseSession.Plant(0, GreenhouseExpansionCatalog.Items.SeedMushroom, 1);
                Check(planted, "mushroom spores planted in Greenhouse Plot 0");
                Check(greenhouseSession.System.Plots[0].stage == (int)GreenhouseStage.Sprouting, "plot 0 transitioned to Sprouting");
                bool watered = greenhouseSession.Water(0, 20f, tainted: false);
                Check(watered, "plot 0 irrigated with 20L clean water");
                Check(greenhouseSession.System.Plots[0].water >= 20f, "soil moisture recorded in bed");

                // 7. Time Advance: Day 1 -> Day 2 Transition & Need Decay
                int foodBefore = inv.CountById("canned_food");
                int waterBefore = inv.CountById("clean_water");

                // Daily consumption (3 survivors × 1 ration = 3 food, 3 water)
                invSession.Remove("canned_food", 3);
                invSession.Remove("clean_water", 3);
                survivorsSession.TickHour(24f); // 24h needs + radiation decay
                greenhouseSession.TickDay(2, 6f, 0.04f);
                startingSession.TickDay();

                Check(startingState.day == 2, "time advanced to Day 2");
                Check(inv.CountById("canned_food") == foodBefore - 3, "canned food decremented by 3 for daily rations");
                Check(inv.CountById("clean_water") == waterBefore - 3, "clean water decremented by 3 for daily hydration");
                Check(greenhouseSession.System.Plots[0].growth > 0f, "greenhouse crop growth advanced on Day 2 tick");

                // 8. Save State Persistence
                bool startingSaved = StartingLevelSaveStore.TrySave(startingSession.CaptureState());
                bool invSaved = InventorySaveStore.TrySave(invSession.CaptureSave());
                bool survivorsSaved = SurvivorsSaveStore.TrySave(survivorsSession.CaptureSave());
                bool greenhouseSaved = GreenhouseSaveStore.TrySave(greenhouseSession.CaptureSave());
                Check(startingSaved && invSaved && survivorsSaved && greenhouseSaved, "all systems persisted cleanly to disk");

                // 9. Clean Reload Verification
                var reloadedStarting = StartingLevelSaveStore.TryLoad();
                var reloadedInv = InventorySaveStore.TryLoad();
                var reloadedGreenhouse = GreenhouseSaveStore.TryLoad();

                Check(reloadedStarting != null && reloadedStarting.day == 2, "reloaded save retains Day 2");
                Check(reloadedStarting!.rooms.Find(r => r.roomId == "room_bunks_living")?.material == "Lead", "reloaded save retains Lead bunk fortification");
                Check(reloadedInv != null, "reloaded inventory save is valid");
                Check(reloadedGreenhouse != null && (reloadedGreenhouse.plots[0].stage == (int)GreenhouseStage.Growing || reloadedGreenhouse.plots[0].stage == (int)GreenhouseStage.Sprouting), "reloaded greenhouse save retains plot crop state");

                GD.Print($"[Day1PlayableSelfTest] Failures: {failures}");
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[Day1PlayableSelfTest] Exception thrown: {ex.Message}\n{ex.StackTrace}");
                failures++;
            }

            return EmitSummary("day1_playable_selftest", failures == 0, failures == 0 ? 0 : 1, details: failures == 0 ? "PASS" : $"FAIL ({failures})");
        }

    }
}
