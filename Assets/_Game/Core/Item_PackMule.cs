using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class PackMuleState
    {
        public string itemId = "item_pack_mule";
        public float carryWeightMultiplier = 3.0f;
        public float speedMultiplier = 0.3f;
        public float panicChance = 0.5f;
    }

    public class Item_PackMule
    {
        public event Action<string> OnMulePanicked;
        public event Action<string, string> OnLootScattered;
        public event Action<string, float> OnCarryWeightModified;

        private PackMuleState _state;

        public Item_PackMule()
        {
            _state = new PackMuleState();
        }

        public Item_PackMule(PackMuleState state)
        {
            _state = state ?? new PackMuleState();
        }

        public PackMuleState CaptureState() => _state;

        public void RestoreState(PackMuleState state)
        {
            _state = state ?? new PackMuleState();
        }

        /// <summary>
        /// Assigns the mutated pack mule to a survivor's expedition.
        /// Triples carry weight but drastically reduces travel speed.
        /// </summary>
        /// <param name="survivorId">The survivor taking the mule on expedition.</param>
        public void UseOnExpedition(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId))
                return;

            OnCarryWeightModified?.Invoke(survivorId, _state.carryWeightMultiplier);
        }

        /// <summary>
        /// Rolls against the mule's panic chance during an ambush. If panicked,
        /// the mule bolts and scatters carried loot across 3 adjacent nodes.
        /// </summary>
        /// <param name="survivorId">The survivor who owned the mule.</param>
        /// <param name="adjacentNodeIds">Pool of nearby node IDs to scatter loot into.</param>
        /// <param name="rng">Deterministic random source.</param>
        /// <returns>List of node IDs where loot was scattered (empty if no panic).</returns>
        public List<string> CheckAmbushPanic(
            string survivorId,
            List<string> adjacentNodeIds,
            Random rng)
        {
            var scattered = new List<string>();

            if (string.IsNullOrEmpty(survivorId) || rng == null)
                return scattered;

            double roll = rng.NextDouble();
            if (roll >= _state.panicChance)
                return scattered;

            // Mule panics and bolts
            OnMulePanicked?.Invoke(survivorId);

            // Scatter loot across up to 3 adjacent nodes
            int scatterCount = 3;
            if (adjacentNodeIds != null && adjacentNodeIds.Count > 0)
            {
                int nodesToUse = Math.Min(scatterCount, adjacentNodeIds.Count);
                for (int i = 0; i < nodesToUse; i++)
                {
                    int index = rng.Next(adjacentNodeIds.Count);
                    string nodeId = adjacentNodeIds[index];
                    scattered.Add(nodeId);
                    OnLootScattered?.Invoke(survivorId, nodeId);
                }
            }

            return scattered;
        }

        /// <summary>
        /// Returns the travel-speed multiplier — the mule is extremely slow.
        /// </summary>
        public float GetSpeedMultiplier() => _state.speedMultiplier;

        public float GetCarryWeightMultiplier() => _state.carryWeightMultiplier;
    }
}
