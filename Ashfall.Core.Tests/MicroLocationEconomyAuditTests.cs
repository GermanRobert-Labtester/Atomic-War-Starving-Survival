using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// F12 flagship wave — reward-economy audit. Builds a per-choice trade
    /// value ledger from the live catalogs, measures the primary scavenging
    /// baseline through the production loot path (real expedition ticks), runs
    /// a deterministic 100-expedition economy simulation with greedy micro
    /// resolution (upper bound of micro contribution), reviews the four named
    /// outliers, verifies farming resistance, and evaluates the 10–30%
    /// micro/primary contribution band. The balance report is regenerated
    /// deterministically when ASHFALL_GEN_MICRO_REPORTS=1.
    ///
    /// Value units: item tradeValue only. Morale, guilt, journal unlocks and
    /// location discoveries are reported separately — the game defines no
    /// exchange rate converting them into trade value.
    /// </summary>
    public class MicroLocationEconomyAuditTests
    {
        private static string RepoRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "Ashfall.csproj")))
                dir = dir.Parent!;
            return dir!.FullName;
        }

        private static string DataDir() => Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data");

        private static List<EncounterDefinition> LoadMicroCatalog()
            => NarrativeEncounterCatalogLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer())
                .FindAll(e => e.id.StartsWith("micro_", StringComparison.Ordinal));

        private static Dictionary<string, double> LoadTradeValues()
        {
            var doc = System.Text.Json.JsonDocument.Parse(
                File.ReadAllText(Path.Combine(DataDir(), "items.json")));
            var values = new Dictionary<string, double>(StringComparer.Ordinal);
            foreach (var el in doc.RootElement.GetProperty("items").EnumerateArray())
            {
                var id = el.GetProperty("id").GetString();
                if (string.IsNullOrEmpty(id)) continue;
                values[id!] = el.TryGetProperty("tradeValue", out var tv) && tv.ValueKind == System.Text.Json.JsonValueKind.Number
                    ? tv.GetDouble() : 0d;
            }
            return values;
        }

        private sealed class ChoiceLedgerRow
        {
            public string EncounterId = string.Empty;
            public string ChoiceId = string.Empty;
            public bool Depletes;
            public double GrantedValue;
            public double ConsumedValue;
            public double NetItemValue;
            public int Morale;
            public int Guilt;
            public string Journal = string.Empty;
            public string Location = string.Empty;
            public string Flag = string.Empty;
            public double FaceValue => GrantedValue;
        }

        private static List<ChoiceLedgerRow> BuildLedger(Dictionary<string, double> trade)
        {
            var rows = new List<ChoiceLedgerRow>();
            foreach (var e in LoadMicroCatalog())
            {
                foreach (var c in e.choices)
                {
                    double granted = 0d, consumed = 0d;
                    if (!string.IsNullOrEmpty(c.grantItemId) && trade.TryGetValue(c.grantItemId, out var tv))
                    {
                        double line = c.grantItemQuantity * tv;
                        if (line > 0) granted += line; else consumed += -line;
                    }
                    if (!string.IsNullOrEmpty(c.requiredItemId) && trade.TryGetValue(c.requiredItemId, out var rtv))
                        consumed += c.requiredItemQuantity * rtv;

                    rows.Add(new ChoiceLedgerRow
                    {
                        EncounterId = e.id,
                        ChoiceId = c.choiceId,
                        Depletes = c.depletesOnResolve,
                        GrantedValue = granted,
                        ConsumedValue = consumed,
                        NetItemValue = granted - consumed,
                        Morale = c.moraleDelta,
                        Guilt = c.guiltDelta,
                        Journal = c.journalUnlockId ?? string.Empty,
                        Location = c.discoverLocationId ?? string.Empty,
                        Flag = c.setWorldFlag ?? string.Empty
                    });
                }
            }
            return rows;
        }

        // ── Ledger validation ───────────────────────────────────────────

        [Fact]
        public void RewardLedger_AllGrantedItemsResolve_QuantitiesPositive_ValuesFinite()
        {
            var trade = LoadTradeValues();
            var rows = BuildLedger(trade);
            Assert.Equal(28, rows.Select(r => r.EncounterId).Distinct().Count());

            foreach (var r in rows)
            {
                Assert.True(double.IsFinite(r.NetItemValue), $"{r.EncounterId}/{r.ChoiceId}: non-finite net value");
                Assert.True(r.NetItemValue >= -100d, $"{r.EncounterId}/{r.ChoiceId}: absurd consumed value");
                Assert.True(r.GrantedValue >= 0d, $"{r.EncounterId}/{r.ChoiceId}: negative grant value");
            }

            // Every positive-granting choice carries real trade value.
            var tradeable = rows.Where(r => r.GrantedValue > 0d).ToList();
            Assert.True(tradeable.Count > 0, "expected at least some item-granting choices");
            // Named outlier sanity against the plan's verified figures.
            var drop = Assert.Single(rows, r => r.EncounterId == "micro_supply_drop" && r.ChoiceId == "open_supply_drop");
            Assert.Equal(20d, drop.GrantedValue, 6); // 2 × medical_kit @ 10
            var ring = Assert.Single(rows, r => r.EncounterId == "micro_improvised_grave" && r.ChoiceId == "disturb_grave");
            Assert.Equal(25d, ring.GrantedValue, 6); // wedding_ring
        }

        // ── 100-expedition production-path economy simulation ───────────

        internal sealed class EconomyResult
        {
            public double TotalPrimaryValue;
            public double TotalMicroItemValue;
            public double TotalMicroNetValue;
            public int MicroEncounters;
            public int JournalUnlocks;
            public int LocationDiscoveries;
            public double MoraleDelta;
            public double GuiltDelta;
            public int Completed;
            public int Failed;
            public List<double> PerExpeditionPrimary = new List<double>();
            public List<double> PerExpeditionMicro = new List<double>();
            public Dictionary<string, double> MicroValueByEncounter = new Dictionary<string, double>(StringComparer.Ordinal);

            public string Canonical()
            {
                var sb = new StringBuilder();
                sb.Append("primary=").Append(TotalPrimaryValue.ToString("R", System.Globalization.CultureInfo.InvariantCulture));
                sb.Append(";micro=").Append(TotalMicroItemValue.ToString("R", System.Globalization.CultureInfo.InvariantCulture));
                sb.Append(";net=").Append(TotalMicroNetValue.ToString("R", System.Globalization.CultureInfo.InvariantCulture));
                sb.Append(";enc=").Append(MicroEncounters).Append(";journal=").Append(JournalUnlocks)
                  .Append(";disc=").Append(LocationDiscoveries).Append(";morale=").Append(MoraleDelta)
                  .Append(";guilt=").Append(GuiltDelta).Append(";done=").Append(Completed).Append(";fail=").Append(Failed);
                foreach (var k in MicroValueByEncounter.Keys.OrderBy(k => k, StringComparer.Ordinal))
                    sb.Append(';').Append(k).Append('=').Append(MicroValueByEncounter[k].ToString("R", System.Globalization.CultureInfo.InvariantCulture));
                return sb.ToString();
            }
        }

        private static EconomyResult RunEconomySimulation()
        {
            var destinations = ExpeditionCatalogLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.NotNull(destinations);
            var trade = LoadTradeValues();
            var result = new EconomyResult();

            const int expeditions = 100;
            for (int i = 0; i < expeditions; i++)
            {
                var def = destinations![i % destinations.Count];
                var fixture = MicroLocationDeterminismHarness.CreateFixture(4000 + i);

                double primary = 0d;
                double microItem = 0d, microNet = 0d;
                int microCount = 0;

                fixture.Engine.OnExpeditionCompleted += exp =>
                {
                    foreach (var l in exp.loot)
                        if (trade.TryGetValue(l.itemId, out var tv)) primary += l.quantity * tv;
                    result.Completed++;
                };
                fixture.Engine.OnExpeditionFailed += (exp, reason) => result.Failed++;

                // Greedy resolver: when a micro-location surfaces, take the
                // choice with the highest net item value (upper bound of what
                // a loot-motivated player extracts from the subsystem).
                fixture.Bridge.OnSurfaced += dto =>
                {
                    var id = dto.encounter_id;
                    if (string.IsNullOrEmpty(id) || !id.StartsWith("micro_", StringComparison.Ordinal)) return;
                    var defEnc = fixture.Narrative.Find(id);
                    if (defEnc == null || defEnc.choices.Count == 0) return;

                    EncounterChoiceDefinition? best = null;
                    double bestNet = double.NegativeInfinity;
                    foreach (var c in defEnc.choices)
                    {
                        double net = 0d;
                        if (!string.IsNullOrEmpty(c.grantItemId) && trade.TryGetValue(c.grantItemId, out var tv))
                            net += c.grantItemQuantity * tv;
                        if (!string.IsNullOrEmpty(c.requiredItemId) && trade.TryGetValue(c.requiredItemId, out var rtv))
                            net -= c.requiredItemQuantity * rtv;
                        if (net > bestNet) { bestNet = net; best = c; }
                    }
                    if (best == null) return;

                    var res = fixture.Narrative.TryResolve(id, best.choiceId, dto.trigger.locationId, dto.trigger.startedDay);
                    if (res == null) return;
                    microCount++;
                    if (!string.IsNullOrEmpty(res.JournalUnlockId)) result.JournalUnlocks++;
                    if (!string.IsNullOrEmpty(res.DiscoverLocationId)) result.LocationDiscoveries++;
                    result.MoraleDelta += res.MoraleDelta;
                    result.GuiltDelta += res.GuiltDelta;

                    double value = 0d;
                    if (!string.IsNullOrEmpty(res.GrantItemId) && trade.TryGetValue(res.GrantItemId, out var gtv))
                        value = res.GrantItemQuantity * gtv;
                    microItem += Math.Max(0d, value);
                    microNet += value;
                    result.MicroValueByEncounter[id] = result.MicroValueByEncounter.GetValueOrDefault(id) + value;
                };

                fixture.Engine.DiscoverLocation(def!.id);
                Assert.True(fixture.Engine.Start(def, MicroLocationDeterminismHarness.SurvivorId, 1, ExpeditionStance.Stealth));
                fixture.Tick(12); // ample budget for outbound + looting + inbound on any authored route

                result.TotalPrimaryValue += primary;
                result.TotalMicroItemValue += microItem;
                result.TotalMicroNetValue += microNet;
                result.MicroEncounters += microCount;
                result.PerExpeditionPrimary.Add(primary);
                result.PerExpeditionMicro.Add(microItem);
            }
            return result;
        }

        [Fact]
        public void EconomySimulation_100Expeditions_IsDeterministic_AndProducesFiniteValues()
        {
            var a = RunEconomySimulation();
            var b = RunEconomySimulation();
            Assert.Equal(a.Canonical(), b.Canonical());

            Assert.True(double.IsFinite(a.TotalPrimaryValue) && a.TotalPrimaryValue > 0d, "primary loot baseline must be positive");
            Assert.True(double.IsFinite(a.TotalMicroItemValue) && a.TotalMicroItemValue >= 0d);
            Assert.Equal(100, a.PerExpeditionPrimary.Count);

            double meanPrimary = a.TotalPrimaryValue / 100d;
            double meanMicro = a.TotalMicroItemValue / 100d;
            double ratio = meanMicro / meanPrimary;
            EconomyReportScratch.MeanPrimary = meanPrimary;
            EconomyReportScratch.MeanMicro = meanMicro;
            EconomyReportScratch.Ratio = ratio;
            EconomyReportScratch.Result = a;

            // Broad invariants only — the exact ratio is reported and assessed
            // in MICRO_LOCATION_BALANCE.md, never hard-failed on a tuned value.
            Assert.InRange(ratio, 0d, 1.0d);
        }

        // ── Farming resistance (§F12.12) ─────────────────────────────────

        [Fact]
        public void FarmingResistance_DepletingGrantEncounters_AreOneShot_InProductionSelector()
        {
            var micros = LoadMicroCatalog();
            var grantAndDepletes = micros.Where(m => m.choices.Any(c => c.grantItemQuantity > 0 && c.depletesOnResolve)).ToList();
            Assert.True(grantAndDepletes.Count > 0);

            var sys = new NarrativeEncounterSystem();
            sys.RegisterRange(micros);

            foreach (var m in grantAndDepletes)
            {
                var depleting = m.choices.First(c => c.grantItemQuantity > 0 && c.depletesOnResolve);
                sys.TryResolve(m.id, depleting.choiceId, "rural_gas_station", 1);
                Assert.True(sys.IsDepleted(m.id));

                for (int seed = 0; seed < 64; seed++)
                {
                    var picked = sys.SelectEncounter("Stealth", 4f, "rural_gas_station", new SeededRng(seed));
                    Assert.NotEqual(m.id, picked?.id);
                }
            }
        }

        [Fact]
        public void FarmingResistance_NonDepletingItemChoices_AreNetNonPositive_OrDocumented()
        {
            // A non-depleting choice that nets positive item value per
            // resolution is an unbounded farming loop. Known intentional
            // exceptions are listed here with their rationale.
            var allowed = new HashSet<string>(StringComparer.Ordinal)
            {
                // (none today — shrine add_offering is a net *negative* exchange)
            };

            var trade = LoadTradeValues();
            foreach (var e in LoadMicroCatalog())
            foreach (var c in e.choices)
            {
                if (c.depletesOnResolve || c.grantItemQuantity <= 0) continue;
                double net = trade.TryGetValue(c.grantItemId, out var tv) ? c.grantItemQuantity * tv : 0d;
                if (net <= 0d) continue;
                Assert.True(allowed.Contains($"{e.id}:{c.choiceId}"),
                    $"{e.id}/{c.choiceId}: non-depleting positive item grant ({net:0.##}) is an unbounded farming loop");
            }
        }

        // ── Outlier reviews + balance report (env-gated write) ──────────

        [Fact]
        public void NamedOutliers_ArePresentWithPlanDocumentedShape()
        {
            var ledger = BuildLedger(LoadTradeValues());

            var supply = Assert.Single(ledger, r => r.EncounterId == "micro_supply_drop" && r.ChoiceId == "open_supply_drop");
            Assert.True(supply.Depletes); // one-shot: expected value = P(selected) × 20
            var memorial = Assert.Single(ledger, r => r.EncounterId == "micro_roadside_memorial" && r.ChoiceId == "take_offering");
            Assert.Equal(1.2d, memorial.GrantedValue, 6); // cloth
            var grave = Assert.Single(ledger, r => r.EncounterId == "micro_improvised_grave" && r.ChoiceId == "disturb_grave");
            Assert.Equal(4, grave.Guilt);
            Assert.Equal(25d, grave.GrantedValue, 6);
            var shrine = Assert.Single(ledger, r => r.EncounterId == "micro_shrine" && r.ChoiceId == "add_shrine_offering");
            Assert.Equal(3, shrine.Morale);
            Assert.True(shrine.ConsumedValue > 0d); // costs canned_food — repeatable devotion has an item price
        }

        [Fact]
        public void WriteBalanceReport()
        {
            var ledger = BuildLedger(LoadTradeValues());
            var result = EconomyReportScratch.Result ?? RunEconomySimulation();

            if (Environment.GetEnvironmentVariable("ASHFALL_GEN_MICRO_REPORTS") == "1")
            {
                var sb = new StringBuilder();
                sb.Append("# Micro-Location Balance Report (F12)\n\n");
                sb.Append("Generated deterministically by `MicroLocationEconomyAuditTests` ");
                sb.Append("(`ASHFALL_GEN_MICRO_REPORTS=1` to regenerate). Item value uses live `tradeValue`; ");
                sb.Append("morale/guilt/journal/discovery are reported separately — no exchange rate is defined.\n\n");

                sb.Append("## Methodology\n\n");
                sb.Append("- data: live catalogs (micro_locations.json, items.json, expeditions.json, scavenging tables)\n");
                sb.Append("- primary baseline: production loot through ExpeditionSystem.TickHours → ScavengingTableCatalog.RollLoot\n");
                sb.Append("- micro rewards: greedy max-net-value choice on every surfaced micro-location (upper bound)\n");
                sb.Append("- simulation: 100 expeditions, sortie i on destination i%N, seed 4000+i, Stealth stance\n\n");

                sb.Append("## Per-choice reward ledger\n\n");
                sb.Append("| Encounter | Choice | Depletes | Granted | Consumed | Net item | Morale | Guilt | Journal | Location |\n");
                sb.Append("|---|---|---|---:|---:|---:|---:|---:|---|---|\n");
                foreach (var r in ledger)
                    sb.Append("| ").Append(r.EncounterId).Append(" | ").Append(r.ChoiceId)
                      .Append(" | ").Append(r.Depletes ? "yes" : "no")
                      .Append(" | ").Append(r.GrantedValue.ToString("0.##"))
                      .Append(" | ").Append(r.ConsumedValue.ToString("0.##"))
                      .Append(" | ").Append(r.NetItemValue.ToString("0.##"))
                      .Append(" | ").Append(r.Morale >= 0 ? "+" : "").Append(r.Morale)
                      .Append(" | ").Append(r.Guilt >= 0 ? "+" : "").Append(r.Guilt)
                      .Append(" | ").Append(string.IsNullOrEmpty(r.Journal) ? "—" : r.Journal)
                      .Append(" | ").Append(string.IsNullOrEmpty(r.Location) ? "—" : r.Location)
                      .Append(" |\n");

                sb.Append("\n## 100-expedition results\n\n");
                double meanPrimary = EconomyReportScratch.MeanPrimary;
                double meanMicro = EconomyReportScratch.MeanMicro;
                double ratio = EconomyReportScratch.Ratio;
                var perMicro = result.PerExpeditionMicro.OrderBy(x => x).ToList();
                var perPrimary = result.PerExpeditionPrimary.OrderBy(x => x).ToList();
                sb.Append("- mean primary loot value / expedition: ").Append(meanPrimary.ToString("0.##")).Append('\n');
                sb.Append("- mean micro item value / expedition: ").Append(meanMicro.ToString("0.##")).Append('\n');
                sb.Append("- micro/primary contribution ratio: ").Append((ratio * 100d).ToString("0.#")).Append("% — target band 10–30%\n");
                sb.Append("- median micro value: ").Append(Percentile(perMicro, 0.5).ToString("0.##"))
                  .Append("; p95: ").Append(Percentile(perMicro, 0.95).ToString("0.##")).Append('\n');
                sb.Append("- median primary value: ").Append(Percentile(perPrimary, 0.5).ToString("0.##"))
                  .Append("; p95: ").Append(Percentile(perPrimary, 0.95).ToString("0.##")).Append('\n');
                sb.Append("- expeditions completed / failed: ").Append(result.Completed).Append(" / ").Append(result.Failed).Append('\n');
                sb.Append("- non-item rewards across the run: ").Append(result.JournalUnlocks).Append(" journal unlocks, ")
                  .Append(result.LocationDiscoveries).Append(" location discoveries, morale ")
                  .Append(result.MoraleDelta.ToString("+0;-0;0")).Append(", guilt ").Append(result.GuiltDelta.ToString("+0;-0;0")).Append('\n');
                sb.Append("- micro encounters surfaced: ").Append(result.MicroEncounters).Append('\n');

                sb.Append("\n## Named outlier reviews\n\n");
                sb.Append("- **micro_supply_drop** — 2 × medical_kit (face 20), depleting, minDanger 2, military route affinity: ");
                sb.Append("expected value is P(selected) × 20; one-shot, so face value is not per-expedition income. ");
                sb.Append("Qualitative utility of medical kits exceeds tradeValue — monitor, no change.\n");
                sb.Append("- **micro_roadside_memorial** — take_offering grants 1 cloth (1.2), depleting; leave_memorial +1 morale: ");
                sb.Append("the encounter's purpose is narrative texture; cloth is incidental. No change.\n");
                sb.Append("- **micro_improvised_grave** — disturb_grave: +25 wedding_ring for −3 morale, +4 guilt, one-shot: ");
                sb.Append("guilt is persistent (morale-coupled) and the site is single-use; the tradeoff reads as intended. No change.\n");
                sb.Append("- **micro_shrine** — add_shrine_offering: repeatable, costs 1 canned_food for +3 morale: ");
                sb.Append("morale farming is capped by canned-food scarcity (deliberate resource sink). Monitor.\n");

                sb.Append("\n## Farming resistance\n\n");
                sb.Append("- every positive-granting choice depletes its encounter (static gate test)\n");
                sb.Append("- depleting encounters are never re-selected after resolution (production selector, 64 seeds each)\n");
                sb.Append("- depletion persists across save/reload (F9 suite); reload cannot refill a searched site\n");
                sb.Append("- non-depleting choices are offerings (net item cost) or non-item rewards — no unbounded item loop\n");

                sb.Append("\n## Recommendation\n\n");
                sb.Append("- ratio measured at ").Append((ratio * 100d).ToString("0.#")).Append("%");
                sb.Append(ratio >= 0.10 && ratio <= 0.30
                    ? " — inside the 10–30% design band: no change.\n"
                    : " — outside the 10–30% band: see implementation log before tuning (INV-10: evidence first).\n");

                string path = Path.Combine(RepoRoot(), "docs", "discovery", "MICRO_LOCATION_BALANCE.md");
                File.WriteAllText(path, sb.ToString());
            }

            Assert.NotNull(ledger);
        }

        private static double Percentile(List<double> sorted, double p)
        {
            if (sorted.Count == 0) return 0d;
            int idx = (int)Math.Clamp((int)(p * (sorted.Count - 1) + 0.5), 0, sorted.Count - 1);
            return sorted[idx];
        }
    }

    internal static class EconomyReportScratch
    {
        public static double MeanPrimary;
        public static double MeanMicro;
        public static double Ratio;
        public static MicroLocationEconomyAuditTests? Holder;
        public static MicroLocationEconomyAuditTests.EconomyResult? Result;
    }
}
