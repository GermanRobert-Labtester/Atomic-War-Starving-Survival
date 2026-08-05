using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class RebelVsTerrorState
    {
        public string id = "skirmish_rebel_vs_terror";
        public string locationId;
        public bool isPermanentTraderUnlocked = false;
        public string traderId = "rebel_civilian_trader";
    }

    /// <summary>
    /// Prompt #344: Skirmish: Rebels vs. Terrorists.
    /// Desperate defense of a civilian cache. Helping the Rebels defend unlocks a permanent Trader
    /// at this location for the remainder of the campaign.
    /// </summary>
    public class Skirmish_Rebel_vs_Terror
    {
        private RebelVsTerrorState _state = new RebelVsTerrorState();

        public event Action<RebelVsTerrorState, string> OnPermanentTraderUnlocked;

        public RebelVsTerrorState State => _state;

        public Skirmish_Rebel_vs_Terror(string locationId)
        {
            _state.locationId = locationId;
        }

        public bool AssistRebelDefense()
        {
            if (_state.isPermanentTraderUnlocked) return false;
            _state.isPermanentTraderUnlocked = true;

            OnPermanentTraderUnlocked?.Invoke(_state, _state.traderId);
            return true;
        }
    }
}
