using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class CCTVState
    {
        public string moduleId = "shelter_module_cctv";
        public bool isActive = false;
    }

    /// <summary>
    /// Prompt #805: Security Cameras.
    /// Wires rooms to monitor. Eliminates Paranoia and Unseen debuffs for the operator.
    /// Allows seeing into rooms remotely.
    /// </summary>
    public class ShelterModule_CCTV
    {
        public event Action OnCCTVActivated;
        public event Action<string> OnParanoiaEliminated;   // operatorId
        public event Action<string> OnUnseenEliminated;     // operatorId
        public event Action OnCCTVDeactivated;

        private CCTVState _state;
        private string _currentOperatorId;

        public ShelterModule_CCTV(CCTVState state = null)
        {
            _state = state ?? new CCTVState();
        }

        public string ModuleId => _state.moduleId;

        /// <summary>
        /// Activate CCTV and assign an operator. Eliminates Paranoia and Unseen debuffs.
        /// </summary>
        public void Activate(string operatorId)
        {
            if (string.IsNullOrEmpty(operatorId))
            {
                Debug.LogWarning("[ShelterModule_CCTV] Activate called with null/empty operatorId.");
                return;
            }

            _state.isActive = true;
            _currentOperatorId = operatorId;

            OnCCTVActivated?.Invoke();
            OnParanoiaEliminated?.Invoke(operatorId);
            OnUnseenEliminated?.Invoke(operatorId);
        }

        /// <summary>
        /// Deactivate CCTV system.
        /// </summary>
        public void Deactivate()
        {
            _state.isActive = false;
            _currentOperatorId = null;
            OnCCTVDeactivated?.Invoke();
        }

        /// <summary>
        /// Whether the CCTV operator can see into a given room.
        /// Always returns true when CCTV is active (all rooms are wired).
        /// </summary>
        public bool CanSeeRoom(string roomId)
        {
            if (!_state.isActive)
                return false;

            return true;
        }

        public bool IsActive() => _state.isActive;

        public string GetCurrentOperator() => _currentOperatorId;

        public CCTVState CaptureState()
        {
            return new CCTVState
            {
                moduleId = _state.moduleId,
                isActive = _state.isActive
            };
        }

        public void RestoreState(CCTVState state)
        {
            _state = state ?? new CCTVState();
        }
    }
}
