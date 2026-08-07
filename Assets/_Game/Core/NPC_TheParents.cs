using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class TheParentsState
    {
        public string id = "npc_the_parents";
        public string displayName = "The Parents";
        public bool isDoorLocked = true;
        public bool hasBabyInside = true;
        public bool isHostile = false;
        public bool isSneakedPast = false;
        public bool isKilled = false;
        public string babyFoodItem = "infant_formula_can";
    }

    /// <summary>
    /// Prompt #347: NPC Encounter: The Parents.
    /// Highly aggressive civilians protecting a locked door containing their baby.
    /// Shoots on sight if player approaches door. Player can sneak past or murder them for baby food.
    /// </summary>
    public class NPC_TheParents
    {
        private TheParentsState _state = new TheParentsState();

        public event Action<TheParentsState> OnTerritorialAggressionTriggered;
        public event Action<TheParentsState> OnSneakedPast;
        public event Action<TheParentsState, string> OnKilledForBabyFood;

        public TheParentsState State => _state;

        public void ApproachDoor()
        {
            if (!_state.isSneakedPast && !_state.isKilled)
            {
                _state.isHostile = true;
                OnTerritorialAggressionTriggered?.Invoke(_state);
            }
        }

        public bool TryStealthSneak(int playerStealthSkill)
        {
            if (playerStealthSkill >= 12)
            {
                _state.isSneakedPast = true;
                OnSneakedPast?.Invoke(_state);
                return true;
            }
            ApproachDoor();
            return false;
        }

        public string KillParents()
        {
            _state.isKilled = true;
            _state.isHostile = false;

            OnKilledForBabyFood?.Invoke(_state, _state.babyFoodItem);
            return _state.babyFoodItem;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public TheParentsState CaptureState() => _state;

        public void RestoreState(TheParentsState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
