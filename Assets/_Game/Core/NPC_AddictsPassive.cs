using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class AddictsPassiveState
    {
        public string id = "npc_addicts_passive";
        public string displayName = "Passive Addicts";
        public bool inSevereWithdrawal = true;
        public List<string> highValueTradeGoods = new List<string> { "assault_rifle_pristine", "classified_sector_map" };
    }

    /// <summary>
    /// Prompt #350: NPC Encounter: Passive Addicts.
    /// Found in Ruined Pharmacies. In severe withdrawal. Will trade top-tier goods (Guns, Maps) for Morphine.
    /// </summary>
    public class NPC_AddictsPassive
    {
        private AddictsPassiveState _state = new AddictsPassiveState();

        public event Action<AddictsPassiveState, string> OnHighValueChemTraded;

        public AddictsPassiveState State => _state;

        public string TradeMorphineForHighValueItem(ref int playerMorphineCount)
        {
            if (playerMorphineCount >= 1 && _state.highValueTradeGoods.Count > 0)
            {
                playerMorphineCount--;
                string item = _state.highValueTradeGoods[0];
                _state.highValueTradeGoods.RemoveAt(0);

                OnHighValueChemTraded?.Invoke(_state, item);
                return item;
            }
            return null;
        }
    }
}
