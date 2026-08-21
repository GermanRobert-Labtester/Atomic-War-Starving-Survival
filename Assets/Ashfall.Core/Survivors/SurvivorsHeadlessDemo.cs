namespace Ashfall.Core.Survivors
{
    /// <summary>
    /// Headless verification of the survivor roster core port.
    /// Invoked by `dotnet test` and by Godot `-- --survivors-selftest`.
    /// </summary>
    public static class SurvivorsHeadlessDemo
    {
        public static HeadlessReport Run(ILog log = null!)
        {
            CatalogLocator.UseInvariantCulture();
            log = log ?? NullLog.Instance;
            var report = new HeadlessReport();

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

            log.Info("[SurvivorsHeadlessDemo] begin");

            var sys = new SurvivorRosterSystem();
            sys.RegisterDefinition(new SurvivorDefinition { id = "sv_mae", displayName = "Mae", profession = "Farmer", baseHealth = 90f });
            sys.RegisterDefinition(new SurvivorDefinition { id = "sv_iora", displayName = "Iora", profession = "Doctor", baseHealth = 100f });
            Check(sys.Catalog.Count == 2, "catalog registers two definitions");

            int joined = 0, died = 0;
            sys.OnSurvivorJoined += e => joined++;
            sys.OnSurvivorDied += (e, r) => died++;
            Check(sys.Join("sv_mae", 40), "Mae joins the bunker on Day 40");
            Check(!sys.Join("sv_mae", 41), "duplicate join refused");
            Check(!sys.Join("sv_missing", 41), "unknown definition refused");
            Check(sys.Join("sv_iora", 41), "Iora joins on Day 41");
            Check(joined == 2, "join events fired for both");
            Check(sys.LivingCount == 2, "two living survivors");

            Check(sys.Die("sv_mae", "Died of thirst."), "Mae dies");
            Check(died == 1, "death event fired");
            Check(!sys.Die("sv_mae", "again"), "double death refused");
            Check(sys.LivingCount == 1, "one living survivor remains");

            string before = SaveChecksum.Compute(sys.CaptureState());
            var restored = new SurvivorRosterSystem();
            restored.RegisterDefinition(new SurvivorDefinition { id = "sv_mae" });
            restored.RegisterDefinition(new SurvivorDefinition { id = "sv_iora" });
            restored.RestoreState(sys.CaptureState());
            string after = SaveChecksum.Compute(restored.CaptureState());
            Check(before == after, "save/load checksum stable");
            Check(!restored.Find("sv_mae")!.isAlive && restored.Find("sv_iora")!.isAlive,
                "death state survives the round trip");

            var snapshot = sys.CaptureState();
            snapshot.entries[0].deathReason = "injected";
            Check(sys.Find("sv_mae")!.deathReason == "Died of thirst.",
                "capture returns snapshot, not live state");

            report.Passed = report.FailedCount == 0;
            report.Summary =
                $"[SurvivorsHeadlessDemo] {(report.Passed ? "PASS" : "FAIL")} " +
                $"{report.PassedCount}/{report.PassedCount + report.FailedCount}";
            log.Info(report.Summary);
            return report;
        }
    }
}
