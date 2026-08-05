using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class MotionSensorState
    {
        public string moduleId = "shelter_module_motion_sensor";
        public string displayName = "Motion Sensors";
        public bool isBuilt = false;
        public bool hasPower = true;
        public int detectionRadiusNodes = 2;
        public List<string> activeDetectedThreats = new List<string>();
    }

    /// <summary>
    /// Prompt #400: Module: Motion Sensors.
    /// External sensors requiring ElectronicScrap. Gives visual UI pings on the map
    /// when Factions or Hordes move within 2 nodes of the shelter, preventing surprise raids.
    /// </summary>
    public class ShelterModule_MotionSensor
    {
        private MotionSensorState _state = new MotionSensorState();

        public event Action<MotionSensorState, string, int> OnThreatPingedOnMap;

        public MotionSensorState State => _state;

        public void DetectThreatsWithinRadius(string threatId, int distanceInNodes)
        {
            if (!_state.isBuilt || !_state.hasPower) return;

            if (distanceInNodes <= _state.detectionRadiusNodes)
            {
                if (!_state.activeDetectedThreats.Contains(threatId))
                {
                    _state.activeDetectedThreats.Add(threatId);
                }
                OnThreatPingedOnMap?.Invoke(_state, threatId, distanceInNodes);
            }
        }
    }
}
