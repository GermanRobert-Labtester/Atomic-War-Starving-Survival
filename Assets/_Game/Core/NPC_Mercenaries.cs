using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class MercenariesState
    {
        public string id = "npc_mercenaries";
        public string displayName = "The Mercenaries (Ex-PMCs)";
        public int hireCostPreWarMoney = 150;
        public bool isHiredToClear = false;
        public float lootPenaltyRatio = 0.50f; // Sacrifices 50% loot
    }

    /// <summary>
    /// Prompt #364: NPC Encounter: The Mercenaries.
    /// Ex-PMCs. Can be hired (PreWarMoney/Gold) to clear the current map node for the player,
    /// suffering 0 damage but sacrificing 50% of the loot.
    /// </summary>
    /// <summary>DEMOTE-NPC-batch — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class NPC_Mercenaries
    {
        private MercenariesState _state = new MercenariesState();

        public event Action<MercenariesState, string> OnNodeClearedByMercenaries;

        public MercenariesState State => _state;

        public bool TryHireToClearNode(ref int playerPreWarMoney, string locationId, out float lootModifier)
        {
            lootModifier = 1.0f;
            if (playerPreWarMoney >= _state.hireCostPreWarMoney && !_state.isHiredToClear)
            {
                playerPreWarMoney -= _state.hireCostPreWarMoney;
                _state.isHiredToClear = true;
                lootModifier = 1.0f - _state.lootPenaltyRatio; // 0.50

                OnNodeClearedByMercenaries?.Invoke(_state, locationId);
                return true;
            }
            return false;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public MercenariesState CaptureState() => _state;

        public void RestoreState(MercenariesState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
