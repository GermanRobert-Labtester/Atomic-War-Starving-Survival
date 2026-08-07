using AtomicWar._Game.Inventory;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Expanded world catalog: materials, stations, medical, food packs,
    /// protective gear, and starter pistols. Trade values follow
    /// <see cref="Item_TradeValues"/> (weapons highest, scrap lowest).
    /// </summary>
    public static partial class Item_WorldCatalog
    {
        public const string WoodBlock = "wood_block";
        public const string SawdustBlock = "sawdust_block";
        public const string Book = "book";
        public const string Charcoal = "charcoal";
        public const string Coal = "coal";
        public const string Sugar = "sugar";
        public const string ScrapWood = "scrap_wood";
        public const string PlywoodSheet = "plywood_sheet";
        public const string Bricks = "bricks";
        public const string CementMix = "cement_mix";
        public const string PlasticMaterial = "plastic_material";
        public const string TacticalScrap = "tactical_scrap";
        public const string TungstenBar = "tungsten_bar";
        public const string TitaniumBar = "titanium_bar";
        public const string Nails = "nails";
        public const string BoxOfNails10 = "box_of_nails_10";
        public const string BoxOfNails5 = "box_of_nails_5";
        public const string DuctTape = "duct_tape";
        public const string Rope2MOf2M = "rope_2m_of_2m";
        public const string CopperWire10MOf10M = "copper_wire_10m_of_10m";
        public const string ElectricalCable = "electrical_cable";
        public const string RubberHose = "rubber_hose";
        public const string MechanicalParts = "mechanical_parts";
        public const string Fuse = "fuse";
        public const string FuseAssortment = "fuse_assortment";
        public const string CircuitBoard = "circuit_board";
        public const string VacuumTube = "vacuum_tube";
        public const string GeneratorParts = "generator_parts";
        public const string GeneratorAlternator = "generator_alternator";
        public const string BasicToolHandle = "basic_tool_handle";
        public const string AdvancedToolHandle = "advanced_tool_handle";
        public const string MultitoolBase = "multitool_base";
        public const string DryYeastPowder = "dry_yeast_powder";
        public const string WheatFlour = "wheat_flour";
        public const string OatFlour = "oat_flour";
        public const string CookingOil = "cooking_oil";
        public const string Salt = "salt";
        public const string MilitaryGradeSandstone = "military_grade_sandstone";
        public const string WaterPurificationTablet = "water_purification_tablet";
        public const string WaterPurificationTablets40Of40 = "water_purification_tablets_40_of_40";
        public const string WaterPurificationTablets20Of40 = "water_purification_tablets_20_of_40";
        public const string WaterPurificationTablets0Of40 = "water_purification_tablets_0_of_40";
        public const string FlareRed = "flare_red";
        public const string FlareGreen = "flare_green";
        public const string FlareYellow = "flare_yellow";
        public const string SmokeGrenade = "smoke_grenade";
        public const string Flashbang = "flashbang";
        public const string WorkbenchBasic = "workbench_basic";
        public const string WorkbenchIntermediate = "workbench_intermediate";
        public const string WorkbenchAdvanced = "workbench_advanced";
        public const string WorkbenchProfessional = "workbench_professional";
        public const string WorkbenchUpgradeKit = "workbench_upgrade_kit";
        public const string ResearchTable = "research_table";
        public const string BasicCookingStove = "basic_cooking_stove";
        public const string ImprovisedCookingStove = "improvised_cooking_stove";
        public const string AdvancedCookingStove = "advanced_cooking_stove";
        public const string BasicHeater = "basic_heater";
        public const string ImprovisedHeater = "improvised_heater";
        public const string AdvancedHeater = "advanced_heater";
        public const string HeaterLamp = "heater_lamp";
        public const string Distiller = "distiller";
        public const string AlcoholDistiller = "alcohol_distiller";
        public const string FilterItem = "filter_item";
        public const string BasicWaterBoiler = "basic_water_boiler";
        public const string ImprovisedWaterBoiler = "improvised_water_boiler";
        public const string AdvancedWaterBoiler = "advanced_water_boiler";
        public const string BasicHerbGarden = "basic_herb_garden";
        public const string ImprovisedHerbGarden = "improvised_herb_garden";
        public const string AdvancedHerbGarden = "advanced_herb_garden";
        public const string HerbalFarmMaxTier = "herbal_farm_max_tier";
        public const string SmallAnimalTrap = "small_animal_trap";
        public const string MediumAnimalTrap = "medium_animal_trap";
        public const string BasicRecycler = "basic_recycler";
        public const string ImprovisedRecycler = "improvised_recycler";
        public const string AdvancedRecycleBench = "advanced_recycle_bench";
        public const string SimpleToolWorkshop = "simple_tool_workshop";
        public const string BasicToolWorkshop = "basic_tool_workshop";
        public const string ImprovisedToolWorkshop = "improvised_tool_workshop";
        public const string AdvancedToolWorkshop = "advanced_tool_workshop";
        public const string BasicGunbench = "basic_gunbench";
        public const string ImprovisedGunbench = "improvised_gunbench";
        public const string TacticalWeaponsBench = "tactical_weapons_bench";
        public const string AdvancedTacticalWeaponBench = "advanced_tactical_weapon_bench";
        public const string BasicRefinementBench = "basic_refinement_bench";
        public const string ImprovisedRefinementBench = "improvised_refinement_bench";
        public const string TacticalRefinementBench = "tactical_refinement_bench";
        public const string AdvancedTacticalRefinementWorkshop = "advanced_tactical_refinement_workshop";
        public const string BasicTobaccoLeaf = "basic_tobacco_leaf";
        public const string QualityTobaccoLeaf = "quality_tobacco_leaf";
        public const string BasicRollupCigarette = "basic_rollup_cigarette";
        public const string QualityRollupCigarette = "quality_rollup_cigarette";
        public const string HerbalCigarette = "herbal_cigarette";
        public const string Herbs = "herbs";
        public const string MentholLeaf = "menthol_leaf";
        public const string MentholCigarette = "menthol_cigarette";
        public const string DisposableVape = "disposable_vape";
        public const string Ejuice10Ml10Mg = "ejuice_10ml_10mg";
        public const string Ejuice10Ml20Mg = "ejuice_10ml_20mg";
        public const string Ejuice20Ml35Mg = "ejuice_20ml_35mg";
        public const string NicotinePouch = "nicotine_pouch";
        public const string QualityTobaccoNicotinePouch = "quality_tobacco_nicotine_pouch";
        public const string CoffeeArabicaBean = "coffee_arabica_bean";
        public const string CoffeeRobustaBean = "coffee_robusta_bean";
        public const string InstantCoffee = "instant_coffee";
        public const string InstantCoffee10XContainer = "instant_coffee_10x_container";
        public const string CoffeeCreamer = "coffee_creamer";
        public const string BoxOfTea20 = "box_of_tea_20";
        public const string IceTea05LPackage = "ice_tea_0_5l_package";
        public const string HerbalTea = "herbal_tea";
        public const string PackageRolledOats1KgOf1Kg = "package_rolled_oats_1kg_of_1kg";
        public const string DryRice1KgOf1Kg = "dry_rice_1kg_of_1kg";
        public const string DriedPasta2KgOf2Kg = "dried_pasta_2kg_of_2kg";
        public const string SoyAndRiceMilk1LOf1L = "soy_and_rice_milk_1l_of_1l";
        public const string EmergencyCivilianRationBox5 = "emergency_civilian_ration_box_5";
        public const string EmergencyCivilianRation1 = "emergency_civilian_ration_1";
        public const string CannedFish = "canned_fish";
        public const string CannedBeans = "canned_beans";
        public const string JamPreserves = "jam_preserves";
        public const string BasicBreakfastBowl = "basic_breakfast_bowl";
        public const string QualityDinnerBowl = "quality_dinner_bowl";
        public const string HighQualityOatBreakfast = "high_quality_oat_breakfast";
        public const string CeramicWaterFilter = "ceramic_water_filter";
        public const string CanOpener = "can_opener";
        public const string CanBreaker = "can_breaker";
        public const string InsulatedFlask = "insulated_flask";
        public const string HerbalPills = "herbal_pills";
        public const string HerbalBandage = "herbal_bandage";
        public const string BandageRoll = "bandage_roll";
        public const string Medkit = "medkit";
        public const string AdhesiveBandagesBox6 = "adhesive_bandages_box_6";
        public const string Antiseptic1LOf1L = "antiseptic_1l_of_1l";
        public const string SterilisedBandage = "sterilised_bandage";
        public const string OpioidPainkillers = "opioid_painkillers";
        public const string AlcoholWipesBox10Of10 = "alcohol_wipes_box_10_of_10";
        public const string AntibioticsBottle20 = "antibiotics_bottle_20";
        public const string Splint = "splint";
        public const string EpiPen = "epi_pen";
        public const string Thermometer = "thermometer";
        public const string MedicalScissors = "medical_scissors";
        public const string IodinePillsBottle10Of10 = "iodine_pills_bottle_10_of_10";
        public const string PersonalDosimeter = "personal_dosimeter";
        public const string GeigerCounter = "geiger_counter";
        public const string Respirator = "respirator";
        public const string RespiratorFilterBox5 = "respirator_filter_box_5";
        public const string RespiratorFilter = "respirator_filter";
        public const string ProtectiveGoggles = "protective_goggles";
        public const string ProtectiveRubberGloves = "protective_rubber_gloves";
        public const string DecontaminationSoap5Of5 = "decontamination_soap_5_of_5";
        public const string PlasticContaminationBagBox5 = "plastic_contamination_bag_box_5";
        public const string MilitaryGradeShovel = "military_grade_shovel";
        public const string MilitaryGradeHatchet = "military_grade_hatchet";
        public const string FirefighterGradeFireaxe = "firefighter_grade_fireaxe";
        public const string Pliers = "pliers";
        public const string SewingKit10Of10 = "sewing_kit_10_of_10";
        public const string Flashlight = "flashlight";
        public const string MilitaryGradeFlashlight = "military_grade_flashlight";
        public const string Matches = "matches";
        public const string CigaretteLighter = "cigarette_lighter";
        public const string CarBattery = "car_battery";
        public const string RechargeableBattery = "rechargeable_battery";
        public const string AaBatteriesPackage10 = "aa_batteries_package_10";
        public const string HandCrankRadio = "hand_crank_radio";
        public const string SmallSolarPanel = "small_solar_panel";
        public const string MediumSolarPanel = "medium_solar_panel";
        public const string Generator = "generator";
        public const string KeroseneLantern = "kerosene_lantern";
        public const string JetfuelJerrycan10LOf10L = "jetfuel_jerrycan_10l_of_10l";
        public const string WinterCoat = "winter_coat";
        public const string WorkBoots = "work_boots";
        public const string WoolBlanket = "wool_blanket";
        public const string ImprovisedRollupBed = "improvised_rollup_bed";
        public const string Woolbed = "woolbed";
        public const string AdvancedHeatingBed = "advanced_heating_bed";
        public const string WoolGloves = "wool_gloves";
        public const string FamilyPhotograph = "family_photograph";
        public const string CassetteTape = "cassette_tape";
        public const string SealedGovernmentDocument = "sealed_government_document";
        public const string Diamond = "diamond";
        public const string Ruby = "ruby";
        public const string Sapphire = "sapphire";
        public const string Amber = "amber";
        public const string PistolCz759X19 = "pistol_cz75_9x19";
        public const string PistolBeretta929X19 = "pistol_beretta_92_9x19";
        public const string PistolSteyrM99X19 = "pistol_steyr_m9_9x19";

        private static void BuildExpanded()
        {
            Add(new WorldItemDef
            {
                Id = "wood_block",
                DisplayName = "Wood Block",
                Description = "Sawn timber block. Framing and fuel.",
                Type = ItemType.Material,
                StackMax = 40,
                Weight = 1.2f,
                TradeValue = 1.0f,
                TradeTier = ItemTradeTier.BulkMaterial,
            });
            Add(new WorldItemDef
            {
                Id = "sawdust_block",
                DisplayName = "Sawdust Block",
                Description = "Compressed sawdust. Kindling and filler.",
                Type = ItemType.Material,
                StackMax = 30,
                Weight = 0.4f,
                TradeValue = 0.35f,
                TradeTier = ItemTradeTier.Scrap,
            });
            Add(new WorldItemDef
            {
                Id = "book",
                DisplayName = "Book",
                Description = "Bound pages. Morale, kindling, or knowledge.",
                Type = ItemType.Comfort,
                StackMax = 10,
                Weight = 0.4f,
                TradeValue = 3.0f,
                TradeTier = ItemTradeTier.Consumable,
                MoraleEffect = 2f,
            });
            Add(new WorldItemDef
            {
                Id = "charcoal",
                DisplayName = "Charcoal",
                Description = "Burned wood. Filters and slow heat.",
                Type = ItemType.Material,
                StackMax = 40,
                Weight = 0.2f,
                TradeValue = 1.1f,
                TradeTier = ItemTradeTier.BulkMaterial,
            });
            Add(new WorldItemDef
            {
                Id = "coal",
                DisplayName = "Coal",
                Description = "Hard coal. Long burn for heaters.",
                Type = ItemType.Fuel,
                StackMax = 30,
                Weight = 0.5f,
                TradeValue = 2.0f,
                TradeTier = ItemTradeTier.Ingredient,
            });
            Add(new WorldItemDef
            {
                Id = "sugar",
                DisplayName = "Sugar",
                Description = "White crystals. Cooking and bad morale swaps.",
                Type = ItemType.Material,
                StackMax = 25,
                Weight = 0.3f,
                TradeValue = 2.2f,
                TradeTier = ItemTradeTier.Ingredient,
            });
            Add(new WorldItemDef
            {
                Id = "scrap_wood",
                DisplayName = "Scrap Wood",
                Description = "Broken boards. Crude repairs.",
                Type = ItemType.Material,
                StackMax = 40,
                Weight = 0.8f,
                TradeValue = 0.6f,
                TradeTier = ItemTradeTier.Scrap,
            });
            Add(new WorldItemDef
            {
                Id = "plywood_sheet",
                DisplayName = "Plywood Sheet",
                Description = "Thin sheet. Walls and shutters.",
                Type = ItemType.Material,
                StackMax = 15,
                Weight = 2.5f,
                TradeValue = 2.5f,
                TradeTier = ItemTradeTier.BulkMaterial,
            });
            Add(new WorldItemDef
            {
                Id = "bricks",
                DisplayName = "Bricks",
                Description = "Fired clay. Shelter upgrades.",
                Type = ItemType.Material,
                StackMax = 20,
                Weight = 2.0f,
                TradeValue = 1.8f,
                TradeTier = ItemTradeTier.BulkMaterial,
            });
            Add(new WorldItemDef
            {
                Id = "cement_mix",
                DisplayName = "Cement Mix",
                Description = "Dry cement. Needs 1L water for hatch/shelter work.",
                Type = ItemType.Material,
                StackMax = 10,
                Weight = 5.0f,
                TradeValue = 4.0f,
                TradeTier = ItemTradeTier.Ingredient,
                Capacity = 1f,
                Fill = 1f,
                DrainPerUse = 1f,
                CapacityUnit = "batch",
            });
            Add(new WorldItemDef
            {
                Id = "plastic_material",
                DisplayName = "Plastic Material",
                Description = "Salvaged plastic stock.",
                Type = ItemType.Material,
                StackMax = 40,
                Weight = 0.15f,
                TradeValue = 0.9f,
                TradeTier = ItemTradeTier.BulkMaterial,
            });
            Add(new WorldItemDef
            {
                Id = "tactical_scrap",
                DisplayName = "Tactical Scrap",
                Description = "Mil-spec offcuts. Rare salvage.",
                Type = ItemType.Material,
                StackMax = 20,
                Weight = 0.4f,
                TradeValue = 6.0f,
                TradeTier = ItemTradeTier.Ingredient,
                MilitaryGrade = true,
            });
            Add(new WorldItemDef
            {
                Id = "tungsten_bar",
                DisplayName = "Tungsten Bar",
                Description = "Dense hard metal bar.",
                Type = ItemType.Material,
                StackMax = 5,
                Weight = 1.5f,
                TradeValue = 28.0f,
                TradeTier = ItemTradeTier.Precious,
                ExtremelyRare = true,
            });
            Add(new WorldItemDef
            {
                Id = "titanium_bar",
                DisplayName = "Titanium Bar",
                Description = "Light strong metal bar.",
                Type = ItemType.Material,
                StackMax = 5,
                Weight = 0.9f,
                TradeValue = 32.0f,
                TradeTier = ItemTradeTier.Precious,
                ExtremelyRare = true,
            });
            Add(new WorldItemDef
            {
                Id = "nails",
                DisplayName = "Nails",
                Description = "Loose nails. Framing.",
                Type = ItemType.Material,
                StackMax = 80,
                Weight = 0.02f,
                TradeValue = 0.3f,
                TradeTier = ItemTradeTier.Scrap,
            });
            Add(new WorldItemDef
            {
                Id = "box_of_nails_10",
                DisplayName = "Box of Nails (10x)",
                Description = "Ten nails boxed.",
                Type = ItemType.Material,
                StackMax = 20,
                Weight = 0.25f,
                TradeValue = 2.5f,
                TradeTier = ItemTradeTier.BulkMaterial,
            });
            Add(new WorldItemDef
            {
                Id = "box_of_nails_5",
                DisplayName = "Box of Nails (5x)",
                Description = "Five nails boxed.",
                Type = ItemType.Material,
                StackMax = 25,
                Weight = 0.15f,
                TradeValue = 1.4f,
                TradeTier = ItemTradeTier.BulkMaterial,
            });
            Add(new WorldItemDef
            {
                Id = "duct_tape",
                DisplayName = "Duct Tape",
                Description = "Grey tape. Fixes everything poorly.",
                Type = ItemType.Material,
                StackMax = 15,
                Weight = 0.15f,
                TradeValue = 3.5f,
                TradeTier = ItemTradeTier.Ingredient,
            });
            Add(new WorldItemDef
            {
                Id = "rope_2m_of_2m",
                DisplayName = "Rope 2M / 2M",
                Description = "Two metres of rope. 0.4M per use.",
                Type = ItemType.Material,
                StackMax = 5,
                Weight = 0.6f,
                TradeValue = 5.0f,
                TradeTier = ItemTradeTier.UtilityTool,
                Capacity = 2f,
                Fill = 2f,
                DrainPerUse = 0.4f,
                CapacityUnit = "m",
            });
            Add(new WorldItemDef
            {
                Id = "copper_wire_10m_of_10m",
                DisplayName = "Copper Wire 10M / 10M",
                Description = "Ten metres copper. 1M per use.",
                Type = ItemType.Material,
                StackMax = 5,
                Weight = 0.5f,
                TradeValue = 6.0f,
                TradeTier = ItemTradeTier.Ingredient,
                Capacity = 10f,
                Fill = 10f,
                DrainPerUse = 1f,
                CapacityUnit = "m",
            });
            Add(new WorldItemDef
            {
                Id = "electrical_cable",
                DisplayName = "Electrical Cable",
                Description = "Insulated cable length.",
                Type = ItemType.Material,
                StackMax = 20,
                Weight = 0.3f,
                TradeValue = 2.8f,
                TradeTier = ItemTradeTier.BulkMaterial,
            });
            Add(new WorldItemDef
            {
                Id = "rubber_hose",
                DisplayName = "Rubber Hose",
                Description = "Flexible hose. Plumbing and siphons.",
                Type = ItemType.Material,
                StackMax = 15,
                Weight = 0.5f,
                TradeValue = 2.2f,
                TradeTier = ItemTradeTier.BulkMaterial,
            });
            Add(new WorldItemDef
            {
                Id = "mechanical_parts",
                DisplayName = "Mechanical Parts",
                Description = "Gears, bolts, springs.",
                Type = ItemType.Material,
                StackMax = 30,
                Weight = 0.25f,
                TradeValue = 3.0f,
                TradeTier = ItemTradeTier.Ingredient,
            });
            Add(new WorldItemDef
            {
                Id = "fuse",
                DisplayName = "Fuse",
                Description = "Single electrical fuse.",
                Type = ItemType.Material,
                StackMax = 30,
                Weight = 0.02f,
                TradeValue = 1.0f,
                TradeTier = ItemTradeTier.BulkMaterial,
            });
            Add(new WorldItemDef
            {
                Id = "fuse_assortment",
                DisplayName = "Fuse Assortment",
                Description = "Mixed fuse set.",
                Type = ItemType.Material,
                StackMax = 10,
                Weight = 0.15f,
                TradeValue = 4.5f,
                TradeTier = ItemTradeTier.Ingredient,
            });
            Add(new WorldItemDef
            {
                Id = "circuit_board",
                DisplayName = "Circuit Board",
                Description = "Salvaged board. Electronics craft.",
                Type = ItemType.Material,
                StackMax = 15,
                Weight = 0.1f,
                TradeValue = 8.0f,
                TradeTier = ItemTradeTier.SurvivalGear,
            });
            Add(new WorldItemDef
            {
                Id = "vacuum_tube",
                DisplayName = "Vacuum Tube",
                Description = "Glass tube. Old radios and quirks.",
                Type = ItemType.Material,
                StackMax = 10,
                Weight = 0.08f,
                TradeValue = 5.5f,
                TradeTier = ItemTradeTier.Ingredient,
            });
            Add(new WorldItemDef
            {
                Id = "generator_parts",
                DisplayName = "Generator Parts",
                Description = "Spare gen components.",
                Type = ItemType.Material,
                StackMax = 8,
                Weight = 1.2f,
                TradeValue = 14.0f,
                TradeTier = ItemTradeTier.SurvivalGear,
            });
            Add(new WorldItemDef
            {
                Id = "generator_alternator",
                DisplayName = "Generator Alternator",
                Description = "Heavy alternator core.",
                Type = ItemType.Material,
                StackMax = 3,
                Weight = 4.0f,
                TradeValue = 22.0f,
                TradeTier = ItemTradeTier.SurvivalGear,
            });
            Add(new WorldItemDef
            {
                Id = "basic_tool_handle",
                DisplayName = "Basic Tool Handle",
                Description = "Wooden handle blank.",
                Type = ItemType.Material,
                StackMax = 20,
                Weight = 0.2f,
                TradeValue = 1.0f,
                TradeTier = ItemTradeTier.BulkMaterial,
            });
            Add(new WorldItemDef
            {
                Id = "advanced_tool_handle",
                DisplayName = "Advanced Tool Handle",
                Description = "Reinforced handle blank.",
                Type = ItemType.Material,
                StackMax = 12,
                Weight = 0.25f,
                TradeValue = 3.5f,
                TradeTier = ItemTradeTier.Ingredient,
            });
            Add(new WorldItemDef
            {
                Id = "multitool_base",
                DisplayName = "Multitool Base",
                Description = "Frame for multitool craft.",
                Type = ItemType.Material,
                StackMax = 10,
                Weight = 0.2f,
                TradeValue = 4.0f,
                TradeTier = ItemTradeTier.Ingredient,
            });
            Add(new WorldItemDef
            {
                Id = "dry_yeast_powder",
                DisplayName = "Dry Yeast Powder",
                Description = "Baking and brewing yeast.",
                Type = ItemType.Material,
                StackMax = 20,
                Weight = 0.05f,
                TradeValue = 2.0f,
                TradeTier = ItemTradeTier.Ingredient,
            });
            Add(new WorldItemDef
            {
                Id = "wheat_flour",
                DisplayName = "Wheat Flour",
                Description = "Milled wheat. Bread and batter.",
                Type = ItemType.Material,
                StackMax = 15,
                Weight = 1.0f,
                TradeValue = 2.8f,
                TradeTier = ItemTradeTier.Ingredient,
            });
            Add(new WorldItemDef
            {
                Id = "oat_flour",
                DisplayName = "Oat Flour (High Quality)",
                Description = "Fine oat flour.",
                Type = ItemType.Material,
                StackMax = 12,
                Weight = 1.0f,
                TradeValue = 3.5f,
                TradeTier = ItemTradeTier.Ingredient,
            });
            Add(new WorldItemDef
            {
                Id = "cooking_oil",
                DisplayName = "Cooking Oil",
                Description = "Cooking fat. Stoves and recipes.",
                Type = ItemType.Material,
                StackMax = 12,
                Weight = 0.9f,
                TradeValue = 3.2f,
                TradeTier = ItemTradeTier.Ingredient,
            });
            Add(new WorldItemDef
            {
                Id = "salt",
                DisplayName = "Salt",
                Description = "Preserving and seasoning.",
                Type = ItemType.Material,
                StackMax = 25,
                Weight = 0.3f,
                TradeValue = 1.8f,
                TradeTier = ItemTradeTier.Ingredient,
            });
            Add(new WorldItemDef
            {
                Id = "military_grade_sandstone",
                DisplayName = "Military Grade Sandstone",
                Description = "Hard cut stone. Fortification.",
                Type = ItemType.Material,
                StackMax = 10,
                Weight = 3.0f,
                TradeValue = 8.0f,
                TradeTier = ItemTradeTier.BulkMaterial,
                MilitaryGrade = true,
            });
            Add(new WorldItemDef
            {
                Id = "water_purification_tablet",
                DisplayName = "Water Purification Tablet",
                Description = "Single tablet. Treats a bottle.",
                Type = ItemType.Medical,
                StackMax = 40,
                Weight = 0.01f,
                TradeValue = 1.5f,
                TradeTier = ItemTradeTier.Medical,
            });
            Add(new WorldItemDef
            {
                Id = "water_purification_tablets_40_of_40",
                DisplayName = "Water Purification Tablets 40/40",
                Description = "Full bottle. 2 tablets per use.",
                Type = ItemType.Medical,
                StackMax = 1,
                Weight = 0.12f,
                TradeValue = 28.0f,
                TradeTier = ItemTradeTier.Medical,
                Capacity = 40f,
                Fill = 40f,
                DrainPerUse = 2f,
                CapacityUnit = "tablets",
            });
            Add(new WorldItemDef
            {
                Id = "water_purification_tablets_20_of_40",
                DisplayName = "Water Purification Tablets 20/40",
                Description = "Half-full bottle. 2 tablets per use.",
                Type = ItemType.Medical,
                StackMax = 1,
                Weight = 0.08f,
                TradeValue = 15.0f,
                TradeTier = ItemTradeTier.Medical,
                Capacity = 40f,
                Fill = 20f,
                DrainPerUse = 2f,
                CapacityUnit = "tablets",
            });
            Add(new WorldItemDef
            {
                Id = "water_purification_tablets_0_of_40",
                DisplayName = "Water Purification Tablets 0/40 (Empty)",
                Description = "Empty bottle. Refill elsewhere.",
                Type = ItemType.Material,
                StackMax = 5,
                Weight = 0.04f,
                TradeValue = 1.0f,
                TradeTier = ItemTradeTier.Scrap,
                Capacity = 40f,
                Fill = 0f,
                DrainPerUse = 2f,
                CapacityUnit = "tablets",
            });
            Add(new WorldItemDef
            {
                Id = "flare_red",
                DisplayName = "Flare (Red)",
                Description = "Red signal flare. Night mark.",
                Type = ItemType.Tool,
                StackMax = 10,
                Weight = 0.2f,
                TradeValue = 6.0f,
                TradeTier = ItemTradeTier.UtilityTool,
            });
            Add(new WorldItemDef
            {
                Id = "flare_green",
                DisplayName = "Flare (Green)",
                Description = "Green signal flare.",
                Type = ItemType.Tool,
                StackMax = 10,
                Weight = 0.2f,
                TradeValue = 6.0f,
                TradeTier = ItemTradeTier.UtilityTool,
            });
            Add(new WorldItemDef
            {
                Id = "flare_yellow",
                DisplayName = "Flare (Yellow)",
                Description = "Yellow signal flare.",
                Type = ItemType.Tool,
                StackMax = 10,
                Weight = 0.2f,
                TradeValue = 6.0f,
                TradeTier = ItemTradeTier.UtilityTool,
            });
            Add(new WorldItemDef
            {
                Id = "smoke_grenade",
                DisplayName = "Smoke Grenade",
                Description = "Screening smoke. Cover and signal.",
                Type = ItemType.Weapon,
                StackMax = 6,
                Weight = 0.35f,
                TradeValue = 55.0f,
                TradeTier = ItemTradeTier.Weapon,
            });
            Add(new WorldItemDef
            {
                Id = "flashbang",
                DisplayName = "Flashbang",
                Description = "Stun charge. Self-defense.",
                Type = ItemType.Weapon,
                StackMax = 6,
                Weight = 0.3f,
                TradeValue = 70.0f,
                TradeTier = ItemTradeTier.Weapon,
            });
            Add(new WorldItemDef
            {
                Id = "workbench_basic",
                DisplayName = "Workbench (Basic)",
                Description = "Entry craft station.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 10.0f,
                TradeValue = 28.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "workbench_intermediate",
                DisplayName = "Workbench (Intermediate)",
                Description = "Expanded craft recipes.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 12.0f,
                TradeValue = 45.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "workbench_advanced",
                DisplayName = "Workbench (Advanced)",
                Description = "Complex assemblies.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 14.0f,
                TradeValue = 70.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "workbench_professional",
                DisplayName = "Workbench (Professional)",
                Description = "Top-tier civilian craft.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 16.0f,
                TradeValue = 110.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "workbench_upgrade_kit",
                DisplayName = "Workbench Upgrade Kit",
                Description = "Upgrades a workbench one tier.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 12.0f,
                TradeValue = 40.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "research_table",
                DisplayName = "Research Table",
                Description = "Blueprints and analysis.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 12.0f,
                TradeValue = 48.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "basic_cooking_stove",
                DisplayName = "Basic Cooking Stove",
                Description = "Cooks rations. Needs fuel/matches.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 10.0f,
                TradeValue = 30.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "improvised_cooking_stove",
                DisplayName = "Improvised Cooking Stove",
                Description = "Scrap stove. Smoky.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 8.0f,
                TradeValue = 18.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "advanced_cooking_stove",
                DisplayName = "Advanced Cooking Stove",
                Description = "Efficient field kitchen.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 14.0f,
                TradeValue = 65.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "basic_heater",
                DisplayName = "Basic Heater",
                Description = "Bunker warmth. Fuel hungry.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 10.0f,
                TradeValue = 32.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "improvised_heater",
                DisplayName = "Improvised Heater",
                Description = "Scrap heater. Risk of smoke.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 8.0f,
                TradeValue = 16.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "advanced_heater",
                DisplayName = "Advanced Heater",
                Description = "Controlled bunker heat.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 14.0f,
                TradeValue = 68.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "heater_lamp",
                DisplayName = "Heater Lamp",
                Description = "Small radiant lamp.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 10.0f,
                TradeValue = 22.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "distiller",
                DisplayName = "Distiller",
                Description = "Water/alcohol distillation.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 12.0f,
                TradeValue = 50.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "alcohol_distiller",
                DisplayName = "Alcohol Distiller",
                Description = "Spirits still. Fuel and yeast.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 12.0f,
                TradeValue = 52.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "filter_item",
                DisplayName = "Filter Item",
                Description = "Generic filter cartridge frame.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 10.0f,
                TradeValue = 12.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "basic_water_boiler",
                DisplayName = "Basic Water Boiler",
                Description = "Boils water clean.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 10.0f,
                TradeValue = 26.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "improvised_water_boiler",
                DisplayName = "Improvised Water Boiler",
                Description = "Tin boiler. Slow.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 8.0f,
                TradeValue = 14.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "advanced_water_boiler",
                DisplayName = "Advanced Water Boiler",
                Description = "Fast safe boil.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 14.0f,
                TradeValue = 55.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "basic_herb_garden",
                DisplayName = "Basic Herb Garden",
                Description = "Indoor herbs.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 10.0f,
                TradeValue = 24.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "improvised_herb_garden",
                DisplayName = "Improvised Herb Garden",
                Description = "Tins and dirt.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 8.0f,
                TradeValue = 12.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "advanced_herb_garden",
                DisplayName = "Advanced Herb Garden",
                Description = "Lush indoor herbs.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 14.0f,
                TradeValue = 50.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "herbal_farm_max_tier",
                DisplayName = "Herbal Farm (Max Tier)",
                Description = "Full bunker herb farm.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 16.0f,
                TradeValue = 95.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "small_animal_trap",
                DisplayName = "Small Animal Trap",
                Description = "Catches small game.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 10.0f,
                TradeValue = 15.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "medium_animal_trap",
                DisplayName = "Medium Animal Trap",
                Description = "Larger game trap.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 12.0f,
                TradeValue = 28.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "basic_recycler",
                DisplayName = "Basic Recycler",
                Description = "Breaks scrap to parts.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 10.0f,
                TradeValue = 30.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "improvised_recycler",
                DisplayName = "Improvised Recycler",
                Description = "Crude scrap mill.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 8.0f,
                TradeValue = 16.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "advanced_recycle_bench",
                DisplayName = "Advanced Recycle Bench",
                Description = "High-yield recycling.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 14.0f,
                TradeValue = 72.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "simple_tool_workshop",
                DisplayName = "Simple Tool Workshop",
                Description = "Basic tool craft.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 10.0f,
                TradeValue = 26.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "basic_tool_workshop",
                DisplayName = "Basic Tool Workshop",
                Description = "Standard tool craft.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 10.0f,
                TradeValue = 32.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "improvised_tool_workshop",
                DisplayName = "Improvised Tool Workshop",
                Description = "Scrap tool bench.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 8.0f,
                TradeValue = 15.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "advanced_tool_workshop",
                DisplayName = "Advanced Tool Workshop",
                Description = "Precision tools.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 14.0f,
                TradeValue = 68.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "basic_gunbench",
                DisplayName = "Basic Gunbench",
                Description = "Simple firearm maintenance.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 12.0f,
                TradeValue = 55.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "improvised_gunbench",
                DisplayName = "Improvised Gunbench",
                Description = "Crude weapon bench.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 10.0f,
                TradeValue = 35.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "tactical_weapons_bench",
                DisplayName = "Tactical Weapons Bench",
                Description = "Military weapon work.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 14.0f,
                TradeValue = 90.0f,
                TradeTier = ItemTradeTier.Station,
                MilitaryGrade = true,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "advanced_tactical_weapon_bench",
                DisplayName = "Advanced Tactical Weapon Bench",
                Description = "Spec-ops grade weapon bench.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 16.0f,
                TradeValue = 125.0f,
                TradeTier = ItemTradeTier.Station,
                MilitaryGrade = true,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "basic_refinement_bench",
                DisplayName = "Basic Refinement Bench",
                Description = "Ore and part refine.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 10.0f,
                TradeValue = 34.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "improvised_refinement_bench",
                DisplayName = "Improvised Refinement Bench",
                Description = "Scrap refine.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 8.0f,
                TradeValue = 17.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "tactical_refinement_bench",
                DisplayName = "Tactical Refinement Bench",
                Description = "Mil-spec refine.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 14.0f,
                TradeValue = 80.0f,
                TradeTier = ItemTradeTier.Station,
                MilitaryGrade = true,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "advanced_tactical_refinement_workshop",
                DisplayName = "Advanced Tactical Refinement Workshop",
                Description = "Top refine workshop.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 16.0f,
                TradeValue = 120.0f,
                TradeTier = ItemTradeTier.Station,
                MilitaryGrade = true,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "basic_tobacco_leaf",
                DisplayName = "Basic Tobacco Leaf",
                Description = "Dried leaf. Harsh smoke.",
                Type = ItemType.Material,
                StackMax = 30,
                Weight = 0.05f,
                TradeValue = 1.5f,
                TradeTier = ItemTradeTier.Ingredient,
            });
            Add(new WorldItemDef
            {
                Id = "quality_tobacco_leaf",
                DisplayName = "Quality Tobacco Leaf",
                Description = "Cured leaf. Smoother.",
                Type = ItemType.Material,
                StackMax = 20,
                Weight = 0.05f,
                TradeValue = 3.0f,
                TradeTier = ItemTradeTier.Ingredient,
            });
            Add(new WorldItemDef
            {
                Id = "basic_rollup_cigarette",
                DisplayName = "Basic Rollup Cigarette",
                Description = "Hand-rolled smoke.",
                Type = ItemType.Comfort,
                StackMax = 40,
                Weight = 0.01f,
                TradeValue = 1.2f,
                TradeTier = ItemTradeTier.Consumable,
                MoraleEffect = 1f,
            });
            Add(new WorldItemDef
            {
                Id = "quality_rollup_cigarette",
                DisplayName = "Quality Rollup Cigarette",
                Description = "Better roll.",
                Type = ItemType.Comfort,
                StackMax = 30,
                Weight = 0.01f,
                TradeValue = 2.2f,
                TradeTier = ItemTradeTier.Consumable,
                MoraleEffect = 2f,
            });
            Add(new WorldItemDef
            {
                Id = "herbal_cigarette",
                DisplayName = "Herbal Cigarette",
                Description = "Garden herbs rolled.",
                Type = ItemType.Comfort,
                StackMax = 30,
                Weight = 0.01f,
                TradeValue = 1.5f,
                TradeTier = ItemTradeTier.Consumable,
                MoraleEffect = 1f,
            });
            Add(new WorldItemDef
            {
                Id = "herbs",
                DisplayName = "Herbs",
                Description = "Mixed garden herbs.",
                Type = ItemType.Material,
                StackMax = 40,
                Weight = 0.05f,
                TradeValue = 1.8f,
                TradeTier = ItemTradeTier.Ingredient,
            });
            Add(new WorldItemDef
            {
                Id = "menthol_leaf",
                DisplayName = "Menthol Leaf",
                Description = "Cool leaf.",
                Type = ItemType.Material,
                StackMax = 25,
                Weight = 0.04f,
                TradeValue = 2.0f,
                TradeTier = ItemTradeTier.Ingredient,
            });
            Add(new WorldItemDef
            {
                Id = "menthol_cigarette",
                DisplayName = "Menthol Cigarette",
                Description = "Menthol smoke.",
                Type = ItemType.Comfort,
                StackMax = 30,
                Weight = 0.01f,
                TradeValue = 2.0f,
                TradeTier = ItemTradeTier.Consumable,
                MoraleEffect = 2f,
            });
            Add(new WorldItemDef
            {
                Id = "disposable_vape",
                DisplayName = "Disposable Vape",
                Description = "Sealed vape stick.",
                Type = ItemType.Comfort,
                StackMax = 10,
                Weight = 0.08f,
                TradeValue = 5.0f,
                TradeTier = ItemTradeTier.Consumable,
                MoraleEffect = 2f,
            });
            Add(new WorldItemDef
            {
                Id = "ejuice_10ml_10mg",
                DisplayName = "E-Juice 10ML 10mg Nicotine",
                Description = "Low-nic e-liquid.",
                Type = ItemType.Material,
                StackMax = 15,
                Weight = 0.05f,
                TradeValue = 4.0f,
                TradeTier = ItemTradeTier.Ingredient,
            });
            Add(new WorldItemDef
            {
                Id = "ejuice_10ml_20mg",
                DisplayName = "E-Juice 10ML 20mg Nicotine",
                Description = "Mid-nic e-liquid.",
                Type = ItemType.Material,
                StackMax = 12,
                Weight = 0.05f,
                TradeValue = 5.0f,
                TradeTier = ItemTradeTier.Ingredient,
            });
            Add(new WorldItemDef
            {
                Id = "ejuice_20ml_35mg",
                DisplayName = "E-Juice 20ML 35mg Nicotine",
                Description = "High-nic e-liquid.",
                Type = ItemType.Material,
                StackMax = 8,
                Weight = 0.1f,
                TradeValue = 8.0f,
                TradeTier = ItemTradeTier.Ingredient,
            });
            Add(new WorldItemDef
            {
                Id = "nicotine_pouch",
                DisplayName = "Nicotine Pouch",
                Description = "Oral pouch.",
                Type = ItemType.Comfort,
                StackMax = 40,
                Weight = 0.01f,
                TradeValue = 1.8f,
                TradeTier = ItemTradeTier.Consumable,
                MoraleEffect = 1f,
            });
            Add(new WorldItemDef
            {
                Id = "quality_tobacco_nicotine_pouch",
                DisplayName = "Quality Tobacco Nicotine Pouch",
                Description = "Premium pouch.",
                Type = ItemType.Comfort,
                StackMax = 30,
                Weight = 0.01f,
                TradeValue = 3.0f,
                TradeTier = ItemTradeTier.Consumable,
                MoraleEffect = 2f,
            });
            Add(new WorldItemDef
            {
                Id = "coffee_arabica_bean",
                DisplayName = "Coffee Arabica Bean",
                Description = "Better bean.",
                Type = ItemType.Material,
                StackMax = 25,
                Weight = 0.05f,
                TradeValue = 2.5f,
                TradeTier = ItemTradeTier.Ingredient,
            });
            Add(new WorldItemDef
            {
                Id = "coffee_robusta_bean",
                DisplayName = "Coffee Robusta Bean",
                Description = "Harsh strong bean.",
                Type = ItemType.Material,
                StackMax = 25,
                Weight = 0.05f,
                TradeValue = 1.8f,
                TradeTier = ItemTradeTier.Ingredient,
            });
            Add(new WorldItemDef
            {
                Id = "instant_coffee",
                DisplayName = "Instant Coffee",
                Description = "One serve powder.",
                Type = ItemType.Food,
                StackMax = 30,
                Weight = 0.02f,
                TradeValue = 2.0f,
                TradeTier = ItemTradeTier.Consumable,
                HungerRestore = 2f,
                ThirstRestore = 4f,
                MoraleEffect = 2f,
            });
            Add(new WorldItemDef
            {
                Id = "instant_coffee_10x_container",
                DisplayName = "Instant Coffee 10x Container",
                Description = "Ten serves.",
                Type = ItemType.Food,
                StackMax = 5,
                Weight = 0.2f,
                TradeValue = 14.0f,
                TradeTier = ItemTradeTier.Consumable,
                HungerRestore = 2f,
                ThirstRestore = 4f,
                MoraleEffect = 2f,
                Capacity = 10f,
                Fill = 10f,
                DrainPerUse = 1f,
                CapacityUnit = "serves",
            });
            Add(new WorldItemDef
            {
                Id = "coffee_creamer",
                DisplayName = "Coffee Creamer",
                Description = "Powdered creamer.",
                Type = ItemType.Material,
                StackMax = 20,
                Weight = 0.1f,
                TradeValue = 1.5f,
                TradeTier = ItemTradeTier.Ingredient,
            });
            Add(new WorldItemDef
            {
                Id = "box_of_tea_20",
                DisplayName = "Box of Tea (20x)",
                Description = "Twenty bags.",
                Type = ItemType.Food,
                StackMax = 5,
                Weight = 0.15f,
                TradeValue = 8.0f,
                TradeTier = ItemTradeTier.Consumable,
                ThirstRestore = 3f,
                MoraleEffect = 1f,
                Capacity = 20f,
                Fill = 20f,
                DrainPerUse = 1f,
                CapacityUnit = "bags",
            });
            Add(new WorldItemDef
            {
                Id = "ice_tea_0_5l_package",
                DisplayName = "Ice Tea 0.5L Package",
                Description = "Sweet cold tea.",
                Type = ItemType.Food,
                StackMax = 10,
                Weight = 0.55f,
                TradeValue = 3.5f,
                TradeTier = ItemTradeTier.Consumable,
                HungerRestore = 2f,
                ThirstRestore = 10f,
            });
            Add(new WorldItemDef
            {
                Id = "herbal_tea",
                DisplayName = "Herbal Tea",
                Description = "Brewed from garden herbs.",
                Type = ItemType.Food,
                StackMax = 15,
                Weight = 0.2f,
                TradeValue = 3.0f,
                TradeTier = ItemTradeTier.Consumable,
                ThirstRestore = 8f,
                MoraleEffect = 2f,
            });
            Add(new WorldItemDef
            {
                Id = "package_rolled_oats_1kg_of_1kg",
                DisplayName = "Package of Rolled Oats 1KG/1KG",
                Description = "Drains 0.1KG/use → 2 basic breakfast bowls (needs 1× water).",
                Type = ItemType.Food,
                StackMax = 4,
                Weight = 1.1f,
                TradeValue = 7.0f,
                TradeTier = ItemTradeTier.Consumable,
                HungerRestore = 6f,
                Capacity = 1f,
                Fill = 1f,
                DrainPerUse = 0.1f,
                CapacityUnit = "kg",
            });
            Add(new WorldItemDef
            {
                Id = "dry_rice_1kg_of_1kg",
                DisplayName = "Dry Rice 1KG/1KG",
                Description = "Drains 0.1KG/use; needs 1× water.",
                Type = ItemType.Food,
                StackMax = 4,
                Weight = 1.1f,
                TradeValue = 6.5f,
                TradeTier = ItemTradeTier.Consumable,
                HungerRestore = 8f,
                Capacity = 1f,
                Fill = 1f,
                DrainPerUse = 0.1f,
                CapacityUnit = "kg",
            });
            Add(new WorldItemDef
            {
                Id = "dried_pasta_2kg_of_2kg",
                DisplayName = "Dried Pasta 2KG/2KG",
                Description = "Drains 0.2KG/use. Alone: 1 quality dinner. +carrot+potato+2 water: 4 quality dinners.",
                Type = ItemType.Food,
                StackMax = 3,
                Weight = 2.1f,
                TradeValue = 9.0f,
                TradeTier = ItemTradeTier.Consumable,
                HungerRestore = 10f,
                Capacity = 2f,
                Fill = 2f,
                DrainPerUse = 0.2f,
                CapacityUnit = "kg",
            });
            Add(new WorldItemDef
            {
                Id = "soy_and_rice_milk_1l_of_1l",
                DisplayName = "Soy and Rice Milk 1L/1L",
                Description = "Drains 0.25L/use → high-quality oat breakfast ×2.",
                Type = ItemType.Food,
                StackMax = 6,
                Weight = 1.05f,
                TradeValue = 6.0f,
                TradeTier = ItemTradeTier.Consumable,
                HungerRestore = 4f,
                ThirstRestore = 8f,
                Capacity = 1f,
                Fill = 1f,
                DrainPerUse = 0.25f,
                CapacityUnit = "L",
            });
            Add(new WorldItemDef
            {
                Id = "emergency_civilian_ration_box_5",
                DisplayName = "Emergency Civilian Ration Box (5x)",
                Description = "Sealed five-pack.",
                Type = ItemType.Food,
                StackMax = 4,
                Weight = 2.0f,
                TradeValue = 18.0f,
                TradeTier = ItemTradeTier.Consumable,
                HungerRestore = 20f,
            });
            Add(new WorldItemDef
            {
                Id = "emergency_civilian_ration_1",
                DisplayName = "Emergency Civilian Ration (1x)",
                Description = "Single emergency meal.",
                Type = ItemType.Food,
                StackMax = 15,
                Weight = 0.4f,
                TradeValue = 4.0f,
                TradeTier = ItemTradeTier.Consumable,
                HungerRestore = 18f,
            });
            Add(new WorldItemDef
            {
                Id = "canned_fish",
                DisplayName = "Canned Fish",
                Description = "Oily protein tin.",
                Type = ItemType.Food,
                StackMax = 12,
                Weight = 0.4f,
                TradeValue = 5.0f,
                TradeTier = ItemTradeTier.Consumable,
                HungerRestore = 20f,
            });
            Add(new WorldItemDef
            {
                Id = "canned_beans",
                DisplayName = "Canned Beans",
                Description = "Beans in brine.",
                Type = ItemType.Food,
                StackMax = 15,
                Weight = 0.4f,
                TradeValue = 3.5f,
                TradeTier = ItemTradeTier.Consumable,
                HungerRestore = 16f,
            });
            Add(new WorldItemDef
            {
                Id = "jam_preserves",
                DisplayName = "Jam Preserves",
                Description = "Sweet jar. Morale sugar.",
                Type = ItemType.Food,
                StackMax = 10,
                Weight = 0.4f,
                TradeValue = 4.0f,
                TradeTier = ItemTradeTier.Consumable,
                HungerRestore = 8f,
                MoraleEffect = 3f,
            });
            Add(new WorldItemDef
            {
                Id = "basic_breakfast_bowl",
                DisplayName = "Basic Breakfast Bowl",
                Description = "Cooked oats bowl.",
                Type = ItemType.Food,
                StackMax = 10,
                Weight = 0.35f,
                TradeValue = 3.5f,
                TradeTier = ItemTradeTier.Consumable,
                HungerRestore = 14f,
                ThirstRestore = 2f,
            });
            Add(new WorldItemDef
            {
                Id = "quality_dinner_bowl",
                DisplayName = "Quality Dinner Bowl",
                Description = "Cooked pasta dinner.",
                Type = ItemType.Food,
                StackMax = 8,
                Weight = 0.5f,
                TradeValue = 6.0f,
                TradeTier = ItemTradeTier.Consumable,
                HungerRestore = 22f,
                MoraleEffect = 3f,
            });
            Add(new WorldItemDef
            {
                Id = "high_quality_oat_breakfast",
                DisplayName = "High Quality Oat Breakfast",
                Description = "Oats with milk.",
                Type = ItemType.Food,
                StackMax = 8,
                Weight = 0.4f,
                TradeValue = 5.0f,
                TradeTier = ItemTradeTier.Consumable,
                HungerRestore = 18f,
                MoraleEffect = 2f,
            });
            Add(new WorldItemDef
            {
                Id = "ceramic_water_filter",
                DisplayName = "Ceramic Water Filter",
                Description = "Reusable ceramic filter.",
                Type = ItemType.Filter,
                StackMax = 5,
                Weight = 0.6f,
                TradeValue = 22.0f,
                TradeTier = ItemTradeTier.SurvivalGear,
                Durability = 80f,
            });
            Add(new WorldItemDef
            {
                Id = "can_opener",
                DisplayName = "Can Opener Tool",
                Description = "Opens tins cleanly.",
                Type = ItemType.Tool,
                StackMax = 5,
                Weight = 0.15f,
                TradeValue = 8.0f,
                TradeTier = ItemTradeTier.UtilityTool,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "can_breaker",
                DisplayName = "Can Breaker",
                Description = "Brutal tin opener. 1 use tool-ish.",
                Type = ItemType.Tool,
                StackMax = 8,
                Weight = 0.3f,
                TradeValue = 3.0f,
                TradeTier = ItemTradeTier.UtilityTool,
                Durability = 20f,
            });
            Add(new WorldItemDef
            {
                Id = "insulated_flask",
                DisplayName = "Insulated Flask",
                Description = "Keeps liquid hot/cold.",
                Type = ItemType.Tool,
                StackMax = 4,
                Weight = 0.5f,
                TradeValue = 10.0f,
                TradeTier = ItemTradeTier.UtilityTool,
            });
            Add(new WorldItemDef
            {
                Id = "herbal_pills",
                DisplayName = "Herbal Pills",
                Description = "Mild herbal dose.",
                Type = ItemType.Medical,
                StackMax = 20,
                Weight = 0.02f,
                TradeValue = 4.0f,
                TradeTier = ItemTradeTier.Medical,
                HealthEffect = 5f,
            });
            Add(new WorldItemDef
            {
                Id = "herbal_bandage",
                DisplayName = "Herbal Bandage",
                Description = "Herb-treated wrap.",
                Type = ItemType.Medical,
                StackMax = 20,
                Weight = 0.05f,
                TradeValue = 3.5f,
                TradeTier = ItemTradeTier.Medical,
                HealthEffect = 6f,
            });
            Add(new WorldItemDef
            {
                Id = "bandage_roll",
                DisplayName = "Bandage Roll",
                Description = "Clean cloth roll.",
                Type = ItemType.Medical,
                StackMax = 25,
                Weight = 0.08f,
                TradeValue = 4.0f,
                TradeTier = ItemTradeTier.Medical,
                HealthEffect = 8f,
            });
            Add(new WorldItemDef
            {
                Id = "medkit",
                DisplayName = "Medkit",
                Description = "Field trauma kit.",
                Type = ItemType.Medical,
                StackMax = 4,
                Weight = 1.2f,
                TradeValue = 35.0f,
                TradeTier = ItemTradeTier.Medical,
                HealthEffect = 25f,
            });
            Add(new WorldItemDef
            {
                Id = "adhesive_bandages_box_6",
                DisplayName = "Adhesive Bandages Box (6x)",
                Description = "Takes 2 per use.",
                Type = ItemType.Medical,
                StackMax = 10,
                Weight = 0.05f,
                TradeValue = 5.0f,
                TradeTier = ItemTradeTier.Medical,
                HealthEffect = 4f,
                Capacity = 6f,
                Fill = 6f,
                DrainPerUse = 2f,
                CapacityUnit = "strips",
            });
            Add(new WorldItemDef
            {
                Id = "antiseptic_1l_of_1l",
                DisplayName = "Antiseptic 1L/1L",
                Description = "Drains 0.1L/use or sterilises 2 bandages → 2 sterilised.",
                Type = ItemType.Medical,
                StackMax = 4,
                Weight = 1.1f,
                TradeValue = 14.0f,
                TradeTier = ItemTradeTier.Medical,
                Capacity = 1f,
                Fill = 1f,
                DrainPerUse = 0.1f,
                CapacityUnit = "L",
            });
            Add(new WorldItemDef
            {
                Id = "sterilised_bandage",
                DisplayName = "Sterilised Bandage",
                Description = "Antiseptic-treated bandage.",
                Type = ItemType.Medical,
                StackMax = 20,
                Weight = 0.06f,
                TradeValue = 5.5f,
                TradeTier = ItemTradeTier.Medical,
                HealthEffect = 10f,
            });
            Add(new WorldItemDef
            {
                Id = "opioid_painkillers",
                DisplayName = "Opioid Painkillers",
                Description = "Strong pain relief. 1×.",
                Type = ItemType.Medical,
                StackMax = 15,
                Weight = 0.02f,
                TradeValue = 28.0f,
                TradeTier = ItemTradeTier.Medical,
                MoraleEffect = 2f,
                HealthEffect = 15f,
            });
            Add(new WorldItemDef
            {
                Id = "alcohol_wipes_box_10_of_10",
                DisplayName = "Alcohol Wipes Box 10/10",
                Description = "1 wipe per use.",
                Type = ItemType.Medical,
                StackMax = 8,
                Weight = 0.1f,
                TradeValue = 7.0f,
                TradeTier = ItemTradeTier.Medical,
                Capacity = 10f,
                Fill = 10f,
                DrainPerUse = 1f,
                CapacityUnit = "wipes",
            });
            Add(new WorldItemDef
            {
                Id = "antibiotics_bottle_20",
                DisplayName = "Antibiotics Bottle (20x)",
                Description = "Uses 2 per treatment.",
                Type = ItemType.Medical,
                StackMax = 5,
                Weight = 0.08f,
                TradeValue = 40.0f,
                TradeTier = ItemTradeTier.Medical,
                HealthEffect = 20f,
                Capacity = 20f,
                Fill = 20f,
                DrainPerUse = 2f,
                CapacityUnit = "pills",
            });
            Add(new WorldItemDef
            {
                Id = "splint",
                DisplayName = "Splint",
                Description = "Immobilises a limb.",
                Type = ItemType.Medical,
                StackMax = 8,
                Weight = 0.4f,
                TradeValue = 9.0f,
                TradeTier = ItemTradeTier.Medical,
            });
            Add(new WorldItemDef
            {
                Id = "epi_pen",
                DisplayName = "Epi-Pen",
                Description = "Emergency adrenaline.",
                Type = ItemType.Medical,
                StackMax = 4,
                Weight = 0.1f,
                TradeValue = 45.0f,
                TradeTier = ItemTradeTier.Medical,
                ExtremelyRare = true,
                HealthEffect = 30f,
            });
            Add(new WorldItemDef
            {
                Id = "thermometer",
                DisplayName = "Thermometer",
                Description = "Checks temperature; craft component.",
                Type = ItemType.Tool,
                StackMax = 8,
                Weight = 0.05f,
                TradeValue = 6.0f,
                TradeTier = ItemTradeTier.UtilityTool,
            });
            Add(new WorldItemDef
            {
                Id = "medical_scissors",
                DisplayName = "Medical Scissors",
                Description = "Trauma shears. Crafting.",
                Type = ItemType.Tool,
                StackMax = 8,
                Weight = 0.1f,
                TradeValue = 7.0f,
                TradeTier = ItemTradeTier.UtilityTool,
                Durability = 80f,
            });
            Add(new WorldItemDef
            {
                Id = "iodine_pills_bottle_10_of_10",
                DisplayName = "Iodine Pills Bottle 10/10",
                Description = "Thyroid block. 1 per use.",
                Type = ItemType.Iodine,
                StackMax = 5,
                Weight = 0.05f,
                TradeValue = 18.0f,
                TradeTier = ItemTradeTier.Medical,
                Capacity = 10f,
                Fill = 10f,
                DrainPerUse = 1f,
                CapacityUnit = "pills",
            });
            Add(new WorldItemDef
            {
                Id = "personal_dosimeter",
                DisplayName = "Personal Dosimeter",
                Description = "Logs cumulative dose.",
                Type = ItemType.Device,
                StackMax = 2,
                Weight = 0.3f,
                TradeValue = 30.0f,
                TradeTier = ItemTradeTier.SurvivalGear,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "geiger_counter",
                DisplayName = "Geiger Counter",
                Description = "Live rate meter.",
                Type = ItemType.Device,
                StackMax = 2,
                Weight = 0.8f,
                TradeValue = 42.0f,
                TradeTier = ItemTradeTier.SurvivalGear,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "respirator",
                DisplayName = "Respirator",
                Description = "Face seal. Needs filters.",
                Type = ItemType.Protective,
                StackMax = 3,
                Weight = 0.6f,
                TradeValue = 28.0f,
                TradeTier = ItemTradeTier.Protective,
                IsEquipable = true,
                EquipSlot = "Face",
                Durability = 80f,
                RadProtection = 1f,
            });
            Add(new WorldItemDef
            {
                Id = "respirator_filter_box_5",
                DisplayName = "Respirator Filter Box (5x)",
                Description = "Five filters.",
                Type = ItemType.Filter,
                StackMax = 6,
                Weight = 0.4f,
                TradeValue = 20.0f,
                TradeTier = ItemTradeTier.SurvivalGear,
                Capacity = 5f,
                Fill = 5f,
                DrainPerUse = 1f,
                CapacityUnit = "filters",
            });
            Add(new WorldItemDef
            {
                Id = "respirator_filter",
                DisplayName = "Respirator Filter (1x)",
                Description = "Single filter cartridge.",
                Type = ItemType.Filter,
                StackMax = 20,
                Weight = 0.08f,
                TradeValue = 5.0f,
                TradeTier = ItemTradeTier.SurvivalGear,
            });
            Add(new WorldItemDef
            {
                Id = "protective_goggles",
                DisplayName = "Protective Goggles",
                Description = "Eye seal.",
                Type = ItemType.Protective,
                StackMax = 5,
                Weight = 0.15f,
                TradeValue = 8.0f,
                TradeTier = ItemTradeTier.Protective,
                IsEquipable = true,
                EquipSlot = "Face",
                Durability = 60f,
            });
            Add(new WorldItemDef
            {
                Id = "protective_rubber_gloves",
                DisplayName = "Protective Rubber Gloves",
                Description = "Chem gloves.",
                Type = ItemType.Protective,
                StackMax = 8,
                Weight = 0.1f,
                TradeValue = 6.0f,
                TradeTier = ItemTradeTier.Protective,
                IsEquipable = true,
                EquipSlot = "Hands",
                Durability = 40f,
            });
            Add(new WorldItemDef
            {
                Id = "decontamination_soap_5_of_5",
                DisplayName = "Decontamination Soap 5/5",
                Description = "5 washes.",
                Type = ItemType.Medical,
                StackMax = 8,
                Weight = 0.2f,
                TradeValue = 12.0f,
                TradeTier = ItemTradeTier.Medical,
                Capacity = 5f,
                Fill = 5f,
                DrainPerUse = 1f,
                CapacityUnit = "uses",
            });
            Add(new WorldItemDef
            {
                Id = "plastic_contamination_bag_box_5",
                DisplayName = "Plastic Contamination Bag Box (5x)",
                Description = "Five hazmat bags.",
                Type = ItemType.Material,
                StackMax = 10,
                Weight = 0.3f,
                TradeValue = 6.0f,
                TradeTier = ItemTradeTier.BulkMaterial,
                Capacity = 5f,
                Fill = 5f,
                DrainPerUse = 1f,
                CapacityUnit = "bags",
            });
            Add(new WorldItemDef
            {
                Id = "military_grade_shovel",
                DisplayName = "Military Grade Shovel",
                Description = "Entrenching tool.",
                Type = ItemType.Tool,
                StackMax = 2,
                Weight = 1.5f,
                TradeValue = 22.0f,
                TradeTier = ItemTradeTier.UtilityTool,
                MilitaryGrade = true,
                Durability = 140f,
            });
            Add(new WorldItemDef
            {
                Id = "military_grade_hatchet",
                DisplayName = "Military Grade Hatchet",
                Description = "Compact axe.",
                Type = ItemType.Weapon,
                StackMax = 2,
                Weight = 1.2f,
                TradeValue = 48.0f,
                TradeTier = ItemTradeTier.Weapon,
                MilitaryGrade = true,
                Durability = 120f,
            });
            Add(new WorldItemDef
            {
                Id = "firefighter_grade_fireaxe",
                DisplayName = "Firefighter Grade Fireaxe",
                Description = "Heavy breach axe.",
                Type = ItemType.Weapon,
                StackMax = 1,
                Weight = 2.8f,
                TradeValue = 55.0f,
                TradeTier = ItemTradeTier.Weapon,
                Durability = 140f,
            });
            Add(new WorldItemDef
            {
                Id = "pliers",
                DisplayName = "Pliers",
                Description = "Crafting grip tool.",
                Type = ItemType.Tool,
                StackMax = 10,
                Weight = 0.25f,
                TradeValue = 6.0f,
                TradeTier = ItemTradeTier.UtilityTool,
                Durability = 90f,
            });
            Add(new WorldItemDef
            {
                Id = "sewing_kit_10_of_10",
                DisplayName = "Sewing Kit 10/10",
                Description = "10 stitches/repairs.",
                Type = ItemType.Tool,
                StackMax = 5,
                Weight = 0.15f,
                TradeValue = 8.0f,
                TradeTier = ItemTradeTier.UtilityTool,
                Durability = 100f,
                Capacity = 10f,
                Fill = 10f,
                DrainPerUse = 1f,
                CapacityUnit = "uses",
            });
            Add(new WorldItemDef
            {
                Id = "flashlight",
                DisplayName = "Flashlight",
                Description = "Battery lamp.",
                Type = ItemType.Tool,
                StackMax = 5,
                Weight = 0.3f,
                TradeValue = 9.0f,
                TradeTier = ItemTradeTier.UtilityTool,
                Durability = 80f,
            });
            Add(new WorldItemDef
            {
                Id = "military_grade_flashlight",
                DisplayName = "Military Grade Flashlight",
                Description = "Hardened lamp.",
                Type = ItemType.Tool,
                StackMax = 3,
                Weight = 0.4f,
                TradeValue = 24.0f,
                TradeTier = ItemTradeTier.UtilityTool,
                MilitaryGrade = true,
                Durability = 120f,
            });
            Add(new WorldItemDef
            {
                Id = "matches",
                DisplayName = "Matches",
                Description = "Stove and heater light.",
                Type = ItemType.Material,
                StackMax = 40,
                Weight = 0.02f,
                TradeValue = 1.5f,
                TradeTier = ItemTradeTier.Ingredient,
            });
            Add(new WorldItemDef
            {
                Id = "cigarette_lighter",
                DisplayName = "Cigarette Lighter",
                Description = "Reusable spark.",
                Type = ItemType.Tool,
                StackMax = 15,
                Weight = 0.05f,
                TradeValue = 3.5f,
                TradeTier = ItemTradeTier.UtilityTool,
            });
            Add(new WorldItemDef
            {
                Id = "car_battery",
                DisplayName = "Car Battery",
                Description = "Heavy 12V cell.",
                Type = ItemType.Device,
                StackMax = 2,
                Weight = 12.0f,
                TradeValue = 25.0f,
                TradeTier = ItemTradeTier.SurvivalGear,
            });
            Add(new WorldItemDef
            {
                Id = "rechargeable_battery",
                DisplayName = "Rechargeable Battery",
                Description = "Pack cell.",
                Type = ItemType.Material,
                StackMax = 15,
                Weight = 0.1f,
                TradeValue = 4.0f,
                TradeTier = ItemTradeTier.Ingredient,
            });
            Add(new WorldItemDef
            {
                Id = "aa_batteries_package_10",
                DisplayName = "AA Batteries Package (10x)",
                Description = "Home upgrades craft.",
                Type = ItemType.Material,
                StackMax = 10,
                Weight = 0.2f,
                TradeValue = 6.0f,
                TradeTier = ItemTradeTier.Ingredient,
                Capacity = 10f,
                Fill = 10f,
                DrainPerUse = 1f,
                CapacityUnit = "cells",
            });
            Add(new WorldItemDef
            {
                Id = "hand_crank_radio",
                DisplayName = "Hand-Crank Radio",
                Description = "News without grid.",
                Type = ItemType.Device,
                StackMax = 2,
                Weight = 0.8f,
                TradeValue = 20.0f,
                TradeTier = ItemTradeTier.SurvivalGear,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "small_solar_panel",
                DisplayName = "Small Solar Panel",
                Description = "Trickle power.",
                Type = ItemType.Device,
                StackMax = 2,
                Weight = 2.5f,
                TradeValue = 35.0f,
                TradeTier = ItemTradeTier.SurvivalGear,
            });
            Add(new WorldItemDef
            {
                Id = "medium_solar_panel",
                DisplayName = "Medium Solar Panel",
                Description = "Serious solar.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 6.0f,
                TradeValue = 55.0f,
                TradeTier = ItemTradeTier.SurvivalGear,
            });
            Add(new WorldItemDef
            {
                Id = "generator",
                DisplayName = "Generator",
                Description = "Fuel generator.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 25.0f,
                TradeValue = 80.0f,
                TradeTier = ItemTradeTier.Station,
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "kerosene_lantern",
                DisplayName = "Kerosene Lantern",
                Description = "Uses fuel or jetfuel.",
                Type = ItemType.Tool,
                StackMax = 3,
                Weight = 0.7f,
                TradeValue = 12.0f,
                TradeTier = ItemTradeTier.UtilityTool,
                Durability = 90f,
            });
            Add(new WorldItemDef
            {
                Id = "jetfuel_jerrycan_10l_of_10l",
                DisplayName = "Jetfuel Jerrycan 10L/10L",
                Description = "Refills kerosene lantern.",
                Type = ItemType.Fuel,
                StackMax = 2,
                Weight = 9.0f,
                TradeValue = 30.0f,
                TradeTier = ItemTradeTier.Ingredient,
                Capacity = 10f,
                Fill = 10f,
                DrainPerUse = 0.5f,
                CapacityUnit = "L",
            });
            Add(new WorldItemDef
            {
                Id = "winter_coat",
                DisplayName = "Winter Coat",
                Description = "Heavy coat. Warmth.",
                Type = ItemType.Protective,
                StackMax = 2,
                Weight = 2.5f,
                TradeValue = 18.0f,
                TradeTier = ItemTradeTier.Protective,
                IsEquipable = true,
                EquipSlot = "Torso",
                Durability = 80f,
            });
            Add(new WorldItemDef
            {
                Id = "work_boots",
                DisplayName = "Work Boots",
                Description = "Hard soles.",
                Type = ItemType.Protective,
                StackMax = 2,
                Weight = 1.5f,
                TradeValue = 12.0f,
                TradeTier = ItemTradeTier.Protective,
                IsEquipable = true,
                EquipSlot = "None",
                Durability = 100f,
            });
            Add(new WorldItemDef
            {
                Id = "wool_blanket",
                DisplayName = "Wool Blanket",
                Description = "Sleep warmth.",
                Type = ItemType.Comfort,
                StackMax = 4,
                Weight = 1.2f,
                TradeValue = 8.0f,
                TradeTier = ItemTradeTier.Consumable,
                MoraleEffect = 2f,
            });
            Add(new WorldItemDef
            {
                Id = "improvised_rollup_bed",
                DisplayName = "Improvised Rollup Bed",
                Description = "Scrap bedroll.",
                Type = ItemType.Comfort,
                StackMax = 2,
                Weight = 2.0f,
                TradeValue = 5.0f,
                TradeTier = ItemTradeTier.Consumable,
            });
            Add(new WorldItemDef
            {
                Id = "woolbed",
                DisplayName = "Wool Bed",
                Description = "Proper wool bed.",
                Type = ItemType.Comfort,
                StackMax = 1,
                Weight = 8.0f,
                TradeValue = 22.0f,
                TradeTier = ItemTradeTier.SurvivalGear,
            });
            Add(new WorldItemDef
            {
                Id = "advanced_heating_bed",
                DisplayName = "Advanced Heating Bed",
                Description = "Heated bunk.",
                Type = ItemType.Device,
                StackMax = 1,
                Weight = 12.0f,
                TradeValue = 60.0f,
                TradeTier = ItemTradeTier.Station,
            });
            Add(new WorldItemDef
            {
                Id = "wool_gloves",
                DisplayName = "Wool Gloves",
                Description = "Warm hands.",
                Type = ItemType.Protective,
                StackMax = 5,
                Weight = 0.15f,
                TradeValue = 5.0f,
                TradeTier = ItemTradeTier.Protective,
                IsEquipable = true,
                EquipSlot = "Hands",
                Durability = 50f,
            });
            Add(new WorldItemDef
            {
                Id = "family_photograph",
                DisplayName = "Family Photograph",
                Description = "Paper memory. Morale.",
                Type = ItemType.Comfort,
                StackMax = 5,
                Weight = 0.02f,
                TradeValue = 2.0f,
                TradeTier = ItemTradeTier.Consumable,
                MoraleEffect = 5f,
            });
            Add(new WorldItemDef
            {
                Id = "cassette_tape",
                DisplayName = "Cassette Tape",
                Description = "Recorded voice. Rare comfort.",
                Type = ItemType.Comfort,
                StackMax = 8,
                Weight = 0.05f,
                TradeValue = 4.0f,
                TradeTier = ItemTradeTier.Consumable,
                MoraleEffect = 3f,
            });
            Add(new WorldItemDef
            {
                Id = "sealed_government_document",
                DisplayName = "Sealed Government Document",
                Description = "Quest item. Do not open lightly.",
                Type = ItemType.Quest,
                StackMax = 1,
                Weight = 0.05f,
                TradeValue = 0.0f,
                TradeTier = ItemTradeTier.Quest,
            });
            Add(new WorldItemDef
            {
                Id = "diamond",
                DisplayName = "Diamond",
                Description = "Hard stone. Below guns in barter.",
                Type = ItemType.Trade,
                StackMax = 5,
                Weight = 0.01f,
                TradeValue = 45.0f,
                TradeTier = ItemTradeTier.Precious,
            });
            Add(new WorldItemDef
            {
                Id = "ruby",
                DisplayName = "Ruby",
                Description = "Red gem.",
                Type = ItemType.Trade,
                StackMax = 5,
                Weight = 0.01f,
                TradeValue = 38.0f,
                TradeTier = ItemTradeTier.Precious,
            });
            Add(new WorldItemDef
            {
                Id = "sapphire",
                DisplayName = "Sapphire",
                Description = "Blue gem.",
                Type = ItemType.Trade,
                StackMax = 5,
                Weight = 0.01f,
                TradeValue = 36.0f,
                TradeTier = ItemTradeTier.Precious,
            });
            Add(new WorldItemDef
            {
                Id = "amber",
                DisplayName = "Amber",
                Description = "Fossil resin.",
                Type = ItemType.Trade,
                StackMax = 8,
                Weight = 0.02f,
                TradeValue = 18.0f,
                TradeTier = ItemTradeTier.Precious,
            });
            Add(new WorldItemDef
            {
                Id = "pistol_cz75_9x19",
                DisplayName = "CZ-75 9×19mm Pistol",
                Description = "Service pistol. Chambers 9×19.",
                Type = ItemType.Weapon,
                StackMax = 1,
                Weight = 1.1f,
                TradeValue = 105.0f,
                TradeTier = ItemTradeTier.Weapon,
                MilitaryGrade = true,
                Durability = 120f,
            });
            Add(new WorldItemDef
            {
                Id = "pistol_beretta_92_9x19",
                DisplayName = "Beretta 92 9×19mm Pistol",
                Description = "Full-size 9×19 service pistol.",
                Type = ItemType.Weapon,
                StackMax = 1,
                Weight = 1.15f,
                TradeValue = 108.0f,
                TradeTier = ItemTradeTier.Weapon,
                MilitaryGrade = true,
                Durability = 120f,
            });
            Add(new WorldItemDef
            {
                Id = "pistol_steyr_m9_9x19",
                DisplayName = "Steyr M9 9×19mm Pistol",
                Description = "Polymer 9×19 sidearm.",
                Type = ItemType.Weapon,
                StackMax = 1,
                Weight = 0.95f,
                TradeValue = 102.0f,
                TradeTier = ItemTradeTier.Weapon,
                MilitaryGrade = true,
                Durability = 120f,
            });
        }
    }
}
