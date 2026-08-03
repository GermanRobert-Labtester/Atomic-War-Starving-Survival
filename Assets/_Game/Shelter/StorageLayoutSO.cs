using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// Data-driven storage layout: defines the slots within a room, their spatial
    /// positions, and adjacency relationships for cross-contamination spread.
    /// Authored as a .asset on a shelter room or the shelter itself.
    /// </summary>
    [CreateAssetMenu(fileName = "NewStorageLayout", menuName = "ASHFALL/Shelter/Storage Layout")]
    public class StorageLayoutSO : ScriptableObject
    {
        [Header("Layout Identification")]
        public string layoutId;
        public string displayName;

        [Header("Cross-Contamination Parameters")]
        [Tooltip("Base contamination transfer rate per hour between adjacent slots (0..1)")]
        [Range(0f, 0.1f)]
        public float contaminationTransferRate = 0.005f;
        
        [Tooltip("Distance at which transfer rate halves (Manhattan units). 0 = no falloff.")]
        public float halfFalloffDistance = 2f;

        [Header("Slot Definitions")]
        [Tooltip("Slot grid positions and adjacency pairs")]
        public List<SlotDefinition> slots = new List<SlotDefinition>();

        [Header("Adjacency (slot index pairs)")]
        [Tooltip("Explicit adjacency pairs. If empty, adjacency is auto-computed by proximity.")]
        public List<AdjacencyPair> adjacencies = new List<AdjacencyPair>();

        [Serializable]
        public class SlotDefinition
        {
            public string slotId;
            public Vector2Int position;
            [Tooltip("Category restriction: 0 = any, 1 = food/water only, 2 = gear only")]
            public int categoryRestriction = 0;
        }

        [Serializable]
        public class AdjacencyPair
        {
            public int slotA;
            public int slotB;
        }

        /// <summary>
        /// Build the runtime StorageSlot list from this layout data, with adjacency
        /// resolved either from explicit pairs or proximity fallback.
        /// </summary>
        public List<StorageSlot> BuildSlots()
        {
            var result = new List<StorageSlot>(slots.Count);
            for (int i = 0; i < slots.Count; i++)
            {
                var def = slots[i];
                result.Add(new StorageSlot(def.slotId, def.position));
            }

            if (adjacencies != null && adjacencies.Count > 0)
            {
                foreach (var pair in adjacencies)
                {
                    if (pair.slotA >= 0 && pair.slotA < result.Count &&
                        pair.slotB >= 0 && pair.slotB < result.Count)
                    {
                        if (!result[pair.slotA].AdjacentSlotIndices.Contains(pair.slotB))
                            result[pair.slotA].AdjacentSlotIndices.Add(pair.slotB);
                        if (!result[pair.slotB].AdjacentSlotIndices.Contains(pair.slotA))
                            result[pair.slotB].AdjacentSlotIndices.Add(pair.slotA);
                    }
                }
            }
            else
            {
                // Auto-adjacency: any slot within Manhattan distance <= 2 is adjacent
                for (int i = 0; i < result.Count; i++)
                {
                    for (int j = i + 1; j < result.Count; j++)
                    {
                        int dist = Mathf.Abs(result[i].Position.x - result[j].Position.x) +
                                   Mathf.Abs(result[i].Position.y - result[j].Position.y);
                        if (dist <= 2)
                        {
                            result[i].AdjacentSlotIndices.Add(j);
                            result[j].AdjacentSlotIndices.Add(i);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Compute the falloff factor for cross-contamination at a given distance.
        /// Returns 1 at distance 0, 0.5 at halfFalloffDistance, approaches 0 further away.
        /// </summary>
        public float FalloffAtDistance(float distance)
        {
            if (halfFalloffDistance <= 0f || distance <= 0f) return 1f;
            return Mathf.Pow(0.5f, distance / halfFalloffDistance);
        }
    }
}
