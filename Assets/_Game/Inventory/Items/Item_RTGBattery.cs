using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Inventory
{
    [Serializable]
    public class RTGBatteryState
    {
        public string itemId = "item_rtg_battery";
        public string displayName = "Prototype RTG Battery";
        public float powerOutput = 500f;
        public float radiationPerHour = 5f;
        public bool isPluggedIn = false;
        public bool isLeadLined = false;
        public float weight = 25f;
    }

    /// <summary>
    /// Prompt #609: Item: Prototype RTG Battery.
    /// Radioisotope thermoelectric generator providing massive infinite power.
    /// Constantly emits radiation unless stored in a Lead-Lined room.
    /// </summary>
    public class Item_RTGBattery
    {
        private RTGBatteryState _state = new RTGBatteryState();

        public event Action<RTGBatteryState, bool> OnRTGPluggedIn;
        public event Action<RTGBatteryState, float> OnRadiationLeaked;
        public event Action<RTGBatteryState> OnRTGShielded;

        public RTGBatteryState State => _state;

        public void PlugIn(bool isLeadLinedRoom)
        {
            _state.isPluggedIn = true;
            _state.isLeadLined = isLeadLinedRoom;

            OnRTGPluggedIn?.Invoke(_state, isLeadLinedRoom);

            if (isLeadLinedRoom)
                OnRTGShielded?.Invoke(_state);
        }

        public void TickHour()
        {
            if (_state.isPluggedIn && !_state.isLeadLined)
            {
                OnRadiationLeaked?.Invoke(_state, _state.radiationPerHour);
            }
        }

        public float GetPowerOutput()
        {
            return _state.isPluggedIn ? _state.powerOutput : 0f;
        }

        public float GetRadiationLeak()
        {
            if (_state.isPluggedIn && !_state.isLeadLined)
                return _state.radiationPerHour;

            return 0f;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public RTGBatteryState CaptureState() => _state;

        public void RestoreState(RTGBatteryState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
