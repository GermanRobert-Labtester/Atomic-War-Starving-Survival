using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class AutomatedArmoryState
    {
        public string node_id = "node_automated_armory";
        public float turret_disabled_seconds = 30f;
        public bool is_disabled = false;
        public float disable_timer_remaining = 0f;
        public string last_disable_method;
        public List<string> survivors_shot = new List<string>();
    }

    public sealed class Node_AutomatedArmory
    {
        private AutomatedArmoryState _state;

        public event Action OnTurretsDisabled;
        public event Action OnTurretsReactivated;
        public event Action<string> OnSurvivorShot;    // survivor_id — caught outside cover

        public string NodeId => _state.node_id;
        public bool IsDisabled => _state.is_disabled;

        public Node_AutomatedArmory()
        {
            _state = new AutomatedArmoryState();
        }

        /// <summary>
        /// Disables turrets using "emp" or "hack" method.
        /// Opens a timed window (default 30 seconds) for safe movement.
        /// </summary>
        public void DisableTurrets(string method)
        {
            if (string.IsNullOrEmpty(method))
            {
                Debug.LogError("[Node_AutomatedArmory] method is null or empty.");
                return;
            }

            string method_lower = method.ToLowerInvariant();
            if (method_lower != "emp" && method_lower != "hack")
            {
                Debug.LogError($"[Node_AutomatedArmory] Invalid method '{method}'. Use 'emp' or 'hack'.");
                return;
            }

            _state.is_disabled = true;
            _state.disable_timer_remaining = _state.turret_disabled_seconds;
            _state.last_disable_method = method_lower;

            OnTurretsDisabled?.Invoke();
            Debug.Log($"[Node_AutomatedArmory] Turrets disabled via {method_lower} " +
                      $"for {_state.turret_disabled_seconds}s.");
        }

        /// <summary>
        /// Ticks the disable timer. Call each game-second to count down.
        /// When the timer expires, turrets reactivate.
        /// </summary>
        public void TickDisableTimer(float delta_seconds)
        {
            if (!_state.is_disabled)
                return;

            _state.disable_timer_remaining -= delta_seconds;

            if (_state.disable_timer_remaining <= 0f)
            {
                _state.disable_timer_remaining = 0f;
                _state.is_disabled = false;
                OnTurretsReactivated?.Invoke();
                Debug.Log("[Node_AutomatedArmory] Turrets reactivated.");
            }
        }

        /// <summary>
        /// Moves a survivor to cover. If the turret disable window is active,
        /// the move is safe. Otherwise, the survivor is shot.
        /// Returns true if the survivor survived (safe).
        /// </summary>
        public bool MoveToCover(string survivor_id, bool in_window)
        {
            if (string.IsNullOrEmpty(survivor_id))
            {
                Debug.LogError("[Node_AutomatedArmory] survivor_id is null or empty.");
                return false;
            }

            if (in_window && _state.is_disabled)
            {
                Debug.Log($"[Node_AutomatedArmory] Survivor '{survivor_id}' moved to cover safely.");
                return true;
            }

            // Caught outside cover while turrets are active
            if (!_state.survivors_shot.Contains(survivor_id))
            {
                _state.survivors_shot.Add(survivor_id);
            }

            OnSurvivorShot?.Invoke(survivor_id);
            Debug.Log($"[Node_AutomatedArmory] Survivor '{survivor_id}' shot by turret — " +
                      "not in disable window.");
            return false;
        }

        /// <summary>
        /// Returns whether turrets are currently active (shooting).
        /// </summary>
        public bool IsTurretActive()
        {
            return !_state.is_disabled;
        }

        public AutomatedArmoryState CaptureState()
        {
            return new AutomatedArmoryState
            {
                node_id = _state.node_id,
                turret_disabled_seconds = _state.turret_disabled_seconds,
                is_disabled = _state.is_disabled,
                disable_timer_remaining = _state.disable_timer_remaining,
                last_disable_method = _state.last_disable_method,
                survivors_shot = new List<string>(_state.survivors_shot)
            };
        }

        public void RestoreState(AutomatedArmoryState saved)
        {
            _state = saved ?? new AutomatedArmoryState();
        }
    }
}
