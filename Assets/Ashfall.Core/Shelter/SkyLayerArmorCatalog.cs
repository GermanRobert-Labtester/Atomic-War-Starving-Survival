using System;
using System.Collections.Generic;
using Ashfall.Core.IO;

namespace Ashfall.Core.Shelter
{
    [Serializable]
    public sealed class ArmorMaterialCostDef
    {
        public string item_id { get; set; } = string.Empty;
        public int quantity { get; set; } = 1;
    }

    /// <summary>
    /// DTO defining an authored sky-layer armor configuration.
    /// </summary>
    [Serializable]
    public sealed class SkyLayerArmorConfigDef
    {
        public string id { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string tier { get; set; } = "improvised"; // improvised, reinforced, military_grade
        public CeilingMaterialTier material_tier { get; set; } = CeilingMaterialTier.Dirt;
        public float default_thickness_meters { get; set; } = 1.0f;
        public float blast_resistance_mj { get; set; } = 10f;
        public float attenuation_factor { get; set; } = 0.5f;
        public float degradation_rate { get; set; } = 0.2f;
        public List<ArmorMaterialCostDef> composition { get; set; } = new List<ArmorMaterialCostDef>();
        public List<ArmorMaterialCostDef> repair_cost { get; set; } = new List<ArmorMaterialCostDef>();
    }

    /// <summary>
    /// Container for sky_layer_armor_catalog.json.
    /// </summary>
    [Serializable]
    public sealed class SkyLayerArmorCatalogContainer
    {
        public int schema_version { get; set; } = 1;
        public List<SkyLayerArmorConfigDef> configurations { get; set; } = new List<SkyLayerArmorConfigDef>();
    }

    /// <summary>
    /// Loader and query authority for authored sky-layer armor configurations.
    /// </summary>
    public static class SkyLayerArmorCatalogLoader
    {
        public const string CatalogFileName = "sky_layer_armor_catalog.json";

        public static List<SkyLayerArmorConfigDef> Load(string dataDir, IFileIO? fileIO = null, IJsonSerializer? serializer = null)
        {
            fileIO ??= new FileSystemIO();
            serializer ??= new SystemTextJsonSerializer();

            if (string.IsNullOrEmpty(dataDir))
                return GetDefaultConfigurations();

            string fullPath = fileIO.Combine(dataDir, CatalogFileName);
            if (!fileIO.FileExists(fullPath))
                return GetDefaultConfigurations();

            try
            {
                string json = fileIO.ReadAllText(fullPath);
                var container = serializer.Deserialize<SkyLayerArmorCatalogContainer>(json);
                if (container?.configurations != null && container.configurations.Count > 0)
                    return container.configurations;
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(fullPath, "SkyLayerArmorCatalogContainer", ex);
            }

            return GetDefaultConfigurations();
        }

