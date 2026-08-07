using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class DrunksAggroState
    {
        public string id = "npc_drunks_aggro";
        public string displayName = "Aggressive Drunks";
        public float hp = 100f;
        public bool hasHighPainTolerance = true;
        public float damageReductionFactor = 0.50f;
        public float karmaLossOnDeath = 15f;
        public string lootDrop = "moonshine_bottle";
    }

    /// <summary>
    /// Prompt #349: NPC Encounter: Aggressive Drunks.
    /// Found in Bars/Gas Stations. Unpredictable melee attackers with high pain tolerance (50% damage reduction).
    /// Killing yields Moonshine but damages Karma (-15).
    /// </summary>
    public class NPC_DrunksAggro
    {
        private DrunksAggroState _state = new DrunksAggroState();

        public event Action<DrunksAggroState, float, string> OnKilled;

        public DrunksAggroState State => _state;

        public float TakeDamage(float incomingDamage)
        {
            float actualDamage = _state.hasHighPainTolerance ? incomingDamage * (1f - _state.damageReductionFactor) : incomingDamage;
            _state.hp -= actualDamage;

            if (_state.hp <= 0f)
            {
                _state.hp = 0f;
                OnKilled?.Invoke(_state, _state.karmaLossOnDeath, _state.lootDrop);
            }
            return actualDamage;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public DrunksAggroState CaptureState() => _state;

        public void RestoreState(DrunksAggroState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
