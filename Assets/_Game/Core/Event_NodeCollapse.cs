using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class NodeCollapseState
    {
        public string event_id = "event_node_collapse";
        public string node_id;
        public int turns_until_collapse = 10;
        public int turns_remaining = 10;
        public bool is_active = false;
        public bool node_deleted = false;
        public List<string> survivors_inside = new List<string>();
        public List<string> escaped_survivor_ids = new List<string>();
        public List<string> trapped_survivor_ids = new List<string>();
    }

    public sealed class Event_NodeCollapse
    {
        private NodeCollapseState _state;

        public event Action<string> OnCollapseStarted;          // node_id
        public event Action<string, int> OnCountdownTick;       // (node_id, turns_remaining)
        public event Action<string> OnNodeDeleted;              // node_id
        public event Action<string> OnSurvivorTrapped;          // survivor_id

        public string EventId => _state.event_id;
        public bool IsActive => _state.is_active;
        public int TurnsRemaining => _state.turns_remaining;

        public Event_NodeCollapse()
        {
            _state = new NodeCollapseState();
        }

        /// <summary>
        /// Triggers a collapse countdown for the specified node.
        /// Starts the 10-turn timer.
        /// </summary>
        public void TriggerCollapse(string node_id)
        {
            if (string.IsNullOrEmpty(node_id))
            {
                Debug.LogError("[Event_NodeCollapse] node_id is null or empty.");
                return;
            }

            if (_state.is_active)
            {
                Debug.LogWarning("[Event_NodeCollapse] Collapse is already active.");
                return;
            }

            _state.node_id = node_id;
            _state.turns_remaining = _state.turns_until_collapse;
            _state.is_active = true;
            _state.node_deleted = false;
            _state.escaped_survivor_ids.Clear();
            _state.trapped_survivor_ids.Clear();

            OnCollapseStarted?.Invoke(node_id);
            Debug.Log($"[Event_NodeCollapse] Collapse triggered for node '{node_id}'. " +
                      $"{_state.turns_remaining} turns remaining.");
        }

        /// <summary>
        /// Advances the collapse timer by one turn.
        /// Returns turns remaining. At 0, the node is permanently deleted
        /// and anyone still inside is trapped (killed).
        /// </summary>
        public int Tick()
        {
            if (!_state.is_active)
            {
                Debug.LogWarning("[Event_NodeCollapse] Collapse is not active.");
                return -1;
            }

            _state.turns_remaining--;
            OnCountdownTick?.Invoke(_state.node_id, _state.turns_remaining);

            if (_state.turns_remaining <= 0)
            {
                _state.is_active = false;
                _state.node_deleted = true;

                // Anyone still inside is trapped
                for (int i = 0; i < _state.survivors_inside.Count; i++)
                {
                    string survivor_id = _state.survivors_inside[i];
                    if (!_state.escaped_survivor_ids.Contains(survivor_id))
                    {
                        if (!_state.trapped_survivor_ids.Contains(survivor_id))
                        {
                            _state.trapped_survivor_ids.Add(survivor_id);
                        }

                        OnSurvivorTrapped?.Invoke(survivor_id);
                    }
                }

                OnNodeDeleted?.Invoke(_state.node_id);
                Debug.Log($"[Event_NodeCollapse] Node '{_state.node_id}' collapsed. " +
                          $"{_state.trapped_survivor_ids.Count} survivor(s) trapped.");
            }

            return _state.turns_remaining;
        }

        /// <summary>
        /// A survivor escapes the collapsing node before it collapses.
        /// </summary>
        public void Escape(string survivor_id)
        {
            if (string.IsNullOrEmpty(survivor_id))
            {
                Debug.LogError("[Event_NodeCollapse] survivor_id is null or empty.");
                return;
            }

            if (!_state.is_active)
            {
                Debug.LogWarning("[Event_NodeCollapse] Collapse is not active — cannot escape.");
                return;
            }

            if (!_state.escaped_survivor_ids.Contains(survivor_id))
            {
                _state.escaped_survivor_ids.Add(survivor_id);
            }

            Debug.Log($"[Event_NodeCollapse] Survivor '{survivor_id}' escaped node '{_state.node_id}'.");
        }

        /// <summary>
        /// Registers a survivor as being inside the collapsing node.
        /// </summary>
        public void RegisterSurvivorInside(string survivor_id)
        {
            if (string.IsNullOrEmpty(survivor_id))
                return;

            if (!_state.survivors_inside.Contains(survivor_id))
            {
                _state.survivors_inside.Add(survivor_id);
            }
        }

        public NodeCollapseState CaptureState()
        {
            return new NodeCollapseState
            {
                event_id = _state.event_id,
                node_id = _state.node_id,
                turns_until_collapse = _state.turns_until_collapse,
                turns_remaining = _state.turns_remaining,
                is_active = _state.is_active,
                node_deleted = _state.node_deleted,
                survivors_inside = new List<string>(_state.survivors_inside),
                escaped_survivor_ids = new List<string>(_state.escaped_survivor_ids),
                trapped_survivor_ids = new List<string>(_state.trapped_survivor_ids)
            };
        }

        public void RestoreState(NodeCollapseState saved)
        {
            _state = saved ?? new NodeCollapseState();
        }
    }
}
