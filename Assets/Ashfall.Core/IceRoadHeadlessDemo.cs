using System.Collections.Generic;
using System.Text;

namespace Ashfall.Core
{
    public sealed class HeadlessCheck
    {
        public string Name;
        public bool Passed;
    }

    public class HeadlessReport
    {
        public bool Passed;
        public int PassedCount;
        public int FailedCount;
        public List<HeadlessCheck> Checks = new List<HeadlessCheck>();
        public string Summary;
        public IceRoadSystemState IceRoad;
        public int LocationCount;
        public int QuestCount;

        public int ExitCode => Passed ? 0 : 1;
    }

    /// <summary>
    /// Vertical-slice smoke: ice-road window open/close + Holdfast JSON load.
    /// Invoked by `dotnet test` and by Godot `-- --ice-road-selftest`.
    /// </summary>
    public static class IceRoadHeadlessDemo
    {
        public const int DefaultSeed = 808;

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

            log.Info("[IceRoadHeadlessDemo] begin");

            var dark = new IceRoadSystem(DefaultSeed);
            dark.NotifyClerkStarted();
            dark.TickDaily(1, WeatherKind.Blizzard, -20f);
            dark.TickDaily(2, WeatherKind.Blizzard, -20f);
            dark.TickDaily(3, WeatherKind.IceStorm, -25f);
            Check(!dark.IsUnlocked, "dark until unlock");
            Check(!dark.IsOpen, "closed while locked");
            Check(dark.IsTravelBlocked(IceRoadSystem.LocIceRoadGate), "gate blocked while locked");

            var ice = new IceRoadSystem(DefaultSeed);
            ice.Unlock(90);
            for (int d = 90; d < 120; d++)
                ice.TickDaily(d, WeatherKind.Blizzard, -22f);
            Check(!ice.IsOpen, "first window waits on the clerk");

            ice.NotifyClerkStarted();
            ice.TickDaily(120, WeatherKind.Blizzard, -22f);
            Check(ice.IceThicknessM >= IceRoadSystem.OpenThicknessM, "thickness at open threshold");
            Check(ice.IsOpen, "window opens after clerk + freeze");
            Check(!ice.IsTravelBlocked(IceRoadSystem.LocIceRoadGate), "gate open during window");

            int remaining = ice.WindowDaysRemaining;
            Check(remaining >= IceRoadSystem.MinWindowDays, "window length at least 11 days");
            int openedOn = ice.State.lastOpenDay;
            for (int i = 0; i < remaining; i++)
                ice.TickDaily(openedOn + 1 + i, WeatherKind.Clear, -12f);
            Check(!ice.IsOpen, "window closes after length");
            Check(ice.IsTravelBlocked(IceRoadSystem.LocKilometre19), "cut blocked after close");

            var storm = new IceRoadSystem(909);
            storm.Unlock(1);
            storm.NotifyClerkStarted();
            for (int d = 1; d <= 60; d++)
                storm.TickDaily(d, WeatherKind.FalloutStorm, -30f);
            Check(!storm.IsOpen, "fallout storm does not open the road");

            var beacon = new IceRoadSystem(DefaultSeed);
            beacon.Unlock(1);
            beacon.NotifyClerkStarted();
            for (int d = 1; d <= 80 && !beacon.IsOpen; d++)
                beacon.TickDaily(d, WeatherKind.Blizzard, -22f);
            Check(beacon.IsOpen, "beacon fixture opened");
            beacon.SetBeaconLit(IceRoadSystem.LocSouthBeacon, false);
            Check(!beacon.IsOpen, "dark south beacon closes the road");

            var json = new SystemTextJsonSerializer();
            var roundtrip = new IceRoadSystem(1);
            ice.LogAccident();
            string blob = json.Serialize(ice.CaptureState());
            roundtrip.RestoreState(json.Deserialize<IceRoadSystemState>(blob)!);
            Check(roundtrip.IsUnlocked, "save roundtrip unlocked");
            Check(roundtrip.State.clerkStarted, "save roundtrip clerk");
            Check(roundtrip.State.accidentCount == ice.State.accidentCount, "save roundtrip accidents");

            Check(!ice.IsCutNode(IceRoadSystem.LocShallowsMarket), "shallows is not a cut node");

            if (!string.IsNullOrEmpty(dataDirectory))
            {
                var loader = new HoldfastCatalogLoader(new FileSystemIO(), json, log);
                var catalog = loader.Load(dataDirectory);
                report.LocationCount = catalog.Locations.Count;
                report.QuestCount = catalog.Quests.Count;
                Check(catalog.Locations.Count >= 11, "holdfast_locations.json loaded (>=11)");
                Check(catalog.GetLocation(IceRoadSystem.LocIceRoadGate) != null, "loc_ice_road_gate present");
                Check(catalog.Quests.Count == 10, "holdfast_quests.json loaded (10)");
                Check(catalog.GetQuest("quest_holdfast_the_sheet") != null, "quest_holdfast_the_sheet present");
            }

            report.IceRoad = ice.CaptureState();
            report.Passed = report.FailedCount == 0;
            var sb = new StringBuilder();
            sb.Append("IceRoadHeadlessDemo ");
            sb.Append(report.Passed ? "PASS" : "FAIL");
            sb.Append(" ").Append(report.PassedCount).Append("/").Append(report.PassedCount + report.FailedCount);
            if (report.LocationCount > 0)
                sb.Append(" locations=").Append(report.LocationCount).Append(" quests=").Append(report.QuestCount);
            report.Summary = sb.ToString();
            log.Info(report.Summary);
            return report;
        }
    }
}
