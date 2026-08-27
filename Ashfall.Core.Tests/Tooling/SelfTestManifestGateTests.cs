// SPDX-License-Identifier: MIT
// ASHFALL CI gate: validates machine-readable self-test manifest completeness,
// uniqueness, schema compliance, and synchronization with HostCliRegistry.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class SelfTestManifestGateTests
    {
        private static string FindManifestPath()
        {
            string[] candidates =
            {
                Directory.GetCurrentDirectory(),
                AppContext.BaseDirectory
            };
            foreach (string start in candidates)
            {
                var dir = new DirectoryInfo(Path.GetFullPath(start));
                while (dir != null)
                {
                    string probe = Path.Combine(dir.FullName, "docs", "ci", "SELFTEST_MANIFEST.json");
                    if (File.Exists(probe))
                        return probe;
                    dir = dir.Parent;
                }
            }
            throw new FileNotFoundException("Could not locate docs/ci/SELFTEST_MANIFEST.json from the test run");
        }

        [Fact]
        public void Manifest_GenerateJsonManifest_ProducesValidJsonDocument()
        {
            string json = HostCliRegistry.GenerateJsonManifest();
            Assert.False(string.IsNullOrWhiteSpace(json));

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            Assert.Equal("1.0.0", root.GetProperty("schema_version").GetString());
            Assert.True(root.GetProperty("total_tests").GetInt32() >= 80);
            Assert.True(root.GetProperty("headless_test_count").GetInt32() >= 80);

            var tests = root.GetProperty("tests");
            Assert.Equal(JsonValueKind.Array, tests.ValueKind);
            Assert.Equal(root.GetProperty("total_tests").GetInt32(), tests.GetArrayLength());

            foreach (var elem in tests.EnumerateArray())
            {
                Assert.False(string.IsNullOrWhiteSpace(elem.GetProperty("test_id").GetString()));
                Assert.False(string.IsNullOrWhiteSpace(elem.GetProperty("action").GetString()));
                Assert.False(string.IsNullOrWhiteSpace(elem.GetProperty("category").GetString()));
                Assert.StartsWith("--", elem.GetProperty("primary_flag").GetString());
                Assert.False(string.IsNullOrWhiteSpace(elem.GetProperty("description").GetString()));
                Assert.False(string.IsNullOrWhiteSpace(elem.GetProperty("expected_summary_id").GetString()));
                Assert.True(elem.GetProperty("timeout_seconds").GetInt32() > 0);
            }
        }

        [Fact]
        public void Manifest_OnDiskFile_MatchesLiveRegistry()
        {
            string manifestPath = FindManifestPath();
            string jsonOnDisk = File.ReadAllText(manifestPath);

            var manifest = HostCliRegistry.CreateSelfTestManifest();
            var options = new JsonSerializerOptions { WriteIndented = true };
            string expectedJson = JsonSerializer.Serialize(manifest, options) + "\n";

            using var diskDoc = JsonDocument.Parse(jsonOnDisk);
            using var expectedDoc = JsonDocument.Parse(expectedJson);

            Assert.Equal(
                expectedDoc.RootElement.GetProperty("total_tests").GetInt32(),
                diskDoc.RootElement.GetProperty("total_tests").GetInt32());

            Assert.Equal(
                expectedDoc.RootElement.GetProperty("headless_test_count").GetInt32(),
                diskDoc.RootElement.GetProperty("headless_test_count").GetInt32());
        }

        [Fact]
        public void Manifest_AllTestIdsAndFlagsAreUnique()
        {
            var manifest = HostCliRegistry.CreateSelfTestManifest();
            var testIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var flags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var test in manifest.Tests)
            {
                Assert.True(
                    testIds.Add(test.TestId),
                    $"Duplicate test_id '{test.TestId}' found in self-test manifest");

                Assert.True(
                    flags.Add(test.PrimaryFlag),
                    $"Duplicate primary_flag '{test.PrimaryFlag}' found in self-test manifest");

                foreach (var alias in test.Aliases)
                {
                    Assert.True(
                        flags.Add(alias),
                        $"Duplicate alias flag '{alias}' found in self-test manifest for test '{test.TestId}'");
                }
            }
        }

        [Fact]
        public void Manifest_EveryIsTestDescriptor_IsCataloged()
        {
            var manifest = HostCliRegistry.CreateSelfTestManifest();
            var catalogedTestIds = new HashSet<string>(manifest.Tests.Select(t => t.TestId), StringComparer.OrdinalIgnoreCase);

            var expectedTestDescriptors = HostCliRegistry.AllDescriptors.Where(d => d.IsTest).ToList();
            Assert.Equal(expectedTestDescriptors.Count, manifest.Tests.Count);

            foreach (var desc in expectedTestDescriptors)
            {
                Assert.True(
                    catalogedTestIds.Contains(desc.TestId),
                    $"Descriptor '{desc.Action}' ({desc.PrimaryFlag}) is marked as test but missing from manifest");
            }
        }
    }
}
