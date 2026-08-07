using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class HotShowerState
    {
        public string moduleId = "shelter_module_hot_shower";
        public string displayName = "Makeshift Hot Shower";
        public bool isBuilt = false;
        public int waterCostPerUse = 5;
        public float moraleBoostAmount = 50f; // Massive morale burst
    }

    /// <summary>
    /// Prompt #448: Module: Makeshift Hot Shower.
    /// Consumes 5 CleanWater and requires active shelter Heat.
    /// Restores Hygiene to 100% and provides a massive +50 Morale burst for broken spirits.
    /// </summary>
    public class ShelterModule_HotShower
    {
        private HotShowerState _state = new HotShowerState();

        public event Action<HotShowerState, string, float> OnHotShowerTakenMoraleBoosted;

        public HotShowerState State => _state;

        public bool TakeHotShower(string survivorId, ref int cleanWaterStorage, bool isHeatAvailable, ref float survivorHygiene, ref float survivorMorale)
        {
            if (!_state.isBuilt || !isHeatAvailable || cleanWaterStorage < _state.waterCostPerUse)
                return false;

            cleanWaterStorage -= _state.waterCostPerUse;
            survivorHygiene = 100f;
            survivorMorale = Mathf.Min(100f, survivorMorale + _state.moraleBoostAmount);

            OnHotShowerTakenMoraleBoosted?.Invoke(_state, survivorId, _state.moraleBoostAmount);
            return true;
        }
    
        public HotShowerState CaptureState()
        {
            return _state;
        }

        public void RestoreState(HotShowerState saved)
        {
            _state = saved ?? new HotShowerState();
        }
    }
}

