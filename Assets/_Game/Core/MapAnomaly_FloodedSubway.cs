using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class FloodedSubwayState
    {
        public string anomalyId = "map_anomaly_flooded_subway";
        public string displayName = "Flooded Subway Tunnel";
        public bool isShortcutActive = true;
        public string hypothermiaAffliction = "hypothermia_affliction";
    }

    /// <summary>
    /// Prompt #453: Anomaly: Flooded Subway Tunnel.
    /// City map shortcut requiring the "Wade" action.
    /// Drops Warmth to 0, guarantees Hypothermia, and ruins equipped Hazmat suits.
    /// </summary>
    public class MapAnomaly_FloodedSubway
    {
        private FloodedSubwayState _state = new FloodedSubwayState();

        public event Action<FloodedSubwayState, string> OnSubwayWadedHypothermiaInflicted;

        public FloodedSubwayState State => _state;

        public bool WadeThroughSubway(string survivorId, ref float survivorWarmth, ref float hazmatSuitDurability, out string contractedHypothermia)
        {
            survivorWarmth = 0f;
            hazmatSuitDurability = 0f;
            contractedHypothermia = _state.hypothermiaAffliction;

            OnSubwayWadedHypothermiaInflicted?.Invoke(_state, survivorId);
            return true;
        }
    }
}
