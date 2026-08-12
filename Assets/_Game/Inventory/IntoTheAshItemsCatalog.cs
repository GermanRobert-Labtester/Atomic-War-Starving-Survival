using System.Collections.Generic;

namespace AtomicWar._Game.Inventory
{
    /// <summary>
    /// "Into the Ash" expansion items catalog (Part II Locations + Part III Questlines).
    /// Canonical snake_case ids for all new quest items, lore items, comfort items,
    /// weapons, and materials introduced across the 8 locations and 6 quest chains.
    ///
    /// This catalog is a pure C# registry of spec rows. The host reads
    /// <see cref="SpecRows"/> directly at bootstrap to build item definitions.
    /// Items that already exist in the base catalog or AshGetsDeeper are
    /// referenced but not duplicated; only genuinely new ids are created here.
    /// </summary>
    public static class IntoTheAshItemsCatalog
    {
        public const string IdPrefix = "ashdeep_";

        // ── Canonical id registry ─────────────────────────────────────────
        public static class Ids
        {
            // ── Quest Items ───────────────────────────────────────────
            // district_coordination_office
            public const string CivilianRecordFiles    = "civilian_record_files";
            public const string ComplianceLedger       = "compliance_ledger";
            public const string DcoKeyRing             = "dco_key_ring";

            // checkpoint_kilo_memorial
            public const string PrivateMarenJournal     = "private_maren_journal";
            public const string ProtocolNineDocument    = "protocol_nine_document";
            public const string GarrisonRequisitionForms = "garrison_requisition_forms";

            // militia_grain_exchange
            public const string RequisitionLedger       = "requisition_ledger";
            public const string MartaReceipt            = "marta_receipt";

            // the_glow_chapel
            public const string GlowCup                 = "glow_cup";
            public const string BrotherOrinJournal      = "brother_orin_journal";

            // kael_tribute_ledger_site
            public const string TributeLedger           = "tribute_ledger";
            public const string KaelCodeDocument        = "kael_code_document";
            public const string SableJournal            = "sable_journal";

            // st_maren_hospital_annex
            public const string PatientRecordsAnnex     = "patient_records_annex";
            public const string AgnesLetter             = "agnes_letter";
            public const string HeadNurseCombinationCard = "head_nurse_combination_card";

            // radio_tower_seven_bunker
            public const string FrequencyLogSealed      = "frequency_log_sealed";
            public const string CaptainVennKeycard      = "captain_venn_keycard";
            public const string OperatorFinalLog        = "operator_final_log";
            public const string BroadcastFrequencyManual = "broadcast_frequency_manual";

            // marta_farmhouse
            public const string TomasSchoolbook         = "tomas_schoolbook";
            public const string MilitiaYouthProgramFlyer = "militia_youth_program_flyer";

            // ── Comfort Items ─────────────────────────────────────────
            public const string StampDcoOfficial        = "stamp_dco_official";
            public const string ChildShoeSingle         = "child_shoe_single";
            public const string StuffedRabbitChild      = "stuffed_rabbit_child";
            public const string LenaStuffedRabbit       = "lena_stuffed_rabbit";

            // ── Lore Items ───────────────────────────────────────────
            public const string MilitiaCharterFeedSack  = "militia_charter_feed_sack";

            // ── Trade Goods ──────────────────────────────────────────
            public const string MilitiaUniformPatch     = "militia_uniform_patch";

            // ── Materials ────────────────────────────────────────────
            public const string ConvertHandprintPaint   = "convert_handprint_paint";
            public const string PhysiotherapyBands      = "physiotherapy_bands";

            // ── Weapons ──────────────────────────────────────────────
            public const string HuntingRifleBolt        = "hunting_rifle_bolt";
            public const string WeaponShotgunDouble     = "weapon_shotgun_double";

            // ── Tools / Devices ──────────────────────────────────────
            public const string WheelchairFolded        = "wheelchair_folded";
            public const string RadioBroadcastEquipment = "radio_broadcast_equipment";
            public const string BackupGeneratorParts    = "backup_generator_parts";
        }

