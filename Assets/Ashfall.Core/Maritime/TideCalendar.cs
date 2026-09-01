using System;

namespace Ashfall.Core.Maritime
{
    /// <summary>Deterministic tide phase, derived from campaign day (no wall clock, no RNG).</summary>
    public enum TidePhase
    {
        Low = 0,
        Rising = 1,
        High = 2,
        Falling = 3
    }

    /// <summary>Authored tide-window kinds a dive site may gate its launch on.</summary>
    public enum TideWindow
    {
        Any = 0,
        Slack = 1,        // turning water only (Rising or Falling)
        LowOnly = 2,      // shallows exposed
        HighOnly = 3,     // deep-water approach
        FallingOnly = 4,  // narrow entry drains open
        UnsafeAtPeak = 5  // closed during peak flow (Rising)
    }

    /// <summary>
    /// ASHFALL Plan 23 — deterministic tide calendar for the Drowned Coast.
    /// Derives phase purely from the authoritative campaign day (4-day cycle,
    /// two tidal turns): day%4 → Low, Rising, High, Falling. No wall clock, no
    /// RNG, no serialized state — same day means same tide in every host and
    /// every save. Old saves (no day authority) default to ungated.
    /// </summary>
    public static class TideCalendar
    {
        /// <summary>Days per full tidal cycle (two low/high turns).</summary>
        public const int CycleDays = 4;

        public static TidePhase PhaseFor(int campaignDay)
        {
            if (campaignDay < 0) return TidePhase.High; // ungated fallback for pre-day callers
            return (TidePhase)(campaignDay % CycleDays);
        }

        public static string PhaseName(TidePhase phase) => phase switch
        {
            TidePhase.Low => "Low Tide",
            TidePhase.Rising => "Rising Tide",
            TidePhase.High => "High Tide",
            TidePhase.Falling => "Falling Tide",
            _ => "Any Tide"
        };

        /// <summary>True when the authored window admits a launch on the given campaign day.</summary>
        public static bool IsWindowOpen(TideWindow window, int campaignDay)
        {
            if (window == TideWindow.Any) return true;
            if (campaignDay < 0) return true; // no authoritative day: legacy saves stay ungated
            return WindowAdmits(window, PhaseFor(campaignDay));
        }

        /// <summary>Days until the window next opens (0 when open now).</summary>
        public static int DaysUntilOpen(TideWindow window, int campaignDay)
        {
            if (IsWindowOpen(window, campaignDay)) return 0;
            for (int i = 1; i <= CycleDays; i++)
                if (WindowAdmits(window, PhaseFor(campaignDay + i))) return i;
            return 0;
        }

        private static bool WindowAdmits(TideWindow window, TidePhase phase)
        {
            switch (window)
            {
                case TideWindow.Any: return true;
                case TideWindow.Slack: return phase == TidePhase.Rising || phase == TidePhase.Falling;
                case TideWindow.LowOnly: return phase == TidePhase.Low;
                case TideWindow.HighOnly: return phase == TidePhase.High;
                case TideWindow.FallingOnly: return phase == TidePhase.Falling;
                case TideWindow.UnsafeAtPeak: return phase != TidePhase.Rising;
                default: return true;
            }
        }
    }
}
