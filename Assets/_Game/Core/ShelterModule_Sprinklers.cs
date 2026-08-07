using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class SprinklersState
    {
        public string moduleId = "shelter_module_sprinklers";
        public string displayName = "Fire Suppression Sprinklers";
        public bool isBuilt = false;
        public int waterDumpAmount = 50; // Dumps 50 CleanWater
    }

    /// <summary>
    /// Prompt #401: Module: Fire Suppression Sprinklers.
    /// Connected to WaterStorage. Automatically triggers when a FireEntity spawns,
    /// instantly killing the fire but permanently dumping 50 CleanWater.
    /// </summary>
    public class ShelterModule_Sprinklers
    {
        private SprinklersState _state = new SprinklersState();

        public event Action<SprinklersState, int> OnFireExtinguishedWaterDumped;

        public SprinklersState State => _state;

        public bool TriggerFireSuppression(ref int cleanWaterStorage)
        {
            if (!_state.isBuilt || cleanWaterStorage <= 0)
                return false;

            int waterDrained = Mathf.Min(cleanWaterStorage, _state.waterDumpAmount);
            cleanWaterStorage -= waterDrained;

            OnFireExtinguishedWaterDumped?.Invoke(_state, waterDrained);
            return true;
        }
    
        public SprinklersState CaptureState()
        {
            return _state;
        }

        public void RestoreState(SprinklersState saved)
        {
            _state = saved ?? new SprinklersState();
        }
    }
}

