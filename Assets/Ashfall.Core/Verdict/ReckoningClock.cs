// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Endgame;

namespace Ashfall.Core.Verdict
{
    /// <summary>
    /// Boundary adapter for translating campaign simulation day to/from authored Verdict day (DEC-Y2-13).
    /// Shifts timing through a pure offset without mutating authored quest, radio, or evidence day gates.
    /// </summary>
    public static class ReckoningClock
    {
        public static int ToVerdictDay(int campaignDay, int offset)
        {
            return campaignDay - offset;
        }

        public static int ToCampaignDay(int verdictDay, int offset)
        {
            return verdictDay + offset;
        }

        public static int ToVerdictDay(int campaignDay, ChapterProfileDef? profile)
        {
            if (profile == null) return campaignDay;
            return ToVerdictDay(campaignDay, profile.reckoning_offset);
        }

        public static int ToCampaignDay(int verdictDay, ChapterProfileDef? profile)
        {
            if (profile == null) return verdictDay;
            return ToCampaignDay(verdictDay, profile.reckoning_offset);
        }
    }
}
