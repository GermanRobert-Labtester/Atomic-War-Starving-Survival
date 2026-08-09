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

        /// <summary>Food units one rescued child consumes per day.</summary>
        public const float FoodUnitsPerChildPerDay = 0.5f;

        /// <summary>Food units one existing bunker occupant consumes per day.</summary>
        public const float FoodUnitsPerOccupantPerDay = 1f;

        /// <summary>
        /// Fallback days-of-food used when no real supply provider is wired. Kept so
        /// callers that never call <see cref="SetFoodSupplyProvider"/> behave exactly
        /// as they did before the food strain estimate was made real.
        /// </summary>
        public const float AssumedDaysOfFoodWhenUnwired = 30f;

        private OrphanageState _state;
        private Func<float> _getFoodUnits;
        private Func<int> _getOccupantCount;

        public Node_Orphanage(OrphanageState state = null)
        {
            _state = state ?? new OrphanageState();
        }

        /// <summary>
        /// MAP-55B: wire the real bunker food stores so the strain warning reflects
        /// actual supply. Previously <see cref="RescueChildren"/> derived "days of
        /// food left" from a hardcoded 30-day constant, so the warning was the same
        /// whether the pantry was full or nearly empty — exactly the decision the
        /// warning exists to inform.
        /// </summary>
        public void SetFoodSupplyProvider(Func<float> getFoodUnits, Func<int> getOccupantCount)
        {
            _getFoodUnits = getFoodUnits;
            _getOccupantCount = getOccupantCount;
        }

        /// <summary>
        /// Days of food remaining once <paramref name="rescuedChildren"/> extra mouths
        /// are added. Returns the legacy estimate when no supply provider is wired.
        /// </summary>
        internal float EstimateDaysOfFood(int rescuedChildren)
        {
            if (_getFoodUnits == null)
                return Mathf.Max(0f, AssumedDaysOfFoodWhenUnwired - (rescuedChildren * 3f));

            float foodUnits = Mathf.Max(0f, _getFoodUnits());
            int occupants = _getOccupantCount != null ? Mathf.Max(0, _getOccupantCount()) : 0;
            float dailyBurn = (occupants * FoodUnitsPerOccupantPerDay)
                              + (rescuedChildren * FoodUnitsPerChildPerDay);

            // No mouths to feed means the stores never run down — report them as
            // effectively indefinite rather than dividing by zero.
            if (dailyBurn <= 0f) return float.PositiveInfinity;
            return foodUnits / dailyBurn;
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

            if (rng == null) rng = AtomicWar._Game.Utilities.SeededRandom.Stream("node_orphanage");

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

            // Food strain: days of food left once the rescued children are eating too.
            // Uses the real pantry when a supply provider is wired (see
            // SetFoodSupplyProvider), otherwise the documented legacy estimate.
            float estimatedDaysOfFood = EstimateDaysOfFood(rescued);
            OnFoodStrainWarning?.Invoke(
                float.IsPositiveInfinity(estimatedDaysOfFood)
                    ? "∞"
                    : estimatedDaysOfFood.ToString("F1"));
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
