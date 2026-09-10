// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Crafting;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using Xunit;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Flagship Trapping Integration — Task 1:
    /// Trap Identity Convergence, Recipe Output Mapping, and Deployment Consumption.
    /// </summary>
    public sealed class WildlifeTrapCatalogMappingTests
    {
        private static string FindDataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out var dir)) return dir;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dir)) return dir;
            return "Assets/StreamingAssets/Data";
        }

        private static (ItemCatalog items, List<Recipe> recipes, WildlifeTrappingCatalog traps) LoadAuthority()
        {
            string dataDir = FindDataDir();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var items = ItemCatalogLoader.LoadCatalog(dataDir, fileIO, json);
            var recipes = RecipeCatalogLoader.Load(dataDir, fileIO, json, items);
            var traps = WildlifeTrappingCatalogLoader.Load(dataDir, fileIO, json);
            Assert.NotNull(traps);
            return (items, recipes, traps!);
        }

        // ====================================================================
        // WORKSTREAM A: Trap definition ID to inventory item ID convergence
        // ====================================================================

        [Fact]
        public void AllAuthoredTraps_HaveMatchingItemInItemsCatalog()
        {
            var (items, _, traps) = LoadAuthority();
            Assert.NotEmpty(traps.Traps);

            foreach (var trapDef in traps.Traps.Values)
            {
                var item = items.Get(trapDef.trap_id);
                Assert.True(item != null, $"Trap '{trapDef.trap_id}' must exist as an item in items.json");
                Assert.Equal(trapDef.trap_id, item!.id);
            }
        }

        [Theory]
        [InlineData("trap_improvised_wire")]
        [InlineData("trap_box")]
        [InlineData("trap_fish")]
        [InlineData("trap_body_grip")]
        [InlineData("trap_snare")]
        [InlineData("trap_deadfall")]
        [InlineData("trap_pit")]
        [InlineData("trap_net")]
        [InlineData("trap_cage")]
        [InlineData("trap_bird_snare")]
        public void CanonicalTrap_ExistsInBothCatalogsWithExactIdMatch(string trapId)
        {
            var (items, _, traps) = LoadAuthority();
            Assert.True(traps.Traps.ContainsKey(trapId), $"Trap '{trapId}' missing in wildlife_trapping_catalog.json");
            var item = items.Get(trapId);
            Assert.NotNull(item);
            Assert.Equal(trapId, item!.id);
        }

        [Fact]
        public void TrapCatalogValidator_RejectsDuplicateTrapId()
        {
            string scratch = Path.Combine(Path.GetTempPath(), "trap_dupe_" + Guid.NewGuid().ToString("N"));
            try
            {
                Directory.CreateDirectory(scratch);
                File.WriteAllText(Path.Combine(scratch, "wildlife_trapping_catalog.json"),
                    "{\"schema_version\":1,\"traps\":[" +
                    "{\"trap_id\":\"trap_box\",\"trapType\":\"box\",\"compatiblePrey\":[\"rabbit\"]}," +
                    "{\"trap_id\":\"trap_box\",\"trapType\":\"box\",\"compatiblePrey\":[\"rabbit\"]}" +
                    "],\"prey\":[{\"speciesId\":\"rabbit\",\"preferredTrapType\":\"box\",\"attractedByBaitIds\":[\"bait_test\"]}]," +
                    "\"baits\":[{\"baitId\":\"bait_test\",\"catchBonusMultiplier\":1.2,\"toxicReduction\":0.0}]}");

                var report = new CatalogIntegrityReport();
                CatalogIntegrityValidator.ValidateWildlifeTrappingCatalog(scratch, new FileSystemIO(), report);
                Assert.Contains(report.Errors, e => e.Contains("duplicate trap_id 'trap_box'"));
            }
            finally
            {
                if (Directory.Exists(scratch)) Directory.Delete(scratch, true);
            }
        }

        [Fact]
        public void TrapCatalogValidator_RejectsTrapWithNoMatchingItem_WhenItemsCatalogExists()
        {
            string scratch = Path.Combine(Path.GetTempPath(), "trap_no_item_" + Guid.NewGuid().ToString("N"));
            try
            {
                Directory.CreateDirectory(scratch);
                File.WriteAllText(Path.Combine(scratch, "items.json"),
                    "{\"schema_version\":1,\"items\":[{\"id\":\"trap_valid\",\"name\":\"Valid Trap\"}]}");
                File.WriteAllText(Path.Combine(scratch, "wildlife_trapping_catalog.json"),
                    "{\"schema_version\":1,\"traps\":[" +
                    "{\"trap_id\":\"trap_unregistered\",\"trapType\":\"box\",\"compatiblePrey\":[\"rabbit\"]}" +
                    "],\"prey\":[{\"speciesId\":\"rabbit\",\"preferredTrapType\":\"box\",\"attractedByBaitIds\":[\"bait_test\"]}]," +
                    "\"baits\":[{\"baitId\":\"bait_test\",\"catchBonusMultiplier\":1.2,\"toxicReduction\":0.0}]}");

                var report = new CatalogIntegrityReport();
                CatalogIntegrityValidator.ValidateWildlifeTrappingCatalog(scratch, new FileSystemIO(), report);
                Assert.Contains(report.Errors, e => e.Contains("trap 'trap_unregistered' not found in items catalog"));
            }
            finally
            {
                if (Directory.Exists(scratch)) Directory.Delete(scratch, true);
            }
        }

        [Fact]
        public void TrapCatalogValidator_RejectsUndefinedMigrationSpeciesReference()
        {
            string scratch = Path.Combine(Path.GetTempPath(), "trap_bad_migration_" + Guid.NewGuid().ToString("N"));
            try
            {
                Directory.CreateDirectory(scratch);
                File.WriteAllText(Path.Combine(scratch, "wildlife_ecosystem.json"),
                    "{\"schema_version\":1,\"species\":[{\"id\":\"species_known\"}]}" );
                File.WriteAllText(Path.Combine(scratch, "wildlife_trapping_catalog.json"),
                    "{\"schema_version\":1,\"traps\":[" +
                    "{\"trap_id\":\"trap_box\",\"trapType\":\"box\",\"compatiblePrey\":[\"rabbit\"]}]," +
                    "\"prey\":[{\"speciesId\":\"rabbit\",\"preferredTrapType\":\"box\",\"migrationSpeciesId\":\"species_missing\",\"attractedByBaitIds\":[\"bait_test\"]}]," +
                    "\"baits\":[{\"baitId\":\"bait_test\",\"catchBonusMultiplier\":1.2,\"toxicReduction\":0.0}]}" );

                var report = new CatalogIntegrityReport();
                CatalogIntegrityValidator.ValidateWildlifeTrappingCatalog(scratch, new FileSystemIO(), report);
                Assert.Contains(report.Errors, e => e.Contains("prey 'rabbit'") && e.Contains("species_missing"));
            }
            finally
            {
                if (Directory.Exists(scratch)) Directory.Delete(scratch, true);
            }
        }

        // ====================================================================
        // WORKSTREAM B: Craftable recipe output mapping
        // ====================================================================

        [Fact]
        public void AllTrapCraftingRecipes_ProduceValidTrapDefinitionId()
        {
            var (_, recipes, traps) = LoadAuthority();
            var trapRecipes = recipes.Where(r => r.id.StartsWith("craft_trap_", StringComparison.Ordinal)).ToList();
            Assert.NotEmpty(trapRecipes);

            foreach (var recipe in trapRecipes)
            {
                Assert.NotNull(recipe.result);
                Assert.False(string.IsNullOrEmpty(recipe.result.id), $"Recipe '{recipe.id}' must have result item");
                Assert.True(traps.Traps.ContainsKey(recipe.result.id),
                    $"Recipe '{recipe.id}' produces '{recipe.result.id}' which must exist in wildlife_trapping_catalog.json");
            }
        }

        [Theory]
        [InlineData("craft_trap_improvised_wire", "trap_improvised_wire")]
        [InlineData("craft_trap_box", "trap_box")]
        [InlineData("craft_trap_fish", "trap_fish")]
        public void KnownCraftableTraps_ProduceExactMatchingResultItem(string recipeId, string expectedResultItemId)
        {
            var (_, recipes, traps) = LoadAuthority();
            var recipe = recipes.FirstOrDefault(r => r.id == recipeId);
            Assert.NotNull(recipe);
            Assert.NotNull(recipe!.result);
            Assert.Equal(expectedResultItemId, recipe.result.id);
            Assert.True(traps.Traps.ContainsKey(expectedResultItemId));
        }

        [Fact]
        public void NonCraftableTraps_AreSpecialistOrLootScavengeOnly()
        {
            var (_, recipes, traps) = LoadAuthority();
            string[] specialistTrapIds =
            {
                "trap_snare", "trap_deadfall", "trap_pit", "trap_net",
                "trap_cage", "trap_bird_snare", "trap_body_grip"
            };

            foreach (var trapId in specialistTrapIds)
            {
                Assert.True(traps.Traps.ContainsKey(trapId), $"Specialist trap '{trapId}' missing in catalog");
                Assert.DoesNotContain(recipes, r => r.result != null && r.result.id == trapId);
            }
        }

        // ====================================================================
        // WORKSTREAM C: Runtime deployment proofs
        // ====================================================================

        [Theory]
        [InlineData("trap_improvised_wire")]
        [InlineData("trap_box")]
        [InlineData("trap_fish")]
        [InlineData("trap_body_grip")]
        public void DeployTrap_WhenItemInInventory_ConsumesExactlyOneTrapItem(string trapId)
        {
            var (items, _, traps) = LoadAuthority();
            var trapDef = traps.Traps[trapId];

            var inv = new InventoryContainer();
            inv.Add(items.Get(trapId)!, 2);

            foreach (var cost in trapDef.setupCosts)
            {
                if (!string.IsNullOrEmpty(cost.itemId) && cost.amount > 0)
                    inv.Add(items.Get(cost.itemId)!, cost.amount * 5);
            }

            var bill = new InventoryBill();
            if (inv.CountById(trapId) >= 1)
            {
                bill.AddCost(trapId, 1);
            }
            else
            {
                bill = trapDef.CalculateSetupBill();
            }

            using var tx = inv.BeginTransaction(bill);
            Assert.True(tx.Validation.IsValid);
            Assert.True(tx.TryCommit());

            Assert.Equal(1, inv.CountById(trapId));

            foreach (var cost in trapDef.setupCosts)
            {
                if (!string.IsNullOrEmpty(cost.itemId) && cost.amount > 0 && cost.itemId != trapId)
                    Assert.Equal(cost.amount * 5, inv.CountById(cost.itemId));
            }
        }

        [Theory]
        [InlineData("trap_box")]
        [InlineData("trap_fish")]
        [InlineData("trap_snare")]
        public void DeployTrap_WhenItemNotInInventory_ConsumesAuthoredSetupCosts(string trapId)
        {
            var (items, _, traps) = LoadAuthority();
            var trapDef = traps.Traps[trapId];

            var inv = new InventoryContainer();
            Assert.Equal(0, inv.CountById(trapId));

            foreach (var cost in trapDef.setupCosts)
            {
                inv.Add(items.Get(cost.itemId)!, cost.amount);
            }

            var bill = new InventoryBill();
            if (inv.CountById(trapId) >= 1)
            {
                bill.AddCost(trapId, 1);
            }
            else
            {
                bill = trapDef.CalculateSetupBill();
            }

            using var tx = inv.BeginTransaction(bill);
            Assert.True(tx.Validation.IsValid);
            Assert.True(tx.TryCommit());

            Assert.Equal(0, inv.CountById(trapId));

            foreach (var cost in trapDef.setupCosts)
            {
                Assert.Equal(0, inv.CountById(cost.itemId));
            }
        }

        [Fact]
        public void DeployTrap_InsufficientMaterials_FailsTransactionAndLeavesInventoryUntouched()
        {
            var (items, _, traps) = LoadAuthority();
            var trapDef = traps.Traps["trap_box"];

            var inv = new InventoryContainer();
            inv.Add(items.Get("scrap_wood")!, 1);

            var bill = new InventoryBill();
            if (inv.CountById("trap_box") >= 1)
            {
                bill.AddCost("trap_box", 1);
            }
            else
            {
                bill = trapDef.CalculateSetupBill();
            }

            using var tx = inv.BeginTransaction(bill);
            Assert.False(tx.Validation.IsValid);
            tx.Cancel();

            Assert.Equal(1, inv.CountById("scrap_wood"));
        }

        [Theory]
        [InlineData("trap_improvised_wire", 1, 3)]
        [InlineData("trap_box", 2, 15)]
        [InlineData("trap_fish", 3, 12)]
        [InlineData("trap_pit", 4, 10)]
        public void SetTrap_InitializesDurabilityAndCheckInterval_FromCatalogDefinition(
            string trapId, int expectedInterval, int expectedDurability)
        {
            var (_, _, traps) = LoadAuthority();
            var trapDef = traps.Traps[trapId];
            Assert.Equal(expectedInterval, trapDef.checkIntervalDays);
            Assert.Equal(expectedDurability, trapDef.durabilityChecks);

            var sys = new WildlifeTrappingSystem(new SeededRng(100));
            var result = sys.SetTrap("site_alpha", "bait_scrap_meat", "hunter_01",
                trapDef.trapType, trapDef.trap_id, trapDef.checkIntervalDays, trapDef.durabilityChecks);

            Assert.True(result.IsSuccess);
            var site = sys.State.trapSites.Single(s => s.siteId == "site_alpha");
            Assert.Equal(trapDef.trap_id, site.trapId);
            Assert.Equal(trapDef.trapType, site.trapType);
            Assert.Equal(expectedInterval, site.checkIntervalDays);
            Assert.Equal(expectedDurability, site.remainingDurability);
            Assert.False(site.isBroken);
        }
    }
}
