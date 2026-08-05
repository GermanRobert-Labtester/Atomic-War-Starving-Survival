using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class DeconShowerState
    {
        public string moduleId = "shelter_module_decon_shower";
        public string displayName = "Decontamination Showers";
        public bool isBuilt = false;
        public int waterCostPerUse = 10;
    }

    /// <summary>
    /// Prompt #406: Module: Decontamination Showers.
    /// Upgrades airlock decontamination, instantly clearing 100% radiation contamination
    /// from returning scavengers at the cost of 10 CleanWater per use.
    /// </summary>
    public class ShelterModule_DeconShower
    {
        private DeconShowerState _state = new DeconShowerState();

        public event Action<DeconShowerState, string> OnInstantDecontaminationExecuted;

        public DeconShowerState State => _state;

        public bool UseDeconShower(string survivorId, ref float survivorContamination, ref int cleanWaterStorage)
        {
            if (!_state.isBuilt || cleanWaterStorage < _state.waterCostPerUse)
                return false;

            cleanWaterStorage -= _state.waterCostPerUse;
            survivorContamination = 0f;

            OnInstantDecontaminationExecuted?.Invoke(_state, survivorId);
            return true;
        }
    }
}
