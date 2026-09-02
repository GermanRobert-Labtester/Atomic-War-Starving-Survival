using System;
using System.Collections.Generic;

namespace Ashfall.Core.World
{
    /// <summary>Raw deserialization shape for wasteland_map_v1.json.</summary>
    [Serializable]
    public sealed class WastelandMapCatalogContainer
    {
        /// <summary>Schema version of the wasteland travel map JSON catalog.</summary>
        public int schema_version { get; set; }

        /// <summary>Collection of wasteland travel node definitions.</summary>
        public List<MapNodeDef> nodes { get; set; } = new List<MapNodeDef>();

        /// <summary>Collection of wasteland travel route definitions connecting nodes.</summary>
        public List<MapRouteDef> routes { get; set; } = new List<MapRouteDef>();
    }

    /// <summary>Data Transfer Object representing a wasteland map location node in JSON.</summary>
    [Serializable]
    public sealed class MapNodeDef
    {
        /// <summary>Unique identifier for the location node (e.g. loc_holdfast, loc_foghorn).</summary>
        public string id { get; set; } = string.Empty;

        /// <summary>Display name rendered in UI panels and map view labels.</summary>
        public string displayName { get; set; } = string.Empty;

        /// <summary>Danger rating string (e.g. "none", "low", "medium", "high", "locked").</summary>
        public string danger { get; set; } = "none";

        /// <summary>Controlling faction identifier, or null if unaligned.</summary>
        public string? faction { get; set; }

        /// <summary>Loot table identifier for scavenging rolls, or null if none.</summary>
        public string? lootTable { get; set; }

        /// <summary>Horizontal position (X coordinate) on the wasteland map canvas.</summary>
        public float positionX { get; set; }

        /// <summary>Vertical position (Y coordinate) on the wasteland map canvas.</summary>
        public float positionY { get; set; }

        /// <summary>Whether this node can be discovered via exploration/scouting.</summary>
        public bool discoverable { get; set; }

        /// <summary>Whether this node starts unlocked and visible at campaign Day 1.</summary>
        public bool startingUnlocked { get; set; }
    }

    /// <summary>Data Transfer Object representing a travel route edge between two map nodes in JSON.</summary>
    [Serializable]
    public sealed class MapRouteDef
    {
        /// <summary>Origin map node identifier.</summary>
        public string from { get; set; } = string.Empty;

        /// <summary>Destination map node identifier.</summary>
        public string to { get; set; } = string.Empty;

        /// <summary>Distance between nodes in kilometers.</summary>
        public float distanceKm { get; set; }

        /// <summary>Weather hazard risk multiplier along this travel corridor.</summary>
        public float weatherHazard { get; set; }

        /// <summary>Travel domain: "land" or "water".</summary>
        public string travelDomain { get; set; } = "land";

        /// <summary>Water current strength (-1.0 to 1.0).</summary>
        public float currentStrength { get; set; } = 0f;

        /// <summary>Toxic water contamination level (0.0 to 1.0).</summary>
        public float toxicContamination { get; set; } = 0f;
    }

    /// <summary>Categories of route validation failures in the map catalog.</summary>
    public enum MapRouteErrorKind
    {
        /// <summary>A duplicate directed route exists between the same endpoints.</summary>
        DuplicateRoute,

        /// <summary>One or both endpoints do not resolve to a known map node ID.</summary>
        DanglingEndpoint,

        /// <summary>The route distance is non-positive, NaN, or infinite.</summary>
        NegativeOrZeroDistance,

        /// <summary>The route origin and destination point to the exact same node.</summary>
        SelfRoute
    }

    /// <summary>Represents a validation error encountered when checking wasteland map routes.</summary>
    public sealed class MapRouteValidationError
    {
        /// <summary>Description of the route with the validation issue (e.g. "from->to").</summary>
        public string RouteDescription { get; set; } = string.Empty;

        /// <summary>Human-readable error description explaining the validation failure.</summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>Kind of route validation error detected.</summary>
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
                        WeatherHazard = r.weatherHazard,
                        TravelDomain = r.travelDomain ?? "land",
                        CurrentStrength = r.currentStrength,
                        ToxicContamination = r.toxicContamination
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
