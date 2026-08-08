using System;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class OrphanageState
    {
        public string nodeId = "node_orphanage";
        public int childCount = 0;
        public bool hasBeenVisited = false;
    }

    public class Node_Orphanage
    {
        public event Action<string, int> OnChildrenRescued;
        public event Action<string> OnGuiltApplied;
        public event Action<string> OnFoodStrainWarning;

        private OrphanageState _state;

        public Node_Orphanage(OrphanageState state = null)
        {
            _state = state ?? new OrphanageState();
        }

        public string NodeId => _state.nodeId;
        public int ChildCount => _state.childCount;
        public bool HasBeenVisited => _state.hasBeenVisited;

        /// <summary>
        /// Scavenger discovers the orphanage. Returns the number of children found (random 3-8).
        /// Can only be discovered once.
        /// </summary>
        public int Discover(string scavengerId, System.Random rng)
        {
            if (string.IsNullOrEmpty(scavengerId))
            {
                Debug.LogWarning("[Node_Orphanage] Discover called with null/empty scavengerId.");
                return 0;
            }

            if (_state.hasBeenVisited)
            {
                Debug.LogWarning("[Node_Orphanage] Orphanage already visited.");
                return _state.childCount;
            }

            if (rng == null) rng = AtomicWar._Game.Utilities.SeededRandom.CreateFixed("node_orphanage");

            _state.childCount = rng.Next(3, 9); // 3 to 8 inclusive
            _state.hasBeenVisited = true;

            return _state.childCount;
        }

        /// <summary>
        /// Rescues the specified number of children from the orphanage.
        /// Adds food strain to the bunker. Raises food strain warning with estimated days of food remaining.
        /// </summary>
        public void RescueChildren(string scavengerId, int count)
        {
            if (string.IsNullOrEmpty(scavengerId))
            {
                Debug.LogWarning("[Node_Orphanage] RescueChildren called with null/empty scavengerId.");
                return;
            }

            int rescued = Mathf.Clamp(count, 0, _state.childCount);
            _state.childCount -= rescued;

            OnChildrenRescued?.Invoke(scavengerId, rescued);

            // Estimate food strain: each child consumes ~0.5 food-units/day.
            // Warning carries a rough "days of food left" estimate.
            // The actual food system should query this externally; we pass a placeholder.
            float estimatedDaysOfFood = Mathf.Max(0f, 30f - (rescued * 3f));
            OnFoodStrainWarning?.Invoke(estimatedDaysOfFood.ToString("F1"));
        }

        /// <summary>
        /// Abandons the children. Applies a permanent guilt debuff to the scavenger.
        /// </summary>
        public void AbandonChildren(string scavengerId)
        {
            if (string.IsNullOrEmpty(scavengerId))
            {
                Debug.LogWarning("[Node_Orphanage] AbandonChildren called with null/empty scavengerId.");
                return;
            }

            _state.childCount = 0;
            OnGuiltApplied?.Invoke(scavengerId);
        }

        public OrphanageState CaptureState()
        {
            return new OrphanageState
            {
                nodeId = _state.nodeId,
                childCount = _state.childCount,
                hasBeenVisited = _state.hasBeenVisited
            };
        }

        public void RestoreState(OrphanageState state)
        {
            _state = state ?? new OrphanageState();
        }
    }
}
