using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class HandCrankState
    {
        public string moduleId = "shelter_module_hand_crank";
        public string displayName = "Hand-Crank Dynamo";
        public bool isBuilt = false;
        public float powerOutputWatts = 5f; // 5 Watts
        public float hoursToMaxFatigue = 2.0f;
        public string blisterAffliction = "hand_blisters";
    }

    /// <summary>
    /// Prompt #408: Module: Hand-Crank Dynamo.
    /// Desperate manual power source producing 5 Watts (enough for a light or radio).
    /// Requires physical labor that maxes out survivor Fatigue in 2 hours and causes Blisters.
    /// </summary>
    public class ShelterModule_HandCrank
    {
        private HandCrankState _state = new HandCrankState();

        public event Action<HandCrankState, string, float> OnPowerCrankedFatigued;

        public HandCrankState State => _state;

        public float CrankDynamo(string survivorId, float durationHours, ref float survivorFatigue, out string contractedBlisters)
        {
            contractedBlisters = null;
            if (!_state.isBuilt) return 0f;

            survivorFatigue = 100f; // Max out fatigue after 2 hrs of cranking
            contractedBlisters = _state.blisterAffliction;

            float wattsGenerated = _state.powerOutputWatts;
            OnPowerCrankedFatigued?.Invoke(_state, survivorId, wattsGenerated);
            return wattsGenerated;
        }
    
        public HandCrankState CaptureState()
        {
            return _state;
        }

        public void RestoreState(HandCrankState saved)
        {
            _state = saved ?? new HandCrankState();
        }
    }
}

