using System;

namespace Ashfall.Core
{
    /// <summary>
    /// Master id list for the ASHFALL: THE HOLDFAST endings (Sprint 4 "Shelf &amp; endings").
    /// All ids come from docs/expansions/expansion_the_holdfast_plan.md §5.4
    /// (five endings, mutually exclusive — exactly one may be armed at a time).
    /// Do not invent ids outside this list; the Godot host guards SetEnding with IsKnown.
    /// </summary>
    public static class HoldfastEndings
    {
        public const string None = "";

        public const string Schedule = "ending_holdfast_schedule";
        public const string Reserve = "ending_holdfast_reserve";
        public const string DarkRoad = "ending_holdfast_dark_road";
        public const string Tender = "ending_holdfast_tender";
        public const string White = "ending_holdfast_white";

        /// <summary>Master list, order-preserved. The host cycles through this.</summary>
        public static readonly string[] All = { Schedule, Reserve, DarkRoad, Tender, White };

        public static bool IsKnown(string endingId)
        {
            if (string.IsNullOrEmpty(endingId)) return false;
            for (int i = 0; i < All.Length; i++)
                if (All[i] == endingId) return true;
            return false;
        }

        public static string DisplayName(string endingId)
        {
            switch (endingId)
            {
                case Schedule: return "The Schedule Holds";
                case Reserve: return "The Reserve";
                case DarkRoad: return "The Road Goes Dark";
                case Tender: return "Stand-Up";
                case White: return "The White";
                default: return endingId;
            }
        }
    }
}
