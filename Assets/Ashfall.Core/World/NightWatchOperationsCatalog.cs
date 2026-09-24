// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 36 — The Watch
// Authored operations catalog: posts, routes, gate rules, detection profiles,
// alarm protocols, and readiness drills.
//
// The catalog describes operational choices only. Territory, physical perimeter
// equipment, acoustic estimates, survivor eligibility, and gate locks remain
// owned by their existing systems.
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core.IO;

namespace Ashfall.Core.World
{
    [Serializable]
    public sealed class NightWatchPostDefinition
    {
        public string post_id = string.Empty;
        public string display_name = string.Empty;
        public string node_id = string.Empty;
        public string sector_id = string.Empty;
        public List<string> sightlines = new List<string>();
        public string shelter_from = string.Empty;
        public int capacity = 1;
        public int relief_hours = 4;
        public int initial_condition_permille = 1000;
        public int night_vision_bonus_permille = 0;
        public bool requires_power = false;
        public string required_capability = string.Empty;
    }

    [Serializable]
    public sealed class NightWatchRouteDefinition
    {
        public string route_id = string.Empty;
        public string display_name = string.Empty;
        public List<string> waypoint_location_ids = new List<string>();
        public double distance_km = 0.0;
        public double duration_hours = 0.0;
        public List<string> sector_ids = new List<string>();
        public int danger = 1;
        public bool requires_debrief = true;
        public string debrief_rule = string.Empty;
    }

    [Serializable]
    public sealed class NightWatchGateRuleDefinition
    {
        public string rule_id = string.Empty;
        public string display_name = string.Empty;
        public string applies_to = string.Empty;
        public string evidence = string.Empty;
        public string appeal = string.Empty;
        public bool active_by_default = true;
    }

    [Serializable]
    public sealed class NightWatchDetectionProfileDefinition
    {
        public string profile_id = string.Empty;
        public string display_name = string.Empty;
        public string channel = string.Empty;
        public int confidence_permille = 0;
        public string verification = string.Empty;
        public bool acoustic = false;
    }

    [Serializable]
    public sealed class NightWatchAlarmProtocolDefinition
    {
        public string protocol_id = string.Empty;
        public string display_name = string.Empty;
        public int trigger_confidence_permille = 0;
        public List<string> channels = new List<string>();
        public string response = string.Empty;
        public string drill_id = string.Empty;
    }

    [Serializable]
    public sealed class NightWatchDrillDefinition
    {
        public string drill_id = string.Empty;
        public string display_name = string.Empty;
        public List<string> participant_roles = new List<string>();
        public int target_minutes = 0;
        public List<string> failure_modes = new List<string>();
        public int readiness_effect_permille = 0;
    }

    [Serializable]
    public sealed class NightWatchOperationsCatalog
    {
        public int schema_version = 1;
        public string collection_id = "night_watch_operations";
        public List<NightWatchPostDefinition> posts = new List<NightWatchPostDefinition>();
        public List<NightWatchRouteDefinition> routes = new List<NightWatchRouteDefinition>();
        public List<NightWatchGateRuleDefinition> gate_rules = new List<NightWatchGateRuleDefinition>();
        public List<NightWatchDetectionProfileDefinition> detection_profiles = new List<NightWatchDetectionProfileDefinition>();
        public List<NightWatchAlarmProtocolDefinition> alarm_protocols = new List<NightWatchAlarmProtocolDefinition>();
        public List<NightWatchDrillDefinition> drills = new List<NightWatchDrillDefinition>();

        public NightWatchPostDefinition? Post(string id) => Find(posts, id, x => x.post_id);
        public NightWatchRouteDefinition? Route(string id) => Find(routes, id, x => x.route_id);
        public NightWatchGateRuleDefinition? GateRule(string id) => Find(gate_rules, id, x => x.rule_id);
        public NightWatchDetectionProfileDefinition? DetectionProfile(string id) => Find(detection_profiles, id, x => x.profile_id);
        public NightWatchAlarmProtocolDefinition? AlarmProtocol(string id) => Find(alarm_protocols, id, x => x.protocol_id);
        public NightWatchDrillDefinition? Drill(string id) => Find(drills, id, x => x.drill_id);

        private static T? Find<T>(List<T> rows, string id, Func<T, string> selector)
            where T : class
        {
            if (string.IsNullOrWhiteSpace(id)) return null;
            for (int i = 0; i < rows.Count; i++)
            {
                if (rows[i] != null && string.Equals(selector(rows[i]), id.Trim(), StringComparison.OrdinalIgnoreCase))
                    return rows[i];
            }
            return null;
        }
    }

    public sealed class NightWatchOperationsCatalogLoadResult
    {
        public bool Success => Errors.Count == 0;
        public NightWatchOperationsCatalog? Catalog { get; set; }
        public List<string> Errors { get; } = new List<string>();
    }

