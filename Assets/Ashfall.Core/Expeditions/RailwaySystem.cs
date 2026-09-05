// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Expeditions
{
    public enum TrainDispatchStatus
    {
        Idle,
        Preparing,
        EnRoute,
        Derailment,
        RobberyAmbush,
        Arrived
    }

    [Serializable]
    public sealed class RailNodeDef
    {
        public string node_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string zone_id { get; set; } = string.Empty;
        public string node_type { get; set; } = "Terminal";
    }

    [Serializable]
    public sealed class TrackSegmentDef
    {
        public string segment_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string start_node_id { get; set; } = string.Empty;
        public string end_node_id { get; set; } = string.Empty;
        public float distance_km { get; set; } = 20.0f;
        public float base_integrity { get; set; } = 0.8f;
        public bool bridge_required { get; set; } = false;
        public float max_train_mass { get; set; } = 200.0f;
        public List<string> hazard_tags { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class TrainCarDef
    {
        public string car_type_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public float empty_mass { get; set; } = 30.0f;
        public float cargo_capacity { get; set; } = 50.0f;
        public float armor_rating { get; set; } = 50.0f;
        public float max_fuel_capacity { get; set; } = 0.0f;
        public float fuel_burn_per_km { get; set; } = 0.0f;
        public string vehicle_class { get; set; } = "locomotive";      // "locomotive", "handcar"
        public float crew_stamina_max { get; set; } = 1.0f;              // 0..1
        public float stamina_drain_per_km { get; set; } = 0.0f;           // stamina units per km
        public float stamina_recovery_per_stop { get; set; } = 0.3f;       // recovered at terminal
    }

    [Serializable]
    public sealed class RailwayNetworkCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<RailNodeDef> nodes { get; set; } = new List<RailNodeDef>();
        public List<TrackSegmentDef> segments { get; set; } = new List<TrackSegmentDef>();
        public List<TrainCarDef> cars { get; set; } = new List<TrainCarDef>();
    }

    [Serializable]
    public sealed class TrackSegmentState
    {
        public string segmentId { get; set; } = string.Empty;
        public float integrity { get; set; } = 1.0f;
        public bool bridgeIntact { get; set; } = true;
        public bool isSabotaged { get; set; } = false;
    }

    [Serializable]
    public sealed class TrainCarInstance
    {
        public string instanceId { get; set; } = string.Empty;
        public string carTypeId { get; set; } = string.Empty;
        public float condition { get; set; } = 100.0f;
    }

    [Serializable]
    public sealed class TrainState
    {
        public string trainId { get; set; } = string.Empty;
        public string displayName { get; set; } = string.Empty;
        public string currentNodeId { get; set; } = string.Empty;
        public string? activeSegmentId { get; set; } = null;
        public float segmentProgress { get; set; } = 0.0f;
        public float currentFuel { get; set; } = 100.0f;
        public float maxFuel { get; set; } = 300.0f;
        public List<TrainCarInstance> cars { get; set; } = new List<TrainCarInstance>();
        public TrainDispatchStatus status { get; set; } = TrainDispatchStatus.Idle;
        public List<string> plannedPath { get; set; } = new List<string>();
        public float crewStamina { get; set; } = 1.0f;                  // 0..1
        public float maxCrewStamina { get; set; } = 1.0f;
        public float staminaDrainPerKm { get; set; } = 0.0f;
        public float staminaRecoveryPerStop { get; set; } = 0.3f;
        public bool isCrewExhausted { get; set; } = false;
        public string vehicleClass { get; set; } = "locomotive";
        public bool isOnExpedition { get; set; } = false;
    }

    [Serializable]
    public sealed class RailwayState
    {
        public int schema_version { get; set; } = 1;
        public Dictionary<string, TrackSegmentState> segments { get; set; } = new Dictionary<string, TrackSegmentState>(StringComparer.Ordinal);
        public List<TrainState> trains { get; set; } = new List<TrainState>();
    }

    public sealed class RailwaySystem
    {
        private readonly ISeededRng _rng;
        private readonly Inventory.Inventory _inventory;
        private readonly ILog _log;

        private readonly Dictionary<string, RailNodeDef> _nodes = new Dictionary<string, RailNodeDef>(StringComparer.Ordinal);
        private readonly Dictionary<string, TrackSegmentDef> _segmentDefs = new Dictionary<string, TrackSegmentDef>(StringComparer.Ordinal);
        private readonly Dictionary<string, TrainCarDef> _carDefs = new Dictionary<string, TrainCarDef>(StringComparer.Ordinal);
        private RailwayState _state = new RailwayState();

        public event Action<string, string>? OnTrainDispatched;
        public event Action<string, string>? OnTrainArrived;
        public event Action<string, string>? OnDerailment;
        public event Action<string, string>? OnTrainAmbushed;
        public event Action<string, float>? OnTrackRepaired;

        public RailwayState State => _state;
        public IReadOnlyDictionary<string, RailNodeDef> Nodes => _nodes;
        public IReadOnlyDictionary<string, TrackSegmentDef> SegmentDefs => _segmentDefs;

        public RailwaySystem(
            ISeededRng? rng = null,
            Inventory.Inventory? inventory = null,
            ILog? log = null)
        {
            _rng = rng ?? new SeededRng(191);
            _inventory = inventory ?? new Inventory.Inventory();
            _log = log ?? NullLog.Instance;
        }

        public void RegisterCatalog(RailwayNetworkCatalog catalog)
        {
            if (catalog == null) return;
            foreach (var n in catalog.nodes) _nodes[n.node_id] = n;
            foreach (var s in catalog.segments)
            {
                _segmentDefs[s.segment_id] = s;
                if (!_state.segments.ContainsKey(s.segment_id))
                {
                    _state.segments[s.segment_id] = new TrackSegmentState
                    {
                        segmentId = s.segment_id,
                        integrity = s.base_integrity,
                        bridgeIntact = !s.bridge_required || s.base_integrity >= 0.5f,
                        isSabotaged = false
                    };
                }
            }
            foreach (var c in catalog.cars) _carDefs[c.car_type_id] = c;
        }

        public TrackSegmentState EnsureSegmentState(string segmentId)
        {
            if (!_state.segments.TryGetValue(segmentId, out var segState))
            {
                _segmentDefs.TryGetValue(segmentId, out var def);
                segState = new TrackSegmentState
                {
                    segmentId = segmentId,
                    integrity = def?.base_integrity ?? 0.5f,
                    bridgeIntact = !(def?.bridge_required ?? false)
                };
                _state.segments[segmentId] = segState;
            }
            return segState;
        }

        public TrainState CreateStarterTrain(string trainId, string displayName, string startingNodeId)
        {
            var train = new TrainState
            {
                trainId = trainId,
                displayName = displayName,
                currentNodeId = startingNodeId,
                currentFuel = 250.0f,
                maxFuel = 500.0f,
                status = TrainDispatchStatus.Idle,
                cars = new List<TrainCarInstance>
                {
                    new TrainCarInstance { instanceId = $"{trainId}_locomotive", carTypeId = "car_locomotive_diesel" },
                    new TrainCarInstance { instanceId = $"{trainId}_hopper", carTypeId = "car_freight_hopper" }
                }
            };
            _state.trains.Add(train);
            return train;
        }

        public ActionResult RepairTrack(string segmentId, float integrityRestored)
        {
            if (!_segmentDefs.TryGetValue(segmentId, out var def))
                return ActionResult.Blocked("unknown_segment", "railway.unknown_segment");

            var seg = EnsureSegmentState(segmentId);
            if (seg.integrity >= 1.0f)
                return ActionResult.Blocked("already_repaired", "railway.already_repaired");

            // Check steel rail or scrap
            if (_inventory.CountById("steel_rail_segment") < 1 && _inventory.CountById("scrap_metal") < 10)
                return ActionResult.Blocked("insufficient_repair_materials", "railway.insufficient_repair_materials");

            if (_inventory.CountById("steel_rail_segment") >= 1)
                _inventory.RemoveById("steel_rail_segment", 1);
            else
                _inventory.RemoveById("scrap_metal", 10);

            seg.integrity = Math.Min(1.0f, seg.integrity + integrityRestored);
            seg.isSabotaged = false;
            OnTrackRepaired?.Invoke(segmentId, seg.integrity);

            return ActionResult.Success("railway.track_repaired");
        }

        public ActionResult RepairBridge(string segmentId)
        {
            if (!_segmentDefs.TryGetValue(segmentId, out var def))
                return ActionResult.Blocked("unknown_segment", "railway.unknown_segment");

            var seg = EnsureSegmentState(segmentId);
            if (seg.bridgeIntact)
                return ActionResult.Blocked("bridge_intact", "railway.bridge_intact");

            if (_inventory.CountById("steel_rail_segment") < 2 || _inventory.CountById("railroad_ties") < 2)
                return ActionResult.Blocked("insufficient_bridge_materials", "railway.insufficient_bridge_materials");

            _inventory.RemoveById("steel_rail_segment", 2);
            _inventory.RemoveById("railroad_ties", 2);

            seg.bridgeIntact = true;
            seg.integrity = Math.Max(seg.integrity, 0.75f);

            return ActionResult.Success("railway.bridge_reconstructed");
        }

        public bool CanTraverseSegment(TrainState train, string segmentId)
        {
            if (!_segmentDefs.TryGetValue(segmentId, out var def)) return false;
            var seg = EnsureSegmentState(segmentId);

            if (seg.integrity < 0.40f) return false;
            if (def.bridge_required && !seg.bridgeIntact) return false;
            if (seg.isSabotaged) return false;

            float totalMass = CalculateTrainMass(train);
            if (totalMass > def.max_train_mass) return false;

            return true;
        }

        public float CalculateTrainMass(TrainState train)
        {
            float mass = 0f;
            for (int i = 0; i < train.cars.Count; i++)
            {
                if (_carDefs.TryGetValue(train.cars[i].carTypeId, out var cDef))
                {
                    mass += cDef.empty_mass;
                }
                else
                {
                    mass += 25f;
                }
            }
            return mass;
        }

        public float EstimateCoalRequired(TrainState train, string segmentId)
        {
            if (!_segmentDefs.TryGetValue(segmentId, out var def)) return 0f;
            float mass = CalculateTrainMass(train);
            float burnRate = 2.0f + (mass * 0.015f);
            return def.distance_km * burnRate;
        }

        public ActionResult DispatchTrain(string trainId, string segmentId)
        {
            var train = _state.trains.Find(t => t.trainId == trainId);
            if (train == null) return ActionResult.Blocked("train_not_found", "railway.train_not_found");
            if (train.status != TrainDispatchStatus.Idle && train.status != TrainDispatchStatus.Arrived)
                return ActionResult.Blocked("train_not_idle", "railway.train_not_idle");

            if (!_segmentDefs.TryGetValue(segmentId, out var def))
                return ActionResult.Blocked("invalid_segment", "railway.invalid_segment");

            if (def.start_node_id != train.currentNodeId && def.end_node_id != train.currentNodeId)
                return ActionResult.Blocked("segment_not_connected", "railway.segment_not_connected");

            if (!CanTraverseSegment(train, segmentId))
                return ActionResult.Blocked("cannot_traverse_segment", "railway.cannot_traverse_segment");

            float coalNeeded = EstimateCoalRequired(train, segmentId);
            if (train.currentFuel < coalNeeded)
            {
                // Attempt to fuel from inventory
                int coalItemsNeeded = (int)Math.Ceiling(coalNeeded - train.currentFuel);
                if (_inventory.CountById("train_coal") < coalItemsNeeded)
                    return ActionResult.Blocked("insufficient_fuel", "railway.insufficient_fuel");

                _inventory.RemoveById("train_coal", coalItemsNeeded);
                train.currentFuel += coalItemsNeeded;
            }

            train.currentFuel -= coalNeeded;
            train.activeSegmentId = segmentId;
            train.segmentProgress = 0f;
            train.status = TrainDispatchStatus.EnRoute;

            OnTrainDispatched?.Invoke(trainId, segmentId);
            return ActionResult.Success("railway.train_dispatched");
        }

        public void TickTravel(string trainId, float progressDelta = 0.5f)
        {
            var train = _state.trains.Find(t => t.trainId == trainId);
            if (train == null || train.status != TrainDispatchStatus.EnRoute || string.IsNullOrEmpty(train.activeSegmentId))
                return;

            var seg = EnsureSegmentState(train.activeSegmentId);
            _segmentDefs.TryGetValue(train.activeSegmentId, out var def);

            // 1. Derailment risk check on degraded track (Plan 73 §7.6)
            if (seg.integrity < 0.65f)
            {
                double derailChance = (0.65f - seg.integrity) * 0.40;
                if (_rng.NextDouble() < derailChance)
                {
                    train.status = TrainDispatchStatus.Derailment;
                    // Consequences: segment integrity loss + car condition damage
                    seg.integrity = Math.Max(0f, seg.integrity - 0.15f);
                    for (int i = 1; i < train.cars.Count && i <= 2; i++)
                    {
                        train.cars[i].condition = Math.Max(0f, train.cars[i].condition - 30f);
                    }
                    OnDerailment?.Invoke(trainId, train.activeSegmentId);
                    return;
                }
            }

            // 2. Ambush check in hostile territory
            if (def != null && def.hazard_tags.Contains("ambush_point"))
            {
                if (_rng.NextDouble() < 0.15)
                {
                    train.status = TrainDispatchStatus.RobberyAmbush;
                    OnTrainAmbushed?.Invoke(trainId, train.activeSegmentId);
                    return;
                }
            }

            // 3. Stamina drain for handcar (Plan 73 §7.1)
            float effectiveDelta = progressDelta;
            if (def != null && train.cars.Count > 0
                && _carDefs.TryGetValue(train.cars[0].carTypeId, out var leadCarDef)
                && leadCarDef.vehicle_class == "handcar")
            {
                float drain = leadCarDef.stamina_drain_per_km * def.distance_km * progressDelta;
                train.crewStamina = Math.Max(0f, train.crewStamina - drain);
                if (train.crewStamina <= 0f && !train.isCrewExhausted)
                    train.isCrewExhausted = true;
                if (train.isCrewExhausted)
                    effectiveDelta *= 0.5f; // exhaustion speed penalty
            }

            // 4. Advance progress
            train.segmentProgress += effectiveDelta;
            if (train.segmentProgress >= 1.0f)
            {
                train.segmentProgress = 1.0f;
                train.status = TrainDispatchStatus.Arrived;

                // Recover crew stamina at terminal stop (handcar)
                if (train.cars.Count > 0
                    && _carDefs.TryGetValue(train.cars[0].carTypeId, out var recoverCar))
                {
                    float maxStamina = train.maxCrewStamina > 0f ? train.maxCrewStamina : 1f;
                    train.crewStamina = Math.Min(maxStamina, train.crewStamina + recoverCar.stamina_recovery_per_stop);
                    train.isCrewExhausted = false;
                }

                // Set new current node to destination
                if (def != null)
                {
                    train.currentNodeId = (train.currentNodeId == def.start_node_id) ? def.end_node_id : def.start_node_id;
                }

                string arrivedAt = train.currentNodeId;
                train.activeSegmentId = null;
                OnTrainArrived?.Invoke(trainId, arrivedAt);

                // Auto-advance along planned expedition route (switchyard / Plan 73 §7.4)
                if (train.plannedPath.Count > 0)
                {
                    string nextSeg = train.plannedPath[0];
                    train.plannedPath.RemoveAt(0);
                    if (_segmentDefs.TryGetValue(nextSeg, out var nextDef) &&
                        (nextDef.start_node_id == train.currentNodeId || nextDef.end_node_id == train.currentNodeId))
                    {
                        if (CanTraverseSegment(train, nextSeg))
                        {
                            train.activeSegmentId = nextSeg;
                            train.segmentProgress = 0f;
                            train.status = TrainDispatchStatus.EnRoute;
                            OnTrainDispatched?.Invoke(trainId, nextSeg);
                        }
                        else
                        {
                            train.status = TrainDispatchStatus.Idle;
                        }
                    }
                    else
                    {
                        train.status = TrainDispatchStatus.Idle;
                    }
                }
            }
        }

        /// <summary>Plan a multi-segment route from the train's current node (Plan 73 §7.4).</summary>
        public ActionResult PlanRoute(string trainId, List<string> segmentIds)
        {
            var train = _state.trains.Find(t => t.trainId == trainId);
            if (train == null) return ActionResult.Blocked("train_not_found", "railway.train_not_found");
            if (train.status != TrainDispatchStatus.Idle && train.status != TrainDispatchStatus.Arrived)
                return ActionResult.Blocked("train_not_idle", "railway.train_not_idle");

            train.plannedPath.Clear();
            string currentNode = train.currentNodeId;
            foreach (var segId in segmentIds)
            {
                if (!_segmentDefs.TryGetValue(segId, out var def))
                    return ActionResult.Blocked("invalid_segment", "railway.invalid_segment");
                if (def.start_node_id != currentNode && def.end_node_id != currentNode)
                    return ActionResult.Blocked("segment_not_connected", "railway.segment_not_connected");
                train.plannedPath.Add(segId);
                currentNode = (def.start_node_id == currentNode) ? def.end_node_id : def.start_node_id;
            }
            return ActionResult.Success("railway.route_planned");
        }

        /// <summary>Shortest-path BFS between two nodes.</summary>
        private List<string>? FindShortestPath(string startNodeId, string endNodeId)
        {
            if (startNodeId == endNodeId) return new List<string>();
            var queue = new Queue<(string node, List<string> path)>();
            var visited = new HashSet<string>(StringComparer.Ordinal) { startNodeId };
            queue.Enqueue((startNodeId, new List<string>()));
            while (queue.Count > 0)
            {
                var (node, path) = queue.Dequeue();
                foreach (var seg in _segmentDefs.Values)
                {
                    string neighbor = seg.start_node_id == node ? seg.end_node_id
                        : seg.end_node_id == node ? seg.start_node_id
                        : null;
                    if (neighbor == null) continue;
                    if (!visited.Add(neighbor)) continue;
                    var newPath = new List<string>(path) { seg.segment_id };
                    if (neighbor == endNodeId) return newPath;
                    queue.Enqueue((neighbor, newPath));
                }
            }
            return null;
        }

        /// <summary>Estimate a rail expedition from origin to destination (Plan 73 §7.3).</summary>
        public RailExpeditionEstimate? EstimateExpeditionTravel(string originNodeId, string destinationNodeId)
        {
            if (!_nodes.ContainsKey(originNodeId) || !_nodes.ContainsKey(destinationNodeId))
                return null;
            var path = FindShortestPath(originNodeId, destinationNodeId);
            if (path == null || path.Count == 0) return null;

            float totalFuel = 0f;
            float totalStamina = 0f;
            foreach (var segId in path)
            {
                if (_segmentDefs.TryGetValue(segId, out var def))
                {
                    totalFuel += def.distance_km * 2.0f; // rough locomotive per-km burn
                    totalStamina += 0.035f * def.distance_km; // handcar stamina drain
                }
            }
            return new RailExpeditionEstimate
            {
                travelTicks = path.Count * 2,
                fuelRequired = totalFuel,
                staminaCost = totalStamina,
                path = path
            };
        }

        /// <summary>Dispatch a train on an expedition route (Plan 73 §7.3).</summary>
        public ActionResult DispatchExpedition(string trainId, string destinationNodeId)
        {
            var train = _state.trains.Find(t => t.trainId == trainId);
            if (train == null) return ActionResult.Blocked("train_not_found", "railway.train_not_found");
            if (train.status != TrainDispatchStatus.Idle && train.status != TrainDispatchStatus.Arrived)
                return ActionResult.Blocked("train_not_idle", "railway.train_not_idle");

            if (!_nodes.ContainsKey(destinationNodeId))
                return ActionResult.Blocked("invalid_destination", "railway.invalid_destination");

            var path = FindShortestPath(train.currentNodeId, destinationNodeId);
            if (path == null || path.Count == 0)
                return ActionResult.Blocked("no_route", "railway.no_route");

            float totalFuel = 0f;
            foreach (var segId in path)
                totalFuel += EstimateCoalRequired(train, segId);

            if (train.currentFuel < totalFuel)
            {
                int coalItemsNeeded = (int)Math.Ceiling(totalFuel - train.currentFuel);
                if (_inventory.CountById("train_coal") < coalItemsNeeded)
                    return ActionResult.Blocked("insufficient_fuel", "railway.insufficient_fuel");
                _inventory.RemoveById("train_coal", coalItemsNeeded);
                train.currentFuel += totalFuel;
            }
            train.currentFuel -= totalFuel;

            train.isOnExpedition = true;
            train.plannedPath.Clear();
            train.plannedPath.AddRange(path);
            return DispatchFirstSegment(train);
        }

        private ActionResult DispatchFirstSegment(TrainState train)
        {
            if (train.plannedPath.Count == 0)
                return ActionResult.Blocked("empty_route", "railway.empty_route");
            string firstSeg = train.plannedPath[0];
            if (!CanTraverseSegment(train, firstSeg))
                return ActionResult.Blocked("cannot_traverse_first_segment", "railway.cannot_traverse_segment");
            train.activeSegmentId = firstSeg;
            train.segmentProgress = 0f;
            train.status = TrainDispatchStatus.EnRoute;
            train.plannedPath.RemoveAt(0);
            OnTrainDispatched?.Invoke(train.trainId, firstSeg);
            return ActionResult.Success("railway.expedition_dispatched");
        }

        public ActionResult ClearDerailment(string trainId)
        {
            var train = _state.trains.Find(t => t.trainId == trainId);
            if (train == null) return ActionResult.Blocked("train_not_found", "railway.train_not_found");
            if (train.status != TrainDispatchStatus.Derailment)
                return ActionResult.Blocked("not_derailed", "railway.not_derailed");

            train.status = TrainDispatchStatus.Idle;
            train.segmentProgress = 0f;
            train.activeSegmentId = null;
            train.isCrewExhausted = false;

            return ActionResult.Success("railway.derailment_cleared");
        }

        public void RestoreState(RailwayState state)
        {
            if (state == null) return;
            _state = state;
        }

        /// <summary>Simple estimate DTO for rail expedition planning.</summary>
        public sealed class RailExpeditionEstimate
        {
            public int travelTicks { get; set; }
            public float fuelRequired { get; set; }
            public float staminaCost { get; set; }
            public List<string> path { get; set; } = new List<string>();
        }
    }
}
