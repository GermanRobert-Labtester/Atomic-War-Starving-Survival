using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class WristDosimeterState
    {
        public string itemId = "item_wrist_dosimeter";
        public string displayName = "Wrist-Mounted Dosimeter";
        public string slotType = "Accessory";
        public bool isBroken = false;
    }

    /// <summary>
    /// Prompt #424: Gear: Wrist-Mounted Dosimeter.
    /// Occupies the Accessory slot, allowing 2-handed weapons in hand slots.
    /// Fragile device that breaks instantly if the survivor enters melee combat.
    /// </summary>
    public class Item_WristDosimeter
    {
        private WristDosimeterState _state = new WristDosimeterState();

        public event Action<WristDosimeterState> OnDosimeterBrokenInMelee;

        public WristDosimeterState State => _state;

        public void TriggerMeleeImpact()
        {
            if (!_state.isBroken)
            {
                _state.isBroken = true;
                OnDosimeterBrokenInMelee?.Invoke(_state);
            }
        }
    }
}
