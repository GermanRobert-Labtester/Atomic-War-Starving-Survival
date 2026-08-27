using System;
#pragma warning disable CS8618
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Crafting;
using Ashfall.Core.Inventory;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot-host session for CraftingSystem (ported from Unity's
    /// _Game/Crafting/CraftingSystem). Shares the migrated Inventory; seeds a
    /// small recipe catalog; persists active crafts via CraftingSaveStore.
    /// No gameplay rules — hosts only present.
    /// </summary>
    public sealed class CraftingHostSession
    : HostSessionBase{
        public CraftingSystem Engine { get; }
        public InventoryContainer Inventory { get; }
        public System.Collections.Generic.List<Recipe> Recipes { get; } =
            new System.Collections.Generic.List<Recipe>();

        public string LastEvent { get; private set; } = string.Empty;
        public CraftingHostSession(InventoryContainer inventory = null!, System.Collections.Generic.List<Recipe> recipes = null!)
        {
            Inventory = inventory ?? new InventoryContainer();
            Engine = new CraftingSystem(Inventory);
            Engine.OnCraftStarted += _ => RaiseStateChanged();
            Engine.OnCraftCompleted += _ => RaiseStateChanged();
            SeedStation();
            if (recipes != null && recipes.Count > 0)
            {
                Recipes.AddRange(recipes);
            }
            else
            {
                SeedRecipes();
            }
            Engine.SetRecipeLookup(id => FindRecipe(id));
        }

        public static CraftingHostSession Create(string dataDir, InventoryContainer inventory)
        {
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();
            var itemCatalog = ItemCatalogLoader.LoadCatalog(dataDir, fileIO, serializer);
            var recipes = RecipeCatalogLoader.Load(dataDir, fileIO, serializer, itemCatalog);

            return new CraftingHostSession(inventory, recipes);
        }

        private void SeedStation()
        {
            Engine.AddStation(new CraftingStation { id = "workbench", displayName = "Civilian Workbench" });
        }

        private void SeedRecipes()
        {
            Recipe Make(string id, string name, ItemDefinition result, int resultAmount,
                (string ingredientId, int amount)[] ingredients, float hours, string stationId = "workbench")
            {
                var r = new Recipe
                {
                    id = id,
                    recipeName = name,
                    result = result,
                    resultAmount = resultAmount,
                    craftingTimeHours = hours,
                    requiredStationId = stationId
                };
                foreach (var (ingId, amt) in ingredients)
                {
                    var def = Catalog.Get(ingId);
                    if (def != null) r.ingredients.Add(new Ingredient { item = def, amount = amt });
                }
                return r;
            }

            // Seed 5 canonical recipes. Ids follow the master snake_case list.
            Recipes.Add(Make("recipe_water_filter", "Water Filter (charcoal)",
                Catalog.Get("water_filter")!, 1,
                new[] { ("scrap_mechanical", 2), ("scrap_electronic", 1) }, 4f));
            Recipes.Add(Make("recipe_bandage", "Bandage (clean cloth)",
                Catalog.Get("bandage")!, 2,
                new[] { ("scrap_mechanical", 1) }, 1f));
            Recipes.Add(Make("recipe_iodine_kit", "Iodine Kit",
                Catalog.Get("iodine_pills")!, 1,
                new[] { ("scrap_chemical", 1), ("scrap_mechanical", 1) }, 2f));
            Recipes.Add(Make("recipe_rad_away", "Rad-Away (chelators)",
                Catalog.Get("rad_away")!, 1,
                new[] { ("scrap_chemical", 2), ("scrap_electronic", 1) }, 6f));
            Recipes.Add(Make("recipe_gas_mask_filter", "Filter Pack (gas mask)",
                Catalog.Get("filter_pack")!, 1,
                new[] { ("scrap_electronic", 2), ("scrap_mechanical", 1) }, 3f));
            Recipes.Add(Make("recipe_inhaler", "Improvised Inhaler",
                Catalog.Get("inhaler")!, 1,
                new[] { ("scrap_chemical", 2), ("scrap_mechanical", 1) }, 3f));
            Recipes.Add(Make("recipe_herbal_tea", "Herbal Tea (respiratory relief)",
                Catalog.Get("herbal_tea")!, 2,
                new[] { ("scrap_mechanical", 1) }, 0.5f, stationId: ""));
        }

        /// <summary>Catalog shared with InventoryHostSession seed (same ids).</summary>
        public static ItemCatalog Catalog { get; } = BuildSeedCatalog();

        private static ItemCatalog BuildSeedCatalog()
        {
            var c = new ItemCatalog();
            c.Register(new ItemDefinition { id = "canned_food", displayName = "Canned Food", type = ItemType.Food, stackMax = 6, weight = 0.5f, hungerRestore = 40f, tradeValue = 6f });
            c.Register(new ItemDefinition { id = "clean_water", displayName = "Clean Water", type = ItemType.Water, stackMax = 4, weight = 0.8f, thirstRestore = 50f, tradeValue = 8f });
            c.Register(new ItemDefinition { id = "scrap_mechanical", displayName = "Mechanical Parts", type = ItemType.Material, stackMax = 50, weight = 0.2f, tradeValue = 2f });
            c.Register(new ItemDefinition { id = "scrap_electronic", displayName = "Electronic Scrap", type = ItemType.Material, stackMax = 50, weight = 0.1f, tradeValue = 3f });
            c.Register(new ItemDefinition { id = "scrap_chemical", displayName = "Chemicals", type = ItemType.Material, stackMax = 50, weight = 0.3f, tradeValue = 4f });
            c.Register(new ItemDefinition { id = "bandage", displayName = "Bandage", type = ItemType.Medical, stackMax = 8, weight = 0.1f, healthEffect = 10f, tradeValue = 5f });
            c.Register(new ItemDefinition { id = "iodine_pills", displayName = "Iodine Pills", type = ItemType.Iodine, stackMax = 5, weight = 0.05f, tradeValue = 12f });
            c.Register(new ItemDefinition { id = "rad_away", displayName = "Rad-Away", type = ItemType.AntiRad, stackMax = 3, weight = 0.2f, radCleanse = 30f, tradeValue = 20f });
            c.Register(new ItemDefinition { id = "water_filter", displayName = "Water Filter", type = ItemType.Filter, stackMax = 4, weight = 0.5f, tradeValue = 25f });
            c.Register(new ItemDefinition { id = "filter_pack", displayName = "Filter Pack", type = ItemType.Filter, stackMax = 6, weight = 0.3f, tradeValue = 10f });
            c.Register(new ItemDefinition { id = "inhaler", displayName = "Improvised Inhaler", type = ItemType.Medical, stackMax = 4, weight = 0.15f, tradeValue = 15f });
            c.Register(new ItemDefinition { id = "herbal_tea", displayName = "Herbal Tea", type = ItemType.Medical, stackMax = 10, weight = 0.05f, tradeValue = 3f });
            return c;
        }

        public Recipe? FindRecipe(string id)
        {
            for (int i = 0; i < Recipes.Count; i++)
                if (Recipes[i] != null && Recipes[i].id == id) return Recipes[i];
            return null;
        }

        // ── Craft ops ──────────────────────────────────────────────────

        public string Start(string recipeId)
        {
            var recipe = FindRecipe(recipeId);
            if (recipe == null) return $"Unknown recipe: {recipeId}.";
            bool ok = Engine.StartCraft(recipe);
            LastEvent = ok
                ? $"Started {recipe.recipeName} ({recipe.craftingTimeHours:F0}h)."
                : $"Cannot start {recipe.recipeName}: missing ingredients, station, or room.";
            RaiseStateChanged();
            return LastEvent;
        }

        public string CompleteAll(float gameHours)
        {
            Engine.Tick(gameHours);
            LastEvent = $"Advanced crafting by {gameHours:F0}h. {Engine.ActiveCraftCount} craft(s) queued.";
            RaiseStateChanged();
            return LastEvent;
        }

        // ── Status ─────────────────────────────────────────────────────

        public string CraftingLine()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("CRAFTING — WORKBENCH\n");
            for (int i = 0; i < Recipes.Count; i++)
            {
                var r = Recipes[i];
                if (r == null) continue;
                sb.Append(r.id).Append(": ").Append(r.result?.displayName ?? "?")
                  .Append(" ×").Append(r.resultAmount)
                  .Append(" · ").Append(r.craftingTimeHours.ToString("F0")).Append("h · ");
                for (int j = 0; j < r.ingredients.Count; j++)
                {
                    var ing = r.ingredients[j];
                    if (ing?.item == null) continue;
                    sb.Append(ing.item.id).Append("×").Append(ing.amount);
                    if (j < r.ingredients.Count - 1) sb.Append(" + ");
                }
                sb.Append('\n');
            }
            if (Engine.ActiveCraftCount > 0)
            {
                sb.Append("IN PROGRESS:\n");
                for (int i = 0; i < Engine.ActiveCrafts.Count; i++)
                {
                    var c = Engine.ActiveCrafts[i];
                    if (c?.Recipe == null) continue;
                    sb.Append("  ").Append(c.Recipe.id).Append(" — ")
                      .Append(c.HoursRemaining.ToString("F1")).Append("h remaining\n");
                }
            }
            return sb.ToString().TrimEnd();
        }

        public string CheckRecipe(string recipeId)
        {
            var recipe = FindRecipe(recipeId);
            if (recipe == null) return $"Unknown recipe: {recipeId}.";
            var sb = new System.Text.StringBuilder();
            sb.Append("RECIPE CHECK: ").Append(recipe.recipeName).Append('\n');
            for (int i = 0; i < recipe.ingredients.Count; i++)
            {
                var ing = recipe.ingredients[i];
                if (ing?.item == null) continue;
                int held = Inventory.Count(ing.item);
                string status = held >= ing.amount ? "[OK]" : "[!!]";
                sb.Append("  ").Append(status).Append(' ')
                  .Append(ing.item.id).Append(" ×").Append(ing.amount)
                  .Append(" (held ").Append(held).Append(")\n");
            }
            return sb.ToString().TrimEnd();
        }

        // ── Save / Load ────────────────────────────────────────────────

        public CraftingSystemSave CaptureSave() => Engine.CaptureState();

        public void RestoreSave(CraftingSystemSave save)
        {
            Engine.SetRecipeLookup(id => FindRecipe(id));
            Engine.RestoreState(save);
            RaiseStateChanged();
        }
    }
}
