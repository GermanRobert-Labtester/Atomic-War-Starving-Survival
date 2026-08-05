using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class BioFogState
    {
        public string weatherId = "weather_bio_fog";
        public string displayName = "Bioluminescent Fog";
        public bool isActive = false;
        public bool isStealthPossible = false; // Stealth impossible
        public float encounterRateMultiplier = 2.0f; // Encounter rates double
        public float radiationAnxietyPerDelta = 5f;
    }

    /// <summary>
    /// Prompt #374: System: Bioluminescent Fog.
    /// Glowing green irradiated fog. Zero visibility, high ambient light. Nullifies stealth, doubles encounter rates,
    /// and creeps into the airlock, causing RadiationAnxiety in survivors.
    /// </summary>
    public class Weather_BioFog
    {
        private BioFogState _state = new BioFogState();

        public event Action<BioFogState> OnBioFogStarted;
        public event Action<BioFogState, float> OnRadiationAnxietyInflicted;

        public BioFogState State => _state;

        public void ActivateBioFog()
        {
            _state.isActive = true;
            _state.isStealthPossible = false;
            _state.encounterRateMultiplier = 2.0f;

            OnBioFogStarted?.Invoke(_state);
        }

        public float CreepIntoAirlock(float deltaHours)
        {
            if (!_state.isActive) return 0f;
            float anxiety = deltaHours * _state.radiationAnxietyPerDelta;
            OnRadiationAnxietyInflicted?.Invoke(_state, anxiety);
            return anxiety;
        }

        public void DeactivateBioFog()
        {
            _state.isActive = false;
            _state.isStealthPossible = true;
            _state.encounterRateMultiplier = 1.0f;
        }
    }
}
