using System;

namespace Ashfall.Core.Performance;

/// <summary>
/// Scale tier definitions for major state dimensions.
/// Actual sizes are repository-semantic; document them in code comments.
/// </summary>
public static class ScaleTier
{
    // ── Roster ──────────────────────────────────────────────────────
    /// <summary>Small roster: 3 survivors.</summary>
    public const string RosterSmall = "small";       // 3 survivors

    /// <summary>Normal roster: 6 survivors.</summary>
    public const string RosterNormal = "normal";     // 6 survivors

    /// <summary>Large roster: 12 survivors.</summary>
    public const string RosterLarge = "large";       // 12 survivors

    /// <summary>Stress roster: 24 survivors.</summary>
    public const string RosterStress = "stress";     // 24 survivors

    // ── Catalog ────────────────────────────────────────────────────
    /// <summary>Minimal catalog subset for smoke tests.</summary>
    public const string CatalogMinimal = "minimal";

    /// <summary>Normal catalog: full authored content.</summary>
    public const string CatalogNormal = "normal";

    /// <summary>Stress catalog: repeated definitions to stress index/lookup.</summary>
    public const string CatalogStress = "stress";

    // ── Journal ────────────────────────────────────────────────────
    /// <summary>Short journal: early-game entry count.</summary>
    public const string JournalShort = "short";      // ~20 entries

    /// <summary>Medium journal: mid-game entry count.</summary>
    public const string JournalMedium = "medium";    // ~100 entries

    /// <summary>Late-game journal: extended entry count.</summary>
    public const string JournalLate = "late";        // ~300 entries

    /// <summary>Stress journal: maximum entry count.</summary>
    public const string JournalStress = "stress";    // ~1000 entries

    // ── Expeditions ────────────────────────────────────────────────
    /// <summary>No active expeditions.</summary>
    public const string ExpeditionNone = "none";

    /// <summary>Typical active set: 2 concurrent sorties.</summary>
    public const string ExpeditionTypical = "typical"; // 2 active

    /// <summary>High active set: 6 concurrent sorties.</summary>
    public const string ExpeditionHigh = "high";      // 6 active

    /// <summary>Stress active set + retained history.</summary>
    public const string ExpeditionStress = "stress";  // 10 active + history

    // ── World State ────────────────────────────────────────────────
    /// <summary>Starting visited locations.</summary>
    public const string WorldSmall = "small";

    /// <summary>Normal explored world state.</summary>
    public const string WorldNormal = "normal";

    /// <summary>Expanded late-game world state.</summary>
    public const string WorldLarge = "large";

    /// <summary>Stress world state with dense encounter/flag/faction entries.</summary>
    public const string WorldStress = "stress";

    /// <summary>Get a roster tier multiplier relative to normal.</summary>
    public static int RosterCount(string tier)
    {
        return tier switch
        {
            RosterSmall => 3,
            RosterNormal => 6,
            RosterLarge => 12,
            RosterStress => 24,
            _ => 6,
        };
    }

    /// <summary>Get a journal entry count for a tier.</summary>
    public static int JournalEntryCount(string tier)
    {
        return tier switch
        {
            JournalShort => 20,
            JournalMedium => 100,
            JournalLate => 300,
            JournalStress => 1000,
            _ => 100,
        };
    }
}
