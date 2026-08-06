using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class ShockCollarState
    {
        public string itemId = "item_shock_collar";
        public bool isEquipped = false;
        public string captiveId = "";
        public float powerRequired = 10f;
        public bool isCollarActive = false;
    }

    /// <summary>
    /// Prompt #661: Item: Shock Collar.
    /// Capture Raider → equip collar → Forced Laborer. Executes tasks without Morale.
    /// Power drop → collar deactivates → instant mutiny.
    /// </summary>
    public class Item_ShockCollar
    {
        private ShockCollarState _state = new ShockCollarState();

        public event Action<ShockCollarState, string> OnCollarEquipped;
        public event Action<ShockCollarState> OnCollarActivated;
        public event Action<ShockCollarState> OnCollarDeactivated;
        public event Action<ShockCollarState> OnMutinyTriggered;

        public ShockCollarState State => _state;

        public bool Equip(string raiderId, bool hasPower)
        {
            if (_state.isEquipped || string.IsNullOrEmpty(raiderId))
                return false;

            _state.captiveId = raiderId;
            _state.isEquipped = true;
            _state.isCollarActive = hasPower;

            OnCollarEquipped?.Invoke(_state, raiderId);

            if (hasPower)
                OnCollarActivated?.Invoke(_state);

            return true;
        }

        public bool TickHour(bool powerAvailable)
        {
            if (!_state.isEquipped)
                return false;

            bool wasActive = _state.isCollarActive;
            _state.isCollarActive = powerAvailable;

            if (wasActive && !powerAvailable)
            {
                OnCollarDeactivated?.Invoke(_state);
                OnMutinyTriggered?.Invoke(_state);
                return true; // mutiny triggered
            }

            if (!wasActive && powerAvailable)
                OnCollarActivated?.Invoke(_state);

            return false;
        }
    }
}
