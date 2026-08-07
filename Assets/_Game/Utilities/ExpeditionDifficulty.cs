namespace AtomicWar._Game.Utilities
{
    /// <summary>
    /// Difficulty chosen on the "New Expedition" dialog.
    ///
    /// Recorded only. No gameplay system reads this yet -- the project has no
    /// difficulty/modifier system, and inventing one was explicitly out of
    /// scope for the main-menu work. It is carried on
    /// <see cref="PendingGameLoad"/> so the player's choice is not silently
    /// discarded once such a system exists.
    /// </summary>
    public enum ExpeditionDifficulty
    {
        /// <summary>Standard resource availability.</summary>
        Operative = 0,

        /// <summary>Scarce resources.</summary>
        Veteran = 1
    }
}
