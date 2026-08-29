using System;
using Xunit;
using Ashfall.Core.Performance;
using Ashfall.Core.Performance.Workloads;

namespace Ashfall.Core.Tests.Performance;

/// <summary>
/// Framework-correctness tests for the Task 130 performance infrastructure.
/// These tests validate the measurement plumbing independently of any gameplay system.
/// </summary>
public class PerformanceFrameworkTests
{
    [Fact]
    public void PerfStopwatch_ElapsedMilliseconds_IsPositive()
    {
        using var sw = PerfStopwatch.StartNew();
        System.Threading.Thread.Sleep(1);
        var elapsed = sw.Stop().ElapsedMilliseconds;
        Assert.True(elapsed > 0, "Stopwatch elapsed time must be positive.");
    }

    [Fact]
    public void PerfSession_WarmupExcludedFromStatistics()
    {
        var context = new PerfWorkloadContext { WorkloadId = "warmup_test", Seed = 1 };
        using var session = new PerfSession(context, trackAllocations: false);

        session.Warmup(() => { });
        session.Warmup(() => { });
        session.Measure(() => { });
        session.Measure(() => { });
        session.Measure(() => { });

        var stats = session.ComputeStatistics();
        Assert.Equal(3, stats.Count);
        Assert.Equal(3, session.MeasuredSamples.Count);
        Assert.Equal(2, session.WarmupSamples.Count);
    }

    [Fact]
    public void PerfSession_Statistics_MedianAndMaxAreCorrect()
    {
        var context = new PerfWorkloadContext { WorkloadId = "stats_test", Seed = 1 };
        using var session = new PerfSession(context, trackAllocations: false);

        session.Measure(() => { }); // ~0 ms
        session.Measure(() => { }); // ~0 ms
        System.Threading.Thread.Sleep(2);
        session.Measure(() => { }); // ~2 ms
        System.Threading.Thread.Sleep(3);
        session.Measure(() => { }); // ~5 ms

        var stats = session.ComputeStatistics();
        Assert.Equal(4, stats.Count);
        Assert.True(stats.Maximum >= stats.Median, "Maximum must be >= median.");
        Assert.True(stats.Median >= stats.Minimum, "Median must be >= minimum.");
        Assert.True(stats.P95 >= stats.Median, "P95 must be >= median.");
    }

    [Fact]
    public void PerfSession_AllocationTracking_RecordsBytes()
    {
        var context = new PerfWorkloadContext { WorkloadId = "alloc_test", Seed = 1 };
        using var session = new PerfSession(context, trackAllocations: true);

        session.Measure(() =>
        {
            var list = new System.Collections.Generic.List<int>(1000);
            for (int i = 0; i < 1000; i++) list.Add(i);
        });

        var stats = session.ComputeStatistics();
        Assert.Equal(1, stats.AllocationSampleCount);
        Assert.True(stats.TotalAllocatedBytes > 0, "Allocated bytes must be positive when tracking allocations.");
        Assert.True(stats.MedianAllocatedBytes > 0, "Median allocated bytes must be positive.");
    }

    [Fact]
    public void PerfResult_SerializesToValidJson()
    {
        var context = new PerfWorkloadContext
        {
            WorkloadId = "json_test",
            CampaignDays = 30,
            Seed = 9001,
            Platform = "Linux",
            BuildConfiguration = "Release",
            Runtime = "dotnet",
        };

        using var session = new PerfSession(context, trackAllocations: false);
        session.Measure(() => { });
        var result = session.ToResult("day_advance_30d", "advisory");

        var json = System.Text.Json.JsonSerializer.Serialize(result);
        Assert.False(string.IsNullOrWhiteSpace(json));

        var deserialized = System.Text.Json.JsonSerializer.Deserialize<PerfResult>(json);
        Assert.NotNull(deserialized);
        Assert.Equal("day_advance_30d", deserialized.BenchmarkId);
        Assert.Equal("advisory", deserialized.ThresholdClassification);
        Assert.Equal(1, deserialized.IterationCount);
    }

    [Fact]
    public void PerfResult_RegressionPercent_ComputesCorrectly()
    {
        var context = new PerfWorkloadContext { WorkloadId = "regen_test", Seed = 1 };
        using var session = new PerfSession(context, trackAllocations: false);
        session.Measure(() => { });
        var result = session.ToResult("regen", baselineMedianMs: 10.0);

        Assert.NotNull(result.RegressionPercent);
        Assert.True(result.RegressionPercent.Value >= -100, "Regression percent should be bounded.");
    }

    [Fact]
    public void WorkloadProfile_Properties_AreCorrect()
    {
        Assert.Equal(30, WorkloadProfile.Days30.CampaignDays);
        Assert.Equal(ScaleTier.RosterNormal, WorkloadProfile.Days30.RosterTier);
        Assert.True(WorkloadProfile.Days30.IncludesPersistence);
        Assert.False(WorkloadProfile.Days30.IncludesLifecycle);

        Assert.Equal(180, WorkloadProfile.Days180.CampaignDays);
        Assert.Equal(360, WorkloadProfile.Days360.CampaignDays);
    }

    [Fact]
    public void ScaleTier_RosterCount_ReturnsExpectedValues()
    {
        Assert.Equal(3, ScaleTier.RosterCount(ScaleTier.RosterSmall));
        Assert.Equal(6, ScaleTier.RosterCount(ScaleTier.RosterNormal));
        Assert.Equal(12, ScaleTier.RosterCount(ScaleTier.RosterLarge));
        Assert.Equal(24, ScaleTier.RosterCount(ScaleTier.RosterStress));
    }

    [Fact]
    public void ScaleTier_JournalEntryCount_ReturnsExpectedValues()
    {
        Assert.Equal(20, ScaleTier.JournalEntryCount(ScaleTier.JournalShort));
        Assert.Equal(100, ScaleTier.JournalEntryCount(ScaleTier.JournalMedium));
        Assert.Equal(300, ScaleTier.JournalEntryCount(ScaleTier.JournalLate));
        Assert.Equal(1000, ScaleTier.JournalEntryCount(ScaleTier.JournalStress));
    }

    [Fact]
    public void PerfWorkloadContext_Defaults_AreStable()
    {
        var ctx = new PerfWorkloadContext { WorkloadId = "defaults_test" };
        Assert.Equal(9001, ctx.Seed);
        Assert.Equal(ScaleTier.RosterNormal, ctx.RosterTier);
        Assert.Equal("dotnet", ctx.Runtime);
    }
}
