using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Inventory
{
    [Serializable]
    public class WalkieTalkieState
    {
        public string itemId = "item_walkie_talkie";
        public string displayName = "Walkie-Talkie";
        public int batteriesRemaining = 10;
        public int maxBatteries = 10;
        public bool isEquipped = false;
        public bool isManualControl = false;
    }

    /// <summary>
    /// Prompt #629: Item: Walkie-Talkie.
    /// Equip to gain manual control on the Expedition Map (stance, reroute, flee).
    /// When batteries run out, control reverts to UtilityAI and comms are severed.
    /// </summary>
    public class Item_WalkieTalkie
    {
        private WalkieTalkieState _state = new WalkieTalkieState();

        public event Action<WalkieTalkieState> OnEquipped;
        public event Action<WalkieTalkieState> OnBatteryDepleted;
        public event Action<WalkieTalkieState> OnCommsSevered;

        public WalkieTalkieState State => _state;

        public void Equip()
        {
            if (_state.batteriesRemaining <= 0) return;

            _state.isEquipped = true;
            _state.isManualControl = true;
            OnEquipped?.Invoke(_state);
        }

        public void TickHour(float hours)
        {
            if (!_state.isEquipped) return;

            float consumed = hours;
            int drain = Mathf.CeilToInt(consumed);
            _state.batteriesRemaining = Mathf.Max(0, _state.batteriesRemaining - drain);

            if (_state.batteriesRemaining <= 0)
            {
                _state.isManualControl = false;
                _state.isEquipped = false;
                OnBatteryDepleted?.Invoke(_state);
                OnCommsSevered?.Invoke(_state);
            }
        }

        public void UseBattery()
        {
            if (_state.batteriesRemaining > 0)
            {
                _state.batteriesRemaining--;

                if (_state.batteriesRemaining <= 0)
                {
                    _state.isManualControl = false;
                    _state.isEquipped = false;
                    OnBatteryDepleted?.Invoke(_state);
                    OnCommsSevered?.Invoke(_state);
                }
            }
        }

        public bool IsCommsActive()
        {
            return _state.isEquipped && _state.batteriesRemaining > 0;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public WalkieTalkieState CaptureState() => _state;

        public void RestoreState(WalkieTalkieState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
