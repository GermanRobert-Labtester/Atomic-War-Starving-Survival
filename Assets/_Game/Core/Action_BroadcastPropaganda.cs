using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class PropagandaState
    {
        public string actionId = "action_broadcast_propaganda";
        public string targetFactionId;
        public bool isFactionRemovedFromPool = false;
        public int removalDaysRemaining = 0;
    }

    /// <summary>
    /// Prompt #415: System: Radio Propaganda.
    /// Broadcast lies about a hostile faction using a charismatic survivor and a RadioTower.
    /// Causes rival factions to eliminate them, removing them from the RNG spawn pool for 30 days.
    /// </summary>
    public class Action_BroadcastPropaganda
    {
        private PropagandaState _state = new PropagandaState();

        public event Action<PropagandaState, string, int> OnPropagandaBroadcastSuccess;
        public event Action<PropagandaState, string> OnFactionReturnedToSpawnPool;

        public PropagandaState State => _state;

        public bool BroadcastPropaganda(string targetFactionId, bool hasRadioTower, int survivorCharisma, System.Random rng)
        {
            if (!hasRadioTower || survivorCharisma < 10) return false;

            if (rng.NextDouble() < 0.70)
            {
                _state.targetFactionId = targetFactionId;
                _state.isFactionRemovedFromPool = true;
                _state.removalDaysRemaining = 30;

                OnPropagandaBroadcastSuccess?.Invoke(_state, targetFactionId, 30);
                return true;
            }
            return false;
        }

        public void TickDaily(int daysPassed = 1)
        {
            if (_state.isFactionRemovedFromPool)
            {
                _state.removalDaysRemaining -= daysPassed;
                if (_state.removalDaysRemaining <= 0)
                {
                    _state.removalDaysRemaining = 0;
                    _state.isFactionRemovedFromPool = false;
                    OnFactionReturnedToSpawnPool?.Invoke(_state, _state.targetFactionId);
                }
            }
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public PropagandaState CaptureState() => _state;

        public void RestoreState(PropagandaState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
