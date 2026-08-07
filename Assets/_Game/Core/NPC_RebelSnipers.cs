using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class RebelSnipersState
    {
        public string id = "npc_rebel_snipers";
        public string displayName = "4x Rebel Sniper Squad";
        public int squadSize = 4;
        public int currentCoverStage = 0;
        public int requiredCoverStages = 3;
        public bool isMeleeRangeReached = false;
        public float headshotDamage = 65f;
    }

    /// <summary>
    /// Prompt #328: NPC Encounter: The 4x Rebel Sniper Squad.
    /// Spawns strictly as a group of 4. Player cannot engage in melee directly; must solve a cover puzzle
    /// using Smoke Grenades or Agility checks. Failure deals massive headshot damage.
    /// </summary>
    public class NPC_RebelSnipers
    {
        private RebelSnipersState _state = new RebelSnipersState();

        public event Action<RebelSnipersState, int, bool> OnCoverAdvanced;
        public event Action<RebelSnipersState, float> OnHeadshotDealt;
        public event Action<RebelSnipersState> OnMeleeRangeReached;

        public RebelSnipersState State => _state;

        public bool TryAdvanceCover(bool hasSmokeGrenade, int playerAgility, out float damageTaken)
        {
            damageTaken = 0f;
            bool success = hasSmokeGrenade || playerAgility >= 12;

            if (success)
            {
                _state.currentCoverStage++;
                OnCoverAdvanced?.Invoke(_state, _state.currentCoverStage, true);

                if (_state.currentCoverStage >= _state.requiredCoverStages)
                {
                    _state.isMeleeRangeReached = true;
                    OnMeleeRangeReached?.Invoke(_state);
                }
                return true;
            }
            else
            {
                damageTaken = _state.headshotDamage;
                OnHeadshotDealt?.Invoke(_state, damageTaken);
                OnCoverAdvanced?.Invoke(_state, _state.currentCoverStage, false);
                return false;
            }
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public RebelSnipersState CaptureState() => _state;

        public void RestoreState(RebelSnipersState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
