using System;
using Xunit;
using Ashfall.Core.Performance;
using Ashfall.Core.Performance.Workloads;

namespace Ashfall.Core.Tests.Performance;

/// <summary>
/// Panel/lifecycle and session-discipline tests.
/// Validates that measurement sessions are reusable across workloads.
/// </summary>
public class PerformanceLifecycleTests
{
    [Fact]
    public void Harness_Dispose_CleansUpAndAllowsMultipleInstances()
    {
        var ctx = new PerfWorkloadContext { WorkloadId = "lifecycle_dispose", CampaignDays = 5, Seed = 9001 };
        using var harness = new PerformanceCampaignHarness(ctx);
        harness.AdvanceDays(ctx.CampaignDays);

        harness.Dispose();
        Assert.Throws<ObjectDisposedException>(() => harness.AdvanceDays(1));
    }

    [Fact]
    public void Session_MultipleWorkloads_CanRunFromSameSessionTemplate()
    {
        var template = new PerfWorkloadContext
        {
            WorkloadId = "lifecycle_template",
            Seed = 9001,
            Platform = "Linux",
            BuildConfiguration = "Release",
            Runtime = "dotnet",
        };

        using var sessionA = new PerfSession(template, trackAllocations: false);
        sessionA.Measure(() => { });
        var statsA = sessionA.ComputeStatistics();
        Assert.Equal(1, statsA.Count);

        using var sessionB = new PerfSession(template, trackAllocations: false);
        sessionB.Measure(() => { });
        sessionB.Measure(() => { });
        var statsB = sessionB.ComputeStatistics();
        Assert.Equal(2, statsB.Count);
    }

    [Fact]
    public void Harness_30DayLifecycle_RetainedMemoryIsWithinBound()
    {
        const int days = 30;
        var ctx = new PerfWorkloadContext
        {
            WorkloadId = $"lifecycle_retained_{days}d",
            CampaignDays = days,
            Seed = 9001,
            RosterTier = ScaleTier.RosterNormal,
            JournalTier = ScaleTier.JournalShort,
            ExpeditionTier = ScaleTier.ExpeditionTypical,
            WorldStateTier = ScaleTier.WorldNormal,
        };

        long before = GC.GetTotalMemory(forceFullCollection: true);
        using var harness = new PerformanceCampaignHarness(ctx);
        harness.AdvanceDays(ctx.CampaignDays);
        harness.Dispose();

        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        long after = GC.GetTotalMemory(forceFullCollection: true);

        Assert.True((after - before) < 20_000_000L,
            "Retained memory must stay within bound.");
    }

    [Fact]
    public void Harness_PersistenceGate_30Day_CompletesWithin500MS()
    {
        var ctx = new PerfWorkloadContext
        {
            WorkloadId = "lifecycle_save_30d",
            CampaignDays = 30,
            Seed = 9001,
            RosterTier = ScaleTier.RosterNormal,
            JournalTier = ScaleTier.JournalShort,
            ExpeditionTier = ScaleTier.ExpeditionTypical,
            WorldStateTier = ScaleTier.WorldNormal,
        };

        using var harness = new PerformanceCampaignHarness(ctx);
        harness.AdvanceDays(ctx.CampaignDays);

        double latencyMs = harness.MeasureSaveLatency();
        Assert.True(latencyMs < 500, $"Save latency must be < 500ms. Actual: {latencyMs:F1}ms");
    }

    [Fact]
    public void Harness_AllocGrowth_CompletesWithinMemoryBound()
    {
        var ctx = new PerfWorkloadContext
        {
            WorkloadId = "lifecycle_alloc",
            CampaignDays = 30,
            Seed = 9001,
            RosterTier = ScaleTier.RosterNormal,
            JournalTier = ScaleTier.JournalShort,
            ExpeditionTier = ScaleTier.ExpeditionTypical,
            WorldStateTier = ScaleTier.WorldNormal,
        };

        using var harness = new PerformanceCampaignHarness(ctx);
        harness.AdvanceDays(3);

        using var session = new PerfSession(ctx, trackAllocations: true);
        for (int i = 0; i < 8; i++)
            session.Measure(() => harness.AdvanceDays(1));

        var stats = session.ComputeStatistics();
        Assert.True(stats.MedianAllocatedBytes < 5_000_000,
            $"Per-day allocation median must be < 5MB. Actual: {stats.MedianAllocatedBytes:N0} bytes");
    }
}
