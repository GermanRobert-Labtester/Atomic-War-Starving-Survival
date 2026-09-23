// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Ashfall.Core.Campaign;
using Ashfall.Core.World;

namespace Ashfall.Core.Exploration
{
    public enum TerrainCategory
    {
        Urban = 0,
        Rural = 1,
        Industrial = 2,
        Wasteland = 3,
        Water = 4
    }

    public enum MapQualityTier
    {
        Rough = 0,
        Standard = 1,
        Detailed = 2
    }

    [Serializable]
    public sealed class MapRegion
    {
        public string RegionId { get; set; } = string.Empty;
        public string RegionName { get; set; } = string.Empty;
        public TerrainCategory Terrain { get; set; } = TerrainCategory.Wasteland;
        public bool IsDiscovered { get; set; } = false;
        public float ExplorationProgress { get; set; } = 0f; // 0 to 100
        public List<string> KnownPoiIds { get; set; } = new List<string>();
        public List<string> Hazards { get; set; } = new List<string>();
        public float ScoutingDifficulty { get; set; } = 1.0f;
    }

    [Serializable]
    public sealed class MapDiscovery
    {
        public string DiscoveryId { get; set; } = string.Empty;
        public string RegionId { get; set; } = string.Empty;
        public string LocationId { get; set; } = string.Empty;
        public int DiscoveredDay { get; set; } = 1;
        public string DiscoveredBy { get; set; } = string.Empty;
        public float Quality { get; set; } = 50f;
    }

    [Serializable]
    public sealed class CartographySkill
    {
        public string SurvivorId { get; set; } = string.Empty;
        public float Proficiency { get; set; } = 10f; // 0 to 100
        public int MapsCreated { get; set; } = 0;
    }

