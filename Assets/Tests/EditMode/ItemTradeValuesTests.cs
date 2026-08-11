using System.Text.RegularExpressions;
using NUnit.Framework;
using AtomicWar._Game.Core;
using AtomicWar._Game.Inventory;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Barter ladder: weapons/self-defense highest, craft scrap lowest.
    /// Shortage spikes meds/food/guns; UI labels never show digits.
    /// </summary>
    [TestFixture]
    public class ItemTradeValuesTests
    {
        private static readonly Regex Digits = new Regex(@"\d", RegexOptions.Compiled);

        [Test]
        public void Weapon_Base_Exceeds_Material_And_Scrap()
        {
            Assert.Greater(Item_TradeValues.BaseWeapon, Item_TradeValues.BaseProtective);
            Assert.Greater(Item_TradeValues.BaseWeapon, Item_TradeValues.BaseMedical);
            Assert.Greater(Item_TradeValues.BaseMedical, Item_TradeValues.BaseBulkMaterial);
            Assert.Greater(Item_TradeValues.BaseBulkMaterial, Item_TradeValues.BaseScrap);
        }

        [Test]
        public void Catalog_Pistols_Outvalue_Wood_And_Sawdust()
        {
            Assert.IsTrue(Item_WorldCatalog.TryGet("pistol_cz75_9x19", out var cz));
            Assert.IsTrue(Item_WorldCatalog.TryGet("pistol_beretta_92_9x19", out var beretta));
            Assert.IsTrue(Item_WorldCatalog.TryGet("pistol_steyr_m9_9x19", out var steyr));
            Assert.IsTrue(Item_WorldCatalog.TryGet("wood_block", out var wood));
            Assert.IsTrue(Item_WorldCatalog.TryGet("sawdust_block", out var sawdust));

            Assert.AreEqual(ItemTradeTier.Weapon, cz.TradeTier);
            Assert.AreEqual(ItemType.Weapon, cz.Type);
            Assert.Greater(cz.TradeValue, wood.TradeValue);
            Assert.Greater(beretta.TradeValue, sawdust.TradeValue);
            Assert.Greater(steyr.TradeValue, wood.TradeValue);
            Assert.Greater(cz.TradeValue, Item_TradeValues.BaseBulkMaterial * 10f);
        }

        [Test]
        public void Expanded_Key_Items_Registered()
        {
            string[] required =
            {
                "wood_block", "sawdust_block", "book", "charcoal", "coal", "sugar",
                "water_purification_tablets_40_of_40", "water_purification_tablets_20_of_40",
                "water_purification_tablets_0_of_40", "flare_red", "flare_green", "flare_yellow",
                "smoke_grenade", "flashbang",
                "workbench_basic", "workbench_intermediate", "workbench_advanced",
                "workbench_professional", "workbench_upgrade_kit", "research_table",
                "basic_cooking_stove", "improvised_cooking_stove", "advanced_cooking_stove",
                "basic_heater", "advanced_heater", "improvised_heater",
                "distiller", "alcohol_distiller", "filter_item",
                "basic_water_boiler", "improvised_water_boiler", "advanced_water_boiler",
                "basic_herb_garden", "improvised_herb_garden", "advanced_herb_garden",
                "herbal_farm_max_tier", "small_animal_trap", "medium_animal_trap",
                "basic_recycler", "improvised_recycler", "advanced_recycle_bench",
                "simple_tool_workshop", "basic_tool_workshop", "improvised_tool_workshop",
                "advanced_tool_workshop", "basic_gunbench", "improvised_gunbench",
                "tactical_weapons_bench", "advanced_tactical_weapon_bench",
                "heater_lamp", "basic_tool_handle", "advanced_tool_handle", "multitool_base",
                "basic_refinement_bench", "improvised_refinement_bench",
                "tactical_refinement_bench", "advanced_tactical_refinement_workshop",
                "dry_yeast_powder", "basic_tobacco_leaf", "quality_tobacco_leaf",
                "basic_rollup_cigarette", "quality_rollup_cigarette", "herbal_cigarette",
                "herbs", "menthol_leaf", "menthol_cigarette", "disposable_vape",
                "ejuice_10ml_10mg", "ejuice_10ml_20mg", "ejuice_20ml_35mg",
                "nicotine_pouch", "quality_tobacco_nicotine_pouch",
                "coffee_arabica_bean", "coffee_robusta_bean", "instant_coffee",
                "instant_coffee_10x_container", "coffee_creamer",
                "tactical_scrap", "tungsten_bar", "titanium_bar",
                "wheat_flour", "oat_flour", "package_rolled_oats_1kg_of_1kg",
                "dry_rice_1kg_of_1kg", "dried_pasta_2kg_of_2kg",
                "emergency_civilian_ration_box_5", "emergency_civilian_ration_1",
                "cooking_oil", "salt", "canned_fish", "soy_and_rice_milk_1l_of_1l",
                "jam_preserves", "box_of_tea_20", "ice_tea_0_5l_package", "herbal_tea",
                "ceramic_water_filter", "plastic_material", "canned_beans",
                "can_opener", "can_breaker", "insulated_flask",
                "herbal_pills", "herbal_bandage", "bandage_roll", "medkit",
                "adhesive_bandages_box_6", "antiseptic_1l_of_1l", "opioid_painkillers",
                "alcohol_wipes_box_10_of_10", "antibiotics_bottle_20", "splint", "epi_pen",
                "thermometer", "medical_scissors", "iodine_pills_bottle_10_of_10",
                "personal_dosimeter", "geiger_counter", "respirator",
                "respirator_filter_box_5", "respirator_filter",
                "protective_goggles", "protective_rubber_gloves",
                "decontamination_soap_5_of_5", "plastic_contamination_bag_box_5",
                "nails", "box_of_nails_10", "box_of_nails_5",
                "military_grade_shovel", "military_grade_sandstone",
                "military_grade_hatchet", "firefighter_grade_fireaxe",
                "pliers", "sewing_kit_10_of_10", "duct_tape",
                "rope_2m_of_2m", "copper_wire_10m_of_10m", "electrical_cable",
                "scrap_wood", "plywood_sheet", "bricks", "cement_mix", "rubber_hose",
                "flashlight", "military_grade_flashlight", "mechanical_parts",
                "car_battery", "rechargeable_battery", "aa_batteries_package_10",
                "matches", "cigarette_lighter", "diamond", "ruby", "sapphire", "amber",
                "hand_crank_radio", "small_solar_panel", "generator", "generator_parts",
                "kerosene_lantern", "jetfuel_jerrycan_10l_of_10l",
                "medium_solar_panel", "generator_alternator", "fuse", "fuse_assortment",
                "circuit_board", "vacuum_tube",
                "winter_coat", "work_boots", "wool_blanket", "improvised_rollup_bed",
                "woolbed", "advanced_heating_bed", "wool_gloves",
                "family_photograph", "cassette_tape", "sealed_government_document",
                "pistol_cz75_9x19", "pistol_beretta_92_9x19", "pistol_steyr_m9_9x19",
                "pistol_walther_ppk_380acp", "pistol_grand_power_p380_380acp",
                "pistol_cz52_762x25", "pistol_norinco_type54_762x25", "pistol_zastava_m57_762x25",
                "smg_m1928a1_thompson_45acp", "smg_hk_ump45_45acp", "smg_kriss_vector_45acp",
                "pistol_bt_apc45_mini_45acp", "smg_bt_apc45_45acp",
                "smg_sites_spectre_m4_9x21", "smg_imi_micro_uzi_9x21", "smg_cz_scorpion_evo3_9x21",
                "smg_steyr_solo_s1_100_765x21", "smg_mp34_765x21",
                "shotgun_benelli_m4_super90_12ga", "shotgun_remington_model1100_12ga",
                "shotgun_browning_auto5_16ga", "shotgun_franchi_al48_16ga",
                "rifle_m4a1_carbine_556x45", "rifle_hk416_556x45", "rifle_fn_scar_l_556x45", "rifle_steyr_aug_a3_556x45",
                "rifle_ak47_762x39", "rifle_cmmg_mk47_mutant_762x39",
                "lmg_rpk74_545x39", "rifle_ak74u_545x39",
                "rifle_fn_fal_762x51", "rifle_hk_g3_762x51",
                "rifle_q_honey_badger_300blk", "rifle_sig_mcx_rattler_300blk", "rifle_ddm4_pdw_300blk",
                "pdw_fn_p90_57x28", "carbine_ruger_lc_57x28",
                "pdw_hk_mp7a2_46x30", "pdw_cmmg_four6_46x30", "pdw_tb_tactical_t7_46x30",
                "sniper_mosin_nagant_m9031_762x54r", "sniper_svd_dragunov_762x54r", "sniper_romak3_psl_762x54r",
                "sniper_steyr_ssg08_338lapua", "sniper_sako_trg42_338lapua", "sniper_dsr1_338lapua",
                "sniper_cheytac_m200_intervention_408cheytac", "sniper_voere_mk_x3_408cheytac", "sniper_voere_mk_x4_408cheytac",
                "sniper_barrett_m82a1_50bmg"
            };

            foreach (var id in required)
            {
                Assert.IsTrue(Item_WorldCatalog.Contains(id), "missing catalog id: " + id);
                Assert.IsTrue(Item_WorldCatalog.TryGet(id, out var def), id);
                Assert.GreaterOrEqual(def.TradeValue, 0f, id);
            }
        }

        [Test]
        public void Purification_Tablets_Have_Fill_And_Drain()
        {
            Assert.IsTrue(Item_WorldCatalog.TryGet(
                "water_purification_tablets_40_of_40", out var full));
            Assert.IsTrue(Item_WorldCatalog.TryGet(
                "water_purification_tablets_20_of_40", out var half));
            Assert.IsTrue(Item_WorldCatalog.TryGet(
                "water_purification_tablets_0_of_40", out var empty));

            Assert.AreEqual(40f, full.Capacity, 0.01f);
            Assert.AreEqual(40f, full.Fill, 0.01f);
            Assert.AreEqual(2f, full.DrainPerUse, 0.01f);
            Assert.AreEqual(20f, half.Fill, 0.01f);
            Assert.AreEqual(0f, empty.Fill, 0.01f);
            Assert.Greater(full.TradeValue, half.TradeValue);
            Assert.Greater(half.TradeValue, empty.TradeValue);
        }

        [Test]
        public void Shortage_Raises_Medical_And_Weapon_Softens_Precious()
        {
            float medBase = Item_TradeValues.BaseMedical;
            float gunBase = Item_TradeValues.BaseWeapon;
            float gemBase = Item_TradeValues.BasePrecious;

            float medShort = Item_TradeValues.Resolve(medBase, ItemTradeTier.Medical, 1f, true);
            float gunShort = Item_TradeValues.Resolve(gunBase, ItemTradeTier.Weapon, 1f, true);
            float gemShort = Item_TradeValues.Resolve(gemBase, ItemTradeTier.Precious, 1f, true);

            Assert.Greater(medShort, medBase);
            Assert.Greater(gunShort, gunBase);
            Assert.Less(gemShort, gemBase);

            float foodShort = Item_TradeValues.Resolve(
                Item_TradeValues.BaseConsumable, ItemTradeTier.Consumable, 1f, true);
            Assert.Greater(foodShort, Item_TradeValues.BaseConsumable);
        }

        [Test]
        public void Demand_Multiplier_Scales_Resolved_Value()
        {
            float neutral = Item_TradeValues.Resolve(10f, ItemTradeTier.UtilityTool, 1f, false);
            float scarce = Item_TradeValues.Resolve(10f, ItemTradeTier.UtilityTool, 2f, false);
            Assert.AreEqual(10f, neutral, 0.001f);
            Assert.AreEqual(20f, scarce, 0.001f);
        }

        [Test]
        public void FormatWorthLabel_Never_Contains_Digits()
        {
            float[] samples =
            {
                0f, 0.3f, 1f, 3f, 8f, 20f, 40f, 80f, 100f, 150f
            };
            foreach (var v in samples)
            {
                string label = Item_TradeValues.FormatWorthLabel(v);
                Assert.IsFalse(string.IsNullOrEmpty(label), "empty label for " + v);
                Assert.IsFalse(Digits.IsMatch(label), "digits in label: " + label);
            }

            // Catalog path also digit-free
            string pistolLabel = Item_WorldCatalog.FormatWorthLabel("pistol_cz75_9x19");
            string scrapLabel = Item_WorldCatalog.FormatWorthLabel("sawdust_block");
            Assert.IsFalse(Digits.IsMatch(pistolLabel));
            Assert.IsFalse(Digits.IsMatch(scrapLabel));
            // CZ-75 base 105 → "rare"; sawdust floor → "scrap"
            Assert.AreEqual("rare", pistolLabel);
            Assert.AreEqual("scrap", scrapLabel);
        }

        [Test]
        public void Quest_Item_Has_Zero_Trade_Value()
        {
            Assert.IsTrue(Item_WorldCatalog.TryGet("sealed_government_document", out var doc));
            Assert.AreEqual(ItemTradeTier.Quest, doc.TradeTier);
            Assert.AreEqual(0f, doc.TradeValue, 0.001f);
            Assert.AreEqual(0f, Item_WorldCatalog.ResolveTradeValue(doc.Id), 0.001f);
            Assert.AreEqual("worthless", Item_WorldCatalog.FormatWorthLabel(doc.Id));
        }

        [Test]
        public void Station_Ladder_Increases_With_Level()
        {
            Assert.Less(Item_TradeValues.StationBase(0), Item_TradeValues.StationBase(1));
            Assert.Less(Item_TradeValues.StationBase(1), Item_TradeValues.StationBase(2));
            Assert.Less(Item_TradeValues.StationBase(2), Item_TradeValues.StationBase(3));
            Assert.Less(Item_TradeValues.StationBase(3), Item_TradeValues.StationBase(4));

            Assert.IsTrue(Item_WorldCatalog.TryGet("workbench_basic", out var basic));
            Assert.IsTrue(Item_WorldCatalog.TryGet("workbench_professional", out var pro));
            Assert.Greater(pro.TradeValue, basic.TradeValue);
            Assert.AreEqual(ItemTradeTier.Station, basic.TradeTier);
        }

        [Test]
        public void ScaleByFill_Empty_Is_Floor_Not_Zero()
        {
            float full = 20f;
            float empty = Item_TradeValues.ScaleByFill(full, 0f, 40f, emptyFloor: 0.15f);
            float mid = Item_TradeValues.ScaleByFill(full, 20f, 40f, emptyFloor: 0.15f);
            Assert.AreEqual(full * 0.15f, empty, 0.01f);
            Assert.Greater(mid, empty);
            Assert.Less(mid, full);
        }

        [Test]
        public void GetTradeValue_Uses_Explicit_TradeTier_From_ItemDefinition()
        {
            var eco = new AtomicWar._Game.Economy.DynamicEconomySystem();
            var item = UnityEngine.ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = "custom_attachment";
            // UtilityTool is a trade *tier*, not an ItemType. ItemType.Tool is the
            // type that InferTier maps to ItemTradeTier.UtilityTool.
            item.type = ItemType.Tool;
            // Left at 0 deliberately: Item_TradeValues.Resolve only consults the tier's
            // base value when the item carries no explicit tradeValue, so a non-zero
            // tradeValue here would make both branches return the same number and the
            // assertion below could never distinguish explicit from inferred.
            item.tradeValue = 0f;
            item.tradeTier = ItemTradeTier.Attachment;

            float valWithExplicitTier = eco.GetTradeValue(item);

            // Change tradeTier back to Scrap (unspecified) so it falls back to InferTier(ItemType.Tool)
            item.tradeTier = ItemTradeTier.Scrap;
            float valWithInferredTier = eco.GetTradeValue(item);

            Assert.AreNotEqual(valWithExplicitTier, valWithInferredTier);
        }

        [Test]
        public void Catalog_Count_Includes_Expanded_Set()
        {
            // Original ~70 + expanded ~184
            Assert.Greater(Item_WorldCatalog.AllIds().Count, 200);
        }

        [Test]
        public void New_Firearms_Registered_With_Weapon_TradeTier()
        {
            string[] newGuns =
            {
                "pistol_walther_ppk_380acp",
                "pistol_grand_power_p380_380acp",
                "pistol_cz52_762x25",
                "pistol_norinco_type54_762x25",
                "pistol_zastava_m57_762x25",
                "smg_m1928a1_thompson_45acp",
                "smg_hk_ump45_45acp",
                "smg_kriss_vector_45acp",
                "pistol_bt_apc45_mini_45acp",
                "smg_bt_apc45_45acp",
                "smg_sites_spectre_m4_9x21",
                "smg_imi_micro_uzi_9x21",
                "smg_cz_scorpion_evo3_9x21",
                "smg_steyr_solo_s1_100_765x21",
                "smg_mp34_765x21",
                "shotgun_benelli_m4_super90_12ga",
                "shotgun_remington_model1100_12ga",
                "shotgun_browning_auto5_16ga",
                "shotgun_franchi_al48_16ga",
                "rifle_m4a1_carbine_556x45",
                "rifle_hk416_556x45",
                "rifle_fn_scar_l_556x45",
                "rifle_steyr_aug_a3_556x45",
                "rifle_ak47_762x39",
                "rifle_cmmg_mk47_mutant_762x39",
                "lmg_rpk74_545x39",
                "rifle_ak74u_545x39",
                "rifle_fn_fal_762x51",
                "rifle_hk_g3_762x51",
                "rifle_q_honey_badger_300blk",
                "rifle_sig_mcx_rattler_300blk",
                "rifle_ddm4_pdw_300blk",
                "pdw_fn_p90_57x28",
                "carbine_ruger_lc_57x28",
                "pdw_hk_mp7a2_46x30",
                "pdw_cmmg_four6_46x30",
                "pdw_tb_tactical_t7_46x30",
                "sniper_mosin_nagant_m9031_762x54r",
                "sniper_svd_dragunov_762x54r",
                "sniper_romak3_psl_762x54r",
                "sniper_steyr_ssg08_338lapua",
                "sniper_sako_trg42_338lapua",
                "sniper_dsr1_338lapua",
                "sniper_cheytac_m200_intervention_408cheytac",
                "sniper_voere_mk_x3_408cheytac",
                "sniper_voere_mk_x4_408cheytac",
                "sniper_barrett_m82a1_50bmg"
            };

            foreach (var gunId in newGuns)
            {
                Assert.IsTrue(Item_WorldCatalog.Contains(gunId), "Missing gun: " + gunId);
                Assert.IsTrue(Item_WorldCatalog.TryGet(gunId, out var def), gunId);
                Assert.AreEqual(ItemType.Weapon, def.Type, gunId);
                Assert.AreEqual(ItemTradeTier.Weapon, def.TradeTier, gunId);
                Assert.Greater(def.TradeValue, 50f, gunId);
                Assert.IsTrue(def.MilitaryGrade, gunId);
            }
        }
    }
}
