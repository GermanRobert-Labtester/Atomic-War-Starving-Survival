// SPDX-License-Identifier: MIT
// ASHFALL: Content Utilization Self-Test
//
// Deterministic diagnostic mode that exercises representative runtime
// content wiring and generates the utilization manifest.
// Run via: godot --headless --path . -- --content-utilization-selftest

using Godot;
using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Content;

namespace AtomicWar.GodotApp
{
    public static class ContentUtilizationSelfTest
    {
        public static int Run(string repoRoot, string dataDir, string coreDir, string srcDir)
        {
            GD.Print("=== Content Utilization Self-Test ===");
            GD.Print($"Data directory: {dataDir}");
            GD.Print($"Core directory: {coreDir}");
            GD.Print();

            int exitCode = 0;
            ContentUtilizationGraph? graph = null;

            try
            {
                // Phase 1–3: Static scan
                GD.Print("[Phase 1–3] Static content inventory...");
                var scanner = new ContentUtilizationScanner(repoRoot, dataDir, coreDir, srcDir, new GodotLog());
                graph = scanner.Scan();
                GD.Print($"  Discovered {graph.TotalCatalogs} catalogs");
                GD.Print($"  Gameplay-consumed: {graph.GameplayConsumedCatalogs}");
                GD.Print($"  Codex-only: {graph.CodexOnlyCatalogs}");
                GD.Print($"  Orphaned: {graph.OrphanedCatalogs}");
                GD.Print($"  Unresolved: {graph.UnresolvedCatalogs}");
                GD.Print();

                // Phase 4: Runtime instrumentation
                GD.Print("[Phase 4] Runtime instrumentation...");
                var instrumentation = new ContentUtilizationInstrumentation();
                GD.Print("  Collecting runtime evidence from deterministic campaign...");
                instrumentation = ContentUtilizationRuntimeCollector.Collect(dataDir);
                instrumentation.MergeInto(graph);
                graph.ComputeSummaries();
                GD.Print($"  Runtime events collected: {instrumentation.EventCount}");
                GD.Print($"  Queried catalogs: {instrumentation.QueriedCatalogs.Count}");
                GD.Print($"  Queried definitions: {instrumentation.QueriedDefinitions.Count}");
                GD.Print();

                // Phase 7: Generate manifest
                GD.Print("[Phase 7] Generating utilization manifest...");
                string manifestPath = Path.Combine(repoRoot, "artifacts", "content-utilization.json");
                string commitHash = TryGetCurrentCommitHash(repoRoot);
                ContentUtilizationManifest.WriteManifest(graph, manifestPath, commitHash);
                GD.Print($"  Manifest written to: {manifestPath}");
                GD.Print(string.IsNullOrEmpty(commitHash)
                    ? "  Commit provenance: unavailable (not a git checkout or git not on PATH)"
                    : $"  Commit provenance: {commitHash}");
                GD.Print();

                // Phase 7: Generate human report
                GD.Print("[Phase 7] Generating human-readable report...");
                string reportPath = Path.Combine(repoRoot, "artifacts", "content-utilization.md");
                ContentUtilizationManifest.WriteReport(graph, reportPath);
                GD.Print($"  Report written to: {reportPath}");
                GD.Print();

                // Phase 10: CI gate
                GD.Print("[Phase 10] CI Gate...");
                var baseline = ContentUtilizationGate.LoadBaseline();
                var gateResult = ContentUtilizationGate.Run(graph, baseline);

                if (baseline == null)
                {
                    // First run — create baseline
                    GD.Print("  No baseline found — creating initial baseline.");
                    var newBaseline = ContentUtilizationGate.CreateBaseline(graph);
                    ContentUtilizationGate.SaveBaseline(newBaseline);
                    GD.Print("  Baseline saved.");
                    GD.Print("  CI gate: PASS (first run)");
                }
                else
                {
                    GD.Print(ContentUtilizationGate.FormatReport(gateResult));
                    if (!gateResult.Passed)
                    {
                        GD.PrintErr("  CI gate: FAIL");
                        exitCode = 1;
                    }
                    else
                    {
                        GD.Print("  CI gate: PASS");
                    }
                }
                GD.Print();

                // Phase 11: Deep-chain gate (Plan 49)
                GD.Print("[Phase 11] Deep-chain gate (flagship content chains)...");
                var deepReport = ContentDeepChainGate.Evaluate(graph);
                string deepJsonPath = Path.Combine(repoRoot, "artifacts", "content-utilization-deep-chain.json");
                WriteDeepChainArtifact(deepJsonPath, deepReport);
                GD.Print($"  Chains evaluated: {deepReport.ChainsEvaluated}");
                GD.Print($"  Hard failures: {deepReport.HardFailures}");
                GD.Print($"  Warnings: {deepReport.Warnings}");
                foreach (var finding in deepReport.Findings)
                {
                    string line = $"    [{finding.Severity}] {finding.ChainId}/{finding.HopId}: {finding.MissingCategory} — {finding.Details}";
                    if (finding.Severity == "HARD") GD.PrintErr(line);
                    else GD.Print(line);
                }
                GD.Print($"  Deep-chain artifact: {deepJsonPath}");
                if (!deepReport.HardGatePassed)
                {
                    GD.PrintErr("  Deep-chain gate: FAIL");
                    exitCode = 1;
                }
                else
                {
                    GD.Print("  Deep-chain gate: PASS");
                }
                GD.Print();

                // Phase 9: Exemption + content-reachability disposition check
                GD.Print("[Phase 9] Exemption + content-reachability validation...");
                var exemptions = DefaultExemptions.CreateDefault();

                // T086: every UNRESOLVED catalog must carry a reviewed, owned
                // disposition. The policy is a data file so the disposition set
                // is reviewable and diffable rather than implied by silence.
                string policyPath = Path.Combine(repoRoot, "docs", "ci", "content_reachability_dispositions.json");
                if (File.Exists(policyPath))
                {
                    try
                    {
                        var policy = new SystemTextJsonSerializer().Deserialize<ExemptionRegistry>(File.ReadAllText(policyPath));
                        if (policy != null)
                        {
                            foreach (var entry in policy.Exemptions)
                            {
                                if (!exemptions.TryGetExemption(entry.ContentPath, out _))
                                    exemptions.Exemptions.Add(entry);
                            }
                            GD.Print($"  Disposition policy: {policy.Exemptions.Count} entries");

                            var noExpiry = policy.Exemptions
                                .Where(e => string.IsNullOrWhiteSpace(e.ExpiryCondition))
                                .Select(e => e.ContentPath)
                                .OrderBy(p => p, StringComparer.Ordinal)
                                .ToList();
                            GD.Print($"  Dispositions without an expiry: {noExpiry.Count}");
                            if (noExpiry.Count > 0)
                            {
                                foreach (var path in noExpiry)
                                    GD.PrintErr($"    NO_EXPIRY: {path}");
                                exitCode = 1;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        GD.PrintErr($"  Disposition policy failed to parse: {ex.Message}");
                        exitCode = 1;
                    }

                    var undispositioned = graph.Catalogs
                        .Where(c => c.Classification == ContentClassification.UNRESOLVED)
                        .Where(c => !exemptions.TryGetExemption(c.Path, out _))
                        .Select(c => c.Path)
                        .OrderBy(p => p, StringComparer.Ordinal)
                        .ToList();
                    GD.Print($"  Unresolved without a disposition: {undispositioned.Count}");
                    if (undispositioned.Count > 0)
                    {
                        foreach (var path in undispositioned)
                            GD.PrintErr($"    UNDISPOSITIONED: {path}");
                        exitCode = 1;
                    }
                }
                else
                {
                    GD.PrintErr($"  Disposition policy missing: {policyPath}");
                    exitCode = 1;
                }

                var invalid = exemptions.GetInvalidExemptions();
                var stale = exemptions.GetStaleExemptions(graph);
                GD.Print($"  Total exemptions: {exemptions.Exemptions.Count}");
                GD.Print($"  Invalid: {invalid.Count}");
                GD.Print($"  Stale: {stale.Count}");
                if (invalid.Count > 0)
                {
                    foreach (var inv in invalid)
                        GD.PrintErr($"    INVALID: {inv.ExemptionId} — missing required fields");
                    exitCode = 1;
                }
                if (stale.Count > 0)
                {
                    foreach (var s in stale)
                        GD.Print($"    STALE: {s.ExemptionId} — references missing content");
                }
                GD.Print();

                // Summary
                GD.Print("=== Content Utilization Summary ===");
                GD.Print($"  Total catalogs:    {graph.TotalCatalogs}");
                GD.Print($"  Gameplay-consumed: {graph.GameplayConsumedCatalogs}");
                GD.Print($"  UI-only:           {graph.UiOnlyCatalogs}");
                GD.Print($"  Codex-only:        {graph.CodexOnlyCatalogs}");
                GD.Print($"  Optional:          {graph.OptionalCatalogs}");
                GD.Print($"  Test-only:         {graph.TestOnlyCatalogs}");
                GD.Print($"  Orphaned:          {graph.OrphanedCatalogs}");
                GD.Print($"  Unresolved:        {graph.UnresolvedCatalogs}");
                GD.Print($"  Exempted:          {graph.ExemptedCatalogs}");

                if (graph.OrphanedCatalogs > 0)
                {
                    GD.Print();
                    GD.Print("  Orphaned catalogs:");
                    foreach (var cat in graph.Catalogs)
                    {
                        if (cat.Classification == ContentClassification.ORPHANED)
                        {
                            GD.Print($"    - {cat.Path} (loader: {cat.Loader})");
                        }
                    }
                }

                GD.Print();
                GD.Print("=== Content Utilization Self-Test Complete ===");
                return exitCode;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Content utilization self-test failed: {ex.Message}");
                GD.PrintErr(ex.StackTrace);
                return 1;
            }
        }

        /// <summary>Deterministic deep-chain artifact (ordinal ordering).</summary>
        private static void WriteDeepChainArtifact(string path, DeepChainReport report)
        {
            var lines = new System.Text.StringBuilder();
            lines.AppendLine("{");
            lines.AppendLine($"  \"schemaVersion\": \"{report.SchemaVersion}\",");
            lines.AppendLine($"  \"chainsEvaluated\": {report.ChainsEvaluated},");
            lines.AppendLine($"  \"hardFailures\": {report.HardFailures},");
            lines.AppendLine($"  \"warnings\": {report.Warnings},");
            lines.AppendLine("  \"findings\": [");
            var f = report.Findings;
            for (int i = 0; i < f.Count; i++)
            {
                string comma = i < f.Count - 1 ? "," : string.Empty;
                lines.AppendLine($"    {{ \"chain\": \"{Escape(f[i].ChainId)}\", \"hop\": \"{Escape(f[i].HopId)}\", \"category\": \"{Escape(f[i].MissingCategory)}\", \"severity\": \"{Escape(f[i].Severity)}\", \"details\": \"{Escape(f[i].Details)}\", \"fix\": \"{Escape(f[i].RecommendedFix)}\" }}{comma}");
            }
            lines.AppendLine("  ]");
            lines.AppendLine("}");
            File.WriteAllText(path, lines.ToString());
        }

        private static string Escape(string s) =>
            (s ?? string.Empty).Replace("\\", "\\\\", StringComparison.Ordinal)
                               .Replace("\"", "\\\"", StringComparison.Ordinal);

        /// <summary>
        /// Best-effort current commit hash for manifest provenance. Returns
        /// empty string (never throws) when git is unavailable or this is a
        /// non-repo build — the manifest's GeneratedAt timestamp still
        /// establishes when the report was produced even without it.
        /// </summary>
        private static string TryGetCurrentCommitHash(string repoRoot)
        {
            try
            {
                var psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "git",
                    Arguments = "rev-parse HEAD",
                    WorkingDirectory = repoRoot,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                using var proc = System.Diagnostics.Process.Start(psi);
                if (proc == null) return string.Empty;
                string output = proc.StandardOutput.ReadToEnd().Trim();
                proc.WaitForExit(5000);
                return proc.ExitCode == 0 ? output : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