        public static List<SkyLayerArmorConfigDef> GetDefaultConfigurations()
        {
            return new List<SkyLayerArmorConfigDef>
            {
                new SkyLayerArmorConfigDef
                {
                    id = "sky_armor_sandbag_layer",
                    name = "Improvised Sandbag Layer",
                    description = "Stacked burlap sacks filled with dry soil and gravel, braced with timber struts. Protects against light shrapnel but degrades rapidly under direct kinetic hits.",
                    tier = "improvised",
                    material_tier = CeilingMaterialTier.Dirt,
                    default_thickness_meters = 0.8f,
                    blast_resistance_mj = 5.0f,
                    attenuation_factor = 0.60f,
                    degradation_rate = 0.35f,
                    composition = new List<ArmorMaterialCostDef>
                    {
                        new ArmorMaterialCostDef { item_id = "scrap_wood", quantity = 4 },
                        new ArmorMaterialCostDef { item_id = "cloth", quantity = 6 }
                    },
                    repair_cost = new List<ArmorMaterialCostDef>
                    {
                        new ArmorMaterialCostDef { item_id = "scrap_wood", quantity = 1 },
                        new ArmorMaterialCostDef { item_id = "cloth", quantity = 2 }
                    }
                },
                new SkyLayerArmorConfigDef
                {
                    id = "sky_armor_scrap_overlay",
                    name = "Scrap-Plate Overlay",
                    description = "Overlapping salvage steel plates bolted across roof framing. Provides resilient deflection against low-velocity orbital debris.",
                    tier = "improvised",
                    material_tier = CeilingMaterialTier.Wood,
                    default_thickness_meters = 1.0f,
                    blast_resistance_mj = 12.0f,
                    attenuation_factor = 0.45f,
                    degradation_rate = 0.25f,
                    composition = new List<ArmorMaterialCostDef>
                    {
                        new ArmorMaterialCostDef { item_id = "scrap_metal", quantity = 8 },
                        new ArmorMaterialCostDef { item_id = "mechanical_parts", quantity = 4 }
                    },
                    repair_cost = new List<ArmorMaterialCostDef>
                    {
                        new ArmorMaterialCostDef { item_id = "scrap_metal", quantity = 3 }
                    }
                },
                new SkyLayerArmorConfigDef
                {
                    id = "sky_armor_reinforced_concrete",
                    name = "Reinforced Concrete Slab",
                    description = "Poured aggregate cement laced with scrap steel rebar. High mass and blast dampening offer durable long-term orbital mitigation.",
                    tier = "reinforced",
                    material_tier = CeilingMaterialTier.ReinforcedConcrete,
                    default_thickness_meters = 1.5f,
                    blast_resistance_mj = 37.5f,
                    attenuation_factor = 0.20f,
                    degradation_rate = 0.15f,
                    composition = new List<ArmorMaterialCostDef>
                    {
                        new ArmorMaterialCostDef { item_id = "scrap_metal", quantity = 10 },
                        new ArmorMaterialCostDef { item_id = "chemicals", quantity = 4 },
                        new ArmorMaterialCostDef { item_id = "scrap_wood", quantity = 4 }
                    },
                    repair_cost = new List<ArmorMaterialCostDef>
                    {
                        new ArmorMaterialCostDef { item_id = "scrap_metal", quantity = 3 },
                        new ArmorMaterialCostDef { item_id = "chemicals", quantity = 1 }
                    }
                },
                new SkyLayerArmorConfigDef
                {
                    id = "sky_armor_steel_hull_plating",
                    name = "Steel Hull Plating",
                    description = "Heavy naval and industrial pressure-hull plate welded directly over bunker arches. Excellent resistance against hypervelocity kinetic penetrators.",
                    tier = "reinforced",
                    material_tier = CeilingMaterialTier.LeadSheeting,
                    default_thickness_meters = 1.2f,
                    blast_resistance_mj = 45.0f,
                    attenuation_factor = 0.05f,
                    degradation_rate = 0.12f,
                    composition = new List<ArmorMaterialCostDef>
                    {
                        new ArmorMaterialCostDef { item_id = "scrap_metal", quantity = 14 },
                        new ArmorMaterialCostDef { item_id = "mechanical_parts", quantity = 6 },
                        new ArmorMaterialCostDef { item_id = "fuel", quantity = 2 }
                    },
                    repair_cost = new List<ArmorMaterialCostDef>
                    {
                        new ArmorMaterialCostDef { item_id = "scrap_metal", quantity = 4 },
                        new ArmorMaterialCostDef { item_id = "mechanical_parts", quantity = 2 }
                    }
                },
                new SkyLayerArmorConfigDef
                {
                    id = "sky_armor_composite_military",
                    name = "Composite Military-Grade Armor",
                    description = "Layered tungsten-carbide tiles and shock-absorbing ceramic matrix. Top-tier kinetic and radiation shielding designed for military continuity vaults.",
                    tier = "military_grade",
                    material_tier = CeilingMaterialTier.TungstenComposite,
                    default_thickness_meters = 2.0f,
                    blast_resistance_mj = 160.0f,
                    attenuation_factor = 0.01f,
                    degradation_rate = 0.08f,
                    composition = new List<ArmorMaterialCostDef>
                    {
                        new ArmorMaterialCostDef { item_id = "scrap_metal", quantity = 20 },
                        new ArmorMaterialCostDef { item_id = "electronic_scrap", quantity = 8 },
                        new ArmorMaterialCostDef { item_id = "mechanical_parts", quantity = 10 }
                    },
                    repair_cost = new List<ArmorMaterialCostDef>
                    {
                        new ArmorMaterialCostDef { item_id = "scrap_metal", quantity = 5 },
                        new ArmorMaterialCostDef { item_id = "electronic_scrap", quantity = 2 },
                        new ArmorMaterialCostDef { item_id = "mechanical_parts", quantity = 2 }
                    }
                },
                new SkyLayerArmorConfigDef
                {
                    id = "sky_armor_emergency_blast_canopy",
                    name = "Emergency Blast Canopy",
                    description = "A sacrificial pitched timber and sheet canopy designed for rapid deployment ahead of predicted orbital debris showers.",
                    tier = "improvised",
                    material_tier = CeilingMaterialTier.Wood,
                    default_thickness_meters = 0.6f,
                    blast_resistance_mj = 8.0f,
                    attenuation_factor = 0.50f,
                    degradation_rate = 0.40f,
                    composition = new List<ArmorMaterialCostDef>
                    {
                        new ArmorMaterialCostDef { item_id = "scrap_wood", quantity = 6 },
                        new ArmorMaterialCostDef { item_id = "cloth", quantity = 4 },
                        new ArmorMaterialCostDef { item_id = "scrap_metal", quantity = 2 }
                    },
                    repair_cost = new List<ArmorMaterialCostDef>
                    {
                        new ArmorMaterialCostDef { item_id = "scrap_wood", quantity = 2 },
                        new ArmorMaterialCostDef { item_id = "cloth", quantity = 2 }
                    }
                }
            };
        }
    }
}
