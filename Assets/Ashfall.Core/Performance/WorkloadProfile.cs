// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.Performance;

/// <summary>
/// Representative campaign workload profiles.
/// </summary>
public sealed class WorkloadProfile
{
    /// <summary>Profile name.</summary>
    public string Name { get; }

    /// <summary>Campaign days to advance.</summary>
    public int CampaignDays { get; }

    /// <summary>Default roster tier.</summary>
    public string RosterTier { get; }

    /// <summary>Default journal tier.</summary>
    public string JournalTier { get; }

    /// <summary>Default expedition tier.</summary>
    public string ExpeditionTier { get; }

    /// <summary>Default world-state tier.</summary>
    public string WorldStateTier { get; }

    /// <summary>Whether this profile includes save/load measurements.</summary>
    public bool IncludesPersistence { get; }

    /// <summary>Whether this profile includes leak/lifecycle scenarios.</summary>
    public bool IncludesLifecycle { get; }

    private WorkloadProfile(string name, int campaignDays, string rosterTier, string journalTier,
        string expeditionTier, string worldStateTier, bool includesPersistence, bool includesLifecycle)
    {
        Name = name;
        CampaignDays = campaignDays;
        RosterTier = rosterTier;
        JournalTier = journalTier;
        ExpeditionTier = expeditionTier;
        WorldStateTier = worldStateTier;
        IncludesPersistence = includesPersistence;
        IncludesLifecycle = includesLifecycle;
    }

    /// <summary>30-day early-game baseline profile.</summary>
    public static readonly WorkloadProfile Days30 = new WorkloadProfile(
        name: "30d",
        campaignDays: 30,
        rosterTier: ScaleTier.RosterNormal,
        journalTier: ScaleTier.JournalShort,
        expeditionTier: ScaleTier.ExpeditionTypical,
        worldStateTier: ScaleTier.WorldNormal,
        includesPersistence: true,
        includesLifecycle: false);

    /// <summary>180-day mid-game mature profile.</summary>
    public static readonly WorkloadProfile Days180 = new WorkloadProfile(
        name: "180d",
        campaignDays: 180,
        rosterTier: ScaleTier.RosterLarge,
        journalTier: ScaleTier.JournalMedium,
        expeditionTier: ScaleTier.ExpeditionHigh,
        worldStateTier: ScaleTier.WorldLarge,
        includesPersistence: true,
        includesLifecycle: true);

    /// <summary>360-day late-game stress profile.</summary>
    public static readonly WorkloadProfile Days360 = new WorkloadProfile(
        name: "360d",
        campaignDays: 360,
        rosterTier: ScaleTier.RosterStress,
        journalTier: ScaleTier.JournalStress,
        expeditionTier: ScaleTier.ExpeditionStress,
        worldStateTier: ScaleTier.WorldStress,
        includesPersistence: true,
        includesLifecycle: true);

    /// <summary>
    /// Build a profile for a caller-specified population tier. Used by the
    /// large-population soak benchmarks (50/100/250/500 survivors).
    /// </summary>
    public static WorkloadProfile Custom(string name, int campaignDays, string rosterTier,
        string journalTier = ScaleTier.JournalMedium,
        string expeditionTier = ScaleTier.ExpeditionTypical,
        string worldStateTier = ScaleTier.WorldNormal,
        bool includesPersistence = false,
        bool includesLifecycle = false)
        => new WorkloadProfile(name, campaignDays, rosterTier, journalTier,
            expeditionTier, worldStateTier, includesPersistence, includesLifecycle);

    /// <summary>50-survivor soak profile (90 in-game days).</summary>
    public static WorkloadProfile LargeShelter50 => Custom("large_50", 90, ScaleTier.RosterSquad);

    /// <summary>100-survivor soak profile (90 in-game days).</summary>
    public static WorkloadProfile LargeShelter100 => Custom("large_100", 90, ScaleTier.RosterCompany);

    /// <summary>250-survivor soak profile (90 in-game days).</summary>
    public static WorkloadProfile LargeShelter250 => Custom("large_250", 90, ScaleTier.RosterCrowd);

    /// <summary>500-survivor soak profile (90 in-game days).</summary>
    public static WorkloadProfile LargeShelter500 => Custom("large_500", 90, ScaleTier.RosterMass);

    /// <summary>Two in-game year replay profile (720 days) at the stress tier.</summary>
    public static WorkloadProfile LongReplay720 => Custom(
        "replay_720d", 720, ScaleTier.RosterStress,
        ScaleTier.JournalStress, ScaleTier.ExpeditionStress, ScaleTier.WorldStress);

    public override string ToString() => Name;
}
