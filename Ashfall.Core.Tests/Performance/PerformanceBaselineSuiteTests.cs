// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using System.Text.Json;
using Xunit;
using Ashfall.Core.Campaign;
using Ashfall.Core.Performance;
using Ashfall.Core.Performance.Workloads;

namespace Ashfall.Core.Tests.Performance;

/// <summary>
/// Focused tests for the perf-sprint-1 baseline suite: the per-owner day
/// profile, the large-population roster tiers, and the machine-readable
/// baseline report. These validate measurement plumbing; they do not assert
/// wall-clock performance (the CI gate owns regression comparison).
/// </summary>
public class PerformanceBaselineSuiteTests
{
    [Fact]
    public void DayProfile_RecordsOwnersAndRanksByInclusiveTime()
    {
        var profile = new PerfDayProfile("unit_profile");
        profile.Record(BuildArgs(day: 1,
            ("slow_owner", 12.5),
            ("fast_owner", 1.0),
            ("mid_owner", 5.0)));
        profile.Record(BuildArgs(day: 2,
            ("slow_owner", 7.5),
            ("fast_owner", 1.0),
            ("mid_owner", 5.0)));

        Assert.Equal(2, profile.DaysObserved);
        Assert.Equal(3, profile.OwnerCount);
        Assert.Equal(32.0, profile.TotalObservedMs, 3);

        var ranked = profile.RankedByInclusiveTime();
        Assert.Equal("slow_owner", ranked[0].OwnerId);
        Assert.Equal("mid_owner", ranked[1].OwnerId);
        Assert.Equal("fast_owner", ranked[2].OwnerId);

        Assert.Equal(2, ranked[0].Calls);
        Assert.Equal(20.0, ranked[0].TotalMs, 3);
        Assert.Equal(10.0, ranked[0].MeanMs, 3);
        Assert.Equal(12.5, ranked[0].MaxMs, 3);
        Assert.True(profile.ShareOf(ranked[0]) > profile.ShareOf(ranked[1]));
    }

    [Fact]
    public void DayProfile_IgnoresNullAndEmptyReports()
    {
        var profile = new PerfDayProfile();
        profile.Record(null);
        Assert.Equal(0, profile.DaysObserved);

        profile.Record(new DayAdvancedEventArgs(1, Array.Empty<DayOwnerReport>()));
        Assert.Equal(1, profile.DaysObserved);
        Assert.Equal(0, profile.OwnerCount);
    }

    [Fact]
    public void Harness_DayProfile_CapturesRegisteredOwners()
    {
        var context = new PerfWorkloadContext
        {
            WorkloadId = "profile_harness",
            CampaignDays = 3,
            Seed = 1234,
            RosterTier = ScaleTier.RosterNormal,
            JournalTier = ScaleTier.JournalShort,
            ExpeditionTier = ScaleTier.ExpeditionTypical,
            WorldStateTier = ScaleTier.WorldNormal,
        };

        using var harness = new PerformanceCampaignHarness(context);
        harness.AdvanceDays(3);

        Assert.Equal(3, harness.DayProfile.DaysObserved);
        Assert.Equal(5, harness.DayProfile.OwnerCount);
        Assert.Contains(harness.DayProfile.RankedByInclusiveTime(), o => o.OwnerId == "perf_survivors");
        Assert.True(harness.DayProfile.TotalObservedMs >= 0);
    }

    [Theory]
    [InlineData(ScaleTier.RosterSquad, 50)]
    [InlineData(ScaleTier.RosterCompany, 100)]
    [InlineData(ScaleTier.RosterCrowd, 250)]
    [InlineData(ScaleTier.RosterMass, 500)]
    public void LargeRosterTier_ConstructsRequestedPopulation(string tier, int expected)
    {
        Assert.Equal(expected, ScaleTier.RosterCount(tier));

        var context = new PerfWorkloadContext
        {
            WorkloadId = $"large_{expected}",
            CampaignDays = 1,
            Seed = 9001,
            RosterTier = tier,
            JournalTier = ScaleTier.JournalShort,
            ExpeditionTier = ScaleTier.ExpeditionNone,
            WorldStateTier = ScaleTier.WorldNormal,
        };

        using var harness = new PerformanceCampaignHarness(context);
        Assert.Equal(expected, harness.Survivors.LivingCount);
    }

    [Fact]
    public void LargeRosterTier_IsDeterministicAcrossRuns()
    {
        var context = new PerfWorkloadContext
        {
            WorkloadId = "large_determinism",
            CampaignDays = 1,
            Seed = 9001,
            RosterTier = ScaleTier.RosterMass,
            JournalTier = ScaleTier.JournalShort,
            ExpeditionTier = ScaleTier.ExpeditionNone,
            WorldStateTier = ScaleTier.WorldNormal,
        };

        using var first = new PerformanceCampaignHarness(context);
        using var second = new PerformanceCampaignHarness(context);
        first.AdvanceDays(1);
        second.AdvanceDays(1);

        Assert.Equal(first.CaptureSavePayload(), second.CaptureSavePayload());
    }

    [Fact]
    public void BaselineSuite_SmallRun_ProducesMachineReadableResults()
    {
        var report = PerformanceBaselineSuite.Run(new PerformanceBaselineOptions
        {
            MeasuredIterations = 1,
            WarmupIterations = 1,
            IncludeLargeShelter = false,
            IncludeLongReplay = false,
            Platform = "test",
            Runtime = "dotnet",
            BuildConfiguration = "Debug",
        });

        string[] expected =
        {
            "cold_start", "day_advance_30d", "day_advance_180d", "day_advance_360d",
            "save_30d", "load_30d", "checksum_30d",
        };
        var ids = report.Results.Select(r => r.BenchmarkId).ToArray();
        foreach (var id in expected)
            Assert.Contains(id, ids);

        foreach (var result in report.Results)
        {
            Assert.NotNull(result.Statistics);
            Assert.Equal(1, result.IterationCount);
            Assert.True(result.Statistics!.Median >= 0);
            Assert.True(result.Statistics.P95 >= result.Statistics.Median);
        }

        Assert.Equal(5, report.DayProfile.OwnerCount);

        string json = JsonSerializer.Serialize(report);
        Assert.Contains("day_advance_360d", json);
        Assert.Contains("DayProfile", json);
    }

    private static DayAdvancedEventArgs BuildArgs(int day, params (string ownerId, double ms)[] owners)
    {
        var reports = new DayOwnerReport[owners.Length];
        for (int i = 0; i < owners.Length; i++)
        {
            reports[i] = new DayOwnerReport(owners[i].ownerId, true, Array.Empty<DayStateChangeEvent>(), string.Empty)
            {
                DurationMs = owners[i].ms,
            };
        }
        return new DayAdvancedEventArgs(day, reports);
    }
}
