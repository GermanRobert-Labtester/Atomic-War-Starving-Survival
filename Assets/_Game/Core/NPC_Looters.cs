using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class LootersState
    {
        public string id = "npc_looters";
        public string displayName = "Looters";
        public bool isHidingInShadows = true;
        public bool isAttacking = false;
        public float healthPercentageThreshold = 0.50f; // Below 50%
        public float encumbranceThreshold = 1.0f;       // Max encumbrance (100%)
    }

    /// <summary>
    /// Prompt #361: NPC Encounter: Looters.
    /// Opportunistic scavengers. Only attacks if player's Health is below 50% or if player is at maximum encumbrance.
    /// Otherwise, stays hidden in the shadows.
    /// </summary>
    /// <summary>DEMOTE-NPC-batch — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class NPC_Looters
    {
        private LootersState _state = new LootersState();

        public event Action<LootersState, string> OnAmbushTriggered;

        public LootersState State => _state;

        public bool EvaluateTarget(float currentHealth, float maxHealth, float encumbranceRatio)
        {
            float healthRatio = maxHealth > 0 ? currentHealth / maxHealth : 1f;
            bool targetIsWeak = healthRatio < _state.healthPercentageThreshold || encumbranceRatio >= _state.encumbranceThreshold;

            if (targetIsWeak)
            {
                _state.isHidingInShadows = false;
                _state.isAttacking = true;

                string reason = healthRatio < _state.healthPercentageThreshold ? "Low Health Target" : "Maximum Encumbrance Target";
                OnAmbushTriggered?.Invoke(_state, reason);
                return true;
            }

            _state.isHidingInShadows = true;
            _state.isAttacking = false;
            return false;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public LootersState CaptureState() => _state;

        public void RestoreState(LootersState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
