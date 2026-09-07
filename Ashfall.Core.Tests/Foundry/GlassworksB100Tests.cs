using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Foundry;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests.Foundry
{
    public sealed class GlassworksB100Tests
    {
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found.");
        }

        private sealed class Harness
        {
            public readonly Dictionary<string, int> Inventory = new Dictionary<string, int>();
            public readonly SilentFoundrySystem System;
            public readonly SilentFoundryCatalog Catalog;
            public readonly GlassworksCatalog Glassworks;
            public readonly List<FoundryProductionRecord> Completed = new List<FoundryProductionRecord>();

            public Harness(int seed = 1009)
            {
                var files = new FileSystemIO();
                var json = new SystemTextJsonSerializer();
                string dataDir = FindDataDir();
                Catalog = new SilentFoundryCatalog();
                Catalog.Load(
                    SilentFoundryCatalogLoader.LoadProduction(dataDir, files, json),
                    SilentFoundryCatalogLoader.LoadFaction(dataDir, files, json));
                Glassworks = GlassworksCatalogLoader.Load(dataDir, files, json);

                Inventory["solar_cell"] = 10;
                Inventory["scrap_metal"] = 100;
                Inventory["item_welders_glass"] = 10;
                Inventory["item_cast_borosilicate_glass_blank"] = 10;
                Inventory[SilentFoundryIds.ItemCoal] = 100;
                Inventory[SilentFoundryIds.ItemCharcoal] = 100;
                Inventory[SilentFoundryIds.ItemCleanWater] = 100;

                System = new SilentFoundrySystem(rng: new SeededRng(seed));
                System.BindCatalog(Catalog, 4);
                System.BindGlassworksCatalog(Glassworks);
                System.BindInventory(
                    id => Inventory.TryGetValue(id, out int value) ? value : 0,
                    (_, _) => true,
                    (id, amount) => Inventory[id] = (Inventory.TryGetValue(id, out int value) ? value : 0) + amount,
                    (id, amount) => Inventory[id] = Math.Max(0, (Inventory.TryGetValue(id, out int value) ? value : 0) - amount));
                System.OnProductionCompleted += record => Completed.Add(record);
                System.Unlock(1);
            }

            public int Count(string itemId)
                => Inventory.TryGetValue(itemId, out int count) ? count : 0;

            public void Run(string recipeId)
            {
                Assert.Contains(recipeId, Catalog.GetByCategory("glass_works")
                    .ConvertAll(product => product.product_id));
                Assert.Contains("Heat started", System.StartProduction(recipeId, 3, 0.8f, 2));

                int day = 3;
                for (int guard = 0; guard < 20 && System.HeatStage != FoundryHeatStage.Complete; guard++, day++)
                {
                    System.TickDaily(day);
                    if (System.HeatStage == FoundryHeatStage.AtHeat && guard > 0)
                        System.TapAndCast(day);
                }
            }
        }

        [Fact]
        public void Catalog_LoadsAndMergesAuthoredGlassworksProducts()
        {
            var h = new Harness();

            Assert.Empty(h.Glassworks.Errors);
            Assert.Equal(2, h.Glassworks.Recipes.Count);
            Assert.NotNull(h.Glassworks.GetRecipe("foundry_prod_glass_borosilicate_blank"));
            Assert.Equal(2, h.Catalog.GetByCategory("glass_works").Count);
        }

        [Fact]
        public void StartGlassworks_MissingChargeIsAtomic()
        {
            var h = new Harness();
            h.Inventory["solar_cell"] = 0;
            int solarBefore = h.Count("solar_cell");
            int fuelBefore = h.Count(SilentFoundryIds.ItemCoal);

            string result = h.System.StartProduction(
                "foundry_prod_glass_borosilicate_blank", 2, 0.7f, 2);

            Assert.Contains("Missing charge material", result);
            Assert.Equal(solarBefore, h.Count("solar_cell"));
            Assert.Equal(fuelBefore, h.Count(SilentFoundryIds.ItemCoal));
            Assert.Equal(FoundryHeatStage.Idle, h.System.HeatStage);
        }

        [Fact]
        public void Glassworks_ProducesCanonicalBorosilicateBlank()
        {
            var h = new Harness();
            int before = h.Count("item_cast_borosilicate_glass_blank");

            h.Run("foundry_prod_glass_borosilicate_blank");

            Assert.Single(h.Completed);
            Assert.Equal(before + 1, h.Count("item_cast_borosilicate_glass_blank"));
        }

        [Fact]
        public void Glassworks_ProducesCanonicalViewportGlass()
        {
            var h = new Harness();
            int before = h.Count("item_laminated_ballistic_viewport_glass");

            h.Run("foundry_prod_glass_laminated_viewport");

            Assert.Single(h.Completed);
            Assert.Equal(before + 1, h.Count("item_laminated_ballistic_viewport_glass"));
        }

        [Fact]
        public void Glassworks_SameSeedProducesSameRecord()
        {
            var a = new Harness(4400);
            var b = new Harness(4400);

            a.Run("foundry_prod_glass_borosilicate_blank");
            b.Run("foundry_prod_glass_borosilicate_blank");

            Assert.Single(a.Completed);
            Assert.Single(b.Completed);
            Assert.Equal(a.Completed[0].tier, b.Completed[0].tier);
            Assert.Equal(a.System.State.pendingQuality, b.System.State.pendingQuality);
        }
    }
}
