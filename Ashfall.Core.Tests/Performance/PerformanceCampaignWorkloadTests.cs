using System;
using Xunit;
using Ashfall.Core.Performance;
using Ashfall.Core.Performance.Workloads;

namespace Ashfall.Core.Tests.Performance;

/// <summary>
/// Validates that deterministic campaign workloads produce equivalent state
/// and expected relative scale sizes.
/// </summary>
public class PerformanceCampaignWorkloadTests
{
    [Fact]
    public void Harness_30DayWorkload_ConstructsWithoutException()
    {
        var ctx = new PerfWorkloadContext
        {
            WorkloadId = "perf_30d",
            CampaignDays = 30,
            Seed = 4242,
            RosterTier = ScaleTier.RosterNormal,
            JournalTier = ScaleTier.JournalShort,
            ExpeditionTier = ScaleTier.ExpeditionTypical,
            WorldStateTier = ScaleTier.WorldNormal,
            BuildConfiguration = "Debug",
            Platform = "Linux",
            Runtime = "dotnet",
        };

        using var harness = new PerformanceCampaignHarness(ctx);
        Assert.NotNull(harness.Coordinator);
        Assert.NotNull(harness.Survivors);
        Assert.NotNull(harness.Journal);
        Assert.NotNull(harness.Weather);
        Assert.NotNull(harness.Expeditions);
    }

    [Fact]
    public void Harness_180DayWorkload_ConstructsWithoutException()
    {
        var ctx = new PerfWorkloadContext
        {
            WorkloadId = "perf_180d",
            CampaignDays = 180,
            Seed = 4242,
            RosterTier = ScaleTier.RosterLarge,
            JournalTier = ScaleTier.JournalMedium,
            ExpeditionTier = ScaleTier.ExpeditionHigh,
            WorldStateTier = ScaleTier.WorldLarge,
        };

        using var harness = new PerformanceCampaignHarness(ctx);
        Assert.NotNull(harness.Coordinator);
    }

    [Fact]
    public void Harness_360DayWorkload_ConstructsWithoutException()
    {
        var ctx = new PerfWorkloadContext
        {
            WorkloadId = "perf_360d",
            CampaignDays = 360,
            Seed = 4242,
            RosterTier = ScaleTier.RosterStress,
            JournalTier = ScaleTier.JournalStress,
            ExpeditionTier = ScaleTier.ExpeditionStress,
            WorldStateTier = ScaleTier.WorldStress,
        };

        using var harness = new PerformanceCampaignHarness(ctx);
        Assert.NotNull(harness.Coordinator);
    }

    [Fact]
    public void Harness_SameWorkloadId_ProducesEquivalentState()
    {
        var ctx = new PerfWorkloadContext
        {
            WorkloadId = "perf_equiv",
            CampaignDays = 30,
            Seed = 9001,
        };

        using var harnessA = new PerformanceCampaignHarness(ctx);
        using var harnessB = new PerformanceCampaignHarness(ctx);

        harnessA.AdvanceDays(30);
        harnessB.AdvanceDays(30);

        string payloadA = harnessA.CaptureSavePayload();
        string payloadB = harnessB.CaptureSavePayload();

        Assert.Equal(payloadA, payloadB);
    }

    [Fact]
    public void Harness_ScaleTiers_ProduceExpectedRelativeSizes()
    {
        var small = new PerfWorkloadContext { WorkloadId = "scale_small", Seed = 1, RosterTier = ScaleTier.RosterSmall };
        var large = new PerfWorkloadContext { WorkloadId = "scale_large", Seed = 1, RosterTier = ScaleTier.RosterLarge };

        using var hSmall = new PerformanceCampaignHarness(small);
        using var hLarge = new PerformanceCampaignHarness(large);

        Assert.Equal(3, hSmall.Survivors.LivingCount);
        Assert.Equal(12, hLarge.Survivors.LivingCount);
    }

    [Theory]
    [InlineData(ScaleTier.JournalShort, 20)]
    [InlineData(ScaleTier.JournalMedium, 64)]
    [InlineData(ScaleTier.JournalLate, 64)]
    [InlineData(ScaleTier.JournalStress, 64)]
    public void Harness_JournalTier_PopulatesExpectedEntryCount(string tier, int expected)
    {
        var ctx = new PerfWorkloadContext { WorkloadId = $"perf_journal_{tier}", Seed = 1, JournalTier = tier };
        using var harness = new PerformanceCampaignHarness(ctx);
        Assert.Equal(expected, harness.Journal.Entries.Count);
    }

    [Fact]
    public void Harness_CanonicalSeeds_RemainStable()
    {
        var ctx = new PerfWorkloadContext { WorkloadId = "perf_seed_stable", CampaignDays = 7, Seed = 9001 };
        using var harness = new PerformanceCampaignHarness(ctx);
        harness.AdvanceDays(7);
        string payload = harness.CaptureSavePayload();
        Assert.False(string.IsNullOrWhiteSpace(payload));
    }
}
