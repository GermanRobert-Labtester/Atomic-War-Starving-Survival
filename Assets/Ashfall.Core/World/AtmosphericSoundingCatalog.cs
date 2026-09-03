using System;
using System.Collections.Generic;

namespace Ashfall.Core.World
{
    /// <summary>One data-driven altitude band for atmospheric sounding (Plan 71 §6.2).</summary>
    public sealed class SoundingAltitudeBandDef
    {
        public string band_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public float altitude_min_m { get; set; }
        public float altitude_max_m { get; set; }
        /// <summary>Multiplier on sampled wind speed for drift through this band.</summary>
        public float wind_variability { get; set; } = 1f;
        public string temperature_profile_tag { get; set; } = string.Empty;
        public string dust_profile { get; set; } = string.Empty;
        /// <summary>Multiplier on sampled radiation intensity.</summary>
        public float radiation_sampling_modifier { get; set; } = 1f;
        /// <summary>Multiplier on observation-quality gain while in this band.</summary>
        public float telemetry_quality_modifier { get; set; } = 1f;
        public List<string> tags { get; set; } = new List<string>();
    }

    /// <summary>One sonde payload definition (envelope, sensors, recovery table).</summary>
    public sealed class SoundingPayloadDef
    {
        public string payload_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string envelope_class { get; set; } = string.Empty;
        /// <summary>Abstract lift-gas class (gameplay tag only — no synthesis detail).</summary>
        public string lift_gas_class { get; set; } = string.Empty;
        public float sensor_mass_kg { get; set; } = 0.6f;
        public float burst_altitude_min_m { get; set; } = 24000f;
        public float burst_altitude_max_m { get; set; } = 30000f;
        public float telemetry_range_km { get; set; } = 40f;
        /// <summary>Descent speed (km per flight tick) under parachute after burst.</summary>
        public float parachute_descent_rate_km_per_tick { get; set; } = 6f;
        public float recovery_value { get; set; } = 10f;
        public float sensor_durability { get; set; } = 1f;
        /// <summary>Data-driven reward table: highest min_condition band satisfied wins.</summary>
        public List<SoundingRecoveryRewardDef> recovery_rewards { get; set; } = new List<SoundingRecoveryRewardDef>();
        public List<string> tags { get; set; } = new List<string>();
    }

    public sealed class SoundingRecoveryRewardDef
    {
        public float min_condition { get; set; }
        public string item_id { get; set; } = string.Empty;
        public int amount { get; set; } = 1;
    }

    /// <summary>Container shape for atmospheric_sounding_catalog.json (the authority).</summary>
    public sealed class AtmosphericSoundingCatalogContainer
    {
        public int schema_version { get; set; } = 1;
        public List<SoundingAltitudeBandDef> altitude_bands { get; set; } = new List<SoundingAltitudeBandDef>();
        public List<SoundingPayloadDef> payloads { get; set; } = new List<SoundingPayloadDef>();
    }

    /// <summary>
    /// Loads atmospheric sounding altitude bands and payload definitions from
    /// JSON. Engine-agnostic: uses IFileIO and IJsonSerializer ports. The sonde
    /// system remains the sole sounding authority — the catalog only
    /// parameterizes bands, drift and the recovery reward table.
    /// </summary>
    public static class AtmosphericSoundingCatalogLoader
    {
        public const string DefaultFileName = "atmospheric_sounding_catalog.json";

        public static AtmosphericSoundingCatalogContainer Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return new AtmosphericSoundingCatalogContainer();

            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                return new AtmosphericSoundingCatalogContainer();

            string rawText = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(rawText))
                return new AtmosphericSoundingCatalogContainer();

            var container = json.Deserialize<AtmosphericSoundingCatalogContainer>(rawText);
            return container ?? new AtmosphericSoundingCatalogContainer();
        }
    }
}
