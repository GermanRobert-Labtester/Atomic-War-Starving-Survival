using System;

namespace AtomicWar._Game.Utilities
{
    /// <summary>
    /// Data model for procedural/variable loot node definitions across world and scavenge systems.
    /// </summary>
    [Serializable]
    public class VariableLootNode
    {
        public string ItemId;
        public int MinQty;
        public int MaxQty;
        public float SpawnChance; // 0..1
        public float DegradationChance;
        public string DegradedItemId; // What it degrades into
        public string Description;
    }
}
