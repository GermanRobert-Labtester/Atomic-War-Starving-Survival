// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Ashfall.Core.Combat
{
    /// <summary>
    /// Authored sound-ranging profile (Plan 123 Phase 2). Defensive
    /// early-warning values only — bearing error, region radius, confidence
    /// caps, weather sensitivity. This catalog intentionally carries NO
    /// fire-control or weapon-targeting schema.
    /// </summary>
    public sealed class SoundRangingArrayProfile
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string sensor_profile_id { get; set; } = string.Empty;
        public int sensor_count { get; set; }
        public float base_bearing_error_deg { get; set; }
        public int base_region_radius_cells { get; set; }
        public float timing_quality { get; set; }
        public float weather_sensitivity { get; set; }
        public int maintenance_drift_per_day_bp { get; set; }
        public int confidence_cap_bp { get; set; }
        public List<string> install_item_ids { get; set; } = new List<string>();
        public List<string> repair_item_ids { get; set; } = new List<string>();
        public string maintenance_profile_id { get; set; } = string.Empty;
        public List<string> tags { get; set; } = new List<string>();

        public bool Validate(out string error)
        {
            error = string.Empty;
            if (string.IsNullOrWhiteSpace(id)) { error = "id is required"; return false; }
            if (sensor_count < 1 || sensor_count > 32) { error = "sensor_count out of [1,32]"; return false; }
            if (base_bearing_error_deg <= 0f || base_bearing_error_deg > 90f) { error = "base_bearing_error_deg out of (0,90]"; return false; }
            if (base_region_radius_cells < 1 || base_region_radius_cells > 64) { error = "base_region_radius_cells out of [1,64]"; return false; }
            if (timing_quality <= 0f || timing_quality > 1f) { error = "timing_quality out of (0,1]"; return false; }
            if (weather_sensitivity < 0f || weather_sensitivity > 1f) { error = "weather_sensitivity out of [0,1]"; return false; }
            if (maintenance_drift_per_day_bp < 0) { error = "maintenance_drift_per_day_bp must be >= 0"; return false; }
            if (confidence_cap_bp <= 0 || confidence_cap_bp > 10000) { error = "confidence_cap_bp out of (0,10000]"; return false; }
            if (string.IsNullOrWhiteSpace(sensor_profile_id)) { error = "sensor_profile_id is required"; return false; }
            return true;
        }
    }

    public sealed class SoundRangingSensorProfile
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public int reliability_bp { get; set; }
        public List<string> install_item_ids { get; set; } = new List<string>();
        public List<string> repair_item_ids { get; set; } = new List<string>();

        public bool Validate(out string error)
        {
            error = string.Empty;
            if (string.IsNullOrWhiteSpace(id)) { error = "id is required"; return false; }
            if (reliability_bp <= 0 || reliability_bp > 10000) { error = "reliability_bp out of (0,10000]"; return false; }
            return true;
        }
    }

    public sealed class SoundTimingQualityProfile
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public int quality_bp { get; set; }
        public int drift_per_day_bp { get; set; }

        public bool Validate(out string error)
        {
            error = string.Empty;
            if (string.IsNullOrWhiteSpace(id)) { error = "id is required"; return false; }
            if (quality_bp <= 0 || quality_bp > 10000) { error = "quality_bp out of (0,10000]"; return false; }
            if (drift_per_day_bp < 0) { error = "drift_per_day_bp must be >= 0"; return false; }
            return true;
        }
    }

    public sealed class SoundAtmosphericErrorProfile
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public int error_multiplier_bp { get; set; }

        public bool Validate(out string error)
        {
            error = string.Empty;
            if (string.IsNullOrWhiteSpace(id)) { error = "id is required"; return false; }
            if (error_multiplier_bp <= 0 || error_multiplier_bp > 50000) { error = "error_multiplier_bp out of (0,50000] (bp scale: 10000 = 1.0x)"; return false; }
            return true;
        }
    }

    public sealed class SoundSourceClassSignature
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public float bearing_error_bonus_deg { get; set; }
        public int region_radius_bonus_cells { get; set; }

        public bool Validate(out string error)
        {
            error = string.Empty;
            if (string.IsNullOrWhiteSpace(id)) { error = "id is required"; return false; }
            if (bearing_error_bonus_deg < 0f || bearing_error_bonus_deg > 45f) { error = "bearing_error_bonus_deg out of [0,45]"; return false; }
            if (region_radius_bonus_cells < 0 || region_radius_bonus_cells > 16) { error = "region_radius_bonus_cells out of [0,16]"; return false; }
            return true;
        }
    }

    public sealed class SoundMaintenanceProfile
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public int inspection_interval_days { get; set; }
        public int repair_skill_minimum { get; set; }

        public bool Validate(out string error)
        {
            error = string.Empty;
            if (string.IsNullOrWhiteSpace(id)) { error = "id is required"; return false; }
            if (inspection_interval_days <= 0) { error = "inspection_interval_days must be > 0"; return false; }
            if (repair_skill_minimum < 0 || repair_skill_minimum > 100) { error = "repair_skill_minimum out of [0,100]"; return false; }
            return true;
        }
    }

    /// <summary>
    /// Sound-ranging catalog root (Plan 123 Phase 2). Defensive-warning
    /// balance values only. Loaders/validators only; the threat engine
    /// arrives in Phase 5.
    /// </summary>
    public sealed class SoundRangingCatalog
    {
        public int schema_version { get; set; } = 1;
        public string description { get; set; } = string.Empty;
        public List<SoundRangingArrayProfile> array_profiles { get; set; } = new List<SoundRangingArrayProfile>();
        public List<SoundRangingSensorProfile> sensor_profiles { get; set; } = new List<SoundRangingSensorProfile>();
        public List<SoundTimingQualityProfile> timing_quality_profiles { get; set; } = new List<SoundTimingQualityProfile>();
        public List<SoundAtmosphericErrorProfile> atmospheric_error_profiles { get; set; } = new List<SoundAtmosphericErrorProfile>();
        public List<SoundSourceClassSignature> source_class_signatures { get; set; } = new List<SoundSourceClassSignature>();
        public List<SoundMaintenanceProfile> maintenance_profiles { get; set; } = new List<SoundMaintenanceProfile>();

        private readonly Dictionary<string, SoundRangingArrayProfile> _arrays = new(StringComparer.Ordinal);
        private readonly Dictionary<string, SoundRangingSensorProfile> _sensors = new(StringComparer.Ordinal);
        private readonly Dictionary<string, SoundTimingQualityProfile> _timing = new(StringComparer.Ordinal);
        private readonly Dictionary<string, SoundAtmosphericErrorProfile> _atmos = new(StringComparer.Ordinal);
        private readonly Dictionary<string, SoundSourceClassSignature> _classes = new(StringComparer.Ordinal);
        private readonly Dictionary<string, SoundMaintenanceProfile> _maintenance = new(StringComparer.Ordinal);

        public void Index()
        {
            _arrays.Clear();
            foreach (var d in array_profiles) if (d != null && !string.IsNullOrEmpty(d.id)) _arrays[d.id] = d;
            _sensors.Clear();
            foreach (var d in sensor_profiles) if (d != null && !string.IsNullOrEmpty(d.id)) _sensors[d.id] = d;
            _timing.Clear();
            foreach (var d in timing_quality_profiles) if (d != null && !string.IsNullOrEmpty(d.id)) _timing[d.id] = d;
            _atmos.Clear();
            foreach (var d in atmospheric_error_profiles) if (d != null && !string.IsNullOrEmpty(d.id)) _atmos[d.id] = d;
            _classes.Clear();
            foreach (var d in source_class_signatures) if (d != null && !string.IsNullOrEmpty(d.id)) _classes[d.id] = d;
            _maintenance.Clear();
            foreach (var d in maintenance_profiles) if (d != null && !string.IsNullOrEmpty(d.id)) _maintenance[d.id] = d;
        }

        public SoundRangingArrayProfile? GetArray(string id) => string.IsNullOrEmpty(id) ? null : _arrays.TryGetValue(id, out var v) ? v : null;
        public SoundRangingSensorProfile? GetSensor(string id) => string.IsNullOrEmpty(id) ? null : _sensors.TryGetValue(id, out var v) ? v : null;
        public SoundTimingQualityProfile? GetTiming(string id) => string.IsNullOrEmpty(id) ? null : _timing.TryGetValue(id, out var v) ? v : null;
        public SoundAtmosphericErrorProfile? GetAtmospheric(string id) => string.IsNullOrEmpty(id) ? null : _atmos.TryGetValue(id, out var v) ? v : null;
        public SoundSourceClassSignature? GetSourceClass(string id) => string.IsNullOrEmpty(id) ? null : _classes.TryGetValue(id, out var v) ? v : null;
        public SoundMaintenanceProfile? GetMaintenance(string id) => string.IsNullOrEmpty(id) ? null : _maintenance.TryGetValue(id, out var v) ? v : null;
        public IReadOnlyCollection<SoundRangingArrayProfile> Arrays => _arrays.Values;

        /// <summary>
        /// Structural validation: duplicate-ID rejection, per-row ranges, and
        /// internal foreign keys. There is deliberately no targeting-output
        /// section to validate — the schema has none.
        /// </summary>
        public bool ValidateCatalog(out string error)
        {
            error = string.Empty;
            if (schema_version != 1) { error = "unsupported schema_version"; return false; }

            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var d in array_profiles)
            {
                if (d == null || !d.Validate(out error)) return false;
                if (!seen.Add("array:" + d.id)) { error = $"duplicate array id '{d.id}'"; return false; }
            }
            foreach (var d in sensor_profiles)
            {
                if (d == null || !d.Validate(out error)) return false;
                if (!seen.Add("sensor:" + d.id)) { error = $"duplicate sensor id '{d.id}'"; return false; }
            }
            foreach (var d in timing_quality_profiles)
            {
                if (d == null || !d.Validate(out error)) return false;
                if (!seen.Add("timing:" + d.id)) { error = $"duplicate timing id '{d.id}'"; return false; }
            }
            foreach (var d in atmospheric_error_profiles)
            {
                if (d == null || !d.Validate(out error)) return false;
                if (!seen.Add("atmos:" + d.id)) { error = $"duplicate atmospheric id '{d.id}'"; return false; }
            }
            foreach (var d in source_class_signatures)
            {
                if (d == null || !d.Validate(out error)) return false;
                if (!seen.Add("class:" + d.id)) { error = $"duplicate source_class id '{d.id}'"; return false; }
            }
            foreach (var d in maintenance_profiles)
            {
                if (d == null || !d.Validate(out error)) return false;
                if (!seen.Add("maint:" + d.id)) { error = $"duplicate maintenance id '{d.id}'"; return false; }
            }

            foreach (var array in array_profiles)
            {
                if (GetSensor(array.sensor_profile_id) == null)
                { error = $"array '{array.id}': unresolved sensor_profile_id '{array.sensor_profile_id}'"; return false; }
                if (!string.IsNullOrEmpty(array.maintenance_profile_id) && GetMaintenance(array.maintenance_profile_id) == null)
                { error = $"array '{array.id}': unresolved maintenance_profile_id '{array.maintenance_profile_id}'"; return false; }
            }
            return true;
        }
    }

    public static class SoundRangingCatalogLoader
    {
        public const string DefaultFileName = "sound_ranging_catalog.json";

        public static SoundRangingCatalog Load(string dataDir, IFileIO fileIo)
        {
            if (fileIo == null || string.IsNullOrEmpty(dataDir))
                return Empty();

            string path = fileIo.Combine(dataDir, DefaultFileName);
            if (!fileIo.FileExists(path))
                return Empty();

            string json = fileIo.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(json))
                return Empty();

            var catalog = JsonSerializer.Deserialize<SoundRangingCatalog>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? Empty();

            catalog.Index();
            return catalog;
        }

        private static SoundRangingCatalog Empty()
        {
            var catalog = new SoundRangingCatalog();
            catalog.Index();
            return catalog;
        }
    }
}