        // ── Spec rows (materialised by host) ─────────────────────────────
        public static readonly List<ItemSpecRow> SpecRows = new List<ItemSpecRow>
        {
            // ── Quest Items ───────────────────────────────────────────────
            new ItemSpecRow
            {
                Id = Ids.CivilianRecordFiles,
                DisplayName = "Civilian Record Files",
                Description = "Manila folders containing the personal data of 4,200 Tessarat citizens. Births, deaths, rations, shelter assignments. A weapon of leverage.",
                Category = ItemCategory.QuestItem,
                Weight = 0.3f,
                StackSize = 4,
                TradeValue = 80f
            },
            new ItemSpecRow
            {
                Id = Ids.ComplianceLedger,
                DisplayName = "Master Compliance Ledger",
                Description = "The District Coordinator's master ledger. Determines who gets rations in the Garrison's system. The Garrison wants it. The Militia wants it destroyed.",
                Category = ItemCategory.QuestItem,
                Weight = 2.0f,
                StackSize = 1,
                TradeValue = 250f
            },
            new ItemSpecRow
            {
                Id = Ids.DcoKeyRing,
                DisplayName = "DCO Key Ring",
                Description = "A ring of keys on a hook behind reception. Opens the filing cabinets and the back office of the District Coordination Office.",
                Category = ItemCategory.QuestItem,
                Weight = 0.2f,
                StackSize = 1,
                TradeValue = 5f
            },
            new ItemSpecRow
            {
                Id = Ids.PrivateMarenJournal,
                DisplayName = "Private Maren's Journal",
                Description = "Forty handwritten pages. Maren's account of loading 89 bodies, the woman at the fence, the transfer request denied. The truth in a dead soldier's hand.",
                Category = ItemCategory.QuestItem,
                Weight = 0.5f,
                StackSize = 1,
                TradeValue = 40f
            },
            new ItemSpecRow
            {
                Id = Ids.ProtocolNineDocument,
                DisplayName = "Protocol Nine Document",
                Description = "The sealed order. 'This facility is at capacity. Proceed to your assigned district shelter.' The words that killed 89 people.",
                Category = ItemCategory.QuestItem,
                Weight = 0.1f,
                StackSize = 1,
                TradeValue = 100f
            },
            new ItemSpecRow
            {
                Id = Ids.GarrisonRequisitionForms,
                DisplayName = "Garrison Requisition Forms",
                Description = "Twelve signed receipts for civilian supplies requisitioned by the garrison. Every can, every litre, every bandage — accounted for. The garrison never missed a form.",
                Category = ItemCategory.QuestItem,
                Weight = 0.3f,
                StackSize = 12,
                TradeValue = 10f
            },
            new ItemSpecRow
            {
                Id = Ids.RequisitionLedger,
                DisplayName = "Requisition Ledger",
                Description = "340 family entries. Each entry is a caloric allocation. The allocations decrease by 5% each week. At the current rate, the villages will be below survival threshold in 40 days.",
                Category = ItemCategory.QuestItem,
                Weight = 1.5f,
                StackSize = 1,
                TradeValue = 200f
            },
            new ItemSpecRow
            {
                Id = Ids.MartaReceipt,
                DisplayName = "Marta's Receipt",
                Description = "A militia requisition receipt. 'RECEIVED: 40 kg potatoes, 12 eggs. FROM: Marta K., Eastern Lowlands. REASON: Outstanding contribution. SIGNED: Cmdr. Voss.'",
                Category = ItemCategory.QuestItem,
                Weight = 0.05f,
                StackSize = 1,
                TradeValue = 1f
            },
            new ItemSpecRow
            {
                Id = Ids.GlowCup,
                DisplayName = "The Glow Cup",
                Description = "A wooden bowl from the Garden of Light. The water inside reads 15 mSv/L. Not lethal. Not safe. Enough to make the dosimeter click. Enough to feel chosen.",
                Category = ItemCategory.QuestItem,
                Weight = 0.4f,
                StackSize = 1,
                TradeValue = 30f
            },
            new ItemSpecRow
            {
                Id = Ids.BrotherOrinJournal,
                DisplayName = "Brother Orin's Journal",
                Description = "Sixty pages. The theology of the Glow. Orin writes about Checkpoint Kilo, the militia's tax, the warlords' tribute. 'Every institution is a machine that grinds people into fuel. I offer a machine that grinds them into light.'",
                Category = ItemCategory.QuestItem,
                Weight = 0.5f,
                StackSize = 1,
                TradeValue = 40f
            },
            new ItemSpecRow
            {
                Id = Ids.TributeLedger,
                DisplayName = "Tribute Ledger",
                Description = "Forty-seven shelter entries. Each entry is a weekly payment calibrated to what the shelter can afford. The forty-eighth entry is yours.",
                Category = ItemCategory.QuestItem,
                Weight = 1.5f,
                StackSize = 1,
                TradeValue = 180f
            },
            new ItemSpecRow
            {
                Id = Ids.KaelCodeDocument,
                DisplayName = "Kael's Code",
                Description = "Five rules, handwritten. Rule 1: Always leave one thing. Rule 2: The math must balance. Rule 3: Never take from children. Rule 4: Thursday is Thursday. Rule 5: The code is the code.",
                Category = ItemCategory.QuestItem,
                Weight = 0.1f,
                StackSize = 1,
                TradeValue = 20f
            },
            new ItemSpecRow
            {
                Id = Ids.SableJournal,
                DisplayName = "Sable's Journal",
                Description = "Sable's private thoughts. 'Kael died because he left too much. He left them hope. Hope makes people stop paying. I leave the flashlight. One thing. Just one. Enough to remember we're not monsters.'",
                Category = ItemCategory.QuestItem,
                Weight = 0.4f,
                StackSize = 1,
                TradeValue = 30f
            },
            new ItemSpecRow
            {
                Id = Ids.PatientRecordsAnnex,
                DisplayName = "Patient Records — East Annex",
                Description = "Thirty-eight patient files. Admission dates, diagnoses, next-of-kin contacts. The next-of-kin are all dead or gone. The contacts are numbers that don't answer.",
                Category = ItemCategory.QuestItem,
                Weight = 1.0f,
                StackSize = 1,
                TradeValue = 5f
            },
            new ItemSpecRow
            {
                Id = Ids.AgnesLetter,
                DisplayName = "Agnes's Letter",
                Description = "A letter from a daughter, dated Day -2. 'Dear Mum, do you need anything from the market? I'm going Thursday. Love, Elena.' Agnes never answered. The market is a crater.",
                Category = ItemCategory.QuestItem,
                Weight = 0.05f,
                StackSize = 1,
                TradeValue = 1f
            },
            new ItemSpecRow
            {
                Id = Ids.HeadNurseCombinationCard,
                DisplayName = "Head Nurse's Combination Card",
                Description = "A card in the head nurse's drawer. 'IN CASE OF EMERGENCY: Pharmacy Combination 7-23-41.' The emergency was Day 0. The card was never used.",
                Category = ItemCategory.QuestItem,
                Weight = 0.02f,
                StackSize = 1,
                TradeValue = 10f
            },
            new ItemSpecRow
            {
                Id = Ids.FrequencyLogSealed,
                DisplayName = "Frequency Log — Tower 7",
                Description = "A sealed frequency log from the base station. Contains the bunker access code. A page is torn out — the torn entry reads: 'TRANSMIT ORDER 7741. CONFIRMED. GOD FORGIVE US.'",
                Category = ItemCategory.QuestItem,
                Weight = 0.3f,
                StackSize = 1,
                TradeValue = 50f
            },
            new ItemSpecRow
            {
                Id = Ids.CaptainVennKeycard,
                DisplayName = "Captain Venn's Keycard",
                Description = "A military-grade keycard. On Captain Venn's body, inside the sealed bunker. Required to open the airlock from either side.",
                Category = ItemCategory.QuestItem,
                Weight = 0.05f,
                StackSize = 1,
                TradeValue = 25f
            },
            new ItemSpecRow
            {
                Id = Ids.OperatorFinalLog,
                DisplayName = "Operator's Final Log",
                Description = "Captain Venn's last recording. 'Day 35. The ceasefire has not been confirmed. The bunker will remain sealed. The operators are alive. I am alive. The frequencies are quiet.' He signed off.",
                Category = ItemCategory.QuestItem,
                Weight = 0.1f,
                StackSize = 1,
                TradeValue = 30f
            },
            new ItemSpecRow
            {
                Id = Ids.BroadcastFrequencyManual,
                DisplayName = "Broadcast Frequency Manual",
                Description = "A military manual listing all broadcast frequencies in the Tessarat region. Military, civilian, emergency, survivor. Every channel. Every silence.",
                Category = ItemCategory.QuestItem,
                Weight = 0.4f,
                StackSize = 1,
                TradeValue = 25f
            },
            new ItemSpecRow
            {
                Id = Ids.TomasSchoolbook,
                DisplayName = "Tomas's Schoolbook",
                Description = "A child's schoolbook, open to a page about photosynthesis. 'Plants need sunlight to grow.' There is no sunlight. There hasn't been for two months.",
                Category = ItemCategory.QuestItem,
                Weight = 0.3f,
                StackSize = 1,
                TradeValue = 1f
            },
            new ItemSpecRow
            {
                Id = Ids.MilitiaYouthProgramFlyer,
                DisplayName = "Militia Youth Program Flyer",
                Description = "A printed flyer: 'YOUR CHILD'S FUTURE IS SECURE. The Upland Militia Youth Program provides nutrition, education, and vocational training. Enrollment is mandatory for all non-compliant families.'",
                Category = ItemCategory.QuestItem,
                Weight = 0.02f,
                StackSize = 1,
                TradeValue = 1f
            },

            // ── Comfort Items ─────────────────────────────────────────────
            new ItemSpecRow
            {
                Id = Ids.StampDcoOfficial,
                DisplayName = "DCO Official Stamp",
                Description = "The clerk's stamp from Window Three. The last thing she held. 'NEXT, PLEASE.' She kept stamping blank paper because if she stopped, she'd have to think about the people who weren't coming back.",
                Category = ItemCategory.Comfort,
                Weight = 0.3f,
                StackSize = 1,
                TradeValue = 5f,
                ComfortValue = 3f
            },
            new ItemSpecRow
            {
                Id = Ids.ChildShoeSingle,
                DisplayName = "Child's Shoe (Left, Size 3)",
                Description = "A child's left shoe, still caught in the chain-link fence at Checkpoint Kilo. The child didn't make it over. The shoe stayed.",
                Category = ItemCategory.Comfort,
                Weight = 0.2f,
                StackSize = 1,
                TradeValue = 1f,
                ComfortValue = -2f
            },
            new ItemSpecRow
            {
                Id = Ids.StuffedRabbitChild,
                DisplayName = "Stuffed Rabbit",
                Description = "A child's stuffed rabbit. One ear torn. Found in the holding cell at the Toll House. The child has been quiet for four days. The rabbit has not.",
                Category = ItemCategory.Comfort,
                Weight = 0.2f,
                StackSize = 1,
                TradeValue = 5f,
                ComfortValue = 2f
            },
            new ItemSpecRow
            {
                Id = Ids.LenaStuffedRabbit,
                DisplayName = "Lena's Stuffed Rabbit",
                Description = "A stuffed rabbit on a child's pillow. One ear is torn. Lena, age 5, left it behind when the militia took her. It's been waiting for two months.",
                Category = ItemCategory.Comfort,
                Weight = 0.2f,
                StackSize = 1,
                TradeValue = 2f,
                ComfortValue = -3f
            },

            // ── Lore Items ─────────────────────────────────────────────────
            new ItemSpecRow
            {
                Id = Ids.MilitiaCharterFeedSack,
                DisplayName = "Militia Charter (Feed Sack)",
                Description = "The founding charter of the Upland Militia, written on a feed sack. 'We protect the land. We protect the people on the land. No one takes what we grew.' The ink is faded. The words are still true. The words are also a lie.",
                Category = ItemCategory.Lore,
                Weight = 0.3f,
                StackSize = 1,
                TradeValue = 15f
            },

            // ── Trade Items ────────────────────────────────────────────────
            new ItemSpecRow
            {
                Id = Ids.MilitiaUniformPatch,
                DisplayName = "Militia Uniform Patch",
                Description = "A cloth patch bearing the Upland Militia insignia — a wheat stalk crossed with a rifle. Trade value with militia-aligned traders. Worth nothing to the garrison.",
                Category = ItemCategory.Material,
                Weight = 0.05f,
                StackSize = 5,
                TradeValue = 12f
            },

            // ── Materials ──────────────────────────────────────────────────
            new ItemSpecRow
            {
                Id = Ids.ConvertHandprintPaint,
                DisplayName = "Convert's Handprint Paint",
                Description = "A grey paste of ash and animal fat. Used by the Glow Chapel's converts to mark their handprints on the sanctuary walls. Eighty-three handprints. Three are small.",
                Category = ItemCategory.Material,
                Weight = 0.3f,
                StackSize = 8,
                TradeValue = 3f
            },
            new ItemSpecRow
            {
                Id = Ids.PhysiotherapyBands,
                DisplayName = "Physiotherapy Resistance Bands",
                Description = "Rubber resistance bands from the St. Maren's physio room. Someone was rebuilding their strength here. The bands are still stretched from their last use.",
                Category = ItemCategory.Material,
                Weight = 0.25f,
                StackSize = 6,
                TradeValue = 6f
            },

            // ── Weapons ────────────────────────────────────────────────────
            new ItemSpecRow
            {
                Id = Ids.HuntingRifleBolt,
                DisplayName = "Bolt-Action Hunting Rifle",
                Description = "A pre-war bolt-action hunting rifle from the militia guard tower. Iron sights, wooden stock, scratches on the receiver from years of use. Durability 70.",
                Category = ItemCategory.Weapon,
                Weight = 3.5f,
                StackSize = 1,
                TradeValue = 120f,
                WeaponCaliber = "cal_762x51",
                WeaponDurability = 70
            },
            new ItemSpecRow
            {
                Id = Ids.WeaponShotgunDouble,
                DisplayName = "Double-Barrel Shotgun",
                Description = "A pre-war double-barrel shotgun from the Toll House weapon rack. Simple, reliable, devastating at close range. Durability 60.",
                Category = ItemCategory.Weapon,
                Weight = 3.2f,
                StackSize = 1,
                TradeValue = 140f,
                WeaponCaliber = "cal_12ga",
                WeaponDurability = 60
            },

            // ── Tools / Devices ────────────────────────────────────────────
            new ItemSpecRow
            {
                Id = Ids.WheelchairFolded,
                DisplayName = "Folded Wheelchair",
                Description = "A hospital-issue wheelchair from St. Maren's East Annex. Foldable, lightweight aluminium frame. Can carry an incapacitated survivor or heavy cargo.",
                Category = ItemCategory.Tool,
                Weight = 8.0f,
                StackSize = 1,
                TradeValue = 30f
            },
            new ItemSpecRow
            {
                Id = Ids.RadioBroadcastEquipment,
                DisplayName = "Radio Broadcast Equipment",
                Description = "Salvaged broadcast equipment from Tower 7's base station. Military-grade transmitter, amplifier, and antenna coupling. Needs 3x electronic_scrap to repair. Can broadcast on any frequency.",
                Category = ItemCategory.Tool,
                Weight = 12.0f,
                StackSize = 1,
                TradeValue = 180f
            },
            new ItemSpecRow
            {
                Id = Ids.BackupGeneratorParts,
                DisplayName = "Backup Generator Parts",
                Description = "Salvaged components from Tower 7's backup generator. Alternator, voltage regulator, and cold-start injector. Can be used to repair or upgrade a shelter generator.",
                Category = ItemCategory.Material,
                Weight = 5.0f,
                StackSize = 1,
                TradeValue = 65f
            }
        };

        // ── Spec row data structure ─────────────────────────────────────────
        public class ItemSpecRow
        {
            public string Id;
            public string DisplayName;
            public string Description;
            public ItemCategory Category;
            public float Weight;
            public int StackSize;
            public float TradeValue;
            public float ComfortValue;
            public string WeaponCaliber;
            public float WeaponDurability;
        }

        public enum ItemCategory
        {
            QuestItem,
            Comfort,
            Lore,
            Material,
            Weapon,
            Tool
        }
    }
}
