using System;

namespace Ashfall.Core.Performance;

/// <summary>
/// Describes the deterministic workload under measurement.
/// </summary>
public sealed class PerfWorkloadContext
{
    /// <summary>Canonical workload identifier.</summary>
    public string WorkloadId { get; set; } = string.Empty;

    /// <summary>Campaign age in days.</summary>
    public int CampaignDays { get; set; }

    /// <summary>Fixed master seed for deterministic construction.</summary>
    public int Seed { get; set; } = 9001;

    /// <summary>Roster scale tier.</summary>
    public string RosterTier { get; set; } = "normal";

    /// <summary>Catalog scale tier.</summary>
    public string CatalogTier { get; set; } = "normal";

    /// <summary>Journal scale tier.</summary>
    public string JournalTier { get; set; } = "normal";

    /// <summary>Expedition scale tier.</summary>
    public string ExpeditionTier { get; set; } = "normal";

    /// <summary>World-state scale tier.</summary>
    public string WorldStateTier { get; set; } = "normal";

    /// <summary>Build configuration (Debug/Release).</summary>
    public string BuildConfiguration { get; set; } = string.Empty;

    /// <summary>Platform identifier.</summary>
    public string Platform { get; set; } = string.Empty;

    /// <summary>Runtime identifier.</summary>
    public string Runtime { get; set; } = "dotnet";

    /// <summary>Optional description of the workload.</summary>
    public string Description { get; set; } = string.Empty;
}
