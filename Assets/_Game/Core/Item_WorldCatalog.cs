using System;
using System.Collections.Generic;
using AtomicWar._Game.Inventory;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Master snake_case item-id registry for world loot beyond the ammo catalog:
    /// military attachments (extremely rare, fielded on mil/rebel/black-ops/spec-ops/
    /// merc/bandit/insurgent gear), scrap components, deprecated scrap-only bullets,
    /// armour, food, fuel, tools, and water bottles.
    /// Definitions are data-only; StreamingAssets/Data/items.json mirrors these ids
    /// for the JSON → ScriptableObject importer.
    /// </summary>
    public static partial class Item_WorldCatalog
    {
        /// <summary>Lightweight definition used by tests and future loot tables.</summary>
        public sealed class WorldItemDef
        {
            public string Id;
            public string DisplayName;
            public string Description;
            public ItemType Type;
            public int StackMax;
            public float Weight;
            public float TradeValue;
            /// <summary>Logical barter tier (weapons top, scrap bottom).</summary>
            public ItemTradeTier TradeTier;
            public bool IsEquipable;
            public string EquipSlot;
            public float Durability;
            public float RadProtection;
            public float HungerRestore;
            public float ThirstRestore;
            public float HealthEffect;
            public float MoraleEffect;
            public bool ScrapOnly;
            public bool MilitaryGrade;
            public bool ExtremelyRare;
            public string[] ScrapMaterials;
            public int[] ScrapAmounts;
            /// <summary>Max capacity units (tablets, kg, L, m, uses). 0 = n/a.</summary>
            public float Capacity;
            /// <summary>Current fill of capacity.</summary>
            public float Fill;
            /// <summary>Units drained per use.</summary>
            public float DrainPerUse;
            /// <summary>Unit label for capacity (kg, L, m, uses, tablets).</summary>
            public string CapacityUnit;
        }

        private static readonly Dictionary<string, WorldItemDef> Catalog =
            new Dictionary<string, WorldItemDef>(StringComparer.Ordinal);
        private static bool _built;

        // ── Military attachments (item ids — extremely rare field loot) ────
        public const string AttMilSuppressor = "att_mil_suppressor";
        public const string AttMilLaserdot = "att_mil_laserdot";
        public const string AttMilTacticalGrip = "att_mil_tactical_grip";
        public const string AttMilLongRangeScope = "att_mil_long_range_scope";
        public const string AttMilHolosight = "att_mil_holosight";
        public const string AttMilDoubleScope5x10x = "att_mil_double_scope_5x_10x";

        // ── Scrap / components ────────────────────────────────────────────
        public const string ShellCasing = "shell_casing";
        public const string BulletCasing = "bullet_casing";
        public const string Gunpowder = "gunpowder";
        public const string Sulphur = "sulphur";
        public const string ExplosivePowderNitroglycerin = "explosive_powder_nitroglycerin";
        public const string ScrapMetal = "scrap_metal";
        public const string Fertilizer = "fertilizer";
        public const string Cloth = "cloth";
        public const string SalvagedTechTrash = "salvaged_tech_trash";

        // ── Armour ────────────────────────────────────────────────────────
        public const string BodyArmourDeprecated = "body_armour_deprecated";
        public const string BodyArmourMilitary = "body_armour_military";
        public const string HelmetMilitary = "helmet_military";
        public const string NvGogglesMilitary = "nv_goggles_military";
        public const string HelmetHeavyMilitary = "helmet_heavy_military";
        public const string ArmourHeavyMilitary = "armour_heavy_military";
        public const string HelmetHeavyDeprecated = "helmet_heavy_deprecated";
        public const string HelmetDeprecated = "helmet_deprecated";
        public const string BodyArmourHeavyDeprecated = "body_armour_heavy_deprecated";

        // ── Food ──────────────────────────────────────────────────────────
        public const string CannedFood = "canned_food";
        public const string VegetableCarrot = "vegetable_carrot";
        public const string VegetablePotato = "vegetable_potato";
        public const string VegetableBeetroot = "vegetable_beetroot";
        public const string CannedMeat = "canned_meat";
        public const string PreservedCrackers = "preserved_crackers";
        public const string MreMilitary = "mre_military";
        public const string BoiledVegetableSoup = "boiled_vegetable_soup";
        public const string HeartyMealCooked = "hearty_meal_cooked";

        // ── Fuel / accelerant ─────────────────────────────────────────────
        public const string Fuel1L = "fuel_1l";
        public const string FuelHalfOf1L = "fuel_0_5l_of_1l";
        public const string AccelerantFull = "accelerant_full";
        public const string AccelerantHalf = "accelerant_half";

        // ── Tools / weapons / broken ──────────────────────────────────────
        public const string KnifeImprovised = "knife_improvised";
        public const string KnifeSwissBattle = "knife_swiss_battle";
        public const string BayonetSwissMachete = "bayonet_swiss_machete";
        public const string Hammer = "hammer";
        public const string Screwdriver = "screwdriver";
        public const string Multitool = "multitool";
        public const string Shovel = "shovel";
        public const string GrenadeMilitary = "grenade_military";
        public const string Crowbar = "crowbar";
        public const string WireCutters = "wire_cutters";
        public const string Lockpick = "lockpick";
        public const string MetalPipe = "metal_pipe";
        public const string CrowbarBroken = "crowbar_broken";
        public const string WireCuttersBroken = "wire_cutters_broken";
        public const string MetalPipeBroken = "metal_pipe_broken";
        public const string ShovelBroken = "shovel_broken";
        public const string MultitoolBroken = "multitool_broken";
        public const string KnifeBroken = "knife_broken";
        public const string HammerBroken = "hammer_broken";
        public const string ScrewdriverBroken = "screwdriver_broken";

        // ── Water bottles (fill / capacity) ───────────────────────────────
        public const string WaterBottle1LFull = "water_bottle_1l_full";
        public const string WaterBottle2LFull = "water_bottle_2l_full";
        public const string WaterBottle1LOf2L = "water_bottle_1l_of_2l";
        public const string WaterBottleHalfOf1L = "water_bottle_0_5l_of_1l";
        public const string WaterBottleHalfOf2L = "water_bottle_0_5l_of_2l";
        public const string WaterBottle1_5LOf2L = "water_bottle_1_5l_of_2l";
        public const string WaterBottleEmpty = "water_bottle_empty";

        public static IReadOnlyDictionary<string, WorldItemDef> All
        {
            get
            {
                Ensure();
                return Catalog;
            }
        }

        public static bool TryGet(string id, out WorldItemDef def)
        {
            Ensure();
            return Catalog.TryGetValue(id ?? string.Empty, out def);
        }

        public static bool Contains(string id)
        {
            Ensure();
            return !string.IsNullOrEmpty(id) && Catalog.ContainsKey(id);
        }

        /// <summary>
        /// Catalog-aware trade value (code-only). Prefer this over raw TradeValue when
        /// applying demand / shortage. UI must use <see cref="Item_TradeValues.FormatWorthLabel"/>.
        /// </summary>
        public static float ResolveTradeValue(
            string itemId,
            float demandMultiplier = 1f,
            bool suppliesShort = false)
        {
            if (string.IsNullOrEmpty(itemId)) return 0f;
            if (!TryGet(itemId, out var def) || def == null)
                return Item_TradeValues.Resolve(0f, ItemTradeTier.BulkMaterial, demandMultiplier, suppliesShort);
            float bas = def.TradeValue > 0f ? def.TradeValue : Item_TradeValues.BaseForTier(def.TradeTier);
            return Item_TradeValues.Resolve(bas, def.TradeTier, demandMultiplier, suppliesShort);
        }

        /// <summary>Player-facing worth label for a catalog id (never digits).</summary>
        public static string FormatWorthLabel(
            string itemId,
            float demandMultiplier = 1f,
            bool suppliesShort = false)
        {
            return Item_TradeValues.FormatWorthLabel(
                ResolveTradeValue(itemId, demandMultiplier, suppliesShort));
        }

        public static IReadOnlyList<string> AllIds()
        {
            Ensure();
            return new List<string>(Catalog.Keys);
        }

        public static IReadOnlyList<string> MilitaryAttachmentIds()
        {
            return new[]
            {
                AttMilSuppressor,
                AttMilLaserdot,
                AttMilTacticalGrip,
                AttMilLongRangeScope,
                AttMilHolosight,
                AttMilDoubleScope5x10x
            };
        }

        /// <summary>
        /// Deprecated scrap-only bullet id for a caliber (e.g. cal_9x19 → ammo_deprecated_cal_9x19).
        /// </summary>
        public static string DeprecatedBulletId(string caliberId)
        {
            if (string.IsNullOrEmpty(caliberId)) return "ammo_deprecated_unknown";
            return "ammo_deprecated_" + caliberId;
        }

        private static void Ensure()
        {
            if (_built) return;
            lock (Catalog)
            {
                if (_built) return;
                Build();
                BuildExpanded();
                _built = true;
            }
        }

        private static void Build()
        {
            Catalog.Clear();

            // Military attachments — obtainable but extremely rare; already fitted on
            // military, rebel, black-ops, spec-ops, merc, bandit, insurgent, terrorist gear.
            Add(Att(AttMilSuppressor, "Military Grade Suppressor",
                "Threaded suppressor. Cuts report; fielded on mil/rebel/specialist long guns."));
            Add(Att(AttMilLaserdot, "Military Grade Laserdot Scope",
                "IR-capable laser aiming module. Rare detachment from specialist weapons."));
            Add(Att(AttMilTacticalGrip, "Military Grade Tactical Grip",
                "Angled/vertical grip. Recoil control for battle rifles and SMGs."));
            Add(Att(AttMilLongRangeScope, "Military Grade Long Range Scope",
                "High-magnification optic. Sniper and designated-marksman issue."));
            Add(Att(AttMilHolosight, "Military Grade Holosight",
                "Holographic close-quarters optic. Spec-ops / black-ops common fit."));
            Add(Att(AttMilDoubleScope5x10x, "Military Grade Double Scope 5×/10×",
                "Flip dual-magnification scope. Extremely rare; marksman issue only."));

            // Components / scrap
            Add(Mat(ShellCasing, "Shell Casing", "Empty shotgun/rifle hull. Scrap or reload feedstock.", 0.01f, 50, Item_TradeValues.BaseScrap, ItemTradeTier.Scrap));
            Add(Mat(BulletCasing, "Bullet Casing", "Empty brass. Scrap or reload feedstock.", 0.005f, 80, Item_TradeValues.BaseScrap, ItemTradeTier.Scrap));
            Add(Mat(Gunpowder, "Gunpowder", "Propellant grains. Reloading and improvised charges.", 0.02f, 40, 3.5f, ItemTradeTier.Ingredient));
            Add(Mat(Sulphur, "Sulphur", "Yellow powder. Chemistry and powder mixes.", 0.05f, 30, 2f, ItemTradeTier.Ingredient));
            Add(Mat(ExplosivePowderNitroglycerin, "Explosive Powder (Nitroglycerin)",
                "Unstable high explosive. Handle cold. Military demo caches.", 0.08f, 10, 22f,
                ItemTradeTier.Weapon, mil: true, rare: true));
            Add(Mat(ScrapMetal, "Scrap Metal", "Twisted plate and fittings. Universal repair feedstock.", 0.5f, 50, Item_TradeValues.BaseBulkMaterial, ItemTradeTier.BulkMaterial));
            Add(Mat(Fertilizer, "Fertilizer", "Compost / chemical fertilizer. Grow beds and bad decisions.", 1f, 20, 1.5f, ItemTradeTier.BulkMaterial));
            Add(Mat(Cloth, "Cloth", "Rags and fabric. Bandages, filters, padding.", 0.1f, 40, Item_TradeValues.BaseBulkMaterial, ItemTradeTier.BulkMaterial));
            Add(Mat(SalvagedTechTrash, "Salvaged Tech-Trash",
                "Broken boards and connectors. Scrape for electronic scrap.", 0.3f, 25, 1.8f, ItemTradeTier.BulkMaterial));

            // Deprecated bullets (any caliber) — scrap only, no combat use
            string[] calibers =
            {
                "cal_9x19", "cal_380acp", "cal_762x25", "cal_45acp", "cal_9x21", "cal_765x21",
                "cal_12ga", "cal_16ga", "cal_556x45", "cal_762x39", "cal_545x39", "cal_762x51",
                "cal_300blk", "cal_57x28", "cal_46x30", "cal_762x54r", "cal_338lapua",
                "cal_408cheytac", "cal_50bmg"
            };
            for (int i = 0; i < calibers.Length; i++)
            {
                string cal = calibers[i];
                string id = DeprecatedBulletId(cal);
                Add(new WorldItemDef
                {
                    Id = id,
                    DisplayName = "Deprecated Bullets (" + cal.Replace("cal_", "") + ")",
                    Description = "Corroded or recalled " + cal + " rounds. Scrap only — not safe to fire.",
                    Type = ItemType.Material,
                    StackMax = 60,
                    Weight = 0.015f,
                    TradeValue = Item_TradeValues.BaseScrap,
                    TradeTier = ItemTradeTier.Scrap,
                    ScrapOnly = true,
                    ScrapMaterials = new[] { ShellCasing, BulletCasing, Gunpowder },
                    ScrapAmounts = new[] { 1, 1, 1 }
                });
            }

            // Armour
            Add(Armour(BodyArmourDeprecated, "Deprecated Body Armour",
                "Cracked plates and rotten straps. Scrap or last-resort padding.",
                rad: 0f, dur: 20f, trade: 5f, mil: false));
            Add(Armour(BodyArmourMilitary, "Military Grade Body Armour",
                "Plate carrier with ceramic inserts. Fielded on mil/rebel line troops.",
                rad: 2f, dur: 120f, trade: 48f, mil: true));
            Add(Armour(HelmetMilitary, "Military Grade Helmet",
                "Ballistic helmet. Standard issue on military and mercenary forces.",
                rad: 1f, dur: 80f, trade: 28f, mil: true, slot: "Head"));
            Add(Armour(NvGogglesMilitary, "Military Grade NV Goggles",
                "Night-vision goggles. Spec-ops / black-ops / specialist mercs.",
                rad: 0f, dur: 60f, trade: 62f, mil: true, rare: true, slot: "Head"));
            Add(Armour(HelmetHeavyMilitary, "Heavy Military Grade Helmet",
                "Heavy ballistic helmet with face shield mounts.",
                rad: 2f, dur: 140f, trade: 42f, mil: true, slot: "Head"));
            Add(Armour(ArmourHeavyMilitary, "Heavy Military Grade Armour",
                "Full heavy plate. Slow, loud, survives hatch breaches.",
                rad: 4f, dur: 180f, trade: 75f, mil: true, rare: true));
            Add(Armour(HelmetHeavyDeprecated, "Deprecated Heavy Helmet",
                "Rusted heavy helmet. Scrap metal with a memory of rank.",
                rad: 0f, dur: 15f, trade: 3.5f, mil: false, slot: "Head"));
            Add(Armour(HelmetDeprecated, "Deprecated Helmet",
                "Split shell. Better as scrap than protection.",
                rad: 0f, dur: 10f, trade: 2.5f, mil: false, slot: "Head"));
            Add(Armour(BodyArmourHeavyDeprecated, "Deprecated Heavy Body Armour",
                "Warped heavy plates. Scrap only if you value your spine.",
                rad: 0f, dur: 25f, trade: 5.5f, mil: false));

            // Food (canned_food may already exist in JSON — same id is intentional)
            Add(Food(CannedFood, "Canned Food", "Sealed tin. Cold calories.", 18f, 0f, 0.4f, 12, 4f));
            Add(Food(VegetableCarrot, "Vegetable — Carrot", "Root crop. Thin but clean.", 8f, 2f, 0.15f, 20, 1.5f));
            Add(Food(VegetablePotato, "Vegetable — Potato", "Starchy root. Boil or roast.", 10f, 1f, 0.25f, 20, 1.5f));
            Add(Food(VegetableBeetroot, "Vegetable — Beetroot", "Earth-sweet root. Stains everything.", 9f, 2f, 0.2f, 20, 1.5f));
            Add(Food(CannedMeat, "Canned Meat", "Dense protein tin. Salt and fat.", 22f, 0f, 0.45f, 10, 5f));
            Add(Food(PreservedCrackers, "Preserved Crackers", "Dry rations. Last forever, taste like dust.", 12f, 0f, 0.2f, 25, 2.5f));
            Add(Food(MreMilitary, "Military Grade MRE",
                "Sealed field ration. High calories, low joy. Mil/rebel issue.",
                30f, 8f, 0.6f, 8, 12f, mil: true));
            Add(Food(BoiledVegetableSoup, "Boiled Vegetable Soup", "Hot thin soup. Morale bump.", 14f, 12f, 0.5f, 6, 4.5f, morale: 2f));
            Add(Food(HeartyMealCooked, "Hearty Meal (Cooked)", "Real food. Rare luxury.", 28f, 6f, 0.7f, 4, 8f, morale: 6f));

            // Fuel
            Add(Fuel(Fuel1L, "Fuel 1L", "One litre of usable fuel.", 1f, 5f));
            Add(Fuel(FuelHalfOf1L, "Fuel 0.5L / 1L", "Half-full one-litre can.", 0.55f, 2.5f));
            Add(Fuel(AccelerantFull, "Accelerant (Full)", "Full accelerant tin. Fire-starting and bad ideas.", 1.2f, 6.5f));
            Add(Fuel(AccelerantHalf, "Accelerant (Half-Full)", "Half tin of accelerant.", 0.7f, 3.5f));

            // Tools / self-defense
            Add(Tool(KnifeImprovised, "Improvised Knife", "Scrap blade and tape. Cuts, barely.", 0.4f, 6f, 40f, tier: ItemTradeTier.Weapon));
            Add(Tool(KnifeSwissBattle, "Swiss Battle Knife", "Folding multi-blade. Reliable edge.", 0.35f, 28f, 100f, mil: true, tier: ItemTradeTier.Weapon));
            Add(Tool(BayonetSwissMachete, "Swiss Bayonet Machete", "Long blade / bayonet hybrid. Clearing and close work.", 1.1f, 42f, 120f, mil: true, tier: ItemTradeTier.Weapon));
            Add(Tool(Hammer, "Hammer", "Claw hammer. Nails, skulls, and scrap.", 0.8f, 7f, 80f));
            Add(Tool(Screwdriver, "Screwdriver", "Flat/Phillips salvage tool.", 0.2f, 5f, 60f));
            Add(Tool(Multitool, "MultiTool", "Pliers, blades, drivers in one frame.", 0.3f, 12f, 90f));
            Add(Tool(Shovel, "Shovel", "Dig, bury, barricade.", 2f, 8f, 100f));
            Add(Tool(GrenadeMilitary, "Military Grade Hand Grenade",
                "Fragmentation grenade. Extremely rare loose loot — usually already on a belt.",
                0.4f, 95f, 1f, mil: true, rare: true, stack: 4, tier: ItemTradeTier.Weapon));
            Add(Tool(Crowbar, "Crowbar", "Pry, break, defend.", 2.5f, 14f, 120f, tier: ItemTradeTier.Weapon));
            Add(Tool(WireCutters, "Wire Cutters", "Snip fences and leads.", 0.4f, 7f, 70f));
            Add(Tool(Lockpick, "Lockpick", "Slim steel. Quiet doors.", 0.05f, 16f, 30f));
            Add(Tool(MetalPipe, "Metal Pipe", "Blunt tool and plumbing scrap.", 1.5f, 4f, 50f));

            // Broken tools — scrap only
            Add(Broken(CrowbarBroken, "Broken Crowbar", Crowbar));
            Add(Broken(WireCuttersBroken, "Broken Wire Cutters", WireCutters));
            Add(Broken(MetalPipeBroken, "Broken Metal Pipe", MetalPipe));
            Add(Broken(ShovelBroken, "Broken Shovel", Shovel));
            Add(Broken(MultitoolBroken, "Broken Multitool", Multitool));
            Add(Broken(KnifeBroken, "Broken Knife", KnifeImprovised));
            Add(Broken(HammerBroken, "Broken Hammer", Hammer));
            Add(Broken(ScrewdriverBroken, "Broken Screwdriver", Screwdriver));

            // Water bottles
            Add(Water(WaterBottle1LFull, "Water Bottle 1L (Full)", 1f, 1f, 1.1f, 5f));
            Add(Water(WaterBottle2LFull, "Water Bottle 2L (Full)", 2f, 2f, 2.2f, 8f));
            Add(Water(WaterBottle1LOf2L, "Water Bottle 1L / 2L", 1f, 2f, 1.6f, 5f));
            Add(Water(WaterBottleHalfOf1L, "Water Bottle 0.5L / 1L", 0.5f, 1f, 0.7f, 3f));
            Add(Water(WaterBottleHalfOf2L, "Water Bottle 0.5L / 2L", 0.5f, 2f, 1.1f, 3f));
            Add(Water(WaterBottle1_5LOf2L, "Water Bottle 1.5L / 2L", 1.5f, 2f, 1.9f, 6.5f));
            Add(new WorldItemDef
            {
                Id = WaterBottleEmpty,
                DisplayName = "Empty Water Bottle",
                Description = "Empty bottle. Refill at clean sources or boil first.",
                Type = ItemType.Material,
                StackMax = 10,
                Weight = 0.1f,
                TradeValue = Item_TradeValues.BaseBulkMaterial,
                TradeTier = ItemTradeTier.BulkMaterial
            });
        }

        private static WorldItemDef Att(string id, string name, string desc) => new WorldItemDef
        {
            Id = id,
            DisplayName = name,
            Description = desc + " Extremely rare loose; usually already mounted on faction weapons.",
            Type = ItemType.Tool,
            StackMax = 1,
            Weight = 0.4f,
            TradeValue = Item_TradeValues.BaseAttachment,
            TradeTier = ItemTradeTier.Attachment,
            Durability = 100f,
            IsEquipable = false,
            MilitaryGrade = true,
            ExtremelyRare = true,
            ScrapMaterials = new[] { "mechanical_parts", "electronic_scrap" },
            ScrapAmounts = new[] { 2, 1 }
        };

        private static WorldItemDef Mat(
            string id, string name, string desc, float weight, int stack, float trade,
            ItemTradeTier tier = ItemTradeTier.BulkMaterial,
            bool mil = false, bool rare = false) => new WorldItemDef
        {
            Id = id,
            DisplayName = name,
            Description = desc,
            Type = ItemType.Material,
            StackMax = stack,
            Weight = weight,
            TradeValue = trade,
            TradeTier = tier,
            MilitaryGrade = mil,
            ExtremelyRare = rare
        };

        private static WorldItemDef Armour(
            string id, string name, string desc, float rad, float dur, float trade,
            bool mil, bool rare = false, string slot = "Body") => new WorldItemDef
        {
            Id = id,
            DisplayName = name,
            Description = desc,
            Type = ItemType.Protective,
            StackMax = 1,
            Weight = mil ? 4f : 2.5f,
            TradeValue = trade,
            TradeTier = mil ? ItemTradeTier.Protective : ItemTradeTier.UtilityTool,
            IsEquipable = true,
            EquipSlot = slot,
            Durability = dur,
            RadProtection = rad,
            MilitaryGrade = mil,
            ExtremelyRare = rare,
            ScrapMaterials = new[] { ScrapMetal, "mechanical_parts" },
            ScrapAmounts = new[] { 2, 1 }
        };

        private static WorldItemDef Food(
            string id, string name, string desc, float hunger, float thirst, float weight,
            int stack, float trade, float morale = 0f, bool mil = false) => new WorldItemDef
        {
            Id = id,
            DisplayName = name,
            Description = desc,
            Type = ItemType.Food,
            StackMax = stack,
            Weight = weight,
            TradeValue = trade,
            TradeTier = ItemTradeTier.Consumable,
            HungerRestore = hunger,
            ThirstRestore = thirst,
            MoraleEffect = morale,
            MilitaryGrade = mil
        };

        private static WorldItemDef Fuel(string id, string name, string desc, float weight, float trade) => new WorldItemDef
        {
            Id = id,
            DisplayName = name,
            Description = desc,
            Type = ItemType.Fuel,
            StackMax = 10,
            Weight = weight,
            TradeValue = trade,
            TradeTier = ItemTradeTier.Ingredient
        };

        private static WorldItemDef Tool(
            string id, string name, string desc, float weight, float trade, float dur,
            bool mil = false, bool rare = false, int stack = 1,
            ItemTradeTier tier = ItemTradeTier.UtilityTool) => new WorldItemDef
        {
            Id = id,
            DisplayName = name,
            Description = desc,
            Type = tier == ItemTradeTier.Weapon ? ItemType.Weapon : ItemType.Tool,
            StackMax = stack,
            Weight = weight,
            TradeValue = trade,
            TradeTier = tier,
            Durability = dur,
            MilitaryGrade = mil,
            ExtremelyRare = rare,
            ScrapMaterials = new[] { ScrapMetal, "mechanical_parts" },
            ScrapAmounts = new[] { 1, 1 }
        };

        private static WorldItemDef Broken(string id, string name, string ofId) => new WorldItemDef
        {
            Id = id,
            DisplayName = name,
            Description = "Broken " + ofId.Replace('_', ' ') + ". Scrap only.",
            Type = ItemType.Material,
            StackMax = 5,
            Weight = 0.5f,
            TradeValue = Item_TradeValues.BaseScrap,
            TradeTier = ItemTradeTier.Scrap,
            ScrapOnly = true,
            ScrapMaterials = new[] { ScrapMetal },
            ScrapAmounts = new[] { 1 }
        };

        private static WorldItemDef Water(
            string id, string name, float fillL, float capL, float weight, float trade) => new WorldItemDef
        {
            Id = id,
            DisplayName = name,
            Description = $"Bottle {fillL:0.#}L of {capL:0.#}L capacity. Clean if sealed.",
            Type = ItemType.Water,
            StackMax = 4,
            Weight = weight,
            TradeValue = trade,
            TradeTier = ItemTradeTier.Consumable,
            ThirstRestore = fillL * 20f,
            Capacity = capL,
            Fill = fillL,
            CapacityUnit = "L"
        };

        internal static void Add(WorldItemDef def)
        {
            if (def == null || string.IsNullOrEmpty(def.Id)) return;
            if (def.TradeTier == ItemTradeTier.Scrap && def.TradeValue <= 0f)
                def.TradeValue = Item_TradeValues.BaseScrap;
            Catalog[def.Id] = def;
        }
    }
}
