using System;
using System.Diagnostics;

namespace Ashfall.Core.Performance;

/// <summary>
/// Monotonic performance timer based on Stopwatch.GetTimestamp().
/// Avoids wall-clock adjustments and provides high-resolution elapsed ticks.
/// </summary>
public readonly struct PerfStopwatch : IDisposable
{
    public static long Frequency => Stopwatch.Frequency;
    public static double TicksToNanoseconds(long ticks) => (double)ticks / Frequency * 1_000_000_000L;
    public static double TicksToMicroseconds(long ticks) => (double)ticks / Frequency * 1_000_000L;
    public static double TicksToMilliseconds(long ticks) => (double)ticks / Frequency * 1_000L;

    private readonly long _start;
    private readonly long _elapsed;

    private PerfStopwatch(long start, long elapsed)
    {
        _start = start;
        _elapsed = elapsed;
    }

    /// <summary>Create and start a new stopwatch.</summary>
    public static PerfStopwatch StartNew()
    {
        long start = Stopwatch.GetTimestamp();
        return new PerfStopwatch(start, 0L);
    }

    /// <summary>Elapsed ticks since StartNew.</summary>
    public long ElapsedTicks => _elapsed == 0L ? Stopwatch.GetTimestamp() - _start : _elapsed;

    /// <summary>Elapsed milliseconds.</summary>
    public double ElapsedMilliseconds => TicksToMilliseconds(ElapsedTicks);

    /// <summary>Elapsed microseconds.</summary>
    public double ElapsedMicroseconds => TicksToMicroseconds(ElapsedTicks);

    /// <summary>Elapsed nanoseconds.</summary>
    public double ElapsedNanoseconds => TicksToNanoseconds(ElapsedTicks);

    /// <summary>Stop the stopwatch and freeze elapsed time.</summary>
    public PerfStopwatch Stop()
    {
        long now = Stopwatch.GetTimestamp();
        return new PerfStopwatch(_start, now - _start);
    }

    public void Dispose()
    {
        // No-op; struct is immutable.
    }
}
