using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Performance;

/// <summary>
/// Stable distribution statistics for a measured sample set.
/// </summary>
public sealed class PerfStatistics
{
    /// <summary>Number of samples.</summary>
    public int Count { get; }

    /// <summary>Minimum observed value.</summary>
    public double Minimum { get; }

    /// <summary>Median observed value.</summary>
    public double Median { get; }

    /// <summary>Mean observed value.</summary>
    public double Mean { get; }

    /// <summary>P95 observed value (or Maximum when sample count is too small).</summary>
    public double P95 { get; }

    /// <summary>Maximum observed value.</summary>
    public double Maximum { get; }

    /// <summary>Standard deviation (population, or 0 when sample count < 2).</summary>
    public double StdDev { get; }

    /// <summary>Total allocated bytes across all samples where captured.</summary>
    public long TotalAllocatedBytes { get; }

    /// <summary>Number of samples that included allocation data.</summary>
    public int AllocationSampleCount { get; }

    /// <summary>Median allocated bytes, or 0 when no allocation samples.</summary>
    public long MedianAllocatedBytes { get; }

    public PerfStatistics(IReadOnlyList<double> values, IReadOnlyList<long>? allocatedBytes = null)
    {
        if (values == null || values.Count == 0)
            throw new ArgumentException("Values must contain at least one sample.", nameof(values));

        Count = values.Count;
        var sorted = values.ToArray();
        Array.Sort(sorted);

        Minimum = sorted[0];
        Maximum = sorted[^1];
        Mean = sorted.Average();

        Median = Count % 2 == 0
            ? (sorted[Count / 2 - 1] + sorted[Count / 2]) / 2.0
            : sorted[Count / 2];

        P95 = ComputePercentile(sorted, 0.95);

        if (Count > 1)
        {
            double sumSq = 0;
            foreach (double v in sorted)
            {
                double diff = v - Mean;
                sumSq += diff * diff;
            }
            StdDev = Math.Sqrt(sumSq / Count);
        }
        else
        {
            StdDev = 0.0;
        }

        if (allocatedBytes != null && allocatedBytes.Count > 0)
        {
            AllocationSampleCount = allocatedBytes.Count;
            long total = 0;
            foreach (long b in allocatedBytes)
            {
                if (b >= 0)
                    total += b;
            }
            TotalAllocatedBytes = total;

            var sortedBytes = new long[allocatedBytes.Count];
            for (int i = 0; i < sortedBytes.Length; i++)
                sortedBytes[i] = allocatedBytes[i] < 0 ? 0 : allocatedBytes[i];
            Array.Sort(sortedBytes);
            MedianAllocatedBytes = sortedBytes.Length % 2 == 0
                ? (sortedBytes[sortedBytes.Length / 2 - 1] + sortedBytes[sortedBytes.Length / 2]) / 2L
                : sortedBytes[sortedBytes.Length / 2];
        }
        else
        {
            AllocationSampleCount = 0;
            TotalAllocatedBytes = 0;
            MedianAllocatedBytes = 0;
        }
    }

    /// <summary>Parameterless constructor for JSON deserialization.</summary>
    public PerfStatistics() { }

    private static double ComputePercentile(double[] sorted, double percentile)
    {
        if (sorted.Length == 1)
            return sorted[0];

        double rank = percentile * (sorted.Length - 1);
        int lower = (int)Math.Floor(rank);
        int upper = (int)Math.Ceiling(rank);

        if (lower == upper)
            return sorted[lower];

        double fraction = rank - lower;
        return sorted[lower] * (1 - fraction) + sorted[upper] * fraction;
    }
}
