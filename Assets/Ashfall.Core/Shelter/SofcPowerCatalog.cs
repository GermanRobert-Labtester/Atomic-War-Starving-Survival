// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Ashfall.Core.Shelter
{
    /// <summary>
    /// Authored SOFC plant profile (Plan 122 Phase 2). Gameplay abstractions
    /// only — fuel quality, thermal band, degradation, waste heat, acoustic
    /// signature class. No real electrochemistry or process procedures.
    /// </summary>
    public sealed class SofcThermalBand
    {
        public float minimum { get; set; }
        public float optimal_min { get; set; }
        public float optimal_max { get; set; }
        public float maximum { get; set; }

        public bool Validate(out string error)
        {
            error = string.Empty;
            if (minimum < 0f || minimum > 1f) { error = "thermal_band.minimum out of [0,1]"; return false; }
            if (optimal_min < 0f || optimal_min > 1f) { error = "thermal_band.optimal_min out of [0,1]"; return false; }
            if (optimal_max < 0f || optimal_max > 1f) { error = "thermal_band.optimal_max out of [0,1]"; return false; }
            if (maximum < 0f || maximum > 1f) { error = "thermal_band.maximum out of [0,1]"; return false; }
            if (!(minimum <= optimal_min && optimal_min <= optimal_max && optimal_max <= maximum))
            { error = "thermal_band must be ordered minimum<=optimal_min<=optimal_max<=maximum"; return false; }
            return true;
        }
    }

    public sealed class SofcStackProfile
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string fuel_profile_id { get; set; } = string.Empty;
        public string grade_profile_id { get; set; } = string.Empty;
        public string waste_heat_profile_id { get; set; } = string.Empty;
        public string maintenance_profile_id { get; set; } = string.Empty;
        public float rated_power_kw { get; set; }
        public float fuel_efficiency { get; set; }
        public float waste_heat_units_per_tick { get; set; }
        public int startup_ticks { get; set; }
        public int cooldown_ticks { get; set; }
        public SofcThermalBand thermal_band { get; set; } = new SofcThermalBand();
        public int degradation_per_operating_tick_bp { get; set; }
        public string acoustic_signature_class { get; set; } = string.Empty;
        public List<string> install_item_ids { get; set; } = new List<string>();
        public List<string> repair_item_ids { get; set; } = new List<string>();
        public List<string> tags { get; set; } = new List<string>();

        public bool Validate(out string error)
        {
            error = string.Empty;
            if (string.IsNullOrWhiteSpace(id)) { error = "id is required"; return false; }
            if (rated_power_kw <= 0f || rated_power_kw > 200f) { error = "rated_power_kw out of (0,200]"; return false; }
            if (fuel_efficiency <= 0f || fuel_efficiency >= 1f) { error = "fuel_efficiency out of (0,1)"; return false; }
            if (waste_heat_units_per_tick < 0f) { error = "waste_heat_units_per_tick must be >= 0"; return false; }
            if (startup_ticks <= 0) { error = "startup_ticks must be > 0"; return false; }
            if (cooldown_ticks <= 0) { error = "cooldown_ticks must be > 0"; return false; }
            if (degradation_per_operating_tick_bp < 0) { error = "degradation_per_operating_tick_bp must be >= 0"; return false; }
            if (!SofcPowerCatalog.SignatureClasses.Contains(acoustic_signature_class))
            { error = $"acoustic_signature_class '{acoustic_signature_class}' is not a known class"; return false; }
            if (string.IsNullOrWhiteSpace(fuel_profile_id)) { error = "fuel_profile_id is required"; return false; }
            if (thermal_band == null || !thermal_band.Validate(out error)) { error ??= "thermal_band invalid"; return false; }
            return true;
        }
    }

    public sealed class SofcFuelProfile
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string quality_class { get; set; } = string.Empty;
        public int output_modifier_bp { get; set; }
        public int degradation_multiplier_bp { get; set; }
        public int fault_risk_bp { get; set; }
        public List<string> feedstock_item_ids { get; set; } = new List<string>();

        public bool Validate(out string error)
        {
            error = string.Empty;
            if (string.IsNullOrWhiteSpace(id)) { error = "id is required"; return false; }
            if (!SofcPowerCatalog.FuelQualityClasses.Contains(quality_class))
            { error = $"quality_class '{quality_class}' is not a known class"; return false; }
            if (output_modifier_bp <= 0 || output_modifier_bp > 10000) { error = "output_modifier_bp out of (0,10000]"; return false; }
            if (degradation_multiplier_bp <= 0 || degradation_multiplier_bp > 50000) { error = "degradation_multiplier_bp out of (0,50000] (bp scale: 10000 = 1.0x)"; return false; }
            if (fault_risk_bp < 0 || fault_risk_bp > 10000) { error = "fault_risk_bp out of [0,10000]"; return false; }
            return true;
        }
    }

    public sealed class SofcGradeProfile
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public int stack_health_bonus_bp { get; set; }
        public int seal_integrity_bonus_bp { get; set; }

        public bool Validate(out string error)
        {
            error = string.Empty;
            if (string.IsNullOrWhiteSpace(id)) { error = "id is required"; return false; }
            if (stack_health_bonus_bp < 0 || stack_health_bonus_bp > 5000) { error = "stack_health_bonus_bp out of [0,5000]"; return false; }
            if (seal_integrity_bonus_bp < 0 || seal_integrity_bonus_bp > 5000) { error = "seal_integrity_bonus_bp out of [0,5000]"; return false; }
            return true;
        }
    }

    public sealed class SofcWasteHeatProfile
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public float heat_kw_per_unit { get; set; }
        public float max_allocatable_kw { get; set; }

        public bool Validate(out string error)
        {
            error = string.Empty;
            if (string.IsNullOrWhiteSpace(id)) { error = "id is required"; return false; }
            if (heat_kw_per_unit <= 0f) { error = "heat_kw_per_unit must be > 0"; return false; }
            if (max_allocatable_kw <= 0f) { error = "max_allocatable_kw must be > 0"; return false; }
            return true;
        }
    }

    public sealed class SofcMaintenanceProfile
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
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
    /// SOFC power catalog root (Plan 122 Phase 2). Loaders/validators only —
    /// the simulation engine arrives in Phase 3 and consumes this catalog.
    /// </summary>
    public sealed class SofcPowerCatalog
    {
        public const string SignatureVeryLow = "very_low";
        public const string SignatureLow = "low";
        public const string SignatureMedium = "medium";
        public const string SignatureHigh = "high";
        public const string QualityDirty = "dirty";
        public const string QualityTreated = "treated";
        public const string QualityClean = "clean";

        public static readonly IReadOnlyList<string> SignatureClasses = new[]
        {
            SignatureVeryLow, SignatureLow, SignatureMedium, SignatureHigh
        };

        public static readonly IReadOnlyList<string> FuelQualityClasses = new[]
        {
            QualityDirty, QualityTreated, QualityClean
        };

        public int schema_version { get; set; } = 1;
        public string description { get; set; } = string.Empty;
        public List<string> acoustic_signature_classes { get; set; } = new List<string>();
        public List<string> fuel_quality_classes { get; set; } = new List<string>();
        public List<SofcStackProfile> stack_profiles { get; set; } = new List<SofcStackProfile>();
        public List<SofcFuelProfile> fuel_profiles { get; set; } = new List<SofcFuelProfile>();
        public List<SofcGradeProfile> grade_profiles { get; set; } = new List<SofcGradeProfile>();
        public List<SofcWasteHeatProfile> waste_heat_profiles { get; set; } = new List<SofcWasteHeatProfile>();
        public List<SofcMaintenanceProfile> maintenance_profiles { get; set; } = new List<SofcMaintenanceProfile>();

        private readonly Dictionary<string, SofcStackProfile> _stacks = new(StringComparer.Ordinal);
        private readonly Dictionary<string, SofcFuelProfile> _fuels = new(StringComparer.Ordinal);
        private readonly Dictionary<string, SofcGradeProfile> _grades = new(StringComparer.Ordinal);
        private readonly Dictionary<string, SofcWasteHeatProfile> _wasteHeat = new(StringComparer.Ordinal);
        private readonly Dictionary<string, SofcMaintenanceProfile> _maintenance = new(StringComparer.Ordinal);

        public void Index()
        {
            _stacks.Clear();
            foreach (var d in stack_profiles) if (d != null && !string.IsNullOrEmpty(d.id)) _stacks[d.id] = d;
            _fuels.Clear();
            foreach (var d in fuel_profiles) if (d != null && !string.IsNullOrEmpty(d.id)) _fuels[d.id] = d;
            _grades.Clear();
            foreach (var d in grade_profiles) if (d != null && !string.IsNullOrEmpty(d.id)) _grades[d.id] = d;
            _wasteHeat.Clear();
            foreach (var d in waste_heat_profiles) if (d != null && !string.IsNullOrEmpty(d.id)) _wasteHeat[d.id] = d;
            _maintenance.Clear();
            foreach (var d in maintenance_profiles) if (d != null && !string.IsNullOrEmpty(d.id)) _maintenance[d.id] = d;
        }

        public SofcStackProfile? GetStack(string id) => string.IsNullOrEmpty(id) ? null : _stacks.TryGetValue(id, out var v) ? v : null;
        public SofcFuelProfile? GetFuel(string id) => string.IsNullOrEmpty(id) ? null : _fuels.TryGetValue(id, out var v) ? v : null;
        public SofcGradeProfile? GetGrade(string id) => string.IsNullOrEmpty(id) ? null : _grades.TryGetValue(id, out var v) ? v : null;
        public SofcWasteHeatProfile? GetWasteHeat(string id) => string.IsNullOrEmpty(id) ? null : _wasteHeat.TryGetValue(id, out var v) ? v : null;
        public SofcMaintenanceProfile? GetMaintenance(string id) => string.IsNullOrEmpty(id) ? null : _maintenance.TryGetValue(id, out var v) ? v : null;
        public IReadOnlyCollection<SofcStackProfile> Stacks => _stacks.Values;

        /// <summary>
        /// Structural validation: duplicate-ID rejection, per-row ranges, and
        /// internal foreign keys. Item-ID foreign keys are enforced by the
        /// shared catalog integrity walk; only non-empty internal refs here.
        /// </summary>
        public bool ValidateCatalog(out string error)
        {
            error = string.Empty;
            if (schema_version != 1) { error = "unsupported schema_version"; return false; }

            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var d in stack_profiles)
            {
                if (d == null || !d.Validate(out error)) return false;
                if (!seen.Add("stack:" + d.id)) { error = $"duplicate stack id '{d.id}'"; return false; }
            }
            foreach (var d in fuel_profiles)
            {
                if (d == null || !d.Validate(out error)) return false;
                if (!seen.Add("fuel:" + d.id)) { error = $"duplicate fuel id '{d.id}'"; return false; }
            }
            foreach (var d in grade_profiles)
            {
                if (d == null || !d.Validate(out error)) return false;
                if (!seen.Add("grade:" + d.id)) { error = $"duplicate grade id '{d.id}'"; return false; }
            }
            foreach (var d in waste_heat_profiles)
            {
                if (d == null || !d.Validate(out error)) return false;
                if (!seen.Add("heat:" + d.id)) { error = $"duplicate waste_heat id '{d.id}'"; return false; }
            }
            foreach (var d in maintenance_profiles)
            {
                if (d == null || !d.Validate(out error)) return false;
                if (!seen.Add("maint:" + d.id)) { error = $"duplicate maintenance id '{d.id}'"; return false; }
            }

            foreach (var stack in stack_profiles)
            {
                if (GetFuel(stack.fuel_profile_id) == null) { error = $"stack '{stack.id}': unresolved fuel_profile_id '{stack.fuel_profile_id}'"; return false; }
                if (!string.IsNullOrEmpty(stack.grade_profile_id) && GetGrade(stack.grade_profile_id) == null)
                { error = $"stack '{stack.id}': unresolved grade_profile_id '{stack.grade_profile_id}'"; return false; }
                if (!string.IsNullOrEmpty(stack.waste_heat_profile_id) && GetWasteHeat(stack.waste_heat_profile_id) == null)
                { error = $"stack '{stack.id}': unresolved waste_heat_profile_id '{stack.waste_heat_profile_id}'"; return false; }
                if (!string.IsNullOrEmpty(stack.maintenance_profile_id) && GetMaintenance(stack.maintenance_profile_id) == null)
                { error = $"stack '{stack.id}': unresolved maintenance_profile_id '{stack.maintenance_profile_id}'"; return false; }
            }
            return true;
        }
    }

    public static class SofcPowerCatalogLoader
    {
        public const string DefaultFileName = "sofc_power_catalog.json";

        public static SofcPowerCatalog Load(string dataDir, IFileIO fileIo)
        {
            if (fileIo == null || string.IsNullOrEmpty(dataDir))
                return Empty();

            string path = fileIo.Combine(dataDir, DefaultFileName);
            if (!fileIo.FileExists(path))
                return Empty();

            string json = fileIo.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(json))
                return Empty();

            var catalog = JsonSerializer.Deserialize<SofcPowerCatalog>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? Empty();

            catalog.Index();
            return catalog;
        }

        private static SofcPowerCatalog Empty()
        {
            var catalog = new SofcPowerCatalog();
            catalog.Index();
            return catalog;
        }
    }
}
