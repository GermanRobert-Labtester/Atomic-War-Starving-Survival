using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class PresidentialSealState
    {
        public string itemId = "item_presidential_seal";
        public string displayName = "Presidential Seal Plaque";
        public float negotiationSuccessBonusRatio = 0.10f; // +10% negotiation bonus
        public bool isEquipped = false;
    }

    /// <summary>
    /// Prompt #467: Artifact: Presidential Seal Plaque.
    /// Found in the Government Bunker node. Equipping it grants a Leadership aura,
    /// giving a +10% bonus to negotiation success with traders and warlords.
    /// </summary>
    public class Item_PresidentialSeal
    {
        private PresidentialSealState _state = new PresidentialSealState();

        public event Action<PresidentialSealState, float> OnLeadershipAuraActivated;

        public PresidentialSealState State => _state;

        public float EquipPlaque()
        {
            _state.isEquipped = true;
            OnLeadershipAuraActivated?.Invoke(_state, _state.negotiationSuccessBonusRatio);
            return _state.negotiationSuccessBonusRatio;
        }
    }
}
