using System;
using System.Collections.Generic;
using System.IO;

namespace Ashfall.Core.World
{
    /// <summary>
    /// Schema container for world_evolution_events.json catalog.
    /// </summary>
    [Serializable]
    public sealed class WorldEvolutionCatalogContainer
    {
        public int schema_version { get; set; } = 1;
        public List<WorldEvolutionEventDef> events { get; set; } = new List<WorldEvolutionEventDef>();
    }

    /// <summary>
    /// DTO defining an authored world evolution event.
    /// </summary>
    [Serializable]
    public sealed class WorldEvolutionEventDef
    {
        public string id { get; set; } = string.Empty;
        public string type { get; set; } = string.Empty; // "blockade", "territory_flip", "site_degradation", "hazard_bloom"
        public int trigger_day { get; set; }
        public string required_flag { get; set; } = string.Empty;
        public string target_location_id { get; set; } = string.Empty;
        public string target_node_id { get; set; } = string.Empty;
        public string title { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string recast_text { get; set; } = string.Empty;
        public bool locks_node { get; set; }
        public string new_owner { get; set; } = string.Empty;
        public float danger_delta { get; set; }
        public float depletion_amount { get; set; }
        public string added_threat { get; set; } = string.Empty;
        public float contamination_delta { get; set; }
    }

    /// <summary>
    /// Persistent state for WorldEvolutionEngine.
    /// </summary>
    [Serializable]
    public sealed class WorldEvolutionState
    {
        public int schema_version = 1;
        public int lastEvaluatedDay = -1;
        public List<string> triggeredEventIds = new List<string>();
    }

    /// <summary>
    /// Core engine that evaluates and applies living geography mutations for Plan 11.
    /// </summary>
    public sealed class WorldEvolutionEngine
    {
        public const string CatalogFileName = "world_evolution_events.json";

        private readonly List<WorldEvolutionEventDef> _events = new List<WorldEvolutionEventDef>();
        private readonly WorldEvolutionState _state = new WorldEvolutionState();
        private readonly HashSet<string> _triggeredEvents = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<WorldEvolutionEventDef> Events => _events;
        public IReadOnlyCollection<string> TriggeredEventIds => _triggeredEvents;
        public event Action<WorldEvolutionEventDef>? OnEvolutionTriggered;

        public WorldEvolutionEngine(string dataDir, IFileIO? fileIO = null, IJsonSerializer? serializer = null)
        {
            fileIO ??= new FileSystemIO();
            serializer ??= new SystemTextJsonSerializer();

            string fullPath = Path.Combine(dataDir, CatalogFileName);
            if (fileIO.FileExists(fullPath))
            {
                try
                {
                    string json = fileIO.ReadAllText(fullPath);
                    var container = serializer.Deserialize<WorldEvolutionCatalogContainer>(json);
                    if (container?.events != null && container.events.Count > 0)
                    {
                        _events.AddRange(container.events);
                    }
                }
                catch
                {
                    // Fallback to defaults
                }
            }

            if (_events.Count == 0)
            {
                _events.AddRange(GetDefaultEvents());
            }
        }

        public void TickDay(
            int day,
            HashSet<string>? activeWorldFlags,
            LocationEvolutionSystem? evolution,
            LandmarkDegradationSystem? landmarks,
            WastelandMapSystem? map)
        {
            _state.lastEvaluatedDay = day;

            foreach (var evt in _events)
            {
                if (_triggeredEvents.Contains(evt.id)) continue;

                // Check day threshold
                if (day < evt.trigger_day) continue;

                // Check required flag if set
                if (!string.IsNullOrEmpty(evt.required_flag) &&
                    (activeWorldFlags == null || !activeWorldFlags.Contains(evt.required_flag)))
                {
                    continue;
                }

                // Trigger evolution event
                ApplyEvent(evt, day, evolution, landmarks, map);
                _triggeredEvents.Add(evt.id);
                _state.triggeredEventIds.Add(evt.id);
                OnEvolutionTriggered?.Invoke(evt);
            }
        }

        public void ApplyEvent(
            WorldEvolutionEventDef evt,
            int day,
            LocationEvolutionSystem? evolution,
            LandmarkDegradationSystem? landmarks,
            WastelandMapSystem? map)
        {
            if (evt.locks_node && map != null && !string.IsNullOrEmpty(evt.target_node_id))
            {
                map.Lock(evt.target_node_id);
            }

            if (evolution != null && !string.IsNullOrEmpty(evt.target_location_id))
            {
                if (!string.IsNullOrEmpty(evt.new_owner))
                {
                    evolution.SetLocationOwner(evt.target_location_id, evt.new_owner);
                }

                if (evt.depletion_amount > 0f)
                {
                    evolution.MarkDepleted(evt.target_location_id, evt.depletion_amount);
                }

                if (!string.IsNullOrEmpty(evt.added_threat))
                {
                    evolution.AddThreat(evt.target_location_id, evt.added_threat);
                }

                if (evt.contamination_delta > 0f)
                {
                    var rec = evolution.GetOrCreateRecord(evt.target_location_id);
                    if (rec != null)
                    {
                        rec.contaminationLevel = Math.Clamp(rec.contaminationLevel + evt.contamination_delta, 0f, 1f);
                    }
                }
            }

            if (landmarks != null && !string.IsNullOrEmpty(evt.target_location_id) && evt.type == "site_degradation")
            {
                landmarks.DamageLandmark(evt.target_location_id, 50f, day);
            }
        }

