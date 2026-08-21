using System.Text;

namespace Ashfall.Core
{
    /// <summary>Headless-report extension for the Cluster / Order 12-C slice.</summary>
    public sealed class Cluster12CHeadlessReport : HeadlessReport
    {
        public HoldfastSave Save;
    }

    /// <summary>
    /// Vertical-slice smoke for Holdfast Sprint 3 ("Cluster &amp; claim"): Order 12-C
    /// is dormant until the office acts, the levy-refusal path activates it, the
    /// Second List quest gates on the refuse branch, and the v3 save envelope
    /// carries the quest snapshot through a JSON roundtrip.
    /// Invoked by `dotnet test`; wired into the Godot host as `-- --cluster-selftest`.
    /// </summary>
    public static class Cluster12CHeadlessDemo
    {
        public static Cluster12CHeadlessReport Run(string dataDirectory, ILog log = null!)
        {
            CatalogLocator.UseInvariantCulture();
            log = log ?? NullLog.Instance;
            var report = new Cluster12CHeadlessReport();

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

            log.Info("[Cluster12CHeadlessDemo] begin");

            var session = HoldfastSession.Load(dataDirectory, 808, expansionUnlocked: true, log);
            Check(!session.Census.Order12CActive, "12-C dormant until the office acts");
            Check(!session.Quests.IsStarted(HoldfastQuestSystem.SecondList), "second list not started");

            // S3 story gate: the Office has shown the drawer (drawerRead), so the
            // levy quest can start and then be refused — the organic 12-C path.
            session.Quests.State.drawerRead = true;
            int events = 0;
            session.Census.On12CActivated += () => events++;
            int day = 100;
            Check(session.Quests.TryStart(HoldfastQuestSystem.Levy, day), "levy quest starts after drawer");
            session.RefuseLevy(day);

            Check(session.Census.Order12CActive, "refuse levy activates Order 12-C");
            Check(session.Census.State.edorWaitingAtHatch, "Edor waits at the hatch after refusal");
            Check(session.Census.LevyRefuse, "refuse branch flag set");
            Check(session.Quests.HasRefuseBranch(), "levy progress carries the refuse branch");
            Check(events == 1, "refusal raised On12CActivated once");

            // The Second List quest gates on the refuse branch.
            session.TickDaily(day + 1, WeatherKind.Clear, -12f, hasMapItem: false,
                hasFormulaLore: false, hasLettersLore: false);
            Check(session.Quests.IsStarted(HoldfastQuestSystem.SecondList),
                "second list starts on the refuse branch");

            // Activate12C is idempotent: further calls do not re-fire the event.
            session.Census.Activate12C();
            session.Census.Activate12C();
            Check(events == 1, "On12CActivated stays at one (idempotent)");

            // Envelope v3: the quest snapshot + 12-C survive a JSON roundtrip.
            var json = new SystemTextJsonSerializer();
            var clock = new SimClock(day + 1);
            var save = HoldfastSaveCodec.Capture(
                session.IceRoad, session.Census, session.Brine, session.Quests, clock);
            Check(save.saveVersion == HoldfastSave.CurrentSaveVersion, "envelope at current version");
            Check(save.quests != null && save.quests.quests.Count > 0, "quest snapshot in envelope");
            Check(save.census.order12cActive, "12-C captured in envelope");
            string text = HoldfastSaveCodec.Encode(save, json);
            var loaded = HoldfastSaveCodec.Decode(text, json);

            var fresh = HoldfastSession.Load(dataDirectory, 808, expansionUnlocked: true, log);
            var freshClock = new SimClock(1);
            HoldfastSaveCodec.Restore(
                loaded, fresh.IceRoad, fresh.Census, fresh.Brine, fresh.Quests, freshClock);
            Check(fresh.Census.Order12CActive, "12-C survives roundtrip");
            Check(fresh.Census.State.edorWaitingAtHatch, "edor wait survives roundtrip");
            Check(fresh.Census.LevyRefuse, "refuse branch survives roundtrip");
            Check(fresh.Quests.IsStarted(HoldfastQuestSystem.SecondList),
                "second list progress survives roundtrip");
            Check(fresh.Quests.HasRefuseBranch(), "refuse branch survives roundtrip");
            Check(freshClock.Day == day + 1, "sim day survives roundtrip");

            report.Save = save;
            report.Passed = report.FailedCount == 0;
            var sb = new StringBuilder();
            sb.Append("Cluster12CHeadlessDemo ");
            sb.Append(report.Passed ? "PASS" : "FAIL");
            sb.Append(" ").Append(report.PassedCount).Append("/").Append(report.PassedCount + report.FailedCount);
            report.Summary = sb.ToString();
            log.Info(report.Summary);
            return report;
        }
    }
}
