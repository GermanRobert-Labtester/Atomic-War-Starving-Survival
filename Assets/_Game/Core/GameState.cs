using System;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// High-level lifecycle phases of a game session.
    /// </summary>
    public enum GamePhase
    {
        MainMenu,
        Running,
        Paused,
        GameOver
    }

    /// <summary>
    /// Authoritative snapshot of the current session (phase, day, run flags).
    /// Save/load safe: holds only serializable primitives. Behaviour lives in
    /// the dedicated systems; this is the state they mutate and the SaveSystem
    /// persists.
    /// </summary>
    [Serializable]
    public class GameState
    {
        private GamePhase _phase = GamePhase.MainMenu;

        public GamePhase Phase
        {
            get => _phase;
            set
            {
                if (_phase != value)
                {
                    _phase = value;
                    OnPhaseChanged?.Invoke(value);
                }
            }
        }

        public int Day { get; set; }
        public bool IsPaused { get; set; }

        /// <summary>
        /// Photosensitivity-safe accessibility toggle. When true, the Day-30
        /// flashpoint white flash is shorter and desaturated, and camera
        /// shake amplitude is reduced. Read by the FlashpointChoreographer
        /// via a delegate supplied at construction.
        /// </summary>
        public bool AccessibilitySafeMode { get; set; }

        /// <summary>Fired when Phase actually changes (not on redundant sets).</summary>
        public event Action<GamePhase> OnPhaseChanged;

        /// <summary>Reset to a fresh new-game state.</summary>
        public void Reset()
        {
            _phase = GamePhase.MainMenu;
            Day = 1;
            IsPaused = false;
            AccessibilitySafeMode = false;
        }
    }
}
