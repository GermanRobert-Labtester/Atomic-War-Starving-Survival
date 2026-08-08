using System;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class TeachChildState
    {
        public string actionId = "action_teach_child";
        public float skillXpPerHour = 10f;
        public float adultFatiguePerHour = 0.15f;
    }

    /// <summary>DEMOTE-Action-batch — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class Action_TeachChild
    {
        public event Action<string, string, float> OnChildGainedXP;
        public event Action<string, float> OnAdultFatigued;

        private TeachChildState _state;

        public Action_TeachChild(TeachChildState state = null)
        {
            _state = state ?? new TeachChildState();
        }

        public string ActionId => _state.actionId;

        /// <summary>
        /// Adult teaches child a skill for the specified hours.
        /// Generates XP for child, drains adult fatigue. Adult can't do other work during this time.
        /// </summary>
        public float Teach(string adultId, string childId, string skillId, float hours)
        {
            if (string.IsNullOrEmpty(adultId) || string.IsNullOrEmpty(childId) || string.IsNullOrEmpty(skillId))
            {
                Debug.LogWarning("[Action_TeachChild] Teach called with null/empty id.");
                return 0f;
            }

            if (hours <= 0f)
            {
                Debug.LogWarning("[Action_TeachChild] Teach called with non-positive hours.");
                return 0f;
            }

            float xpGenerated = _state.skillXpPerHour * hours;
            float fatigueAccumulated = _state.adultFatiguePerHour * hours;

            OnChildGainedXP?.Invoke(childId, skillId, xpGenerated);
            OnAdultFatigued?.Invoke(adultId, fatigueAccumulated);

            return xpGenerated;
        }

        public TeachChildState CaptureState()
        {
            return new TeachChildState
            {
                actionId = _state.actionId,
                skillXpPerHour = _state.skillXpPerHour,
                adultFatiguePerHour = _state.adultFatiguePerHour
            };
        }

        public void RestoreState(TeachChildState state)
        {
            _state = state ?? new TeachChildState();
        }
    }
}
