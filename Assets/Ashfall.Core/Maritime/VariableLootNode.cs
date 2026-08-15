using System;

namespace Ashfall.Core.Maritime
{
    /// <summary>
    /// ASHFALL: THE BLACK FLOTILLA (Expansion 09) — data model for procedural
    /// loot node definitions. Used by ProceduralScavengeSystem and the
    /// DeepLoreLocationCatalog. Engine-agnostic, serializable.
    /// </summary>
    [Serializable]
    public class VariableLootNode
    {
        public string ItemId = string.Empty;
        public int MinQty;
        public int MaxQty;
        public float SpawnChance;
        public float DegradationChance;
        public string DegradedItemId = string.Empty;
        public string Description = string.Empty;
    }
}
