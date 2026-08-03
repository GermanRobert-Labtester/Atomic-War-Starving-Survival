namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Specific endgame conditions evaluating campaign victory or defeat (Prompt #41).
    /// </summary>
    public enum EndgameConditionKind
    {
        None,
        /// <summary>Defeat: All survivors in the shelter have perished.</summary>
        AllSurvivorsDeceased,
        /// <summary>Defeat: Both air filtration and radiation shielding health dropped to 0%.</summary>
        BunkerStructuralCollapse,
        /// <summary>Victory: Radio broadcast contact completed military rescue countdown (Day >= 60).</summary>
        RescueExtractionSuccess,
        /// <summary>Victory: Shelter operates for 100 days with functioning hydroponics and zero deaths.</summary>
        LongTermSelfSufficiency
    }
}
