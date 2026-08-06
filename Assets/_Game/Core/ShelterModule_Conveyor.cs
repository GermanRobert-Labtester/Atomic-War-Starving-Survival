using System;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class ConveyorState
    {
        public string moduleId = "shelter_module_conveyor";
        public float lacerationChance = 0.05f;
        public bool isActive = false;
    }

    /// <summary>
    /// Prompt #801: Conveyor Belts.
    /// Moves heavy items between rooms instantly.
    /// 5% chance to catch clothes and cause Laceration if a survivor stands on it.
    /// </summary>
    public class ShelterModule_Conveyor
    {
        public event Action<string, string, string> OnItemMoved;       // itemId, fromRoom, toRoom
        public event Action<string> OnSurvivorLacerated;               // survivorId

        private ConveyorState _state;

        public ShelterModule_Conveyor(ConveyorState state = null)
        {
            _state = state ?? new ConveyorState();
        }

        public string ModuleId => _state.moduleId;

        public void Activate()
        {
            _state.isActive = true;
        }

        public void Deactivate()
        {
            _state.isActive = false;
        }

        public bool IsActive() => _state.isActive;

        /// <summary>
        /// Move an item between rooms instantly via conveyor belt.
        /// </summary>
        public void MoveItem(string itemId, string fromRoom, string toRoom)
        {
            if (!_state.isActive)
            {
                Debug.LogWarning("[ShelterModule_Conveyor] MoveItem called but conveyor is not active.");
                return;
            }

            if (string.IsNullOrEmpty(itemId) || string.IsNullOrEmpty(fromRoom) || string.IsNullOrEmpty(toRoom))
            {
                Debug.LogWarning("[ShelterModule_Conveyor] MoveItem called with null/empty argument.");
                return;
            }

            OnItemMoved?.Invoke(itemId, fromRoom, toRoom);
        }

        /// <summary>
        /// Check whether a survivor standing on the belt gets lacerated.
        /// Returns true if laceration occurred.
        /// </summary>
        public bool CheckSurvivorOnBelt(string survivorId, System.Random rng)
        {
            if (!_state.isActive)
                return false;

            if (string.IsNullOrEmpty(survivorId) || rng == null)
            {
                Debug.LogWarning("[ShelterModule_Conveyor] CheckSurvivorOnBelt called with null argument.");
                return false;
            }

            double roll = rng.NextDouble();
            if (roll < _state.lacerationChance)
            {
                OnSurvivorLacerated?.Invoke(survivorId);
                return true;
            }

            return false;
        }

        public ConveyorState CaptureState()
        {
            return new ConveyorState
            {
                moduleId = _state.moduleId,
                lacerationChance = _state.lacerationChance,
                isActive = _state.isActive
            };
        }

        public void RestoreState(ConveyorState state)
        {
            _state = state ?? new ConveyorState();
        }
    }
}
