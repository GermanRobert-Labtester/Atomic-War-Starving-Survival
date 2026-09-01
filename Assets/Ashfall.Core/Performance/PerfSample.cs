using System;

namespace Ashfall.Core.Performance;

/// <summary>
/// Single measured sample from one iteration of an operation.
/// </summary>
public sealed class PerfSample
{
    /// <summary>Workload identifier.</summary>
    public string WorkloadId { get; set; } = string.Empty;

    /// <summary>Scale tier name.</summary>
    public string ScaleTier { get; set; } = string.Empty;

    /// <summary>Iteration index within the measured batch (0-based).</summary>
    public int Iteration { get; set; }

    /// <summary>Elapsed wall-clock time in milliseconds.</summary>
    public double ElapsedMs { get; set; }

    /// <summary>Allocated bytes for this iteration, if captured.</summary>
    public long AllocatedBytes { get; set; }

    /// <summary>True when allocated bytes were successfully captured.</summary>
    public bool HasAllocatedBytes { get; set; }

    /// <summary>Optional owner/phase label for day-advance measurements.</summary>
    public string? OwnerId { get; set; }

    /// <summary>Optional phase number.</summary>
    public int? Phase { get; set; }

    /// <summary>Platform identifier (e.g. Linux, Windows, macOS).</summary>
    public string Platform { get; set; } = string.Empty;

    /// <summary>Build configuration (Debug/Release).</summary>
    public string BuildConfiguration { get; set; } = string.Empty;

    /// <summary>Runtime identifier (e.g. dotnet, godot).</summary>
    public string Runtime { get; set; } = string.Empty;
}
