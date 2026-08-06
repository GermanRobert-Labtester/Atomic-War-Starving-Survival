using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class MapRotState
    {
        public string eventId = "ui_event_map_rot";
        public int daysBeforeRot = 30;
        public bool isActive = false;
    }

    /// <summary>
    /// Prompt #756: Map Rot.
    /// As FalloutStorms rage, Map PNG texture burns/fades. Nodes not visited
    /// in 30 days become obscured. Forces re-scouting.
    /// </summary>
    public class UIEvent_MapRot
    {
        private MapRotState _state = new MapRotState();
        private readonly Dictionary<string, int> _lastVisitedDay = new Dictionary<string, int>();
        private readonly HashSet<string> _rottedNodes = new HashSet<string>();

        public event Action<string> OnNodeRotted;
        public event Action<string> OnNodeRestored;

        public MapRotState State => _state;

        public void TickDay(int currentDay, bool falloutStormActive)
        {
            if (!falloutStormActive)
                return;

            _state.isActive = true;

            foreach (var kvp in _lastVisitedDay)
            {
                string nodeId = kvp.Key;
                int lastVisit = kvp.Value;

                if (currentDay - lastVisit >= _state.daysBeforeRot)
                {
                    if (!_rottedNodes.Contains(nodeId))
                    {
                        _rottedNodes.Add(nodeId);
                        OnNodeRotted?.Invoke(nodeId);
                    }
                }
            }
        }

        public void VisitNode(string nodeId, int currentDay)
        {
            _lastVisitedDay[nodeId] = currentDay;

            if (_rottedNodes.Remove(nodeId))
            {
                OnNodeRestored?.Invoke(nodeId);
            }
        }

        public bool IsRotted(string nodeId) => _rottedNodes.Contains(nodeId);

        /// <summary>
        /// Captures serializable snapshot of visit timestamps and rotted state.
        /// </summary>
        public MapRotSaveData CaptureState()
        {
            var data = new MapRotSaveData();
            foreach (var kvp in _lastVisitedDay)
            {
                data.lastVisitedDays[kvp.Key] = kvp.Value;
            }
            data.rottedNodeIds.AddRange(_rottedNodes);
            return data;
        }

        /// <summary>
        /// Restores from a previously captured save snapshot.
        /// </summary>
        public void RestoreState(MapRotSaveData data)
        {
            _lastVisitedDay.Clear();
            _rottedNodes.Clear();

            if (data == null)
                return;

            foreach (var kvp in data.lastVisitedDays)
            {
                _lastVisitedDay[kvp.Key] = kvp.Value;
            }
            foreach (string id in data.rottedNodeIds)
            {
                _rottedNodes.Add(id);
            }
        }
    }

    [Serializable]
    public class MapRotSaveData
    {
        public Dictionary<string, int> lastVisitedDays = new Dictionary<string, int>();
        public List<string> rottedNodeIds = new List<string>();
    }
}
