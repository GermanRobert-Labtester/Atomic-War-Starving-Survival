using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class ArmoredTruckState
    {
        public string vehicleId = "vehicle_armored_truck";
        public string displayName = "The Beast (Armored Truck)";
        public float encumbranceCapacityKg = 500f;
        public bool isImmuneToBanditAmbush = true;
        public float fuelConsumptionMultiplier = 3.0f; // Catastrophic fuel burn
        public float noiseOutputPercentage = 1.0f;     // 100% noise
    }

    /// <summary>
    /// Prompt #381: Vehicle Type: The Beast (Armored Truck).
    /// Massive 500kg carry capacity, immune to Bandit ambushes.
    /// Consumes fuel at 3x rate and generates 100% Noise, attracting Military/Terrorist attention.
    /// </summary>
    public class Vehicle_ArmoredTruck
    {
        private ArmoredTruckState _state = new ArmoredTruckState();

        public event Action<ArmoredTruckState, string> OnMilitaryTerroristAttracted;

        public ArmoredTruckState State => _state;

        public float CalculateFuelBurn(float baseFuelNeeded)
        {
            return baseFuelNeeded * _state.fuelConsumptionMultiplier;
        }

        public void EmitDriveNoise(string currentRouteId)
        {
            OnMilitaryTerroristAttracted?.Invoke(_state, currentRouteId);
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public ArmoredTruckState CaptureState() => _state;

        public void RestoreState(ArmoredTruckState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
