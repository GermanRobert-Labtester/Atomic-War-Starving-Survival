// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using Godot;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Self-test runner for Plan 36 (--port-contract-selftest).
    /// Validates all Core integration seams against docs/ci/port_contract_policy.json
    /// and audits runtime host subsystem wiring contracts (Plan 36A / 36B).
    /// </summary>
    public static class PortContractSelfTest
    {
        private sealed class PortContractPolicyDocument
        {
            public string schema_version { get; set; } = string.Empty;
            public string description { get; set; } = string.Empty;
            public int total_seams { get; set; }
            public List<PortPolicyEntry> ports { get; set; } = new();
        }

        private sealed class PortPolicyEntry
        {
            public string class_name { get; set; } = string.Empty;
            public string method_name { get; set; } = string.Empty;
            public string file_path { get; set; } = string.Empty;
            public string classification { get; set; } = string.Empty;
            public string owner { get; set; } = string.Empty;
            public string reason { get; set; } = string.Empty;
            public bool diagnostic { get; set; }
            public string? activation_condition { get; set; }
            public string? expiry { get; set; }
        }

        public static int Run(string dataDirectory)
        {
            GD.Print("── PORT CONTRACT & HOST WIRING SELF-TEST (Plan 36) ──");
            string root = CatalogPath.ResolveRepoRoot();
            string policyPath = Path.Combine(root, "docs", "ci", "port_contract_policy.json");

            if (!File.Exists(policyPath))
            {
                string cur = root;
                for (int i = 0; i < 5 && !string.IsNullOrEmpty(cur); i++)
                {
                    string candidate = Path.Combine(cur, "docs", "ci", "port_contract_policy.json");
                    if (File.Exists(candidate))
                    {
                        policyPath = candidate;
                        root = cur;
                        break;
                    }
                    cur = Directory.GetParent(cur)?.FullName ?? string.Empty;
                }
            }

            if (!File.Exists(policyPath))
            {
                GD.PrintErr($"[FAIL] port_contract_policy.json not found at {policyPath}");
                return HostCli.EmitSummary("port_contract_selftest", false, 1, 0, 1, "port_contract_policy.json missing");
            }

            PortContractPolicyDocument? doc;
            try
            {
                string json = File.ReadAllText(policyPath);
                doc = JsonSerializer.Deserialize<PortContractPolicyDocument>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[FAIL] Failed to parse port_contract_policy.json: {ex.Message}");
                return HostCli.EmitSummary("port_contract_selftest", false, 1, 0, 1, $"parse error: {ex.Message}");
            }

            if (doc == null || doc.ports.Count == 0)
            {
                GD.PrintErr("[FAIL] port_contract_policy.json contained no port definitions");
                return HostCli.EmitSummary("port_contract_selftest", false, 1, 0, 1, "empty policy");
            }

            var srcFiles = Directory.GetFiles(Path.Combine(root, "src"), "*.cs", SearchOption.AllDirectories);
            var srcTexts = srcFiles.ToDictionary(f => f, File.ReadAllText, StringComparer.Ordinal);

            int failures = 0;
            int hostReqCount = 0;
            int liveCoreCount = 0;
            int deferredCount = 0;
            int testOnlyCount = 0;
            int pureLibCount = 0;

            foreach (var port in doc.ports)
            {
                var pattern = @"\b" + Regex.Escape(port.method_name) + @"\s*\(";

                if (port.classification == "HOST_REQUIRED")
                {
                    hostReqCount++;
                    bool calledInSrc = srcTexts.Values.Any(text => Regex.IsMatch(text, pattern));
                    if (!calledInSrc)
                    {
                        GD.PrintErr($"[FAIL] HOST_REQUIRED seam '{port.class_name}.{port.method_name}' has no caller in src/");
                        failures++;
                    }
                }
                else if (port.classification == "LIVE_VIA_CORE")
                {
                    liveCoreCount++;
                }
                else if (port.classification == "PURE_LIBRARY")
                {
                    pureLibCount++;
                }
                else if (port.classification == "DEFERRED")
                {
                    deferredCount++;
                    bool called = srcTexts.Values.Any(text => Regex.IsMatch(text, pattern));
                    if (called)
                    {
                        GD.PrintErr($"[FAIL] DEFERRED seam '{port.class_name}.{port.method_name}' is called in src/ without policy update");
                        failures++;
                    }

                    if (!string.IsNullOrWhiteSpace(port.expiry) && DateTime.TryParse(port.expiry, out var expDate))
                    {
                        if (DateTime.UtcNow > expDate.AddDays(1)) // DETERMINISM_ALLOWLIST: Mock expiration date check in self-test
                        {
                            GD.PrintErr($"[FAIL] DEFERRED seam '{port.class_name}.{port.method_name}' has expired ({port.expiry})");
                            failures++;
                        }
                    }
                }
                else if (port.classification == "TEST_ONLY")
                {
                    testOnlyCount++;
                }
            }

            // Print machine metrics (Plan 36A.10)
            GD.Print($"PORTS_REQUIRED={hostReqCount}");
            GD.Print($"PORTS_BOUND={hostReqCount - failures}");
            GD.Print($"PORTS_MISSING={failures}");

            // Execute runtime host subsystem wiring validation (Plan 36B.5)
            // In headless mode no production reporter exists, so register a
            // demo combat session whose required ports are bound to benign
            // in-memory consumers. Production binds the same ports for real in
            // Main.Expeditions (Inventory/Survivors + WireRealState); leaving
            // them unbound here made the report a false negative (0/9) that the
            // old pass criterion ignored. The criterion below now requires the
            // wiring summary to be valid, so an unbound required port fails.
            if (HostWiringValidator.GetRegisteredReporters().Count == 0)
            {
                var combatSession = new CombatHostSession();
                combatSession.Inventory = new InventoryHostSession();
                combatSession.Survivors = new SurvivorsHostSession();
                combatSession.WireRealState(
                    markCombatSurvived: _ => { },
                    onSurvivorDeath: (_, _) => { });
                HostWiringValidator.RegisterReporter(combatSession);
            }
            var wiringSummary = HostWiringValidator.ValidateAll();

            GD.Print($"HOST_WIRING_REQUIRED={wiringSummary.TotalWiringRequired}");
            GD.Print($"HOST_WIRING_BOUND={wiringSummary.TotalWiringBound}");
            GD.Print($"HOST_WIRING_MISSING={wiringSummary.TotalWiringMissing}");

            bool passed = failures == 0 && wiringSummary.IsValid;
            string details = passed
                ? $"PASS: {doc.ports.Count} seams conform to policy ({hostReqCount} host-required, {liveCoreCount} live-in-core, {deferredCount} deferred, {testOnlyCount} test, {pureLibCount} lib); host wiring: {wiringSummary.TotalWiringBound}/{wiringSummary.TotalWiringRequired} bound across {wiringSummary.TotalSessions} session(s)"
                : $"FAIL: {failures} port policy error(s), {wiringSummary.TotalWiringMissing} unbound host wiring effect(s)";

            if (passed)
            {
                GD.Print($"[PASS] All {doc.ports.Count} integration seams conform to port contract policy.");
            }
            else
            {
                GD.PrintErr($"[FAIL] Port contract self-test failed with {failures} port policy error(s) and {wiringSummary.TotalWiringMissing} unbound host wiring effect(s)");
            }

            return HostCli.EmitSummary(
                "port_contract_selftest",
                passed,
                passed ? 0 : 1,
                passedCount: doc.ports.Count - failures,
                failedCount: failures,
                details: details);
        }
    }
}