    [Serializable]
    public sealed class CartographyState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public List<MapRegion> Regions { get; set; } = new List<MapRegion>();
        public List<MapDiscovery> Discoveries { get; set; } = new List<MapDiscovery>();
        public List<CartographySkill> Cartographers { get; set; } = new List<CartographySkill>();
    }

    /// <summary>
    /// Read-only cartography overlay for the canonical wasteland map. The map
    /// system remains the authority for topology, discovery, fog state, and
    /// persistence; this DTO only gives presentation and trade callers a
    /// stable survey-quality/provenance view.
    /// </summary>
    [Serializable]
    public sealed class CanonicalMapSurvey
    {
        public string NodeId { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public MapFogState FogState { get; set; } = MapFogState.Unknown;
        public float SurveyQuality { get; set; }
        public MapQualityTier QualityTier { get; set; } = MapQualityTier.Rough;
        public int LastConfirmedDay { get; set; }
        public string ProvenanceSourceId { get; set; } = string.Empty;
        public KnowledgeSourceKind? ProvenanceKind { get; set; }
        public IReadOnlyList<string> Traits { get; set; } = Array.Empty<string>();
    }

    /// <summary>
    /// Plan 163 — Wasteland Cartography & Mapping System.
    /// Tracks regional fog-of-war, scouting discoveries, points of interest,
    /// cartography proficiency per surveyor, and map completeness across the wasteland.
    /// </summary>
    public sealed class CartographySystem
    {
        private readonly CartographyState _state;

        public event Action<MapRegion>? OnRegionDiscovered;
        public event Action<MapDiscovery>? OnPoiMapped;
        public event Action<CartographySkill>? OnCartographySkillAdvanced;

        public Action<MapRegion>? OnRegionDiscoveredSeam { get; set; }
        public Action<MapDiscovery>? OnPoiMappedSeam { get; set; }
        public Action<CartographySkill>? OnCartographySkillAdvancedSeam { get; set; }

        public int DiscoveredRegionCount => _state.Regions.Count(r => r.IsDiscovered);
        public int TotalRegionCount => _state.Regions.Count;
        public IReadOnlyList<MapRegion> Regions => _state.Regions;
        public IReadOnlyList<MapDiscovery> Discoveries => _state.Discoveries;

        public CartographySystem(CartographyState? state = null)
        {
            _state = state ?? new CartographyState();
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;

            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.TryGetProperty("regions", out var regionsEl) && regionsEl.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in regionsEl.EnumerateArray())
                    {
                        string id = item.TryGetProperty("region_id", out var idProp) ? idProp.GetString() ?? "" : "";
                        string name = item.TryGetProperty("region_name", out var nameProp) ? nameProp.GetString() ?? "" : "";
                        string terrainStr = item.TryGetProperty("terrain", out var tProp) ? tProp.GetString() ?? "" : "";
                        float diff = item.TryGetProperty("scouting_difficulty", out var dProp) ? (float)dProp.GetDouble() : 1.0f;

                        var pois = new List<string>();
                        if (item.TryGetProperty("points_of_interest", out var poiEl) && poiEl.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var p in poiEl.EnumerateArray())
                            {
                                string? str = p.GetString();
                                if (!string.IsNullOrEmpty(str)) pois.Add(str);
                            }
                        }

                        var hazards = new List<string>();
                        if (item.TryGetProperty("hazards", out var hazEl) && hazEl.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var h in hazEl.EnumerateArray())
                            {
                                string? str = h.GetString();
                                if (!string.IsNullOrEmpty(str)) hazards.Add(str);
                            }
                        }

                        TerrainCategory terrain = terrainStr.ToLowerInvariant() switch
                        {
                            "urban" => TerrainCategory.Urban,
                            "rural" => TerrainCategory.Rural,
                            "industrial" => TerrainCategory.Industrial,
                            "water" => TerrainCategory.Water,
                            _ => TerrainCategory.Wasteland
                        };

                        if (!string.IsNullOrEmpty(id))
                        {
                            var reg = RegisterRegion(id, name, terrain, pois, hazards);
                            reg.ScoutingDifficulty = diff;
                        }
                    }
                }
            }
            catch
            {
                // Catalog parse fallback
            }
        }

        public static float GetEquipmentBonus(string toolName)
        {
            if (string.IsNullOrWhiteSpace(toolName)) return 0f;
            return toolName.ToLowerInvariant() switch
            {
                "compass" => 10f,
                "sextant" => 20f,
                "survey_tools" => 30f,
                "cartography_kit" => 50f,
                _ => 0f
            };
        }

        /// <summary>
        /// Projects canonical world knowledge into cartography terms without
        /// copying or mutating the world graph. Quality is deliberately a
        /// deterministic interpretation of the authoritative fog state:
        /// rumor, survey, and visit are progressively more reliable.
        /// </summary>
        public static IReadOnlyList<CanonicalMapSurvey> ProjectCanonicalMap(
            IReadOnlyList<MapNode>? nodes,
            IReadOnlyList<MapNodeKnowledgeState>? knowledge)
        {
            var knowledgeById = new Dictionary<string, MapNodeKnowledgeState>(StringComparer.Ordinal);
            if (knowledge != null)
            {
                for (int i = 0; i < knowledge.Count; i++)
                {
                    var record = knowledge[i];
                    if (record == null || string.IsNullOrEmpty(record.NodeId)) continue;
                    knowledgeById[record.NodeId] = record;
                }
            }

            var result = new List<CanonicalMapSurvey>();
            if (nodes == null) return result;
            for (int i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];
                if (node == null || string.IsNullOrEmpty(node.Id)) continue;
                knowledgeById.TryGetValue(node.Id, out var record);
                var fog = record?.FogState ?? MapFogState.Unknown;
                float quality = fog switch
                {
                    MapFogState.Rumored => 30f,
                    MapFogState.Surveyed => 65f,
                    MapFogState.Visited => 100f,
                    _ => 0f
                };
                result.Add(new CanonicalMapSurvey
                {
                    NodeId = node.Id,
                    DisplayName = node.DisplayName ?? node.Id,
                    FogState = fog,
                    SurveyQuality = quality,
                    QualityTier = quality >= 67f
                        ? MapQualityTier.Detailed
                        : quality >= 34f ? MapQualityTier.Standard : MapQualityTier.Rough,
                    LastConfirmedDay = record?.LastConfirmedDay ?? 0,
                    ProvenanceSourceId = record?.Provenance?.SourceId ?? string.Empty,
                    ProvenanceKind = record?.Provenance?.SourceKind,
                    Traits = record?.Traits != null
                        ? new List<string>(record.Traits)
                        : Array.Empty<string>()
                });
            }

            result.Sort((left, right) => string.CompareOrdinal(left.NodeId, right.NodeId));
            return result;
        }

        public MapRegion RegisterRegion(
            string regionId,
            string name,
            TerrainCategory terrain,
            IEnumerable<string>? pois = null,
            IEnumerable<string>? hazards = null)
        {
            if (string.IsNullOrWhiteSpace(regionId)) throw new ArgumentNullException(nameof(regionId));

            var region = _state.Regions.FirstOrDefault(r => string.Equals(r.RegionId, regionId, StringComparison.OrdinalIgnoreCase));
            if (region == null)
            {
                region = new MapRegion
                {
                    RegionId = regionId.Trim(),
                    RegionName = string.IsNullOrWhiteSpace(name) ? regionId : name.Trim(),
                    Terrain = terrain,
                    IsDiscovered = false,
                    ExplorationProgress = 0f,
                    KnownPoiIds = pois != null ? new List<string>(pois) : new List<string>(),
                    Hazards = hazards != null ? new List<string>(hazards) : new List<string>()
                };
                _state.Regions.Add(region);
            }

            return region;
        }

        public bool DiscoverRegion(string regionId, string scoutSurvivorId, int currentDay)
        {
            var region = _state.Regions.FirstOrDefault(r => string.Equals(r.RegionId, regionId, StringComparison.OrdinalIgnoreCase));
            if (region == null) return false;

            if (!region.IsDiscovered)
            {
                region.IsDiscovered = true;
                region.ExplorationProgress = Math.Max(20f, region.ExplorationProgress);
                OnRegionDiscovered?.Invoke(region);
                OnRegionDiscoveredSeam?.Invoke(region);
            }

            return true;
        }

        public MapDiscovery? SurveyRegion(string regionId, string surveyorId, int currentDay, float equipmentBonus = 0f)
        {
            var region = _state.Regions.FirstOrDefault(r => string.Equals(r.RegionId, regionId, StringComparison.OrdinalIgnoreCase));
            if (region == null) return null;

            region.IsDiscovered = true;

            var skill = _state.Cartographers.FirstOrDefault(c => string.Equals(c.SurvivorId, surveyorId, StringComparison.OrdinalIgnoreCase));
            if (skill == null && !string.IsNullOrWhiteSpace(surveyorId))
            {
                skill = new CartographySkill { SurvivorId = surveyorId.Trim(), Proficiency = 15f };
                _state.Cartographers.Add(skill);
            }

            float prof = skill?.Proficiency ?? 10f;
            float quality = Math.Clamp(prof + equipmentBonus, 10f, 100f);

            region.ExplorationProgress = Math.Clamp(region.ExplorationProgress + (quality * 0.4f), 0f, 100f);

            var discovery = new MapDiscovery
            {
                DiscoveryId = $"dsc_{_state.NextSequence++}",
                RegionId = region.RegionId,
                LocationId = region.KnownPoiIds.FirstOrDefault() ?? region.RegionId,
                DiscoveredDay = currentDay,
                DiscoveredBy = surveyorId ?? string.Empty,
                Quality = quality
            };
            _state.Discoveries.Add(discovery);

            if (skill != null)
            {
                skill.Proficiency = Math.Clamp(skill.Proficiency + 2.5f, 0f, 100f);
                skill.MapsCreated++;
                OnCartographySkillAdvanced?.Invoke(skill);
                OnCartographySkillAdvancedSeam?.Invoke(skill);
            }

            OnPoiMapped?.Invoke(discovery);
            OnPoiMappedSeam?.Invoke(discovery);
            return discovery;
        }

        public bool PurchaseOrAcquireMap(string regionId, float qualityBoost = 50f)
        {
            var region = _state.Regions.FirstOrDefault(r => string.Equals(r.RegionId, regionId, StringComparison.OrdinalIgnoreCase));
            if (region == null) return false;

            region.IsDiscovered = true;
            region.ExplorationProgress = Math.Clamp(region.ExplorationProgress + qualityBoost, 0f, 100f);
            return true;
        }

        public float GetMapCompleteness()
        {
            if (_state.Regions.Count == 0) return 0f;
            float sum = _state.Regions.Sum(r => r.ExplorationProgress);
            return MathF.Round(sum / _state.Regions.Count, 1);
        }

        public MapRegion? GetRegion(string regionId)
        {
            return _state.Regions.FirstOrDefault(r => string.Equals(r.RegionId, regionId, StringComparison.OrdinalIgnoreCase));
        }

        public CartographyState CaptureState()
        {
            var state = new CartographyState
            {
                SchemaVersion = _state.SchemaVersion,
                NextSequence = _state.NextSequence,
                Regions = new List<MapRegion>(_state.Regions.Count),
                Discoveries = new List<MapDiscovery>(_state.Discoveries.Count),
                Cartographers = new List<CartographySkill>(_state.Cartographers.Count)
            };

            foreach (var r in _state.Regions)
            {
                state.Regions.Add(new MapRegion
                {
                    RegionId = r.RegionId,
                    RegionName = r.RegionName,
                    Terrain = r.Terrain,
                    IsDiscovered = r.IsDiscovered,
                    ExplorationProgress = r.ExplorationProgress,
                    KnownPoiIds = new List<string>(r.KnownPoiIds),
                    Hazards = new List<string>(r.Hazards),
                    ScoutingDifficulty = r.ScoutingDifficulty
                });
            }

            foreach (var d in _state.Discoveries)
            {
                state.Discoveries.Add(new MapDiscovery
                {
                    DiscoveryId = d.DiscoveryId,
                    RegionId = d.RegionId,
                    LocationId = d.LocationId,
                    DiscoveredDay = d.DiscoveredDay,
                    DiscoveredBy = d.DiscoveredBy,
                    Quality = d.Quality
                });
            }

            foreach (var c in _state.Cartographers)
            {
                state.Cartographers.Add(new CartographySkill
                {
                    SurvivorId = c.SurvivorId,
                    Proficiency = c.Proficiency,
                    MapsCreated = c.MapsCreated
                });
            }

            return state;
        }

        public void RestoreState(CartographyState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            _state.SchemaVersion = state.SchemaVersion;
            _state.NextSequence = state.NextSequence;
            _state.Regions.Clear();
            _state.Discoveries.Clear();
            _state.Cartographers.Clear();

            if (state.Regions != null)
            {
                foreach (var r in state.Regions)
                {
                    _state.Regions.Add(new MapRegion
                    {
                        RegionId = r.RegionId,
                        RegionName = r.RegionName,
                        Terrain = r.Terrain,
                        IsDiscovered = r.IsDiscovered,
                        ExplorationProgress = r.ExplorationProgress,
                        KnownPoiIds = new List<string>(r.KnownPoiIds ?? Enumerable.Empty<string>()),
                        Hazards = new List<string>(r.Hazards ?? Enumerable.Empty<string>()),
                        ScoutingDifficulty = r.ScoutingDifficulty
                    });
                }
            }

            if (state.Discoveries != null)
            {
                foreach (var d in state.Discoveries)
                {
                    _state.Discoveries.Add(new MapDiscovery
                    {
                        DiscoveryId = d.DiscoveryId,
                        RegionId = d.RegionId,
                        LocationId = d.LocationId,
                        DiscoveredDay = d.DiscoveredDay,
                        DiscoveredBy = d.DiscoveredBy,
                        Quality = d.Quality
                    });
                }
            }

            if (state.Cartographers != null)
            {
                foreach (var c in state.Cartographers)
                {
                    _state.Cartographers.Add(new CartographySkill
                    {
                        SurvivorId = c.SurvivorId,
                        Proficiency = c.Proficiency,
                        MapsCreated = c.MapsCreated
                    });
                }
            }
        }
    }
}
