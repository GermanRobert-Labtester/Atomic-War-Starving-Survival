using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Inventory
{
    [Serializable]
    public class UndeliveredMailState
    {
        public string itemId = "item_undelivered_mail";
        public string displayName = "Stack of Undelivered Mail";
        public bool isRead = false;
        public bool isBurnedAsTinder = false;
        public float tinderHeatDurationHours = 1.0f;
    }

    /// <summary>
    /// Prompt #468: Artifact: Stack of Undelivered Mail.
    /// Can be read during downtime to unlock survivor backstory journal entries,
    /// OR burned in the Cast Iron Stove (#441) as tinder for 1 hour of heat. Lore vs survival dilemma.
    /// </summary>
    public class Item_UndeliveredMail
    {
        private UndeliveredMailState _state = new UndeliveredMailState();

        public event Action<UndeliveredMailState, string> OnMailReadBackstoryUnlocked;
        public event Action<UndeliveredMailState, float> OnMailBurnedAsTinder;

        public UndeliveredMailState State => _state;

        public bool ReadMail(string survivorId)
        {
            if (!_state.isBurnedAsTinder)
            {
                _state.isRead = true;
                OnMailReadBackstoryUnlocked?.Invoke(_state, survivorId);
                return true;
            }
            return false;
        }

        public float BurnAsTinder()
        {
            if (!_state.isBurnedAsTinder)
            {
                _state.isBurnedAsTinder = true;
                OnMailBurnedAsTinder?.Invoke(_state, _state.tinderHeatDurationHours);
                return _state.tinderHeatDurationHours;
            }
            return 0f;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public UndeliveredMailState CaptureState() => _state;

        public void RestoreState(UndeliveredMailState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
