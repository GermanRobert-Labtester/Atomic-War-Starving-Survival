using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Shelter.Modules
{
    [Serializable]
    public class DeadManSwitchState
    {
        public string moduleId = "shelter_module_dead_man_switch";
        public string displayName = "Dead Man's Switch";
        public bool isArmed = false;
        public string operatorSurvivorId = string.Empty;
        public bool isTriggered = false;
        public bool broadcastsIntelCache = true;
    }

    /// <summary>
    /// Prompt #638: Module: Dead Man's Switch.
    /// Wired to the Radio. If the bunker is breached and the designated operator
    /// is killed, the switch broadcasts the entire Intel cache on all frequencies.
    /// Every Faction swarms to annihilate the killers.
    /// </summary>
    public class ShelterModule_DeadManSwitch
    {
        private DeadManSwitchState _state = new DeadManSwitchState();

        public event Action<DeadManSwitchState, string> OnArmed;
        public event Action<DeadManSwitchState> OnTriggered;
        public event Action<DeadManSwitchState> OnRevengeBroadcast;

        public DeadManSwitchState State => _state;

        public void Arm(string operatorId)
        {
            if (string.IsNullOrEmpty(operatorId)) return;

            _state.isArmed = true;
            _state.operatorSurvivorId = operatorId;
            _state.isTriggered = false;
            OnArmed?.Invoke(_state, operatorId);
        }

        public bool CheckTrigger(bool isBunkerBreached, bool isOperatorDead)
        {
            if (!_state.isArmed || _state.isTriggered) return false;

            if (isBunkerBreached && isOperatorDead)
            {
                _state.isTriggered = true;
                OnTriggered?.Invoke(_state);
                ExecuteRevengeBroadcast();
                return true;
            }

            return false;
        }

        public void ExecuteRevengeBroadcast()
        {
            if (!_state.isTriggered) return;

            _state.broadcastsIntelCache = true;
            OnRevengeBroadcast?.Invoke(_state);
        }
    
        public DeadManSwitchState CaptureState()
        {
            return _state;
        }

        public void RestoreState(DeadManSwitchState saved)
        {
            _state = saved ?? new DeadManSwitchState();
        }
    }
}

