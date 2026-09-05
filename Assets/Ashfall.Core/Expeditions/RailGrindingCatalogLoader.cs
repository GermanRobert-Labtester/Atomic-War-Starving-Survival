// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Expeditions
{
    /// <summary>
    /// Rail grinding catalog loaded from
    /// <c>Assets/StreamingAssets/Data/rail_grinding_catalog.json</c>.
    /// Grinding head tuning and rail profiles are data authority — the engine
    /// registers from here.
    /// </summary>
    [Serializable]
    public sealed class RailGrindingCatalog
    {
        public int schema_version = 1;
        public List<RailGrindingCatalogProfileDef> rail_profiles = new List<RailGrindingCatalogProfileDef>();
        public List<RailGrindingCatalogHeadDef> grinding_heads = new List<RailGrindingCatalogHeadDef>();
        public RailGrindingCatalogMaintenanceDef maintenance = new RailGrindingCatalogMaintenanceDef();
    }

    [Serializable]
    public sealed class RailGrindingCatalogProfileDef
    {
        public string id = string.Empty;
        public string route_mode = "rail";
        public string nominal_profile = string.Empty;
        public float roughness_good_threshold = 0.25f;
        public float roughness_bad_threshold = 0.75f;
    }

    [Serializable]
    public sealed class RailGrindingCatalogHeadDef
    {
        public string id = string.Empty;
        public string display_name_key = string.Empty;
        public string display_name = string.Empty;
        public List<string> compatible_vehicle_tags = new List<string>();
        public float nominal_rpm = 3600.0f;
        public float min_rpm = 2000.0f;
        public float max_rpm = 4800.0f;
        public float pass_depth_mm = 0.4f;
        public float base_work_rate_km_per_hour = 4.0f;
        public float stone_wear_per_km = 1.5f;
        public float water_suppression_per_km = 25.0f;
        public float target_roughness = 0.20f;
        public float max_speed_upgrade_kph = 80.0f;
        public float spark_hazard_base = 0.12f;
        public float stone_shatter_base = 0.04f;
        public List<string> required_items = new List<string>();
    }

    [Serializable]
    public sealed class RailGrindingCatalogMaintenanceDef
    {
        public float stone_replacement_min_diameter_mm = 120.0f;
        public float cylinder_calibration_interval_km = 50.0f;
    }

    public static class RailGrindingCatalogLoader
    {
        public static RailGrindingCatalog Load(string dataDir, IFileIO files, IJsonSerializer json)
        {
            if (string.IsNullOrWhiteSpace(dataDir)) throw new ArgumentException("dataDir required", nameof(dataDir));
            if (files == null) throw new ArgumentNullException(nameof(files));
            if (json == null) throw new ArgumentNullException(nameof(json));

            string path = files.Combine(dataDir, "rail_grinding_catalog.json");
            if (!files.FileExists(path))
                return new RailGrindingCatalog(); // Empty catalog; engine seeds stay in force

            string raw = files.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return new RailGrindingCatalog();

            var catalog = json.Deserialize<RailGrindingCatalog>(raw);
            if (catalog == null)
                throw new InvalidOperationException("Failed to deserialize rail_grinding_catalog.json");

            var seenHeads = new HashSet<string>(StringComparer.Ordinal);
            foreach (var head in catalog.grinding_heads)
            {
                if (string.IsNullOrEmpty(head.id) || !seenHeads.Add(head.id))
                    throw new InvalidOperationException($"Duplicate or empty rail grinding head id: '{head?.id}'");
                if (head.base_work_rate_km_per_hour <= 0f || head.nominal_rpm <= 0f)
                    throw new InvalidOperationException($"rail grinding head '{head.id}' has non-positive work rate or rpm");
                if (head.max_speed_upgrade_kph < 0f)
                    throw new InvalidOperationException($"rail grinding head '{head.id}' has a negative speed cap");
            }
            var seenProfiles = new HashSet<string>(StringComparer.Ordinal);
            foreach (var profile in catalog.rail_profiles)
            {
                if (string.IsNullOrEmpty(profile.id) || !seenProfiles.Add(profile.id))
                    throw new InvalidOperationException($"Duplicate or empty rail profile id: '{profile?.id}'");
                if (profile.roughness_good_threshold < 0f || profile.roughness_bad_threshold < profile.roughness_good_threshold)
                    throw new InvalidOperationException($"rail profile '{profile.id}' thresholds are inverted");
            }
            return catalog;
        }
    }

    /// <summary>Mapping helpers from catalog DTOs onto engine definitions.</summary>
    public static class RailGrindingCatalogMapping
    {
        public static IEnumerable<RailGrindingHeadDef> ToHeadDefs(this RailGrindingCatalog catalog)
        {
            foreach (var h in catalog.grinding_heads)
            {
                yield return new RailGrindingHeadDef
                {
                    Id = h.id,
                    DisplayName = string.IsNullOrEmpty(h.display_name) ? h.display_name_key : h.display_name,
                    CompatibleVehicleTags = new List<string>(h.compatible_vehicle_tags),
                    NominalRpm = h.nominal_rpm,
                    BaseWorkRateKmPerHour = h.base_work_rate_km_per_hour,
                    StoneWearPerKm = h.stone_wear_per_km,
                    WaterSuppressionPerKm = h.water_suppression_per_km,
                    TargetRoughness = h.target_roughness,
                    MaxSpeedUpgradeKph = h.max_speed_upgrade_kph,
                    SparkHazardBase = h.spark_hazard_base,
                    StoneShatterBase = h.stone_shatter_base
                };
            }
        }
    }
}
