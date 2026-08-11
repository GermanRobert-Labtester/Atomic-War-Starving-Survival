using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Shelter.Modules
{
    [Serializable]
    public class ChoreBoardState
    {
        public string moduleId = "shelter_module_chore_board";
        public string displayName = "The Chore Board (Bulletin)";
        public bool isBuilt = false;
        public float globalUtilityAISpeedBonusRatio = 0.05f; // +5% speed bonus
    }

    /// <summary>
    /// Prompt #447: Module: The Chore Board (Bulletin).
    /// Low-cost bulletin board placed in a high-traffic room.
    /// Increases execution speed of ALL UtilityAI tasks in the shelter by 5% through organizational efficiency.
    /// </summary>
    public class ShelterModule_ChoreBoard
    {
        private ChoreBoardState _state = new ChoreBoardState();

        public event Action<ChoreBoardState, float> OnGlobalChoreEfficiencyBuffApplied;

        public ChoreBoardState State => _state;

        public float GetUtilityAISpeedMultiplier()
        {
            if (!_state.isBuilt) return 1.0f;
            float mult = 1.0f + _state.globalUtilityAISpeedBonusRatio;

            OnGlobalChoreEfficiencyBuffApplied?.Invoke(_state, mult);
            return mult;
        }
    
        public ChoreBoardState CaptureState()
        {
            return _state;
        }

        public void RestoreState(ChoreBoardState saved)
        {
            _state = saved ?? new ChoreBoardState();
        }
    }
}

