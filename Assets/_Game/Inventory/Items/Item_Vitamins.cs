using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Inventory
{
    [Serializable]
    public class VitaminsState
    {
        public string itemId = "item_vitamins";
        public string displayName = "Pre-War Vitamins";
        public int chargesRemaining = 30;
        public bool preventsScurvyAndListless = true;
    }

    /// <summary>
    /// Prompt #432: Item: Pre-War Vitamins.
    /// Bottle with 30 charges. Taking one daily completely prevents Scurvy (#57) and Listless (#17),
    /// allowing survivors to subsist purely on nutrient paste/fungi without health decay.
    /// </summary>
    public class Item_Vitamins
    {
        private VitaminsState _state = new VitaminsState();

        public event Action<VitaminsState, string, int> OnDailyVitaminConsumed;

        public VitaminsState State => _state;

        public bool ConsumeDailySupplement(string survivorId, out bool resistsScurvyListless)
        {
            resistsScurvyListless = false;
            if (_state.chargesRemaining > 0)
            {
                _state.chargesRemaining--;
                resistsScurvyListless = true;
                OnDailyVitaminConsumed?.Invoke(_state, survivorId, _state.chargesRemaining);
                return true;
            }
            return false;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public VitaminsState CaptureState() => _state;

        public void RestoreState(VitaminsState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
