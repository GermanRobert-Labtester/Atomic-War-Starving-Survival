using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class StructuralFailureState
    {
        public string crisisId = "crisis_structural_failure";
        public int daysAtZeroIntegrity = 0;
        public int breachThresholdDays = 3;
        public bool isBreached = false;
        public bool shieldingPermanentlyZeroed = false;
        public bool evacuationRequired = false;
    }

    /// <summary>
    /// Prompt #561: Crisis — Bunker Breach (Structural Failure).
    /// If StructuralIntegrity stays at 0% for 3 consecutive days, walls crack
    /// permanently. Radiation Shielding is permanently reduced to 0. The bunker
    /// is no longer safe — the player must evacuate to a Vehicle or die.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class Crisis_StructuralFailure
    {
        private StructuralFailureState _state = new StructuralFailureState();

        // -- Events --
        public event Action<StructuralFailureState, int> OnIntegrityZeroDayCounted;
        public event Action<StructuralFailureState> OnBunkerBreached;
        public event Action<StructuralFailureState> OnEvacuationRequired;

        public StructuralFailureState State => _state;

        /// <summary>
        /// Called once per game-day. Tracks consecutive days at zero integrity.
        /// Triggers a permanent breach after <see cref="StructuralFailureState.breachThresholdDays"/>
        /// consecutive days.
        /// </summary>
        public void TickDay(float currentStructuralIntegrity)
        {
            if (_state.isBreached) return;

            if (currentStructuralIntegrity <= 0f)
            {
                _state.daysAtZeroIntegrity++;
                OnIntegrityZeroDayCounted?.Invoke(_state, _state.daysAtZeroIntegrity);

                if (_state.daysAtZeroIntegrity >= _state.breachThresholdDays)
                {
                    TriggerBreach();
                }
            }
            else
            {
                // Any day with positive integrity resets the counter.
                _state.daysAtZeroIntegrity = 0;
            }
        }

        /// <summary>
        /// Permanently breaches the bunker. Sets all failure flags and fires
        /// <see cref="OnBunkerBreached"/> and <see cref="OnEvacuationRequired"/>.
        /// </summary>
        public void TriggerBreach()
        {
            if (_state.isBreached) return;

            _state.isBreached = true;
            _state.shieldingPermanentlyZeroed = true;
            _state.evacuationRequired = true;

            OnBunkerBreached?.Invoke(_state);
            OnEvacuationRequired?.Invoke(_state);
        }

        /// <summary>Returns true while the bunker is still habitable (not breached).</summary>
        public bool IsBunkerHabitable() => !_state.isBreached;

        /// <summary>
        /// Returns 0f when breached (shielding forced to zero), or -1f when
        /// no override is active (caller should use normal shielding value).
        /// </summary>
        public float GetShieldingOverride()
        {
            return _state.isBreached ? 0f : -1f;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public StructuralFailureState GetState() => _state;

        // ── Save / Load ────────────────────────────────────────────────


        public StructuralFailureState CaptureState() => _state;



        public void RestoreState(StructuralFailureState state)
        {
            _state = state ?? new StructuralFailureState();
        }

}
}
