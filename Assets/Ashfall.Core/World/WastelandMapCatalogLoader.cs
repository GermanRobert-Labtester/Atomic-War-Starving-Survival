using System;
using System.Collections.Generic;

namespace Ashfall.Core.World
{
    /// <summary>Raw deserialization shape for wasteland_map_v1.json.</summary>
    [Serializable]
    public sealed class WastelandMapCatalogContainer
    {
        public int schema_version { get; set; }
        public List<MapNodeDef> nodes { get; set; } = new List<MapNodeDef>();
        public List<MapRouteDef> routes { get; set; } = new List<MapRouteDef>();
    }

    [Serializable]
    public sealed class MapNodeDef
    {
        public string id { get; set; } = string.Empty;
        public string displayName { get; set; } = string.Empty;
        public string danger { get; set; } = "none";
        public string? faction { get; set; }
        public string? lootTable { get; set; }
        public float positionX { get; set; }
        public float positionY { get; set; }
        public bool discoverable { get; set; }
        public bool startingUnlocked { get; set; }
    }

    [Serializable]
    public sealed class MapRouteDef
    {
        public string from { get; set; } = string.Empty;
        public string to { get; set; } = string.Empty;
        public float distanceKm { get; set; }
        public float weatherHazard { get; set; }
    }

    public enum MapRouteErrorKind
    {
        DuplicateRoute,
        DanglingEndpoint,
        NegativeOrZeroDistance,
        SelfRoute
    }

    public sealed class MapRouteValidationError
    {
        public string RouteDescription { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public MapRouteErrorKind Kind { get; set; }
    }

    /// <summary>
    /// Loads wasteland travel map definitions from JSON data authority.
    /// Engine-agnostic: uses IFileIO and IJsonSerializer ports.
    /// </summary>
    public static class WastelandMapCatalogLoader
    {
        public const string DefaultFileName = "wasteland_map_v1.json";

        public static List<MapRouteValidationError> ValidateRoutes(
            IReadOnlyList<MapNode> nodes,
            IReadOnlyList<MapRoute> routes)
        {
            var errors = new List<MapRouteValidationError>();
            if (routes == null) return errors;

            var nodeSet = new HashSet<string>(StringComparer.Ordinal);
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    if (node != null && !string.IsNullOrWhiteSpace(node.Id))
                        nodeSet.Add(node.Id);
                }
            }

            var seenRoutes = new HashSet<string>(StringComparer.Ordinal);

            foreach (var route in routes)
            {
                if (route == null) continue;
                string from = route.From ?? string.Empty;
                string to = route.To ?? string.Empty;
                string routeKey = $"{from}->{to}";

                // 1. Dangling endpoints
                if (string.IsNullOrWhiteSpace(from) || !nodeSet.Contains(from))
                {
                    errors.Add(new MapRouteValidationError
                    {
                        RouteDescription = routeKey,
                        ErrorMessage = $"Dangling or empty 'from' endpoint: '{from}'. Node does not exist.",
                        Kind = MapRouteErrorKind.DanglingEndpoint
                    });
                }

                if (string.IsNullOrWhiteSpace(to) || !nodeSet.Contains(to))
                {
                    errors.Add(new MapRouteValidationError
                    {
                        RouteDescription = routeKey,
                        ErrorMessage = $"Dangling or empty 'to' endpoint: '{to}'. Node does not exist.",
                        Kind = MapRouteErrorKind.DanglingEndpoint
                    });
                }

                // 2. Self-routes
                if (!string.IsNullOrWhiteSpace(from) && !string.IsNullOrWhiteSpace(to) && string.Equals(from, to, StringComparison.Ordinal))
                {
                    errors.Add(new MapRouteValidationError
                    {
                        RouteDescription = routeKey,
                        ErrorMessage = $"Self-route detected: 'from' and 'to' are both '{from}'.",
                        Kind = MapRouteErrorKind.SelfRoute
                    });
                }

                // 3. Negative or zero distances
                if (route.DistanceKm <= 0f || float.IsNaN(route.DistanceKm) || float.IsInfinity(route.DistanceKm))
                {
                    errors.Add(new MapRouteValidationError
                    {
                        RouteDescription = routeKey,
                        ErrorMessage = $"Invalid distance ({route.DistanceKm} km) for route {routeKey}. Distance must be strictly positive.",
                        Kind = MapRouteErrorKind.NegativeOrZeroDistance
                    });
                }

                // 4. Duplicate routes
                if (!string.IsNullOrWhiteSpace(from) && !string.IsNullOrWhiteSpace(to))
                {
                    if (seenRoutes.Contains(routeKey))
                    {
                        errors.Add(new MapRouteValidationError
                        {
                            RouteDescription = routeKey,
                            ErrorMessage = $"Duplicate route detected: {routeKey}.",
                            Kind = MapRouteErrorKind.DuplicateRoute
                        });
                    }
                    else
                    {
                        seenRoutes.Add(routeKey);
                    }
                }
            }

