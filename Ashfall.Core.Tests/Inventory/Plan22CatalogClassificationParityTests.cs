// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests.Plan22ConsumableBills
{
    /// <summary>
    /// C2 / Plan 22 — real-catalog classification parity: after retiring the
    /// hardcoded medical ID list in CraftingSystem, every recipe in the live
    /// data must classify identically to the retired oracle
    /// (type == Medical OR id ∈ {bandage, morphine, anti_rad, rad_away,
    /// antibiotics, iodine_pills}). Data-dependent — requires a valid
    /// items.json (skip-worthy only if the data file is broken, which the
    /// integrity gate separately reports).
    /// </summary>
    public sealed class Plan22CatalogClassificationParityTests
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
                dir = dir.Parent!;
            }
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        private static readonly string[] RetiredLiteralList =
        {
            "bandage", "morphine", "anti_rad", "rad_away", "antibiotics", "iodine_pills"
        };

        private static bool RetiredOracle(ItemDefinition def)
        {
            if (def == null) return false;
            if (def.type == ItemType.Medical) return true;
            return Array.IndexOf(RetiredLiteralList, def.id) >= 0;
        }

        [Fact]
        public void EveryProtectiveAndMedicalItem_ClassifiesIdenticallyToRetiredOracle()
        {
            var catalog = ItemCatalogLoader.LoadCatalog(
                GetDataDir(), new Ashfall.Core.FileSystemIO(), new Ashfall.Core.SystemTextJsonSerializer());
            Assert.NotNull(catalog);

            int checkedCount = 0;
            foreach (string id in catalog.Ids)
            {
                var def = catalog.Get(id);
                if (def == null) continue;
                bool oracle = RetiredOracle(def);
                bool actual = ItemTagCatalog.IsMedical(def);
                Assert.True(oracle == actual,
                    $"classification drift for '{def.id}': retired oracle={oracle}, tag authority={actual}");
                if (oracle) checkedCount++;
            }
            Assert.True(checkedCount >= 6, "expected at least the six retired medical ids in the catalog");
        }

        [Fact]
        public void AuthoredRepairBills_Parse_WithAuthoredCaps()
        {
            var catalog = LoadCatalog();
            var mask = FindDef(catalog, "gas_mask");
            var suit = FindDef(catalog, "hazmat_suit");

            Assert.NotNull(mask?.repairRecipe);
            Assert.NotNull(suit?.repairRecipe);
            Assert.Equal(0.85f, mask!.repairRecipe!.MaxRepairConditionFraction, 3);
            Assert.Equal(0.85f, suit!.repairRecipe!.MaxRepairConditionFraction, 3);
            Assert.True(mask.repairRecipe.costs.Count > 0);
            Assert.True(suit.repairRecipe.costs.Count > 0);
        }

        [Fact]
        public void MedicalTags_AreAuthored_OnTheTypeMismatchedItems()
        {
            var catalog = LoadCatalog();
            foreach (string id in RetiredLiteralList)
            {
                var def = FindDef(catalog, id);
                Assert.NotNull(def);
                // type == Medical satisfies the oracle directly; the three
                // type-mismatched items must carry the authored medical tag.
                if (def!.type != ItemType.Medical)
                    Assert.True(def.HasTag("medical"),
                        $"'{id}' relies on the retired literal list — author the medical tag");
            }
        }

        [Fact]
        public void AuthoredRepairBillCosts_ResolveToRealItems()
        {
            var catalog = LoadCatalog();
            foreach (string id in catalog.Ids)
            {
                var def = catalog.Get(id);
                if (def?.repairRecipe?.costs == null) continue;
                foreach (var cost in def.repairRecipe.costs)
                    Assert.True(catalog.Contains(cost.materialId),
                        $"repair bill on '{id}' references unknown material '{cost.materialId}'");
            }
        }

        private static ItemCatalog LoadCatalog()
            => ItemCatalogLoader.LoadCatalog(
                GetDataDir(), new Ashfall.Core.FileSystemIO(), new Ashfall.Core.SystemTextJsonSerializer());

        private static ItemDefinition? FindDef(ItemCatalog catalog, string id)
            => catalog.Get(id);
    }

    /// <summary>
    /// C2 / Plan 22 (§36) — trade parity: repair-bill materials and replacement
    /// canisters must be real trade goods (no special gear-shop pricing).
    /// </summary>
    public sealed class Plan22TradeParityTests
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
                dir = dir.Parent!;
            }
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        [Fact]
        public void RepairBillMaterials_AndCanisters_AreTradeGoods()
        {
            string goodsJson = File.ReadAllText(Path.Combine(GetDataDir(), "economy_goods.json"));
            foreach (string id in new[] { "cloth", "scrap_metal", "air_filter", "water_filter", "item_air_filter_hepa" })
                Assert.Contains("\"id\": \"" + id + "\"", goodsJson, StringComparison.Ordinal);
        }

        [Fact]
        public void RepairBillMaterials_ResolveToCatalogItems()
        {
            var catalog = ItemCatalogLoader.LoadCatalog(
                GetDataDir(), new Ashfall.Core.FileSystemIO(), new Ashfall.Core.SystemTextJsonSerializer());
            foreach (string id in new[] { "cloth", "scrap_metal", "air_filter", "item_air_filter_hepa" })
                Assert.NotNull(catalog.Get(id));
        }
    }
}
