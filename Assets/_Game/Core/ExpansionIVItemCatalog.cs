using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Inventory;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Expansion IV — New item definitions for the Logistics of Ruin.
    /// Factory methods that create ItemDefinition ScriptableObjects for
    /// items introduced in this expansion. Called by GameBootstrap during init.
    /// </summary>
    public static class ExpansionIVItemCatalog
    {
        // ── Item ids ──────────────────────────────────────────────────
        public const string Item_FatRendered = "fat_rendered";
        public const string Item_SledImprovised = "sled_improvised";
        public const string Item_TetherRope5m = "tether_rope_5m";
        public const string Item_CrematoriumAsh = "crematorium_ash";
        public const string Item_SignalSplicer = "signal_splicer";
        public const string Item_FlareWhite = "flare_white";
        public const string Item_GoldFilling = "gold_filling";
        public const string Item_FamilyPhotograph = "family_photograph";

        /// <summary>Create all Expansion IV item definitions.</summary>
        public static List<ItemDefinition> CreateAll()
        {
            return new List<ItemDefinition>
            {
                CreateFatRendered(),
                CreateSledImprovised(),
                CreateTetherRope5m(),
                CreateCrematoriumAsh(),
                CreateSignalSplicer(),
                CreateFlareWhite(),
                CreateGoldFilling(),
                CreateFamilyPhotograph()
            };
        }

        public static ItemDefinition CreateFatRendered()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_FatRendered;
            item.displayName = "Rendered Fat";
            item.description = "Boiled down from the dead. Makes excellent tallow candles " +
                "and waterproofing wax. The smell lingers on your hands for a week.";
            item.type = ItemType.Material;
            item.stackMax = 10;
            item.weight = 0.5f;
            item.tradeValue = 2f;
            return item;
        }

        public static ItemDefinition CreateSledImprovised()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_SledImprovised;
            item.displayName = "Plywood Sled";
            item.description = "Drags 60kg of loot through the ash. Leaves a trench. " +
                "The warlords will follow the trench.";
            item.type = ItemType.Tool;
            item.stackMax = 1;
            item.weight = 8f;
            item.tradeValue = 15f;
            return item;
        }

        public static ItemDefinition CreateTetherRope5m()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_TetherRope5m;
            item.displayName = "Heavy Tether (5m)";
            item.description = "Clips to two harnesses. If one falls through the ice " +
                "or ash-crust, the other gets pulled in unless they have the leverage to brace.";
            item.type = ItemType.Tool;
            item.stackMax = 2;
            item.weight = 1.2f;
            item.tradeValue = 8f;
            return item;
        }

        public static ItemDefinition CreateCrematoriumAsh()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_CrematoriumAsh;
            item.displayName = "Bone Ash Fertilizer";
            item.description = "High phosphorus. The tomatoes grow huge and red. " +
                "The the_chef refuses to cook them.";
            item.type = ItemType.Material;
            item.stackMax = 20;
            item.weight = 1f;
            item.tradeValue = 1.5f;
            return item;
        }

        public static ItemDefinition CreateSignalSplicer()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_SignalSplicer;
            item.displayName = "Signal Splicer";
            item.description = "Jury-rigged radio interceptor. Allows injection of false " +
                "audio into automated military loops. Draws massive power.";
            item.type = ItemType.Device;
            item.stackMax = 1;
            item.weight = 3.5f;
            item.tradeValue = 45f;
            return item;
        }

        public static ItemDefinition CreateFlareWhite()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_FlareWhite;
            item.displayName = "Phosphorus Flare (White)";
            item.description = "Blindingly bright. Burns at 2,000°C. Will permanently " +
                "blind the Lightless and trigger a panic swarm.";
            item.type = ItemType.Tool;
            item.stackMax = 5;
            item.weight = 0.3f;
            item.tradeValue = 12f;
            return item;
        }

        public static ItemDefinition CreateGoldFilling()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_GoldFilling;
            item.displayName = "Gold Filling";
            item.description = "Extracted from a corpse's teeth. Trade value: 8. " +
                "The dentist's hands did not shake.";
            item.type = ItemType.Trade;
            item.stackMax = 20;
            item.weight = 0.05f;
            item.tradeValue = 8f;
            return item;
        }

        public static ItemDefinition CreateFamilyPhotograph()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_FamilyPhotograph;
            item.displayName = "Family Photograph";
            item.description = "A pre-war photograph. Smiling faces. A park. " +
                "A dog. The kind of thing that makes you forget where you are. " +
                "Then you remember.";
            item.type = ItemType.Comfort;
            item.stackMax = 5;
            item.weight = 0.01f;
            item.tradeValue = 0f;
            item.moraleEffect = 3f;
            return item;
        }
    }

    // ── Expansion IV Recipe Definitions ────────────────────────────────

    /// <summary>
    /// New recipes for Expansion IV: craft_sled, render_fat, craft_tether, splice_signal.
    /// </summary>
    public static class ExpansionIVRecipeCatalog
    {
        public static List<RecipeEntry> CreateAll()
        {
            return new List<RecipeEntry>
            {
                new RecipeEntry
                {
                    Id = "craft_sled",
                    DisplayName = "Build Ash Sled",
                    StationId = "workbench",
                    TimeHours = 3f,
                    Ingredients = new List<RecipeIngredient>
                    {
                        new RecipeIngredient { ItemId = "plywood_sheet", Amount = 2 },
                        new RecipeIngredient { ItemId = "rope_2m_of_2m", Amount = 2 },
                        new RecipeIngredient { ItemId = "scrap_metal", Amount = 4 }
                    },
                    OutputItemId = ExpansionIVItemCatalog.Item_SledImprovised,
                    OutputAmount = 1
                },
                new RecipeEntry
                {
                    Id = "render_fat",
                    DisplayName = "Boil Down Biomass",
                    StationId = "stove",
                    TimeHours = 2f,
                    Ingredients = new List<RecipeIngredient>
                    {
                        new RecipeIngredient { ItemId = "raw_meat", Amount = 3 },
                        new RecipeIngredient { ItemId = "clean_water", Amount = 1 },
                        new RecipeIngredient { ItemId = "fuel_1l", Amount = 1 }
                    },
                    OutputItemId = ExpansionIVItemCatalog.Item_FatRendered,
                    OutputAmount = 2
                },
                new RecipeEntry
                {
                    Id = "craft_tether",
                    DisplayName = "Braid Heavy Tether",
                    StationId = "workbench",
                    TimeHours = 1f,
                    Ingredients = new List<RecipeIngredient>
                    {
                        new RecipeIngredient { ItemId = "rope_2m_of_2m", Amount = 3 },
                        new RecipeIngredient { ItemId = "scrap_metal", Amount = 2 }
                    },
                    OutputItemId = ExpansionIVItemCatalog.Item_TetherRope5m,
                    OutputAmount = 1
                },
                new RecipeEntry
                {
                    Id = "splice_signal",
                    DisplayName = "Splicer Maintenance",
                    StationId = "workbench",
                    TimeHours = 1.5f,
                    Ingredients = new List<RecipeIngredient>
                    {
                        new RecipeIngredient { ItemId = "vacuum_tube", Amount = 1 },
                        new RecipeIngredient { ItemId = "copper_wire_10m_of_10m", Amount = 2 },
                        new RecipeIngredient { ItemId = "soldering_iron", Amount = 1 }
                    },
                    OutputItemId = ExpansionIVItemCatalog.Item_SignalSplicer,
                    OutputAmount = 1
                }
            };
        }
    }

    [Serializable]
    public class RecipeEntry
    {
        public string Id;
        public string DisplayName;
        public string StationId;
        public float TimeHours;
        public List<RecipeIngredient> Ingredients;
        public string OutputItemId;
        public int OutputAmount;
    }

    [Serializable]
    public class RecipeIngredient
    {
        public string ItemId;
        public int Amount;
    }
}
