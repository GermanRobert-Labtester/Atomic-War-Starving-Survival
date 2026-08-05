using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class PsychopathPairState
    {
        public string id = "npc_psychopath_pair";
        public string displayName = "The Psychopath Pair";
        public bool isSniperAlive = true;
        public bool isMeleeAlive = true;
        public bool isFrenzyActive = false;
        public float damageMultiplier = 1.0f;
        public bool isImmuneToPain = false;
    }

    /// <summary>
    /// Prompt #338: NPC Encounter: The Psychopath Pair (Hunters).
    /// Hunters pair (one Sniper, one Melee). Killing one causes the remaining survivor to enter
    /// Frenzy mode (2x damage, immune to pain).
    /// </summary>
    public class NPC_PsychopathPair
    {
        private PsychopathPairState _state = new PsychopathPairState();

        public event Action<PsychopathPairState, string> OnOneKilledFrenzyEntered;

        public PsychopathPairState State => _state;

        public void KillSniper()
        {
            if (!_state.isSniperAlive) return;
            _state.isSniperAlive = false;
            TriggerFrenzy("Melee Hunter");
        }

        public void KillMelee()
        {
            if (!_state.isMeleeAlive) return;
            _state.isMeleeAlive = false;
            TriggerFrenzy("Sniper Hunter");
        }

        private void TriggerFrenzy(string remainingRole)
        {
            if (_state.isSniperAlive || _state.isMeleeAlive)
            {
                _state.isFrenzyActive = true;
                _state.damageMultiplier = 2.0f;
                _state.isImmuneToPain = true;
                OnOneKilledFrenzyEntered?.Invoke(_state, remainingRole);
            }
        }
    }
}
