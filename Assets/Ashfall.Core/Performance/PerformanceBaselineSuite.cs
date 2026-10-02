// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Performance.Workloads;

namespace Ashfall.Core.Performance;

/// <summary>
/// Controls which benchmarks <see cref="PerformanceBaselineSuite"/> runs and how
/// many measured iterations each collects.
/// </summary>
public sealed class PerformanceBaselineOptions
{
    /// <summary>Measured iterations per benchmark (median + P95 need at least 5).</summary>
    public int MeasuredIterations { get; set; } = 5;

    /// <summary>Warm-up iterations discarded from statistics.</summary>
    public int WarmupIterations { get; set; } = 1;

    /// <summary>Run the 50/100/250/500 survivor soak benchmarks.</summary>
    public bool IncludeLargeShelter { get; set; } = true;

    /// <summary>Unmeasured days advanced to mature the large-shelter state.</summary>
    public int LargeShelterSoakDays { get; set; } = 90;

    /// <summary>Run the 720-day long campaign replay benchmark.</summary>
    public bool IncludeLongReplay { get; set; } = true;

    /// <summary>Days per measured long-replay iteration.</summary>
    public int LongReplayDays { get; set; } = 720;

    /// <summary>Fixed master seed (deterministic construction).</summary>
    public int Seed { get; set; } = 9001;

    /// <summary>Platform label for the report.</summary>
    public string Platform { get; set; } = "unknown";

    /// <summary>Runtime label for the report (godot, dotnet).</summary>
    public string Runtime { get; set; } = "dotnet";

    /// <summary>Build configuration label for the report.</summary>
    public string BuildConfiguration { get; set; } = "unknown";
}

/// <summary>
/// Machine-readable output of one baseline run: the measured results plus the
/// ranked per-owner campaign-day profile.
/// </summary>
public sealed class PerformanceBaselineReport
{
    /// <summary>Report schema version.</summary>
    public string SchemaVersion { get; set; } = "1.0.0";

    /// <summary>Logical workload set name.</summary>
    public string WorkloadSet { get; set; } = "perf-sprint1";

    /// <summary>UTC generation timestamp (ISO-8601).</summary>
    public string GeneratedAtUtc { get; set; } = string.Empty;

    /// <summary>Platform the run was captured on.</summary>
    public string Platform { get; set; } = string.Empty;

    /// <summary>Runtime the run was captured on.</summary>
    public string Runtime { get; set; } = string.Empty;

    /// <summary>Build configuration.</summary>
    public string BuildConfiguration { get; set; } = string.Empty;

    /// <summary>Fixed master seed.</summary>
    public int Seed { get; set; }

    /// <summary>Honest measurement caveats.</summary>
    public string Notes { get; set; } = string.Empty;

    /// <summary>One result per benchmark, each with median/P95/allocations.</summary>
    public List<PerfResult> Results { get; set; } = new();

    /// <summary>Ranked per-owner campaign-day profile from the widest day run.</summary>
    public PerfDayProfile DayProfile { get; set; } = new();
}

/// <summary>
/// The repeatable runtime baseline suite: cold start, day advancement,
/// save/load/checksum, large-population soak, and long campaign replay. Every
/// benchmark is deterministic (fixed seed) and reports median, P95, and
/// allocations through <see cref="PerfSession"/>.
///
/// Measurement caveat: the Core harness is a structural proxy — it registers
/// five synthetic day owners and its survivor loop is a liveness scan, not the
/// full needs/social/Utility-AI stack. The large-shelter numbers therefore
/// measure coordinator + owner overhead scaling, not real subsystem cost. The
/// host can adopt the same suite against the real coordinator without changing
/// this contract.
/// </summary>
public static class PerformanceBaselineSuite
{
    /// <summary>Run the full baseline suite.</summary>
    public static PerformanceBaselineReport Run(PerformanceBaselineOptions? options = null)
    {
        var opts = options ?? new PerformanceBaselineOptions();
        var report = new PerformanceBaselineReport
        {
            GeneratedAtUtc = DateTimeOffset.UtcNow.ToString("o"),
            Platform = opts.Platform,
            Runtime = opts.Runtime,
            BuildConfiguration = opts.BuildConfiguration,
            Seed = opts.Seed,
            Notes = "Core proxy harness (five synthetic owners; survivor loop is structural). "
                + "Large-shelter numbers measure coordinator/owner overhead scaling, not real subsystem cost.",
        };

        report.Results.Add(MeasureColdStart(opts));
        report.Results.Add(MeasureDayAdvance(opts, WorkloadProfile.Days30, "day_advance_30d").Result);
        report.Results.Add(MeasureDayAdvance(opts, WorkloadProfile.Days180, "day_advance_180d").Result);
        var widest = MeasureDayAdvance(opts, WorkloadProfile.Days360, "day_advance_360d");
        report.Results.Add(widest.Result);
        report.DayProfile = widest.Profile;

        AddPersistence(opts, report);

        if (opts.IncludeLargeShelter)
        {
            AddLargeShelter(opts, report, WorkloadProfile.LargeShelter50, "large_shelter_50");
            AddLargeShelter(opts, report, WorkloadProfile.LargeShelter100, "large_shelter_100");
            AddLargeShelter(opts, report, WorkloadProfile.LargeShelter250, "large_shelter_250");
            AddLargeShelter(opts, report, WorkloadProfile.LargeShelter500, "large_shelter_500");
        }

        if (opts.IncludeLongReplay)
            AddLongReplay(opts, report);

        return report;
    }

