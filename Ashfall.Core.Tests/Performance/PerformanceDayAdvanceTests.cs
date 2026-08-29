using System;
using Xunit;
using Ashfall.Core.Performance;
using Ashfall.Core.Performance.Workloads;

namespace Ashfall.Core.Tests.Performance;

/// <summary>
/// Day-advance latency tests at 30/180/360 campaign-day scales.
/// </summary>
public class PerformanceDayAdvanceTests
{
    [Fact]
    public void AdvanceDays_30DayWorkload_CompletesWithinAdvisoryBound()
    {
        var ctx = new PerfWorkloadContext
        {
            WorkloadId = "perf_30d_advance",
            CampaignDays = 30,
            Seed = 9001,
            RosterTier = ScaleTier.RosterNormal,
            JournalTier = ScaleTier.JournalShort,
            ExpeditionTier = ScaleTier.ExpeditionTypical,
            WorldStateTier = ScaleTier.WorldNormal,
            BuildConfiguration = "Release",
            Platform = "Linux",
            Runtime = "dotnet",
        };

        using var harness = new PerformanceCampaignHarness(ctx);
        harness.AdvanceDays(Math.Min(3, ctx.CampaignDays));

        using var session = new PerfSession(ctx, trackAllocations: false);
        session.Measure(() => harness.AdvanceDays(ctx.CampaignDays));
        var stats = session.ComputeStatistics();

        Assert.True(stats.Median < 2000,
            $"30d day-advance median must be < 2s. Actual: {stats.Median:F1}ms");
    }

    [Fact]
    public void AdvanceDays_180DayWorkload_CompletesWithinAdvisoryBound()
    {
        var ctx = new PerfWorkloadContext
        {
            WorkloadId = "perf_180d_advance",
            CampaignDays = 180,
            Seed = 9001,
            RosterTier = ScaleTier.RosterLarge,
            JournalTier = ScaleTier.JournalMedium,
            ExpeditionTier = ScaleTier.ExpeditionHigh,
            WorldStateTier = ScaleTier.WorldLarge,
        };

        using var harness = new PerformanceCampaignHarness(ctx);
        harness.AdvanceDays(Math.Min(3, ctx.CampaignDays));

        using var session = new PerfSession(ctx, trackAllocations: false);
        session.Measure(() => harness.AdvanceDays(ctx.CampaignDays));
        var stats = session.ComputeStatistics();

        Assert.True(stats.Median < 12000,
            $"180d day-advance median must be < 12s. Actual: {stats.Median:F1}ms");
    }

    [Fact]
    public void AdvanceDays_360DayWorkload_CompletesWithinAdvisoryBound()
    {
        var ctx = new PerfWorkloadContext
        {
            WorkloadId = "perf_360d_advance",
            CampaignDays = 360,
            Seed = 9001,
            RosterTier = ScaleTier.RosterStress,
            JournalTier = ScaleTier.JournalStress,
            ExpeditionTier = ScaleTier.ExpeditionStress,
            WorldStateTier = ScaleTier.WorldStress,
        };

        using var harness = new PerformanceCampaignHarness(ctx);
        harness.AdvanceDays(Math.Min(3, ctx.CampaignDays));

        using var session = new PerfSession(ctx, trackAllocations: false);
        session.Measure(() => harness.AdvanceDays(ctx.CampaignDays));
        var stats = session.ComputeStatistics();

        Assert.True(stats.Median < 30000,
            $"360d day-advance median must be < 30s. Actual: {stats.Median:F1}ms");
    }

    [Fact]
    public void AdvanceDays_SameSeed_ProducesSameLatency()
    {
        const int seed = 4242;
        const int days = 30;

        using var harnessA = BuildHarness(seed, days);
        using var harnessB = BuildHarness(seed, days);

        harnessA.AdvanceDays(Math.Min(3, days));
        harnessB.AdvanceDays(Math.Min(3, days));

        double msA = harnessA.AdvanceDays(days);
        double msB = harnessB.AdvanceDays(days);

        double larger = Math.Max(msA, msB);
        double smaller = Math.Min(msA, msB);
        Assert.True(larger == 0 || (larger - smaller) / larger < 0.20,
            string.Format("Same-seed workloads must produce similar latency (within 20%). Actual: {0:F1}ms vs {1:F1}ms", msA, msB));
    }

    [Fact]
    public void AdvanceDays_P95_IsBoundedFor30DayWorkload()
    {
        var ctx = new PerfWorkloadContext
        {
            WorkloadId = "perf_30d_p95",
            CampaignDays = 30,
            Seed = 9001,
            RosterTier = ScaleTier.RosterNormal,
            JournalTier = ScaleTier.JournalShort,
            ExpeditionTier = ScaleTier.ExpeditionTypical,
            WorldStateTier = ScaleTier.WorldNormal,
        };

        using var harness = new PerformanceCampaignHarness(ctx);
        harness.AdvanceDays(Math.Min(3, ctx.CampaignDays));

        using var session = new PerfSession(ctx, trackAllocations: false);
        for (int i = 0; i < 8; i++)
            session.Measure(() => harness.AdvanceDays(ctx.CampaignDays));

        var stats = session.ComputeStatistics();
        Assert.True(stats.P95 < Math.Max(stats.Median * 6, 10.0),
            $"P95 must not diverge wildly from median. Median={stats.Median:F1}ms, P95={stats.P95:F1}ms");
    }

    private static PerformanceCampaignHarness BuildHarness(int seed, int days)
    {
        var ctx = new PerfWorkloadContext
        {
            WorkloadId = $"perf_seed_latency_{days}d",
            CampaignDays = days,
            Seed = seed,
            RosterTier = ScaleTier.RosterNormal,
            JournalTier = ScaleTier.JournalShort,
            ExpeditionTier = ScaleTier.ExpeditionTypical,
            WorldStateTier = ScaleTier.WorldNormal,
        };
        return new PerformanceCampaignHarness(ctx);
    }
}
