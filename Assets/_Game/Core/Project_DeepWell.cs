using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class DeepWellState
    {
        public string projectId = "project_deep_well";
        public int constructionDaysRequired = 30;
        public int daysSpent = 0;
        public bool isComplete = false;
        public int pipesRequired = 10;
        public int pumpsRequired = 3;
        public int pipesProvided = 0;
        public int pumpsProvided = 0;
    }

    /// <summary>
    /// Prompt #579: Project: Deep Well.
    /// Takes 30 days to dig, massive Fatigue. Requires Pipes and Pumps.
    /// On completion provides infinite clean water.
    /// </summary>
    public class Project_DeepWell
    {
        private DeepWellState _state = new DeepWellState();

        public event Action<DeepWellState> OnConstructionStarted;
        public event Action<DeepWellState, string, int> OnMaterialAdded;
        public event Action<DeepWellState, int> OnConstructionDayProgressed;
        public event Action<DeepWellState> OnDeepWellCompleted;

        public DeepWellState State => _state;

        public void StartConstruction()
        {
            if (_state.isComplete) return;
            _state.daysSpent = 0;
            OnConstructionStarted?.Invoke(_state);
        }

        public void AddMaterial(string type, int count)
        {
            if (_state.isComplete) return;

            switch (type)
            {
                case "pipes":
                    _state.pipesProvided = Mathf.Min(_state.pipesProvided + count, _state.pipesRequired);
                    break;
                case "pumps":
                    _state.pumpsProvided = Mathf.Min(_state.pumpsProvided + count, _state.pumpsRequired);
                    break;
            }

            OnMaterialAdded?.Invoke(_state, type, count);
        }

        public void TickDay(float availableFatigue)
        {
            if (_state.isComplete) return;
            if (_state.pipesProvided < _state.pipesRequired) return;
            if (_state.pumpsProvided < _state.pumpsRequired) return;

            if (availableFatigue > 20f)
            {
                _state.daysSpent++;
                OnConstructionDayProgressed?.Invoke(_state, _state.daysSpent);

                if (_state.daysSpent >= _state.constructionDaysRequired)
                {
                    _state.isComplete = true;
                    OnDeepWellCompleted?.Invoke(_state);
                }
            }
        }

        public bool IsWaterSupplyInfinite()
        {
            return _state.isComplete;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public DeepWellState CaptureState() => _state;

        public void RestoreState(DeepWellState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
