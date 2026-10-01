// SPDX-License-Identifier: MIT
// ASHFALL release-craft gate: hotfix save-schema stability.
//
// Plan 48 / C2[21] phase-5 residual. A hotfix MUST NOT move a save codec schema
// version. This gate proves the live curated codecs still match the immutable
// v1.1.0 historical snapshot, and pins the non-mutating iron-rule rehearsal that
// scripts/ci/version-gate.py --self-test now runs.
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests.Save
{
    public sealed class HotfixRehearsalGateTests
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

        [Fact]
        public void HistoricalSchemaSnapshot_MatchesLiveCuratedCodecs()
        {
            string snapshotPath = Path.Combine(
                RepoRoot, "artifacts", "golden_saves", "historical", "schema_snapshot_v1.1.0.json");
            Assert.True(File.Exists(snapshotPath), "Historical schema snapshot is required for the hotfix gate.");

            using var doc = JsonDocument.Parse(File.ReadAllText(snapshotPath));
            Assert.True(doc.RootElement.TryGetProperty("schema_map", out var schemaMap));

            var live = VersionReport.SaveSchemaVersions.ToDictionary(
                s => s.Store, s => s.CurrentVersion, StringComparer.Ordinal);

            foreach (var entry in schemaMap.EnumerateObject())
            {
                Assert.True(live.TryGetValue(entry.Name, out int current),
                    $"Curated codec '{entry.Name}' is missing from VersionReport.SaveSchemaVersions.");
                Assert.True(current == entry.Value.GetInt32(),
                    $"Hotfix gate: codec '{entry.Name}' moved from snapshot v{entry.Value.GetInt32()} to v{current}.");
            }
        }

        [Fact]
        public void IronRuleRehearsal_IsPresentInVersionGate()
        {
            string gate = File.ReadAllText(Path.Combine(RepoRoot, "scripts", "ci", "version-gate.py"));
            Assert.Contains("Hotfix iron-rule rehearsal", gate);
            Assert.Contains("schema constant bump was not detected", gate);
            Assert.Contains("non-schema change was wrongly flagged", gate);
        }

        [Fact]
        public void ThrowawayFixtureRehearsal_HarnessIsPresent()
        {
            string path = Path.Combine(RepoRoot, "scripts", "release", "hotfix-rehearsal.sh");
            Assert.True(File.Exists(path), "hotfix-rehearsal.sh must exist.");
            string script = File.ReadAllText(path);
            Assert.Contains("HOTFIX_REHEARSAL PASS", script);
            Assert.Contains("production schema bump is rejected", script);
            Assert.Contains("docs-only change passes", script);
        }
    }
}
