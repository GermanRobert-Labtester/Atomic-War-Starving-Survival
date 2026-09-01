using System;
using System.Collections.Generic;
using Ashfall.Core.IO;

namespace Ashfall.Core.Shelter
{
    [Serializable]
    public sealed class ShelterRoomCostDef
    {
        public string item_id { get; set; } = string.Empty;
        public int quantity { get; set; } = 1;
    }

    /// <summary>
    /// Static authored definition for a shelter room type.
    /// </summary>
    [Serializable]
    public sealed class ShelterRoomDef
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string function { get; set; } = "General"; // Dormitory, Workshop, MedicalBay, Kitchen, Storage, Greenhouse, RadioRoom, Armory, Laboratory, CommonArea, Airlock, GeneratorRoom, FiltrationStack, Corridor
        public int capacity { get; set; } = 2;
        public int max_upgrade_level { get; set; } = 3;
        public string required_skill_id { get; set; } = string.Empty;
        public string workstation_id { get; set; } = string.Empty;
        public float base_condition { get; set; } = 100.0f;
        public List<ShelterRoomCostDef> build_cost { get; set; } = new List<ShelterRoomCostDef>();
        public List<ShelterRoomCostDef> repair_cost { get; set; } = new List<ShelterRoomCostDef>();
        public List<string> tags { get; set; } = new List<string>();
    }

    /// <summary>
    /// Static authored definition for an assignment rule governing survivor fit.
    /// </summary>
    [Serializable]
    public sealed class ShelterAssignmentRuleDef
    {
        public string id { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string target_room_function { get; set; } = string.Empty;
        public string required_skill_id { get; set; } = string.Empty;
        public string bonus_type { get; set; } = "efficiency";
        public float bonus_magnitude { get; set; } = 0.20f;
        public float penalty_magnitude { get; set; } = -0.10f;
        public bool is_hard_gate { get; set; } = false;
    }

    /// <summary>
    /// Container for shelter_rooms.json catalog authority.
    /// </summary>
    [Serializable]
    public sealed class ShelterRoomCatalogContainer
    {
        public int schema_version { get; set; } = 1;
        public string collection_id { get; set; } = "shelter_rooms";
        public List<ShelterRoomDef> rooms { get; set; } = new List<ShelterRoomDef>();
        public List<ShelterAssignmentRuleDef> assignment_rules { get; set; } = new List<ShelterAssignmentRuleDef>();
    }

    /// <summary>
    /// Authority loader for shelter room definitions and assignment rules.
    /// </summary>
    public static class ShelterRoomCatalogLoader
    {
        public const string CatalogFileName = "shelter_rooms.json";

        public static ShelterRoomCatalogContainer Load(string dataDir, IFileIO? fileIO = null, IJsonSerializer? serializer = null)
        {
            fileIO ??= new FileSystemIO();
            serializer ??= new SystemTextJsonSerializer();

            if (string.IsNullOrEmpty(dataDir))
                return GetDefaultCatalog();

            string fullPath = fileIO.Combine(dataDir, CatalogFileName);
            if (!fileIO.FileExists(fullPath))
                return GetDefaultCatalog();

            try
            {
                string json = fileIO.ReadAllText(fullPath);
                var container = serializer.Deserialize<ShelterRoomCatalogContainer>(json);
                if (container != null && container.rooms.Count > 0)
                    return container;
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(fullPath, "ShelterRoomCatalogContainer", ex);
            }

            return GetDefaultCatalog();
        }

        public static ShelterRoomCatalogContainer GetDefaultCatalog()
        {
            return new ShelterRoomCatalogContainer
            {
                schema_version = 1,
                collection_id = "shelter_rooms",
                rooms = GetDefaultRooms(),
                assignment_rules = GetDefaultRules()
            };
        }

        public static List<ShelterRoomDef> GetDefaultRooms()
        {
            return new List<ShelterRoomDef>
            {
                new ShelterRoomDef
                {
                    id = "room_bunker_corridor",
                    display_name = "Central Access Corridor",
                    description = "Access concourse and structural spine connecting bunker sectors, hatchways, and stairwells.",
                    function = "Corridor",
                    capacity = 0,
                    max_upgrade_level = 3,
                    tags = new List<string> { "spine", "circulation" },
                    build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 4 } },
                    repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 1 } }
                },
                new ShelterRoomDef
                {
                    id = "room_bunks_crowded",
                    display_name = "Crowded Bunkhouse",
                    description = "Triple-tiered pipe bunks packed wall-to-wall for maximum occupancy at the cost of privacy and comfort.",
                    function = "Dormitory",
                    capacity = 6,
                    max_upgrade_level = 2,
                    tags = new List<string> { "residential", "high_density" },
                    build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_wood", quantity = 6 }, new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 4 } },
                    repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_wood", quantity = 2 } }
                },
                new ShelterRoomDef
                {
                    id = "room_bunks",
                    display_name = "Standard Dormitory",
                    description = "Double-tiered steel bunk frames bolted to concrete slab with personal locker space.",
                    function = "Dormitory",
                    capacity = 4,
                    max_upgrade_level = 3,
                    tags = new List<string> { "residential", "standard" },
                    build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_wood", quantity = 4 }, new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 6 } },
                    repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 2 } }
                },
                new ShelterRoomDef
                {
                    id = "room_quarters_private",
                    display_name = "Partitioned Quarters",
                    description = "Acoustically insulated private sleeping cubicles providing restorative rest and privacy.",
                    function = "Dormitory",
                    capacity = 2,
                    max_upgrade_level = 3,
                    tags = new List<string> { "residential", "comfort" },
                    build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_wood", quantity = 8 }, new ShelterRoomCostDef { item_id = "cloth", quantity = 4 } },
                    repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_wood", quantity = 2 }, new ShelterRoomCostDef { item_id = "cloth", quantity = 1 } }
                },
                new ShelterRoomDef
                {
                    id = "room_workshop",
                    display_name = "General Workshop",
                    description = "Fabrication benches, vise clamps, and hand tools for general repairs and scrap repurposing.",
                    function = "Workshop",
                    capacity = 2,
                    max_upgrade_level = 3,
                    required_skill_id = "skill_rough_repairs",
                    tags = new List<string> { "crafting", "repair" },
                    build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 8 }, new ShelterRoomCostDef { item_id = "mechanical_parts", quantity = 4 } },
                    repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 3 } }
                },
                new ShelterRoomDef
                {
                    id = "room_workshop_heavy",
                    display_name = "Heavy Machinery Workshop",
                    description = "Reinforced flooring, overhead crane hoist, and hydraulic presses for engine overhauls.",
                    function = "Workshop",
                    capacity = 2,
                    max_upgrade_level = 3,
                    required_skill_id = "skill_workshop_sense",
                    tags = new List<string> { "crafting", "heavy_industrial" },
                    build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 12 }, new ShelterRoomCostDef { item_id = "heavy_industrial_motor", quantity = 1 } },
                    repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 4 } }
                },
                new ShelterRoomDef
                {
                    id = "room_workshop_precision",
                    display_name = "Precision Tooling Bench",
                    description = "Clean room environment with jeweler loupes, soldering irons, and precision instruments.",
                    function = "Workshop",
                    capacity = 2,
                    max_upgrade_level = 3,
                    required_skill_id = "skill_workshop_sense",
                    tags = new List<string> { "crafting", "precision" },
                    build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 6 }, new ShelterRoomCostDef { item_id = "scrap_electronic", quantity = 4 } },
                    repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_electronic", quantity = 2 } }
                },
                new ShelterRoomDef
                {
                    id = "room_clinic",
                    display_name = "Field Clinic",
                    description = "Emergency triage tables, sterile dressings, and disinfectant wash for treating trauma.",
                    function = "MedicalBay",
                    capacity = 2,
                    max_upgrade_level = 3,
                    required_skill_id = "skill_field_dressing",
                    tags = new List<string> { "medical", "triage" },
                    build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_wood", quantity = 4 }, new ShelterRoomCostDef { item_id = "cloth", quantity = 6 } },
                    repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "cloth", quantity = 2 } }
                },
                new ShelterRoomDef
                {
                    id = "room_ward_clinical",
                    display_name = "Clinical Surgical Ward",
                    description = "Sealed operating bay with surgical lighting, oxygen manifold, and surgical instruments.",
                    function = "MedicalBay",
                    capacity = 2,
                    max_upgrade_level = 3,
                    required_skill_id = "skill_steady_hands",
                    tags = new List<string> { "medical", "surgery" },
                    build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 8 }, new ShelterRoomCostDef { item_id = "chemicals", quantity = 4 } },
                    repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "chemicals", quantity = 2 } }
                },
                new ShelterRoomDef
                {
                    id = "room_ward_quarantine",
                    display_name = "Isolation Quarantine Bay",
                    description = "Negative-pressure containment cell with ultraviolet sanitization for infectious outbreaks.",
                    function = "MedicalBay",
                    capacity = 2,
                    max_upgrade_level = 3,
                    required_skill_id = "skill_field_dressing",
                    tags = new List<string> { "medical", "quarantine" },
                    build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 6 }, new ShelterRoomCostDef { item_id = "cloth", quantity = 4 } },
                    repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 2 } }
                },
                new ShelterRoomDef
                {
                    id = "room_kitchen",
                    display_name = "Galley Kitchen",
                    description = "Three-kettle stove, butchering block, and ration preparation counter with exhaust flue.",
                    function = "Kitchen",
                    capacity = 2,
                    max_upgrade_level = 3,
                    required_skill_id = "skill_ration_stretcher",
                    tags = new List<string> { "nutrition", "canteen" },
                    build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 6 }, new ShelterRoomCostDef { item_id = "scrap_wood", quantity = 4 } },
                    repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 2 } }
                },
                new ShelterRoomDef
                {
                    id = "room_storage_bay",
                    display_name = "General Storage Bay",
                    description = "Banded wooden shelving and pallet racks for dry rations, raw scrap, and tools.",
                    function = "Storage",
                    capacity = 1,
                    max_upgrade_level = 3,
                    required_skill_id = "skill_quartermaster",
                    tags = new List<string> { "logistics", "general" },
                    build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_wood", quantity = 8 } },
                    repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_wood", quantity = 2 } }
                },
                new ShelterRoomDef
                {
                    id = "room_storage_secure",
                    display_name = "Reinforced Armored Vault",
                    description = "Heavy blast-lock room for secure caching of firearms, ammunition, and rare medical isotopes.",
                    function = "Storage",
                    capacity = 1,
                    max_upgrade_level = 3,
                    required_skill_id = "skill_watchful",
                    tags = new List<string> { "logistics", "secure" },
                    build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 12 } },
                    repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 3 } }
                },
                new ShelterRoomDef
                {
                    id = "room_greenhouse_shelter",
                    display_name = "Subterranean Greenhouse",
                    description = "Tiered hydroponic growth trays under full-spectrum sodium lamps fed from filtered water.",
                    function = "Greenhouse",
                    capacity = 2,
                    max_upgrade_level = 3,
                    required_skill_id = "skill_mycology",
                    tags = new List<string> { "agriculture", "hydroponics" },
                    build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 8 }, new ShelterRoomCostDef { item_id = "chemicals", quantity = 4 } },
                    repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "chemicals", quantity = 2 } }
                },
                new ShelterRoomDef
                {
                    id = "room_radio_tuner",
                    display_name = "Radio Communications Bay",
                    description = "Shortwave tuner rack, antenna lead-in, and signal decoding station for monitoring the wasteland.",
                    function = "RadioRoom",
                    capacity = 1,
                    max_upgrade_level = 3,
                    required_skill_id = "skill_signal_ear",
                    tags = new List<string> { "communications", "intel" },
                    build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 4 }, new ShelterRoomCostDef { item_id = "scrap_electronic", quantity = 4 } },
                    repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_electronic", quantity = 2 } }
                },
                new ShelterRoomDef
                {
                    id = "room_armory_munitions",
                    display_name = "Armory & Munitions Depot",
                    description = "Rifle lockers, cleaning solvent basins, and a reloading press for ammunition refits.",
                    function = "Armory",
                    capacity = 1,
                    max_upgrade_level = 3,
                    required_skill_id = "skill_watchful",
                    tags = new List<string> { "combat", "defense" },
                    build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 10 }, new ShelterRoomCostDef { item_id = "mechanical_parts", quantity = 4 } },
                    repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 3 } }
                },
                new ShelterRoomDef
                {
                    id = "room_laboratory_research",
                    display_name = "Science & Research Lab",
                    description = "Centrifuges, distillation tubes, and analytical charts for cataloging pre-war engineering archives.",
                    function = "Laboratory",
                    capacity = 2,
                    max_upgrade_level = 3,
                    required_skill_id = "skill_cold_analysis",
                    tags = new List<string> { "research", "science" },
                    build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 8 }, new ShelterRoomCostDef { item_id = "scrap_electronic", quantity = 6 } },
                    repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_electronic", quantity = 2 } }
                },
                new ShelterRoomDef
                {
                    id = "room_common_mess_hall",
                    display_name = "Communal Mess Hall",
                    description = "Long communal benches and notice board where survivors gather for meals, meetings, and morale.",
                    function = "CommonArea",
                    capacity = 4,
                    max_upgrade_level = 3,
                    tags = new List<string> { "social", "morale" },
                    build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_wood", quantity = 10 } },
                    repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_wood", quantity = 2 } }
                },
                new ShelterRoomDef
                {
                    id = "room_reading_quiet_room",
                    display_name = "Quiet Archive & Study",
                    description = "A quiet reading nook insulated from generator vibration for decompression and technical manual study.",
                    function = "CommonArea",
                    capacity = 2,
                    max_upgrade_level = 2,
                    tags = new List<string> { "social", "study" },
                    build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_wood", quantity = 6 }, new ShelterRoomCostDef { item_id = "cloth", quantity = 2 } },
                    repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_wood", quantity = 2 } }
                },
                new ShelterRoomDef
                {
                    id = "room_airlock",
                    display_name = "Decontamination Airlock",
                    description = "Double airtight blast hatch with chemical decontam sprayers and dosimeter staging rack.",
                    function = "Airlock",
                    capacity = 2,
                    max_upgrade_level = 3,
                    required_skill_id = "skill_field_dressing",
                    tags = new List<string> { "perimeter", "expedition" },
                    build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 10 }, new ShelterRoomCostDef { item_id = "mechanical_parts", quantity = 4 } },
                    repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 3 } }
                },
                new ShelterRoomDef
                {
                    id = "room_generator",
                    display_name = "Primary Diesel Generator",
                    description = "Heavy industrial diesel dynamo providing electrical power across main lighting and pump circuits.",
                    function = "GeneratorRoom",
                    capacity = 2,
                    max_upgrade_level = 3,
                    required_skill_id = "skill_rough_repairs",
                    tags = new List<string> { "power", "infrastructure" },
                    build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 12 }, new ShelterRoomCostDef { item_id = "fuel", quantity = 2 } },
                    repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 4 } }
                },
                new ShelterRoomDef
                {
                    id = "room_filtration",
                    display_name = "Filtration & Scrubber Stack",
                    description = "HEPA filter banks and activated charcoal canisters providing clean breathable air to the shelter.",
                    function = "FiltrationStack",
                    capacity = 1,
                    max_upgrade_level = 3,
                    required_skill_id = "skill_rough_repairs",
                    tags = new List<string> { "life_support", "infrastructure" },
                    build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 8 }, new ShelterRoomCostDef { item_id = "cloth", quantity = 4 } },
                    repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "cloth", quantity = 2 } }
                }
            };
        }

        public static List<ShelterAssignmentRuleDef> GetDefaultRules()
        {
            return new List<ShelterAssignmentRuleDef>
            {
                new ShelterAssignmentRuleDef
                {
                    id = "rule_medical_field_surgery",
                    name = "Triage & Field Medicine",
                    description = "Medically trained survivors accelerate wound recovery and reduce medical supply consumption.",
                    target_room_function = "MedicalBay",
                    required_skill_id = "skill_field_dressing",
                    bonus_type = "treatment_efficiency",
                    bonus_magnitude = 0.25f,
                    penalty_magnitude = -0.10f
                },
                new ShelterAssignmentRuleDef
                {
                    id = "rule_workshop_machinist",
                    name = "Machinist & Maintenance",
                    description = "Skilled mechanics improve structural repair speed and minimize part wear.",
                    target_room_function = "Workshop",
                    required_skill_id = "skill_rough_repairs",
                    bonus_type = "repair_speed",
                    bonus_magnitude = 0.20f,
                    penalty_magnitude = -0.05f
                },
                new ShelterAssignmentRuleDef
                {
                    id = "rule_workshop_precision",
                    name = "Precision Tooling Focus",
                    description = "Deep workshop sense boosts yield and crafting reliability on high-tier items.",
                    target_room_function = "Workshop",
                    required_skill_id = "skill_workshop_sense",
                    bonus_type = "crafting_yield",
                    bonus_magnitude = 0.20f,
                    penalty_magnitude = -0.05f
                },
                new ShelterAssignmentRuleDef
                {
                    id = "rule_radio_communications",
                    name = "Radio Signal Processing",
                    description = "Trained signal operators discern faint broadcasts through background ionospheric noise.",
                    target_room_function = "RadioRoom",
                    required_skill_id = "skill_signal_ear",
                    bonus_type = "signal_clarity",
                    bonus_magnitude = 0.25f,
                    penalty_magnitude = -0.10f
                },
                new ShelterAssignmentRuleDef
                {
                    id = "rule_kitchen_nutrition",
                    name = "Canteen Ration Mastery",
                    description = "Skilled cooks stretch limited calories without sacrificing survivor nutrition or morale.",
                    target_room_function = "Kitchen",
                    required_skill_id = "skill_ration_stretcher",
                    bonus_type = "meal_efficiency",
                    bonus_magnitude = 0.25f,
                    penalty_magnitude = -0.10f
                },
                new ShelterAssignmentRuleDef
                {
                    id = "rule_laboratory_analysis",
                    name = "Scientific Analysis",
                    description = "Disciplined researchers accelerate knowledge decoding from recovered technical archives.",
                    target_room_function = "Laboratory",
                    required_skill_id = "skill_cold_analysis",
                    bonus_type = "research_speed",
                    bonus_magnitude = 0.30f,
                    penalty_magnitude = -0.10f
                },
                new ShelterAssignmentRuleDef
                {
                    id = "rule_greenhouse_botany",
                    name = "Hydroponic Cultivation",
                    description = "Hardy growers optimize nutrient feeds to boost harvest yields in artificial lighting.",
                    target_room_function = "Greenhouse",
                    required_skill_id = "skill_mycology",
                    bonus_type = "harvest_yield",
                    bonus_magnitude = 0.25f,
                    penalty_magnitude = -0.05f
                },
                new ShelterAssignmentRuleDef
                {
                    id = "rule_generator_maintenance",
                    name = "Turbine & Power Tuning",
                    description = "Experienced engineers reduce fuel consumption and prevent unexpected grid brownouts.",
                    target_room_function = "GeneratorRoom",
                    required_skill_id = "skill_rough_repairs",
                    bonus_type = "fuel_efficiency",
                    bonus_magnitude = 0.20f,
                    penalty_magnitude = -0.10f
                },
                new ShelterAssignmentRuleDef
                {
                    id = "rule_armory_service",
                    name = "Armory Maintenance",
                    description = "Watchful armorers keep firearms cleaned and expedition equipment primed.",
                    target_room_function = "Armory",
                    required_skill_id = "skill_watchful",
                    bonus_type = "weapon_maintenance",
                    bonus_magnitude = 0.20f,
                    penalty_magnitude = -0.05f
                },
                new ShelterAssignmentRuleDef
                {
                    id = "rule_storage_logistics",
                    name = "Logistics Organization",
                    description = "Organized quartermasters streamline inventory handling and reduce spoilage.",
                    target_room_function = "Storage",
                    required_skill_id = "skill_quartermaster",
                    bonus_type = "handling_speed",
                    bonus_magnitude = 0.15f,
                    penalty_magnitude = -0.05f
                },
                new ShelterAssignmentRuleDef
                {
                    id = "rule_airlock_decontamination",
                    name = "Airlock Protocol",
                    description = "Proper decontamination procedures accelerate expedition turnaround and scrub fallout.",
                    target_room_function = "Airlock",
                    required_skill_id = "skill_field_dressing",
                    bonus_type = "turnaround_speed",
                    bonus_magnitude = 0.20f,
                    penalty_magnitude = -0.05f
                },
                new ShelterAssignmentRuleDef
                {
                    id = "rule_dormitory_caretaker",
                    name = "Shelter Caretaking",
                    description = "Attentive caretakers maintain clean living quarters, improving rest quality and morale.",
                    target_room_function = "Dormitory",
                    required_skill_id = "skill_hard_living",
                    bonus_type = "rest_quality",
                    bonus_magnitude = 0.15f,
                    penalty_magnitude = 0.0f
                }
            };
        }
    }
}
