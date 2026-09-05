using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core
{
    /// <summary>
    /// ASHFALL: THE GLASS ORCHARD (Expansion 05 / XI) — Pure Catalog & ID Registry.
    /// Central ID registry and crop-definition table for the greenhouse simulation.
    /// </summary>
    public static class GreenhouseExpansionCatalog
    {
        public const string SaveId = "greenhouse";

        public static class Items
        {
            // F18 — pre-existing generic seed packet (items.json "seed_packets",
            // "assorted vegetable seeds, some dated, some anonymous"). Route it
            // through the canonical crop catalog so grants (e.g. the ruined
            // greenhouse micro-location) are real agriculture inputs instead of
            // dead inventory. Data-contract fix at the crop authority — no
            // micro-location-specific planting exception exists.
            public const string SeedPacketsMixed = "seed_packets";

            public const string SeedMushroom = "item_seed_mushroom";
            public const string SeedTuber = "item_seed_tuber";
            public const string SeedGrain = "item_seed_grain";
            public const string SeedWheat = "item_seed_wheat";
            public const string SeedHardyTuber = "item_seed_hardy_tuber";
            public const string SeedAshGrain = "item_seed_ash_grain";
            public const string SeedBiolumMushroom = "item_seed_biolum_mushroom";
            public const string SeedNutrientAlgae = "item_seed_nutrient_algae";
            public const string SeedMedicinalHerb = "item_seed_medicinal_herb";
            public const string SeedLeafyGreen = "item_seed_leafy_green";
            public const string SeedOilseed = "item_seed_oilseed";
            public const string SeedColdLegume = "item_seed_cold_legume";

            public const string PlanterBox = "item_planter_box";
            public const string GrowLamp = "item_grow_lamp";
            public const string LeadGlassPane = "item_lead_glass_pane";
            public const string BlightTreatment = "item_blight_treatment";
            public const string GrowMedium = "item_grow_medium";

            public const string CropMushroom = "crop_mushroom";
            public const string CropTuber = "crop_tuber";
            public const string CropGrain = "crop_grain";
            public const string CropWheat = "crop_wheat";
            public const string CropHardyTuber = "crop_hardy_tuber";
            public const string CropAshGrain = "crop_ash_grain";
            public const string CropBiolumMushroom = "crop_biolum_mushroom";
            public const string CropNutrientAlgae = "crop_nutrient_algae";
            public const string CropMedicinalHerb = "crop_medicinal_herb";
            public const string CropLeafyGreen = "crop_leafy_green";
            public const string CropOilseed = "crop_oilseed";
            public const string CropColdLegume = "crop_cold_legume";
            public const string TaintedFood = "tainted_food";
        }

        public static class Locations
        {
            public const string GlasshouseRuins = "location_glasshouse_ruins";
            public const string SeedVault = "location_seed_vault";
            public const string HydroBaronsAquaponics = "location_hydro_barons_aquaponics";
            public const string RotFarmersCompostYard = "location_rot_farmers_compost_yard";
        }

        public static class Events
        {
            public const string FirstSprout = "greenhouse_first_sprout";
            public const string BlightOutbreak = "greenhouse_blight_outbreak";
            public const string TaintedHarvest = "greenhouse_tainted_harvest";
            public const string TheOffering = "greenhouse_the_offering";
            public const string DeadGardener = "greenhouse_dead_gardener";
            public const string GlassBreaks = "greenhouse_glass_breaks";
        }

        public static class Flags
        {
            public const string FirstSproutSeen = "flag_greenhouse_first_sprout_seen";
            public const string WheatUnlocked = "flag_greenhouse_wheat_unlocked";
        }

        public static class Lore
        {
            public const string MunicipalFeeding = "lore_greenhouse_municipal_feeding";
            public const string SeedVaultPurpose = "lore_greenhouse_seed_vault";
            public const string FirstGardener = "lore_greenhouse_first_gardener";
            public const string LeadGlassWorks = "lore_greenhouse_lead_glass_works";
        }

        [Serializable]
        public class CropDef
        {
            public string SeedItemId;
            public string YieldCleanId;
            public string YieldTaintedId;
            public float GrowthHoursToMature;
            public float WaterPerDay;
            public float LightHoursPerDay;
            public int BaseYield;
            public float BlightResistance;
            public float ContaminationTolerance;
            public bool RequiresUnlock;
        }

        public static class CropCatalog
        {
            public static readonly CropDef[] All =
            {
                new CropDef
                {
                    SeedItemId = Items.SeedMushroom,
                    YieldCleanId = Items.CropMushroom,
                    YieldTaintedId = Items.TaintedFood,
                    GrowthHoursToMature = 96f,
                    WaterPerDay = 8f,
                    LightHoursPerDay = 4f,
                    BaseYield = 2,
                    BlightResistance = 0.85f,
                    ContaminationTolerance = 60f,
                    RequiresUnlock = false
                },
                new CropDef
                {
                    // F18 — mixed assorted-vegetable packet maps to the hardy
                    // starter staple: anonymous seeds most reliably grow tubers.
                    // Stats mirror the item_seed_tuber profile (no invented
                    // balance layer); plantability flows through the one
                    // canonical CropCatalog lookup, unchanged for every caller.
                    SeedItemId = Items.SeedPacketsMixed,
                    YieldCleanId = Items.CropTuber,
                    YieldTaintedId = Items.TaintedFood,
                    GrowthHoursToMature = 144f,
                    WaterPerDay = 12f,
                    LightHoursPerDay = 6f,
                    BaseYield = 3,
                    BlightResistance = 0.70f,
                    ContaminationTolerance = 45f,
                    RequiresUnlock = false
                },
                new CropDef
                {
                    SeedItemId = Items.SeedTuber,
                    YieldCleanId = Items.CropTuber,
                    YieldTaintedId = Items.TaintedFood,
                    GrowthHoursToMature = 144f,
                    WaterPerDay = 12f,
                    LightHoursPerDay = 6f,
                    BaseYield = 3,
                    BlightResistance = 0.70f,
                    ContaminationTolerance = 45f,
                    RequiresUnlock = false
                },
                new CropDef
                {
                    SeedItemId = Items.SeedGrain,
                    YieldCleanId = Items.CropGrain,
                    YieldTaintedId = Items.TaintedFood,
                    GrowthHoursToMature = 192f,
                    WaterPerDay = 16f,
                    LightHoursPerDay = 8f,
                    BaseYield = 4,
                    BlightResistance = 0.55f,
                    ContaminationTolerance = 30f,
                    RequiresUnlock = false
                },
                new CropDef
                {
                    SeedItemId = Items.SeedWheat,
                    YieldCleanId = Items.CropWheat,
                    YieldTaintedId = Items.TaintedFood,
                    GrowthHoursToMature = 240f,
                    WaterPerDay = 20f,
                    LightHoursPerDay = 10f,
                    BaseYield = 6,
                    BlightResistance = 0.40f,
                    ContaminationTolerance = 20f,
                    RequiresUnlock = true
                },
                new CropDef
                {
                    SeedItemId = Items.SeedHardyTuber,
                    YieldCleanId = Items.CropHardyTuber,
                    YieldTaintedId = Items.TaintedFood,
                    GrowthHoursToMature = 120f,
                    WaterPerDay = 10f,
                    LightHoursPerDay = 5f,
                    BaseYield = 3,
                    BlightResistance = 0.80f,
                    ContaminationTolerance = 55f,
                    RequiresUnlock = false
                },
                new CropDef
                {
                    SeedItemId = Items.SeedAshGrain,
                    YieldCleanId = Items.CropAshGrain,
                    YieldTaintedId = Items.TaintedFood,
                    GrowthHoursToMature = 168f,
                    WaterPerDay = 14f,
                    LightHoursPerDay = 6f,
                    BaseYield = 4,
                    BlightResistance = 0.65f,
                    ContaminationTolerance = 40f,
                    RequiresUnlock = false
                },
                new CropDef
                {
                    SeedItemId = Items.SeedBiolumMushroom,
                    YieldCleanId = Items.CropBiolumMushroom,
                    YieldTaintedId = Items.TaintedFood,
                    GrowthHoursToMature = 84f,
                    WaterPerDay = 6f,
                    LightHoursPerDay = 2f,
                    BaseYield = 2,
                    BlightResistance = 0.90f,
                    ContaminationTolerance = 70f,
                    RequiresUnlock = false
                },
                new CropDef
                {
                    SeedItemId = Items.SeedNutrientAlgae,
                    YieldCleanId = Items.CropNutrientAlgae,
                    YieldTaintedId = Items.TaintedFood,
                    GrowthHoursToMature = 72f,
                    WaterPerDay = 18f,
                    LightHoursPerDay = 8f,
                    BaseYield = 5,
                    BlightResistance = 0.75f,
                    ContaminationTolerance = 50f,
                    RequiresUnlock = false
                },
                new CropDef
                {
                    SeedItemId = Items.SeedMedicinalHerb,
                    YieldCleanId = Items.CropMedicinalHerb,
                    YieldTaintedId = Items.TaintedFood,
                    GrowthHoursToMature = 160f,
                    WaterPerDay = 12f,
                    LightHoursPerDay = 7f,
                    BaseYield = 3,
                    BlightResistance = 0.60f,
                    ContaminationTolerance = 35f,
                    RequiresUnlock = false
                },
                new CropDef
                {
                    SeedItemId = Items.SeedLeafyGreen,
                    YieldCleanId = Items.CropLeafyGreen,
                    YieldTaintedId = Items.TaintedFood,
                    GrowthHoursToMature = 60f,
                    WaterPerDay = 10f,
                    LightHoursPerDay = 6f,
                    BaseYield = 3,
                    BlightResistance = 0.50f,
                    ContaminationTolerance = 25f,
                    RequiresUnlock = false
                },
                new CropDef
                {
                    SeedItemId = Items.SeedOilseed,
                    YieldCleanId = Items.CropOilseed,
                    YieldTaintedId = Items.TaintedFood,
                    GrowthHoursToMature = 210f,
                    WaterPerDay = 15f,
                    LightHoursPerDay = 9f,
                    BaseYield = 4,
                    BlightResistance = 0.55f,
                    ContaminationTolerance = 30f,
                    RequiresUnlock = false
                },
                new CropDef
                {
                    SeedItemId = Items.SeedColdLegume,
                    YieldCleanId = Items.CropColdLegume,
                    YieldTaintedId = Items.TaintedFood,
                    GrowthHoursToMature = 150f,
                    WaterPerDay = 11f,
                    LightHoursPerDay = 6f,
                    BaseYield = 4,
                    BlightResistance = 0.75f,
                    ContaminationTolerance = 40f,
                    RequiresUnlock = false
                }
            };

            private static Dictionary<string, CropDef> _bySeed;

            public static CropDef? Get(string seedItemId)
            {
                if (string.IsNullOrEmpty(seedItemId)) return null;
                if (_bySeed == null)
                {
                    var map = new Dictionary<string, CropDef>(StringComparer.Ordinal);
                    for (int i = 0; i < All.Length; i++)
                    {
                        var c = All[i];
                        if (c != null && !string.IsNullOrEmpty(c.SeedItemId))
                            map[c.SeedItemId] = c;
                    }
                    _bySeed = map;
                }
                _bySeed.TryGetValue(seedItemId, out var def);
                return def;
            }
        }
    }
}
