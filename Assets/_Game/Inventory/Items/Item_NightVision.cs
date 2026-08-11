using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Inventory
{
    [Serializable]
    public class NightVisionState
    {
        public string itemId = "item_night_vision";
        public string displayName = "Night Vision Goggles (NVG)";
        public float batteryCharges = 10f;
        public float stealthBonusRatio = 0.40f;
        public float accuracyBonusRatio = 0.30f;
    }

    /// <summary>
    /// Prompt #425: Gear: Night Vision Goggles (NVG).
    /// Perfect vision during Night or Blackout conditions. Grants massive Stealth and Accuracy bonuses.
    /// Requires Batteries to function.
    /// </summary>
    public class Item_NightVision
    {
        private NightVisionState _state = new NightVisionState();

        public event Action<NightVisionState> OnBatteriesDepleted;

        public NightVisionState State => _state;

        public bool UseNightVision(bool isNightOrBlackout, float hoursUsed)
        {
            if (!isNightOrBlackout) return false;

            if (_state.batteryCharges > 0f)
            {
                _state.batteryCharges = Mathf.Max(0f, _state.batteryCharges - hoursUsed);
                if (_state.batteryCharges <= 0f)
                {
                    OnBatteriesDepleted?.Invoke(_state);
                }
                return true;
            }
            return false;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public NightVisionState CaptureState() => _state;

        public void RestoreState(NightVisionState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
