using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class TravelingCoupleState
    {
        public string id = "npc_traveling_couple";
        public string displayName = "The Traveling Couple";
        public bool isPartner1Alive = true;
        public bool isPartner2Alive = true;
        public bool isVengeanceActive = false;
        public float vengeanceMultiplier = 3.0f; // +200% accuracy and damage
    }

    /// <summary>
    /// Prompt #351: NPC Encounter: The Traveling Couple.
    /// Heavily armed, passive pair. If player attacks and kills one partner,
    /// the remaining survivor fights to the death with a +200% vengeance buff to accuracy and damage.
    /// </summary>
    /// <summary>DEMOTE-NPC-batch — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class NPC_TravelingCouple
    {
        private TravelingCoupleState _state = new TravelingCoupleState();

        public event Action<TravelingCoupleState, string> OnVengeanceActivated;

        public TravelingCoupleState State => _state;

        public void KillPartner1()
        {
            if (!_state.isPartner1Alive) return;
            _state.isPartner1Alive = false;
            TriggerVengeance("Partner 2");
        }

        public void KillPartner2()
        {
            if (!_state.isPartner2Alive) return;
            _state.isPartner2Alive = false;
            TriggerVengeance("Partner 1");
        }

        private void TriggerVengeance(string survivorName)
        {
            if (_state.isPartner1Alive || _state.isPartner2Alive)
            {
                _state.isVengeanceActive = true;
                OnVengeanceActivated?.Invoke(_state, survivorName);
            }
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public TravelingCoupleState CaptureState() => _state;

        public void RestoreState(TravelingCoupleState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
