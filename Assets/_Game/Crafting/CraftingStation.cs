using UnityEngine;

namespace AtomicWar._Game.Crafting
{
    /// <summary>
    /// A craft station (workbench, water purifier, chemistry set) that gates which
    /// recipes can run and wears down with use. Save/load safe.
    /// </summary>
    [System.Serializable]
    public class CraftingStation
    {
        public string id;
        public string displayName;
        public float Condition = 100f;

        public bool IsOperational => Condition > 0f;

        /// <summary>Reduce station condition by an amount (wear from crafting), floored at 0.</summary>
        public void Degrade(float amount)
        {
            Condition = Mathf.Max(0f, Condition - Mathf.Max(0f, amount));
        }
    }
}
