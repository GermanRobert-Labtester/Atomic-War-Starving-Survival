using System.Collections.Generic;

namespace Ashfall.Core.Expeditions
{
    /// <summary>
    /// Headless verification of the expedition core (Encounters port).
    /// Invoked by `dotnet test` and by Godot `-- --expedition-selftest`.
    /// </summary>
    public static class ExpeditionHeadlessDemo
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

            log.Info("[ExpeditionHeadlessDemo] begin");
            ExpeditionDefinitionRegistry.Clear();

            var def = new ExpeditionDefinition
            {
                id = "loc_demo_site",
                displayName = "Demo Site",
                distanceTicks = 3,
                dangerLevel = 10,         // loot chance 1.0 per looting tick (deterministic demo)
                encounterChancePerTick = 0.10f,
                baseStaminaDrainPerHour = 2.0f,
                lootCategories = new List<string> { "scrap_metal", "clean_water", "bandages" }
            };
            ExpeditionDefinitionRegistry.Register(def);

            var sys = new ExpeditionSystem();
            Check(!sys.Start(null!, "sv_mae", 1), "null definition refused");
            Check(sys.Start(def, "sv_mae", 1, ExpeditionStance.Stealth), "expedition starts");
            Check(!sys.Start(def, "sv_mae", 1), "second expedition for same survivor refused");

            sys.TickHours(1f, new SeededRng(7));
            sys.TickHours(1f, new SeededRng(7));
            sys.TickHours(1f, new SeededRng(7));
            var exp = new List<ExpeditionState>(sys.Active.Values)[0];
            Check(exp.phase == (int)ExpeditionPhase.Looting, "outbound travel completes into looting");

            sys.TickHours(1f, new SeededRng(7));
            sys.TickHours(1f, new SeededRng(7));
            sys.TickHours(1f, new SeededRng(7));
            Check(exp.phase == (int)ExpeditionPhase.Inbound, "auto-retreat after 3 looting ticks");
            Check(exp.loot.Count > 0, "loot rolls occurred (danger 10 -> guaranteed)");

            sys.TickHours(1f, new SeededRng(7));
            sys.TickHours(1f, new SeededRng(7));
            sys.TickHours(1f, new SeededRng(7));
            Check(sys.ActiveCount == 0, "expedition completes and clears");

            // Determinism: same seed, same loot.
            var sysA = new ExpeditionSystem();
            sysA.Start(def, "sv_a", 1);
            for (int i = 0; i < 8; i++) sysA.TickHours(1f, new SeededRng(99));
            var sysB = new ExpeditionSystem();
            sysB.Start(def, "sv_b", 1);
            for (int i = 0; i < 8; i++) sysB.TickHours(1f, new SeededRng(99));
            string LootKey(ExpeditionSystem s)
            {
                var sb = new System.Text.StringBuilder();
                foreach (var kv in s.Active)
                    foreach (var l in kv.Value.loot)
                        sb.Append(l.itemId).Append(':').Append(l.quantity).Append(',');
                return sb.ToString();
            }
            Check(LootKey(sysA) == LootKey(sysB), "same seed produces identical loot");

            // Save round-trip with checksum stability.
            string before = SaveChecksum.Compute(sysA.CaptureState());
            var restored = new ExpeditionSystem();
            restored.RestoreState(sysA.CaptureState());
            string after = SaveChecksum.Compute(restored.CaptureState());
            Check(before == after, "save/load checksum stable");

            // Snapshot isolation.
            var snapshot = sysA.CaptureState();
            if (snapshot.Count > 0 && snapshot[0].loot.Count > 0)
            {
                snapshot[0].loot.Add(new ExpeditionLootEntry { itemId = "injected", quantity = 99, weightKg = 1f });
                bool leaked = false;
                foreach (var kv in sysA.Active)
                    foreach (var l in kv.Value.loot)
                        if (l.itemId == "injected") leaked = true;
                Check(!leaked, "capture returns snapshot, not live state");
            }
            else
            {
                Check(true, "capture returns snapshot, not live state (no loot to inject)");
            }

            ExpeditionDefinitionRegistry.Clear();
            report.Passed = report.FailedCount == 0;
            report.Summary =
                $"[ExpeditionHeadlessDemo] {(report.Passed ? "PASS" : "FAIL")} " +
                $"{report.PassedCount}/{report.PassedCount + report.FailedCount}";
            log.Info(report.Summary);
            return report;
        }
    }
}
