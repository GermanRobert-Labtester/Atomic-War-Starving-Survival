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
    /// F11 flagship wave — content utilization audit for the authored
    /// micro-location catalog. Static reference validation, an
    /// eligibility-context matrix over live destinations, a reproducible
    /// 1000-opportunity utilization simulation with dead/orphan/too-common
    /// classification, and a redundancy scan. Findings classify; only broken
    /// references or zero-context entries fail. The utilization report is
    /// regenerated deterministically when ASHFALL_GEN_MICRO_REPORTS=1.
    ///
    /// Divergences (recorded in the implementation log): the catalog holds 28
    /// entries (plan assumed 25), and journalUnlockIds are free-form
    /// KnowledgeBase keys validated by micro_ namespace discipline (the
    /// runtime composes prose from the voice layer; no static catalog lists
    /// them).
    /// </summary>
    public class MicroLocationUtilizationAuditTests
    {
        private static readonly string[] ValidCategories = { "Discovery", "Hazard", "Social", "Trade" };

        private static string RepoRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "Ashfall.csproj")))
                dir = dir.Parent!;
            return dir!.FullName;
        }

        private static string DataDir() => Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data");

        private static List<EncounterDefinition> LoadMicroCatalog()
        {
            var defs = NarrativeEncounterCatalogLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            return defs.FindAll(e => e.id.StartsWith("micro_", StringComparison.Ordinal));
        }

        private static Dictionary<string, double> LoadTradeValues()
        {
            var doc = System.Text.Json.JsonDocument.Parse(
                File.ReadAllText(Path.Combine(DataDir(), "items.json")));
            var values = new Dictionary<string, double>(StringComparer.Ordinal);
            foreach (var el in doc.RootElement.GetProperty("items").EnumerateArray())
            {
                var id = el.GetProperty("id").GetString();
                if (string.IsNullOrEmpty(id)) continue;
                double tv = el.TryGetProperty("tradeValue", out var tvEl) && tvEl.ValueKind == System.Text.Json.JsonValueKind.Number
                    ? tvEl.GetDouble() : 0d;
                values[id!] = tv;
            }
            return values;
        }

        private static List<ExpeditionDefinition> LoadDestinations()
        {
            var loaded = ExpeditionCatalogLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.NotNull(loaded);
            Assert.NotEmpty(loaded!);
            return loaded!;
        }

        private static double EligibleWeight(EncounterDefinition e, ExpeditionDefinition d, string stance)
            => e.GetEffectiveWeight(stance, d.dangerLevel, d.id, d.lootCategories);

        private static bool Triggers(ExpeditionDefinition d, ISeededRng rng)
            => rng.NextDouble() < d.encounterChancePerTick * 0.5f; // Stealth parity with RollEncounter

        // ── Static validation ───────────────────────────────────────────

        [Fact]
        public void MicroLocationCatalog_UniqueIds_AndValidRequiredFields()
        {
            var micros = LoadMicroCatalog();
            Assert.Equal(28, micros.Count); // divergence D1: plan said 25; catalog grew

            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var e in micros)
            {
                Assert.True(seen.Add(e.id), $"duplicate micro-location id: {e.id}");
                Assert.False(string.IsNullOrWhiteSpace(e.title), $"{e.id}: empty title");
                Assert.False(string.IsNullOrWhiteSpace(e.description), $"{e.id}: empty description");
                Assert.Contains(e.category, ValidCategories);
                Assert.True(float.IsFinite(e.baseWeight) && e.baseWeight >= 0f, $"{e.id}: bad baseWeight");
                Assert.True(float.IsFinite(e.minDangerLevel) && e.minDangerLevel >= 0f, $"{e.id}: bad minDangerLevel");
                Assert.NotEmpty(e.choices);
                var choiceIds = new HashSet<string>(StringComparer.Ordinal);
                foreach (var c in e.choices)
                {
                    Assert.False(string.IsNullOrWhiteSpace(c.choiceId), $"{e.id}: empty choiceId");
                    Assert.True(choiceIds.Add(c.choiceId), $"{e.id}: duplicate choiceId {c.choiceId}");
                    Assert.False(string.IsNullOrWhiteSpace(c.text), $"{e.id}/{c.choiceId}: empty text");
                    if (c.grantItemQuantity > 0) Assert.False(string.IsNullOrEmpty(c.grantItemId), $"{e.id}/{c.choiceId}: grant without item id");
                    if (c.grantItemQuantity < 0) Assert.False(string.IsNullOrEmpty(c.grantItemId), $"{e.id}/{c.choiceId}: offering without item id");
                }
                if (!string.IsNullOrEmpty(e.requiredLocationId))
                    Assert.False(string.IsNullOrWhiteSpace(e.requiredLocationId), $"{e.id}: whitespace requiredLocationId");
            }
        }

        [Fact]
        public void AllItemReferences_ResolveAgainstItemsCatalog()
        {
            var trade = LoadTradeValues();
            foreach (var e in LoadMicroCatalog())
            {
                foreach (var c in e.choices)
                {
                    if (!string.IsNullOrEmpty(c.grantItemId))
                    {
                        Assert.True(trade.ContainsKey(c.grantItemId),
                            $"{e.id}/{c.choiceId}: grantItemId '{c.grantItemId}' does not resolve");
                        Assert.True(trade[c.grantItemId] >= 0d, $"{e.id}/{c.choiceId}: negative trade value");
                    }
                    if (!string.IsNullOrEmpty(c.requiredItemId))
                    {
                        Assert.True(trade.ContainsKey(c.requiredItemId),
                            $"{e.id}/{c.choiceId}: requiredItemId '{c.requiredItemId}' does not resolve");
                        Assert.True(c.requiredItemQuantity > 0, $"{e.id}/{c.choiceId}: requiredItemQuantity must be positive");
                    }
                }
            }
        }

        [Fact]
        public void AllDiscoveryAndRequiredLocations_ResolveAndAreStructurallyReachable()
        {
            var destinations = LoadDestinations();
            var byId = destinations.ToDictionary(d => d.id, StringComparer.Ordinal);

            foreach (var e in LoadMicroCatalog())
            {
                foreach (var c in e.choices)
                {
                    if (!string.IsNullOrEmpty(c.discoverLocationId))
                        Assert.True(byId.ContainsKey(c.discoverLocationId),
                            $"{e.id}/{c.choiceId}: discoverLocationId '{c.discoverLocationId}' is not an expedition destination");
                }

                if (string.IsNullOrEmpty(e.requiredLocationId)) continue;
                Assert.True(byId.TryGetValue(e.requiredLocationId, out var host),
                    $"{e.id}: requiredLocationId '{e.requiredLocationId}' is not an expedition destination");
                // Structural reachability: the host destination's authored danger
                // must satisfy the entry's danger gate at least at base stance.
                Assert.True(host!.dangerLevel >= e.minDangerLevel,
                    $"{e.id}: structurally unreachable — host '{host.id}' danger {host.dangerLevel} < minDangerLevel {e.minDangerLevel}");
            }
        }

        [Fact]
        public void JournalUnlockKeys_StayInMicroNamespace_AndAreUniquePerEncounter()
        {
            foreach (var e in LoadMicroCatalog())
            {
                var keys = new HashSet<string>(StringComparer.Ordinal);
                foreach (var c in e.choices)
                {
                    if (string.IsNullOrEmpty(c.journalUnlockId)) continue;
                    Assert.True(c.journalUnlockId.StartsWith("micro_", StringComparison.Ordinal),
                        $"{e.id}/{c.choiceId}: journal key outside micro_ namespace (free-form KnowledgeBase key — see divergence note)");
                    Assert.True(keys.Add(c.journalUnlockId), $"{e.id}: duplicate journal key {c.journalUnlockId}");
                }
            }
        }

        // ── Eligibility-context matrix ──────────────────────────────────

        [Fact]
        public void EveryMicroLocation_HasAtLeastOneEligibleContext()
        {
            var destinations = LoadDestinations();
            var micros = LoadMicroCatalog();
            var report = new StringBuilder();
            int contextsTotal = 0;

            foreach (var e in micros)
            {
                int contexts = 0;
                string firstContext = string.Empty;
                foreach (var d in destinations)
                {
                    if (EligibleWeight(e, d, "Stealth") > 0f || EligibleWeight(e, d, "Speed") > 0f)
                    {
                        contexts++;
                        if (contexts == 1) firstContext = d.id;
                    }
                }
                contextsTotal += contexts;
                report.Append("| ").Append(e.id).Append(" | ").Append(contexts).Append(" | ").Append(firstContext).Append(" |\n");
                Assert.True(contexts > 0,
                    $"DEAD entry {e.id}: no destination/stance context makes it eligible (weight zero everywhere)");
            }

            UtilizationReportScratch.EligibilityMatrix = report.ToString();
            Assert.True(contextsTotal >= micros.Count, "sanity: every entry should hold at least one context");
        }

        // ── 1000-opportunity utilization simulation ─────────────────────

        private sealed class UtilizationStats
        {
            public Dictionary<string, int> Eligible = new Dictionary<string, int>(StringComparer.Ordinal);
            public Dictionary<string, int> Selected = new Dictionary<string, int>(StringComparer.Ordinal);
            public int Opportunities;
            public int Triggered;
            public int NoSelection;

            public string Canonical()
            {
                var sb = new StringBuilder();
                sb.Append("opps=").Append(Opportunities).Append(";triggered=").Append(Triggered).Append(";none=").Append(NoSelection);
                foreach (var k in Eligible.Keys.OrderBy(k => k, StringComparer.Ordinal))
                    sb.Append(';').Append(k).Append(":e=").Append(Eligible[k]).Append(",s=").Append(Selected.GetValueOrDefault(k));
                return sb.ToString();
            }
        }

        private static UtilizationStats RunUtilizationSimulation()
        {
            var destinations = LoadDestinations();
            var micros = LoadMicroCatalog();

            // One persistent campaign system: depleting entries exhaust and
            // leave the pool exactly as in a real campaign (§9.3).
            var sys = new NarrativeEncounterSystem();
            sys.RegisterRange(micros);

            var stats = new UtilizationStats();
            foreach (var m in micros) { stats.Eligible[m.id] = 0; stats.Selected[m.id] = 0; }

            const int opportunities = 1000;
            for (int i = 0; i < opportunities; i++)
            {
                var d = destinations[i % destinations.Count];
                stats.Opportunities++;
                var rng = new SeededRng(9000 + i);

                // Eligibility bookkeeping consumes no RNG (metadata only).
                foreach (var m in micros)
                    if (m.GetEffectiveWeight("Stealth", d.dangerLevel, d.id, d.lootCategories) > 0f)
                        stats.Eligible[m.id]++;

                if (!Triggers(d, rng)) continue;
                stats.Triggered++;
                var picked = sys.SelectEncounter("Stealth", d.dangerLevel, d.id, rng, d.lootCategories);
                if (picked == null) { stats.NoSelection++; continue; }
                stats.Selected[picked.id] = stats.Selected.TryGetValue(picked.id, out var n) ? n + 1 : 1;
            }
            return stats;
        }

        [Fact]
        public void UtilizationSimulation_1000Opportunities_IsReproducible_AndClassifies()
        {
            var a = RunUtilizationSimulation();
            var b = RunUtilizationSimulation();
            Assert.Equal(a.Canonical(), b.Canonical()); // deterministic evidence

            Assert.Equal(1000, a.Opportunities);
            Assert.True(a.Triggered > 0, "expected at least some triggered opportunities");

            var micros = LoadMicroCatalog();
            var destinations = LoadDestinations();
            var rows = new List<string>();
            var findings = new List<string>();

            foreach (var m in micros)
            {
                int eligible = a.Eligible.GetValueOrDefault(m.id);
                int selected = a.Selected.GetValueOrDefault(m.id);
                double rateOverall = selected / (double)a.Opportunities;
                double rateEligible = eligible > 0 ? selected / (double)eligible : 0d;

                string status = "OK";
                if (eligible == 0)
                {
                    bool orphan = !string.IsNullOrEmpty(m.requiredLocationId);
                    status = orphan ? "ORPHAN_REQUIRED_LOCATION" : "DEAD_NO_ELIGIBLE_CONTEXT";
                }
                else if (selected == 0)
                {
                    // Route-locked or heavily gated entries may miss a 1000-slot
                    // sample by luck; compute expected selections.
                    double expected = ExpectedSelections(m, destinations, a);
                    status = expected < 1.0 ? "ELIGIBLE_BUT_NOT_SELECTED_IN_SAMPLE" : "SUSPECT_LOW_YIELD";
                }
                else if (selected > 100)
                {
                    status = "REVIEW_TOO_COMMON";
                }

                rows.Add($"| {m.id} | {m.category} | {m.baseWeight:0.##} | {m.minDangerLevel:0.#} | {(string.IsNullOrEmpty(m.requiredLocationId) ? "—" : m.requiredLocationId)} | {eligible} | {selected} | {rateOverall:P1} | {rateEligible:P1} | {status} |");
                if (status != "OK") findings.Add($"{m.id}: {status}");
            }

            UtilizationReportScratch.Rows = rows;
            UtilizationReportScratch.Findings = findings;
            UtilizationReportScratch.StatsCanonical = a.Canonical();
            WriteUtilizationReport(a);

            // Only structurally broken content fails the audit; sample luck and
            // frequency outliers are reported findings, not failures (INV-10).
            Assert.DoesNotContain(findings, f => f.Contains("DEAD_NO_ELIGIBLE_CONTEXT"));
            Assert.DoesNotContain(findings, f => f.Contains("ORPHAN_REQUIRED_LOCATION"));
        }

        private static double ExpectedSelections(EncounterDefinition m, List<ExpeditionDefinition> destinations, UtilizationStats stats)
        {
            // Expected selections ≈ Σ over sample slots on matching destinations
            // of P(trigger) × weight-share at that context (empty depletion —
            // the entry cannot deplete if it never got selected).
            double expected = 0d;
            int slots = stats.Opportunities / destinations.Count;
            foreach (var d in destinations)
            {
                float w = m.GetEffectiveWeight("Stealth", d.dangerLevel, d.id, d.lootCategories);
                if (w <= 0f) continue;
                double total = 0d;
                foreach (var other in LoadMicroCatalog())
                    total += other.GetEffectiveWeight("Stealth", d.dangerLevel, d.id, d.lootCategories);
                if (total <= 0d) continue;
                expected += slots * (d.encounterChancePerTick * 0.5f) * (w / total);
            }
            return expected;
        }

        // ── Redundancy scan (editorial review, not failure) ─────────────

        [Fact]
        public void RedundancyScan_ProducesReviewedCandidatePairs()
        {
            var micros = LoadMicroCatalog();
            var pairs = new List<string>();

            static HashSet<string> Tokens(string s)
            {
                var set = new HashSet<string>(StringComparer.Ordinal);
                foreach (var t in s.ToLowerInvariant().Split(new[] { ' ', ',', '.', ';', ':', '—', '-' }, StringSplitOptions.RemoveEmptyEntries))
                    if (t.Length > 3) set.Add(t);
                return set;
            }

            static double Jaccard(HashSet<string> a, HashSet<string> b)
            {
                if (a.Count == 0 || b.Count == 0) return 0d;
                int inter = a.Count(t => b.Contains(t));
                return inter / (double)(a.Count + b.Count - inter);
            }

            for (int i = 0; i < micros.Count; i++)
            {
                for (int j = i + 1; j < micros.Count; j++)
                {
                    var a = micros[i];
                    var b = micros[j];
                    if (a.category != b.category) continue;

                    var grantsA = a.choices.Where(c => c.grantItemQuantity > 0).Select(c => c.grantItemId).ToHashSet(StringComparer.Ordinal);
                    var grantsB = b.choices.Where(c => c.grantItemQuantity > 0).Select(c => c.grantItemId).ToHashSet(StringComparer.Ordinal);
                    bool sameReward = grantsA.Count > 0 && grantsA.SetEquals(grantsB);
                    double sim = Jaccard(Tokens(a.description), Tokens(b.description));
                    if (sameReward && sim >= 0.5)
                        pairs.Add($"{a.id} ↔ {b.id}: shared category+reward, description similarity {sim:0.00} → needs narrative differentiation");
                    else if (sim >= 0.6)
                        pairs.Add($"{a.id} ↔ {b.id}: description similarity {sim:0.00} → review for thematic echo");
                }
            }

            UtilizationReportScratch.RedundancyPairs = pairs;
            Assert.True(pairs.Count >= 0); // scan always runs; pairs are editorial findings
        }

        // ── Report generation (env-gated; inert in normal test runs) ────

        private static void WriteUtilizationReport(UtilizationStats stats)
        {
            if (Environment.GetEnvironmentVariable("ASHFALL_GEN_MICRO_REPORTS") != "1") return;

            var sb = new StringBuilder();
            sb.Append("# Micro-Location Utilization Report (F11)\n\n");
            sb.Append("Generated deterministically by `MicroLocationUtilizationAuditTests` ");
            sb.Append("(set `ASHFALL_GEN_MICRO_REPORTS=1` to regenerate). Values come from the live catalogs.\n\n");
            sb.Append("## Audit configuration\n\n");
            sb.Append("- catalog: micro_locations.json (28 entries)\n");
            sb.Append("- simulation: 1000 encounter opportunities, one persistent campaign system (depletion accumulates)\n");
            sb.Append("- seeds: opportunity i uses SeededRng(9000+i); destinations cycle the authored expedition catalog\n");
            sb.Append("- stance: Stealth (encounter chance ×0.5, parity with ExpeditionSystem.RollEncounter)\n\n");
            sb.Append("## Eligibility-context matrix\n\n");
            sb.Append("| Entry | Eligible contexts | First context |\n|---|---:|---|\n");
            sb.Append(UtilizationReportScratch.EligibilityMatrix).Append('\n');
            sb.Append("## 1000-opportunity utilization\n\n");
            sb.Append("| Entry | Category | Weight | Min danger | Required location | Eligible opportunities | Selected | Overall rate | Eligible rate | Status |\n");
            sb.Append("|---|---|---:|---:|---|---:|---:|---:|---:|---|\n");
            foreach (var row in UtilizationReportScratch.Rows) sb.Append(row).Append('\n');
            sb.Append("\n## Findings\n\n");
            if (UtilizationReportScratch.Findings.Count == 0) sb.Append("- none — every entry is reachable and selected within expected bounds\n");
            else foreach (var f in UtilizationReportScratch.Findings) sb.Append("- ").Append(f).Append('\n');
            sb.Append("\n## Redundancy review pairs\n\n");
            if (UtilizationReportScratch.RedundancyPairs.Count == 0) sb.Append("- none above threshold\n");
            else foreach (var p in UtilizationReportScratch.RedundancyPairs) sb.Append("- ").Append(p).Append('\n');
            sb.Append("\n## Simulation canonical trace\n\n```\n").Append(stats.Canonical()).Append("\n```\n");

            string path = Path.Combine(RepoRoot(), "docs", "discovery", "MICRO_LOCATION_UTILIZATION.md");
            File.WriteAllText(path, sb.ToString());
        }
    }

    /// <summary>Scratch channel between audit tests and the report writer
    /// within one test-class run (xUnit runs tests in one class serially).</summary>
    internal static class UtilizationReportScratch
    {
        public static string EligibilityMatrix = string.Empty;
        public static List<string> Rows = new List<string>();
        public static List<string> Findings = new List<string>();
        public static List<string> RedundancyPairs = new List<string>();
        public static string StatsCanonical = string.Empty;
    }
}
