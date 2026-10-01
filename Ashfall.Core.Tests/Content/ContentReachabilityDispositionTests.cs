// SPDX-License-Identifier: MIT
// ASHFALL content-reachability disposition gate (T079–T086).
//
// Every UNRESOLVED catalog must carry a reviewed, owned disposition. The
// disposition set is a reviewable data file; this test pins its structure and
// the runtime coverage gate that consumes it.
using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Content;
using Xunit;

namespace Ashfall.Core.Tests.Content
{
    public sealed class ContentReachabilityDispositionTests
    {
        private static readonly string RepoRoot = FindRepoRoot();

        private static string FindRepoRoot()
        {
            string dir = AppContext.BaseDirectory;
            for (int i = 0; i < 10; i++)
            {
                if (File.Exists(Path.Combine(dir, "project.godot")))
                    return dir;
                var parent = Directory.GetParent(dir);
                if (parent == null) break;
                dir = parent.FullName;
            }
            return AppContext.BaseDirectory;
        }

        private static ExemptionRegistry LoadPolicy()
        {
            string path = Path.Combine(RepoRoot, "docs", "ci", "content_reachability_dispositions.json");
            Assert.True(File.Exists(path), $"Disposition policy is missing: {path}");
            var registry = new SystemTextJsonSerializer().Deserialize<ExemptionRegistry>(File.ReadAllText(path));
            Assert.NotNull(registry);
            return registry!;
        }

        [Fact]
        public void DispositionPolicy_EveryEntryIsValidAndOwned()
        {
            var registry = LoadPolicy();
            Assert.True(registry.Exemptions.Count >= 99,
                $"Expected a disposition for every currently-unresolved catalog, found {registry.Exemptions.Count}.");
            Assert.Empty(registry.GetInvalidExemptions());
            Assert.All(registry.Exemptions, e =>
            {
                Assert.False(string.IsNullOrWhiteSpace(e.Owner));
                Assert.False(string.IsNullOrWhiteSpace(e.Classification));
                Assert.False(string.IsNullOrWhiteSpace(e.Rationale));
                Assert.False(string.IsNullOrWhiteSpace(e.ExpiryCondition),
                    $"Disposition '{e.ContentPath}' must carry an expiry condition.");
            });
        }

        [Fact]
        public void DispositionPolicy_HasNoDuplicatePaths()
        {
            var registry = LoadPolicy();
            var duplicates = registry.Exemptions
                .GroupBy(e => e.ContentPath, StringComparer.OrdinalIgnoreCase)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();
            Assert.Empty(duplicates);
        }

        [Fact]
        public void DispositionPolicy_NamesKnownWiredCatalogs()
        {
            var registry = LoadPolicy();
            Assert.True(registry.TryGetExemption("accessibility_profiles.json", out var difficulty));
            Assert.False(string.IsNullOrWhiteSpace(difficulty.Owner));
            Assert.True(registry.TryGetExemption("alloys_and_ores.json", out var chronic));
            Assert.False(string.IsNullOrWhiteSpace(chronic.Classification));
        }

        [Fact]
        public void ContentUtilizationSelfTest_EnforcesDispositionCoverage()
        {
            string source = File.ReadAllText(Path.Combine(
                RepoRoot, "src", "Host", "ContentUtilizationSelfTest.cs"));
            Assert.Contains("content_reachability_dispositions.json", source);
            Assert.Contains("Unresolved without a disposition", source);
            Assert.Contains("UNDISPOSITIONED", source);
            Assert.Contains("Dispositions without an expiry", source);
        }

        [Fact]
        public void ReachabilityReportGenerator_ExistsAndPublishesTheArtifact()
        {
            string tool = Path.Combine(RepoRoot, "tools", "gotools", "cmd", "reachability-report", "main.go");
            Assert.True(File.Exists(tool), "The reachability report generator must exist.");
            string source = File.ReadAllText(tool);
            Assert.Contains("disposition without an expiry", source);
            Assert.Contains("unresolved catalog without a disposition", source);

            string artifact = Path.Combine(RepoRoot, "artifacts", "content-reachability-report.md");
            Assert.True(File.Exists(artifact), "The reachability report artifact must be generated.");
        }
    }
}
