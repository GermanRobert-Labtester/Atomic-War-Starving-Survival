using System;
using System.Collections.Generic;

namespace Ashfall.Core.Performance;

/// <summary>
/// Reusable measurement session: warm-up + measured iterations + statistics.
/// </summary>
public sealed class PerfSession : IDisposable
{
    private readonly List<PerfSample> _samples = new();
    private readonly List<PerfSample> _warmup = new();
    private bool _disposed;

    /// <summary>Workload context for this session.</summary>
    public PerfWorkloadContext Context { get; }

    /// <summary>Warm-up samples (not included in statistics).</summary>
    public IReadOnlyList<PerfSample> WarmupSamples => _warmup;

    /// <summary>Measured samples.</summary>
    public IReadOnlyList<PerfSample> MeasuredSamples => _samples;

    /// <summary>Statistics computed from measured samples.</summary>
    public PerfStatistics? Statistics { get; private set; }

    /// <summary>Whether allocation tracking is enabled.</summary>
    public bool TrackAllocations { get; }

    public PerfSession(PerfWorkloadContext context, bool trackAllocations = true)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        TrackAllocations = trackAllocations;
    }

    /// <summary>
    /// Run a warm-up iteration. Timing is recorded but excluded from results.
    /// </summary>
    public void Warmup(Action action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));
        if (_disposed) throw new ObjectDisposedException(nameof(PerfSession));

        var sw = PerfStopwatch.StartNew();
        long before = TrackAllocations ? GC.GetAllocatedBytesForCurrentThread() : 0;
        action();
        long after = TrackAllocations ? GC.GetAllocatedBytesForCurrentThread() : 0;

        var sample = new PerfSample
        {
            WorkloadId = Context.WorkloadId,
            ScaleTier = Context.RosterTier,
            Iteration = _warmup.Count,
            ElapsedMs = sw.Stop().ElapsedMilliseconds,
            HasAllocatedBytes = TrackAllocations,
            AllocatedBytes = TrackAllocations ? Math.Max(0, after - before) : 0,
            Platform = Context.Platform,
            BuildConfiguration = Context.BuildConfiguration,
            Runtime = Context.Runtime,
        };
        _warmup.Add(sample);
    }

    /// <summary>
    /// Run a measured iteration and record the sample.
    /// </summary>
    public PerfSample Measure(Action action, string? ownerId = null, int? phase = null)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));
        if (_disposed) throw new ObjectDisposedException(nameof(PerfSession));

        var sw = PerfStopwatch.StartNew();
        long before = TrackAllocations ? GC.GetAllocatedBytesForCurrentThread() : 0;
        action();
        long after = TrackAllocations ? GC.GetAllocatedBytesForCurrentThread() : 0;
        var elapsed = sw.Stop();

        var sample = new PerfSample
        {
            WorkloadId = Context.WorkloadId,
            ScaleTier = Context.RosterTier,
            Iteration = _samples.Count,
            ElapsedMs = elapsed.ElapsedMilliseconds,
            HasAllocatedBytes = TrackAllocations,
            AllocatedBytes = TrackAllocations ? Math.Max(0, after - before) : 0,
            OwnerId = ownerId,
            Phase = phase,
            Platform = Context.Platform,
            BuildConfiguration = Context.BuildConfiguration,
            Runtime = Context.Runtime,
        };
        _samples.Add(sample);
        return sample;
    }

    /// <summary>
    /// Compute statistics from measured samples.
    /// </summary>
    public PerfStatistics ComputeStatistics()
    {
        if (_disposed) throw new ObjectDisposedException(nameof(PerfSession));
        if (_samples.Count == 0)
            throw new InvalidOperationException("No measured samples to compute statistics.");

        var values = new double[_samples.Count];
        var allocated = new List<long>(_samples.Count);
        for (int i = 0; i < _samples.Count; i++)
        {
            values[i] = _samples[i].ElapsedMs;
            if (_samples[i].HasAllocatedBytes)
                allocated.Add(_samples[i].AllocatedBytes);
        }

        Statistics = new PerfStatistics(values, allocated.Count > 0 ? allocated : null);
        return Statistics;
    }

    /// <summary>
    /// Build a PerfResult from this session.
    /// </summary>
    public PerfResult ToResult(string benchmarkId, string thresholdClassification = "advisory",
        double? baselineMedianMs = null, string? slowestOwnerId = null, double? slowestOwnerMs = null,
        long? payloadBytes = null)
    {
        if (_disposed) throw new ObjectDisposedException(nameof(PerfSession));

        // Capture the result: the compiler cannot see that ComputeStatistics()
        // assigns the Statistics property, so reading it back was a possible null
        // dereference (CS8602).
        var stats = Statistics ?? ComputeStatistics();

        double? regressionPercent = null;
        if (baselineMedianMs.HasValue && stats.Median > 0)
        {
            regressionPercent = (stats.Median - baselineMedianMs.Value) / baselineMedianMs.Value * 100.0;
        }

        return new PerfResult
        {
            BenchmarkId = benchmarkId,
            Context = Context,
            WarmupCount = _warmup.Count,
            IterationCount = _samples.Count,
            Statistics = stats,
            Samples = new List<PerfSample>(_samples),
            ThresholdClassification = thresholdClassification,
            BaselineMedianMs = baselineMedianMs,
            RegressionPercent = regressionPercent,
            SlowestOwnerId = slowestOwnerId,
            SlowestOwnerMs = slowestOwnerMs,
            PayloadBytes = payloadBytes,
        };
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _samples.Clear();
        _warmup.Clear();
        Statistics = null;
    }
}
