// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Plan124CvdDiamond
{
    /// <summary>
    /// Plan 124 Phase 2 — cvd_diamond_catalog.json characterization: schema,
    /// duplicate-ID rejection, grade ordering, defect ranges, consumer
    /// bindings (no consumer-less outputs), internal foreign keys, and the
    /// no-global-zero-wear invariant. Engine lands in Phase 4.
    /// </summary>
    public sealed class Plan124CvdDiamondCatalogTests
    {
        private static string GetDataDir()
        {
            string candidate = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
            if (Directory.Exists(candidate)) return Path.GetFullPath(candidate);
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string check = Path.Combine(dir.FullName, "Assets/StreamingAssets/Data");
                if (Directory.Exists(check)) return check;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        private sealed class RealFileIo : IFileIO
        {
            public bool DirectoryExists(string path) => Directory.Exists(path);
            public bool FileExists(string path) => File.Exists(path);
            public string ReadAllText(string path) => File.ReadAllText(path);
            public void WriteAllText(string path, string contents) => File.WriteAllText(path, contents);
            public string Combine(params string[] parts) => Path.Combine(parts);
        }

        private static CvdDiamondCatalog LoadReal()
        {
            var catalog = CvdDiamondCatalogLoader.Load(GetDataDir(), new RealFileIo());
            Assert.True(catalog.reactor_profiles.Count > 0, "cvd_diamond_catalog.json must load with reactor profiles");
            return catalog;
        }

        // TEST-AGGREGATION: source_rows=6 rule families; failures reported per row.
        [Fact]
        public void Real_catalog_passes_structural_validation()
        {
            var catalog = LoadReal();
            Assert.True(catalog.ValidateCatalog(out string error), $"catalog invalid: {error}");
        }

        [Fact]
        public void Grades_are_ordered_and_bounded()
        {
            var catalog = LoadReal();
            var failures = new List<string>();
            DiamondGrowthGrade? previous = null;
            foreach (var grade in catalog.growth_grades)
            {
                if (grade.rank < 0) failures.Add($"{grade.id}: negative rank");
                if (grade.wear_factor_bp < 0) failures.Add($"{grade.id}: negative wear factor");
                if (grade.rank == 0 && grade.wear_factor_bp != 0)
                    failures.Add($"{grade.id}: rank-0 rejected sentinel must carry wear_factor_bp 0");
                if (previous != null && grade.rank > 0 && previous.rank > 0
                    && grade.wear_factor_bp > previous.wear_factor_bp)
                    failures.Add($"{grade.id}: better grade has worse wear factor than '{previous.id}'");
                previous = grade;
            }
            Assert.True(failures.Count == 0, string.Join("; ", failures));
        }

        [Fact]
        public void No_grade_grants_zero_wear_to_production()
        {
            // No-global-zero-wear invariant (plan §6.17 / §24): only the
            // rejected sentinel may carry wear_factor 0; every usable grade
            // must still wear.
            var failures = new List<string>();
            foreach (var grade in LoadReal().growth_grades)
                if (grade.wear_factor_bp == 0 && grade.rank > 0)
                    failures.Add($"{grade.id}: usable grade with zero wear (global zero-wear risk)");
            Assert.True(failures.Count == 0, string.Join("; ", failures));
        }

        [Fact]
        public void Every_output_has_registered_consumers()
        {
            var catalog = LoadReal();
            var failures = new List<string>();
            foreach (var component in catalog.tool_components)
                if (component.accepted_consumer_tags.Count == 0)
                    failures.Add($"{component.id}: authored output with no consumer");
            Assert.True(failures.Count == 0, string.Join("; ", failures));
        }

        [Fact]
        public void Consumer_and_grade_foreign_keys_resolve()
        {
            var catalog = LoadReal();
            var failures = new List<string>();
            foreach (var component in catalog.tool_components)
                if (catalog.GetGrade(component.min_grade_id) == null)
                    failures.Add($"{component.id}: min_grade_id '{component.min_grade_id}' unresolved");
            foreach (var consumer in catalog.consumer_mappings)
                foreach (var componentId in consumer.accepted_component_ids)
                    if (catalog.GetComponent(componentId) == null)
                        failures.Add($"{consumer.id}: accepted_component_id '{componentId}' unresolved");
            foreach (var reactor in catalog.reactor_profiles)
                if (catalog.GetMaintenance(reactor.maintenance_profile_id) == null)
                    failures.Add($"{reactor.id}: maintenance_profile_id unresolved");
            Assert.True(failures.Count == 0, string.Join("; ", failures));
        }

        [Fact]
        public void Item_references_exist_in_items_json()
        {
            var dataDir = GetDataDir();
            var text = File.ReadAllText(Path.Combine(dataDir, "items.json"));
            var itemIds = new HashSet<string>(
                System.Text.RegularExpressions.Regex.Matches(text, "\"id\"\\s*:\\s*\"(item_[a-z0-9_]+)\"")
                    .Select(m => m.Groups[1].Value), StringComparer.Ordinal);
            Assert.True(itemIds.Count > 0, "items.json produced no ids");

            var catalog = LoadReal();
            var failures = new List<string>();
            foreach (var reactor in catalog.reactor_profiles)
                foreach (var itemId in reactor.install_item_ids.Concat(reactor.repair_item_ids))
                    if (!itemIds.Contains(itemId)) failures.Add($"reactor {reactor.id}: item '{itemId}' not in items.json");
            foreach (var feed in catalog.feed_profiles)
                foreach (var itemId in feed.feedstock_item_ids)
                    if (!itemIds.Contains(itemId)) failures.Add($"feed {feed.id}: item '{itemId}' not in items.json");
            foreach (var substrate in catalog.substrate_profiles)
                if (!itemIds.Contains(substrate.input_item_id)) failures.Add($"substrate {substrate.id}: item '{substrate.input_item_id}' not in items.json");
            foreach (var component in catalog.tool_components)
                foreach (var itemId in component.install_item_ids)
                    if (!itemIds.Contains(itemId)) failures.Add($"component {component.id}: item '{itemId}' not in items.json");
            Assert.True(failures.Count == 0, string.Join("; ", failures));
        }

        [Fact]
        public void Duplicate_ids_are_rejected()
        {
            var catalog = new CvdDiamondCatalog
            {
                growth_grades =
                {
                    new DiamondGrowthGrade { id = "dup_grade", rank = 1, wear_factor_bp = 7000 },
                    new DiamondGrowthGrade { id = "dup_grade", rank = 2, wear_factor_bp = 4000 }
                }
            };
            catalog.Index();
            Assert.False(catalog.ValidateCatalog(out string error));
            Assert.Contains("duplicate grade id 'dup_grade'", error);
        }

        [Fact]
        public void Grade_rank_collision_is_rejected()
        {
            var catalog = new CvdDiamondCatalog
            {
                growth_grades =
                {
                    new DiamondGrowthGrade { id = "grade_a", rank = 1, wear_factor_bp = 7000 },
                    new DiamondGrowthGrade { id = "grade_b", rank = 1, wear_factor_bp = 7000 }
                }
            };
            catalog.Index();
            Assert.False(catalog.ValidateCatalog(out string error));
            Assert.Contains("duplicate grade rank 1", error);
        }

        [Fact]
        public void Load_is_deterministic()
        {
            var dataDir = GetDataDir();
            var a = CvdDiamondCatalogLoader.Load(dataDir, new RealFileIo());
            var b = CvdDiamondCatalogLoader.Load(dataDir, new RealFileIo());
            Assert.Equal(a.reactor_profiles.Count, b.reactor_profiles.Count);
            foreach (var (x, y) in a.reactor_profiles.Zip(b.reactor_profiles, (x, y) => (x, y)))
                Assert.Equal((x.id, x.growth_rate_per_tick_bp, x.plasma_stability_baseline_bp),
                             (y.id, y.growth_rate_per_tick_bp, y.plasma_stability_baseline_bp));
        }

        [Fact]
        public void Missing_file_loads_empty_without_throwing()
        {
            var catalog = CvdDiamondCatalogLoader.Load(Path.Combine(Path.GetTempPath(), "no_such_dir_124"), new RealFileIo());
            Assert.Empty(catalog.reactor_profiles);
        }
    }
}
