using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ashfall.Core.World
{
    /// <summary>
    /// Raw deserialization shape for faction_territory.json.
    /// </summary>
    [Serializable]
    public sealed class FactionTerritoryCatalogContainer
    {
        public int schema_version { get; set; } = 1;
        public string collection_id { get; set; } = "faction_territory_catalog";
        public List<FactionTerritoryDef> territories { get; set; } = new List<FactionTerritoryDef>();
        public List<ContestedZoneDef> contested_zones { get; set; } = new List<ContestedZoneDef>();
    }

    /// <summary>
    /// DTO defining a faction's territorial footprint, node control, and strategic anchors.
    /// </summary>
    [Serializable]
    public sealed class FactionTerritoryDef
    {
        public string id { get; set; } = string.Empty;
        public string faction { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string classification { get; set; } = "territorial"; // territorial, nomadic, ideological, mixed
        public string territory_scale { get; set; } = "minor"; // major, medium, minor, none
        public string primary_resource_interest { get; set; } = string.Empty;
        public List<string> controlled_nodes { get; set; } = new List<string>();
        public List<string> control_points { get; set; } = new List<string>();
        public List<string> contested_with { get; set; } = new List<string>();
        public int control_strength { get; set; } = 50;
        public float trade_tax { get; set; } = 0.0f;
        public float travel_safety { get; set; } = 1.0f;
        public string shift_trigger { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO defining a contested territorial flashpoint with overlapping faction claims.
    /// </summary>
    [Serializable]
    public sealed class ContestedZoneDef
    {
        public string id { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string strategic_value { get; set; } = string.Empty;
        public string focal_node_id { get; set; } = string.Empty;
        public string focal_location_id { get; set; } = string.Empty;
        public List<string> claimant_factions { get; set; } = new List<string>();
        public string conflict_driver { get; set; } = string.Empty;
        public int hazard_rating { get; set; } = 1;
        public int dispute_intensity { get; set; } = 50;
    }

    /// <summary>
    /// Authoritative domain catalog for faction territories and contested zones in ASHFALL (Plan 44).
    /// </summary>
    public sealed class FactionTerritoryCatalog
    {
        public const string DefaultFileName = "faction_territory.json";

        private readonly Dictionary<string, FactionTerritoryDef> _territoriesById;
        private readonly Dictionary<string, FactionTerritoryDef> _territoriesByFaction;
        private readonly Dictionary<string, ContestedZoneDef> _contestedZonesById;
        private readonly List<FactionTerritoryDef> _territories;
        private readonly List<ContestedZoneDef> _contestedZones;

        public IReadOnlyList<FactionTerritoryDef> Territories => _territories;
        public IReadOnlyList<ContestedZoneDef> ContestedZones => _contestedZones;
        public int TerritoryCount => _territories.Count;
        public int ContestedZoneCount => _contestedZones.Count;

        public FactionTerritoryCatalog(IEnumerable<FactionTerritoryDef> territories, IEnumerable<ContestedZoneDef>? contestedZones = null)
        {
            _territories = territories?.Where(t => t != null && !string.IsNullOrEmpty(t.id)).ToList() ?? new List<FactionTerritoryDef>();
            _contestedZones = contestedZones?.Where(z => z != null && !string.IsNullOrEmpty(z.id)).ToList() ?? new List<ContestedZoneDef>();

            _territoriesById = new Dictionary<string, FactionTerritoryDef>(StringComparer.OrdinalIgnoreCase);
            _territoriesByFaction = new Dictionary<string, FactionTerritoryDef>(StringComparer.OrdinalIgnoreCase);
            _contestedZonesById = new Dictionary<string, ContestedZoneDef>(StringComparer.OrdinalIgnoreCase);

            foreach (var t in _territories)
            {
                _territoriesById[t.id] = t;
                if (!string.IsNullOrEmpty(t.faction))
                {
                    _territoriesByFaction[t.faction] = t;
                }
            }

            foreach (var z in _contestedZones)
            {
                _contestedZonesById[z.id] = z;
            }
        }

        public bool TryGetTerritory(string territoryId, out FactionTerritoryDef territory)
        {
            return _territoriesById.TryGetValue(territoryId, out territory!);
        }

        public bool TryGetTerritoryByFaction(string factionId, out FactionTerritoryDef territory)
        {
            return _territoriesByFaction.TryGetValue(factionId, out territory!);
        }

        public bool TryGetContestedZone(string zoneId, out ContestedZoneDef zone)
        {
            return _contestedZonesById.TryGetValue(zoneId, out zone!);
        }

        public IEnumerable<FactionTerritoryDef> GetTerritoriesForNode(string mapNodeId)
        {
            if (string.IsNullOrEmpty(mapNodeId)) yield break;
            foreach (var t in _territories)
            {
                if (t.controlled_nodes.Contains(mapNodeId, StringComparer.OrdinalIgnoreCase))
                {
                    yield return t;
                }
            }
        }

        public static FactionTerritoryCatalog LoadFromDirectory(string dataDirectory, IFileIO fileIO, IJsonSerializer? jsonSerializer = null)
        {
            if (fileIO == null) throw new ArgumentNullException(nameof(fileIO));
            string path = Path.Combine(dataDirectory, DefaultFileName);
            if (!fileIO.FileExists(path))
            {
                return new FactionTerritoryCatalog(Enumerable.Empty<FactionTerritoryDef>());
            }

            string json = fileIO.ReadAllText(path);
            return LoadFromJson(json, jsonSerializer);
        }

        public static FactionTerritoryCatalog LoadFromJson(string json, IJsonSerializer? jsonSerializer = null)
        {
            var serializer = jsonSerializer ?? new SystemTextJsonSerializer();
            var container = serializer.Deserialize<FactionTerritoryCatalogContainer>(json);
            if (container == null)
            {
                return new FactionTerritoryCatalog(Enumerable.Empty<FactionTerritoryDef>());
            }

            return new FactionTerritoryCatalog(container.territories, container.contested_zones);
        }
    }
}
