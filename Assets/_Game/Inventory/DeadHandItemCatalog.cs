using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Inventory;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Expansion III — The Dead Hand &amp; The Oxide Wastes item catalog.
    /// 10 new items for UXO navigation, automated threat countermeasures,
    /// and electromagnetic shielding.
    /// </summary>
    public static class DeadHandItemCatalog
    {
        public const string Item_MineProd = "item_mine_prod";
        public const string Item_FaradayMesh = "item_faraday_mesh";
        public const string Item_AcousticDecoy = "item_acoustic_decoy";
        public const string Item_LogicBoard = "item_logic_board";
        public const string Item_SoundBaffling = "item_sound_baffling";
        public const string Item_EMPGrenade = "item_emp_grenade";
        public const string Item_TungstenCore = "item_tungsten_core";
        public const string Item_PneumaticHose = "item_pneumatic_hose";
        public const string Item_MasterOverride = "item_master_override";
        public const string Item_HeadphonesMil = "item_headphones_mil";

        public static List<ItemDefinition> CreateAll()
        {
            return new List<ItemDefinition>
            {
                CreateMineProd(),
                CreateFaradayMesh(),
                CreateAcousticDecoy(),
                CreateLogicBoard(),
                CreateSoundBaffling(),
                CreateEMPGrenade(),
                CreateTungstenCore(),
                CreatePneumaticHose(),
                CreateMasterOverride(),
                CreateHeadphonesMil()
            };
        }

        public static ItemDefinition CreateMineProd()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_MineProd;
            item.displayName = "Mine Prodder";
            item.description = "A fiberglass rod with a steel tip. Essential for probing UXO fields without setting off pressure fuzes. One wrong angle and you lose a foot.";
            item.type = ItemType.Tool;
            item.stackMax = 1;
            item.weight = 1.5f;
            item.tradeValue = 15f;
            return item;
        }

        public static ItemDefinition CreateFaradayMesh()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_FaradayMesh;
            item.displayName = "Faraday Mesh";
            item.description = "Woven copper and silk. Shields electronics from EMP and magnetic anomalies. The only thing between your dosimeter and a lie.";
            item.type = ItemType.Material;
            item.stackMax = 5;
            item.weight = 3f;
            item.tradeValue = 45f;
            return item;
        }

        public static ItemDefinition CreateAcousticDecoy()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_AcousticDecoy;
            item.displayName = "Acoustic Decoy";
            item.description = "A wind-up metronome rigged to a speaker. Draws automated sentry fire until the barrel melts. The ticking is the loneliest sound in the wastes.";
            item.type = ItemType.Device;
            item.stackMax = 2;
            item.weight = 2f;
            item.tradeValue = 30f;
            return item;
        }

        public static ItemDefinition CreateLogicBoard()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_LogicBoard;
            item.displayName = "Logic Board";
            item.description = "Pre-war military processing board. Highly susceptible to EMP rot. The Custodian's brain, extracted from a dead sentry.";
            item.type = ItemType.Device;
            item.stackMax = 1;
            item.weight = 0.8f;
            item.tradeValue = 60f;
            item.empShielded = false;
            return item;
        }

        public static ItemDefinition CreateSoundBaffling()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_SoundBaffling;
            item.displayName = "Sound Baffling";
            item.description = "Acoustic foam panels. Lines the shelter walls to reduce AcousticSignature. Silence is survival when the machines are listening.";
            item.type = ItemType.Material;
            item.stackMax = 5;
            item.weight = 4f;
            item.tradeValue = 25f;
            return item;
        }

        public static ItemDefinition CreateEMPGrenade()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_EMPGrenade;
            item.displayName = "EMP Grenade";
            item.description = "A localised electromagnetic pulse. Fries sentries and drones, but ruins your own dosimeters. A necessary betrayal of your own instruments.";
            item.type = ItemType.Weapon;
            item.stackMax = 2;
            item.weight = 1f;
            item.tradeValue = 85f;
            return item;
        }

        public static ItemDefinition CreateTungstenCore()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_TungstenCore;
            item.displayName = "Tungsten Core";
            item.description = "AP round penetrator. Required to craft armour-piercing ammo to pierce Custodian chassis. Dense enough to stop a tank. Or a drone.";
            item.type = ItemType.Material;
            item.stackMax = 10;
            item.weight = 2.5f;
            item.tradeValue = 40f;
            return item;
        }

        public static ItemDefinition CreatePneumaticHose()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_PneumaticHose;
            item.displayName = "Pneumatic Hose";
            item.description = "High-pressure air line. Used to repair automated turrets or shelter airlocks. The hiss of compressed air is the sound of something still working.";
            item.type = ItemType.Material;
            item.stackMax = 3;
            item.weight = 1.2f;
            item.tradeValue = 18f;
            return item;
        }

        public static ItemDefinition CreateMasterOverride()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_MasterOverride;
            item.displayName = "Master Override Key";
            item.description = "A heavy, encrypted brass key. Disables the Dead Hand protocol in a local sector. The war ends here, one switch at a time.";
            item.type = ItemType.Quest;
            item.stackMax = 1;
            item.weight = 0.5f;
            item.tradeValue = 0f;
            return item;
        }

        public static ItemDefinition CreateHeadphonesMil()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_HeadphonesMil;
            item.displayName = "Military Headphones";
            item.description = "Active noise-cancelling. Protects against acoustic warfare and tinnitus. The world goes quiet when you put them on — and that's the point.";
            item.type = ItemType.Tool;
            item.stackMax = 1;
            item.weight = 0.4f;
            item.tradeValue = 22f;
            return item;
        }
    }
}
