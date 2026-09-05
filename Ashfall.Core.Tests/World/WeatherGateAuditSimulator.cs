using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    /// <summary>
    /// Shared audit engine for F13–F16 weather gate verification.
    /// Builds a deterministic 360-day weather timeline using the real
    /// production WeatherSystem and season data, then evaluates all gates
    /// against every day. One timeline, many analyses.
    /// </summary>
    public sealed class WeatherGateAuditSimulator
    {
        public const int AuditSeed = 424242;
        public const int CampaignDays = 360;

        private readonly SeasonProfileDef _profile;
        private readonly WeatherRouteGateCatalog _routeCatalog;
        private readonly WeatherGateCatalog _domainCatalog;
        private readonly WeatherGateEvaluator _evaluator;
        private readonly List<WeatherAuditDay> _timeline;
        private readonly List<GateDayEvaluation> _gateEvaluations;

        public SeasonProfileDef Profile => _profile;
        public WeatherRouteGateCatalog RouteCatalog => _routeCatalog;
        public WeatherGateCatalog DomainCatalog => _domainCatalog;
        public WeatherGateEvaluator Evaluator => _evaluator;
        public IReadOnlyList<WeatherAuditDay> Timeline => _timeline;
        public IReadOnlyList<GateDayEvaluation> GateEvaluations => _gateEvaluations;
        public int GateCount => _routeCatalog.Gates.Count;

        public WeatherGateAuditSimulator(string dataDir)
        {
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            _profile = WeatherProfileLoader.Load(dataDir, fileIO, json)
                       ?? throw new InvalidOperationException("Failed to load weather_seasons.json");
            _routeCatalog = WeatherRouteGateCatalog.LoadFromDirectory(dataDir, fileIO);
            // Build domain catalog from route catalog using the canonical FromDef conversion.
            // WeatherGateFile.Load fails because the JSON uses snake_case but WeatherGate
            // has PascalCase fields. The route catalog (WeatherGateDef with snake_case) is
            // the actual data authority.
            _domainCatalog = new WeatherGateCatalog();
            foreach (var def in _routeCatalog.Gates)
                _domainCatalog.Register(WeatherGateEvaluator.FromDef(def));
            _evaluator = new WeatherGateEvaluator(_domainCatalog);

            _timeline = BuildTimeline(AuditSeed, CampaignDays);
            _gateEvaluations = EvaluateAllGates(_timeline);
        }

        /// <summary>Build a deterministic weather timeline using the real WeatherSystem.</summary>
        public static List<WeatherAuditDay> BuildTimeline(int seed, int days)
        {
            var profile = LoadProfileFromData();
            var sys = new WeatherSystem();
            sys.BindProfile(profile, seed);

            var timeline = new List<WeatherAuditDay>(days);
            for (int day = 0; day < days; day++)
            {
                // Advance 24 hours per day (4 ticks at 6h interval)
                sys.Tick(24f);
                var season = sys.GetSeasonForDay(day);
                timeline.Add(new WeatherAuditDay(day, season.id, sys.Current));
            }
            return timeline;
        }

        /// <summary>Evaluate all gates against every day in the timeline.</summary>
        public List<GateDayEvaluation> EvaluateAllGates(List<WeatherAuditDay> timeline)
        {
            var results = new List<GateDayEvaluation>(timeline.Count * _routeCatalog.Gates.Count);
            foreach (var day in timeline)
            {
                foreach (var gateDef in _routeCatalog.Gates)
                {
                    var domain = WeatherGateEvaluator.FromDef(gateDef);
                    var state = WeatherGateEvaluator.EvaluateGateStatic(domain, day.Weather);
                    bool overrideAvailable = !string.IsNullOrEmpty(gateDef.override_item);
                    results.Add(new GateDayEvaluation(
                        day.Day, gateDef.id, gateDef.target, day.Weather,
                        !state.IsOpen, overrideAvailable, state.Reason));
                }
            }
            return results;
        }

        /// <summary>Calculate per-gate utilization statistics.</summary>
        public List<GateUtilizationStats> CalculateUtilization()
        {
            var stats = new Dictionary<string, GateUtilizationStats>();
            foreach (var gateDef in _routeCatalog.Gates)
            {
                stats[gateDef.id] = new GateUtilizationStats
                {
                    GateId = gateDef.id,
                    Target = gateDef.target,
                    GateType = gateDef.gate_type,
                    BlockedWeather = gateDef.blocked_weather?.ToList() ?? new List<string>(),
                    RequiredWeather = gateDef.required_weather?.ToList() ?? new List<string>(),
                    OverrideItem = gateDef.override_item ?? "",
                };
            }

            foreach (var eval in _gateEvaluations)
            {
                if (!stats.TryGetValue(eval.GateId, out var s)) continue;
                if (eval.IsBlocked) s.BlockedDays++; else s.OpenDays++;
            }

            // Calculate streaks
            foreach (var gateDef in _routeCatalog.Gates)
            {
                var gateEvals = _gateEvaluations
                    .Where(e => e.GateId == gateDef.id)
                    .OrderBy(e => e.Day)
                    .ToList();

                int currentBlockedRun = 0, currentOpenRun = 0;
                int maxBlocked = 0, maxOpen = 0;
                int transitions = 0;
                bool? lastBlocked = null;

                foreach (var e in gateEvals)
                {
                    bool blocked = e.IsBlocked;
                    if (lastBlocked.HasValue && lastBlocked.Value != blocked) transitions++;
                    lastBlocked = blocked;

                    if (blocked)
                    {
                        currentBlockedRun++;
                        currentOpenRun = 0;
                        if (currentBlockedRun > maxBlocked) maxBlocked = currentBlockedRun;
                    }
                    else
                    {
                        currentOpenRun++;
                        currentBlockedRun = 0;
                        if (currentOpenRun > maxOpen) maxOpen = currentOpenRun;
                    }
                }

                if (stats.TryGetValue(gateDef.id, out var s))
                {
                    s.LongestBlockedRun = maxBlocked;
                    s.LongestOpenRun = maxOpen;
                    s.Transitions = transitions;
                }
            }

            return stats.Values.OrderBy(s => s.GateId, StringComparer.Ordinal).ToList();
        }

        /// <summary>Calculate per-gate per-season blocked percentages.</summary>
        public Dictionary<string, Dictionary<string, (int Blocked, int Total)>> CalculateSeasonalDistribution()
        {
            var result = new Dictionary<string, Dictionary<string, (int, int)>>(StringComparer.Ordinal);
            foreach (var gateDef in _routeCatalog.Gates)
            {
                var seasonMap = new Dictionary<string, (int Blocked, int Total)>(StringComparer.Ordinal);
                var gateEvals = _gateEvaluations.Where(e => e.GateId == gateDef.id);
                foreach (var e in gateEvals)
                {
                    var day = _timeline[e.Day];
                    if (!seasonMap.TryGetValue(day.SeasonId, out var counts))
                        counts = (0, 0);
                    counts.Total++;
                    if (e.IsBlocked) counts.Blocked++;
                    seasonMap[day.SeasonId] = counts;
                }
                result[gateDef.id] = seasonMap;
            }
            return result;
        }

        /// <summary>Calculate weather frequency table from the timeline.</summary>
        public Dictionary<WeatherKind, int> WeatherFrequency()
        {
            var freq = new Dictionary<WeatherKind, int>();
            foreach (var day in _timeline)
            {
                if (!freq.ContainsKey(day.Weather)) freq[day.Weather] = 0;
                freq[day.Weather]++;
            }
            return freq;
        }

        /// <summary>Calculate per-day network closure metrics.</summary>
        public (int WorstDay, int MaxBlocked, int DaysOver50Pct, int DaysZeroOpen) NetworkClosureMetrics()
        {
            int worstDay = 0, maxBlocked = 0, daysOver50 = 0, daysZeroOpen = 0;
            int gateCount = _routeCatalog.Gates.Count;
            if (gateCount == 0) return (0, 0, 0, 0);

            for (int day = 0; day < CampaignDays; day++)
            {
                int blocked = _gateEvaluations.Count(e => e.Day == day && e.IsBlocked);
                int open = gateCount - blocked;
                if (blocked > maxBlocked) { maxBlocked = blocked; worstDay = day; }
                if (blocked > gateCount / 2) daysOver50++;
                if (open == 0) daysZeroOpen++;
            }
            return (worstDay, maxBlocked, daysOver50, daysZeroOpen);
        }

        /// <summary>Serialize a gate trace row for byte-for-byte comparison.</summary>
        public static string SerializeTraceRow(GateDayEvaluation eval)
        {
            return $"{eval.GateId}|{(eval.IsBlocked ? 1 : 0)}|{(eval.OverrideAvailable ? 1 : 0)}|{eval.Reason}";
        }

        /// <summary>Serialize a full gate evaluation trace for a given weather kind.</summary>
        public static string SerializeTrace(IEnumerable<GateDayEvaluation> evaluations)
        {
            var sb = new StringBuilder();
            foreach (var eval in evaluations.OrderBy(e => e.GateId, StringComparer.Ordinal))
            {
                sb.AppendLine(SerializeTraceRow(eval));
            }
            return sb.ToString();
        }

        /// <summary>Compute SHA-256 hash of a trace string.</summary>
        public static string ComputeTraceHash(string trace)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(trace);
            byte[] hash = SHA256.HashData(bytes);
            return Convert.ToHexString(hash).ToLowerInvariant();
        }

        /// <summary>Evaluate all gates for a single weather kind (used by determinism tests).</summary>
        public List<GateDayEvaluation> EvaluateAllGatesForWeather(WeatherKind weather)
        {
            var results = new List<GateDayEvaluation>(_routeCatalog.Gates.Count);
            foreach (var gateDef in _routeCatalog.Gates)
            {
                var domain = WeatherGateEvaluator.FromDef(gateDef);
                var state = WeatherGateEvaluator.EvaluateGateStatic(domain, weather);
                bool overrideAvailable = !string.IsNullOrEmpty(gateDef.override_item);
                results.Add(new GateDayEvaluation(
                    0, gateDef.id, gateDef.target, weather,
                    !state.IsOpen, overrideAvailable, state.Reason));
            }
            return results;
        }

        private static SeasonProfileDef LoadProfileFromData()
        {
            string dataDir = FindDataDir();
            var profile = WeatherProfileLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            return profile ?? throw new InvalidOperationException("Failed to load weather_seasons.json");
        }

        public static string FindDataDir()
        {
            string search = Directory.GetCurrentDirectory();
            for (int i = 0; i < 6; i++)
            {
                string candidate = Path.Combine(search, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                string parent = Directory.GetParent(search)?.FullName;
                if (parent == null) break;
                search = parent;
            }
            return string.Empty;
        }
    }

    // ── Audit DTOs ──────────────────────────────────────────────────────

    public sealed record WeatherAuditDay(int Day, string SeasonId, WeatherKind Weather);

    public sealed record GateDayEvaluation(
        int Day,
        string GateId,
        string Target,
        WeatherKind Weather,
        bool IsBlocked,
        bool OverrideAvailable,
        string Reason);

    public sealed class GateUtilizationStats
    {
        public string GateId { get; set; } = "";
        public string Target { get; set; } = "";
        public string GateType { get; set; } = "";
        public List<string> BlockedWeather { get; set; } = new();
        public List<string> RequiredWeather { get; set; } = new();
        public string OverrideItem { get; set; } = "";
        public int BlockedDays { get; set; }
        public int OpenDays { get; set; }
        public int LongestBlockedRun { get; set; }
        public int LongestOpenRun { get; set; }
        public int Transitions { get; set; }

        public double BlockedPct => BlockedDays + OpenDays > 0
            ? 100.0 * BlockedDays / (BlockedDays + OpenDays) : 0;
        public double OpenPct => BlockedDays + OpenDays > 0
            ? 100.0 * OpenDays / (BlockedDays + OpenDays) : 0;
    }
}
