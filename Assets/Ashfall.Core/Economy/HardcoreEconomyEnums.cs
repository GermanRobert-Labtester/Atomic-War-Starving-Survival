namespace Ashfall.Core.Economy
{
    /// <summary>Scarcity tier affecting market multiplier.</summary>
    public enum ScarcityTier
    {
        Critical,
        High,
        Moderate,
        Stable,
        Reconstruction,
        LateScarcity,
        DeepWinter,
        Endgame,

        // Legacy name retained for callers that used the pre-expansion
        // low-pressure tier. The expanded catalog uses Stable.
        Low = Stable
    }

    /// <summary>Transient price-shock event kinds.</summary>
    public enum PriceShockKind
    {
        PlumePassing,
        ConvoyAmbush,
        FactionConflict,
        SeasonalScarcity,
        DiseaseOutbreak,
        FuelShortage,

        // Legacy names retained as aliases for scenario and UI callers that
        // predate the expanded canonical shock vocabulary.
        FactionWar = FactionConflict,
        WinterDeepens = SeasonalScarcity
    }
}
