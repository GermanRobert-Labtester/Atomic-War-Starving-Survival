using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class MobileCampState
    {
        public bool isCampingActive = false;
        public string vehicleType; // Van or Truck
        public float fatigueRestoredPerHour = 12f;
        public float nightAmbushChance = 0.35f;
    }

    /// <summary>
    /// Prompt #384: System: The Mobile Hatch (Car Camping).
    /// Allows survivors in a Van or Truck to execute "Camp" action on the map to rest mid-travel,
    /// but exposes them to high NightAmbush risk.
    /// </summary>
    public class MobileCampSystem
    {
        private MobileCampState _state = new MobileCampState();

        public event Action<MobileCampState, float> OnCampRestTicked;
        public event Action<MobileCampState> OnNightAmbushTriggered;

        public MobileCampState State => _state;

        public float StartCamping(string vehicleType, float hoursToCamp, bool isNight, System.Random rng)
        {
            if (vehicleType != "Van" && vehicleType != "Truck") return 0f;

            _state.isCampingActive = true;
            _state.vehicleType = vehicleType;

            float fatigueRestored = hoursToCamp * _state.fatigueRestoredPerHour;
            OnCampRestTicked?.Invoke(_state, fatigueRestored);

            if (isNight && rng.NextDouble() < _state.nightAmbushChance)
            {
                OnNightAmbushTriggered?.Invoke(_state);
            }

            _state.isCampingActive = false;
            return fatigueRestored;
        }
    }
}
