using System;
using System.Collections.Generic;
using System.IO;

namespace Ashfall.Core.Waystation
{
    [Serializable]
    public sealed class WaystationDef
    {
        public string id { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string node_id { get; set; } = string.Empty;
        public string region { get; set; } = string.Empty;
        public string keeper_name { get; set; } = string.Empty;
        public string specialty { get; set; } = string.Empty;
        public float condition { get; set; } = 100f;
        public float filter_health { get; set; } = 100f;
        public int defense_rating { get; set; } = 3;
        public List<string> services { get; set; } = new List<string>();
        public List<string> stock_item_ids { get; set; } = new List<string>();
        public string local_problem { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class WaystationsCatalogContainer
    {
        public int schema_version { get; set; }
        public List<WaystationDef> waystations { get; set; } = new List<WaystationDef>();
    }

    public static class WaystationCatalogLoader
    {
        public const string DefaultFileName = "waystations.json";

        public static List<WaystationDef> Load(
            string dataDir,
            IFileIO? fileIO = null,
            IJsonSerializer? json = null)
        {
            fileIO ??= new FileSystemIO();
            json ??= new SystemTextJsonSerializer();

            string path = Path.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
            {
                return GetDefaultWaystations();
            }

            try
            {
                string raw = fileIO.ReadAllText(path);
                var container = json.Deserialize<WaystationsCatalogContainer>(raw);
                if (container?.waystations != null && container.waystations.Count > 0)
                {
                    return container.waystations;
                }
            }
            catch
            {
                // Fallback to compiled defaults on read error
            }

            return GetDefaultWaystations();
        }

        public static List<WaystationDef> GetDefaultWaystations()
        {
            return new List<WaystationDef>
            {
                new WaystationDef
                {
                    id = "waystation_alpha_cut",
                    name = "Waystation A — The Cut",
                    node_id = "loc_cut_abandoned_depot",
                    region = "industrial_belt",
                    keeper_name = "Warden Kessel",
                    specialty = "Industrial Tools & Fasteners",
                    condition = 85f,
                    filter_health = 90f,
                    defense_rating = 4,
                    services = new List<string> { "trade", "staging", "rest", "filter_recharge" },
                    stock_item_ids = new List<string> { "scrap_metal", "scrap_mechanical", "water_filter" },
                    local_problem = "Filter degradation under heavy coal soot accumulation."
                },
                new WaystationDef
                {
                    id = "waystation_switchback",
                    name = "The Switchback Waystation",
                    node_id = "loc_shrine_switchback_waystation",
                    region = "high_scarp",
                    keeper_name = "Deacon Vane",
                    specialty = "Cold-Weather Fuel & Thermal Liners",
                    condition = 80f,
                    filter_health = 85f,
                    defense_rating = 3,
                    services = new List<string> { "trade", "staging", "rest", "blessing" },
                    stock_item_ids = new List<string> { "fuel", "clean_water", "medical_kit" },
                    local_problem = "Scree instability threatening the southern bunkhouse foundation."
                },
                new WaystationDef
                {
                    id = "waystation_span44",
                    name = "Span 44 Rail Waystation",
                    node_id = "loc_railway_span_44_alpha",
                    region = "industrial_belt",
                    keeper_name = "Foreman Taggart",
                    specialty = "Railway Iron & Pneumatic Parts",
                    condition = 75f,
                    filter_health = 80f,
                    defense_rating = 5,
                    services = new List<string> { "trade", "staging", "repair" },
                    stock_item_ids = new List<string> { "scrap_metal", "electronic_scrap", "ammo_308" },
                    local_problem = "Bridge abutment settling requiring heavy structural shoring."
                },
                new WaystationDef
                {
                    id = "waystation_verity",
                    name = "Verity Motel Staging Post",
                    node_id = "loc_motel_verity",
                    region = "dead_suburbs",
                    keeper_name = "Mistress Corvo",
                    specialty = "Medical Supplies & Clean Water",
                    condition = 90f,
                    filter_health = 95f,
                    defense_rating = 3,
                    services = new List<string> { "trade", "staging", "rest", "intelligence" },
                    stock_item_ids = new List<string> { "medical_kit", "clean_water", "item_comm_codebook_alpha" },
                    local_problem = "Deep well pump motor seized by fine sand."
                },
                new WaystationDef
                {
                    id = "waystation_coast_lock",
                    name = "Lock Gate Four Maritime Staging",
                    node_id = "loc_lock_gate_four",
                    region = "deep_coast",
                    keeper_name = "Diver Renn",
                    specialty = "Saline Iodine & Marine Salvage",
                    condition = 70f,
                    filter_health = 75f,
                    defense_rating = 4,
                    services = new List<string> { "trade", "staging", "saline_wash" },
                    stock_item_ids = new List<string> { "clean_water", "water_filter", "diesel_fuel" },
                    local_problem = "Sluice gate valve seized by heavy salt crust."
                },
                new WaystationDef
                {
                    id = "waystation_grain_verge",
                    name = "Verge Silo Waystation",
                    node_id = "loc_grain_silo",
                    region = "ash_flats",
                    keeper_name = "Weigher Orlov",
                    specialty = "Preserved Grain & Honey Comb",
                    condition = 85f,
                    filter_health = 88f,
                    defense_rating = 4,
                    services = new List<string> { "trade", "staging", "grain_exchange" },
                    stock_item_ids = new List<string> { "dried_rations", "clean_water", "scrap_wood" },
                    local_problem = "Ration quota disputes with garrison sentries."
                }
            };
        }
    }
}
