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
    /// Plan 14B (C1 checkpoint C1.2) — RegionalPriceAtlas: regional_prices.json
    /// characterization (schema, vocabularies, bounds, duplicates), modifier
    /// resolution (item over category, neutral fallback), deterministic best
    /// region, and goods-catalog cross-resolution. Market/caravan wiring and
    /// embargo composition belong to checkpoint C1.3.
    /// </summary>
    public sealed class RegionalPriceAtlasTests
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

        private static RegionalPriceAtlas LoadRealAtlas()
        {
            var load = RegionalPriceCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.False(load.HasErrors, string.Join("; ", load.Errors));
            return new RegionalPriceAtlas(RegionalPriceCatalogLoader.ToCatalog(load));
        }

        private static GoodsCatalog LoadGoods()
        {
            var load = GoodsCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.False(load.HasErrors, string.Join("; ", load.Errors));
            return GoodsCatalogLoader.ToCatalog(load);
        }

        // ── Data characterization ─────────────────────────────────────────

        [Fact]
        public void RealCatalog_LoadsClean_WithAllFiveCanonicalRegions()
        {
            var atlas = LoadRealAtlas();
            Assert.True(atlas.Catalog.Count >= 18);
            foreach (var region in new[] { "flotilla", "foundry", "greenhouse", "traplines", "settlement" })
                Assert.NotEmpty(atlas.GetRegionalGoods(region));
        }

        [Fact]
        public void RealCatalog_EveryItemIdResolves_InGoodsCatalog()
        {
            var atlas = LoadRealAtlas();
            var goods = LoadGoods();
            foreach (var entry in atlas.Catalog.All())
            {
                if (string.IsNullOrEmpty(entry.ItemId)) continue;
                Assert.True(goods.Find(entry.ItemId) != null,
                    $"regional_prices.json item '{entry.ItemId}' does not resolve in economy_goods.json");
            }
        }

        [Fact]
        public void RealCatalog_EveryCategoryRow_IsAKnownCategory_AndEveryModifierInBounds()
        {
            var atlas = LoadRealAtlas();
            foreach (var entry in atlas.Catalog.All())
            {
                if (string.IsNullOrEmpty(entry.Category)) continue;
                Assert.True(GoodCategories.IsKnown(entry.Category), $"unknown category '{entry.Category}'");
                Assert.InRange(entry.BasePriceModifierPermille, RegionalPriceAtlas.MinModifierPermille, RegionalPriceAtlas.MaxModifierPermille);
            }
        }

        [Fact]
        public void RealCatalog_LoadIsDeterministic()
        {
            var a = LoadRealAtlas();
            var b = LoadRealAtlas();
            var first = a.Catalog.All();
            var second = b.Catalog.All();
            Assert.Equal(second.Count, first.Count);
            for (int i = 0; i < first.Count; i++)
            {
                Assert.Equal(second[i].Region, first[i].Region);
                Assert.Equal(second[i].ItemId, first[i].ItemId);
                Assert.Equal(second[i].Category, first[i].Category);
                Assert.Equal(second[i].BasePriceModifierPermille, first[i].BasePriceModifierPermille);
            }
        }

        // ── Modifier resolution ───────────────────────────────────────────

        [Fact]
        public void FlotillaSpecialtyGoods_ResolveCheapMultiplier()
        {
            var atlas = LoadRealAtlas();
            Assert.Equal(700, atlas.GetModifierPermille("item_desal_membrane", "materials", "flotilla"));
            Assert.Equal(700, atlas.GetModifierPermille("item_ro_membrane", "materials", "flotilla"));
            Assert.Equal(700f / 1000f * 85f, atlas.GetRegionalPrice(85f, "item_desal_membrane", "materials", "flotilla"), 3);
        }

        [Fact]
        public void ExpensiveImportedGoods_ResolveExpectedMultiplier()
        {
            var atlas = LoadRealAtlas();
            Assert.Equal(1500, atlas.GetModifierPermille("seed_packets", "food", "flotilla"));
            Assert.Equal(1500, atlas.GetModifierPermille("electronic_scrap", "materials", "traplines"));
        }

        [Fact]
        public void CategoryRow_AppliesToEveryGoodOfThatCategory()
        {
            var atlas = LoadRealAtlas(); // foundry category food 1300.
            Assert.Equal(1300, atlas.GetModifierPermille("cooked_meat", "food", "foundry"));
            Assert.Equal(1300, atlas.GetModifierPermille("canned_food", "food", "foundry"));
        }

        [Fact]
        public void ItemEntry_OverridesCategoryEntry()
        {
            var catalog = new RegionalPriceCatalog();
            catalog.Add(new RegionalPriceEntry("foundry", "", "food", 1300, "imported_scarce"));
            catalog.Add(new RegionalPriceEntry("foundry", "canned_food", "", 900, "balanced"));
            var atlas = new RegionalPriceAtlas(catalog);
            Assert.Equal(900, atlas.GetModifierPermille("canned_food", "food", "foundry"));
            Assert.Equal(1300, atlas.GetModifierPermille("cooked_meat", "food", "foundry"));
        }

        [Fact]
        public void UnknownItemOrRegion_ResolveNeutral_NeverCrash()
        {
            var atlas = LoadRealAtlas();
            Assert.Equal(1000, atlas.GetModifierPermille("item_not_in_catalog", "misc", "foundry"));
            Assert.Equal(1000, atlas.GetModifierPermille("bandages", "medical", "not_a_region"));
            Assert.Equal(1000, atlas.GetModifierPermille("bandages", "medical", null!));
            Assert.Equal(0f, atlas.GetRegionalPrice(-1f, "bandages", "medical", "foundry")); // invalid base guarded
        }

        [Fact]
        public void NeutralEntry_PreservesBasePrice_OldCallerCompat()
        {
            var atlas = LoadRealAtlas();
            Assert.Equal(15f, atlas.GetRegionalPrice(15f, "bandages", "medical", "foundry"), 3);
            // traplines has no entry for bandages — the atlas stays neutral there.
            Assert.Equal(14f, atlas.GetRegionalPrice(14f, "bandages", "medical", "traplines"), 3);
        }

        [Fact]
        public void TryGetRegionalEntry_FindsItemRows_Only()
        {
            var atlas = LoadRealAtlas();
            Assert.True(atlas.TryGetRegionalEntry("cooked_meat", "traplines", out var entry));
            Assert.Equal(700, entry!.BasePriceModifierPermille);
            Assert.False(atlas.TryGetRegionalEntry("cooked_meat", "foundry", out _)); // category row, not item row
        }

        // ── Best region (deterministic) ───────────────────────────────────

        [Fact]
        public void GetBestRegion_IsDeterministic_AndPrefersCheapest()
        {
            var atlas = LoadRealAtlas();
            string[] regions = { "flotilla", "foundry", "greenhouse", "traplines", "settlement" };
            Assert.Equal("traplines", atlas.GetBestRegion("cooked_meat", "food", regions));      // 700
            Assert.Equal("foundry", atlas.GetBestRegion("electronic_scrap", "materials", regions)); // 800 < 1500
            Assert.Equal("greenhouse", atlas.GetBestRegion("seed_packets", "food", regions));    // 800
        }

        [Fact]
        public void GetBestRegion_TieBreaksByOrdinalRegionName()
        {
            var catalog = new RegionalPriceCatalog();
            catalog.Add(new RegionalPriceEntry("traplines", "bandages", "", 900, "balanced"));
            catalog.Add(new RegionalPriceEntry("flotilla", "bandages", "", 900, "balanced"));
            var atlas = new RegionalPriceAtlas(catalog);
            Assert.Equal("flotilla", atlas.GetBestRegion("bandages", "medical", new[] { "traplines", "flotilla" }));
            Assert.Null(atlas.GetBestRegion("item_not_mapped", "misc", new[] { "flotilla", "foundry" }));
            Assert.Null(atlas.GetBestRegion("bandages", "medical", null));
        }

        // ── Region coverage (heat-map seed) ───────────────────────────────

        [Fact]
        public void GetRegionsForItem_And_GetRegionalGoods_SupportHeatMap()
        {
            var atlas = LoadRealAtlas();
            Assert.Equal(new[] { "foundry", "traplines" }, atlas.GetRegionsForItem("electronic_scrap"));
            var foundryRows = atlas.GetRegionalGoods("foundry");
            Assert.Equal(4, foundryRows.Count); // 3 item rows + 1 category row
            Assert.Contains(foundryRows, r => r.Category == "food");
        }

        // ── Loader validation ─────────────────────────────────────────────

        [Fact]
        public void Loader_MissingFile_CollectsError_DoesNotThrow()
        {
            var result = RegionalPriceCatalogLoader.Load("/nonexistent_dir", new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.True(result.HasErrors);
            Assert.Empty(result.Entries);
        }

        [Fact]
        public void Loader_RejectsMalformedEntries_WithNamedErrors()
        {
            var dir = GetDataDir();
            string source = File.ReadAllText(Path.Combine(dir, RegionalPriceCatalogLoader.FileName));

            var mutations = new (string description, string find, string replace)[]
            {
                ("unknown region", "\"region\": \"flotilla\"", "\"region\": \"not_a_region\""),
                ("out-of-bounds modifier", "\"base_price_modifier_permille\": 700", "\"base_price_modifier_permille\": 100"),
                ("unknown scarcity profile", "\"scarcity_profile\": \"local_surplus\"", "\"scarcity_profile\": \"shiny\""),
                ("unknown category", "\"category\": \"food\"", "\"category\": \"not_a_category\"")
            };

            foreach (var (description, find, replace) in mutations)
            {
                string mutated = source.Replace(find, replace);
                Assert.NotEqual(source, mutated); // the mutation must actually apply
                string tempDir = Path.Combine(Path.GetTempPath(), "plan14b_atlas_" + Guid.NewGuid().ToString("N"));
                Directory.CreateDirectory(tempDir);
                try
                {
                    File.WriteAllText(Path.Combine(tempDir, RegionalPriceCatalogLoader.FileName), mutated);
                    var load = RegionalPriceCatalogLoader.Load(tempDir, new FileSystemIO(), new SystemTextJsonSerializer());
                    Assert.True(load.HasErrors, $"mutation '{description}' should be rejected");
                }
                finally
                {
                    Directory.Delete(tempDir, true);
                }
            }
        }
    }
}