    private static PerfResult MeasureColdStart(PerformanceBaselineOptions opts)
    {
        var context = BuildContext(opts, WorkloadProfile.Days30);
        using var session = new PerfSession(context, trackAllocations: true);
        for (int i = 0; i < opts.WarmupIterations; i++)
            session.Warmup(() => { using var _ = new PerformanceCampaignHarness(context); });
        for (int i = 0; i < opts.MeasuredIterations; i++)
            session.Measure(() => { using var _ = new PerformanceCampaignHarness(context); });
        return session.ToResult("cold_start", "advisory");
    }

    private readonly struct DayAdvanceMeasurement
    {
        public readonly PerfResult Result;
        public readonly PerfDayProfile Profile;
        public DayAdvanceMeasurement(PerfResult result, PerfDayProfile profile)
        {
            Result = result;
            Profile = profile;
        }
    }

    private static DayAdvanceMeasurement MeasureDayAdvance(
        PerformanceBaselineOptions opts, WorkloadProfile profile, string benchmarkId)
    {
        var context = BuildContext(opts, profile);
        using var harness = new PerformanceCampaignHarness(context);
        harness.AdvanceDays(Math.Min(3, profile.CampaignDays));

        using var session = new PerfSession(context, trackAllocations: true);
        for (int i = 0; i < opts.WarmupIterations; i++)
            session.Warmup(() => harness.AdvanceDays(profile.CampaignDays));
        for (int i = 0; i < opts.MeasuredIterations; i++)
            session.Measure(() => harness.AdvanceDays(profile.CampaignDays));

        return new DayAdvanceMeasurement(session.ToResult(benchmarkId, "advisory"), harness.DayProfile);
    }

    private static void AddPersistence(PerformanceBaselineOptions opts, PerformanceBaselineReport report)
    {
        var context = BuildContext(opts, WorkloadProfile.Days30);
        using var harness = new PerformanceCampaignHarness(context);
        harness.AdvanceDays(WorkloadProfile.Days30.CampaignDays);
        string payload = harness.CaptureSavePayload();

        using (var saveSession = new PerfSession(context, trackAllocations: true))
        {
            for (int i = 0; i < opts.WarmupIterations; i++) saveSession.Warmup(() => harness.MeasureSaveLatency());
            for (int i = 0; i < opts.MeasuredIterations; i++) saveSession.Measure(() => harness.MeasureSaveLatency());
            report.Results.Add(saveSession.ToResult("save_30d", "advisory", payloadBytes: payload.Length));
        }

        using (var loadSession = new PerfSession(context, trackAllocations: true))
        {
            for (int i = 0; i < opts.WarmupIterations; i++) loadSession.Warmup(() => harness.MeasureLoadLatency(payload));
            for (int i = 0; i < opts.MeasuredIterations; i++) loadSession.Measure(() => harness.MeasureLoadLatency(payload));
            report.Results.Add(loadSession.ToResult("load_30d", "advisory", payloadBytes: payload.Length));
        }

        using (var checksumSession = new PerfSession(context, trackAllocations: true))
        {
            for (int i = 0; i < opts.WarmupIterations; i++)
                checksumSession.Warmup(() => PerformanceCampaignHarness.MeasureChecksumLatency(payload));
            for (int i = 0; i < opts.MeasuredIterations; i++)
                checksumSession.Measure(() => PerformanceCampaignHarness.MeasureChecksumLatency(payload));
            report.Results.Add(checksumSession.ToResult("checksum_30d", "advisory", payloadBytes: payload.Length));
        }
    }

    private static void AddLargeShelter(
        PerformanceBaselineOptions opts, PerformanceBaselineReport report, WorkloadProfile profile, string benchmarkId)
    {
        var context = BuildContext(opts, profile);
        using var harness = new PerformanceCampaignHarness(context);
        harness.AdvanceDays(opts.LargeShelterSoakDays);

        using var session = new PerfSession(context, trackAllocations: true);
        for (int i = 0; i < opts.WarmupIterations; i++) session.Warmup(() => harness.AdvanceDays(1));
        for (int i = 0; i < opts.MeasuredIterations; i++) session.Measure(() => harness.AdvanceDays(1));
        report.Results.Add(session.ToResult(benchmarkId, "advisory"));
    }

    private static void AddLongReplay(PerformanceBaselineOptions opts, PerformanceBaselineReport report)
    {
        var profile = WorkloadProfile.LongReplay720;
        var context = BuildContext(opts, profile);
        using var harness = new PerformanceCampaignHarness(context);
        harness.AdvanceDays(3);

        using var session = new PerfSession(context, trackAllocations: true);
        for (int i = 0; i < opts.WarmupIterations; i++) session.Warmup(() => harness.AdvanceDays(opts.LongReplayDays));
        for (int i = 0; i < opts.MeasuredIterations; i++) session.Measure(() => harness.AdvanceDays(opts.LongReplayDays));
        report.Results.Add(session.ToResult("long_replay_720d", "advisory"));
    }

    private static PerfWorkloadContext BuildContext(PerformanceBaselineOptions opts, WorkloadProfile profile)
    {
        return new PerfWorkloadContext
        {
            WorkloadId = $"perf_{profile.Name}",
            CampaignDays = profile.CampaignDays,
            Seed = opts.Seed,
            RosterTier = profile.RosterTier,
            JournalTier = profile.JournalTier,
            ExpeditionTier = profile.ExpeditionTier,
            WorldStateTier = profile.WorldStateTier,
            Platform = opts.Platform,
            Runtime = opts.Runtime,
            BuildConfiguration = opts.BuildConfiguration,
        };
    }
}
