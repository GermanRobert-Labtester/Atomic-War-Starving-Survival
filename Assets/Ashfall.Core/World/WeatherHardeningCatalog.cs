using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Ashfall.Core.World
{
    [Serializable]
    public sealed class WeatherHardeningUpgradeDef
    {
        [JsonPropertyName("upgrade_id")]
        public string UpgradeId { get; set; } = string.Empty;

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("target_type")]
        public string TargetType { get; set; } = string.Empty;

        [JsonPropertyName("thermal_retention")]
        public float ThermalRetention { get; set; }

        [JsonPropertyName("electrical_watts")]
        public float ElectricalWatts { get; set; }

        [JsonPropertyName("freeze_risk_reduction")]
        public float FreezeRiskReduction { get; set; }

        [JsonPropertyName("material_costs")]
        public List<MaterialCost> MaterialCosts { get; set; } = new List<MaterialCost>();

        [JsonPropertyName("fuel_item_id")]
        public string? FuelItemId { get; set; }

        [JsonPropertyName("fuel_consumption_per_day")]
        public float FuelConsumptionPerDay { get; set; }

        [JsonPropertyName("heat_output_kw")]
        public float HeatOutputKw { get; set; }

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class MaterialCost
    {
        [JsonPropertyName("item_id")]
        public string ItemId { get; set; } = string.Empty;

        [JsonPropertyName("amount")]
        public int Amount { get; set; }
    }

    [Serializable]
    public sealed class WeatherHardeningCatalog
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("upgrades")]
        public List<WeatherHardeningUpgradeDef> Upgrades { get; set; } = new List<WeatherHardeningUpgradeDef>();
    }
}
