using System;
using System.Collections.Generic;

namespace Ashfall.Core.Performance;

/// <summary>
/// Machine-readable performance result for one workload/operation.
/// </summary>
public sealed class PerfResult
{
    /// <summary>Schema version for this result format.</summary>
    public string SchemaVersion { get; set; } = "1.0.0";

    /// <summary>Benchmark identifier.</summary>
    public string BenchmarkId { get; set; } = string.Empty;

    /// <summary>Workload context.</summary>
    public PerfWorkloadContext Context { get; set; } = new PerfWorkloadContext();

    /// <summary>Warm-up iteration count.</summary>
    public int WarmupCount { get; set; }

    /// <summary>Measured iteration count.</summary>
    public int IterationCount { get; set; }

    /// <summary>Observed statistics.</summary>
    public PerfStatistics Statistics { get; set; } = null!;

    /// <summary>All raw measured samples.</summary>
    public List<PerfSample> Samples { get; set; } = new();

    /// <summary>Threshold classification (advisory or hard gate).</summary>
    public string ThresholdClassification { get; set; } = "advisory";

    /// <summary>Baseline value this result was compared against, if any.</summary>
    public double? BaselineMedianMs { get; set; }

    /// <summary>Regression percentage relative to baseline, if compared.</summary>
    public double? RegressionPercent { get; set; }

    /// <summary>Slowest owner/phase observed, if applicable.</summary>
    public string? SlowestOwnerId { get; set; }

    /// <summary>Latency of the slowest owner/phase in milliseconds.</summary>
    public double? SlowestOwnerMs { get; set; }

    /// <summary>Payload or state size in bytes, if measured.</summary>
    public long? PayloadBytes { get; set; }

    /// <summary>Whether this result passed all applicable checks.</summary>
    public bool Passed { get; set; } = true;

    /// <summary>Human-readable diagnostic message.</summary>
    public string? Message { get; set; }
}
