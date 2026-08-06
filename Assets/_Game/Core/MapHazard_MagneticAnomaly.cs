using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class MagneticAnomalyState
    {
        public string hazard_id = "map_hazard_magnetic_anomaly";
        public bool scrambles_electronics = true;
        public bool fog_covers_entire_map = true;
        public List<string> affected_nodes = new List<string>();
    }

    public sealed class MapHazard_MagneticAnomaly
    {
        private MagneticAnomalyState _state;

        public event Action OnFogOfWarExpanded;
        public event Action OnCompassScrambled;

        public string HazardId => _state.hazard_id;

        public MapHazard_MagneticAnomaly()
        {
            _state = new MagneticAnomalyState();
        }

        public void EnterNode(string node_id)
        {
            if (string.IsNullOrEmpty(node_id))
            {
                Debug.LogError("[MapHazard_MagneticAnomaly] node_id is null or empty.");
                return;
            }

            if (!_state.affected_nodes.Contains(node_id))
            {
                _state.affected_nodes.Add(node_id);
            }

            if (_state.fog_covers_entire_map)
            {
                OnFogOfWarExpanded?.Invoke();
                Debug.Log("[MapHazard_MagneticAnomaly] Fog of war expanded to cover entire map.");
            }

            if (_state.scrambles_electronics)
            {
                OnCompassScrambled?.Invoke();
                Debug.Log("[MapHazard_MagneticAnomaly] Compass scrambled.");
            }
        }

        public bool IsActive(string node_id)
        {
            if (string.IsNullOrEmpty(node_id))
                return false;

            return _state.affected_nodes.Contains(node_id);
        }

        public void ExitNode(string node_id)
        {
            if (string.IsNullOrEmpty(node_id))
            {
                Debug.LogError("[MapHazard_MagneticAnomaly] node_id is null or empty.");
                return;
            }

            if (_state.affected_nodes.Contains(node_id))
            {
                _state.affected_nodes.Remove(node_id);
            }

            Debug.Log($"[MapHazard_MagneticAnomaly] Exited node '{node_id}'. Previously explored nodes now require re-scouting.");
        }

        public MagneticAnomalyState CaptureState()
        {
            return new MagneticAnomalyState
            {
                hazard_id = _state.hazard_id,
                scrambles_electronics = _state.scrambles_electronics,
                fog_covers_entire_map = _state.fog_covers_entire_map,
                affected_nodes = new List<string>(_state.affected_nodes)
            };
        }

        public void RestoreState(MagneticAnomalyState saved)
        {
            _state = saved ?? new MagneticAnomalyState();
        }
    }
}
