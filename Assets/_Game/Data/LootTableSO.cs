using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Data
{
    /// <summary>One weighted loot entry, gated to a minimum world phase.</summary>
    [Serializable]
    public class LootEntry
    {
        public ItemDefinition item;
        public float weight = 1f;

        /// <summary>Entry is eligible once the campaign has reached this phase or later.</summary>
        public WorldPhase phaseRequirement = WorldPhase.CivilWar;
    }

    /// <summary>
    /// Designer-authored, phase-gated loot table. LocationScavengingSystem rolls
    /// against the entries valid for the current WorldPhase.
    /// </summary>
    [CreateAssetMenu(fileName = "NewLootTable", menuName = "ASHFALL/Data/Loot Table")]
    public class LootTableSO : ScriptableObject
    {
        public List<LootEntry> entries = new List<LootEntry>();

        /// <summary>Entries whose phaseRequirement has been reached by currentPhase.</summary>
        public List<LootEntry> GetValidEntries(WorldPhase currentPhase)
        {
            var valid = new List<LootEntry>();
            if (entries == null) return valid;

            for (int i = 0; i < entries.Count; i++)
            {
                var entry = entries[i];
                if (entry != null && entry.item != null && currentPhase >= entry.phaseRequirement)
                {
                    valid.Add(entry);
                }
            }
            return valid;
        }
    }
}
