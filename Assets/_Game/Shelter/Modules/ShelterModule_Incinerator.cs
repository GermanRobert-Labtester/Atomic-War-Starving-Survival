using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Shelter.Modules
{
    [Serializable]
    public class IncineratorState
    {
        public string moduleId = "shelter_module_incinerator";
        public string displayName = "The Incinerator";
        public bool isBuilt = false;
        public float heatOutputGenerated = 25f;
        public float hatchVisibilityMax = 1.0f; // Max visibility when burning corpses
    }

    /// <summary>
    /// Prompt #407: Module: The Incinerator.
    /// Safely destroys Waste, Corpses, and SpoiledFood while generating heat for the shelter.
    /// Burning bodies produces heavy smoke that raises HatchVisibility to maximum, attracting Cultists/Bandits.
    /// </summary>
    public class ShelterModule_Incinerator
    {
        private IncineratorState _state = new IncineratorState();

        public event Action<IncineratorState, string, float> OnMaterialIncinerated;
        public event Action<IncineratorState> OnCorpseSmokeHatchVisibilityMaxed;

        public IncineratorState State => _state;

        public bool IncinerateItem(string itemType, ref float hatchVisibility, ref float shelterHeat)
        {
            if (!_state.isBuilt) return false;

            shelterHeat += _state.heatOutputGenerated;
            if (itemType == "corpse")
            {
                hatchVisibility = _state.hatchVisibilityMax;
                OnCorpseSmokeHatchVisibilityMaxed?.Invoke(_state);
            }

            OnMaterialIncinerated?.Invoke(_state, itemType, _state.heatOutputGenerated);
            return true;
        }
    
        public IncineratorState CaptureState()
        {
            return _state;
        }

        public void RestoreState(IncineratorState saved)
        {
            _state = saved ?? new IncineratorState();
        }
    }
}

