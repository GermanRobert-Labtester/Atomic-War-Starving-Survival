// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests.Tooling
{
    public class CiGateManifestDriftTests
    {
        private static string FindRepoRoot()
        {
            string search = Directory.GetCurrentDirectory();
            for (int i = 0; i < 6; i++)
            {
                if (File.Exists(Path.Combine(search, "project.godot")) &&
                    File.Exists(Path.Combine(search, "Ashfall.csproj")))
                {
                    return search;
                }
                string? parent = Directory.GetParent(search)?.FullName;
                if (parent == null) break;
                search = parent;
            }
            throw new InvalidOperationException("Could not locate repository root from " + Directory.GetCurrentDirectory());
        }

        private static JsonDocument LoadManifest(string root)
        {
            string manifestPath = Path.Combine(root, "docs", "ci", "CI_GATE_MANIFEST.json");
            Assert.True(File.Exists(manifestPath), $"CI Gate Manifest must exist at {manifestPath}");
            string jsonText = File.ReadAllText(manifestPath);
            return JsonDocument.Parse(jsonText);
        }

        [Fact]
        public void Manifest_ExistsAndIsWellFormedJson()
        {
            string root = FindRepoRoot();
            using var doc = LoadManifest(root);
            var rootEl = doc.RootElement;

            Assert.True(rootEl.TryGetProperty("schema_version", out var sv));
            Assert.False(string.IsNullOrWhiteSpace(sv.GetString()));

            Assert.True(rootEl.TryGetProperty("gates", out var gatesEl));
            Assert.Equal(JsonValueKind.Array, gatesEl.ValueKind);
            Assert.True(gatesEl.GetArrayLength() >= 25, $"Expected at least 25 registered gates, got {gatesEl.GetArrayLength()}");
        }

        [Fact]
        public void Manifest_EveryGateHasRequiredFields()
        {
            string root = FindRepoRoot();
            using var doc = LoadManifest(root);
            var gates = doc.RootElement.GetProperty("gates").EnumerateArray().ToList();

            foreach (var gate in gates)
            {
                Assert.True(gate.TryGetProperty("gate_id", out var idProp), "Gate missing 'gate_id'");
                string? gateId = idProp.GetString();
                Assert.False(string.IsNullOrWhiteSpace(gateId), "gate_id cannot be blank");

                Assert.True(gate.TryGetProperty("name", out var nameProp), $"Gate '{gateId}' missing 'name'");
                Assert.False(string.IsNullOrWhiteSpace(nameProp.GetString()), $"Gate '{gateId}' has empty name");

                Assert.True(gate.TryGetProperty("command", out var cmdProp), $"Gate '{gateId}' missing 'command'");
                Assert.False(string.IsNullOrWhiteSpace(cmdProp.GetString()), $"Gate '{gateId}' has empty command");

                Assert.True(gate.TryGetProperty("timeout_seconds", out var timeoutProp), $"Gate '{gateId}' missing 'timeout_seconds'");
                Assert.True(timeoutProp.GetInt32() > 0, $"Gate '{gateId}' timeout must be > 0");

                Assert.True(gate.TryGetProperty("classification", out var classProp), $"Gate '{gateId}' missing 'classification'");
                string? classification = classProp.GetString();
                Assert.True(classification == "fast" || classification == "full" || classification == "performance" || classification == "release",
                    $"Gate '{gateId}' classification must be 'fast', 'full', 'performance', or 'release', got '{classification}'");
            }
        }

        [Fact]
        public void Manifest_HeaderCountsMatchGatesArray()
        {
            string root = FindRepoRoot();
            using var doc = LoadManifest(root);
            var rootEl = doc.RootElement;
            var gates = rootEl.GetProperty("gates").EnumerateArray().ToList();

            Assert.True(rootEl.TryGetProperty("total_gates", out var totalProp), "manifest must declare total_gates");
            Assert.Equal(gates.Count, totalProp.GetInt32());

            Assert.True(rootEl.TryGetProperty("fast_tier_count", out var fastProp), "manifest must declare fast_tier_count");
            int actualFast = gates.Count(g => g.GetProperty("classification").GetString() == "fast");
            Assert.Equal(actualFast, fastProp.GetInt32());
        }

        [Fact]
        public void Manifest_SchemaVersionNoteIsBounded()
        {
            string root = FindRepoRoot();
            using var doc = LoadManifest(root);
            var rootEl = doc.RootElement;

            Assert.True(rootEl.TryGetProperty("_schema_version_note", out var noteProp), "manifest must declare _schema_version_note");
            string note = noteProp.GetString()!;
            Assert.False(string.IsNullOrWhiteSpace(note), "_schema_version_note cannot be blank");
            Assert.True(note.Length <= 2000,
                $"_schema_version_note grew to {note.Length} chars; move historical entries to docs/ci/GATE_INVENTORY.md or a changelog");
        }

        [Fact]
        public void Manifest_GateIdsAreUniqueAndSnakeCase()
        {
            string root = FindRepoRoot();
            using var doc = LoadManifest(root);
            var gates = doc.RootElement.GetProperty("gates").EnumerateArray().ToList();

            var seenIds = new HashSet<string>(StringComparer.Ordinal);
            var snakeRegex = new Regex(@"^[a-z0-9_]+$", RegexOptions.Compiled);

            foreach (var gate in gates)
            {
                string gateId = gate.GetProperty("gate_id").GetString()!;
                Assert.Matches(snakeRegex, gateId);
                Assert.True(seenIds.Add(gateId), $"Duplicate gate_id detected in manifest: '{gateId}'");
            }
        }

        [Fact]
        public void Manifest_DependencyIdsReferenceRegisteredGates()
        {
            string root = FindRepoRoot();
            using var doc = LoadManifest(root);
            var gates = doc.RootElement.GetProperty("gates").EnumerateArray().ToList();
            var ids = gates.Select(g => g.GetProperty("gate_id").GetString()!).ToHashSet(StringComparer.Ordinal);

            foreach (var gate in gates)
            {
                string gateId = gate.GetProperty("gate_id").GetString()!;
                if (!gate.TryGetProperty("depends_on", out var deps) || deps.ValueKind != JsonValueKind.Array)
                {
                    continue;
                }

                foreach (var dep in deps.EnumerateArray())
                {
                    string? depId = dep.GetString();
                    Assert.False(string.IsNullOrWhiteSpace(depId), $"Gate '{gateId}' has a blank depends_on entry");
                    Assert.True(ids.Contains(depId!), $"Gate '{gateId}' depends_on unregistered gate '{depId}'");
                }
            }
        }

        [Fact]
        public void Manifest_AllReferencedScriptFilesExist()
        {
            string root = FindRepoRoot();
            using var doc = LoadManifest(root);
            var gates = doc.RootElement.GetProperty("gates").EnumerateArray().ToList();

            var scriptRegex = new Regex(@"(?:bash|python3)\s+([^\s]+)", RegexOptions.Compiled);

            foreach (var gate in gates)
            {
                string gateId = gate.GetProperty("gate_id").GetString()!;
                string command = gate.GetProperty("command").GetString()!;

                var match = scriptRegex.Match(command);
                if (match.Success)
                {
                    string scriptRelPath = match.Groups[1].Value;
                    string fullPath = Path.Combine(root, scriptRelPath);
                    Assert.True(File.Exists(fullPath),
                        $"Gate '{gateId}' references missing script: {scriptRelPath} (resolved: {fullPath})");
                }
            }
        }

        [Fact]
        public void Manifest_CoversAllCanonicalCoreVerificationGates()
        {
            string root = FindRepoRoot();
            using var doc = LoadManifest(root);
            var registeredIds = doc.RootElement.GetProperty("gates")
                .EnumerateArray()
                .Select(g => g.GetProperty("gate_id").GetString()!)
                .ToHashSet();

            string[] requiredGates =
            {
                "whitespace_hygiene",
                "json_schema_policy",
                "build_core_tests",
                "test_core_suite",
                "build_godot_host",
                "godot_import",
                "data_integrity",
                "bridge_removal",
                "asset_registry",
                "player_panels_uitest",
                "panel_bind_lifecycle",
                "save_load_failure",
                "holdfast_save",
                "inventory_save",
                "journal_save",
                "playable_shell",
                "day1_onboarding",
                "expansions_completeness",
                "triad_drift",
                "cli_catalog_drift",
                "save_store_matrix_drift",
                "architecture_map_drift",
                "compiler_warning_baseline",
                "docs_index_drift",
                "forbidden_core_apis",
                "catch_policy_lint",
                "persistent_filename_registry",
                "central_package_management",
                "doc_link_portability",
                "lfs_health_check",
                "release_workflow_parity",
                "repository_size_budget",
                "compile_set_reachability",
                "test_only_production_source"
            };

            foreach (string required in requiredGates)
            {
                Assert.True(registeredIds.Contains(required),
                    $"Canonical gate '{required}' is missing from docs/ci/CI_GATE_MANIFEST.json");
            }
        }

        [Fact]
        public void Workflow_CallsCanonicalRunnerAndPublishesArtifact()
        {
            string root = FindRepoRoot();
            string workflowPath = Path.Combine(root, ".github", "workflows", "ci.yml");
            Assert.True(File.Exists(workflowPath), $"Workflow file must exist at {workflowPath}");

            string workflowText = File.ReadAllText(workflowPath);

            // Parity check: workflow delegates to run-gates.py
            Assert.Contains("run-gates.py", workflowText);
            Assert.Contains("--tier fast", workflowText);
            Assert.Contains("--fail-artifact", workflowText);

            // Artifact publish check
            Assert.Contains("actions/upload-artifact", workflowText);
            Assert.Contains("build/reports/", workflowText);

            // Zero Unity references
            Assert.DoesNotContain("unity", workflowText, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("batchmode", workflowText, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("UNITY_LICENSE", workflowText);
        }

        [Fact]
        public void LocalRunner_DelegatesToCanonicalRunner()
        {
            string root = FindRepoRoot();
            string verifyScriptPath = Path.Combine(root, "scripts", "ci", "verify-fast.sh");
            Assert.True(File.Exists(verifyScriptPath), $"verify-fast.sh must exist at {verifyScriptPath}");

            string verifyText = File.ReadAllText(verifyScriptPath);
            Assert.Contains("run-gates.py", verifyText);
            Assert.Contains("--tier fast", verifyText);
        }
    }
}
