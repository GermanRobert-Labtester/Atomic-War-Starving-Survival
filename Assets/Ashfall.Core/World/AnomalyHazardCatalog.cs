// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 176 — Anomaly Hazard Catalog (anomalies.json).
// Authored definitions for moving radiation storm fronts and deep-zone
// anomaly zones. This catalog owns THREAT SHAPES only:
//   • movement profile (storm_front | wind_drift | static) and speed
//   • center radiation rate and falloff radius
//   • detection threshold, warning radius/profile, duration
//   • optional canonical loot-table reference and loot-site count
//   • bounded wildlife modifier (bp) consumed by the ecology layer
// It never owns dose accumulation (RadiationSystem), weather truth
// (WeatherSystem), or wildlife populations (WildlifeEcosystemSystem).
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Ashfall.Core.World
{
    /// <summary>One authored anomaly definition. All values bounded; loader enforces ranges.</summary>
    [Serializable]
    public sealed class AnomalyDefinition
    {
        public string anomaly_id { get; set; } = string.Empty;          // anomaly_*
        public string display_name { get; set; } = string.Empty;
        public string anomaly_type { get; set; } = string.Empty;        // thematic label, snake_case
        public List<string> spawn_region_tags { get; set; } = new List<string>();
        public string movement_profile { get; set; } = "static";        // storm_front | wind_drift | static
        public float movement_speed_kph { get; set; }                   // storm_front only; 0 otherwise
        public float wind_response { get; set; }                        // wind_drift multiplier; 0..3
        public float radiation_rate { get; set; }                       // rads/hr at center
        public float radius_km { get; set; }                            // falloff radius
        public int duration_days { get; set; }
        public float warning_radius_km { get; set; }                    // >= radius
        public string warning_profile { get; set; } = "standard";       // early | standard | late
        public float detection_threshold { get; set; }                  // rads/hr a device must resolve
        public string loot_table_id { get; set; } = string.Empty;       // canonical table_loot_* or empty
        public int loot_site_count { get; set; }                        // 0..3 gated sites
        public int wildlife_modifier_bp { get; set; }                   // -1000..1000 (avoidance < 0)
        public List<string> environmental_effect_tags { get; set; } = new List<string>();
        public List<string> tags { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class AnomalyCatalogRoot
    {
        public int schema_version { get; set; } = 1;
        public List<AnomalyDefinition> anomalies { get; set; } = new List<AnomalyDefinition>();
    }

    /// <summary>Load outcome: rows plus validation errors (domain result, no exceptions).</summary>
    public sealed class AnomalyCatalogLoadResult
    {
        public List<AnomalyDefinition> Anomalies { get; } = new List<AnomalyDefinition>();
        public List<string> Errors { get; } = new List<string>();
        public bool HasErrors => Errors.Count > 0;
    }

    /// <summary>
    /// Engine-agnostic loader for anomalies.json with load-time validation:
    /// duplicate ids, snake_case vocabulary, bounded numeric ranges,
    /// warning radius ≥ falloff radius, movement profile consistency
    /// (storm_front requires speed &gt; 0; static/wind_drift require 0).
    /// Errors are collected, never thrown.
    /// </summary>
    public static class AnomalyCatalogLoader
    {
        public const string FileName = "anomalies.json";
        public const int CurrentSchemaVersion = 1;

        public static readonly IReadOnlyList<string> AcceptedMovementProfiles =
            new[] { "storm_front", "wind_drift", "static" };

        public static readonly IReadOnlyList<string> AcceptedWarningProfiles =
            new[] { "early", "standard", "late" };

        public const float MaxRadiationRate = 400f;
        public const float MaxRadiusKm = 60f;
        public const float MaxMovementSpeedKph = 60f;
        public const float MaxWindResponse = 3f;
        public const int MaxDurationDays = 120;
        public const int MaxLootSites = 3;
        public const int MaxWildlifeModifierBp = 1000;

        private static readonly Regex SnakeCase = new Regex("^[a-z0-9_]+$", RegexOptions.Compiled);

        public static AnomalyCatalogLoadResult Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var result = new AnomalyCatalogLoadResult();
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
            {
                result.Errors.Add("loader requires dataDir, IFileIO and IJsonSerializer");
                return result;
            }

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
            {
                result.Errors.Add("catalog file missing: " + FileName);
                return result;
            }

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
            {
                result.Errors.Add("catalog file empty: " + FileName);
                return result;
            }

            AnomalyCatalogRoot root;
            try
            {
                root = json.Deserialize<AnomalyCatalogRoot>(raw);
            }
            catch (Exception e)
            {
                result.Errors.Add("catalog malformed JSON: " + e.Message);
                return result;
            }
            if (root == null)
            {
                result.Errors.Add("catalog parsed to null");
                return result;
            }
            if (root.schema_version > CurrentSchemaVersion)
            {
                result.Errors.Add($"schema_version {root.schema_version} newer than supported {CurrentSchemaVersion}");
                return result;
            }
            if (root.schema_version < 1)
            {
                result.Errors.Add($"schema_version {root.schema_version} invalid");
                return result;
            }
            if (root.anomalies == null || root.anomalies.Count == 0)
            {
                result.Errors.Add("catalog has no anomaly rows");
                return result;
            }

            var seenIds = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < root.anomalies.Count; i++)
            {
                var def = root.anomalies[i];
                string at = $"anomalies[{i}]";
                if (def == null)
                {
                    result.Errors.Add(at + ": null row");
                    continue;
                }

                if (string.IsNullOrEmpty(def.anomaly_id) || !SnakeCase.IsMatch(def.anomaly_id)
                    || !def.anomaly_id.StartsWith("anomaly_", StringComparison.Ordinal))
                {
                    result.Errors.Add(at + ": anomaly_id must be snake_case with anomaly_ prefix");
                    continue;
                }
                if (!seenIds.Add(def.anomaly_id))
                {
                    result.Errors.Add(at + ": duplicate anomaly_id " + def.anomaly_id);
                    continue;
                }
                if (string.IsNullOrWhiteSpace(def.display_name))
                    result.Errors.Add(at + " (" + def.anomaly_id + "): display_name required");
                if (string.IsNullOrWhiteSpace(def.anomaly_type) || !SnakeCase.IsMatch(def.anomaly_type))
                    result.Errors.Add(at + " (" + def.anomaly_id + "): anomaly_type must be snake_case");

                if (!AcceptedMovementProfiles.Contains(def.movement_profile))
                    result.Errors.Add(at + " (" + def.anomaly_id + "): movement_profile must be one of " +
                        string.Join("|", AcceptedMovementProfiles));
                if (def.movement_profile == "storm_front" && def.movement_speed_kph <= 0f)
                    result.Errors.Add(at + " (" + def.anomaly_id + "): storm_front requires movement_speed_kph > 0");
                if (def.movement_profile != "storm_front" && def.movement_speed_kph != 0f)
                    result.Errors.Add(at + " (" + def.anomaly_id + "): non-storm profiles require movement_speed_kph = 0");
                if (def.movement_profile == "static" && def.wind_response != 0f)
                    result.Errors.Add(at + " (" + def.anomaly_id + "): static profile requires wind_response = 0");
                if (def.movement_speed_kph < 0f || def.movement_speed_kph > MaxMovementSpeedKph)
                    result.Errors.Add(at + " (" + def.anomaly_id + "): movement_speed_kph out of range [0," + MaxMovementSpeedKph + "]");
                if (def.wind_response < 0f || def.wind_response > MaxWindResponse)
                    result.Errors.Add(at + " (" + def.anomaly_id + "): wind_response out of range [0," + MaxWindResponse + "]");

                if (def.radiation_rate < 0f || def.radiation_rate > MaxRadiationRate)
                    result.Errors.Add(at + " (" + def.anomaly_id + "): radiation_rate out of range [0," + MaxRadiationRate + "]");
                if (def.radius_km <= 0f || def.radius_km > MaxRadiusKm)
                    result.Errors.Add(at + " (" + def.anomaly_id + "): radius_km out of range (0," + MaxRadiusKm + "]");
                if (def.duration_days < 1 || def.duration_days > MaxDurationDays)
                    result.Errors.Add(at + " (" + def.anomaly_id + "): duration_days out of range [1," + MaxDurationDays + "]");
                if (def.warning_radius_km < def.radius_km)
                    result.Errors.Add(at + " (" + def.anomaly_id + "): warning_radius_km must be >= radius_km");
                if (def.detection_threshold < 0f || def.detection_threshold > MaxRadiationRate)
                    result.Errors.Add(at + " (" + def.anomaly_id + "): detection_threshold out of range [0," + MaxRadiationRate + "]");

                if (!AcceptedWarningProfiles.Contains(def.warning_profile))
                    result.Errors.Add(at + " (" + def.anomaly_id + "): warning_profile must be one of " +
                        string.Join("|", AcceptedWarningProfiles));

                if (!string.IsNullOrEmpty(def.loot_table_id))
                {
                    if (!SnakeCase.IsMatch(def.loot_table_id) || !def.loot_table_id.StartsWith("table_loot_", StringComparison.Ordinal))
                        result.Errors.Add(at + " (" + def.anomaly_id + "): loot_table_id must be a canonical table_loot_* id");
                    if (def.loot_site_count < 1)
                        result.Errors.Add(at + " (" + def.anomaly_id + "): loot_table_id requires loot_site_count >= 1");
                }
                if (def.loot_site_count < 0 || def.loot_site_count > MaxLootSites)
                    result.Errors.Add(at + " (" + def.anomaly_id + "): loot_site_count out of range [0," + MaxLootSites + "]");

                if (def.wildlife_modifier_bp < -MaxWildlifeModifierBp || def.wildlife_modifier_bp > MaxWildlifeModifierBp)
                    result.Errors.Add(at + " (" + def.anomaly_id + "): wildlife_modifier_bp out of range [-1000,1000]");

                if (def.spawn_region_tags == null || def.spawn_region_tags.Count == 0)
                    result.Errors.Add(at + " (" + def.anomaly_id + "): spawn_region_tags required");

                result.Anomalies.Add(def);
            }

            return result;
        }

        /// <summary>Index a successful load result into the immutable runtime catalog.
        /// Rows that failed validation are skipped (errors already recorded).</summary>
        public static AnomalyDefinitionCatalog ToCatalog(AnomalyCatalogLoadResult result)
        {
            var catalog = new AnomalyDefinitionCatalog();
            if (result == null) return catalog;
            foreach (var def in result.Anomalies)
                if (def != null && !string.IsNullOrEmpty(def.anomaly_id)) catalog.Add(def);
            return catalog;
        }
    }

    /// <summary>In-memory anomaly definition catalog, indexed by id. Immutable after load.</summary>
    public sealed class AnomalyDefinitionCatalog
    {
        private readonly Dictionary<string, AnomalyDefinition> _byId =
            new Dictionary<string, AnomalyDefinition>(StringComparer.Ordinal);

        public int Count => _byId.Count;
        public IReadOnlyDictionary<string, AnomalyDefinition> ById => _byId;

        public AnomalyDefinition? Find(string anomalyId)
        {
            return !string.IsNullOrEmpty(anomalyId) && _byId.TryGetValue(anomalyId, out var def) ? def : null;
        }

        internal void Add(AnomalyDefinition def) => _byId[def.anomaly_id] = def;
    }
}
