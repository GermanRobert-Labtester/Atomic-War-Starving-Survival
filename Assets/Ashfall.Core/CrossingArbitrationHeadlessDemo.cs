using System.Collections.Generic;
using System.Text;

namespace Ashfall.Core
{
    /// <summary>Headless-report extension for the Standing slice.</summary>
    public sealed class ArbitrationHeadlessReport : HeadlessReport
    {
        public CrossingArbitrationState Arbitration;
        public int RulingsCalled;
        public int RulingsOverturned;
    }

    /// <summary>
    /// Vertical-slice smoke for the Crossing Standing: a principled ruling is
    /// held by three, a bought one comes out rigged, three counters overturn
    /// it, a dead backer lets a ruling fall, and the whole board survives a
    /// JSON roundtrip through the port. Backer ids are master-list ids from
    /// characters.json; the topic is quest_crossing_the_standing.
    /// Invoked by `dotnet test`; host CLI wiring mirrors --census-selftest.
    /// </summary>
    public static class CrossingArbitrationHeadlessDemo
    {
        public static ArbitrationHeadlessReport Run(ILog log = null)
        {
            CatalogLocator.UseInvariantCulture();
            log = log ?? NullLog.Instance;
            var report = new ArbitrationHeadlessReport();

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

            log.Info("[CrossingArbitrationHeadlessDemo] begin");

            var sys = new CrossingArbitrationSystem();
            sys.OnStandingCalled += _ => report.RulingsCalled++;
            sys.OnRulingOverturned += _ => report.RulingsOverturned++;
            sys.LoadBackerPool(new List<BackerDef>
            {
                new BackerDef { id = "npc_osran_kell", displayName = "Osran Kell", principled = true },
                new BackerDef { id = "npc_mattis_cray", displayName = "Mattis Cray", principled = true },
                new BackerDef { id = "npc_bram_ostrowski", displayName = "Bram Ostrowski", principled = false },
                new BackerDef { id = "npc_leva_quist", displayName = "Leva Quist", principled = false },
                new BackerDef { id = "npc_halden_mire", displayName = "Halden Mire", principled = true }
            });

            const string topic = CrossingIds.TheStanding;
            Check(sys.CallStanding(topic, 45), "standing called");
            Check(sys.GetRuling(topic).shape == RulingShape.Pending, "pending until three hold it");
            Check(report.RulingsCalled == 1, "OnStandingCalled fired once");

            Check(sys.DeclareBacker(topic, CrossingIds.NpcOsran), "first backer declares");
            Check(sys.DeclareBacker(topic, CrossingIds.NpcMattis), "second backer declares");
            Check(sys.GetRuling(topic).shape == RulingShape.Pending, "two backers do not hold");
            Check(sys.DeclareBacker(topic, "npc_halden_mire"), "third backer declares");
            Check(sys.GetRuling(topic).shape == RulingShape.Honest, "principled majority → honest");
            Check(sys.IsRulingHeld(topic), "ruling held by three");

            Check(!sys.OverturnRuling(topic, new List<string> { "npc_bram_ostrowski" }),
                "one counter cannot overturn");
            Check(sys.OverturnRuling(topic, new List<string>
            {
                "npc_bram_ostrowski", "npc_leva_quist", "npc_halden_mire"
            }), "three counters overturn");
            Check(sys.GetRuling(topic).shape == RulingShape.Overturned, "shape is overturned");
            Check(sys.GetRuling(topic).backers.Count == 0, "backers cleared on overturn");
            Check(report.RulingsOverturned == 1, "OnRulingOverturned fired once");

            // A second, bought standing comes out rigged.
            const string riggedTopic = CrossingIds.ScaleIntegrity;
            Check(sys.CallStanding(riggedTopic, 46), "second standing called");
            Check(sys.DeclareBacker(riggedTopic, CrossingIds.NpcOsran), "principled backer joins");
            Check(sys.DeclareBacker(riggedTopic, "npc_bram_ostrowski"), "bought backer joins");
            Check(sys.DeclareBacker(riggedTopic, "npc_leva_quist"), "second bought backer joins");
            Check(sys.GetRuling(riggedTopic).shape == RulingShape.Rigged, "non-principled majority → rigged");
            Check(!sys.IsRulingHeld(riggedTopic), "rigged rulings do not hold");

            // Death lets a held ruling fall back to pending.
            Check(sys.CallStanding(CrossingIds.TheTerms, 47), "third standing called");
            Check(sys.DeclareBacker(CrossingIds.TheTerms, CrossingIds.NpcOsran), "holder one");
            Check(sys.DeclareBacker(CrossingIds.TheTerms, CrossingIds.NpcMattis), "holder two");
            Check(sys.DeclareBacker(CrossingIds.TheTerms, "npc_halden_mire"), "holder three");
            Check(sys.IsRulingHeld(CrossingIds.TheTerms), "third ruling held");
            Check(sys.RemoveBacker(CrossingIds.NpcMattis), "backer removed");
            Check(sys.GetRuling(CrossingIds.TheTerms).shape == RulingShape.Pending,
                "held ruling falls when a holder dies");

            // JSON roundtrip through the port.
            var json = new SystemTextJsonSerializer();
            var blob = json.Serialize(sys.CaptureState());
            var restored = new CrossingArbitrationSystem();
            restored.RestoreState(json.Deserialize<CrossingArbitrationState>(blob));
            Check(restored.BackerPool.Count == 5, "roundtrip backer pool");
            Check(restored.IsRulingOverturned(topic), "roundtrip overturned ruling");
            Check(restored.State.rulingsCalled == sys.State.rulingsCalled, "roundtrip call count");
            Check(restored.State.rulingsOverturned == sys.State.rulingsOverturned, "roundtrip overturn count");

            report.Arbitration = sys.CaptureState();
            report.Passed = report.FailedCount == 0;
            var sb = new StringBuilder();
            sb.Append("CrossingArbitrationHeadlessDemo ");
            sb.Append(report.Passed ? "PASS" : "FAIL");
            sb.Append(" ").Append(report.PassedCount).Append("/").Append(report.PassedCount + report.FailedCount);
            report.Summary = sb.ToString();
            log.Info(report.Summary);
            return report;
        }
    }
}
