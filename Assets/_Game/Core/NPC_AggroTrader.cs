using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class AggroTraderState
    {
        public string id = "npc_aggro_trader";
        public string displayName = "Aggressive Traders (Extortionists)";
        public bool isCorneredInDeadEnd = true;
        public string forcedJunkItem = "useless_broken_watch";
        public int forcedPricePremium = 100; // 100 scrap/money
        public bool purchaseMade = false;
        public bool isHostile = false;
    }

    /// <summary>
    /// Prompt #363: NPC Encounter: Aggressive Traders (Extortionists).
    /// Corners player in a dead-end room ("Buy something or die"). Forced to purchase a useless junk item
    /// at a massive premium to avoid a lethal firefight.
    /// </summary>
    /// <summary>DEMOTE-NPC-batch — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class NPC_AggroTrader
    {
        private AggroTraderState _state = new AggroTraderState();

        public event Action<AggroTraderState, string, int> OnForcedPurchaseMade;
        public event Action<AggroTraderState> OnRefusedPurchaseHostile;

        public AggroTraderState State => _state;

        public string ComplyAndBuyJunk(ref int playerScrapCount)
        {
            if (playerScrapCount >= _state.forcedPricePremium && !_state.purchaseMade)
            {
                playerScrapCount -= _state.forcedPricePremium;
                _state.purchaseMade = true;
                _state.isHostile = false;

                OnForcedPurchaseMade?.Invoke(_state, _state.forcedJunkItem, _state.forcedPricePremium);
                return _state.forcedJunkItem;
            }
            RefusePurchase();
            return null;
        }

        public void RefusePurchase()
        {
            _state.isHostile = true;
            OnRefusedPurchaseHostile?.Invoke(_state);
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public AggroTraderState CaptureState() => _state;

        public void RestoreState(AggroTraderState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
