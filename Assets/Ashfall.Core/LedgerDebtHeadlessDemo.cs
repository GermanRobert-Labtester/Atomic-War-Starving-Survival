using System.Text;

#pragma warning disable CS8618
namespace Ashfall.Core
{
    /// <summary>Headless-report extension for the ledger slice.</summary>
    public sealed class LedgerDebtHeadlessReport : HeadlessReport
    {
        public LedgerDebtSystemState Ledger;
        public int SignedCount;
        public int PaidCount;
        public int ForfeitCount;
        public int RenegotiatedCount;
        public int TamperedCount;
    }

    /// <summary>
    /// Vertical-slice smoke for the Underwrite ledger: read-twice signing,
    /// ink-freeze, term expiry into a named forfeit, the honoured path (pay
    /// after due), replacement-not-amendment renegotiation, one-shot tamper,
    /// and a JSON roundtrip through the port. Debtor ids are master-list ids
    /// from characters.json (Wyn = npc_wyn_sabler, Ivo = npc_ivo_fenn).
    /// Invoked by `dotnet test`; host CLI wiring mirrors --census-selftest.
    /// </summary>
    public static class LedgerDebtHeadlessDemo
    {
        public static LedgerDebtHeadlessReport Run(ILog log = null!)
        {
            CatalogLocator.UseInvariantCulture();
            log = log ?? NullLog.Instance;
            var report = new LedgerDebtHeadlessReport();

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

            log.Info("[LedgerDebtHeadlessDemo] begin");

            var ledger = new LedgerDebtSystem();
            ledger.OnContractSigned += _ => report.SignedCount++;
            ledger.OnContractPaid += _ => report.PaidCount++;
            ledger.OnForfeitTriggered += _ => report.ForfeitCount++;
            ledger.OnContractRenegotiated += _ => report.RenegotiatedCount++;
            ledger.OnLedgerTampered += () => report.TamperedCount++;

            const string wyn = "npc_wyn_sabler";
            const string ivo = "npc_ivo_fenn";

            Check(ledger.PresentContract(wyn, 12f, 30, 0.2f, "the pledged grain"), "first reading");
            Check(!ledger.SignContract(wyn, 40), "one reading is not ink");
            Check(ledger.PresentContract(wyn, 12f, 30, 0.2f, "the pledged grain"), "second reading");
            Check(ledger.SignContract(wyn, 40), "signed after two readings");
            Check(report.SignedCount == 1, "OnContractSigned fired once");
            Check(System.Math.Abs(ledger.TotalOwed(wyn) - 14.4f) < 0.001f, "flat rate: 12 × 1.2");
            Check(!ledger.PresentContract(wyn, 999f, 1, 0.9f, "anything"),
                "ink is ink — no new draft over an open debt");

            // Ivo's draft: read once, then torn up and written again. His term
            // outlasts Wyn's so only Wyn's forfeit fires in the tick window below.
            Check(ledger.PresentContract(ivo, 5f, 10, 0.1f, "one day's labour"), "ivo first reading");
            Check(ledger.RenegotiateContract(ivo, 8f, 60, 0.15f, "two days at the Lockup"),
                "draft renegotiated before ink");
            Check(report.RenegotiatedCount == 1, "OnContractRenegotiated fired");
            Check(ledger.PresentContract(ivo, 8f, 60, 0.15f, "two days at the Lockup"), "ivo reread 1");
            Check(ledger.PresentContract(ivo, 8f, 60, 0.15f, "two days at the Lockup"), "ivo reread 2");
            Check(ledger.SignContract(ivo, 41), "ivo signed");

            // Wyn's term runs out. The forfeit was named up front.
            for (int d = 0; d < 30; d++)
                ledger.TickDaily(41 + d);
            Check(ledger.GetContract(wyn)!.forfeited, "term expired → forfeit due");
            Check(report.ForfeitCount == 1, "OnForfeitTriggered fired once");
            Check(ledger.GetContract(wyn)!.forfeit == "the pledged grain", "forfeit is the named good");
            Check(!ledger.RenegotiateContract(wyn, 1f, 10, 0f, "anything"),
                "no renegotiation while a forfeit pends");

            // The honoured path: pay the named good back.
            Check(ledger.PayContract(wyn, 71), "forfeited debt can still be honoured");
            Check(ledger.GetContract(wyn)!.paid && !ledger.GetContract(wyn)!.forfeited, "paid in full");
            Check(report.PaidCount == 1, "OnContractPaid fired once");

            // One strike on the ledger, once.
            Check(ledger.TamperLedger(), "ledger tampered");
            Check(!ledger.TamperLedger(), "one strike per playthrough");
            Check(report.TamperedCount == 1, "OnLedgerTampered fired once");

            // JSON roundtrip through the port.
            var json = new SystemTextJsonSerializer();
            var blob = json.Serialize(ledger.CaptureState());
            var restored = new LedgerDebtSystem();
            restored.RestoreState(json.Deserialize<LedgerDebtSystemState>(blob)!);
            Check(restored.Contracts.Count == 2, "roundtrip contracts");
            Check(restored.GetContract(wyn)!.paid, "roundtrip paid");
            Check(restored.GetContract(ivo)!.signed, "roundtrip ivo signed");
            Check(restored.LedgerTampered, "roundtrip tamper");

            report.Ledger = ledger.CaptureState();
            report.Passed = report.FailedCount == 0;
            var sb = new StringBuilder();
            sb.Append("LedgerDebtHeadlessDemo ");
            sb.Append(report.Passed ? "PASS" : "FAIL");
            sb.Append(" ").Append(report.PassedCount).Append("/").Append(report.PassedCount + report.FailedCount);
            report.Summary = sb.ToString();
            log.Info(report.Summary);
            return report;
        }
    }
}
