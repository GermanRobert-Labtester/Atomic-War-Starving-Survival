using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class VaultDoorState
    {
        public string moduleId = "shelter_module_vault_door";
        public string displayName = "The Vault Door Upgrade";
        public bool isBuilt = false;
        public float securityRating = 10000f; // Unbreachable by raiders
        public bool isOpen = false;
        public bool isPowerActive = true;
        public bool isStuck = false;
    }

    /// <summary>
    /// Prompt #402: Module: The Vault Door Upgrade.
    /// Endgame hatch replacement with 10,000 ShelterSecurity rating (unbreachable).
    /// Requires electrical motor power to operate. If power drops, the door is stuck (open or closed).
    /// </summary>
    public class ShelterModule_VaultDoor
    {
        private VaultDoorState _state = new VaultDoorState();

        public event Action<VaultDoorState, bool> OnDoorStateChanged;
        public event Action<VaultDoorState> OnDoorStuckDueToPowerFailure;

        public VaultDoorState State => _state;

        public bool ToggleDoorState(bool hasPower)
        {
            _state.isPowerActive = hasPower;
            if (!hasPower)
            {
                _state.isStuck = true;
                OnDoorStuckDueToPowerFailure?.Invoke(_state);
                return false;
            }

            _state.isStuck = false;
            _state.isOpen = !_state.isOpen;
            OnDoorStateChanged?.Invoke(_state, _state.isOpen);
            return true;
        }
    
        public VaultDoorState CaptureState()
        {
            return _state;
        }

        public void RestoreState(VaultDoorState saved)
        {
            _state = saved ?? new VaultDoorState();
        }
    }
}

