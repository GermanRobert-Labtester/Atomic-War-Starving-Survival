// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Ashfall.Core.Expeditions
{
    /// <summary>
    /// Authored amphibious draisine kit profile (Plan 125 Phase 2).
    /// Fictionalized vehicle-kit capability — flotation, current tolerance,
    /// ingress risk, pumps, deployment. Water routes stay topology-owned;
    /// open-water naval travel stays with the naval authority.
    /// </summary>
    public sealed class AmphibiousKitProfile
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public List<string> compatible_vehicle_tags { get; set; } = new List<string>();
        public float flotation_rating { get; set; }
        public float current_tolerance { get; set; }
        public int cargo_capacity_modifier_bp { get; set; }
        public int water_speed_modifier_bp { get; set; }
        public int deployment_ticks { get; set; }
        public float mass_kg { get; set; }
        public int min_vehicle_condition_bp { get; set; }
        public string pump_profile_id { get; set; } = string.Empty;
        public List<string> install_item_ids { get; set; } = new List<string>();
        public List<string> repair_item_ids { get; set; } = new List<string>();
        public string maintenance_profile_id { get; set; } = string.Empty;
        public List<string> route_class_ids { get; set; } = new List<string>();
        public List<string> tags { get; set; } = new List<string>();

        public bool Validate(out string error)
        {
            error = string.Empty;
            if (string.IsNullOrWhiteSpace(id)) { error = "id is required"; return false; }
            if (compatible_vehicle_tags.Count == 0) { error = "compatible_vehicle_tags is required"; return false; }
            if (flotation_rating <= 0f || flotation_rating > 1f) { error = "flotation_rating out of (0,1]"; return false; }
            if (current_tolerance <= 0f || current_tolerance > 1f) { error = "current_tolerance out of (0,1]"; return false; }
            if (cargo_capacity_modifier_bp <= 0 || cargo_capacity_modifier_bp > 10000) { error = "cargo_capacity_modifier_bp out of (0,10000]"; return false; }
            if (water_speed_modifier_bp <= 0 || water_speed_modifier_bp > 10000) { error = "water_speed_modifier_bp out of (0,10000]"; return false; }
            if (deployment_ticks <= 0) { error = "deployment_ticks must be > 0"; return false; }
            if (mass_kg <= 0f) { error = "mass_kg must be > 0"; return false; }
            if (min_vehicle_condition_bp < 0 || min_vehicle_condition_bp > 10000) { error = "min_vehicle_condition_bp out of [0,10000]"; return false; }
            if (string.IsNullOrWhiteSpace(pump_profile_id)) { error = "pump_profile_id is required"; return false; }
            if (route_class_ids.Count == 0) { error = "route_class_ids is required (kits unlock only authored route classes)"; return false; }
            return true;
        }
    }

    public sealed class AmphibiousPumpProfile
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public int ingress_mitigation_bp { get; set; }
        public string power_source { get; set; } = string.Empty;
        public List<string> repair_item_ids { get; set; } = new List<string>();

        public bool Validate(out string error)
        {
            error = string.Empty;
            if (string.IsNullOrWhiteSpace(id)) { error = "id is required"; return false; }
            if (ingress_mitigation_bp < 0 || ingress_mitigation_bp > 10000) { error = "ingress_mitigation_bp out of [0,10000]"; return false; }
            if (power_source != "manual" && power_source != "battery" && power_source != "fuel")
            { error = $"power_source '{power_source}' is not a known source (manual|battery|fuel)"; return false; }
            return true;
        }
    }

    public sealed class AmphibiousRouteClassProfile
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public float max_current_risk { get; set; }
        public float min_flotation_rating { get; set; }
        public List<string> tags { get; set; } = new List<string>();

        public bool Validate(out string error)
        {
            error = string.Empty;
            if (string.IsNullOrWhiteSpace(id)) { error = "id is required"; return false; }
            if (max_current_risk <= 0f || max_current_risk > 1f) { error = "max_current_risk out of (0,1]"; return false; }
            if (min_flotation_rating <= 0f || min_flotation_rating > 1f) { error = "min_flotation_rating out of (0,1]"; return false; }
            return true;
        }
    }

    public sealed class AmphibiousMaintenanceProfile
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
    /// Amphibious draisine catalog root (Plan 125 Phase 2). Loaders/validators
    /// only; the capability engine arrives in Phase 6.
    /// </summary>
    public sealed class AmphibiousDraisineCatalog
    {
        public int schema_version { get; set; } = 1;
        public string description { get; set; } = string.Empty;
        public List<AmphibiousKitProfile> kit_profiles { get; set; } = new List<AmphibiousKitProfile>();
        public List<AmphibiousPumpProfile> pump_profiles { get; set; } = new List<AmphibiousPumpProfile>();
        public List<AmphibiousRouteClassProfile> route_class_profiles { get; set; } = new List<AmphibiousRouteClassProfile>();
        public List<AmphibiousMaintenanceProfile> repair_profiles { get; set; } = new List<AmphibiousMaintenanceProfile>();

        private readonly Dictionary<string, AmphibiousKitProfile> _kits = new(StringComparer.Ordinal);
        private readonly Dictionary<string, AmphibiousPumpProfile> _pumps = new(StringComparer.Ordinal);
        private readonly Dictionary<string, AmphibiousRouteClassProfile> _routeClasses = new(StringComparer.Ordinal);
        private readonly Dictionary<string, AmphibiousMaintenanceProfile> _repair = new(StringComparer.Ordinal);

        public void Index()
        {
            _kits.Clear();
            foreach (var d in kit_profiles) if (d != null && !string.IsNullOrEmpty(d.id)) _kits[d.id] = d;
            _pumps.Clear();
            foreach (var d in pump_profiles) if (d != null && !string.IsNullOrEmpty(d.id)) _pumps[d.id] = d;
            _routeClasses.Clear();
            foreach (var d in route_class_profiles) if (d != null && !string.IsNullOrEmpty(d.id)) _routeClasses[d.id] = d;
            _repair.Clear();
            foreach (var d in repair_profiles) if (d != null && !string.IsNullOrEmpty(d.id)) _repair[d.id] = d;
        }

        public AmphibiousKitProfile? GetKit(string id) => string.IsNullOrEmpty(id) ? null : _kits.TryGetValue(id, out var v) ? v : null;
        public AmphibiousPumpProfile? GetPump(string id) => string.IsNullOrEmpty(id) ? null : _pumps.TryGetValue(id, out var v) ? v : null;
        public AmphibiousRouteClassProfile? GetRouteClass(string id) => string.IsNullOrEmpty(id) ? null : _routeClasses.TryGetValue(id, out var v) ? v : null;
        public AmphibiousMaintenanceProfile? GetRepair(string id) => string.IsNullOrEmpty(id) ? null : _repair.TryGetValue(id, out var v) ? v : null;
        public IReadOnlyCollection<AmphibiousKitProfile> Kits => _kits.Values;
        public IReadOnlyCollection<AmphibiousRouteClassProfile> RouteClasses => _routeClasses.Values;

        /// <summary>
        /// Structural validation: duplicate-ID rejection, per-row ranges, and
        /// internal foreign keys (kit→pump, kit→route_class). Route classes
        /// keep water traversal topology-owned — a kit unlocks only the
        /// route classes listed here, never raw water tiles.
        /// </summary>
        public bool ValidateCatalog(out string error)
        {
            error = string.Empty;
            if (schema_version != 1) { error = "unsupported schema_version"; return false; }

            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var d in kit_profiles)
            {
                if (d == null || !d.Validate(out error)) return false;
                if (!seen.Add("kit:" + d.id)) { error = $"duplicate kit id '{d.id}'"; return false; }
            }
            foreach (var d in pump_profiles)
            {
                if (d == null || !d.Validate(out error)) return false;
                if (!seen.Add("pump:" + d.id)) { error = $"duplicate pump id '{d.id}'"; return false; }
            }
            foreach (var d in route_class_profiles)
            {
                if (d == null || !d.Validate(out error)) return false;
                if (!seen.Add("route:" + d.id)) { error = $"duplicate route_class id '{d.id}'"; return false; }
            }
            foreach (var d in repair_profiles)
            {
                if (d == null || !d.Validate(out error)) return false;
                if (!seen.Add("repair:" + d.id)) { error = $"duplicate repair_profile id '{d.id}'"; return false; }
            }

            foreach (var kit in kit_profiles)
            {
                if (GetPump(kit.pump_profile_id) == null)
                { error = $"kit '{kit.id}': unresolved pump_profile_id '{kit.pump_profile_id}'"; return false; }
                if (!string.IsNullOrEmpty(kit.maintenance_profile_id) && GetRepair(kit.maintenance_profile_id) == null)
                { error = $"kit '{kit.id}': unresolved maintenance_profile_id '{kit.maintenance_profile_id}'"; return false; }
                foreach (var routeClassId in kit.route_class_ids)
                {
                    if (GetRouteClass(routeClassId) == null)
                    { error = $"kit '{kit.id}': unresolved route_class_id '{routeClassId}'"; return false; }
                    var routeClass = GetRouteClass(routeClassId)!;
                    // A kit must be able to actually clear the route classes it
                    // unlocks: flotation and current tolerance must clear the
                    // class thresholds, otherwise the binding is incoherent.
                    if (kit.flotation_rating < routeClass.min_flotation_rating)
                    { error = $"kit '{kit.id}': flotation {kit.flotation_rating} below route_class '{routeClassId}' minimum {routeClass.min_flotation_rating}"; return false; }
                    if (kit.current_tolerance < routeClass.max_current_risk)
                    { error = $"kit '{kit.id}': current_tolerance {kit.current_tolerance} below route_class '{routeClassId}' risk {routeClass.max_current_risk}"; return false; }
                }
            }
            return true;
        }
    }

    public static class AmphibiousDraisineCatalogLoader
    {
        public const string DefaultFileName = "amphibious_draisine_catalog.json";

        public static AmphibiousDraisineCatalog Load(string dataDir, IFileIO fileIo)
        {
            if (fileIo == null || string.IsNullOrEmpty(dataDir))
                return Empty();

            string path = fileIo.Combine(dataDir, DefaultFileName);
            if (!fileIo.FileExists(path))
                return Empty();

            string json = fileIo.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(json))
                return Empty();

            var catalog = JsonSerializer.Deserialize<AmphibiousDraisineCatalog>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? Empty();

            catalog.Index();
            return catalog;
        }

        private static AmphibiousDraisineCatalog Empty()
        {
            var catalog = new AmphibiousDraisineCatalog();
            catalog.Index();
            return catalog;
        }
    }
}
