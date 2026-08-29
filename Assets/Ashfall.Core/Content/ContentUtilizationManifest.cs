// SPDX-License-Identifier: MIT
// ASHFALL Core: Content Utilization Manifest Generator
//
// Generates deterministic machine-readable utilization manifest
// and human-readable report from the content graph.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Ashfall.Core.Content
{
    public static class ContentUtilizationManifest
    {
        public const string DefaultManifestPath = "artifacts/content-utilization.json";
        public const string DefaultReportPath = "artifacts/content-utilization.md";

        /// <summary>
        /// Generate the machine-readable manifest from a utilization graph.
        /// </summary>
        public static string GenerateManifest(ContentUtilizationGraph graph, string commitHash = "", IWallClock? wallClock = null)
        {
            wallClock ??= SystemWallClock.Instance;
            graph.GeneratedFromCommit = commitHash;
            graph.GeneratedAt = wallClock.FormatIsoUtc();
            graph.Stabilize();
            graph.ComputeSummaries();

            var options = new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
            };

            return System.Text.Json.JsonSerializer.Serialize(graph, options);
        }

        /// <summary>
        /// Write the manifest to disk.
        /// </summary>
        public static void WriteManifest(ContentUtilizationGraph graph, string path, string commitHash = "", IWallClock? wallClock = null)
        {
            string json = GenerateManifest(graph, commitHash, wallClock);
            string dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            File.WriteAllText(path, json);
        }

        /// <summary>
        /// Generate a human-readable report from the utilization graph.
        /// </summary>
        public static string GenerateReport(ContentUtilizationGraph graph, IWallClock? wallClock = null)
        {
            wallClock ??= SystemWallClock.Instance;
            var sb = new StringBuilder();
            sb.AppendLine("# ASHFALL Content Utilization Report");
            sb.AppendLine();
            sb.AppendLine($"Generated: {wallClock.FormatIsoUtc()}");
            if (!string.IsNullOrEmpty(graph.GeneratedFromCommit))
                sb.AppendLine($"Commit: {graph.GeneratedFromCommit}");
            sb.AppendLine($"Schema: {graph.SchemaVersion}");
            sb.AppendLine();

            // ── Summary ──
            sb.AppendLine("## Summary");
            sb.AppendLine();
            sb.AppendLine("| Metric | Count |");
            sb.AppendLine("|--------|-------|");
            sb.AppendLine($"| Total Catalogs | {graph.TotalCatalogs} |");
            sb.AppendLine($"| Total Definitions | {graph.TotalDefinitions} |");
            sb.AppendLine($"| Gameplay-Consumed | {graph.GameplayConsumedCatalogs} |");
            sb.AppendLine($"| UI-Only | {graph.UiOnlyCatalogs} |");
            sb.AppendLine($"| Codex-Only | {graph.CodexOnlyCatalogs} |");
            sb.AppendLine($"| Optional | {graph.OptionalCatalogs} |");
            sb.AppendLine($"| Test-Only | {graph.TestOnlyCatalogs} |");
            sb.AppendLine($"| Orphaned | {graph.OrphanedCatalogs} |");
            sb.AppendLine($"| Unresolved | {graph.UnresolvedCatalogs} |");
            sb.AppendLine($"| Exempted | {graph.ExemptedCatalogs} |");
            sb.AppendLine();

            // ── Content Family Summary ──
            sb.AppendLine("## Content Family Summary");
            sb.AppendLine();
            sb.AppendLine("| Family | Catalogs | Gameplay | UI | Codex | Optional | Test | Orphaned | Unresolved |");
            sb.AppendLine("|--------|----------|----------|-----|-------|----------|------|----------|------------|");
            foreach (var family in graph.FamilySummaries)
            {
                sb.AppendLine($"| {family.Family} | {family.Catalogs} | {family.GameplayConsumed} | {family.UiOnly} | {family.CodexOnly} | {family.Optional} | {family.TestOnly} | {family.Orphaned} | {family.Unresolved} |");
            }
            sb.AppendLine();

            // ── Orphaned Catalogs ──
            var orphaned = graph.Catalogs.Where(c => c.Classification == ContentClassification.ORPHANED).ToList();
            if (orphaned.Count > 0)
            {
                sb.AppendLine("## ⚠ Orphaned Catalogs");
                sb.AppendLine();
                sb.AppendLine("| Catalog | Last Stage | Loader | Findings |");
                sb.AppendLine("|---------|------------|--------|----------|");
                foreach (var cat in orphaned)
                {
                    sb.AppendLine($"| {cat.Path} | {cat.MaxStage} | {cat.Loader} | {string.Join("; ", cat.Findings)} |");
                }
                sb.AppendLine();
            }

            // ── Loaded But Not Queried ──
            var loadedNotQueried = graph.Catalogs
                .Where(c => c.MaxStage >= UtilizationStage.LOADED && c.MaxStage < UtilizationStage.QUERIED
                    && c.Classification != ContentClassification.CODEX_ONLY
                    && c.Classification != ContentClassification.OPTIONAL)
                .ToList();
            if (loadedNotQueried.Count > 0)
            {
                sb.AppendLine("## ⚠ Loaded But Not Queried");
                sb.AppendLine();
                sb.AppendLine("| Catalog | Stage | Loader |");
                sb.AppendLine("|---------|-------|--------|");
                foreach (var cat in loadedNotQueried)
                {
                    sb.AppendLine($"| {cat.Path} | {cat.MaxStage} | {cat.Loader} |");
                }
                sb.AppendLine();
            }

            // ── Disconnects ──
            if (graph.Disconnects.Count > 0)
            {
                sb.AppendLine("## ⚠ Disconnected Content");
                sb.AppendLine();
                sb.AppendLine("| Catalog | Category | Last Stage | Missing Link |");
                sb.AppendLine("|---------|----------|------------|-------------|");
                foreach (var dc in graph.Disconnects)
                {
                    sb.AppendLine($"| {dc.Catalog} | {dc.Category} | {dc.LastStage} | {dc.MissingLink} |");
                }
                sb.AppendLine();
            }

            // ── Hardcoded Authorities ──
            if (graph.HardcodedAuthorities.Count > 0)
            {
                sb.AppendLine("## ⚠ Hardcoded Runtime Authorities");
                sb.AppendLine();
                sb.AppendLine("| System | Hardcoded Source | Available Catalog | Uses JSON? | Recommendation |");
                sb.AppendLine("|--------|-----------------|-------------------|------------|----------------|");
                foreach (var hc in graph.HardcodedAuthorities)
                {
                    sb.AppendLine($"| {hc.RuntimeSystem} | {hc.HardcodedSource} | {hc.AvailableCatalog} | {(hc.RuntimeUsesJson ? "Yes" : "No")} | {hc.RecommendedStatus} |");
                }
                sb.AppendLine();
            }

            // ── Reachability Findings ──
            if (graph.ReachabilityFindings.Count > 0)
            {
                var broken = graph.ReachabilityFindings.Where(r => r.Status == ReachabilityStatus.BROKEN_GATE).ToList();
                if (broken.Count > 0)
                {
                    sb.AppendLine("## ⚠ Broken Gate Chains");
                    sb.AppendLine();
                    sb.AppendLine("| Definition | Status | Broken Chain |");
                    sb.AppendLine("|------------|--------|-------------|");
                    foreach (var rf in broken)
                    {
                        sb.AppendLine($"| {rf.DefinitionId} | {rf.Status} | {rf.BrokenChain} |");
                    }
                    sb.AppendLine();
                }
            }

            // ── Evidence Breakdown ──
            sb.AppendLine("## Evidence Breakdown");
            sb.AppendLine();
            var byEvidence = graph.Catalogs.GroupBy(c => c.BestEvidence).ToDictionary(g => g.Key, g => g.Count());
            sb.AppendLine("| Evidence Tier | Catalog Count |");
            sb.AppendLine("|---------------|---------------|");
            foreach (EvidenceTier tier in Enum.GetValues(typeof(EvidenceTier)))
            {
                byEvidence.TryGetValue(tier, out int count);
                sb.AppendLine($"| {tier} | {count} |");
            }
            sb.AppendLine();

            // ── Stage Breakdown ──
            sb.AppendLine("## Stage Breakdown");
            sb.AppendLine();
            var byStage = graph.Catalogs.GroupBy(c => c.MaxStage).ToDictionary(g => g.Key, g => g.Count());
            sb.AppendLine("| Stage | Catalog Count |");
            sb.AppendLine("|-------|---------------|");
            foreach (UtilizationStage stage in Enum.GetValues(typeof(UtilizationStage)))
            {
                byStage.TryGetValue(stage, out int count);
                sb.AppendLine($"| {stage} | {count} |");
            }
            sb.AppendLine();

            // ── Actionable Priorities ──
            sb.AppendLine("## Actionable Priorities");
            sb.AppendLine();
            sb.AppendLine("1. **Required Orphaned Catalogs**: " + orphaned.Count(c => !string.IsNullOrEmpty(c.ExemptionId)));
            sb.AppendLine("2. **Loaded-Not-Queried Catalogs**: " + loadedNotQueried.Count);
            sb.AppendLine("3. **Broken Gate Chains**: " + graph.ReachabilityFindings.Count(r => r.Status == ReachabilityStatus.BROKEN_GATE));
            sb.AppendLine("4. **Hardcoded Authorities Bypassing JSON**: " + graph.HardcodedAuthorities.Count);
            sb.AppendLine("5. **Missing Tests**: " + graph.Catalogs.Count(c => c.Findings.Any(f => f.Contains("test"))));
            sb.AppendLine();

            return sb.ToString();
        }

        /// <summary>
        /// Write the human-readable report to disk.
        /// </summary>
        public static void WriteReport(ContentUtilizationGraph graph, string path, IWallClock? wallClock = null)
        {
            string report = GenerateReport(graph, wallClock);
            string dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            File.WriteAllText(path, report);
        }
    }
}