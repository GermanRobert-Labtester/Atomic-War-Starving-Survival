using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class NPC_OsteophagesState
    {
        public string id = "faction_osteophages";
        public string displayName = "The Osteophages";
        public bool isActive = true;
        public bool isHostile = false;
        public float recyclingTrust = 50f;
    }

    /// <summary>
    /// Expansion IV — Chapter 40: Factions of the Long Dark.
    /// The Osteophages (The Rust-Eaters): Heavy metal poisoning and pica sufferers gnawing copper wire & rusted pipes.
    /// Dark recycling faction: Players can exile chelation-starved mentally broken survivors or dump toxic tech trash
    /// to receive purified item_copper_wire and item_scrap_metal via the airlock.
    /// </summary>
    public class NPC_Osteophages
    {
        private NPC_OsteophagesState _state = new NPC_OsteophagesState();

        public event Action<NPC_OsteophagesState> OnRecyclingCompleted;

        public NPC_OsteophagesState State => _state;

        public bool ProcessScrapRecycling(string trashItemId, out string resultItemId, out int resultAmount)
        {
            resultItemId = "item_scrap_metal";
            resultAmount = 1;

            if (string.Equals(trashItemId, "salvaged_tech_trash", StringComparison.OrdinalIgnoreCase))
            {
                resultItemId = "item_copper_wire";
                resultAmount = 3;
                return true;
            }
            else if (string.Equals(trashItemId, "body_armour_deprecated", StringComparison.OrdinalIgnoreCase))
            {
                resultItemId = "item_scrap_metal";
                resultAmount = 5;
                return true;
            }

            return false;
        }

        public NPC_OsteophagesState CaptureState() => _state;
        public void RestoreState(NPC_OsteophagesState saved) { _state = saved ?? new NPC_OsteophagesState(); }
    }
}
