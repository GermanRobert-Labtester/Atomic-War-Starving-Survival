using System;
using System.Collections.Generic;
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
        // Plan IV (F3) — template/consequence catalog coverage.
        public int CatalogErrorCount = -1;
        public int TemplateCount;
        public int ConsequenceCount;
        public int MissingReferenceCount;
        public int EscalationCycleCount;
        public int ConsequenceDispatchCount;
        public int DispatcherRoundtripRedispatches = -1;
    }

    /// <summary>
    /// Vertical-slice smoke for the Underwrite ledger: read-twice signing,
    /// ink-freeze, term expiry into a named forfeit, the honoured path (pay
    /// after due), replacement-not-amendment renegotiation, one-shot tamper,
    /// and a JSON roundtrip through the port. Debtor ids are master-list ids
    /// from characters.json (Wyn = npc_wyn_sabler, Ivo = npc_ivo_fenn).
    ///
    /// Plan IV (F3) extends the same demo into a compact integration oracle:
    /// template catalog loading (15 templates / 10 consequences, zero errors),
    /// consequence foreign keys, escalation-graph cycle detection, catalog-
    /// driven consequence dispatch (standing AND collateral), dispatcher
    /// fired-state persistence round-trip, embargo/bounty/labor payload
    /// coverage, and ledger-native forgiveness.
    /// Invoked by `dotnet test` and the expansion master suite.
    /// </summary>
    public static class LedgerDebtHeadlessDemo
    {
        public static LedgerDebtHeadlessReport Run(string? dataDirectory = null, ILog? log = null)
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

            // ── Plan IV (F3): template/consequence integration oracle ──────

            string? found = dataDirectory;
            if (string.IsNullOrEmpty(found) &&
                CatalogLocator.TryFindDataDirectory(System.IO.Directory.GetCurrentDirectory(), out string f1))
                found = f1;
            if (string.IsNullOrEmpty(found) &&
                CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out string f2))
                found = f2;
            Check(!string.IsNullOrEmpty(found), "data directory located");

            if (!string.IsNullOrEmpty(found))
                RunCatalogOracle((string)found!, json, report, Check, log);

            report.Ledger = ledger.CaptureState();
            report.Passed = report.FailedCount == 0;
            var sb = new StringBuilder();
            sb.Append("LedgerDebtHeadlessDemo ");
            sb.Append(report.Passed ? "PASS" : "FAIL");
            sb.Append(" ").Append(report.PassedCount).Append("/").Append(report.PassedCount + report.FailedCount);
            sb.Append(" (core runtime · save roundtrip · template catalog · escalation graph · consequence dispatch · dispatcher roundtrip)");
            report.Summary = sb.ToString();
            log.Info(report.Summary);
            return report;
        }

        private static void RunCatalogOracle(
            string dataDirectory,
            SystemTextJsonSerializer json,
            LedgerDebtHeadlessReport report,
            Action<bool, string> check,
            ILog log)
        {
            var catalog = DebtTemplateCatalogLoader.Load(dataDirectory, new FileSystemIO(), json);
            report.CatalogErrorCount = catalog.Errors.Count;
            report.TemplateCount = catalog.Templates.Count;
            report.ConsequenceCount = catalog.Consequences.Count;
            check(report.CatalogErrorCount == 0, "debt catalog loads with zero errors");
            check(report.TemplateCount == 15, "catalog carries 15 templates (got " + report.TemplateCount + ")");
            check(report.ConsequenceCount == 10, "catalog carries 10 consequences (got " + report.ConsequenceCount + ")");

            // F3.3/F3.4 — foreign keys with per-reference diagnostics.
            int missing = 0;
            for (int i = 0; i < catalog.Templates.Count; i++)
            {
                var t = catalog.Templates[i];
                if (catalog.GetConsequence(t.consequenceId) == null)
                {
                    missing++;
                    log.Error("[LedgerDebt/Templates] Template " + t.id + " references missing consequence " + t.consequenceId);
                }
            }
            for (int i = 0; i < catalog.Consequences.Count; i++)
            {
                var c = catalog.Consequences[i];
                if (!string.IsNullOrEmpty(c.escalationId) && catalog.GetConsequence(c.escalationId) == null)
                {
                    missing++;
                    log.Error("[LedgerDebt/Escalations] Consequence " + c.id + " references missing escalation " + c.escalationId);
                }
            }
            report.MissingReferenceCount = missing;
            check(missing == 0, "every template consequence and escalation target resolves");

            // F3.5 — escalation graph must be acyclic (walk with a visited set).
            int cycles = 0;
            for (int i = 0; i < catalog.Consequences.Count; i++)
            {
                var node = catalog.Consequences[i];
                var visited = new HashSet<string>();
                var path = new List<string> { node.id };
                var cursor = node;
                while (cursor != null && !string.IsNullOrEmpty(cursor.escalationId))
                {
                    if (!visited.Add(cursor.id))
                    {
                        cycles++;
                        log.Error("[LedgerDebt/Cycles] escalation cycle: " + string.Join(" -> ", path) + " -> " + cursor.id);
                        break;
                    }
                    path.Add(cursor.escalationId);
                    cursor = catalog.GetConsequence(cursor.escalationId);
                }
            }
            report.EscalationCycleCount = cycles;
            check(cycles == 0, "escalation graph has no cycles");

            // F3.6-F3.11 — rations scenario: template fields drive the debt,
            // the forfeit dispatches the authored standing consequence exactly once.
            var rations = catalog.GetTemplate("debt_supply_corps_rations");
            check(rations != null, "rations template present");
            if (rations == null) return;

            check(rations.principalItemId == "canned_food" && rations.principalQuantity == 8,
                "rations principal is 8 × canned_food");
            check(rations.termDays == 20 && Math.Abs(rations.rate - 0.15f) < 0.0001f,
                "rations terms are 20 days at 15%");

            var scenarioLedger = new LedgerDebtSystem();
            var dispatcher = new DebtConsequenceDispatcher(scenarioLedger, catalog);
            dispatcher.SetDayProvider(() => 60);
            int forfeits = 0, dispatched = 0, standings = 0, seizures = 0, embargoes = 0, bounties = 0;
            string? standingFaction = null;
            int standingDelta = 0;
            scenarioLedger.OnForfeitTriggered += _ => forfeits++;
            dispatcher.OnConsequenceDispatched += (_, _) => dispatched++;
            dispatcher.OnStandingPenalty += (c, f, _) => { standings++; standingFaction = f; standingDelta = c.standingDelta; };
            dispatcher.OnCollateralSeizure += (_, _, _) => seizures++;
            dispatcher.OnEmbargoRequested += (_, _, _) => embargoes++;
            dispatcher.OnBountyRequested += (_, _) => bounties++;

            const string debtor = "npc_dessa_penn";
            check(scenarioLedger.PresentContract(debtor, rations.principalQuantity, rations.termDays,
                rations.rate, rations.forfeitDescription, rations.creditorId, rations.id), "rations first reading");
            check(!scenarioLedger.SignContract(debtor, 40), "signing before two readings is refused");
            check(scenarioLedger.PresentContract(debtor, rations.principalQuantity, rations.termDays,
                rations.rate, rations.forfeitDescription, rations.creditorId, rations.id), "rations second reading");
            check(scenarioLedger.SignContract(debtor, 40), "rations signed");

            for (int d = 0; d < rations.termDays; d++)
                scenarioLedger.TickDaily(41 + d);
            check(forfeits == 1, "forfeit triggered once");
            check(dispatched == 1, "consequence dispatched once");
            check(standings == 1, "standing consequence fired once");
            check(standingFaction == "faction_supply_corps",
                "standing targets the creditor (got " + (standingFaction ?? "none") + ")");
            check(standingDelta == -5, "standing delta is the authored -5 (got " + standingDelta + ")");

            // F3.12 — fired-state JSON roundtrip prevents redispatch.
            var dispatcherBlob = json.Serialize(dispatcher.CaptureState());
            var ledgerBlob = json.Serialize(scenarioLedger.CaptureState());
            var roundtripLedger = new LedgerDebtSystem();
            roundtripLedger.RestoreState(json.Deserialize<LedgerDebtSystemState>(ledgerBlob)!);
            var roundtripDispatcher = new DebtConsequenceDispatcher(roundtripLedger, catalog);
            roundtripDispatcher.RestoreState(json.Deserialize<DebtDispatcherState>(dispatcherBlob)!);
            int redispatches = 0;
            roundtripDispatcher.OnConsequenceDispatched += (_, _) => redispatches++;
            for (int d = 0; d < 30; d++)
                roundtripLedger.TickDaily(70 + d);
            report.DispatcherRoundtripRedispatches = redispatches;
            check(redispatches == 0, "restored fired-state prevents redispatch");

            // F3.13 — a second consequence type proves dispatch is catalog-driven
            // (bounty_and_seizure takes the pledged principal, fires no standing).
            var scav = catalog.GetTemplate("debt_scavengers_food");
            check(scav != null, "scavenger template present");
            string? seizedItem = null;
            int seizedQty = 0;
            dispatcher.OnCollateralSeizure += (item, qty, _) => { seizedItem = item; seizedQty = qty; };
            check(DefaultNewScenario(scenarioLedger, scav!, debtor, 90), "scavenger debt defaults");
            check(seizedItem == "dried_rations" && seizedQty == scav!.principalQuantity,
                "collateral seizure takes the pledged principal");
            check(standings == 1, "no standing penalty for the seizure consequence");

            // F3.14 — embargo/bounty typed payloads from real templates.
            var water = catalog.GetTemplate("debt_hydro_barons_water");
            string? embargoScope = null;
            int embargoDays = 0;
            dispatcher.OnEmbargoRequested += (scope, days, _) => { embargoScope = scope; embargoDays = days; };
            check(DefaultNewScenario(scenarioLedger, water!, debtor, 95), "water debt defaults");
            check(embargoScope == "creditor_faction" && embargoDays == 14,
                "embargo payload is the authored scope and duration");

            var engine = catalog.GetTemplate("debt_railway_guild_transport");
            int bountiesBeforeEngine = bounties;
            check(DefaultNewScenario(scenarioLedger, engine!, debtor, 100), "engine debt defaults");
            check(bounties - bountiesBeforeEngine == 2,
                "bounty request emitted with its raid escalation (got " + (bounties - bountiesBeforeEngine) + ")");

            // F3.15 — forgiveness is a ledger state transition, not an event.
            var mercy = new DebtTemplate
            {
                id = "debt_fixture_mercy", creditorId = "faction_supply_corps",
                principalItemId = "canned_food", principalQuantity = 4, termDays = 10, rate = 0.1f,
                forfeitDescription = "four tins held in mercy", consequenceId = "conseq_forgiveness_rare",
                displayName = "Fixture Mercy Credit", description = "fixture"
            };
            catalog.Templates.Add(mercy); // deterministic forced path for the rare consequence
            check(DefaultNewScenario(scenarioLedger, mercy, debtor, 105), "mercy debt defaults");
            var mercied = scenarioLedger.GetContract(debtor);
            check(mercied != null && mercied.forgiven, "forgiveness_rare clears the ledger balance");
            check(mercied != null && !mercied.paid, "forgiveness consumed no payment");

            report.ConsequenceDispatchCount = dispatched;
        }

        /// <summary>Settle any open ink (the honoured path archives it), then
        /// present twice, sign, and tick to default on a fresh contract.</summary>
        private static bool DefaultNewScenario(LedgerDebtSystem ledger, DebtTemplate template, string debtor, int day)
        {
            var existing = ledger.GetContract(debtor);
            if (existing != null && existing.signed && !existing.paid && !existing.forgiven)
            {
                if (!ledger.PayContract(debtor, day)) return false;
            }
            if (!ledger.PresentContract(debtor, template.principalQuantity, template.termDays,
                    template.rate, template.forfeitDescription, template.creditorId, template.id))
                return false;
            if (!ledger.PresentContract(debtor, template.principalQuantity, template.termDays,
                    template.rate, template.forfeitDescription, template.creditorId, template.id))
                return false;
            if (!ledger.SignContract(debtor, day))
                return false;
            for (int d = 0; d < template.termDays; d++)
                ledger.TickDaily(day + 1 + d);
            var settled = ledger.GetContract(debtor);
            return settled != null && (settled.forfeited || settled.forgiven);
        }
    }
}
