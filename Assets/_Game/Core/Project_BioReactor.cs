using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class BioReactorState
    {
        public string projectId = "project_bio_reactor";
        public bool isBuilt = false;
        public int constructionDays = 25;
        public int daysSpent = 0;
        public float powerOutput = 200f;
        public float moraleDisgustDebuff = -15f;
        public int biomassStored = 0;
        public int maxBiomassCapacity = 100;
    }

    /// <summary>
    /// Prompt #585: Project: Bio-Reactor.
    /// Consumes Biomass (spoiled food, feral dogs, corpses) to produce Power.
    /// Permanent Disgust morale debuff while active.
    /// </summary>
    public class Project_BioReactor
    {
        private BioReactorState _state = new BioReactorState();

        public event Action<BioReactorState> OnBioReactorBuilt;
        public event Action<BioReactorState, string, int> OnBiomassConsumed;
        public event Action<BioReactorState, string> OnCorpseFueled;
        public event Action<BioReactorState, float> OnPowerGenerated;

        public BioReactorState State => _state;

        public void StartConstruction()
        {
            if (_state.isBuilt) return;
            _state.daysSpent = 0;
        }

        public void TickDay()
        {
            if (_state.isBuilt) return;

            _state.daysSpent++;
            if (_state.daysSpent >= _state.constructionDays)
            {
                _state.isBuilt = true;
                OnBioReactorBuilt?.Invoke(_state);
            }
        }

        public bool AddBiomass(string materialType, int amount)
        {
            if (!_state.isBuilt) return false;

            int unitValue = GetBiomassUnitValue(materialType);
            if (unitValue <= 0) return false;

            int spaceAvailable = _state.maxBiomassCapacity - _state.biomassStored;
            int biomassUnits = amount * unitValue;
            int toAdd = Mathf.Min(biomassUnits, spaceAvailable);
            if (toAdd <= 0) return false;

            _state.biomassStored += toAdd;

            if (materialType == "corpse")
            {
                OnCorpseFueled?.Invoke(_state, materialType);
            }

            OnBiomassConsumed?.Invoke(_state, materialType, toAdd);
            return true;
        }

        public float GetPowerOutput()
        {
            if (!_state.isBuilt) return 0f;
            if (_state.biomassStored <= 0) return 0f;
            return _state.powerOutput;
        }

        public float GetMoraleDebuff()
        {
            if (!_state.isBuilt) return 0f;
            return _state.moraleDisgustDebuff;
        }

        private static int GetBiomassUnitValue(string materialType)
        {
            switch (materialType)
            {
                case "spoiled_food": return 10;
                case "feral_dog": return 25;
                case "corpse": return 50;
                default: return 0;
            }
        }
    }
}
