using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.World
{
    [Serializable]
    public class TireFireState
    {
        public string anomalyId = "map_anomaly_tire_fire";
        public string displayName = "Burning Tire Fire";
        public int pollutionRadiusNodes = 2;
        public float airQualityPenaltyRatio = 0.50f;
        public float mutatedCreatureAttractionMultiplier = 2.0f;
    }

    /// <summary>
    /// Prompt #450: Anomaly: Burning Tire Fire.
    /// Dump fire that never goes out. Generates infinite toxic smoke, lowering world map AirQuality
    /// in a 2-node radius and drawing mutated creatures to the warmth.
    /// </summary>
    public class MapAnomaly_TireFire
    {
        private TireFireState _state = new TireFireState();

        public event Action<TireFireState, int, float> OnRegionalAirPollutionApplied;

        public TireFireState State => _state;

        public float ApplyRegionalPollution(int nodeDistance)
        {
            if (nodeDistance <= _state.pollutionRadiusNodes)
            {
                float penalty = _state.airQualityPenaltyRatio;
                OnRegionalAirPollutionApplied?.Invoke(_state, nodeDistance, penalty);
                return penalty;
            }
            return 0f;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public TireFireState CaptureState()
        {
            return new TireFireState
            {
                anomalyId = _state.anomalyId,
                displayName = _state.displayName,
                pollutionRadiusNodes = _state.pollutionRadiusNodes,
                airQualityPenaltyRatio = _state.airQualityPenaltyRatio,
                mutatedCreatureAttractionMultiplier = _state.mutatedCreatureAttractionMultiplier,
            };
        }

        public void RestoreState(TireFireState saved)
        {
            if (saved == null)
            {
                _state = new TireFireState();
                return;
            }
            _state = new TireFireState
            {
                anomalyId = saved.anomalyId,
                displayName = saved.displayName,
                pollutionRadiusNodes = saved.pollutionRadiusNodes,
                airQualityPenaltyRatio = saved.airQualityPenaltyRatio,
                mutatedCreatureAttractionMultiplier = saved.mutatedCreatureAttractionMultiplier,
            };
        }
    }
}
