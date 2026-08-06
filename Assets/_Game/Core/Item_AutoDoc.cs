using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class AutoDocState
    {
        public string itemId = "item_auto_doc";
        public string displayName = "Surgical Auto-Doc Arm";
        public float powerDrainActive = 100f;
        public float surgerySuccessRate = 1.0f;
        public bool isInstalled = false;
        public bool requiresMedicalBed = true;
    }

    /// <summary>
    /// Prompt #617: Item: Surgical Auto-Doc Arm.
    /// Immense medical robotic arm. Installed in MedicalBed for 100% surgery success
    /// with no doctor needed. Drains 100W while active.
    /// </summary>
    public class Item_AutoDoc
    {
        private AutoDocState _state = new AutoDocState();

        public event Action<AutoDocState> OnAutoDocInstalled;
        public event Action<AutoDocState, string, string, float> OnSurgeryPerformed;
        public event Action<AutoDocState, string, string> OnSurgeryFailed;

        public AutoDocState State => _state;

        public bool Install(bool hasMedicalBed, bool hasPower)
        {
            if (!hasMedicalBed || !hasPower)
                return false;

            _state.isInstalled = true;
            OnAutoDocInstalled?.Invoke(_state);
            return true;
        }

        public (bool success, float powerConsumed) PerformSurgery(string patientId, string surgeryType)
        {
            if (!_state.isInstalled)
            {
                OnSurgeryFailed?.Invoke(_state, patientId, "not_installed");
                return (false, 0f);
            }

            float powerConsumed = _state.powerDrainActive;

            OnSurgeryPerformed?.Invoke(_state, patientId, surgeryType, powerConsumed);
            return (true, powerConsumed);
        }
    }
}
