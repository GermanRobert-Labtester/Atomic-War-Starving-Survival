using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 91 — greenhouse item catalog expansion (14 live → 30).
    /// Gates catalog count, parity of the preserved baseline, schema/numeric
    /// validity, global-ID uniqueness (Model A merged registry), crafting and
    /// scavenging reference resolution, and live-consumer honesty (no new
    /// item claims a greenhouse effect that no system consumes).
    /// </summary>
    public class GreenhouseItemCatalogTests
    {
        private static readonly string[] PreservedBaselineIds =
        {
            "item_seed_mushroom", "item_seed_tuber", "item_seed_grain", "item_seed_wheat",
            "item_planter_box", "item_grow_lamp", "item_lead_glass_pane",
            "item_blight_treatment", "item_grow_medium",
            "crop_mushroom", "crop_tuber", "crop_grain", "crop_wheat", "tainted_food"
        };

        private static readonly string[] AcceptedItemTypes =
        {
            "Food", "Water", "IrradiatedWater", "Medical", "AntiRad", "Iodine",
            "Protective", "Tool", "Fuel", "Filter", "Material", "Trade", "Comfort",
            "Quest", "Device", "Weapon", "Corpse", "ContaminatedFood", "Relic"
        };

        private static readonly string[] NewSupplyIds =
        {
            "item_greenhouse_trowel", "item_greenhouse_pruning_shears",
            "item_greenhouse_watering_can", "item_greenhouse_hand_cultivator",
            "item_greenhouse_compost", "item_greenhouse_ash_fertilizer",
            "item_greenhouse_fish_emulsion",
            "item_greenhouse_insecticidal_soap", "item_greenhouse_sticky_traps",
            "item_greenhouse_pest_mesh",
            "item_greenhouse_drip_kit", "item_greenhouse_line_filter",
            "item_greenhouse_catchment_kit",
            "item_greenhouse_glass_pane", "item_greenhouse_uv_sheeting",
            "item_greenhouse_shade_cloth"
        };

        private static string ResolveDataDir()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string dataDir = Path.GetFullPath(Path.Combine(baseDir, "../../../../Assets/StreamingAssets/Data"));
            if (!Directory.Exists(dataDir))
                dataDir = Path.GetFullPath(Path.Combine(baseDir, "../../../Assets/StreamingAssets/Data"));
            return dataDir;
        }

        private static string DataPath(string file) => Path.Combine(ResolveDataDir(), file);

        private static List<Dictionary<string, object>> LoadEntries(string file)
        {
            var json = System.Text.Json.JsonDocument.Parse(File.ReadAllText(DataPath(file)));
            var root = json.RootElement;
            var target = root;
            if (root.ValueKind == System.Text.Json.JsonValueKind.Object)
            {
                foreach (var prop in root.EnumerateObject())
                {
                    if (prop.Value.ValueKind == System.Text.Json.JsonValueKind.Array)
                    {
                        target = prop.Value;
                        break;
                    }
                }
            }
            var list = new List<Dictionary<string, object>>();
            foreach (var element in target.EnumerateArray())
                list.Add(System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(
                    element.GetRawText()));
            return list;
        }

        private static string Str(Dictionary<string, object> e, string key)
        {
            var v = e[key];
            if (v is string s) return s;
            return ((System.Text.Json.JsonElement)v).GetString()!;
        }

        private static ItemCatalog LoadGlobalCatalog()
        {
            var loader = new ItemCatalogLoaderHarness();
            return loader.Catalog;
        }

        /// <summary>Thin loader harness exposing the merged global ItemCatalog.</summary>
        private sealed class ItemCatalogLoaderHarness
        {
            public readonly ItemCatalog Catalog = new ItemCatalog();
            public ItemCatalogLoaderHarness()
            {
                ItemCatalogLoader.LoadInto(Catalog, ResolveDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            }
        }

        // ── Catalog ─────────────────────────────────────────────────────

        [Fact]
        public void GreenhouseFile_ContainsExactlyThirtyEntries()
        {
            var entries = LoadEntries("greenhouse_items.json");
            Assert.Equal(30, entries.Count);
        }

        [Fact]
        public void GreenhouseFile_PreservesOriginalFourteen()
        {
            var entries = LoadEntries("greenhouse_items.json");
            var ids = entries.Select(e => Str(e, "id")).ToHashSet();
            foreach (var id in PreservedBaselineIds)
                Assert.True(ids.Contains(id), $"baseline entry '{id}' missing");
        }

        [Fact]
        public void GreenhouseFile_ContainsAllSixteenNewSupplies()
        {
            var entries = LoadEntries("greenhouse_items.json");
            var ids = entries.Select(e => Str(e, "id")).ToHashSet();
            foreach (var id in NewSupplyIds)
                Assert.True(ids.Contains(id), $"new supply '{id}' missing");
        }

        [Fact]
        public void GreenhouseFile_HasUniqueIds()
        {
            var entries = LoadEntries("greenhouse_items.json");
            var ids = entries.Select(e => Str(e, "id")).ToList();
            Assert.Equal(ids.Count, ids.Distinct().Count());
        }

        // ── Schema / numeric validity ───────────────────────────────────

        [Fact]
        public void GreenhouseFile_AllTypesAreValidItemTypeValues()
        {
            var accepted = AcceptedItemTypes.ToHashSet(StringComparer.OrdinalIgnoreCase);
            foreach (var e in LoadEntries("greenhouse_items.json"))
            {
                var type = ((System.Text.Json.JsonElement)e["type"]).GetString();
                Assert.True(accepted.Contains(type), $"entry '{e["id"]}' has invalid type '{type}'");
            }
        }

        [Fact]
        public void GreenhouseFile_NamesAndDescriptionsNonEmpty()
        {
            foreach (var e in LoadEntries("greenhouse_items.json"))
            {
                var name = ((System.Text.Json.JsonElement)e["displayName"]).GetString();
                var desc = ((System.Text.Json.JsonElement)e["description"]).GetString();
                Assert.False(string.IsNullOrWhiteSpace(name), $"entry '{e["id"]}' empty displayName");
                Assert.False(string.IsNullOrWhiteSpace(desc), $"entry '{e["id"]}' empty description");
            }
        }

        [Fact]
        public void GreenhouseFile_NumericRangesValid()
        {
            foreach (var e in LoadEntries("greenhouse_items.json"))
            {
                var id = Str(e, "id");
                int stack = ((System.Text.Json.JsonElement)e["stackMax"]).GetInt32();
                float weight = ((System.Text.Json.JsonElement)e["weight"]).GetSingle();
                float trade = ((System.Text.Json.JsonElement)e["tradeValue"]).GetSingle();
                Assert.True(stack >= 1, $"{id} stackMax < 1");
                Assert.True(weight >= 0f, $"{id} negative weight");
                Assert.True(trade >= 0f, $"{id} negative tradeValue");
            }
        }

        [Fact]
        public void GreenhouseFile_HandToolsHaveLowStacksAndLowWeight()
        {
            foreach (var id in new[] { "item_greenhouse_trowel", "item_greenhouse_pruning_shears", "item_greenhouse_watering_can", "item_greenhouse_hand_cultivator" })
            {
                var e = LoadEntries("greenhouse_items.json").First(x => Str(x, "id") == id);
                Assert.True(((System.Text.Json.JsonElement)e["stackMax"]).GetInt32() <= 2, $"{id} tool stack too high");
                Assert.True(((System.Text.Json.JsonElement)e["weight"]).GetSingle() <= 1.0f, $"{id} hand tool implausibly heavy");
            }
        }

        [Fact]
        public void GreenhouseFile_NewSuppliesAreNotSameValueClones()
        {
            // Trade values must not collapse into one band (plan §10, §47).
            var entries = LoadEntries("greenhouse_items.json");
            var values = NewSupplyIds
                .Select(id => entries.First(e => Str(e, "id") == id))
                .Select(e => ((System.Text.Json.JsonElement)e["tradeValue"]).GetSingle())
                .ToList();
            Assert.True(values.Distinct().Count() >= 8, "new supply trade values are suspiciously uniform");
            Assert.Equal(2f, values.Min());
            Assert.Equal(18f, values.Max());
        }

        // ── Global registry (Model A) ───────────────────────────────────

        [Fact]
        public void GlobalCatalog_RegistersAllThirtyGreenhouseEntries()
        {
            var catalog = LoadGlobalCatalog();
            foreach (var e in LoadEntries("greenhouse_items.json"))
            {
                var id = Str(e, "id");
                Assert.True(catalog.Contains(id), $"global registry missing greenhouse entry '{id}'");
            }
        }

        [Fact]
        public void GlobalCatalog_NewSuppliesResolveAcrossCategories()
        {
            var catalog = LoadGlobalCatalog();
            var tool = catalog.Get("item_greenhouse_trowel");
            var amendment = catalog.Get("item_greenhouse_compost");
            var water = catalog.Get("item_greenhouse_drip_kit");
            var structural = catalog.Get("item_greenhouse_uv_sheeting");
            var filter = catalog.Get("item_greenhouse_line_filter");
            Assert.NotNull(tool);
            Assert.NotNull(amendment);
            Assert.NotNull(water);
            Assert.NotNull(structural);
            Assert.NotNull(filter);
            Assert.Equal(ItemType.Tool, tool.type);
            Assert.Equal(ItemType.Material, amendment.type);
            Assert.Equal(ItemType.Material, water.type);
            Assert.Equal(ItemType.Material, structural.type);
            Assert.Equal(ItemType.Filter, filter.type);
        }

        [Fact]
        public void GlobalCatalog_NoIdCollisionsAcrossItemFiles()
        {
            // Within a single file, duplicated ids would be an authoring error;
            // cross-file duplicates are tolerated by the loader but the
            // greenhouse file must not add new ones against the primary.
            var primary = LoadEntries("items.json").Select(e => Str(e, "id")).ToHashSet();
            foreach (var id in NewSupplyIds)
                Assert.False(primary.Contains(id), $"'{id}' already exists in items.json — duplicate definition");
        }

        [Fact]
        public void GreenhouseFile_DeadParityCopiesRemoved()
        {
            // The 16 seed/crop pairs live (improved) in items.json; the
            // greenhouse file must not carry stale second copies (plan §36).
            var gh = LoadEntries("greenhouse_items.json").Select(e => Str(e, "id")).ToHashSet();
            foreach (var dead in new[] { "item_seed_hardy_tuber", "item_seed_ash_grain", "item_seed_biolum_mushroom", "item_seed_nutrient_algae", "item_seed_medicinal_herb", "item_seed_leafy_green", "item_seed_oilseed", "item_seed_cold_legume" })
                Assert.False(gh.Contains(dead), $"'{dead}' is defined in items.json — remove the stale greenhouse copy");
        }

        [Fact]
        public void GreenhouseFile_NewSuppliesClaimNoConsumableEffectFields()
        {
            // Plan §1.11 / §49: no live consumer exists for the new supplies —
            // they must not carry effect fields that fake a mechanic.
            var forbidden = new[] { "hungerRestore", "thirstRestore", "healthEffect", "moraleEffect", "contamination", "radProtection" };
            foreach (var e in LoadEntries("greenhouse_items.json"))
            {
                var id = Str(e, "id");
                if (!NewSupplyIds.Contains(id)) continue;
                foreach (var field in forbidden)
                    Assert.False(e.ContainsKey(field), $"'{id}' carries unsupported effect field '{field}'");
            }
        }

        // ── Crafting bindings (plan §37-38, §55-58) ─────────────────────

        [Fact]
        public void Crafting_FourGreenhouseRecipesExistAndAreUnique()
        {
            var recipes = LoadEntries("recipes.json");
            var expected = new[] { "craft_greenhouse_trowel", "craft_greenhouse_watering_can", "craft_greenhouse_drip_kit", "craft_greenhouse_catchment_kit" };
            var ids = recipes.Select(r => Str(r, "id")).ToList();
            foreach (var id in expected)
                Assert.Equal(1, ids.Count(x => x == id));
        }

        [Fact]
        public void Crafting_GreenhouseRecipeOutputsResolveInGlobalRegistry()
        {
            var catalog = LoadGlobalCatalog();
            var recipes = LoadEntries("recipes.json")
                .Where(r => Str(r, "id").StartsWith("craft_greenhouse_"))
                .ToList();
            Assert.Equal(4, recipes.Count);
            foreach (var r in recipes)
            {
                var result = ((System.Text.Json.JsonElement)r["resultItemId"]).GetString();
                Assert.True(catalog.Contains(result), $"recipe '{r["id"]}' outputs unresolvable item '{result}'");
            }
        }

        [Fact]
        public void Crafting_GreenhouseRecipeIngredientsResolve()
        {
            var catalog = LoadGlobalCatalog();
            foreach (var r in LoadEntries("recipes.json").Where(r => Str(r, "id").StartsWith("craft_greenhouse_")))
            {
                var ingredients = (System.Text.Json.JsonElement)r["ingredients"];
                foreach (var ing in ingredients.EnumerateArray())
                {
                    var itemId = ing.GetProperty("itemId").GetString();
                    var amount = ing.GetProperty("amount").GetInt32();
                    Assert.True(catalog.Contains(itemId), $"recipe '{r["id"]}' uses unresolvable ingredient '{itemId}'");
                    Assert.True(amount > 0, $"recipe '{r["id"]}' ingredient '{itemId}' non-positive amount");
                }
            }
        }

        [Fact]
        public void Crafting_GreenhouseOutputsNotPricedBelowInputValue()
        {
            // Plan §46 arbitrage guard: output trade value should cover its
            // ingredient cost in a barter economy (crafting labor is the margin).
            var catalog = LoadGlobalCatalog();
            foreach (var r in LoadEntries("recipes.json").Where(r => Str(r, "id").StartsWith("craft_greenhouse_")))
            {
                float input = 0f;
                foreach (var ing in ((System.Text.Json.JsonElement)r["ingredients"]).EnumerateArray())
                {
                    var def = catalog.Get(ing.GetProperty("itemId").GetString());
                    Assert.NotNull(def);
                    input += def.tradeValue * ing.GetProperty("amount").GetInt32();
                }
                var output = catalog.Get(((System.Text.Json.JsonElement)r["resultItemId"]).GetString());
                Assert.True(output.tradeValue >= input * 0.75f,
                    $"recipe '{r["id"]}': output trade {output.tradeValue} undercuts ingredient cost {input}");
            }
        }

        // ── Scavenging bindings (plan §39-41, §59-63) ───────────────────

        [Fact]
        public void Scavenging_GreenhouseTableBindsThreePlan91Items()
        {
            var tables = LoadEntries("scavenging_tables.json");
            var gh = tables.First(t => Str(t, "id") == "table_loot_greenhouse");
            var entries = (System.Text.Json.JsonElement)gh["entries"];
            var bound = entries.EnumerateArray()
                .Select(e => e.GetProperty("item_id").GetString())
                .ToHashSet();
            Assert.Contains("item_greenhouse_glass_pane", bound);
            Assert.Contains("item_greenhouse_uv_sheeting", bound);
            Assert.Contains("item_greenhouse_drip_kit", bound);
        }

        [Fact]
        public void Scavenging_BoundItemIdsResolveInGlobalRegistry()
        {
            var catalog = LoadGlobalCatalog();
            var tables = LoadEntries("scavenging_tables.json");
            foreach (var t in tables)
            {
                foreach (var e in ((System.Text.Json.JsonElement)t["entries"]).EnumerateArray())
                {
                    var itemId = e.GetProperty("item_id").GetString();
                    if (string.IsNullOrEmpty(itemId)) continue; // map-fragment reward pattern
                    if (((string)itemId).StartsWith("item_greenhouse_"))
                        Assert.True(catalog.Contains(itemId), $"table '{t["id"]}' references unresolvable '{itemId}'");
                }
            }
        }

        [Fact]
        public void Scavenging_BoundEntriesUseSaneWeightsAndRarity()
        {
            var tables = LoadEntries("scavenging_tables.json");
            var gh = tables.First(t => Str(t, "id") == "table_loot_greenhouse");
            var bound = ((System.Text.Json.JsonElement)gh["entries"]).EnumerateArray()
                .Where(e => ((string)e.GetProperty("item_id").GetString()).StartsWith("item_greenhouse_"))
                .ToList();
            Assert.Equal(3, bound.Count);
            foreach (var e in bound)
            {
                Assert.True(e.GetProperty("weight").GetInt32() > 0);
                var rarity = e.GetProperty("rarity_tier").GetString();
                Assert.Contains(rarity, new[] { "common", "uncommon", "rare" });
                var drip = bound.First(x => x.GetProperty("item_id").GetString() == "item_greenhouse_drip_kit");
                Assert.Equal("rare", drip.GetProperty("rarity_tier").GetString());
            }
        }
    }
}
