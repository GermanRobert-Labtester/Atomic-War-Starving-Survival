using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class ClimbingGearState
    {
        public string itemId = "item_climbing_gear";
        public string displayName = "Climbing Harness";
        public bool isEquipped = false;
        public float setupTimeHours = 1.0f;
        public float rubbleStaminaMultiplier = 0.50f; // Halves stamina cost
    }

    /// <summary>
    /// Prompt #427: Gear: Climbing Harness.
    /// Allows survivors to bypass collapsed stairwells in Urban multi-story nodes.
    /// Halves the Stamina cost of navigating Rubble, but takes 1 in-game hour to equip and unequip safely.
    /// </summary>
    public class Item_ClimbingGear
    {
        private ClimbingGearState _state = new ClimbingGearState();

        public event Action<ClimbingGearState, bool> OnClimbingGearToggled;

        public ClimbingGearState State => _state;

        public bool ToggleGearEquipped(out float timeSpentHours)
        {
            timeSpentHours = _state.setupTimeHours;
            _state.isEquipped = !_state.isEquipped;

            OnClimbingGearToggled?.Invoke(_state, _state.isEquipped);
            return _state.isEquipped;
        }

        public float GetRubbleStaminaCostMultiplier()
        {
            return _state.isEquipped ? _state.rubbleStaminaMultiplier : 1.0f;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public ClimbingGearState CaptureState() => _state;

        public void RestoreState(ClimbingGearState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
