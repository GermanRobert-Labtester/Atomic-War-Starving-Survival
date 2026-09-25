// SPDX-License-Identifier: MIT
// Alpha feature F1 — Salvage Teardown Bench: catalog validation, atomicity,
// tool wear via the existing durability owner, and inventory outcomes.
using System.Collections.Generic;
using System.IO;

using System.Text.Json;
using Ashfall.Core.Inventory;
using Xunit;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests.SalvageTeardown
{
    public sealed class SalvageTeardownSystemTests
    {
        private const string CatalogJson = """
        {
          "schema_version": 1,
          "collection_id": "salvage_test",
          "recipes": [
            {
              "id": "teardown_tool_a",
              "source_item_id": "tool_a",
              "display_name": "Tool A",
              "components": [ { "item_id": "part_x", "amount": 2 }, { "item_id": "part_y", "amount": 1 } ]
            },
            {
              "id": "teardown_tool_b",
              "source_item_id": "tool_b",
              "display_name": "Tool B",
              "components": [ { "item_id": "part_x", "amount": 1 } ],
              "required_tool_id": "bench_cutter",
              "tool_wear": 10
            }
          ]
        }
        """;

        private static SalvageTeardownSystem BuildSystem()
        {
            var catalog = SalvageTeardownCatalog.Load(CatalogJson);
            Assert.Empty(catalog.Errors);
            return new SalvageTeardownSystem(catalog, id => new ItemDefinition
            {
                id = id,
                displayName = id,
                stackMax = 99,
                weight = 0.1f,
            });
        }

        [Fact]
        public void Catalog_Loads_Recipes_And_Rejects_Empty_Components()
        {
            var catalog = SalvageTeardownCatalog.Load(CatalogJson);
            Assert.Equal(2, catalog.Count);
            Assert.True(catalog.TryGet("tool_a", out var recipe));
            Assert.Equal(2, recipe.Components.Count);
            Assert.True(recipe.RequiresTool == false);

            var broken = SalvageTeardownCatalog.Load("""
            { "schema_version": 1, "recipes": [ { "id": "x", "source_item_id": "y", "components": [] } ] }
            """);
            Assert.Equal(0, broken.Count);
            Assert.NotEmpty(broken.Errors);
        }

        [Fact]
        public void NoRecipe_IsReported_WithoutMutation()
        {
            var system = BuildSystem();
            var inv = new InventoryContainer();
            inv.Add(new ItemDefinition { id = "tool_a", stackMax = 99 }, 1);

            var result = system.TryTeardown(inv, "unlisted_item");

            Assert.Equal(SalvageOutcome.NoRecipe, result.Outcome);
            Assert.Equal(1, inv.CountById("tool_a"));
            Assert.Equal(0, inv.CountById("part_x"));
        }

        [Fact]
        public void MissingSource_LeavesInventory_Unchanged()
        {
            var system = BuildSystem();
            var inv = new InventoryContainer();

            var result = system.TryTeardown(inv, "tool_a");

            Assert.Equal(SalvageOutcome.InsufficientSource, result.Outcome);
            Assert.Equal(0, inv.CountById("part_x"));
        }

        [Fact]
        public void Teardown_ConsumesSource_And_YieldsComponents()
        {
            var system = BuildSystem();
            var inv = new InventoryContainer();
            inv.Add(new ItemDefinition { id = "tool_a", stackMax = 99 }, 3);

            var result = system.TryTeardown(inv, "tool_a", 2);

            Assert.True(result.IsSuccess);
            Assert.Equal(1, inv.CountById("tool_a"));
            Assert.Equal(4, inv.CountById("part_x"));
            Assert.Equal(2, inv.CountById("part_y"));
        }

        [Fact]
        public void RequiredTool_Missing_Blocks_Teardown()
        {
            var system = BuildSystem();
            var inv = new InventoryContainer();
            inv.Add(new ItemDefinition { id = "tool_b", stackMax = 99 }, 1);

            var result = system.TryTeardown(inv, "tool_b");

            Assert.Equal(SalvageOutcome.MissingTool, result.Outcome);
            Assert.Equal(1, inv.CountById("tool_b"));
            Assert.Equal(0, inv.CountById("part_x"));
        }

        [Fact]
        public void RequiredTool_Is_Worn_Through_The_Slot_Durability_Owner()
        {
            var system = BuildSystem();
            var inv = new InventoryContainer();
            inv.Add(new ItemDefinition { id = "tool_b", stackMax = 99 }, 1);
            var cutter = new ItemDefinition { id = "bench_cutter", stackMax = 1, durability = 40f };
            inv.Add(cutter, 1);
            var slot = inv.FindSlot("bench_cutter");
            Assert.NotNull(slot);
            slot!.CurrentDurability = 40f;

            var result = system.TryTeardown(inv, "tool_b");

            Assert.True(result.IsSuccess);
            Assert.Equal("bench_cutter", result.WornToolId);
            Assert.Equal(10f, result.ToolWear);
            Assert.Equal(30f, inv.FindSlot("bench_cutter")!.CurrentDurability);
            Assert.Equal(1, inv.CountById("part_x"));
        }

        [Fact]
        public void WeightLimit_Blocks_Atomically_NoConsumeNoYield()
        {
            var system = BuildSystem();
            var inv = new InventoryContainer { MaxWeight = 0.25f };
            inv.Add(new ItemDefinition { id = "tool_a", stackMax = 99, weight = 0.1f }, 1);

            var result = system.TryTeardown(inv, "tool_a");

            Assert.Equal(SalvageOutcome.NoCapacity, result.Outcome);
            Assert.Equal(1, inv.CountById("tool_a"));
            Assert.Equal(0, inv.CountById("part_x"));
        }

        [Fact]
        public void DuplicateRecipe_Is_Rejected_With_Error()
        {
            var catalog = SalvageTeardownCatalog.Load("""
            {
              "schema_version": 1,
              "recipes": [
                { "id": "a", "source_item_id": "tool_a", "components": [ { "item_id": "part_x", "amount": 1 } ] },
                { "id": "b", "source_item_id": "tool_a", "components": [ { "item_id": "part_y", "amount": 1 } ] }
              ]
            }
            """);
            Assert.Equal(1, catalog.Count);
            Assert.NotEmpty(catalog.Errors);
        }

        [Fact]
        public void Shipped_Recipes_Preview_Assets_All_Resolve_In_Items_Json()
        {
            // Alpha feature G1 — the salvage yield preview promises real names
            // and a real tool, so every shipped recipe component and tool id
            // must resolve in the item authority. Verify the premise, not the
            // formatting.
            string dataDir;
            if (!CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out dataDir!))
                dataDir = System.AppContext.BaseDirectory;
            if (!CatalogLocator.TryFindDataDirectory(dataDir, out dataDir!))
                throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");

            var catalog = SalvageTeardownCatalog.Load(File.ReadAllText(Path.Combine(dataDir, SalvageTeardownCatalog.FileName)));
            Assert.Empty(catalog.Errors);
            Assert.True(catalog.Count >= 10, "expected the shipped teardown bench to carry recipes");

            var itemIds = new HashSet<string>();
            using (var doc = JsonDocument.Parse(File.ReadAllText(Path.Combine(dataDir, "items.json"))))
            {
                foreach (var element in doc.RootElement.GetProperty("items").EnumerateArray())
                {
                    if (element.TryGetProperty("id", out var id)) itemIds.Add(id.GetString() ?? string.Empty);
                }
            }

            foreach (var recipe in catalog.All)
            {
                foreach (var component in recipe.Components)
                    Assert.True(itemIds.Contains(component.ItemId),
                        $"recipe '{recipe.Id}' yields '{component.ItemId}' which does not resolve in items.json");
                if (recipe.RequiredToolId != null)
                    Assert.True(itemIds.Contains(recipe.RequiredToolId),
                        $"recipe '{recipe.Id}' requires tool '{recipe.RequiredToolId}' which does not resolve in items.json");
            }
        }
    }
}
