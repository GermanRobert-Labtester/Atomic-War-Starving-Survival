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
        LongTermSelfSufficiency,
        /// <summary>
        /// Victory (bittersweet): Lifeboat Transmission — exactly one survivor extracted;
        /// the rest condemned (Prompt #20). Mutually exclusive with full RescueExtractionSuccess.
        /// </summary>
        LifeboatPartialExtraction,
        /// <summary>Victory: Mutually Assured Destruction — automated strike triggered.</summary>
        MAD,
        /// <summary>Victory: Migration — escaped fallout zone via convoy.</summary>
        Migration,
        /// <summary>Victory: The Broadcast — established regional emergency communications.</summary>
        TheBroadcast,
        /// <summary>Victory: The Cure — synthesized anti-radiation genetic compound.</summary>
        TheCure,
        /// <summary>Victory: The Martian — complete closed-loop shelter self-sufficiency.</summary>
        TheMartian,
        /// <summary>Victory: True Ending — negotiated lasting peace between faction remnants.</summary>
        TrueEnding,
        /// <summary>Victory: Underground City — expanded sub-pen into permanent subterranean settlement.</summary>
        UndergroundCity,
        /// <summary>Victory: Unifier — subjugated rival wasteland factions under one banner.</summary>
        Unifier
    }
}
