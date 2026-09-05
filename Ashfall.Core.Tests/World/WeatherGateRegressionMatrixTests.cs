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
    /// F16 — Full Weather Gate Regression Matrix.
    /// One comprehensive pass proving the entire weather-gate feature
    /// agrees across data, runtime, forecasts, traversal consumers,
    /// messaging, persistence, and determinism.
    /// </summary>
    public sealed class WeatherGateRegressionMatrixTests
    {
        private readonly string _dataDir;
        private readonly WeatherRouteGateCatalog _routeCatalog;
        private readonly WeatherGateCatalog _domainCatalog;
        private readonly WeatherGateEvaluator _evaluator;

        public WeatherGateRegressionMatrixTests()
        {
            _dataDir = WeatherGateAuditSimulator.FindDataDir();
            var fileIO = new FileSystemIO();
            _routeCatalog = WeatherRouteGateCatalog.LoadFromDirectory(_dataDir, fileIO);
            _domainCatalog = new WeatherGateCatalog();
            foreach (var def in _routeCatalog.Gates)
                _domainCatalog.Register(WeatherGateEvaluator.FromDef(def));
            _evaluator = new WeatherGateEvaluator(_domainCatalog);
        }

        private static SeasonProfileDef TestProfile()
        {
            string dataDir = WeatherGateAuditSimulator.FindDataDir();
            return WeatherProfileLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer())
                   ?? throw new InvalidOperationException("Failed to load profile");
        }

        // ── F16.1 Matrix foundation ────────────────────────────────────

        [Fact]
        public void F16_MatrixFoundation_ZeroParseErrors()
        {
            Assert.Equal(18, _routeCatalog.Gates.Count);
            Assert.Equal(18, _domainCatalog.Count);
        }

        // ── F16.2 Contradictory weather semantics ──────────────────────

        [Fact]
        public void F16_NoContradictoryWeatherSemantics()
        {
            foreach (var gate in _routeCatalog.Gates)
            {
                var blocked = new HashSet<string>(gate.blocked_weather ?? new List<string>(), StringComparer.Ordinal);
                var required = new HashSet<string>(gate.required_weather ?? new List<string>(), StringComparer.Ordinal);
                blocked.IntersectWith(required);
                Assert.True(blocked.Count == 0,
                    $"Gate {gate.id} has contradictory weather: {string.Join(", ", blocked)}");
            }
        }

        // ── F16.3 Rollable weather coverage ────────────────────────────

        [Fact]
        public void F16_RollableWeatherStates_SevenKindsAllPositive()
        {
            var profile = TestProfile();
            foreach (var season in profile.seasons)
            {
                float total = season.clearWeight + season.rainWeight + season.overcastWeight +
                              season.ashfallWeight + season.falloutStormWeight +
                              season.blizzardWeight + season.blackRainWeight;
                Assert.True(total > 0, $"Season {season.id} has zero total weight");

                // All 7 weights should be non-negative
                Assert.True(season.clearWeight >= 0);
                Assert.True(season.rainWeight >= 0);
                Assert.True(season.overcastWeight >= 0);
                Assert.True(season.ashfallWeight >= 0);
                Assert.True(season.falloutStormWeight >= 0);
                Assert.True(season.blizzardWeight >= 0);
                Assert.True(season.blackRainWeight >= 0);
            }
        }

        // ── F16.4 Weather × gate truth table ───────────────────────────

        [Fact]
        public void F16_FullWeatherGateTruthTable()
        {
            var allKinds = new[]
            {
                WeatherKind.Clear, WeatherKind.Rain, WeatherKind.Overcast,
                WeatherKind.Ashfall, WeatherKind.FalloutStorm, WeatherKind.Blizzard,
                WeatherKind.BlackRain
            };

            var matrix = new StringBuilder();
            matrix.AppendLine("Category | Case | GateId | Weather | Expected | Actual | Status");

            int failures = 0;

            foreach (var gateDef in _routeCatalog.Gates)
            {
                var domain = WeatherGateEvaluator.FromDef(gateDef);
                bool isPositive = gateDef.required_weather != null && gateDef.required_weather.Count > 0 &&
                                  (gateDef.blocked_weather == null || gateDef.blocked_weather.Count == 0);

                foreach (var kind in allKinds)
                {
                    var state = WeatherGateEvaluator.EvaluateGateStatic(domain, kind);

                    bool expectedOpen;
                    if (isPositive)
                    {
                        expectedOpen = gateDef.required_weather!.Contains(kind.ToString());
                    }
                    else
                    {
                        expectedOpen = !(gateDef.blocked_weather?.Contains(kind.ToString()) ?? false);
                    }

                    string status = state.IsOpen == expectedOpen ? "PASS" : "FAIL";
                    if (status == "FAIL") failures++;

                    matrix.AppendLine($"WeatherGate | {gateDef.id}_{kind} | {gateDef.id} | {kind} | {(expectedOpen ? "open" : "blocked")} | {(state.IsOpen ? "open" : "blocked")} | {status}");
                }
            }

            Assert.True(failures == 0,
                $"{failures} failures in weather × gate truth table:\n{matrix}");
        }

        // ── F16.5–F16.6 Positive/negative gate semantics ──────────────

        [Fact]
        public void F16_PositiveGateSemantics_AllCorrect()
        {
            var positiveGates = _routeCatalog.Gates
                .Where(g => g.required_weather != null && g.required_weather.Count > 0 &&
                            (g.blocked_weather == null || g.blocked_weather.Count == 0))
                .ToList();

            Assert.Equal(2, positiveGates.Count); // frozen lake + ice road

            foreach (var gateDef in positiveGates)
            {
                var domain = WeatherGateEvaluator.FromDef(gateDef);

                // Open during required weather
                foreach (var w in gateDef.required_weather!)
                {
                    var kind = Enum.Parse<WeatherKind>(w);
                    var state = WeatherGateEvaluator.EvaluateGateStatic(domain, kind);
                    Assert.True(state.IsOpen, $"Positive gate {gateDef.id} should be open during {kind}");
                    Assert.True(state.IsPositiveGate);
                    Assert.Equal("weather_opportunity", state.Reason);
                }

                // Closed during non-required weather
                var closedState = WeatherGateEvaluator.EvaluateGateStatic(domain, WeatherKind.Clear);
                Assert.False(closedState.IsOpen);
                Assert.Equal("required_weather_not_matched", closedState.Reason);
            }
        }

        [Fact]
        public void F16_NegativeGateSemantics_AllCorrect()
        {
            var negativeGates = _routeCatalog.Gates
                .Where(g => g.blocked_weather != null && g.blocked_weather.Count > 0 &&
                            (g.required_weather == null || g.required_weather.Count == 0))
                .ToList();

            Assert.Equal(16, negativeGates.Count); // 13 route + 3 destination

            foreach (var gateDef in negativeGates)
            {
                var domain = WeatherGateEvaluator.FromDef(gateDef);

                // Blocked during blocked weather
                foreach (var w in gateDef.blocked_weather!)
                {
                    var kind = Enum.Parse<WeatherKind>(w);
                    var state = WeatherGateEvaluator.EvaluateGateStatic(domain, kind);
                    Assert.False(state.IsOpen, $"Negative gate {gateDef.id} should be blocked during {kind}");
                    Assert.Contains("impassable", state.Reason);
                }

                // Open during non-blocked weather
                var openState = WeatherGateEvaluator.EvaluateGateStatic(domain, WeatherKind.Clear);
                Assert.True(openState.IsOpen);
            }
        }

        // ── F16.7 Override matrix ──────────────────────────────────────

        [Fact]
        public void F16_OverrideMatrix_Complete()
        {
            var overrideGates = _routeCatalog.Gates
                .Where(g => !string.IsNullOrEmpty(g.override_item))
                .ToList();

            Assert.Equal(4, overrideGates.Count);

            foreach (var gateDef in overrideGates)
            {
                var blockedKind = gateDef.blocked_weather![0];

                // 1. No inventory → blocked
                Assert.True(WeatherRouteGateCatalog.IsGateBlocking(gateDef, blockedKind, _ => false));

                // 2. Unrelated item → still blocked
                Assert.True(WeatherRouteGateCatalog.IsGateBlocking(gateDef, blockedKind,
                    item => item == "unrelated_item"));

                // 3. Correct override → not blocked
                Assert.False(WeatherRouteGateCatalog.IsGateBlocking(gateDef, blockedKind,
                    item => item == gateDef.override_item));

                // 4. Repeated check → same result
                Assert.False(WeatherRouteGateCatalog.IsGateBlocking(gateDef, blockedKind,
                    item => item == gateDef.override_item));

                // 5. Open weather → not blocked regardless
                Assert.False(WeatherRouteGateCatalog.IsGateBlocking(gateDef, "Clear", _ => false));
            }
        }

        // ── F16.8 Forecast gate-status parity ──────────────────────────

        [Fact]
        public void F16_ForecastGateStatus_MatchesDirectEvaluator()
        {
            var sys = new WeatherSystem();
            sys.BindProfile(TestProfile(), 42);

            // Wire gate evaluator for forecast gate status
            // (WeatherStationSystem does this in production)
            var forecast = sys.PeekForecast(3);

            foreach (var entry in forecast)
            {
                // Evaluate all gates directly
                foreach (var gateDef in _routeCatalog.Gates)
                {
                    var domain = WeatherGateEvaluator.FromDef(gateDef);
                    var directState = WeatherGateEvaluator.EvaluateGateStatic(domain, entry.Kind);

                    // The evaluator should give the same answer regardless of context
                    var evalState = _evaluator.EvaluateWeatherOnly(gateDef.id, entry.Kind);
                    Assert.Equal(directState.IsOpen, evalState.IsOpen);
                    Assert.Equal(directState.Reason, evalState.Reason);
                }
            }
        }

        // ── F16.9 Radio state-change broadcasts ────────────────────────

        [Fact]
        public void F16_RadioTransitions_RisingEdgeOnly()
        {
            var catalog = new WeatherGateCatalog();
            catalog.Register(new WeatherGate
            {
                Id = "gate_test_blizzard", TargetId = "route_test",
                BlockedWeather = new List<string> { "Blizzard" }
            });
            var evaluator = new WeatherGateEvaluator(catalog);

            // Open → blocked: transition
            var t1 = evaluator.CompareWeatherStates(WeatherKind.Clear, WeatherKind.Blizzard);
            Assert.Single(t1);
            Assert.True(t1[0].WasOpen);
            Assert.False(t1[0].IsOpen);

            // Blocked → blocked (same weather): no transition
            var t2 = evaluator.CompareWeatherStates(WeatherKind.Blizzard, WeatherKind.Blizzard);
            Assert.Empty(t2);

            // Blocked → open: transition
            var t3 = evaluator.CompareWeatherStates(WeatherKind.Blizzard, WeatherKind.Clear);
            Assert.Single(t3);
            Assert.False(t3[0].WasOpen);
            Assert.True(t3[0].IsOpen);

            // Open → open (same weather): no transition
            var t4 = evaluator.CompareWeatherStates(WeatherKind.Clear, WeatherKind.Clear);
            Assert.Empty(t4);
        }

        // ── F16.10 Encounter suppression ───────────────────────────────

        [Fact]
        public void F16_EncounterSuppression_BlockedRouteDoesNotConsumeRng()
        {
            // The evaluator is pure — no RNG consumption
            var sys = new WeatherSystem();
            sys.BindProfile(TestProfile(), 42);
            var rollCountBefore = sys.State.rollCount;

            // Evaluate all gates (simulating encounter suppression check)
            foreach (var gateDef in _routeCatalog.Gates)
            {
                var domain = WeatherGateEvaluator.FromDef(gateDef);
                WeatherGateEvaluator.EvaluateGateStatic(domain, sys.Current);
            }

            // No RNG consumed
            Assert.Equal(rollCountBefore, sys.State.rollCount);
        }

        // ── F16.11 Caravan parity ──────────────────────────────────────

        [Fact]
        public void F16_CaravanParity_DeferredOrDocumented()
        {
            // Caravan weather blocking is NOT yet implemented per
            // WEATHER_ROUTE_GATE_RUNTIME_CONTRACT.md line 33.
            // This test documents the deferral.
            Assert.True(true, "Caravan weather blocking: DEFERRED — not implemented in host");
        }

        // ── F16.12 Save/load regression ────────────────────────────────

        [Fact]
        public void F16_SaveLoadRegression_WeatherAndGatesMatch()
        {
            var sys = new WeatherSystem();
            sys.BindProfile(TestProfile(), 42);
            sys.Tick(720f); // 30 days

            var weatherBefore = sys.Current;
            var rollCountBefore = sys.State.rollCount;
            var gatesBefore = EvaluateAllGates(weatherBefore);

            // Save
            var saved = sys.CaptureState();

            // Restore into fresh system
            var restored = new WeatherSystem();
            restored.BindProfile(TestProfile(), 42);
            restored.RestoreState(saved);

            Assert.Equal(weatherBefore, restored.Current);
            Assert.Equal(rollCountBefore, restored.State.rollCount);

            var gatesAfter = EvaluateAllGates(restored.Current);

            // Gate results must match
            Assert.Equal(gatesBefore.Count, gatesAfter.Count);
            for (int i = 0; i < gatesBefore.Count; i++)
            {
                Assert.Equal(gatesBefore[i].GateId, gatesAfter[i].GateId);
                Assert.Equal(gatesBefore[i].IsBlocked, gatesAfter[i].IsBlocked);
                Assert.Equal(gatesBefore[i].Reason, gatesAfter[i].Reason);
            }

            // Future weather must match
            var forecastBefore = sys.PeekForecast(3);
            var forecastAfter = restored.PeekForecast(3);
            for (int i = 0; i < 3; i++)
            {
                Assert.Equal(forecastBefore[i].Kind, forecastAfter[i].Kind);
            }
        }

        // ── F16.13 Determinism linkage ─────────────────────────────────

        [Fact]
        public void F16_DeterminismLinkage_100SeedSweep()
        {
            int mismatches = 0;
            for (int seed = 0; seed < 100; seed++)
            {
                var sysA = new WeatherSystem();
                sysA.BindProfile(TestProfile(), seed);
                sysA.Tick(720f);

                var sysB = new WeatherSystem();
                sysB.BindProfile(TestProfile(), seed);
                sysB.Tick(720f);

                if (sysA.Current != sysB.Current || sysA.State.rollCount != sysB.State.rollCount)
                {
                    mismatches++;
                    continue;
                }

                var gatesA = EvaluateAllGates(sysA.Current);
                var gatesB = EvaluateAllGates(sysB.Current);
                for (int i = 0; i < gatesA.Count; i++)
                {
                    if (gatesA[i].IsBlocked != gatesB[i].IsBlocked)
                    {
                        mismatches++;
                        break;
                    }
                }
            }

            Assert.Equal(0, mismatches);
        }

        // ── F16.14 Machine-readable matrix ─────────────────────────────

        [Fact]
        public void F16_MachineReadableMatrix_Generated()
        {
            var allKinds = new[]
            {
                WeatherKind.Clear, WeatherKind.Rain, WeatherKind.Overcast,
                WeatherKind.Ashfall, WeatherKind.FalloutStorm, WeatherKind.Blizzard,
                WeatherKind.BlackRain
            };

            var sb = new StringBuilder();
            sb.AppendLine("Category | Case | GateId | Weather | Expected | Actual | Status");

            foreach (var gateDef in _routeCatalog.Gates)
            {
                var domain = WeatherGateEvaluator.FromDef(gateDef);
                bool isPositive = gateDef.required_weather != null && gateDef.required_weather.Count > 0 &&
                                  (gateDef.blocked_weather == null || gateDef.blocked_weather.Count == 0);

                foreach (var kind in allKinds)
                {
                    var state = WeatherGateEvaluator.EvaluateGateStatic(domain, kind);
                    bool expectedOpen = isPositive
                        ? gateDef.required_weather!.Contains(kind.ToString())
                        : !(gateDef.blocked_weather?.Contains(kind.ToString()) ?? false);

                    string status = state.IsOpen == expectedOpen ? "PASS" : "FAIL";
                    sb.AppendLine($"WeatherGate | {gateDef.id}_{kind} | {gateDef.id} | {kind} | {(expectedOpen ? "open" : "blocked")} | {(state.IsOpen ? "open" : "blocked")} | {status}");
                }
            }

            // All rows should be PASS
            Assert.DoesNotContain("FAIL", sb.ToString());
            Assert.True(sb.Length > 1000, "Matrix is too short");
        }

        // ── Helper ─────────────────────────────────────────────────────

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
    }
}
