using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class BanditsState
    {
        public string id = "npc_bandits";
        public string displayName = "The Bandits";
        public bool isHostile = false;
        public float demandedInventoryRatio = 0.50f; // Exactly 50%
        public bool extortionPaid = false;
    }

    /// <summary>
    /// Prompt #339: NPC Encounter: The Bandits.
    /// Standard highwaymen demanding exactly 50% of scavenger's inventory value.
    /// If paid, they depart peacefully without combat.
    /// </summary>
    public class NPC_Bandits
    {
        private BanditsState _state = new BanditsState();

        public event Action<BanditsState, float> OnExtortionDemanded;
        public event Action<BanditsState> OnExtortionPaid;
        public event Action<BanditsState> OnExtortionRefusedHostile;

        public BanditsState State => _state;

        public void InitiateExtortion(float totalInventoryValue)
        {
            float demand = totalInventoryValue * _state.demandedInventoryRatio;
            _state.extortionPaid = false;
            _state.isHostile = false;

            OnExtortionDemanded?.Invoke(_state, demand);
        }

        public bool PayExtortion()
        {
            _state.extortionPaid = true;
            _state.isHostile = false;

            OnExtortionPaid?.Invoke(_state);
            return true;
        }

        public void RefuseExtortion()
        {
            _state.isHostile = true;
            OnExtortionRefusedHostile?.Invoke(_state);
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public BanditsState CaptureState() => _state;

        public void RestoreState(BanditsState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
