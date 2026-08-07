using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class WeldingGogglesState
    {
        public string itemId = "item_welding_goggles";
        public string displayName = "Welding Goggles";
        public bool negatesFlashBlindness = true;
        public bool negatesCataracts = true;
        public float perceptionAccuracyPenalty = 0.30f; // 30% penalty
    }

    /// <summary>
    /// Prompt #420: Gear: Welding Goggles.
    /// Protects eyes by negating FlashBlindness and Cataract effects,
    /// but reduces Perception and Accuracy by 30% due to dark lenses.
    /// </summary>
    public class Item_WeldingGoggles
    {
        private WeldingGogglesState _state = new WeldingGogglesState();

        public event Action<WeldingGogglesState, float> OnWeldingGogglesEquipped;

        public WeldingGogglesState State => _state;

        public float EquipGoggles(out bool protectsVision)
        {
            protectsVision = true;
            float multiplier = 1.0f - _state.perceptionAccuracyPenalty;

            OnWeldingGogglesEquipped?.Invoke(_state, multiplier);
            return multiplier;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public WeldingGogglesState CaptureState() => _state;

        public void RestoreState(WeldingGogglesState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
