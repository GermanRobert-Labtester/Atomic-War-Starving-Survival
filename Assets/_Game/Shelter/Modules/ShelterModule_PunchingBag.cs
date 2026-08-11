using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Shelter.Modules
{
    [Serializable]
    public class PunchingBagState
    {
        public string moduleId = "shelter_module_punching_bag";
        public string displayName = "The Punching Bag";
        public bool isBuilt = false;
        public float anxietyReductionAmount = 45f;
    }

    /// <summary>
    /// Prompt #444: Module: The Punching Bag.
    /// Constructed with Sand and Leather/Canvas. Survivors with high Anxiety or near a ViolentBreak
    /// auto-path here to vent anger safely without assaulting other crew members.
    /// </summary>
    public class ShelterModule_PunchingBag
    {
        private PunchingBagState _state = new PunchingBagState();

        public event Action<PunchingBagState, string, float> OnAngerVentedSafely;

        public PunchingBagState State => _state;

        public bool VentAngerOnBag(string survivorId, ref float survivorAnxiety)
        {
            if (!_state.isBuilt) return false;

            survivorAnxiety = Mathf.Max(0f, survivorAnxiety - _state.anxietyReductionAmount);
            OnAngerVentedSafely?.Invoke(_state, survivorId, _state.anxietyReductionAmount);
            return true;
        }
    
        public PunchingBagState CaptureState()
        {
            return _state;
        }

        public void RestoreState(PunchingBagState saved)
        {
            _state = saved ?? new PunchingBagState();
        }
    }
}

