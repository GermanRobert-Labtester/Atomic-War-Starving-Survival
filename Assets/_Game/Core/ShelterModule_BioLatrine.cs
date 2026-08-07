using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class BioLatrineState
    {
        public string moduleId = "shelter_module_bio_latrine";
        public string displayName = "Composting Bio-Latrine";
        public bool isBuilt = false;
        public bool negatesDiseaseSpread = true;
        public int daysSinceLastFertilizer = 0;
        public int highYieldFertilizerOutput = 10;
    }

    /// <summary>
    /// Prompt #445: Module: Composting Bio-Latrine.
    /// Upgrades the bucket toilet using Sawdust or Ash.
    /// Completely negates disease spread in the shelter and produces high-yield Fertilizer every 10 days.
    /// </summary>
    public class ShelterModule_BioLatrine
    {
        private BioLatrineState _state = new BioLatrineState();

        public event Action<BioLatrineState, int> OnHighYieldFertilizerProduced;

        public BioLatrineState State => _state;

        public int TickDaily(bool hasSawdustOrAsh)
        {
            if (!_state.isBuilt || !hasSawdustOrAsh) return 0;

            _state.daysSinceLastFertilizer++;
            if (_state.daysSinceLastFertilizer >= 10)
            {
                _state.daysSinceLastFertilizer = 0;
                int output = _state.highYieldFertilizerOutput;
                OnHighYieldFertilizerProduced?.Invoke(_state, output);
                return output;
            }
            return 0;
        }
    
        public BioLatrineState CaptureState()
        {
            return _state;
        }

        public void RestoreState(BioLatrineState saved)
        {
            _state = saved ?? new BioLatrineState();
        }
    }
}

