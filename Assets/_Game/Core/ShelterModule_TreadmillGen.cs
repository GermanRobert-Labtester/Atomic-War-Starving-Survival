using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class TreadmillGenState
    {
        public string moduleId = "shelter_module_treadmill_gen";
        public string displayName = "The Treadmill / Bicycle Gen";
        public bool isBuilt = false;
        public float powerOutputWatts = 15f;
        public float fatigueDrainPerHour = 25f;
        public float calorieDrainPerHour = 300f;
        public float strengthXPGainPerHour = 50f;
    }

    /// <summary>
    /// Prompt #443: Module: The Treadmill / Bicycle Gen.
    /// Generates 15W Power when actively manned by a survivor.
    /// Drains Fatigue and Calories heavily, but passively increases Strength and MaxStamina stats over time.
    /// </summary>
    public class ShelterModule_TreadmillGen
    {
        private TreadmillGenState _state = new TreadmillGenState();

        public event Action<TreadmillGenState, string, float> OnTreadmillMannedPowerGenerated;

        public TreadmillGenState State => _state;

        public float ManTreadmill(string survivorId, float hours, ref float survivorFatigue, ref float survivorCalories, ref float survivorStrengthXP)
        {
            if (!_state.isBuilt) return 0f;

            survivorFatigue = Mathf.Min(100f, survivorFatigue + (_state.fatigueDrainPerHour * hours));
            survivorCalories = Mathf.Max(0f, survivorCalories - (_state.calorieDrainPerHour * hours));
            survivorStrengthXP += _state.strengthXPGainPerHour * hours;

            float powerGenerated = _state.powerOutputWatts;
            OnTreadmillMannedPowerGenerated?.Invoke(_state, survivorId, powerGenerated);
            return powerGenerated;
        }
    
        public TreadmillGenState CaptureState()
        {
            return _state;
        }

        public void RestoreState(TreadmillGenState saved)
        {
            _state = saved ?? new TreadmillGenState();
        }
    }
}

