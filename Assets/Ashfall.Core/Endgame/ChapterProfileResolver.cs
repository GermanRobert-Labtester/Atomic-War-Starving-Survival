// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Factions;

namespace Ashfall.Core.Endgame
{
    public sealed class ChapterProfileResolutionContext
    {
        public string? PreferredProfileId { get; set; }
        public FactionBranchKind FactionBranchKind { get; set; } = FactionBranchKind.None;
        public string CommittedFactionId { get; set; } = string.Empty;
        public bool HasMusterApproach { get; set; }
        public bool IsStandingD { get; set; }
    }

    /// <summary>
    /// Pure, deterministic resolver for selecting a campaign's Chapter Profile.
    /// Never mutates state or introduces RNG; preserves legacy default when no branch active.
    /// </summary>
    public static class ChapterProfileResolver
    {
        public static string ResolveProfileId(ChapterProfileResolutionContext? context)
        {
            if (context == null) return ChapterProfileCatalog.DefaultProfileId;

            if (!string.IsNullOrWhiteSpace(context.PreferredProfileId))
                return context.PreferredProfileId;

            if (context.IsStandingD)
                return "profile_standing_d";

            if (context.FactionBranchKind == FactionBranchKind.Military ||
                string.Equals(context.CommittedFactionId, "faction_military", StringComparison.OrdinalIgnoreCase))
            {
                return "profile_military";
            }

            if (context.FactionBranchKind == FactionBranchKind.Rebel ||
                string.Equals(context.CommittedFactionId, "faction_rebel", StringComparison.OrdinalIgnoreCase))
            {
                return "profile_rebel";
            }

            if (context.FactionBranchKind == FactionBranchKind.Independent ||
                string.Equals(context.CommittedFactionId, "faction_independent", StringComparison.OrdinalIgnoreCase))
            {
                return "profile_independent";
            }

            if (context.HasMusterApproach)
            {
                return "profile_muster";
            }

            return ChapterProfileCatalog.DefaultProfileId;
        }
    }
}
