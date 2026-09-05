// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Content;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests.WildlifeTrapping
{
    /// <summary>
    /// Flagship Task 7: Content Utilization Suite for Trapping Domain.
    ///
    /// Non-Negotiable Contract:
    /// - Every authored trap in wildlife_trapping_catalog.json has a valid item in items.json.
    /// - Every trap has at least one live acquisition route (recipe, trade, or expedition).
    /// - Every prey has at least one compatible trap; every bait has at least one attracted prey.
    /// - ContentUtilizationScanner registers wildlife_trapping_catalog.json with zero orphaned content.
    /// </summary>
    public sealed class WildlifeTrappingContentUtilizationTests : CatalogTestBase
    {
        private (WildlifeTrappingCatalog catalog, HashSet<string> itemIds, List<RecipeDto> recipes, List<string> economyItems, List<(string expId, string itemRef)> expLoot) LoadData()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var catalog = WildlifeTrappingCatalogLoader.Load(DataDirectory, files, json);
            Assert.NotNull(catalog);

            // Load all items
            var itemIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var itemFile in Directory.GetFiles(DataDirectory, "*item*.json"))
            {
                var doc = JsonDocument.Parse(File.ReadAllText(itemFile));
                if (doc.RootElement.TryGetProperty("items", out var items))
                {
                    foreach (var it in items.EnumerateArray())
                    {
                        if (it.TryGetProperty("id", out var idProp))
                        {
                            string id = idProp.GetString() ?? "";
                            if (!string.IsNullOrEmpty(id)) itemIds.Add(id);
                        }
                    }
                }
            }

            // Load recipes
            var recipes = new List<RecipeDto>();
            string recipesPath = Path.Combine(DataDirectory, "recipes.json");
            if (File.Exists(recipesPath))
            {
                var doc = JsonDocument.Parse(File.ReadAllText(recipesPath));
                if (doc.RootElement.TryGetProperty("recipes", out var recArray))
                {
                    foreach (var r in recArray.EnumerateArray())
                    {
                        string rid = r.GetProperty("id").GetString() ?? "";
                        string resId = r.GetProperty("resultItemId").GetString() ?? "";
                        recipes.Add(new RecipeDto { Id = rid, ResultItemId = resId });
                    }
                }
            }

            // Load economy goods
            var econItems = new List<string>();
            string econPath = Path.Combine(DataDirectory, "economy_goods.json");
            if (File.Exists(econPath))
            {
                var doc = JsonDocument.Parse(File.ReadAllText(econPath));
                if (doc.RootElement.TryGetProperty("goods", out var goodsArray))
                {
                    foreach (var g in goodsArray.EnumerateArray())
                    {
                        string id = g.TryGetProperty("item_id", out var ip) ? ip.GetString() ?? "" :
                                    g.TryGetProperty("id", out var idp) ? idp.GetString() ?? "" : "";
                        if (!string.IsNullOrEmpty(id)) econItems.Add(id);
                    }
                }
            }

            // Load expedition loot
            var expLoot = new List<(string, string)>();
            string expPath = Path.Combine(DataDirectory, "expeditions.json");
            if (File.Exists(expPath))
            {
                var doc = JsonDocument.Parse(File.ReadAllText(expPath));
                if (doc.RootElement.TryGetProperty("expeditions", out var exps))
                {
                    foreach (var e in exps.EnumerateArray())
                    {
                        string eid = e.GetProperty("id").GetString() ?? "";
                        if (e.TryGetProperty("lootCategories", out var cats))
                        {
                            foreach (var c in cats.EnumerateArray())
                            {
                                string cat = c.GetString() ?? "";
                                if (!string.IsNullOrEmpty(cat)) expLoot.Add((eid, cat));
                            }
                        }
                    }
                }
            }

            return (catalog!, itemIds, recipes, econItems, expLoot);
        }

        public sealed class RecipeDto
        {
            public string Id { get; set; } = string.Empty;
            public string ResultItemId { get; set; } = string.Empty;
        }

        [Fact]
        public void AllTraps_HaveCorrespondingItemInItemsJson()
        {
            var (catalog, itemIds, _, _, _) = LoadData();
            foreach (var trap in catalog.Traps.Values)
            {
                Assert.True(itemIds.Contains(trap.trap_id),
                    $"Trap definition '{trap.trap_id}' has no corresponding item in items catalogs.");
            }
        }

        [Fact]
        public void AllTrapItems_HaveCorrespondingTrapDefinition()
        {
            var (catalog, _, _, _, _) = LoadData();
            string[] knownTrapItems = new[]
            {
                "trap_snare", "trap_deadfall", "trap_pit", "trap_net", "trap_fish",
                "trap_cage", "trap_bird_snare", "trap_body_grip", "trap_box", "trap_improvised_wire"
            };

            foreach (var itemId in knownTrapItems)
            {
                Assert.True(catalog.Traps.ContainsKey(itemId),
                    $"Trap item '{itemId}' has no corresponding TrapDefinition in wildlife_trapping_catalog.json");
            }
        }

        [Fact]
        public void AllTraps_HaveAtLeastOneAcquisitionRoute()
        {
            var (catalog, _, recipes, econItems, expLoot) = LoadData();
            var recipeOutputs = recipes.Select(r => r.ResultItemId).ToHashSet(StringComparer.OrdinalIgnoreCase);
            var econSet = econItems.ToHashSet(StringComparer.OrdinalIgnoreCase);
            var expLootSet = expLoot.Select(e => e.itemRef).ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var trap in catalog.Traps.Values)
            {
                bool hasRecipe = recipeOutputs.Contains(trap.trap_id);
                bool hasTrade = econSet.Contains(trap.trap_id);
                bool hasExpedition = expLootSet.Contains(trap.trap_id);

                Assert.True(hasRecipe || hasTrade || hasExpedition,
                    $"Orphan trap definition '{trap.trap_id}': 0 acquisition routes found.");
            }
        }

        [Fact]
        public void ImprovisedWireSnare_HasCraftingRecipe()
        {
            var (_, _, recipes, _, _) = LoadData();
            Assert.Contains(recipes, r => r.ResultItemId == "trap_improvised_wire");
        }

        [Fact]
        public void BoxTrap_HasCraftingRecipeAndTradeEntry()
        {
            var (_, _, recipes, econItems, _) = LoadData();
            Assert.Contains(recipes, r => r.ResultItemId == "trap_box");
            Assert.Contains("trap_box", econItems);
        }

        [Fact]
        public void FishTrap_HasCraftingRecipeAndExpeditionLoot()
        {
            var (_, _, recipes, _, expLoot) = LoadData();
            Assert.Contains(recipes, r => r.ResultItemId == "trap_fish");
            Assert.Contains(expLoot, e => e.itemRef == "trap_fish" && e.expId == "loc_water_station");
        }

        [Fact]
        public void BodyGripTrap_HasExpeditionLootRoute()
        {
            var (_, _, _, _, expLoot) = LoadData();
            Assert.Contains(expLoot, e => e.itemRef == "trap_body_grip" && e.expId == "ruined_garage");
        }

        [Fact]
        public void WireSnare_HasCraftingRecipe()
        {
            var (_, _, recipes, _, _) = LoadData();
            Assert.Contains(recipes, r => r.ResultItemId == "trap_snare");
        }

        [Fact]
        public void AllPrey_HaveAtLeastOneCompatibleTrap()
        {
            var (catalog, _, _, _, _) = LoadData();
            var compatiblePreyAll = new HashSet<string>(StringComparer.Ordinal);
            foreach (var t in catalog.Traps.Values)
            {
                if (t.compatiblePrey != null)
                {
                    foreach (var cp in t.compatiblePrey)
                        compatiblePreyAll.Add(cp);
                }
            }

            foreach (var prey in catalog.Prey.Values)
            {
                Assert.True(compatiblePreyAll.Contains(prey.speciesId),
                    $"Prey species '{prey.speciesId}' has no compatible trap definition.");
            }
        }

        [Fact]
        public void AllBaits_AreAttractedByAtLeastOnePrey()
        {
            var (catalog, _, _, _, _) = LoadData();
            var attractedBaits = new HashSet<string>(StringComparer.Ordinal);
            foreach (var p in catalog.Prey.Values)
            {
                if (p.attractedByBaitIds != null)
                {
                    foreach (var b in p.attractedByBaitIds)
                        attractedBaits.Add(b);
                }
            }

            foreach (var bait in catalog.Baits.Values)
            {
                Assert.True(attractedBaits.Contains(bait.baitId),
                    $"Bait profile '{bait.baitId}' is not listed in any prey attractedByBaitIds list.");
            }
        }

        [Fact]
        public void CraftingRecipes_ProduceRecognizedTrapItems()
        {
            var (catalog, itemIds, recipes, _, _) = LoadData();
            var trapRecipes = recipes.Where(r => r.ResultItemId.StartsWith("trap_", StringComparison.Ordinal)).ToList();

            Assert.NotEmpty(trapRecipes);
            foreach (var r in trapRecipes)
            {
                Assert.True(itemIds.Contains(r.ResultItemId),
                    $"Recipe '{r.Id}' produces uncatalogued item '{r.ResultItemId}'");
                Assert.True(catalog.Traps.ContainsKey(r.ResultItemId),
                    $"Recipe '{r.Id}' produces item '{r.ResultItemId}' which has no TrapDefinition");
            }
        }

        [Fact]
        public void ExpeditionLoot_ResolvesToExistingTrapItems()
        {
            var (catalog, itemIds, _, _, expLoot) = LoadData();
            var trapLoot = expLoot.Where(e => e.itemRef.StartsWith("trap_", StringComparison.Ordinal)).ToList();

            Assert.NotEmpty(trapLoot);
            foreach (var (expId, itemRef) in trapLoot)
            {
                Assert.True(itemIds.Contains(itemRef),
                    $"Expedition '{expId}' contains uncatalogued trap loot '{itemRef}'");
                Assert.True(catalog.Traps.ContainsKey(itemRef),
                    $"Expedition '{expId}' contains trap loot '{itemRef}' with no TrapDefinition");
            }
        }

        [Fact]
        public void TrappingCatalog_IsRegisteredInContentUtilizationScanner()
        {
            Assert.True(ContentUtilizationScanner.IsAuthoritativeCatalog("wildlife_trapping_catalog.json"),
                "wildlife_trapping_catalog.json must be registered in ContentUtilizationScanner.AuthoritativeCatalogs");
        }

        [Fact]
        public void ContentUtilizationScanner_ReportsZeroOrphansForTrappingContent()
        {
            var scanner = new ContentUtilizationScanner(
                repoRoot: Path.GetFullPath(Path.Combine(DataDirectory, "../../..")),
                dataDir: DataDirectory,
                coreDir: Path.GetFullPath(Path.Combine(DataDirectory, "../../Ashfall.Core")),
                srcDir: Path.GetFullPath(Path.Combine(DataDirectory, "../../../src")));

            var report = scanner.Scan();
            Assert.True(report.OrphanedCatalogs == 0,
                $"Expected 0 orphans from content utilization scan, found {report.OrphanedCatalogs}");
        }

        [Fact]
        public void TrappingContentGraph_PreservesDeterministicKeyOrdering()
        {
            var (catalog, _, _, _, _) = LoadData();
            var sortedTraps = catalog.Traps.Keys.OrderBy(k => k, StringComparer.Ordinal).ToList();
            var sortedPrey = catalog.Prey.Keys.OrderBy(k => k, StringComparer.Ordinal).ToList();
            var sortedBaits = catalog.Baits.Keys.OrderBy(k => k, StringComparer.Ordinal).ToList();

            Assert.Equal(10, sortedTraps.Count);
            Assert.Equal(15, sortedPrey.Count);
            Assert.Equal(6, sortedBaits.Count);

            // Re-evaluating order produces identical sequence
            Assert.Equal(sortedTraps, catalog.Traps.Keys.OrderBy(k => k, StringComparer.Ordinal).ToList());
            Assert.Equal(sortedPrey, catalog.Prey.Keys.OrderBy(k => k, StringComparer.Ordinal).ToList());
            Assert.Equal(sortedBaits, catalog.Baits.Keys.OrderBy(k => k, StringComparer.Ordinal).ToList());
        }
    }
}
