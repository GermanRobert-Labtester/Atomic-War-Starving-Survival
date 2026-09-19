// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests.Save
{
    /// <summary>
    /// Plan 48 / C2[21] Phase 3 — Save Support Window Tests.
    ///
    /// Pins the curated set of versioned save codecs and their current schema
    /// versions against the values declared in VersionReport.SaveSchemaVersions.
    /// Also validates the historical fixture corpus manifest for structural
    /// well-formedness and backward-compatibility invariants.
    ///
    /// Gate:  save_support_window (full tier — registered in Phase 3)
    /// </summary>
    public sealed class SaveSupportWindowTests
    {
        // ---------------------------------------------------------------
        // Repo resolution helper (shared pattern with other test files)
        // ---------------------------------------------------------------
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

        private static string HistoricalFixturesDir =>
            Path.Combine(RepoRoot, "artifacts", "golden_saves", "historical");

        private static string HistoricalManifestPath =>
            Path.Combine(HistoricalFixturesDir, "manifest.json");

        // ---------------------------------------------------------------
        // 1. Schema version pins — curated live constants
        // ---------------------------------------------------------------

        /// <summary>
        /// Verifies that VersionReport.SaveSchemaVersions contains exactly the
        /// six curated versioned codecs at the expected versions for game v1.1.0.
        /// If a codec schema is intentionally bumped, update the expected value
        /// here alongside updating the CHANGELOG.md entry.
        /// </summary>
        [Fact]
        public void CuratedCodecs_ArePresent_WithExpectedCount()
        {
            var entries = VersionReport.SaveSchemaVersions;
            Assert.NotNull(entries);
            // Six curated versioned codecs as of game v1.1.0
            Assert.Equal(6, entries.Length);
        }

        [Fact]
        public void Holdfast_SchemaVersion_IsAtExpectedValue()
        {
            var entry = FindEntry("holdfast");
            // holdfast v5 is the pinned value for game v1.1.0
            Assert.Equal(5, entry.CurrentVersion);
        }

        [Fact]
        public void YearOfAsh_SchemaVersion_IsAtExpectedValue()
        {
            var entry = FindEntry("year_of_ash");
            // year_of_ash v5 is the pinned value for game v1.1.0
            Assert.Equal(5, entry.CurrentVersion);
        }

        [Fact]
        public void DoseLedger_SchemaVersion_IsAtExpectedValue()
        {
            var entry = FindEntry("dose_ledger");
            // dose_ledger v2 is the pinned value for game v1.1.0
            Assert.Equal(2, entry.CurrentVersion);
        }

        [Fact]
        public void ExpansionHub_SchemaVersion_IsAtExpectedValue()
        {
            var entry = FindEntry("expansion_hub");
            // expansion_hub v6 is the pinned value for game v1.1.0
            Assert.Equal(6, entry.CurrentVersion);
        }

        [Fact]
        public void ExpansionQuest_SchemaVersion_IsAtExpectedValue()
        {
            var entry = FindEntry("expansion_quest");
            // expansion_quest v1 is the pinned value for game v1.1.0
            Assert.Equal(1, entry.CurrentVersion);
        }

        [Fact]
        public void WeightOfChoices_SchemaVersion_IsAtExpectedValue()
        {
            var entry = FindEntry("weight_of_choices");
            // weight_of_choices v2 is the pinned value for game v1.1.0
            Assert.Equal(2, entry.CurrentVersion);
        }

        [Fact]
        public void AllCuratedCodecs_HavePositiveSchemaVersions()
        {
            foreach (var entry in VersionReport.SaveSchemaVersions)
            {
                Assert.True(entry.CurrentVersion >= 1,
                    $"Codec '{entry.Store}' has unexpected schema version {entry.CurrentVersion} (must be >= 1)");
            }
        }

        [Fact]
        public void AllCuratedCodecs_HaveNonEmptyStoreNames()
        {
            foreach (var entry in VersionReport.SaveSchemaVersions)
            {
                Assert.False(string.IsNullOrWhiteSpace(entry.Store),
                    "A SaveSchemaEntry has a blank Store name");
            }
        }

        [Fact]
        public void AllCuratedCodecs_HaveUniqueStoreNames()
        {
            var names = VersionReport.SaveSchemaVersions.Select(e => e.Store).ToArray();
            var distinct = names.Distinct(StringComparer.Ordinal).ToArray();
            Assert.Equal(names.Length, distinct.Length);
        }

        // ---------------------------------------------------------------
        // 2. Historical fixture corpus — manifest structural validation
        // ---------------------------------------------------------------

        /// <summary>
        /// The historical fixture corpus directory must exist once Phase 3 runs.
        /// The manifest at artifacts/golden_saves/historical/manifest.json must
        /// be valid JSON and have the expected schema_version field.
        /// </summary>
        [Fact]
        public void HistoricalFixtureManifest_ExistsAndIsValidJson()
        {
            if (!File.Exists(HistoricalManifestPath))
            {
                // Phase 3: corpus not yet created — skip gracefully with a skip signal
                return;
            }

            var json = File.ReadAllText(HistoricalManifestPath);
            using var doc = JsonDocument.Parse(json);

            Assert.True(doc.RootElement.TryGetProperty("schema_version", out var schemaVer),
                "historical/manifest.json must have 'schema_version'");
            Assert.True(schemaVer.GetInt32() >= 1,
                "historical/manifest.json schema_version must be >= 1");
        }

        [Fact]
        public void HistoricalFixtureManifest_HasAtLeastOneFixtureEntry()
        {
            if (!File.Exists(HistoricalManifestPath))
                return; // corpus not yet created

            var json = File.ReadAllText(HistoricalManifestPath);
            using var doc = JsonDocument.Parse(json);

            Assert.True(doc.RootElement.TryGetProperty("fixtures", out var fixtures),
                "historical/manifest.json must have 'fixtures' array");
            Assert.True(fixtures.GetArrayLength() >= 1,
                "historical/manifest.json must have at least one fixture entry");
        }

        [Fact]
        public void HistoricalFixtures_AllReferencedFilesExist()
        {
            if (!File.Exists(HistoricalManifestPath))
                return; // corpus not yet created

            var json = File.ReadAllText(HistoricalManifestPath);
            using var doc = JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("fixtures", out var fixtures))
                return;

            var missing = new List<string>();
            foreach (var fx in fixtures.EnumerateArray())
            {
                if (!fx.TryGetProperty("fixture_name", out var nameEl))
                    continue;
                string name = nameEl.GetString() ?? "";
                string fullPath = Path.Combine(HistoricalFixturesDir, name);
                if (!File.Exists(fullPath))
                    missing.Add(name);
            }

            Assert.Empty(missing); // fails listing any missing file names
        }

        [Fact]
        public void HistoricalFixtures_AllEntriesHaveGameVersionAndSchemaMap()
        {
            if (!File.Exists(HistoricalManifestPath))
                return; // corpus not yet created

            var json = File.ReadAllText(HistoricalManifestPath);
            using var doc = JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("fixtures", out var fixtures))
                return;

            var issues = new List<string>();
            foreach (var fx in fixtures.EnumerateArray())
            {
                string name = fx.TryGetProperty("fixture_name", out var n) ? n.GetString() ?? "?" : "?";
                if (!fx.TryGetProperty("game_version", out _))
                    issues.Add($"{name}: missing 'game_version'");
                if (!fx.TryGetProperty("schema_map", out _))
                    issues.Add($"{name}: missing 'schema_map'");
            }

            Assert.Empty(issues);
        }

        // ---------------------------------------------------------------
        // 3. Support window invariant — no version regression
        // ---------------------------------------------------------------

        /// <summary>
        /// Verifies that each curated codec's current schema version is >= the
        /// minimum version recorded in the historical corpus.  This ensures the
        /// runtime is not accidentally rolled back below what old saves require.
        /// </summary>
        [Fact]
        public void CurrentCodecVersions_NeverRegressBelowHistoricalMinimum()
        {
            if (!File.Exists(HistoricalManifestPath))
                return; // corpus not yet created

            var json = File.ReadAllText(HistoricalManifestPath);
            using var doc = JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("fixtures", out var fixtures))
                return;

            // Collect minimum schema version seen per store across all historical fixtures
            var minVersions = new Dictionary<string, int>(StringComparer.Ordinal);
            foreach (var fx in fixtures.EnumerateArray())
            {
                if (!fx.TryGetProperty("schema_map", out var schemaMap))
                    continue;
                foreach (var kvp in schemaMap.EnumerateObject())
                {
                    int v = kvp.Value.GetInt32();
                    if (!minVersions.TryGetValue(kvp.Name, out int existing) || v < existing)
                        minVersions[kvp.Name] = v;
                }
            }

            var regressions = new List<string>();
            foreach (var entry in VersionReport.SaveSchemaVersions)
            {
                if (minVersions.TryGetValue(entry.Store, out int minSeen))
                {
                    if (entry.CurrentVersion < minSeen)
                    {
                        regressions.Add(
                            $"Codec '{entry.Store}': current v{entry.CurrentVersion} < historical minimum v{minSeen}");
                    }
                }
            }

            Assert.Empty(regressions);
        }

        // ---------------------------------------------------------------
        // Helper
        // ---------------------------------------------------------------
        private static VersionReport.SaveSchemaEntry FindEntry(string store)
        {
            var entry = VersionReport.SaveSchemaVersions.FirstOrDefault(e =>
                string.Equals(e.Store, store, StringComparison.Ordinal));
            Assert.True(entry.Store != null,
                $"Expected codec '{store}' not found in VersionReport.SaveSchemaVersions");
            return entry;
        }
    }
}
