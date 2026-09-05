using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.IO;

namespace Ashfall.Core.World
{
    /// <summary>
    /// One route reachable through one region tag (D3). Pure association
    /// data — weather semantics for a route's gate live in WeatherGate,
    /// never here. BlockedWeather/RequiredWeather stay empty in the
    /// topology; the gate evaluator fills them when joining.
    /// </summary>
    [Serializable]
    public sealed class RouteDefinition
    {
        public string RouteId { get; init; } = "";
        public string TargetId { get; init; } = "";
        public string RegionTag { get; init; } = "";
        public IReadOnlyList<string> BlockedWeather { get; init; } = Array.Empty<string>();
        public IReadOnlyList<string> RequiredWeather { get; init; } = Array.Empty<string>();
    }

    /// <summary>
    /// Route↔region association contract (D3/D7).
    ///
    /// Authority: Assets/StreamingAssets/Data/region_route_topology.json —
    ///   { "schema_version": 1,
    ///     "regions": [ { "region_tag": "high_scarp",
    ///                    "route_targets": ["route_12_the_cloud_eyrie_meteorological_ascent"],
    ///                    "traversable_weather": ["Clear", "Overcast", "Blizzard"] } ] }
    ///
    /// traversable_weather lists the weather kinds under which the region's
    /// routes ARE passable; the gate evaluator intersects that with each
    /// route's gate blocked/required lists. If the file is absent the
    /// mapping is derived from the gate catalog instead (every route gate
    /// target belongs to every region holding an encounter tagged with
    /// that region). Unknown region tags never suppress encounters —
    /// weather gating is the gate evaluator's job, not the topology's.
    /// </summary>
    public sealed class RouteRegionTopology
    {
        private readonly Dictionary<string, List<RouteDefinition>> _byRegion =
            new Dictionary<string, List<RouteDefinition>>(StringComparer.Ordinal);

        private readonly string _dataDir;
        private readonly IFileIO _fileIO;
        private readonly IJsonSerializer _json;

        public const string FileName = "region_route_topology.json";

        public RouteRegionTopology(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            _dataDir = dataDir ?? "";
            _fileIO = fileIO ?? throw new ArgumentNullException(nameof(fileIO));
            _json = json ?? throw new ArgumentNullException(nameof(json));
            Load();
        }

        private void Load()
        {
            if (_fileIO == null || string.IsNullOrEmpty(_dataDir))
                return;

            string path = _fileIO.Combine(_dataDir, FileName);
            if (!_fileIO.FileExists(path))
                return; // optional: mapping will be derived from the gate catalog

            string raw = _fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return;

            try
            {
                var parsed = _json.Deserialize<TopologyEnvelope>(raw);
                if (parsed?.regions == null)
                    return;

                foreach (var region in parsed.regions)
                {
                    if (region == null || string.IsNullOrEmpty(region.region_tag))
                        continue;

                    var routes = new List<RouteDefinition>();
                    if (region.route_targets != null)
                    {
                        foreach (var target in region.route_targets)
                        {
                            if (string.IsNullOrEmpty(target))
                                continue;
                            routes.Add(new RouteDefinition
                            {
                                RouteId = target,
                                TargetId = target,
                                RegionTag = region.region_tag
                            });
                        }
                    }

                    _byRegion[region.region_tag] = routes;
                }
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(path, "TopologyEnvelope", ex);
            }
        }

        [Serializable]
        public sealed class TopologyEnvelope
        {
            public int schema_version = 1;
            public List<TopologyRegion> regions = new List<TopologyRegion>();
        }

        [Serializable]
        public sealed class TopologyRegion
        {
            public string region_tag = "";
            public List<string> route_targets = new List<string>();
            public List<string> traversable_weather = new List<string>();
        }

        // ── Queries ──────────────────────────────────────────────────

        public IReadOnlyList<RouteDefinition> GetRoutesForRegion(string regionTag)
        {
            if (string.IsNullOrEmpty(regionTag))
                return Array.Empty<RouteDefinition>();
            return _byRegion.TryGetValue(regionTag, out var routes)
                ? routes
                : Array.Empty<RouteDefinition>();
        }

        public IReadOnlyList<string> GetRouteTargetsForRegion(string regionTag)
        {
            return GetRoutesForRegion(regionTag)
                .Select(r => r.TargetId)
                .ToList();
        }

        public IReadOnlyList<string> GetAllRegionTags()
        {
            return _byRegion.Keys.OrderBy(k => k, StringComparer.Ordinal).ToList();
        }
    }
}
