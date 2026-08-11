using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Inventory
{
    [Serializable]
    public class CarrierBirdState
    {
        public string itemId = "item_carrier_bird";
        public string displayName = "Carrier Bird";
        public float predatorEatChance = 0.20f;
        public bool isReleased = false;
        public bool isAlive = true;
        public string lastMessageOrItemId = string.Empty;
    }

    /// <summary>
    /// Prompt #636: Item: Carrier Bird.
    /// Biological alternative to Walkie-Talkies. Release to send one message or
    /// item to the bunker instantly. 20% chance the bird is eaten by predators
    /// en route. Single-use item.
    /// </summary>
    public class Item_CarrierBird
    {
        private CarrierBirdState _state = new CarrierBirdState();

        public event Action<CarrierBirdState, string, bool> OnBirdReleased;
        public event Action<CarrierBirdState> OnBirdEatenByPredator;
        public event Action<CarrierBirdState, string> OnMessageDelivered;

        public CarrierBirdState State => _state;

        public (bool delivered, string messageOrItemId) Release(string messageOrItemId, System.Random rng)
        {
            if (_state.isReleased || !_state.isAlive)
                return (false, string.Empty);

            _state.isReleased = true;
            _state.lastMessageOrItemId = messageOrItemId ?? string.Empty;

            bool eaten = (float)rng.NextDouble() < _state.predatorEatChance;

            if (eaten)
            {
                _state.isAlive = false;
                OnBirdReleased?.Invoke(_state, messageOrItemId, false);
                OnBirdEatenByPredator?.Invoke(_state);
                return (false, string.Empty);
            }

            OnBirdReleased?.Invoke(_state, messageOrItemId, true);
            OnMessageDelivered?.Invoke(_state, messageOrItemId);
            return (true, messageOrItemId);
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public CarrierBirdState CaptureState() => _state;

        public void RestoreState(CarrierBirdState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
