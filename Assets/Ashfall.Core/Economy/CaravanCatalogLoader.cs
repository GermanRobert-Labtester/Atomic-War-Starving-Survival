using System;
using System.Collections.Generic;
using System.IO;

namespace Ashfall.Core.Economy
{
    [Serializable]
    public sealed class CaravanSpecialtyGoodDef
    {
        public string item_id { get; set; } = string.Empty;
        public int quantity { get; set; } = 1;
        public int price_rations { get; set; } = 1;
    }

    [Serializable]
    public sealed class CaravanDefinition
    {
        public string caravan_id { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string faction_id { get; set; } = string.Empty;
        public string origin_region { get; set; } = string.Empty;
        public List<string> route_node_ids { get; set; } = new List<string>();
        public int stay_duration_days { get; set; } = 2;
        public int guard_count { get; set; } = 4;
        public List<CaravanSpecialtyGoodDef> specialty_goods { get; set; } = new List<CaravanSpecialtyGoodDef>();
    }

    [Serializable]
    public sealed class CaravansCatalogContainer
    {
        public int schema_version { get; set; }
        public List<CaravanDefinition> caravans { get; set; } = new List<CaravanDefinition>();
    }

    public static class CaravanCatalogLoader
    {
        public const string DefaultFileName = "caravans.json";

        public static List<CaravanDefinition> Load(
            string dataDir,
            IFileIO? fileIO = null,
            IJsonSerializer? json = null)
        {
            fileIO ??= new FileSystemIO();
            json ??= new SystemTextJsonSerializer();

            string path = Path.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
            {
                return GetDefaultCaravans();
            }

            try
            {
                string raw = fileIO.ReadAllText(path);
                var container = json.Deserialize<CaravansCatalogContainer>(raw);
                if (container?.caravans != null && container.caravans.Count > 0)
                {
                    return container.caravans;
                }
            }
            catch
            {
                // Fallback to compiled defaults
            }

            return GetDefaultCaravans();
        }

        public static List<CaravanDefinition> GetDefaultCaravans()
        {
            return new List<CaravanDefinition>
            {
                new CaravanDefinition
                {
                    caravan_id = "caravan_flotilla_salt_run",
                    name = "Salt & Saline Flotilla Convoy",
                    faction_id = "faction_the_fleet",
                    origin_region = "deep_coast",
                    route_node_ids = new List<string> { "loc_black_flotilla_outpost", "loc_the_shallows_market", "loc_lock_gate_four", "loc_water_station", "loc_holdfast" },
                    stay_duration_days = 2,
                    guard_count = 5,
                    specialty_goods = new List<CaravanSpecialtyGoodDef>
                    {
                        new CaravanSpecialtyGoodDef { item_id = "clean_water", quantity = 15, price_rations = 1 },
                        new CaravanSpecialtyGoodDef { item_id = "water_filter", quantity = 4, price_rations = 6 },
                        new CaravanSpecialtyGoodDef { item_id = "diesel_fuel", quantity = 5, price_rations = 4 }
                    }
                },
                new CaravanDefinition
                {
                    caravan_id = "caravan_verge_grain_convoy",
                    name = "Verge Agricultural Hauler",
                    faction_id = "faction_rebuilders",
                    origin_region = "ash_flats",
                    route_node_ids = new List<string> { "loc_grain_silo", "loc_forward_roster_camp", "loc_cut_merchant_caravanserai", "loc_grange_hall", "loc_the_allotments" },
                    stay_duration_days = 2,
                    guard_count = 4,
                    specialty_goods = new List<CaravanSpecialtyGoodDef>
                    {
                        new CaravanSpecialtyGoodDef { item_id = "dried_rations", quantity = 20, price_rations = 1 },
                        new CaravanSpecialtyGoodDef { item_id = "clean_water", quantity = 10, price_rations = 1 },
                        new CaravanSpecialtyGoodDef { item_id = "scrap_wood", quantity = 12, price_rations = 2 }
                    }
                },
                new CaravanDefinition
                {
                    caravan_id = "caravan_foundry_coal_iron",
                    name = "Foundry Iron & Coal Column",
                    faction_id = "faction_silent_foundry",
                    origin_region = "industrial_belt",
                    route_node_ids = new List<string> { "loc_recovery_yard", "loc_railway_span_44_alpha", "loc_weighbridge", "loc_cut_abandoned_depot", "loc_holdfast" },
                    stay_duration_days = 2,
                    guard_count = 6,
                    specialty_goods = new List<CaravanSpecialtyGoodDef>
                    {
                        new CaravanSpecialtyGoodDef { item_id = "scrap_metal", quantity = 18, price_rations = 2 },
                        new CaravanSpecialtyGoodDef { item_id = "scrap_mechanical", quantity = 8, price_rations = 3 },
                        new CaravanSpecialtyGoodDef { item_id = "ammo_308", quantity = 12, price_rations = 4 }
                    }
                },
                new CaravanDefinition
                {
                    caravan_id = "caravan_free_trader_circuit",
                    name = "Scale Free-Trader Circuit",
                    faction_id = "faction_the_scale",
                    origin_region = "settlement",
                    route_node_ids = new List<string> { "loc_cut_merchant_caravanserai", "loc_motel_verity", "loc_shrine_switchback_waystation", "loc_low_background_lab", "loc_water_station" },
                    stay_duration_days = 3,
                    guard_count = 4,
                    specialty_goods = new List<CaravanSpecialtyGoodDef>
                    {
                        new CaravanSpecialtyGoodDef { item_id = "medical_kit", quantity = 6, price_rations = 5 },
                        new CaravanSpecialtyGoodDef { item_id = "military_radio", quantity = 2, price_rations = 10 },
                        new CaravanSpecialtyGoodDef { item_id = "electronic_scrap", quantity = 10, price_rations = 3 }
                    }
                }
            };
        }
    }
}
