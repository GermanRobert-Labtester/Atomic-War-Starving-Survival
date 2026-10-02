// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Campaign;

namespace Ashfall.Core.Performance;

/// <summary>
/// Aggregated per-owner timing across one or more campaign day advances.
/// Observational only; never affects the simulation.
/// </summary>
public sealed class PerfOwnerAggregate
{
    /// <summary>Stable owner id as registered with the campaign day coordinator.</summary>
    public string OwnerId { get; set; } = string.Empty;

    /// <summary>Number of day ticks observed for this owner.</summary>
    public int Calls { get; set; }

    /// <summary>Number of observed ticks that failed.</summary>
    public int FailureCount { get; set; }

    /// <summary>Sum of inclusive owner-tick durations (ms).</summary>
    public double TotalMs { get; set; }

    /// <summary>Slowest single observed tick (ms).</summary>
    public double MaxMs { get; set; }

    /// <summary>Mean inclusive duration per observed tick (ms).</summary>
    public double MeanMs => Calls == 0 ? 0.0 : TotalMs / Calls;
}

/// <summary>
/// Ranks each campaign-day subsystem by inclusive time and calls per tick,
/// built from the coordinator's existing <see cref="DayOwnerReport.DurationMs"/>
/// observation. This is the instrument for "profile campaign-day orchestration
/// and rank each subsystem by inclusive time, calls per tick".
///
/// It is a pure accumulator: <see cref="Record"/> only reads the coordinator's
/// reports, so it cannot change tick order, events, save state, or determinism.
/// </summary>
public sealed class PerfDayProfile
{
    private readonly Dictionary<string, PerfOwnerAggregate> _owners = new(StringComparer.Ordinal);
    private readonly List<PerfOwnerAggregate> _ranked = new();
    private bool _rankedDirty;

    /// <summary>Workload identifier this profile was collected under.</summary>
    public string WorkloadId { get; set; } = string.Empty;

    /// <summary>Number of day advances folded into this profile.</summary>
    public int DaysObserved { get; private set; }

    /// <summary>Total inclusive owner time observed across all days (ms).</summary>
    public double TotalObservedMs { get; private set; }

    /// <summary>Number of distinct owners observed.</summary>
    public int OwnerCount => _owners.Count;

    /// <summary>
    /// Owners ranked by inclusive time, exposed as a serializable snapshot for
    /// machine-readable reports. Same order as <see cref="RankedByInclusiveTime"/>.
    /// </summary>
    public IReadOnlyList<PerfOwnerAggregate> Owners => RankedByInclusiveTime();

    /// <summary>Parameterless constructor for JSON deserialization.</summary>
    public PerfDayProfile() { }

    public PerfDayProfile(string workloadId) => WorkloadId = workloadId ?? string.Empty;

    /// <summary>
    /// Fold one day-advance result into the profile. Null reports are ignored,
    /// so a guarded/rejected advance contributes nothing.
    /// </summary>
    public void Record(DayAdvancedEventArgs? args)
    {
        if (args?.OwnerReports == null) return;
        DaysObserved++;
        for (int i = 0; i < args.OwnerReports.Length; i++)
        {
            var report = args.OwnerReports[i];
            if (report == null || string.IsNullOrEmpty(report.OwnerId)) continue;

            if (!_owners.TryGetValue(report.OwnerId, out var aggregate))
            {
                aggregate = new PerfOwnerAggregate { OwnerId = report.OwnerId };
                _owners.Add(report.OwnerId, aggregate);
            }

            aggregate.Calls++;
            if (!report.Succeeded) aggregate.FailureCount++;

            double ms = report.DurationMs < 0 ? 0.0 : report.DurationMs;
            aggregate.TotalMs += ms;
            if (ms > aggregate.MaxMs) aggregate.MaxMs = ms;
            TotalObservedMs += ms;
        }
        _rankedDirty = true;
    }

    /// <summary>
    /// Owners ranked by total inclusive time (descending), ties broken by
    /// ordinal owner id so the ranking is deterministic across runs.
    /// </summary>
    public IReadOnlyList<PerfOwnerAggregate> RankedByInclusiveTime()
    {
        if (_rankedDirty)
        {
            _ranked.Clear();
            foreach (var aggregate in _owners.Values) _ranked.Add(aggregate);
            _ranked.Sort(static (a, b) =>
            {
                int byTime = b.TotalMs.CompareTo(a.TotalMs);
                return byTime != 0 ? byTime : string.CompareOrdinal(a.OwnerId, b.OwnerId);
            });
            _rankedDirty = false;
        }
        return _ranked;
    }

    /// <summary>Share of total observed time owned by <paramref name="owner"/> (0..1).</summary>
    public double ShareOf(PerfOwnerAggregate owner)
        => owner == null || TotalObservedMs <= 0 ? 0.0 : owner.TotalMs / TotalObservedMs;

    /// <summary>Owners with their time share, ranked by inclusive time.</summary>
    public IReadOnlyList<PerfOwnerShare> RankedWithShare()
    {
        var ranked = RankedByInclusiveTime();
        var result = new List<PerfOwnerShare>(ranked.Count);
        for (int i = 0; i < ranked.Count; i++)
            result.Add(new PerfOwnerShare(ranked[i], ShareOf(ranked[i])));
        return result;
    }
}

/// <summary>A ranked owner plus its share of observed inclusive time.</summary>
public sealed class PerfOwnerShare
{
    /// <summary>The owner aggregate.</summary>
    public PerfOwnerAggregate Owner { get; set; } = new PerfOwnerAggregate();

    /// <summary>Fraction of total observed time (0..1).</summary>
    public double Share { get; set; }

    /// <summary>Parameterless constructor for JSON deserialization.</summary>
    public PerfOwnerShare() { }

    public PerfOwnerShare(PerfOwnerAggregate owner, double share)
    {
        Owner = owner;
        Share = share;
    }
}
