// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    /// <summary>
    /// Plan 212 Phase 1 — commodity_baselines.json characterization: schema,
    /// live-category requirement, duplicate rejection, permille bounds,
    /// floor/ceiling ordering, elasticity vocabulary, base-within-own-bounds,
    /// and schema-version guard. Engine behavior lives in
    /// Plan212DynamicEconomyTests.
    /// </summary>
    public sealed class Plan212CommodityBaselineCatalogTests
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

        private static CommodityBaselineLoadResult LoadReal()
        {
            return CommodityBaselineCatalogLoader.Load(
                GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
        }

        // TEST-AGGREGATION: source_rows=12 authored categories x 4 rule
        // families (bounds, ordering, vocabulary, uniqueness); aggregate_cases
        // keep per-row diagnostics via asserted messages.
        [Fact]
        public void RealCatalog_Loads_AllTwelveKnownCategories_WithNoErrors()
        {
            var load = LoadReal();
            Assert.False(load.HasErrors, string.Join("; ", load.Errors));
            Assert.Equal(12, load.Categories.Count);
            // Every known goods category has an authored row: no category is
            // left without behavior (and no orphan rows exist).
            var authored = load.Categories.Select(c => c.category_id).ToHashSet();
            foreach (var known in GoodCategories.Known)
                Assert.True(authored.Contains(known), $"missing baseline for category '{known}'");
        }

        [Fact]
        public void RealCatalog_EveryRow_BaseWithinOwnBounds_AndFloorsBelowCeilings()
        {
            var load = LoadReal();
            foreach (var c in load.Categories)
            {
                Assert.InRange(c.base_multiplier_permille, CommodityBaselineCatalogLoader.MinMultiplierPermille, CommodityBaselineCatalogLoader.MaxMultiplierPermille);
                Assert.InRange(c.scarcity_floor_permille, CommodityBaselineCatalogLoader.MinMultiplierPermille, CommodityBaselineCatalogLoader.MaxMultiplierPermille);
                Assert.InRange(c.scarcity_ceiling_permille, CommodityBaselineCatalogLoader.MinMultiplierPermille, CommodityBaselineCatalogLoader.MaxMultiplierPermille);
                Assert.True(c.scarcity_floor_permille < c.scarcity_ceiling_permille, $"{c.category_id}: floor must be below ceiling");
                Assert.InRange(c.base_multiplier_permille, c.scarcity_floor_permille, c.scarcity_ceiling_permille);
                Assert.True(CommodityBaselineCatalogLoader.IsAcceptedElasticity(c.elasticity_class), $"{c.category_id}: elasticity class must be low/medium/high");
            }
        }

        [Fact]
        public void RealCatalog_LoadIsDeterministic_IdempotentAcrossRuns()
        {
            var first = LoadReal();
            var second = LoadReal();
            Assert.Equal(first.Categories.Count, second.Categories.Count);
            for (int i = 0; i < first.Categories.Count; i++)
            {
                Assert.Equal(first.Categories[i].category_id, second.Categories[i].category_id);
                Assert.Equal(first.Categories[i].base_multiplier_permille, second.Categories[i].base_multiplier_permille);
                Assert.Equal(first.Categories[i].scarcity_floor_permille, second.Categories[i].scarcity_floor_permille);
                Assert.Equal(first.Categories[i].scarcity_ceiling_permille, second.Categories[i].scarcity_ceiling_permille);
            }
        }

        [Fact]
        public void ToCatalog_Find_ReturnsBoundRows_Only()
        {
            var load = LoadReal();
            var catalog = CommodityBaselineCatalogLoader.ToCatalog(load);
            Assert.Equal(12, catalog.Count);
            Assert.NotNull(catalog.Find("medical"));
            Assert.Equal(1100, catalog.Find("medical")!.base_multiplier_permille);
            Assert.Null(catalog.Find("not_a_category"));
            Assert.Null(catalog.Find(""));
        }

        [Fact]
        public void Loader_MissingFile_CollectsError_DoesNotThrow()
        {
            var result = CommodityBaselineCatalogLoader.Load(
                "/nonexistent_dir", new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.True(result.HasErrors);
            Assert.Empty(result.Categories);
        }

        [Fact]
        public void Loader_RejectsUnknownCategory_UnreachableRowsCannotExist()
        {
            var dir = GetDataDir();
            string source = File.ReadAllText(Path.Combine(dir, CommodityBaselineCatalogLoader.FileName));
            string mutated = source.Replace("\"category_id\": \"food\"", "\"category_id\": \"not_a_goods_category\"");
            string tempDir = Path.Combine(Path.GetTempPath(), "plan212_catalog_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);
            try
            {
                File.WriteAllText(Path.Combine(tempDir, CommodityBaselineCatalogLoader.FileName), mutated);
                var load = CommodityBaselineCatalogLoader.Load(tempDir, new FileSystemIO(), new SystemTextJsonSerializer());
                Assert.True(load.HasErrors);
                Assert.Contains(load.Errors, e => e.Contains("not a known goods category"));
                Assert.DoesNotContain(load.Categories, c => c.category_id == "not_a_goods_category");
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }

        [Fact]
        public void Loader_RejectsFloorAtOrAboveCeiling()
        {
            var dir = GetDataDir();
            string source = File.ReadAllText(Path.Combine(dir, CommodityBaselineCatalogLoader.FileName));
            // food: floor 700 ceiling 2000 → floor 2000 ceiling 700.
            string mutated = source.Replace("\"scarcity_floor_permille\": 700,\n      \"scarcity_ceiling_permille\": 2000", "\"scarcity_floor_permille\": 2000,\n      \"scarcity_ceiling_permille\": 700");
            string tempDir = Path.Combine(Path.GetTempPath(), "plan212_catalog_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);
            try
            {
                File.WriteAllText(Path.Combine(tempDir, CommodityBaselineCatalogLoader.FileName), mutated);
                var load = CommodityBaselineCatalogLoader.Load(tempDir, new FileSystemIO(), new SystemTextJsonSerializer());
                Assert.True(load.HasErrors);
                Assert.Contains(load.Errors, e => e.Contains("must be below"));
                Assert.DoesNotContain(load.Categories, c => c.category_id == "food");
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }

        [Fact]
        public void Loader_RejectsBaseMultiplierOutsideOwnBounds()
        {
            var dir = GetDataDir();
            string source = File.ReadAllText(Path.Combine(dir, CommodityBaselineCatalogLoader.FileName));
            // luxury: base 900, bounds [500,2000] → base 2600 escapes the ceiling.
            string mutated = source.Replace("\"base_multiplier_permille\": 900", "\"base_multiplier_permille\": 2600");
            string tempDir = Path.Combine(Path.GetTempPath(), "plan212_catalog_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);
            try
            {
                File.WriteAllText(Path.Combine(tempDir, CommodityBaselineCatalogLoader.FileName), mutated);
                var load = CommodityBaselineCatalogLoader.Load(tempDir, new FileSystemIO(), new SystemTextJsonSerializer());
                Assert.True(load.HasErrors);
                Assert.Contains(load.Errors, e => e.Contains("must lie within its own scarcity bounds"));
                Assert.DoesNotContain(load.Categories, c => c.category_id == "luxury");
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }
    }
}
