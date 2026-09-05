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
    /// F13 — Weather Gate Determinism Harness.
    /// Proves gate evaluation and forecast-driven gate evaluation are
    /// reproducible across resets, seeds, save/load boundaries, and
    /// repeated runs.
    /// </summary>
    public sealed class WeatherGateDeterminismHarnessTests
    {
        private readonly string _dataDir;
        private readonly WeatherRouteGateCatalog _routeCatalog;

        public WeatherGateDeterminismHarnessTests()
        {
            _dataDir = WeatherGateAuditSimulator.FindDataDir();
            _routeCatalog = WeatherRouteGateCatalog.LoadFromDirectory(_dataDir, new FileSystemIO());
        }

        private static SeasonProfileDef TestProfile()
        {
            string dataDir = WeatherGateAuditSimulator.FindDataDir();
            return WeatherProfileLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer())
                   ?? throw new InvalidOperationException("Failed to load profile");
        }

        private static WeatherSystem CreateWeather(int seed)
        {
            var sys = new WeatherSystem();
            sys.BindProfile(TestProfile(), seed);
            return sys;
        }

        /// <summary>Advance weather to a target day by ticking 24h per day.</summary>
        private static void AdvanceToDay(WeatherSystem sys, int targetDay)
        {
            for (int d = 0; d < targetDay; d++)
                sys.Tick(24f);
        }

        private List<string> FixedInventory => new List<string> { "gas_mask", "hazmat_suit" };

        private List<GateDayEvaluation> EvaluateAllGates(WeatherKind weather)
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

        // ── F13.4 Seed 42 / Day 30 ────────────────────────────────────

        [Fact]
        public void F13_Seed42_Day30_DeterministicGateTrace()
        {
            // Pass A
            var sysA = CreateWeather(42);
            AdvanceToDay(sysA, 30);
            int rollCountA = sysA.State.rollCount;
            var weatherA = sysA.Current;
            var traceA = EvaluateAllGates(weatherA);

            // Pass B — fresh system, same seed
            var sysB = CreateWeather(42);
            AdvanceToDay(sysB, 30);
            int rollCountB = sysB.State.rollCount;
            var weatherB = sysB.Current;
            var traceB = EvaluateAllGates(weatherB);

            Assert.Equal(weatherA, weatherB);
            Assert.Equal(rollCountA, rollCountB);
            Assert.Equal(traceA.Count, traceB.Count);

            string serializedA = WeatherGateAuditSimulator.SerializeTrace(traceA);
            string serializedB = WeatherGateAuditSimulator.SerializeTrace(traceB);
            Assert.Equal(serializedA, serializedB);
            Assert.Equal(
                WeatherGateAuditSimulator.ComputeTraceHash(serializedA),
                WeatherGateAuditSimulator.ComputeTraceHash(serializedB));
        }

        // ── F13.5 Seed 99 / Day 100 ───────────────────────────────────

        [Fact]
        public void F13_Seed99_Day100_DeterministicGateTrace()
        {
            var sysA = CreateWeather(99);
            AdvanceToDay(sysA, 100);
            var weatherA = sysA.Current;
            var traceA = EvaluateAllGates(weatherA);

            var sysB = CreateWeather(99);
            AdvanceToDay(sysB, 100);
            var weatherB = sysB.Current;
            var traceB = EvaluateAllGates(weatherB);

            Assert.Equal(weatherA, weatherB);
            Assert.Equal(sysA.State.rollCount, sysB.State.rollCount);
            Assert.Equal(
                WeatherGateAuditSimulator.ComputeTraceHash(WeatherGateAuditSimulator.SerializeTrace(traceA)),
                WeatherGateAuditSimulator.ComputeTraceHash(WeatherGateAuditSimulator.SerializeTrace(traceB)));
        }

        // ── F13.6 Seed 7 / Day 200 ────────────────────────────────────

        [Fact]
        public void F13_Seed7_Day200_DeterministicGateTrace()
        {
            var sysA = CreateWeather(7);
            AdvanceToDay(sysA, 200);
            var weatherA = sysA.Current;
            var traceA = EvaluateAllGates(weatherA);

            var sysB = CreateWeather(7);
            AdvanceToDay(sysB, 200);
            var weatherB = sysB.Current;
            var traceB = EvaluateAllGates(weatherB);

            Assert.Equal(weatherA, weatherB);
            Assert.Equal(sysA.State.rollCount, sysB.State.rollCount);
            Assert.Equal(
                WeatherGateAuditSimulator.ComputeTraceHash(WeatherGateAuditSimulator.SerializeTrace(traceA)),
                WeatherGateAuditSimulator.ComputeTraceHash(WeatherGateAuditSimulator.SerializeTrace(traceB)));
        }

        // ── F13.7 Forecast determinism ─────────────────────────────────

        [Fact]
        public void F13_PeekForecast_DeterministicAcrossResets()
        {
            var sysA = CreateWeather(42);
            AdvanceToDay(sysA, 30);
            var forecastA = sysA.PeekForecast(3);
            int rollCountBefore = sysA.State.rollCount;
            var weatherBefore = sysA.Current;

            var sysB = CreateWeather(42);
            AdvanceToDay(sysB, 30);
            var forecastB = sysB.PeekForecast(3);

            Assert.Equal(3, forecastA.Count);
            Assert.Equal(3, forecastB.Count);
            for (int i = 0; i < 3; i++)
            {
                Assert.Equal(forecastA[i].Day, forecastB[i].Day);
                Assert.Equal(forecastA[i].Kind, forecastB[i].Kind);
                Assert.Equal(forecastA[i].OutdoorRad, forecastB[i].OutdoorRad, 3);
                Assert.Equal(forecastA[i].Visibility, forecastB[i].Visibility, 3);
            }

            // PeekForecast must not mutate state
            Assert.Equal(rollCountBefore, sysA.State.rollCount);
            Assert.Equal(weatherBefore, sysA.Current);
        }

        // ── F13.8 Forecast-to-gate determinism ─────────────────────────

        [Fact]
        public void F13_ForecastGateMatrix_DeterministicAcrossResets()
        {
            var sysA = CreateWeather(42);
            AdvanceToDay(sysA, 30);
            var forecastA = sysA.PeekForecast(3);

            var sysB = CreateWeather(42);
            AdvanceToDay(sysB, 30);
            var forecastB = sysB.PeekForecast(3);

            // Evaluate all gates against each forecast day
            for (int i = 0; i < 3; i++)
            {
                var gatesA = EvaluateAllGates(forecastA[i].Kind);
                var gatesB = EvaluateAllGates(forecastB[i].Kind);

                string hashA = WeatherGateAuditSimulator.ComputeTraceHash(
                    WeatherGateAuditSimulator.SerializeTrace(gatesA));
                string hashB = WeatherGateAuditSimulator.ComputeTraceHash(
                    WeatherGateAuditSimulator.SerializeTrace(gatesB));
                Assert.Equal(hashA, hashB);
            }
        }

        // ── F13.9 Mid-evaluation save/load parity ──────────────────────

        [Fact]
        public void F13_SaveLoad_MidGateSweep_MatchesSinglePass()
        {
            // Single pass: evaluate all gates at once
            var sysSingle = CreateWeather(42);
            AdvanceToDay(sysSingle, 30);
            var singleTrace = EvaluateAllGates(sysSingle.Current);

            // Split pass: evaluate first 7, save/restore, evaluate remaining
            var sysSplit = CreateWeather(42);
            AdvanceToDay(sysSplit, 30);
            var splitWeather = sysSplit.Current;
            var splitTrace = new List<GateDayEvaluation>();

            var allGates = _routeCatalog.Gates.ToList();
            for (int i = 0; i < allGates.Count; i++)
            {
                if (i == 7)
                {
                    // Save and restore
                    var saved = sysSplit.CaptureState();
                    var restored = new WeatherSystem();
                    restored.BindProfile(TestProfile(), 42);
                    restored.RestoreState(saved);
                    Assert.Equal(splitWeather, restored.Current);
                    Assert.Equal(sysSplit.State.rollCount, restored.State.rollCount);
                }

                var domain = WeatherGateEvaluator.FromDef(allGates[i]);
                var state = WeatherGateEvaluator.EvaluateGateStatic(domain, splitWeather);
                bool overrideAvailable = !string.IsNullOrEmpty(allGates[i].override_item);
                splitTrace.Add(new GateDayEvaluation(
                    0, allGates[i].id, allGates[i].target, splitWeather,
                    !state.IsOpen, overrideAvailable, state.Reason));
            }

            string singleHash = WeatherGateAuditSimulator.ComputeTraceHash(
                WeatherGateAuditSimulator.SerializeTrace(singleTrace));
            string splitHash = WeatherGateAuditSimulator.ComputeTraceHash(
                WeatherGateAuditSimulator.SerializeTrace(splitTrace));
            Assert.Equal(singleHash, splitHash);
        }

        // ── F13.10 RNG non-consumption ─────────────────────────────────

        [Fact]
        public void F13_EvaluateGate_DoesNotAdvanceWeatherRng()
        {
            var sys = CreateWeather(42);
            AdvanceToDay(sys, 30);
            int rollCountBefore = sys.State.rollCount;
            var weatherBefore = sys.Current;

            // Evaluate one gate
            var gate = _routeCatalog.Gates[0];
            var domain = WeatherGateEvaluator.FromDef(gate);
            WeatherGateEvaluator.EvaluateGateStatic(domain, sys.Current);

            Assert.Equal(rollCountBefore, sys.State.rollCount);
            Assert.Equal(weatherBefore, sys.Current);

            // Evaluate all gates
            foreach (var g in _routeCatalog.Gates)
            {
                var d = WeatherGateEvaluator.FromDef(g);
                WeatherGateEvaluator.EvaluateGateStatic(d, sys.Current);
            }

            Assert.Equal(rollCountBefore, sys.State.rollCount);
            Assert.Equal(weatherBefore, sys.Current);

            // Evaluate forecast gate matrix
            var forecast = sys.PeekForecast(3);
            foreach (var entry in forecast)
            {
                foreach (var g in _routeCatalog.Gates)
                {
                    var d = WeatherGateEvaluator.FromDef(g);
                    WeatherGateEvaluator.EvaluateGateStatic(d, entry.Kind);
                }
            }

            Assert.Equal(rollCountBefore, sys.State.rollCount);
            Assert.Equal(weatherBefore, sys.Current);
        }

        // ── F13.11 Inventory immutability ──────────────────────────────

        [Fact]
        public void F13_IsGateBlocking_DoesNotMutateInventoryPredicate()
        {
            var inventory = new Dictionary<string, int>
            {
                { "gas_mask", 1 },
                { "hazmat_suit", 1 },
                { "water_filter", 2 }
            };

            var snapshot = new Dictionary<string, int>(inventory);

            // Evaluate a gate with override
            var fogGate = _routeCatalog.Gates.First(g => g.id == "gate_lowland_marsh_fog");
            WeatherRouteGateCatalog.IsGateBlocking(fogGate, "BioFog",
                item => inventory.ContainsKey(item) && inventory[item] > 0);

            // Evaluate a gate without override
            var blizGate = _routeCatalog.Gates.First(g => g.id == "gate_mountain_pass_blizzard");
            WeatherRouteGateCatalog.IsGateBlocking(blizGate, "Blizzard",
                item => inventory.ContainsKey(item) && inventory[item] > 0);

            // Evaluate all gates
            foreach (var g in _routeCatalog.Gates)
            {
                WeatherRouteGateCatalog.IsGateBlocking(g, "Clear",
                    item => inventory.ContainsKey(item) && inventory[item] > 0);
            }

            // Inventory must be unchanged
            Assert.Equal(snapshot.Count, inventory.Count);
            foreach (var kv in snapshot)
            {
                Assert.True(inventory.ContainsKey(kv.Key), $"Missing key: {kv.Key}");
                Assert.Equal(kv.Value, inventory[kv.Key]);
            }
        }

        // ── F13.12 100-seed sweep ──────────────────────────────────────

        [Theory]
        [InlineData(30)]
        [InlineData(100)]
        [InlineData(200)]
        public void F13_100SeedSweep_ZeroDeterminismMismatches(int day)
        {
            int mismatches = 0;
            var details = new StringBuilder();

            for (int seed = 0; seed < 100; seed++)
            {
                var sysA = CreateWeather(seed);
                AdvanceToDay(sysA, day);
                var weatherA = sysA.Current;
                int rollA = sysA.State.rollCount;
                var traceA = EvaluateAllGates(weatherA);

                var sysB = CreateWeather(seed);
                AdvanceToDay(sysB, day);
                var weatherB = sysB.Current;
                int rollB = sysB.State.rollCount;
                var traceB = EvaluateAllGates(weatherB);

                if (weatherA != weatherB || rollA != rollB)
                {
                    mismatches++;
                    details.AppendLine($"seed={seed} day={day}: weather {weatherA} vs {weatherB}, rollCount {rollA} vs {rollB}");
                    continue;
                }

                string hashA = WeatherGateAuditSimulator.ComputeTraceHash(
                    WeatherGateAuditSimulator.SerializeTrace(traceA));
                string hashB = WeatherGateAuditSimulator.ComputeTraceHash(
                    WeatherGateAuditSimulator.SerializeTrace(traceB));
                if (hashA != hashB)
                {
                    mismatches++;
                    details.AppendLine($"seed={seed} day={day}: trace hash mismatch");
                }

                // Also check forecast determinism
                var fcA = sysA.PeekForecast(3);
                var fcB = sysB.PeekForecast(3);
                for (int i = 0; i < 3; i++)
                {
                    if (fcA[i].Kind != fcB[i].Kind)
                    {
                        mismatches++;
                        details.AppendLine($"seed={seed} day={day} forecast[{i}]: {fcA[i].Kind} vs {fcB[i].Kind}");
                    }
                }
            }

            Assert.True(mismatches == 0,
                $"{mismatches} determinism mismatches in 100-seed sweep at day {day}:\n{details}");
        }
    }
}
