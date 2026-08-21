using System;
using System.Collections.Generic;

namespace Ashfall.Core.Narrative
{
    [Serializable]
    public sealed class TradeCaravanRouteEntry
    {
        public string route_id;
        public string route_name;
        public string origin_hub;
        public string destination_hub;
        public float travel_days;
        public int hazard_index;
        public string primary_cargo_manifest;
        public string[] key_waypoints;
        public string caravan_master_log;
        public string[] tags;
    }

    [Serializable]
    public sealed class TradeCaravanFile
    {
        public int schema_version;
        public string collection_id;
        public List<TradeCaravanRouteEntry> routes = new List<TradeCaravanRouteEntry>();
    }

    /// <summary>
    /// Engine-agnostic loader and query interface for The 18 Wasteland Trade Caravans & Hazard Waypoints.
    /// </summary>
    public sealed class TradeCaravanCatalog
    {
        private readonly Dictionary<string, TradeCaravanRouteEntry> _byId =
            new Dictionary<string, TradeCaravanRouteEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly List<TradeCaravanRouteEntry> _allRoutes = new List<TradeCaravanRouteEntry>();

        public IReadOnlyList<TradeCaravanRouteEntry> AllRoutes => _allRoutes;

        public void Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var file = serializer.Deserialize<TradeCaravanFile>(json);
            if (file?.routes == null) return;

            foreach (var r in file.routes)
            {
                if (r == null || string.IsNullOrEmpty(r.route_id)) continue;
                _byId[r.route_id] = r;
                _allRoutes.Add(r);
            }
        }

        public TradeCaravanRouteEntry? GetById(string routeId)
        {
            if (string.IsNullOrEmpty(routeId)) return null;
            _byId.TryGetValue(routeId, out var entry);
            return entry;
        }

        public List<TradeCaravanRouteEntry> GetRoutesFromHub(string hubId)
        {
            var results = new List<TradeCaravanRouteEntry>();
            if (string.IsNullOrEmpty(hubId)) return results;

            for (int i = 0; i < _allRoutes.Count; i++)
            {
                var r = _allRoutes[i];
                if (string.Equals(r.origin_hub, hubId, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(r.destination_hub, hubId, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(r);
                }
            }
            return results;
        }

        public List<TradeCaravanRouteEntry> GetLowHazardSafeRoutes(int maxHazard = 2)
        {
            var results = new List<TradeCaravanRouteEntry>();
            for (int i = 0; i < _allRoutes.Count; i++)
            {
                var r = _allRoutes[i];
                if (r.hazard_index <= maxHazard)
                {
                    results.Add(r);
                }
            }
            return results;
        }

        public List<TradeCaravanRouteEntry> GetByTag(string tag)
        {
            var results = new List<TradeCaravanRouteEntry>();
            if (string.IsNullOrEmpty(tag)) return results;

            for (int i = 0; i < _allRoutes.Count; i++)
            {
                var r = _allRoutes[i];
                if (r.tags == null) continue;
                for (int j = 0; j < r.tags.Length; j++)
                {
                    if (string.Equals(r.tags[j], tag, StringComparison.OrdinalIgnoreCase))
                    {
                        results.Add(r);
                        break;
                    }
                }
            }
            return results;
        }
    }
}
