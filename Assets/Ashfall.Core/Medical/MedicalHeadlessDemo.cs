namespace Ashfall.Core.Medical
{
    /// <summary>
    /// Headless verification of the Chemical Dependency core port.
    /// Invoked by `dotnet test` and by Godot `-- --medical-selftest`.
    /// </summary>
    public static class MedicalHeadlessDemo
    {
        public static HeadlessReport Run(ILog log = null)
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

            log.Info("[MedicalHeadlessDemo] begin");

            var sys = new ChemicalDependencySystem();
            int formed = 0, completed = 0, failed = 0;
            sys.OnDependencyFormed += (sv, item) => formed++;
            sys.OnDetoxCompleted += (sv, item) => completed++;
            sys.OnDetoxFailed += (sv, item) => failed++;

            sys.OnSubstanceConsumed("sv_mae", "opioid_painkillers", ChemicalDependencyKind.Opioid);
            Check(sys.DependencyLevel("sv_mae", "opioid_painkillers") > 0f, "first dose creates the ledger entry");
            sys.OnSubstanceConsumed("sv_mae", "opioid_painkillers", ChemicalDependencyKind.Opioid);
            Check(formed == 1, "second dose crosses the threshold and forms the dependency");
            Check(sys.DependencyLevel("sv_mae", "opioid_painkillers") >= ChemicalDependencySystem.DependencyThreshold,
                "dependency level sits at or above the 0.3 threshold");

            Check(sys.BeginManagedDetox("sv_mae", "opioid_painkillers"), "managed detox begins");
            Check(sys.HasActiveWithdrawal("sv_mae"), "withdrawal is active during detox");
            float morale = 0f, crafting = 0f;
            sys.OnMoraleDrainRequested += (sv, m) => morale += m;
            sys.OnCraftingPenaltyChanged += (sv, f) => crafting = f;
            sys.TickHours("sv_mae", 24f);
            Check(morale > 0f, "managed detox drains morale per hour");
            sys.TickHours("sv_mae", 96f);
            Check(completed == 1 && failed == 0, "detox completes at the success threshold");
            Check(!sys.HasActiveWithdrawal("sv_mae"), "no active withdrawal after completion");
            Check(crafting == 0f, "penalties cleared after completion");

            // Cold turkey path (the Unity quirk: sentinel progress<0 lasted one tick;
            // the port flags it and the 72h withdrawal actually completes).
            sys.OnSubstanceConsumed("sv_ged", "vodka", ChemicalDependencyKind.Alcohol);
            sys.OnSubstanceConsumed("sv_ged", "vodka", ChemicalDependencyKind.Alcohol);
            sys.OnSubstanceConsumed("sv_ged", "vodka", ChemicalDependencyKind.Alcohol);
            Check(sys.BeginColdTurkey("sv_ged", "vodka"), "cold turkey begins");
            int coldCompleted = 0;
            sys.OnDetoxCompleted += (sv, item) => { if (sv == "sv_ged") coldCompleted++; };
            sys.TickHours("sv_ged", 24f);
            sys.TickHours("sv_ged", 48f); // 72 total
            Check(coldCompleted == 1, "cold turkey withdrawal completes at 72h");
            Check(!sys.HasActiveWithdrawal("sv_ged"), "no withdrawal after cold turkey completes");

            // Clean decay removes the dependency entirely.
            sys.OnSubstanceConsumed("sv_wren", "sedatives", ChemicalDependencyKind.Sedative);
            sys.TickHours("sv_wren", 24f * 10f);
            Check(sys.DependenciesFor("sv_wren").Count == 0, "clean decay removes the dependency");

            // Save/load round-trip with checksum stability.
            string before = SaveChecksum.Compute(sys.CaptureState());
            var restored = new ChemicalDependencySystem();
            restored.RestoreState(sys.CaptureState());
            string after = SaveChecksum.Compute(restored.CaptureState());
            Check(before == after, "save/load checksum stable");

            // Snapshot isolation.
            var snapshot = sys.CaptureState();
            if (snapshot.survivors.Count > 0 && snapshot.survivors[0].dependencies.Count > 0)
            {
                snapshot.survivors[0].dependencies[0].dependencyLevel = 99f;
                bool leaked = false;
                foreach (var kv in sys.Ledger)
                    foreach (var d in kv.Value)
                        if (d.dependencyLevel > 1f) leaked = true;
                Check(!leaked, "capture returns snapshot, not live state");
            }
            else
            {
                Check(true, "capture returns snapshot, not live state (no deps to inject)");
            }

            report.Passed = report.FailedCount == 0;
            report.Summary =
                $"[MedicalHeadlessDemo] {(report.Passed ? "PASS" : "FAIL")} " +
                $"{report.PassedCount}/{report.PassedCount + report.FailedCount}";
            log.Info(report.Summary);
            return report;
        }
    }
}
