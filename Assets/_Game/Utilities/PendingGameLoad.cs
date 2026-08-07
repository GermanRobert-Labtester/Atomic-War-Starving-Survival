namespace AtomicWar._Game.Utilities
{
    /// <summary>
    /// Hands the main menu's intent to the gameplay scene across the
    /// StartScreen -> gameplay scene load.
    ///
    /// A static is the right shape here despite the usual objections: the two
    /// scenes never coexist, the payload is two immutable values, and the
    /// alternative (a DontDestroyOnLoad carrier object) would have to be
    /// created, found and torn down for no added safety. It also keeps
    /// GameBootstrap free of any dependency on the UI assembly.
    ///
    /// Contract: the menu sets the fields, then loads the gameplay scene;
    /// GameBootstrap.Awake calls <see cref="ConsumeSlotId"/> exactly once.
    /// Consuming clears the slot so a later scene reload (or entering play
    /// mode directly on the gameplay scene) starts a fresh game instead of
    /// silently re-loading a stale save.
    /// </summary>
    public static class PendingGameLoad
    {
        /// <summary>
        /// Save slot the player asked to continue, or null for a new game.
        /// </summary>
        public static string SlotId { get; set; }

        /// <summary>
        /// Difficulty picked on the New Expedition dialog. Recorded only --
        /// see <see cref="ExpeditionDifficulty"/>.
        /// </summary>
        public static ExpeditionDifficulty Difficulty { get; set; } = ExpeditionDifficulty.Operative;

        /// <summary>
        /// Read the pending slot and clear it in one step, so it cannot be
        /// applied twice. Returns null when no continue was requested.
        /// </summary>
        public static string ConsumeSlotId()
        {
            string slotId = SlotId;
            SlotId = null;
            return slotId;
        }

        /// <summary>
        /// Reset to "new game" defaults. Used by the New Expedition flow and by
        /// tests, which need a clean starting point because static state
        /// survives between test cases within a domain.
        /// </summary>
        public static void Clear()
        {
            SlotId = null;
            Difficulty = ExpeditionDifficulty.Operative;
        }
    }
}
