using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Encounters
{
    [Serializable]
    public class UrbanSinkholeState
    {
        public string hazardId = "map_hazard_sinkhole_collapse";
        public float collapseChance = 0.10f;
        public bool dropsToSubway = true;
    }

    /// <summary>DEMOTE-MapHazard-batch — dormant ghost. Re-promote with Boot+Save+host.</summary>
    public class MapHazard_SinkholeCollapse
    {
        public event Action<string, string> OnCollapseTriggered; // survivorId, subwayNodeId
        public event Action<string> OnSafePassage; // survivorId

        private UrbanSinkholeState _state;

        public MapHazard_SinkholeCollapse()
        {
            _state = new UrbanSinkholeState();
        }

        public MapHazard_SinkholeCollapse(UrbanSinkholeState state)
        {
            _state = state ?? new UrbanSinkholeState();
        }

        public UrbanSinkholeState State => _state;

        public bool WalkOver(string survivorId, string currentNodeId, Random rng)
        {
            if (rng == null)
                throw new ArgumentNullException(nameof(rng));

            float roll = (float)rng.NextDouble();
            if (roll < _state.collapseChance)
            {
                string subwayNodeId = GetDropNodeId(currentNodeId);
                OnCollapseTriggered?.Invoke(survivorId, subwayNodeId);
                return true;
            }

            OnSafePassage?.Invoke(survivorId);
            return false;
        }

        public string GetDropNodeId(string currentNodeId)
        {
            return "subway_below_" + currentNodeId;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public UrbanSinkholeState CaptureState()
        {
            return new UrbanSinkholeState
            {
                hazardId = string.IsNullOrEmpty(_state.hazardId) ? "map_hazard_sinkhole_collapse" : _state.hazardId,
                collapseChance = _state.collapseChance,
                dropsToSubway = _state.dropsToSubway
            };
        }

        public void RestoreState(UrbanSinkholeState saved)
        {
            _state = saved != null
                ? new UrbanSinkholeState
                {
                    hazardId = string.IsNullOrEmpty(saved.hazardId) ? "map_hazard_sinkhole_collapse" : saved.hazardId,
                    collapseChance = saved.collapseChance,
                    dropsToSubway = saved.dropsToSubway
                }
                : new UrbanSinkholeState();
        }
    }
}
