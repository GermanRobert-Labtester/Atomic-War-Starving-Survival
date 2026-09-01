using System;
using System.Collections.Generic;
using System.IO;

namespace Ashfall.Core.Excavation
{
    /// <summary>
    /// Schema container for excavation_sites.json catalog.
    /// </summary>
    [Serializable]
    public sealed class ExcavationCatalogContainer
    {
        public int schema_version { get; set; } = 1;
        public List<ExcavationSiteDef> sites { get; set; } = new List<ExcavationSiteDef>();
    }

    /// <summary>
    /// DTO defining an authored deep-strata excavation site.
    /// </summary>
    [Serializable]
    public sealed class ExcavationSiteDef
    {
        public string site_id { get; set; } = string.Empty;
        public string location_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public float max_depth_meters { get; set; } = 100f;
        public float required_progress { get; set; } = 100f;
        public float structural_risk { get; set; } = 0.3f;
        public List<string> required_tools { get; set; } = new List<string>();
        public List<string> shoring_materials { get; set; } = new List<string>();
        public string hazard_type { get; set; } = string.Empty;
        public string relic_reward_id { get; set; } = string.Empty;
        public string loot_table { get; set; } = "salvage_common";
        public List<ExcavationDepthBandDef> depth_bands { get; set; } = new List<ExcavationDepthBandDef>();
        public string journal_entry_id { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO defining an individual depth stratum / band.
    /// </summary>
    [Serializable]
    public sealed class ExcavationDepthBandDef
    {
        public float depth_meters { get; set; }
        public string label { get; set; } = string.Empty;
        public float risk { get; set; }
    }

    /// <summary>
    /// Loader and query authority for authored deep-strata excavation sites.
    /// </summary>
    public static class ExcavationCatalogLoader
    {
        public const string CatalogFileName = "excavation_sites.json";

        public static List<ExcavationSiteDef> Load(string dataDir, IFileIO? fileIO = null, IJsonSerializer? serializer = null)
        {
            fileIO ??= new FileSystemIO();
            serializer ??= new SystemTextJsonSerializer();

            string fullPath = Path.Combine(dataDir, CatalogFileName);
            if (!fileIO.FileExists(fullPath))
            {
                return GetDefaultSites();
            }

            try
            {
                string json = fileIO.ReadAllText(fullPath);
                var container = serializer.Deserialize<ExcavationCatalogContainer>(json);
                if (container != null && container.sites != null && container.sites.Count > 0)
                {
                    return container.sites;
                }
            }
            catch
            {
                // Fallback to built-in defaults if JSON parse fails
            }

            return GetDefaultSites();
        }

        public static List<ExcavationSiteDef> GetDefaultSites()
        {
            return new List<ExcavationSiteDef>
            {
                new ExcavationSiteDef
                {
                    site_id = "excavation_command_vault",
                    location_id = "loc_excavation_command_vault",
                    display_name = "Collapsed Civil Defense Command Vault",
                    description = "A buried regional continuity command bunker constructed to survive strategic nuclear strikes. Blast damage collapsed the surface approach, leaving deeper operational rooms partially pressurized.",
                    max_depth_meters = 120f,
                    required_progress = 160f,
                    structural_risk = 0.38f,
                    required_tools = new List<string> { "tools_precision", "shovel" },
                    shoring_materials = new List<string> { "scrap_metal", "mechanical_parts" },
                    hazard_type = "hazard_radiation_hotspot",
                    relic_reward_id = "item_comm_codebook_alpha",
                    loot_table = "salvage_rare",
                    journal_entry_id = "journal_excavation_command_vault",
                    depth_bands = new List<ExcavationDepthBandDef>
                    {
                        new ExcavationDepthBandDef { depth_meters = 20f, label = "Cratered Rubble Approach", risk = 0.25f },
                        new ExcavationDepthBandDef { depth_meters = 55f, label = "Collapsed Service Conduit", risk = 0.38f },
                        new ExcavationDepthBandDef { depth_meters = 90f, label = "Reinforced Command Shell", risk = 0.28f },
                        new ExcavationDepthBandDef { depth_meters = 120f, label = "Sealed Operations Center", risk = 0.52f }
                    }
                },
                new ExcavationSiteDef
                {
                    site_id = "excavation_utility_tunnels",
                    location_id = "loc_excavation_utility_tunnels",
                    display_name = "Utility Tunnel Network",
                    description = "Subterranean municipal service corridors and conduit trunks running beneath the old district grid. Flooded sections and silted pipes conceal pre-war electrical salvage and emergency maintenance tools.",
                    max_depth_meters = 65f,
                    required_progress = 95f,
                    structural_risk = 0.22f,
                    required_tools = new List<string> { "tools_precision", "wrench" },
                    shoring_materials = new List<string> { "scrap_wood", "metal_sheet" },
                    hazard_type = "hazard_flood",
                    relic_reward_id = "tools_precision",
                    loot_table = "salvage_common",
                    journal_entry_id = "journal_excavation_utility_tunnels",
                    depth_bands = new List<ExcavationDepthBandDef>
                    {
                        new ExcavationDepthBandDef { depth_meters = 15f, label = "Service Access Hatch", risk = 0.15f },
                        new ExcavationDepthBandDef { depth_meters = 38f, label = "Flooded Conduit Junction", risk = 0.26f },
                        new ExcavationDepthBandDef { depth_meters = 65f, label = "Main Infrastructure Sump", risk = 0.35f }
                    }
                },
                new ExcavationSiteDef
                {
                    site_id = "excavation_metro_interchange",
                    location_id = "loc_excavation_metro_interchange",
                    display_name = "Buried Metro Interchange",
                    description = "A multi-tier transit nexus crushed under collapsed concrete avenues. Damp platform recesses foster spore mold colonies among abandoned commuter baggage and maintenance gear.",
                    max_depth_meters = 105f,
                    required_progress = 145f,
                    structural_risk = 0.42f,
                    required_tools = new List<string> { "shovel", "crowbar" },
                    shoring_materials = new List<string> { "steel_columns", "hydraulic_jack" },
                    hazard_type = "hazard_spore_mold",
                    relic_reward_id = "item_logistics_cipher_sheet",
                    loot_table = "salvage_rare",
                    journal_entry_id = "journal_excavation_metro_interchange",
                    depth_bands = new List<ExcavationDepthBandDef>
                    {
                        new ExcavationDepthBandDef { depth_meters = 25f, label = "Collapsed Ticket Concourse", risk = 0.30f },
                        new ExcavationDepthBandDef { depth_meters = 55f, label = "Sub-Platform Mezzanine", risk = 0.42f },
                        new ExcavationDepthBandDef { depth_meters = 80f, label = "Mold-Choked Track Tunnel", risk = 0.50f },
                        new ExcavationDepthBandDef { depth_meters = 105f, label = "Sealed Express Dispatch Platform", risk = 0.58f }
                    }
                },
                new ExcavationSiteDef
                {
                    site_id = "excavation_mine_shaft",
                    location_id = "loc_excavation_mine_shaft",
                    display_name = "Industrial Mine Shaft Adit 4",
                    description = "A deep extraction adit bored into fractured granitic bedrock. Rotting timber supports and pockets of trapped methane create extreme cave-in hazards protecting rich industrial machinery.",
                    max_depth_meters = 150f,
                    required_progress = 190f,
                    structural_risk = 0.55f,
                    required_tools = new List<string> { "pickaxe", "hydraulic_jack" },
                    shoring_materials = new List<string> { "timber_beams", "hydraulic_jack" },
                    hazard_type = "hazard_methane_pocket",
                    relic_reward_id = "heavy_industrial_motor",
                    loot_table = "salvage_rare",
                    journal_entry_id = "journal_excavation_mine_shaft",
                    depth_bands = new List<ExcavationDepthBandDef>
                    {
                        new ExcavationDepthBandDef { depth_meters = 30f, label = "Weathered Mine Collar", risk = 0.32f },
                        new ExcavationDepthBandDef { depth_meters = 70f, label = "Fractured Gallery Level 1", risk = 0.55f },
                        new ExcavationDepthBandDef { depth_meters = 115f, label = "Timbered Drift Level 2", risk = 0.65f },
                        new ExcavationDepthBandDef { depth_meters = 150f, label = "Flooded Ore Sump & Machine Pocket", risk = 0.75f }
                    }
                },
                new ExcavationSiteDef
                {
                    site_id = "excavation_archive_bunker",
                    location_id = "loc_excavation_archive_bunker",
                    display_name = "Sealed Archive Bunker",
                    description = "A climate-isolated subterranean depository housing pre-war technological patents and civil registries. Damp air locks hold aggressive spore mold around hardened vault vaults.",
                    max_depth_meters = 85f,
                    required_progress = 135f,
                    structural_risk = 0.32f,
                    required_tools = new List<string> { "tools_precision", "crowbar" },
                    shoring_materials = new List<string> { "reinforced_arches", "scrap_metal" },
                    hazard_type = "hazard_spore_mold",
                    relic_reward_id = "item_archive_index_cylinder",
                    loot_table = "salvage_rare",
                    journal_entry_id = "journal_excavation_archive_bunker",
                    depth_bands = new List<ExcavationDepthBandDef>
                    {
                        new ExcavationDepthBandDef { depth_meters = 15f, label = "Debris Trench Access", risk = 0.18f },
                        new ExcavationDepthBandDef { depth_meters = 45f, label = "Microfilm Catalog Ante-Chamber", risk = 0.32f },
                        new ExcavationDepthBandDef { depth_meters = 85f, label = "Master Climate Vault", risk = 0.46f }
                    }
                },
                new ExcavationSiteDef
                {
                    site_id = "excavation_drainage_network",
                    location_id = "loc_excavation_drainage_network",
                    display_name = "Drainage Network Sluice 09",
                    description = "Stormwater culverts and overflow sluices converted into illicit smuggling routes before the bombardment. Silt and contaminated backwash hide sealed waterproof caches.",
                    max_depth_meters = 45f,
                    required_progress = 75f,
                    structural_risk = 0.20f,
                    required_tools = new List<string> { "shovel", "wrench" },
                    shoring_materials = new List<string> { "scrap_wood", "scrap_metal" },
                    hazard_type = "hazard_flood",
                    relic_reward_id = "water_filter",
                    loot_table = "salvage_common",
                    journal_entry_id = "journal_excavation_drainage_network",
                    depth_bands = new List<ExcavationDepthBandDef>
                    {
                        new ExcavationDepthBandDef { depth_meters = 12f, label = "Culvert Silt Layer", risk = 0.12f },
                        new ExcavationDepthBandDef { depth_meters = 28f, label = "Cracked Service Channel", risk = 0.22f },
                        new ExcavationDepthBandDef { depth_meters = 45f, label = "Submerged Siphon Chamber", risk = 0.32f }
                    }
                },
                new ExcavationSiteDef
                {
                    site_id = "excavation_storage_chamber",
                    location_id = "loc_excavation_storage_chamber",
                    display_name = "Forgotten Storage Chamber 14",
                    description = "An auxiliary military logistics cache sealed in haste during civil evacuation. Unstable masonry slabs overhang intact pallets of rations and industrial spares.",
                    max_depth_meters = 75f,
                    required_progress = 110f,
                    structural_risk = 0.34f,
                    required_tools = new List<string> { "crowbar", "shovel" },
                    shoring_materials = new List<string> { "scrap_metal", "mechanical_parts" },
                    hazard_type = "hazard_toxic_air",
                    relic_reward_id = "spring_mechanism",
                    loot_table = "salvage_rare",
                    journal_entry_id = "journal_excavation_storage_chamber",
                    depth_bands = new List<ExcavationDepthBandDef>
                    {
                        new ExcavationDepthBandDef { depth_meters = 20f, label = "Loading Bay Collapse", risk = 0.25f },
                        new ExcavationDepthBandDef { depth_meters = 48f, label = "Intermediate Staging Void", risk = 0.35f },
                        new ExcavationDepthBandDef { depth_meters = 75f, label = "Sealed Logistics Vault", risk = 0.48f }
                    }
                },
                new ExcavationSiteDef
                {
                    site_id = "excavation_civilian_shelter",
                    location_id = "loc_excavation_civilian_shelter",
                    display_name = "Pre-War Civilian Shelter B-12",
                    description = "A privately funded neighborhood shelter built beneath a residential complex. Shorter excavation depths yield domestic survival gear, medical supplies, and handwritten diaries.",
                    max_depth_meters = 40f,
                    required_progress = 65f,
                    structural_risk = 0.18f,
                    required_tools = new List<string> { "shovel", "crowbar" },
                    shoring_materials = new List<string> { "scrap_wood", "scrap_metal" },
                    hazard_type = "hazard_toxic_air",
                    relic_reward_id = "music_box_comb",
                    loot_table = "salvage_common",
                    journal_entry_id = "journal_excavation_civilian_shelter",
                    depth_bands = new List<ExcavationDepthBandDef>
                    {
                        new ExcavationDepthBandDef { depth_meters = 10f, label = "Basement Masonry Fill", risk = 0.10f },
                        new ExcavationDepthBandDef { depth_meters = 24f, label = "Stairwell Air-Lock Rubble", risk = 0.20f },
                        new ExcavationDepthBandDef { depth_meters = 40f, label = "Civilian Bunking Quarters", risk = 0.28f }
                    }
                }
            };
        }
    }
}
