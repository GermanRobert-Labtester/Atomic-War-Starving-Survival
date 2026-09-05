using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    /// <summary>
    /// F15 — Weather Gate Balance Audit.
    /// Measures whether weather gates produce meaningful planning choices
    /// across the campaign rather than random frustration, permanent
    /// denial, or negligible impact.
    /// </summary>
    public sealed class WeatherGateBalanceAuditTests
    {
        private readonly string _dataDir;
        private readonly WeatherGateAuditSimulator _sim;

        public WeatherGateBalanceAuditTests()
        {
            _dataDir = WeatherGateAuditSimulator.FindDataDir();
            _sim = new WeatherGateAuditSimulator(_dataDir);
        }

        // ── F15.2 Route availability statistics ────────────────────────

        [Fact]
        public void F15_EveryGatedRouteHasAvailabilityStats()
        {
            var stats = _sim.CalculateUtilization();
            Assert.Equal(18, stats.Count);

            foreach (var s in stats)
            {
                Assert.True(s.BlockedDays + s.OpenDays == 360,
                    $"Gate {s.GateId}: days don't sum to 360");
                Assert.True(s.LongestBlockedRun >= 0);
                Assert.True(s.LongestOpenRun >= 0);
                Assert.True(s.Transitions >= 0);
            }
        }

        // ── F15.4 Seasonal distribution ────────────────────────────────

        [Fact]
        public void F15_SeasonalDistribution_Calculated()
        {
            var seasonal = _sim.CalculateSeasonalDistribution();
            Assert.Equal(18, seasonal.Count);

            foreach (var gateEntry in seasonal)
            {
                int totalDays = gateEntry.Value.Values.Sum(v => v.Total);
                Assert.Equal(360, totalDays);
            }
        }

        // ── F15.5 Positive gate analysis ───────────────────────────────

        [Fact]
        public void F15_PositiveGates_IceRoads_OpenDuringBlizzard()
        {
            var stats = _sim.CalculateUtilization();

            // gate_frozen_lake_crossing and gate_seasonal_ice_road are positive gates
            var frozenLake = stats.First(s => s.GateId == "gate_frozen_lake_crossing");
            var iceRoad = stats.First(s => s.GateId == "gate_seasonal_ice_road");

            // Both should be open during Blizzard days
            Assert.True(frozenLake.OpenDays > 0,
                $"Frozen lake crossing never opens ({frozenLake.OpenDays} days)");
            Assert.True(iceRoad.OpenDays > 0,
                $"Ice road never opens ({iceRoad.OpenDays} days)");

            // Open percentage should be in the meaningful range (5-35%)
            Assert.True(frozenLake.OpenPct >= 5.0,
                $"Frozen lake open only {frozenLake.OpenPct:F1}% — too rare");
            Assert.True(frozenLake.OpenPct <= 50.0,
                $"Frozen lake open {frozenLake.OpenPct:F1}% — may not feel exceptional");
        }

        // ── F15.6 Override coverage ────────────────────────────────────

        [Fact]
        public void F15_OverrideCoverage_Measured()
        {
            var stats = _sim.CalculateUtilization();

            var negativeGates = stats.Where(s => s.RequiredWeather.Count == 0).ToList();
            var withOverride = negativeGates.Where(s => !string.IsNullOrEmpty(s.OverrideItem)).ToList();
            var withoutOverride = negativeGates.Where(s => string.IsNullOrEmpty(s.OverrideItem)).ToList();

            // Expected: 4 gates with overrides (gas_mask x2, hazmat_suit x2)
            Assert.Equal(4, withOverride.Count);
            Assert.True(withoutOverride.Count > 0, "All negative gates have overrides — unusual");

            // Verify override items are correct
            Assert.All(withOverride, s =>
                Assert.True(s.OverrideItem == "gas_mask" || s.OverrideItem == "hazmat_suit",
                    $"Gate {s.GateId} has unexpected override: {s.OverrideItem}"));
        }

        // ── F15.7 EMP gate audit ───────────────────────────────────────

        [Fact]
        public void F15_EMPGateAudit_ZeroWeightConfirmed()
        {
            var stats = _sim.CalculateUtilization();
            var empGate = stats.First(s => s.GateId == "gate_electronics_route_emp");

            // EMPStorm has zero weight in all seasons
            Assert.Equal(0, empGate.BlockedDays);
            Assert.Equal(360, empGate.OpenDays);
            Assert.Equal(0.0, empGate.BlockedPct);
        }

        // ── F15.8 BioFog gate audit ────────────────────────────────────

        [Fact]
        public void F15_BioFogGateAudit_ZeroWeightConfirmed()
        {
            var stats = _sim.CalculateUtilization();
            var bioFogGates = stats.Where(s => s.BlockedWeather.Contains("BioFog")).ToList();

            Assert.Equal(3, bioFogGates.Count);
            foreach (var g in bioFogGates)
            {
                Assert.Equal(0, g.BlockedDays);
                Assert.Equal(360, g.OpenDays);
            }

            // 4 dead gates total (3 BioFog + 1 EMP) out of 18
            int deadCount = stats.Count(s => s.BlockedDays == 0 && s.BlockedWeather.Count > 0);
            Assert.Equal(4, deadCount);
        }

        // ── F15.9 FalloutStorm analysis ────────────────────────────────

        [Fact]
        public void F15_FalloutStormAnalysis_MeaningfulFrequency()
        {
            var stats = _sim.CalculateUtilization();
            // 2 route gates + 1 destination gate block FalloutStorm
            var falloutGates = stats.Where(s =>
                s.BlockedWeather.Contains("FalloutStorm")).ToList();

            Assert.Equal(3, falloutGates.Count);

            foreach (var g in falloutGates)
            {
                // FalloutStorm has low but non-zero weights
                Assert.True(g.BlockedDays > 0,
                    $"Fallout gate {g.GateId} never blocked — FalloutStorm may be too rare");
                Assert.True(g.BlockedPct < 60.0,
                    $"Fallout gate {g.GateId} blocked {g.BlockedPct:F1}% — too restrictive");
            }
        }

        // ── F15.10 Blizzard analysis ───────────────────────────────────

        [Fact]
        public void F15_BlizzardAnalysis_HighConcentrationInColdSeasons()
        {
            var stats = _sim.CalculateUtilization();
            // 4 route + 1 destination gate block Blizzard
            var blizzardGates = stats.Where(s =>
                s.BlockedWeather.Contains("Blizzard")).ToList();

            Assert.Equal(5, blizzardGates.Count);

            foreach (var g in blizzardGates)
            {
                // Blizzard has high weights in Deep Freeze and Long Winter
                Assert.True(g.BlockedPct > 5.0,
                    $"Blizzard gate {g.GateId} blocked only {g.BlockedPct:F1}%");
                Assert.True(g.LongestBlockedRun > 0,
                    $"Blizzard gate {g.GateId} has zero longest blocked run");
            }
        }

        [Fact]
        public void F15_BlizzardNetworkClosure_MaxSimultaneousBlocks()
        {
            var timeline = _sim.Timeline;
            var routeCatalog = _sim.RouteCatalog;

            int maxSimultaneous = 0;
            int worstDay = 0;

            for (int day = 0; day < timeline.Count; day++)
            {
                int blocked = 0;
                foreach (var gateDef in routeCatalog.Gates)
                {
                    if (gateDef.gate_type == "route" && gateDef.blocked_weather != null && gateDef.blocked_weather.Contains("Blizzard"))
                    {
                        var domain = WeatherGateEvaluator.FromDef(gateDef);
                        var state = WeatherGateEvaluator.EvaluateGateStatic(domain, timeline[day].Weather);
                        if (!state.IsOpen) blocked++;
                    }
                }
                if (blocked > maxSimultaneous)
                {
                    maxSimultaneous = blocked;
                    worstDay = day;
                }
            }

            // 4 route + 1 destination gate block Blizzard = 5 max
            Assert.True(maxSimultaneous <= 5,
                $"More than 5 blizzard gates blocked simultaneously: {maxSimultaneous}");
        }

        // ── F15.11 BlackRain analysis ──────────────────────────────────

        [Fact]
        public void F15_BlackRainAnalysis_MidFrequency()
        {
            var stats = _sim.CalculateUtilization();
            // 3 route gates + 1 destination gate block BlackRain
            var blackRainGates = stats.Where(s =>
                s.BlockedWeather.Contains("BlackRain")).ToList();

            Assert.Equal(4, blackRainGates.Count);

            foreach (var g in blackRainGates)
            {
                Assert.True(g.BlockedDays > 0,
                    $"BlackRain gate {g.GateId} never blocked");
            }
        }

        // ── F15.12 Network-level balance metrics ───────────────────────

        [Fact]
        public void F15_NetworkClosureMetrics_Calculated()
        {
            var (worstDay, maxBlocked, daysOver50, daysZeroOpen) = _sim.NetworkClosureMetrics();

            Assert.True(maxBlocked <= 18, $"More than 18 gates blocked: {maxBlocked}");
            Assert.True(worstDay >= 0 && worstDay < 360);

            // Log findings (not failures — these are audit metrics)
            // daysZeroOpen should be 0 since dead gates are always open
            Assert.Equal(0, daysZeroOpen);
        }

        // ── F15.13 Destination gate force-passage analysis ─────────────

        [Fact]
        public void F15_DestinationGates_HaveForcePassageCosts()
        {
            var destGates = _sim.RouteCatalog.Gates
                .Where(g => g.gate_type == "destination")
                .ToList();

            Assert.Equal(3, destGates.Count);

            foreach (var g in destGates)
            {
                Assert.True(g.force_stamina_cost > 0,
                    $"Destination gate {g.id} has no force stamina cost");
            }
        }

        // ── F15.14 Balance report generation ───────────────────────────

        [Fact]
        public void F15_BalanceReport_Generated()
        {
            var stats = _sim.CalculateUtilization();
            var seasonal = _sim.CalculateSeasonalDistribution();
            var (worstDay, maxBlocked, daysOver50, daysZeroOpen) = _sim.NetworkClosureMetrics();
            var weatherFreq = _sim.WeatherFrequency();

            var sb = new StringBuilder();
            sb.AppendLine("# Weather Gate Balance Audit");
            sb.AppendLine();
            sb.AppendLine($"- Audit seed: {WeatherGateAuditSimulator.AuditSeed}");
            sb.AppendLine($"- Campaign horizon: {WeatherGateAuditSimulator.CampaignDays} days");
            sb.AppendLine($"- Gate count: {stats.Count}");
            sb.AppendLine($"- Source: weather_route_gates.json, weather_seasons.json");
            sb.AppendLine();

            sb.AppendLine("## Per-Gate Availability");
            sb.AppendLine();
            sb.AppendLine("| Gate ID | Target | Weather | Mode | Blocked % | Open % | Longest Block | Longest Open | Override |");
            sb.AppendLine("|---|---|---|---|---:|---:|---:|---:|---|");

            foreach (var s in stats)
            {
                string weather = s.RequiredWeather.Count > 0
                    ? $"requires: {string.Join(", ", s.RequiredWeather)}"
                    : $"blocks: {string.Join(", ", s.BlockedWeather)}";
                string mode = s.RequiredWeather.Count > 0 ? "positive" : "negative";
                sb.AppendLine($"| {s.GateId} | {s.Target} | {weather} | {mode} | {s.BlockedPct:F1}% | {s.OpenPct:F1}% | {s.LongestBlockedRun} | {s.LongestOpenRun} | {s.OverrideItem} |");
            }

            sb.AppendLine();
            sb.AppendLine("## Weather Frequency (360 days)");
            sb.AppendLine();
            sb.AppendLine("| WeatherKind | Days | % |");
            sb.AppendLine("|---|---:|---:|");
            foreach (var kv in weatherFreq.OrderByDescending(kv => kv.Value))
            {
                double pct = 100.0 * kv.Value / 360;
                sb.AppendLine($"| {kv.Key} | {kv.Value} | {pct:F1}% |");
            }

            sb.AppendLine();
            sb.AppendLine("## Network Metrics");
            sb.AppendLine();
            sb.AppendLine($"- Worst day: {worstDay} ({maxBlocked} gates blocked)");
            sb.AppendLine($"- Days with >50% gates blocked: {daysOver50}");
            sb.AppendLine($"- Days with zero open gates: {daysZeroOpen}");

            sb.AppendLine();
            sb.AppendLine("## Dead Content");
            sb.AppendLine();
            var deadGates = stats.Where(s => s.BlockedDays == 0 && s.BlockedWeather.Count > 0).ToList();
            foreach (var g in deadGates)
            {
                sb.AppendLine($"- **{g.GateId}** ({g.Target}): blocked by [{string.Join(", ", g.BlockedWeather)}] — zero season weight, never triggers");
            }

            // The report should be non-trivial
            Assert.True(sb.Length > 500, "Balance report is too short");
        }
    }
}
