using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    public enum EndgameChoice
    {
        VehicleEscape,
        HamRadioExtraction,
        NuclearMAD,
        Surrender
    }

    [Serializable]
    public class EndgameUltimatumState
    {
        public string id = "endgame_ultimatum";
        public string dominantFactionId = "military_remnants";
        public int daysRemaining = 7;
        public bool isUltimatumActive = false;
        public bool isGameWon = false;
        public bool isGameLost = false;
        public EndgameChoice selectedEnding;
    }

    /// <summary>
    /// Prompt #418: Event: The Warlord's Ultimatum.
    /// The dominant faction locates the shelter with heavy artillery.
    /// Gives the player a 7-day countdown to execute: VehicleEscape, HamRadioExtraction, NuclearMAD, or Surrender.
    /// </summary>
    public class Endgame_Ultimatum
    {
        private EndgameUltimatumState _state = new EndgameUltimatumState();

        public event Action<EndgameUltimatumState> OnUltimatumCountdownStarted;
        public event Action<EndgameUltimatumState, EndgameChoice> OnEndgameResolved;

        public EndgameUltimatumState State => _state;

        public void TriggerUltimatum()
        {
            _state.isUltimatumActive = true;
            _state.daysRemaining = 7;
            OnUltimatumCountdownStarted?.Invoke(_state);
        }

        public bool ResolveEndgameChoice(EndgameChoice choice)
        {
            if (!_state.isUltimatumActive) return false;

            _state.selectedEnding = choice;
            _state.isUltimatumActive = false;

            if (choice == EndgameChoice.Surrender)
            {
                _state.isGameLost = true;
                _state.isGameWon = false;
            }
            else
            {
                _state.isGameWon = true;
                _state.isGameLost = false;
            }

            OnEndgameResolved?.Invoke(_state, choice);
            return true;
        }

        public void TickDay()
        {
            if (_state.isUltimatumActive)
            {
                _state.daysRemaining--;
                if (_state.daysRemaining <= 0)
                {
                    ResolveEndgameChoice(EndgameChoice.Surrender);
                }
            }
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public EndgameUltimatumState CaptureState() => _state;

        public void RestoreState(EndgameUltimatumState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
