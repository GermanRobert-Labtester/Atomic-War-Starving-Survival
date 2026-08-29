using System;
using Xunit;
using Ashfall.Core.Performance;
using Ashfall.Core.Performance.Workloads;

namespace Ashfall.Core.Tests.Performance;

/// <summary>
/// Memory and lifecycle tests: allocation bounds and retained-memory checks.
/// </summary>
public class PerformanceMemoryTests
{
    [Fact]
    public void AdvanceDays_PerIterationAllocation_GrowthIsBounded()
    {
        const int seed = 9001;
        using var harness = BuildHarness(seed, 30, ScaleTier.RosterNormal, ScaleTier.JournalShort, ScaleTier.ExpeditionTypical, ScaleTier.WorldNormal);
        harness.AdvanceDays(Math.Min(3, 30));

        var ctx = BuildContext(30, ScaleTier.RosterNormal, ScaleTier.JournalShort, ScaleTier.ExpeditionTypical, ScaleTier.WorldNormal);
        using var session = new PerfSession(ctx, trackAllocations: true);

        for (int i = 0; i < 8; i++)
            session.Measure(() => harness.AdvanceDays(1));

        var stats = session.ComputeStatistics();
        Assert.True(stats.AllocationSampleCount > 0, "Allocation samples should be captured when tracking allocations.");
        Assert.True(stats.MedianAllocatedBytes < 5_000_000,
            $"Per-day allocation median must be < 5MB. Actual: {stats.MedianAllocatedBytes:N0} bytes");
    }

    [Theory]
    [InlineData(ScaleTier.RosterNormal, ScaleTier.JournalShort)]
    [InlineData(ScaleTier.RosterLarge, ScaleTier.JournalMedium)]
    public void Lifecycle_RetainedMemory_AfterNewGame_IsWithinBound(string rosterTier, string journalTier)
    {
        var ctx = BuildContext(30, rosterTier, journalTier, ScaleTier.ExpeditionTypical, ScaleTier.WorldNormal);
        long before = GC.GetTotalMemory(forceFullCollection: true);
        using var harness = new PerformanceCampaignHarness(ctx);
        harness.AdvanceDays(ctx.CampaignDays);
        harness.Dispose();

        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        long after = GC.GetTotalMemory(forceFullCollection: true);
        long retained = after - before;

        Assert.True(retained < 20_000_000L,
            $"Retained memory after lifecycle must be < 20MB. Actual: {retained / 1_000_000}MB");
    }

    [Fact]
    public void Lifecycle_MultipleHarnesses_DoNotLeakIndefinitely()
    {
        const int iterations = 3;
        long baseline = GC.GetTotalMemory(forceFullCollection: true);
        long maxRetained = baseline;

        for (int i = 0; i < iterations; i++)
        {
            var ctx = BuildContext(30, ScaleTier.RosterNormal, ScaleTier.JournalShort, ScaleTier.ExpeditionTypical, ScaleTier.WorldNormal);
            using var harness = new PerformanceCampaignHarness(ctx);
            harness.AdvanceDays(ctx.CampaignDays);
            harness.Dispose();
        }

        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        maxRetained = Math.Max(maxRetained, GC.GetTotalMemory(forceFullCollection: true));

        long leaked = maxRetained - baseline;
        Assert.True(leaked < 10_000_000L,
            $"Repeated lifecycle must not leak > 10MB. Actual leaked: {leaked / 1_000_000}MB");
    }

    [Fact]
    public void Session_AllocationTracking_DoesNotAffectMeasuredLatency()
    {
        var ctx = new PerfWorkloadContext { WorkloadId = "alloc_impact", Seed = 1 };
        using var sessionOff = new PerfSession(ctx, trackAllocations: false);
        using var sessionOn = new PerfSession(ctx, trackAllocations: true);

        sessionOff.Measure(() => { });
        sessionOn.Measure(() => { });

        var statsOff = sessionOff.ComputeStatistics();
        var statsOn = sessionOn.ComputeStatistics();
        double ratio = statsOn.Median / Math.Max(0.01, statsOff.Median);
        Assert.True(ratio < 2.5, "Allocation tracking must not increase median latency by more than 2.5x.");
    }

    private static PerformanceCampaignHarness BuildHarness(
        int seed, int days, string rosterTier, string journalTier, string expeditionTier, string worldStateTier)
    {
        var ctx = new PerfWorkloadContext
        {
            WorkloadId = $"perf_mem_{days}d",
            CampaignDays = days,
            Seed = seed,
            RosterTier = rosterTier,
            JournalTier = journalTier,
            ExpeditionTier = expeditionTier,
            WorldStateTier = worldStateTier,
        };
        return new PerformanceCampaignHarness(ctx);
    }

    private static PerfWorkloadContext BuildContext(
        int days, string rosterTier, string journalTier, string expeditionTier, string worldStateTier)
    {
        return new PerfWorkloadContext
        {
            WorkloadId = $"perf_mem_{days}d",
            CampaignDays = days,
            Seed = 9001,
            RosterTier = rosterTier,
            JournalTier = journalTier,
            ExpeditionTier = expeditionTier,
            WorldStateTier = worldStateTier,
        };
    }
}
