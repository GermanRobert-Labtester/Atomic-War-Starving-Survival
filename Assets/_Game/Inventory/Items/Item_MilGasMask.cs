using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Inventory
{
    [Serializable]
    public class MilGasMaskState
    {
        public string itemId = "item_mil_gas_mask";
        public string displayName = "Military Gas Mask";
        public bool isAirborneAfflictionBlocked = true;
        public float filterDurabilityMinutes = 120f; // 2 hours per cartridge
        public bool isSuffocating = false;
    }

    /// <summary>
    /// Prompt #423: Gear: Military Gas Mask.
    /// Prevents all airborne Afflictions (ToxicGas, DustLung). Requires FilterCartridges.
    /// If the cartridge depletes mid-expedition, the survivor begins suffocating.
    /// </summary>
    public class Item_MilGasMask
    {
        private MilGasMaskState _state = new MilGasMaskState();

        public event Action<MilGasMaskState> OnFilterCartridgeDepletedSuffocating;

        public MilGasMaskState State => _state;

        public bool TickFilterConsumption(float deltaMinutes, ref int filterCartridgeCount)
        {
            _state.filterDurabilityMinutes -= deltaMinutes;
            if (_state.filterDurabilityMinutes <= 0f)
            {
                if (filterCartridgeCount > 0)
                {
                    filterCartridgeCount--;
                    _state.filterDurabilityMinutes = 120f;
                    _state.isSuffocating = false;
                    return true;
                }
                else
                {
                    _state.filterDurabilityMinutes = 0f;
                    _state.isSuffocating = true;
                    OnFilterCartridgeDepletedSuffocating?.Invoke(_state);
                    return false;
                }
            }
            return true;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public MilGasMaskState CaptureState() => _state;

        public void RestoreState(MilGasMaskState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
