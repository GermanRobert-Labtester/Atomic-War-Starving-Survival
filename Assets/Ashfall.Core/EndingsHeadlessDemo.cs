using System.Text;

namespace Ashfall.Core
{
    /// <summary>Headless-report extension for the endings slice.</summary>
    public sealed class EndingsHeadlessReport : HeadlessReport
    {
        public HoldfastSave Save;
    }

    /// <summary>
    /// Vertical-slice smoke for Holdfast Sprint 4 ("Shelf &amp; endings"): no ending
    /// is armed by default, all five master-list ids arm, arming a second ending
    /// overwrites the first (mutual exclusivity), unknown ids are refused by the
    /// master list, and the armed ending survives a v3 envelope roundtrip.
    /// No data directory needed — SetEnding does not read the catalog.
    /// Invoked by `dotnet test`; wired into the Godot host as `-- --endings-selftest`.
    /// </summary>
    public static class EndingsHeadlessDemo
    {
        public static EndingsHeadlessReport Run(ILog log = null)
        {
            CatalogLocator.UseInvariantCulture();
            log = log ?? NullLog.Instance;
            var report = new EndingsHeadlessReport();

            void Check(bool condition, string name)
            {
                report.Checks.Add(new HeadlessCheck { Name = name, Passed = condition });
                if (condition)
                {
                    report.PassedCount++;
                    log.Info("[PASS] " + name);
                }
                else
                {
                    report.FailedCount++;
                    log.Error("[FAIL] " + name);
                }
            }

            log.Info("[EndingsHeadlessDemo] begin");

            Check(HoldfastEndings.All.Length == 5, "master list holds five endings");
            bool unique = true;
            for (int i = 0; i < HoldfastEndings.All.Length && unique; i++)
                for (int j = i + 1; j < HoldfastEndings.All.Length && unique; j++)
                    if (HoldfastEndings.All[i] == HoldfastEndings.All[j]) unique = false;
            Check(unique, "master list ids are unique");
            Check(!HoldfastEndings.IsKnown("ending_holdfast_invented"),
                "unknown ending refused by master list");
            Check(HoldfastEndings.DisplayName(HoldfastEndings.None) == HoldfastEndings.None,
                "empty ending displays as itself");

            var quests = new HoldfastQuestSystem();
            Check(string.IsNullOrEmpty(quests.State.endingId), "no ending armed by default");

            // Every master-list ending arms.
            bool allArm = true;
            for (int i = 0; i < HoldfastEndings.All.Length; i++)
            {
                quests.SetEnding(HoldfastEndings.All[i]);
                if (quests.State.endingId != HoldfastEndings.All[i]) allArm = false;
            }
            Check(allArm, "all five endings arm via SetEnding");

            // Mutual exclusivity: arming a second ending replaces the first.
            quests.SetEnding(HoldfastEndings.Schedule);
            quests.SetEnding(HoldfastEndings.White);
            Check(quests.State.endingId == HoldfastEndings.White, "second ending overwrites the first");

            // Roundtrip through the v3 envelope.
            var ice = new IceRoadSystem(808);
            var census = new CensusClaimSystem();
            var brine = new BrineWaterSystem();
            quests.SetEnding(HoldfastEndings.DarkRoad);
            var clock = new SimClock(210);
            var save = HoldfastSaveCodec.Capture(ice, census, brine, quests, clock);
            Check(save.saveVersion == HoldfastSave.CurrentSaveVersion, "envelope at current version");
            Check(save.quests.endingId == HoldfastEndings.DarkRoad, "ending captured in envelope");

            string text = HoldfastSaveCodec.Encode(save, new SystemTextJsonSerializer());
            var loaded = HoldfastSaveCodec.Decode(text, new SystemTextJsonSerializer());
            var fresh = new HoldfastQuestSystem();
            HoldfastSaveCodec.Restore(
                loaded, new IceRoadSystem(808), new CensusClaimSystem(),
                new BrineWaterSystem(), fresh, new SimClock(1));
            Check(fresh.State.endingId == HoldfastEndings.DarkRoad, "ending survives roundtrip");
            Check(clock.Day == 210, "sim day survives roundtrip");

            report.Save = save;
            report.Passed = report.FailedCount == 0;
            var sb = new StringBuilder();
            sb.Append("EndingsHeadlessDemo ");
            sb.Append(report.Passed ? "PASS" : "FAIL");
            sb.Append(" ").Append(report.PassedCount).Append("/").Append(report.PassedCount + report.FailedCount);
            report.Summary = sb.ToString();
            log.Info(report.Summary);
            return report;
        }
    }
}
