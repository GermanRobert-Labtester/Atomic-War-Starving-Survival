using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class StrandingState
    {
        public string vehicleId;
        public string strandedLocationId;
        public string survivorId;
        public bool isStranded = false;
        public bool isVehicleAbandoned = false;
    }

    /// <summary>
    /// Prompt #383: System: Vehicle Breakdown & Stranding.
    /// Halts expedition if vehicle engine breaks down mid-travel.
    /// Survivor can abandon the vehicle permanently to walk home, or wait for a rescue survivor with MechanicalParts.
    /// </summary>
    public class VehicleStrandingSystem
    {
        private readonly Dictionary<string, StrandingState> _strandedMap = new Dictionary<string, StrandingState>();

        public event Action<StrandingState> OnVehicleStranded;
        public event Action<StrandingState> OnVehicleRescued;
        public event Action<StrandingState> OnVehicleAbandonedPermanently;

        public IReadOnlyDictionary<string, StrandingState> StrandedMap => _strandedMap;

        public void TriggerBreakdown(string vehicleId, string locationId, string survivorId)
        {
            var state = new StrandingState
            {
                vehicleId = vehicleId,
                strandedLocationId = locationId,
                survivorId = survivorId,
                isStranded = true
            };
            _strandedMap[vehicleId] = state;
            OnVehicleStranded?.Invoke(state);
        }

        public bool AbandonVehicle(string vehicleId)
        {
            if (_strandedMap.TryGetValue(vehicleId, out var state) && state.isStranded)
            {
                state.isVehicleAbandoned = true;
                state.isStranded = false;
                OnVehicleAbandonedPermanently?.Invoke(state);
                return true;
            }
            return false;
        }

        public bool PerformRescueMission(string vehicleId, bool hasMechanicalParts)
        {
            if (_strandedMap.TryGetValue(vehicleId, out var state) && state.isStranded && hasMechanicalParts)
            {
                state.isStranded = false;
                OnVehicleRescued?.Invoke(state);
                return true;
            }
            return false;
        }
    }
}
