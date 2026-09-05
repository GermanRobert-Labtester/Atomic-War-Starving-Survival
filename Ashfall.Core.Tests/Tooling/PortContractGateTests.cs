// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// REM-002 / R12 — Mechanical port contract and integration seam gate.
    ///
    /// Ensures every public integration seam (Bind*, Wire*, Register*, Configure*)
    /// in Assets/Ashfall.Core is authoritatively declared in docs/ci/port_contract_policy.json
    /// with an explicit classification:
    ///   - HOST_REQUIRED: must have a production caller in src/
    ///   - LIVE_VIA_CORE: invoked internally by Core compositions/subsystems
    ///   - TEST_ONLY: test-only helper/fixture with diagnostic flag
    ///   - DEFERRED: dormant expansion feature with activation condition and expiry
    ///   - PURE_LIBRARY: pure utility or interface contract
    /// </summary>
    public sealed class PortContractGateTests
    {
        private static string RepoRoot()
        {
            string dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                if (Directory.Exists(Path.Combine(dir, "Assets", "Ashfall.Core")))
                    return dir;
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("repo root not found from test context");
        }

        public sealed class PortContractPolicyDocument
        {
            public string schema_version { get; set; } = string.Empty;
            public string description { get; set; } = string.Empty;
            public int total_seams { get; set; }
            public List<PortPolicyEntry> ports { get; set; } = new();
        }

        public sealed class PortPolicyEntry
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

        private static PortContractPolicyDocument LoadPolicy()
        {
            string root = RepoRoot();
            string path = Path.Combine(root, "docs", "ci", "port_contract_policy.json");
            Assert.True(File.Exists(path), $"port_contract_policy.json missing at {path}");
            string json = File.ReadAllText(path);
            var doc = JsonSerializer.Deserialize<PortContractPolicyDocument>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(doc);
            Assert.False(string.IsNullOrWhiteSpace(doc!.schema_version), "policy missing schema_version");
            Assert.True(doc.ports.Count >= 50, $"expected port entries, found only {doc.ports.Count}");
            return doc;
        }

        [Fact]
        public void PolicyFile_LoadsAndHasValidSchema_AndNoExpiredEntries()
        {
            var doc = LoadPolicy();
            var seen = new HashSet<string>(StringComparer.Ordinal);
            var errors = new List<string>();

            foreach (var item in doc.ports)
            {
                string key = $"{item.class_name}.{item.method_name}";
                if (string.IsNullOrWhiteSpace(item.class_name) || string.IsNullOrWhiteSpace(item.method_name))
                {
                    errors.Add($"Invalid entry with missing class or method name");
                    continue;
                }

                if (!seen.Add(key))
                    errors.Add($"Duplicate seam key '{key}' in policy");

                if (string.IsNullOrWhiteSpace(item.owner))
                    errors.Add($"Seam '{key}' missing required 'owner'");

                if (string.IsNullOrWhiteSpace(item.reason))
                    errors.Add($"Seam '{key}' missing required 'reason'");

                var validClassifications = new[] { "HOST_REQUIRED", "LIVE_VIA_CORE", "TEST_ONLY", "DEFERRED", "PURE_LIBRARY" };
                if (!validClassifications.Contains(item.classification))
                    errors.Add($"Seam '{key}' has invalid classification '{item.classification}'");

                if (item.classification == "DEFERRED")
                {
                    if (string.IsNullOrWhiteSpace(item.activation_condition))
                        errors.Add($"Deferred seam '{key}' missing 'activation_condition'");

                    if (string.IsNullOrWhiteSpace(item.expiry))
                    {
                        errors.Add($"Deferred seam '{key}' missing 'expiry' date");
                    }
                    else if (DateTime.TryParse(item.expiry, out var expiryDate))
                    {
                        if (DateTime.UtcNow > expiryDate.AddDays(1))
                        {
                            errors.Add($"Deferred seam '{key}' has expired ({item.expiry}); must be resolved or re-budgeted");
                        }
                    }
                    else
                    {
                        errors.Add($"Deferred seam '{key}' has unparseable expiry date '{item.expiry}'");
                    }
                }
            }

            Assert.True(errors.Count == 0, "Port contract policy errors:\n  " + string.Join("\n  ", errors));
        }

        [Fact]
        public void EveryHostRequiredPort_HasProductionCaller()
        {
            var doc = LoadPolicy();
            string root = RepoRoot();
            var srcFiles = Directory.GetFiles(Path.Combine(root, "src"), "*.cs", SearchOption.AllDirectories);
            var srcTexts = srcFiles.ToDictionary(f => f, File.ReadAllText, StringComparer.Ordinal);

            var uncalled = new List<string>();
            foreach (var port in doc.ports.Where(p => p.classification == "HOST_REQUIRED"))
            {
                var pattern = @"\b" + Regex.Escape(port.method_name) + @"\s*\(";
                bool calledInSrc = srcTexts.Values.Any(text => Regex.IsMatch(text, pattern));
                if (!calledInSrc)
                {
                    uncalled.Add($"{port.class_name}.{port.method_name} ({port.file_path}) — marked HOST_REQUIRED but not called in src/");
                }
            }

            Assert.True(uncalled.Count == 0,
                "HOST_REQUIRED seams with NO callers in src/:\n  " + string.Join("\n  ", uncalled));
        }

        [Fact]
        public void EveryDeferredPort_IsNotCalledInProduction()
        {
            var doc = LoadPolicy();
            string root = RepoRoot();
            var srcFiles = Directory.GetFiles(Path.Combine(root, "src"), "*.cs", SearchOption.AllDirectories);
            var srcTexts = srcFiles.ToDictionary(f => f, File.ReadAllText, StringComparer.Ordinal);

            var stealthCalled = new List<string>();
            foreach (var port in doc.ports.Where(p => p.classification == "DEFERRED"))
            {
                var pattern = @"\b" + Regex.Escape(port.method_name) + @"\s*\(";
                bool called = srcTexts.Values.Any(text => Regex.IsMatch(text, pattern));
                if (called)
                {
                    stealthCalled.Add($"{port.class_name}.{port.method_name} is marked DEFERRED but called in src/ — upgrade classification to HOST_REQUIRED");
                }
            }

            Assert.True(stealthCalled.Count == 0,
                "DEFERRED seams called from production:\n  " + string.Join("\n  ", stealthCalled));
        }

        [Fact]
        public void AllCoreIntegrationSeams_AreTrackedInPolicy()
        {
            var doc = LoadPolicy();
            var tracked = new HashSet<string>(doc.ports.Select(p => $"{p.class_name}.{p.method_name}"), StringComparer.Ordinal);

            string root = RepoRoot();
            var coreFiles = Directory.GetFiles(Path.Combine(root, "Assets", "Ashfall.Core"), "*.cs", SearchOption.AllDirectories);
            var pattern = new Regex(@"public\s+(?:static\s+|override\s+|virtual\s+|async\s+)*(?:[\w<>\[\]?,]+\s+)+(Bind\w*|Wire\w*|Register\w*|Configure\w*)\s*\(", RegexOptions.Compiled);

            var untracked = new List<string>();
            foreach (var file in coreFiles)
            {
                string content = File.ReadAllText(file);
                foreach (Match m in pattern.Matches(content))
                {
                    string mname = m.Groups[1].Value;
                    string before = content.Substring(0, m.Index);
                    var cm = Regex.Matches(before, @"(?:class|struct|interface)\s+(\w+)");
                    string cname = cm.Count > 0 ? cm[cm.Count - 1].Groups[1].Value : "Unknown";
                    string key = $"{cname}.{mname}";

                    if (!tracked.Contains(key))
                    {
                        untracked.Add($"{key} ({Path.GetFileName(file)})");
                    }
                }
            }

            Assert.True(untracked.Count == 0,
                "Unclassified integration seams in Core:\n  " + string.Join("\n  ", untracked.Distinct()));
        }

        [Fact]
        public void SyntheticValidation_CatchesUncalledAndAcceptsWired()
        {
            var fakeSrc = new Dictionary<string, string>
            {
                ["src/Host/Sample.cs"] = "engine.BindPorts(ports); engine.WireSensors();"
            };

            bool wired = fakeSrc.Values.Any(v => Regex.IsMatch(v, @"\bBindPorts\s*\("));
            Assert.True(wired, "Expected BindPorts to be recognized as wired");

            bool orphan = fakeSrc.Values.Any(v => Regex.IsMatch(v, @"\bUncalledSeam\s*\("));
            Assert.False(orphan, "Expected UncalledSeam to be detected as unwired");
        }
    }
}
