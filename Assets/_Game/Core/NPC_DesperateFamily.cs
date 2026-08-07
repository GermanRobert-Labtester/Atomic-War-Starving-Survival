using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class DesperateFamilyState
    {
        public string id = "npc_desperate_family";
        public string displayName = "The Desperate Family";
        public bool isStarving = true;
        public float charityHopeBuff = 35f;
        public int foodRequiredForCharity = 2;
        public bool isRobbed = false;
        public bool isHelped = false;
    }

    /// <summary>
    /// Prompt #346: NPC Encounter: The Desperate Family.
    /// Starving family (mother, father, child). Player can trade, rob them (they fight weakly),
    /// or give them food for a massive Hope morale buff (+35).
    /// </summary>
    public class NPC_DesperateFamily
    {
        private DesperateFamilyState _state = new DesperateFamilyState();

        public event Action<DesperateFamilyState, float> OnCharityGiven;
        public event Action<DesperateFamilyState> OnRobbed;

        public DesperateFamilyState State => _state;

        public float GiveFoodCharity(ref int playerFoodCount)
        {
            if (playerFoodCount >= _state.foodRequiredForCharity && !_state.isHelped)
            {
                playerFoodCount -= _state.foodRequiredForCharity;
                _state.isHelped = true;
                _state.isStarving = false;

                OnCharityGiven?.Invoke(_state, _state.charityHopeBuff);
                return _state.charityHopeBuff;
            }
            return 0f;
        }

        public List<string> RobFamily()
        {
            if (_state.isRobbed) return new List<string>();
            _state.isRobbed = true;

            OnRobbed?.Invoke(_state);
            return new List<string> { "meager_scraps", "tattered_blanket" };
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public DesperateFamilyState CaptureState() => _state;

        public void RestoreState(DesperateFamilyState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
