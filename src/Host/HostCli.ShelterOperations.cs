// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Events;
using Ashfall.Core.Inventory;
using Ashfall.Core.Settlements;
using Ashfall.Core.Shelter;
using Ashfall.Core.UI;
using Godot;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        public static int RunShelterOperationsBoardSelfTest(string dataDirectory)
        {
            int pass = 0, fail = 0;
            void Check(string name, bool ok)
            {
                if (ok) { pass++; GD.Print($"[PASS] shelter-operations/{name}"); }
                else { fail++; GD.Print($"[FAIL] shelter-operations/{name}"); }
            }

            try
            {
                string constructionJson = File.ReadAllText(Path.Combine(dataDirectory, "shelter_construction.json"));
                string outpostJson = File.ReadAllText(Path.Combine(dataDirectory, "outposts.json"));
                string celebrationJson = File.ReadAllText(Path.Combine(dataDirectory, "shelter_celebrations.json"));

                var construction = new ShelterExpansionSystem();
                construction.LoadCatalog(constructionJson);
                var inventory = new Ashfall.Core.Inventory.Inventory { MaxWeight = 5000f };
                inventory.TryProduce("scrap_metal", 10);
                inventory.TryProduce("scrap_wood", 10);
                var started = construction.TryStartRoomConstruction(
                    "bp_deep_bunkhouse", 1, 0, 1, 1, inventory);
                Check("authored_blueprint_and_skill_rules_load",
                    construction.Blueprints.Count == 6
                    && construction.CrewRules.SkillId == "skill_crafting");
                Check("build_bills_atomically", started.Succeeded
                    && inventory.CountById("scrap_metal") == 0
                    && inventory.CountById("scrap_wood") == 0);

                int practices = 0, fatigue = 0;
                construction.RecordCrewSkillPractice = (_, skillId, _, _) =>
                {
                    if (skillId == "skill_crafting") practices++;
                };
                construction.ApplyCrewFatigue = (_, _) => fatigue++;
                construction.ResolveCrewSkillBonus = (_, _) => 0.1f;
                Check("crew_can_be_assigned", construction.TryAssignCrew(started.Project!.ProjectId, "survivor_builder"));
                for (int day = 1; day <= 6; day++) construction.ProgressAssignedProjects(day);
                Check("crew_work_practice_and_fatigue", practices == 6 && fatigue == 6
                    && construction.GetProject(started.Project.ProjectId)!.Status == ProjectStatus.Completed);
                Check("completed_bunks_project_capacity",
                    construction.GetCompletedCapacityBonuses().TryGetValue("room_bunks", out int bonus) && bonus == 4);

                inventory.TryProduce("scrap_metal", 50);
                inventory.TryProduce("scrap_wood", 30);
                inventory.TryProduce("dried_rations", 22);
                var outposts = OutpostSettlementSystem.FromJson(outpostJson);
                Check("outpost_build_bill_commits",
                    outposts.TryEstablishOutpost("outpost_north_watch", inventory)
                    && inventory.CountById("scrap_metal") == 0
                    && inventory.CountById("scrap_wood") == 0);
                Check("outpost_supply_bill_commits",
                    outposts.TrySupplyOutpost("outpost_north_watch", "dried_rations", 2, inventory)
                    && outposts.GetInstance("outpost_north_watch")!.RationReserve == 2);
                Check("outpost_abandon_returns_position",
                    outposts.AbandonOutpost("outpost_north_watch")
                    && !outposts.GetInstance("outpost_north_watch")!.IsEstablished);

                var celebrations = new SeasonalCelebrationSystem();
                celebrations.LoadCatalog(celebrationJson);
                inventory.TryProduce("dried_rations", 4);
                inventory.TryProduce("fuel", 2);
                bool holidayHeld = celebrations.TryHoldCelebration(
                    "hol_new_year", "small", 4, 1, "dried_rations", "fuel",
                    inventory, new SeededRng(701), out _);
                Check("holiday_consumes_once_per_cycle", holidayHeld
                    && !celebrations.TryHoldCelebration(
                        "hol_new_year", "small", 4, 1, "dried_rations", "fuel",
                        inventory, new SeededRng(701), out _));
                var restoredCelebrations = new SeasonalCelebrationSystem();
                restoredCelebrations.LoadCatalog(celebrationJson);
                restoredCelebrations.RestoreState(celebrations.CaptureState());
                Check("holiday_occurrence_survives_restore",
                    restoredCelebrations.WasHolidayOccurrenceHeld("hol_new_year", 1));

                Check("holiday_skip_is_cycle_scoped",
                    celebrations.TrySkipHoliday("hol_midsummer_day", 180, out _)
                    && !celebrations.TrySkipHoliday("hol_midsummer_day", 180, out _)
                    && !celebrations.WasHolidayOccurrenceSkipped("hol_midsummer_day", 540));

                PanelRegistryBootstrap.RegisterAll();
                var route = PanelRegistry.Get("shelter_operations");
                Check("operations_route_registered", route != null
                    && route.Group == PanelGroup.Expanded
                    && route.IsPlayerNavigable);
            }
            catch (Exception ex)
            {
                fail++;
                GD.PrintErr($"[FAIL] shelter-operations/exception — {ex.Message}");
            }

            return EmitSummary("shelter_operations_board_selftest", fail == 0, fail,
                details: $"{pass} passed, {fail} failed");
        }

    }
}
