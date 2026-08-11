using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Endgame
{
    [Serializable]
    public class MADState
    {
        public string victoryId = "victory_mad";
        public bool requiresLaunchCodes = true;
        public bool isTriggered = false;
    }

    /// <summary>
    /// Prompt #764: Mutually Assured Destruction.
    /// Ultimate spite ending. If overrun by Warlords, fire ICBM at own coordinates.
    /// Flash of white. Game Over. No one wins.
    /// </summary>
    public class Victory_MAD
    {
        private MADState _state = new MADState();

        public event Action OnEndingTriggered;
        public event Action OnICBMLaunched;

        public MADState State => _state;

        /// <summary>
        /// Fire the ICBM at own coordinates. Requires launch codes and being overrun by Warlords.
        /// Returns true if the MAD ending was successfully triggered.
        /// </summary>
        public bool FireAtOwnCoordinates(bool hasLaunchCodes, bool overrunByWarlords)
        {
            if (_state.isTriggered) return false;
            if (!hasLaunchCodes) return false;
            if (!overrunByWarlords) return false;

            _state.isTriggered = true;

            OnICBMLaunched?.Invoke();
            OnEndingTriggered?.Invoke();

            return true;
        }

        /// <summary>
        /// Returns the ending narration text for the MAD scenario.
        /// </summary>
        public string GetEndingText()
        {
            return "A flash of white light. Game Over. No one wins.";
        }

        public bool IsVictoryAchieved()
        {
            return _state.isTriggered;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public MADState CaptureState()
        {
            return new MADState
            {
                victoryId = _state.victoryId,
                requiresLaunchCodes = _state.requiresLaunchCodes,
                isTriggered = _state.isTriggered,
            };
        }

        public void RestoreState(MADState state)
        {
            if (state == null)
            {
                _state = new MADState();
                return;
            }
            _state = new MADState
            {
                victoryId = state.victoryId,
                requiresLaunchCodes = state.requiresLaunchCodes,
                isTriggered = state.isTriggered,
            };
        }
    }
}
