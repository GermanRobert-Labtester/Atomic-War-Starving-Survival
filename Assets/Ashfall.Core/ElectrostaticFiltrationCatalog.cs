using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>One discrete operating profile of an electrostatic stage (safe, abstract).</summary>
    public sealed class ElectrostaticProfileDef
    {
        public string profile_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public float nominal_power_w { get; set; } = 350f;
        public float capture_efficiency_pm25 { get; set; } = 0.8f;
        public float capture_efficiency_pm10 { get; set; } = 0.9f;
        public float hot_ash_capture_efficiency { get; set; } = 0.7f;
        public float ozone_output_rate_ppm_per_day { get; set; } = 8f;
        /// <summary>Base daily arc-fault probability in basis points (deterministic seed rolls).</summary>
        public int arc_risk_base_bp { get; set; } = 40;
    }

    /// <summary>One electrostatic precipitator stage definition.</summary>
    public sealed class ElectrostaticStageDef
    {
        public string stage_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public List<ElectrostaticProfileDef> operating_profiles { get; set; } = new List<ElectrostaticProfileDef>();
        public float dust_capacity_kg { get; set; } = 12f;
        public int maintenance_interval_days { get; set; } = 14;
        public List< ElectrostaticComponentCost> required_component_ids { get; set; } = new List<ElectrostaticComponentCost>();
        public List<string> tags { get; set; } = new List<string>();
    }

    public sealed class ElectrostaticComponentCost
    {
        public string item_id { get; set; } = string.Empty;
        public int amount { get; set; } = 1;
    }

    /// <summary>Container shape for electrostatic_filtration_catalog.json (the authority).</summary>
    public sealed class ElectrostaticFiltrationCatalogContainer
    {
        public int schema_version { get; set; } = 1;
        public List<ElectrostaticStageDef> stages { get; set; } = new List<ElectrostaticStageDef>();
    }

    /// <summary>
    /// Loads electrostatic air-filtration stage definitions from JSON.
    /// Engine-agnostic: uses IFileIO and IJsonSerializer ports. The ventilation
    /// system remains the sole air-quality authority — this catalog only
    /// parameterizes the electrostatic stage installed into it.
    /// </summary>
    public static class ElectrostaticFiltrationCatalogLoader
    {
        public const string DefaultFileName = "electrostatic_filtration_catalog.json";

        public static List<ElectrostaticStageDef> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return new List<ElectrostaticStageDef>();

            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                return new List<ElectrostaticStageDef>();

            string rawText = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(rawText))
                return new List<ElectrostaticStageDef>();

            var container = json.Deserialize<ElectrostaticFiltrationCatalogContainer>(rawText);
            return container?.stages ?? new List<ElectrostaticStageDef>();
        }

        /// <summary>
        /// Core-owned intake mapping: weather truth → daily particulate mass
        /// (kg) drawn through the stage. Hosts pass weather state; this helper
        /// keeps the conversion in Core so hosts never calculate mechanics.
        /// </summary>
        public static float WeatherIntakeParticulateKg(WeatherKind weather, bool mainDuctOpen)
        {
            if (!mainDuctOpen) return 0f;
            return weather switch
            {
                WeatherKind.FalloutStorm => 2.4f,
                WeatherKind.Ashfall => 1.2f,
                WeatherKind.Rain => 0.3f,
                WeatherKind.Overcast => 0.15f,
                _ => 0.05f
            };
        }

        /// <summary>Hot-ash (charged fallout) load is present in ash-bearing weather.</summary>
        public static bool IsHotAshLoad(WeatherKind weather)
            => weather is WeatherKind.Ashfall or WeatherKind.FalloutStorm;
    }
}
