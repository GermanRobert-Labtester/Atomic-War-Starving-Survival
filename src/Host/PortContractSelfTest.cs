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
    /// Self-test runner for REM-002 (--port-contract-selftest).
    /// Validates all Core integration seams against docs/ci/port_contract_policy.json.
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
            GD.Print("── PORT CONTRACT SELF-TEST (REM-002) ──");
            string root = Directory.GetCurrentDirectory();
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
                return 1;
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
                return 1;
            }

            if (doc == null || doc.ports.Count == 0)
            {
                GD.PrintErr("[FAIL] port_contract_policy.json contained no port definitions");
                return 1;
            }

            var srcFiles = Directory.GetFiles(Path.Combine(root, "src"), "*.cs", SearchOption.AllDirectories);
            var srcTexts = srcFiles.ToDictionary(f => f, File.ReadAllText, StringComparer.Ordinal);

            int failures = 0;
            int hostReqCount = 0;
            int liveCoreCount = 0;
            int deferredCount = 0;
            int testOnlyCount = 0;

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
                        if (DateTime.UtcNow > expDate.AddDays(1))
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

            if (failures == 0)
            {
                GD.Print($"[PASS] All {doc.ports.Count} integration seams conform to port contract policy ({hostReqCount} host-required, {liveCoreCount} live-in-core, {deferredCount} deferred, {testOnlyCount} test/diag, 0 errors)");
                return 0;
            }

            GD.PrintErr($"[FAIL] Port contract self-test failed with {failures} error(s)");
            return 1;
        }
    }
}
