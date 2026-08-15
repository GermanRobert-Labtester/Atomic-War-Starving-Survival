using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Inventory;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Expansion II — The Black Aquifer &amp; Myco-Necrosis item catalog.
    /// 10 new items for subterranean exploration, hydrostatic management,
    /// and fungal countermeasures.
    /// </summary>
    public static class BlackAquiferItemCatalog
    {
        public const string Item_ShoringTimber = "item_shoring_timber";
        public const string Item_MyceliumBricks = "item_mycelium_bricks";
        public const string Item_RebreatherScrubber = "item_rebreather_scrubber";
        public const string Item_BlackWaterVial = "item_black_water_vial";
        public const string Item_GeigerTether = "item_geiger_tether";
        public const string Item_BioluminescentMoss = "item_bioluminescent_moss";
        public const string Item_PneumaticJack = "item_pneumatic_jack";
        public const string Item_ROMembrane = "item_ro_membrane";
        public const string Item_FungicideFogger = "item_fungicide_fogger";
        public const string Item_SubmergedServer = "item_submerged_server";

        public static List<ItemDefinition> CreateAll()
        {
            return new List<ItemDefinition>
            {
                CreateShoringTimber(),
                CreateMyceliumBricks(),
                CreateRebreatherScrubber(),
                CreateBlackWaterVial(),
                CreateGeigerTether(),
                CreateBioluminescentMoss(),
                CreatePneumaticJack(),
                CreateROMembrane(),
                CreateFungicideFogger(),
                CreateSubmergedServer()
            };
        }

        public static ItemDefinition CreateShoringTimber()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_ShoringTimber;
            item.displayName = "Shoring Timber";
            item.description = "Heavy, treated wood. Essential for preventing cave-ins during subterranean expansion. Smells of creosote and desperation.";
            item.type = ItemType.Material;
            item.stackMax = 5;
            item.weight = 8f;
            item.tradeValue = 15f;
            return item;
        }

        public static ItemDefinition CreateMyceliumBricks()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_MyceliumBricks;
            item.displayName = "Mycelium Bricks";
            item.description = "Compressed ash and fungal binders. Smells awful, but provides excellent radiation shielding. The Rot Farmers' signature product.";
            item.type = ItemType.Material;
            item.stackMax = 10;
            item.weight = 4f;
            item.tradeValue = 8f;
            return item;
        }

        public static ItemDefinition CreateRebreatherScrubber()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_RebreatherScrubber;
            item.displayName = "Rebreather Scrubber";
            item.description = "Soda-lime cartridge. Filters out CO2 and hydrogen sulfide in deep tunnels. Breathe easy — for forty minutes.";
            item.type = ItemType.Filter;
            item.stackMax = 3;
            item.weight = 1f;
            item.tradeValue = 35f;
            return item;
        }

        public static ItemDefinition CreateBlackWaterVial()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_BlackWaterVial;
            item.displayName = "Black Water Vial";
            item.description = "A sample of the toxic aquifer. Proof of the deep-earth fracturing. The liquid is thick, iridescent, and smells of almonds and sulfur.";
            item.type = ItemType.Quest;
            item.stackMax = 5;
            item.weight = 0.5f;
            item.tradeValue = 0f;
            // Toxic aquifer sample: high contamination — ingesting would be lethal.
            item.contamination = 0.85f;
            return item;
        }

        public static ItemDefinition CreateGeigerTether()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_GeigerTether;
            item.displayName = "Geiger Tether";
            item.description = "A 50m spool of copper wire connected to a surface dosimeter. Allows mapping rad-pockets without entering them. The clicks tell you where death lives.";
            item.type = ItemType.Tool;
            item.stackMax = 1;
            item.weight = 1.5f;
            item.tradeValue = 20f;
            return item;
        }

        public static ItemDefinition CreateBioluminescentMoss()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_BioluminescentMoss;
            item.displayName = "Bioluminescent Moss";
            item.description = "Grows on the Sunken Grid. Provides dim, cold light without batteries. Calms those afraid of the dark. The Dredgers cultivate it like livestock.";
            item.type = ItemType.Comfort;
            item.stackMax = 5;
            item.weight = 0.2f;
            item.tradeValue = 12f;
            item.moraleEffect = 3f;
            return item;
        }

        public static ItemDefinition CreatePneumaticJack()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_PneumaticJack;
            item.displayName = "Pneumatic Jack";
            item.description = "Heavy mining tool. Required to safely excavate collapsed rubble without triggering secondary cave-ins. The compressed air hisses like something alive.";
            item.type = ItemType.Tool;
            item.stackMax = 1;
            item.weight = 12f;
            item.tradeValue = 85f;
            return item;
        }

        public static ItemDefinition CreateROMembrane()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_ROMembrane;
            item.displayName = "RO Membrane";
            item.description = "Reverse Osmosis filter. The only thing that can strip the Black Aquifer's chemical toxicity. Each use degrades it by 20%.";
            item.type = ItemType.Device;
            item.stackMax = 1;
            item.weight = 3f;
            item.tradeValue = 60f;
            item.durability = 100f;
            return item;
        }

        public static ItemDefinition CreateFungicideFogger()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_FungicideFogger;
            item.displayName = "Fungicide Fogger";
            item.description = "Pressurised canister of copper sulfate. Clears a room of Ash-Blight spores instantly. The mist is acrid and stains everything blue-green.";
            item.type = ItemType.Tool;
            item.stackMax = 2;
            item.weight = 2f;
            item.tradeValue = 45f;
            return item;
        }

        public static ItemDefinition CreateSubmergedServer()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_SubmergedServer;
            item.displayName = "Submerged Server Blade";
            item.description = "A watertight blade from the Sunken Grid. Contains pre-war tectonic survey data. The Dredgers trade these for UV lamps to treat their rickets.";
            item.type = ItemType.Quest;
            item.stackMax = 1;
            item.weight = 5f;
            item.tradeValue = 90f;
            return item;
        }
    }
}
