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
    [System.Serializable]
    public class GameState
    {
        public GamePhase Phase { get; set; } = GamePhase.MainMenu;
        public int Day { get; set; }
        public bool IsPaused { get; set; }

        /// <summary>Reset to a fresh new-game state.</summary>
        public void Reset() => throw new System.NotImplementedException();
    }
}
