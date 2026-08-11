using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Inventory;

namespace AtomicWar._Game.Crafting
{
    /// <summary>
    /// New Recipes (Section XI) — 10 additions authored as a single
    /// catalog builder so the host can materialise them via
    /// <c>RecipeCatalogSO</c> without requiring per-recipe .asset
    /// files. Each recipe is a plain C# Recipe instance initialised
    /// with id, name, ingredients (itemId + count), station id, time,
    /// and output. The host wires this into
    /// <c>RecipeCatalogSO.recipes</c> at boot.
    ///
    /// Where the existing <c>Recipe</c> uses <c>ItemDefinition</c>
    /// objects, this builder stores the ingredient/output ids as
    /// strings and resolves them through an injected lookup at
    /// materialisation time. This keeps the catalog free of asset
    /// references and lets the data importer feed it from JSON.
    /// </summary>
    public static class NewRecipesCatalog
    {
        public const string StationWorkbench = "workbench";
        public const string StationStove = "stove";
        public const string StationDistiller = "distiller";

        // ── Canonical recipe ids (Section XI) ────────────────────────────
        public static class Ids
        {
            public const string Tourniquet      = "craft_tourniquet";
            public const string SalineDrip      = "craft_saline_drip";
            public const string CookRatMeat     = "cook_rat_meat";
            public const string PressInsectBrick= "press_insect_brick";
            public const string AshBread        = "craft_ash_bread";
            public const string RepairGasket    = "repair_gasket";
            public const string ImprovisedMolotov= "craft_improvised_molotov";
            public const string DistillWater    = "distill_clean_water";
            public const string LeadVest        = "craft_lead_vest";
            public const string TallowCandle    = "render_tallow_candle";
        }

        /// <summary>
        /// One row in the new catalog. The host resolves <see cref="ItemId"/>
        /// into an <see cref="ItemDefinition"/> via the lookup at materialise time.
        /// </summary>
        public class Spec
        {
            public string Id;
            public string DisplayName;
            public List<SpecIngredient> Ingredients = new List<SpecIngredient>();
            public string ResultItemId;
            public int ResultAmount = 1;
            public string StationId;
            public float CraftingTimeHours;
            /// <summary>Special result: when set, this recipe does not produce an item but
            /// applies a numeric effect (e.g. <c>repair_gasket</c> adds 15 % hatch seal).</summary>
            public string EffectKey;
            public float EffectAmount;
        }

        public class SpecIngredient
        {
            public string ItemId;
            public int Count;
            public SpecIngredient() {}
            public SpecIngredient(string id, int count) { ItemId = id; Count = count; }
        }

