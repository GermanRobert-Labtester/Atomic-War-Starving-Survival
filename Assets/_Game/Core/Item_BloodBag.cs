using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class BloodBagState
    {
        public string itemId = "item_blood_bag";
        public string displayName = "Refrigerated Blood Bag";
        public bool isSpoiled = false;
        public float hoursWithoutPower = 0f;
    }

    /// <summary>
    /// Prompt #437: Item: Refrigerated Blood Bag.
    /// Used for Transfusions (#55). Must be stored in a powered Fridge module.
    /// If power goes down for > 1 hour, the blood spoils and becomes useless.
    /// </summary>
    public class Item_BloodBag
    {
        private BloodBagState _state = new BloodBagState();

        public event Action<BloodBagState> OnBloodBagSpoiled;

        public BloodBagState State => _state;

        public void TickPowerStatus(bool isFridgePowered, float deltaHours)
        {
            if (_state.isSpoiled) return;

            if (!isFridgePowered)
            {
                _state.hoursWithoutPower += deltaHours;
                if (_state.hoursWithoutPower > 1.0f)
                {
                    _state.isSpoiled = true;
                    OnBloodBagSpoiled?.Invoke(_state);
                }
            }
            else
            {
                _state.hoursWithoutPower = 0f;
            }
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public BloodBagState CaptureState() => _state;

        public void RestoreState(BloodBagState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
