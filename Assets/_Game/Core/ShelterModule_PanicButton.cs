using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class PanicButtonState
    {
        public string moduleId = "shelter_module_panic_button";
        public bool isLockedDown = false;
    }

    /// <summary>
    /// Prompt #803: Panic Button (Bulkhead Seal).
    /// Drops titanium blast doors on ALL rooms. Halts Raiders but locks survivors
    /// in their current room until deactivated.
    /// </summary>
    public class ShelterModule_PanicButton
    {
        public event Action OnLockdownActivated;
        public event Action OnLockdownDeactivated;
        public event Action<string> OnSurvivorLockedIn;   // survivorId
        public event Action<string> OnRaiderHalted;        // raiderId

        private PanicButtonState _state;
        private List<string> _lockedSurvivorIds = new List<string>();

        public ShelterModule_PanicButton(PanicButtonState state = null)
        {
            _state = state ?? new PanicButtonState();
        }

        public string ModuleId => _state.moduleId;

        /// <summary>
        /// Activate lockdown: drops blast doors, halts all raiders, locks survivors in rooms.
        /// survivorRoomAssignments: list of survivorIds currently inside the shelter.
        /// </summary>
        public void Activate(List<string> survivorRoomAssignments)
        {
            if (_state.isLockedDown)
            {
                Debug.LogWarning("[ShelterModule_PanicButton] Activate called but already locked down.");
                return;
            }

            _state.isLockedDown = true;
            _lockedSurvivorIds.Clear();

            // Lock all survivors in their current rooms
            if (survivorRoomAssignments != null)
            {
                foreach (string survivorId in survivorRoomAssignments)
                {
                    if (!string.IsNullOrEmpty(survivorId))
                    {
                        _lockedSurvivorIds.Add(survivorId);
                        OnSurvivorLockedIn?.Invoke(survivorId);
                    }
                }
            }

            OnLockdownActivated?.Invoke();
        }

        /// <summary>
        /// Halts a specific raider (called by external combat system when blast doors block them).
        /// </summary>
        public void HaltRaider(string raiderId)
        {
            if (!_state.isLockedDown || string.IsNullOrEmpty(raiderId))
                return;

            OnRaiderHalted?.Invoke(raiderId);
        }

        /// <summary>
        /// Deactivate lockdown: lifts blast doors, frees survivors.
        /// </summary>
        public void Deactivate()
        {
            if (!_state.isLockedDown)
            {
                Debug.LogWarning("[ShelterModule_PanicButton] Deactivate called but not locked down.");
                return;
            }

            _state.isLockedDown = false;
            _lockedSurvivorIds.Clear();
            OnLockdownDeactivated?.Invoke();
        }

        public bool IsLockedDown() => _state.isLockedDown;

        public List<string> GetLockedSurvivors() => new List<string>(_lockedSurvivorIds);

        public PanicButtonState CaptureState()
        {
            return new PanicButtonState
            {
                moduleId = _state.moduleId,
                isLockedDown = _state.isLockedDown
            };
        }

        public void RestoreState(PanicButtonState state)
        {
            _state = state ?? new PanicButtonState();
        }
    }
}
