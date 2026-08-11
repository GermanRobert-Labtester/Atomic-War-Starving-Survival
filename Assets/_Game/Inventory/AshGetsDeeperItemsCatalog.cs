using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Inventory
{
    /// <summary>
    /// "The Ash Gets Deeper" content batch (Prompts #326–#330) — 80 new
    /// items authored as a single data-driven catalog builder. Mirrors the
    /// <see cref="Crafting.NewRecipesCatalog"/> pattern: pure C# Spec rows
    /// that materialise through <c>MaterialiseAll</c> with a host-supplied
    /// id → <see cref="ItemDefinition"/> lookup, so the data can later be
    /// JSON-imported without code changes.
    ///
    /// Item ids are stable snake_case and match the brief exactly. Where
    /// the brief leaves the description blank (e.g. <c>ec...</c> columns),
    /// we keep a short fixture so the catalog stays self-explanatory.
    /// </summary>
    public static class AshGetsDeeperItemsCatalog
    {
        public const string IdPrefix = "ashdeep_";

        // ── Canonical id registry (Section I) ─────────────────────────────
        public static class Ids
        {
            // Medical & pathology
            public const string MorphineAmpoule       = "morphine_ampoule";
            public const string SurgicalThread        = "surgical_thread";
            public const string Tourniquet            = "tourniquet";
            public const string BoneSplintKit         = "bone_splint_kit";
            public const string BloodPressureCuff     = "blood_pressure_cuff";
            public const string PotassiumPermanganate = "potassium_permanganate";
            public const string SalineDripBag         = "saline_drip_bag";
            public const string TetanusAntitoxin      = "tetanus_shot";
            public const string BurnGauze             = "burn_gauze";
            public const string DefibrillatorPads     = "defibrillator_pads";
            public const string ColostomyBag          = "colostomy_bag";
            public const string DialysisFilter        = "dialysis_filter";
            public const string PrussianBlueCapsules  = "prussian_blue_capsules";
            public const string WoundPackingGauze     = "wound_packing_gauze";

            // Survival & shelter
            public const string AshTarp               = "ash_tarp";
            public const string LeadSheet             = "lead_sheet";
            public const string ConcretePatchMix      = "concrete_patch_mix";
            public const string InsulationWrap        = "insulation_wrap";
            public const string CandleTallow          = "candle_tallow";
            public const string HandWarmerPacket      = "hand_warmer_packet";
            public const string RatTrapSpring         = "rat_trap_spring";
            public const string GlueTrap              = "mousetrap_glue";
            public const string WaterCatchmentSheet   = "water_catchment_sheet";
            public const string RopeLadder5m          = "rope_ladder_5m";
            public const string CharcoalBriquette     = "charcoal_briquette";
            public const string PeatBlock             = "peat_block";

            // Weapons & combat (grounded, no fantasy)
            public const string PipeShotgunSingle     = "pipe_shotgun_single";
            public const string CrossbowSalvaged      = "crossbow_salvaged";
            public const string BoltSteelPack10       = "bolt_steel_pack_10";
            public const string NailBat               = "nail_bat";
            public const string SlingshotSteel        = "slingshot_steel";
            public const string ImprovisedIncendiary  = "improvised_molotov";
            public const string HatchetRusted         = "hatchet_rusted";
            public const string BearTrapSalvaged      = "bear_trap_salvaged";

            // Comfort, morale & narrative
            public const string HarmonicaDented       = "harmonica_dented";
            public const string ChildShoePair         = "child_shoe_pair";
            public const string SeedEnvelopeTomato    = "seed_envelope_tomato";
            public const string SeedEnvelopeWheat     = "seed_envelope_wheat";
            public const string PlayingCardsWorn      = "playing_cards_worn";
            public const string ChessSetCarved        = "chess_set_carved";
            public const string AshPressFlower        = "ash_press_flower";
            public const string PocketWatchStopped    = "pocket_watch_stopped";
            public const string CigarettePackSealed   = "cigarette_pack_sealed";
            public const string VialOfSoil            = "vial_of_soil";

            // Scavenged tech & components
            public const string GeigerTubeSalvaged    = "geiger_tube_salvaged";
            public const string CapacitorBankSmall    = "capacitor_bank_small";
            public const string CopperTubing1m        = "copper_tubing_1m";
            public const string RubberGasketSet       = "rubber_gasket_set";
            public const string WeldingRodPack20      = "welding_rod_pack_20";
            public const string CircuitBreaker30a     = "circuit_breaker_30a";
            public const string BearingSetIndustrial  = "bearing_set_industrial";

            // Food & consumables (expanded)
            public const string RatMeatSkewer         = "rat_meat_skewer";
            public const string InsectPasteBrick      = "insect_paste_brick";
            public const string AshBreadFlat          = "ash_bread_flat";
            public const string CannedMilkCondensed   = "canned_milk_condensed";
            public const string VinegarBottle         = "vinegar_bottle";
            public const string DriedMushroomString   = "dried_mushroom_string";
            public const string SaltPorkStrip         = "salt_pork_strip";
            public const string WildGarlicBulb        = "wild_garlic_bulb";

            // Clothing & protective gear (degraded tier)
            public const string PatchedRaincoat       = "patched_raincoat";
            public const string LayeredRagScarf       = "layered_rag_scarf";
            public const string ImprovisedLeadVest    = "improvised_lead_vest";
            public const string WeldingLeathers        = "welding_leathers";
            public const string ThermalUnderlayer     = "thermal_underlayer";
            public const string ImprovisedGaiters     = "improvised_gaiters";
        }

        public class Spec
        {
            public string Id;
            public string DisplayName;
            public ItemType Type;
            public int StackMax = 1;
            public float Weight;
            public float RadProtection;
            public float Durability;
            public float TradeValue;
            [TextArea(2, 4)] public string Description;
        }

        public static List<Spec> BuildAll()
        {
            var list = new List<Spec>();
            AddMedical(list);
            AddSurvival(list);
            AddWeapons(list);
            AddComfort(list);
            AddTech(list);
            AddFood(list);
            AddClothing(list);
            return list;
        }

        // ── Materialise ──────────────────────────────────────────────────
        public static ItemDefinition Materialise(Spec spec,
            System.Func<string, ItemDefinition> lookup)
        {
            if (spec == null) return null;
            var def = ScriptableObject.CreateInstance<ItemDefinition>();
            def.id = spec.Id;
            def.displayName = spec.DisplayName;
            def.description = spec.Description ?? string.Empty;
            def.type = spec.Type;
            def.stackMax = spec.StackMax;
            def.weight = spec.Weight;
            def.radProtection = spec.RadProtection;
            def.durability = spec.Durability;
            def.tradeValue = spec.TradeValue;
            return def;
        }

        public static List<ItemDefinition> MaterialiseAll(
            System.Func<string, ItemDefinition> lookup)
        {
            var specs = BuildAll();
            var outList = new List<ItemDefinition>(specs.Count);
            for (int i = 0; i < specs.Count; i++) outList.Add(Materialise(specs[i], lookup));
            return outList;
        }

        // ── I. Medical & pathology ───────────────────────────────────────
        private static void AddMedical(List<Spec> list)
        {
            list.Add(new Spec {
                Id = Ids.MorphineAmpoule, DisplayName = "Morphine Ampoule", Type = ItemType.Medical,
                StackMax = 8, Weight = 0.03f, Durability = 0, TradeValue = 32f,
                Description = "Single-use glass ampoule. Breaks the pain. Breaks the person after. Addiction track starts at dose three."
            });
            list.Add(new Spec {
                Id = Ids.SurgicalThread, DisplayName = "Surgical Thread", Type = ItemType.Medical,
                StackMax = 15, Weight = 0.02f, Durability = 0, TradeValue = 6f,
                Description = "Catgut on a curved needle. Closes wounds. Infection risk if the room isn't clean."
            });
            list.Add(new Spec {
                Id = Ids.Tourniquet, DisplayName = "Tourniquet", Type = ItemType.Medical,
                StackMax = 5, Weight = 0.15f, Durability = 40, TradeValue = 8f,
                Description = "Stops arterial bleed. Leaves the limb numb. Four hours max before tissue dies."
            });
            list.Add(new Spec {
                Id = Ids.BoneSplintKit, DisplayName = "Bone Splint Kit", Type = ItemType.Medical,
                StackMax = 3, Weight = 0.6f, Durability = 30, TradeValue = 14f,
                Description = "Aluminum stays and gauze wrap. Sets fractures. The screaming is free."
            });
            list.Add(new Spec {
                Id = Ids.BloodPressureCuff, DisplayName = "Blood Pressure Cuff", Type = ItemType.Tool,
                StackMax = 3, Weight = 0.2f, Durability = 60, TradeValue = 10f,
                Description = "Diagnoses internal bleeding and shock. Without it, you guess. Guessing kills."
            });
            list.Add(new Spec {
                Id = Ids.PotassiumPermanganate, DisplayName = "Potassium Permanganate", Type = ItemType.Material,
                StackMax = 15, Weight = 0.1f, Durability = 0, TradeValue = 4.5f,
                Description = "Purple crystals. Water sterilization, wound cleaning, signal fire dye. Versatile and finite."
            });
            list.Add(new Spec {
                Id = Ids.SalineDripBag, DisplayName = "Saline Drip Bag", Type = ItemType.Medical,
                StackMax = 6, Weight = 0.5f, Durability = 0, TradeValue = 18f,
                Description = "IV rehydration. The only thing that pulls someone back from severe dehydration without killing their kidneys."
            });
            list.Add(new Spec {
                Id = Ids.TetanusAntitoxin, DisplayName = "Tetanus Antitoxin", Type = ItemType.Medical,
                StackMax = 5, Weight = 0.05f, Durability = 0, TradeValue = 22f,
                Description = "One injection. Prevents lockjaw from dirty wounds. After symptoms appear, it's too late."
            });
            list.Add(new Spec {
                Id = Ids.BurnGauze, DisplayName = "Burn Gauze", Type = ItemType.Medical,
                StackMax = 10, Weight = 0.08f, Durability = 0, TradeValue = 9f,
                Description = "Silver-impregnated dressing. For second-degree burns. Third-degree needs skin that isn't there anymore."
            });
            list.Add(new Spec {
                Id = Ids.DefibrillatorPads, DisplayName = "Defibrillator Pads", Type = ItemType.Medical,
                StackMax = 2, Weight = 0.3f, Durability = 20, TradeValue = 25f,
                Description = "Adhesive pads. One shot. Needs a charged capacitor. The heart restarts or it doesn't."
            });
            list.Add(new Spec {
                Id = Ids.ColostomyBag, DisplayName = "Colostomy Bag", Type = ItemType.Medical,
                StackMax = 8, Weight = 0.1f, Durability = 0, TradeValue = 7f,
                Description = "For survivors with abdominal surgery. Dignity is a luxury the bunker can't afford."
            });
            list.Add(new Spec {
                Id = Ids.DialysisFilter, DisplayName = "Dialysis Filter", Type = ItemType.Medical,
                StackMax = 4, Weight = 0.8f, Durability = 30, TradeValue = 30f,
                Description = "Cleans blood of toxins and radiation metabolites. Without it, kidney failure is a slow drowning."
            });
            list.Add(new Spec {
                Id = Ids.PrussianBlueCapsules, DisplayName = "Prussian Blue Capsules", Type = ItemType.AntiRad,
                StackMax = 10, Weight = 0.03f, Durability = 0, TradeValue = 26f,
                Description = "Binds cesium and thallium in the gut. The only real decorporation agent left. Ration them like bullets."
            });
            list.Add(new Spec {
                Id = Ids.WoundPackingGauze, DisplayName = "Wound Packing Gauze", Type = ItemType.Medical,
                StackMax = 12, Weight = 0.1f, Durability = 0, TradeValue = 7.5f,
                Description = "Shove it into a gunshot hole and press. Stops the bleed. The wound still needs real surgery later."
            });
        }

        // ── I. Survival & shelter ────────────────────────────────────────
        private static void AddSurvival(List<Spec> list)
        {
            list.Add(new Spec {
                Id = Ids.AshTarp, DisplayName = "Ash Tarp", Type = ItemType.Material,
                StackMax = 8, Weight = 1.5f, RadProtection = 5, Durability = 40, TradeValue = 5f,
                Description = "Heavy plastic sheeting. Covers air intakes, patches roof leaks, wraps corpses. Three uses for a dead world."
            });
            list.Add(new Spec {
                Id = Ids.LeadSheet, DisplayName = "Lead Sheet", Type = ItemType.Material,
                StackMax = 6, Weight = 4f, RadProtection = 25, Durability = 0, TradeValue = 35f,
                Description = "Salvaged from hospital X-ray rooms. Lines walls. Heavy. The radiation doesn't care about your back."
            });
            list.Add(new Spec {
                Id = Ids.ConcretePatchMix, DisplayName = "Concrete Patch Mix", Type = ItemType.Material,
                StackMax = 10, Weight = 3f, Durability = 0, TradeValue = 8f,
                Description = "Seals cracks in bunker walls. Needs water. Every repair costs a drink."
            });
            list.Add(new Spec {
                Id = Ids.InsulationWrap, DisplayName = "Insulation Wrap", Type = ItemType.Material,
                StackMax = 10, Weight = 0.8f, Durability = 0, TradeValue = 6f,
                Description = "Fiberglass roll. Stops heat bleeding through pipes. Fingers itch for days."
            });
            list.Add(new Spec {
                Id = Ids.CandleTallow, DisplayName = "Tallow Candle", Type = ItemType.Comfort,
                StackMax = 20, Weight = 0.1f, Durability = 0, TradeValue = 1.5f,
                Description = "Rendered fat and a wick. Burns four hours. Smells like something that used to be alive."
            });
            list.Add(new Spec {
                Id = Ids.HandWarmerPacket, DisplayName = "Hand Warmer Packet", Type = ItemType.Comfort,
                StackMax = 15, Weight = 0.05f, Durability = 0, TradeValue = 2f,
                Description = "Iron powder oxidation. Twelve hours of warmth for two hands. Then it's garbage."
            });
            list.Add(new Spec {
                Id = Ids.RatTrapSpring, DisplayName = "Rat Trap (Spring)", Type = ItemType.Tool,
                StackMax = 10, Weight = 0.2f, Durability = 30, TradeValue = 3f,
                Description = "Kills one rat. Resets. The rats are getting smarter. The traps are getting fewer."
            });
            list.Add(new Spec {
                Id = Ids.GlueTrap, DisplayName = "Glue Trap", Type = ItemType.Tool,
                StackMax = 12, Weight = 0.1f, Durability = 0, TradeValue = 2f,
                Description = "Single-use adhesive board. The rat dies slowly. You hear it from the bunkroom."
            });
            list.Add(new Spec {
                Id = Ids.WaterCatchmentSheet, DisplayName = "Water Catchment Sheet", Type = ItemType.Material,
                StackMax = 5, Weight = 2f, Durability = 0, TradeValue = 9f,
                Description = "Spread on the surface after ash settles. Collects condensation. Also collects fallout. Filter before drinking. Always filter."
            });
            list.Add(new Spec {
                Id = Ids.RopeLadder5m, DisplayName = "Rope Ladder (5m)", Type = ItemType.Tool,
                StackMax = 2, Weight = 3.5f, Durability = 60, TradeValue = 16f,
                Description = "Climbing gear for surface access. The rungs are bent. It holds. Probably."
            });
            list.Add(new Spec {
                Id = Ids.CharcoalBriquette, DisplayName = "Charcoal Briquette", Type = ItemType.Fuel,
                StackMax = 30, Weight = 0.3f, Durability = 0, TradeValue = 1.8f,
                Description = "Compressed. Burns longer than wood. Less smoke. The bunker needs less smoke."
            });
            list.Add(new Spec {
                Id = Ids.PeatBlock, DisplayName = "Peat Block", Type = ItemType.Fuel,
                StackMax = 20, Weight = 1f, Durability = 0, TradeValue = 1.2f,
                Description = "Dried bog earth. Burns slow, smells like the ground remembers being alive."
            });
        }

        // ── I. Weapons & combat (grounded, no fantasy) ───────────────────
        private static void AddWeapons(List<Spec> list)
        {
            list.Add(new Spec {
                Id = Ids.PipeShotgunSingle, DisplayName = "Single-Shot Pipe Gun", Type = ItemType.Weapon,
                StackMax = 1, Weight = 2.8f, Durability = 25, TradeValue = 18f,
                Description = "One barrel, one shell. Miss and you're holding a club. The weld is the weak point."
            });
            list.Add(new Spec {
                Id = Ids.CrossbowSalvaged, DisplayName = "Salvaged Crossbow", Type = ItemType.Weapon,
                StackMax = 1, Weight = 3.2f, Durability = 50, TradeValue = 28f,
                Description = "Silent. Reusable bolts. The string frays after thirty shots. Thirty shots between you and the next string."
            });
            list.Add(new Spec {
                Id = Ids.BoltSteelPack10, DisplayName = "Steel Bolts (10x)", Type = ItemType.Weapon,
                StackMax = 20, Weight = 0.1f, Durability = 0, TradeValue = 4f,
                Description = "Crossbow ammunition. Recoverable if the target doesn't hit bone."
            });
            list.Add(new Spec {
                Id = Ids.NailBat, DisplayName = "Nail Bat", Type = ItemType.Weapon,
                StackMax = 1, Weight = 1.8f, Durability = 35, TradeValue = 5f,
                Description = "Two-by-four with nails hammered through. Crude. Effective. The nails bend after five good hits."
            });
            list.Add(new Spec {
                Id = Ids.SlingshotSteel, DisplayName = "Steel Slingshot", Type = ItemType.Weapon,
                StackMax = 1, Weight = 0.3f, Durability = 60, TradeValue = 6f,
                Description = "Ball bearings as ammo. Silent. Weak. Good for rats and warning shots. Not good for men."
            });
            list.Add(new Spec {
                Id = Ids.ImprovisedIncendiary, DisplayName = "Improvised Incendiary", Type = ItemType.Weapon,
                StackMax = 4, Weight = 0.8f, Durability = 0, TradeValue = 12f,
                Description = "Glass bottle, cloth wick, scavenged fuel. One use. Sets a two-metre area on fire. Including you if the wind changes."
            });
            list.Add(new Spec {
                Id = Ids.HatchetRusted, DisplayName = "Rusted Hatchet", Type = ItemType.Weapon,
                StackMax = 1, Weight = 1.4f, Durability = 20, TradeValue = 7f,
                Description = "The edge chips. Tetanus is the real threat. Still splits wood and bone alike."
            });
            list.Add(new Spec {
                Id = Ids.BearTrapSalvaged, DisplayName = "Salvaged Bear Trap", Type = ItemType.Tool,
                StackMax = 2, Weight = 4.5f, Durability = 40, TradeValue = 22f,
                Description = "Perimeter defense. Closes on a leg. The scream carries. Others will hear it. That's the point."
            });
        }

        // ── I. Comfort, morale & narrative ───────────────────────────────
        private static void AddComfort(List<Spec> list)
        {
            list.Add(new Spec {
                Id = Ids.HarmonicaDented, DisplayName = "Dented Harmonica", Type = ItemType.Comfort,
                StackMax = 1, Weight = 0.1f, Durability = 40, TradeValue = 4f,
                Description = "Three reeds are bent. Plays four notes. In the bunker, four notes is an orchestra."
            });
            list.Add(new Spec {
                Id = Ids.ChildShoePair, DisplayName = "Child's Shoes (Pair)", Type = ItemType.Comfort,
                StackMax = 2, Weight = 0.3f, Durability = 20, TradeValue = 1f,
                Description = "Size 4. No child fits them anymore. Someone keeps them anyway."
            });
            list.Add(new Spec {
                Id = Ids.SeedEnvelopeTomato, DisplayName = "Tomato Seeds (Envelope)", Type = ItemType.Material,
                StackMax = 10, Weight = 0.02f, Durability = 0, TradeValue = 3.5f,
                Description = "Pre-war heirloom. May not germinate in irradiated soil. Planting them is an act of faith."
            });
            list.Add(new Spec {
                Id = Ids.SeedEnvelopeWheat, DisplayName = "Wheat Seeds (Envelope)", Type = ItemType.Material,
                StackMax = 10, Weight = 0.03f, Durability = 0, TradeValue = 4f,
                Description = "Enough for one planter box. Sixty days to harvest. If the light holds. If the water holds. If."
            });
            list.Add(new Spec {
                Id = Ids.PlayingCardsWorn, DisplayName = "Worn Playing Cards", Type = ItemType.Comfort,
                StackMax = 3, Weight = 0.1f, Durability = 30, TradeValue = 2.5f,
                Description = "Fifty-one cards. The queen of hearts is missing. Someone drew a new one in pencil."
            });
            list.Add(new Spec {
                Id = Ids.ChessSetCarved, DisplayName = "Carved Chess Set", Type = ItemType.Comfort,
                StackMax = 1, Weight = 1.2f, Durability = 80, TradeValue = 12f,
                Description = "Hand-cut from a table leg. The knight is a little lopsided. Two survivors play every night. They don't talk during the game. That's the point."
            });
            list.Add(new Spec {
                Id = Ids.AshPressFlower, DisplayName = "Pressed Flower (Ash Rose)", Type = ItemType.Comfort,
                StackMax = 3, Weight = 0.01f, Durability = 0, TradeValue = 1f,
                Description = "Dried between two pages of a field manual. The petals are grey now. It was red once. Someone remembers."
            });
            list.Add(new Spec {
                Id = Ids.PocketWatchStopped, DisplayName = "Stopped Pocket Watch", Type = ItemType.Comfort,
                StackMax = 2, Weight = 0.15f, Durability = 0, TradeValue = 8f,
                Description = "Frozen at 11:47. The moment the sky lit up. Nobody winds it. Nobody throws it away."
            });
            list.Add(new Spec {
                Id = Ids.CigarettePackSealed, DisplayName = "Sealed Cigarette Pack (20)", Type = ItemType.Comfort,
                StackMax = 5, Weight = 0.05f, Durability = 0, TradeValue = 9f,
                Description = "Pre-war brand. Still sealed. The tax stamp is from a country that doesn't exist. Worth more than ammunition to the right person."
            });
            list.Add(new Spec {
                Id = Ids.VialOfSoil, DisplayName = "Vial of Pre-War Soil", Type = ItemType.Quest,
                StackMax = 1, Weight = 0.2f, Durability = 0, TradeValue = 0f,
                Description = "Dark, clean earth in a glass tube. Smells like rain. A survivor from the lowlands asked for it. You don't know why. You don't ask."
            });
        }

        // ── I. Scavenged tech & components ────────────────────────────────
        private static void AddTech(List<Spec> list)
        {
            list.Add(new Spec {
                Id = Ids.GeigerTubeSalvaged, DisplayName = "Salvaged Geiger Tube", Type = ItemType.Material,
                StackMax = 5, Weight = 0.15f, Durability = 0, TradeValue = 14f,
                Description = "The sensing element from a dead counter. Needs recalibration. Without it, you're blind to the invisible."
            });
            list.Add(new Spec {
                Id = Ids.CapacitorBankSmall, DisplayName = "Small Capacitor Bank", Type = ItemType.Material,
                StackMax = 6, Weight = 0.8f, Durability = 0, TradeValue = 11f,
                Description = "Stores a charge. Powers a defibrillator, a radio burst, or a single light bulb for a night. Pick one."
            });
            list.Add(new Spec {
                Id = Ids.CopperTubing1m, DisplayName = "Copper Tubing (1m)", Type = ItemType.Material,
                StackMax = 10, Weight = 0.6f, Durability = 0, TradeValue = 5f,
                Description = "Distillation coils, water routing, still construction. The plumbers fought over these."
            });
            list.Add(new Spec {
                Id = Ids.RubberGasketSet, DisplayName = "Rubber Gasket Set", Type = ItemType.Material,
                StackMax = 10, Weight = 0.1f, Durability = 0, TradeValue = 7f,
                Description = "Hatch seals, pipe joints, air filter housings. When these crack, the outside gets in."
            });
            list.Add(new Spec {
                Id = Ids.WeldingRodPack20, DisplayName = "Welding Rods (20x)", Type = ItemType.Material,
                StackMax = 8, Weight = 0.4f, Durability = 0, TradeValue = 8.5f,
                Description = "Joins metal. Needs a torch and a steady hand. The fumes in an enclosed space are their own hazard."
            });
            list.Add(new Spec {
                Id = Ids.CircuitBreaker30a, DisplayName = "Circuit Breaker (30A)", Type = ItemType.Material,
                StackMax = 8, Weight = 0.3f, Durability = 0, TradeValue = 9f,
                Description = "Replaces a blown breaker in the power grid. Without it, one short kills the whole shelter's lighting."
            });
            list.Add(new Spec {
                Id = Ids.BearingSetIndustrial, DisplayName = "Industrial Bearing Set", Type = ItemType.Material,
                StackMax = 5, Weight = 0.5f, Durability = 0, TradeValue = 12f,
                Description = "Generator shafts, fan motors, treadmill axles. When a bearing seizes, the lights go out."
            });
        }

        // ── I. Food & consumables (expanded) ─────────────────────────────
        private static void AddFood(List<Spec> list)
        {
            list.Add(new Spec {
                Id = Ids.RatMeatSkewer, DisplayName = "Rat Meat Skewer", Type = ItemType.Food,
                StackMax = 8, Weight = 0.2f, Durability = 0, TradeValue = 1f,
                Description = "Cooked over the heater. Tastes like chicken if you close your eyes. You don't close your eyes."
            });
            list.Add(new Spec {
                Id = Ids.InsectPasteBrick, DisplayName = "Insect Paste Brick", Type = ItemType.Food,
                StackMax = 12, Weight = 0.3f, Durability = 0, TradeValue = 2f,
                Description = "Ground beetles and larvae, pressed and dried. Protein is protein. The texture is not negotiable."
            });
            list.Add(new Spec {
                Id = Ids.AshBreadFlat, DisplayName = "Ash Bread (Flat)", Type = ItemType.Food,
                StackMax = 10, Weight = 0.25f, Durability = 0, TradeValue = 2.5f,
                Description = "Flour, water, a hot surface. No yeast. No salt. Chewing it is an act of will."
            });
            list.Add(new Spec {
                Id = Ids.CannedMilkCondensed, DisplayName = "Condensed Milk (Can)", Type = ItemType.Food,
                StackMax = 8, Weight = 0.4f, Durability = 0, TradeValue = 6f,
                Description = "Sugar and fat. A spoonful in coffee makes the bunker feel like a kitchen for ten seconds."
            });
            list.Add(new Spec {
                Id = Ids.VinegarBottle, DisplayName = "Vinegar Bottle", Type = ItemType.Material,
                StackMax = 6, Weight = 0.5f, Durability = 0, TradeValue = 3f,
                Description = "Preserves food, cleans wounds, cuts through grime. The smell is the smell of someone trying."
            });
            list.Add(new Spec {
                Id = Ids.DriedMushroomString, DisplayName = "Dried Mushrooms (String)", Type = ItemType.Food,
                StackMax = 10, Weight = 0.15f, Durability = 0, TradeValue = 2f,
                Description = "Foraged from the sub-basement. The Geiger clicks near them. Cook them twice. Pray."
            });
            list.Add(new Spec {
                Id = Ids.SaltPorkStrip, DisplayName = "Salt Pork Strip", Type = ItemType.Food,
                StackMax = 8, Weight = 0.3f, Durability = 0, TradeValue = 5f,
                Description = "Cured and tough. Lasts months. Tastes like the animal died afraid."
            });
            list.Add(new Spec {
                Id = Ids.WildGarlicBulb, DisplayName = "Wild Garlic Bulb", Type = ItemType.Food,
                StackMax = 15, Weight = 0.08f, Durability = 0, TradeValue = 1.5f,
                Description = "Grows in the cracks where the ash is thin. Sharp smell. Fights scurvy a little."
            });
        }

        // ── I. Clothing & protective gear (degraded tier) ────────────────
        private static void AddClothing(List<Spec> list)
        {
            list.Add(new Spec {
                Id = Ids.PatchedRaincoat, DisplayName = "Patched Raincoat", Type = ItemType.Protective,
                StackMax = 1, Weight = 1.2f, RadProtection = 8, Durability = 35, TradeValue = 7f,
                Description = "Three layers of duct tape over a split seam. Keeps the ash off. The ash gets in anyway."
            });
            list.Add(new Spec {
                Id = Ids.LayeredRagScarf, DisplayName = "Layered Rag Scarf", Type = ItemType.Protective,
                StackMax = 3, Weight = 0.3f, RadProtection = 5, Durability = 25, TradeValue = 2.5f,
                Description = "Wrapped tight over the nose. Filters coarse ash. Not fine particulate. Better than breathing glass."
            });
            list.Add(new Spec {
                Id = Ids.ImprovisedLeadVest, DisplayName = "Improvised Lead Vest", Type = ItemType.Protective,
                StackMax = 1, Weight = 8f, RadProtection = 20, Durability = 40, TradeValue = 28f,
                Description = "Lead sheets sewn into a canvas jacket. Heavy. The back gives out before the radiation does."
            });
            list.Add(new Spec {
                Id = Ids.WeldingLeathers, DisplayName = "Welding Leathers", Type = ItemType.Protective,
                StackMax = 1, Weight = 3.5f, RadProtection = 3, Durability = 70, TradeValue = 14f,
                Description = "Thick cowhide. Stops sparks, minor shrapnel, and the worst of the cold. Smells like burnt hair."
            });
            list.Add(new Spec {
                Id = Ids.ThermalUnderlayer, DisplayName = "Thermal Underlayer", Type = ItemType.Protective,
                StackMax = 2, Weight = 0.5f, Durability = 50, TradeValue = 11f,
                Description = "Thin synthetic base layer. Traps body heat. Without it, the cold gets in at the joints first."
            });
            list.Add(new Spec {
                Id = Ids.ImprovisedGaiters, DisplayName = "Improvised Gaiters", Type = ItemType.Protective,
                StackMax = 2, Weight = 0.6f, RadProtection = 2, Durability = 30, TradeValue = 4f,
                Description = "Canvas wraps from knee to boot. Stops ash from getting into shoes. Stops trench foot. Maybe."
            });
        }
    }
}
