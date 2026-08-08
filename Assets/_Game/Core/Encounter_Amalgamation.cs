using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class AmalgamationState
    {
        public string id = "encounter_amalgamation";
        public string displayName = "The Amalgamation";
        public float healthPool = 500f;
        public float fireVulnerabilityMultiplier = 3.0f;
        public float firearmDamageReduction = 0.95f;
        public List<string> lootDropIds = new List<string>();
    }

    /// <summary>
    /// Prompt #550: Encounter: The Amalgamation (Biomass).
    /// Found deep in Sewers. A mass of fused feral dogs and humans.
    /// Cannot be killed with firearms (bullets absorb). Must use Fire (Molotovs, Flares) or avoid.
    /// </summary>
    /// <summary>DEMOTE-Encounter-batch — dormant ghost; SO expedition encounters remain live. Re-promote with Boot+Save+host.</summary>
    public class Encounter_Amalgamation
    {
        private AmalgamationState _state = new AmalgamationState();

        public event Action<AmalgamationState> OnAmalgamationEngaged;
        public event Action<AmalgamationState> OnAmalgamationDefeated;
        public event Action<AmalgamationState, bool> OnFleeAttempt;

        public AmalgamationState State => _state;

        public float ApplyDamage(float baseDamage, string damageType, System.Random rng)
        {
            float actualDamage = baseDamage;

            if (damageType == "firearm")
            {
                actualDamage *= (1f - _state.firearmDamageReduction);
            }
            else if (damageType == "fire")
            {
                actualDamage *= _state.fireVulnerabilityMultiplier;
            }

            _state.healthPool = Mathf.Max(0f, _state.healthPool - actualDamage);
            OnAmalgamationEngaged?.Invoke(_state);

            if (IsDefeated())
            {
                OnAmalgamationDefeated?.Invoke(_state);
            }

            return actualDamage;
        }

        public bool IsDefeated()
        {
            return _state.healthPool <= 0f;
        }

        public bool TryFlee(System.Random rng, float staminaPercent)
        {
            float fleeChance = 0.60f + (staminaPercent * 0.40f);
            double roll = rng.NextDouble();
            bool success = roll < fleeChance;
            OnFleeAttempt?.Invoke(_state, success);
            return success;
        }

        public AmalgamationState CaptureState()
        {
            return new AmalgamationState
            {
                id = _state.id,
                displayName = _state.displayName,
                healthPool = _state.healthPool,
                fireVulnerabilityMultiplier = _state.fireVulnerabilityMultiplier,
                firearmDamageReduction = _state.firearmDamageReduction,
                lootDropIds = _state.lootDropIds != null
                    ? new List<string>(_state.lootDropIds)
                    : new List<string>()
            };
        }

        public void RestoreState(AmalgamationState saved)
        {
            _state = saved ?? new AmalgamationState();
            if (_state.lootDropIds == null)
                _state.lootDropIds = new List<string>();
        }
    }
}
