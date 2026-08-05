using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// Factory that creates the five shelter layout definitions (Prompts #80-#84).
    /// These are programmatic ScriptableObject instances since we cannot create
    /// .asset files from the command line. Called by GameBootstrap at init.
    /// </summary>
    public static class ShelterLayoutFactory
    {
        public static Shelter.ShelterMapSO CreateSuburbanSplitLevel()
        {
            var layout = ScriptableObject.CreateInstance<Shelter.ShelterMapSO>();
            layout.layoutId = "layout_suburban";
            layout.layoutName = "Suburban Split-Level";
            layout.description =
                "A 1970s split-level with a finished basement. The living room above " +
                "collapsed in the first shelling. The water heater is intact — for now.";
            layout.roomCount = 3;
            layout.roomIds = new[] { "basement_main", "laundry_room", "storage_closet" };
            layout.roomNames = new[] { "Basement Main", "Laundry Room", "Storage Closet" };
            layout.roomSizes = new[] { 2f, 1f, 0.5f };

            layout.startingModules = new Shelter.ShelterLayoutModule[]
            {
                new Shelter.ShelterLayoutModule { moduleId = "bed", level = 1, roomId = "basement_main", filterHealth = 100f },
                new Shelter.ShelterLayoutModule { moduleId = "workbench", level = 1, roomId = "basement_main" }
            };

            layout.traits = new[]
            {
                Shelter.ShelterLayoutTrait.WaterHeater,
                Shelter.ShelterLayoutTrait.FallenBeam
            };

            layout.debrisType = Shelter.DebrisType.WoodRubble;
            layout.startingDebrisHours = 16f;
            layout.inherentShielding = 0f;
            layout.debrisShieldingBonus = 0.25f;
            layout.startingCleanWater = 20f;
            layout.hasWaterHeater = true;
            layout.startingHouseDurability = 80f;
            layout.startingIntegrity = 90f;
            layout.startingHatchSecurity = 5f;
            layout.anomalies = new[] { "fallen_beam_stairs" };
            return layout;
        }

        public static Shelter.ShelterMapSO CreateRuralFarmhouse()
        {
            var layout = ScriptableObject.CreateInstance<Shelter.ShelterMapSO>();
            layout.layoutId = "layout_farmhouse";
            layout.layoutName = "Rural Farmhouse";
            layout.description =
                "A century-old root cellar under a farmhouse. Dirt floors, damp walls, " +
                "but the shelves are still stocked with mason jars. The hatch opens to " +
                "open sky — beautiful until the ash starts falling.";
            layout.roomCount = 1;
            layout.roomIds = new[] { "cellar" };
            layout.roomNames = new[] { "Root Cellar" };
            layout.roomSizes = new[] { 3f };

            layout.startingModules = new Shelter.ShelterLayoutModule[]
            {
                new Shelter.ShelterLayoutModule { moduleId = "bed", level = 1, roomId = "cellar" },
                new Shelter.ShelterLayoutModule { moduleId = "catchment_surface", level = 1, roomId = "cellar", isEnabled = true }
            };

            layout.traits = new[]
            {
                Shelter.ShelterLayoutTrait.RootCellar,
                Shelter.ShelterLayoutTrait.DirtFloor,
                Shelter.ShelterLayoutTrait.ExposedHatch
            };

            layout.debrisType = Shelter.DebrisType.Dirt;
            layout.startingDebrisHours = 4f;
            layout.inherentShielding = 0f;
            layout.debrisShieldingBonus = 0f;
            layout.startingCleanWater = 5f;
            layout.startingHouseDurability = 60f;
            layout.startingIntegrity = 75f;
            layout.integrityDegradeMultiplier = 1.5f;
            layout.startingHatchSecurity = 2f;
            layout.startingHatchDamage = 30f;
            layout.anomalies = new[] { "exposed_hatch", "dirt_floor" };
            return layout;
        }

        public static Shelter.ShelterMapSO CreateUrbanBrownstone()
        {
            var layout = ScriptableObject.CreateInstance<Shelter.ShelterMapSO>();
            layout.layoutId = "layout_brownstone";
            layout.layoutName = "Urban Brownstone Basement";
            layout.description =
                "A narrow four-room basement between two identical brownstones. " +
                "The brick walls have held for a century. The shared plumbing has not. " +
                "Sometimes water disappears. Sometimes it comes back wrong.";
            layout.roomCount = 4;
            layout.roomIds = new[] { "front_cellar", "coal_room", "kitchen_cellar", "back_cellar" };
            layout.roomNames = new[] { "Front Cellar", "Coal Room", "Kitchen Cellar", "Back Cellar" };
            layout.roomSizes = new[] { 1f, 0.5f, 1f, 1f };

            layout.startingModules = new Shelter.ShelterLayoutModule[]
            {
                new Shelter.ShelterLayoutModule { moduleId = "bed", level = 1, roomId = "front_cellar" },
                new Shelter.ShelterLayoutModule { moduleId = "heater", level = 1, roomId = "coal_room", fuel = 20f },
                new Shelter.ShelterLayoutModule { moduleId = "workbench", level = 1, roomId = "kitchen_cellar" }
            };

            layout.traits = new[]
            {
                Shelter.ShelterLayoutTrait.SharedWalls,
                Shelter.ShelterLayoutTrait.SharedPipe
            };

            layout.debrisType = Shelter.DebrisType.BrickRubble;
            layout.startingDebrisHours = 20f;
            layout.inherentShielding = 0.05f;
            layout.debrisShieldingBonus = 0.15f;
            layout.startingCleanWater = 15f;
            layout.startingHouseDurability = 120f;
            layout.startingIntegrity = 95f;
            layout.startingHatchSecurity = 15f;
            layout.anomalies = new[] { "shared_pipe" };
            return layout;
        }

        public static Shelter.ShelterMapSO CreatePrepperCabin()
        {
            var layout = ScriptableObject.CreateInstance<Shelter.ShelterMapSO>();
            layout.layoutId = "layout_prepper";
            layout.layoutName = "Prepper's Cabin";
            layout.description =
                "The previous owner knew what was coming. They just didn't survive " +
                "the raid. The air filter is broken. The generator has no fuel. " +
                "But the walls are reinforced, and the scavengers know it.";
            layout.roomCount = 2;
            layout.roomIds = new[] { "main_bunker", "generator_room" };
            layout.roomNames = new[] { "Main Bunker", "Generator Room" };
            layout.roomSizes = new[] { 2f, 1f };

            layout.startingModules = new Shelter.ShelterLayoutModule[]
            {
                new Shelter.ShelterLayoutModule { moduleId = "air_filtration", level = 1, roomId = "main_bunker", filterHealth = 20f, isEnabled = false },
                new Shelter.ShelterLayoutModule { moduleId = "radiation_shielding", level = 1, roomId = "main_bunker" },
                new Shelter.ShelterLayoutModule { moduleId = "bed", level = 2, roomId = "main_bunker" },
                new Shelter.ShelterLayoutModule { moduleId = "workbench", level = 1, roomId = "main_bunker" }
            };

            layout.traits = new[]
            {
                Shelter.ShelterLayoutTrait.PrepperCache
            };

            layout.debrisType = Shelter.DebrisType.MetalDebris;
            layout.startingDebrisHours = 8f;
            layout.inherentShielding = 0.1f;
            layout.debrisShieldingBonus = 0.1f;
            layout.startingCleanWater = 10f;
            layout.startingHouseDurability = 90f;
            layout.startingIntegrity = 85f;
            layout.startingHatchSecurity = 20f;
            layout.startingHatchDamage = 50f;
            layout.anomalies = new[] { "pre_raid_damage" };
            return layout;
        }

        public static Shelter.ShelterMapSO CreateMakeshiftMetro()
        {
            var layout = ScriptableObject.CreateInstance<Shelter.ShelterMapSO>();
            layout.layoutId = "layout_metro";
            layout.layoutName = "Makeshift Metro Station";
            layout.description =
                "An abandoned subway maintenance tunnel, thirty feet below the street. " +
                "The depth is a blessing — the water seeping through the ceiling is not. " +
                "No roof for a catchment. No easy exit. But the radiation can't reach here.";
            layout.roomCount = 5;
            layout.roomIds = new[] { "platform", "tunnel_a", "tunnel_b", "maintenance_room", "signal_room" };
            layout.roomNames = new[] { "Platform", "Tunnel A", "Tunnel B", "Maintenance", "Signal Room" };
            layout.roomSizes = new[] { 3f, 2f, 2f, 1f, 0.5f };

            layout.startingModules = new Shelter.ShelterLayoutModule[]
            {
                new Shelter.ShelterLayoutModule { moduleId = "bed", level = 1, roomId = "maintenance_room" },
                new Shelter.ShelterLayoutModule { moduleId = "workbench", level = 1, roomId = "platform" },
                new Shelter.ShelterLayoutModule { moduleId = "radiation_shielding", level = 3, roomId = "platform" }
            };

            layout.traits = new[]
            {
                Shelter.ShelterLayoutTrait.DeepUnderground,
                Shelter.ShelterLayoutTrait.Flooded
            };

            layout.debrisType = Shelter.DebrisType.ConcreteRubble;
            layout.startingDebrisHours = 30f;
            layout.inherentShielding = 0.4f;
            layout.debrisShieldingBonus = 0.1f;
            layout.startingCleanWater = 0f;
            layout.startingHouseDurability = 200f;
            layout.startingIntegrity = 100f;
            layout.startingHatchSecurity = 10f;
            layout.anomalies = new[] { "flooded", "no_catchment" };
            return layout;
        }

        // ═══════════════════════════════════════════════════════════════
        // PROMPTS #129–#133 — ADDITIONAL LAYOUTS
        // ═══════════════════════════════════════════════════════════════

        public static Shelter.ShelterMapSO CreateGasStation()
        {
            var l = ScriptableObject.CreateInstance<Shelter.ShelterMapSO>();
            l.layoutId = "layout_gas_station";
            l.layoutName = "Abandoned Gas Station";
            l.description = "The underground fuel reservoir. It still smells like gasoline. " +
                "The walls are slick with residue. One spark and the whole place goes up — " +
                "but there's enough fuel down here to run a generator for months.";
            l.roomCount = 2;
            l.roomIds = new[] { "main_tank", "pump_room" };
            l.roomNames = new[] { "Main Tank", "Pump Room" };
            l.roomSizes = new[] { 2f, 1f };
            l.startingModules = new Shelter.ShelterLayoutModule[]
            {
                new Shelter.ShelterLayoutModule { moduleId = "bed", level = 1, roomId = "main_tank" },
                new Shelter.ShelterLayoutModule { moduleId = "workbench", level = 1, roomId = "pump_room" }
            };
            l.traits = new[] { Shelter.ShelterLayoutTrait.PrepperCache };
            l.debrisType = Shelter.DebrisType.MetalDebris;
            l.startingDebrisHours = 6f;
            l.inherentShielding = 0.05f;
            l.debrisShieldingBonus = 0.1f;
            l.startingCleanWater = 0f;
            l.startingHouseDurability = 50f;
            l.startingIntegrity = 70f;
            l.startingHatchSecurity = 8f;
            l.anomalies = new[] { "toxic_fumes", "fire_risk" };
            return l;
        }

        public static Shelter.ShelterMapSO CreateBankVault()
        {
            var l = ScriptableObject.CreateInstance<Shelter.ShelterMapSO>();
            l.layoutId = "layout_bank_vault";
            l.layoutName = "Bank Vault";
            l.description = "The vault door is three feet of steel. Nothing gets through it — " +
                "not radiation, not raiders, not the blast wave. But nothing gets out either. " +
                "No ventilation. No airflow. Every breath consumed is gone forever. " +
                "You can't keep the door shut forever.";
            l.roomCount = 1;
            l.roomIds = new[] { "vault" };
            l.roomNames = new[] { "The Vault" };
            l.roomSizes = new[] { 3f };
            l.startingModules = new Shelter.ShelterLayoutModule[]
            {
                new Shelter.ShelterLayoutModule { moduleId = "bed", level = 2, roomId = "vault" },
                new Shelter.ShelterLayoutModule { moduleId = "radiation_shielding", level = 5, roomId = "vault" }
            };
            l.traits = new[] { Shelter.ShelterLayoutTrait.DeepUnderground };
            l.debrisType = Shelter.DebrisType.ConcreteRubble;
            l.startingDebrisHours = 4f;
            l.inherentShielding = 0.9f;
            l.debrisShieldingBonus = 0.05f;
            l.startingCleanWater = 5f;
            l.startingHouseDurability = 200f;
            l.startingIntegrity = 100f;
            l.startingHatchSecurity = 100f;
            l.anomalies = new[] { "no_ventilation", "vault_door_open" };
            return l;
        }

        public static Shelter.ShelterMapSO CreateSewerShaft()
        {
            var l = ScriptableObject.CreateInstance<Shelter.ShelterMapSO>();
            l.layoutId = "layout_sewer";
            l.layoutName = "Sewer Maintenance Shaft";
            l.description = "A maintenance access point in the old city sewer system. " +
                "The mold predates the war. The rats have organized. But the tunnels " +
                "go for miles, and raiders can't find what they can't see from the surface.";
            l.roomCount = 3;
            l.roomIds = new[] { "shaft_room", "pipe_gallery", "overflow_chamber" };
            l.roomNames = new[] { "Shaft Room", "Pipe Gallery", "Overflow Chamber" };
            l.roomSizes = new[] { 1f, 2f, 1.5f };
            l.startingModules = new Shelter.ShelterLayoutModule[]
            {
                new Shelter.ShelterLayoutModule { moduleId = "bed", level = 1, roomId = "shaft_room" },
                new Shelter.ShelterLayoutModule { moduleId = "workbench", level = 1, roomId = "pipe_gallery" }
            };
            l.traits = new[] { Shelter.ShelterLayoutTrait.DirtFloor };
            l.debrisType = Shelter.DebrisType.Dirt;
            l.startingDebrisHours = 8f;
            l.inherentShielding = 0.2f;
            l.debrisShieldingBonus = 0.05f;
            l.startingCleanWater = 10f;
            l.startingHouseDurability = 100f;
            l.startingIntegrity = 80f;
            l.startingHatchSecurity = 25f;
            l.anomalies = new[] { "unbreakable_mold", "pest_infested", "deep_tunnel_access" };
            return l;
        }

        public static Shelter.ShelterMapSO CreateTechBunker()
        {
            var l = ScriptableObject.CreateInstance<Shelter.ShelterMapSO>();
            l.layoutId = "layout_tech_bunker";
            l.layoutName = "Tech-Bunker";
            l.description = "A pre-war smart-home enthusiast's paranoid dream. Automated doors, " +
                "centralized climate control, a water recycler that still hums. Then the EMP " +
                "hit. Now the doors open and close on their own. The intercom plays static " +
                "at 3 AM. The bunker is alive — and it doesn't recognize you.";
            l.roomCount = 4;
            l.roomIds = new[] { "control_room", "living_quarters", "utility_closet", "server_room" };
            l.roomNames = new[] { "Control Room", "Living Quarters", "Utility Closet", "Server Room" };
            l.roomSizes = new[] { 1.5f, 2f, 0.5f, 1f };
            l.startingModules = new Shelter.ShelterLayoutModule[]
            {
                new Shelter.ShelterLayoutModule { moduleId = "air_filtration", level = 3, roomId = "utility_closet", filterHealth = 90f },
                new Shelter.ShelterLayoutModule { moduleId = "water_purifier", level = 2, roomId = "utility_closet", filterHealth = 85f },
                new Shelter.ShelterLayoutModule { moduleId = "bed", level = 2, roomId = "living_quarters" },
                new Shelter.ShelterLayoutModule { moduleId = "workbench", level = 1, roomId = "control_room" }
            };
            l.traits = new[] { Shelter.ShelterLayoutTrait.PrepperCache };
            l.debrisType = Shelter.DebrisType.MetalDebris;
            l.startingDebrisHours = 2f;
            l.inherentShielding = 0.15f;
            l.debrisShieldingBonus = 0.1f;
            l.startingCleanWater = 30f;
            l.startingHouseDurability = 150f;
            l.startingIntegrity = 90f;
            l.startingHatchSecurity = 15f;
            l.anomalies = new[] { "emp_glitched_doors", "hostile_ai" };
            return l;
        }

        public static Shelter.ShelterMapSO CreateShippingContainers()
        {
            var l = ScriptableObject.CreateInstance<Shelter.ShelterMapSO>();
            l.layoutId = "layout_containers";
            l.layoutName = "Shipping Containers";
            l.description = "Two forty-foot shipping containers, buried side by side and welded " +
                "together at the seam. It's not pretty — rust is already eating the corners — " +
                "but it's dry, it's hidden, and the corrugated steel walls muffle sound. " +
                "Just don't let the humidity get above fifty percent.";
            l.roomCount = 2;
            l.roomIds = new[] { "container_a", "container_b" };
            l.roomNames = new[] { "Container A", "Container B" };
            l.roomSizes = new[] { 1f, 1f };
            l.startingModules = new Shelter.ShelterLayoutModule[]
            {
                new Shelter.ShelterLayoutModule { moduleId = "bed", level = 1, roomId = "container_a" },
                new Shelter.ShelterLayoutModule { moduleId = "workbench", level = 1, roomId = "container_b" }
            };
            l.traits = new[] { Shelter.ShelterLayoutTrait.DirtFloor };
            l.debrisType = Shelter.DebrisType.MetalDebris;
            l.startingDebrisHours = 2f;
            l.inherentShielding = 0.02f;
            l.debrisShieldingBonus = 0f;
            l.startingCleanWater = 5f;
            l.startingHouseDurability = 30f;
            l.startingIntegrity = 40f;
            l.integrityDegradeMultiplier = 2f;
            l.startingHatchSecurity = 5f;
            l.anomalies = new[] { "rapid_rust", "low_ceiling", "thin_walls" };
            return l;
        }

        /// <summary>All ten layouts.</summary>
        public static List<Shelter.ShelterMapSO> CreateAll()
        {
            return new List<Shelter.ShelterMapSO>
            {
                CreateSuburbanSplitLevel(),
                CreateRuralFarmhouse(),
                CreateUrbanBrownstone(),
                CreatePrepperCabin(),
                CreateMakeshiftMetro(),
                CreateGasStation(),
                CreateBankVault(),
                CreateSewerShaft(),
                CreateTechBunker(),
                CreateShippingContainers()
            };
        }
    }
}
