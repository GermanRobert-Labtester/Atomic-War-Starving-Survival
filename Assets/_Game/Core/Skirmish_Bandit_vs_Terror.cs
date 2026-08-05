using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class BanditVsTerrorState
    {
        public string id = "skirmish_bandit_vs_terror";
        public string locationId;
        public bool isBanditRescued = false;
        public bool isBanditSlaughtered = false;
        public string hiddenStashLocationId = "subway_depot";
    }

    /// <summary>
    /// Prompt #342: Skirmish: Bandits vs. Terrorists.
    /// Bandits are outgunned and fleeing from terrorists. Helping them escape rewards a hidden stash location;
    /// ignoring them lets the Terrorists slaughter them.
    /// </summary>
    public class Skirmish_Bandit_vs_Terror
    {
        private BanditVsTerrorState _state = new BanditVsTerrorState();

        public event Action<BanditVsTerrorState, string> OnBanditsRescued;
        public event Action<BanditVsTerrorState> OnBanditsSlaughtered;

        public BanditVsTerrorState State => _state;

        public Skirmish_Bandit_vs_Terror(string locationId)
        {
            _state.locationId = locationId;
        }

        public string RescueBandits()
        {
            if (_state.isBanditRescued || _state.isBanditSlaughtered) return null;
            _state.isBanditRescued = true;

            OnBanditsRescued?.Invoke(_state, _state.hiddenStashLocationId);
            return _state.hiddenStashLocationId;
        }

        public void LetTerroristsSlaughter()
        {
            if (_state.isBanditRescued || _state.isBanditSlaughtered) return;
            _state.isBanditSlaughtered = true;

            OnBanditsSlaughtered?.Invoke(_state);
        }
    }
}
