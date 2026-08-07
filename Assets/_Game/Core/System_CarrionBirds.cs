using System;
using UnityEngine;

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
        /// <summary>Shelter-node danger added while vultures circle (host applies).</summary>
        public const float MapDangerBoost = 2f;

        /// <summary>Morale lost per living survivor per day while vultures circle.</summary>
        public const float MoralePressurePerDay = 3f;

        private CarrionBirdsState _state;

        // -- Events --
        public event Action<CarrionBirdsState> OnVulturesArrived;
        public event Action<CarrionBirdsState> OnVulturesDeparted;
        public event Action<CarrionBirdsState, int> OnCorpseAdded;
        public event Action<CarrionBirdsState> OnCorpsesRemoved;

        public System_CarrionBirds(CarrionBirdsState state = null)
        {
            _state = state != null ? CloneState(state) : new CarrionBirdsState();
            if (string.IsNullOrEmpty(_state.systemId))
                _state.systemId = "system_carrion_birds";
            if (string.IsNullOrEmpty(_state.displayName))
                _state.displayName = "Carrion Birds";
        }

        public CarrionBirdsState State => _state;
        public string SystemId => _state.systemId;
        public int CorpseCount => _state.corpseCount;
        public bool VulturesPresent => _state.vulturesPresent;

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

        /// <summary>Snapshot copy for save (does not share live state).</summary>
        public CarrionBirdsState CaptureState()
        {
            return CloneState(_state);
        }

        /// <summary>Legacy alias used by older call sites; prefer <see cref="CaptureState"/>.</summary>
        public CarrionBirdsState GetState() => CaptureState();

        public void RestoreState(CarrionBirdsState state)
        {
            if (state == null)
            {
                _state = new CarrionBirdsState();
                return;
            }

            _state = CloneState(state);
            if (string.IsNullOrEmpty(_state.systemId))
                _state.systemId = "system_carrion_birds";
            if (string.IsNullOrEmpty(_state.displayName))
                _state.displayName = "Carrion Birds";
            _state.corpseCount = Mathf.Max(0, _state.corpseCount);
            _state.hatchVisibilityOverride = Mathf.Clamp01(_state.hatchVisibilityOverride);
            if (!_state.vulturesPresent)
                _state.hatchVisibilityOverride = 0f;
            else if (_state.hatchVisibilityOverride <= 0f)
                _state.hatchVisibilityOverride = 1f;
        }

        private static CarrionBirdsState CloneState(CarrionBirdsState src)
        {
            return new CarrionBirdsState
            {
                systemId = string.IsNullOrEmpty(src.systemId) ? "system_carrion_birds" : src.systemId,
                displayName = string.IsNullOrEmpty(src.displayName) ? "Carrion Birds" : src.displayName,
                corpseCount = Mathf.Max(0, src.corpseCount),
                vulturesPresent = src.vulturesPresent,
                hatchVisibilityOverride = Mathf.Clamp01(src.hatchVisibilityOverride)
            };
        }
    }
}
