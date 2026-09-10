// SPDX-License-Identifier: MIT
// ASHFALL: Narrative Continuity Self-Test (Plan 50 / Task 16, Phase 16H).
//
// Deterministic diagnostic mode that runs the Core narrative continuity
// engine over the corpus and writes artifacts.
// Run via: godot --headless --path . -- --narrative-continuity-selftest

using Godot;
using System;
using System.IO;
using System.Linq;
using System.Text;
using Ashfall.Core.Narrative.Continuity;

namespace AtomicWar.GodotApp
{
    public static class NarrativeContinuitySelfTest
    {
        public static int Run(string repoRoot, string dataDir)
        {
            GD.Print("=== Narrative Continuity Self-Test ===");
            GD.Print($"Data directory: {dataDir}");
            GD.Print();

            try
            {
                // The SAME engine the unit tests call — parity by construction.
                var engine = new NarrativeContinuityEngine(dataDir);
                var report = engine.Analyze();

                GD.Print($"  Files scanned:  {report.FilesScanned}");
                GD.Print($"  Graphs:         {report.Graphs.Count}");
                GD.Print($"  Nodes:          {report.NodesCount}");
                GD.Print($"  Edges:          {report.EdgesCount}");
                GD.Print($"  Flags set:      {report.FlagsSet.Count}");
                GD.Print($"  Flags read:     {report.FlagsRead.Count}");
                GD.Print($"  Hard errors:    {report.HardErrors}");
                GD.Print($"  Warnings:       {report.Warnings}");
                GD.Print();

                string jsonPath = Path.Combine(repoRoot, "artifacts", "narrative-continuity.json");
                string mdPath = Path.Combine(repoRoot, "artifacts", "narrative-continuity.md");
                WriteJsonArtifact(jsonPath, report);
                WriteMarkdownArtifact(mdPath, report);
                GD.Print($"  Artifact (json): {jsonPath}");
                GD.Print($"  Artifact (md):   {mdPath}");
                GD.Print();

                foreach (var f in report.Findings)
                {
                    string line = $"    [{f.Severity}] {f.Rule} @ {f.SourceFile} :: {f.NodeId} — {f.Details}";
                    if (f.Severity == "HARD") GD.PrintErr(line);
                    else GD.Print(line);
                }
                GD.Print();

                GD.Print(report.Passed
                    ? "NARRATIVE_CONTINUITY PASS"
                    : "NARRATIVE_CONTINUITY FAIL");
                return report.Passed ? 0 : 1;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Narrative continuity self-test failed: {ex.Message}");
                GD.PrintErr(ex.StackTrace);
                return 1;
            }
        }

        private static void WriteJsonArtifact(string path, NarrativeContinuityReport report)
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine($"  \"schemaVersion\": \"{report.SchemaVersion}\",");
            sb.AppendLine($"  \"filesScanned\": {report.FilesScanned},");
            sb.AppendLine($"  \"graphs\": {report.Graphs.Count},");
            sb.AppendLine($"  \"nodes\": {report.NodesCount},");
            sb.AppendLine($"  \"edges\": {report.EdgesCount},");
            sb.AppendLine($"  \"flagsSet\": {report.FlagsSet.Count},");
            sb.AppendLine($"  \"flagsRead\": {report.FlagsRead.Count},");
            sb.AppendLine($"  \"hardErrors\": {report.HardErrors},");
            sb.AppendLine($"  \"warnings\": {report.Warnings},");
            sb.AppendLine("  \"findings\": [");
            for (int i = 0; i < report.Findings.Count; i++)
            {
                var f = report.Findings[i];
                string comma = i < report.Findings.Count - 1 ? "," : string.Empty;
                sb.AppendLine($"    {{ \"rule\": \"{Esc(f.Rule)}\", \"severity\": \"{Esc(f.Severity)}\", \"file\": \"{Esc(f.SourceFile)}\", \"node\": \"{Esc(f.NodeId)}\", \"details\": \"{Esc(f.Details)}\" }}{comma}");
            }
            sb.AppendLine("  ],");
            sb.AppendLine("  \"allowlistApplied\": [");
            for (int i = 0; i < report.AppliedAllowlistEntries.Count; i++)
            {
                string comma = i < report.AppliedAllowlistEntries.Count - 1 ? "," : string.Empty;
                sb.AppendLine($"    \"{Esc(report.AppliedAllowlistEntries[i])}\"{comma}");
            }
            sb.AppendLine("  ]");
            sb.AppendLine("}");
            File.WriteAllText(path, sb.ToString());
        }

