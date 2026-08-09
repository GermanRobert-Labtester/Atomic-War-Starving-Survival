using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class FissureState
    {
        public string event_id = "world_event_fissure";
        public bool is_triggered = false;
        public int trigger_day = 0;
        public List<string> destroyed_connections = new List<string>();
        public bool map_split = false;
        public bool aircraft_required = true;
        public List<string> severed_nodes = new List<string>();
    }

    /// <summary>
    /// Prompt #856: Tectonic Fissure — Earthquake splits the Expedition Map
    /// in half. Giant glowing crack. All bridges/roads crossing destroyed
    /// permanently. Cut off from half the world unless aircraft available.
    /// Triggers on a random day after Day 60.
    /// </summary>
    public sealed class WorldEvent_Fissure
    {
        private FissureState _state;

        private const int EarliestTriggerDay = 60;

        public event Action OnEarthquake;
        public event Action<string> OnFissureFormed;                    // fissure_id (== event_id)
        public event Action<string, string> OnConnectionDestroyed;      // from_node, to_node
        public event Action OnMapSplit;
        public event Action<string[]> OnNodesSevered;                   // isolated_nodes

        public string EventId => _state.event_id;

        public WorldEvent_Fissure()
        {
            _state = new FissureState();
        }

        /// <summary>
        /// Triggers the fissure event on the given day. Must be after Day 60.
        /// Permanently destroys all connections crossing the fissure line.
        /// </summary>
        public void Trigger(int day)
        {
            if (_state.is_triggered)
            {
                Debug.LogWarning("[WorldEvent_Fissure] Fissure already triggered.");
                return;
            }

            if (day < EarliestTriggerDay)
            {
                Debug.LogWarning($"[WorldEvent_Fissure] Cannot trigger before day {EarliestTriggerDay}. " +
                                 $"Requested day: {day}.");
                return;
            }

            _state.is_triggered = true;
            _state.trigger_day = day;
            _state.map_split = true;

            OnEarthquake?.Invoke();
            OnFissureFormed?.Invoke(_state.event_id);
            OnMapSplit?.Invoke();

            GameLog.Log($"[WorldEvent_Fissure] Tectonic fissure triggered on Day {day}. Map split.");
        }

        /// <summary>
        /// Registers a connection (from_node → to_node) as destroyed by the
        /// fissure. Call this for each path crossing the crack.
        /// </summary>
        public void DestroyConnection(string from_node, string to_node)
        {
            if (string.IsNullOrEmpty(from_node) || string.IsNullOrEmpty(to_node))
            {
                Debug.LogError("[WorldEvent_Fissure] from_node or to_node is null or empty.");
                return;
            }

            string connection_key = $"{from_node}->{to_node}";
            if (!_state.destroyed_connections.Contains(connection_key))
            {
                _state.destroyed_connections.Add(connection_key);
            }

            OnConnectionDestroyed?.Invoke(from_node, to_node);
            GameLog.Log($"[WorldEvent_Fissure] Connection destroyed: {from_node} → {to_node}.");
        }

        /// <summary>
        /// Returns the list of destroyed connection keys (from-&gt;to).
        /// </summary>
        public List<string> GetDestroyedConnections()
        {
            return new List<string>(_state.destroyed_connections);
        }

        /// <summary>
        /// Checks whether the path between two nodes is blocked by the fissure.
        /// </summary>
        public bool IsPathBlocked(string from_node, string to_node)
        {
            if (string.IsNullOrEmpty(from_node) || string.IsNullOrEmpty(to_node))
                return false;

            string key = $"{from_node}->{to_node}";
            string reverse_key = $"{to_node}->{from_node}";
            return _state.destroyed_connections.Contains(key) ||
                   _state.destroyed_connections.Contains(reverse_key);
        }

        /// <summary>
        /// Returns whether the fissure can be crossed. Only aircraft can cross.
        /// </summary>
        public bool CanCross(bool has_aircraft)
        {
            return has_aircraft && _state.aircraft_required;
        }

        /// <summary>
        /// Registers a node as severed (isolated on the far side of the fissure).
        /// </summary>
        public void AddSeveredNode(string node_id)
        {
            if (string.IsNullOrEmpty(node_id))
                return;

            if (!_state.severed_nodes.Contains(node_id))
            {
                _state.severed_nodes.Add(node_id);
            }
        }

        /// <summary>
        /// Returns all nodes severed from the player's side of the map.
        /// Fires the OnNodesSevered event with the full list.
        /// </summary>
        public string[] GetSeveredNodes()
        {
            string[] nodes = _state.severed_nodes.ToArray();
            if (nodes.Length > 0)
            {
                OnNodesSevered?.Invoke(nodes);
            }
            return nodes;
        }

        public FissureState CaptureState()
        {
            return new FissureState
            {
                event_id = _state.event_id,
                is_triggered = _state.is_triggered,
                trigger_day = _state.trigger_day,
                destroyed_connections = new List<string>(_state.destroyed_connections),
                map_split = _state.map_split,
                aircraft_required = _state.aircraft_required,
                severed_nodes = new List<string>(_state.severed_nodes)
            };
        }

        public void RestoreState(FissureState saved)
        {
            _state = saved ?? new FissureState();
        }
    }
}
