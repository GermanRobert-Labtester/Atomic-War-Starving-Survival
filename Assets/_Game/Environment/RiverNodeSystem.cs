using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Environment
{
    [Serializable]
    public class RiverNodeState
    {
        public string nodeId;
        public bool hasBridge = false;
        public bool isBlockaded = false;
        public int blockadeTollCost = 0;
        public float crossWithoutBridgeRadExposure = 50f;
        public float crossWithoutBridgeTimeMultiplier = 10f;
    }

    [Serializable]
    public class RiverNodeSave
    {
        public List<RiverNodeState> entries = new List<RiverNodeState>();
    }

    /// <summary>
    /// Crossing result returned by TryCrossRiver.
    /// </summary>
    public struct RiverCrossingResult
    {
        public bool success;
        public float timeCostHours;
        public float radiationExposure;
        public int fuelCost;
        public int tollPaid;
        public string method;
        public string message;
    }

    /// <summary>
    /// Prompt #569: System: River Map Nodes and Bridges.
    /// Rivers split the map. Crossing without a bridge takes 10x travel time and
    /// guarantees Radiation exposure. Bridges are natural chokepoints with 90% chance
    /// of FactionBlockade. Players must pay tolls, fight, or find a boat.
    /// </summary>
    public class RiverNodeSystem
    {
        private const float BlockadeChance = 0.90f;
        private const int DefaultTollCost = 5;
        private const float WadeRadExposure = 50f;
        private const float WadeTimeMultiplier = 10f;

        private readonly Dictionary<string, RiverNodeState> _riverNodes = new Dictionary<string, RiverNodeState>();

        public event Action<string, string> OnRiverCrossed;            // (nodeId, method)
        public event Action<string> OnBlockadeEncountered;             // (nodeId)
        public event Action<string, int> OnTollPaid;                   // (nodeId, tollCost)
        public event Action<string, float> OnRadiationExposureFromWading; // (nodeId, radAmount)

        public IReadOnlyDictionary<string, RiverNodeState> RiverNodes => _riverNodes;

        /// <summary>
        /// Deterministically generate river nodes across the map using a seeded RNG.
        /// Approximately 20% of map nodes become river crossings; bridges placed on ~50%.
        /// </summary>
        public void GenerateRiverNodes(List<MapNode> mapNodes, int seed)
        {
            _riverNodes.Clear();
            if (mapNodes == null || mapNodes.Count == 0) return;

            var rng = new System.Random(seed);

            for (int i = 0; i < mapNodes.Count; i++)
            {
                // ~20% of nodes become river crossings
                if (rng.NextDouble() > 0.20) continue;

                var nodeId = mapNodes[i].NodeId;
                var state = new RiverNodeState
                {
                    nodeId = nodeId,
                    hasBridge = rng.NextDouble() < 0.50,
                    isBlockaded = false,
                    blockadeTollCost = 0
                };

                if (state.hasBridge)
                {
                    // 90% chance of blockade on bridges
                    if (rng.NextDouble() < BlockadeChance)
                    {
                        state.isBlockaded = true;
                        state.blockadeTollCost = DefaultTollCost + rng.Next(0, 6); // 5-10
                    }
                }

                _riverNodes[nodeId] = state;
            }
        }

        /// <summary>
        /// Attempt to cross the river at the given node using the specified method.
        /// Methods: "bridge" (subject to blockade), "wade" (10x time, rad exposure), "boat" (requires rowboat).
        /// </summary>
        public RiverCrossingResult TryCrossRiver(string nodeId, string method, int playerFuel, System.Random rng)
        {
            var result = new RiverCrossingResult { method = method };

            if (string.IsNullOrEmpty(nodeId) || string.IsNullOrEmpty(method))
            {
                result.message = "Invalid crossing parameters.";
                return result;
            }

            if (!_riverNodes.TryGetValue(nodeId, out var state))
            {
                result.message = "No river at this location.";
                return result;
            }

            switch (method)
            {
                case "bridge":
                    if (!state.hasBridge)
                    {
                        result.message = "There is no bridge here.";
                        return result;
                    }
                    if (state.isBlockaded)
                    {
                        OnBlockadeEncountered?.Invoke(nodeId);
                        result.message = "A faction blockade blocks the bridge. Pay toll or find another way.";
                        result.tollPaid = state.blockadeTollCost;
                        return result;
                    }
                    result.success = true;
                    result.timeCostHours = 1f;
                    result.message = "Crossed the bridge safely.";
                    OnRiverCrossed?.Invoke(nodeId, method);
                    break;

                case "wade":
                    result.success = true;
                    result.timeCostHours = WadeTimeMultiplier;
                    result.radiationExposure = WadeRadExposure;
                    result.message = "Waded through the irradiated river. It took far too long.";
                    OnRadiationExposureFromWading?.Invoke(nodeId, WadeRadExposure);
                    OnRiverCrossed?.Invoke(nodeId, method);
                    break;

                case "boat":
                    if (playerFuel < 1)
                    {
                        result.message = "No fuel to power the boat.";
                        return result;
                    }
                    result.success = true;
                    result.timeCostHours = 2f;
                    result.fuelCost = 1;
                    result.message = "Crossed by boat. The water was still and silent.";
                    OnRiverCrossed?.Invoke(nodeId, method);
                    break;

                default:
                    result.message = "Unknown crossing method.";
                    break;
            }

            return result;
        }

        /// <summary>
        /// Pay the blockade toll at a bridge node, clearing the blockade.
        /// Returns false if the node has no bridge or is not blockaded.
        /// </summary>
        public bool PayToll(string nodeId, int availableFuel)
        {
            if (!_riverNodes.TryGetValue(nodeId, out var state)) return false;
            if (!state.isBlockaded) return false;
            if (availableFuel < state.blockadeTollCost) return false;

            int toll = state.blockadeTollCost;
            state.isBlockaded = false;
            state.blockadeTollCost = 0;

            OnTollPaid?.Invoke(nodeId, toll);
            return true;
        }

        /// <summary>
        /// Returns a summary of crossing costs for the given method at a node.
        /// Returns null if the node is not a river node.
        /// </summary>
        public RiverCrossingResult GetCrossingCost(string nodeId, string method)
        {
            if (!_riverNodes.TryGetValue(nodeId, out var state))
                return new RiverCrossingResult { message = "No river here." };

            var result = new RiverCrossingResult { method = method };

            switch (method)
            {
                case "bridge":
                    result.timeCostHours = state.hasBridge ? 1f : 0f;
                    result.message = state.hasBridge
                        ? (state.isBlockaded ? "Bridge is blockaded." : "Bridge is clear.")
                        : "No bridge at this location.";
                    break;
                case "wade":
                    result.timeCostHours = WadeTimeMultiplier;
                    result.radiationExposure = WadeRadExposure;
                    result.message = "Wading: slow and irradiated.";
                    break;
                case "boat":
                    result.timeCostHours = 2f;
                    result.fuelCost = 1;
                    result.message = "Boat crossing: requires fuel.";
                    break;
                default:
                    result.message = "Unknown method.";
                    break;
            }

            return result;
        }

        public RiverNodeSave CaptureState()
        {
            var save = new RiverNodeSave();
            foreach (var kvp in _riverNodes)
            {
                save.entries.Add(new RiverNodeState
                {
                    nodeId = kvp.Value.nodeId,
                    hasBridge = kvp.Value.hasBridge,
                    isBlockaded = kvp.Value.isBlockaded,
                    blockadeTollCost = kvp.Value.blockadeTollCost,
                    crossWithoutBridgeRadExposure = kvp.Value.crossWithoutBridgeRadExposure,
                    crossWithoutBridgeTimeMultiplier = kvp.Value.crossWithoutBridgeTimeMultiplier
                });
            }
            return save;
        }

        public void RestoreState(RiverNodeSave save)
        {
            _riverNodes.Clear();
            if (save?.entries == null) return;
            foreach (var entry in save.entries)
            {
                _riverNodes[entry.nodeId] = new RiverNodeState
                {
                    nodeId = entry.nodeId,
                    hasBridge = entry.hasBridge,
                    isBlockaded = entry.isBlockaded,
                    blockadeTollCost = entry.blockadeTollCost,
                    crossWithoutBridgeRadExposure = entry.crossWithoutBridgeRadExposure,
                    crossWithoutBridgeTimeMultiplier = entry.crossWithoutBridgeTimeMultiplier
                };
            }
        }
    }
}
