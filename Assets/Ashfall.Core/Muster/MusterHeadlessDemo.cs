using Ashfall.Core;
using System.Collections.Generic;

namespace Ashfall.Core.Muster
{    public sealed class MusterHeadlessReport : HeadlessReport
    {
        public MusterState Muster;
    }

    /// <summary>
    /// Vertical-slice smoke for Expansion 06 (The Muster): approach registry,
    /// Day-260 trigger, ending-key resolution, save/load checksum stability.
    /// Invoked by `dotnet test` and by Godot `-- --muster-selftest`.
    /// </summary>
    public static class MusterHeadlessDemo
    {
        public static MusterHeadlessReport Run(ILog log = null)
        {
            CatalogLocator.UseInvariantCulture();
            log = log ?? NullLog.Instance;
            var report = new MusterHeadlessReport();

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

            log.Info("[MusterHeadlessDemo] begin");

            var sys = new MusterSystem();
            Check(sys.Catalog.Count == 3, "founding catalog registers 3 questlines");

            var muster = sys.FindDefinition("quest_the_muster_uprising");
            Check(muster != null && muster.approaches.Count == 4, "Muster questline offers all 4 strategies");

            sys.SetEscalationDay(259);
            Check(!sys.MusterTriggered, "muster stays dormant before Day 260");
            sys.SetEscalationDay(260);
            Check(sys.MusterTriggered, "muster triggers at Day 260");
            Check(sys.EscalationDay == 260, "escalation day recorded");

            Check(sys.SelectApproach(QuestApproach.C), "approach C accepted (Nobody Stays)");
            Check(!sys.SelectApproach(QuestApproach.A), "second selection rejected after resolution");
            Check(sys.ResolveEndingKey() == "the_corridor", "ending key resolves to the corridor");

            Check(!sys.SelectApproachFor("quest_the_unsigned_order", QuestApproach.B), "unsigned order rejects all approaches (no fork)");
            Check(sys.SelectApproachFor("quest_the_rate_card_war", QuestApproach.C), "rate card war accepts approach C (Seize)");
            Check(sys.EndingKeyFor("quest_the_rate_card_war") == "the_administrator", "rate card war ending key resolves");

            // Save/load round-trip through the checksum (cross-host stability).
            string before = SaveChecksum.Compute(sys.CaptureState());
            var restored = new MusterSystem();
            restored.RestoreState(sys.CaptureState());
            string after = SaveChecksum.Compute(restored.CaptureState());
            Check(before == after, "save/load checksum stable");

            // Snapshot isolation (no aliasing into the live envelope).
            var snapshot = sys.CaptureState();
            snapshot.records[0].endingKey = "injected";
            Check(sys.ResolveEndingKey() == "the_corridor", "capture returns snapshot, not live state");

            report.Muster = sys.CaptureState();
            report.Passed = report.FailedCount == 0;
            report.Summary =
                $"[MusterHeadlessDemo] {(report.Passed ? "PASS" : "FAIL")} " +
                $"{report.PassedCount}/{report.PassedCount + report.FailedCount}";
            log.Info(report.Summary);
            return report;
        }
    }
}
