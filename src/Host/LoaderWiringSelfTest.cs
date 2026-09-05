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
    /// Self-test runner for REM-001 (--loader-wiring-selftest).
    /// Validates all catalog loaders against docs/ci/loader_wiring_policy.json.
    /// </summary>
    public static class LoaderWiringSelfTest
    {
        private sealed class LoaderWiringPolicyDocument
        {
            public string schema_version { get; set; } = string.Empty;
            public string description { get; set; } = string.Empty;
            public List<LoaderPolicyItem> loaders { get; set; } = new();
        }

        private sealed class LoaderPolicyItem
        {
            public string loader_type { get; set; } = string.Empty;
            public List<string> entry_points { get; set; } = new();
            public string? owning_host_session { get; set; }
            public string disposition { get; set; } = string.Empty;
            public string? required_catalog { get; set; }
            public string? owner { get; set; }
            public string? reason { get; set; }
            public string? activation_condition { get; set; }
            public string? expiry { get; set; }
        }

        public static int Run(string dataDirectory)
        {
            GD.Print("── LOADER WIRING SELF-TEST (REM-001) ──");
            string root = Directory.GetCurrentDirectory();
            string policyPath = Path.Combine(root, "docs", "ci", "loader_wiring_policy.json");

            if (!File.Exists(policyPath))
            {
                string cur = root;
                for (int i = 0; i < 5 && !string.IsNullOrEmpty(cur); i++)
                {
                    string candidate = Path.Combine(cur, "docs", "ci", "loader_wiring_policy.json");
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
                GD.PrintErr($"[FAIL] loader_wiring_policy.json not found at {policyPath}");
                return 1;
            }

            LoaderWiringPolicyDocument? doc;
            try
            {
                string json = File.ReadAllText(policyPath);
                doc = JsonSerializer.Deserialize<LoaderWiringPolicyDocument>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[FAIL] Failed to parse loader_wiring_policy.json: {ex.Message}");
                return 1;
            }

            if (doc == null || doc.loaders.Count == 0)
            {
                GD.PrintErr("[FAIL] loader_wiring_policy.json contained no loaders");
                return 1;
            }

            var coreFiles = Directory.GetFiles(Path.Combine(root, "Assets", "Ashfall.Core"), "*.cs", SearchOption.AllDirectories);
            var srcFiles = Directory.GetFiles(Path.Combine(root, "src"), "*.cs", SearchOption.AllDirectories);
            var allFiles = coreFiles.Concat(srcFiles).ToDictionary(f => f, File.ReadAllText, StringComparer.Ordinal);
            var srcDict = srcFiles.ToDictionary(f => f, f => allFiles[f], StringComparer.Ordinal);

            int failures = 0;
            int liveCount = 0;
            int deferredCount = 0;

            foreach (var item in doc.loaders)
            {
                if (item.disposition == "LIVE")
                {
                    liveCount++;
                    string defFile = coreFiles.FirstOrDefault(f =>
                        Regex.IsMatch(allFiles[f], @"(?:class|struct)\s+" + Regex.Escape(item.loader_type) + @"\b")) ?? string.Empty;

                    var eps = item.entry_points.Where(ep => !string.IsNullOrWhiteSpace(ep)).ToList();
                    string pattern = @"\b" + Regex.Escape(item.loader_type) + @"\s*\.\s*(?:" + string.Join("|", eps.Select(Regex.Escape)) + @")\b";
                    bool wired = allFiles.Any(kv =>
                        kv.Key != defFile
                        && !kv.Key.Contains("Tests", StringComparison.Ordinal)
                        && Regex.IsMatch(kv.Value, pattern));

                    if (!wired)
                    {
                        GD.PrintErr($"[FAIL] LIVE loader '{item.loader_type}' has no production caller");
                        failures++;
                    }
                }
                else if (item.disposition == "DEFERRED")
                {
                    deferredCount++;
                    var eps = item.entry_points.Where(ep => !string.IsNullOrWhiteSpace(ep)).ToList();
                    string pattern = @"\b" + Regex.Escape(item.loader_type) + @"\s*\.\s*(?:" + string.Join("|", eps.Select(Regex.Escape)) + @")\b";
                    bool wired = srcDict.Any(kv => Regex.IsMatch(kv.Value, pattern));

                    if (wired)
                    {
                        GD.PrintErr($"[FAIL] DEFERRED loader '{item.loader_type}' is invoked in src/");
                        failures++;
                    }

                    if (!string.IsNullOrWhiteSpace(item.expiry) && DateTime.TryParse(item.expiry, out var expDate))
                    {
                        if (DateTime.UtcNow > expDate.AddDays(1))
                        {
                            GD.PrintErr($"[FAIL] DEFERRED loader '{item.loader_type}' has expired ({item.expiry})");
                            failures++;
                        }
                    }
                }
            }

            if (failures == 0)
            {
                GD.Print($"[PASS] All {doc.loaders.Count} loaders conform to policy ({liveCount} live, {deferredCount} deferred, 0 errors)");
                return 0;
            }

            GD.PrintErr($"[FAIL] Loader wiring self-test failed with {failures} error(s)");
            return 1;
        }
    }
}