    /// <summary>
    /// Strict loader for the authored Watch operations catalog. It validates
    /// closed ranges, unique IDs, sector vocabulary, and references between
    /// alarm protocols and drills. World-location references are checked by the
    /// host through <see cref="ValidateWorldLocations"/> so this loader remains
    /// usable with isolated test fixtures.
    /// </summary>
    public static class NightWatchOperationsCatalogLoader
    {
        public const string DefaultFileName = "night_watch_operations.json";
        public static readonly IReadOnlyList<string> Sectors = new[] { "north", "east", "south", "west", "gate" };
        public static readonly IReadOnlyList<string> GateSubjects = new[] { "all", "visitor", "resident", "trader", "medical", "emergency" };
        public static readonly IReadOnlyList<string> DetectionChannels = new[] { "sound", "sight", "contact", "smell", "instrument", "any" };

        public static NightWatchOperationsCatalogLoadResult Load(string dataDir, IFileIO? fileIO = null)
        {
            fileIO ??= new FileSystemIO();
            var result = new NightWatchOperationsCatalogLoadResult();
            if (string.IsNullOrWhiteSpace(dataDir))
            {
                result.Errors.Add("Data directory is empty.");
                return result;
            }

            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
            {
                result.Errors.Add($"File not found: {path}");
                return result;
            }

            try
            {
                return LoadFromJson(fileIO.ReadAllText(path));
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Failed to read {path}: {ex.Message}");
                return result;
            }
        }

        public static NightWatchOperationsCatalogLoadResult LoadFromJson(string json)
        {
            var result = new NightWatchOperationsCatalogLoadResult();
            if (string.IsNullOrWhiteSpace(json))
            {
                result.Errors.Add("JSON content is empty.");
                return result;
            }

            try
            {
                var options = new JsonSerializerOptions
                {
                    IncludeFields = true,
                    PropertyNameCaseInsensitive = true
                };
                var catalog = JsonSerializer.Deserialize<NightWatchOperationsCatalog>(json, options);
                if (catalog == null)
                {
                    result.Errors.Add("Deserialized catalog is null.");
                    return result;
                }

                Validate(catalog, result.Errors);
                if (result.Errors.Count == 0) result.Catalog = catalog;
            }
            catch (Exception ex)
            {
                result.Errors.Add($"JSON deserialization error: {ex.Message}");
            }

            return result;
        }

        public static void ValidateWorldLocations(
            NightWatchOperationsCatalog? catalog,
            IReadOnlySet<string> knownLocationIds,
            List<string> errors)
        {
            if (catalog == null || knownLocationIds == null || errors == null) return;
            foreach (var post in catalog.posts)
            {
                if (post != null && !knownLocationIds.Contains(post.node_id))
                    errors.Add($"Watch post '{post.post_id}' references unknown location '{post.node_id}'.");
            }
            foreach (var route in catalog.routes)
            {
                if (route == null) continue;
                foreach (var waypoint in route.waypoint_location_ids)
                {
                    if (!knownLocationIds.Contains(waypoint))
                        errors.Add($"Watch route '{route.route_id}' references unknown waypoint '{waypoint}'.");
                }
            }
        }

        private static void Validate(NightWatchOperationsCatalog catalog, List<string> errors)
        {
            if (catalog.schema_version != 1)
                errors.Add($"Unsupported schema_version {catalog.schema_version}; expected 1.");
            if (!string.Equals(catalog.collection_id, "night_watch_operations", StringComparison.Ordinal))
                errors.Add($"Unexpected collection_id '{catalog.collection_id}'.");

            RequireRows(catalog.posts, "posts", errors);
            RequireRows(catalog.routes, "routes", errors);
            RequireRows(catalog.gate_rules, "gate_rules", errors);
            RequireRows(catalog.detection_profiles, "detection_profiles", errors);
            RequireRows(catalog.alarm_protocols, "alarm_protocols", errors);
            RequireRows(catalog.drills, "drills", errors);

            var ids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var post in catalog.posts)
            {
                if (post == null) { errors.Add("Watch post contains a null row."); continue; }
                Unique(post.post_id, "post", ids, errors);
                Text(post.display_name, $"post '{post.post_id}' display_name", errors);
                Text(post.node_id, $"post '{post.post_id}' node_id", errors);
                Sector(post.sector_id, $"post '{post.post_id}'", errors);
                if (post.capacity < 1 || post.capacity > 8) errors.Add($"Post '{post.post_id}' capacity must be 1..8.");
                if (post.relief_hours < 1 || post.relief_hours > 12) errors.Add($"Post '{post.post_id}' relief_hours must be 1..12.");
                Range(post.initial_condition_permille, 0, 1000, $"post '{post.post_id}' initial_condition_permille", errors);
                Range(post.night_vision_bonus_permille, 0, 1000, $"post '{post.post_id}' night_vision_bonus_permille", errors);
                if (post.sightlines == null || post.sightlines.Count == 0) errors.Add($"Post '{post.post_id}' has no sightlines.");
            }

