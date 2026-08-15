using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Inventory;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Expansion VI — New items for the Comfort Economy. Items that serve no
    /// mechanical survival purpose, but are vital for Morale and Sanity.
    /// </summary>
    public static class ExpansionVIItemCatalog
    {
        public const string Item_AshCakeSweet = "ash_cake_sweet";
        public const string Item_CarvedWoodenAnimal = "carved_wooden_animal";
        public const string Item_ContrabandRadioParts = "contraband_radio_parts";
        public const string Item_TornBookPages = "torn_book_pages";
        public const string Item_HallucinogenicTea = "hallucinogenic_tea";
        public const string Item_PrussianBlue = "prussian_blue";

        public static List<ItemDefinition> CreateAll()
        {
            return new List<ItemDefinition>
            {
                CreateAshCakeSweet(),
                CreateCarvedWoodenAnimal(),
                CreateContrabandRadioParts(),
                CreateTornBookPages(),
                CreateHallucinogenicTea(),
                CreatePrussianBlue()
            };
        }

        public static ItemDefinition CreateAshCakeSweet()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_AshCakeSweet;
            item.displayName = "Sweet Ash Cake";
            item.description = "Flour, water, and a precious pinch of sugar. Tastes like dirt and memory. +15 Morale to the child who eats it.";
            item.type = ItemType.Comfort;
            item.stackMax = 1;
            item.weight = 0.2f;
            item.tradeValue = 0f;
            item.moraleEffect = 15f;
            return item;
        }

        public static ItemDefinition CreateCarvedWoodenAnimal()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_CarvedWoodenAnimal;
            item.displayName = "Carved Wooden Animal";
            item.description = "Whittled from scrap_wood by the Insomniac. It looks like a dog, or maybe a rat. It sits on the Naive Son's pillow.";
            item.type = ItemType.Comfort;
            item.stackMax = 3;
            item.weight = 0.1f;
            item.tradeValue = 2f;
            item.moraleEffect = 5f;
            return item;
        }

        public static ItemDefinition CreateContrabandRadioParts()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_ContrabandRadioParts;
            item.displayName = "Contraband Radio Parts";
            item.description = "The smashed remains of a handheld radio. The Tech Bro is trying to rebuild it in secret.";
            item.type = ItemType.Material;
            item.stackMax = 5;
            item.weight = 0.3f;
            item.tradeValue = 8f;
            return item;
        }

        public static ItemDefinition CreateTornBookPages()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_TornBookPages;
            item.displayName = "Torn Book Pages";
            item.description = "Half a story. The other half is ash in the heater. The Teacher reads the fragments to the children anyway.";
            item.type = ItemType.Comfort;
            item.stackMax = 20;
            item.weight = 0.02f;
            item.tradeValue = 0.5f;
            item.moraleEffect = 2f;
            return item;
        }

        public static ItemDefinition CreateHallucinogenicTea()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_HallucinogenicTea;
            item.displayName = "Hallucinogenic Tea";
            item.description = "Brewed from ash-flower roots. Stops Mental Breaks for 24 hours. The visions are not real. The calm is.";
            item.type = ItemType.Medical;
            item.stackMax = 10;
            item.weight = 0.2f;
            item.tradeValue = 12f;
            item.moraleEffect = 10f;
            return item;
        }

        public static ItemDefinition CreatePrussianBlue()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_PrussianBlue;
            item.displayName = "Prussian Blue";
            item.description = "Rad-purging compound. Synthesized from fertilizer and scrap_metal. Tastes like chalk. Works like prayer.";
            item.type = ItemType.AntiRad;
            item.stackMax = 10;
            item.weight = 0.3f;
            item.tradeValue = 15f;
            item.radCleanse = 20f;
            return item;
        }
    }

    /// <summary>
    /// Expansion VI — New recipes for the Comfort Economy.
    /// </summary>
    public static class ExpansionVIRecipeCatalog
    {
        public static List<RecipeEntry> CreateAll()
        {
            return new List<RecipeEntry>
            {
                new RecipeEntry
                {
                    Id = "craft_ash_cake",
                    DisplayName = "Bake Sweet Ash Cake",
                    StationId = "stove",
                    TimeHours = 0.5f,
                    Ingredients = new List<RecipeIngredient>
                    {
                        new RecipeIngredient { ItemId = "wheat_flour", Amount = 1 },
                        new RecipeIngredient { ItemId = "clean_water", Amount = 1 },
                        new RecipeIngredient { ItemId = "sugar", Amount = 1 },
                        new RecipeIngredient { ItemId = "fuel_1l", Amount = 1 }
                    },
                    OutputItemId = ExpansionVIItemCatalog.Item_AshCakeSweet,
                    OutputAmount = 1
                },
                new RecipeEntry
                {
                    Id = "whittle_toy",
                    DisplayName = "Whittle Wooden Toy",
                    StationId = "workbench",
                    TimeHours = 2f,
                    Ingredients = new List<RecipeIngredient>
                    {
                        new RecipeIngredient { ItemId = "scrap_wood", Amount = 1 },
                        new RecipeIngredient { ItemId = "knife_improvised", Amount = 1 }
                    },
                    OutputItemId = ExpansionVIItemCatalog.Item_CarvedWoodenAnimal,
                    OutputAmount = 1
                },
                new RecipeEntry
                {
                    Id = "splice_contraband",
                    DisplayName = "Splice Contraband Radio",
                    StationId = "workbench",
                    TimeHours = 3f,
                    Ingredients = new List<RecipeIngredient>
                    {
                        new RecipeIngredient { ItemId = "contraband_radio_parts", Amount = 3 },
                        new RecipeIngredient { ItemId = "copper_wire_10m_of_10m", Amount = 1 }
                    },
                    OutputItemId = "handheld_radio",
                    OutputAmount = 1
                }
            };
        }
    }
}
