using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class AggroScavengersState
    {
        public string id = "npc_aggro_scavengers";
        public string displayName = "Aggressive Scavengers";
        public bool warningShotFired = false;
        public bool isHostile = false;
        public float armorRating = 15f; // Low armor
        public List<string> weaponsEquipped = new List<string> { "nail_gun", "crowbar" };
    }

    /// <summary>
    /// Prompt #359: NPC Encounter: Aggressive Scavengers.
    /// "Finders Keepers." Fires warning shots upon entering node; attacks if player advances.
    /// Equipped with low armor, NailGuns, and Crowbars.
    /// </summary>
    public class NPC_AggroScavengers
    {
        private AggroScavengersState _state = new AggroScavengersState();

        public event Action<AggroScavengersState> OnWarningShotFired;
        public event Action<AggroScavengersState> OnPlayerAdvancedHostile;

        public AggroScavengersState State => _state;

        public void TriggerNodeEntry()
        {
            if (!_state.warningShotFired)
            {
                _state.warningShotFired = true;
                OnWarningShotFired?.Invoke(_state);
            }
        }

        public void AdvanceIntoNode()
        {
            _state.isHostile = true;
            OnPlayerAdvancedHostile?.Invoke(_state);
        }
    }
}
