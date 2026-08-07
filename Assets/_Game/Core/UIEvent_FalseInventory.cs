using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class FalseInventoryState
    {
        public string eventId = "ui_event_false_inventory";
        public float paranoiaThreshold = 0.7f;
        public float flickerChance = 0.3f;
    }

    /// <summary>
    /// Prompt #750: False Inventory Counts.
    /// High Paranoia: Storage UI shows "0 Food", flickers back to real count.
    /// Makes player feel survivors' fear.
    /// </summary>
    public class UIEvent_FalseInventory
    {
        private FalseInventoryState _state = new FalseInventoryState();

        public event Action<string, int> OnFalseCountDisplayed;
        public event Action<string, int> OnRealCountRestored;

        public FalseInventoryState State => _state;

        public bool ShouldFlicker(float paranoiaLevel, System.Random rng)
        {
            if (paranoiaLevel < _state.paranoiaThreshold)
                return false;

            return rng.NextDouble() < _state.flickerChance;
        }

        public int GetDisplayedCount(string itemId, int realCount, float paranoiaLevel, System.Random rng)
        {
            if (!ShouldFlicker(paranoiaLevel, rng))
            {
                OnRealCountRestored?.Invoke(itemId, realCount);
                return realCount;
            }

            // Show false count of 0 to induce fear
            OnFalseCountDisplayed?.Invoke(itemId, 0);
            return 0;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public FalseInventoryState CaptureState() => _state;

        public void RestoreState(FalseInventoryState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