        public static List<Spec> BuildAll()
        {
            var list = new List<Spec>();
            list.Add(new Spec
            {
                Id = Ids.Tourniquet, DisplayName = "Craft Tourniquet",
                Ingredients = { new SpecIngredient("cloth", 2), new SpecIngredient("rope_2m", 1) },
                ResultItemId = "tourniquet", ResultAmount = 1,
                StationId = StationWorkbench, CraftingTimeHours = 0.5f
            });
            list.Add(new Spec
            {
                Id = Ids.SalineDrip, DisplayName = "Prepare Saline Drip",
                Ingredients = { new SpecIngredient("salt", 1), new SpecIngredient("clean_water", 2), new SpecIngredient("plastic_material", 1) },
                ResultItemId = "saline_drip_bag", ResultAmount = 1,
                StationId = StationWorkbench, CraftingTimeHours = 1.0f
            });
            list.Add(new Spec
            {
                Id = Ids.CookRatMeat, DisplayName = "Cook Rat Meat",
                Ingredients = { new SpecIngredient("raw_rat_meat", 1), new SpecIngredient("fuel", 1) },
                ResultItemId = "rat_meat_skewer", ResultAmount = 1,
                StationId = StationStove, CraftingTimeHours = 0.3f
            });
            list.Add(new Spec
            {
                Id = Ids.PressInsectBrick, DisplayName = "Press Insect Brick",
                Ingredients = { new SpecIngredient("raw_insects", 3), new SpecIngredient("salt", 1) },
                ResultItemId = "insect_paste_brick", ResultAmount = 2,
                StationId = StationWorkbench, CraftingTimeHours = 1.5f
            });
            list.Add(new Spec
            {
                Id = Ids.AshBread, DisplayName = "Bake Ash Bread",
                Ingredients = { new SpecIngredient("wheat_flour", 1), new SpecIngredient("clean_water", 1) },
                ResultItemId = "ash_bread_flat", ResultAmount = 2,
                StationId = StationStove, CraftingTimeHours = 0.5f
            });
            list.Add(new Spec
            {
                Id = Ids.RepairGasket, DisplayName = "Patch Hatch Gasket",
                Ingredients = { new SpecIngredient("rubber_gasket_set", 2), new SpecIngredient("duct_tape", 1) },
                ResultItemId = null, ResultAmount = 0,
                StationId = StationWorkbench, CraftingTimeHours = 2.0f,
                EffectKey = "hatch_seal_integrity", EffectAmount = 0.15f
            });
            list.Add(new Spec
            {
                Id = Ids.ImprovisedMolotov, DisplayName = "Assemble Incendiary",
                Ingredients = { new SpecIngredient("water_bottle_empty", 1), new SpecIngredient("fuel", 1), new SpecIngredient("cloth", 1) },
                ResultItemId = "improvised_molotov", ResultAmount = 1,
                StationId = StationWorkbench, CraftingTimeHours = 0.3f
            });
            list.Add(new Spec
            {
                Id = Ids.DistillWater, DisplayName = "Distill Water",
                Ingredients = { new SpecIngredient("dirty_water", 3), new SpecIngredient("fuel", 2) },
                ResultItemId = "clean_water", ResultAmount = 2,
                StationId = StationDistiller, CraftingTimeHours = 1.0f
            });
            list.Add(new Spec
            {
                Id = Ids.LeadVest, DisplayName = "Sew Lead Vest",
                Ingredients = { new SpecIngredient("lead_sheet", 2), new SpecIngredient("cloth", 3), new SpecIngredient("duct_tape", 1) },
                ResultItemId = "improvised_lead_vest", ResultAmount = 1,
                StationId = StationWorkbench, CraftingTimeHours = 4.0f
            });
            list.Add(new Spec
            {
                Id = Ids.TallowCandle, DisplayName = "Render Tallow Candle",
                Ingredients = { new SpecIngredient("raw_meat", 2), new SpecIngredient("cloth", 1) },
                ResultItemId = "candle_tallow", ResultAmount = 3,
                StationId = StationStove, CraftingTimeHours = 1.0f
            });
            return list;
        }

        /// <summary>
        /// Materialise a single Spec into a real <see cref="Recipe"/> ScriptableObject,
        /// resolving item ids to <see cref="ItemDefinition"/> via <paramref name="lookup"/>.
        /// </summary>
        public static Recipe Materialise(Spec spec, System.Func<string, ItemDefinition> lookup)
        {
            if (spec == null) return null;
            var r = ScriptableObject.CreateInstance<Recipe>();
            r.id = spec.Id;
            r.recipeName = spec.DisplayName;
            r.craftingTimeHours = spec.CraftingTimeHours;
            r.requiredStationId = spec.StationId;
            r.resultAmount = spec.ResultAmount;
            if (!string.IsNullOrEmpty(spec.ResultItemId) && lookup != null)
            {
                r.result = lookup(spec.ResultItemId);
            }
            for (int i = 0; i < spec.Ingredients.Count; i++)
            {
                var ing = spec.Ingredients[i];
                r.ingredients.Add(new Ingredient
                {
                    item = lookup != null ? lookup(ing.ItemId) : null,
                    amount = ing.Count
                });
            }
            return r;
        }

        /// <summary>Materialise all specs and return the new Recipe list.</summary>
        public static List<Recipe> MaterialiseAll(System.Func<string, ItemDefinition> lookup)
        {
            var specs = BuildAll();
            var outList = new List<Recipe>(specs.Count);
            for (int i = 0; i < specs.Count; i++) outList.Add(Materialise(specs[i], lookup));
            return outList;
        }
    }
}
