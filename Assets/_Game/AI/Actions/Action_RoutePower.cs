using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.AI.Actions
{
    [Serializable]
    public class RoutePowerState
    {
        public string action_id = "action_route_power";
        public int breakers_required = 3;
        public int copper_per_breaker = 2;
        public int breakers_repaired = 0;
        public bool elevator_activated = false;
    }

    /// <summary>DEMOTE-Action-remaining — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public sealed class Action_RoutePower
    {
        private RoutePowerState _state;

        public event Action<string, int> OnBreakerRepaired;    // (survivor_id, breaker_index)
        public event Action<string> OnElevatorActivated;        // (survivor_id)

        public string ActionId => _state.action_id;
        public int BreakersRepaired => _state.breakers_repaired;
        public int BreakersRequired => _state.breakers_required;

        public Action_RoutePower()
        {
            _state = new RoutePowerState();
        }

        /// <summary>
        /// Attempt to repair one breaker box. Requires >= copper_per_breaker copper scrap.
        /// Returns true if the breaker was successfully repaired.
        /// </summary>
        public bool RepairBreaker(string survivor_id, int copper_scrap)
        {
            if (string.IsNullOrEmpty(survivor_id))
            {
                Debug.LogError("[Action_RoutePower] survivor_id is null or empty.");
                return false;
            }

            if (_state.breakers_repaired >= _state.breakers_required)
            {
                Debug.LogWarning("[Action_RoutePower] All breakers already repaired.");
                return false;
            }

            if (copper_scrap < _state.copper_per_breaker)
            {
                GameLog.Log($"[Action_RoutePower] Not enough copper. " +
                          $"Need {_state.copper_per_breaker}, have {copper_scrap}.");
                return false;
            }

            _state.breakers_repaired++;
            OnBreakerRepaired?.Invoke(survivor_id, _state.breakers_repaired);
            GameLog.Log($"[Action_RoutePower] Breaker {_state.breakers_repaired}/{_state.breakers_required} " +
                      $"repaired by '{survivor_id}'.");
            return true;
        }

        /// <summary>
        /// Returns true if all required breakers have been repaired.
        /// </summary>
        public bool AllBreakersRepaired()
        {
            return _state.breakers_repaired >= _state.breakers_required;
        }

        /// <summary>
        /// Activates the freight elevator if all breakers are repaired.
        /// </summary>
        public void ActivateElevator(string survivor_id)
        {
            if (string.IsNullOrEmpty(survivor_id))
            {
                Debug.LogError("[Action_RoutePower] survivor_id is null or empty.");
                return;
            }

            if (_state.elevator_activated)
            {
                Debug.LogWarning("[Action_RoutePower] Elevator is already activated.");
                return;
            }

            if (!AllBreakersRepaired())
            {
                GameLog.Log($"[Action_RoutePower] Cannot activate elevator — " +
                          $"only {_state.breakers_repaired}/{_state.breakers_required} breakers repaired.");
                return;
            }

            _state.elevator_activated = true;
            OnElevatorActivated?.Invoke(survivor_id);
            GameLog.Log($"[Action_RoutePower] Freight elevator activated by '{survivor_id}'.");
        }

        public RoutePowerState CaptureState()
        {
            return new RoutePowerState
            {
                action_id = _state.action_id,
                breakers_required = _state.breakers_required,
                copper_per_breaker = _state.copper_per_breaker,
                breakers_repaired = _state.breakers_repaired,
                elevator_activated = _state.elevator_activated
            };
        }

        public void RestoreState(RoutePowerState saved)
        {
            _state = saved ?? new RoutePowerState();
        }
    }
}
