using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Shelter.Modules
{
    [Serializable]
    public class HammockModuleState
    {
        public string moduleId = "shelter_module_hammock";
        public string displayName = "The Hammock";
        public bool isBuilt = false;
        public float sleepQualityMultiplier = 0.60f; // Low sleep quality
        public bool consumesFloorSpace = false;       // Stacked vertically
    }

    /// <summary>
    /// Prompt #439: Module: The Hammock.
    /// Early-game bed providing low sleep quality, but uses vertical space and consumes 0 grid floor space.
    /// </summary>
    public class ShelterModule_Hammock
    {
        private HammockModuleState _state = new HammockModuleState();

        public event Action<HammockModuleState, string> OnHammockSleptIn;

        public HammockModuleState State => _state;

        public float RestInHammock(string survivorId, float baseRestRate)
        {
            if (!_state.isBuilt) return 0f;

            float qualityRest = baseRestRate * _state.sleepQualityMultiplier;
            OnHammockSleptIn?.Invoke(_state, survivorId);
            return qualityRest;
        }
    
        public HammockModuleState CaptureState()
        {
            return _state;
        }

        public void RestoreState(HammockModuleState saved)
        {
            _state = saved ?? new HammockModuleState();
        }
    }
}

