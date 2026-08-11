using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class MilitaryPatrolState
    {
        public string id = "npc_military_patrol";
        public string displayName = "Standard Military Patrol";
        public bool isHostileToPlayer;
        public bool tollDemanded;
        public bool tollPaid;
        public int foodTollRequired = 2;
        public int medsTollRequired = 1;
        public float armorRating = 75f;
        public bool suppressingFireActive = true;
    }

    /// <summary>
    /// Prompt #322: NPC Encounter: Standard Military Patrol.
    /// Military unit that demands food/meds toll for scavenging access; turns hostile if refused.
    /// </summary>
    /// <summary>DEMOTE-NPC-batch — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class NPC_MilitaryPatrol
    {
        private MilitaryPatrolState _state = new MilitaryPatrolState();

        public event Action<MilitaryPatrolState> OnTollDemanded;
        public event Action<MilitaryPatrolState> OnTollPaid;
        public event Action<MilitaryPatrolState> OnPatrolTurnedHostile;

        public MilitaryPatrolState State => _state;

        public void InitiateEncounter(bool playerIsArmed = false)
        {
            _state.tollDemanded = true;
            _state.tollPaid = false;
            // Passive to player if unarmed/compliant, but demands toll
            _state.isHostileToPlayer = playerIsArmed; 
            OnTollDemanded?.Invoke(_state);
        }

        public bool PayToll(ref int playerFoodCount, ref int playerMedsCount)
        {
            if (playerFoodCount >= _state.foodTollRequired || playerMedsCount >= _state.medsTollRequired)
            {
                if (playerFoodCount >= _state.foodTollRequired)
                    playerFoodCount -= _state.foodTollRequired;
                else
                    playerMedsCount -= _state.medsTollRequired;

                _state.tollPaid = true;
                _state.isHostileToPlayer = false;
                OnTollPaid?.Invoke(_state);
                return true;
            }
            return false;
        }

        public void RefuseToll()
        {
            _state.isHostileToPlayer = true;
            _state.suppressingFireActive = true;
            OnPatrolTurnedHostile?.Invoke(_state);
        }

        public float CalculateDamageTaken(float incomingDamage)
        {
            float effectiveArmor = _state.suppressingFireActive ? _state.armorRating * 1.25f : _state.armorRating;
            float damageReduction = effectiveArmor / (effectiveArmor + 50f);
            return Mathf.Max(1f, incomingDamage * (1f - damageReduction));
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public MilitaryPatrolState CaptureState() => _state;

        public void RestoreState(MilitaryPatrolState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
