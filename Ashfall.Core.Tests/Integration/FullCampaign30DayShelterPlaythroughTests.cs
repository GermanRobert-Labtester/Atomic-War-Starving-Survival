// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Excavation;
using Ashfall.Core.Inventory;
using Ashfall.Core.Memorial;
using Ashfall.Core.Quests;
using Ashfall.Core.Radio;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;
using Xunit;
using Xunit.Abstractions;

namespace Ashfall.Core.Tests.Integration
{
    public class FullCampaign30DayShelterPlaythroughTests
    {
        private readonly ITestOutputHelper _out;

        public FullCampaign30DayShelterPlaythroughTests(ITestOutputHelper output)
        {
            _out = output;
        }

        private static string FindRepoRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                if (File.Exists(Path.Combine(dir.FullName, "project.godot")) ||
                    Directory.Exists(Path.Combine(dir.FullName, "Assets", "StreamingAssets", "Data")))
                    return dir.FullName;
                dir = dir.Parent;
            }
            return AppContext.BaseDirectory;
        }

        private static string LoadDataFile(string relativePath)
        {
            string root = FindRepoRoot();
            string path = Path.Combine(root, relativePath);
            return File.Exists(path) ? File.ReadAllText(path) : string.Empty;
        }

        [Fact]
        public void PreflightAuthoredIds_ExistInAuthoritativeCatalogs()
        {
            // 1. social_event_bunk_noise_friction in shelter_social_events.json
            string socialJson = LoadDataFile("Assets/StreamingAssets/Data/shelter_social_events.json");
            Assert.Contains("social_event_bunk_noise_friction", socialJson);

            // 2. MERIDIAN-ACT-7 in radio_intercepts.json with loc_diesel_tank_farm
            string radioJson = LoadDataFile("Assets/StreamingAssets/Data/radio_intercepts.json");
            Assert.Contains("MERIDIAN-ACT-7", radioJson);
            Assert.Contains("radio_intercept_meridian_supply_column_01", radioJson);
            Assert.Contains("loc_diesel_tank_farm", radioJson);

            // 3. loc_diesel_tank_farm in locations.json
            string locJson = LoadDataFile("Assets/StreamingAssets/Data/locations.json");
            Assert.Contains("loc_diesel_tank_farm", locJson);

            // 4. room_common_mess_hall in shelter_rooms.json
            string roomsJson = LoadDataFile("Assets/StreamingAssets/Data/shelter_rooms.json");
            Assert.Contains("room_common_mess_hall", roomsJson);
        }

        private sealed class CampaignContext
        {
            public Inventory.Inventory Inventory { get; set; } = null!;
            public ShelterWorkshopSystem Workshop { get; set; } = null!;
            public ShelterRadioStationSystem RadioStation { get; set; } = null!;
            public ShelterSocialDynamicsSystem SocialDynamics { get; set; } = null!;
            public ExcavationHazardSystem ExcavationHazards { get; set; } = null!;
            public DynamicQuestlineSystem DynamicQuests { get; set; } = null!;
            public WastelandMapSystem WastelandMap { get; set; } = null!;
            public MemorialSystem Memorial { get; set; } = null!;
            public SurvivorFateSystem SurvivorFate { get; set; } = null!;

            public readonly List<string> Survivors = new List<string>
            {
                "dweller_alice", "dweller_bob", "dweller_charlie",
                "dweller_dan", "dweller_elena", "dweller_frank"
            };

            public int CurrentDay { get; set; } = 1;

            public void WireCrossSystemEvents()
            {
                RadioStation.OnLocationTriangulated += (interceptId, locId) =>
                {
                    WastelandMap.Discover(locId);
                    DynamicQuests.TriggerInvestigateRadioDepotQuest(interceptId, locId, CurrentDay);
                };

                ExcavationHazards.OnRescueStarted += (secId, count) =>
                {
                    var trapped = ExcavationHazards.State.sectors.TryGetValue(secId, out var s)
                        ? s.ActiveTrappedMiners
                        : new List<string>();
                    DynamicQuests.TriggerRescueMinersQuest($"rescue_{secId}_{CurrentDay}", secId, trapped, CurrentDay);
                };

                ExcavationHazards.OnRescueSucceeded += (secId) =>
                {
                    var q = DynamicQuests.GetActiveQuest(DynamicQuestlineSystem.RescueMinersQuestId);
                    if (q != null && q.TargetLocationId == secId)
                    {
                        DynamicQuests.CompleteQuest(q.QuestId);
                    }
                };

                ExcavationHazards.OnRescueFailed += (secId) =>
                {
                    var q = DynamicQuests.GetActiveQuest(DynamicQuestlineSystem.RescueMinersQuestId);
                    if (q != null && q.TargetLocationId == secId)
                    {
                        DynamicQuests.FailQuest(q.QuestId);
                    }
                    if (ExcavationHazards.State.sectors.TryGetValue(secId, out var s))
                    {
                        foreach (var m in s.ActiveTrappedMiners)
                        {
                            SurvivorFate.ReportDeath(m, SurvivorDeathCause.Scripted, $"Died in cave-in ({secId})", "excavation_hazard");
                        }
                    }
                };

                Workshop.OnJobCompleted += (job) =>
                {
                    DynamicQuests.AdvanceQuestProgress(DynamicQuestlineSystem.ArmoryMunitionsRefurbishQuestId, 1);
                };
            }
        }

        private static CampaignContext CreateFreshCampaign(int seed)
        {
            var ctx = new CampaignContext();
            string dataDir = Path.Combine(FindRepoRoot(), "Assets", "StreamingAssets", "Data");

            ctx.Inventory = new Inventory.Inventory { Capacity = 500, MaxWeight = 5000f };
            ctx.Inventory.Add(new ItemDefinition { id = "spent_casing", stackMax = 999 }, 40);
            ctx.Inventory.Add(new ItemDefinition { id = "scrap_metal", stackMax = 999 }, 60);
            ctx.Inventory.Add(new ItemDefinition { id = "scrap_chemical", stackMax = 999 }, 30);
            ctx.Inventory.Add(new ItemDefinition { id = "ammo_9x19", stackMax = 999 }, 30);
            ctx.Inventory.Add(new ItemDefinition { id = "mechanical_parts", stackMax = 999 }, 15);
            ctx.Inventory.Add(new ItemDefinition { id = "machine_oil", stackMax = 999 }, 12);
            ctx.Inventory.Add(new ItemDefinition { id = "mitigation_shoring_reinforce_kit", stackMax = 999 }, 8);

            ctx.Workshop = new ShelterWorkshopSystem(ctx.Inventory, new SeededRng(seed + 101));
            string workshopJson = LoadDataFile("Assets/StreamingAssets/Data/workshop_recipes.json");
            if (!string.IsNullOrEmpty(workshopJson)) ctx.Workshop.LoadCatalog(workshopJson);

            ctx.RadioStation = new ShelterRadioStationSystem(new SeededRng(seed + 202));
            string radioJson = LoadDataFile("Assets/StreamingAssets/Data/radio_intercepts.json");
            if (!string.IsNullOrEmpty(radioJson)) ctx.RadioStation.LoadCatalog(radioJson);

            ctx.SocialDynamics = new ShelterSocialDynamicsSystem(new SeededRng(seed + 303));
            string socialJson = LoadDataFile("Assets/StreamingAssets/Data/shelter_social_events.json");
            if (!string.IsNullOrEmpty(socialJson)) ctx.SocialDynamics.LoadCatalog(socialJson);

            ctx.ExcavationHazards = new ExcavationHazardSystem(ctx.Inventory, new SeededRng(seed + 404));
            string excavationJson = LoadDataFile("Assets/StreamingAssets/Data/excavation_hazard_mitigation.json");
            if (!string.IsNullOrEmpty(excavationJson)) ctx.ExcavationHazards.LoadCatalog(excavationJson);

            ctx.DynamicQuests = new DynamicQuestlineSystem();
            ctx.WastelandMap = WastelandMapCatalogLoader.CreateSystem(dataDir);
            ctx.Memorial = new MemorialSystem(new MemorialState());
            ctx.SurvivorFate = new SurvivorFateSystem(memorial: ctx.Memorial);

            // Setup rooms and machines
            ctx.Workshop.GetOrCreateMachineState("room_armory_munitions");
            ctx.Workshop.GetOrCreateMachineState("room_workshop_precision");
            ctx.Workshop.GetOrCreateMachineState("room_workshop_heavy");

            // Setup survivors: 2 in private quarters, 4 in crowded bunks
            ctx.SocialDynamics.RegisterSurvivorRoom(ctx.Survivors[0], "room_quarters_private");
            ctx.SocialDynamics.RegisterSurvivorRoom(ctx.Survivors[1], "room_quarters_private");
            ctx.SocialDynamics.RegisterSurvivorRoom(ctx.Survivors[2], "room_bunks_crowded");
            ctx.SocialDynamics.RegisterSurvivorRoom(ctx.Survivors[3], "room_bunks_crowded");
            ctx.SocialDynamics.RegisterSurvivorRoom(ctx.Survivors[4], "room_bunks_crowded");
            ctx.SocialDynamics.RegisterSurvivorRoom(ctx.Survivors[5], "room_bunks_crowded");

            // Setup sector with blower
            var sector = ctx.ExcavationHazards.GetOrCreateSector("sector_excavation_alpha");
            sector.InstalledMitigationIds.Add("mitigation_ventilation_blower_install");

            ctx.WireCrossSystemEvents();
            return ctx;
        }

        [Fact]
        public void Full30DayCampaign_WithMidRunSaveReloadShock_CompletesWithoutInvariantsDrift()
        {
            var ctx = CreateFreshCampaign(seed: 42);
            var rng = new SeededRng(42);

            for (int day = 1; day <= 30; day++)
            {
                ctx.CurrentDay = day;
                var dayEvents = new List<DayStateChangeEvent>();

                // ── Day 11: Save / Reload Shock ──────────────────────────────
                if (day == 11)
                {
                    _out.WriteLine("Executing Day 11 Mid-Run Save/Reload Shock...");

                    // Capture all systems
                    var workshopSave = ctx.Workshop.CaptureState();
                    var radioSave = ctx.RadioStation.CaptureState();
                    var socialSave = ctx.SocialDynamics.CaptureState();
                    var excavationSave = ctx.ExcavationHazards.CaptureState();
                    var dynamicQuestSave = ctx.DynamicQuests.CaptureState();
                    var mapSave = ctx.WastelandMap.CaptureState();
                    var memorialSave = ctx.Memorial.CaptureState();
                    var fateSave = ctx.SurvivorFate.CaptureState();
                    var invSave = ctx.Inventory.CaptureState();

                    // Serialize to JSON and deserialize back
                    var s = new SystemTextJsonSerializer();
                    var workshopJson = s.Serialize(workshopSave);
                    var radioJson = s.Serialize(radioSave);
                    var socialJson = s.Serialize(socialSave);
                    var excavationJson = s.Serialize(excavationSave);
                    var questJson = s.Serialize(dynamicQuestSave);
                    var mapJson = s.Serialize(mapSave);
                    var memorialJson = s.Serialize(memorialSave);
                    var fateJson = s.Serialize(fateSave);
                    var invJson = s.Serialize(invSave);

                    // Reconstruct fresh systems
                    string dataDir = Path.Combine(FindRepoRoot(), "Assets", "StreamingAssets", "Data");
                    var newInv = new Inventory.Inventory();
                    newInv.RestoreState(s.Deserialize<InventorySaveState>(invJson)!, id => new ItemDefinition { id = id, displayName = id, stackMax = 999 });

                    var newWorkshop = new ShelterWorkshopSystem(newInv, new SeededRng(42 + 101));
                    string wCatalog = LoadDataFile("Assets/StreamingAssets/Data/workshop_recipes.json");
                    if (!string.IsNullOrEmpty(wCatalog)) newWorkshop.LoadCatalog(wCatalog);
                    newWorkshop.RestoreState(s.Deserialize<ShelterWorkshopSave>(workshopJson));

                    var newRadio = new ShelterRadioStationSystem(new SeededRng(42 + 202));
                    string rCatalog = LoadDataFile("Assets/StreamingAssets/Data/radio_intercepts.json");
                    if (!string.IsNullOrEmpty(rCatalog)) newRadio.LoadCatalog(rCatalog);
                    newRadio.RestoreState(s.Deserialize<RadioStationStateSave>(radioJson));

                    var newSocial = new ShelterSocialDynamicsSystem(new SeededRng(42 + 303));
                    string sCatalog = LoadDataFile("Assets/StreamingAssets/Data/shelter_social_events.json");
                    if (!string.IsNullOrEmpty(sCatalog)) newSocial.LoadCatalog(sCatalog);
                    newSocial.RestoreState(s.Deserialize<ShelterSocialSave>(socialJson));

                    var newExcavation = new ExcavationHazardSystem(newInv, new SeededRng(42 + 404));
                    string eCatalog = LoadDataFile("Assets/StreamingAssets/Data/excavation_hazard_mitigation.json");
                    if (!string.IsNullOrEmpty(eCatalog)) newExcavation.LoadCatalog(eCatalog);
                    newExcavation.RestoreState(s.Deserialize<ExcavationHazardSave>(excavationJson));

                    var newDynamicQuests = new DynamicQuestlineSystem();
                    newDynamicQuests.RestoreState(s.Deserialize<DynamicQuestSave>(questJson));

                    var (nodes, routes) = WastelandMapCatalogLoader.Load(dataDir);
                    var newMap = new WastelandMapSystem(s.Deserialize<WastelandMapState>(mapJson)!, nodes, routes);

                    var newMemorial = new MemorialSystem(s.Deserialize<MemorialState>(memorialJson)!);
                    var newFate = new SurvivorFateSystem(memorial: newMemorial);
                    newFate.RestoreState(s.Deserialize<SurvivorFateSaveState>(fateJson));

                    // Replace context references
                    ctx.Inventory = newInv;
                    ctx.Workshop = newWorkshop;
                    ctx.RadioStation = newRadio;
                    ctx.SocialDynamics = newSocial;
                    ctx.ExcavationHazards = newExcavation;
                    ctx.DynamicQuests = newDynamicQuests;
                    ctx.WastelandMap = newMap;
                    ctx.Memorial = newMemorial;
                    ctx.SurvivorFate = newFate;
                    ctx.WireCrossSystemEvents();

                    // Assert state preserved
                    Assert.Equal(workshopSave.machines.Count, ctx.Workshop.State.machines.Count);
                    Assert.Equal(radioSave.intercepts.Count, ctx.RadioStation.State.intercepts.Count);
                    Assert.Equal(socialSave.privacyProfiles.Count, ctx.SocialDynamics.State.privacyProfiles.Count);
                    Assert.Equal(excavationSave.sectors.Count, ctx.ExcavationHazards.State.sectors.Count);
                }

                // ── Daily Activity ──────────────────────────────────────────
                // Inventory inflow
                ctx.Inventory.Add(new ItemDefinition { id = "spent_casing", stackMax = 999 }, 2);
                ctx.Inventory.Add(new ItemDefinition { id = "scrap_metal", stackMax = 999 }, 3);
                ctx.Inventory.Add(new ItemDefinition { id = "scrap_chemical", stackMax = 999 }, 2);

                // Workshop ammo reloading
                if (ctx.Inventory.CountById("ammo_9x19") < 40 &&
                    ctx.Inventory.CountById("spent_casing") >= 1 &&
                    ctx.Inventory.CountById("scrap_metal") >= 2 &&
                    ctx.Inventory.CountById("scrap_chemical") >= 1)
                {
                    var jobRes = ctx.Workshop.TryStartJob("recipe_workshop_reload_9x19", "room_armory_munitions", null, null, out string jId);
                    if (jobRes.IsSuccess)
                    {
                        ctx.Workshop.AdvanceLaborTicks(60, day);
                        dayEvents.Add(new DayStateChangeEvent("workshop_job_completed", "shelter_workshop", "Reload 9x19mm FMJ", "room_armory_munitions", 20f));
                    }
                }
                ctx.Workshop.TickDay(day);

                // Radio scanning
                if (day == 16)
                {
                    // Tune specifically to MERIDIAN-ACT-7 frequency (7115 kHz)
                    ctx.RadioStation.TuneTo(7115);
                    var scan = ctx.RadioStation.ScanFrequency(day);
                    Assert.True(scan.FoundSignal, "Must detect MERIDIAN-ACT-7 at 7115 kHz");

                    // Decrypt intercept
                    ctx.RadioStation.ProgressDecryption("radio_intercept_meridian_supply_column_01", skillBonus: 5.0f);
                    var intercept = ctx.RadioStation.State.intercepts.Find(i => i.InterceptId == "radio_intercept_meridian_supply_column_01");
                    Assert.NotNull(intercept);
                    Assert.True(intercept.IsDecrypted, "MERIDIAN-ACT-7 must be decrypted on Day 16");
                    dayEvents.Add(new DayStateChangeEvent("radio_intercept_decrypted", "radio_station", "MERIDIAN-ACT-7", null, 0f));
                }
                else if (day == 17)
                {
                    // Bearing 1 and 2
                    ctx.RadioStation.RecordBearing("radio_intercept_meridian_supply_column_01", 45);
                    ctx.RadioStation.RecordBearing("radio_intercept_meridian_supply_column_01", 90);
                }
                else if (day == 18)
                {
                    // Bearing 3 resolves triangulation!
                    bool resolved = ctx.RadioStation.RecordBearing("radio_intercept_meridian_supply_column_01", 180);
                    Assert.True(resolved, "Third distinct bearing must resolve triangulation");

                    // Assert WastelandMap discovery and dynamic quest trigger
                    Assert.True(ctx.WastelandMap.IsDiscovered("loc_diesel_tank_farm"), "loc_diesel_tank_farm must be discovered on map");
                    var depotQuest = ctx.DynamicQuests.GetActiveQuest(DynamicQuestlineSystem.InvestigateRadioDepotQuestId);
                    Assert.NotNull(depotQuest);
                    Assert.Equal("loc_diesel_tank_farm", depotQuest.TargetLocationId);

                    // Advance stages
                    ctx.DynamicQuests.AdvanceQuestStage(depotQuest.QuestId);
                    ctx.DynamicQuests.AdvanceQuestStage(depotQuest.QuestId);
                    ctx.DynamicQuests.AdvanceQuestStage(depotQuest.QuestId); // Completed
                    Assert.Contains(depotQuest.QuestId, ctx.DynamicQuests.CompletedIds);
                }
                ctx.RadioStation.TickDay(day);

                // Social dynamics
                ctx.SocialDynamics.EvaluateRoomDynamics("room_quarters_private", new[] { ctx.Survivors[0], ctx.Survivors[1] }, day);
                var inc = ctx.SocialDynamics.EvaluateRoomDynamics("room_bunks_crowded", new[] { ctx.Survivors[2], ctx.Survivors[3], ctx.Survivors[4], ctx.Survivors[5] }, day);
                if (inc != null && !inc.Resolved)
                {
                    ctx.SocialDynamics.TryMediateIncident(inc.IncidentId, ctx.Survivors[0]);
                }
                ctx.SocialDynamics.TickDay(day);

                // Excavation hazards
                if (day == 19)
                {
                    // Trigger cave-in rescue emergency
                    ctx.ExcavationHazards.TriggerCaveInRescue("sector_excavation_alpha", new[] { "miner_jake", "miner_tomas" }, deadlineDays: 3, requiredLabor: 200);

                    var rescueQuest = ctx.DynamicQuests.GetActiveQuest(DynamicQuestlineSystem.RescueMinersQuestId);
                    Assert.NotNull(rescueQuest);
                    Assert.Equal("sector_excavation_alpha", rescueQuest.TargetLocationId);
                    Assert.Equal(2, rescueQuest.TargetSurvivorIds.Count);
                    dayEvents.Add(new DayStateChangeEvent("subterranean_rescue_active", "excavation_hazards", "sector_excavation_alpha", null, 200f));
                }
                else if (day == 20)
                {
                    ctx.ExcavationHazards.ProgressRescueLabor("sector_excavation_alpha", 100);
                    ctx.DynamicQuests.AdvanceQuestProgress(DynamicQuestlineSystem.RescueMinersQuestId, 100);
                }
                else if (day == 21)
                {
                    ctx.ExcavationHazards.ProgressRescueLabor("sector_excavation_alpha", 100); // Completes rescue!
                    var sector = ctx.ExcavationHazards.GetOrCreateSector("sector_excavation_alpha");
                    Assert.True(sector.RescueCompleted, "Rescue must complete when remaining labor reaches zero");
                    Assert.Empty(sector.ActiveTrappedMiners);
                    Assert.Contains(DynamicQuestlineSystem.RescueMinersQuestId, ctx.DynamicQuests.CompletedIds);
                    dayEvents.Add(new DayStateChangeEvent("subterranean_rescue_completed", "excavation_hazards", "sector_excavation_alpha", null, 0f));
                }
                ctx.ExcavationHazards.TickDay(day);
                ctx.DynamicQuests.TickDay(day);

                // Build Daily Briefing Report
                var report = DailyBriefingReportBuilder.BuildFromDayEvents(day, day * 7, dayEvents);
                Assert.NotNull(report);
                Assert.Equal($"DAY {day} BRIEFING", report.Title);
            }

            _out.WriteLine("30-Day Campaign Playthrough completed successfully!");
            Assert.True(ctx.Inventory.CountById("ammo_9x19") > 0);
            Assert.True(ctx.DynamicQuests.CompletedIds.Count >= 2);
            Assert.True(ctx.WastelandMap.IsDiscovered("loc_diesel_tank_farm"));
        }

        [Fact]
        public void FatalCaveInRescue_ReportsCasualtiesToSurvivorFate_AndMemorial()
        {
            var ctx = CreateFreshCampaign(seed: 88);

            // Start cave in with 2 miners
            ctx.ExcavationHazards.TriggerCaveInRescue("sector_excavation_alpha", new[] { "miner_adam", "miner_ben" }, deadlineDays: 2, requiredLabor: 200);

            // Tick past deadline without applying labor
            ctx.ExcavationHazards.TickDay(1);
            ctx.DynamicQuests.TickDay(1);

            ctx.ExcavationHazards.TickDay(2);
            ctx.DynamicQuests.TickDay(2);

            ctx.ExcavationHazards.TickDay(3); // Deadline expired!
            ctx.DynamicQuests.TickDay(3);

            var sector = ctx.ExcavationHazards.GetOrCreateSector("sector_excavation_alpha");
            Assert.True(sector.RescueFailed, "Rescue should fail on expired deadline");

            // Assert dynamic quest failed
            Assert.Contains(DynamicQuestlineSystem.RescueMinersQuestId, ctx.DynamicQuests.FailedIds);

            // Assert casualties reported to SurvivorFate and Memorial
            Assert.Equal(2, ctx.SurvivorFate.DeathCount);
            Assert.Equal(2, ctx.Memorial.Entries.Count);
            Assert.Contains(ctx.Memorial.Entries, e => e.SurvivorId == "miner_adam");
            Assert.Contains(ctx.Memorial.Entries, e => e.SurvivorId == "miner_ben");
        }

        [Fact]
        public void DualRun_SameSeedDeterminism_ProducesIdenticalCampaignOutcome()
        {
            void RunSimulation(int seed, out int finalAmmo, out float latheHealth, out int decrypted, out int disputes, out int methanePpm)
            {
                var ctx = CreateFreshCampaign(seed);
                for (int day = 1; day <= 30; day++)
                {
                    ctx.Inventory.Add(new ItemDefinition { id = "spent_casing", stackMax = 999 }, 1);
                    ctx.Inventory.Add(new ItemDefinition { id = "scrap_metal", stackMax = 999 }, 2);
                    ctx.Inventory.Add(new ItemDefinition { id = "scrap_chemical", stackMax = 999 }, 1);

                    if (ctx.Inventory.CountById("ammo_9x19") < 35 &&
                        ctx.Inventory.CountById("spent_casing") >= 1 &&
                        ctx.Inventory.CountById("scrap_metal") >= 2 &&
                        ctx.Inventory.CountById("scrap_chemical") >= 1)
                    {
                        var res = ctx.Workshop.TryStartJob("recipe_workshop_reload_9x19", "room_armory_munitions", null, null, out string jId);
                        if (res.IsSuccess) ctx.Workshop.AdvanceLaborTicks(60, day);
                    }
                    ctx.Workshop.TickDay(day);

                    int freq = 14200 + (day % 3) * 50;
                    ctx.RadioStation.TuneTo(freq);
                    ctx.RadioStation.ScanFrequency(day);
                    foreach (var i in ctx.RadioStation.State.intercepts)
                    {
                        if (!i.IsDecrypted && !i.IsExpired) ctx.RadioStation.ProgressDecryption(i.InterceptId, 1.0f);
                    }
                    ctx.RadioStation.TickDay(day);

                    ctx.SocialDynamics.EvaluateRoomDynamics("room_quarters_private", new[] { ctx.Survivors[0], ctx.Survivors[1] }, day);
                    var inc = ctx.SocialDynamics.EvaluateRoomDynamics("room_bunks_crowded", new[] { ctx.Survivors[2], ctx.Survivors[3], ctx.Survivors[4], ctx.Survivors[5] }, day);
                    if (inc != null && !inc.Resolved) ctx.SocialDynamics.TryMediateIncident(inc.IncidentId, ctx.Survivors[0]);
                    ctx.SocialDynamics.TickDay(day);

                    ctx.ExcavationHazards.TickDay(day);
                    ctx.DynamicQuests.TickDay(day);
                }

                finalAmmo = ctx.Inventory.CountById("ammo_9x19");
                latheHealth = ctx.Workshop.GetOrCreateMachineState("room_workshop_precision").ToolingHealth;
                decrypted = ctx.RadioStation.State.intercepts.Count(i => i.IsDecrypted);
                disputes = ctx.SocialDynamics.State.recentIncidents.Count;
                methanePpm = ctx.ExcavationHazards.GetOrCreateSector("sector_excavation_alpha").MethanePpm;
            }

            RunSimulation(999, out int ammoA, out float latheA, out int decA, out int dispA, out int methA);
            RunSimulation(999, out int ammoB, out float latheB, out int decB, out int dispB, out int methB);

            Assert.Equal(ammoA, ammoB);
            Assert.Equal(latheA, latheB);
            Assert.Equal(decA, decB);
            Assert.Equal(dispA, dispB);
            Assert.Equal(methA, methB);
        }
    }
}
