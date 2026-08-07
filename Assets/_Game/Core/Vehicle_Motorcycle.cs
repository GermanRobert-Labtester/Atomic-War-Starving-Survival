using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class MotorcycleState
    {
        public string vehicleId = "vehicle_motorcycle";
        public string displayName = "The Scrambler (Motorcycle)";
        public int maxPassengers = 1;
        public float speedMultiplier = 2.5f;
        public float fuelConsumptionMultiplier = 0.3f;
        public bool hasRadiationShielding = false;
        public bool hasWeatherProtection = false;
    }

    /// <summary>
    /// Prompt #382: Vehicle Type: The Scrambler (Motorcycle).
    /// Seats 1 survivor. Fast (2.5x) and extremely fuel-efficient (0.3x).
    /// Zero radiation or weather protection; stray bullets in skirmishes cause severe crash trauma.
    /// </summary>
    public class Vehicle_Motorcycle
    {
        private MotorcycleState _state = new MotorcycleState();

        public event Action<MotorcycleState, float> OnStrayBulletCrashTrauma;

        public MotorcycleState State => _state;

        public float TriggerStrayBulletImpact(float baseCrashDamage)
        {
            float totalDamage = baseCrashDamage * 2.0f; // Severe crash trauma
            OnStrayBulletCrashTrauma?.Invoke(_state, totalDamage);
            return totalDamage;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public MotorcycleState CaptureState() => _state;

        public void RestoreState(MotorcycleState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
