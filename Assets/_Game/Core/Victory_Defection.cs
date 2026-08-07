using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class DefectionState
    {
        public string victoryId = "victory_defection";
        public bool isActive = false;
        public int siegeLevelRequired = 5;
        public bool isSurrendered = false;
        public bool isGameOver = false;
    }

    /// <summary>
    /// Prompt #668: Endgame: Defection (Bad Ending).
    /// Cowardly alt ending. Level 5 Siege → surrender AI Core + resources.
    /// Warlord spares you. Survive as slave. Bad Ending.
    /// </summary>
    public class Victory_Defection
    {
        private DefectionState _state = new DefectionState();

        public event Action<DefectionState> OnSurrendered;
        public event Action<DefectionState> OnGameOver;

        public DefectionState State => _state;

        public bool Surrender(int currentSiegeLevel, bool hasAICore)
        {
            if (_state.isGameOver || _state.isSurrendered)
                return false;

            if (currentSiegeLevel < _state.siegeLevelRequired)
                return false;

            if (!hasAICore)
                return false;

            _state.isActive = true;
            _state.isSurrendered = true;
            _state.isGameOver = true;

            OnSurrendered?.Invoke(_state);
            OnGameOver?.Invoke(_state);
            return true;
        }

        public bool IsBadEnding()
        {
            return true;
        }

        public string GetEndingText()
        {
            return "You surrendered the AI Core and all resources to the Warlord. " +
                   "Your people survive as slaves under their rule. " +
                   "This is not the ending you hoped for.";
        }

        // ── Save / Load ────────────────────────────────────────────────

        public DefectionState CaptureState()
        {
            return new DefectionState
            {
                victoryId = _state.victoryId,
                isActive = _state.isActive,
                siegeLevelRequired = _state.siegeLevelRequired,
                isSurrendered = _state.isSurrendered,
                isGameOver = _state.isGameOver,
            };
        }

        public void RestoreState(DefectionState state)
        {
            if (state == null)
            {
                _state = new DefectionState();
                return;
            }
            _state = new DefectionState
            {
                victoryId = state.victoryId,
                isActive = state.isActive,
                siegeLevelRequired = state.siegeLevelRequired,
                isSurrendered = state.isSurrendered,
                isGameOver = state.isGameOver,
            };
        }
    }
}
