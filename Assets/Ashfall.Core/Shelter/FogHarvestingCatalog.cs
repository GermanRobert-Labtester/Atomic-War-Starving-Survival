// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.IO;

namespace Ashfall.Core.Shelter
{
    // ─────────────────────────────────────────────────────────────────
    // Plan 145 — Atmospheric fog harvesting and water-security arrays
    // (catalog DTOs). Wave 1 contract only: data shapes + parse helper.
    // Collection inputs derive from the canonical weather authority
    // (FogPresenceEvaluator contract); output is Raw water only — the
    // WaterTreatmentSystem owns potability. No real electrical physics.
    // ─────────────────────────────────────────────────────────────────

    [Serializable]
    public sealed class FogHarvesterProfileDef
    {
        public string profile_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public int collection_area_units { get; set; } = 20;
        public bool passive { get; set; } = true;
        /// <summary>Preferred wind band: calm | moderate | strong | any.</summary>
        public string wind_band { get; set; } = "moderate";
        /// <summary>Preferred humidity band: low | moderate | high.</summary>
        public string humidity_band { get; set; } = "high";
        public int base_yield_units { get; set; } = 12;
        public int wear_rate_per_day { get; set; } = 2;
        public string contamination_profile { get; set; } = "surface_exposed";
        /// <summary>Always "raw" — harvested water enters the canonical treatment authority, never potable directly.</summary>
        public string output_water_type { get; set; } = "raw";
        public float power_draw_kwh_per_day { get; set; } = 0f;
        public Dictionary<string, int> construction_required_items { get; set; } = new Dictionary<string, int>();
        public List<string> siting_tags { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class FogBandsDef
    {
        /// <summary>Fog presence contribution by canonical weather kind (0..1). Keys must match WeatherSystem kinds.</summary>
        public Dictionary<string, float> presence_by_weather_kind { get; set; } = new Dictionary<string, float>();
        public Dictionary<string, float> wind_band_multipliers { get; set; } = new Dictionary<string, float>();
        public Dictionary<string, float> humidity_band_multipliers { get; set; } = new Dictionary<string, float>();
        public float powered_assist_multiplier { get; set; } = 1.5f;
    }

    [Serializable]
    public sealed class FogStormEventDef
    {
        public int damage_condition_permille { get; set; } = 250;
        public int disable_below_condition_permille { get; set; } = 200;
    }

    [Serializable]
    public sealed class FogBufferDef
    {
        public int max_units { get; set; } = 40;
    }

    [Serializable]
    public sealed class FogHarvestingCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<FogHarvesterProfileDef> harvester_profiles { get; set; } = new List<FogHarvesterProfileDef>();
        public FogBandsDef fog_bands { get; set; } = new FogBandsDef();
        public FogStormEventDef storm_event { get; set; } = new FogStormEventDef();
        public FogBufferDef buffer { get; set; } = new FogBufferDef();

        public FogHarvesterProfileDef? FindProfile(string profileId)
        {
            foreach (var p in harvester_profiles)
                if (p.profile_id == profileId) return p;
            return null;
        }

        /// <summary>Hardened parse: failures route through <see cref="CatalogDiagnostics"/> and return null (never throw).</summary>
        public static FogHarvestingCatalog? FromJson(string json, string path)
        {
            try { return System.Text.Json.JsonSerializer.Deserialize<FogHarvestingCatalog>(json); }
            catch (Exception e) { CatalogDiagnostics.Warn(path, "fog_harvesting_catalog", e); return null; }
        }
    }
}