        private static void WriteMarkdownArtifact(string path, NarrativeContinuityReport report)
        {
            var sb = new StringBuilder();
            sb.AppendLine("# Narrative Continuity Report (Plan 50)");
            sb.AppendLine();
            sb.AppendLine("| Metric | Value |");
            sb.AppendLine("|---|---|");
            sb.AppendLine($"| Files scanned | {report.FilesScanned} |");
            sb.AppendLine($"| Graphs | {report.Graphs.Count} |");
            sb.AppendLine($"| Nodes | {report.NodesCount} |");
            sb.AppendLine($"| Edges | {report.EdgesCount} |");
            sb.AppendLine($"| Flags set | {report.FlagsSet.Count} |");
            sb.AppendLine($"| Flags read | {report.FlagsRead.Count} |");
            sb.AppendLine($"| Hard errors | {report.HardErrors} |");
            sb.AppendLine($"| Warnings | {report.Warnings} |");
            sb.AppendLine();
            sb.AppendLine("## Dangling references / structural errors");
            sb.AppendLine();
            foreach (var f in report.Findings.Where(f => f.Severity == "HARD"))
                sb.AppendLine($"- **{f.Rule}** `{f.SourceFile}` :: `{f.NodeId}` — {f.Details}");
            if (report.HardErrors == 0) sb.AppendLine("- (none)");
            sb.AppendLine();
            sb.AppendLine("## Unreachable / reserved nodes");
            sb.AppendLine();
            foreach (var f in report.Findings.Where(f => f.Rule == "UNREACHABLE_NODE"))
                sb.AppendLine($"- [{f.Severity}] `{f.SourceFile}` :: `{f.NodeId}` — {f.Details}");
            if (!report.Findings.Any(f => f.Rule == "UNREACHABLE_NODE")) sb.AppendLine("- (none)");
            sb.AppendLine();
            sb.AppendLine("## Set but never read");
            sb.AppendLine();
            foreach (var f in report.Findings.Where(f => f.Rule == "SET_NEVER_READ"))
                sb.AppendLine($"- `{f.Details}`");
            if (!report.Findings.Any(f => f.Rule == "SET_NEVER_READ")) sb.AppendLine("- (none)");
            sb.AppendLine();
            sb.AppendLine("## Read but never set");
            sb.AppendLine();
            foreach (var f in report.Findings.Where(f => f.Rule == "READ_NEVER_SET"))
                sb.AppendLine($"- `{f.Details}`");
            if (!report.Findings.Any(f => f.Rule == "READ_NEVER_SET")) sb.AppendLine("- (none)");
            sb.AppendLine();
            sb.AppendLine("## Case mismatches");
            sb.AppendLine();
            foreach (var f in report.Findings.Where(f => f.Rule == "CASE_MISMATCH"))
                sb.AppendLine($"- `{f.Details}`");
            if (!report.Findings.Any(f => f.Rule == "CASE_MISMATCH")) sb.AppendLine("- (none)");
            sb.AppendLine();
            sb.AppendLine("## Warnings");
            sb.AppendLine();
            foreach (var f in report.Findings.Where(f => f.Severity == "WARN" && f.Rule != "SET_NEVER_READ" && f.Rule != "READ_NEVER_SET" && f.Rule != "UNREACHABLE_NODE" && f.Rule != "CASE_MISMATCH"))
                sb.AppendLine($"- **{f.Rule}** `{f.SourceFile}` :: `{f.NodeId}` — {f.Details}");
            sb.AppendLine();
            sb.AppendLine("## Allowlisted findings");
            sb.AppendLine();
            foreach (var a in report.AppliedAllowlistEntries)
                sb.AppendLine($"- {a}");
            if (report.AppliedAllowlistEntries.Count == 0) sb.AppendLine("- (none)");
            File.WriteAllText(path, sb.ToString());
        }

        private static string Esc(string s) =>
            (s ?? string.Empty).Replace("\\", "\\\\", StringComparison.Ordinal)
                               .Replace("\"", "\\\"", StringComparison.Ordinal);
    }
}
