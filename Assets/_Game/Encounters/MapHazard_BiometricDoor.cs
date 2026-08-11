using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Encounters
{
    [Serializable]
    public class BiometricDoorState
    {
        public string hazardId = "map_hazard_biometric_door";
        public string requiredCommanderId = "";
        public bool isUnlocked = false;
    }

    /// <summary>
    /// Biometric Locks — high-security doors that require the fingerprint of a
    /// specific faction commander. The only way to open them is to assassinate
    /// the commander and harvest their severed hand as a key item.
    /// Prompt #796: MapHazard_BiometricDoor
    /// </summary>
    /// <summary>DEMOTE-MapHazard-batch — dormant ghost. Re-promote with Boot+Save+host.</summary>
    public class MapHazard_BiometricDoor
    {
        // -- Events --
        public event Action<string> OnDoorUnlocked;  // survivorId who opened it
        public event Action<string> OnDoorRejected;  // survivorId who was denied

        // -- State --
        private string _requiredCommanderId = "";
        private bool _isUnlocked = false;

        // -- Public API --

        /// <summary>
        /// Sets the faction commander whose fingerprint is required to open this door.
        /// </summary>
        public void SetRequiredCommander(string commanderId)
        {
            if (string.IsNullOrEmpty(commanderId))
            {
                Debug.LogWarning("[BiometricDoor] Commander id cannot be null or empty.");
                return;
            }
            _requiredCommanderId = commanderId;
        }

        /// <summary>
        /// Attempts to open the biometric door. The survivor must have the
        /// severed hand of the required commander in their inventory
        /// (item id: "item_severed_hand_" + commanderId).
        /// Returns true if the door was successfully opened.
        /// </summary>
        public bool TryOpen(string survivorId, List<string> inventoryItemIds)
        {
            if (string.IsNullOrEmpty(survivorId))
            {
                Debug.LogWarning("[BiometricDoor] Survivor id cannot be null or empty.");
                return false;
            }
            if (_isUnlocked)
            {
                Debug.LogWarning("[BiometricDoor] Door is already unlocked.");
                return true;
            }
            if (string.IsNullOrEmpty(_requiredCommanderId))
            {
                Debug.LogWarning("[BiometricDoor] No required commander set.");
                OnDoorRejected?.Invoke(survivorId);
                return false;
            }

            string requiredItemId = "item_severed_hand_" + _requiredCommanderId;
            if (inventoryItemIds != null && inventoryItemIds.Contains(requiredItemId))
            {
                _isUnlocked = true;
                OnDoorUnlocked?.Invoke(survivorId);
                return true;
            }

            OnDoorRejected?.Invoke(survivorId);
            return false;
        }

        /// <summary>Returns the commander id required to open this door.</summary>
        public string GetRequiredCommanderId() => _requiredCommanderId;

        /// <summary>Returns true if the door has been unlocked.</summary>
        public bool IsUnlocked() => _isUnlocked;

        // -- Save / Load --

        public BiometricDoorState CaptureState()
        {
            return new BiometricDoorState
            {
                hazardId = "map_hazard_biometric_door",
                requiredCommanderId = _requiredCommanderId,
                isUnlocked = _isUnlocked
            };
        }

        public void RestoreState(BiometricDoorState saved)
        {
            if (saved == null) return;
            _requiredCommanderId = saved.requiredCommanderId;
            _isUnlocked = saved.isUnlocked;
        }
    }
}
