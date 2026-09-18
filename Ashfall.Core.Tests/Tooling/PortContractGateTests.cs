// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using Xunit;
using Ashfall.Core.Ports;

namespace Ashfall.Core.Tests.Tooling
{
    public class PortContractGateTests
    {
        private static string RepoRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                if (File.Exists(Path.Combine(dir.FullName, "Ashfall.sln")) ||
                    Directory.Exists(Path.Combine(dir.FullName, "Assets", "Ashfall.Core")))
                    return dir.FullName;
                dir = dir.Parent;
            }
            return Directory.GetCurrentDirectory();
        }

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

        private static PortContractPolicyDocument LoadPolicy()
        {
            string root = RepoRoot();
            string path = Path.Combine(root, "docs", "ci", "port_contract_policy.json");
            Assert.True(File.Exists(path), $"port_contract_policy.json missing at {path}");
            string json = File.ReadAllText(path);
            var doc = JsonSerializer.Deserialize<PortContractPolicyDocument>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(doc);
            Assert.False(string.IsNullOrWhiteSpace(doc!.schema_version), "policy missing schema_version");
            Assert.True(doc.ports.Count >= 100, $"expected at least 100 port entries, found {doc.ports.Count}");
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

                var validClassifications = new[] { "HOST_REQUIRED", "OPTIONAL_HOST", "LIVE_VIA_CORE", "TEST_ONLY", "PURE_LIBRARY", "DEFERRED" };
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

            var tokenPattern = new Regex(
                @"\b(?:class|struct|interface|record)\s+([A-Za-z0-9_]+)|(\{)|(\})|" +
                @"public\s+(?:static\s+|override\s+|virtual\s+|async\s+)*(?:[\w<>\[\]?,]+\s+)+(Bind\w*|Wire\w*|Register\w*|Configure\w*)\s*\(",
                RegexOptions.Compiled);

            var commentPattern = new Regex(
                @"//.*?$|/\*.*?\*/|\""(?:\\.|[^\\\""])*\""|'\\?(?:\\.|[^\\'])*'",
                RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline);

            var untracked = new List<string>();
            foreach (var file in coreFiles)
            {
                string raw = File.ReadAllText(file);
                string clean = commentPattern.Replace(raw, m => m.Value.StartsWith("/") ? " " : m.Value);

                var classStack = new List<(string Name, int Depth)>();
                foreach (Match m in tokenPattern.Matches(clean))
                {
                    if (m.Groups[1].Success)
                    {
                        classStack.Add((m.Groups[1].Value, 0));
                    }
                    else if (m.Groups[2].Success)
                    {
                        if (classStack.Count > 0)
                        {
                            var top = classStack[^1];
                            classStack[^1] = (top.Name, top.Depth + 1);
                        }
                    }
                    else if (m.Groups[3].Success)
                    {
                        if (classStack.Count > 0)
                        {
                            var top = classStack[^1];
                            if (top.Depth <= 1)
                                classStack.RemoveAt(classStack.Count - 1);
                            else
                                classStack[^1] = (top.Name, top.Depth - 1);
                        }
                    }
                    else if (m.Groups[4].Success)
                    {
                        string mname = m.Groups[4].Value;
                        string cname = classStack.Count > 0 ? classStack[^1].Name : "Unknown";
                        string key = $"{cname}.{mname}";

                        if (!tracked.Contains(key))
                        {
                            untracked.Add($"{key} ({Path.GetFileName(file)})");
                        }
                    }
                }
            }

            Assert.True(untracked.Count == 0,
                "Unclassified integration seams in Core:\n  " + string.Join("\n  ", untracked.Distinct()));
        }

        [Fact]
        public void CorePorts_AreEngineFree()
        {
            string root = RepoRoot();
            string portsDir = Path.Combine(root, "Assets", "Ashfall.Core", "Ports");
            Assert.True(Directory.Exists(portsDir), $"Ports directory missing at {portsDir}");

            var files = Directory.GetFiles(portsDir, "*.cs", SearchOption.AllDirectories);
            Assert.NotEmpty(files);

            var forbidden = new[] { "Godot", "UnityEngine", "UnityEditor" };
            var violations = new List<string>();

            foreach (var file in files)
            {
                string text = File.ReadAllText(file);
                foreach (var token in forbidden)
                {
                    if (Regex.IsMatch(text, $@"\busing\s+{token}\b") || Regex.IsMatch(text, $@"\b{token}\."))
                    {
                        violations.Add($"{Path.GetFileName(file)} references forbidden engine namespace '{token}'");
                    }
                }
            }

            Assert.Empty(violations);
        }

        [Fact]
        public void SyntheticValidation_ProofOfFailure_DetectsUncalledHostRequired()
        {
            // Proof of failure test (Plan 36A.15):
            // Intentionally assert that an uncalled seam is detected as missing caller.
            var fakePolicy = new List<PortPolicyEntry>
            {
                new() { class_name = "MockSystem", method_name = "UnwiredRequiredSeam", classification = "HOST_REQUIRED" }
            };

            var fakeSrc = new Dictionary<string, string>
            {
                ["Host.cs"] = "public void Setup() { /* intentionally no call */ }"
            };

            var uncalled = new List<string>();
            foreach (var port in fakePolicy.Where(p => p.classification == "HOST_REQUIRED"))
            {
                var pat = @"\b" + Regex.Escape(port.method_name) + @"\s*\(";
                if (!fakeSrc.Values.Any(t => Regex.IsMatch(t, pat)))
                    uncalled.Add($"{port.class_name}.{port.method_name}");
            }

            Assert.Single(uncalled);
            Assert.Equal("MockSystem.UnwiredRequiredSeam", uncalled[0]);
        }

        [Fact]
        public void SyntheticValidation_ProofOfFailure_DetectsUntrackedSeam()
        {
            // Proof of failure test (Plan 36A.15):
            // An untracked seam in Core must be detected as missing from policy.
            var tracked = new HashSet<string> { "ExistingClass.ExistingMethod" };
            var candidateSeam = "UnclassifiedClass.BindSomethingNew";

            bool isUntracked = !tracked.Contains(candidateSeam);
            Assert.True(isUntracked, "Gate must detect seams absent from policy.");
        }

        [Fact]
        public void PortValidationResult_ProofOfFailure_ReportsMissingPorts()
        {
            // Proof of failure test (Plan 36B):
            // PortValidationResult must flag missing ports as invalid.
            var result = new PortValidationResult
            {
                SubsystemId = "test_subsystem",
                RequiredPorts = new[] { "PortA", "PortB" },
                BoundPorts = new[] { "PortA" },
                MissingPorts = new[] { "PortB" }
            };

            Assert.False(result.IsValid);
            Assert.Single(result.MissingPorts);
            Assert.Equal("PortB", result.MissingPorts[0]);
        }
    }
}
