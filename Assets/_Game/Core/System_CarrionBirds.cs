using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class CarrionBirdsState
    {
        public string systemId = "system_carrion_birds";
        public string displayName = "Carrion Birds";
        public int corpseCount = 0;
        public bool vulturesPresent = false;
        public float hatchVisibilityOverride = 1.0f;
    }

    /// <summary>
    /// Prompt #658: System — Carrion Birds.
    /// Corpses left outside the hatch attract mutated vultures. Circling vultures
    /// raise HatchVisibility to 100%, drawing Warlord attention. Remove corpses
    /// to disperse the flock.
    /// </summary>
    public class System_CarrionBirds
    {
        private CarrionBirdsState _state = new CarrionBirdsState();

        // -- Events --
        public event Action<CarrionBirdsState> OnVulturesArrived;
        public event Action<CarrionBirdsState> OnVulturesDeparted;
        public event Action<CarrionBirdsState, int> OnCorpseAdded;
        public event Action<CarrionBirdsState> OnCorpsesRemoved;

        public CarrionBirdsState State => _state;

        /// <summary>
        /// Adds a corpse to the area outside the hatch.
        /// </summary>
        public void AddCorpse()
        {
            _state.corpseCount++;
            OnCorpseAdded?.Invoke(_state, _state.corpseCount);
        }

        /// <summary>
        /// Daily tick. Vultures arrive if there are corpses; they leave if
        /// all corpses have been removed.
        /// </summary>
        public void TickDay()
        {
            bool shouldBePresent = _state.corpseCount > 0;

            if (shouldBePresent && !_state.vulturesPresent)
            {
                _state.vulturesPresent = true;
                _state.hatchVisibilityOverride = 1.0f;
                OnVulturesArrived?.Invoke(_state);
            }
            else if (!shouldBePresent && _state.vulturesPresent)
            {
                _state.vulturesPresent = false;
                _state.hatchVisibilityOverride = 0f;
                OnVulturesDeparted?.Invoke(_state);
            }
        }

        /// <summary>
        /// Returns the current hatch visibility override. 1.0f (100%) when
        /// vultures are circling, 0f otherwise.
        /// </summary>
        public float GetHatchVisibility()
        {
            return _state.vulturesPresent ? _state.hatchVisibilityOverride : 0f;
        }

        /// <summary>
        /// Removes all corpses from outside the hatch, dispersing vultures
        /// on the next daily tick.
        /// </summary>
        public void RemoveCorpses()
        {
            _state.corpseCount = 0;
            OnCorpsesRemoved?.Invoke(_state);
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public CarrionBirdsState GetState() => _state;

        public void RestoreState(CarrionBirdsState state)
        {
            _state = state ?? new CarrionBirdsState();
        }
    }
}
