using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Inventory;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Expansion IX/X — New items for the procedural scavenging system.
    /// Tools, containers, and materials from deep-lore locations.
    /// </summary>
    public static class ExpansionIXItemCatalog
    {
        public const string Item_PaperScrap = "paper_scrap";
        public const string Item_SuitcaseLocked = "item_suitcase_locked";
        public const string Item_IndustrialBleach = "industrial_bleach";
        public const string Item_AmmoniaTank = "ammonia_tank";
        public const string Item_AnchorNotes = "item_anchor_notes";
        public const string Item_HalonTank = "halon_tank";
        public const string Item_PipeWrench = "pipe_wrench";
        public const string Item_Crayon = "crayon";
        public const string Item_SawdustBlock = "sawdust_block";
        public const string Item_AshGhillie = "item_ash_ghillie";
        public const string Item_TeddyBear = "item_teddy_bear";
        public const string Item_CarKeys = "item_car_keys";
        public const string Item_IcePick = "item_ice_pick";
        public const string Item_BrassFittings = "brass_fittings";

        public static List<ItemDefinition> CreateAll()
        {
            return new List<ItemDefinition>
            {
                CreateItem(Item_PaperScrap, "Paper Scrap", "Useless for reading. Used for BioLatrine hygiene or fire starter.",
                    ItemType.Material, 100, 0.01f, 0.1f),
                CreateItem(Item_SuitcaseLocked, "Locked Suitcase", "Leather and brass. Rattling inside. Requires lockpick. Might hold insulin, or just rocks and grief.",
                    ItemType.Tool, 1, 4f, 5f),
                CreateItem(Item_IndustrialBleach, "Industrial Bleach (5L)", "Sodium hypochlorite. Cleans bio-sludge off scavenged meat. Burns the lungs if spilled.",
                    ItemType.Material, 2, 5.5f, 18f),
                CreateItem(Item_AmmoniaTank, "Pressurized Ammonia Tank", "Heavy steel cylinder. Used for refrigeration or jury-rigging toxic gas traps.",
                    ItemType.Material, 1, 12f, 25f),
                CreateItem(Item_AnchorNotes, "The Anchor's Final Script", "Handwritten on teleprompter paper. The last words spoken to the city before the dark.",
                    ItemType.Quest, 1, 0.1f, 0f),
                CreateItem(Item_HalonTank, "Halon Fire Suppressant", "Saves the generator room from fire. Extremely heavy.",
                    ItemType.Material, 1, 15f, 40f),
                CreateItem(Item_PipeWrench, "Heavy Pipe Wrench", "Cast iron. Turns rusted municipal valves. Also breaks skulls with terrifying efficiency.",
                    ItemType.Tool, 1, 3.5f, 14f),
                CreateItem(Item_Crayon, "Crayon", "Used by children to draw on bunker walls. Maps the ash. Predicts weather.",
                    ItemType.Comfort, 30, 0.02f, 0.5f),
                CreateItem(Item_SawdustBlock, "Compressed Sawdust Block", "Excellent, long-burning heater fuel. Lights easily, burns slow.",
                    ItemType.Fuel, 50, 1f, 2f),
                CreateItem(Item_AshGhillie, "Ash Ghillie Suit", "Burlap and pine needles. Blends into the ash. Cult zealots ignore you.",
                    ItemType.Protective, 1, 3f, 20f),
                CreateItem(Item_TeddyBear, "Teddy Bear", "Stuffed animal. Stops NightTerrorSystem events for children.",
                    ItemType.Comfort, 4, 0.3f, 2f),
                CreateItem(Item_CarKeys, "Car Keys", "Useless for starting cars. The Forger can melt them for lead/zinc.",
                    ItemType.Material, 5, 0.05f, 1f),
                CreateItem(Item_IcePick, "Ice Pick", "Essential for chipping clean water from frozen reservoirs. Without it, yield drops 80%.",
                    ItemType.Tool, 2, 0.8f, 10f),
                CreateItem(Item_BrassFittings, "Brass Fittings", "Door handles, nameplates, lamp bases. High trade value with Rebuilders.",
                    ItemType.Material, 30, 0.3f, 8f)
            };
        }

        private static ItemDefinition CreateItem(string id, string name, string desc,
            ItemType type, int stackMax, float weight, float tradeValue)
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = id;
            item.displayName = name;
            item.description = desc;
            item.type = type;
            item.stackMax = stackMax;
            item.weight = weight;
            item.tradeValue = tradeValue;
            return item;
        }
    }
}
