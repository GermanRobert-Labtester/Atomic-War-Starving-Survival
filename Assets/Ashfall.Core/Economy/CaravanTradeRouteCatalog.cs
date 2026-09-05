// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.IO;

namespace Ashfall.Core.Economy
{
    [Serializable]
    public sealed class CaravanRouteDefinition
    {
        public string route_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string faction_id { get; set; } = string.Empty;
        public string origin_region_id { get; set; } = string.Empty;
        public string destination_region_id { get; set; } = string.Empty;
        public int travel_days { get; set; } = 5;
        public int base_risk_permille { get; set; } = 100;
        public int season_start_day { get; set; } = 1;
        public int season_end_day { get; set; } = 360;
        public int arrival_interval_days { get; set; } = 20;
        public int base_guard_strength { get; set; } = 5;
        public float weather_risk_multiplier { get; set; } = 1.0f;
        public float bandit_risk_multiplier { get; set; } = 1.0f;
        public List<string> import_demands { get; set; } = new List<string>();
        public List<string> export_surpluses { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class CaravanTradeRoutesContainer
    {
        public int schema_version { get; set; } = 1;
        public List<CaravanRouteDefinition> routes { get; set; } = new List<CaravanRouteDefinition>();
    }

    public static class CaravanTradeRouteCatalogLoader
    {
        public const string DefaultFileName = "caravan_trade_routes.json";

        public static List<CaravanRouteDefinition> Load(
            string dataDir,
            IFileIO? fileIO = null,
            IJsonSerializer? json = null)
        {
            fileIO ??= new FileSystemIO();
            json ??= new SystemTextJsonSerializer();

            string path = Path.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
            {
                return new List<CaravanRouteDefinition>();
            }

            try
            {
                string raw = fileIO.ReadAllText(path);
                var container = json.Deserialize<CaravanTradeRoutesContainer>(raw);
                return container?.routes ?? new List<CaravanRouteDefinition>();
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(path, "CaravanTradeRoutesContainer", ex);
                return new List<CaravanRouteDefinition>();
            }
        }
    }
}
