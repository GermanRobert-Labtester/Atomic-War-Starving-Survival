using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class AscendancyState
    {
        public string victoryId = "victory_ascendancy";
        public float radThreshold = 1000f;
        public bool requiresRadiotrophicMutation = true;
    }

    /// <summary>
    /// Prompt #766: Ascendancy (Cult Victory).
    /// All survivors reach 1000 mSv without dying via Radiotrophic mutations.
    /// Walk into a Level 5 Rad Storm. Homo-Radiata is born.
    /// </summary>
    public class Victory_Ascendancy
    {
        private AscendancyState _state = new AscendancyState();

        public event Action OnEndingTriggered;
        public event Action<string> OnSurvivorAdapted;

        public AscendancyState State => _state;

        /// <summary>
        /// Check whether every survivor has reached the radiation threshold
        /// with the Radiotrophic mutation and is still alive.
        /// Each entry is (survivorId, currentRadLevel, hasRadiotrophicMutation).
        /// Returns true if the Ascendancy victory condition is met.
        /// </summary>
        public bool CheckVictory(List<(string id, float radLevel, bool hasRadiotrophic)> survivors)
        {
            if (survivors == null || survivors.Count == 0)
                return false;

            for (int i = 0; i < survivors.Count; i++)
            {
                var s = survivors[i];

                // Must have the radiotrophic mutation
                if (_state.requiresRadiotrophicMutation && !s.hasRadiotrophic)
                    return false;

                // Must have reached the radiation threshold
                if (s.radLevel < _state.radThreshold)
                    return false;

                // Fire adapted event for each qualifying survivor
                if (!string.IsNullOrEmpty(s.id))
                {
                    OnSurvivorAdapted?.Invoke(s.id);
                }
            }

            OnEndingTriggered?.Invoke();
            return true;
        }

        /// <summary>
        /// Returns the ending narration text for the Ascendancy scenario.
        /// </summary>
        public string GetEndingText()
        {
            return "They walked into the storm and did not fall. "
                 + "The Geiger screamed until it stopped. "
                 + "Humanity is dead; Homo-Radiata is born.";
        }

        public bool IsVictoryAchieved(List<(string id, float radLevel, bool hasRadiotrophic)> survivors)
        {
            return CheckVictory(survivors);
        }

        // ── Save / Load ────────────────────────────────────────────────

        public AscendancyState CaptureState()
        {
            return new AscendancyState
            {
                victoryId = _state.victoryId,
                radThreshold = _state.radThreshold,
                requiresRadiotrophicMutation = _state.requiresRadiotrophicMutation,
            };
        }

        public void RestoreState(AscendancyState state)
        {
            if (state == null)
            {
                _state = new AscendancyState();
                return;
            }
            _state = new AscendancyState
            {
                victoryId = state.victoryId,
                radThreshold = state.radThreshold,
                requiresRadiotrophicMutation = state.requiresRadiotrophicMutation,
            };
        }
    }
}
