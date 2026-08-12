using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Inventory;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Expansion VIII — New items for the bureaucracy of the apocalypse.
    /// Stamps, ink, forged documents, acoustic foam, and the tools of administration.
    /// </summary>
    public static class ExpansionVIIIItemCatalog
    {
        public const string Item_StampMinistry = "stamp_ministry_official";
        public const string Item_InkIndelible = "ink_indelible";
        public const string Item_TransitPassForged = "transit_pass_forged";
        public const string Item_LedgerDistrict9 = "ledger_district9";
        public const string Item_AcousticFoam = "acoustic_foam_panel";
        public const string Item_RationCardBlank = "ration_card_blank";
        public const string Item_MetronomeWindup = "metronome_windup";
        public const string Item_AcousticDecoy = "acoustic_decoy";
        public const string Item_OxygenTank = "oxygen_tank";

        public static List<ItemDefinition> CreateAll()
        {
            return new List<ItemDefinition>
            {
                CreateStampMinistry(),
                CreateInkIndelible(),
                CreateTransitPassForged(),
                CreateLedgerDistrict9(),
                CreateAcousticFoam(),
                CreateRationCardBlank(),
                CreateMetronomeWindup(),
                CreateAcousticDecoy(),
                CreateOxygenTank()
            };
        }

        public static ItemDefinition CreateStampMinistry()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_StampMinistry;
            item.displayName = "Ministry Stamp";
            item.description = "Heavy brass. The ink pad is dry, but the authority remains. Used to forge transit passes and ration cards.";
            item.type = ItemType.Tool;
            item.stackMax = 1;
            item.weight = 0.2f;
            item.tradeValue = 40f;
            return item;
        }

        public static ItemDefinition CreateInkIndelible()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_InkIndelible;
            item.displayName = "Indelible Ink (Blood/Base)";
            item.description = "Smells like copper and alcohol. Does not wash off. Does not fade. The signature of the apocalypse.";
            item.type = ItemType.Material;
            item.stackMax = 5;
            item.weight = 0.1f;
            item.tradeValue = 15f;
            return item;
        }

        public static ItemDefinition CreateTransitPassForged()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_TransitPassForged;
            item.displayName = "Forged Transit Pass";
            item.description = "Looks real to a tired guard at a dark checkpoint. Will not survive a close inspection under a flashlight.";
            item.type = ItemType.Quest;
            item.stackMax = 1;
            item.weight = 0.05f;
            item.tradeValue = 0f;
            return item;
        }

        public static ItemDefinition CreateLedgerDistrict9()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_LedgerDistrict9;
            item.displayName = "District 9 Caloric Ledger";
            item.description = "The math of starvation. Proves the central government intentionally starved the uplands. Highly explosive politically.";
            item.type = ItemType.Quest;
            item.stackMax = 1;
            item.weight = 1.5f;
            item.tradeValue = 60f;
            return item;
        }

        public static ItemDefinition CreateAcousticFoam()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_AcousticFoam;
            item.displayName = "Acoustic Foam Panel";
            item.description = "Wedge-shaped polyurethane. Absorbs sound. Makes a room feel like the inside of a coffin, but keeps the secrets in.";
            item.type = ItemType.Material;
            item.stackMax = 10;
            item.weight = 0.8f;
            item.tradeValue = 12f;
            return item;
        }

        public static ItemDefinition CreateRationCardBlank()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_RationCardBlank;
            item.displayName = "Blank Ration Card";
            item.description = "Pre-war cardstock. Waiting for a stamp that will never come.";
            item.type = ItemType.Material;
            item.stackMax = 20;
            item.weight = 0.02f;
            item.tradeValue = 2f;
            return item;
        }

        public static ItemDefinition CreateMetronomeWindup()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_MetronomeWindup;
            item.displayName = "Wind-up Metronome";
            item.description = "Ticks at 60 BPM. Used by the Foley Artist to calibrate audio traps, or by the Teacher to keep time in a world without clocks.";
            item.type = ItemType.Tool;
            item.stackMax = 2;
            item.weight = 0.4f;
            item.tradeValue = 8f;
            return item;
        }

        public static ItemDefinition CreateAcousticDecoy()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_AcousticDecoy;
            item.displayName = "Acoustic Decoy";
            item.description = "A wind-up metronome placed in a tin can. Draws raiders to a specific location during a siege.";
            item.type = ItemType.Tool;
            item.stackMax = 3;
            item.weight = 0.6f;
            item.tradeValue = 10f;
            return item;
        }

        public static ItemDefinition CreateOxygenTank()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = Item_OxygenTank;
            item.displayName = "Oxygen Tank";
            item.description = "Compressed oxygen. Keeps the living alive when the scrubbers die. Finite. Every breath is a withdrawal.";
            item.type = ItemType.Material;
            item.stackMax = 5;
            item.weight = 3f;
            item.tradeValue = 25f;
            return item;
        }
    }
}
