using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class ElevatorState
    {
        public string projectId = "project_elevator";
        public bool isBuilt = false;
        public int constructionDays = 20;
        public int daysSpent = 0;
        public float powerRequired = 50f;
        public bool negatesHaulingFatigue = true;
        public string trappedSurvivorId;
        public float o2Remaining = 100f;
    }

    /// <summary>
    /// Prompt #583: Project: Elevator.
    /// Replaces staircases. Negates Fatigue from InternalHauling.
    /// Requires 50W constant Power. If power fails, survivor trapped losing O2.
    /// </summary>
    public class Project_Elevator
    {
        private ElevatorState _state = new ElevatorState();

        public event Action<ElevatorState> OnElevatorBuilt;
        public event Action<ElevatorState, string> OnSurvivorTrapped;
        public event Action<ElevatorState, string> OnSurvivorRescued;
        public event Action<ElevatorState, string, float> OnO2Critical;

        public ElevatorState State => _state;

        public void StartConstruction()
        {
            if (_state.isBuilt) return;
            _state.daysSpent = 0;
        }

        public void TickDay()
        {
            if (_state.isBuilt) return;

            _state.daysSpent++;
            if (_state.daysSpent >= _state.constructionDays)
            {
                _state.isBuilt = true;
                OnElevatorBuilt?.Invoke(_state);
            }
        }

        public bool TryUseElevator(string survivorId, bool hasPower)
        {
            if (!_state.isBuilt) return false;

            if (!hasPower)
            {
                _state.trappedSurvivorId = survivorId;
                _state.o2Remaining = 100f;
                OnSurvivorTrapped?.Invoke(_state, survivorId);
                return false;
            }

            return true;
        }

        public void TickHourWhileTrapped(string survivorId, float hours)
        {
            if (string.IsNullOrEmpty(_state.trappedSurvivorId)) return;
            if (_state.trappedSurvivorId != survivorId) return;

            // O2 drains over time; roughly 100% over ~6 hours at default rate
            _state.o2Remaining -= hours * 16.67f;
            _state.o2Remaining = Mathf.Max(_state.o2Remaining, 0f);

            if (_state.o2Remaining <= 20f)
            {
                OnO2Critical?.Invoke(_state, survivorId, _state.o2Remaining);
            }
        }

        public void RescueSurvivor()
        {
            if (string.IsNullOrEmpty(_state.trappedSurvivorId)) return;

            string rescued = _state.trappedSurvivorId;
            _state.trappedSurvivorId = null;
            _state.o2Remaining = 100f;
            OnSurvivorRescued?.Invoke(_state, rescued);
        }

        public float GetFatigueMultiplier()
        {
            if (_state.isBuilt && _state.negatesHaulingFatigue)
                return 0f;
            return 1f;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public ElevatorState CaptureState() => _state;

        public void RestoreState(ElevatorState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
