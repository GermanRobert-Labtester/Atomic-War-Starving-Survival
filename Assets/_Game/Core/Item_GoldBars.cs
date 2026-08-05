using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class GoldBarsState
    {
        public string itemId = "item_gold_bars";
        public string displayName = "Stashed Gold Bars";
        public int scavengerTradeValue = 0; // Useless to ordinary scavengers
        public bool satisfiesEndgameFactionTribute = true;
    }

    /// <summary>
    /// Prompt #460: Artifact: Stashed Gold Bars.
    /// Pre-war wealth useless to starving scavengers (0 trade value),
    /// but endgame dominant Factions accept it as 100% full payment for tribute or ransom.
    /// </summary>
    public class Item_GoldBars
    {
        private GoldBarsState _state = new GoldBarsState();

        public event Action<GoldBarsState, string> OnGoldBarsUsedForTribute;

        public GoldBarsState State => _state;

        public bool PayFactionTributeWithGold(string factionId)
        {
            OnGoldBarsUsedForTribute?.Invoke(_state, factionId);
            return _state.satisfiesEndgameFactionTribute;
        }
    }
}
