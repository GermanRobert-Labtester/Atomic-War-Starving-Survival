using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class ScapegoatState
    {
        public string dynamicId = "dynamic_scapegoat";
        public float moraleThreshold = 0.3f;
        public float moraleDrainPerBlame = 0.5f;
        public string currentScapegoatId;
    }

    public class Dynamic_Scapegoat
    {
        public event Action<string> OnScapegoatSelected;
        public event Action<string, string> OnBlameApplied;
        public event Action<string> OnMoraleDrained;

        private readonly ScapegoatState _state;
        private string _currentScapegoatId;

        public Dynamic_Scapegoat()
        {
            _state = new ScapegoatState();
        }

        public Dynamic_Scapegoat(ScapegoatState state)
        {
            _state = state ?? new ScapegoatState();
            _currentScapegoatId = _state.currentScapegoatId;
        }

        /// <summary>
        /// If average morale is below 30%, selects the weakest / lowest-skill survivor
        /// as the group's scapegoat.
        /// </summary>
        public void SelectScapegoat(
            List<(string id, float skill, float strength)> survivors,
            float avgMorale)
        {
            if (survivors == null || survivors.Count == 0) return;

            // avgMorale is 0-1 range; threshold is 0.3
            if (avgMorale >= _state.moraleThreshold) return;

            // Select survivor with lowest combined skill + strength score
            string chosenId = null;
            float lowestScore = float.MaxValue;

            for (int i = 0; i < survivors.Count; i++)
            {
                float score = survivors[i].skill + survivors[i].strength;
                if (score < lowestScore)
                {
                    lowestScore = score;
                    chosenId = survivors[i].id;
                }
            }

            if (chosenId != null)
            {
                _currentScapegoatId = chosenId;
                _state.currentScapegoatId = chosenId;
                OnScapegoatSelected?.Invoke(chosenId);
            }
        }

        /// <summary>
        /// Blames the current scapegoat for a module breakage, draining their morale to 0.
        /// </summary>
        public void BlameForBreakage(string moduleId, string reason)
        {
            if (string.IsNullOrEmpty(_currentScapegoatId)) return;

            OnBlameApplied?.Invoke(_currentScapegoatId, reason);
            OnMoraleDrained?.Invoke(_currentScapegoatId);
        }

        public string GetScapegoat()
        {
            return _currentScapegoatId;
        }

        /// <summary>
        /// Clears the scapegoat if morale has recovered above the threshold.
        /// </summary>
        public void ClearScapegoat()
        {
            _currentScapegoatId = null;
            _state.currentScapegoatId = null;
        }

        public ScapegoatState CaptureState()
        {
            _state.currentScapegoatId = _currentScapegoatId;
            return _state;
        }

        public void RestoreState(ScapegoatState state)
        {
            if (state == null) return;
            _state.dynamicId = state.dynamicId;
            _state.moraleThreshold = state.moraleThreshold;
            _state.moraleDrainPerBlame = state.moraleDrainPerBlame;
            _currentScapegoatId = state.currentScapegoatId;
            _state.currentScapegoatId = state.currentScapegoatId;
        }
    }
}
