using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public sealed class LandmineState
    {
        public string itemId = "item_landmine";
        public bool isDeployed;
        public string deployedNodeId = "";
        public bool isIndiscriminate = true;
    }

    public sealed class Item_Landmine
    {
        public event Action<string, string> OnMineDeployed;       // (survivorId, nodeId)
        public event Action<string, string, string> OnMineDetonated; // (nodeId, entityId, entityType)

        private LandmineState _state = new LandmineState();

        // Deploy a landmine at the given node.
        public void Deploy(string survivorId, string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId))
                throw new ArgumentNullException(nameof(nodeId));
            if (_state.isDeployed)
                throw new InvalidOperationException("Landmine is already deployed.");

            _state.isDeployed = true;
            _state.deployedNodeId = nodeId;
            OnMineDeployed?.Invoke(survivorId, nodeId);
        }

        // Check if an entity stepping onto the node triggers the mine.
        // 100% lethal to any entity type (Raider, Animal, Allied Trader, etc.).
        public bool CheckTrigger(string nodeId, string entityId, string entityType)
        {
            if (!_state.isDeployed) return false;
            if (_state.deployedNodeId != nodeId) return false;

            // Detonation: mine is consumed.
            _state.isDeployed = false;
            string triggeredNode = _state.deployedNodeId;
            _state.deployedNodeId = "";

            OnMineDetonated?.Invoke(triggeredNode, entityId, entityType);
            return true; // 100% lethal — caller must kill the entity.
        }

        public bool IsActive(string nodeId)
        {
            return _state.isDeployed && _state.deployedNodeId == nodeId;
        }

        public LandmineState CaptureState() => new LandmineState
        {
            itemId = _state.itemId,
            isDeployed = _state.isDeployed,
            deployedNodeId = _state.deployedNodeId,
            isIndiscriminate = _state.isIndiscriminate
        };

        public void RestoreState(LandmineState saved)
        {
            _state.itemId = saved.itemId;
            _state.isDeployed = saved.isDeployed;
            _state.deployedNodeId = saved.deployedNodeId;
            _state.isIndiscriminate = saved.isIndiscriminate;
        }
    }
}
