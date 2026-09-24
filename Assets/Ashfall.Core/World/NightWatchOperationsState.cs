// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 36 — The Watch
// Additive state contracts. These records ride the existing perimeter-defense
// save owner; they do not create a second perimeter or alarm registry.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.World
{
    [Serializable]
    public sealed class NightWatchPostState
    {
        public string post_id = string.Empty;
        public bool active = true;
        public int condition_permille = 1000;
        public int last_maintenance_day = -1;
        public int shifts_completed = 0;

        public NightWatchPostState Clone() => new NightWatchPostState
        {
            post_id = post_id ?? string.Empty,
            active = active,
            condition_permille = Math.Clamp(condition_permille, 0, 1000),
            last_maintenance_day = Math.Max(-1, last_maintenance_day),
            shifts_completed = Math.Max(0, shifts_completed)
        };
    }

    [Serializable]
    public sealed class NightWatchRouteState
    {
        public string route_id = string.Empty;
        public int rounds_completed = 0;
        public int last_completed_day = -1;
        public int last_debrief_day = -1;
        public bool debrief_pending = false;

        public NightWatchRouteState Clone() => new NightWatchRouteState
        {
            route_id = route_id ?? string.Empty,
            rounds_completed = Math.Max(0, rounds_completed),
            last_completed_day = Math.Max(-1, last_completed_day),
            last_debrief_day = Math.Max(-1, last_debrief_day),
            debrief_pending = debrief_pending
        };
    }

    [Serializable]
    public sealed class NightWatchDrillState
    {
        public string drill_id = string.Empty;
        public int last_completed_day = -1;
        public int passes = 0;
        public int failures = 0;
        public int readiness_permille = 0;

        public NightWatchDrillState Clone() => new NightWatchDrillState
        {
            drill_id = drill_id ?? string.Empty,
            last_completed_day = Math.Max(-1, last_completed_day),
            passes = Math.Max(0, passes),
            failures = Math.Max(0, failures),
            readiness_permille = Math.Clamp(readiness_permille, 0, 1000)
        };
    }

    /// <summary>
    /// Watch-specific state nested under <c>PerimeterDefenseSave</c>. Physical
    /// perimeter emplacements and alarm history remain in their existing
    /// fields; this object contains only operational watch posts, routes,
    /// drills, and the last derived readiness snapshot.
    /// </summary>
    [Serializable]
    public sealed class NightWatchOperationsState
    {
        public int schema_version = 1;
        public List<NightWatchPostState> posts = new List<NightWatchPostState>();
        public List<NightWatchRouteState> routes = new List<NightWatchRouteState>();
        public List<NightWatchDrillState> drills = new List<NightWatchDrillState>();
        public int last_evaluation_day = -1;
        public int last_readiness_permille = 0;
        public int last_gate_readiness_permille = 0;
        public int last_coverage_grade = 0;
        public string last_coverage_sector = string.Empty;

        public NightWatchOperationsState Clone()
        {
            var copy = new NightWatchOperationsState
            {
                schema_version = schema_version <= 0 ? 1 : schema_version,
                last_evaluation_day = Math.Max(-1, last_evaluation_day),
                last_readiness_permille = Math.Clamp(last_readiness_permille, 0, 1000),
                last_gate_readiness_permille = Math.Clamp(last_gate_readiness_permille, 0, 1000),
                last_coverage_grade = Math.Clamp(last_coverage_grade, 0, 4),
                last_coverage_sector = last_coverage_sector ?? string.Empty,
                posts = posts?.Where(x => x != null).Select(x => x.Clone()).ToList() ?? new List<NightWatchPostState>(),
                routes = routes?.Where(x => x != null).Select(x => x.Clone()).ToList() ?? new List<NightWatchRouteState>(),
                drills = drills?.Where(x => x != null).Select(x => x.Clone()).ToList() ?? new List<NightWatchDrillState>()
            };
            return copy;
        }

        public void EnsureCollections()
        {
            if (schema_version <= 0) schema_version = 1;
            if (schema_version > 1)
                throw new InvalidOperationException($"night watch operations schema {schema_version} is newer than supported 1.");
            posts ??= new List<NightWatchPostState>();
            routes ??= new List<NightWatchRouteState>();
            drills ??= new List<NightWatchDrillState>();
            last_coverage_sector ??= string.Empty;
        }

        /// <summary>Drops unknown rows and adds authored rows with safe defaults.</summary>
        public void BindCatalog(NightWatchOperationsCatalog? catalog)
        {
            EnsureCollections();
            if (catalog == null)
            {
                NormalizeCollections();
                return;
            }

            var postIds = new HashSet<string>(
                (catalog.posts ?? new List<NightWatchPostDefinition>())
                    .Where(x => x != null && !string.IsNullOrWhiteSpace(x.post_id))
                    .Select(x => x.post_id.Trim()),
                StringComparer.OrdinalIgnoreCase);
            var routeIds = new HashSet<string>(
                (catalog.routes ?? new List<NightWatchRouteDefinition>())
                    .Where(x => x != null && !string.IsNullOrWhiteSpace(x.route_id))
                    .Select(x => x.route_id.Trim()),
                StringComparer.OrdinalIgnoreCase);
            var drillIds = new HashSet<string>(
                (catalog.drills ?? new List<NightWatchDrillDefinition>())
                    .Where(x => x != null && !string.IsNullOrWhiteSpace(x.drill_id))
                    .Select(x => x.drill_id.Trim()),
                StringComparer.OrdinalIgnoreCase);

            var normalizedPosts = new Dictionary<string, NightWatchPostState>(StringComparer.OrdinalIgnoreCase);
            foreach (var post in posts ?? new List<NightWatchPostState>())
            {
                if (post == null || !postIds.Contains(post.post_id ?? string.Empty)) continue;
                var copy = post.Clone();
                copy.post_id = copy.post_id.Trim();
                if (!normalizedPosts.ContainsKey(copy.post_id)) normalizedPosts.Add(copy.post_id, copy);
            }
            posts = normalizedPosts.Values.OrderBy(x => x.post_id, StringComparer.Ordinal).ToList();

            var normalizedRoutes = new Dictionary<string, NightWatchRouteState>(StringComparer.OrdinalIgnoreCase);
            foreach (var route in routes ?? new List<NightWatchRouteState>())
            {
                if (route == null || !routeIds.Contains(route.route_id ?? string.Empty)) continue;
                var copy = route.Clone();
                copy.route_id = copy.route_id.Trim();
                if (!normalizedRoutes.ContainsKey(copy.route_id)) normalizedRoutes.Add(copy.route_id, copy);
            }
            routes = normalizedRoutes.Values.OrderBy(x => x.route_id, StringComparer.Ordinal).ToList();

            var normalizedDrills = new Dictionary<string, NightWatchDrillState>(StringComparer.OrdinalIgnoreCase);
            foreach (var drill in drills ?? new List<NightWatchDrillState>())
            {
                if (drill == null || !drillIds.Contains(drill.drill_id ?? string.Empty)) continue;
                var copy = drill.Clone();
                copy.drill_id = copy.drill_id.Trim();
                if (!normalizedDrills.ContainsKey(copy.drill_id)) normalizedDrills.Add(copy.drill_id, copy);
            }
            drills = normalizedDrills.Values.OrderBy(x => x.drill_id, StringComparer.Ordinal).ToList();

            foreach (var definition in catalog.posts ?? new List<NightWatchPostDefinition>())
            {
                if (definition == null) continue;
                if (posts.All(x => !string.Equals(x.post_id, definition.post_id, StringComparison.OrdinalIgnoreCase)))
                {
                    posts.Add(new NightWatchPostState
                    {
                        post_id = definition.post_id,
                        active = true,
                        condition_permille = Math.Clamp(definition.initial_condition_permille, 0, 1000)
                    });
                }
            }
            foreach (var definition in catalog.routes ?? new List<NightWatchRouteDefinition>())
            {
                if (definition == null) continue;
                if (routes.All(x => !string.Equals(x.route_id, definition.route_id, StringComparison.OrdinalIgnoreCase)))
                    routes.Add(new NightWatchRouteState { route_id = definition.route_id });
            }
            foreach (var definition in catalog.drills ?? new List<NightWatchDrillDefinition>())
            {
                if (definition == null) continue;
                if (drills.All(x => !string.Equals(x.drill_id, definition.drill_id, StringComparison.OrdinalIgnoreCase)))
                    drills.Add(new NightWatchDrillState { drill_id = definition.drill_id });
            }

            posts = posts.OrderBy(x => x.post_id, StringComparer.Ordinal).ToList();
            routes = routes.OrderBy(x => x.route_id, StringComparer.Ordinal).ToList();
            drills = drills.OrderBy(x => x.drill_id, StringComparer.Ordinal).ToList();
        }

        private void NormalizeCollections()
        {
            var normalizedPosts = new Dictionary<string, NightWatchPostState>(StringComparer.OrdinalIgnoreCase);
            foreach (var post in posts ?? new List<NightWatchPostState>())
            {
                if (post == null || string.IsNullOrWhiteSpace(post.post_id)) continue;
                var copy = post.Clone();
                copy.post_id = copy.post_id.Trim();
                if (!normalizedPosts.ContainsKey(copy.post_id)) normalizedPosts.Add(copy.post_id, copy);
            }
            posts = normalizedPosts.Values.OrderBy(x => x.post_id, StringComparer.Ordinal).ToList();

            var normalizedRoutes = new Dictionary<string, NightWatchRouteState>(StringComparer.OrdinalIgnoreCase);
            foreach (var route in routes ?? new List<NightWatchRouteState>())
            {
                if (route == null || string.IsNullOrWhiteSpace(route.route_id)) continue;
                var copy = route.Clone();
                copy.route_id = copy.route_id.Trim();
                if (!normalizedRoutes.ContainsKey(copy.route_id)) normalizedRoutes.Add(copy.route_id, copy);
            }
            routes = normalizedRoutes.Values.OrderBy(x => x.route_id, StringComparer.Ordinal).ToList();

            var normalizedDrills = new Dictionary<string, NightWatchDrillState>(StringComparer.OrdinalIgnoreCase);
            foreach (var drill in drills ?? new List<NightWatchDrillState>())
            {
                if (drill == null || string.IsNullOrWhiteSpace(drill.drill_id)) continue;
                var copy = drill.Clone();
                copy.drill_id = copy.drill_id.Trim();
                if (!normalizedDrills.ContainsKey(copy.drill_id)) normalizedDrills.Add(copy.drill_id, copy);
            }
            drills = normalizedDrills.Values.OrderBy(x => x.drill_id, StringComparer.Ordinal).ToList();
        }
    }
}
