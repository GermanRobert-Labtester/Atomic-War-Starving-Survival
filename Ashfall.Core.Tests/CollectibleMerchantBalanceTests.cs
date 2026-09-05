using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Task 6: Merchant Economy and Trade Value Balance (Sections 6.1–6.12).
    /// Enforces canonical authored trade values in items.json, raw-material
    /// reference basket comparisons, rarity monotonicity, unique price caps,
    /// and absence of arbitrage exploits.
    /// </summary>
    public class CollectibleMerchantBalanceTests
    {
        private static readonly string DataDir = FindDataDir();
        private static readonly IFileIO FileIO = new FileSystemIO();
        private static readonly IJsonSerializer Serializer = new SystemTextJsonSerializer();

        private static string FindDataDir()
        {
            string? dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            while (dir != null)
            {
                string probe = Path.Combine(dir, "Assets", "StreamingAssets", "Data", "collectibles.json");
                if (File.Exists(probe)) return Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                dir = Path.GetDirectoryName(dir.TrimEnd(Path.DirectorySeparatorChar));
            }
            throw new DirectoryNotFoundException("data authority not found");
        }

        private static double Median(List<double> values)
        {
            if (values == null || values.Count == 0) return 0;
            var sorted = values.OrderBy(v => v).ToList();
            int mid = sorted.Count / 2;
            if (sorted.Count % 2 != 0) return sorted[mid];
            return (sorted[mid - 1] + sorted[mid]) / 2.0;
        }

        [Fact]
        public void Test01_CanonicalTradeValue_LoadedFromItemsJson()
        {
            var itemCatalog = ItemCatalogLoader.LoadCatalog(DataDir, FileIO, Serializer);
            var colCatalog = CollectibleCatalogLoader.Load(DataDir, FileIO, Serializer);
            Assert.NotNull(colCatalog);

            foreach (var def in colCatalog!.ByItemId.Values)
            {
                var item = itemCatalog.Get(def.item_id);
                Assert.NotNull(item);
                Assert.True(item!.tradeValue > 0, $"Collectible '{def.item_id}' has tradeValue <= 0.");
            }
        }

        [Fact]
        public void Test02_RawMaterialReferenceBasket_MedianCalculated()
        {
            var itemCatalog = ItemCatalogLoader.LoadCatalog(DataDir, FileIO, Serializer);

            // Canonical common crafting/survival material basket (Section 6.3)
            var basketIds = new[] { "scrap_metal", "scrap_wood", "sandbags", "clean_water", "canned_food" };
            var valuesPerKg = new List<double>();

            foreach (var id in basketIds)
            {
                var item = itemCatalog.Get(id);
                Assert.NotNull(item);
                float w = Math.Max(0.05f, item!.weight);
                valuesPerKg.Add(item.tradeValue / w);
            }

            double basketMedian = Median(valuesPerKg);
            // sandbags (0.33), scrap_wood (2.0), scrap_metal (2.4), canned_food (24.0), clean_water (30.0) -> median 2.4
            Assert.InRange(basketMedian, 2.0, 3.0);
        }

        [Fact]
        public void Test03_CommonCeiling_NoLuxuryPricing()
        {
            var itemCatalog = ItemCatalogLoader.LoadCatalog(DataDir, FileIO, Serializer);
            var colCatalog = CollectibleCatalogLoader.Load(DataDir, FileIO, Serializer);

            var commonDefs = colCatalog!.ByItemId.Values.Where(c => c.rarity == "common").ToList();
            Assert.NotEmpty(commonDefs);

            foreach (var c in commonDefs)
            {
                var item = itemCatalog.Get(c.item_id)!;
                // Common collectibles represent everyday pre-war remnants (posters, letters, matches)
                // Trade value must be low and accessible (<= 5 trade units)
                Assert.InRange(item.tradeValue, 1f, 5f);
            }

            var commonValues = commonDefs.Select(c => (double)itemCatalog.Get(c.item_id)!.tradeValue).ToList();
            double commonMedian = Median(commonValues);
            Assert.InRange(commonMedian, 1.0, 3.0);
        }

        [Fact]
        public void Test04_RareCeiling_BoundedValue()
        {
            var itemCatalog = ItemCatalogLoader.LoadCatalog(DataDir, FileIO, Serializer);
            var colCatalog = CollectibleCatalogLoader.Load(DataDir, FileIO, Serializer);

            var rareDefs = colCatalog!.ByItemId.Values.Where(c => c.rarity == "rare").ToList();
            Assert.NotEmpty(rareDefs);

            foreach (var c in rareDefs)
            {
                var item = itemCatalog.Get(c.item_id)!;
                Assert.InRange(item.tradeValue, 8f, 25f);
            }

            var rareValues = rareDefs.Select(c => (double)itemCatalog.Get(c.item_id)!.tradeValue).ToList();
            double rareMedian = Median(rareValues);
            Assert.InRange(rareMedian, 10.0, 20.0);
        }

        [Fact]
        public void Test05_UniqueCap_Under100()
        {
            var itemCatalog = ItemCatalogLoader.LoadCatalog(DataDir, FileIO, Serializer);
            var colCatalog = CollectibleCatalogLoader.Load(DataDir, FileIO, Serializer);

            var uniqueDefs = colCatalog!.ByItemId.Values.Where(c => c.unique).ToList();
            Assert.NotEmpty(uniqueDefs);

            foreach (var u in uniqueDefs)
            {
                var item = itemCatalog.Get(u.item_id)!;
                Assert.InRange(item.tradeValue, 1f, 100f);
            }
        }

        [Fact]
        public void Test06_MonotonicRarityMedians()
        {
            var itemCatalog = ItemCatalogLoader.LoadCatalog(DataDir, FileIO, Serializer);
            var colCatalog = CollectibleCatalogLoader.Load(DataDir, FileIO, Serializer);

            var common = colCatalog!.ByItemId.Values
                .Where(c => c.rarity == "common")
                .Select(c => (double)itemCatalog.Get(c.item_id)!.tradeValue).ToList();

            var uncommon = colCatalog.ByItemId.Values
                .Where(c => c.rarity == "uncommon")
                .Select(c => (double)itemCatalog.Get(c.item_id)!.tradeValue).ToList();

            var rare = colCatalog.ByItemId.Values
                .Where(c => c.rarity == "rare")
                .Select(c => (double)itemCatalog.Get(c.item_id)!.tradeValue).ToList();

            double medCommon = Median(common);
            double medUncommon = Median(uncommon);
            double medRare = Median(rare);

            Assert.True(medCommon < medUncommon, $"Common median ({medCommon}) must be strictly less than Uncommon median ({medUncommon}).");
            Assert.True(medUncommon < medRare, $"Uncommon median ({medUncommon}) must be strictly less than Rare median ({medRare}).");
        }

        [Fact]
        public void Test07_OutlierAudit_WeightsNonNegative_AndLowAverage()
        {
            var itemCatalog = ItemCatalogLoader.LoadCatalog(DataDir, FileIO, Serializer);
            var colCatalog = CollectibleCatalogLoader.Load(DataDir, FileIO, Serializer);

            double totalWeight = 0;
            foreach (var c in colCatalog!.ByItemId.Values)
            {
                var item = itemCatalog.Get(c.item_id)!;
                Assert.True(item.weight >= 0f, $"Item '{c.item_id}' has negative weight {item.weight}.");
                totalWeight += item.weight;
            }

            double avgWeight = totalWeight / colCatalog.Count;
            Assert.True(avgWeight < 1.0, $"Average collectible weight must be under 1.0 kg (actual: {avgWeight:F2} kg).");
        }

        [Fact]
        public void Test08_ArbitrageProof_SellPriceLessThanOrEqualToBuyPrice()
        {
            var routes = CaravanTradeRouteCatalogLoader.Load(DataDir, FileIO, Serializer);
            var inventory = new Inventory.Inventory();
            var network = new CaravanTradeNetworkSystem(routes, inventory, new SeededRng(42));
            var colCatalog = CollectibleCatalogLoader.Load(DataDir, FileIO, Serializer);

            foreach (var route in routes)
            {
                var manifest = new CaravanManifestState
                {
                    manifest_id = "test_manifest_" + route.route_id,
                    route_id = route.route_id,
                    faction_id = route.faction_id,
                    status = CaravanStatus.Arrived
                };

                foreach (var c in colCatalog!.ByItemId.Values)
                {
                    float buyPrice = network.CalculateItemBuyPrice(manifest, c.item_id);
                    float sellPrice = network.CalculateItemSellPrice(manifest, c.item_id);

                    Assert.True(buyPrice >= sellPrice,
                        $"Arbitrage exploit on route '{route.route_id}' for item '{c.item_id}': Buy {buyPrice} < Sell {sellPrice}");
                }
            }
        }

        [Fact]
        public void Test09_UniqueItems_ExcludedFromCaravanSurpluses()
        {
            var routes = CaravanTradeRouteCatalogLoader.Load(DataDir, FileIO, Serializer);
            var colCatalog = CollectibleCatalogLoader.Load(DataDir, FileIO, Serializer);
            var uniques = colCatalog!.ByItemId.Values.Where(c => c.unique).Select(c => c.item_id).ToHashSet();

            foreach (var route in routes)
            {
                foreach (var s in route.export_surpluses)
                {
                    Assert.DoesNotContain(s, uniques);
                }
            }
        }
    }
}
