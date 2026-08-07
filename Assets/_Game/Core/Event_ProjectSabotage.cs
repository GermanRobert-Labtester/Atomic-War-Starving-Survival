using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class ProjectSabotageState
    {
        public string eventId = "event_project_sabotage";
        public bool isActive = false;
        public int saboteurStrength = 0;
        public int guardsAssigned = 0;
        public float sabotageProgress = 0f;
        public string constructionSiteId;
    }

    /// <summary>
    /// Prompt #584: Event: Project Sabotage.
    /// Factions send saboteurs to destroy scaffolding. Player must assign Guards.
    /// If guards &lt; saboteurs/3, sabotage progresses each hour.
    /// </summary>
    public class Event_ProjectSabotage
    {
        private ProjectSabotageState _state = new ProjectSabotageState();

        public event Action<ProjectSabotageState, string, int> OnSabotageAttempt;
        public event Action<ProjectSabotageState, int> OnGuardAssigned;
        public event Action<ProjectSabotageState, string, float> OnConstructionDamaged;
        public event Action<ProjectSabotageState, string> OnSabotageRepelled;

        public ProjectSabotageState State => _state;

        public void TriggerSabotage(string projectId, int saboteurCount)
        {
            _state.constructionSiteId = projectId;
            _state.saboteurStrength = saboteurCount;
            _state.guardsAssigned = 0;
            _state.sabotageProgress = 0f;
            _state.isActive = true;

            OnSabotageAttempt?.Invoke(_state, projectId, saboteurCount);
        }

        public void AssignGuard(int count)
        {
            if (!_state.isActive) return;

            _state.guardsAssigned += count;
            OnGuardAssigned?.Invoke(_state, _state.guardsAssigned);
        }

        public void TickHour(System.Random rng)
        {
            if (!_state.isActive) return;

            float guardThreshold = _state.saboteurStrength / 3f;

            if (_state.guardsAssigned >= guardThreshold)
            {
                // Guards repel saboteurs — progress decreases
                _state.sabotageProgress -= 5f * rng.Next(1, 3);
                _state.sabotageProgress = Mathf.Max(_state.sabotageProgress, 0f);

                if (_state.sabotageProgress <= 0f)
                {
                    _state.isActive = false;
                    OnSabotageRepelled?.Invoke(_state, _state.constructionSiteId);
                }
                return;
            }

            // Insufficient guards — sabotage progresses
            float progressRate = (guardThreshold - _state.guardsAssigned) / guardThreshold;
            _state.sabotageProgress += 10f * progressRate * (float)(0.5 + rng.NextDouble());
            _state.sabotageProgress = Mathf.Min(_state.sabotageProgress, 100f);

            OnConstructionDamaged?.Invoke(_state, _state.constructionSiteId, _state.sabotageProgress);
        }

        public bool IsSabotaged()
        {
            return _state.sabotageProgress >= 100f;
        }

        public ProjectSabotageState CaptureState()
        {
            return _state;
        }

        public void RestoreState(ProjectSabotageState saved)
        {
            _state = saved ?? new ProjectSabotageState();
        }
    }
}
