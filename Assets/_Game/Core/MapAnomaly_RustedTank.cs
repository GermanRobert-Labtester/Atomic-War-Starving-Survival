using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class RustedTankState
    {
        public string anomalyId = "map_anomaly_rusted_tank";
        public string displayName = "Rusted Tank Barricade";
        public bool isImpassableForVehicles = true;
        public float stormRadiationProtectionRatio = 1.0f; // 100% rad & weather protection
    }

    /// <summary>
    /// Prompt #457: Anomaly: Rusted Tank Barricade.
    /// Acts as an impassable roadblock for vehicles.
    /// If caught in a FalloutStorm near this node, survivors can crawl inside the tank hull for 100% protection.
    /// </summary>
    public class MapAnomaly_RustedTank
    {
        private RustedTankState _state = new RustedTankState();

        public event Action<RustedTankState, string> OnSurvivorsShelteredInsideTank;

        public RustedTankState State => _state;

        public bool ShelterInsideTank(string partyId, bool isFalloutStormActive)
        {
            if (isFalloutStormActive)
            {
                OnSurvivorsShelteredInsideTank?.Invoke(_state, partyId);
                return true;
            }
            return false;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public RustedTankState CaptureState()
        {
            return new RustedTankState
            {
                anomalyId = _state.anomalyId,
                displayName = _state.displayName,
                isImpassableForVehicles = _state.isImpassableForVehicles,
                stormRadiationProtectionRatio = _state.stormRadiationProtectionRatio,
            };
        }

        public void RestoreState(RustedTankState saved)
        {
            if (saved == null)
            {
                _state = new RustedTankState();
                return;
            }
            _state = new RustedTankState
            {
                anomalyId = saved.anomalyId,
                displayName = saved.displayName,
                isImpassableForVehicles = saved.isImpassableForVehicles,
                stormRadiationProtectionRatio = saved.stormRadiationProtectionRatio,
            };
        }
    }
}
