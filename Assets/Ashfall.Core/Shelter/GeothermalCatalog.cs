using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Shelter
{
    [Serializable]
    public sealed class GeothermalStrataDef
    {
        [JsonPropertyName("strata_id")]
        public string StrataId { get; set; } = string.Empty;

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("start_depth_m")]
        public float StartDepthM { get; set; }

        [JsonPropertyName("end_depth_m")]
        public float EndDepthM { get; set; }

        [JsonPropertyName("rock_hardness")]
        public float RockHardness { get; set; }

        [JsonPropertyName("drill_wear_per_meter")]
        public float DrillWearPerMeter { get; set; }

        [JsonPropertyName("seismic_hazard")]
        public float SeismicHazard { get; set; }

        [JsonPropertyName("steam_temperature_c")]
        public float SteamTemperatureC { get; set; }

        [JsonPropertyName("thermal_output_kw")]
        public float ThermalOutputKw { get; set; }

        [JsonPropertyName("water_yield_liters_per_day")]
        public float WaterYieldLitersPerDay { get; set; }

        [JsonPropertyName("min_casing_tier")]
        public int MinCasingTier { get; set; }

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class GeothermalDrillingCatalog
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("strata")]
        public List<GeothermalStrataDef> Strata { get; set; } = new List<GeothermalStrataDef>();
    }
}