            return errors;
        }

        public static (List<MapNode> nodes, List<MapRoute> routes, List<MapRouteValidationError> errors) LoadWithValidation(
            string dataDir,
            IFileIO? fileIO = null,
            IJsonSerializer? json = null)
        {
            var (nodes, routes) = Load(dataDir, fileIO, json);
            var errors = ValidateRoutes(nodes, routes);
            return (nodes, routes, errors);
        }

        public static (List<MapNode> nodes, List<MapRoute> routes) Load(
            string dataDir,
            IFileIO? fileIO = null,
            IJsonSerializer? json = null)
        {
            fileIO ??= new FileSystemIO();
            json ??= new SystemTextJsonSerializer();

            var nodes = new List<MapNode>();
            var routes = new List<MapRoute>();

            if (string.IsNullOrEmpty(dataDir))
                return (nodes, routes);

            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                return (nodes, routes);

            string rawText = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(rawText))
                return (nodes, routes);

            var container = json.Deserialize<WastelandMapCatalogContainer>(rawText);
            if (container == null)
                return (nodes, routes);

            if (container.nodes != null)
            {
                foreach (var n in container.nodes)
                {
                    if (n == null || string.IsNullOrWhiteSpace(n.id)) continue;
                    nodes.Add(new MapNode
                    {
                        Id = n.id,
                        DisplayName = n.displayName ?? n.id,
                        Danger = ParseDanger(n.danger),
                        FactionId = n.faction ?? string.Empty,
                        LootTableId = n.lootTable ?? string.Empty,
                        PositionX = n.positionX,
                        PositionY = n.positionY,
                        Discoverable = n.discoverable,
                        StartingUnlocked = n.startingUnlocked
                    });
                }
            }

            if (container.routes != null)
            {
                foreach (var r in container.routes)
                {
                    if (r == null || string.IsNullOrWhiteSpace(r.from) || string.IsNullOrWhiteSpace(r.to)) continue;
                    routes.Add(new MapRoute
                    {
                        From = r.from,
                        To = r.to,
                        DistanceKm = r.distanceKm,
                        WeatherHazard = r.weatherHazard
                    });
                }
            }

            return (nodes, routes);
        }

        public static WastelandMapSystem CreateSystem(
            string dataDir,
            WastelandMapState? state = null,
            IFileIO? fileIO = null,
            IJsonSerializer? json = null)
        {
            var (nodes, routes) = Load(dataDir, fileIO, json);
            if (nodes.Count == 0)
            {
                // Fallback default node if no catalog found
                nodes.Add(new MapNode
                {
                    Id = "loc_holdfast",
                    DisplayName = "Holdfast",
                    Danger = MapNodeDanger.None,
                    PositionX = 500,
                    PositionY = 300,
                    StartingUnlocked = true
                });
            }
            return new WastelandMapSystem(state ?? new WastelandMapState(), nodes, routes);
        }

        private static MapNodeDanger ParseDanger(string? danger)
        {
            if (string.IsNullOrEmpty(danger)) return MapNodeDanger.None;
            return danger.Trim().ToLowerInvariant() switch
            {
                "low" => MapNodeDanger.Low,
                "medium" => MapNodeDanger.Medium,
                "high" => MapNodeDanger.High,
                "locked" => MapNodeDanger.Locked,
                _ => MapNodeDanger.None
            };
        }
    }
}
