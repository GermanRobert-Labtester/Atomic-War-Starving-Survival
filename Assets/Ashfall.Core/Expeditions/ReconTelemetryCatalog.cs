using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Expeditions
{
    [Serializable]
    public sealed class MaterialCost
    {
        [JsonPropertyName("item_id")]
        public string ItemId { get; set; } = string.Empty;

        [JsonPropertyName("amount")]
        public int Amount { get; set; }
    }

    [Serializable]
    public sealed class ReconProbeDef
    {
        [JsonPropertyName("platform_id")]
        public string PlatformId { get; set; } = string.Empty;

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("platform_type")]
        public string PlatformType { get; set; } = string.Empty;

        [JsonPropertyName("range_km")]
        public float RangeKm { get; set; }

        [JsonPropertyName("operational_altitude_m")]
        public float OperationalAltitudeM { get; set; }

        [JsonPropertyName("endurance_hours")]
        public float EnduranceHours { get; set; }

        [JsonPropertyName("wind_limit_kph")]
        public float WindLimitKph { get; set; }

        [JsonPropertyName("launch_costs")]
        public List<MaterialCost> LaunchCosts { get; set; } = new List<MaterialCost>();

        [JsonPropertyName("battery_cost")]
        public float BatteryCost { get; set; }

        [JsonPropertyName("survey_resolution")]
        public int SurveyResolution { get; set; }

        [JsonPropertyName("sensor_suite")]
        public List<string> SensorSuite { get; set; } = new List<string>();

        [JsonPropertyName("recoverable")]
        public bool Recoverable { get; set; }

        [JsonPropertyName("radio_range_multiplier")]
        public float? RadioRangeMultiplier { get; set; }

        [JsonPropertyName("forecast_horizon_hours")]
        public int? ForecastHorizonHours { get; set; }

        [JsonPropertyName("survey_radius_sectors")]
        public int? SurveyRadiusSectors { get; set; }

        [JsonPropertyName("route_speed_multiplier")]
        public float? RouteSpeedMultiplier { get; set; }

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class ReconTelemetryCatalog
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("platforms")]
        public List<ReconProbeDef> Platforms { get; set; } = new List<ReconProbeDef>();
    }
}
