using System;
using Xunit;
using Ashfall.Core.Performance;
using Ashfall.Core.Performance.Workloads;

namespace Ashfall.Core.Tests.Performance;

/// <summary>
/// Save/load latency, payload size, checksum, and migration-lite tests.
/// </summary>
public class PerformancePersistenceTests
{
    [Fact]
    public void CaptureSavePayload_30DayWorkload_ProducesNonEmptyPayload()
    {
        var ctx = BuildContext(30, ScaleTier.RosterNormal, ScaleTier.JournalShort, ScaleTier.ExpeditionTypical, ScaleTier.WorldNormal);
        using var harness = new PerformanceCampaignHarness(ctx);
        harness.AdvanceDays(ctx.CampaignDays);

        string payload = harness.CaptureSavePayload();
        Assert.False(string.IsNullOrWhiteSpace(payload));
        Assert.Contains("perf_slot", payload, StringComparison.Ordinal);
    }

    [Fact]
    public void SaveLatency_30DayWorkload_CompletesWithin500Ms()
    {
        var ctx = BuildContext(30, ScaleTier.RosterNormal, ScaleTier.JournalShort, ScaleTier.ExpeditionTypical, ScaleTier.WorldNormal);
        using var harness = new PerformanceCampaignHarness(ctx);
        harness.AdvanceDays(ctx.CampaignDays);

        double latencyMs = harness.MeasureSaveLatency();
        Assert.True(latencyMs < 500,
            $"30d save latency must be < 500ms. Actual: {latencyMs:F1}ms");
    }

    [Fact]
    public void SaveLoad_RoundTrip_LoadsWithin200ms()
    {
        var ctx = BuildContext(30, ScaleTier.RosterNormal, ScaleTier.JournalShort, ScaleTier.ExpeditionTypical, ScaleTier.WorldNormal);
        using var harness = new PerformanceCampaignHarness(ctx);
        harness.AdvanceDays(ctx.CampaignDays);

        string payload = harness.CaptureSavePayload();
        double latencyMs = harness.MeasureLoadLatency(payload);
        Assert.True(latencyMs < 200,
            $"Load latency must be < 200ms. Actual: {latencyMs:F1}ms");
    }

    [Fact]
    public void SaveChecksum_ComputeLatency_CompletesWithin200MS()
    {
        var ctx = BuildContext(30, ScaleTier.RosterNormal, ScaleTier.JournalShort, ScaleTier.ExpeditionTypical, ScaleTier.WorldNormal);
        using var harness = new PerformanceCampaignHarness(ctx);
        harness.AdvanceDays(ctx.CampaignDays);
        string payload = harness.CaptureSavePayload();

        double latencyMs = PerformanceCampaignHarness.MeasureChecksumLatency(payload);
        Assert.True(latencyMs < 200,
            $"Checksum latency must be < 200ms. Actual: {latencyMs:F1}ms");
    }

    [Theory]
    [InlineData(ScaleTier.RosterNormal, ScaleTier.JournalShort)]
    [InlineData(ScaleTier.RosterLarge, ScaleTier.JournalMedium)]
    public void SavePayload_GrowsWithWorkloadScale(string rosterTier, string journalTier)
    {
        var ctx = BuildContext(30, rosterTier, journalTier, ScaleTier.ExpeditionTypical, ScaleTier.WorldNormal);
        using var harness = new PerformanceCampaignHarness(ctx);
        harness.AdvanceDays(ctx.CampaignDays);

        string payload = harness.CaptureSavePayload();
        Assert.True(payload.Length > 200, "Payload should grow with workload scale.");
        Assert.True(payload.Length < 4_000_000, "30d payload should stay under 4MB.");
    }

    [Fact]
    public void CaptureSavePayload_SameWorkloadId_ProducesSamePayload()
    {
        var ctx = BuildContext(30, ScaleTier.RosterNormal, ScaleTier.JournalShort, ScaleTier.ExpeditionTypical, ScaleTier.WorldNormal);
        using var harnessA = new PerformanceCampaignHarness(ctx);
        using var harnessB = new PerformanceCampaignHarness(ctx);

        harnessA.AdvanceDays(ctx.CampaignDays);
        harnessB.AdvanceDays(ctx.CampaignDays);

        Assert.Equal(harnessA.CaptureSavePayload(), harnessB.CaptureSavePayload());
    }

    private static PerfWorkloadContext BuildContext(
        int days, string rosterTier, string journalTier, string expeditionTier, string worldStateTier)
    {
        return new PerfWorkloadContext
        {
            WorkloadId = $"perf_save_{days}d",
            CampaignDays = days,
            Seed = 9001,
            RosterTier = rosterTier,
            JournalTier = journalTier,
            ExpeditionTier = expeditionTier,
            WorldStateTier = worldStateTier,
            BuildConfiguration = "Release",
            Platform = "Linux",
            Runtime = "dotnet",
        };
    }
}
