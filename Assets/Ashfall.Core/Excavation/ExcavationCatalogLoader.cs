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
                    display_name = "Collapsed Command Vault",
                    description = "A reinforced military command installation buried beneath tons of blasted granite.",
                    max_depth_meters = 90f,
                    required_progress = 120f,
                    structural_risk = 0.35f,
                    required_tools = new List<string> { "tools_precision", "shovel" },
                    shoring_materials = new List<string> { "scrap_metal", "mechanical_parts" },
                    hazard_type = "hazard_toxic_air",
                    relic_reward_id = "item_comm_codebook_alpha",
                    loot_table = "salvage_rare",
                    journal_entry_id = "journal_excavation_command_vault",
                    depth_bands = new List<ExcavationDepthBandDef>
                    {
                        new ExcavationDepthBandDef { depth_meters = 20f, label = "Rubble Throat", risk = 0.20f },
                        new ExcavationDepthBandDef { depth_meters = 50f, label = "Collapsed Operations Level", risk = 0.35f },
                        new ExcavationDepthBandDef { depth_meters = 90f, label = "Sealed Communications Vault", risk = 0.50f }
                    }
                },
                new ExcavationSiteDef
                {
                    site_id = "excavation_utility_tunnels",
                    location_id = "loc_excavation_utility_tunnels",
                    display_name = "Utility Tunnel Network",
                    description = "Subterranean municipal service corridors and conduit trunks choked with mud and mold.",
                    max_depth_meters = 70f,
                    required_progress = 100f,
                    structural_risk = 0.25f,
                    required_tools = new List<string> { "tools_precision", "wrench" },
                    shoring_materials = new List<string> { "scrap_wood", "metal_sheet" },
                    hazard_type = "hazard_spore_mold",
                    relic_reward_id = "tools_precision",
                    loot_table = "salvage_common",
                    journal_entry_id = "journal_excavation_utility_tunnels",
                    depth_bands = new List<ExcavationDepthBandDef>
                    {
                        new ExcavationDepthBandDef { depth_meters = 15f, label = "Service Access Hatch", risk = 0.15f },
                        new ExcavationDepthBandDef { depth_meters = 40f, label = "Flooded Conduit Junction", risk = 0.25f },
                        new ExcavationDepthBandDef { depth_meters = 70f, label = "Primary Infrastructure Trunk", risk = 0.40f }
                    }
                },
                new ExcavationSiteDef
                {
                    site_id = "excavation_metro_interchange",
                    location_id = "loc_excavation_metro_interchange",
                    display_name = "Buried Metro Interchange",
                    description = "A multi-tier transit hub collapsed during the orbital strikes.",
                    max_depth_meters = 110f,
                    required_progress = 150f,
                    structural_risk = 0.40f,
                    required_tools = new List<string> { "shovel", "crowbar" },
                    shoring_materials = new List<string> { "steel_columns", "hydraulic_jack" },
                    hazard_type = "hazard_spore_mold",
                    relic_reward_id = "item_logistics_cipher_sheet",
                    loot_table = "salvage_rare",
                    journal_entry_id = "journal_excavation_metro_interchange",
                    depth_bands = new List<ExcavationDepthBandDef>
                    {
                        new ExcavationDepthBandDef { depth_meters = 25f, label = "Street Concourse Collapse", risk = 0.25f },
                        new ExcavationDepthBandDef { depth_meters = 60f, label = "Sub-Platform Mezzanine", risk = 0.40f },
                        new ExcavationDepthBandDef { depth_meters = 110f, label = "Deep Track Interchange & Service Vault", risk = 0.55f }
                    }
                },
                new ExcavationSiteDef
                {
                    site_id = "excavation_mine_shaft",
                    location_id = "loc_excavation_mine_shaft",
                    display_name = "Industrial Mine Shaft Adit 4",
                    description = "A heavy extraction shaft dropping into mineral-rich bedrock.",
                    max_depth_meters = 140f,
                    required_progress = 180f,
                    structural_risk = 0.50f,
                    required_tools = new List<string> { "pickaxe", "hydraulic_jack" },
                    shoring_materials = new List<string> { "timber_beams", "hydraulic_jack" },
                    hazard_type = "hazard_methane_pocket",
                    relic_reward_id = "heavy_industrial_motor",
                    loot_table = "salvage_rare",
                    journal_entry_id = "journal_excavation_mine_shaft",
                    depth_bands = new List<ExcavationDepthBandDef>
                    {
                        new ExcavationDepthBandDef { depth_meters = 30f, label = "Collapsed Mine Adit", risk = 0.30f },
                        new ExcavationDepthBandDef { depth_meters = 75f, label = "Primary Extraction Level", risk = 0.50f },
                        new ExcavationDepthBandDef { depth_meters = 140f, label = "Deep Machinery & Ore Sump", risk = 0.70f }
                    }
                },
                new ExcavationSiteDef
                {
                    site_id = "excavation_archive_bunker",
                    location_id = "loc_excavation_archive_bunker",
                    display_name = "Pre-War Archive Bunker",
                    description = "An underground scientific and administrative depository sealed under blast arches.",
                    max_depth_meters = 80f,
                    required_progress = 130f,
                    structural_risk = 0.30f,
                    required_tools = new List<string> { "tools_precision", "crowbar" },
                    shoring_materials = new List<string> { "reinforced_arches", "scrap_metal" },
                    hazard_type = "hazard_sealed_air",
                    relic_reward_id = "item_archive_index_cylinder",
                    loot_table = "salvage_rare",
                    journal_entry_id = "journal_excavation_archive_bunker",
                    depth_bands = new List<ExcavationDepthBandDef>
                    {
                        new ExcavationDepthBandDef { depth_meters = 10f, label = "Security Gate Rubble", risk = 0.15f },
                        new ExcavationDepthBandDef { depth_meters = 35f, label = "Catalog & Microfilm Reading Room", risk = 0.30f },
                        new ExcavationDepthBandDef { depth_meters = 80f, label = "Climate-Sealed Master Repository Vault", risk = 0.45f }
                    }
                }
            };
        }
    }
}
