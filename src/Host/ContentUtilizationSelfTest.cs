// SPDX-License-Identifier: MIT
// ASHFALL: Content Utilization Self-Test
//
// Deterministic diagnostic mode that exercises representative runtime
// content wiring and generates the utilization manifest.
// Run via: godot --headless --path . -- --content-utilization-selftest

using Godot;
using System;
using System.IO;
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
                ContentUtilizationManifest.WriteManifest(graph, manifestPath);
                GD.Print($"  Manifest written to: {manifestPath}");
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

                // Phase 9: Exemptions check
                GD.Print("[Phase 9] Exemption validation...");
                var exemptions = DefaultExemptions.CreateDefault();
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
    }
}
