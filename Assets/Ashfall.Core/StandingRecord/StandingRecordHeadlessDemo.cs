using System.Collections.Generic;
using System.Text;

namespace Ashfall.Core
{
    /// <summary>
    /// Headless verification smoke for Expansion 03: The Standing Record.
    /// Tests: location layouts catalog loading, room hierarchy, room lighting / unlocking,
    /// room inspection triggers, and layout state serialization.
    /// </summary>
    public static class StandingRecordHeadlessDemo
    {
        public static HeadlessReport Run(string dataDirectory = null, ILog log = null)
        {
            CatalogLocator.UseInvariantCulture();
            log = log ?? NullLog.Instance;
            var report = new HeadlessReport();

            void Check(bool condition, string name)
            {
                report.Checks.Add(new HeadlessCheck { Name = name, Passed = condition });
                if (condition) report.PassedCount++;
                else
                {
                    report.FailedCount++;
                    log.Error("[FAIL] " + name);
                }
                if (condition) log.Info("[PASS] " + name);
            }

            log.Info("[StandingRecordHeadlessDemo] begin");

            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var layoutSys = new LocationLayoutSystem(files, json, log);
            layoutSys.Load(dataDirectory);

            report.LocationCount = layoutSys.LayoutCount;

            Check(layoutSys.LayoutCount >= 14, "all 14 standing record layouts loaded");
            var km19 = layoutSys.GetLayout(LocationLayoutSystem.LocKilometre19);
            Check(km19 != null, "loc_cut_kilometre_19 layout present");
            Check(km19 != null && km19.RoomCount == 4, "km19 layout has 4 rooms");

            var transit = layoutSys.GetLayout(LocationLayoutSystem.LocTransitHq);
            Check(transit != null, "loc_transit_authority_hq layout present");
            Check(transit != null && transit.RoomCount == 5, "transit HQ layout has 5 rooms");

            var ministry = layoutSys.GetLayout("location_ministry_of_truth_bunker");
            Check(ministry != null && ministry.RoomCount == 6, "ministry bunker layout has 6 rooms");

            var lock4 = layoutSys.GetLayout("loc_lock_gate_four");
            Check(lock4 != null && lock4.RoomCount == 6, "lock gate four layout has 6 rooms");

            var vault = layoutSys.GetLayout("location_the_memory_vault");
            Check(vault != null && vault.RoomCount == 6, "memory vault layout has 6 rooms");

            // Navigation & Room inspection smoke
            layoutSys.Unlock();
            Check(layoutSys.ArriveAtParent(LocationLayoutSystem.LocKilometre19), "arrive at parent loc_cut_kilometre_19");
            Check(layoutSys.CanEnter(LocationLayoutSystem.RoomKm19Post), "can enter entry room room_km19_post");
            Check(!layoutSys.CanEnter(LocationLayoutSystem.RoomKm19Seam), "cannot enter room_km19_seam before post inspection");

            bool entered = layoutSys.EnterRoom(LocationLayoutSystem.RoomKm19Post);
            Check(entered, "entered room_km19_post");

            bool inspected = layoutSys.InspectRoom(LocationLayoutSystem.RoomKm19Post);
            Check(inspected, "inspected room_km19_post");
            Check(layoutSys.CanEnter(LocationLayoutSystem.RoomKm19Seam), "adjacent room_km19_seam unlocked after post inspection");
            Check(layoutSys.CanEnter(LocationLayoutSystem.RoomKm19OilTin), "adjacent room_km19_oil_tin unlocked after post inspection");

            // Save / Load roundtrip
            string blob = json.Serialize(layoutSys.CaptureState());
            var restored = new LocationLayoutSystem(files, json, log);
            restored.Load(dataDirectory);
            restored.RestoreState(json.Deserialize<LocationLayoutState>(blob)!);

            Check(restored.ArriveAtParent(LocationLayoutSystem.LocKilometre19), "arrive at parent after restore");
            Check(restored.CanEnter(LocationLayoutSystem.RoomKm19Seam), "save roundtrip preserved unlocked room state");

            // LocationMemorySystem — strata recasts
            var memory = new LocationMemorySystem(files, json, log);
            memory.Load(dataDirectory);
            memory.Unlock();
            Check(memory.StratumCount >= 30, "standing record memory strata loaded (>=30)");
            Check(memory.GetStratumText("loc_cut_kilometre_19", "pre") != null, "km19 pre stratum present");
            memory.ApplyMutation(LocationMemorySystem.MutationKm19Plated);
            Check(memory.GetActiveRecast("loc_cut_kilometre_19") != null, "km19 now recast after plated mutation");
            memory.ApplyMutation(LocationMemorySystem.MutationKm19Scraped);
            Check(memory.GetActiveRecast("loc_cut_kilometre_19")!.Contains("short one post"),
                "scraped recast wins over plated");

            // SiteEncounterSystem — room-keyed, Overlay withdraws after 3 scrapes
            var site = new SiteEncounterSystem(1808);
            site.Unlock();
            Check(site.StartEncounter("enc_site_plate_screwer", "room_km19_post",
                SiteEncounterSystem.KindPlateScrewer, 75, "mutation_km19_plated"),
                "site encounter starts");
            Check(site.ResolveEncounter("enc_site_plate_screwer", 75), "site encounter resolves");
            for (int s = 0; s < SiteEncounterSystem.OverlayWithdrawPlateCount; s++)
                site.ScrapePlate(76 + s);
            Check(!site.OverlayAccess, "three scrapes withdraw Overlay access");
            Check(site.PlatesScraped == SiteEncounterSystem.OverlayWithdrawPlateCount,
                "plate count recorded");

            // StandingRecordCatalog — all ten mains with world-change bars
            var catLoader = new StandingRecordCatalogLoader(files, json, log);
            var recordCat = catLoader.Load(dataDirectory);
            Check(recordCat.Quests.Count >= 10, "standing record quests loaded (>=10)");
            Check(recordCat.GetQuest("quest_record_the_book") != null, "quest_record_the_book present");
            Check(recordCat.GetQuest("quest_record_which_gazetteer") != null,
                "quest_record_which_gazetteer present (spine end)");
            bool allBar = true;
            for (int i = 0; i < recordCat.Quests.Count; i++)
            {
                if (string.IsNullOrEmpty(recordCat.Quests[i].complete_mutation)
                    || recordCat.Quests[i].StageCount < 3)
                {
                    allBar = false;
                    break;
                }
            }
            Check(allBar, "every record main quest names a mutation and has 3+ objectives");

            report.Passed = report.FailedCount == 0;
            var sb = new StringBuilder();
            sb.Append("StandingRecordHeadlessDemo ");
            sb.Append(report.Passed ? "PASS" : "FAIL");
            sb.Append(" ").Append(report.PassedCount).Append("/").Append(report.PassedCount + report.FailedCount);
            sb.Append(" layouts=").Append(report.LocationCount)
                .Append(" quests=").Append(recordCat.Quests.Count)
                .Append(" strata=").Append(memory.StratumCount);
            report.Summary = sb.ToString();
            log.Info(report.Summary);
            return report;
        }
    }
}
