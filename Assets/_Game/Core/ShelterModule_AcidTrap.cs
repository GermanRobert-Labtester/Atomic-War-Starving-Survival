using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class AcidTrapState
    {
        public string moduleId = "shelter_module_acid_trap";
        public float acidReserve;
        public bool isActive;
        public int triggeredCount;
        public int toxicUntilDay;
        public bool lootDestroyed;
    }

    /// <summary>
    /// Prompt #828: Acid Trap. Built into the Airlock ceiling. If raiders
    /// breach the outer door, the player triggers it. Instantly melts
    /// unarmored raiders, destroys their loot, and leaves the Airlock
    /// highly toxic for 7 in-game days.
    /// Plain C#. Save/load safe.
    /// </summary>
    public class ShelterModule_AcidTrap
    {
        private AcidTrapState _state = new AcidTrapState();

        private const float AcidCostPerTrigger = 25f;
        private const float UnarmoredThreshold = 0.3f;
        private const int ToxicDurationDays = 7;

        // -- Events --
        public event Action OnArmed;
        public event Action<bool, bool> OnTriggered;        // (raidersKilled, lootDestroyed)
        public event Action<int> OnToxicityApplied;         // duration in days
        public event Action OnRefilled;

        public AcidTrapState State => _state;

        /// <summary>
        /// Arm the acid trap. Requires acid reserve to be above zero.
        /// </summary>
        public void Arm()
        {
            if (_state.acidReserve <= 0f) return;

            _state.isActive = true;
            OnArmed?.Invoke();
        }

        /// <summary>
        /// Trigger the acid trap on raiders breaching the outer door.
        /// Unarmored raiders (armor &lt; 0.3) are killed instantly.
        /// Armored raiders survive with burns. All raider loot is destroyed.
        /// The Airlock becomes toxic for 7 days.
        /// </summary>
        /// <param name="raiderArmorLevel">
        /// Armor level of the incoming raiders [0–1].
        /// </param>
        /// <returns>
        /// True if unarmored raiders were killed. False if they survived
        /// (armored) or the trap could not fire.
        /// </returns>
        public bool Trigger(float raiderArmorLevel)
        {
            if (!CanTrigger()) return false;

            _state.acidReserve -= AcidCostPerTrigger;
            _state.triggeredCount++;
            _state.lootDestroyed = true;

            bool raidersKilled = raiderArmorLevel < UnarmoredThreshold;

            // Apply toxic debuff to the Airlock
            // Caller must pass current day to compute toxicUntilDay
            // We store a relative duration; caller resolves absolute day.
            OnToxicityApplied?.Invoke(ToxicDurationDays);
            OnTriggered?.Invoke(raidersKilled, _state.lootDestroyed);

            // Deactivate if reserve is exhausted
            if (_state.acidReserve <= 0f)
            {
                _state.isActive = false;
            }

            return raidersKilled;
        }

        /// <summary>
        /// Set the toxic-until-day value when the trap is triggered.
        /// Call after Trigger() with the current in-game day.
        /// </summary>
        /// <param name="currentDay">The current in-game day.</param>
        public void ApplyToxicity(int currentDay)
        {
            _state.toxicUntilDay = currentDay + ToxicDurationDays;
        }

        /// <summary>
        /// Returns the number of days of toxicity remaining.
        /// </summary>
        /// <param name="currentDay">The current in-game day.</param>
        public int GetToxicityRemaining(int currentDay)
        {
            return Math.Max(0, _state.toxicUntilDay - currentDay);
        }

        /// <summary>
        /// Refill the acid reserve with chemicals.
        /// </summary>
        /// <param name="amount">Amount of acid to add.</param>
        public void Refill(float amount)
        {
            if (amount <= 0f) return;

            _state.acidReserve += amount;
            OnRefilled?.Invoke();
        }

        /// <summary>
        /// True when the trap is armed and has enough acid to fire.
        /// </summary>
        public bool CanTrigger()
        {
            return _state.isActive && _state.acidReserve >= AcidCostPerTrigger;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public AcidTrapState CaptureState()
        {
            return new AcidTrapState
            {
                moduleId = _state.moduleId,
                acidReserve = _state.acidReserve,
                isActive = _state.isActive,
                triggeredCount = _state.triggeredCount,
                toxicUntilDay = _state.toxicUntilDay,
                lootDestroyed = _state.lootDestroyed
            };
        }

        public void RestoreState(AcidTrapState saved)
        {
            _state = saved ?? new AcidTrapState();
        }
    }
}