        public WorldEvolutionState CaptureState()
        {
            return new WorldEvolutionState
            {
                schema_version = 1,
                lastEvaluatedDay = _state.lastEvaluatedDay,
                triggeredEventIds = new List<string>(_triggeredEvents)
            };
        }

        public void RestoreState(WorldEvolutionState? saved, WastelandMapSystem? map = null)
        {
            if (saved == null) return;
            _state.lastEvaluatedDay = saved.lastEvaluatedDay;
            _triggeredEvents.Clear();
            _state.triggeredEventIds.Clear();

            if (saved.triggeredEventIds != null)
            {
                foreach (var id in saved.triggeredEventIds)
                {
                    _triggeredEvents.Add(id);
                    _state.triggeredEventIds.Add(id);

                    // Re-apply locked nodes on map
                    var evt = _events.Find(e => string.Equals(e.id, id, StringComparison.OrdinalIgnoreCase));
                    if (evt != null && evt.locks_node && map != null && !string.IsNullOrEmpty(evt.target_node_id))
                    {
                        map.Lock(evt.target_node_id);
                    }
                }
            }
        }

        public static List<WorldEvolutionEventDef> GetDefaultEvents()
        {
            return new List<WorldEvolutionEventDef>
            {
                new WorldEvolutionEventDef
                {
                    id = "event_evolution_checkpoint_kilo",
                    type = "blockade",
                    trigger_day = 20,
                    target_location_id = "loc_cut_abandoned_depot",
                    target_node_id = "loc_cut_abandoned_depot",
                    title = "Faction Checkpoint Established",
                    locks_node = true
                },
                new WorldEvolutionEventDef
                {
                    id = "event_evolution_bridge_debris",
                    type = "blockade",
                    trigger_day = 35,
                    target_location_id = "loc_eastern_road",
                    target_node_id = "loc_cut_arsenal_ruin",
                    title = "Bridge Debris Collapse",
                    locks_node = true
                },
                new WorldEvolutionEventDef
                {
                    id = "event_evolution_cut_road_closure",
                    type = "blockade",
                    trigger_day = 50,
                    target_location_id = "loc_cut_arsenal_ruin",
                    target_node_id = "loc_cut_arsenal_ruin",
                    title = "Contested Road Barricade",
                    locks_node = true
                },
                new WorldEvolutionEventDef
                {
                    id = "event_evolution_warlord_expansion",
                    type = "territory_flip",
                    trigger_day = 18,
                    target_location_id = "loc_neutral_ground",
                    target_node_id = "loc_neutral_ground",
                    title = "Warlords Claim Neutral Ground",
                    new_owner = "warlords_sector_4",
                    danger_delta = 2f
                },
                new WorldEvolutionEventDef
                {
                    id = "event_evolution_faction_retreat",
                    type = "territory_flip",
                    trigger_day = 45,
                    target_location_id = "loc_black_flotilla_outpost",
                    target_node_id = "loc_black_flotilla_outpost",
                    title = "Flotilla Garrison Retreat",
                    new_owner = "none",
                    danger_delta = -2f
                },
                new WorldEvolutionEventDef
                {
                    id = "event_evolution_warehouse_stripped",
                    type = "site_degradation",
                    trigger_day = 15,
                    target_location_id = "loc_excavation_utility_tunnels",
                    target_node_id = "loc_excavation_utility_tunnels",
                    title = "Utility Warehouse Stripped Bare",
                    depletion_amount = 0.4f
                },
                new WorldEvolutionEventDef
                {
                    id = "event_evolution_settlement_abandoned",
                    type = "site_degradation",
                    trigger_day = 40,
                    target_location_id = "suburban_house",
                    target_node_id = "suburban_house",
                    title = "Outskirts Settlement Abandoned",
                    depletion_amount = 0.5f
                },
                new WorldEvolutionEventDef
                {
                    id = "event_evolution_water_tower_collapse",
                    type = "site_degradation",
                    trigger_day = 60,
                    target_location_id = "loc_water_station",
                    target_node_id = "loc_water_station",
                    title = "Water Station Collapse",
                    depletion_amount = 0.6f
                },
                new WorldEvolutionEventDef
                {
                    id = "event_evolution_rad_hotspot_bloom",
                    type = "hazard_bloom",
                    trigger_day = 12,
                    target_location_id = "loc_cut_radiation_zone_alpha",
                    target_node_id = "loc_cut_radiation_zone_alpha",
                    title = "Fallout Dust Plume Intensifies",
                    added_threat = "threat_rad_squatters",
                    contamination_delta = 0.35f
                },
                new WorldEvolutionEventDef
                {
                    id = "event_evolution_subway_mold_bloom",
                    type = "hazard_bloom",
                    trigger_day = 25,
                    target_location_id = "loc_excavation_metro_interchange",
                    target_node_id = "loc_excavation_metro_interchange",
                    title = "Spore Mold Bloom",
                    added_threat = "threat_wild_beasts",
                    contamination_delta = 0.25f
                }
            };
        }
    }
}
