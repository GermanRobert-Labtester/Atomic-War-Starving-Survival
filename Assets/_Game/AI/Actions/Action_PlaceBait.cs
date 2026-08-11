using System;
using System.Collections.Generic;

namespace AtomicWar._Game.AI.Actions
{
    [Serializable]
    public class BaitStationState
    {
        public string actionId = "action_place_bait";
        public float hoursUntilSpawn = 24f;
        public bool isPlaced = false;
    }

    /// <summary>
    /// Per-node bait tracking data. Stored alongside the station state so
    /// CaptureState / RestoreState can persist multiple active bait sites.
    /// </summary>
    [Serializable]
    public class BaitNodeData
    {
        public string nodeId;
        public string baitType;
        public string survivorId;
        public float hoursRemaining;
        public bool spawnReady;
    }

    /// <summary>DEMOTE-Action-remaining — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class Action_PlaceBait
    {
        public event Action<string, string> OnBaitPlaced;
        public event Action<string, string> OnMutantPackSpawned;
        public event Action<string> OnBaitExpired;

        private BaitStationState _state;
        private readonly Dictionary<string, BaitNodeData> _activeBaits
            = new Dictionary<string, BaitNodeData>();

        public Action_PlaceBait()
        {
            _state = new BaitStationState();
        }

        public Action_PlaceBait(BaitStationState state)
        {
            _state = state ?? new BaitStationState();
        }

        public BaitStationState CaptureState() => _state;

        public void RestoreState(BaitStationState state)
        {
            _state = state ?? new BaitStationState();
        }

        public Dictionary<string, BaitNodeData> CaptureBaitNodes()
        {
            return new Dictionary<string, BaitNodeData>(_activeBaits);
        }

        public void RestoreBaitNodes(Dictionary<string, BaitNodeData> nodes)
        {
            _activeBaits.Clear();
            if (nodes != null)
            {
                foreach (var kvp in nodes)
                    _activeBaits[kvp.Key] = kvp.Value;
            }
        }

        /// <summary>
        /// Places bait (human corpse or spoiled meat) on a map node.
        /// The bait will attract a mutant pack after 24 hours.
        /// </summary>
        public void PlaceBait(string survivorId, string nodeId, string baitType)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(nodeId))
                return;

            _activeBaits[nodeId] = new BaitNodeData
            {
                nodeId = nodeId,
                baitType = baitType,
                survivorId = survivorId,
                hoursRemaining = _state.hoursUntilSpawn,
                spawnReady = false
            };

            _state.isPlaced = true;
            OnBaitPlaced?.Invoke(survivorId, nodeId);
        }

        /// <summary>
        /// Advances the bait timer. After 24 hours the mutant pack spawns.
        /// </summary>
        public void TickHour(string nodeId, float hours)
        {
            if (string.IsNullOrEmpty(nodeId) || !_activeBaits.ContainsKey(nodeId))
                return;

            BaitNodeData data = _activeBaits[nodeId];
            if (data.spawnReady)
                return;

            data.hoursRemaining -= hours;

            if (data.hoursRemaining <= 0f)
            {
                data.spawnReady = true;
                data.hoursRemaining = 0f;
                string packType = ResolvePackType(data.baitType);
                OnMutantPackSpawned?.Invoke(nodeId, packType);
            }
        }

        /// <summary>
        /// Returns true when the bait has fully incubated and a mutant pack is present.
        /// </summary>
        public bool IsSpawnReady(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId) || !_activeBaits.ContainsKey(nodeId))
                return false;

            return _activeBaits[nodeId].spawnReady;
        }

        /// <summary>
        /// Removes the bait station from a node (after harvest or expiry).
        /// </summary>
        public void ExpireBait(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId) || !_activeBaits.ContainsKey(nodeId))
                return;

            _activeBaits.Remove(nodeId);
            OnBaitExpired?.Invoke(nodeId);

            if (_activeBaits.Count == 0)
                _state.isPlaced = false;
        }

        private static string ResolvePackType(string baitType)
        {
            if (string.IsNullOrEmpty(baitType))
                return "mutant_pack_generic";

            switch (baitType)
            {
                case "human_corpse": return "mutant_pack_feral";
                case "spoiled_meat": return "mutant_pack_scavenger";
                default: return "mutant_pack_generic";
            }
        }
    }
}
