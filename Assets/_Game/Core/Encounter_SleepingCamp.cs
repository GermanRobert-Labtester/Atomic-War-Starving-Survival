using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class SleepingCampState
    {
        public string id = "encounter_sleeping_camp";
        public string displayName = "The Sleeping Camp";
        public int raiderCount = 4;
        public float lootValue = 300f;
        public float stealSuccessBaseChance = 0.50f;
        public string firefightSeverity = "brutal";
        public bool isLooted = false;
        public bool isFirefightStarted = false;
    }

    /// <summary>
    /// Prompt #608: Encounter: The Sleeping Camp.
    /// Four heavily armed Raiders are asleep with loot in the center. An Agility check
    /// determines whether the player can steal and leave undetected. A failed roll
    /// triggers a point-blank firefight.
    /// </summary>
    public class Encounter_SleepingCamp
    {
        private SleepingCampState _state = new SleepingCampState();

        public event Action<SleepingCampState> OnCampDiscovered;
        public event Action<SleepingCampState, float> OnLootStolen;
        public event Action<SleepingCampState, string> OnFirefightInitiated;
        public event Action<SleepingCampState> OnFleeSuccessful;

        public SleepingCampState State => _state;

        /// <summary>
        /// Notifies that the camp has been discovered by the player.
        /// </summary>
        public void DiscoverCamp()
        {
            OnCampDiscovered?.Invoke(_state);
        }

        /// <summary>
        /// Attempts to steal loot from the sleeping camp.
        /// Success chance = base + (agility / 200).
        /// </summary>
        /// <param name="agility">The survivor's agility stat.</param>
        /// <param name="rng">Random number generator.</param>
        /// <returns>True if loot was stolen successfully; false triggers a firefight.</returns>
        public bool TrySteal(float agility, System.Random rng)
        {
            if (_state.isLooted || _state.isFirefightStarted)
                return false;

            float successChance = _state.stealSuccessBaseChance + (agility / 200f);
            successChance = Mathf.Clamp01(successChance);

            if (rng.NextDouble() < successChance)
            {
                _state.isLooted = true;
                OnLootStolen?.Invoke(_state, _state.lootValue);
                return true;
            }

            _state.isFirefightStarted = true;
            OnFirefightInitiated?.Invoke(_state, _state.firefightSeverity);
            return false;
        }

        /// <summary>
        /// Attempts to flee without looting. Always succeeds but yields no loot.
        /// </summary>
        /// <param name="rng">Random number generator.</param>
        /// <returns>True (always succeeds).</returns>
        public bool TryFleeWithoutLooting(System.Random rng)
        {
            OnFleeSuccessful?.Invoke(_state);
            return true;
        }

        public SleepingCampState CaptureState() => _state;

        public void RestoreState(SleepingCampState saved)
        {
            _state = saved ?? new SleepingCampState();
        }
    }
}