            foreach (var route in catalog.routes)
            {
                if (route == null) { errors.Add("Watch route contains a null row."); continue; }
                Unique(route.route_id, "route", ids, errors);
                Text(route.display_name, $"route '{route.route_id}' display_name", errors);
                if (route.waypoint_location_ids == null || route.waypoint_location_ids.Count < 2)
                    errors.Add($"Route '{route.route_id}' must have at least two waypoints.");
                if (route.distance_km <= 0 || route.duration_hours <= 0)
                    errors.Add($"Route '{route.route_id}' distance and duration must be positive.");
                if (route.danger < 1 || route.danger > 5) errors.Add($"Route '{route.route_id}' danger must be 1..5.");
                if (route.sector_ids == null || route.sector_ids.Count == 0) errors.Add($"Route '{route.route_id}' has no sectors.");
                else foreach (var sector in route.sector_ids) Sector(sector, $"route '{route.route_id}'", errors);
                if (route.requires_debrief) Text(route.debrief_rule, $"route '{route.route_id}' debrief_rule", errors);
            }

            foreach (var rule in catalog.gate_rules)
            {
                if (rule == null) { errors.Add("Gate rule contains a null row."); continue; }
                Unique(rule.rule_id, "gate rule", ids, errors);
                Text(rule.display_name, $"gate rule '{rule.rule_id}' display_name", errors);
                if (!GateSubjects.Contains(rule.applies_to)) errors.Add($"Gate rule '{rule.rule_id}' has unknown applies_to '{rule.applies_to}'.");
                Text(rule.evidence, $"gate rule '{rule.rule_id}' evidence", errors);
                Text(rule.appeal, $"gate rule '{rule.rule_id}' appeal", errors);
            }

            foreach (var profile in catalog.detection_profiles)
            {
                if (profile == null) { errors.Add("Detection profile contains a null row."); continue; }
                Unique(profile.profile_id, "detection profile", ids, errors);
                Text(profile.display_name, $"detection profile '{profile.profile_id}' display_name", errors);
                if (!DetectionChannels.Contains(profile.channel)) errors.Add($"Detection profile '{profile.profile_id}' has unknown channel '{profile.channel}'.");
                Range(profile.confidence_permille, 0, 1000, $"detection profile '{profile.profile_id}' confidence_permille", errors);
                Text(profile.verification, $"detection profile '{profile.profile_id}' verification", errors);
            }

            foreach (var drill in catalog.drills)
            {
                if (drill == null) { errors.Add("Drill contains a null row."); continue; }
                Unique(drill.drill_id, "drill", ids, errors);
                Text(drill.display_name, $"drill '{drill.drill_id}' display_name", errors);
                if (drill.target_minutes < 1 || drill.target_minutes > 120) errors.Add($"Drill '{drill.drill_id}' target_minutes must be 1..120.");
                if (drill.participant_roles == null || drill.participant_roles.Count == 0) errors.Add($"Drill '{drill.drill_id}' has no participant_roles.");
                if (drill.failure_modes == null || drill.failure_modes.Count == 0) errors.Add($"Drill '{drill.drill_id}' has no failure_modes.");
                Range(drill.readiness_effect_permille, -1000, 1000, $"drill '{drill.drill_id}' readiness_effect_permille", errors);
            }

            foreach (var protocol in catalog.alarm_protocols)
            {
                if (protocol == null) { errors.Add("Alarm protocol contains a null row."); continue; }
                Unique(protocol.protocol_id, "alarm protocol", ids, errors);
                Text(protocol.display_name, $"alarm protocol '{protocol.protocol_id}' display_name", errors);
                Range(protocol.trigger_confidence_permille, 0, 1000, $"alarm protocol '{protocol.protocol_id}' trigger_confidence_permille", errors);
                if (protocol.channels == null || protocol.channels.Count == 0) errors.Add($"Alarm protocol '{protocol.protocol_id}' has no channels.");
                Text(protocol.response, $"alarm protocol '{protocol.protocol_id}' response", errors);
                if (!string.IsNullOrWhiteSpace(protocol.drill_id) && catalog.Drill(protocol.drill_id) == null)
                    errors.Add($"Alarm protocol '{protocol.protocol_id}' references unknown drill '{protocol.drill_id}'.");
            }
        }

        private static void RequireRows<T>(List<T> rows, string label, List<string> errors)
        {
            if (rows == null || rows.Count == 0) errors.Add($"Catalog contains no {label}.");
        }

        private static void Unique(string id, string label, HashSet<string> ids, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(id)) { errors.Add($"Empty {label} id."); return; }
            if (!ids.Add(id.Trim())) errors.Add($"Duplicate {label} id '{id}'.");
        }

        private static void Text(string value, string label, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(value)) errors.Add($"Empty {label}.");
        }

        private static void Range(int value, int min, int max, string label, List<string> errors)
        {
            if (value < min || value > max) errors.Add($"{label} must be {min}..{max}.");
        }

        private static void Sector(string value, string label, List<string> errors)
        {
            if (!Sectors.Contains(value)) errors.Add($"{label} has unknown sector '{value}'.");
        }
    }
}
