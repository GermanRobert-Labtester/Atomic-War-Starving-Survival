using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class BatteryBankState
    {
        public string moduleId = "shelter_module_battery_bank";
        public string displayName = "Battery Banks";
        public bool isBuilt = false;
        public float storedWattHours = 0f;
        public float maxCapacityWattHours = 1000f;
        public bool isSilentRunningActive = false;
    }

    /// <summary>
    /// Prompt #405: Module: Battery Banks.
    /// Stores excess generated power wattage. Allows the shelter to engage "Silent Running Mode",
    /// shutting off noisy generators to operate silently (zero Noise) and avoid hostile patrols.
    /// </summary>
    public class ShelterModule_BatteryBank
    {
        private BatteryBankState _state = new BatteryBankState();

        public event Action<BatteryBankState, bool> OnSilentRunningToggled;
        public event Action<BatteryBankState, float> OnPowerStored;

        public BatteryBankState State => _state;

        public void StoreExcessWattage(float excessWatts, float deltaHours)
        {
            if (!_state.isBuilt) return;
            float added = excessWatts * deltaHours;
            _state.storedWattHours = Mathf.Min(_state.maxCapacityWattHours, _state.storedWattHours + added);
            OnPowerStored?.Invoke(_state, _state.storedWattHours);
        }

        public bool ToggleSilentRunningMode(bool enable)
        {
            if (!_state.isBuilt) return false;
            if (enable && _state.storedWattHours <= 0f) return false;

            _state.isSilentRunningActive = enable;
            OnSilentRunningToggled?.Invoke(_state, enable);
            return true;
        }
    }
}
