using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Expansion XI — "The Glass Orchard". Central id registry (the single
    /// source of truth for every id this pack mints) and the crop-definition
    /// table that <see cref="GreenhouseSystem"/> simulates against. Pure data,
    /// no host coupling — so the growth/contamination math is testable without
    /// an engine. Mirrors the const-id pattern of <c>MutatedEcosystemSystem</c>
    /// and the catalog pattern of <c>CrossingIds</c>.
    /// </summary>
    public static class GreenhouseExpansionCatalog
    {
        // ── Saveable id ────────────────────────────────────────────────
        public const string SaveId = "greenhouse";

        // ── Items (defined in greenhouse_items.json) ───────────────────
        public static class Items
        {
            public const string SeedMushroom = "item_seed_mushroom";
            public const string SeedTuber = "item_seed_tuber";
            public const string SeedGrain = "item_seed_grain";
            public const string SeedWheat = "item_seed_wheat";

            public const string PlanterBox = "item_planter_box";
            public const string GrowLamp = "item_grow_lamp";
            public const string LeadGlassPane = "item_lead_glass_pane";
            public const string BlightTreatment = "item_blight_treatment";
            public const string GrowMedium = "item_grow_medium";

            public const string CropMushroom = "crop_mushroom";
            public const string CropTuber = "crop_tuber";
            public const string CropGrain = "crop_grain";
            public const string CropWheat = "crop_wheat";
            public const string TaintedFood = "tainted_food";
        }

        // ── Locations (applied at runtime to the location catalog) ─────
        public static class Locations
        {
            public const string GlasshouseRuins = "location_glasshouse_ruins";
            public const string SeedVault = "location_seed_vault";
            public const string HydroBaronsAquaponics = "location_hydro_barons_aquaponics";
            public const string RotFarmersCompostYard = "location_rot_farmers_compost_yard";
        }

        // ── Events (authored in GreenhouseEventFactory) ────────────────
        public static class Events
        {
            public const string FirstSprout = "greenhouse_first_sprout";
            public const string BlightOutbreak = "greenhouse_blight_outbreak";
            public const string TaintedHarvest = "greenhouse_tainted_harvest";
            public const string TheOffering = "greenhouse_the_offering";
            public const string DeadGardener = "greenhouse_dead_gardener";
            public const string GlassBreaks = "greenhouse_glass_breaks";
        }

        // ── World / event flags (fire-once + state) ────────────────────
        public static class Flags
        {
            public const string FirstSproutSeen = "flag_greenhouse_first_sprout_seen";
            public const string WheatUnlocked = "flag_greenhouse_wheat_unlocked";
        }

        // ── Lore knowledge keys (world_history.json) ───────────────────
        public static class Lore
        {
            public const string MunicipalFeeding = "lore_greenhouse_municipal_feeding";
            public const string SeedVaultPurpose = "lore_greenhouse_seed_vault";
            public const string FirstGardener = "lore_greenhouse_first_gardener";
            public const string LeadGlassWorks = "lore_greenhouse_lead_glass_works";
        }

        // ═══════════════════════════════════════════════════════════════
        // Crop definitions
        // ═══════════════════════════════════════════════════════════════

        /// <summary>
        /// A plantable crop. Keyed by the seed item id the player spends to plant it.
        /// Yields are item ids granted on harvest — clean below
        /// <see cref="ContaminationTolerance"/>, tainted (usually <c>tainted_food</c>) at/above it.
        /// </summary>
        [Serializable]
        public class CropDef
        {
            public string SeedItemId;        // also the crop key stored on a plot
            public string YieldCleanId;
            public string YieldTaintedId;
            public float GrowthHoursToMature; // total light-hours of growth to reach Mature
            public float WaterPerDay;
            public float LightHoursPerDay;    // full-growth light requirement
            public int BaseYield;
            public float BlightResistance;    // 0..1
            public float ContaminationTolerance; // soilContamination (0..100) at/above which harvest is tainted
            public bool RequiresUnlock;       // pre-war wheat: gated behind the seed ledger
        }

        /// <summary>
        /// Authoritative crop table. Tuning lives here (not in JSON) so the
        /// simulation is deterministic and unit-testable without a host — the
        /// same convention <c>MutatedEcosystemSystem</c> uses for its constants.
        /// </summary>
        public static class CropCatalog
        {
            public static readonly CropDef[] All =
            {
                new CropDef
                {
                    SeedItemId = Items.SeedMushroom,
                    YieldCleanId = Items.CropMushroom,
                    YieldTaintedId = Items.TaintedFood,
                    GrowthHoursToMature = 96f,    // ~4 days at full 4h light
                    WaterPerDay = 8f,
                    LightHoursPerDay = 4f,
                    BaseYield = 2,
                    BlightResistance = 0.85f,
                    ContaminationTolerance = 60f,
                    RequiresUnlock = false
                },
                new CropDef
                {
                    SeedItemId = Items.SeedTuber,
                    YieldCleanId = Items.CropTuber,
                    YieldTaintedId = Items.TaintedFood,
                    GrowthHoursToMature = 168f,   // ~7 days at full 8h light
                    WaterPerDay = 16f,
                    LightHoursPerDay = 8f,
                    BaseYield = 3,
                    BlightResistance = 0.6f,
                    ContaminationTolerance = 45f,
                    RequiresUnlock = false
                },
                new CropDef
                {
                    SeedItemId = Items.SeedGrain,
                    YieldCleanId = Items.CropGrain,
                    YieldTaintedId = Items.TaintedFood,
                    GrowthHoursToMature = 240f,   // ~10 days at full 12h light
                    WaterPerDay = 22f,
                    LightHoursPerDay = 12f,
                    BaseYield = 5,
                    BlightResistance = 0.3f,
                    ContaminationTolerance = 35f,
                    RequiresUnlock = false
                },
                new CropDef
                {
                    SeedItemId = Items.SeedWheat,
                    YieldCleanId = Items.CropWheat,
                    YieldTaintedId = Items.TaintedFood,
                    GrowthHoursToMature = 200f,   // ~8 days at full 10h light
                    WaterPerDay = 20f,
                    LightHoursPerDay = 10f,
                    BaseYield = 6,
                    BlightResistance = 0.55f,
                    ContaminationTolerance = 50f,
                    RequiresUnlock = true         // the Svalbard Seed Ledger reward
                }
            };

            /// <summary>Resolve a crop by its seed item id, or null if unknown.</summary>
            public static CropDef Get(string seedItemId)
            {
                if (string.IsNullOrEmpty(seedItemId)) return null;
                for (int i = 0; i < All.Length; i++)
                    if (All[i].SeedItemId == seedItemId) return All[i];
                return null;
            }
        }
    }
}
