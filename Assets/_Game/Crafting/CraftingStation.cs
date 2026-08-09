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

        /// <summary>Restore station condition by an amount, capped at 100. Used by the
        /// CRAFT-003 rollback path when a craft cannot place its result and there is
        /// no overflow stash — wear applied at craft start is undone so the failed
        /// craft costs the player nothing.</summary>
        public void Repair(float amount)
        {
            Condition = Mathf.Min(100f, Condition + Mathf.Max(0f, amount));
        }
    }
}
