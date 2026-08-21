using System.Text;

namespace Ashfall.Core
{
    /// <summary>Headless-report extension for the Census slice.</summary>
    public sealed class CensusHeadlessReport : HeadlessReport
    {
        public CensusClaimSystemState Census;
        public int LevyIssuedCount;
        public int LevyResolvedCount;
        public string ResolvedFlag;
    }

    /// <summary>
    /// Vertical-slice smoke for the Holdfast census/levy loop: the Office names
    /// at most three people, the bunker answers honour/substitute/refuse, days
    /// run the levy down, 12-C and trust stay clamped, and the state survives a
    /// JSON roundtrip through the port (JsonUtility-free).
    /// Invoked by `dotnet test`; wired into the Godot host later as
    /// `-- --census-selftest`.
    /// </summary>
    public static class CensusHeadlessDemo
    {
        public static CensusHeadlessReport Run(ILog log = null!)
        {
            CatalogLocator.UseInvariantCulture();
            log = log ?? NullLog.Instance;
            var report = new CensusHeadlessReport();

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

            log.Info("[CensusHeadlessDemo] begin");

            var census = new CensusClaimSystem();
            census.OnLevyIssued += _ => report.LevyIssuedCount++;
            census.OnLevyResolved += flag => { report.LevyResolvedCount++; report.ResolvedFlag = flag; };

            // The Office names the whole roster; the book caps at three.
            var roster = new[] { "sv_mae", "sv_iora", "sv_ged", "sv_hale", "sv_wren" };
            Check(census.IssueLevy(roster, day: 40), "levy issued");
            Check(census.ActiveLevy.survivorIds.Length == CensusClaimSystem.MaxLevyCount, "levy caps at three named ids");
            Check(census.ActiveLevy.survivorIds[0] == "sv_mae" && census.ActiveLevy.survivorIds[2] == "sv_ged", "first three names kept in order");
            Check(report.LevyIssuedCount == 1, "OnLevyIssued fired once");

            // Honour: flag set, three people marked away. A second levy is
            // refused while this one is active.
            Check(census.HonourLevy(), "honour accepted");
            Check(!census.IssueLevy(new[] { "sv_late" }, 41), "second levy refused while active");
            Check(report.LevyIssuedCount == 1, "no second levy event");
            Check(census.LevyHonour && !census.LevyRefuse, "honour flag set");
            Check(census.IsAssignedAway("sv_iora"), "named survivor assigned away");
            Check(census.AssignedAwayIds().Count == 3, "assigned-away list = 3");
            Check(report.ResolvedFlag == CensusClaimSystem.FlagLevyHonour, "OnLevyResolved carries honour flag");

            // Days run the levy down.
            for (int d = 0; d < CensusClaimSystem.DefaultLevyDays; d++)
                census.TickDaily(41 + d);
            Check(!census.ActiveLevy.active, "levy expires after duration");
            Check(census.AssignedAwayIds().Count == 0, "assignments cleared on expiry");

            // Substitute: three *other* names, still capped.
            Check(census.IssueLevy(roster, day: 80), "second levy issued after expiry");
            Check(census.SubstituteLevy(new[] { "sv_a", "sv_b", "sv_c", "sv_d" }), "substitute accepted");
            Check(census.LevySubstitute, "substitute flag set");
            Check(census.ActiveLevy.survivorIds.Length == 3, "substitute still capped at three");
            Check(!census.IsAssignedAway("sv_mae"), "original names released");
            Check(census.IsAssignedAway("sv_b"), "substitute names assigned");
            Check(report.ResolvedFlag == CensusClaimSystem.FlagLevySubstitute, "OnLevyResolved carries substitute flag");

            // Refuse: Edor waits at the hatch, nobody is taken.
            var refusing = new CensusClaimSystem();
            refusing.UpsertLedger("sv_mae", "Mae", "caretaker", listed: false);
            refusing.UpsertLedger("sv_iora", "Iora", "clerk", listed: false);
            refusing.UpsertLedger("sv_ged", "Ged", "vet", listed: false);
            refusing.UpsertLedger("sv_hale", "Hale", "lamp", listed: false);
            refusing.IssueLevy(new[] { "sv_mae", "sv_iora", "sv_ged" }, 20);
            Check(refusing.RefuseLevy(21), "refuse accepted");
            Check(refusing.LevyRefuse, "refuse flag set");
            Check(refusing.State.edorWaitingAtHatch, "edor waiting at hatch");
            Check(!refusing.IsAssignedAway("sv_mae") && !refusing.IsAssignedAway("sv_hale"), "refuse takes nobody");

            // 12-C and trust clamp.
            refusing.Activate12C();
            refusing.Activate12C();
            Check(refusing.Order12CActive, "12-C active");
            refusing.AdjustOfficeTrust(-500f);
            Check(refusing.State.officeTrust <= -100f, "trust clamps at -100");
            refusing.AdjustOfficeTrust(500f);
            Check(refusing.State.officeTrust >= 100f, "trust clamps at +100");

            // JSON roundtrip through the port.
            var json = new SystemTextJsonSerializer();
            var blob = json.Serialize(refusing.CaptureState());
            var restored = new CensusClaimSystem();
            restored.RestoreState(json.Deserialize<CensusClaimSystemState>(blob)!);
            Check(restored.LevyRefuse, "roundtrip refuse flag");
            Check(restored.Order12CActive, "roundtrip 12-C");
            Check(restored.State.ledger.Count == refusing.State.ledger.Count, "roundtrip ledger size");
            Check(restored.State.officeTrust == refusing.State.officeTrust, "roundtrip trust value");

            report.Census = refusing.CaptureState();
            report.Passed = report.FailedCount == 0;
            var sb = new StringBuilder();
            sb.Append("CensusHeadlessDemo ");
            sb.Append(report.Passed ? "PASS" : "FAIL");
            sb.Append(" ").Append(report.PassedCount).Append("/").Append(report.PassedCount + report.FailedCount);
            report.Summary = sb.ToString();
            log.Info(report.Summary);
            return report;
        }
    }
}
