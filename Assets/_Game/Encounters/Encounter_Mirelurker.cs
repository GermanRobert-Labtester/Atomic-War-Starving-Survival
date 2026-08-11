using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Encounters
{
    [Serializable]
    public class MirelurkerState
    {
        public string id = "encounter_mirelurker";
        public string displayName = "The Mirelurker";
        public float armorRating = 0.95f;
        public float healthPool = 800f;
        public bool requiresExplosivesOrAP = true;
        public bool dragUnderwaterInstantDeath = true;
    }

    /// <summary>
    /// Prompt #577: Encounter: The Mirelurker (Mutant Amphibian).
    /// Apex predator in the swamps.
    /// Massive armor plating, immune to small arms fire.
    /// If player does not have Explosives or ArmorPiercing ammo, must flee or be dragged underwater (instant death).
    /// </summary>
    /// <summary>DEMOTE-Encounter-batch — dormant ghost; SO expedition encounters remain live. Re-promote with Boot+Save+host.</summary>
    public class Encounter_Mirelurker
    {
        private MirelurkerState _state = new MirelurkerState();

        public event Action<MirelurkerState> OnMirelurkerEngaged;
        public event Action<MirelurkerState> OnMirelurkerDefeated;
        public event Action<MirelurkerState> OnDraggedUnderwater;
        public event Action<MirelurkerState, bool> OnFleeAttempt;

        public MirelurkerState State => _state;

        public float ApplyDamage(float baseDamage, string ammoType)
        {
            float actualDamage = baseDamage;

            if (ammoType == "small_arms")
            {
                actualDamage *= (1f - _state.armorRating);
            }
            else if (ammoType == "explosive" || ammoType == "armor_piercing")
            {
                // Full damage
                actualDamage = baseDamage;
            }

            _state.healthPool = Mathf.Max(0f, _state.healthPool - actualDamage);
            OnMirelurkerEngaged?.Invoke(_state);

            if (IsDefeated())
            {
                OnMirelurkerDefeated?.Invoke(_state);
            }

            return actualDamage;
        }

        public bool CanEngage(bool hasExplosives, bool hasArmorPiercingAmmo)
        {
            return hasExplosives || hasArmorPiercingAmmo;
        }

        public bool TryFlee(System.Random rng, float agility)
        {
            float fleeChance = 0.40f + (agility * 0.60f);
            double roll = rng.NextDouble();
            bool success = roll < fleeChance;

            OnFleeAttempt?.Invoke(_state, success);

            if (!success && _state.dragUnderwaterInstantDeath)
            {
                OnDraggedUnderwater?.Invoke(_state);
            }

            return success;
        }

        public bool IsDefeated()
        {
            return _state.healthPool <= 0f;
        }

        public MirelurkerState CaptureState() => _state;

        public void RestoreState(MirelurkerState saved)
        {
            _state = saved ?? new MirelurkerState();
        }
    }
}
