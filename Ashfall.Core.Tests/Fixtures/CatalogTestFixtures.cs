using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Crafting;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Tests.Fixtures
{
    /// <summary>
    /// Reusable sample/test definitions for unit tests that run in isolated,
    /// in-memory contexts without requiring disk access.
    /// </summary>
    public static class CatalogTestFixtures
    {
        public static ItemCatalog CreateSampleItemCatalog()
        {
            var catalog = new ItemCatalog();
            catalog.Register(new ItemDefinition { id = "canned_food", displayName = "Canned Food", type = ItemType.Food, stackMax = 6, weight = 0.5f, hungerRestore = 40f, tradeValue = 6f });
            catalog.Register(new ItemDefinition { id = "clean_water", displayName = "Clean Water", type = ItemType.Water, stackMax = 4, weight = 0.8f, thirstRestore = 50f, tradeValue = 8f });
            catalog.Register(new ItemDefinition { id = "irradiated_water", displayName = "Irradiated Water", type = ItemType.IrradiatedWater, stackMax = 4, weight = 0.8f, thirstRestore = 40f, contamination = 0.6f, tradeValue = 1f });
            catalog.Register(new ItemDefinition { id = "bandage", displayName = "Bandage", type = ItemType.Medical, stackMax = 8, weight = 0.1f, healthEffect = 10f, tradeValue = 5f });
            catalog.Register(new ItemDefinition { id = "iodine_pills", displayName = "Iodine Pills", type = ItemType.Iodine, stackMax = 5, weight = 0.05f, tradeValue = 12f });
            catalog.Register(new ItemDefinition { id = "rad_away", displayName = "Rad-Away", type = ItemType.AntiRad, stackMax = 3, weight = 0.2f, radCleanse = 30f, tradeValue = 20f });
            catalog.Register(new ItemDefinition { id = "gas_mask", displayName = "Gas Mask", type = ItemType.Protective, stackMax = 1, weight = 1.5f, isEquipable = true, equipSlot = EquipSlot.Face, radProtection = 30f, durability = 100f, tradeValue = 40f });
            catalog.Register(new ItemDefinition { id = "hazmat_suit", displayName = "Hazmat Suit", type = ItemType.Protective, stackMax = 1, weight = 5.0f, isEquipable = true, equipSlot = EquipSlot.Body, radProtection = 80f, durability = 100f, tradeValue = 40f });
            catalog.Register(new ItemDefinition { id = "battery", displayName = "Battery", type = ItemType.Tool, stackMax = 10, weight = 0.1f, tradeValue = 4f });
            catalog.Register(new ItemDefinition { id = "scrap_mechanical", displayName = "Mechanical Parts", type = ItemType.Material, stackMax = 50, weight = 0.2f, tradeValue = 2f });
            catalog.Register(new ItemDefinition { id = "scrap_electronic", displayName = "Electronic Scrap", type = ItemType.Material, stackMax = 50, weight = 0.1f, tradeValue = 3f });
            catalog.Register(new ItemDefinition { id = "scrap_chemical", displayName = "Chemicals", type = ItemType.Material, stackMax = 50, weight = 0.3f, tradeValue = 4f });
            catalog.Register(new ItemDefinition { id = "water_filter", displayName = "Water Filter", type = ItemType.Filter, stackMax = 4, weight = 0.5f, tradeValue = 25f });
            catalog.Register(new ItemDefinition { id = "filter_pack", displayName = "Filter Pack", type = ItemType.Filter, stackMax = 6, weight = 0.3f, tradeValue = 10f });
            catalog.Register(new ItemDefinition { id = "inhaler", displayName = "Improvised Inhaler", type = ItemType.Medical, stackMax = 4, weight = 0.15f, healthEffect = 15f, tradeValue = 15f });
            catalog.Register(new ItemDefinition { id = "herbal_tea", displayName = "Herbal Tea", type = ItemType.Medical, stackMax = 10, weight = 0.05f, healthEffect = 5f, tradeValue = 3f });
            return catalog;
        }

        public static List<Recipe> CreateSampleRecipes(ItemCatalog catalog)
        {
            var recipes = new List<Recipe>();
            var bandageDef = catalog.Get("bandage")!;
            var rBandage = new Recipe
            {
                id = "recipe_bandage",
                recipeName = "Bandage (clean cloth)",
                result = bandageDef,
                resultAmount = 2,
                craftingTimeHours = 1f,
                requiredStationId = "workbench"
            };
            rBandage.ingredients.Add(new Ingredient { item = catalog.Get("scrap_mechanical")!, amount = 1 });
            recipes.Add(rBandage);

            var filterDef = catalog.Get("water_filter")!;
            var rFilter = new Recipe
            {
                id = "recipe_water_filter",
                recipeName = "Water Filter (charcoal)",
                result = filterDef,
                resultAmount = 1,
                craftingTimeHours = 4f,
                requiredStationId = "workbench"
            };
            rFilter.ingredients.Add(new Ingredient { item = catalog.Get("scrap_mechanical")!, amount = 2 });
            rFilter.ingredients.Add(new Ingredient { item = catalog.Get("scrap_electronic")!, amount = 1 });
            recipes.Add(rFilter);

            return recipes;
        }

        public static List<StartingSurvivorDefinition> CreateSampleStartingSurvivors()
        {
            return new List<StartingSurvivorDefinition>
            {
                new StartingSurvivorDefinition { id = "survivor_dr_sarah_chen", displayName = "Dr. Sarah Chen", health = 90f, hunger = 20f, thirst = 25f, warmth = 85f, morale = 70f, lifetimeDose = 14f, acuteRad = false },
                new StartingSurvivorDefinition { id = "survivor_gunner_mikhail", displayName = "Gunner Mikhail", health = 80f, hunger = 35f, thirst = 30f, warmth = 75f, morale = 55f, lifetimeDose = 38f, acuteRad = true },
                new StartingSurvivorDefinition { id = "elena_vasquez", displayName = "Elena Vasquez", health = 95f, hunger = 15f, thirst = 20f, warmth = 90f, morale = 65f, lifetimeDose = 8f, acuteRad = false }
            };
        }

        public static List<ExpeditionDefinition> CreateSampleExpeditions()
        {
            return new List<ExpeditionDefinition>
            {
                new ExpeditionDefinition
                {
                    id = "loc_the_allotments",
                    displayName = "The Works Allotment Commune",
                    distanceTicks = 5,
                    dangerLevel = 2,
                    encounterChancePerTick = 0.12f,
                    baseStaminaDrainPerHour = 2.0f,
                    lootCategories = new List<string> { "scrap_metal", "clean_water", "bandages", "food_rations" }
                },
                new ExpeditionDefinition
                {
                    id = "loc_denial_cut_substation",
                    displayName = "The Denial Cut Substation",
                    distanceTicks = 8,
                    dangerLevel = 4,
                    encounterChancePerTick = 0.18f,
                    baseStaminaDrainPerHour = 3.0f,
                    lootCategories = new List<string> { "dosimeter", "copper_wire", "fuel", "item_hydro_baron_queue_chit" }
                }
            };
        }
    }
}
