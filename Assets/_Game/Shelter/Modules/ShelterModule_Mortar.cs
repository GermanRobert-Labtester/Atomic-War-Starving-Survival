using System;
using System.Collections.Generic;
using System.Linq;

namespace AtomicWar._Game.Shelter.Modules
{
    [Serializable]
    public class MortarState
    {
        public string moduleId = "shelter_module_mortar";
        public bool isBuilt = false;
        public bool destroysAllLoot = true;
        public int range = 1;
        public string surfaceLocation;
    }

    public class ShelterModule_Mortar
    {
        public event Action<string, string> OnNodeBombarded;
        public event Action<string> OnLootVaporized;

        private MortarState _state;

        public ShelterModule_Mortar()
        {
            _state = new MortarState();
        }

        public ShelterModule_Mortar(MortarState state)
        {
            _state = state ?? new MortarState();
        }

        public MortarState CaptureState() => _state;

        public void RestoreState(MortarState state)
        {
            _state = state ?? new MortarState();
        }

        /// <summary>
        /// Constructs mortar tubes at a surface location.
        /// Mortars can only be built above ground, not inside the bunker.
        /// </summary>
        public void Build(string surfaceLocationId)
        {
            if (string.IsNullOrEmpty(surfaceLocationId))
                return;

            _state.isBuilt = true;
            _state.surfaceLocation = surfaceLocationId;
        }

        /// <summary>
        /// Bombards an adjacent node with mortar fire.
        /// Destroys all enemies but vaporizes all loot and artifacts on the node.
        /// Pure defense tool — no salvage possible on the target.
        /// </summary>
        /// <param name="operatorId">Survivor operating the mortar.</param>
        /// <param name="targetNodeId">The map node to bombard.</param>
        /// <param name="isAdjacent">Whether the target is within mortar range.</param>
        /// <returns>True if the bombardment was executed.</returns>
        public bool Bombard(string operatorId, string targetNodeId, bool isAdjacent)
        {
            if (string.IsNullOrEmpty(operatorId) || string.IsNullOrEmpty(targetNodeId))
                return false;

            if (!_state.isBuilt || !isAdjacent)
                return false;

            // Destroy all enemies on the node
            OnNodeBombarded?.Invoke(operatorId, targetNodeId);

            // All loot/artifacts are vaporized
            if (_state.destroysAllLoot)
            {
                OnLootVaporized?.Invoke(targetNodeId);
            }

            return true;
        }

        /// <summary>
        /// Checks whether the target node is within mortar range (must be adjacent).
        /// </summary>
        /// <param name="targetNodeId">Candidate target node.</param>
        /// <param name="bunkerNodeId">The bunker's own node position.</param>
        /// <param name="adjacentNodeIds">Set of nodes adjacent to the bunker.</param>
        public bool CanTarget(
            string targetNodeId,
            string bunkerNodeId,
            IReadOnlyCollection<string> adjacentNodeIds)
        {
            if (string.IsNullOrEmpty(targetNodeId) || string.IsNullOrEmpty(bunkerNodeId))
                return false;

            if (!_state.isBuilt)
                return false;

            // Mortar range is exactly 1 hop — target must be adjacent
            if (adjacentNodeIds == null)
                return false;

            return adjacentNodeIds.Contains(targetNodeId);
        }

        public bool IsBuilt => _state.isBuilt;

        public int Range => _state.range;
    }
}
