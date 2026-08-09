using System;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class PlayState
    {
        public string actionId = "action_play";
        public float moraleGenerated = 0.3f;
        public float noiseGenerated = 0.8f;
        public bool quietRulesActive = false;
    }

    /// <summary>DEMOTE-Action-batch — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class Action_Play
    {
        public event Action<string, float> OnMoraleGenerated;
        public event Action<string, float> OnNoiseGenerated;
        public event Action OnQuietRulesEnforced;

        private PlayState _state;

        public Action_Play(PlayState state = null)
        {
            _state = state ?? new PlayState();
        }

        public string ActionId => _state.actionId;
        public bool QuietRulesActive => _state.quietRulesActive;

        /// <summary>
        /// Child plays. Generates morale and noise. If a toy is present, morale is boosted.
        /// When quiet rules are active, morale generation is suppressed and noise still leaks.
        /// </summary>
        public void Play(string childId, bool hasToy)
        {
            if (string.IsNullOrEmpty(childId))
            {
                Debug.LogWarning("[Action_Play] Play called with null/empty childId.");
                return;
            }

            float morale = GetMoraleImpact(_state.quietRulesActive);
            if (hasToy)
            {
                morale *= 1.5f;
            }

            float noise = _state.noiseGenerated;

            if (!_state.quietRulesActive && morale > 0f)
            {
                OnMoraleGenerated?.Invoke(childId, morale);
            }
            else if (_state.quietRulesActive)
            {
                OnQuietRulesEnforced?.Invoke();
            }

            OnNoiseGenerated?.Invoke(childId, noise);
        }

        /// <summary>
        /// Toggles quiet rules. When enforced, child morale is crushed.
        /// </summary>
        public void EnforceQuietRules(bool enforce)
        {
            _state.quietRulesActive = enforce;

            if (enforce)
            {
                OnQuietRulesEnforced?.Invoke();
            }
        }

        /// <summary>
        /// Returns the morale impact of play. Negative if quiet rules are enforced.
        /// </summary>
        public float GetMoraleImpact(bool quietEnforced)
        {
            if (quietEnforced)
            {
                return -_state.moraleGenerated;
            }
            return _state.moraleGenerated;
        }

        public PlayState CaptureState()
        {
            return new PlayState
            {
                actionId = _state.actionId,
                moraleGenerated = _state.moraleGenerated,
                noiseGenerated = _state.noiseGenerated,
                quietRulesActive = _state.quietRulesActive
            };
        }

        public void RestoreState(PlayState state)
        {
            _state = state ?? new PlayState();
        }
    }
}
