using System.Text;

namespace Ashfall.Core
{
    /// <summary>
    /// Holdfast wiring smoke: story gate, quest Advance on arrival, 12-C,
    /// lamps-out delay, brine unlock gate, catalog lock, briefing text.
    /// </summary>
    public static class HoldfastHeadlessDemo
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

            log.Info("[HoldfastHeadlessDemo] begin");

            var session = HoldfastSession.Load(dataDirectory, 808, expansionUnlocked: true, log);
            report.LocationCount = session.Catalog.Locations.Count;
            report.QuestCount = session.Catalog.Quests.Count;

            Check(session.Catalog.Locations.Count >= 26, "unlocked catalog includes District 8 cards");
            Check(session.Catalog.Quests.Count == 10, "ten Holdfast quests");
            var plant = session.Catalog.GetLocation(IceRoadSystem.LocAbandonedDesalination);
            Check(plant != null, "desalination recast present");
            Check(plant == null || plant.displayName.IndexOf("(existing", System.StringComparison.Ordinal) < 0,
                "displayName has no (existing) note");
            Check(!string.IsNullOrEmpty(session.BriefingText(HoldfastQuestSystem.Sheet)), "sheet briefing text exposed");

            var locked = new HoldfastCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer(), log)
                .Load(dataDirectory, expansionUnlocked: false);
            Check(locked.Locations.Count == 3, "locked catalog is recast_always only (3)");
            Check(locked.GetLocation("loc_ice_road_gate") == null, "District 8 gate hidden while locked");

            bool sheetAt90 = false;
            for (int d = 90; d <= 95; d++)
                session.Quests.TickDaily(d, false, false, false);
            sheetAt90 = session.Quests.IsStarted(HoldfastQuestSystem.Sheet);
            Check(!sheetAt90, "S1: day 90 without story keys does not start the sheet");

            session.Quests.TickDaily(90, hasMapItem: true, hasFormulaLore: false, hasLettersLore: false);
            Check(session.Quests.IsStarted(HoldfastQuestSystem.Sheet), "S1: map sheet starts the quest");

            var sheetDef = session.Quests.GetDef(HoldfastQuestSystem.Sheet);
            Check(sheetDef != null && !string.IsNullOrEmpty(sheetDef.target_location_id), "sheet has a target location");
            bool advanced = session.NotifyArrival(sheetDef.target_location_id);
            Check(advanced, "B1: arrival advances the sheet");
            Check(!string.IsNullOrEmpty(session.StageText(HoldfastQuestSystem.Sheet)), "stage text after advance");

            session.Census.IssueLevy(new[] { "sv_a", "sv_b", "sv_c" }, 40);
            Check(session.Census.HonourLevy(), "honour levy");
            Check(session.Census.IsAssignedAway("sv_a"), "B3: levy uses IsAssignedAway");

            var refuseSession = HoldfastSession.Load(dataDirectory, 808, true, log);
            refuseSession.Quests.State.drawerRead = true;
            refuseSession.Quests.TryStart(HoldfastQuestSystem.Levy, 200);
            refuseSession.Census.IssueLevy(new[] { "x", "y", "z" }, 50);
            refuseSession.IceRoad.Unlock(1);
            refuseSession.IceRoad.NotifyClerkStarted();
            for (int d = 1; d <= 80 && !refuseSession.IceRoad.IsOpen; d++)
                refuseSession.IceRoad.TickDaily(d, WeatherKind.Blizzard, -22f);
            bool wasOpen = refuseSession.IceRoad.IsOpen;
            refuseSession.RefuseLevy(51);
            Check(refuseSession.Census.Order12CActive, "B2: levy refuse activates 12-C");
            Check(wasOpen && refuseSession.IceRoad.IsOpen, "B5: refuse does not immediately close the road");
            Check(refuseSession.IceRoad.CuttersAccess, "B5: cutters still on during 11-day delay");

            refuseSession.ResolveMembrane(stripSector4: false, day: 130);
            Check(refuseSession.Census.Order12CActive, "B2: membrane resolution keeps 12-C");
            Check(refuseSession.Quests.IsStarted(HoldfastQuestSystem.Membrane), "membrane quest started from resolution");

            var brine = new BrineWaterSystem();
            for (int d = 1; d <= 25; d++)
                brine.TickDaily(d, WeatherKind.Clear, -18f, false);
            Check(!brine.SteamTripped && brine.MembraneIntegrity > 60f, "B6: brine does not auto-trip while locked");
            brine.Unlock();
            float before = brine.MembraneIntegrity;
            brine.TickDaily(26, WeatherKind.Clear, -18f, false);
            Check(brine.MembraneIntegrity < before, "B6: brine ticks after unlock");

            var ice = new IceRoadSystem(808);
            ice.Unlock(1);
            Check(!ice.IsTravelBlocked(IceRoadSystem.LocAbandonedDesalination), "B4: desalination not ice-gated while locked-closed");
            Check(!ice.IsTravelBlocked(IceRoadSystem.LocFrozenRiverBarge), "B4: barge not ice-gated");
            Check(!ice.IsTravelBlocked(IceRoadSystem.LocCrashedIcebreakerConvoy), "B4: convoy not ice-gated");
            Check(ice.IsTravelBlocked(IceRoadSystem.LocIceRoadGate), "cut gate still blocked until window");

            report.Passed = report.FailedCount == 0;
            var sb = new StringBuilder();
            sb.Append("HoldfastHeadlessDemo ");
            sb.Append(report.Passed ? "PASS" : "FAIL");
            sb.Append(" ").Append(report.PassedCount).Append("/").Append(report.PassedCount + report.FailedCount);
            sb.Append(" locations=").Append(report.LocationCount).Append(" quests=").Append(report.QuestCount);
            report.Summary = sb.ToString();
            log.Info(report.Summary);
            return report;
        }
    }
}
